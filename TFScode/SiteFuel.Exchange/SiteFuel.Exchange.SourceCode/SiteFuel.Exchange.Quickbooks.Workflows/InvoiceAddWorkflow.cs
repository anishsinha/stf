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
    public class InvoiceAddWorkflow : IQuickbooksWorkflow<QbInvoiceViewModel>
    {
        public WorkflowType Type => WorkflowType.InvoiceAdd;

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
            CreateTermAddRequest(viewModel, result);
            if (viewModel.IsRebillInvoice)
            {
                CreateInvoiceQueryRequest(viewModel, result);
            }
            if (viewModel.IsPrimaryOrder)
            {
                CreateSalesOrderQueryRequest(viewModel, result);
                CreateSalesOrderModifyRequest(viewModel, result);
            }
            else
            {
                CreateSalesOrderAddRequest(viewModel, result);
            }
            CreateInvoiceAddRequest(viewModel, result);

            return result;
        }

        private static void CreateInvoiceAddRequest(QbInvoiceViewModel viewModel, WorkflowResult result)
        {
            var invoiceAddAdapter = new InvoiceAddRequestAdapter();
            var invoiceAddXml = invoiceAddAdapter.GetQbXml(viewModel);
            result.AdapterResponses.Add(invoiceAddXml);
            result.Errors.AddRange(invoiceAddXml.ProgressMessages);
        }

        private static void CreateSalesOrderAddRequest(QbInvoiceViewModel viewModel, WorkflowResult result)
        {
            var salesOrderAdapter = new SalesOrderAddRequestAdapter();
            var salesOrderXml = salesOrderAdapter.GetQbXml(viewModel);
            result.AdapterResponses.Add(salesOrderXml);
            result.Errors.AddRange(salesOrderXml.ProgressMessages);
        }

        private static void CreateSalesOrderModifyRequest(QbInvoiceViewModel viewModel, WorkflowResult result)
        {
            var salesOrderModifyAdapter = new SalesOrderModifyRequestAdapter();
            var salesOrderModifyXml = salesOrderModifyAdapter.GetQbXml(viewModel);
            result.AdapterResponses.Add(salesOrderModifyXml);
            result.Errors.AddRange(salesOrderModifyXml.ProgressMessages);
        }

        private static void CreateSalesOrderQueryRequest(QbInvoiceViewModel viewModel, WorkflowResult result)
        {
            var queryViewModel = new SalesOrderQueryViewModel
            {
                QbSalesOrderTxnID = viewModel.QbSalesOrderTxnID,
                OrderId = viewModel.OrderId
            };
            var salesOrderQueryAdapter = new SalesOrderQueryRequestAdapter();
            var salesOrderQueryXml = salesOrderQueryAdapter.GetQbXml(queryViewModel);
            result.AdapterResponses.Add(salesOrderQueryXml);
            result.Errors.AddRange(salesOrderQueryXml.ProgressMessages);
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

        private static void CreateTermAddRequest(QbInvoiceViewModel viewModel, WorkflowResult result)
        {
            var stdTermAddRequestAdapter = new StandardTermAddRequestAdapter();
            var itemXml = stdTermAddRequestAdapter.GetQbXml(viewModel);
            result.AdapterResponses.Add(itemXml);
            result.Errors.AddRange(itemXml.ProgressMessages);
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

        private static void CreateInvoiceQueryRequest(QbInvoiceViewModel viewModel, WorkflowResult result)
        {
            var invoiceQueryAdapter = new InvoiceQueryRequestAdapter();
            QbInvoiceViewModel qbInvoiceViewModel = new QbInvoiceViewModel() { QbInvoiceTxnID = viewModel.QbInvoiceTxnID, InvoiceNumberId = viewModel.OriginalInvoiceNumberId.Value };
            var invoiceQueryXml = invoiceQueryAdapter.GetQbXml(qbInvoiceViewModel);
            result.AdapterResponses.Add(invoiceQueryXml);
            result.Errors.AddRange(invoiceQueryXml.ProgressMessages);
        }
    }
}
