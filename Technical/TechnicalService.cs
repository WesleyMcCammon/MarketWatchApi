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

        //int[] target = new int[] { 0, 15, 30, 45 };

        List<int> minutes = new List<int>();
        int currentMinute = DateTime.Now.Minute;
        while (minutes.Count < 30)
        {
            currentMinute++;
            if (currentMinute == 60) currentMinute = 0;

            minutes.Add(currentMinute);
        }
        int[] target = minutes.ToArray();

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
                    IntradayTechnicalJob(tradingSymbols);
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

    private void IntradayTechnicalJob(List<TradingSymbol> tradingSymbols)
    {
        tradingSymbols.ForEach(tradingSymbol => { 
            PriceData priceData = _historicalDataService.LoadIntraday(tradingSymbol, "M", 15);
            List<HeikinAshiMessage> heikinAshiMessages = CalculateHeikinAshi(priceData);

            if(heikinAshiMessages.Any())
            {
                _technicalDataHub.Clients.All.SendAsync("technical", JsonConvert.SerializeObject(heikinAshiMessages.First(),
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

        tradingSymbols.ForEach(tradingSymbol => {
            PriceData priceData = _historicalDataService.LoadDaily(tradingSymbol);
            List<SkenderOHLC> skenderOHLCs = ConvertCandles(priceData.Prices);
            IEnumerable<PivotPointsResult> pivotPointResults = skenderOHLCs.Skip(skenderOHLCs.Count - 2).Take(2)
                .GetPivotPoints(PeriodSize.Day, GetPivotPointType(pivotName));
            pivotPoints.Add(new PivotPoint(priceData.Symbol, pivotName, pivotPointResults.Last()));

        });

        return pivotPoints;
    }

    #endregion

    #region HeikinAshi

    private List<HeikinAshiMessage> CalculateHeikinAshi(PriceData priceData, bool createHistoricalOutput = false, bool writeOutput = false)
    {
        List<HeikinAshiMessage> heikinAshiMessages = new List<HeikinAshiMessage>();
        List<SkenderOHLC> skenderOHLCs = ConvertCandles(priceData.Prices);
        List<OHLC> heikinAshiCandles = skenderOHLCs.GetHeikinAshi().ToList().Select(ha =>
            new OHLC
            {
                Open = FormatPrice(priceData.Symbol, ha.Open),
                High = ha.High,
                Low = ha.Low,
                Close = FormatPrice(priceData.Symbol, ha.Close),
                Volume = ha.Volume,
                Date = ha.Date
            }).ToList();

        int alertSequenceNumber = 0;

        for (var index = 0; index < heikinAshiCandles.Count(); index++)
        {
            bool isLast = index == heikinAshiCandles.Count - 1;

            OHLC heikinAshiCandle = heikinAshiCandles[index];
            OHLC actualCandle = priceData.Prices.Where(c => c.Date == heikinAshiCandle.Date).First();
            bool alertRed = DifferenceThreshold(priceData.Symbol, heikinAshiCandle.Open, heikinAshiCandle.High);
            bool alertGreen = DifferenceThreshold(priceData.Symbol, heikinAshiCandle.Open, heikinAshiCandle.Low);
            string alertType = alertRed ? "RED" : alertGreen ? "GREEN" : string.Empty;

            alertSequenceNumber = string.IsNullOrEmpty(alertType) ? 0 : alertSequenceNumber + 1;
            if (!string.IsNullOrEmpty(alertType))
            {
                if(isLast && alertSequenceNumber == 1)
                {
                    heikinAshiMessages.Add(new HeikinAshiMessage
                    {
                        Name = "HEIKINASHI",
                        AlertType = alertType,
                        AlertSequenceNumber = alertSequenceNumber,
                        Symbol = priceData.Symbol,
                        TimeFrame = priceData.TimeFrame,
                        TimeFrameLength = priceData.TimeFrameLength,
                        AlertDateTime = actualCandle.Date
                    });
                }
            }
        }

        return heikinAshiMessages;
    }

    #endregion
}