using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Skender.Stock.Indicators;
using MarketWatch.HistoricalData;
using MarketWatch.Models;
using System.Reflection.Metadata;
using System.Text;

namespace MarketWatch.Technical;

public class TechnicalService
{
    private readonly HistoricalDataService _historicalDataService = HistoricalDataService.Instance;
    private IHubContext<TechnicalDataHub> _technicalDataHub = default!;

    public void Start(IHubContext<TechnicalDataHub> technicalDataHub, List<TradingSymbol> tradingSymbols)
    {
        _technicalDataHub = technicalDataHub;
        _historicalDataService.HistoricalDataEventHandler += _historicalDataService_HistoricalDataEventHandler;
        List<PriceData> priceData = _historicalDataService.LoadDaily(tradingSymbols);

        int[] target = new int[] { 0, 15, 30, 45 };

        //List<int> minutes = new List<int>();
        //int currentMinute = DateTime.Now.Minute;
        //while(minutes.Count < 30)
        //{
        //    currentMinute++;
        //    if (currentMinute == 60) currentMinute = 0;

        //    minutes.Add(currentMinute);
        //}
        //int[] target = minutes.ToArray();

        DateTime currentDateTime = DateTime.Now;
        int nextMinute = 0;
        if (!target.Any(t => t > DateTime.Now.Minute))
        {
            nextMinute = target.First();
        }
        else
        {
            nextMinute = target.Where(t => t > DateTime.Now.Minute).First();
        }

        DateTime nextRun = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day,
            currentDateTime.Hour, nextMinute, 0);
        if (nextRun < currentDateTime)
        {
            nextRun = nextRun.AddHours(1);
        }
        double sleepTime = (nextRun - currentDateTime).TotalMinutes;
        var mytask = Task.Run(async () =>
        {
            try
            {
                await Task.Delay(nextRun - currentDateTime);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }
        });

        mytask.Wait();

        var jobTask = Task.Run(() =>
        {
            try
            {
                while (true)
                {
                    _historicalDataService.LoadIntraday(tradingSymbols, "M", 15);
                    Task waitTask = Task.Run(async () => { await Task.Delay(TimeSpan.FromSeconds(15 * 60)); });
                    waitTask.Wait();
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }
        });
    }

    private void _historicalDataService_HistoricalDataEventHandler(PriceDataEvent priceDataEvent)
    {
        if (priceDataEvent.Type == "Daily")
            CalculatePivots(priceDataEvent.PriceData, "STANDARD");
        else
        {
            HeikinAshi(priceDataEvent.PriceData);
        }
    }

    private static List<SkenderOHLC> ConvertCandles(List<OHLC> candles)
    {
        List<SkenderOHLC> skenderOHLCs = new List<SkenderOHLC>();
        foreach (OHLC candle in candles)
            skenderOHLCs.Add(SkenderOHLC.Create(candle));
        return skenderOHLCs;
    }

    private static decimal FormatPrice(string symbol, decimal value)
    {
        return Math.Round(value, symbol.Contains("JPY") ? 2 : 5);
    }

    private bool DifferenceThreshold(string symbol, decimal value1, decimal value2)
    {
        decimal tolerance = symbol.Contains("JPY") ? .0001m : .00004m;
        return Math.Abs(value1 - value2) < tolerance;
    }

    #region Pivots

    private static PivotPointType GetPivotPointType(string pivotName)
    {
        PivotPointType pivotPointType = PivotPointType.Woodie;
        if (pivotName == "CAMARILLA")
            pivotPointType = PivotPointType.Camarilla;
        else if (pivotName == "STANDARD")
            pivotPointType = PivotPointType.Standard;
        else if (pivotName == "WOODIE")
            pivotPointType = PivotPointType.Woodie;

        return pivotPointType;
    }

    public List<PivotPoint> GetPivots(List<TradingSymbol> tradingSymbols, string pivotName)
    {
        List<PivotPoint> pivotPoints = new List<PivotPoint>();
        List<PriceData> priceData = _historicalDataService.LoadDaily(tradingSymbols);

        priceData.ForEach(pd =>
        {
            List<SkenderOHLC> skenderOHLCs = ConvertCandles(pd.Prices);
            IEnumerable<PivotPointsResult> pivotPointResults = skenderOHLCs.Skip(skenderOHLCs.Count - 2).Take(2)
                .GetPivotPoints(PeriodSize.Day, GetPivotPointType(pivotName));
            pivotPoints.Add(new PivotPoint(pd.Symbol, pivotName, pivotPointResults.Last()));
        });

        return pivotPoints;
    }

    private void CalculatePivots(List<PriceData> priceData, string pivotName)
    {
        foreach (PriceData price in priceData)
        {
            List<SkenderOHLC> skenderOHLCs = ConvertCandles(price.Prices);
            IEnumerable<PivotPointsResult> pivotPointResults = skenderOHLCs.Skip(skenderOHLCs.Count - 2).Take(2)
                .GetPivotPoints(PeriodSize.Day, GetPivotPointType(pivotName));

            PivotPointMessage pivotPointMessage = new PivotPointMessage
            {
                Name = "PIVOTS",
                Symbol = price.Symbol,
                PivotPointsResult = pivotPointResults.Last()
            };

            _technicalDataHub.Clients.All.SendAsync("technical", JsonConvert.SerializeObject(pivotPointMessage,
            new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            }));
        }
    }

    #endregion

    #region HeikinAshi

    private void HistoricalHeikinAshi(PriceData price, List<OHLC> heikinAshiCandles, bool writeOutput = false)
    {
        OHLC lastHeikinAshiCandle = heikinAshiCandles.Last();
        var output = new StringBuilder();
        List<HeikinAshiMessage> heikinAshiMessages = new List<HeikinAshiMessage>();

        int alertSequenceNumber = 0;

        heikinAshiCandles.ToList().ForEach(heikinAshiCandle => {
            OHLC actualCandle = price.Prices.Where(c => c.Date == heikinAshiCandle.Date).First();
            bool isLast = heikinAshiCandles.Last() == heikinAshiCandle;

            bool alertRed = DifferenceThreshold(price.Symbol, heikinAshiCandle.Open, heikinAshiCandle.High);
            bool alertGreen = DifferenceThreshold(price.Symbol, heikinAshiCandle.Open, heikinAshiCandle.Low);

            string alertType = alertRed ? "RED" : alertGreen ? "GREEN" : string.Empty;

            alertSequenceNumber = string.IsNullOrEmpty(alertType) ? 0 : alertSequenceNumber + 1;
            if (!string.IsNullOrEmpty(alertType))
            {
                HeikinAshiMessage heikinAshiMessage = new HeikinAshiMessage
                {
                    Name = "HEIKINASHI",
                    AlertType = alertType,
                    AlertSequenceNumber = alertSequenceNumber,
                    Symbol = price.Symbol,
                    TimeFrame = price.TimeFrame,
                    TimeFrameLength = price.TimeFrameLength,
                    AlertDateTime = actualCandle.Date
                };

                if(writeOutput)
                {
                    heikinAshiMessages.Add(heikinAshiMessage);
                    var timeString = string.Format("{0} {1}", actualCandle.Date.ToShortDateString(), actualCandle.Date.ToShortTimeString());
                    output.AppendLine(string.Format("{0},{1},{2}", timeString, alertType, alertSequenceNumber));
                }

                _technicalDataHub.Clients.All.SendAsync("technical", JsonConvert.SerializeObject(heikinAshiMessage,
                new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    }
                }));
            }
        });

        if(writeOutput)
        {
            var fileName = string.Format(@"c:\workspace\{0}.csv", price.Symbol);
            if (File.Exists(fileName)) File.Delete(fileName);
            File.WriteAllText(fileName, output.ToString());
        }
    }

    private void HeikinAshi(List<PriceData> priceData, bool createHistoricalOutput = false, bool writeOutput = false)
    {
        foreach (PriceData price in priceData)
        {
            List<SkenderOHLC> skenderOHLCs = ConvertCandles(price.Prices);
            List<OHLC> heikinAshiCandles = skenderOHLCs.GetHeikinAshi().ToList().Select(ha =>
                new OHLC
                {
                    Open = FormatPrice(price.Symbol, ha.Open),
                    High = ha.High,
                    Low = ha.Low,
                    Close = FormatPrice(price.Symbol, ha.Close),
                    Volume = ha.Volume,
                    Date = ha.Date
                }).ToList();

            if (createHistoricalOutput)
            {
                HistoricalHeikinAshi(price, heikinAshiCandles, writeOutput);
            }
            else
            {
                int alertSequenceNumber = 0;

                for (var index = 0; index < heikinAshiCandles.Count(); index++)
                {
                    bool isLast = index == heikinAshiCandles.Count - 1;

                    OHLC heikinAshiCandle = heikinAshiCandles[index];
                    OHLC actualCandle = price.Prices.Where(c => c.Date == heikinAshiCandle.Date).First();
                    bool alertRed = DifferenceThreshold(price.Symbol, heikinAshiCandle.Open, heikinAshiCandle.High);
                    bool alertGreen = DifferenceThreshold(price.Symbol, heikinAshiCandle.Open, heikinAshiCandle.Low);
                    string alertType = alertRed ? "RED" : alertGreen ? "GREEN" : string.Empty;

                    alertSequenceNumber = string.IsNullOrEmpty(alertType) ? 0 : alertSequenceNumber + 1;
                    if (!string.IsNullOrEmpty(alertType))
                    {
                        HeikinAshiMessage heikinAshiMessage = new HeikinAshiMessage
                        {
                            Name = "HEIKINASHI",
                            AlertType = alertType,
                            AlertSequenceNumber = alertSequenceNumber,
                            Symbol = price.Symbol,
                            TimeFrame = price.TimeFrame,
                            TimeFrameLength = price.TimeFrameLength,
                            AlertDateTime = actualCandle.Date
                        };

                        if (isLast && alertSequenceNumber == 1)
                        {
                            _technicalDataHub.Clients.All.SendAsync("technical", JsonConvert.SerializeObject(heikinAshiMessage,
                            new JsonSerializerSettings
                            {
                                ContractResolver = new DefaultContractResolver
                                {
                                    NamingStrategy = new CamelCaseNamingStrategy()
                                }
                            }));
                        }
                    }
                }

                if (heikinAshiCandles.Any())
                {
                    OHLC lastHeikinAshiCandle = heikinAshiCandles.Last();
                    heikinAshiCandles.ToList().ForEach(heikinAshiCandle => {
                        OHLC actualCandle = price.Prices.Where(c => c.Date == heikinAshiCandle.Date).First();

                        bool alertRed = DifferenceThreshold(price.Symbol, heikinAshiCandle.Open, heikinAshiCandle.High);
                        bool alertGreen = DifferenceThreshold(price.Symbol, heikinAshiCandle.Open, heikinAshiCandle.Low);

                        string alertType = alertRed ? "RED" : alertGreen ? "GREEN" : string.Empty;

                        alertSequenceNumber = string.IsNullOrEmpty(alertType) ? 0 : alertSequenceNumber + 1;
                        if (!string.IsNullOrEmpty(alertType))
                        {
                            HeikinAshiMessage heikinAshiMessage = new HeikinAshiMessage
                            {
                                Name = "HEIKINASHI",
                                AlertType = alertType,
                                AlertSequenceNumber = alertSequenceNumber,
                                Symbol = price.Symbol,
                                TimeFrame = price.TimeFrame,
                                TimeFrameLength = price.TimeFrameLength,
                                AlertDateTime = actualCandle.Date
                            };

                            _technicalDataHub.Clients.All.SendAsync("technical", JsonConvert.SerializeObject(heikinAshiMessage,
                            new JsonSerializerSettings
                            {
                                ContractResolver = new DefaultContractResolver
                                {
                                    NamingStrategy = new CamelCaseNamingStrategy()
                                }
                            }));
                        }
                    });
                }
            }
        }
    }

    #endregion
}