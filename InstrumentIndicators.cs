namespace MarketWatchAPI
{
    public class InstrumentIndicators
    {
        public required string Symbol { get; set; }
        public int PricePrecision { get; set; }
        public required PivotPoints Pivots { get; set; }
        public required OhlcBar[] WeeklyOhlc { get; set; }
        public required OhlcBar[] DailyOhlc { get; set; }
        public required OhlcBar[] AsiaSessionOhlc { get; set; }
        public required OhlcBar[] LondonSessionOhlc { get; set; }
        public required OhlcBar[] NewYorkSessionOhlc { get; set; }
        public required VwapLevels Vwap { get; set; }
        public required VolumeProfile VolumeProfile { get; set; }
    }
}
