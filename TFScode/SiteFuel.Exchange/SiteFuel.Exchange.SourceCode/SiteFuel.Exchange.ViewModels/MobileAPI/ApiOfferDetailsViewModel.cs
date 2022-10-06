using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.Offer;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class ApiOfferDetailsViewModel : StatusViewModel
    {
        public ApiOfferDetailsViewModel()
        {
            FuelPricing = new FuelPricingViewModel();
            FuelFees = new FuelFeesViewModel();
            Locations = new List<OfferLocationViewModel>();
        }

        public FuelPricingViewModel FuelPricing { get; set; }

        public FuelFeesViewModel FuelFees { get; set; }

        [Display(Name = nameof(Resource.lblFuel), ResourceType = typeof(Resource))]
        public int? FuelTypeId { get; set; }

        [Display(Name = nameof(Resource.lblNumber), ResourceType = typeof(Resource))]
        public string OfferNumber { get; set; }

        [Display(Name = nameof(Resource.lblType), ResourceType = typeof(Resource))]
        public string OfferType { get; set; }

        [Display(Name = nameof(Resource.lblFuelType), ResourceType = typeof(Resource))]
        public string FuelType { get; set; }

        public int OfferPricingId { get; set; }

        public int SupplierCompanyId { get; set; }

        public int JobId { get; set; }

        public int Quantity { get; set; }

        public List<OfferLocationViewModel> Locations { get; set; }

        public bool IsActive { get; set; }
    }
}

