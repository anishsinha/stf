using Newtonsoft.Json;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class InvoiceXAdditionalDetailViewModel : JobSpecificBillingInfoViewModel
    {
        public int InvoiceId { get; set; }

        public string DriverComment { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public int? AssignedBy { get; set; }

        public int AssetFilled { get; set; }

        public DateTimeOffset? AssignedDate { get; set; }

        public int? PoContactId { get; set; }

        public string PoContactName { get; set; }

        public string PoContactEmail { get; set; }

        public string PoContactPhoneNumber { get; set; }

        public Nullable<int> JobId { get; set; }

        public string DisplayJobID { get; set; }

        public string JobName { get; set; }

        public string JobAddress { get; set; }

        public string JobAddressLine2 { get; set; }

        public string JobAddressLine3 { get; set; }
        
        public string JobCity { get; set; }

        public string JobStateCode { get; set; }

        public string JobStateName { get; set; }

        public string JobCountryCode { get; set; }

        public string JobCountryName { get; set; }

        public string JobZipCode { get; set; }

        public Nullable<int> BillingAddessId { get; set; }

        public string BillingAddress { get; set; }

        public string BillingAddressLine2 { get; set; }

        public string BillingAddressLine3 { get; set; }

        public string BillingCity { get; set; }

        public string BillingStateCode { get; set; }

        public string BillingStateName { get; set; }

        public string BillingCountryCode { get; set; }

        public string BillingCountryName { get; set; }

        public string BillingZipCode { get; set; }

        public DateTimeOffset? CxmlCheckOutDate { get; set; }

        public string CustomAttribute { get; set; }

        public InvoiceCustomAttributeViewModel CustomAttributeViewModel { get; set; }

        public string SplitLoadChainId { get; set; }

        public int? SplitLoadSequence { get; set; }

        public string Notes { get; set; }

        public PaymentMethods PaymentMethod { get; set; }

        public int? TankFrequencyId { get; set; }

        public string DropTicketNumber { get; set; }

        public string TruckNumber { get; set; }

        public CreationMethod CreationMethod { get; set; } = CreationMethod.SFX;

        public int? OriginalInvoiceId { get; set; }

        public int? OriginalInvoiceHeaderId { get; set; }

        public bool IsSurchargeApplicable { get; set; }

        public bool IsFreightCostApplicable { get; set; }

        public bool IsShowProductDescriptionOnInvoice { get; set; }
        
        public int SurchargePricingType { get; set; }

        public decimal? SurchargePercentage { get; set; }

        public decimal? SurchargeEIAPrice { get; set; }

        public decimal? SupplierAllowance { get; set; }

        public decimal? TotalAllowance { get; set; }

        public decimal? ActualDropQuantity { get; set; }

        //from TPD API
        public bool IsSiteOutOfFuel { get; set; }
        public string OutOfFuelProduct { get; set; }
        public string Tracktor { get; set; }
        public string LoadingBadge { get; set; }
        public string CarrierOrderId { get; set; }
        public string CarrierOrder { get; set; }
        public DateTimeOffset? OrderDate { get; set; }
        public decimal OrderQuantity { get; set; }
        public string ExternalRefId { get; set; }

        public int? NoDataExceptionApprovalId { get; set; }
        public int QuantityIndicatorTypeId { get; set; }

        public FreightPricingMethod FreightPricingMethod { get; set; } = FreightPricingMethod.Manual;
        public FreightRateRuleType? FreightRateRuleType { get; set; }
        public int? FreightRateRuleId { get; set; }
        public TableTypes? FreightRateTableType { get; set; }
        public int? FuelSurchargeTableId { get; set; }
        public TableTypes? FuelSurchargeTableType { get; set; }

        public string PDIOrderId { get; set; }
        public InvoiceXAdditionalDetailViewModel Clone()
        {
            return (InvoiceXAdditionalDetailViewModel)this.MemberwiseClone();
        }
    }

    public sealed class InvoiceCustomAttributeViewModel
    {
        [Display(Name = nameof(Resource.lblWBSNumber), ResourceType = typeof(Resource))]
        public string WBSNumber { get; set; }

        public bool IsTrueFillTax { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
