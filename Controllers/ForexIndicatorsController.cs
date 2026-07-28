using Microsoft.AspNetCore.Mvc;

namespace MarketWatchAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ForexIndicatorsController : ControllerBase
    {
        [HttpGet(Name = "GetForexIndicators")]
        public IActionResult Get()
        {
            var indicators = IndicatorGenerator.GenerateAll(ForexPairCatalog.AllPairs);
            return Ok(indicators);
        }

        [HttpGet("{symbol}", Name = "GetForexIndicatorsForPair")]
        public IActionResult Get(string symbol)
        {
            var pair = ForexPairCatalog.Find(symbol);
            if (pair is null)
            {
                return NotFound(new { Message = $"Unknown forex pair '{symbol}'." });
            }

            return Ok(IndicatorGenerator.Generate(pair));
        }
    }
}
