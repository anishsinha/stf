using SiteFuel.Exchange.Core.StringResources;
using System;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class AssetDuplicateViewModel
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }

        [Display(Name = nameof(Resource.lblName), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string Name { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string Year { get; set; }

        public string Color { get; set; }

        public string FuelType { get; set; }

        public decimal? FuelCapacity { get; set; }

        public string VehicleId { get; set; }

        public string TelematicsProvider { get; set; }

        public string LicensePlateState { get; set; }

        public string LicensePlate { get; set; }

        public string AssetClass { get; set; }

        public string Vendor { get; set; }

        public string Description { get; set; }

        public string Subcontractor { get; set; }

        public DateTimeOffset DateAdded { get; set; }

        public bool IsSelected { get; set; }
    }
}
