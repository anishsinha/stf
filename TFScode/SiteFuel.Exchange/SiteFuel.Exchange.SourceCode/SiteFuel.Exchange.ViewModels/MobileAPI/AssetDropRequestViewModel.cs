using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class AssetDropRequestViewModel
    {
        public int AssetDropId { get; set; }
        public decimal Quantity { get; set; }
        public string Gravity { get; set; }
        public int OrderId { get; set; }
        public int JobXAssetId { get; set; }
    }
}
