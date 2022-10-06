using SiteFuel.Exchange.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.BillingStatement
{
    public class BillingScheduleGridViewModel
    {
        public int Id { get; set; }
        public string BillingStatementId { get; set; }
        public string Frequency { get; set; }
        public string StartDate { get; set; }
        public string Customer { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int TotalCount { get; set; }
    }

    public class BillingScheduleFilterViewModel : BaseInputViewModel
    {
        public string BillingStatementId { get; set; }
    }

    public class BillingDataTableViewModel : DataTableAjaxPostModel
    {
        public string BillingStatementId { get; set; }
    }
}