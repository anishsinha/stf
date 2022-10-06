namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class CurrentCost
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CurrentCost()
        {
            Currency = Currency.USD;
        }

        public int Id { get; set; }
        public decimal Cost { get; set; }
        public int PricingTypeId { get; set; }
        public int FuelTypeId { get; set; } //tfxfueltypeId
        public int SupplierCompanyId { get; set; }
        public bool IsGlobleCost { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Currency Currency { get; set; }
        public int CountryId { get; set; }
        public UoM UoM { get; set; }
        [ForeignKey("FuelTypeId")]
        public virtual MstTfxProduct MstTfxProduct { get; set; }

        [ForeignKey("FuelTypeId")]
        public virtual MstProduct MstProduct { get; set; }

        [ForeignKey("PricingTypeId")]
        public virtual MstPricingType MstPricingType { get; set; }

        [ForeignKey("SupplierCompanyId")]
        public virtual Company SupplierCompany { get; set; }
    }
}
