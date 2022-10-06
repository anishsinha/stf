using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.FilldService
{
    public class FilldDriverViewModel
    {
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string external_driver_id { get; set; }
        public string emergency_contact_name { get; set; }
        public string emergency_contact_phone { get; set; }
    }
    public class DriverDataModel
    {
        public long Id { get; set; }
        public long Dealer_id { get; set; }
        public DateTimeOffset Created_at { get; set; }
        public DateTimeOffset Updated_at { get; set; }
    }
    public class FilldDriverResponseModel : FilldStatusViewModel
    {
        public DriverDataModel Data { get; set; }
    }
}
