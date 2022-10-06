namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class OfferUpdateCommand
    {
        public OfferUpdateCommand()
        {
            OfferPricingItems = new HashSet<OfferPricingItem>();
        }
        public int Id { get; set; }

        [StringLength(11), DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Name { get; set; }

        public int UpdateType { get; set; }

        public int MathOperationId { get; set; }

        public decimal UpdatedAmount { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        public string CommandJson { get; set; }

        public DateTimeOffset? UndoExecutedOn { get; set; }

        public int? UndoExecutedBy { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedByCompanyId { get; set; }

        public int OfferTypeId { get; set; }

        public string Tiers { get; set; }

        public string Customers { get; set; }

        public int FuelTypeId { get; set; }

        public string States { get; set; }

        public string Cities { get; set; }

        public string ZipCodes { get; set; }

        public int? PriceTypeId { get; set; }

        [StringLength(256)]
        public string FeeTypeName { get; set; }

        public int? FeeSubTypeId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OfferPricingItem> OfferPricingItems { get; set; }
    }
}
