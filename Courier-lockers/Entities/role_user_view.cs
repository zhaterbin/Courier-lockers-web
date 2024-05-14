using System.ComponentModel.DataAnnotations.Schema;

namespace Courier_lockers.Entities
{
    [Table("role_user_view")]
    public class role_user_view
    {
        [Column("Id")]
        public int Id { get; set; }
        [Column("FirstName")]
        public string? FirstName { get; set; }
        [Column("LastName")]
        public string? LastName { get; set; }
        [Column("Username")]
        public string Username { get; set; }
        [Column("Password")]
        public string Password { get; set; }
        [Column("avatar")]
        public string avatar { get; set; }
        [Column("introduction")]
        public string introduction { get; set; }
        [Column("RoleName")]
        public string RoleName { get; set; }
    }
}
