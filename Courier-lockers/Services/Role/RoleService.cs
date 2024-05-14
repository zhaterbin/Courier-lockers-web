using Courier_lockers.Data;
using Courier_lockers.Entities;
using Courier_lockers.Repos;
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

        public GetInfoResponse GetRolesByUserId(int userId)
        {
            var st=_context?.roleUsers.FirstOrDefault(x => x.Id == userId);
   
            return  new GetInfoResponse { avatar = st.avatar, introduction = st.introduction, name = st.Username, roles = new List<string> { st.RoleName } };
        }
    }
}
