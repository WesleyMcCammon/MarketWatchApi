using System.Collections;
using System.Collections.Concurrent;

#pragma warning disable CS8618

namespace MarketWatch.LiveData;

class Queue : IEnumerator, IEnumerator<Quote>
{
    BlockingCollection<Quote> queue = new BlockingCollection<Quote>(new ConcurrentQueue<Quote>());
    Quote current;

    internal Queue()
    {

    }

    internal void Disconnect()
    {
        queue.CompleteAdding();
    }

    internal void Push(Quote quote)
    {
        queue.Add(quote);
    }

    public Quote Current
    {
        get
        {
            return current;
        }
    }

    object IEnumerator.Current
    {
        get
        {
            return current;
        }
    }

    public void Dispose()
    {
        queue.Dispose();
    }

    public bool MoveNext()
    {
        if (queue.IsCompleted)
        {
            return false;
        }
        current = queue.Take();
        return true;
    }

    public void Reset()
    {

    }
}