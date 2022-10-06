using FileHelpers;
using System;

namespace SiteFuel.Exchange.PricingSources.Processors
{
    /// <summary>
    /// Actual OPIS pricing history.
    /// </summary>
    public class OPISPricingHistory
    {
        public string ProductIndicator { get; set; }

        public string ProductType { get; set; }

        public DateTimeOffset? LoadDate { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string Supplier { get; set; }

        public string BrandIndicator { get; set; }

        public string Terms { get; set; }

        public decimal? GrossPrice { get; set; }

        public string UniqueMarker { get; set; }

        public int? OctaneLevel { get; set; }

        public string ActualProduct { get; set; }

        public decimal? RVP { get; set; }

        public string DieselBlend { get; set; }

        public string BioType { get; set; }
        public int FileId { get; internal set; }

        public DateTimeOffset? ReportedDate { get; set; }
        public string ProductDescription { get; set; }
        public string ProductCode { get; set; }
    }

    public class OPISProductMappingInfo
    {
        public string ProductGroup { get; set; }
        public string ProductType { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductCode { get; set; }
        public string DieselBlend { get; set; }
    }
}