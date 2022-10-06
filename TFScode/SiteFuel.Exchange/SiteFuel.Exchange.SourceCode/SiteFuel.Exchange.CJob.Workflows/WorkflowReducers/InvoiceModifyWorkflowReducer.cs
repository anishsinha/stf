using SiteFuel.Exchange.CJob.Workflows.Mappers;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Quickbooks.Models;
using SiteFuel.Exchange.Quickbooks.SharedEnums;
using SiteFuel.Exchange.Quickbooks.Workflows;
using SiteFuel.Exchange.ViewModels.AccountingEvent;
using SiteFuel.Exchange.ViewModels.Quickbooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.CJob.Workflows.WorkflowReducers
{
    public class InvoiceModifyWorkflowReducer : IQuickbooksWorkflowReducer
    {
        public AccountingWorkflowType WorkflowType => AccountingWorkflowType.InvoiceModify;

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
            var invoiceItems = invoiceDomain.GetPreviousInvoiceFees(requestParameters.InvoiceId, requestParameters.InvoiceNumberId);
            var consolidatedInvoicePdfViewModel = Task.Run(() => invoiceDomain.GetConsolidatedInvoicePdfFromInvoiceIdAsync(requestParameters.InvoiceId, Utilities.CompanyType.Supplier)).Result;
            if(consolidatedInvoicePdfViewModel?.Invoices != null && consolidatedInvoicePdfViewModel.Invoices.Any())
            {
                var qbInvoiceViewModel = consolidatedInvoicePdfViewModel.ToInvoiceViewModel(invoiceItems.Item1, invoiceItems.Item2);
                var qbDomain = new QbDomain(baseDomain);
                qbInvoiceViewModel.QbInvoiceTxnID = qbDomain.GetInvoiceTxnID(workflow.QbCompanyProfileId, qbInvoiceViewModel.InvoiceNumberId, QbEntityType.Invoice.ToString());
                qbInvoiceViewModel.Items.ForEach(t => t.AccountName = workflow.QbCompanyProfile.IncomeAccountName);
                qbInvoiceViewModel.DiscountItems.ForEach(t => t.AccountName = workflow.QbCompanyProfile.IncomeAccountName);
                var fuelSurcharges = qbInvoiceViewModel.Items.FirstOrDefault(t => t.Name.Contains(QbConstants.FuelSurcharges));
                if (fuelSurcharges != null)
                {
                    fuelSurcharges.IsNewlyAdded = false;
                }
                qbInvoiceViewModel.Memo = consolidatedInvoicePdfViewModel.InvoicePdfHeaderDetail.DisplayInvoiceNumber;
                var invoiceWorkflow = new InvoiceModifyWorkflow();
                var workflowResult = invoiceWorkflow.ExecuteWorkflow(qbInvoiceViewModel);
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
                        InvoiceNumberId = requestParameters.InvoiceNumberId,
                        PoNumber = consolidatedInvoicePdfViewModel.Invoices.FirstOrDefault().PoNumber
                    };
                    qbRequests.Add(request);
                }
            }
            return qbRequests;
        }
    }
}
