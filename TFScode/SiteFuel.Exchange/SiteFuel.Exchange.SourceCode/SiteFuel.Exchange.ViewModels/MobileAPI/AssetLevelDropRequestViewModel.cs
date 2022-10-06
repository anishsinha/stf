using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class AssetLevelDropRequestViewModel
    {
        public AssetLevelDropRequestViewModel()
        {
            DriverId = 0;
        }
        public int CompanyId { get; set; }

        public List<int> OrderId { get; set; }

        public int DriverId { get; set; }
        
    }
}
