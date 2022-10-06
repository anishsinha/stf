using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class DispatchViewModel : BaseViewModel
    {
        public DispatchViewModel()
        {
            InstanceInitialize();
        }

        public DispatchViewModel(Status status)
            : base(status)
        {
            InstanceInitialize(status);
        }

        private void InstanceInitialize(Status status = Status.Failed)
        {
            SelectedDrivers = new List<int>();
            Drivers = new List<DropdownDisplayItem>();
            Country = new CountryViewModel();
        }

        public bool IsTimeCardEnabled { get; set; }

        public List<DropdownDisplayItem> Drivers { get; set; }

        public List<int> SelectedDrivers { get; set; }

        public bool IsScheduleTab { get; set; }        

        public bool IsDispatchTileCollapsed { get; set; }

        public CountryViewModel Country { get; set; }
    }
    public class USPCustomerLoadQueueDetails
    {
        public List<UspJobDetails> jobDetails = new List<UspJobDetails>();
        public List<UspCustomerBrandDetails> customerBrandDetails = new List<UspCustomerBrandDetails>();
        public UspCustomerLoadQueueAttributes customerLoadQueueAttributes = new UspCustomerLoadQueueAttributes();
    }
    public class UspJobDetails
    {
        public int CompanyId { get; set; }
        public int JobId { get; set; }
    }
    public class UspCustomerBrandDetails
    {
        public int SupplierCompanyId { get; set; }
        public int BuyerCompanyId { get; set; }
        public string CustomerId { get; set; }
    }
    public class UspCustomerLoadQueueAttributes
    {
        public string LoadQueueAttributes { get; set; }
        public string DRQueueAttributes { get; set; }
    }
}
