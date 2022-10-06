namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class FuelSurchargeTable
    {
        public int Id { get; set; }
        public TableTypes TableType { get; set; }
        public int? BuyerCompanyId { get; set; }
        public int? SupplierCompanyId { get; set; }
        public SurchargeProductTypes ProductType { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public decimal PriceRangeStartValue { get; set; }
        public decimal PriceRangeEndValue { get; set; }
        public decimal FuelSurchargePercentage { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public int UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }

        [ForeignKey("BuyerCompanyId")]
        public virtual Company CustomerCompany { get; set; }

        [ForeignKey("SupplierCompanyId")]
        public virtual Company SupplierCompany { get; set; }
    }
}