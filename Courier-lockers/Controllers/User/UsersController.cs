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

namespace Courier_lockers.Controllers.User
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("api/user/[action]")]
    public class UsersController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public UsersController(IUserService userService,IRoleService roleService)
        {
            _userService = userService;
            _roleService=roleService;
        }

        [HttpPost("authenticate")]
      
        public ResultResponse Authenticate(AuthenticateRequest model)
        {
            ResultResponse res = new ResultResponse();

            var response = _userService.Authenticate(model);

            var roles=_roleService.GetRolesByUserId(response.Id);
            var claims = new List<Claim>
              {
                  new Claim(ClaimTypes.Name, response.Username),
                 
              };
            claims.AddRange(roles.Select(n => new Claim(ClaimTypes.Role, n.RoleName)));
            
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            Response.Cookies.Append("x-access-token", response.Token,
              new CookieOptions()
              {
                  Path = "/",
                  HttpOnly = true
              });
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
        public ResultResponse getInfo([FromQuery(Name = "token")] string token)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Headers.Add("X-Token", token);
            string tokenValue= _userService.VerifyJwtToken(token);
            ResultResponse res = new ResultResponse();
            res.code = 200;
            res.data = new Repos.Data
            {
                token = token,
                message = "登录成功",
                state = true
            };
            return res;
        }
    }
}
