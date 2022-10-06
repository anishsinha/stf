using System.Collections.Generic;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.ComponentModel.DataAnnotations;
using Foolproof;

namespace SiteFuel.Exchange.ViewModels
{
    public class BrokerFuelRequestDetailsViewModel : StatusViewModel
    {
        public BrokerFuelRequestDetailsViewModel()
        {
            InstanceInitialize();
        }

        public BrokerFuelRequestDetailsViewModel(Status status) : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            PrivateSupplierList = new PrivateSupplierListViewModel();
            PrivateSupplierList.Id = 0;
            DifferentFuelPrices = new List<DifferentFuelPriceViewModel>();
            FuelQuantity = new FuelQuantityViewModel();
            FuelPricing = new BrokerFuelPricingViewModel();
            IsOverageAllowed = false;
            OrderTypeId = (int)OrderType.Spot;
            FuelDeliveryDetails = new FuelDeliveryDetailsViewModel();
            FuelPriceMargin = new BrokerMarginViewModel();
            FuelDisplayGroupId = (int)ProductDisplayGroups.CommonFuelType;
        }
        public int FuelDisplayGroupId { get; set; }
        public int OrderId { get; set; }
        public string PoNumber { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblJobName), ResourceType = typeof(Resource))]
        public int JobId { get; set; }
        public string JobName { get; set; }
        public string Address { get; set; }
        public string Location { get; set; }
        public int StateId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public bool IsVarious { get; set; }
        public int CountryId { get; set; }
        public string CountryCode { get; set; }
        public int FuelTypeId { get; set; }
        public string FuelType { get; set; }
        public PrivateSupplierListViewModel PrivateSupplierList { get; set; }
        public FuelQuantityViewModel FuelQuantity { get; set; }
        public BrokerFuelPricingViewModel FuelPricing { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblOverageAllowed), ResourceType = typeof(Resource))]
        public bool IsOverageAllowed { get; set; }

        public decimal OverageAllowedPercent { get; set; }

        [Display(Name = nameof(Resource.lblOrderType), ResourceType = typeof(Resource))]
        public int OrderTypeId { get; set; }

        public string NonStandardFuelDescription { get; set; }

        [Display(Name = nameof(Resource.lblFuelPriceByQuantity), ResourceType = typeof(Resource))]
        public bool IsDifferentFuelPriceEntered { get; set; }
        public BrokerMarginViewModel FuelPriceMargin { get; set; }

        [RequiredIfTrue("IsDifferentFuelPriceEntered", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public List<DifferentFuelPriceViewModel> DifferentFuelPrices { get; set; }

        public FuelDeliveryDetailsViewModel FuelDeliveryDetails { get; set; }

        public int CreatedBy { get; set; }

        public int? FreightOnBoard { get; set; }

        public bool IsFTLEnabled { get; set; }
        public int? TerminalId { get; set; }

        public bool IsMarineLocation { get; set; }
    }
}
