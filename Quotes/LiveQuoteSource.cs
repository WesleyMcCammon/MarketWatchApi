namespace MarketWatchAPI.Quotes
{
    // Placeholder for a real market data feed. The concrete provider isn't chosen yet --
    // once it is, connect to it here and yield a Quote for each tick as it arrives.
    // To go live, set QuoteStreaming:Source to "Live" in appsettings.json; no other
    // code changes are needed since QuoteStreamingService and the websocket endpoint
    // only depend on IQuoteSource.
    public class LiveQuoteSource : IQuoteSource
    {
        public IAsyncEnumerable<Quote> StreamAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException("Wire up the real market data provider here once it's chosen.");
        }
    }
}
