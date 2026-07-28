using Microsoft.Extensions.Hosting;

namespace MarketWatchAPI.Quotes
{
    // Pulls from whichever IQuoteSource is registered (mock or live) for the lifetime of
    // the app and fans each quote out to connected websocket clients via QuoteBroadcaster.
    public class QuoteStreamingService(IQuoteSource source, QuoteBroadcaster broadcaster, ILogger<QuoteStreamingService> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await foreach (var quote in source.StreamAsync(stoppingToken))
                {
                    broadcaster.Publish(quote);
                }
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                // Normal shutdown.
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Quote source stream ended unexpectedly.");
            }
        }
    }
}
