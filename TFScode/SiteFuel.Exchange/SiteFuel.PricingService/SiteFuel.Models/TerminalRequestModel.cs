using System.Collections.Generic;

namespace SiteFuel.Models
{
    public class TerminalRequestModel
    {
        public int ProductId { get; set; }

        public int CountryId { get; set; }

        public string SearchStringTeminal { get; set; }

        public decimal SrcLatitude { get; set; }

        public decimal SrcLongitude { get; set; }

        public int PricingCodeId { get; set; }

        public int CompanyCountryId { get; set; }
        public bool IsSuppressPricing { get; set; }
    }

    public class TerminalForFueltypesRequestModel
    {
        public string ProductId { get; set; }

        public int CountryId { get; set; }

        public string SearchStringTeminal { get; set; }

        public decimal SrcLatitude { get; set; }

        public decimal SrcLongitude { get; set; }

        public int PricingCodeId { get; set; }
        public int CompanyCountryId { get; set; }
        public bool IsSuppressPricing { get; set; }
    }


    public class TerminalRequestViewModel 
    {
        public int ProductId { get; set; }

        public int CountryId { get; set; }

        public string TerminalIds { get; set; }

        public int PricingTypeId { get; set; }

        public decimal SrcLatitude { get; set; }

        public decimal SrcLongitude { get; set; }

        public int PricingCodeId { get; set; }

        public int CompanyCountryId { get; set; }

        public bool IsSuppressPricing { get; set; }
    }
}
