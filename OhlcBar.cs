namespace MarketWatchAPI
{
    public class OhlcBar
    {
        public DateOnly PeriodStart { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
    }
}
