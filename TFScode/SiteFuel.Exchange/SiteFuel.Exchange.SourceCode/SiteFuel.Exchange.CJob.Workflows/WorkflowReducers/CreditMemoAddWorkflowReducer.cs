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
    public class CreditMemoAddWorkflowReducer : IQuickbooksWorkflowReducer
    {
        public AccountingWorkflowType WorkflowType => AccountingWorkflowType.CreditMemoAdd;

        public List<QbRequestViewModel> Reduce(AccountingWorkflowViewModel workflow, BaseDomain baseDomain)
        {
            var qbRequests = new List<QbRequestViewModel>();
            var qbWorkflowDomain = new QbWorkflowDomain(baseDomain);
            var requestParameters = qbWorkflowDomain.GetRequestParameters(workflow);
            var fuelRequestDomain = new FuelRequestDomain(baseDomain);
            var fuelRequestFees = fuelRequestDomain.GetFuelRequestFees(requestParameters.FuelRequestId);
            var invoiceDomain = new InvoiceDomain(baseDomain);
            var consolidatedInvoicePdfViewModel = Task.Run(() => invoiceDomain.GetConsolidatedInvoicePdfFromInvoiceIdAsync(requestParameters.InvoiceId, Utilities.CompanyType.Supplier)).Result;
            var qbInvoiceViewModel = consolidatedInvoicePdfViewModel.ToInvoiceViewModel(fuelRequestFees, new List<ViewModels.DiscountLineItemViewModel>());
            var firstInvoiceViewModel = consolidatedInvoicePdfViewModel.Invoices.FirstOrDefault(); 
            qbInvoiceViewModel.Memo = firstInvoiceViewModel.OriginalInvoiceNumber + "/" + "{:OriginalInvoiceQbNumber:}"; 
            qbInvoiceViewModel.Items.ForEach(t => t.AccountName = workflow.QbCompanyProfile.IncomeAccountName);
            qbInvoiceViewModel.DiscountItems.ForEach(t => t.AccountName = workflow.QbCompanyProfile.IncomeAccountName);
            var qbDomain = new QbDomain(baseDomain);
            qbInvoiceViewModel.QbInvoiceTxnID = qbDomain.GetInvoiceTxnID(workflow.QbCompanyProfileId, firstInvoiceViewModel.OriginalInvoiceNumberId.Value, QbEntityType.Invoice.ToString());
            var creditMemoWorkflow = new CreditMemoAddWorkflow();
            var workflowResult = creditMemoWorkflow.ExecuteWorkflow(qbInvoiceViewModel);
            SetQbRequestViewModel(workflow, qbRequests, requestParameters, workflowResult);

            return qbRequests;
        }

        private static void SetQbRequestViewModel(AccountingWorkflowViewModel workflow, List<QbRequestViewModel> qbRequests, RequestParameters requestParameters, WorkflowResult workflowResult, int? invoiceNumberId = null)
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
                    Status = (QbRequestStatus)response.Status,
                    QbXmlType = (int)response.QbXmlType,
                    OrderId = requestParameters.OrderId,
                    EntityType = response.EntityType,
                    InvoiceNumberId = invoiceNumberId,
                    PoNumber = orderPoNumber
                };
                qbRequests.Add(request);
            }
        }        
    }
}
