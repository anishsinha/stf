using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels
{
    public class TankVolumeAndUllageModel : StatusModel
    {
        public decimal TankVolume { get; set; }

        public decimal TankUllage { get; set; }
    }
}
