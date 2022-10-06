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
    public class ExternalTerminalMappingCsvViewModel
    {
        [FieldQuoted]
        public string TerminalName { get; set; }

        [FieldOptional]
        [FieldQuoted]
        public string TargetTerminalValue { get; set; }
    }
}
