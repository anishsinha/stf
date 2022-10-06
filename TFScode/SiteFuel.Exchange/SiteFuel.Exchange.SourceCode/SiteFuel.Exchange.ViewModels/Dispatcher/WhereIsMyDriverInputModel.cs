using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Dispatcher
{
    public class WhereIsMyDriverInputModel : DataTableAjaxPostModel
    {
        public WhereIsMyDriverInputModel()
        {
            States = new List<string>();
            Priorities = new List<int>();
            DriverSearch = string.Empty;
        }
        public string RegionId { get; set; }
        public List<string> States { get; set; }
        public List<int> Priorities { get; set; }
        public DeliveryReqPriority Priority { get; set; }
        public DateTimeOffset FromDate { get; set; }
        public DateTimeOffset ToDate { get; set; }
        public string DriverSearch { get; set; }

        public string DispacherId { get; set; }
        public int CustomerId { get; set; }
        public List<int> Carriers { get; set; }
        public Boolean IsShowCarrierManaged { get; set; }
        public string InventoryCaptureType { get; set; }
    }

    public class BuyerWhereIsMyDriverInputModel : DataTableAjaxPostModel
    {
        public BuyerWhereIsMyDriverInputModel()
        {
            States = new List<string>();
            Priorities = new List<int>();
            DriverSearch = string.Empty;
        }
        public List<string> States { get; set; }
        public List<int> Priorities { get; set; }
        public DeliveryReqPriority Priority { get; set; }
        public DateTimeOffset FromDate { get; set; }
        public DateTimeOffset ToDate { get; set; }
        public string DriverSearch { get; set; }
        public string SupplierCompanyIds { get; set; }
        public string CarrierCompanyIds { get; set; }
        public string LocationIds { get; set; }
        public bool IsRequestFromDashboard { get; set; }
        public int CountryId { get; set; }
        public Boolean IsShowCarrierManaged { get; set; }
        public string InventoryCaptureType { get; set; }
    }
}
