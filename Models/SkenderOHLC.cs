using Skender.Stock.Indicators;

namespace MarketWatch.Models
{
    public class SkenderOHLC : IQuote
    {
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public DateTime Date { get; set; }
        public decimal Volume { get; set; }

        public SkenderOHLC(decimal open, decimal high, decimal low, decimal close, decimal volume, DateTime date)
        {
            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = volume;
            Date = date;
        }

        public static SkenderOHLC Create(OHLC candle)
        {
            return new SkenderOHLC(candle.Open, candle.High, candle.Low, candle.Close, candle.Volume, candle.Date);
        }
    }
}
