using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Forcasting
{
    public partial class ForcastingServiceSettingViewModel
    {
        [Display(Name = "Enable IMS Forecasting Setting")]
        public bool IsEnabled { get; set; } = false;

        public int Id { get; set; }
        [RequiredIfTrue("IsEnabled", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [DisplayName("Band Period")]
        public int? BandPeriod { get; set; }

        [RequiredIfTrue("IsEnabled", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [DisplayName("Start Timing")]
        public string StartTime { get; set; }

        //[Range(double.Epsilon, double.MaxValue)]
        [Range(0.0001, 100.00, ErrorMessage = "Please enter valid data.")]
        [RequiredIfTrue("IsEnabled", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [DisplayName("Minimum Load")]
        public decimal? MinimumLoad { get; set; }

        [Range(0.0001, 100.00, ErrorMessage = "Please enter valid data.")]
        [RequiredIfTrue("IsEnabled", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [DisplayName("Average Load")]
        public decimal? AverageLoad { get; set; }

        //[Required(ErrorMessage = "Select Inventory Priority")]
        [DisplayName("Priority Type")]
        public int? ForcastingType { get; set; } = 1;

        //[Required(ErrorMessage = "Select Inventory UOM")]
        [DisplayName("Inventory UOM")]
        public int? InventoryUOM { get; set; }

        [Range(0.0001, int.MaxValue, ErrorMessage = "Please enter valid data.")]
        [RequiredIfTrue("IsEnabled", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [DisplayName("Retain - CouldGo")]
        public int? Retain { get; set; }

        [Range(0.0001, int.MaxValue, ErrorMessage = "Please enter valid data.")]
        [RequiredIfTrue("IsEnabled", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [DisplayName("Safety Stock - ShouldGo")]
        public int? SafetyStock { get; set; }

        [Range(0.0001, int.MaxValue, ErrorMessage = "Please enter valid data.")]
        [RequiredIfTrue("IsEnabled", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [DisplayName("Runout Level - MustGo")]
        public int? RunoutLevel { get; set; }

        [DisplayName("Auto DR Creation")]
        public bool IsAutoDRCreation { get; set; } = false;

        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid data.")]
        [RequiredIfTrue("IsEnabled", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [DisplayName("Start Buffer")]
        public int? StartBuffer { get; set; }
        public int? StartBufferUOM { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid data.")]
        [RequiredIfTrue("IsEnabled", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [DisplayName("End Buffer")]
        public int? EndBuffer { get; set; }
        public int? EndBufferUOM { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid data.")]
        [RequiredIfTrue("IsEnabled", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [DisplayName("Retain Time Buffer")]
        public int? RetainTimeBuffer { get; set; }
        public int? RetainTimeBufferUOM { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid data.")]
        [RequiredIfTrue("IsEnabled", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [DisplayName("Lead Time")]
        public int? LeadTime { get; set; }
        public int? LeadTimeUOM { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid data.")]
        [RequiredIfTrue("IsEnabled", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [DisplayName("Supplier Time")]
        public int? SupplierLead { get; set; }
        public int? SupplierLeadUOM { get; set; }

        [DisplayName("Otto(Auto DR Creation)")]
        public bool IsOttoAutoDRCreation { get; set; } = false;
        public bool IsOttoAutoDRCreationDisplay { get; set; } = false;
        public int? IsOttoAutoDRCreationAllCarrier { get; set; } = -1;
        public bool IsOttoScheduleCreation { get; set; } = false;
        public List<DropdownDisplayItem> CarrierList { get; set; } = new List<DropdownDisplayItem>();
        public List<int> SelectedCarrierList { get; set; } = new List<int>();
        public DateTimeOffset CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public bool IsEditableTpo { get; set; } = false;
    }
}
