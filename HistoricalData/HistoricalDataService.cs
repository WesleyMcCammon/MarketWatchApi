using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using MarketWatch.LiveData;
using MarketWatch.Models;

namespace MarketWatch.HistoricalData
{
    public class CacheObject {
        public string Symbol {get;set;} = string.Empty;
        public DateTime Date {get; set; } = DateTime.Now;
        public string TimeFrame { get; set; } = string.Empty;
        public int TimeFrameLength { get; set; }
    }

    public delegate void HistoricalDataEventHandler(PriceDataEvent priceDataEvents);

    public class HistoricalDataService
    {
        public event HistoricalDataEventHandler HistoricalDataEventHandler = default!;
        private static readonly Lazy<HistoricalDataService> lazy = new Lazy<HistoricalDataService>(() => new HistoricalDataService());

        private IDictionary<CacheObject, PriceData> _historicalDataCache = new Dictionary<CacheObject, PriceData>();
        public HistoricalDataService()
        {
        }

        private List<PriceData> LoadHistorical(List<TradingSymbol> tradingSymbols, string timeFrame, int timeFrameLength)
        {
            List<PriceData> priceData = new List<PriceData>();
            tradingSymbols.ForEach(tradingSymbol =>
            {
                IEnumerable<KeyValuePair<CacheObject, PriceData>> cacheSearch =
                    _historicalDataCache
                    .Where(h => h.Key.Symbol == tradingSymbol.Symbol && 
                        h.Key.Date == DateTime.Now.Date && 
                        h.Key.TimeFrame == timeFrame && 
                        h.Key.TimeFrameLength == timeFrameLength);

                if (cacheSearch.Any())
                {
                    Console.WriteLine(string.Format("Getting {0} {1}{2} from cache", tradingSymbol.Symbol, timeFrame, timeFrameLength));
                    priceData.Add(cacheSearch.First().Value);
                    priceData.Last().Symbol = priceData.Last().Symbol.Replace("_", "");
                }
                else {
                    if (tradingSymbol.HistoricalSource.Source == "Oanda")
                    {
                        Console.WriteLine(string.Format("Getting {0} {1}{2} from source", tradingSymbol.Symbol, timeFrame, timeFrameLength));
                        OandaDataClient oandaDataClient = new OandaDataClient();
                        priceData.Add(oandaDataClient.Load(tradingSymbol.HistoricalSource.Symbol,
                            timeFrame, timeFrameLength));

                        _historicalDataCache.Add(new CacheObject { 
                            Symbol = tradingSymbol.Symbol, 
                            Date = DateTime.Now.Date,
                            TimeFrame = timeFrame,
                            TimeFrameLength = timeFrameLength
                        }, 
                            priceData.Last());
                        priceData.Last().Symbol = priceData.Last().Symbol.Replace("_", "");
                    }
                }
            });

            return priceData;
        }

        public List<PriceData> LoadIntraday(List<TradingSymbol> tradingSymbols, string timeFrame, int timeFrameLength)
        {
            PriceDataEvent priceDataEvent = new PriceDataEvent
            {
                Type = "Intraday",
                PriceData = LoadHistorical(tradingSymbols, timeFrame, timeFrameLength)
            };
            if (HistoricalDataEventHandler != null)
                HistoricalDataEventHandler(priceDataEvent);

            return priceDataEvent.PriceData;
        }

        public List<PriceData> LoadDaily(List<TradingSymbol> tradingSymbols)
        {
            PriceDataEvent priceDataEvent = new PriceDataEvent
            {
                Type = "Daily",
                PriceData = LoadHistorical(tradingSymbols, "D", 1)
            };
            if (HistoricalDataEventHandler != null)
                HistoricalDataEventHandler(priceDataEvent);

            return priceDataEvent.PriceData;
        }

        public static HistoricalDataService Instance => lazy.Value;
    }
}
