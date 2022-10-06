using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class UspTPOViewModel
    {
        public int UserId { get; set; }
        public int BuyerCompanyId { get; set; }
        public string BuyerCompanyName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool IsFTL { get; set; }
        public int FreightOnBoardTypeId { get; set; }
        public int JobId { get; set; }
        public int? OnsiteContactUserId { get; set; }
        public string OnsiteFirstName { get; set; }
        public string OnsiteLastName { get; set; }
        public bool IsTaxExempted { get; set; }
        public int QuantityTypeId { get; set; }
        public decimal MinQuantity { get; set; }
        public decimal MaxQuantity { get; set; }
        public QuantityIndicatorTypes PricingQuantityIndicatorTypeId { get; set; }
        public int DeliveryTypeId { get; set; }
        public string WBSNumber { get; set; }
        public int ProductDisplayGroupId { get; set; }
        public int PaymentTermId { get; set; }
        public int NetDays { get; set; }
        public PaymentMethods PaymentMethod { get; set; }
        public InvoiceNotificationPreferenceTypes BOLInvoicePreferenceId { get; set; }
        public int? SupplierSourceId { get; set; }
        public string SupplierSourceName { get; set; }
        public string SupplierContract { get; set; }
        public int? CarrierId { get; set; }
        public string Carrier { get; set; }
        public string LoadCode { get; set; }
        public string Notes { get; set; }
        public string DRNotes { get; set; }
        public int? OrderClosingThreshold { get; set; }
        public bool IsAssetTracked { get; set; }
        public bool IsAssetDropStatusEnabled { get; set; }
        public bool IsDriverToUpdateBOL { get; set; }
        public bool IsBolImageRequired { get; set; }
        public bool IsDropImageRequired { get; set; }
        public bool SignatureEnabled { get; set; }
        public int FuelRequestTypeId { get; set; }

        public string BillToAddress { get; set; }
        public string BillToAddressLine1 { get; set; }
        public string BillToAddressLine2 { get; set; }
        public string BillToCity { get; set; }
        public int? BillToCountryId { get; set; }
        public string BillToName { get; set; }
        public int? BillToStateId { get; set; }
        public string BillToZipCode { get; set; }
        public System.Nullable<LocationInventoryManagedBy> LocationInventoryManagedBy { get; set; }

        public bool IsMarineLocation { get; set; }
        public int UoM { get; set; }
        public string Berth { get; set; }
        public DestinedForInternationalWaters InternationalWatersType { get; set; }
        public bool IsBDNConfirmationRequired { get; set; }
        public bool IsInvoiceConfirmationRequired { get; set; }
        public int? VessleId { get; set; }
        public string IMONumber { get; set; }
        public string Flag { get; set; }

        public bool IsSuppressPricingEnabled { get; set; }

        public bool IsPDITaxRequired { get; set; }
        public FreightPricingMethod FreightPricingMethod { get; set; }
    }
}
