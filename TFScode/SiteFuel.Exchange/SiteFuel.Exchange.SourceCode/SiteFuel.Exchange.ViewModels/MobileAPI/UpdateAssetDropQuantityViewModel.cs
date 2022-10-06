using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class UpdateAssetDropQuantityViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public int AssetDropId { get; set; }

        public decimal Quantity { get; set; }

        public int DriverId { get; set; }

        public decimal PrimaryMeterStartReading { get; set; }

        public decimal PrimaryMeterEndReading { get; set; }

        public string Gravity { get; set; }

        public List<AssetDropRequestViewModel> AssetDropDetail { get; set; }
    }
}
