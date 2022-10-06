using SiteFuel.Exchange.CJob.Workflows.Mappers;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Quickbooks.Models;
using SiteFuel.Exchange.Quickbooks.SharedEnums;
using SiteFuel.Exchange.Quickbooks.Workflows;
using SiteFuel.Exchange.Quickbooks.Workflows.Models;
using SiteFuel.Exchange.ViewModels.AccountingEvent;
using SiteFuel.Exchange.ViewModels.Quickbooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.CJob.Workflows.WorkflowReducers
{
    public class BillAddWorkflowReducer : IQuickbooksWorkflowReducer
    {
        public AccountingWorkflowType WorkflowType => AccountingWorkflowType.BillAdd;

        public List<QbRequestViewModel> Reduce(AccountingWorkflowViewModel workflow, BaseDomain baseDomain)
        {
            var qbRequests = new List<QbRequestViewModel>();
            var qbWorkflowDomain = new QbWorkflowDomain(baseDomain);
            var requestParameters = qbWorkflowDomain.GetRequestParameters(workflow);

            var invoiceDomain = new InvoiceDomain(baseDomain);
            var invoiceViewModel = Task.Run(() => invoiceDomain.GetConsolidatedBillDetailFromInvoiceIdAsync(requestParameters.InvoiceId)).Result;
            var billViewModel = invoiceViewModel.ToBillViewModel();
            if (billViewModel.IsRebillInvoice)
            {
                QbDomain qbDomain = new QbDomain(baseDomain);
                billViewModel.QbBillTxnID = qbDomain.GetBillTxnID(workflow.QbCompanyProfileId, billViewModel.OrderId, billViewModel.OriginalInvoiceNumberId.Value, QbEntityType.Bill.ToString());
                billViewModel.Memo = billViewModel.OriginalInvoiceNumber + "/" + "{:OriginalBillQbNumber:}";
            }
            billViewModel.Items.ForEach(t =>
            {
                t.Prefix = workflow.QbCompanyProfile.ItemPrefix;
                t.AccountName = workflow.QbCompanyProfile.ExpenseAccountName;
            });

            var billWorkflow = new BillAddWorkflow();
            var workflowResult = billWorkflow.ExecuteWorkflow(billViewModel);
            foreach (var response in workflowResult.AdapterResponses)
            {
                var request = new QbRequestViewModel
                {
                    WorkflowId = workflow.Id,
                    EntityId = response.EntityId,
                    QbXmlRq = response.QbXml,
                    Status = (QbRequestStatus)response.Status,
                    QbXmlType = (int)response.QbXmlType,
                    InvoiceNumberId = billViewModel.IsRebillInvoice && response.QbXmlType == QbXmlType.BillQuery ? billViewModel.OriginalInvoiceNumberId : requestParameters.InvoiceNumberId,
                    EntityType = response.EntityType,
                    PoNumber = billViewModel.PoNumber
                };
                qbRequests.Add(request);
            }

            return qbRequests;
        }
    }
}
