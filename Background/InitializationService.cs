using MarketWatch.DataContext;
using MarketWatch.HistoricalData;
using MarketWatch.LiveData;
using MarketWatch.Models;
using MarketWatch.Technical;
using Microsoft.AspNetCore.SignalR;

namespace MarketWatchApi.Background
{
    public class InitializationService : IHostedService, IDisposable
    {
        private readonly IHubContext<HistoricalDataHub> _historicalDataHub;
        private readonly IHubContext<TechnicalDataHub> _technicalDataHub;
        private readonly IHubContext<LiveDataHub> _liveDataHub;

        private readonly HistoricalDataService _historicalDataService;
        private readonly TechnicalService _technicalService;
        private readonly LiveDataService _liveDataService;

        public InitializationService(
            IHubContext<HistoricalDataHub> historicalDataHub,
            IHubContext<TechnicalDataHub> technicalDataHub,
            IHubContext<LiveDataHub> liveDataHub,
            HistoricalDataService historicalDataService,
            TechnicalService technicalService,
            LiveDataService liveDataService)
        {
            _historicalDataHub = historicalDataHub;
            _technicalDataHub = technicalDataHub;
            _liveDataHub = liveDataHub;

            _historicalDataService = historicalDataService;
            _technicalService = technicalService;
            _liveDataService = liveDataService;
        }

        public void Dispose()
        {
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            List<TradingSymbol> tradingSymbols = new DataService().GetTradingSymbols(true);

            if (tradingSymbols.Any())
            {
                Task liveDataTask = Task.Run(() =>
                {
                    _liveDataService.TOSLiveData(_liveDataHub, tradingSymbols);
                });

                Task technicalServiceTask = Task.Run(() =>
                {
                    _technicalService.Start(_technicalDataHub, tradingSymbols);
                });

            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
