using System.Net;
using System.Text;
using MarketWatch.Models;

namespace MarketWatch.HistoricalData;

#pragma warning disable SYSLIB0014
public class OandaDataClient : IDataClient
{
    private readonly string baseUrl = "https://api-fxpractice.oanda.com";

    public PriceData Load(string symbol, string timeFrame, int timeFrameLength)
    {
        string granularity = CreateGranularity(timeFrame, timeFrameLength);
        var url = string.Format("{0}/v3/instruments/{1}/candles?granularity={2}", baseUrl, symbol, granularity);
        var request = (HttpWebRequest)WebRequest.Create(url);

        string json = "";
        string credentialHeader = String.Format("Bearer 1aa2f86b2ce2b328b9a2c415e68c7ae3-cc0091d0eb4a28b72073d3e58fe99b3c");
        request.Method = "GET";
        request.ContentType = "application/json";
        request.Headers.Add("Authorization", credentialHeader);

        HttpWebResponse webresponse = (HttpWebResponse)request.GetResponse();

        var sw = new StreamReader(webresponse.GetResponseStream(), System.Text.Encoding.ASCII);
        json = sw.ReadToEnd();
        sw.Close();

        return ParseJson(symbol, timeFrame, timeFrameLength, json);
    }

    private string CreateGranularity(string timeFrame, int timeFrameLength)
    {
        StringBuilder granularity = new StringBuilder(timeFrame.Substring(0, 1).ToUpper());
        if (granularity.ToString() != "D")
            granularity.Append(timeFrameLength);

        return granularity.ToString();
    }
    private PriceData ParseJson(string instrument, string timeFrame, int timeFrameLength, string json)
    {
        var candlesIndex = json.IndexOf("[", json.IndexOf("candles"));
        var candles = json.Substring(candlesIndex);
        candles = candles.Remove(candles.Length - 1, 1);

        PriceData priceData = new PriceData(instrument, timeFrame, timeFrameLength);
        priceData.AddCandle(ParseOHLC(candles));

        return priceData;
    }

    private static List<OHLC> ParseOHLC(string data)
    {
        int baseIndex = 0;
        List<OHLC> candles = new List<OHLC>();

        while (true)
        {
            int firstIndex = data.IndexOf("{", baseIndex);
            int secondIndex = data.IndexOf("{", firstIndex + 1);
            int thirdIndex = data.IndexOf("}", secondIndex + 1);
            int fourthIndex = data.IndexOf("}", thirdIndex + 1);
            if (firstIndex < 0 || secondIndex < 0 || thirdIndex < 0 || fourthIndex < 0)
            {
                break;
            }
            string rcd = data.Substring(firstIndex, (fourthIndex - firstIndex) + 1);

            int t1 = rcd.IndexOf(":", rcd.IndexOf("time"));
            int t2 = rcd.IndexOf(",", t1);
            string timeString = rcd.Substring(t1 + 1, t2 - t1 - 1).Replace("\"", "");

            int c1 = rcd.IndexOf(":", rcd.IndexOf("complete"));
            int c2 = rcd.IndexOf(",", c1);
            string completeString = rcd.Substring(c1 + 1, c2 - c1 - 1).Replace("\"", "");

            string[] mid = rcd.Substring(rcd.IndexOf("{", t1)).Replace("\"", "").Replace("{", "").Replace("}", "").Split(',');

            OHLC candle = new OHLC
            {
                Date = DateTime.Parse(timeString),
                Open = 0,
                High = 0,
                Low = 0,
                Close = 0,
                Closed = completeString.ToUpper() == "TRUE"
            };

            candles.Add(candle);

            foreach (string m in mid)
            {
                var price = m.Split(':')[0];
                var value = m.Split(':')[1];
                decimal open = 0m;
                decimal high = 0m;
                decimal low = 0m;
                decimal close = 0m;

                if (price == "o")
                {
                    decimal.TryParse(value, out open);
                    candle.Open = open;
                }
                if (price == "h")
                {
                    decimal.TryParse(value, out high);
                    candle.High = high;
                }
                if (price == "l")
                {
                    decimal.TryParse(value, out low);
                    candle.Low = low;
                }
                if (price == "c")
                {
                    decimal.TryParse(value, out close);
                    candle.Close = close;
                }
            }

            baseIndex = fourthIndex + 1;
        }

        //return candles.Where(c => c.Closed == true).ToList();
        return candles.ToList();
    }
}