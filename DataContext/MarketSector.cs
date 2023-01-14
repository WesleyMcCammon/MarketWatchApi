namespace MarketWatch.DataContext;
internal class MarketSector
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int MarketId { get; set; }
    public Market Market { get; set; } = default!;
}
