using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class TerminalDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Distance { get; set; }
        public string AvgPrice { get; set; }
    }

    public class BulkPlantRequestModel
    {
        public int CountryId { get; set; }

        public string BulkPlantIds { get; set; }

        public decimal JobLatitude { get; set; }

        public decimal JobLongitude { get; set; }

        public int CompanyCountryId { get; set; }

        public int CompanyId { get; set; }
    }
}
