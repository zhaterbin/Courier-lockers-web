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

namespace Courier_lockers.Controllers.User
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("api/user/[action]")]
    public class UsersController : ControllerBase
    {

        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
      
        public ResultResponse Authenticate(AuthenticateRequest model)
        {
            ResultResponse res = new ResultResponse();
            var response = _userService.Authenticate(model);


            var claims = new Claim[]
              {
                  new Claim(ClaimTypes.Name, response.Username),
              };
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

        [Authorize]
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
