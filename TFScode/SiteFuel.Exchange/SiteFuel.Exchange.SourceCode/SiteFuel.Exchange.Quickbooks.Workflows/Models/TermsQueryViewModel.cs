using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Quickbooks.Workflows.Models
{
    public class TermsQueryViewModel : WorkflowRequest
    {
        public int CompanyId { get; set; }
    }
}
