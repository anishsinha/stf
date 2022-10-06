using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
   public class TankRetainWindowInfo
    {
        public int Id { get; set; }
        public string siteId { get; set; }
        public string tankId { get; set; }
        public string storageId { get; set; }
        public int startBuffer { get; set; }
        public int endBuffer { get; set; }
        public int maxBuffer { get; set; }
        public decimal Quantity { get; set; }
        public int startBufferUOM { get; set; } = 1;
        public int endBufferUOM { get; set; } = 1;
        public int maxBufferUOM { get; set; } = 1;
        public string TankName { get; set; }
    }

    public class TankRetainInfo
    {
        public int Id { get; set; }
        public string siteId { get; set; }
        public string tankId { get; set; }
        public string storageId { get; set; }
        public string RetainTime { get; set; }
        public string RetainDate { get; set; }
        public string WindowStartTime { get; set; }
        public string WindowStartDate { get; set; }
        public string WindowEndTime { get; set; }
        public string WindowEndDate { get; set; }
        public string TankName { get; set; }
    }
}
