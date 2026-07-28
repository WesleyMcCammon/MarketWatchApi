using System.Threading.Channels;

namespace MarketWatchAPI.Quotes
{
    // In-memory pub/sub so the single upstream IQuoteSource stream can fan out to any
    // number of connected websocket clients without each client re-triggering the source.
    public class QuoteBroadcaster
    {
        private readonly List<Channel<Quote>> _subscribers = [];
        private readonly object _gate = new();

        public ChannelReader<Quote> Subscribe()
        {
            // Bounded + drop-oldest so one slow client can't apply backpressure to the
            // broadcaster or block delivery to everyone else.
            var channel = Channel.CreateBounded<Quote>(new BoundedChannelOptions(100)
            {
                FullMode = BoundedChannelFullMode.DropOldest
            });

            lock (_gate)
            {
                _subscribers.Add(channel);
            }

            return channel.Reader;
        }

        public void Unsubscribe(ChannelReader<Quote> reader)
        {
            lock (_gate)
            {
                _subscribers.RemoveAll(c => c.Reader == reader);
            }
        }

        public void Publish(Quote quote)
        {
            lock (_gate)
            {
                foreach (var channel in _subscribers)
                {
                    channel.Writer.TryWrite(quote);
                }
            }
        }
    }
}
