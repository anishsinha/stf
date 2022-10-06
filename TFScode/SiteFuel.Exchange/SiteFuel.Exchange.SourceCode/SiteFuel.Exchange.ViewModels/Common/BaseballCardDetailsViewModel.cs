using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class BaseballCardDetailsViewModel
    {
        public BaseballCardDetailsViewModel()
        {

        }

        public string OnTime30Days { get; set; }
        public string OnTime3Months { get; set; }
        public string OnTime6Months { get; set; }
        public string OnTimeAll { get; set; }

        public string Accuracy30Days { get; set; }
        public string Accuracy3Months { get; set; }
        public string Accuracy6Months { get; set; }
        public string AccuracyAll { get; set; }

        public string MobileDrop30Days { get; set; }
        public string MobileDrop3Months { get; set; }
        public string MobileDrop6Months { get; set; }
        public string MobileDropAll { get; set; }

        public string AvgTime30Days { get; set; }
        public string AvgTime3Months { get; set; }
        public string AvgTime6Months { get; set; }
        public string AvgTimeAll { get; set; }
    }
}
