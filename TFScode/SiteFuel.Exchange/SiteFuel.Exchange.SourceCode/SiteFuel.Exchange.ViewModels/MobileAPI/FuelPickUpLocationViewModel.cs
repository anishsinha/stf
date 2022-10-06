using SiteFuel.Exchange.Utilities;
using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class FuelPickUpLocationViewModel
    {
        public FuelPickUpLocationViewModel()
        {
            
        }

        public int? Id { get; set; }

        public int? TerminalId { get; set; }

        public bool IsPickUpLocation { get; set; }

        public string TerminalName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string StateCode { get; set; }

        public string ZipCode { get; set; }

        public string CountryCode { get; set; }

        public string CountyName { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }
    }
}
