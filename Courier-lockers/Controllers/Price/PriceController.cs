﻿using Courier_lockers.Entities;
using Courier_lockers.Repos;
using Courier_lockers.Repos.Price;
using Courier_lockers.Services.Price;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WMSService.Models;

namespace Courier_lockers.Controllers.Price
{
    [ApiController]
    [Route("api/InStorage/[action]")]
    public class PriceController : ControllerBase
    {


        [HttpPost]
        public async Task<ActionResult<Result>> InPriceRuler([FromServices] IPriceRulerRepository priceRulerRepository,InPriceTime priceRuler)
        {
            var s=await priceRulerRepository.InPriceRuler(priceRuler);
            return Ok(s);
        }

        [HttpPost]
        public async Task<ActionResult<Page<PriceRuler>>> PriceRulerPage([FromServices] IPriceRulerRepository priceRulerRepository, Page<InPriceTimePage> priceRuler)
        {
           var s= await priceRulerRepository.PriceRulerPage(priceRuler);
            return Ok(s);
        }


    }
}
