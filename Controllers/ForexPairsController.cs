using Microsoft.AspNetCore.Mvc;

namespace MarketWatchAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ForexPairsController : ControllerBase
    {
        [HttpGet(Name = "GetForexPairs")]
        public IActionResult Get()
        {
            return Ok(new
            {
                Major = ForexPairCatalog.MajorPairs,
                Minor = ForexPairCatalog.MinorPairs
            });
        }
    }
}
