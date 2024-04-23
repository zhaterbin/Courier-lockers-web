using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Courier_lockers.Entities;
using Courier_lockers.Repos.Cell;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Courier_lockers.WPF
{
    public partial class MainViewModel : ObservableObject
    {
        HttpClient _client;
        [ObservableProperty]
        List<Box> boxes = new();

        [ObservableProperty]
        int rows;

        [ObservableProperty]
        int cols;
        [ObservableProperty]
        double opacity=1;
        [ObservableProperty]
        Visibility pendingVisibility=Visibility.Collapsed;
        [ObservableProperty]
        string area = "01";
        [ObservableProperty]
        string error;
        [RelayCommand]
        void KeyboardInput(string code)
        {
            switch (code)
            {
                case "←":
                    if (Area.Length==2)
                    {
                        Area = Area[0..1];
                    }
                    else if (Area.Length==1)
                    {
                        Area = "";
                    }
                    break;
                case "×":
                    Area = "";
                    break;
                default:
                    if (Area.Length<2)
                    {
                        Area += code;
                    }
                    break;
            }
        }
        [RelayCommand]
        async Task GetArea()
        {
            Error = "";
            _client = new();
            if (int.TryParse(Area,out int no))
            {
                if (no<0||no>6)
                {
                    Error = "快递柜号必须为01~06";
                    return;
                }
             
            }
            else
            {
                Error = "快递柜号必须为01~06";
                return;
            }
                
        
            var data = new StringContent(JsonConvert.SerializeObject(new CellRequst() { AreaCode = Area }), Encoding.UTF8,"application/json");
            Opacity = 0.5;
            PendingVisibility = Visibility.Visible;
            var re = await _client.PostAsync("http://localhost:9501/api/Cell/GetCellArea", data);

            var cellStr = await re.Content.ReadAsStringAsync();

            var cellReq = JsonConvert.DeserializeObject<CellReqReturn>(cellStr);
            var cells = cellReq.GetX_Y_Zs;
            Rows= cells.Select(n => n.Cell_Y).Distinct().Count();
            Cols = cells.Select(n => n.Cell_Z).Distinct().Count();

            Boxes = cellReq.GetX_Y_Zs.Select(n => new Box { Id = n.Cell_Code, IsOccupied = n.Cell_Status != "Nohave" }).ToList();
            Opacity =1;
            PendingVisibility = Visibility.Collapsed;
        }
    }
}
