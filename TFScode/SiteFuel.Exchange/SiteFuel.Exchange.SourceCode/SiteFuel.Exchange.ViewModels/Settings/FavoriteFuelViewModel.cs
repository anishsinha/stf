using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class FavoriteFuelViewModel
    {
        public FavoriteFuelViewModel()
        {
            SelectedFuelTypes = new List<int>();
            FuelTypeList = new List<DropdownDisplayItem>();
        }

        public List<int> SelectedFuelTypes { get; set; }

        public List<DropdownDisplayItem> FuelTypeList { get; set; }
    }
}
