using System.Runtime.CompilerServices;

namespace MarketWatchAPI.Quotes
{
    // Default quote source: streams a randomly-walked mock price for every forex pair,
    // futures contract, and CFD in the catalogs. Stands in until a real market data
    // provider is wired up via LiveQuoteSource.
    public class MockQuoteSource : IQuoteSource
    {
        private static readonly TimeSpan TickInterval = TimeSpan.FromMilliseconds(250);

        // Only a handful of instruments move per tick so the feed reads like a real
        // market instead of every symbol updating in lockstep every 250ms.
        private const int SymbolsPerTick = 5;

        public async IAsyncEnumerable<Quote> StreamAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var instruments = AllInstruments.ToDictionary(i => i.Symbol);
            var prices = instruments.ToDictionary(kv => kv.Key, kv => kv.Value.BasePrice);
            var symbols = prices.Keys.ToArray();

            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(TickInterval, cancellationToken);

                foreach (var symbol in symbols.OrderBy(_ => Random.Shared.Next()).Take(SymbolsPerTick))
                {
                    var instrument = instruments[symbol];
                    var price = prices[symbol];

                    var drift = price * 0.0004m * (decimal)(Random.Shared.NextDouble() * 2 - 1);
                    price = Math.Max(price + drift, 0.0001m);
                    prices[symbol] = price;

                    var spread = price * 0.0002m;

                    yield return new Quote(
                        symbol,
                        Math.Round(price - spread / 2, instrument.PricePrecision),
                        Math.Round(price + spread / 2, instrument.PricePrecision),
                        Math.Round(price, instrument.PricePrecision),
                        DateTimeOffset.UtcNow);
                }
            }
        }

        private static IEnumerable<IPricedInstrument> AllInstruments =>
            ForexPairCatalog.AllPairs
                .Concat<IPricedInstrument>(FuturesContractCatalog.AllContracts)
                .Concat(CfdCatalog.All);
    }
}
