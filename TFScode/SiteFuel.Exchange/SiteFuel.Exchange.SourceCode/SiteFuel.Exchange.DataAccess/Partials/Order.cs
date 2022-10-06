using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public partial class Order
    {
        public bool SendDtnFile
        {
            get
            {
                return OrderAdditionalDetail != null &&
                        (OrderAdditionalDetail.BOLInvoicePreferenceId == (int)InvoiceNotificationPreferenceTypes.SendDDTWithBillingFile ||
                        OrderAdditionalDetail.BOLInvoicePreferenceId == (int)InvoiceNotificationPreferenceTypes.SendOnlyBillingFile ||
                        OrderAdditionalDetail.BOLInvoicePreferenceId == (int)InvoiceNotificationPreferenceTypes.SendInvoiceWithBillingFile);
            }
        }
    }
}
