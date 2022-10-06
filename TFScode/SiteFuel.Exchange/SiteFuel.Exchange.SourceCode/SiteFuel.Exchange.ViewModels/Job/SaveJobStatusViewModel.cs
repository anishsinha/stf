using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.CustomAttributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class SaveJobStatusViewModel : StatusViewModel
    {     
        public bool IsAssetTrackingEnabled { get; set; }  
        
        public bool IsRetailJob { get; set; }
        public int JobId { get; set; }
    }
}
