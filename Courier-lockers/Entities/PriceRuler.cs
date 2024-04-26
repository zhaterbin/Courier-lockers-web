using System.ComponentModel.DataAnnotations.Schema;

namespace Courier_lockers.Entities
{
    [Table("PriceRuler")]
    public class PriceRuler
    {
        [Column("PriceId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int priceId { get; set; }

        [Column("Price")]
        public decimal price { get; set; }

        [Column("PriceTime")]
        public string PriceTime { get; set; }

        [Column("StartDateTime")]
        public string? startDateTime { get; set; }

        [Column("EndDateTime")]
        public string? endDateTime { get; set; }

        [Column("Activate")]
        public int Activate { get; set; }
    }
}
