namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CompanyFavoriteFuel
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }

        public int FuelTypeId { get; set; }

        public int TfxFuelTypeId { get; set; }

        public int AddedBy { get; set; }

        public DateTimeOffset AddedDate { get; set; }

        public Nullable<int> RemovedBy { get; set; }

        public Nullable<System.DateTimeOffset> RemovedDate { get; set; }

        public virtual Company Company { get; set; }
		
        public virtual MstProduct MstProduct { get; set; }

        [ForeignKey("TfxFuelTypeId")]
        public virtual MstTfxProduct MstTfxProduct { get; set; }

        public virtual User User { get; set; }
		
        public virtual User User1 { get; set; }
    }
}
