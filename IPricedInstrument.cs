namespace MarketWatchAPI
{
    // Common shape shared by tradable instruments (forex pairs, futures contracts, ...)
    // so mock indicator data can be generated the same way for all of them.
    public interface IPricedInstrument
    {
        string Symbol { get; }
        int PricePrecision { get; }
        decimal BasePrice { get; }
    }
}
