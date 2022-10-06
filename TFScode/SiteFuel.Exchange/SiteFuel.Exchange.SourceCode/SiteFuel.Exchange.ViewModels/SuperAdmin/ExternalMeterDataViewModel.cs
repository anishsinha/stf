using SiteFuel.Exchange.Core.StringResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class ExternalMeterDataViewModel
    {
        public string MeterData { get; set; }

        [Display(Name = nameof(Resource.lblSupplierCompany), ResourceType = typeof(Resource))]
        public int SupplierCompanyId { get; set; }
    }
}
