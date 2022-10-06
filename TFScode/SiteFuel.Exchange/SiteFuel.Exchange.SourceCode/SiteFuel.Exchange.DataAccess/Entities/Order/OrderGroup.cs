namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class OrderGroup
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OrderGroup()
        {
            OrderGroupXOrders = new HashSet<OrderGroupXOrder>();
            TermOrderGroupHistories = new HashSet<TermOrderGroupHistory>();
        }

        public int Id { get; set; }

        public int BuyerCompanyId { get; set; }

        public int JobId { get; set; }

        public int SupplierCompanyId { get; set; }

        public ProductCategory ProductType { get; set; }

        public DateTimeOffset? StartDate { get; set; }

        public int RenewalFrequency { get; set; }

        public int RenewalCount { get; set; }

        public int RenewedCount { get; set; }

        public OrderGroupType GroupType { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        public int? ParentGroupId { get; set; }

        [ForeignKey("BuyerCompanyId")]
        public virtual Company BuyerCompany { get; set; }

        [ForeignKey("SupplierCompanyId")]
        public virtual Company SupplierCompany { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User CreatedByUser { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderGroupXOrder> OrderGroupXOrders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TermOrderGroupHistory> TermOrderGroupHistories { get; set; }
    }
}
