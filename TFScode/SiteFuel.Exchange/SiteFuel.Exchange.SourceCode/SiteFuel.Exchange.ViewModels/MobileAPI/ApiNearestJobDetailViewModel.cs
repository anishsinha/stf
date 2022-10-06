using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class ApiNearestJobDetailViewModel
    {
        public List<ApiJobDetailViewModel> Jobs { get; set; }

        public List<ApiTankDetailViewModel> Tanks { get; set; }
    }
}
