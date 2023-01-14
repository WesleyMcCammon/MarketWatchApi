using Skender.Stock.Indicators;

namespace MarketWatch.Models
{
    public class TechnicalMessage
    {
        public string Name { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
    }

    public class PivotPointMessage : TechnicalMessage
    {
        public PivotPointsResult PivotPointsResult { get; set; } = default!;
    }


    public class HeikinAshiMessage : TechnicalMessage
    {
        public string AlertType { get; set; } = string.Empty;
        public int AlertSequenceNumber { get; set; }
        public string TimeFrame { get; set; } = string.Empty;
        public int TimeFrameLength { get; set; }
        public DateTime AlertDateTime { get; set; }
    }
}
