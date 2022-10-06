using SiteFuel.Exchange.CJob.Workflows.Mappers;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Quickbooks.Models;
using SiteFuel.Exchange.Quickbooks.SharedEnums;
using SiteFuel.Exchange.Quickbooks.Workflows;
using SiteFuel.Exchange.ViewModels.AccountingEvent;
using SiteFuel.Exchange.ViewModels.Quickbooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.CJob.Workflows.WorkflowReducers
{
    public class PurchaseOrderWorkflowReducer : IQuickbooksWorkflowReducer
    {
        public AccountingWorkflowType WorkflowType => AccountingWorkflowType.PurchaseOrder;

        public List<QbRequestViewModel> Reduce(AccountingWorkflowViewModel workflow, BaseDomain baseDomain)
        {
            var qbRequests = new List<QbRequestViewModel>();
            var qbWorkflowDomain = new QbWorkflowDomain(baseDomain);
            var requestParameters = qbWorkflowDomain.GetRequestParameters(workflow);

            var orderDomain = new OrderDomain(baseDomain);
            var orderPoViewModel = Task.Run(() => orderDomain.GetOrderPoAsync(requestParameters.OrderId)).Result;
            var purchaseOrderViewModel = orderPoViewModel.ToPurchaseOrderViewModel();
            purchaseOrderViewModel.ClassName = requestParameters.ClassName;
            purchaseOrderViewModel.Items.ForEach(t =>
            {
                t.Prefix = workflow.QbCompanyProfile.ItemPrefix;
                t.AccountName = workflow.QbCompanyProfile.ExpenseAccountName;
            });

            purchaseOrderViewModel.ParentOrderId = requestParameters.ParentOrderId;
            purchaseOrderViewModel.IsBrokeredOrder = !string.IsNullOrWhiteSpace(requestParameters.InvoiceBrokeredChainId);

            var purchaseOrderWorkflow = new PurchaseOrderAddWorkflow();
            var workflowResult = purchaseOrderWorkflow.ExecuteWorkflow(purchaseOrderViewModel);
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
