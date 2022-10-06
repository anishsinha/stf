using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class FtlDriverDropOrderViewModel : DriverDropOrderViewModel
    {
        public List<DemurrageDetailsViewModel> DemurrageDetails { get; set; } = new List<DemurrageDetailsViewModel>();

        public DispatchLocationViewModel FuelDropLocation { get; set; } 

        public FuelTruckRetainDetailsViewModel FuelTruckRetainDetails { get; set; }

        public bool IsSplitTank { get; set; }

        public bool IsSplitLoad { get; set; }

        public string SplitLoadChainId { get; set; }

        public int FuelSurchargeDistance { get; set; }

        public new FtlDriverDropOrderViewModel Clone(int orderId = 0)
        {
            var thisObject = (FtlDriverDropOrderViewModel)this.MemberwiseClone();
            thisObject.Driver = thisObject.Driver.Clone();
            if (orderId > 0)
            {
                thisObject.OrderId = orderId;
            }
            return thisObject;
        }
    }
}
