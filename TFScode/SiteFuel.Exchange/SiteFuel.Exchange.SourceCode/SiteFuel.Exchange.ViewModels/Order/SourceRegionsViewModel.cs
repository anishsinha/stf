using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class SourceRegionsViewModel
    {
        public int CompanyId { get; set; }

        [Display(Name = nameof(Resource.lblSourceRegion), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public List<int> SelectedSourceRegions { get; set; } = new List<int>();

        [Display(Name = nameof(Resource.lblTerminals), ResourceType = typeof(Resource))]
        public List<int> SelectedTerminals { get; set; } = new List<int>();

        [Display(Name = nameof(Resource.lblBulkPlants), ResourceType = typeof(Resource))]
        public List<int> SelectedBulkPlants { get; set; } = new List<int>();

        [Display(Name = nameof(Resource.lblApprovedTerminal), ResourceType = typeof(Resource))]
        public string ApprovedTerminal { get; set; }

        [Display(Name = nameof(Resource.lblFreightPricingMethod), ResourceType = typeof(Resource))]
        public FreightPricingMethod FreightPricingMethod { get; set; } = FreightPricingMethod.Manual;

        public int? ApprovedTerminalId { get; set; }

        [Display(Name = nameof(Resource.lblApprovedBulkPlant), ResourceType = typeof(Resource))]
        public string ApprovedBulkPlant { get; set; }

        public int? ApprovedBulkPlantId { get; set; }

        public int? FuelTypeId { get; set; }
        public int? JobId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int PricingCodeId { get; set; }
        public int PricingSourceId { get; set; }
        public int CountryId { get; set; }
        public int OrderId { get; set; }
        public int PricingTypeId { get; set; }
    }
}
