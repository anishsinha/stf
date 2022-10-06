namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class OfferPricing
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OfferPricing()
        {
            FuelRequests = new HashSet<FuelRequest>();
            OfferBuyerStatuses = new HashSet<OfferBuyerStatus>();
            FuelFees = new HashSet<FuelFee>();
            DifferentFuelPrices = new HashSet<DifferentFuelPrice>();
            OfferPricingItems = new HashSet<OfferPricingItem>();
            Currency = Currency.USD;
            UoM = UoM.Gallons;
        }

        public int Id { get; set; }

        public Guid OfferChainId { get; set; }

        [Required, StringLength(256)]
        public string Name { get; set; }

        public int OfferTypeId { get; set; }

        public DateTimeOffset? ExpirationDate { get; set; }

        public int SupplierCompanyId { get; set; }

        public int StatusId { get; set; }

        public int FuelTypeId { get; set; } //tfxFuelTypeId

        public int PricingTypeId { get; set; }

        public Nullable<int> RackAvgTypeId { get; set; }

        public decimal PricePerGallon { get; set; }

        public Nullable<decimal> SupplierCost { get; set; }

        public Nullable<int> SupplierCostTypeId { get; set; }

        public Nullable<int> TerminalId { get; set; }

        public Nullable<int> CityGroupTerminalId { get; set; }

        public decimal BasePrice { get; set; }

        public decimal? BaseSupplierCost { get; set; }

        public Currency Currency { get; set; }

        public decimal ExchangeRate { get; set; }

        public UoM UoM { get; set; }

        public decimal CreationTimeRackPPG { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsOfferUpdated { get; set; }

        public int CountryId { get; set; }

        public PricingSource PricingSource { get; set; }

        public TruckLoadTypes TruckLoadType { get; set; }

        [ForeignKey("OfferTypeId")]
        public virtual MstOfferType MstOfferType { get; set; }

        [ForeignKey("StatusId")]
        public virtual MstOfferStatus MstOfferStatus { get; set; }

        [ForeignKey("SupplierCompanyId")]
        public virtual Company SupplierCompany { get; set; }

        [ForeignKey("FuelTypeId")]
        public virtual MstTfxProduct MstTfxProduct { get; set; }

        [ForeignKey("FuelTypeId")]
        public virtual MstProduct MstProduct { get; set; }

        [ForeignKey("PricingTypeId")]
        public virtual MstPricingType MstPricingType { get; set; }

        [ForeignKey("RackAvgTypeId")]
        public virtual MstRackAvgPricingType MstRackAvgPricingType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FuelRequest> FuelRequests { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FuelFee> FuelFees { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OfferBuyerStatus> OfferBuyerStatuses { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [ForeignKey("SupplierCostTypeId")]
        public virtual MstSupplierCostTypes MstSupplierCostTypes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DifferentFuelPrice> DifferentFuelPrices { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OfferPricingItem> OfferPricingItems { get; set; }

    }
}
