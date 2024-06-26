﻿using Courier_lockers.Data;
using Courier_lockers.Entities;
using Courier_lockers.Repos;
using ServiceStack;

namespace Courier_lockers.Services.Roles
{
    public class RoleService:IRoleService
    {
        private readonly ServiceDbContext? _context;

        public RoleService(ServiceDbContext context)
        {
            _context=context?? throw new ArgumentNullException(nameof(context));
        }

        public List<rolesList> GetRoles()
        {
             return  _context.roleUsers.Where(f=>true).Select(v=>new rolesList
             {
                 Id=v.Id,
                 Key=v.RoleName,
                 Name=v.Username,
                 Description=v.introduction
             }).ToList();
        }

        public GetInfoResponse GetRolesByUserId(int userId)
        {
            var st=_context?.roleUsers.FirstOrDefault(x => x.Id == userId);
   
            return  new GetInfoResponse { avatar = st.avatar, introduction = st.introduction, name = st.Username, roles = new List<string> { st.RoleName } };
        }
    }
}
