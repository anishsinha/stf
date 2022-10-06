using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class TimeStamps
    {
        public long startDropTime { get; set; }
        public long endDropTime { get; set; }
        public long imageCaptureTime { get; set; }
        public long amountDroppedTime { get; set; }
        public long amountConfirmedTime { get; set; }
        public long assetStartDropTime { get; set; }
        public long assetEndDropTime { get; set; }
    }
}
