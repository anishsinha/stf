namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Quotation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Quotation()
        {
            QuotationFees = new HashSet<FuelFee>();
            QuotationStatuses = new HashSet<QuotationStatus>();
            QuoteRequestDocuments = new HashSet<QuoteRequestDocument>();
            Currency = Currency.USD;
            UoM = UoM.Gallons;
            ExchangeRate = 1;
        }

        public int Id { get; set; }

        public int QuoteRequestId { get; set; }

        public Nullable<int> FuelRequestId { get; set; }

        [StringLength(32)]
        public string QuoteNumber { get; set; }

        public int PricingTypeId { get; set; }

        public Nullable<int> RackAvgTypeId { get; set; }

        public decimal PricePerGallon { get; set; }

        public decimal? SupplierCost { get; set; }

        public int? SupplierCostTypeId { get; set; }

        public string Notes { get; set; }

        public bool IsTnCIncluded { get; set; }

        public int? ImageId { get; set; }      

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        public int Priority { get; set; }

        public decimal BasePrice{ get; set; }

        public decimal? BaseSupplierCost{ get; set; }

        public Currency Currency { get; set; }

        public decimal ExchangeRate{ get; set; }

        public UoM UoM { get; set; }

        public virtual User User { get; set; }

        [ForeignKey("FuelRequestId")]
        public virtual FuelRequest FuelRequest { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FuelFee> QuotationFees { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuotationStatus> QuotationStatuses { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuoteRequestDocument> QuoteRequestDocuments { get; set; }

        [ForeignKey("QuoteRequestId")]
        public virtual QuoteRequest QuoteRequest { get; set; }

        [ForeignKey("ImageId")]
        public virtual Image Image { get; set; }
    }
}
