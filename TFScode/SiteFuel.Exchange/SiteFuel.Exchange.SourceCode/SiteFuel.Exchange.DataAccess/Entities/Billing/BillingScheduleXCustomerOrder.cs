using System.ComponentModel.DataAnnotations.Schema;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public partial class BillingScheduleXCustomerOrder
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BillingScheduleXCustomerOrder()
        {
        }

        public int Id { get; set; }
        public int BillingScheduleId { get; set; }
        public int OrderId { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey("BillingScheduleId")]
        public virtual BillingSchedule BillingSchedule { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }
    }
}
