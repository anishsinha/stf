using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class UnknownDeliveryTankRequestModel
    {
        public List<JobTankDetailsViewModel> JobTanks { get; set; } = new List<JobTankDetailsViewModel>();
        //public List<BuyerSupplierMapping> BuyerSupplierMappings { get; set; } = new List<BuyerSupplierMapping>();
    }

    public class BuyerSupplierMapping
    {
        public List<int> BuyerIds { get; set; }
        public int SupplierId { get; set; }
        public decimal Threshold { get; set; }
    }

    public class UnknownDeliveryTankResponseModel
    {

    }
}
