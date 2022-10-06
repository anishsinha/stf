using SiteFuel.Exchange.Core.StringResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class LFBolEditViewModel
    {
        public LFBolEditViewModel()
        {
            LiftRecord = new LFRecordsGridViewModel();
        }
        public LFRecordsGridViewModel LiftRecord { get; set; }

        [Display(Name = nameof(Resource.lblLiftDate), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public DateTime LiftDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblGrossQuantity), ResourceType = typeof(Resource))]
        public decimal GrossQuantity { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblNetQuantity), ResourceType = typeof(Resource))]
        public decimal NetQuantity { get; set; }

        [Display(Name = nameof(Resource.lblBadgeNumber), ResourceType = typeof(Resource))]
        public string BadgeNumber { get; set; }

        [Display(Name = nameof(Resource.lblFuel), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public int FuelTypeId { get; set; }

        //[Display(Name = nameof(Resource.lblReasonCode), ResourceType = typeof(Resource))]
        //[Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        //public int ReasonCodeId { get; set; }

        [Display(Name = nameof(Resource.lblTerminalName), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public int TerminalId { get; set; }

        [Display(Name = nameof(Resource.lblNote) ,ResourceType =typeof(Resource))]
        public string Notes { get; set; }

        [Display(Name = nameof(Resource.lblBolAndLiftNumber), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string BolNumber { get; set; }

        public int InvoiceFtlDetailId { get; set; }

        [Display(Name = nameof(Resource.lblSelectBolTerminal), ResourceType = typeof(Resource))]
        public int InvoiceFtlDetailIdFromList { get; set; }

        public List<DropdownDisplayItem> InvoiceFtlDetailsList { get; set; } = new List<DropdownDisplayItem>();

        public int? OrderId { get; set; }

        [Display(Name = nameof(Resource.lblTerminalName), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string DisplayTerminalName { get; set; }

        public int PricingSourceId { get; set; }

        public PickupLocationType PickUpLocationType { get; set; }

        public bool IsBulkPlantLift { get; set; }

        //variables used in PartialMatch Resolve from angular scratch report
        public string  DisplayLiftDate { get; set; }
        public DropdownDisplayItem SelectedInvoiceFtlDetailId { get; set; }
        public List<DropdownDisplayItem> TerminalList { get; set; }
        public DropdownDisplayItem SelectedTerminal { get; set; }
        public List<DropdownDisplayItem> FuelTypeList { get; set; }
        public DropdownDisplayItem SelectedFuelType { get; set; }

    }
}
