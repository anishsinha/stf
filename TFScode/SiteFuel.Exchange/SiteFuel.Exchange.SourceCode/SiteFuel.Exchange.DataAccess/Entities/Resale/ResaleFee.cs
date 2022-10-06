namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ResaleFee
    {
        public ResaleFee()
        {
            Currency = Currency.USD;
        }
        public int Id { get; set; }

        public int ResaleId { get; set; }

        public int FuelRequestFeeId { get; set; }

        public decimal PricePerGallon { get; set; }

        public Currency Currency { get; set; }

        public virtual FuelFee FuelRequestFee { get; set; }

        public virtual Resale Resale { get; set; }
    }
}
