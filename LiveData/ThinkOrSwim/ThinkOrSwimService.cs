using Newtonsoft.Json;
using MarketWatch.Models;

namespace MarketWatch.LiveData;

public delegate void ThinkOrSwimEventHandler(ThinkOrSwimQuoteMessage message);

public class ThinkOrSwimService
{
    private static readonly Lazy<ThinkOrSwimService> lazy = new Lazy<ThinkOrSwimService>(() => new ThinkOrSwimService());
    private readonly List<TradingSymbol> _tradingSymbols = new List<TradingSymbol>();
    public event ThinkOrSwimEventHandler ThinkOrSwimEventHandler = default!;
    private bool _keepGoing = true;
    private readonly Client _client = new Client();
    private bool _runAsMockedService = true;

    public void Execute(List<TradingSymbol> tradingSymbols)
    {
        _tradingSymbols.AddRange(tradingSymbols);
        if (!_runAsMockedService)
        {
            _tradingSymbols.ForEach(tradingSymbol =>
            {
                _client.Add(tradingSymbol.LiveSource.Symbol);
            });
            Start();
        }
        else
            _Start();
    }
    public void Stop()
    {
        _keepGoing = false;
    }

    private void _Start()
    {
        List<MockFeedData> mockFeedData = new List<MockFeedData>();

        try
        {
            Dictionary<string, decimal> mockSeedData = new Dictionary<string, decimal>();
            mockSeedData.Add("EURUSD", 1.0762m);
            mockSeedData.Add("GBPUSD", 1.21336m);
            mockSeedData.Add("USDJPY", 131.66m);
            mockSeedData.Add("USDCAD", 1.3648m);
            mockSeedData.Add("USDCHF", 0.92082m);
            mockSeedData.Add("AUDUSD", 0.69085m);
            mockSeedData.Add("NZDUSD", 0.64412m);

            _tradingSymbols.ForEach(instrument =>
            {
                if (mockSeedData.ContainsKey(instrument.Symbol))
                {
                    decimal offset = instrument.Symbol.Contains("JPY") ? .21m : .0021m;
                    mockFeedData.Add(new MockFeedData
                    {
                        Symbol = instrument.Symbol,
                        Bid = mockSeedData[instrument.Symbol],
                        Ask = mockSeedData[instrument.Symbol] + offset
                    });
                }
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        while (_keepGoing)
        {
            Thread.Sleep(1500);

            mockFeedData.ForEach(mfd =>
            {
                var bidMessage = new ThinkOrSwimQuoteMessage
                {
                    Date = DateTime.Now,
                    Symbol = mfd.Symbol,
                    QuoteType = "BID",
                    Value = mfd.Bid
                };
                var askMesage = new ThinkOrSwimQuoteMessage
                {
                    Date = DateTime.Now,
                    Symbol = mfd.Symbol,
                    QuoteType = "ASK",
                    Value = mfd.Ask
                };

                if (ThinkOrSwimEventHandler != null)
                {
                    ThinkOrSwimEventHandler(bidMessage);
                    ThinkOrSwimEventHandler(askMesage);
                }

                int priceMove = DateTime.Now.Ticks % 2 == 0 ? 1 : -1;
                decimal priceMoveValue = mfd.Symbol.Contains("JPY") ? .001m : .00001m;
                mfd.Ask += (priceMove * priceMoveValue);
                mfd.Bid += (priceMove * priceMoveValue);
                Thread.Sleep(500);
            });
        }
    }
    private void Start()
    {
        while (_keepGoing)
        {
            foreach (var quote in _client.Quotes())
            {
                string message = string.Format("{0} {1} {2}", quote.Symbol,
                    quote.Type.ToString(), quote.Value);

                string symbol = _tradingSymbols.Where(i => i.LiveSource.Symbol == quote.Symbol)
                    .Select(i => i.Symbol)
                    .First();
                var thinkOrSwimQuoteMessage = new ThinkOrSwimQuoteMessage
                {
                    Date = DateTime.Now,
                    Symbol = symbol ?? String.Empty,
                    QuoteType = quote.Type.ToString(),
                    Value = (decimal)quote.Value
                };

                if (ThinkOrSwimEventHandler != null) ThinkOrSwimEventHandler(thinkOrSwimQuoteMessage);
            }
        }
    }

    public static ThinkOrSwimService Instance => lazy.Value;
}

internal class MockFeedData
{
    public string Symbol { get; set; } = string.Empty;
    public decimal Bid { get; set; }
    public decimal Ask { get; set; }
}