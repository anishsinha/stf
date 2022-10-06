namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ExternalSupplierNote
    {
        public int Id { get; set; }

        public int ExternalSupplierId { get; set; }

        public string Note { get; set; }

        public bool IsCompleted { get; set; }

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        [ForeignKey("ExternalSupplierId")]
        public virtual ExternalSupplier ExternalSupplier { get; set; }
    }
}
