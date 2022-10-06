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
    public class InvoicePoModifyWorkflow : IQuickbooksWorkflow<PurchaseOrderViewModel>
    {
        public WorkflowType Type => WorkflowType.InvoicePoMod;

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

            var purchaseOrderQueryAdapter = new PurchaseOrderQueryRequestAdapter();
            var purchaseOrderQueryXml = purchaseOrderQueryAdapter.GetQbXml(viewModel);
            result.AdapterResponses.Add(purchaseOrderQueryXml);
            result.Errors.AddRange(purchaseOrderQueryXml.ProgressMessages);

            var purchaseOrderModifyAdapter = new PurchaseOrderModifyRequestAdapter();
            var purchaseOrderModifyXml = purchaseOrderModifyAdapter.GetQbXml(viewModel);
            result.AdapterResponses.Add(purchaseOrderModifyXml);
            result.Errors.AddRange(purchaseOrderModifyXml.ProgressMessages);

            return result;
        }
    }
}
