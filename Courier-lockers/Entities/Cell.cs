using System.ComponentModel.DataAnnotations.Schema;

namespace Courier_lockers.Entities
{
    [Table("Cell")]
    public class Cell
    {
        [Column("CELL_ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CELL_ID { get; set; }

        [Column("AREA_ID")]
        public int AREA_ID { get; set; }

        [Column("CELL_NAME")]
        public string CELL_NAME { get; set; }

        [Column("CELL_CODE")]
        public string CELL_CODE { get; set; }

        [Column("CELL_TYPE")]
        public string CELL_TYPE { get; set; }


        [Column("CELL_STATUS")]
        public string CELL_STATUS { get; set; }

        [Column("RUN_STATUS")]
        public string RUN_STATUS { get; set; }

        [Column("SHELF_TYPE")]
        public string SHELF_TYPE { get; set; }

        [Column("CELL_X")]
        public string CELL_X { get; set; }

        [Column("CELL_Y")]
        public string CELL_Y { get; set; }

        [Column("CELL_Z")]
        public string CELL_Z { get; set; }

        [Column("Cabinet")]
        public string Cabinet { get; set; }
    }
}
