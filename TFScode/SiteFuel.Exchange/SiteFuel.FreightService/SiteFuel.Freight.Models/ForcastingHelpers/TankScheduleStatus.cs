using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels.ForcastingHelpers
{
    public class TankScheduleStatus
    {
        public string Id { get; set; }
        public int TfxProductTypeId { get; set; }
        public DeliveryReqStatus Status { get; set; }
        public int TfxJobId { get; set; }
    }
}
