namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class FuelFee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FuelFee()
        {
            ResaleFees = new HashSet<ResaleFee>();
            FuelRequests = new HashSet<FuelRequest>();
            Invoices = new HashSet<Invoice>();
            AccessorialFees = new HashSet<AccessorialFee>();
            LeadRequests = new HashSet<LeadRequest>();
            Quotations = new HashSet<Quotation>();
            FeeByQuantities = new HashSet<FeeByQuantity>();
            Currency = Currency.USD;
            UoM = UoM.Gallons;
        }

        public int Id { get; set; }

        public int FeeTypeId { get; set; }

        public int FeeSubTypeId { get; set; }

        public decimal? MinimumGallons { get; set; }

        public decimal Fee { get; set; }

        [StringLength(256)]
        public string FeeDetails { get; set; }

        public Nullable<int> MarginTypeId { get; set; }

        public decimal Margin { get; set; }

        [DefaultValue(false)]
        public bool IncludeInPPG { get; set; }

        public int? InvoiceId { get; set; }

        public decimal? FeeSubQuantity { get; set; }

        public decimal? TotalFee { get; set; }

        public int? OtherFeeTypeId { get; set; }

        public int? FeeConstraintTypeId { get; set; }

        public int? TaxDetailId { get; set; }

        public Nullable<DateTimeOffset> SpecialDate { get; set; }

        public Currency Currency { get; set; }

        public UoM UoM { get; set; }

        public Nullable<int> OfferPricingId { get; set; }

        public Nullable<int> DiscountLineItemId { get; set; }

        public int? WaiveOffTime { get; set; }

        public Nullable<DateTimeOffset> StartTime { get; set; }

        public Nullable<DateTimeOffset> EndTime { get; set; }

        public virtual MstFeeSubType MstFeeSubType { get; set; }

        public virtual MstFeeType MstFeeType { get; set; }

        public virtual MstMarginType MstMarginType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ResaleFee> ResaleFees { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FuelRequest> FuelRequests { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Invoice> Invoices { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccessorialFee> AccessorialFees { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Quotation> Quotations { get; set; }

        [ForeignKey("InvoiceId")]
        public virtual Invoice Invoice { get; set; }

        [ForeignKey("OfferPricingId")]
        public virtual OfferPricing OfferPricing { get; set; }

        [ForeignKey("DiscountLineItemId")]
        public virtual DiscountLineItem DiscountLineItem { get; set; }

        [ForeignKey("OtherFeeTypeId")]
        public virtual MstOtherFeeType MstOtherFeeType { get; set; }

        [ForeignKey("FeeConstraintTypeId")]
        public virtual MstFeeConstraintType MstFeeConstraintType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FeeByQuantity> FeeByQuantities { get; set; }

        [ForeignKey("TaxDetailId")]
        public virtual TaxDetail TaxDetails { get; set; }
        public virtual ICollection<LeadRequest> LeadRequests { get; set; }
    }
}
