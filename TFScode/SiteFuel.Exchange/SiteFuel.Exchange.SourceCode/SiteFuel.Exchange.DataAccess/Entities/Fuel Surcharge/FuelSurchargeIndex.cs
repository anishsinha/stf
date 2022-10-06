namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class FuelSurchargeIndex
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FuelSurchargeIndex()
        {
            FuelSurchargeGeneratedTables = new HashSet<FuelSurchargeGeneratedTable>();
            FreightTableCompanies = new HashSet<FreightTableCompany>();
            FreightTablePickupLocations = new HashSet<FreightTablePickupLocation>();
            FreightTableSourceRegions = new HashSet<FreightTableSourceRegion>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public TableTypes TableType { get; set; }
        public int? SupplierCompanyId { get; set; }
        public SurchargeProductTypes ProductType { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public int UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public int? ProductId { get; set; }
        public int? PeriodId { get; set; }
        public int? AreaId { get; set; }
        public FreightTableStatus StatusId { get; set; }
        public IndexType IndexType { get; set; }
        public DateTimeOffset IndexPriceDate { get; set; }
        public DateTimeOffset? ApiAdjustIndexPriceDate { get; set; }
        public string ApiEffectiveDate { get; set; }
        public DateTimeOffset? ManualEffectiveDate { get; set; }
        public decimal IndexPrice { get; set; }
        public string Notes { get; set; }
        public decimal? PriceStartValue { get; set; }
        public decimal? PriceEndValue { get; set; }
        public decimal? PriceInterval { get; set; }
        public decimal? SurchargeStartPercent { get; set; }
        public decimal? SurchargeInterval { get; set; }

        [ForeignKey("SupplierCompanyId")]
        public virtual Company SupplierCompany { get; set; }

        [ForeignKey("ProductId")]
        public virtual MstLookupType FuelSurchargeProduct { get; set; }

        [ForeignKey("PeriodId")]
        public virtual MstLookupType FuelSurchargePeriod { get; set; }

        [ForeignKey("AreaId")]
        public virtual MstLookupType FuelSurchargeArea { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FuelSurchargeGeneratedTable> FuelSurchargeGeneratedTables { get; set; }

        public virtual ICollection<FreightTableCompany> FreightTableCompanies { get; set; }

        public virtual ICollection<FreightTablePickupLocation> FreightTablePickupLocations { get; set; }

        public virtual ICollection<FreightTableSourceRegion> FreightTableSourceRegions { get; set; }
    }
}