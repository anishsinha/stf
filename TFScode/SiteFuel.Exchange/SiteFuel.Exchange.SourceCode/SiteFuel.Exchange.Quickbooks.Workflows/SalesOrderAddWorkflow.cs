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
    public class SalesOrderAddWorkflow : IQuickbooksWorkflow<SalesOrderViewModel>
    {
        public WorkflowType Type => WorkflowType.SaleOrder;

        public List<string> SupportedVersions { get; set; }

        public WorkflowResult ExecuteWorkflow(SalesOrderViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            var result = new WorkflowResult();
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

            var itemServiceAdapter = new ItemServiceAddRequestAdapter();
            foreach (var item in viewModel.Items)
            {
                var itemXml = itemServiceAdapter.GetQbXml(item);
                result.AdapterResponses.Add(itemXml);
                result.Errors.AddRange(itemXml.ProgressMessages);
            }

            var salesOrderAdapter = new SalesOrderAddRequestAdapter();
            var salesOrderXml = salesOrderAdapter.GetQbXml(viewModel);
            result.AdapterResponses.Add(salesOrderXml);
            result.Errors.AddRange(salesOrderXml.ProgressMessages);

            return result;
        }
    }
}
