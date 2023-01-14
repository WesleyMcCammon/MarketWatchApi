using MarketWatch.Models;

namespace MarketWatch.HistoricalData;

public interface IDataClient
{
    public PriceData Load(string symbol, string timeFrame, int timeFrameLength);
}