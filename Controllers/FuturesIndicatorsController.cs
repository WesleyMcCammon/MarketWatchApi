using Microsoft.AspNetCore.Mvc;

namespace MarketWatchAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FuturesIndicatorsController : ControllerBase
    {
        [HttpGet(Name = "GetFuturesIndicators")]
        public IActionResult Get()
        {
            return Ok(new
            {
                Indices = IndicatorGenerator.GenerateAll(FuturesContractCatalog.Indices),
                Metals = IndicatorGenerator.GenerateAll(FuturesContractCatalog.Metals),
                Currencies = IndicatorGenerator.GenerateAll(FuturesContractCatalog.Currencies),
                InterestRates = IndicatorGenerator.GenerateAll(FuturesContractCatalog.InterestRates),
                Energy = IndicatorGenerator.GenerateAll(FuturesContractCatalog.Energy)
            });
        }

        [HttpGet("{symbol}", Name = "GetFuturesIndicatorsForContract")]
        public IActionResult Get(string symbol)
        {
            var contract = FuturesContractCatalog.Find(symbol);
            if (contract is null)
            {
                return NotFound(new { Message = $"Unknown futures contract '{symbol}'." });
            }

            return Ok(IndicatorGenerator.Generate(contract));
        }
    }
}
