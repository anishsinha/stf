using SiteFuel.Exchange.CJob.Workflows.Mappers;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Quickbooks.SharedEnums;
using SiteFuel.Exchange.Quickbooks.Workflows;
using SiteFuel.Exchange.ViewModels.AccountingEvent;
using SiteFuel.Exchange.ViewModels.Quickbooks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.CJob.Workflows.WorkflowReducers
{
    public class SalesOrderWorkflowReducer : IQuickbooksWorkflowReducer
    {
        public AccountingWorkflowType WorkflowType => AccountingWorkflowType.SaleOrder;

        public List<QbRequestViewModel> Reduce(AccountingWorkflowViewModel workflow, BaseDomain baseDomain)
        {
            var qbRequests = new List<QbRequestViewModel>();
            var qbWorkflowDomain = new QbWorkflowDomain(baseDomain);
            var requestParameters = qbWorkflowDomain.GetRequestParameters(workflow);

            var orderDomain = new OrderDomain(baseDomain);
            var orderPoViewModel = Task.Run(() => orderDomain.GetOrderPoAsync(requestParameters.OrderId)).Result;
            var salesOrderViewModel = orderPoViewModel.ToSalesOrderViewModel();
            salesOrderViewModel.Items.ForEach(t => t.AccountName = workflow.QbCompanyProfile.IncomeAccountName);

            var salesOrderWorkflow = new SalesOrderAddWorkflow();
            var workflowResult = salesOrderWorkflow.ExecuteWorkflow(salesOrderViewModel);
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
                    EntityType = response.EntityType,
                    PoNumber = orderPoViewModel.PoNumber
                };
                qbRequests.Add(request);
            }

            return qbRequests;
        }
    }
}
