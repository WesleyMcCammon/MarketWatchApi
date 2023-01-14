using Microsoft.AspNetCore.Mvc;
using MarketWatch.DataContext;
using MarketWatch.Models;

namespace MarketWatch.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TradingSymbolController : ControllerBase
{
    [HttpGet]
    public List<TradingSymbol> GetTradingSymbols(bool activeOnly = true)
    {
        return new DataService().GetTradingSymbols(activeOnly);
    }
}
