using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Models
{
    public class ProductDetailsRequestModel 
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal Radius { get; set; } = 100;
        public string CountryCode { get; set; }
        public int CompanyCountryId { get; set; }
    }
}
