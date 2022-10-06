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
    public class InvoicePoAddWorkflowReducer : IQuickbooksWorkflowReducer
    {
        public AccountingWorkflowType WorkflowType => AccountingWorkflowType.InvoicePoAdd;

        public List<QbRequestViewModel> Reduce(AccountingWorkflowViewModel workflow, BaseDomain baseDomain)
        {
            var qbRequests = new List<QbRequestViewModel>();
            var qbWorkflowDomain = new QbWorkflowDomain(baseDomain);
            var requestParameters = qbWorkflowDomain.GetRequestParameters(workflow);
            var invoiceDomain = new InvoiceDomain(baseDomain);

            var invoicePdfViewModel = Task.Run(() => invoiceDomain.GetConsolidatedInvoicePdfFromInvoiceIdAsync(requestParameters.InvoiceId, Utilities.CompanyType.Buyer)).Result;
            var qbDomain = new QbDomain(baseDomain);
            var firstInvoice = invoicePdfViewModel.Invoices.FirstOrDefault();
            var isPrimaryOrder = qbDomain.IsPurchaseOrderAlreadyExist(firstInvoice.OrderId ?? 0, firstInvoice.InvoiceNumber.Id);
            Quickbooks.Workflows.Models.PurchaseOrderViewModel qbInvoiceViewModel;
            Tuple<List<ViewModels.FeesViewModel>, List<ViewModels.DiscountLineItemViewModel>> invoiceFees;
            if (isPrimaryOrder)
            {
                var fuelRequestFees = new FuelRequestDomain(baseDomain).GetFuelRequestFees(requestParameters.FuelRequestId);
                qbInvoiceViewModel = invoicePdfViewModel.ToInvoicePoViewModel(fuelRequestFees, new List<ViewModels.DiscountLineItemViewModel>());
                qbDomain.UpdateQbEntityMappingWithInvoiceNumberId(firstInvoice.OrderId.Value, QbEntityType.PurchaseOrder.ToString(), firstInvoice.InvoiceNumber.Id);
            }
            else
            {
                invoiceFees = invoiceDomain.GetPreviousInvoiceFees(requestParameters.InvoiceId, requestParameters.InvoiceNumberId);
                qbInvoiceViewModel = invoicePdfViewModel.ToInvoicePoViewModel(invoiceFees.Item1, invoiceFees.Item2);
            }
            qbInvoiceViewModel.ClassName = requestParameters.ClassName;
            qbInvoiceViewModel.IsPOAlreadyExist = isPrimaryOrder;
            qbInvoiceViewModel.QbPurchaseOrderTxnID = qbDomain.GetOrderTxnID(workflow.QbCompanyProfileId, firstInvoice.OrderId ?? 0, QbEntityType.PurchaseOrder.ToString());
            if (firstInvoice.IsRebillInvoice)
            {
                qbInvoiceViewModel.QbBillTxnID = qbDomain.GetBillTxnID(workflow.QbCompanyProfileId, qbInvoiceViewModel.OrderId, qbInvoiceViewModel.OriginalInvoiceNumberId.Value, QbEntityType.Bill.ToString());
                qbInvoiceViewModel.Memo = firstInvoice.OriginalInvoiceNumber + "/" + "{:OriginalBillQbNumber:}";
            }
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
            qbInvoiceViewModel.ParentOrderId = requestParameters.ParentOrderId;
            qbInvoiceViewModel.IsBrokeredOrder = !string.IsNullOrWhiteSpace(requestParameters.InvoiceBrokeredChainId);

            var invoicePoWorkflow = new InvoicePoAddWorkflow();
            var workflowResult = invoicePoWorkflow.ExecuteWorkflow(qbInvoiceViewModel);
            foreach (var response in workflowResult.AdapterResponses)
            {
                var request = new QbRequestViewModel
                {
                    WorkflowId = workflow.Id,
                    EntityId = response.EntityId,
                    QbXmlRq = response.QbXml,
                    Status = firstInvoice.IsRebillInvoice  && response.QbXmlType == QbXmlType.PurchaseOrderAdd ? (QbRequestStatus)QbXmlStatus.NotReadyToQueue: (QbRequestStatus)response.Status,
                    QbXmlType = (int)response.QbXmlType,
                    OrderId = requestParameters.OrderId,
                    EntityType = response.EntityType,
                    InvoiceNumberId = qbInvoiceViewModel.IsRebillInvoice && response.QbXmlType == QbXmlType.BillQuery ? qbInvoiceViewModel.OriginalInvoiceNumberId : qbInvoiceViewModel.InvoiceNumberId,
                    PoNumber = firstInvoice.PoNumber
                };
                qbRequests.Add(request);
            }

            return qbRequests;
        }
    }
}
