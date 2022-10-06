using SiteFuel.Exchange.Quickbooks.Workflows.Adapters.RequestAdapters;
using SiteFuel.Exchange.Quickbooks.Workflows.Interfaces;
using SiteFuel.Exchange.Quickbooks.Workflows.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Quickbooks.Workflows
{
    public class InvoicePoAddWorkflow : IQuickbooksWorkflow<PurchaseOrderViewModel>
    {
        public WorkflowType Type => WorkflowType.InvoicePoAdd;

        public List<string> SupportedVersions { get; set; }

        public WorkflowResult ExecuteWorkflow(PurchaseOrderViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            var result = new WorkflowResult();
            var vendorAddAdapter = new VendorAddRequestAdapter();
            var vendorXml = vendorAddAdapter.GetQbXml(viewModel);
            result.AdapterResponses.Add(vendorXml);
            result.Errors.AddRange(vendorXml.ProgressMessages);

            var itemServiceAdapter = new ItemServiceAddRequestAdapter();
            foreach (var item in viewModel.Items)
            {
                var itemXml = itemServiceAdapter.GetQbXml(item);
                result.AdapterResponses.Add(itemXml);
                result.Errors.AddRange(itemXml.ProgressMessages);
            }
            foreach (var item in viewModel.DiscountItems)
            {
                item.Rate = -item.Rate;
                var itemXml = itemServiceAdapter.GetQbXml(item);
                result.AdapterResponses.Add(itemXml);
                result.Errors.AddRange(itemXml.ProgressMessages);
            }

            if (viewModel.IsPOAlreadyExist)
            {
                var purchaseOrderQueryAdapter = new PurchaseOrderQueryRequestAdapter();
                var purchaseOrderQueryXml = purchaseOrderQueryAdapter.GetQbXml(viewModel);
                result.AdapterResponses.Add(purchaseOrderQueryXml);
                result.Errors.AddRange(purchaseOrderQueryXml.ProgressMessages);

                var purchaseOrderModifyAdapter = new PurchaseOrderModifyRequestAdapter();
                var purchaseOrderModifyXml = purchaseOrderModifyAdapter.GetQbXml(viewModel);
                result.AdapterResponses.Add(purchaseOrderModifyXml);
                result.Errors.AddRange(purchaseOrderModifyXml.ProgressMessages);
            }
            else
            {   if(viewModel.IsRebillInvoice)             
                {
                    QbBillViewModel qbBill = new QbBillViewModel() {OrderId = viewModel.OrderId, QbBillTxnID = viewModel.QbBillTxnID };
                    var billQueryAdapter = new BillQueryRequestAdapter();
                    var billQueryXml = billQueryAdapter.GetQbXml(qbBill);
                    result.AdapterResponses.Add(billQueryXml);
                    result.Errors.AddRange(billQueryXml.ProgressMessages);
                }
                var purchaseOrderAdapter = new PurchaseOrderAddRequestAdapter();
                var purchaseOrderAddXml = purchaseOrderAdapter.GetQbXml(viewModel);
                result.AdapterResponses.Add(purchaseOrderAddXml);
                result.Errors.AddRange(purchaseOrderAddXml.ProgressMessages);
            }
            return result;
        }
    }
}
