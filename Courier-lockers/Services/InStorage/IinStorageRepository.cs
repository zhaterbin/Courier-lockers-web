using Courier_lockers.Repos;
using Courier_lockers.Repos.InStorage;

namespace Courier_lockers.Services.InStorage
{
    public interface IInStorageRepository
    {
        Task<Result> EntryStorage(ReqStorage reqStorage);
        Task<Result> OutStorage(ReqOutStorage reqOutStorage);
    }
}
