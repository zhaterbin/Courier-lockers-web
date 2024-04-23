using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Courier_lockers.WPF
{
   public partial class Box:ObservableObject
    {
        [ObservableProperty]
        string id;
        [ObservableProperty]
        bool isOccupied;
        [ObservableProperty]
        bool isOpened;
        [ObservableProperty]
        bool isChecking;
    }
}
