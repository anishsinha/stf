using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class SourceRegionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CompanyId { get; set; }
        public List<DropdownDisplayItem> States { get; set; }
        public List<DropdownDisplayExtendedItem> Cities { get; set; }
        public List<DropdownDisplayItem> Terminals { get; set; }
        public List<DropdownDisplayItem> BulkPlants { get; set; }
        public List<DropdownDisplayItem> Carriers { get; set; }
    }

    public class SourceRegionModel
    {
        public int CountryId { get; set; }
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public List<SourceRegionViewModel> Regions { get; set; }
    }

    public class SourceRegionInputViewModel
    {
        public int TableType { get; set; }
        public List<int> CustomerId { get; set; }
        public List<int> CarrierId { get; set; }
    }
}
