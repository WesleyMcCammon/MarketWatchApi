namespace MarketWatch.LiveData;

public enum QuoteType
{
    Last,
    Bid,
    Ask
}

public class Client : IDisposable
{
    Feed feed;

    public Client() : this(10)
    {

    }

    public Client(int heartbeat)
    {
        feed = new Feed(heartbeat);
    }

    public void Add(string symbol)
    {
        feed.Add(symbol, QuoteType.Bid.ToString());
        feed.Add(symbol, QuoteType.Ask.ToString());
    }

    public void Remove(string symbol, QuoteType quoteType)
    {
        feed.Remove(symbol, quoteType.ToString());
    }

    public IEnumerable<Quote> Quotes()
    {
        return feed;
    }

    public void Dispose()
    {
        feed.Stop();
    }
}
