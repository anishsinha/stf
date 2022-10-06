using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiteFuel.Exchange.Core;

namespace SiteFuel.Exchange.ViewModels.Quickbooks
{
    public class QbDataTableModel : DataTableAjaxPostModel
    {
        public string FromDateTime { get; set; }
        public string ToDateTime { get; set; }
        public int CompanyId { get; set; }
        public int AccountingWorkflowId { get; set; }
    }
}
