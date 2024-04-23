namespace Courier_lockers.Repos
{
    public class Result
    {
        public bool Success { get; set; }
        public string Messsage { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public string cell_code { get; set; }
        public string Isopening { get; set; }
    }
    public class Datase
    {
        public string cell_code { get; set; }
        public string Isopening { get; set; }
    }
}
