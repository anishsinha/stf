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
    public class VendorCreditAddWorkflowReducer : IQuickbooksWorkflowReducer
    {
        public AccountingWorkflowType WorkflowType => AccountingWorkflowType.VendorCreditAdd;

        public List<QbRequestViewModel> Reduce(AccountingWorkflowViewModel workflow, BaseDomain baseDomain)
        {
            var qbRequests = new List<QbRequestViewModel>();
            var qbWorkflowDomain = new QbWorkflowDomain(baseDomain);
            var requestParameters = qbWorkflowDomain.GetRequestParameters(workflow);
            var fuelRequestDomain = new FuelRequestDomain(baseDomain);
            var fuelRequestFees = fuelRequestDomain.GetFuelRequestFees(requestParameters.FuelRequestId);
            var invoiceDomain = new InvoiceDomain(baseDomain);
            var invoicePdfViewModel = Task.Run(() => invoiceDomain.GetConsolidatedInvoicePdfFromInvoiceIdAsync(requestParameters.InvoiceId, Utilities.CompanyType.Supplier)).Result;
            var qbInvoiceViewModel = invoicePdfViewModel.ToInvoicePoViewModel(fuelRequestFees, new List<ViewModels.DiscountLineItemViewModel>());
            var firstInvoice = invoicePdfViewModel.Invoices.FirstOrDefault();
            qbInvoiceViewModel.InvoiceNumber = firstInvoice.InvoiceNumber.Number;
            qbInvoiceViewModel.Memo = firstInvoice.OriginalInvoiceNumber + "/" + "{:OriginalBillQbNumber:}";
            qbInvoiceViewModel.Items.ForEach(t =>
            {
                t.Prefix = workflow.QbCompanyProfile.ItemPrefix;
                t.AccountName = workflow.QbCompanyProfile.ExpenseAccountName;
            });
            qbInvoiceViewModel.DiscountItems.ForEach(t =>
            {
                t.Prefix = workflow.QbCompanyProfile.ItemPrefix;
                t.AccountName = workflow.QbCompanyProfile.DiscountAccountName ?? workflow.QbCompanyProfile.ExpenseAccountName;
            });
            var qbDomain = new QbDomain(baseDomain);
            qbInvoiceViewModel.QbBillTxnID = qbDomain.GetBillTxnID(workflow.QbCompanyProfileId, qbInvoiceViewModel.OrderId, firstInvoice.OriginalInvoiceNumberId.Value, QbEntityType.Bill.ToString());
            var vendorCreditAddWorkflow = new VendorCreditAddWorkflow();
            var workflowResult = vendorCreditAddWorkflow.ExecuteWorkflow(qbInvoiceViewModel);
            SetQbRequestViewModel(workflow, qbRequests, requestParameters, workflowResult, firstInvoice.OriginalInvoiceNumberId.Value);

            return qbRequests;
        }

        private static void SetQbRequestViewModel(AccountingWorkflowViewModel workflow, List<QbRequestViewModel> qbRequests, RequestParameters requestParameters, WorkflowResult workflowResult, int originalInvoiceNumberId)
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
                    InvoiceNumberId = response.QbXmlType == QbXmlType.BillQuery ? originalInvoiceNumberId : requestParameters.InvoiceNumberId,
                    PoNumber = orderPoNumber
                };
                qbRequests.Add(request);
            }
        }
    }
}
