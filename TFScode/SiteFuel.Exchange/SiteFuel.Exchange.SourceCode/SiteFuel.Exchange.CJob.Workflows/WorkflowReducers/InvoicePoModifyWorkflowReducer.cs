using SiteFuel.Exchange.CJob.Workflows.Mappers;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Quickbooks.Models;
using SiteFuel.Exchange.Quickbooks.SharedEnums;
using SiteFuel.Exchange.Quickbooks.Workflows;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.AccountingEvent;
using SiteFuel.Exchange.ViewModels.Quickbooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.CJob.Workflows.WorkflowReducers
{
    public class InvoicePoModifyWorkflowReducer : IQuickbooksWorkflowReducer
    {
        public AccountingWorkflowType WorkflowType => AccountingWorkflowType.POModify;

        public List<QbRequestViewModel> Reduce(AccountingWorkflowViewModel workflow, BaseDomain baseDomain)
        {
            var qbRequests = new List<QbRequestViewModel>();
            var qbWorkflowDomain = new QbWorkflowDomain(baseDomain);
            var requestParameters = qbWorkflowDomain.GetRequestParameters(workflow);
            var rqStatus = QbRequestStatus.Unknown;
            if (requestParameters.InvoiceCreationDate != null && requestParameters.InvoiceCreationDate < workflow.QbCompanyProfile.SyncStartDate)
            {
                rqStatus = QbRequestStatus.SyncSkip;
            }

            var invoiceDomain = new InvoiceDomain(baseDomain);
            var invoiceFees = invoiceDomain.GetPreviousInvoiceFees(requestParameters.InvoiceId, requestParameters.InvoiceNumberId);
            var invoicePdfViewModel = Task.Run(() => invoiceDomain.GetConsolidatedInvoicePdfFromInvoiceIdAsync(requestParameters.InvoiceId, Utilities.CompanyType.Buyer)).Result;
            var qbInvoiceViewModel = invoicePdfViewModel.ToInvoicePoViewModel(invoiceFees.Item1, invoiceFees.Item2);
            var qbDomain = new QbDomain(baseDomain);
            var firstInvoice = invoicePdfViewModel.Invoices.FirstOrDefault();
            qbInvoiceViewModel.QbPurchaseOrderTxnID = qbDomain.GetOrderTxnIdToModifyQbPo(workflow.QbCompanyProfileId, firstInvoice.OrderId ?? 0, QbEntityType.PurchaseOrder.ToString(), firstInvoice.InvoiceNumber.Id);
            qbInvoiceViewModel.ClassName = requestParameters.ClassName;
            qbInvoiceViewModel.Items.ForEach(t =>
            {
                t.Prefix = workflow.QbCompanyProfile.ItemPrefix;
                t.AccountName = workflow.QbCompanyProfile.ExpenseAccountName;
            });

            var isDiscountSynced = firstInvoice.CreatedDate >= ApplicationConstants.DiscountSyncStartDate;
            qbInvoiceViewModel.DiscountItems.ForEach(t =>
            {
                t.IsNewlyAdded = isDiscountSynced ? t.IsNewlyAdded : true;
                t.Prefix = workflow.QbCompanyProfile.ItemPrefix;
                t.AccountName = workflow.QbCompanyProfile.DiscountAccountName ?? workflow.QbCompanyProfile.ExpenseAccountName;
            });

            var invoicePoWorkflow = new InvoicePoModifyWorkflow();
            var workflowResult = invoicePoWorkflow.ExecuteWorkflow(qbInvoiceViewModel);
            foreach (var response in workflowResult.AdapterResponses)
            {
                var request = new QbRequestViewModel
                {
                    WorkflowId = workflow.Id,
                    EntityId = response.EntityId,
                    QbXmlRq = response.QbXml,
                    Status = rqStatus == QbRequestStatus.SyncSkip ? QbRequestStatus.SyncSkip : (QbRequestStatus)response.Status,
                    QbXmlType = (int)response.QbXmlType,
                    OrderId = requestParameters.OrderId,
                    EntityType = response.EntityType,
                    InvoiceNumberId = qbInvoiceViewModel.InvoiceNumberId,
                    PoNumber = firstInvoice.PoNumber
                };
                qbRequests.Add(request);
            }

            return qbRequests;
        }
    }
}
