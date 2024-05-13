using Courier_lockers.Data;
using Courier_lockers.Entities;
using ServiceStack;

namespace Courier_lockers.Services.Role
{
    public class RoleService:IRoleService
    {
        private readonly ServiceDbContext? _context;

        public RoleService(ServiceDbContext context)
        {
            _context=context?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<Courier_lockers.Entities.Role> GetRolesByUserId(int userId)
        {
            var res = _context.userRoles.Where(n => n.UserId == userId).Select(n => n.RoleId);
       
            return _context.roles.Where(n => res.Any(m => m == n.Id)).ToList();
        }
    }
}
