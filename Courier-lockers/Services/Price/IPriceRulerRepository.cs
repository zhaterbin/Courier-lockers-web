using Courier_lockers.Entities;
using Courier_lockers.Repos;
using Courier_lockers.Repos.Price;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WMSService.Models;

namespace Courier_lockers.Services.Price
{
    public interface IPriceRulerRepository
    {
        Task<bool> DeleteRuler(int id);
        Task<Result> InPriceRuler(InPriceTime priceRuler);
        Task<ActionResult<Page<PriceRuler>>> PriceRulerPage(PriceRuler priceRuler);
        Task<Page<PriceRuler>> PriceRulerPage(Page<InPriceTimePage> priceRuler);
    }
}
