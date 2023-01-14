namespace MarketWatch.Models;

public class PriceData
{
    public string Symbol { get; set; } = string.Empty;
    public string TimeFrame { get; set; } = String.Empty;
    public int TimeFrameLength { get; set; }
    public List<OHLC> Prices { get; set; } = new List<OHLC>();

    public PriceData() { }
    public PriceData(string symbol, string periodName, int periodValue)
    {
        Symbol = symbol;
        TimeFrame = periodName;
        TimeFrameLength = periodValue;
    }

    public void AddCandle(IList<OHLC> prices)
    {
        Prices.AddRange(prices);
    }
}