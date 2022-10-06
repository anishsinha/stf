using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class DrFilterPreferencesModel : StatusViewModel
    {
        public string Id { get; set; }
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public string FilterData { get; set; }
        public string RegionId { get; set; }
        public DateTimeOffset Date { get; set; }
    }

    public class CalendarFilterModel
    {
        public List<string> Customers { get; set; } = new List<string>();
        public List<int> Locations { get; set; } = new List<int>();
        public List<string> Vessels { get; set; } = new List<string>();
        public bool LocationType { get; set; }
        public List<int> Priorities { get; set; } = new List<int>();
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
