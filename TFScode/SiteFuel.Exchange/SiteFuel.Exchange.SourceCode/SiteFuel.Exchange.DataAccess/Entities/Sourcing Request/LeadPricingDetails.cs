using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public class LeadPricingDetail
    {
        [Key]
        public int Id { get; set; }
        public int LeadRequestId { get; set; }
        public int RequestPriceDetailId { get; set; }
        public int PricingTypeId { get; set; }
        public decimal PricePerGallon { get; set; } = 0;
        [StringLength(256)]
        public string Code { get; set; }
        public int CodeId { get; set; }
        public string CodeDescription { get; set; }
        public decimal? MinQuantity { get; set; }
        public decimal? MaxQuantity { get; set; }
        public Nullable<int> RackAvgTypeId { get; set; }
        public decimal RackPrice { get; set; } = 0;
        public bool EnableCityRack { get; set; }
        public string TerminalName { get; set; }
        public Nullable<int> TerminalId { get; set; }
        public Nullable<int> SupplierCostMarkupTypeId { get; set; }
        public decimal SupplierCostMarkupValue { get; set; } = 0;
        public int? CityGroupTerminalId { get; set; }
        [StringLength(256)]
        public string CityGroupTerminalName { get; set; }
        public int? CityGroupTerminalStateId { get; set; }
        public string PricingNotes { get; set; }
        [ForeignKey("RequestPriceDetailId")]
        public virtual LeadRequestPriceDetails LeadRequestPriceDetails { get; set; }
    }
}
