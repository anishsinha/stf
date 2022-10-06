using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteFuel.DataAccess.Entities
{
    public class ExternalPricingPlatts
    {
        public int Id { get; set; }
        public int TerminalId { get; set; }
        [StringLength(16)]
        public string TerminalAbbreviation { get; set; }
        public int ProductId { get; set; }
        [StringLength(32)]
        public string Symbol { get; set; }
        public DateTimeOffset LoadDate { get; set; }
        public DateTimeOffset ReportedDate { get; set; }
        public decimal Price { get; set; }
        public int Unit { get; set; }
        public int Currency { get; set; }
        public int SupplierNumber { get; set; }
        [StringLength(128)]
        public string Supplier { get; set; }
        [StringLength(256)]
        public string LiftPoint { get; set; }
        public int ExternalProductId { get; set; }
        public int SourceId { get; set; }
        [ForeignKey("TerminalId")]
        public virtual MstExternalTerminal MstExternalTerminal { get; set; }
        [ForeignKey("ProductId")]
        public virtual MstProduct MstProduct { get; set; }
    }
}
