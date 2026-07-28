using Microsoft.AspNetCore.Mvc;

namespace MarketWatchAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CfdsController : ControllerBase
    {
        [HttpGet(Name = "GetCfds")]
        public IActionResult Get()
        {
            return Ok(CfdCatalog.All);
        }

        [HttpGet("{symbol}", Name = "GetCfdBySymbol")]
        public IActionResult Get(string symbol)
        {
            var cfd = CfdCatalog.Find(symbol);
            if (cfd is null)
            {
                return NotFound(new { Message = $"Unknown CFD '{symbol}'." });
            }

            return Ok(cfd);
        }
    }
}
