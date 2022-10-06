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
    public class VendorCreditAddWorkflow : IQuickbooksWorkflow<PurchaseOrderViewModel>
    {
        public WorkflowType Type => WorkflowType.VendorCreditAdd;

        public List<string> SupportedVersions { get; set; }

        public WorkflowResult ExecuteWorkflow(PurchaseOrderViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }
            var result = new WorkflowResult();
            CreateVendorAddRequest(viewModel, result);
            CreateItemAddRequest(viewModel, result);
            CreateDiscountAddRequest(viewModel, result);
            CreateBillQueryRequest(viewModel, result);
            CreateVendorCreditAddRequest(viewModel, result);

            return result;
        }

        private static void CreateVendorCreditAddRequest(PurchaseOrderViewModel viewModel, WorkflowResult result)
        {
            var vendorCreditAddAdapter = new VendorCreditAddRequestAdapter();
            var vendorCreditAddXml = vendorCreditAddAdapter.GetQbXml(viewModel);
            result.AdapterResponses.Add(vendorCreditAddXml);
            result.Errors.AddRange(vendorCreditAddXml.ProgressMessages);
        }

        private static void CreateItemAddRequest(PurchaseOrderViewModel viewModel, WorkflowResult result)
        {
            var itemServiceAdapter = new ItemServiceAddRequestAdapter();
            foreach (var item in viewModel.Items)
            {
                var itemXml = itemServiceAdapter.GetQbXml(item);
                result.AdapterResponses.Add(itemXml);
                result.Errors.AddRange(itemXml.ProgressMessages);
            }
        }

        private static void CreateBillQueryRequest(PurchaseOrderViewModel viewModel, WorkflowResult result)
        {
            var billQueryAdapter = new BillQueryRequestAdapter();
            QbBillViewModel qbBillViewModel = new QbBillViewModel() { OrderId = viewModel.OrderId, QbBillTxnID = viewModel.QbBillTxnID  };
            var billQueryXml = billQueryAdapter.GetQbXml(qbBillViewModel);
            result.AdapterResponses.Add(billQueryXml);
            result.Errors.AddRange(billQueryXml.ProgressMessages);
        }

        private static void CreateDiscountAddRequest(PurchaseOrderViewModel viewModel, WorkflowResult result)
        {
            var itemDiscountAdapter = new ItemDiscountAddRequestAdapter();
            foreach (var item in viewModel.DiscountItems)
            {
                var itemXml = itemDiscountAdapter.GetQbXml(item);
                result.AdapterResponses.Add(itemXml);
                result.Errors.AddRange(itemXml.ProgressMessages);
            }
        }

        private static void CreateVendorAddRequest(PurchaseOrderViewModel viewModel, WorkflowResult result)
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
    }
}
