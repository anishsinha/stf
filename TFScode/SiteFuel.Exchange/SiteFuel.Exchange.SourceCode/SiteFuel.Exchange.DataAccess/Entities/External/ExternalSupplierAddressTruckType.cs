namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ExternalSupplierAddressTruckType
    {
        public int Id { get; set; }

        public int AddressId { get; set; }

        public int TruckTypeId { get; set; }

        [ForeignKey("AddressId")]
        public virtual ExternalSupplierAddress ExternalSupplierAddress { get; set; }
    }
}
