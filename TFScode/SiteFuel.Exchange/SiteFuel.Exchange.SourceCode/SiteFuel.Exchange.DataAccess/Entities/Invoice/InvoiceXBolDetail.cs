using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public class InvoiceXBolDetail
    {
        [Key, Column(Order = 0)]
        public int InvoiceHeaderId { get; set; }
        [Key, Column(Order = 1)]
        public int InvoiceId { get; set; }
        [Key, Column(Order = 2)]
        public int BolDetailId { get; set; }

        [ForeignKey("InvoiceHeaderId")]
        public virtual InvoiceHeaderDetail InvoiceHeaderDetail { get; set; }

        [ForeignKey("InvoiceId")]
        public virtual Invoice Invoice { get; set; }

        [ForeignKey("BolDetailId")]
        public virtual InvoiceFtlDetail InvoiceFtlDetail { get; set; }
    }
}
