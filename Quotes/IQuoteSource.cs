namespace MarketWatchAPI.Quotes
{
    // Abstraction over where quotes come from, so the streaming pipeline (background
    // service + websocket fan-out) doesn't care whether ticks are generated locally or
    // relayed from a real market data provider. Swap the DI registration in Program.cs
    // to change source.
    public interface IQuoteSource
    {
        IAsyncEnumerable<Quote> StreamAsync(CancellationToken cancellationToken);
    }
}
