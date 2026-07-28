namespace MarketWatchAPI
{
    public static class CfdCatalog
    {
        public static readonly CfdInstrument[] All =
        [
            new() { Symbol = "US500", Name = "US 500 Index", PricePrecision = 1, BasePrice = 5850.0m },
            new() { Symbol = "USTEC", Name = "US Tech 100 Index", PricePrecision = 1, BasePrice = 20500.0m },
            new() { Symbol = "US30", Name = "US Wall Street 30 Index", PricePrecision = 0, BasePrice = 42000m },
            new() { Symbol = "XAUUSD", Name = "Gold vs US Dollar", PricePrecision = 2, BasePrice = 2450.00m },
            new() { Symbol = "WTIUSD", Name = "WTI Crude Oil vs US Dollar", PricePrecision = 2, BasePrice = 68.50m },
            new() { Symbol = "BRTUSD", Name = "Brent Crude Oil vs US Dollar", PricePrecision = 2, BasePrice = 72.50m },
            new() { Symbol = "NGAS", Name = "Natural Gas", PricePrecision = 3, BasePrice = 3.150m },
            new() { Symbol = "XAGUSD", Name = "Silver vs US Dollar", PricePrecision = 3, BasePrice = 30.500m }
        ];

        public static CfdInstrument? Find(string symbol)
        {
            return All.FirstOrDefault(c => string.Equals(c.Symbol, symbol, StringComparison.OrdinalIgnoreCase));
        }
    }
}
