namespace Courier_lockers.Repos.Cell
{
    public class CellReqReturn
    {
        public string Area { get; set; }
        public List<CellX_Y_Z> GetX_Y_Zs { get; set; }
    }
    public class CellX_Y_Z
    {
        public string Cell_Type { get; set; }
        public string Cell_Status { get;set; }
        public string Run_Status { get; set; }
        public string Shelf_Type { get; set; }

        public string Cell_Code { get; set; }
        public string Cell_X { get; set; }
        public string Cell_Y { get; set; }
        public string Cell_Z { get; set; }

    }

    public class RequestUnity
    {
        public string code { get; set; }
        public string IsOpening { get; set; }
    }
}
