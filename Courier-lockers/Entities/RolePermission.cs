using System.ComponentModel.DataAnnotations.Schema;

namespace Courier_lockers.Entities
{
    [Table(nameof(RolePermission))]
    public class RolePermission
    {
        [Column("RoleId")]
        public int RoleId { get; set; }
        [Column("PermissionId")]
        public int PermissionId { get; set; }
    }
}
