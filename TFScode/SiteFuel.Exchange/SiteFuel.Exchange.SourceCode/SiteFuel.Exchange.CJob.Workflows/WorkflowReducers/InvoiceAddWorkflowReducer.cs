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
    public class InvoiceAddWorkflowReducer : IQuickbooksWorkflowReducer
    {
        public AccountingWorkflowType WorkflowType => AccountingWorkflowType.InvoiceAdd;

        public List<QbRequestViewModel> Reduce(AccountingWorkflowViewModel workflow, BaseDomain baseDomain)
        {
            var qbRequests = new List<QbRequestViewModel>();
            var qbWorkflowDomain = new QbWorkflowDomain(baseDomain);
            var requestParameters = qbWorkflowDomain.GetRequestParameters(workflow);
            bool isRebillInvoice = false;
            var workflowResult = ExecuteAndGetInvoiceWorkflowResult(workflow, baseDomain, requestParameters, ref isRebillInvoice);
            SetQbRequestViewModel(workflow, qbRequests, requestParameters, workflowResult, isRebillInvoice);

            return qbRequests;
        }

        private static void SetQbRequestViewModel(AccountingWorkflowViewModel workflow, List<QbRequestViewModel> qbRequests, RequestParameters requestParameters, WorkflowResult workflowResult, bool isRebillInvoice, int? invoiceNumberId = null)
        {
            var orderDomain = new OrderDomain();
            var orderPoNumber = Task.Run(() => orderDomain.GetOrderPoNumber(requestParameters.OrderId)).Result;
            foreach (var response in workflowResult.AdapterResponses)
            {
                var request = new QbRequestViewModel
                {
                    WorkflowId = workflow.Id,
                    EntityId = response.EntityId,
                    QbXmlRq = response.QbXml,
                    Status = isRebillInvoice && response.QbXmlType == QbXmlType.SalesOrderAdd ? (QbRequestStatus)QbXmlStatus.NotReadyToQueue : (QbRequestStatus)response.Status,
                    QbXmlType = (int)response.QbXmlType,
                    OrderId = requestParameters.OrderId,
                    EntityType = response.EntityType,
                    InvoiceNumberId = requestParameters.InvoiceNumberId,
                    PoNumber = orderPoNumber
                };
                qbRequests.Add(request);
            }
        }

        private static WorkflowResult ExecuteAndGetInvoiceWorkflowResult(AccountingWorkflowViewModel workflow, BaseDomain baseDomain, RequestParameters requestParameters, ref bool isRebillInvoice)
        {   
            var fuelRequestFees = new List<ViewModels.FeesViewModel>(); // make it empty as new invoices having isNewlyFeeAdded == true
            var invoiceDomain = new InvoiceDomain(baseDomain);
            var consolidatedInvoicePdfViewModel = Task.Run(() => invoiceDomain.GetConsolidatedInvoicePdfFromInvoiceIdAsync(requestParameters.InvoiceId, Utilities.CompanyType.Supplier)).Result;
            var qbInvoiceViewModel = consolidatedInvoicePdfViewModel.ToInvoiceViewModel(fuelRequestFees, new List<ViewModels.DiscountLineItemViewModel>());
            var firstInvoice = consolidatedInvoicePdfViewModel.Invoices.FirstOrDefault();
            var qbDomain = new QbDomain(baseDomain);
            qbInvoiceViewModel.IsPrimaryOrder = qbDomain.IsPrimarySalesOrder(workflow.QbCompanyProfileId, firstInvoice.OrderId ?? 0, QbEntityType.SalesOrder.ToString());
            qbInvoiceViewModel.QbSalesOrderTxnID = qbDomain.GetOrderTxnID(workflow.QbCompanyProfileId, firstInvoice.OrderId ?? 0, QbEntityType.SalesOrder.ToString());
            qbInvoiceViewModel.Memo = firstInvoice.DisplayInvoiceNumber;
            if (qbInvoiceViewModel.IsRebillInvoice)
            {
                isRebillInvoice = true;
                qbInvoiceViewModel.QbInvoiceTxnID = qbDomain.GetInvoiceTxnID(workflow.QbCompanyProfileId, firstInvoice.OriginalInvoiceNumberId.Value, QbEntityType.Invoice.ToString());
                qbInvoiceViewModel.Memo = firstInvoice.DisplayInvoiceNumber + "-" + firstInvoice.OriginalInvoiceNumber + "/" + "{:OriginalInvoiceQbNumber:}";
            }
            if (qbInvoiceViewModel.IsPrimaryOrder)
            {
                qbDomain.UpdateQbEntityMappingWithInvoiceNumberId(requestParameters.OrderId, QbEntityType.SalesOrder.ToString(), requestParameters.InvoiceNumberId);
            }
            qbInvoiceViewModel.Items.ForEach(t => t.AccountName = workflow.QbCompanyProfile.IncomeAccountName);
            qbInvoiceViewModel.DiscountItems.ForEach(t => t.AccountName = workflow.QbCompanyProfile.IncomeAccountName);
            var invoiceWorkflow = new InvoiceAddWorkflow();
            var workflowResult = invoiceWorkflow.ExecuteWorkflow(qbInvoiceViewModel);
            return workflowResult;
        }
    }
}
