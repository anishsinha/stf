using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.ExternalEntityMappings
{
    [DelimitedRecord(",")]
    [IgnoreEmptyLines]
    public class ExternalCustomerLocationMappingCsvViewModel
    {
        [FieldQuoted]
        public string CompanyName { get; set; }

        [FieldQuoted]
        public string CustomerLocationName { get; set; }

        [FieldOptional]
        [FieldQuoted]
        public string TargetCustomerValue { get; set; }

        [FieldOptional]
        [FieldQuoted]
        public string TargetCustomerLocationValue { get; set; }
    }
}
