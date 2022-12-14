using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class DeliveryRequestReportGridViewModel
    {
        public DeliveryRequestReportGridViewModel()
        {

        }

        public string Location { get; set; }
        public string RegionName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerBrandID { get; set; }
        public string ProductType { get; set; }
        public decimal RequestedQuantity { get; set; }
        public string LocationId { get; set; }
        public string PoNumber { get; set; }
        public string DrId { get; set; }
        public int TfxJobId { get; set; }
        public string RegionId { get; set; }
        public int OrderId { get; set; }

        public DeliveryReqPriority Priority { get; set; }
        public int ProductTypeId { get; set; }
        public bool IsRecurringSchedule { get; set; }
        public bool IsAutoDR { get; set; }
        public string UoM { get; set; }

    }

    public class DRReportFilterViewModel
    {
        public List<DropdownDisplayExtended> Regions { get; set; } = new List<DropdownDisplayExtended>();

        public List<DropdownDisplayExtendedItem> Locations { get; set; } = new List<DropdownDisplayExtendedItem>();
    }

    public class DRReportFilterInputViewModel
    {
        public string RegionIds { get; set; }

        public string LocationIds { get; set; }

        public DateTimeOffset FromDate { get; set; }

        public DateTimeOffset ToDate { get; set; }

        public int CompanyId { get; set; }
    }

    public class JobXCustomerBrandId
    {
        public string CustomerBrandID { get; set; }
        public int JobId { get; set; }
        public int BuyerCompanyId { get; set; }

    }
}
