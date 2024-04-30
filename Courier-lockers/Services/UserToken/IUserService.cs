using Courier_lockers.Entities;
using Courier_lockers.Repos.UserInReturn;

namespace Courier_lockers.Services.UserToken
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<User> GetAll();
        User GetById(int id);
        string VerifyJwtToken(string token);
    }
}
