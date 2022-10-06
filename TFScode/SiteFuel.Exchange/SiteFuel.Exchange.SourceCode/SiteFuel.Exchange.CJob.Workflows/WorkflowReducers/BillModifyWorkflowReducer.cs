using SiteFuel.Exchange.CJob.Workflows.Mappers;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Quickbooks.Models;
using SiteFuel.Exchange.Quickbooks.SharedEnums;
using SiteFuel.Exchange.Quickbooks.Workflows;
using SiteFuel.Exchange.Quickbooks.Workflows.Models;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.AccountingEvent;
using SiteFuel.Exchange.ViewModels.Quickbooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.CJob.Workflows.WorkflowReducers
{
    public class BillModWorkflowReducer : IQuickbooksWorkflowReducer
    {
        public AccountingWorkflowType WorkflowType => AccountingWorkflowType.BillModify;

        public List<QbRequestViewModel> Reduce(AccountingWorkflowViewModel workflow, BaseDomain baseDomain)
        {
            var qbRequests = new List<QbRequestViewModel>();
            var qbWorkflowDomain = new QbWorkflowDomain(baseDomain);
            var requestParameters = qbWorkflowDomain.GetRequestParameters(workflow);
            var rqStatus = QbRequestStatus.Unknown;
            var qbDomain = new QbDomain(baseDomain);
            var invoiceDomain = new InvoiceDomain(baseDomain);
            var invoiceFees = invoiceDomain.GetPreviousInvoiceFees(requestParameters.InvoiceId, requestParameters.InvoiceNumberId);
            var invoiceViewModel = Task.Run(() => invoiceDomain.GetConsolidatedBillDetailFromInvoiceIdAsync(requestParameters.InvoiceId)).Result;
            var billViewModel = invoiceViewModel.ToBillViewModel(invoiceFees.Item1, invoiceFees.Item2);
            billViewModel.ClassName = requestParameters.ClassName;
            if (requestParameters.InvoiceCreationDate != null && requestParameters.InvoiceCreationDate < ApplicationConstants.BillModifySyncStartDate)
            {
                rqStatus = QbRequestStatus.SyncSkip;
            }

            billViewModel.QbBillTxnID = qbDomain.GetBillTxnID(workflow.QbCompanyProfileId, billViewModel.OrderId, billViewModel.InvoiceNumberId, QbEntityType.Bill.ToString());
            billViewModel.Items.ForEach(t =>
            {
                t.Prefix = workflow.QbCompanyProfile.ItemPrefix;
                t.AccountName = workflow.QbCompanyProfile.ExpenseAccountName;
            });
            billViewModel.DiscountItems.ForEach(t =>
            {
                t.Prefix = workflow.QbCompanyProfile.ItemPrefix;
                t.AccountName = workflow.QbCompanyProfile.DiscountAccountName ?? workflow.QbCompanyProfile.ExpenseAccountName;
            });

            var billWorkflow = new BillModifyWorkflow();
            var workflowResult = billWorkflow.ExecuteWorkflow(billViewModel);
            foreach (var response in workflowResult.AdapterResponses)
            {
                var request = new QbRequestViewModel
                {
                    WorkflowId = workflow.Id,
                    EntityId = response.EntityId,
                    QbXmlRq = response.QbXml,
                    Status = rqStatus == QbRequestStatus.SyncSkip ? QbRequestStatus.SyncSkip : (QbRequestStatus)response.Status,
                    QbXmlType = (int)response.QbXmlType,
                    OrderId = requestParameters.OrderId,
                    InvoiceNumberId = requestParameters.InvoiceNumberId,
                    EntityType = response.EntityType,
                    PoNumber = billViewModel.PoNumber
                };
                qbRequests.Add(request);
            }

            return qbRequests;
        }
    }
}
