using Microsoft.Win32;
using System.Text;
using System.Collections;
using System.Security.Cryptography;

#pragma warning disable CS8602
#pragma warning disable CS8604
#pragma warning disable CS8600
#pragma warning disable CS8601
#pragma warning disable CS8602
#pragma warning disable CS0168
#pragma warning disable CA1416


namespace MarketWatch.LiveData;

class Feed : IRTDUpdateEvent, IEnumerable<Quote>
{
    readonly string registryKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Tos.RTD\CLSID";
    IRTDServer server;
    Queue queue = new Queue();
    Topics topics = new Topics();

    internal Feed(int heartbeat)
    {
        var rtd = Type.GetTypeFromCLSID(
            new Guid(Registry.GetValue(registryKey, "", null).ToString()));
        server = (IRTDServer)Activator.CreateInstance(rtd);
        HeartbeatInterval = heartbeat;
        server.ServerStart(this);
    }

    internal void Stop()
    {
        server.ServerTerminate();
    }

    internal void Add(string symbol, string type)
    {
        var objects = new object[] { type, symbol };
        var id = getHash(symbol, type);
        topics.Add(id, symbol, type);
        server.ConnectData(id, objects, true);
    }

    internal void Remove(string symbol, string type)
    {
        var id = getHash(symbol, type);
        server.DisconnectData(id);
    }

    short getHash(string symbol, string type)
    {
        var value = string.Format("{0}:{1}", symbol, type);
        using (var h = MD5.Create())
        {
            return Math.Abs(BitConverter.ToInt16(
                h.ComputeHash(Encoding.UTF8.GetBytes(value)), 0));
        }
    }

    public int HeartbeatInterval
    {
        get; set;
    }

    public void Disconnect()
    {
        queue.Disconnect();
    }

    public void UpdateNotify()
    {
        var refresh = server.RefreshData(topics.Count());
        if (refresh.Length > 0)
        {
            for (int i = 0; i < refresh.Length / 2; i++)
            {
                try
                {
                    var id = (int)refresh[0, i];
                    var data = topics.Get(id);
                    queue.Push(new Quote(
                        data.Item1,
                        data.Item2,
                        double.Parse(refresh[1, i].ToString())));
                }
                catch (Exception ex)
                {

                }
            }
        }
    }

    public IEnumerator<Quote> GetEnumerator()
    {
        return queue;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return queue;
    }
}
