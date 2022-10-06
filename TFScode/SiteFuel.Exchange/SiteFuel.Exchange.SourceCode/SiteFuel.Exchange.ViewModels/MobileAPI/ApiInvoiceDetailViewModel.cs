using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class ApiInvoiceDetailViewModel 
    {
        public ApiInvoiceDetailViewModel()
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            FuelRequestFee = new FuelRequestFeeViewModel();
        }

        public ApiInvoiceViewModel Invoice { get; set; }

        public string SupplierCompanyName { get; set; }

        public string PoNumber { get; set; }

        public string JobName { get; set; }

        public List<List<AssetDropViewModel>> Assets { get; set; }

        public FuelRequestFeeViewModel FuelRequestFee { get; set; }
    }
}

