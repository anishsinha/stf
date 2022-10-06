using System;
using System.IO;

namespace SiteFuel.Exchange.ViewModels
{
    public class TankVolumeAndUllageViewModel : StatusViewModel
    {
        public decimal TankVolume { get; set; }

        public decimal TankUllage { get; set; }
        public string CaptureTime { get; set; }
    }
}
