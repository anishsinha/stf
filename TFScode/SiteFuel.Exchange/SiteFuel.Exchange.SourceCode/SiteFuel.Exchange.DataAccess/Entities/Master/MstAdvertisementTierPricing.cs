namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MstAdvertisementTierPricing
    {
        public MstAdvertisementTierPricing()
        {
            Currency = Currency.USD;
            UoM = UoM.Gallons;
        }
        public int Id { get; set; }

        public int MinQuantity { get; set; }

        public Nullable<int> MaxQuantity { get; set; }

        public decimal Amount { get; set; }

        public Currency Currency { get; set; }

        public UoM UoM { get; set; }
    }
}
