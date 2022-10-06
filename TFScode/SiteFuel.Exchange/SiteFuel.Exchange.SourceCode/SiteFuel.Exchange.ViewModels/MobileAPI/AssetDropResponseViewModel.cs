using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class AssetDropResponseViewModel
    {
        public int AssetDropId { get; set; }

        public int OrderId { get; set; }

        public decimal Quantity { get; set; }

        public string FuelType { get; set; }

        public string ProductType { get; set; }

        public int JobXAssetId { get; set; }
    }
}
