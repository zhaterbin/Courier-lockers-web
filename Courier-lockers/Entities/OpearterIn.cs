using System.ComponentModel.DataAnnotations.Schema;

namespace Courier_lockers.Entities
{
    [Table("OpearterIn")]
    public class OpearterIn
    {
        [Column("Operator_Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Operator_Id { get; set; }

        [Column("Operator_Name")]
        public string Operator_Name { get; set;}

        [Column("Price")]
        public decimal Price { get; set;}

        [Column("Storage_ID")]
        public int Storage_ID { get; set; }

        [Column("Storage_Name")]
        public string Storage_Name { get; set;}

        [Column("Date")]
        public string DateTime { get; set; }

        [Column("InCode")]
        public string InCode { get; set; }
    }
}
