namespace MarketWatchAPI
{
    // Generates plausible mock indicator values for an instrument. There is no live market
    // data feed wired up yet, so values are randomized around each instrument's seed
    // BasePrice rather than computed from real ticks.
    public static class IndicatorGenerator
    {
        public static InstrumentIndicators Generate(IPricedInstrument instrument)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            const decimal dailyRangeFraction = 0.006m;
            const decimal weeklyRangeFraction = dailyRangeFraction * 3.5m;

            // Each trading session only covers part of the day, so its range is a fraction of
            // the full daily range. Asia is typically the quietest session; London and New York
            // (which overlap) are typically the most active.
            const decimal asiaSessionRangeFraction = dailyRangeFraction * 0.45m;
            const decimal londonSessionRangeFraction = dailyRangeFraction * 0.85m;
            const decimal newYorkSessionRangeFraction = dailyRangeFraction * 0.90m;

            var dailyBars = GenerateOhlcSeries(instrument, today, stepDays: 1, count: 5, rangeFraction: dailyRangeFraction);
            var weeklyBars = GenerateOhlcSeries(instrument, today, stepDays: 7, count: 5, rangeFraction: weeklyRangeFraction);
            var asiaBars = GenerateOhlcSeries(instrument, today, stepDays: 1, count: 5, rangeFraction: asiaSessionRangeFraction);
            var londonBars = GenerateOhlcSeries(instrument, today, stepDays: 1, count: 5, rangeFraction: londonSessionRangeFraction);
            var newYorkBars = GenerateOhlcSeries(instrument, today, stepDays: 1, count: 5, rangeFraction: newYorkSessionRangeFraction);

            return new InstrumentIndicators
            {
                Symbol = instrument.Symbol,
                PricePrecision = instrument.PricePrecision,
                Pivots = GeneratePivots(instrument, dailyBars[0]),
                DailyOhlc = dailyBars,
                WeeklyOhlc = weeklyBars,
                AsiaSessionOhlc = asiaBars,
                LondonSessionOhlc = londonBars,
                NewYorkSessionOhlc = newYorkBars,
                Vwap = GenerateVwap(instrument),
                VolumeProfile = GenerateVolumeProfile(instrument)
            };
        }

        public static IEnumerable<InstrumentIndicators> GenerateAll(IEnumerable<IPricedInstrument> instruments)
        {
            return instruments.Select(Generate);
        }

        private static OhlcBar[] GenerateOhlcSeries(IPricedInstrument instrument, DateOnly anchorDate, int stepDays, int count, decimal rangeFraction)
        {
            var bars = new OhlcBar[count];
            var price = instrument.BasePrice;

            for (var i = 0; i < count; i++)
            {
                var range = price * rangeFraction * RandomDecimal(0.5m, 1.5m);
                var open = price + RandomOffset(range * 0.3m);
                var close = price + RandomOffset(range * 0.3m);
                var high = Math.Max(open, close) + RandomDecimal(0m, range * 0.4m);
                var low = Math.Min(open, close) - RandomDecimal(0m, range * 0.4m);

                bars[i] = new OhlcBar
                {
                    PeriodStart = anchorDate.AddDays(-stepDays * i),
                    Open = Round(open, instrument),
                    High = Round(high, instrument),
                    Low = Round(low, instrument),
                    Close = Round(close, instrument)
                };

                price = close;
            }

            return bars;
        }

        private static PivotPoints GeneratePivots(IPricedInstrument instrument, OhlcBar priorBar)
        {
            var high = priorBar.High;
            var low = priorBar.Low;
            var close = priorBar.Close;

            var pp = (high + low + close) / 3m;
            var r1 = 2m * pp - low;
            var s1 = 2m * pp - high;
            var r2 = pp + (high - low);
            var s2 = pp - (high - low);
            var r3 = high + 2m * (pp - low);
            var s3 = low - 2m * (high - pp);

            return new PivotPoints
            {
                R3 = Round(r3, instrument),
                R2 = Round(r2, instrument),
                R1 = Round(r1, instrument),
                Pp = Round(pp, instrument),
                S1 = Round(s1, instrument),
                S2 = Round(s2, instrument),
                S3 = Round(s3, instrument)
            };
        }

        private static VwapLevels GenerateVwap(IPricedInstrument instrument)
        {
            var vwap = instrument.BasePrice + RandomOffset(instrument.BasePrice * 0.002m);
            var stdDev = instrument.BasePrice * RandomDecimal(0.0015m, 0.0025m);

            return new VwapLevels
            {
                Vwap = Round(vwap, instrument),
                StdDevPlus1 = Round(vwap + stdDev, instrument),
                StdDevPlus2 = Round(vwap + 2m * stdDev, instrument),
                StdDevPlus3 = Round(vwap + 3m * stdDev, instrument),
                StdDevMinus1 = Round(vwap - stdDev, instrument),
                StdDevMinus2 = Round(vwap - 2m * stdDev, instrument),
                StdDevMinus3 = Round(vwap - 3m * stdDev, instrument)
            };
        }

        private static VolumeProfile GenerateVolumeProfile(IPricedInstrument instrument)
        {
            var poc = instrument.BasePrice + RandomOffset(instrument.BasePrice * 0.0015m);
            var valueAreaWidth = instrument.BasePrice * RandomDecimal(0.002m, 0.004m);

            return new VolumeProfile
            {
                PointOfControl = Round(poc, instrument),
                ValueAreaHigh = Round(poc + valueAreaWidth, instrument),
                ValueAreaLow = Round(poc - valueAreaWidth, instrument)
            };
        }

        private static decimal Round(decimal value, IPricedInstrument instrument) => Math.Round(value, instrument.PricePrecision);

        private static decimal RandomDecimal(decimal min, decimal max) => min + (max - min) * (decimal)Random.Shared.NextDouble();

        private static decimal RandomOffset(decimal magnitude) => RandomDecimal(-magnitude, magnitude);
    }
}
