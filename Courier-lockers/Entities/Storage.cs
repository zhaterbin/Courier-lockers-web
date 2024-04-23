using System.ComponentModel.DataAnnotations.Schema;

namespace Courier_lockers.Entities
{
    [Table("Storage")]
    public class Storage
    {
        [Column("STORAGE_ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int STORAGE_ID { get; set; }

        [Column("CELL_ID")]
        public int CELL_ID { get; set; }

        [Column("Bar_Code")]
        public string Bar_Code { get; set; }

        [Column("Bar_Name")]
        public string Bar_Name { get; set; }

        [Column("Entry_Time")]
        public string Entry_Time { get; set; }

        [Column("Storage_Name")]
        public string Storage_Name { get; set; }

        [Column("InCode")]
        public string InCode { get; set; }
    }
}
