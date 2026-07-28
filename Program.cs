using System.Net.WebSockets;
using System.Text.Json;
using System.Text.Json.Serialization;
using MarketWatchAPI.Quotes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

const string AngularDevCorsPolicy = "AngularDev";

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy(AngularDevCorsPolicy, policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// Quote streaming: which IQuoteSource is registered decides mock vs. live, and nothing
// else (background service, websocket endpoint) needs to change to switch between them.
// Set QuoteStreaming:Source to "Live" in appsettings.json once a real provider exists.
builder.Services.AddSingleton<QuoteBroadcaster>();
builder.Services.AddSingleton<IQuoteSource>(sp =>
{
    var source = sp.GetRequiredService<IConfiguration>()["QuoteStreaming:Source"];
    return string.Equals(source, "Live", StringComparison.OrdinalIgnoreCase)
        ? new LiveQuoteSource()
        : new MockQuoteSource();
});
builder.Services.AddHostedService<QuoteStreamingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseWebSockets();

app.UseCors(AngularDevCorsPolicy);

app.UseAuthorization();

app.MapControllers();

app.Map("/ws/quotes", async context =>
{
    if (!context.WebSockets.IsWebSocketRequest)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        return;
    }

    var broadcaster = context.RequestServices.GetRequiredService<QuoteBroadcaster>();
    using var socket = await context.WebSockets.AcceptWebSocketAsync();
    var reader = broadcaster.Subscribe();

    try
    {
        await foreach (var quote in reader.ReadAllAsync(context.RequestAborted))
        {
            var json = JsonSerializer.SerializeToUtf8Bytes(quote);
            await socket.SendAsync(json, WebSocketMessageType.Text, endOfMessage: true, context.RequestAborted);
        }
    }
    catch (OperationCanceledException)
    {
        // Client disconnected or the server is shutting down.
    }
    finally
    {
        broadcaster.Unsubscribe(reader);
        if (socket.State == WebSocketState.Open)
        {
            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "closing", CancellationToken.None);
        }
    }
});

app.Run();
