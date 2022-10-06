using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;

namespace SiteFuel.Exchange.ViewModels.ExternalEntityMappings
{
    [DelimitedRecord(",")]
    [IgnoreEmptyLines]
    public class ExternalCarrierMappingCsvViewModel
    {
        [FieldQuoted]
        public string CarrierName { get; set; }

        [FieldOptional]
        [FieldQuoted]
        public int TargetCarrierValue { get; set; }
    }
}
