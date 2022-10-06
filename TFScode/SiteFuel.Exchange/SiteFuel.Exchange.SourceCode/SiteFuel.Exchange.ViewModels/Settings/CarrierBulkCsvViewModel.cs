using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Settings
{
    [DelimitedRecord(",")]
    [IgnoreEmptyLines]
    public class CarrierBulkCsvViewModel
    {
        [FieldQuoted]
        public string CarrierCompany { get; set; }

        [FieldQuoted]
        public string Location { get; set; }

        [FieldQuoted]
        [FieldOptional]
        public string Email { get; set; }


    }


    public class CarrierBulkViewModel
    {

        public string CarrierCompany { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
        public int CarrierCompanyId {get; set;}
        public int JobId { get; set; }
        public int UserId { get; set; }
        public CarrierJobViewModel Job { get; set; }

    }
}
