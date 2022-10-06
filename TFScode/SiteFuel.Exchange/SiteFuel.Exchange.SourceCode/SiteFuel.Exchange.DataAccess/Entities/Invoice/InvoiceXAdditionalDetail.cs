namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;   
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class InvoiceXAdditionalDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int InvoiceId { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public Nullable<int> AssignedBy { get; set; }

        public Nullable<System.DateTimeOffset> AssignedDate { get; set; }

        public string DriverComment { get; set; }

        [DefaultValue(0)]
        public int AssetFilled { get; set; }

        [StringLength(256)]
        public string PoContactName { get; set; }

        [StringLength(256)]
        public string PoContactEmail { get; set; }

        [StringLength(16)]
        public string PoContactPhoneNumber { get; set; }

        public Nullable<int> PoContactId { get; set; }

        public Nullable<int> JobId { get; set; }

        [StringLength(256)]
        public string DisplayJobID { get; set; }

        [StringLength(256)]
        public string CustomAttribute { get; set; }

        [StringLength(256)]
        public string JobName { get; set; }

        [StringLength(512)]
        public string JobAddress { get; set; }

        [StringLength(512)]
        public string JobAddressLine2 { get; set; }

        [StringLength(512)]
        public string JobAddressLine3 { get; set; }

        [StringLength(128)]
        public string JobCity { get; set; }

        [StringLength(3)]
        public string JobStateCode { get; set; }

        [StringLength(256)]
        public string JobStateName { get; set; }

        [StringLength(3)]
        public string JobCountryCode { get; set; }

        [StringLength(256)]
        public string JobCountryName { get; set; }

        [StringLength(32)]
        public string JobZipCode { get; set; }

        public Nullable<int> BillingAddessId { get; set; }

        [StringLength(512)]
        public string BillingAddress { get; set; }

        [StringLength(512)]
        public string BillingAddressLine2 { get; set; }

        [StringLength(512)]
        public string BillingAddressLine3 { get; set; }

        [StringLength(256)]
        public string BillingCity { get; set; }

        [StringLength(256)]
        public string BillingStateCode { get; set; }

        [StringLength(256)]
        public string BillingStateName { get; set; }

        [StringLength(256)]
        public string BillingCountryCode { get; set; }

        [StringLength(256)]
        public string BillingCountryName { get; set; }

        [StringLength(32)]
        public string BillingZipCode { get; set; }

        public DateTimeOffset? CxmlCheckOutDate { get; set; }

        [StringLength(256)]
        public string SplitLoadChainId { get; set; }

        public Nullable<int> SplitLoadSequence { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        public PaymentMethods PaymentMethod { get; set; }

        public int? TankFrequencyId { get; set; }

        public bool IsSurchargeApplicable { get; set; }
        public bool IsFreightCostApplicable { get; set; }
        public FuelSurchagePricingType SurchargePricingType { get; set; }
        public decimal? SurchargePercentage { get; set; }
        public decimal? SurchargeEIAPrice { get; set; }
        public decimal? SurchargeTableRangeStart { get; set; }
        public decimal? SurchargeTableRangeEnd { get; set; }
        public decimal? Distance { get; set; }
        public int? OriginalInvoiceId { get; set; }
        public int? OriginalInvoiceHeaderId { get; set; }

        [StringLength(64)]
        public string TruckNumber { get; set; }

        [StringLength(64)]
        public string DropTicketNumber { get; set; }

        public CreationMethod CreationMethod { get; set; }

        public int? AdditionalImageId { get; set; }

        public decimal? SupplierAllowance { get; set; }

        public decimal? TotalAllowance { get; set; }

        public int? OrderGroupId { get; set; }
        public OrderGroupType OrderGroupType { get; set; }

        public bool IsJobSpecificBillToEnabled { get; set; }

        [StringLength(256)]
        public string BillToName { get; set; }

        [StringLength(512)]
        public string BillToAddress { get; set; }

        [StringLength(512)]
        public string BillToAddressLine2 { get; set; }

        [StringLength(512)]
        public string BillToAddressLine3 { get; set; }

        [StringLength(128)]
        public string BillToCity { get; set; }

        [StringLength(32)]
        public string BillToZipCode { get; set; }

        [StringLength(32)]
        public string BillToStateCode { get; set; }

        [StringLength(32)]
        public string BillToStateName { get; set; }

        [StringLength(32)]
        public string BillToCountryCode { get; set; }

        [StringLength(32)]
        public string BillToCountryName { get; set; }

        [StringLength(512)]
        public string AccouningCompanyId { get; set; }

        public bool IsSiteOutOfFuel { get; set; }
        [StringLength(128)]
        public string OutOfFuelProduct { get; set; }

        [StringLength(128)]
        public string Tracktor { get; set; }

        [StringLength(128)]
        public string LoadingBadge { get; set; }

        [StringLength(128)]
        public string CarrierOrderId { get; set; }

        public string CarrierOrder { get; set; }
        public DateTimeOffset? OrderDate { get; set; }
        public decimal OrderQuantity { get; set; }

        [StringLength(256)]
        public string ExternalRefID { get; set; }

        public int? NoDataExceptionApprovalId { get; set; }

        public Nullable<DateTimeOffset> DeliverySentToPDIOn { get; set; }

        [StringLength(16)]
        public string PDIDeliveryDetailsStatus { get; set; }

        [StringLength(512)]
        public string PDIDeliveryOrderNo { get; set; }

        [ForeignKey("AdditionalImageId")]
        public virtual Image AdditionalImage { get; set; }

        public virtual Invoice Invoice { get; set; }

        [ForeignKey("OriginalInvoiceId")]
        public virtual Invoice OriginalInvoice { get; set; }

        [ForeignKey("OriginalInvoiceHeaderId")]
        public virtual InvoiceHeaderDetail OriginalInvoiceHeader { get; set; }

        [ForeignKey("TankFrequencyId")]
        public virtual FuelRequestTankRentalFrequency TankRentalFrequency { get; set; }

        public string ExceptionMessage { get; set; }

        public int? TaxAffidavitImageId { get; set; }
        public int? BDNImageId { get; set; }

        public int? CoastGuardInspectionImageId { get; set; }

        public int? InspectionRequestVoucherImageId { get; set; }

        public bool IsMarine { get; set; }

        public FreightPricingMethod FreightPricingMethod { get; set; } = FreightPricingMethod.Manual;

        public FreightRateRuleType? FreightRateRuleType { get; set; }

        public int? FreightRateRuleId { get; set; }

        public TableTypes? FreightRateTableType { get; set; }

        public int? FuelSurchargeTableId { get; set; }

        public TableTypes? FuelSurchargeTableType { get; set; }

        public bool IsShowProductDescriptionOnInvoice { get; set; }

        [ForeignKey("TaxAffidavitImageId")]
        public virtual Image TaxAffidavitImage { get; set; }

        [ForeignKey("BDNImageId")]
        public virtual Image BDNImage { get; set; }

        [ForeignKey("CoastGuardInspectionImageId")]
        public virtual Image CoastGuardInspectionImage { get; set; }

        [ForeignKey("InspectionRequestVoucherImageId")]
        public virtual Image InspectionRequestVoucherImage { get; set; }
    }
}
