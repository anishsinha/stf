using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels
{
    public class ForecastingDeliveryModel
    {
        public string TankName { get; set; }
        public string TankId { get; set; }
        public string StorageId { get; set; }
        public string SiteId { get; set; }
        public int NoOfDeliveries { get; set; }
        public decimal LastDeliveredQtyValue { get; set; }
        public DateTime LastDeliveredDateTime { get; set; }
        public string LastDeliveredQty { get; set; }
        public string LastDeliveredDate { get; set; }
        public Nullable<int> TankSequence { get; set; }
    }
    public class ForecastingTankDeliveryModel
    {
        public int jobId { get; set; }
        public string tankId { get; set; }
        public string storageId { get; set; }
        public string uOM { get; set; }
        public string startDtTm { get; set; }
    }
}
