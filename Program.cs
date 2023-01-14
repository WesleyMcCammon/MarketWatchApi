using MarketWatch.HistoricalData;
using MarketWatch.LiveData;
using MarketWatch.Technical;
using MarketWatchApi.Background;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<LiveDataService>();
builder.Services.AddSingleton<HistoricalDataService>();
builder.Services.AddSingleton<TechnicalService>();
builder.Services.AddHostedService<InitializationService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder
        .SetIsOriginAllowed(origin => true) // allow any origin
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("CorsPolicy");
app.MapHub<LiveDataHub>("/liveData");
app.MapHub<HistoricalDataHub>("/historicalData");
app.MapHub<TechnicalDataHub>("/technical");
app.Run();

