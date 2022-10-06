using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class FuelFeesViewModel : StatusViewModel
    {
        public FuelFeesViewModel()
        {
            
        }

        public FuelFeesViewModel(Status status) : base(status)
        {
           
        }

        public int Id { get; set; }

        public Currency Currency { get; set; }

        public TruckLoadTypes TruckLoadType { get; set; }

        public UoM UoM { get; set; }

        public List<FeesViewModel> FuelRequestFees { get; set; } = new List<FeesViewModel>();

        public List<FuelRequestResaleFeeViewModel> ResaleFee { get; set; } = new List<FuelRequestResaleFeeViewModel>();

        public List<DiscountLineItemViewModel> DiscountLineItems { get; set; } = new List<DiscountLineItemViewModel>();

        public FeesViewModel ProcessingFee { get; set; }

        public FuelSurchargeFreightFeeViewModel FuelSurchargeFreightFee { get; set; } = new FuelSurchargeFreightFeeViewModel();

        public FreightCostFeeViewModel FreightCostFee { get; set; } = new FreightCostFeeViewModel();
    }
}
