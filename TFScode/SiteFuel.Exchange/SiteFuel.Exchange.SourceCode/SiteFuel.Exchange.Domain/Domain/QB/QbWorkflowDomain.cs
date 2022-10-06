using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.AccountingEvent;
using System;

namespace SiteFuel.Exchange.Domain
{
    public class QbWorkflowDomain : BaseDomain
    {
        public QbWorkflowDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public QbWorkflowDomain(BaseDomain baseDomain) : base(baseDomain)
        {
        }

        public void CreateSalesOrderWorkflow(UserContext userContext, FuelRequest fuelRequest, Order order)
        {
            try
            {
                QbDomain qbDomain = new QbDomain(this);
                int qbProfileId = qbDomain.GetQbCompanyProfileId(order.AcceptedCompanyId);
                if (qbProfileId > 0)
                {
                    var requestParameters = new RequestParameters()
                    {
                        CompanyId = userContext.CompanyId,
                        CustomerCompanyId = order.BuyerCompanyId,
                        CustomerId = fuelRequest.CreatedBy,
                        JobId = fuelRequest.JobId,
                        FuelRequestId = fuelRequest.Id,
                        OrderId = order.Id
                    };
                    var accountingWorkflow = GetAccountingWorkflowViewModel(AccountingWorkflowType.SaleOrder, qbProfileId, requestParameters);
                    qbDomain.CreateNewWorkflow(accountingWorkflow);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("QbWorkflowDomain", "CreateSalesOrderWorkflow", ex.Message, ex);
            }
        }

        public void CreatePurchaseOrderWorkflow(UserContext userContext, FuelRequest fuelRequest, Order order, int? parentOrderId)
        {
            try
            {
                QbDomain qbDomain = new QbDomain(this);
                var qbCompanyProfile = qbDomain.GetQbCompanyProfile(order.BuyerCompanyId);
                if (qbCompanyProfile != null && qbCompanyProfile.CompanyId > 0)
                {
                    var requestParameters = GetPoRequestParameters(userContext, order, parentOrderId);
                    requestParameters.ClassName = qbCompanyProfile.ClassRef;
                    var accountingWorkflow = GetAccountingWorkflowViewModel(AccountingWorkflowType.PurchaseOrder, qbCompanyProfile.CompanyId, requestParameters);
                    qbDomain.CreateNewWorkflow(accountingWorkflow);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("QbWorkflowDomain", "CreatePurchaseOrderWorkflow", ex.Message, ex);
            }
        }

        public void CreateInvoiceWorkflow(UserContext userContext, Order order, Invoice invoice, AccountingWorkflowType workflowType)
        {
            try
            {
                QbDomain qbDomain = new QbDomain(this);
                int qbProfileId = qbDomain.GetQbCompanyProfileId(order.AcceptedCompanyId);
                if (qbProfileId > 0)
                {
                    var requestParameters = GetInvoiceRequestParameters(userContext, order, invoice);
                    var accountingWorkflow = GetAccountingWorkflowViewModel(workflowType, qbProfileId, requestParameters);
                    qbDomain.CreateNewWorkflow(accountingWorkflow);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("QbWorkflowDomain", "CreateInvoiceWorkflow", ex.Message, ex);
            }
        }

        public void CreateInvoicePoWorkflow(UserContext userContext, Order order, Invoice invoice, int? parentOrderId, AccountingWorkflowType workflowType)
        {
            try
            {
                QbDomain qbDomain = new QbDomain(this);
                var qbCompanyProfile = qbDomain.GetQbCompanyProfile(order.BuyerCompanyId);
                if (qbCompanyProfile != null && qbCompanyProfile.CompanyId > 0)
                {
                    var requestParameters = GetPoRequestParameters(userContext, order, parentOrderId);
                    requestParameters.InvoiceId = invoice.Id;
                    requestParameters.InvoiceNumberId = invoice.InvoiceHeader.InvoiceNumberId;
                    requestParameters.InvoiceBrokeredChainId = invoice.BrokeredChainId;
                    requestParameters.IsEndSupplier = order.IsEndSupplier;
                    requestParameters.InvoiceCreationDate = invoice.CreatedDate;
                    requestParameters.ClassName = qbCompanyProfile.ClassRef;
                    var accountingWorkflow = GetAccountingWorkflowViewModel(workflowType, qbCompanyProfile.CompanyId, requestParameters);
                    qbDomain.CreateNewWorkflow(accountingWorkflow);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("QbWorkflowDomain", "CreateInvoicePoWorkflow", ex.Message, ex);
            }
        }

        public void CreateBillWorkflow(UserContext userContext, Order order, Invoice invoice, int? parentOrderId, AccountingWorkflowType workflowType)
        {
            try
            {
                QbDomain qbDomain = new QbDomain(this);
                var qbCompanyProfile = qbDomain.GetQbCompanyProfile(order.BuyerCompanyId);
                if (qbCompanyProfile != null && qbCompanyProfile.CompanyId > 0)
                {
                    var requestParameters = GetPoRequestParameters(userContext, order, parentOrderId);
                    requestParameters.InvoiceId = invoice.Id;
                    requestParameters.InvoiceNumberId = invoice.InvoiceHeader.InvoiceNumberId;
                    requestParameters.InvoiceBrokeredChainId = invoice.BrokeredChainId;
                    requestParameters.IsEndSupplier = order.IsEndSupplier;
                    requestParameters.InvoiceCreationDate = invoice.CreatedDate;
                    requestParameters.ClassName = qbCompanyProfile.ClassRef;
                    var accountingWorkflow = GetAccountingWorkflowViewModel(workflowType, qbCompanyProfile.CompanyId, requestParameters);
                    qbDomain.CreateNewWorkflow(accountingWorkflow);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("QbWorkflowDomain", "CreateBillWorkflow", ex.Message, ex);
            }
        }

        public void CreateCreditMemoWorkflow(UserContext userContext, Order order, Invoice invoice, AccountingWorkflowType workflowType)
        {
            try
            {
                QbDomain qbDomain = new QbDomain(this);
                int qbProfileId = qbDomain.GetQbCompanyProfileId(order.AcceptedCompanyId);
                if (qbProfileId > 0)
                {
                    var requestParameters = GetInvoiceRequestParameters(userContext, order, invoice);
                    var accountingWorkflow = GetAccountingWorkflowViewModel(workflowType, qbProfileId, requestParameters);
                    qbDomain.CreateNewWorkflow(accountingWorkflow);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("QbWorkflowDomain", "CreateCreditMemoWorkflow", ex.Message, ex);
            }
        }

        public void CreateVendorCreditWorkflow(UserContext userContext, Order order, Invoice invoice, AccountingWorkflowType workflowType)
        {
            try
            {
                QbDomain qbDomain = new QbDomain(this);
                int qbProfileId = qbDomain.GetQbCompanyProfileId(order.BuyerCompanyId);
                if (qbProfileId > 0)
                {
                    var requestParameters = GetPoRequestParameters(userContext, order, null);
                    requestParameters.InvoiceId = invoice.Id;
                    requestParameters.InvoiceNumberId = invoice.InvoiceHeader.InvoiceNumberId;
                    requestParameters.InvoiceBrokeredChainId = invoice.BrokeredChainId;
                    requestParameters.IsEndSupplier = order.IsEndSupplier;
                    requestParameters.InvoiceCreationDate = invoice.CreatedDate;
                    var accountingWorkflow = GetAccountingWorkflowViewModel(workflowType, qbProfileId, requestParameters);
                    qbDomain.CreateNewWorkflow(accountingWorkflow);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("QbWorkflowDomain", "CreateVendorCreditWorkflow", ex.Message, ex);
            }
        }

        public RequestParameters GetRequestParameters(AccountingWorkflowViewModel workflow)
        {
            RequestParameters requestParameters = null;
            try
            {
                requestParameters = JsonConvert.DeserializeObject<RequestParameters>(workflow.ParameterJson);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("QbWorkflowDomain", "GetRequestParameters", ex.Message, ex);
            }
            return requestParameters;
        }

        public static AccountingWorkflowViewModel GetAccountingWorkflowViewModel(AccountingWorkflowType workflowType, int qbProfileId, RequestParameters requestParameters)
        {
            var parameterJson = JsonConvert.SerializeObject(requestParameters);
            var accountingWorkflow = new AccountingWorkflowViewModel()
            {
                Type = workflowType,
                ParameterJson = parameterJson,
                Status = AccountingWorkflowStatus.Created,
                QbCompanyProfileId = qbProfileId,
                SoftwareVersion = "13.0",
                CreatedOn = DateTimeOffset.Now,
                UpdatedOn = DateTimeOffset.Now
            };
            return accountingWorkflow;
        }

        private static RequestParameters GetInvoiceRequestParameters(UserContext userContext, Order order, Invoice invoice)
        {
            return new RequestParameters()
            {
                CompanyId = userContext.CompanyId,
                CustomerCompanyId = order.BuyerCompanyId,
                CustomerId = order.FuelRequest.CreatedBy,
                JobId = order.FuelRequest.JobId,
                FuelRequestId = order.FuelRequestId,
                OrderId = order.Id,
                InvoiceId = invoice.Id,
                InvoiceNumberId = invoice.InvoiceHeader.InvoiceNumberId,
                InvoiceBrokeredChainId = invoice.BrokeredChainId,
                IsEndSupplier = order.IsEndSupplier,
                InvoiceCreationDate = invoice.CreatedDate
            };
        }

        private static RequestParameters GetPoRequestParameters(UserContext userContext, Order order, int? parentOrderId)
        {
            return new RequestParameters()
            {
                CompanyId = userContext.CompanyId,
                CustomerCompanyId = order.AcceptedCompanyId,
                CustomerId = order.AcceptedBy,
                JobId = order.FuelRequest.JobId,
                FuelRequestId = order.FuelRequestId,
                OrderId = order.Id,
                ParentOrderId = parentOrderId > 0 ? parentOrderId : null
            };
        }
    }
}
