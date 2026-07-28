namespace MarketWatchAPI.Quotes
{
    public record Quote(string Symbol, decimal Bid, decimal Ask, decimal Last, DateTimeOffset Timestamp);
}
