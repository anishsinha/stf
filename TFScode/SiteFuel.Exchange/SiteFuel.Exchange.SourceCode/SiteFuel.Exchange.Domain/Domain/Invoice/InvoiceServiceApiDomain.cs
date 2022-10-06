using Easy.Common;
using Easy.Common.Interfaces;
using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class InvoiceServiceApiDomain : BaseDomain
    {
        private string _invoiceServiceBaseUrl = string.Empty;
        public InvoiceServiceApiDomain() : base(ContextFactory.Current.ConnectionString)
        {
            InstanceInitialize();
        }
        public InvoiceServiceApiDomain(string connectionString) : base(connectionString)
        {
            InstanceInitialize();
        }

        public InvoiceServiceApiDomain(BaseDomain domain) : base(domain)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            var appDomain = new ApplicationDomain(this);
            _invoiceServiceBaseUrl = appDomain.GetKeySettingValue("InvoiceServiceBaseUrl", "");
        }

        public async Task<T> ApiPostCall<T>(string url, object inputObject)
        {
            T response = default(T);
            if (string.IsNullOrWhiteSpace(_invoiceServiceBaseUrl))
                throw new Exception("ApiPostCall: invoiceService configuration is missing");

            IDictionary<string, IEnumerable<string>> defaultRequestHeaders = new Dictionary<string, IEnumerable<string>>();
            defaultRequestHeaders.Add(ApplicationConstants.Token, new List<string>() { ApplicationConstants.Token });
            using (IRestClient client = new RestClient(defaultRequestHeaders))
            {
                url = _invoiceServiceBaseUrl + url;
                var json = JsonConvert.SerializeObject(inputObject);
                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                HttpResponseMessage apiResponse = await client.PostAsync(url, stringContent);
                if (apiResponse.IsSuccessStatusCode)
                {
                    var responseString = await apiResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<T>(responseString);
                }
            }
            return response;
        }

        public async Task<T> ApiGetCall<T>(string url, int timeout = 100)
        {
            T response = default(T);
            if (string.IsNullOrWhiteSpace(_invoiceServiceBaseUrl))
                throw new Exception("ApiGetCall: invoiceService configuration is missing");

            using (IRestClient client = new RestClient(null, null, null, true, TimeSpan.FromSeconds(timeout)))
            {
                url = _invoiceServiceBaseUrl + url;
                HttpResponseMessage apiResponse = await client.GetAsync(url);
                if (apiResponse.IsSuccessStatusCode)
                {
                    var responseString = await apiResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<T>(responseString);
                }
            }
            return response;
        }


        #region Supplier
        public async Task<List<InvoiceGridViewModel>> GetSupplierInvoiceGridAsync(int companyId, InvoiceDataTableViewModel invoiceFilter, int allowedInvoiceType, ViewInvoices view)
        {
            List<InvoiceGridViewModel> response = new List<InvoiceGridViewModel>();
            try
            {
                var inputObj = new InvoiceGridRequestModel { CompanyId = companyId, ViewModel = invoiceFilter, InvoiceType = allowedInvoiceType, View = view };
                response = await ApiPostCall<List<InvoiceGridViewModel>>(ApplicationConstants.UrlGetSupplierInvoiceGrid, inputObj);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceServiceApiDomain", "GetSupplierInvoicesAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<StatusViewModel> EditInvoicePoNumber(UserContext userContext, int invoiceId, string poNumber)
        {
            var response = new StatusViewModel();
            try
            {
                var inputObj = new EditInvoicePoNumberReqModel { InvoiceId = invoiceId, PoNumber = poNumber, UserContext = userContext };
                response = await ApiPostCall<StatusViewModel>(ApplicationConstants.UrlEditInvoicePoNumber, inputObj);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceServiceApiDomain", "EditInvoicePoNumber", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<InvoiceHistoryGridViewModel>> InvoiceHistoryGrid(int id, int userId)
        {
            var response = new List<InvoiceHistoryGridViewModel>();
            try
            {
                var url = string.Format(ApplicationConstants.UrlGetInvoiceHistoryGrid, id, userId);
                response = await ApiGetCall<List<InvoiceHistoryGridViewModel>>(url);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceServiceApiDomain", "InvoiceHistoryGrid", ex.Message, ex);
            }
            return response;
        }

        public async Task<InvoiceDetailViewModel> GetSupplierInvoiceDetail(int id, UserContext userContext)
        {
            var response = new InvoiceDetailViewModel();
            try
            {
                response = await ApiPostCall<InvoiceDetailViewModel>(string.Format(ApplicationConstants.UrlGetSupplierInvoiceDetail, id), userContext);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceServiceApiDomain", "GetSupplierInvoiceDetail", ex.Message, ex);
            }
            return response;
        }

        public async Task<InvoicePdfViewModel> PartialInvoicePdf(int id, CompanyType companyType)
        {
            var response = new InvoicePdfViewModel();
            try
            {
                var url = string.Format(ApplicationConstants.UrlGetPartialInvoicePdf, id, companyType);
                response = await ApiGetCall<InvoicePdfViewModel>(url);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceServiceApiDomain", "PartialInvoicePdf", ex.Message, ex);
            }
            return response;
        }

        public async Task<BDRPdfViewModel> BDRPdf(int invoiceHeaderId, CompanyType companyType, UserContext userContext)
        {
            var response = new BDRPdfViewModel();
            try
            {
                var inputObj = new InvoicePdfRequestModel() { InvoiceHeaderId = invoiceHeaderId, CompanyType = companyType, UserContext = userContext };
                response = await ApiPostCall<BDRPdfViewModel>(ApplicationConstants.UrlGetBDRPdf, inputObj);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceServiceApiDomain", "BDRPdf", ex.Message, ex);
            }
            return response;
        }

        public async Task<InvoiceDetailViewModel> DownloadBDRSummary(int id, UserContext userContext)
        {
            var response = new InvoiceDetailViewModel();
            try
            {
                var inputObj = new InvoicePdfRequestModel() { InvoiceHeaderId = id, UserContext = userContext };
                response = await ApiPostCall<InvoiceDetailViewModel>(ApplicationConstants.UrlDownloadBDRSummary, inputObj);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceServiceApiDomain", "DownloadBDRSummary", ex.Message, ex);
            }
            return response;
        }

        public async Task<DryRunInvoiceViewModel> GetDryRunInvoice(int id, int currentUserId)
        {
            var response = new DryRunInvoiceViewModel();
            try
            {
                var url = string.Format(ApplicationConstants.UrlGetDryRunInvoice, id, currentUserId);
                response = await ApiGetCall<DryRunInvoiceViewModel>(url);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceServiceApiDomain", "GetDryRunInvoice", ex.Message, ex);
            }
            return response;
        }

        public async Task<DryRunInvoiceViewModel> GetDryRunInvoiceForEdit(int id, int currentUserId)
        {
            var response = new DryRunInvoiceViewModel();
            try
            {
                var url = string.Format(ApplicationConstants.UrlGetDryRunInvoiceForEdit, id, currentUserId);
                response = await ApiGetCall<DryRunInvoiceViewModel>(url);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceServiceApiDomain", "GetDryRunInvoiceForEdit", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<AssignToOrderGridViewModel>> AssignToOrderGrid(int currentUserId)
        {
            var response = new List<AssignToOrderGridViewModel>();
            try
            {
                var url = string.Format(ApplicationConstants.UrlAssignToOrderGrid, currentUserId);
                response = await ApiGetCall<List<AssignToOrderGridViewModel>>(url);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceServiceApiDomain", "AssignToOrderGrid", ex.Message, ex);
            }
            return response;
        }

        public async Task<AssignToOrderPreviewViewModel> OrderPreview(int orderId, int invoiceId)
        {
            var response = new AssignToOrderPreviewViewModel();
            try
            {
                var url = string.Format(ApplicationConstants.UrlOrderPreView, orderId, invoiceId);
                response = await ApiGetCall<AssignToOrderPreviewViewModel>(url);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceServiceApiDomain", "OrderPreView", ex.Message, ex);
            }
            return response;
        }

        public async Task<AssignToOrderViewModel> AssignInvoiceToOrder(int orderId, int invoiceId, int currentUserId)
        {
            var response = new AssignToOrderViewModel();
            try
            {
                var url = string.Format(ApplicationConstants.UrlAssignInvoiceToOrder, orderId, invoiceId, currentUserId);
                response = await ApiGetCall<AssignToOrderViewModel>(url);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceServiceApiDomain", "AssignInvoiceToOrder", ex.Message, ex);
            }
            return response;
        }

        public async Task<ManualInvoiceViewModel> GetManualInvoiceAsync(int orderId)
        {
            var response = new ManualInvoiceViewModel();
            try
            {
                var url = string.Format(ApplicationConstants.UrlGetManualInvoice, orderId);
                response = await ApiGetCall<ManualInvoiceViewModel>(url);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceServiceApiDomain", "GetManualInvoiceAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<ManualInvoiceViewModel> GetManualInvoiceForEdit(int id)
        {
            var response = new ManualInvoiceViewModel();
            try
            {
                var url = string.Format(ApplicationConstants.UrlGetManualInvoiceForEdit, id);
                response = await ApiGetCall<ManualInvoiceViewModel>(url);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceServiceApiDomain", "GetManualInvoiceForEdit", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> CreateManualFtlInvoice(UserContext userContext, ManualInvoiceViewModel viewModel)
        {
            var response = new StatusViewModel();
            try
            {
                var inputObj = new CreateInvoiceViewModel() { UserContext = userContext, ViewModel = viewModel };
                response = await ApiPostCall<StatusViewModel>(ApplicationConstants.UrlCreateManualFtlInvoice, inputObj);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceServiceApiDomain", "CreateManualFtlInvoice", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> CreateManualInvoice(UserContext userContext, ManualInvoiceViewModel viewModel)
        {
            var response = new StatusViewModel();
            try
            {
                var inputObj = new CreateInvoiceViewModel() { UserContext = userContext, ViewModel = viewModel };
                response = await ApiPostCall<StatusViewModel>(ApplicationConstants.UrlCreateManualInvoice, inputObj);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceServiceApiDomain", "CreateManualInvoice", ex.Message, ex);
            }
            return response;
        }


        public async Task<ManualInvoiceViewModel> GetAssetsForInvoice(ManualInvoiceViewModel viewModel)
        {
            var response = new ManualInvoiceViewModel();
            try
            {
                response = await ApiPostCall<ManualInvoiceViewModel>(ApplicationConstants.UrlGetAssetsForInvoice, viewModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceServiceApiDomain", "GetAssetsForInvoice", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> EditDraftDDTAsync(UserContext userContext, ManualInvoiceViewModel viewModel)
        {
            var response = new StatusViewModel();
            try
            {
                var inputObj = new CreateInvoiceViewModel() { UserContext = userContext, ViewModel = viewModel };
                response = await ApiPostCall<StatusViewModel>(ApplicationConstants.UrlEditDraftDDTAsync, inputObj);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceServiceApiDomain", "EditDraftDDTAsync", ex.Message, ex);
            }
            return response;
        }
        public async Task<StatusViewModel> CreateInvoiceFromDropTicketForNonStandardFuel(UserContext userContext, ManualInvoiceViewModel viewModel)
        {
            var response = new StatusViewModel();
            try
            {
                var inputObj = new CreateInvoiceViewModel() { UserContext = userContext, ViewModel = viewModel };
                response = await ApiPostCall<StatusViewModel>(ApplicationConstants.UrlCreateInvoiceFromDropTicketForNonStandardFuel, inputObj);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceServiceApiDomain", "CreateInvoiceFromDropTicketForNonStandardFuel", ex.Message, ex);
            }
            return response;
        }
        public async Task<StatusViewModel> CreateInvoiceFromDropTicketWithBol(ManualInvoiceViewModel viewModel, UserContext userContext)
        {
            var response = new StatusViewModel();
            try
            {
                var inputObj = new CreateInvoiceViewModel() { ViewModel = viewModel, UserContext = userContext };
                response = await ApiPostCall<StatusViewModel>(ApplicationConstants.UrlCreateInvoiceFromDropTicketWithBol, inputObj);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceServiceApiDomain", "CreateInvoiceFromDropTicketWithBol", ex.Message, ex);
            }
            return response;
        }
        public async Task<StatusViewModel> InvoiceEdit(UserContext userContext, ManualInvoiceViewModel viewModel)
        {
            var response = new StatusViewModel();
            try
            {
                var inputObj = new CreateInvoiceViewModel() { UserContext = userContext, ViewModel = viewModel };
                response = await ApiPostCall<StatusViewModel>(ApplicationConstants.UrlInvoiceEdit, inputObj);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceServiceApiDomain", "InvoiceEdit", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> CancelDraftAsync(int id, UserContext userContext)
        {
            var response = new StatusViewModel();
            try
            {
                var inputObj = new InvoicePdfRequestModel { InvoiceHeaderId = id, UserContext = userContext };
                response = await ApiPostCall<StatusViewModel>(ApplicationConstants.UrlCancelDraftAsync, inputObj);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceServiceApiDomain", "CancelDraftAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> ConvertDdtToInvoiceManually(UserContext userContext, int invoiceId, bool IsConvertToInv = false)
        {
            var response = new StatusViewModel();
            try
            {
                var inputObj = new ConvertToInvoiceRequest() { UserContext = userContext, InvoiceId = invoiceId, IsConvertToInv = IsConvertToInv };
                response = await ApiPostCall<StatusViewModel>(ApplicationConstants.UrlConvertDdtToInvoiceManually, inputObj);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceServiceApiDomain", "ConvertDdtToInvoiceManually", ex.Message, ex);
            }
            return response;
        }

        public async Task<NewsfeedMessagesViewModel> GetNewsfeed(UserContext userContext, int entityId, int currentPage, int latestId, EntityType entityTypeId)
        {
            var response = new NewsfeedMessagesViewModel();
            try
            {
                response = await ApiPostCall<NewsfeedMessagesViewModel>(string.Format(ApplicationConstants.UrlGetNewsfeed, entityId, currentPage, latestId, entityTypeId), userContext);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceServiceApiDomain", "GetNewsfeed", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> EditInvoiceNumber(UserContext userContext, int invoiceId, string displayInvoiceNumber)
        {
            var response = new StatusViewModel();
            try
            {
                var pdiInvoiceNo = await Context.DataContext.Invoices.Where(w => w.Id == invoiceId).Select(s => s.PDIInvoiceNumber).FirstOrDefaultAsync();
                if (!string.IsNullOrEmpty(pdiInvoiceNo))
                {
                    response.StatusMessage = Resource.warningPDIInvoiceNoNotEditable;
                    return response;
                }
                response = await ApiPostCall<StatusViewModel>(string.Format(ApplicationConstants.UrlEditInvoiceNumber, invoiceId, displayInvoiceNumber), userContext);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceServiceApiDomain", "EditInvoiceNumber", ex.Message, ex);
            }
            return response;
        }
        public async Task<StatusViewModel> CreateInvoiceFromDraftDdt(UserContext userContext, ManualInvoiceViewModel viewModel)
        {
            var response = new StatusViewModel();
            try
            {
                var inputObj = new CreateInvoiceViewModel() { UserContext = userContext, ViewModel = viewModel };
                response = await ApiPostCall<StatusViewModel>(ApplicationConstants.UrlCreateInvoiceFromDraftDdt, inputObj);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceServiceApiDomain", "CreateInvoiceFromDraftDdt", ex.Message, ex);
            }
            return response;
        }
        public List<InvoiceGridViewModel> FilterCarrierInvoiceRecords(List<InvoiceGridViewModel> invoiceGridViews, int carrierUserId)
        {
            List<InvoiceGridViewModel> response = new List<InvoiceGridViewModel>();
            try
            {
                response = invoiceGridViews;
                var carrierUserInfo = Context.DataContext.Companies.FirstOrDefault(x => x.Id == carrierUserId && x.IsActive);
                if (carrierUserInfo != null)
                {
                    response = response.Where(x => x.Carrier.Contains(carrierUserInfo.Name)).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceServiceApiDomain", "FilterCarrierInvoiceRecords", ex.Message, ex);
            }

            return response;
        }
        public async Task<StatusViewModel> UpdateDeliveryLevelPO(ManualInvoiceViewModel viewModel)
        {
            var response = new StatusViewModel();
            try
            {
                var deliveryScheduleXTrackableSchedules = await Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(w => w.Id == viewModel.DeliveryLevelTrackableScheduleId).FirstOrDefaultAsync();
                if (deliveryScheduleXTrackableSchedules != null)
                {
                    deliveryScheduleXTrackableSchedules.DeliveryLevelPO = viewModel.DeliveryLevelPO;
                    await Context.CommitAsync();
                    response.StatusCode = Status.Success;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = Resource.errorDeliveryLevelPOUpdate;
                LogManager.Logger.WriteException("InvoiceServiceApiDomain", "UpdateDeliveryLevelPO", ex.Message, ex);
            }
            return response;
        }

        public async Task<bool> ProcessDailyDeliveryDataDumpReportCreation()
        {
            var response = false;
            try
            {
                response = await ApiGetCall<bool>(string.Format(ApplicationConstants.UrlTriggerDailyDataDumpReportCreation));
            }
            catch (Exception ex)
            {
                response = false;
                LogManager.Logger.WriteException("InvoiceServiceApiDomain", "ProcessDailyDeliveryDataDumpReportCreation", ex.Message, ex);
            }
            return response;
        }

        #endregion
    }
}
