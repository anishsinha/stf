using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.FilldService
{
    public class FilldTruckViewModel
    {
        public long territory_id { get; set; }
        public string external_truck_id { get; set; }
        public string truck_name { get; set; }
        public string license_plate { get; set; }
        public string make { get; set; }
        public string model { get; set; }
    }
    public class TruckDataModel
    {
        public int Id { get; set; }
        public int TerritoryId { get; set; }
        public int Dealer_Id { get; set; }
        public DateTimeOffset Created_at { get; set; }
        public DateTimeOffset Updated_at { get; set; }
    }
    public class TruckDataResponseModel
    {
        public TruckDataModel Truck { get; set; }
    }
    public class FilldTruckResponseModel : FilldStatusViewModel
    {
        public TruckDataResponseModel data { get; set; }
    }
}
