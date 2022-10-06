using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public partial class Invoice
    {
        public bool ReplaceInvoiceWithDdt
        {
            get
            {
                return Order != null && Order.OrderAdditionalDetail != null &&
                        Order.OrderAdditionalDetail.BOLInvoicePreferenceId == (int)InvoiceNotificationPreferenceTypes.SendDDTWithBillingFile
                       && InvoiceTypeId != (int)InvoiceType.DigitalDropTicketManual && InvoiceTypeId != (int)InvoiceType.DigitalDropTicketMobileApp;
            }
        }
        public bool IsPartOfStatement
        {
            get
            {
                return InvoiceTypeId != (int)InvoiceType.DigitalDropTicketManual && InvoiceTypeId != (int)InvoiceType.DigitalDropTicketMobileApp
                        && BillingStatementXInvoices.Any(t => t.IsActive && t.BillingStatement.IsActive && t.BillingStatement.IsGenerated);
            }
        }

        public bool IsActiveInvoice
        {
            get
            {
                return IsActive && InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active;
            }
        }
    }
}
