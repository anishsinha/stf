using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Models.ExceptionApprover
{
    public class ExceptionTypeXApproverModel
    {
        public int Id { get; set; }
        public int ExceptionTypeId { get; set; }
        public string ExceptionTypeName { get; set; }
        public int ApproverId { get; set; }
        public int ApproverName { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
