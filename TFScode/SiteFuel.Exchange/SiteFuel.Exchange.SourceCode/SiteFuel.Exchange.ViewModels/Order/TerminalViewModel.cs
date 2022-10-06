using System.IO;

namespace SiteFuel.Exchange.ViewModels
{
    public class TerminalViewModel
    {
        public int TerminalId { get; set; }

        public string TerminalName { get; set; }

        public double Distance { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string StateCode { get; set; }

        public string ZipCode { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }
    }
}
