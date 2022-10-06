using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class JobTankDetailViewModel : BaseViewModel
    {
        public int AssetId { get; set; }
        public string SiteId { get; set; }
        public string TankId { get; set; }
        public string StorageId { get; set; }
        public string TankName { get; set; }
        public string TankNumber { get; set; }
        public int ProductTypeId { get; set; }
        public int JobId { get; set; }
        public string JobName { get; set; }
        public int BuyerCompanyId { get; set; }
        public string BuyerCompanyName { get; set; }
        public string Address { get; set; }
    }
}
