using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Resolver;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Quickbooks.Models;
using SiteFuel.Exchange.Quickbooks.SharedEnums;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.AccountingEvent;
using SiteFuel.Exchange.ViewModels.Quickbooks;
using SiteFuel.Exchange.ViewModels.Payments;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using SiteFuel.Exchange.ViewModels.Queue;
using SiteFuel.Exchange.Core.Utilities;
using SiteFuel.Exchange.ViewModels;

namespace SiteFuel.Exchange.Domain
{
    public class QbDomain : BaseDomain
    {
        private readonly Exception emptyException = new Exception();
        public QbDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public QbDomain(BaseDomain domain) : base(domain)
        {
        }

        public AccountingWorkflowViewModel CreateNewWorkflow(AccountingWorkflowViewModel workflow)
        {
            var qbWorkflow = new AccountingWorkflow()
            {
                CreatedOn = DateTimeOffset.Now,
                QbCompanyProfileId = workflow.QbCompanyProfileId,
                Type = (int)workflow.Type,
                UpdatedOn = DateTimeOffset.Now,
                ParameterJson = workflow.ParameterJson,
                SoftwareVersion = workflow.SoftwareVersion,
                Status = (int)workflow.Status
            };

            Context.DataContext.QbWorkflows.Add(qbWorkflow);
            Context.Commit();
            workflow.Id = qbWorkflow.Id;
            return workflow;
        }

        public void CreateQbRequests(List<QbRequestViewModel> qbRequests, int workflowId)
        {
            foreach (var item in qbRequests)
            {
                Context.DataContext.QbRequests.Add(new QbRequest()
                {
                    QbXmlRq = item.QbXmlRq,
                    QbXmlType = item.QbXmlType,
                    Status = (int)item.Status,
                    AccountingWorkflowId = item.WorkflowId,
                    ReadyForQueueOn = DateTimeOffset.Now,
                    CreatedOn = DateTimeOffset.Now,
                    UpdatedOn = DateTimeOffset.Now,
                    EntityId = item.EntityId,
                    EntityType = item.EntityType,
                    PoNumber = item.PoNumber,
                    InvoiceNumberId = item.InvoiceNumberId
                });
            }
            Context.DataContext.QbWorkflows.FirstOrDefault(x => x.Id == workflowId).Status = (int)AccountingWorkflowStatus.Initialized;
            Context.Commit();
        }

        public void CreateQbEntityMappings(List<QbRequestViewModel> qbRequests, int qbProfileId)
        {
            foreach (var item in qbRequests.Where(t => t.EntityId.HasValue && t.EntityId.Value > 0))
            {
                var parentMapping = Context.DataContext.QbEntityMappings.Where(t => t.QbProfileId == qbProfileId
                                    && t.EntityId == (item.EntityId ?? 0) && t.EntityType == item.EntityType
                                    && t.InvoiceNumberId == item.InvoiceNumberId)
                                    .OrderByDescending(t => t.Id).FirstOrDefault();
                Context.DataContext.QbEntityMappings.Add(new QbEntityMapping()
                {
                    QbProfileId = qbProfileId,
                    EntityId = item.EntityId ?? 0,
                    EntityType = item.EntityType,
                    ParentId = parentMapping?.EntityId,
                    InvoiceNumberId = item.InvoiceNumberId,
                    CreatedOn = DateTimeOffset.Now,
                    UpdatedOn = DateTimeOffset.Now
                });
            }
            Context.Commit();
        }

        public void CreateQbInvoiceMappings(List<QbRequestViewModel> qbRequests, int qbProfileId)
        {
            foreach (var item in qbRequests.Where(t => t.OrderId.HasValue && t.OrderId.Value > 0))
            {
                Context.DataContext.QbInvoiceMappings.Add(new QbInvoiceMapping()
                {
                    QbProfileId = qbProfileId,
                    EntityId = item.EntityId ?? 0,
                    EntityType = item.EntityType,
                    OrderId = item.OrderId.Value,
                    CreatedOn = DateTimeOffset.Now,
                    UpdatedOn = DateTimeOffset.Now
                });
            }
            Context.Commit();
        }

        public ViewModels.Quickbooks.QbCompanyProfile ValidateCompanyProfileByTicket(string ticket)
        {
            var companyProfile = Context.DataContext.QbProfiles.FirstOrDefault(x => x.LoginToken == ticket);
            if (companyProfile == null)
            {
                return null;
            }

            return QbCompanyProfileMapper.ToQbProfileModel(companyProfile);
        }

        public void UpdateWorkflowAndRequestStatus(int workflowId, int requestId, QbXmlStatus qbXmlStatus, AccountingWorkflowStatus wfStatus)
        {
            var qbRequest = Context.DataContext.QbRequests.SingleOrDefault(t => t.Id == requestId);
            if (qbRequest != null)
            {
                qbRequest.Status = (int)qbXmlStatus;
                qbRequest.UpdatedOn = DateTimeOffset.Now;
                Context.DataContext.Entry(qbRequest).State = EntityState.Modified;
            }
            SetWorkflowStatus(workflowId, wfStatus);

            Context.DataContext.SaveChanges();
        }

        public void UpdateWorkflowStatus(int workflowId, AccountingWorkflowStatus wfStatus)
        {
            try
            {
                SetWorkflowStatus(workflowId, wfStatus);
                Context.DataContext.SaveChanges();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("QbDomain", "UpdateWorkflowStatus", ex.Message, ex);
            }
        }

        private void SetWorkflowStatus(int workflowId, AccountingWorkflowStatus wfStatus)
        {
            var qbWorkflow = Context.DataContext.QbWorkflows.SingleOrDefault(t => t.Id == workflowId);
            if (qbWorkflow != null)
            {
                qbWorkflow.Status = (int)wfStatus;
                qbWorkflow.UpdatedOn = DateTimeOffset.Now;
                Context.DataContext.Entry(qbWorkflow).State = EntityState.Modified;
            }
        }

        public void UpdateInvoiceNumber(int? entityId, string invoiceNumber)
        {
            var invoice = Context.DataContext.Invoices.FirstOrDefault(t => t.InvoiceHeader.InvoiceNumberId == entityId.Value && t.IsActive && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active);
            if (invoice != null)
            {
                invoice.QbInvoiceNumber = invoiceNumber;
                Context.DataContext.Entry(invoice).State = EntityState.Modified;
                Context.Commit();
            }
        }

        public AccountingWorkflowViewModel UpdateWorkflowWithResponse(int companyProfileId, int workflowId, int requestId, string response)
        {
            var request = Context.DataContext.QbRequests.Join(Context.DataContext.QbWorkflows, x => x.AccountingWorkflowId, y => y.Id,
                (x, y) => new { Request = x, Workflow = y }).
                FirstOrDefault(x => x.Request.Id == requestId && x.Workflow.QbCompanyProfileId == companyProfileId);
            if (request == null)
            {
                return null;
            }
            
            var qbResponse = new QbResponse
            {
                QbRequestId = requestId,
                QbXmlRs = response,
                CreatedOn = DateTimeOffset.Now,
                UpdatedOn = DateTimeOffset.Now
            };
            Context.DataContext.QbResponses.Add(qbResponse);

            Context.Commit();

            AccountingWorkflowViewModel qbWorkflow = ConvertWorkflow(request.Workflow);
            return qbWorkflow;
        }

        public void SaveQbLogs(int entityType, string ticket, string response, string hresult, string message, string jsonMessage = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(jsonMessage))
                {
                    jsonMessage = $"{{TimeStamp:{DateTimeOffset.Now.ToString()}}}";
                }
                var log = new QbLog()
                {
                    EntityType = entityType,
                    Message = message,
                    Response = response,
                    Result = hresult,
                    Ticket = ticket,
                    JsonMessage = jsonMessage
                };

                Context.DataContext.QbLogs.Add(log);
                Context.Commit();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("QbDomain", "SaveQbLogs", ex.Message, ex);
            }
        }

        private static AccountingWorkflowViewModel ConvertWorkflow(AccountingWorkflow workflow, bool loadTerms = false)
        {
            return new AccountingWorkflowViewModel()
            {
                Id = workflow.Id,
                CreatedOn = workflow.CreatedOn,
                QbCompanyProfileId = workflow.QbCompanyProfileId,
                Type = (AccountingWorkflowType)workflow.Type,
                UpdatedOn = workflow.UpdatedOn,
                QbRequests = workflow.QbRequests.Select(x => ConvertQbRequest(x)).ToList(),
                ParameterJson = workflow.ParameterJson,
                Status = (AccountingWorkflowStatus)workflow.Status,
                QbCompanyProfile = workflow.QbCompanyProfile.ToQbProfileModel(loadTerms)
            };
        }

        private static QbRequestViewModel ConvertQbRequest(QbRequest x)
        {
            return new QbRequestViewModel
            {
                Id = x.Id,
                QbXmlRq = x.QbXmlRq,
                QbXmlType = x.QbXmlType,
                Status = (QbRequestStatus)x.Status,
                WorkflowId = x.AccountingWorkflowId,
                EntityId = x.EntityId
            };
        }

        public Tuple<string, string> ValidateQbUsernamePassword(string strUserName, string strPassword)
        {
            var companyProfile = Context.DataContext.QbProfiles.FirstOrDefault(x => x.Username == strUserName);
            if (ValidatePasswordHash(companyProfile.Password, strPassword))
            {
                var previousTicket = companyProfile.LoginToken;
                //TODO connection table need to be maintained to log session
                companyProfile.LoginToken = Guid.NewGuid().ToString();
                Logger.LogManager.Logger.WriteDebug("QbDomain", "ValidateQbUsernamePassword", $"Prev Token - {previousTicket}, Nxt Token- {companyProfile.LoginToken} ");
                companyProfile.LastAccessedOn = DateTimeOffset.Now;
                Context.Commit();
            }

            return Tuple.Create(companyProfile.LoginToken, companyProfile.CompanyFilePath ?? string.Empty);
        }

        private bool ValidatePasswordHash(string password, string strPassword)
        {
            return password == strPassword;
        }

        public void UpdateFailedRequestsByEntityId(string entityId)
        {
            try
            {
                var failedRequestsForEntity = Context.DataContext.QbRequests.Where(x => x.PoNumber == entityId
                                            && x.Status == (int)QbRequestStatus.Failed && x.RetryCount < 4)
                                            .OrderBy(x => x.Id).Select(x => x.Id).ToList();
                foreach (var item in failedRequestsForEntity)
                {
                    RetryQbRequests(item);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("QbDomain", "UpdateFailedRequestsByEntityId", ex.Message, ex);
            }
        }

        public QbRequestViewModel GetNextRequestByTokenAndUpdateStatus(string ticket, List<long> indefinateRqIds = null)
        {
            if (indefinateRqIds == null)
            {
                indefinateRqIds = new List<long>();
            }

            var qbReq = GetNextRequestByToken(ticket, indefinateRqIds);
            if (qbReq != null)
            {
                var template = new TemplateResponse();
                template.AddTemplate("RequestId", $"{qbReq.AccountingWorkflowId}-{qbReq.Id}");
                var rqXml = TemplateResolver.Resolve(qbReq.QbXmlRq, template);
                if (qbReq.Status == (int)QbRequestStatus.MapperDependent)
                {
                    bool isMappingResolved = false;
                    if (qbReq.EntityType == "Invoice")
                    {
                        isMappingResolved = ResolveInvoiceMapping(qbReq);
                    }
                    else
                    {
                        isMappingResolved = ResolveEntityMapping(qbReq);
                    }
                    if (!isMappingResolved && !TemplateResolver.IsXmlComplete(rqXml))
                    {
                        indefinateRqIds.Add(qbReq.Id);
                        return GetNextRequestByTokenAndUpdateStatus(ticket, indefinateRqIds);
                    }
                }
                var req = ConvertQbRequest(qbReq);
                qbReq.Status = (int)QbRequestStatus.Queued;
                qbReq.UpdatedOn = DateTimeOffset.Now;
                Context.Commit();
                if (indefinateRqIds.Any())
                    LogManager.Logger.WriteException("QbDomain", "GetNextRequestByTokenAndUpdateStatus", $"Requests are going in Indefinate stats - {string.Join(",", indefinateRqIds)}", emptyException);
                return req;
            }
            return null;
        }

        private QbRequest GetNextRequestByToken(string ticket, List<long> rqIds, int workflowId = 0)
        {
            var qbReq = (from request in Context.DataContext.QbRequests
                         join workflow in Context.DataContext.QbWorkflows
                         on request.AccountingWorkflowId equals workflow.Id
                         join profile in Context.DataContext.QbProfiles
                         on workflow.QbCompanyProfileId equals profile.CompanyId
                         where (!rqIds.Contains(request.Id)) &&
                         profile.LoginToken == ticket && (workflowId == 0 || workflowId == workflow.Id) &&
                         (request.Status == (int)QbRequestStatus.ReadyToQueue || request.Status == (int)QbRequestStatus.MapperDependent)
                         orderby request.ReadyForQueueOn, request.Id
                         select request).FirstOrDefault();

            return qbReq;
        }

        private bool ResolveEntityMapping(QbRequest qbReq)
        {
            var qbReferenceId = Context.DataContext.QbEntityMappings.Where(x => x.EntityId == qbReq.EntityId
                                                && x.InvoiceNumberId == qbReq.InvoiceNumberId
                                                && x.EntityType == qbReq.EntityType && x.QbReferenceId != null)
                                                .OrderByDescending(t => t.Id).Select(t => t.QbReferenceId).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(qbReferenceId))
            {
                var qbRefId = qbReferenceId;
                var templateResponse = new TemplateResponse();
                templateResponse.Templates.Add($"{qbReq.EntityType}TxnID", qbRefId);
                qbReq.QbXmlRq = TemplateResolver.Resolve(qbReq.QbXmlRq, templateResponse);
            }
            return !string.IsNullOrWhiteSpace(qbReferenceId);
        }

        private bool ResolveInvoiceMapping(QbRequest qbReq)
        {
            var qbReferenceId = Context.DataContext.QbInvoiceMappings.Where(x => x.EntityId == qbReq.EntityId
                                                && x.EntityType == qbReq.EntityType && x.QbReferenceId != null)
                                                .OrderByDescending(t => t.Id).Select(t => t.QbReferenceId).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(qbReferenceId))
            {
                var qbRefId = qbReferenceId;
                var templateResponse = new TemplateResponse();
                templateResponse.Templates.Add($"{qbReq.EntityType}TxnID", qbRefId);
                qbReq.QbXmlRq = TemplateResolver.Resolve(qbReq.QbXmlRq, templateResponse);
            }
            return !string.IsNullOrWhiteSpace(qbReferenceId);
        }

        public void UpdateRequestWithStatus(long id, string requestXml, QbRequestStatus status)
        {
            var request = Context.DataContext.QbRequests.FirstOrDefault(x => x.Id == id);
            request.QbXmlRq = requestXml;
            request.Status = (int)status;
            Context.Commit();
        }

        public void UpdateQbEntityMappingWithInvoiceNumberId(int entityId, string entityType, int invoiceNumberId)
        {
            var entityMapping = Context.DataContext.QbEntityMappings.FirstOrDefault(t =>
                                 t.EntityId == entityId && t.EntityType == entityType && t.InvoiceNumberId == null);
            if (entityMapping != null)
            {
                entityMapping.InvoiceNumberId = invoiceNumberId;
                entityMapping.UpdatedOn = DateTimeOffset.Now;
                Context.Commit();
            }
        }
        public void UpdateQbEntityMapping(long requestId, int workflowId, string qbReferenceId, string editSequence)
        {
            var request = Context.DataContext.QbRequests.AsNoTracking()
                            .Where(x => x.Id == requestId && x.AccountingWorkflowId == workflowId)
                            .Select(t => new
                            {
                                t.EntityId,
                                t.EntityType,
                                t.InvoiceNumberId,
                                t.AccountingWorkflow.QbCompanyProfileId
                            }).FirstOrDefault();
            if (request != null)
            {
                var entityMapping = Context.DataContext.QbEntityMappings.FirstOrDefault(
                                    t => t.QbReferenceId == null && t.QbProfileId == request.QbCompanyProfileId
                                    && t.EntityId == (request.EntityId ?? 0) && t.EntityType == request.EntityType
                                    && (request.InvoiceNumberId == null || t.InvoiceNumberId == request.InvoiceNumberId));
                if (entityMapping != null)
                {
                    entityMapping.QbReferenceId = qbReferenceId;
                    entityMapping.EditSequence = editSequence;
                    entityMapping.UpdatedOn = DateTimeOffset.Now;
                    Context.Commit();
                }
            }
        }

        public void UpdateQbInvoiceMapping(long requestId, int workflowId, string qbReferenceId, string editSequence)
        {
            var request = Context.DataContext.QbRequests.AsNoTracking()
                            .Where(x => x.Id == requestId && x.AccountingWorkflowId == workflowId)
                            .Select(t => new
                            {
                                t.EntityId,
                                t.EntityType,
                                t.InvoiceNumberId,
                                t.AccountingWorkflow.QbCompanyProfileId
                            }).FirstOrDefault();
            if (request != null)
            {
                var invoiceMapping = Context.DataContext.QbInvoiceMappings.FirstOrDefault(
                                    t => t.QbReferenceId == null && t.QbProfileId == request.QbCompanyProfileId
                                    && t.EntityId == (request.EntityId ?? 0) && t.EntityType == request.EntityType);
                if (invoiceMapping != null)
                {
                    invoiceMapping.QbReferenceId = qbReferenceId;
                    invoiceMapping.EditSequence = editSequence;
                    invoiceMapping.UpdatedOn = DateTimeOffset.Now;
                    Context.Commit();
                }
            }
        }

        public List<AccountingWorkflowViewModel> GetNewWorkflowsAndInitialize()
        {
            var workflows = Context.DataContext.QbWorkflows.Include(t => t.QbCompanyProfile).Where(x => x.Status == (int)AccountingWorkflowStatus.Created).ToList();
            workflows.ForEach(x => x.Status = (int)AccountingWorkflowStatus.Initializing);
            Context.Commit();
            return workflows.Select(x => ConvertWorkflow(x, true)).ToList();
        }

        public int GetQbCompanyProfileId(int companyId)
        {
            int response = 0;
            try
            {
                var qbProfile = Context.DataContext.QbProfiles.FirstOrDefault(t => t.CompanyId == companyId && t.IsActive);
                if (qbProfile != null)
                {
                    response = qbProfile.CompanyId;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("QbDomain", "GetQbCompanyProfile", ex.Message, ex);
            }
            return response;
        }

        public ViewModels.Quickbooks.QbCompanyProfile GetQbCompanyProfile(int companyId)
        {
            ViewModels.Quickbooks.QbCompanyProfile response = null;
            try
            {
                var qbProfile = Context.DataContext.QbProfiles.FirstOrDefault(t => t.CompanyId == companyId);
                if (qbProfile != null)
                {
                    response = qbProfile.ToQbProfileModel(true, false);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("QbDomain", "GetQbCompanyProfile", ex.Message, ex);
            }
            return response;
        }

        /// <summary>
        /// Set status of previous requests when we didn't recieve any readable response from QB
        /// and requeue the previous queue request
        /// </summary>
        public void UpdatePreviousRequestInQueueState(string ticket)
        {
            var existingRequest = Context.DataContext.QbRequests.Where(x => x.AccountingWorkflow.QbCompanyProfile.LoginToken == ticket &&
            x.Status == (int)QbRequestStatus.Queued).OrderByDescending(x => x.Id).FirstOrDefault();
            if (existingRequest != null)
            {
                LogManager.Logger.WriteDebug("QbDomain", "UpdatePreviousRequestInQueueState", $"{existingRequest.Id}");

                var xmlType = (QbXmlType)existingRequest.QbXmlType;

                if (xmlType == QbXmlType.InvoiceAdd || xmlType == QbXmlType.PurchaseOrderAdd || xmlType == QbXmlType.SalesOrderAdd)
                {
                    existingRequest.Status = (int)QbRequestStatus.OnFailureDuplicateAttempt;
                }
                else
                {
                    existingRequest.Status = (int)QbRequestStatus.OnFailureRequeued;
                }
                EnqueueExistingRequest(existingRequest);
            }
        }

        public void UpdateFailedRequestAndEnqueue(long requestId, int workflowId)
        {
            var existingRequest = Context.DataContext.QbRequests.Where(x => x.Id == requestId
                                    && x.AccountingWorkflowId == workflowId).FirstOrDefault();
            if (existingRequest != null)
            {
                LogManager.Logger.WriteDebug("QbDomain", "UpdateFailedRequestAndEnqueue", $"{existingRequest.Id}");
                existingRequest.Status = (int)QbRequestStatus.OnFailureRequeued;
                EnqueueExistingRequest(existingRequest);
            }
        }

        private void EnqueueExistingRequest(QbRequest existingRequest)
        {
            if (existingRequest.RetryCount < 5)
            {
                var qbRequestId = $"{existingRequest.AccountingWorkflowId}-{existingRequest.Id}";
                existingRequest.QbXmlRq = existingRequest.QbXmlRq.Replace(qbRequestId, "{:RequestId:}");
                Context.DataContext.QbRequests.Add(new QbRequest()
                {
                    QbXmlRq = existingRequest.QbXmlRq,
                    QbXmlType = existingRequest.QbXmlType,
                    Status = (int)QbXmlStatus.ReadyToQueue,
                    AccountingWorkflowId = existingRequest.AccountingWorkflowId,
                    ReadyForQueueOn = existingRequest.ReadyForQueueOn,
                    CreatedOn = existingRequest.CreatedOn,
                    UpdatedOn = DateTimeOffset.Now,
                    EntityId = existingRequest.EntityId,
                    EntityType = existingRequest.EntityType,
                    RetryCount = existingRequest.RetryCount + 1,
                    PoNumber = existingRequest.PoNumber,
                    InvoiceNumberId = existingRequest.InvoiceNumberId,
                    ParentId = existingRequest.Id
                });
            }
            else
            {
                existingRequest.Status = (int)QbRequestStatus.FailedAndStoppedForRetry;
            }
            Context.Commit();
        }

        public bool IsAnyQbRequestAvailable(string ticket, int workflowId = 0)
        {
            var qbRequest = (from request in Context.DataContext.QbRequests
                             join workflow in Context.DataContext.QbWorkflows
                             on request.AccountingWorkflowId equals workflow.Id
                             join profile in Context.DataContext.QbProfiles
                             on workflow.QbCompanyProfileId equals profile.CompanyId
                             where
                             profile.LoginToken == ticket && (workflowId == 0 || workflowId == workflow.Id) &&
                             (request.Status == (int)QbRequestStatus.ReadyToQueue || request.Status == (int)QbRequestStatus.MapperDependent)
                             orderby request.ReadyForQueueOn, request.Id
                             select request.Id).FirstOrDefault();
            return qbRequest > 0;
        }

        public bool IsAnyNewQbRequestAvailableAfterThisWorkflow(string ticket, int currentWorkflowId)
        {
            var qbRequestId = (from request in Context.DataContext.QbRequests
                               join workflow in Context.DataContext.QbWorkflows
                               on request.AccountingWorkflowId equals workflow.Id
                               join profile in Context.DataContext.QbProfiles
                               on workflow.QbCompanyProfileId equals profile.CompanyId
                               where
                               profile.LoginToken == ticket && (currentWorkflowId < workflow.Id) &&
                               (request.Status == (int)QbRequestStatus.ReadyToQueue || request.Status == (int)QbRequestStatus.MapperDependent)
                               orderby request.ReadyForQueueOn, request.Id
                               select request.Id).FirstOrDefault();
            return qbRequestId > 0;
        }

        public bool IsPrimarySalesOrder(int qbProfileId, int entityId, string entityType)
        {
            var qbMapping = Context.DataContext.QbEntityMappings.Where(t => t.QbProfileId == qbProfileId && t.EntityId == entityId && t.EntityType == entityType)
                            .GroupJoin(Context.DataContext.QbInvoiceMappings, entity => entity.EntityId, invoice => invoice.OrderId,
                            (x, y) => new { entity = x, invoices = y }).Select(t => new { t.entity, t.invoices }).FirstOrDefault();

            // return true if entity mapping is there but no invoice are created, then it is a primary SO
            bool result = qbMapping != null && !qbMapping.invoices.Any();
            return result;
        }

        public bool IsPurchaseOrderAlreadyExist(int qbProfileId, int entityId, string entityType)
        {
            var mappings = Context.DataContext.QbEntityMappings.Where(t => t.QbProfileId == qbProfileId
                             && t.EntityId == entityId && t.EntityType == entityType).ToList();

            if (mappings.Any())
            {
                return mappings.Any(x => x.InvoiceNumberId == null);
            }
            else
            {
                return false;
            }
        }

        public bool IsPurchaseOrderAlreadyExist(int orderId, int invoiceNumberId)
        {
            var IsOtherInvoicesExists = Context.DataContext.Invoices.Any(t => t.OrderId == orderId 
                                        && t.InvoiceHeader.InvoiceNumberId < invoiceNumberId
                                        && 
                                        (   t.InvoiceTypeId == (int)InvoiceType.Manual 
                                            || t.InvoiceTypeId == (int)InvoiceType.MobileApp
                                            || t.InvoiceTypeId == (int)InvoiceType.DryRun
                                            || t.InvoiceTypeId == (int)InvoiceType.Balance
                                            || t.InvoiceTypeId == (int)InvoiceType.TankRental
                                        )
                                        && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active);

            return !IsOtherInvoicesExists;
        }

        public string GetOrderTxnID(int qbProfileId, int entityId, string entityType)
        {
            string txnId = null;
            var qbMapping = Context.DataContext.QbEntityMappings.Where(t => t.QbProfileId == qbProfileId
                            && t.EntityId == entityId && t.EntityType == entityType && t.QbReferenceId != null)
                            .OrderByDescending(t => t.Id).FirstOrDefault();
            if (qbMapping != null)
            {
                txnId = qbMapping.QbReferenceId;
            }
            return txnId;
        }

        public string GetOrderTxnIdToModifyQbPo(int qbProfileId, int entityId, string entityType, int invoiceNumberId)
        {
            string txnId = null;
            var qbMapping = Context.DataContext.QbEntityMappings.Where(t => t.QbProfileId == qbProfileId
                            && t.EntityId == entityId && t.EntityType == entityType && t.QbReferenceId != null
                            && t.InvoiceNumberId != null && t.InvoiceNumberId == invoiceNumberId)
                            .OrderByDescending(t => t.Id).FirstOrDefault();
            if (qbMapping != null)
            {
                txnId = qbMapping.QbReferenceId;
            }
            return txnId;
        }

        public string GetInvoiceTxnID(int qbProfileId, int entityId, string entityType)
        {
            string txnId = null;
            var qbMapping = Context.DataContext.QbInvoiceMappings.Where(t => t.QbProfileId == qbProfileId
                            && t.EntityId == entityId && t.EntityType == entityType && t.QbReferenceId != null)
                            .OrderByDescending(t => t.Id).FirstOrDefault();
            if (qbMapping != null)
            {
                txnId = qbMapping.QbReferenceId;
            }
            return txnId;
        }

        public string GetBillTxnID(int qbProfileId, int entityId, int invoiceNumberId, string entityType)
        {
            string txnId = null;
            var qbMapping = Context.DataContext.QbEntityMappings.Where(t => t.QbProfileId == qbProfileId
                            && t.EntityId == entityId && t.EntityType == entityType && t.QbReferenceId != null
                            && t.InvoiceNumberId == invoiceNumberId)
                            .OrderByDescending(t => t.Id).FirstOrDefault();
            if (qbMapping != null)
            {
                txnId = qbMapping.QbReferenceId;
            }
            return txnId;
        }


        public List<DropdownDisplayItem> GetQbCompanies(int companyId)
        {
            List<DropdownDisplayItem> response = new List<DropdownDisplayItem>();

            if (companyId > 0)
            {
                var company = Context.DataContext.Companies.SingleOrDefault(t => t.Id == companyId);
                if (company != null)
                {
                    response.Add(new DropdownDisplayItem { Id = company.Id, Name = company.Name });
                }
            }
            else
            {
                response = Context.DataContext.Companies
                            .Join(Context.DataContext.QbRequests, company => company.Id, request => request.AccountingWorkflow.QbCompanyProfile.CompanyId,
                            (x, y) => new { company = x })
                            .Select(t => new DropdownDisplayItem() { Id = t.company.Id, Name = t.company.Name }
                            ).Distinct().ToList();
                response.Add(new DropdownDisplayItem { Id = 0, Name = Resource.lblAll });
            }

            return response;
        }

        public void SavePaymentTerms(int companyId, List<ViewModels.Quickbooks.PaymentTerms> terms)
        {
            var dbTerms = Context.DataContext.QbPaymentTerms.
                Where(x => x.QbProfileId == companyId).Select(x => x.TermName).ToList();
            var unSavedTerms = terms.Where(x => !dbTerms.Contains(x.TermName));
            foreach (var item in unSavedTerms)
            {
                Context.DataContext.QbPaymentTerms.Add(
                        new QbPaymentTerm
                        {
                            QbProfileId = companyId,
                            TermName = item.TermName,
                            TermDays = item.TermDays,
                            CreatedDate = DateTimeOffset.Now,
                            UpdatedDate = DateTimeOffset.Now,
                            IsActive = item.TermName.StartsWith(ApplicationConstants.QbTermNet)
                                        || item.TermName.Equals(ApplicationConstants.QbTermDueOnReceipt, StringComparison.OrdinalIgnoreCase)
                        }
                    );
            }
            Context.Commit();
        }

        public async Task<List<USP_QuickBooksSummaryViewModel>> GetQuickBooksSummaryAsync(QbDataTableModel qbModel, int role, DataTableSearchModel searchModel)
        {
            List<USP_QuickBooksSummaryViewModel> response = new List<USP_QuickBooksSummaryViewModel>();
            try
            {
                var spDomain = new StoredProcedureDomain();
                response = await spDomain.GetQbSummary(qbModel.FromDateTime, qbModel.ToDateTime, qbModel.CompanyId, qbModel.AccountingWorkflowId, searchModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("QbDomain", "GetQuickBooksSummaryAsync", ex.Message, ex);
            }
            return response;
        }

        public string GetLastErrorForTicket(string ticket)
        {
            var errorMsg = string.Empty;
            try
            {
                var error = Context.DataContext.QbLogs.Where(x => x.Ticket == ticket && x.EntityType == 2)
                            .OrderByDescending(x => x.Id).FirstOrDefault();
                if (error != null)
                {
                    var xmlErorMsg = GetErrorMessageFromXml(error);
                    if (string.IsNullOrWhiteSpace(xmlErorMsg))
                    {
                        errorMsg = $"{error.Message}. Contact Sfx Support. Error-ticket: {ticket}, Code: {error.Result}";
                    }
                    else
                    {
                        errorMsg = $"{xmlErorMsg}. Error-ticket: {ticket}, Code: {error.Result}";
                    }
                }
                else
                {
                    errorMsg = $"Some error has occurred! Not processing further requests. Contact Sfx Support with logs ticket: {ticket} from View Logs";
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("QbDomain", "GetLastErrorForTicket", ex.Message, ex);
            }
            return errorMsg;
        }

        private string GetErrorMessageFromXml(QbLog qbLog)
        {
            string message = null;
            try
            {
                if (qbLog != null)
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(qbLog.Response);
                    var firstChild = xmlDocument.DocumentElement.FirstChild;
                    if (firstChild != null && firstChild.FirstChild != null)
                    {
                        var statusMessage = firstChild.FirstChild.Attributes["statusMessage"];
                        if (statusMessage != null && !string.IsNullOrWhiteSpace(statusMessage.InnerText))
                        {
                            message = statusMessage.InnerText;
                        }
                    }
                }
            }
            catch
            {
                message = null;
            }
            return message;
        }

        public async Task<List<QbReportViewModel>> GetQuickBooksReportAsync(QbDataTableModel qbModel, DataTableSearchModel searchModel)
        {
            List<QbReportViewModel> response = new List<QbReportViewModel>();
            try
            {
                var spDomain = new StoredProcedureDomain();
                response = await spDomain.GetQbReportGrid(qbModel.FromDateTime, qbModel.ToDateTime, qbModel.CompanyId, searchModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("QbDomain", "GetQuickBooksReportAsync", ex.Message, ex);
            }
            return response;
        }

        public String GetQbRequestXml(int id)
        {
            var response = string.Empty;
            try
            {
                var request = Context.DataContext.QbRequests.SingleOrDefault(t => t.Id == id);
                response = System.Xml.Linq.XDocument.Parse(request.QbXmlRq).ToString();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionLogDomain", "GetQbRequestXml", ex.Message, ex);
            }
            return response;
        }

        public String GetQbResponseXml(int id)
        {
            var response = string.Empty;
            try
            {
                var request = Context.DataContext.QbResponses.SingleOrDefault(t => t.Id == id);
                response = System.Xml.Linq.XDocument.Parse(request.QbXmlRs).ToString();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExceptionLogDomain", "GetQbRequestXml", ex.Message, ex);
            }
            return response;
        }

        public bool RetryQbRequests(long requestId)
        {
            var existingRequest = Context.DataContext.QbRequests.SingleOrDefault(t => t.Id == requestId);
            if (existingRequest != null && existingRequest.RetryCount < 4)
            {
                Context.DataContext.QbRequests.Add(new QbRequest()
                {
                    QbXmlRq = existingRequest.QbXmlRq,
                    QbXmlType = existingRequest.QbXmlType,
                    Status = (int)QbXmlStatus.ReadyToQueue,
                    AccountingWorkflowId = existingRequest.AccountingWorkflowId,
                    ReadyForQueueOn = existingRequest.ReadyForQueueOn,
                    CreatedOn = existingRequest.CreatedOn,
                    UpdatedOn = DateTimeOffset.Now,
                    EntityId = existingRequest.EntityId,
                    EntityType = existingRequest.EntityType,
                    RetryCount = existingRequest.RetryCount + 1,
                    PoNumber = existingRequest.PoNumber,
                    InvoiceNumberId = existingRequest.InvoiceNumberId,
                    ParentId = requestId
                });
                existingRequest.Status = (int)QbXmlStatus.Retried;
                Context.DataContext.Entry(existingRequest).State = EntityState.Modified;
                Context.Commit();
                return true;
            }
            return false;
        }

        public async Task MarkQbRequestsToDeleted(int invoiceNumberId)
        {
            var qbRequests = Context.DataContext.QbRequests.Where(t => t.InvoiceNumberId == invoiceNumberId
                            && t.AccountingWorkflow.Status != (int)AccountingWorkflowStatus.Completed).ToList();
            qbRequests.ForEach(t => t.Status = (int)QbRequestStatus.SFXInvoiceDeleted);

            var invoiceNumber = $"\"InvoiceNumberId\":{invoiceNumberId}";
            var workflows = Context.DataContext.QbWorkflows.Where(t => t.ParameterJson.Contains(invoiceNumber)
                            && t.Status != (int)AccountingWorkflowStatus.Completed).ToList();
            workflows.ForEach(t => t.Status = (int)AccountingWorkflowStatus.SFXInvoiceDeleted);

            await Context.CommitAsync();
        }

        public async Task<StatusViewModel> SaveInvoicePayments(List<QbReceivePaymentViewModel> receivedPaymentList)
        {
            StatusViewModel statusViewModel = new StatusViewModel();
            statusViewModel.StatusCode = Status.Success;
            statusViewModel.StatusMessage = "No record(s) found in SFX";

            try
            {
                if (receivedPaymentList == null || receivedPaymentList.Count == 0)
                {
                    statusViewModel.StatusCode = Status.Failed;
                    statusViewModel.StatusMessage = "No record(s) found";
                    return statusViewModel;
                }

                foreach (var payment in receivedPaymentList)
                {
                    var invoice = await Context.DataContext.Invoices.Where(t => t.Order.AcceptedCompanyId == payment.CompanyId 
                                                    && t.QbInvoiceNumber == payment.QbInvoiceNumber 
                                                    && t.IsActive && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active)
                                            .Select(t1 => new {
                                                                t1.Id,
                                                                t1.InvoiceHeader.InvoiceNumberId,                                                                
                                            }).FirstOrDefaultAsync();

                    if (invoice != null)
                    {
                        var isPaymentAlreadyExists = await Context.DataContext.InvoicePayments.AnyAsync(t => t.QbInvoiceNumber == payment.QbInvoiceNumber && t.TxnId == payment.TxnId && t.AmountPaid == payment.AmountPaid);
                        if (!isPaymentAlreadyExists)
                        {
                            var item = new InvoicePayment()
                            {
                                InvoiceNumberId = invoice.InvoiceNumberId,
                                QbInvoiceNumber = payment.QbInvoiceNumber,
                                AmountPaid = payment.AmountPaid,
                                BalanceRemaining = payment.BalanceRemaining,
                                PaymentMethod = payment.PaymentMethod ?? "-",// send some value as it is not null field, and payment method coming as null from QB
                                TotalAmount = payment.TotalAmount,
                                PaymentDate = DateTimeOffset.Parse(payment.PaymentDate),
                                TxnId = payment.TxnId,
                                TransRefNumber = payment.TransRefNumber,
                                PaymentSource = PaymentSource.QB,
                                IsActive = true,
                                UpdatedDate = DateTimeOffset.Now
                            };
                                                       
                            Context.DataContext.InvoicePayments.Add(item);
                            Context.Commit();

                            await SetInvoiceStatus(invoice.Id);

                            statusViewModel.StatusCode = Status.Success;
                            statusViewModel.StatusMessage = Resource.msgSuccess;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                statusViewModel.StatusCode = Status.Failed;
                statusViewModel.StatusMessage = Resource.gridColumnFailed;
                LogManager.Logger.WriteException("QbDomain", "SaveInvoicePayments", ex.Message, ex);
            }

            return statusViewModel;
        }

        //private async Task<decimal> GetInvoiceTotalAmount(int invoiceId)
        //{
        //    decimal totalInvoiceAmount = 0;
        //    var invoice = await Context.DataContext.Invoices.Where(t => t.Id == invoiceId).Select(t1 => new
        //    {
        //        t1.BasicAmount,
        //        t1.TotalDiscountAmount,
        //        t1.TotalFeeAmount,
        //        t1.TotalTaxAmount,
        //        t1.InvoiceTypeId,
        //        t1.WaitingFor,
        //        t1.OrderId
        //    }).FirstOrDefaultAsync();

        //    if (invoice.WaitingFor == (int)WaitingAction.Nothing && invoice.OrderId != null)
        //    {
        //        totalInvoiceAmount = invoice.BasicAmount + (invoice.TotalFeeAmount ?? 0) - invoice.TotalDiscountAmount;
        //        if (invoice.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketManual && invoice.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketMobileApp)
        //        {
        //            totalInvoiceAmount += invoice.TotalTaxAmount;
        //        }
        //    }

        //    return totalInvoiceAmount;
        //}

        private async Task<PaymentStatus> GetPaymentStatus(int invoiceNumberId)
        {
            PaymentStatus paymentStatus = PaymentStatus.NotPaid;
            try
            {
                var invoicePaidAmounts = await Context.DataContext.InvoicePayments.Where(t => t.InvoiceNumberId == invoiceNumberId && t.IsActive)
                    .Select(t1 => new {
                        t1.AmountPaid,
                        t1.BalanceRemaining
                    }).ToListAsync();

                if (invoicePaidAmounts != null && invoicePaidAmounts.Count > 0)
                {                    
                    var balanceRemaining = Math.Round(invoicePaidAmounts.Min(t => t.BalanceRemaining), 2);
                    if (balanceRemaining == 0)
                        paymentStatus = PaymentStatus.Paid;
                    else if (balanceRemaining > 0)
                        paymentStatus = PaymentStatus.PartiallyPaid;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("QbDomain", "GetPaymentStatus", ex.Message, ex);
            }

            return paymentStatus;
        }

        private async Task SetInvoiceStatus(int invoiceId)
        {
            try
            {
                var invoice = await Context.DataContext.Invoices.SingleOrDefaultAsync(t => t.Id == invoiceId && t.IsActive);

                if (invoice != null)
                {
                    PaymentStatus paymentStatus = await GetPaymentStatus(invoice.InvoiceHeader.InvoiceNumberId);

                    if (paymentStatus == PaymentStatus.Paid)
                        invoice.InvoiceXInvoiceStatusDetails.SingleOrDefault(t => t.IsActive).StatusId = (int)InvoiceStatus.Paid;
                    else if(paymentStatus == PaymentStatus.PartiallyPaid)
                        invoice.InvoiceXInvoiceStatusDetails.SingleOrDefault(t => t.IsActive).StatusId = (int)InvoiceStatus.PartiallyPaid;

                    Context.DataContext.Entry(invoice).State = EntityState.Modified;
                    await Context.CommitAsync();                    
                }               
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("QbDomain", "SetInvoiceStatus", ex.Message, ex);
            }
        }

        public void SaveReceivePaymentQueueMessage(long qbRequestId, int workflowId, int companyId)
        {
            try
            {
                var receivePaymentQueueMessage = new ReceivePaymentQueueMessage();
                receivePaymentQueueMessage.QbRequestId = qbRequestId;
                receivePaymentQueueMessage.WorkflowId = workflowId;
                receivePaymentQueueMessage.CompanyId = companyId;

                string jsonMessage = JsonConvert.SerializeObject(receivePaymentQueueMessage);
                QueueMessageDomain domain = new QueueMessageDomain(this);

                QueueMessageViewModel messageViewModel = new QueueMessageViewModel();
                messageViewModel.JsonMessage = jsonMessage;
                messageViewModel.CreatedBy = (int)SystemUser.System;
                messageViewModel.QueueProcessType = QueueProcessType.ReceivePayment;
                domain.EnqeueMessage(messageViewModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("QbDomain", "SaveReceivePaymentQueueMessage", ex.Message, ex);
            }
        }

        public async Task<List<CompanySyncDateRangeFilter>> GetQbCompaniesSyncDates()
        {
            List<CompanySyncDateRangeFilter> qbCompanyProfiles = new List<CompanySyncDateRangeFilter>();
            try
            {
                qbCompanyProfiles = await Context.DataContext.QbProfiles.Where(t => t.IsActive && t.IsSyncEnabled)
                                                                          .Select(t1 => new CompanySyncDateRangeFilter()
                                                                          {
                                                                              StartDate = t1.SyncStartDate,
                                                                              EndDate = t1.SyncEndDate,
                                                                              CompanyId = t1.CompanyId
                                                                          }).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("QbDomain", "GetQbCompaniesSyncDates", ex.Message, ex);
            }
            
            return qbCompanyProfiles;
        }

        public async Task<bool> ProcessQbReceivePaymentRequestWorkflow()
        {
            bool isProcessed = false;
            try
            {
                // get payment response for 5 days only in one response
                var paymentResponseDaysCount = 5;

                List<CompanySyncDateRangeFilter> companySyncDateRange = await GetQbCompaniesSyncDates();
                if(companySyncDateRange != null && companySyncDateRange.Count > 0)
                {         
                    // set end date to yesterday's date to get payment response till this date
                    var currentDate = DateTimeOffset.Now;
                    bool isWorkflowCreated = false;
                    foreach (var qbCompany in companySyncDateRange)
                    {
                        var syncStartDate = qbCompany.StartDate;
                        //set startdate to enddate, if end date is already set, 
                        if (qbCompany.EndDate != null && qbCompany.EndDate.HasValue)
                        {
                            syncStartDate = qbCompany.EndDate.Value.AddDays(1);
                        }

                        if (syncStartDate.Date > currentDate.Date)
                        {
                            continue;
                        }

                        // calculate total days
                        var totalDays = Math.Ceiling((currentDate - syncStartDate).TotalDays);
                        totalDays = totalDays <= 0 ? 1 : Convert.ToInt32(totalDays);
                        int loopCount = Convert.ToInt32(Math.Ceiling(totalDays / paymentResponseDaysCount));

                        for (int i = 0; i < loopCount; i++)
                        {
                            var toEndDate = syncStartDate.AddDays(paymentResponseDaysCount);
                            if(toEndDate.Date > currentDate.Date)
                            {
                                toEndDate = currentDate;
                            }

                            var requestParameters = new RequestParameters()
                            {
                                CustomerId = qbCompany.CompanyId,
                                FromModifiedDate = syncStartDate.Date,
                                ToModifiedDate = toEndDate.Date
                            };
                            
                            var accountingWorkflow = QbWorkflowDomain.GetAccountingWorkflowViewModel(AccountingWorkflowType.ReceivePayment, qbCompany.CompanyId, requestParameters);
                            CreateNewWorkflow(accountingWorkflow);

                            syncStartDate = syncStartDate.AddDays(paymentResponseDaysCount + 1);
                            isWorkflowCreated = true;

                            if (syncStartDate.Date > currentDate.Date)
                                break;
                        }

                        if (isWorkflowCreated)
                        {
                            // update end date in QB company profiler
                            var qbCmp = await Context.DataContext.QbProfiles.SingleOrDefaultAsync(t => t.CompanyId == qbCompany.CompanyId);
                            if (qbCmp != null)
                            {
                                qbCmp.SyncEndDate = currentDate;
                                Context.DataContext.Entry(qbCmp).State = EntityState.Modified;
                                Context.Commit();
                            }

                            isProcessed = true;
                        }
                    }
                }               
            }
            catch (Exception ex)
            {
                isProcessed = false;
                LogManager.Logger.WriteException("QbDomain", "ProcessQbReceivePaymentRequestWorkflow", ex.Message, ex);
            }

            return isProcessed;
        }

        public string GetReceivePaymentResponseXml(long qbRequestId)
        {
            string qbXmlRs = string.Empty;
            try
            {
                qbXmlRs = Context.DataContext.QbResponses.Where(t => t.QbRequestId == qbRequestId).Select(t => t.QbXmlRs).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("QbDomain", "GetReceivePaymentResponseXml", ex.Message, ex);
            }

            return qbXmlRs;
        }

        public async Task<StatusViewModel> ProcessQbInvoicePayments(ReceivePaymentQueueMessage receivePaymentQueueMessage)
        {
            StatusViewModel statusViewModel = new StatusViewModel();
            statusViewModel.StatusCode = Status.Success;
            statusViewModel.StatusMessage = "No record(s) found";

            try
            {
                var receivePaymentRs = GetReceivePaymentResponseObject(receivePaymentQueueMessage.QbRequestId);
                if (receivePaymentRs != null && receivePaymentRs.ReceivePaymentRet != null)
                {
                    List<QbReceivePaymentViewModel> receivedPaymentList = new List<QbReceivePaymentViewModel>();
                    foreach (var receivePaymentRet in receivePaymentRs.ReceivePaymentRet)
                    {
                        QbReceivePaymentViewModel obj = new QbReceivePaymentViewModel();
                        obj.TxnId = receivePaymentRet.TxnID;
                        obj.TransRefNumber = receivePaymentRet.RefNumber;
                        obj.PaymentDate = receivePaymentRet.TxnDate;
                        obj.PaymentMethod = receivePaymentRet.PaymentMethodRef?.FullName;
                        obj.TotalAmount = receivePaymentRet.TotalAmount;
                        obj.CompanyId = receivePaymentQueueMessage.CompanyId;

                        if (receivePaymentRet.AppliedToTxnRet != null)
                        {
                            foreach (var appliedToTxnRet in receivePaymentRet.AppliedToTxnRet)
                            {
                                obj.QbInvoiceNumber = appliedToTxnRet.RefNumber;
                                obj.BalanceRemaining = appliedToTxnRet.BalanceRemaining;
                                obj.AmountPaid = appliedToTxnRet.Amount;
                            }
                        }

                        receivedPaymentList.Add(obj);
                    }

                    if (receivedPaymentList.Count > 0)
                    {
                        statusViewModel = await SaveInvoicePayments(receivedPaymentList);
                    }
                }
            }
            catch (Exception ex)
            {
                statusViewModel.StatusCode = Status.Failed;
                statusViewModel.StatusMessage = ex.Message;
                LogManager.Logger.WriteException("QbDomain", "ProcessQbInvoicePayments", ex.Message, ex);
            }

            return statusViewModel;
        }

        private ReceivePaymentQueryRs GetReceivePaymentResponseObject(long qbRequestId)
        {
            ReceivePaymentQueryRs receivePaymentQueryRs = null;
            try
            {
                string xmlReceivePaymentQueryRs = GetReceivePaymentResponseXml(qbRequestId);
                if (!string.IsNullOrWhiteSpace(xmlReceivePaymentQueryRs))
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(xmlReceivePaymentQueryRs);
                    receivePaymentQueryRs = XmlSerialization.Deserialize<ReceivePaymentQueryRs>(xmlDocument.DocumentElement.FirstChild.InnerXml);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("QbDomain", "GetReceivePaymentResponseObject", ex.Message, ex);
            }

            return receivePaymentQueryRs;
        }

        public void CreateQbRequestsAndMappings(ViewModels.AccountingEvent.AccountingWorkflowViewModel item, List<ViewModels.Quickbooks.QbRequestViewModel> qbRqs)
        {
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    if (item.Type == ViewModels.AccountingEvent.AccountingWorkflowType.InvoiceAdd)
                    {
                        var salesOrderRq = qbRqs.FirstOrDefault(x => x.EntityType == "SalesOrder");
                        if (salesOrderRq != null && salesOrderRq.EntityId.HasValue)
                            UpdateFailedRequestsByEntityId(salesOrderRq.PoNumber);
                    }
                    // Log what is been initialized
                    CreateQbRequests(qbRqs, item.Id);

                    var requestWithInvoices = qbRqs.Where(t => t.QbXmlType == (int)QbXmlType.InvoiceAdd).ToList();
                    CreateQbInvoiceMappings(requestWithInvoices, item.QbCompanyProfileId);

                    var requestWithoutInvoices = qbRqs.Except(requestWithInvoices).ToList();
                    var includeEntityMappings = new List<int> { (int)QbXmlType.SalesOrderAdd, (int)QbXmlType.PurchaseOrderAdd, (int)QbXmlType.BillAdd, (int)QbXmlType.CreditMemoAdd, (int)QbXmlType.VendorCreditAdd };
                    requestWithoutInvoices = requestWithoutInvoices.Where(t => includeEntityMappings.Contains(t.QbXmlType)).ToList();
                    CreateQbEntityMappings(requestWithoutInvoices, item.QbCompanyProfileId);

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    LogManager.Logger.WriteException("QbDomain", "CreateQbRequestsAndMappings",
                        string.Format("Exception : {0} , Workflow ID : {1} , Item Type : {2}", ex.Message, item.Id, item.Type), ex);

                    var workflow = Context.DataContext.QbWorkflows.Where(x => x.Id == item.Id).FirstOrDefault();
                    workflow.Status = (int)AccountingWorkflowStatus.Created;
                    Context.DataContext.Entry(workflow).State = EntityState.Modified;
                    Context.Commit();
                }
            }
        }
    }
}
