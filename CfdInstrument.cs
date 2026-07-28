using System.Text.Json.Serialization;

namespace MarketWatchAPI
{
    public class CfdInstrument : IPricedInstrument
    {
        public required string Symbol { get; set; }
        public required string Name { get; set; }
        public int PricePrecision { get; set; }

        // Seed price used only to generate realistic-looking mock indicator data, not a live quote.
        [JsonIgnore]
        public decimal BasePrice { get; set; }
    }
}
