namespace Courier_lockers.Services.Role
{
    public interface IRoleService
    {
        IEnumerable<Courier_lockers.Entities.Role> GetRolesByUserId(int userId);
    }
}
