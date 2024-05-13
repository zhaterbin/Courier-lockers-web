using Courier_lockers.Data;
using Courier_lockers.Entities;
using Courier_lockers.Models;
using Courier_lockers.Repos.UserInReturn;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ServiceStack;
using ServiceStack.Web;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace Courier_lockers.Services.UserToken
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly ServiceDbContext _context;
        public UserService(IOptions<AppSettings> appSettings, ServiceDbContext context)
        {
            _appSettings = appSettings.Value;
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public string VerifyJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero // 不允许时间偏移
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;
                // 提取 "id" 声明中的值作为登录名
                var loginName = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "id")?.Value;

                // 返回登录名
                return loginName ?? string.Empty;
            }
            catch (Exception ex)
            {
                // 验证失败时，返回错误信息
                return $"fail: {ex.Message}";
            }
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            
            var user = _context.users.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);
           
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

       
            //_context.users.Update(new User { Token=token,Username=user.Username,Password=user.Password});
            //_context.SaveChanges();
            return new AuthenticateResponse(user, token);
        }

        public IEnumerable<User> GetAll()
        {
            return _context.users;
        }

        public User GetById(int id)
        {
            return _context.users.FirstOrDefault(x => x.Id == id);
        }

        // helper methods

        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
