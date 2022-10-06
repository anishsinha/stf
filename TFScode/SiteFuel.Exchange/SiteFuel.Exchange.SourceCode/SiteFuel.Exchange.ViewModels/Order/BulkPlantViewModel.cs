using System.IO;

namespace SiteFuel.Exchange.ViewModels
{
    public class BulkPlantViewModel
    {
        public int Id { get; set; }

        public string BulkPlantName { get; set; }

        public decimal Distance { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string StateCode { get; set; }

        public string ZipCode { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public string CountryCode { get; set; }

        public string CountyName { get; set; }
    }
}
