using Courier_lockers.Entities;
using Courier_lockers.Repos;
using Courier_lockers.Services.Price;
using Microsoft.AspNetCore.Mvc;

namespace Courier_lockers.Controllers.Price
{
    [ApiController]
    [Route("api/InStorage/[action]")]
    public class PriceController : ControllerBase
    {


        [HttpPost]
        public async Task<ActionResult<Result>> InPriceRuler([FromServices] IPriceRulerRepository priceRulerRepository,PriceRuler priceRuler)
        {
            var s=await priceRulerRepository.InPriceRuler(priceRuler);
            return Ok(s);
        }
    }
}
