namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Resale
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Resale()
        {
            ResaleFees = new HashSet<ResaleFee>();
            FuelRequests = new HashSet<FuelRequest>();
            DifferentFuelPrices = new HashSet<DifferentFuelPrice>();
            Currency = Currency.USD;
            UoM = UoM.Gallons;
            ExchangeRate = 1;
        }

        public int Id { get; set; }

        public int PricingTypeId { get; set; }

        public Nullable<int> RackAvgTypeId { get; set; }

        public decimal PricePerGallon { get; set; }

        public bool IsDDTEnabled { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        public decimal BasePrice { get; set; }

        public Currency Currency { get; set; }

        public decimal ExchangeRate { get; set; }

        public UoM UoM { get; set; }

        public virtual MstPricingType MstPricingType { get; set; }

        public virtual MstRackAvgPricingType MstRackAvgPricingType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ResaleFee> ResaleFees { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FuelRequest> FuelRequests { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DifferentFuelPrice> DifferentFuelPrices { get; set; }
    }
}
