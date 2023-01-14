namespace MarketWatch.DataContext;

internal class CurrencyDescription
{
    public int Id { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}