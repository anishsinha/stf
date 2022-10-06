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
    public class ExternalBulkPlantMappingCsvViewModel
    {
        [FieldQuoted]
        public string BulkPlantName { get; set; }

        [FieldOptional]
        [FieldQuoted]
        public string TargetBulkPlantValue { get; set; }
    }
}
