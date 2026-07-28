namespace MarketWatchAPI
{
    public static class ForexPairCatalog
    {
        // JPY pairs are quoted to 2 decimal places; all other pairs to 4.
        public const int JpyPrecision = 2;
        public const int StandardPrecision = 4;

        public static readonly ForexPair[] MajorPairs =
        [
            new() { Symbol = "EUR/USD", PricePrecision = StandardPrecision, BasePrice = 1.0850m },
            new() { Symbol = "USD/JPY", PricePrecision = JpyPrecision, BasePrice = 155.00m },
            new() { Symbol = "GBP/USD", PricePrecision = StandardPrecision, BasePrice = 1.2650m },
            new() { Symbol = "USD/CHF", PricePrecision = StandardPrecision, BasePrice = 0.8850m },
            new() { Symbol = "USD/CAD", PricePrecision = StandardPrecision, BasePrice = 1.3650m },
            new() { Symbol = "AUD/USD", PricePrecision = StandardPrecision, BasePrice = 0.6550m },
            new() { Symbol = "NZD/USD", PricePrecision = StandardPrecision, BasePrice = 0.6050m }
        ];

        public static readonly ForexPair[] MinorPairs =
        [
            new() { Symbol = "EUR/GBP", PricePrecision = StandardPrecision, BasePrice = 0.8580m },
            new() { Symbol = "GBP/JPY", PricePrecision = JpyPrecision, BasePrice = 196.00m },
            new() { Symbol = "EUR/AUD", PricePrecision = StandardPrecision, BasePrice = 1.6550m },
            new() { Symbol = "GBP/CHF", PricePrecision = StandardPrecision, BasePrice = 1.1200m },
            new() { Symbol = "EUR/JPY", PricePrecision = JpyPrecision, BasePrice = 168.00m }
        ];

        public static IEnumerable<ForexPair> AllPairs => MajorPairs.Concat(MinorPairs);

        public static ForexPair? Find(string symbol)
        {
            var normalized = symbol.Replace("/", "").Replace("-", "").Replace(" ", "").ToUpperInvariant();
            return AllPairs.FirstOrDefault(p => p.Symbol.Replace("/", "").ToUpperInvariant() == normalized);
        }
    }
}
