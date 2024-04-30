namespace Courier_lockers.Repos
{
    public class Result
    {
        public bool Success { get; set; }
        public string Messsage { get; set; }
        public DataBase data { get; set; }
    }

    public class DataBase
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
