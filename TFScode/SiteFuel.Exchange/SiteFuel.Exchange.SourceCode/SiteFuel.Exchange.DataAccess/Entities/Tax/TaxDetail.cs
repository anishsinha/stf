namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TaxDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TaxDetail()
        {
            Invoices = new HashSet<Invoice>();
        }

        public int Id { get; set; }

        public decimal ProductCategory { get; set; }

        [Required, StringLength(256)]
        public string TaxingLevel { get; set; }

        [Required, StringLength(256)]
        public string TaxType { get; set; }

        [Required, StringLength(256)]
        public string RateType { get; set; }

        [Required, StringLength(32)]
        public string RateSubType { get; set; }

        [Required, StringLength(8)]
        public string CalculationTypeInd { get; set; }

        public decimal TaxRate { get; set; }

        public decimal TaxAmount { get; set; }

        [Required, StringLength(8)]
        public string TaxExemptionInd { get; set; }

        public decimal SalesTaxBaseAmount { get; set; }

        [Required, StringLength(256)]
        public string RateDescription { get; set; }

        [Required, StringLength(32)]
        public string Currency { get; set; }

        [Required, StringLength(32)]
        public string UnitOfMeasure { get; set; }

        public bool IsModified { get; set; }
		
        public Nullable<int> LicenseId { get; set; }

        [StringLength(128)]
        public string LicenseNumber { get; set; }

        public Nullable<int> TaxPricingTypeId { get; set; }

        public decimal TradingTaxAmount { get; set; }

        [Required, StringLength(32)]
        public string TradingCurrency { get; set; }

        public decimal ExchangeRate { get; set; }


        [StringLength(256)]
        public string RelatedLineItem { get; set; }

        public virtual MstTaxPricingType MstTaxPricingType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Invoice> Invoices { get; set; }
		
        public virtual TaxExemptLicens TaxExemptLicens { get; set; }
    }
}
