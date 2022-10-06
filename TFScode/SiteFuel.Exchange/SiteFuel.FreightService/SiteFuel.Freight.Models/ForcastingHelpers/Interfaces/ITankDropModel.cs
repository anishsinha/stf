using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels.ForcastingHelpers
{
    public interface ITankDropModel
    {
        string SiteId { get; set; }
        string TankId { get; set; }
        string StorageId { get; set; }
        decimal DroppedQuantity { get; set; }
        DateTime StartTime { get; set; }
        DateTime EndTime { get; set; }
        string TimeZoneName{ get; set; }
    }
}
