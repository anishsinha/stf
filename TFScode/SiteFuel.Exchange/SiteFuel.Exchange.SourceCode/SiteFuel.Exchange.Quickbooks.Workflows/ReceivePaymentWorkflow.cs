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
    public class ReceivePaymentWorkflow : IQuickbooksWorkflow<PaymentViewModel>
    {
        public WorkflowType Type => WorkflowType.ReceivePayment;

        public List<string> SupportedVersions { get; set; }

        public WorkflowResult ExecuteWorkflow(PaymentViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }
            var result = new WorkflowResult();
            CreateReceivePaymentWorkflowAddRequest(viewModel, result);

            return result;
        }

        private static void CreateReceivePaymentWorkflowAddRequest(PaymentViewModel viewModel, WorkflowResult result)
        {
            var receivePaymentQueryAdapter = new ReceivePaymentQueryRequestAdapter();
            var receivePaymentQueryAddXml = receivePaymentQueryAdapter.GetQbXml(viewModel);
            result.AdapterResponses.Add(receivePaymentQueryAddXml);
            result.Errors.AddRange(receivePaymentQueryAddXml.ProgressMessages);
        }
    }
}
