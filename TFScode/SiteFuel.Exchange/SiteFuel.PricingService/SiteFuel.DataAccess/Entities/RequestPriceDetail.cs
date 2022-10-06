using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteFuel.DataAccess.Entities
{
    public class RequestPriceDetail
    {
        public RequestPriceDetail()
        {
            PricingDetails = new HashSet<PricingDetail>();
            CumulationDetails = new HashSet<CumulationDetail>();
        }

        public int Id { get; set; }

        //public int PricingCodeId { get; set; }

        //public Nullable<int> RackAvgTypeId { get; set; }

        //public decimal PricePerGallon { get; set; }

        //public decimal? SupplierCost { get; set; }

        //public int? SupplierCostTypeId { get; set; }

        //public Nullable<int> MarginTypeId { get; set; }

        //public decimal Margin { get; set; }

        //public decimal BasePrice { get; set; }

        //public decimal? BaseSupplierCost { get; set; }

        public int Currency { get; set; }

        public decimal ExchangeRate { get; set; }

        public int UoM { get; set; }

        public int? TierTypeId { get; set; }

        public int? PricingTypeId { get; set; }

        public int? CumulationTypeId { get; set; }
        public int? CumulationResetDay { get; set; }
        public DateTimeOffset? CumulationResetDate { get; set; }
        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PricingDetail> PricingDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CumulationDetail> CumulationDetails { get; set; }
    }
}
