using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels
{
    public class ForecastingExistingScheduleModel
    {
        public string TankId { get; set; }
        public string StorageId { get; set; }
        public string SiteId { get; set; }
        public string TankName { get; set; }
        public int ExistingDeliverySchedule { get; set; }
        public int DeliveryRequest { get; set; }
        public Nullable<int> TankSequence { get; set; }
    }
}
