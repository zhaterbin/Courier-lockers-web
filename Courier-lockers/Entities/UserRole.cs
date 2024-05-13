using System.ComponentModel.DataAnnotations.Schema;

namespace Courier_lockers.Entities
{
    [Table("UserRole")]
    public class UserRole
    {
        [Column("UserId")]
        public int UserId { get; set; }
        [Column("RoleId")]  
        public int RoleId { get; set; }
    }
}
