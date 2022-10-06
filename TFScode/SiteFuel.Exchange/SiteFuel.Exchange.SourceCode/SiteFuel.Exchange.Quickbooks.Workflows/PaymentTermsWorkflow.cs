using SiteFuel.Exchange.Quickbooks.Workflows.Adapters.RequestAdapters;
using SiteFuel.Exchange.Quickbooks.Workflows.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Quickbooks.Workflows
{
    public class PaymentTermsWorkflow
    {
        public WorkflowType Type => WorkflowType.PaymentTerms;

        public List<string> SupportedVersions { get; set; }

        public WorkflowResult ExecuteWorkflow(PaymentTermsViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            var result = new WorkflowResult();

            var termsQueryAdapter = new TermsQueryRequestAdapter();
            var termsQueryViewModel = new TermsQueryViewModel { CompanyId = viewModel.QbCompanyProfileId };
            var termsQueryXml = termsQueryAdapter.GetQbXml(termsQueryViewModel);
            result.AdapterResponses.Add(termsQueryXml);
            result.Errors.AddRange(termsQueryXml.ProgressMessages);

            return result;
        }
    }
}
