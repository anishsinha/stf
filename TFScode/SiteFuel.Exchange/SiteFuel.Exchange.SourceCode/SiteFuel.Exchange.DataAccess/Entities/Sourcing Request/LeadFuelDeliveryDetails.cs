using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public class LeadFuelDeliveryDetail
    {
        [Key]
        public int Id { get; set; }
        public int LeadRequestId { get; set; }
        public int DeliveryTypeId { get; set; }
        public Nullable<DateTimeOffset> StartDate { get; set; }
        public Nullable<DateTimeOffset> EndDate { get; set; }
        [StringLength(256)]
        public string StartTime { get; set; }
        [StringLength(256)]
        public string EndTime { get; set; }
        public SingleDeliverySubTypes SingleDeliverySubTypes { get; set; }
        public PaymentMethods PaymentMethods { get; set; }
        public int PaymentTermId { get; set; }
        public int NetDays { get; set; }
        public bool IsPrePostDipRequired { get; set; }
        public OrderEnforcement OrderEnforcementId { get; set; }
        [ForeignKey("LeadRequestId")]
        public virtual LeadRequest LeadRequest { get; set; }
    }
}
