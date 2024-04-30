namespace Courier_lockers.Repos
{
    public class ResultResponse
    {
        public int code { get; set; }
        public Data? data { get; set; }
    }

    public class Data
    {
        public string token { get; set; }
        public string message { get; set; }
        public bool state { get; set; }
    }

}
