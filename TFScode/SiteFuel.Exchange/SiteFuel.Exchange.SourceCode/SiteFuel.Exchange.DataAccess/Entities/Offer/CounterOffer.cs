namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CounterOffer
    {
        public int Id { get; set; }

        public int FuelRequestId { get; set; }

        public int BuyerId { get; set; }

        public Nullable<int> BuyerStatus { get; set; }

        public int SupplierId { get; set; }

        public Nullable<int> SupplierStatus { get; set; }

        public int OriginalFuelRequestId { get; set; }

        public virtual FuelRequest FuelRequest { get; set; }

        public virtual MstCounterOfferStatus MstCounterOfferStatus { get; set; }

        public virtual MstCounterOfferStatus MstCounterOfferStatus1 { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }
    }
}
