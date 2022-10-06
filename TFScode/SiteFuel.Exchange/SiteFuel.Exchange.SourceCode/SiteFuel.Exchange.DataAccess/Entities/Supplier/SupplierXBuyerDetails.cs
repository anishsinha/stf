namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SupplierXBuyerDetails
    {
        [Key]
        public int Id { get; set; }

        public int SupplierCompanyId { get; set; }

        public int BuyerCompanyId { get; set; }

        [StringLength(256)]
        public string AccountingCompanyId { get; set; }

        public OrderCreationMethod OrderCreationMethod { get; set; }

        public DateTimeOffset LastModifiedDate { get; set; }

        public  int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("SupplierCompanyId")]
        public virtual Company SupplierCompany { get; set; }

        [ForeignKey("BuyerCompanyId")]
        public virtual Company BuyerCompany { get; set; }
        public int JobId { get; set; }
        public bool IsBadgeMandatory { get; set; }
        public bool CompanyOwnedLocation { get; set; }
    }
}
