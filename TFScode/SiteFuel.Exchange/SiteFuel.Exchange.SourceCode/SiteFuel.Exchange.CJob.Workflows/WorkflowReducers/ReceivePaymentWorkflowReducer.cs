using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Quickbooks.SharedEnums;
using SiteFuel.Exchange.Quickbooks.Workflows;
using SiteFuel.Exchange.Quickbooks.Workflows.Models;
using SiteFuel.Exchange.ViewModels.AccountingEvent;
using SiteFuel.Exchange.ViewModels.Quickbooks;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.CJob.Workflows.WorkflowReducers
{
    public class ReceivePaymentWorkflowReducer : IQuickbooksWorkflowReducer
    {
        public AccountingWorkflowType WorkflowType => AccountingWorkflowType.ReceivePayment;

        public List<QbRequestViewModel> Reduce(AccountingWorkflowViewModel workflow, BaseDomain baseDomain)
        {
            var qbRequests = new List<QbRequestViewModel>();
            var qbWorkflowDomain = new QbWorkflowDomain(baseDomain);
            var requestParameters = qbWorkflowDomain.GetRequestParameters(workflow);
            var workflowResult = ExecuteAndGetReceivePaymentWorkflowResult(requestParameters);
            SetQbRequestViewModel(workflow, qbRequests, workflowResult);

            return qbRequests;
        }

        private static void SetQbRequestViewModel(AccountingWorkflowViewModel workflow, List<QbRequestViewModel> qbRequests, WorkflowResult workflowResult)
        {
            foreach (var response in workflowResult.AdapterResponses)
            {
                var request = new QbRequestViewModel
                {
                    WorkflowId = workflow.Id,
                    EntityId = response.EntityId,
                    QbXmlRq = response.QbXml,
                    Status = (QbRequestStatus)response.Status,
                    QbXmlType = (int)response.QbXmlType,
                    EntityType = response.EntityType,
                };
                qbRequests.Add(request);
            }
        }

        private static WorkflowResult ExecuteAndGetReceivePaymentWorkflowResult(RequestParameters requestParameters)
        {
            var receivePaymentWorkflow = new ReceivePaymentWorkflow();
            var paymentViewModel = new PaymentViewModel()
            {
                FromModifiedDate = requestParameters.FromModifiedDate != null && requestParameters.FromModifiedDate.HasValue ? requestParameters.FromModifiedDate.Value : DateTimeOffset.Now,
                ToModifiedDate = requestParameters.ToModifiedDate != null && requestParameters.ToModifiedDate.HasValue ? requestParameters.ToModifiedDate.Value : DateTimeOffset.Now,
            };
            var workflowResult = receivePaymentWorkflow.ExecuteWorkflow(paymentViewModel);
            return workflowResult;
        }
    }
}
