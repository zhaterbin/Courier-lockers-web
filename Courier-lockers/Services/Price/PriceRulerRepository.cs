using Courier_lockers.Data;
using Courier_lockers.Entities;
using Courier_lockers.Repos;

namespace Courier_lockers.Services.Price
{
    public class PriceRulerRepository :IPriceRulerRepository
    {
        private readonly ServiceDbContext _context;
        public PriceRulerRepository(ServiceDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Result> InPriceRuler(PriceRuler priceRuler)
        {
            Result result=new ();
            _context.Add(priceRuler);
            if (_context.SaveChanges() > 0)
            {
                result.Success = true;
                result.Messsage = "送入快递柜成功";
            }
            return  result;
        }
    }
}
