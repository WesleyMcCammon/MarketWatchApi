namespace MarketWatch.Models
{
    public class ThinkOrSwimQuoteMessage
    {
        public DateTime Date { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public string QuoteType { get; set; } = string.Empty;
        public decimal Value { get; set; }
    }
}