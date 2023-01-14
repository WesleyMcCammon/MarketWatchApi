namespace MarketWatch.DataContext;
internal class Instrument
{
    public int Id { get; set; }
    public string Symbol { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int MarketId { get; set; }
    public int MarketSectorId { get; set; }
    public Market Market { get; set; } = default!;
    public MarketSector MarketSector { get; set; } = default!;
}