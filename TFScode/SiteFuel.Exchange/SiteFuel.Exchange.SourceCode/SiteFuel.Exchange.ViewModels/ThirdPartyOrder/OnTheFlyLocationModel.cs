using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class OnTheFlyLocationModel : StatusViewModel
    {
        public OnTheFlyLocationModel()
        {
            DeliveryRequest = new CreateDrModel();
            CustomerDetails = new TPOCustomerViewModel();
            AddressDetails = new TPOAddressViewModel();
            FuelDetails = new FuelDetailsViewModel();
            FuelPricingDetails = new FuelPricingViewModel();
            FuelDeliveryDetails = new FuelDeliveryDetailsViewModel();
        }
        public CreateDrModel DeliveryRequest { get; set; }
        public TPOCustomerViewModel CustomerDetails { get; set; }
        public TPOAddressViewModel AddressDetails { get; set; }
        public FuelDetailsViewModel FuelDetails { get; set; }
        public FuelDeliveryDetailsViewModel FuelDeliveryDetails { get; set; }
        public bool IsBadgeMandatory { get; set; }
        public int? PricingQuantityIndicatorTypeId { get; set; }
        public FuelPricingViewModel FuelPricingDetails { get; set; }
        public bool SendInvitationLink { get; set; }
        public string RegionId { get; set; }
        public bool IsSupressOrderPricing { get; set; }
        public int PreferenceSettingId { get; set; }
        public bool IsTBDRequest { get; set; }
    }

    public class CreateDrModel
    {
        public DeliveryReqPriority Priority { get; set; } = DeliveryReqPriority.MustGo;
        public ScheduleQuantityType ScheduleQuantityType { get; set; } = ScheduleQuantityType.Quantity;
        public decimal? RequiredQuantity { get; set; }
        public string DeliveryLevelPO { get; set; } = string.Empty;
        public string BadgeNo1 { get; set; }
        public string BadgeNo2 { get; set; }
        public string BadgeNo3 { get; set; }
        public string DispatcherNote { get; set; }
        public PickupLocationType PickupLocationType { get; set; } = PickupLocationType.Terminal;
        public DropdownDisplayItem Terminal { get; set; } = new DropdownDisplayItem();
        public DropAddressViewModel Bulkplant { get; set; } = new DropAddressViewModel();
    }
}
