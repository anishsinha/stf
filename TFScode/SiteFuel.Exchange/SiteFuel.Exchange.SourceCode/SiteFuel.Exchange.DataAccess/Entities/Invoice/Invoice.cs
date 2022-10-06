namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Invoice
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Invoice()
        {
            AssetDrops = new HashSet<AssetDrop>();
            Invoices1 = new HashSet<Invoice>();
            InvoiceXDeclineReasons = new HashSet<InvoiceXDeclineReason>();
            InvoiceXInvoiceStatusDetails = new HashSet<InvoiceXInvoiceStatusDetail>();
            InvoiceXSpecialInstructions = new HashSet<InvoiceXSpecialInstruction>();
            Spills = new HashSet<Spill>();
            FuelRequestFees = new HashSet<FuelFee>();
            PaymentDiscounts = new HashSet<PaymentDiscount>();
            TaxDetails = new HashSet<TaxDetail>();
            Discounts = new HashSet<Discount>();
            Currency = Currency.USD;
            UoM = UoM.Gallons;
            ExchangeRate = 1;
            BillingStatementXInvoices = new HashSet<BillingStatementXInvoice>();
            InvoiceExceptions = new HashSet<InvoiceException>();
            InvoiceXBolDetails = new HashSet<InvoiceXBolDetail>();
        }

        public int Id { get; set; }

        public Nullable<int> OrderId { get; set; }

        public int InvoiceHeaderId { get; set; }

        public int Version { get; set; }

        public int InvoiceVersionStatusId { get; set; }

        public int InvoiceTypeId { get; set; }

        public decimal DroppedGallons { get; set; }

        public DateTimeOffset DropStartDate { get; set; }

        public DateTimeOffset DropEndDate { get; set; }

        [StringLength(275)]
        public string PoNumber { get; set; }

        public decimal StateTax { get; set; }

        public decimal FedTax { get; set; }

        public decimal SalesTax { get; set; }

        public decimal BasicAmount { get; set; }

        public bool IsWetHosingDelivery { get; set; }

        public bool IsOverWaterDelivery { get; set; }

        public int PaymentTermId { get; set; }

        public int DDTConversionReason { get; set; }

        public int NetDays { get; set; }

        public DateTimeOffset PaymentDueDate { get; set; }

        public Nullable<DateTimeOffset> PaymentDate { get; set; }

        public Nullable<int> ParentId { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        public decimal TotalTaxAmount { get; set; }

        [Required]
        [StringLength(512)]
        public string TransactionId { get; set; }

        public Nullable<int> DriverId { get; set; }

        [StringLength(256)]
        public string TraceId { get; set; }

        public int? ImageId { get; set; }

        public int? SignatureId { get; set; }

        [StringLength(256)]
        public string FilePath { get; set; }

        [DefaultValue(0)]
        public int WaitingFor { get; set; }

        public int? SupplierPreferredInvoiceTypeId { get; set; }

        public bool IsBuyPriceInvoice { get; set; }

        public decimal? TotalFeeAmount { get; set; }

        [StringLength(256)]
        public string BrokeredChainId { get; set; }
        public decimal BaseDroppedQuntity { get; set; }
        public decimal BasePrice { get; set; }
        public decimal BaseStateTax { get; set; }
        public decimal BaseFedTax { get; set; }
        public decimal BaseSalesTax { get; set; }
        public decimal BaseBasicAmount { get; set; }
        public decimal BaseTotalTaxAmount { get; set; }
        public decimal BaseRackPrice { get; set; }
        public decimal? BaseTotalFeeAmount { get; set; }
        public Currency Currency { get; set; }
        public decimal ExchangeRate { get; set; }
        public UoM UoM { get; set; }

        [StringLength(500)]
        public string DisplayInvoiceNumber { get; set; }

        [StringLength(500)]
        public string ReferenceId { get; set; }

        [StringLength(32)]
        public string QbInvoiceNumber { get; set; }

        public decimal TotalDiscountAmount { get; set; }

        [StringLength(512)]
        public string PDIInvoiceNumber { get; set; }

        public decimal? GrossGallons { get; set; }

        public decimal? NetGallons { get; set; }

        public int? TrackableScheduleId { get; set; }

        public bool IsSignatureReq { get; set; }
        public bool IsBolImageReq { get; set; }
        public bool IsDropImageReq { get; set; }

        public int? QuantityIndicatorTypeId { get; set; }

        public string GroupParentDrId { get; set; }

        public decimal? Gravity { get; set; }

        public decimal? ConvertedQuantity { get; set; }

        public decimal? ConvertedPricing { get; set; }

        public decimal? ConversionFactor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AssetDrop> AssetDrops { get; set; }

        [ForeignKey("InvoiceHeaderId")]
        public virtual InvoiceHeaderDetail InvoiceHeader { get; set; }

        public virtual User Driver { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Invoice> Invoices1 { get; set; }

        public virtual Invoice Invoice1 { get; set; }

        public virtual MstInvoiceType MstInvoiceType { get; set; }

        public virtual MstInvoiceVersionStatus MstInvoiceVersionStatus { get; set; }

        public virtual MstPaymentTerm MstPaymentTerm { get; set; }

        public virtual Order Order { get; set; }

        public virtual User User { get; set; }

        public virtual InvoiceXAdditionalDetail InvoiceXAdditionalDetail { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceXDeclineReason> InvoiceXDeclineReasons { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceXInvoiceStatusDetail> InvoiceXInvoiceStatusDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceXSpecialInstruction> InvoiceXSpecialInstructions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceXAccessorialFee> InvoiceXAccessorialFees { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Spill> Spills { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FuelFee> FuelRequestFees { get; set; }

        [ForeignKey("ImageId")]
        public virtual Image Image { get; set; }

        [ForeignKey("SignatureId")]
        public virtual Signature Signaure { get; set; }

        [ForeignKey("TrackableScheduleId")]
        public virtual DeliveryScheduleXTrackableSchedule TrackableSchedule { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceDispatchLocation> InvoiceDispatchLocation { get; set; } = new HashSet<InvoiceDispatchLocation>();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PaymentDiscount> PaymentDiscounts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaxDetail> TaxDetails { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Discount> Discounts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BillingStatementXInvoice> BillingStatementXInvoices { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceException> InvoiceExceptions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceXBolDetail> InvoiceXBolDetails { get; set; }

        public virtual BDRDetail BDRDetail { get; set; }
    }
}
