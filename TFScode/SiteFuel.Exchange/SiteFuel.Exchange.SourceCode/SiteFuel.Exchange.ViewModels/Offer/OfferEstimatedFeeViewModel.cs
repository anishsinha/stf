using Foolproof;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Offer
{
    public class OfferEstimatedFeeViewModel
    {
        public OfferEstimatedFeeViewModel()
        {
        }
        public string FeeType { get; set; }
        public string FeeSubType { get; set; }
        public string Duration { get; set; }
        public string AssetCount { get; set; }
        public decimal Fee { get; set; }
    }
}
