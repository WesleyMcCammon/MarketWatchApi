using Microsoft.EntityFrameworkCore;
using MarketWatch.Models;

namespace MarketWatch.DataContext;

public partial class DataService
{
    private List<TradingSymbol> _tradingSymbols = new List<TradingSymbol>();

    public List<TradingSymbol> GetTradingSymbols(bool activeOnly = false)
    {
        using (var context = new RetailTraderContext())
        {
            IQueryable<InstrumentDataSource> instrumentDataSource = context.InstrumentDataSource.AsQueryable();

            var query = context.Instrument
                .Include(i => i.Market)
                .Include(i => i.MarketSector)
                .Where(i => (i.IsActive == true && activeOnly) || !activeOnly);

            _tradingSymbols = query.Select(x => new TradingSymbol
            {
                Symbol = x.Symbol,
                MarketName = x.Market.Name,
                IsActive = x.IsActive,
                Description = string.Empty,
                LiveSource = new InstrumentSources
                {
                    Symbol = instrumentDataSource.Where(i => i.InstrumentId == x.Id && i.IsLive).First().Symbol,
                    Source = instrumentDataSource.Where(i => i.InstrumentId == x.Id && i.IsLive).First().Source
                },
                HistoricalSource = new InstrumentSources
                {
                    Symbol = instrumentDataSource.Where(i => i.InstrumentId == x.Id && !i.IsLive).First().Symbol,
                    Source = instrumentDataSource.Where(i => i.InstrumentId == x.Id && !i.IsLive).First().Source
                }
            }).ToList();

            return _tradingSymbols;
        }
    }
}