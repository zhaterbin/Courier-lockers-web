using System.ComponentModel.DataAnnotations;

namespace Courier_lockers.Repos.UserInReturn
{
    public class AuthenticateRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
