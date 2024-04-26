using Courier_lockers.Entities;
using Courier_lockers.Repos;

namespace Courier_lockers.Services.Price
{
    public interface IPriceRulerRepository
    {
        Task<Result> InPriceRuler(PriceRuler priceRuler);
    }
}
