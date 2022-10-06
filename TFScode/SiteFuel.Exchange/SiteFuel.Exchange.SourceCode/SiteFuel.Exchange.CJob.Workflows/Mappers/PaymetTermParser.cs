using SiteFuel.Exchange.Quickbooks;
using SiteFuel.Exchange.Quickbooks.Workflows.Models;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.AccountingEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.CJob.Workflows.Mappers
{
    public static class PaymetTermParser
    {
        public static void ParsePaymetTerm(AccountingWorkflowViewModel workflow, ViewModels.InvoicePdfViewModel invoicePdfViewModel, QbInvoiceViewModel qbInvoiceViewModel)
        {
            ViewModels.Quickbooks.PaymentTerms paymentTerm = null;
            if (invoicePdfViewModel.PaymentTermId == (int)PaymentTerms.NetDays)
            {
                paymentTerm = workflow.QbCompanyProfile.PaymentTerms.FirstOrDefault(x => x.IsActive && x.TermDays == invoicePdfViewModel.NetDays);
            }
            else if (invoicePdfViewModel.PaymentTermId == (int)PaymentTerms.DueOnReceipt)
            {
                paymentTerm = workflow.QbCompanyProfile.PaymentTerms.FirstOrDefault(x => x.IsActive 
                && x.TermName.Equals(ApplicationConstants.QbTermDueOnReceipt, StringComparison.OrdinalIgnoreCase));
            }
            if (paymentTerm != null)
            {
                qbInvoiceViewModel.PaymentTermName = paymentTerm.TermName;
            }
        }
    }
}
