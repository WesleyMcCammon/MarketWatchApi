namespace MarketWatch.LiveData;
public class Quote
{
    public string Symbol { get; protected set; }
    public string Type { get; protected set; }
    public double Value { get; protected set; }

    public Quote(string symbol, string type, double value)
    {
        Symbol = symbol;
        Value = value;
        Type = type;
    }
}
