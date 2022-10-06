using Foolproof;
using Newtonsoft.Json;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class OrderAdditionalDetailsViewModel : BaseViewModel
    {
        private string _drNotes { get; set; }
        [Display(Name = nameof(Resource.lblBillableQuantity), ResourceType = typeof(Resource))]
        public QuantityIndicatorTypes QuantityIndicatorTypes { get; set; }

        [Display(Name = nameof(Resource.lblInvoicePreferences), ResourceType = typeof(Resource))]
        public InvoiceNotificationPreferenceTypes BOLInvoicePreferenceTypes { get; set; } = InvoiceNotificationPreferenceTypes.SendInvoiceDDTWithoutBillingFile;

        [Display(Name = nameof(Resource.lblAllowance), ResourceType = typeof(Resource))]
        [Range(typeof(decimal), "0", ApplicationConstants.DecimalMaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valAllowanceShouldNotBeNegative))]
        public decimal? Allowance { get; set; }

        [Display(Name = nameof(Resource.lblDriverToUpdateBOL), ResourceType = typeof(Resource))]
        public bool IsDriverToUpdateBOL { get; set; } = false;

        [Display(Name = nameof(Resource.lblApplyFuelSurchage), ResourceType = typeof(Resource))]
        public bool IsFuelSurcharge { get; set; } = false;

        public bool IsFuelSurchargeAuto { get; set; } = false; // use for TPO saving perpouse only 

        [Display(Name = nameof(Resource.lblApplyFreightCost), ResourceType = typeof(Resource))]
        public bool IsFreightCost { get; set; } = false;

        [Display(Name = nameof(Resource.lblFuelSurchargePricingType), ResourceType = typeof(Resource))]
        //[RequiredIfTrue("IsFuelSurcharge", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public FuelSurchagePricingType? FuelSurchagePricingType { get; set; }
        public FuelSurchagePricingType? FuelSurchagePricingPeriod { get; set; }

        public FreightRateRuleType? FreightRateRuleType { get; set; }

        public TableTypes? FreightRateTableType { get; set; }

        [RequiredIfTrue("IsFreightCost",ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.lblRequired))]
        public int? FreightRateRuleId { get; set; }

        public TableTypes? FuelSurchargeTableType { get; set; }

        [RequiredIfTrue("IsFuelSurchargeAuto", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.lblRequired))]
        public int? FuelSurchargeTableId { get; set; }

        public TableTypes? AccessorialFeeTableType { get; set; }

        public int? AccessorialFeeId { get; set; }

        public List<DropdownDisplayExtendedItem> SurchargeList { get; set; } = new List<DropdownDisplayExtendedItem>();

        public CarrierViewModel Carrier { get; set; }

        public SupplierSourceViewModel SupplierSource { get; set; } = new SupplierSourceViewModel();

        [Display(Name = nameof(Resource.lblLoadCode), ResourceType = typeof(Resource))]
        public string LoadCode { get; set; }

        public string CustomAttribute { get; set; }

        public CustomAttributeViewModel CustomAttributeViewModel { get; set; }

        [MaxLength(500, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvoiceNotesGreaterThan))]
        public string Notes { get; set; }

        [MaxLength(1024, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageDRNotesGreaterThan))]
        public string DRNotes { get { return _drNotes != null ? _drNotes : string.Empty; } set { _drNotes = value; } }

        [Display(Name = nameof(Resource.lblAssignedProductName), ResourceType = typeof(Resource))]
        public string SupplierAssignedProductName { get; set; }

        [Display(Name = nameof(Resource.lblDriverProductId), ResourceType = typeof(Resource))]
        public string DriverProductId { get; set; }

        [Display(Name = nameof(Resource.lblLocationInventoryManagement), ResourceType = typeof(Resource))]
        public LocationManagedType LocationManagedType { get; set; } = LocationManagedType.NotSpecified;


        public bool IsIncludePricing { get; set; }

        public bool IsPdiTaxRequired { get; set; }

        [Display(Name = nameof(Resource.lblFreightPricingMethod), ResourceType = typeof(Resource))]
        public FreightPricingMethod FreightPricingMethod { get; set; } = FreightPricingMethod.Manual;

        [Display(Name = nameof(Resource.headingDestinedForInternationalWaters), ResourceType = typeof(Resource))]
        public DestinedForInternationalWaters DestinedForInternationalWaters { get; set; }

        [Display(Name = nameof(Resource.lblBerth), ResourceType = typeof(Resource))]
        public string Berth { get; set; }

        public bool IsManualBDNConfirmationRequired { get; set; }
        public bool IsManualInvoiceConfirmationRequired { get; set; }
    }

    // this model is currently being used for order & invoice
    public sealed class CustomAttributeViewModel
    {
        public string WBSNumber { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
