namespace MarketWatch.Models;

public class TradingSymbol
{
    public string Symbol { get; set; } = string.Empty;
    public string MarketName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string Description { get; set; } = string.Empty;
    public InstrumentSources LiveSource { get; set; } = default!;
    public InstrumentSources HistoricalSource { get; set; } = default!;
}

public class InstrumentSources
{
    public string Symbol { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
}