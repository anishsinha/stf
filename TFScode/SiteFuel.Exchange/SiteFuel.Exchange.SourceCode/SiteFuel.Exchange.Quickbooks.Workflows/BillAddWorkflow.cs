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
    public class BillAddWorkflow : IQuickbooksWorkflow<QbBillViewModel>
    {
        public WorkflowType Type => WorkflowType.BillAdd;

        public List<string> SupportedVersions { get; set; }

        public WorkflowResult ExecuteWorkflow(QbBillViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            var result = new WorkflowResult();
            CreateVendorAddRequest(viewModel, result);
            CreateItemAddRequest(viewModel, result);
            CreateTermAddRequest(viewModel, result);
            CreatePurchaseOrderQueryRequest(viewModel, result);
            if(viewModel.IsRebillInvoice)
            {
                CreateBillQueryRequest(viewModel, result);
            }
            var billAdapter = new BillAddRequestAdapter();
            var billXml = billAdapter.GetQbXml(viewModel);
            result.AdapterResponses.Add(billXml);
            result.Errors.AddRange(billXml.ProgressMessages);

            return result;
        }

        private static void CreateVendorAddRequest(QbBillViewModel viewModel, WorkflowResult result)
        {
            var vendorAddAdapter = new VendorAddRequestAdapter();
            PurchaseOrderViewModel vendorViewModel = new PurchaseOrderViewModel()
            {
                VendorName = viewModel.VendorName,
                VendorCompanyName = viewModel.VendorCompanyName,
                VendorAddress = viewModel.VendorAddress,
                VendorId = viewModel.VendorId
            };

            var vendorXml = vendorAddAdapter.GetQbXml(vendorViewModel);
            result.AdapterResponses.Add(vendorXml);
            result.Errors.AddRange(vendorXml.ProgressMessages);
        }

        private static void CreateItemAddRequest(QbBillViewModel viewModel, WorkflowResult result)
        {
            var itemServiceAdapter = new ItemServiceAddRequestAdapter();
            foreach (var item in viewModel.Items)
            {
                var itemXml = itemServiceAdapter.GetQbXml(item);
                result.AdapterResponses.Add(itemXml);
                result.Errors.AddRange(itemXml.ProgressMessages);
            }
        }

        private static void CreateTermAddRequest(QbBillViewModel viewModel, WorkflowResult result)
        {
            var stdTermAddRequestAdapter = new StandardTermAddRequestAdapter();
            QbInvoiceViewModel qbInvoiceViewModel = new QbInvoiceViewModel()
            {
                PaymentTermName = viewModel.PaymentTermName,
                PaymentTermDays = viewModel.PaymentTermDays,
                //PaymentTermDiscountDays = viewModel.PaymentTermDiscountDays,
                //PaymentTermDiscountPct = viewModel.PaymentTermDiscountPct
            };
            var termXml = stdTermAddRequestAdapter.GetQbXml(qbInvoiceViewModel);
            result.AdapterResponses.Add(termXml);
            result.Errors.AddRange(termXml.ProgressMessages);
        }

        private static void CreatePurchaseOrderQueryRequest(QbBillViewModel viewModel, WorkflowResult result)
        {
            var queryViewModel = new PurchaseOrderViewModel
            {
                QbPurchaseOrderTxnID = viewModel.QbPurchaseOrderTxnID,
                OrderId = viewModel.OrderId
            };
            var purchaseOrderQueryAdapter = new PurchaseOrderQueryRequestAdapter();
            var purchaseOrderQueryXml = purchaseOrderQueryAdapter.GetQbXml(queryViewModel);
            result.AdapterResponses.Add(purchaseOrderQueryXml);
            result.Errors.AddRange(purchaseOrderQueryXml.ProgressMessages);
        }

        private static void CreateBillQueryRequest(QbBillViewModel viewModel, WorkflowResult result)
        {
            QbBillViewModel qbBill = new QbBillViewModel() { OrderId = viewModel.OrderId, QbBillTxnID = viewModel.QbBillTxnID };
            var billQueryAdapter = new BillQueryRequestAdapter();
            var billQueryXml = billQueryAdapter.GetQbXml(qbBill);
            result.AdapterResponses.Add(billQueryXml);
            result.Errors.AddRange(billQueryXml.ProgressMessages);
        }
    }
}
