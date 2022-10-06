using System.Collections.Generic;
using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class SalesDataRequestModel
    {
        public string RegionId { get; set; }
        public List<CustomerJobsModel> Jobs { get; set; }
        public int Priority { get; set; }
        public int SelectedTab { get; set; }
        public int CompanyId { get; set; }
        public bool isFromExchangeApiForDataExpose { get; set; }
    }

    public class CustomerJobsModel
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int JobId { get; set; }
        public string JobAddress { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string ZipCode { get; set; }
        public string TimeZoneName { get; set; }
        public string LocationName { get; set; }
        public JobLocationTypes LocationTypeId { get; set; }
        public InventoryDataCaptureType InventoryDataCaptureType { get; set; }
        public int LocationManagedType { get; set; }
        public int UOM { get; set; }
    }
}
