namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ExternalSupplier
    {
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        public int CompanyTypeId { get; set; }

        [StringLength(256)]
        public string Website { get; set; }

        public bool InPipedrive { get; set; }

        [StringLength(256)]
        public string ContactPersonName { get; set; }

        [StringLength(256)]
        public string ContactPersonEmail { get; set; }

        [StringLength(16)]
        public string ContactPersonPhoneNumber { get; set; }

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }
		
        public DateTimeOffset CreatedDate { get; set; }
		
        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        public virtual ICollection<ExternalSupplierStatus> ExternalSupplierStatuses { get; set; }

        public virtual ICollection<ExternalSupplierNote> ExternalSupplierNotes { get; set; }

        public virtual ICollection<ExternalSupplierAddress> ExternalSupplierAddresses { get; set; }

        public virtual MstExternalSupplierType MstExternalSupplierType { get; set; }
    }
}
