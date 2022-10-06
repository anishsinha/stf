using SiteFuel.Exchange.Core.StringResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
   public class AccountingCompanyIdDetailsViewModel
    {
        public int SupplierCompanyId { get; set; }

        public int BuyerCompanyId { get; set; }
        public int JobId { get; set; }
    
        [Display(Name = nameof(Resource.lblAccountingCompanyId), ResourceType = typeof(Resource))]
        public string AccountingCompanyId { get; set; }

        [Display(Name = nameof(Resource.lblCompanyName), ResourceType = typeof(Resource))]
        public string BuyerCompanyName { get; set; }

        [Display(Name = nameof(Resource.lblLocation), ResourceType = typeof(Resource))]
        public string JobName { get; set; }
    }
}
