using Courier_lockers.Helper;
using Courier_lockers.Repos.UserInReturn;
using Courier_lockers.Services.UserToken;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Courier_lockers.Repos;
using System.Net;
using ServiceStack;
using Courier_lockers.Services.Role;
using Microsoft.Extensions.Caching.Memory;

namespace Courier_lockers.Controllers.User
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("api/user/[action]")]
    public class UsersController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IMemoryCache _cache;

        public UsersController(IUserService userService, IRoleService roleService, IMemoryCache cache)
        {
            _userService = userService;
            _roleService = roleService;
            _cache = cache;
        }

        [HttpPost("authenticate")]
      
        public ResultResponse Authenticate(AuthenticateRequest model)
        {
            ResultResponse res = new ResultResponse();

            var response = _userService.Authenticate(model);

            //var roles=_roleService.GetRolesByUserId(response.Id);
            var claims = new Claim[]
             {
                  new Claim(ClaimTypes.Name, response.Username),
                 
              };
            //claims.AddRange(roles.Select(n => new Claim(ClaimTypes.Role, n.RoleName)));
            
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            Response.Cookies.Append("x-access-token", response.Token,
              new CookieOptions()
              {
                  Path = "/",
                  HttpOnly = true
              });
            _cache.Set("x-access-token", response.Token);
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            try
            {
                if (response != null)
                {
                    res.code = 200;
                    res.data = new Repos.Data
                    {
                        token = response.Token,
                        message = "登录成功",
                        state = true
                    };
                }
            }catch(Exception ex)
            {

            }
            
            return res;
        }

        /// <summary>
        /// todo这里没写
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        public GetInfoResponse getInfo()
        {
            //var response = new HttpResponseMessage(HttpStatusCode.OK);
            //response.Headers.Add("X-Token", token);
            var tokenCache = _cache.Get("x-access-token")?.ToString();
            //string accessToken = HttpContext.Request.Cookies[tokenCache];
            var id=_userService.VerifyJwtToken(tokenCache);
            var st=_roleService.GetRolesByUserId(int.Parse(id));
            return st;
        }
    }
}
