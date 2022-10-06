using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Models
{
    public class SourceBasedPriceRequestModel
    {
        public int? TerminalId { get; set; }
        public string ProductId { get; set; }
        //public int? FeedTypeId { get; set; }
        //public int? BrandTypeId { get; set; }
        public int? PriceTypeId { get; set; }
        public int PricingSourceId { get; set; }
        public string CountryCode { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public int PricingCodeId { get; set; }
    }
}
