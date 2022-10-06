namespace SiteFuel.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class MstClearDyedProductMapping
    {
        public int Id { get; set; }

        public int ClearProductId { get; set; }

        public int DyedProductId { get; set; }

        public int ClearExternalProductId { get; set; }

        public int DyedExternalProductId { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        [ForeignKey("ClearProductId")]
        public virtual MstProduct ClearProduct { get; set; }

        [ForeignKey("DyedProductId")]
        public virtual MstProduct DyedProduct { get; set; }

        [ForeignKey("ClearExternalProductId")]
        public virtual MstExternalProduct DyedExternalProduct { get; set; }

        [ForeignKey("DyedExternalProductId")]
        public virtual MstExternalProduct ClearExternalProduct { get; set; }
    }
}
