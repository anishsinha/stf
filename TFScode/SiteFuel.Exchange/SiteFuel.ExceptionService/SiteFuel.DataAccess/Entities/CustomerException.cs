using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.DataAccess.Entities
{
   public class CustomerException
    {
        public int Id { get; set; }
        public int ExceptionTypeId { get; set; }
        public int OwnerCompanyId { get; set; }
        public int EnabledForCompanyId { get; set; }
        public int ApproverCompanyId { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public int UpdateBy { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        [ForeignKey("ExceptionTypeId")]
        public virtual ExceptionType ExceptionType { get; set; }
    }
}
