namespace MarketWatch.Models;
public class OHLC
{

    public decimal Open { get; set; }
    public decimal High { get; set; }
    public decimal Low { get; set; }
    public decimal Close { get; set; }
    public DateTime Date { get; set; }
    public decimal Volume { get; set; }
    public bool Closed { get; set; }

    public OHLC() { }

    public OHLC(DateTime date, decimal open, decimal high, decimal low, decimal close)
    {
        Date = date;
        Open = open;
        High = high;
        Low = low;
        Close = close;
    }
}