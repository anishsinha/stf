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
    public class ExternalSupplierMappingCsvViewModel
    {
        [FieldQuoted]
        public string SupplierName { get; set; }

        [FieldOptional]
        [FieldQuoted]
        public string TargetSupplierValue { get; set; }
    }
}
