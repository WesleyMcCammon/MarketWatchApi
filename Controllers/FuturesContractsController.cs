using Microsoft.AspNetCore.Mvc;

namespace MarketWatchAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FuturesContractsController : ControllerBase
    {
        [HttpGet(Name = "GetFuturesContracts")]
        public IActionResult Get()
        {
            return Ok(new
            {
                Indices = FuturesContractCatalog.Indices,
                Metals = FuturesContractCatalog.Metals,
                Currencies = FuturesContractCatalog.Currencies,
                InterestRates = FuturesContractCatalog.InterestRates,
                Energy = FuturesContractCatalog.Energy
            });
        }

        [HttpGet("{symbol}", Name = "GetFuturesContractBySymbol")]
        public IActionResult Get(string symbol)
        {
            var contract = FuturesContractCatalog.Find(symbol);
            if (contract is null)
            {
                return NotFound(new { Message = $"Unknown futures contract '{symbol}'." });
            }

            return Ok(contract);
        }
    }
}
