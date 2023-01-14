namespace MarketWatch.DataContext;

internal class InstrumentDataSource
{
    public int Id { get; set; }
    public int InstrumentId { get; set; }
    public string Source { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public bool IsLive { get; set; }
}