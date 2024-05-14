using Courier_lockers.Repos;

namespace Courier_lockers.Services.Role
{
    public interface IRoleService
    {
        GetInfoResponse GetRolesByUserId(int userId);
    }
}
