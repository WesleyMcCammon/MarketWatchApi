namespace MarketWatchAPI
{
    public static class FuturesContractCatalog
    {
        public static readonly FuturesContract[] Indices =
        [
            new() { Symbol = "ES", Name = "E-mini S&P 500", Category = FuturesCategory.Indices, Exchange = "CME", IsEMini = true, PricePrecision = 2, BasePrice = 5850.00m },
            new() { Symbol = "MES", Name = "Micro E-mini S&P 500", Category = FuturesCategory.Indices, Exchange = "CME", IsMicro = true, PricePrecision = 2, BasePrice = 5850.00m },
            new() { Symbol = "NQ", Name = "E-mini Nasdaq-100", Category = FuturesCategory.Indices, Exchange = "CME", IsEMini = true, PricePrecision = 2, BasePrice = 20500.00m },
            new() { Symbol = "MNQ", Name = "Micro E-mini Nasdaq-100", Category = FuturesCategory.Indices, Exchange = "CME", IsMicro = true, PricePrecision = 2, BasePrice = 20500.00m },
            new() { Symbol = "YM", Name = "E-mini Dow ($5)", Category = FuturesCategory.Indices, Exchange = "CBOT", IsEMini = true, PricePrecision = 0, BasePrice = 42000m },
            new() { Symbol = "MYM", Name = "Micro E-mini Dow ($0.50)", Category = FuturesCategory.Indices, Exchange = "CBOT", IsMicro = true, PricePrecision = 0, BasePrice = 42000m },
            new() { Symbol = "RTY", Name = "E-mini Russell 2000", Category = FuturesCategory.Indices, Exchange = "CME", IsEMini = true, PricePrecision = 1, BasePrice = 2250.0m },
            new() { Symbol = "M2K", Name = "Micro E-mini Russell 2000", Category = FuturesCategory.Indices, Exchange = "CME", IsMicro = true, PricePrecision = 1, BasePrice = 2250.0m }
        ];

        public static readonly FuturesContract[] Metals =
        [
            new() { Symbol = "GC", Name = "Gold", Category = FuturesCategory.Metals, Exchange = "COMEX", PricePrecision = 1, BasePrice = 2450.0m },
            new() { Symbol = "MGC", Name = "Micro Gold", Category = FuturesCategory.Metals, Exchange = "COMEX", IsMicro = true, PricePrecision = 1, BasePrice = 2450.0m },
            new() { Symbol = "SI", Name = "Silver", Category = FuturesCategory.Metals, Exchange = "COMEX", PricePrecision = 3, BasePrice = 30.500m },
            new() { Symbol = "SIL", Name = "Micro Silver", Category = FuturesCategory.Metals, Exchange = "COMEX", IsMicro = true, PricePrecision = 3, BasePrice = 30.500m },
            new() { Symbol = "HG", Name = "Copper", Category = FuturesCategory.Metals, Exchange = "COMEX", PricePrecision = 4, BasePrice = 4.5000m },
            new() { Symbol = "MHG", Name = "Micro Copper", Category = FuturesCategory.Metals, Exchange = "COMEX", IsMicro = true, PricePrecision = 4, BasePrice = 4.5000m },
            new() { Symbol = "PL", Name = "Platinum", Category = FuturesCategory.Metals, Exchange = "NYMEX", PricePrecision = 1, BasePrice = 980.0m }
        ];

        public static readonly FuturesContract[] Currencies =
        [
            new() { Symbol = "6E", Name = "Euro FX", Category = FuturesCategory.Currencies, Exchange = "CME", PricePrecision = 4, BasePrice = 1.0850m },
            new() { Symbol = "M6E", Name = "Micro EUR/USD", Category = FuturesCategory.Currencies, Exchange = "CME", IsMicro = true, PricePrecision = 4, BasePrice = 1.0850m },
            new() { Symbol = "6B", Name = "British Pound", Category = FuturesCategory.Currencies, Exchange = "CME", PricePrecision = 4, BasePrice = 1.2650m },
            new() { Symbol = "M6B", Name = "Micro GBP/USD", Category = FuturesCategory.Currencies, Exchange = "CME", IsMicro = true, PricePrecision = 4, BasePrice = 1.2650m },
            new() { Symbol = "6J", Name = "Japanese Yen", Category = FuturesCategory.Currencies, Exchange = "CME", PricePrecision = 6, BasePrice = 0.006500m },
            new() { Symbol = "M6J", Name = "Micro JPY/USD", Category = FuturesCategory.Currencies, Exchange = "CME", IsMicro = true, PricePrecision = 6, BasePrice = 0.006500m },
            new() { Symbol = "6A", Name = "Australian Dollar", Category = FuturesCategory.Currencies, Exchange = "CME", PricePrecision = 4, BasePrice = 0.6550m },
            new() { Symbol = "6C", Name = "Canadian Dollar", Category = FuturesCategory.Currencies, Exchange = "CME", PricePrecision = 4, BasePrice = 0.7330m },
            new() { Symbol = "6S", Name = "Swiss Franc", Category = FuturesCategory.Currencies, Exchange = "CME", PricePrecision = 4, BasePrice = 1.1300m }
        ];

        public static readonly FuturesContract[] InterestRates =
        [
            new() { Symbol = "ZT", Name = "2-Year U.S. Treasury Note", Category = FuturesCategory.InterestRates, Exchange = "CBOT", PricePrecision = 4, BasePrice = 103.5000m },
            new() { Symbol = "ZF", Name = "5-Year U.S. Treasury Note", Category = FuturesCategory.InterestRates, Exchange = "CBOT", PricePrecision = 4, BasePrice = 108.5000m },
            new() { Symbol = "ZN", Name = "10-Year U.S. Treasury Note", Category = FuturesCategory.InterestRates, Exchange = "CBOT", PricePrecision = 4, BasePrice = 112.0000m },
            new() { Symbol = "TN", Name = "Ultra 10-Year U.S. Treasury Note", Category = FuturesCategory.InterestRates, Exchange = "CBOT", PricePrecision = 4, BasePrice = 116.0000m },
            new() { Symbol = "ZB", Name = "30-Year U.S. Treasury Bond", Category = FuturesCategory.InterestRates, Exchange = "CBOT", PricePrecision = 4, BasePrice = 120.0000m },
            new() { Symbol = "UB", Name = "Ultra U.S. Treasury Bond", Category = FuturesCategory.InterestRates, Exchange = "CBOT", PricePrecision = 4, BasePrice = 128.0000m },
            new() { Symbol = "ZQ", Name = "30-Day Fed Funds", Category = FuturesCategory.InterestRates, Exchange = "CBOT", PricePrecision = 4, BasePrice = 95.7500m },
            new() { Symbol = "SR3", Name = "3-Month SOFR", Category = FuturesCategory.InterestRates, Exchange = "CME", PricePrecision = 4, BasePrice = 95.5000m }
        ];

        public static readonly FuturesContract[] Energy =
        [
            new() { Symbol = "CL", Name = "WTI Crude Oil", Category = FuturesCategory.Energy, Exchange = "NYMEX", PricePrecision = 2, BasePrice = 68.50m },
            new() { Symbol = "MCL", Name = "Micro WTI Crude Oil", Category = FuturesCategory.Energy, Exchange = "NYMEX", IsMicro = true, PricePrecision = 2, BasePrice = 68.50m },
            new() { Symbol = "BZ", Name = "Brent Crude Oil", Category = FuturesCategory.Energy, Exchange = "NYMEX", PricePrecision = 2, BasePrice = 72.50m },
            new() { Symbol = "NG", Name = "Henry Hub Natural Gas", Category = FuturesCategory.Energy, Exchange = "NYMEX", PricePrecision = 3, BasePrice = 3.150m },
            new() { Symbol = "MNG", Name = "Micro Henry Hub Natural Gas", Category = FuturesCategory.Energy, Exchange = "NYMEX", IsMicro = true, PricePrecision = 3, BasePrice = 3.150m },
            new() { Symbol = "RB", Name = "RBOB Gasoline", Category = FuturesCategory.Energy, Exchange = "NYMEX", PricePrecision = 4, BasePrice = 2.0500m },
            new() { Symbol = "HO", Name = "NY Harbor ULSD (Heating Oil)", Category = FuturesCategory.Energy, Exchange = "NYMEX", PricePrecision = 4, BasePrice = 2.2500m }
        ];

        public static IEnumerable<FuturesContract> AllContracts =>
            Indices.Concat(Metals).Concat(Currencies).Concat(InterestRates).Concat(Energy);

        public static FuturesContract? Find(string symbol)
        {
            return AllContracts.FirstOrDefault(c => string.Equals(c.Symbol, symbol, StringComparison.OrdinalIgnoreCase));
        }
    }
}
