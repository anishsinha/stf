using FileHelpers;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels
{
    [DelimitedRecord(",")]
    [IgnoreEmptyLines]
    public class Is360DemandModel
    {
        [FieldQuoted]
        public string CustomerName { get; set; }
        [FieldQuoted]
        public string TankName { get; set; }
        [FieldQuoted]
        public string InventoryReadingDate { get; set; }
        [FieldQuoted]
        public string InventoryVolume { get; set; }
        [FieldQuoted]
        public string TankLegacyId { get; set; }
        [FieldQuoted]
        public string WaterLevel { get; set; }
    }
    }

