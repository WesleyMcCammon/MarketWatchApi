using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using MarketWatch.Models;

namespace MarketWatch.LiveData;

public class LiveDataService
{
    private readonly ThinkOrSwimService _thinkOrSwimService = ThinkOrSwimService.Instance;
    private IHubContext<LiveDataHub> _liveDataHub = default!;

    public void TOSLiveData(IHubContext<LiveDataHub> liveDataHub, List<TradingSymbol> tradingSymbols)
    {
        _liveDataHub = liveDataHub;
        _thinkOrSwimService.ThinkOrSwimEventHandler += _thinkOrSwimService_ThinkOrSwimEventHandler;
        _thinkOrSwimService.Execute(tradingSymbols);
    }

    private void _thinkOrSwimService_ThinkOrSwimEventHandler(ThinkOrSwimQuoteMessage thinkOrSwimQuoteMessage)
    {
        _liveDataHub.Clients.All.SendAsync("liveData", JsonConvert.SerializeObject(thinkOrSwimQuoteMessage,
            new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            }));
    }
}
