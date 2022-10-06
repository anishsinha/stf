using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Models.ApiModels
{
    public class ExceptionApprovalRequestModel
    {
        public List<int> ExceptionIds { get; set; }
        public int ResolutionTypeId { get; set; }
        /// <summary>
        /// Raised = 1,
        /// Resolved = 2,
        /// AutoApproved = 3
        /// </summary>
        public int StatusId { get; set; }
        public List<int> PendingExceptionIds { get; set; }
        public int ExceptionTypeId { get; set; }
    }
}
