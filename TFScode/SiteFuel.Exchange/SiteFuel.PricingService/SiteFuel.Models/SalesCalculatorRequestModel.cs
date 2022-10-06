using SiteFuel.Exchange.Core;
using System;
using System.Collections.Generic;

namespace SiteFuel.Models
{
    public class SalesCalculatorRequestModel
    {
        public DateTime PriceDate { get; set; }

        public int? ProductId { get; set; }

        public List<int> CityGroupTerminalIds { get; set; } = new List<int>();
        public int? FeedTypeId { get; set; }
        public int? BrandTypeId { get; set; }
        public int? PriceTypeId { get; set; }

        public decimal? SrcLatitude { get; set; }

        public decimal? SrcLongitude { get; set; }

        public int? RecordCount { get; set; }

        public string CountryCode { get; set; }
        public int PricingSourceId { get; set; }
        public int? PricingCodeId { get; set; }
        public int CompanyCountryId { get; set; }

        public DataTableSearchModel requestModel { get; set; } = new DataTableSearchModel(new DataTableAjaxPostModel());
    }
}
