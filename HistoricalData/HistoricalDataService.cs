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

    public class HistoricalDataService
    {
        private static readonly Lazy<HistoricalDataService> lazy = new Lazy<HistoricalDataService>(() => new HistoricalDataService());

        private IDictionary<CacheObject, PriceData> _historicalDataCache = new Dictionary<CacheObject, PriceData>();
        public HistoricalDataService()
        {
        }
        
        private PriceData LoadHistorical(TradingSymbol tradingSymbol, string timeFrame, int timeFrameLength)
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
                PriceData priceData = cacheSearch.First().Value;
                priceData.Symbol = priceData.Symbol.Replace("_", "");
                return priceData;
            }
            else
            {
                if (tradingSymbol.HistoricalSource.Source == "Oanda")
                {
                    Console.WriteLine(string.Format("Getting {0} {1}{2} from source", tradingSymbol.Symbol, timeFrame, timeFrameLength));
                    OandaDataClient oandaDataClient = new OandaDataClient();
                    PriceData priceData = oandaDataClient.Load(tradingSymbol.HistoricalSource.Symbol,
                        timeFrame, timeFrameLength);

                    priceData.Symbol = priceData.Symbol.Replace("_", "");

                    _historicalDataCache.Add(new CacheObject
                    {
                        Symbol = tradingSymbol.Symbol,
                        Date = DateTime.Now.Date,
                        TimeFrame = timeFrame,
                        TimeFrameLength = timeFrameLength
                    }, priceData);

                    return priceData;
                }
                else
                    return new PriceData();
            }
        }

        public PriceData LoadIntraday(TradingSymbol tradingSymbol, string timeFrame, int timeFrameLength)
        {
            return LoadHistorical(tradingSymbol, timeFrame, timeFrameLength);
        }
        public PriceData LoadDaily(TradingSymbol tradingSymbol)
        {
            return LoadHistorical(tradingSymbol, "D", 1);
        }

        public static HistoricalDataService Instance => lazy.Value;
    }
}
