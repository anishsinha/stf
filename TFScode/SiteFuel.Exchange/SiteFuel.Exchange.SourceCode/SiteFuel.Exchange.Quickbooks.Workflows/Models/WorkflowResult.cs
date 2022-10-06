using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Quickbooks.Workflows.Models
{
    public class WorkflowResult
    {
        public WorkflowResult()
        {
            AdapterResponses = new List<AdapterResponse>();
            Errors = new List<string>();
        }

        public List<AdapterResponse> AdapterResponses { get; set; }

        public bool IsSuccess { get; set; }

        public List<string> Errors { get; set; }
    }
}
