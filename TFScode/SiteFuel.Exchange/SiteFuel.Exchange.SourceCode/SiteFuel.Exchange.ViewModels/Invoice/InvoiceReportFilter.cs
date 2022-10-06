using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class InvoiceReportFilter: DataTableAjaxPostModel
    {
        [Display(Name = nameof(Resource.lblFrom), ResourceType = typeof(Resource))]
        public string StartDate { get; set; }

        [Display(Name = nameof(Resource.lblTo), ResourceType = typeof(Resource))]
        public string EndDate { get; set; }

        public int CompanyId { get; set; }

        public List<int> JobIds { get; set; }

        public List<int> CustomerCompanyIds { get; set; }

        public List<int> SupplierCompanyIds { get; set; }

        public int CompanyProfile { get; set; }

        public int UserId { get; set; }

        public string EmailTo { get; set; }
    }
}
