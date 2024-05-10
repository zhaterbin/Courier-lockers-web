using Courier_lockers.Data;
using Courier_lockers.Entities;
using Courier_lockers.Repos;
using Courier_lockers.Repos.Price;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WMSService.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Courier_lockers.Services.Price
{
    public class PriceRulerRepository :IPriceRulerRepository
    {
        private readonly ServiceDbContext _context;
        public PriceRulerRepository(ServiceDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> DeleteRuler(int id)
        {
            var priceRuler=_context.priceRulers.FirstOrDefault(x => x.priceId == id);
            if (priceRuler != null)
            {
                _context.priceRulers.Remove(priceRuler);
            }
            return await _context.SaveChangesAsync()>0;
        }

        public async Task<bool> UpdateRuler(UpdatePriceTime priceRuler)
        {
            Result result = new();
            var pr=_context.priceRulers.FirstOrDefault(n=>n.priceId==priceRuler.PriceId);
            if (pr !=null)
            {
                pr.price = priceRuler.Price;
                pr.Activate = priceRuler.Activate;
                pr.PriceTime = priceRuler.PriceTime;
                _context.priceRulers.Update(pr);
            }

            return await _context.SaveChangesAsync() > 0;

        }

        public async Task<Result> InPriceRuler(InPriceTime priceRuler)
        {

            Result result=new ();

            _context.Add(new PriceRuler
            {
                price = priceRuler.Price,
                PriceTime = priceRuler.PriceTime,
                Activate = 1,
                startDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")

            }); 
            if (_context.SaveChanges() > 0)
            {
                result.Success = true;
                result.Messsage = "送入快递柜成功";
            }
            return  result;
        }

        public Task<ActionResult<Page<PriceRuler>>> PriceRulerPage(PriceRuler priceRuler)
        {
            throw new NotImplementedException();
        }

        public async Task<Page<PriceRuler>> PriceRulerPage(Page<InPriceTimePage> priceRuler)
        {
            try
            {

            var dto=priceRuler.requestData ?? new InPriceTimePage();
                //dto.UserId = null;
           var prices= _context.priceRulers.Where(f => true);

            //todo 以后再写
            if (!string.IsNullOrEmpty(dto.UserId))
            {
                prices = prices.Where(f => f.PriceTime == dto.UserId);
            }
            var totalCount = prices.Count();
            var pageCount = (totalCount + priceRuler.pageSize - 1) / priceRuler.pageSize;
            var result = new Page<PriceRuler>
            {
                current = priceRuler.current,
                pageSize = priceRuler.pageSize,
                total = totalCount,
                data = new List<PriceRuler>()
            };
            if (result.current > pageCount)
            {
                return result;
            }
            result.data = prices.OrderBy(f => f.startDateTime).Skip((priceRuler.current - 1) * priceRuler.pageSize).Take(priceRuler.pageSize)
             .ToList();
                return result;
            }
            catch(Exception ex)
            {

            }
            return null;
        }


    }
}
