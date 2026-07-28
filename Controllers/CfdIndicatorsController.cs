using Microsoft.AspNetCore.Mvc;

namespace MarketWatchAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CfdIndicatorsController : ControllerBase
    {
        [HttpGet(Name = "GetCfdIndicators")]
        public IActionResult Get()
        {
            var indicators = IndicatorGenerator.GenerateAll(CfdCatalog.All);
            return Ok(indicators);
        }

        [HttpGet("{symbol}", Name = "GetCfdIndicatorsForSymbol")]
        public IActionResult Get(string symbol)
        {
            var cfd = CfdCatalog.Find(symbol);
            if (cfd is null)
            {
                return NotFound(new { Message = $"Unknown CFD '{symbol}'." });
            }

            return Ok(IndicatorGenerator.Generate(cfd));
        }
    }
}
