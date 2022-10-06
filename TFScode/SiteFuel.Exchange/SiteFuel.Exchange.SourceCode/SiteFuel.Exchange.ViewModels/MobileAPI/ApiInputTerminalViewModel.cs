using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class ApiInputTerminalViewModel
    {
        public List<int> OrderIds { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }
    }
}
