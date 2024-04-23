using System.ComponentModel.DataAnnotations.Schema;

namespace Courier_lockers.Entities
{
    [Table("edpmain")]
    public class edpmain
    {
        [Column("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("EDPId")]
        public int EDPId { get; set; }

        [Column("EDPName")]
        public string EDPName { get; set; }

        [Column("EDPList")]
        public string EDPList { get; set; }

    }
}
