using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.DataAccess.Entities
{
    public class ExceptionTypeXApprover
    {
        public int Id { get; set; }
        public int ExceptionTypeId { get; set; }
        public int ApproverId { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        [ForeignKey("ExceptionTypeId")]
        public virtual ExceptionType ExceptionType { get; set; }
        [ForeignKey("ApproverId")]
        public virtual ExceptionApprover ExceptionApprover { get; set; }
    }
}
