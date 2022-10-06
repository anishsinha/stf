namespace SiteFuel.DataAccess.Entities
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ExternalPricingData")]
    public partial class ExternalPricingData
    {
        public int Id { get; set; }

        public int TerminalId { get; set; }

        [Required]
        [StringLength(16)]
        public string TerminalAbbreviation { get; set; }

        public int ProductId { get; set; }

        [Required]
        [StringLength(32)]
        public string ProductCode { get; set; }

        public decimal AvgPrice { get; set; }

        [DefaultValue(0)]
        public decimal LowPrice { get; set; }

        [DefaultValue(0)]
        public decimal HighPrice { get; set; }

        public DateTimeOffset EffectiveDate { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        public int Currency { get; set; }

        public virtual MstExternalProduct MstExternalProduct { get; set; }

        public virtual MstExternalTerminal MstExternalTerminal { get; set; }
    }
}
