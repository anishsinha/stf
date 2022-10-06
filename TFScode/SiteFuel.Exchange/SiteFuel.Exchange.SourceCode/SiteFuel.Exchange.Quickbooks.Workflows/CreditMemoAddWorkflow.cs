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
    public class CreditMemoAddWorkflow : IQuickbooksWorkflow<QbInvoiceViewModel>
    {
        public WorkflowType Type => WorkflowType.CreditMemoAdd;

        public List<string> SupportedVersions { get; set; }

        public WorkflowResult ExecuteWorkflow(QbInvoiceViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }
            var result = new WorkflowResult();
            CreateCustomerAddRequest(viewModel, result);
            CreateItemAddRequest(viewModel, result);
            CreateDiscountAddRequest(viewModel, result);
            CreateInvoiceQueryRequest(viewModel, result);
            CreateCreditMemoAddRequest(viewModel, result);

            return result;
        }

        private static void CreateCreditMemoAddRequest(QbInvoiceViewModel viewModel, WorkflowResult result)
        {
            var creditMemoAddAdapter = new CreditMemoAddRequestAdapter();
            var creditMemoAddXml = creditMemoAddAdapter.GetQbXml(viewModel);
            result.AdapterResponses.Add(creditMemoAddXml);
            result.Errors.AddRange(creditMemoAddXml.ProgressMessages);
        }

        private static void CreateItemAddRequest(QbInvoiceViewModel viewModel, WorkflowResult result)
        {
            var itemServiceAdapter = new ItemServiceAddRequestAdapter();
            foreach (var item in viewModel.Items)
            {
                var itemXml = itemServiceAdapter.GetQbXml(item);
                result.AdapterResponses.Add(itemXml);
                result.Errors.AddRange(itemXml.ProgressMessages);
            }
        }

        private static void CreateInvoiceQueryRequest(QbInvoiceViewModel viewModel, WorkflowResult result)
        {
            var invoiceQueryAdapter = new InvoiceQueryRequestAdapter();
            QbInvoiceViewModel qbInvoiceViewModel = new QbInvoiceViewModel() { QbInvoiceTxnID = viewModel.QbInvoiceTxnID, InvoiceNumberId = viewModel.OriginalInvoiceNumberId.Value };
            var invoiceQueryXml = invoiceQueryAdapter.GetQbXml(qbInvoiceViewModel);
            result.AdapterResponses.Add(invoiceQueryXml);
            result.Errors.AddRange(invoiceQueryXml.ProgressMessages);
        }

        private static void CreateDiscountAddRequest(QbInvoiceViewModel viewModel, WorkflowResult result)
        {
            var itemDiscountAdapter = new ItemDiscountAddRequestAdapter();
            foreach (var item in viewModel.DiscountItems)
            {
                var itemXml = itemDiscountAdapter.GetQbXml(item);
                result.AdapterResponses.Add(itemXml);
                result.Errors.AddRange(itemXml.ProgressMessages);
            }
        }

        private static void CreateCustomerAddRequest(QbInvoiceViewModel viewModel, WorkflowResult result)
        {
            var customerViewModel = new CustomerViewModel
            {
                Id = viewModel.CustomerId,
                Name = viewModel.CustomerName,
                CompanyName = viewModel.CustomerCompanyName,
                BillAddress = viewModel.BillAddress,
                ShipAddress = viewModel.ShipAddress
            };
            var customerAddAdapter = new CustomerAddRequestAdapter();
            var customerXml = customerAddAdapter.GetQbXml(customerViewModel);
            result.AdapterResponses.Add(customerXml);
            result.Errors.AddRange(customerXml.ProgressMessages);
        }
    }
}
