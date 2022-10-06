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
    public class ExternalVehicleMappingCsvViewModel
    {
        [FieldQuoted]
        public string TruckName { get; set; }

        [FieldOptional]
        [FieldQuoted]
        public string TargetVehicleValue { get; set; }
    }
}
