using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class AddressViewModel : StatusViewModel
    {
        public AddressViewModel()
        {
        }

        public AddressViewModel(Status status) : base(status)
        {
        }

        public string Address { get; set; }

        public string AddressLine2 { get; set; }

        public string AddressLine3 { get; set; }

        public string City { get; set; }

        public string StateName { get; set; }

        public string StateCode { get; set; }

        public string CountryName { get; set; }

        public string CountryCode { get; set; }

        public string ZipCode { get; set; }

        public string CountyName { get; set; }

        public int LocationType { get; set; }

        public int CountryId { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }
    }
}
