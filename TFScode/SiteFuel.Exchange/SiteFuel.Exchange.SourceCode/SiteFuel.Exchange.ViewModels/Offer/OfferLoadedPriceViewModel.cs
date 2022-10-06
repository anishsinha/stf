using System.Collections.Generic;
namespace SiteFuel.Exchange.ViewModels.Offer
{
    public class OfferLoadedPriceViewModel : BaseViewModel
    {
        public decimal TotalDropAmount { get; set; }
        public decimal TotalTaxAmount { get; set; }
        public decimal TotalFeesAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OfferEstimatedFeeViewModel> EstimatedFees { get; set; }
    }
}
