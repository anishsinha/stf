using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class SourceRegionTpoViewModel
    {
        public int CompanyId { get; set; }

        [Display(Name = nameof(Resource.lblSourceRegion), ResourceType = typeof(Resource))]
        public List<int> SelectedSourceRegions { get; set; } = new List<int>();

        [Display(Name = nameof(Resource.lblTerminals), ResourceType = typeof(Resource))]
        public List<int> SelectedTerminals { get; set; } = new List<int>();

        [Display(Name = nameof(Resource.lblBulkPlants), ResourceType = typeof(Resource))]
        public List<int> SelectedBulkPlants { get; set; } = new List<int>();

        public string ApprovedTerminal { get; set; }

        [Display(Name = nameof(Resource.lblApprovedTerminal), ResourceType = typeof(Resource))]
        public int? ApprovedTerminalId { get; set; }

        public string ApprovedBulkPlant { get; set; }

        [Display(Name = nameof(Resource.lblApprovedBulkPlant), ResourceType = typeof(Resource))]
        public int? ApprovedBulkPlantId { get; set; }
    }

    public class SourceRegionRequestModel
    {
        public int OrderId { get; set; }
        public int FuelTypeId { get; set; }
        public int JobId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int CountryId { get; set; }
        public int PricingCodeId { get; set; }
        public int PricingSourceId { get; set; }
        public int PricingTypeId { get; set; }
        public bool IsSupressPricing { get; set; }
        public List<int> SourceRegionIds { get; set; } = new List<int>();
        public List<int> TerminalIds { get; set; } = new List<int>();
        public List<int> BulkPlantIds { get; set; } = new List<int>();

    }

    public class SourceRegionResponseModel : StatusViewModel
    {
        public SourceRegionResponseModel()
        {

        }

        public SourceRegionResponseModel(Status status)
        {
            this.StatusCode = status;
        }

        public List<DropdownDisplayExtendedItem> Terminals { get; set; } = new List<DropdownDisplayExtendedItem>();
        public List<DropdownDisplayExtendedItem> BulkPlants { get; set; } = new List<DropdownDisplayExtendedItem>();
    }
    public class SourceRegionJSONParameter
    {
        public string SourceRegion { get; set; }
        public string SelectedTerminals { get; set; }
        public string SelectedBulkPlants { get; set; }
    }
    public class SourceRegionPricingRequestModel
    {
        public string ParameterJSON { get; set; }
        public int RequestPricingDetailId { get; set; }
        public int? TerminalId { get; set; }
    }
}
