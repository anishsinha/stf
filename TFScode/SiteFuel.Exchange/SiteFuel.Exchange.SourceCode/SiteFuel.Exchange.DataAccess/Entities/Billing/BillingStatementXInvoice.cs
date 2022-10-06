using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public partial class BillingStatementXInvoice
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BillingStatementXInvoice()
        {
        }

        public int Id { get; set; }
        public int StatementId { get; set; }
        public int InvoiceId { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey("StatementId")]
        public virtual BillingStatement BillingStatement { get; set; }

        [ForeignKey("InvoiceId")]
        public virtual Invoice Invoice { get; set; }
    }
}
