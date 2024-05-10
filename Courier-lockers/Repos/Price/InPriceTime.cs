namespace Courier_lockers.Repos.Price
{
    public class InPriceTime
    {
        public decimal Price { get; set; }
        public string PriceTime { get; set; }
        public string Activate { get; set; }
        public string ReMark { get; set; }
    }

    public class InPriceTimePage
    {
        public string? UserId { get; set; }
    }

    public class UpdatePriceTime 
    {
        public int PriceId { get; set; }
        public decimal Price { get; set; }
        public string PriceTime { get; set; }
        public int Activate { get; set; }
    }
}
