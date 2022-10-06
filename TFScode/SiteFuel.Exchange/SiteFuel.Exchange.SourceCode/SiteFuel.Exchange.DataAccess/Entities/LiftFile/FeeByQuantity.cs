namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class FeeByQuantity
    {
        public FeeByQuantity()
        {
            Currency = Currency.USD;
            UoM = UoM.Gallons;
        }
        public int Id { get; set; }

        public int FeeTypeId { get; set; }

        public int FeeSubTypeId { get; set; }

        public decimal MinQuantity { get; set; }

        public Nullable<decimal> MaxQuantity { get; set; }

        public decimal Fee { get; set; }

        public Nullable<int> MarginTypeId { get; set; }

        public decimal Margin { get; set; }

        public int? FuelFeesId { get; set; }

        public Currency Currency { get; set; }

        public UoM UoM { get; set; }

        public virtual MstFeeSubType MstFeeSubType { get; set; }

        public virtual MstFeeType MstFeeType { get; set; }

        public virtual MstMarginType MstMarginType { get; set; }

        [ForeignKey("FuelFeesId")]
        public virtual FuelFee FuelFee { get; set; }
    }
}
