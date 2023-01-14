using Microsoft.AspNetCore.Mvc;
using MarketWatch.Technical;
using MarketWatch.Models;
using MarketWatch.DataContext;

namespace MarketWatch.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TechnicalController : ControllerBase
{
    private readonly TechnicalService _technicalService = default!;

    public TechnicalController(TechnicalService technicalService)
    {
        _technicalService = technicalService;
    }

    [HttpGet]
    [Route("dailyPivots")]
    public IEnumerable<PivotPoint> DailyPivots(string pivotName = "STANDARD")
    {
        List<TradingSymbol> tradingSymbols = new DataService().GetTradingSymbols(true);
        return _technicalService.GetPivots(tradingSymbols, pivotName);
    }
}
