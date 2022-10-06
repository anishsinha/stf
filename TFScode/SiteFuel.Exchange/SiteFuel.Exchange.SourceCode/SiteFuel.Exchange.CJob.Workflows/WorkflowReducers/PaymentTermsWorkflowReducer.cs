using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Quickbooks.SharedEnums;
using SiteFuel.Exchange.Quickbooks.Workflows;
using SiteFuel.Exchange.Quickbooks.Workflows.Models;
using SiteFuel.Exchange.ViewModels.AccountingEvent;
using SiteFuel.Exchange.ViewModels.Quickbooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.CJob.Workflows.WorkflowReducers
{
    public class PaymentTermsWorkflowReducer : IQuickbooksWorkflowReducer
    {
        public AccountingWorkflowType WorkflowType => AccountingWorkflowType.PaymentTerms;

        public List<QbRequestViewModel> Reduce(AccountingWorkflowViewModel workflow, BaseDomain baseDomain)
        {
            var qbRequests = new List<QbRequestViewModel>();
            var qbWorkflowDomain = new QbWorkflowDomain(baseDomain);
            var requestParameters = qbWorkflowDomain.GetRequestParameters(workflow);

            var paymentTermsViewModel = new PaymentTermsViewModel() { QbCompanyProfileId = requestParameters.CompanyId };
            var paymentTermsWorkflow = new PaymentTermsWorkflow();
            var workflowResult = paymentTermsWorkflow.ExecuteWorkflow(paymentTermsViewModel);
            foreach (var response in workflowResult.AdapterResponses)
            {
                var request = new QbRequestViewModel
                {
                    WorkflowId = workflow.Id,
                    EntityId = response.EntityId,
                    QbXmlRq = response.QbXml,
                    Status = (QbRequestStatus)response.Status,
                    QbXmlType = (int)response.QbXmlType,
                    OrderId = requestParameters.OrderId,
                    EntityType = response.EntityType
                };
                qbRequests.Add(request);
            }

            return qbRequests;
        }
    }
}
