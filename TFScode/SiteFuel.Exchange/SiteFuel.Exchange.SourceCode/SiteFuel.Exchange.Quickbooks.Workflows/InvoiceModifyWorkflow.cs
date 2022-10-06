using SiteFuel.Exchange.Core.Resolver;
using SiteFuel.Exchange.Quickbooks.SharedEnums;
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
    public class InvoiceModifyWorkflow : IQuickbooksWorkflow<QbInvoiceViewModel>
    {
        public WorkflowType Type => WorkflowType.InvoiceModify;

        public List<string> SupportedVersions { get; set; }

        public WorkflowResult ExecuteWorkflow(QbInvoiceViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }
            var result = new WorkflowResult();
            CreateItemAddRequest(viewModel, result);
            CreateDiscountAddRequest(viewModel, result);
            CreateTermAddRequest(viewModel, result);
            CreateInvoiceQueryRequest(viewModel, result);
            CreateSalesOrderQueryRequest(viewModel, result);
            CreateSalesOrderModifyRequest(viewModel, result);
            CreateInvoiceModifyRequest(viewModel, result);

            return result;
        }

        private static void CreateInvoiceModifyRequest(QbInvoiceViewModel viewModel, WorkflowResult result)
        {
            var invoiceModAdapter = new InvoiceModifyRequestAdapter();
            var invoiceModXml = invoiceModAdapter.GetQbXml(viewModel);
            AddResponseToResult(result, invoiceModXml);
        }

        private static void CreateInvoiceQueryRequest(QbInvoiceViewModel viewModel, WorkflowResult result)
        {
            var invoiceQueryAdapter = new InvoiceQueryRequestAdapter();
            var invoiceQueryXml = invoiceQueryAdapter.GetQbXml(viewModel);
            AddResponseToResult(result, invoiceQueryXml);
        }

        private static void CreateSalesOrderModifyRequest(QbInvoiceViewModel viewModel, WorkflowResult result)
        {
            var salesOrderModifyAdapter = new SalesOrderModifyRequestAdapter();
            var salesOrderModifyXml = salesOrderModifyAdapter.GetQbXml(viewModel);
            AddResponseToResult(result, salesOrderModifyXml);
        }

        private static void CreateSalesOrderQueryRequest(QbInvoiceViewModel viewModel, WorkflowResult result)
        {
            var queryViewModel = new SalesOrderQueryViewModel
            {
                OrderId = viewModel.OrderId
            };
            var salesOrderQueryAdapter = new SalesOrderQueryRequestAdapter();
            var salesOrderQueryXml = salesOrderQueryAdapter.GetQbXml(queryViewModel);
            salesOrderQueryXml.Status = QbXmlStatus.NotReadyToQueue;
            AddResponseToResult(result, salesOrderQueryXml);
        }

        private static void CreateItemAddRequest(QbInvoiceViewModel viewModel, WorkflowResult result)
        {
            var itemServiceAdapter = new ItemServiceAddRequestAdapter();
            foreach (var item in viewModel.Items)
            {
                var itemXml = itemServiceAdapter.GetQbXml(item);
                AddResponseToResult(result, itemXml);
            }
        }

        private static void CreateDiscountAddRequest(QbInvoiceViewModel viewModel, WorkflowResult result)
        {
            var itemDiscountAdapter = new ItemDiscountAddRequestAdapter();
            foreach (var item in viewModel.DiscountItems)
            {
                var itemXml = itemDiscountAdapter.GetQbXml(item);
                AddResponseToResult(result, itemXml);
            }
        }

        private static void CreateTermAddRequest(QbInvoiceViewModel viewModel, WorkflowResult result)
        {
            var stdTermAddRequestAdapter = new StandardTermAddRequestAdapter();
            var itemXml = stdTermAddRequestAdapter.GetQbXml(viewModel);
            AddResponseToResult(result, itemXml);
        }

        private static void AddResponseToResult(WorkflowResult result, AdapterResponse responseXml)
        {
            result.AdapterResponses.Add(responseXml);
            result.Errors.AddRange(responseXml.ProgressMessages);
        }
    }
}
