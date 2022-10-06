using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public class LeadCustomerInformation
    {
        [Key]
        public int Id { get; set; }
        public int LeadRequestId { get; set; }
        public int CompanyId { get; set; }
        [StringLength(256)]
        public string CompanyName { get; set; }
        [StringLength(256)]
        public string Name { get; set; }
        public int UserId { get; set; }
        [StringLength(16)]
        public string PhoneNumber { get; set; }
        [StringLength(256)]
        public string Email { get; set; }
        [ForeignKey("LeadRequestId")]
        public virtual LeadRequest LeadRequest { get; set; }
        public bool IsInvitationEnabled { get; set; }
        public bool IsNotifyDeliveries { get; set; }
        public bool IsNotifySchedules { get; set; }
    }
}
