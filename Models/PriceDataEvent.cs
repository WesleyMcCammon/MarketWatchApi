namespace MarketWatch.Models;

public class PriceDataEvent
{
    public List<PriceData> PriceData { get; set; } = new List<PriceData>();
    public string Type { get; set; } = string.Empty;
}
