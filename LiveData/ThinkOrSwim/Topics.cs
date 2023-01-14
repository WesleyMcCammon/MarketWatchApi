using System.Collections.Concurrent;
namespace MarketWatch.LiveData;

class Topics
{
    ConcurrentDictionary<long, Tuple<string, string>> data = new ConcurrentDictionary<long, Tuple<string, string>>();

    internal Topics()
    {

    }

    public int Count()
    {
        return data.Count();
    }

    public Tuple<string, string> Get(int id)
    {
        return data[id];
    }

    public void Add(short id, string symbol, string type)
    {
        data[id] = new Tuple<string, string>(symbol, type);
    }
}
