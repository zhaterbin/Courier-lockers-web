using Courier_lockers.Entities;
using Courier_lockers.Repos;

namespace Courier_lockers.Services.Roles
{
    public interface IRoleService
    {
        List<rolesList> GetRoles();
        GetInfoResponse GetRolesByUserId(int userId);
    }
}
