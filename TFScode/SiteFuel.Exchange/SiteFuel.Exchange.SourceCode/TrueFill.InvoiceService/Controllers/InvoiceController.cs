using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace TrueFill.InvoiceService.Controllers
{
#if DEBUG
    [ApiExplorerSettings(IgnoreApi = false)]
#else
            [ApiExplorerSettings(IgnoreApi = true)]
#endif
    public class InvoiceController : ApiController
    {
        [HttpPost]
        public async Task<List<InvoiceGridViewModel>> GetBuyerInvoiceGrid(InvoiceGridRequestModel input)
        {
            return await ContextFactory.Current.GetDomain<InvoiceDomain>().GetBuyerInvoiceGridAsync(input.UserContext.Id, input.UserContext.CompanyId, input.UserContext.IsBuyerAdmin, input.ViewModel, input.InvoiceType, input.UserContext.BrandedCompanyId);
        }

        [HttpGet]
        public async Task<List<InvoiceGridViewModel>> BuyerInvoiceGridByOrder(int orderId, int userId, int invoiceType)
        {
            return await ContextFactory.Current.GetDomain<InvoiceDomain>().GetBuyerInvoiceGridByOrderAsync(userId, orderId, invoiceType);
        }

        [HttpPost]
        public async Task<StatusViewModel> DealAgree([FromUri()] int discountId, [FromUri()] int invoiceId, [FromUri()] int invoiceHeaderId, [FromBody()] UserContext userContext)
        {
            return await ContextFactory.Current.GetDomain<InvoiceDomain>().DealAgreeAsync(userContext, discountId, invoiceId, invoiceHeaderId);
        }

        [HttpPost]
        public async Task<StatusViewModel> DeclineDeal([FromUri()] int discountId, [FromUri()] int invoiceId, [FromBody()] UserContext userContext)
        {
            return await ContextFactory.Current.GetDomain<InvoiceDomain>().DealNotAgreeAsync(userContext, discountId, invoiceId);
        }

        [HttpGet]
        public async Task<List<DiscountSummaryViewModel>> DiscountGrid(int invoiceId)
        {
            return await ContextFactory.Current.GetDomain<InvoiceDomain>().GetDiscountSummary(invoiceId);
        }

        [HttpPost]
        public async Task<StatusViewModel> SaveDiscount(SaveDiscountRequestModel model)
        {
            return await ContextFactory.Current.GetDomain<InvoiceDomain>().SaveDiscountAsync(model.ViewModel, model.UserContext);
        }

        [HttpGet]
        public async Task<List<InvoiceHistoryGridViewModel>> GetInvoiceHistoryGridBuyerAsync(int userId, int id = 0)
        {
            return await ContextFactory.Current.GetDomain<InvoiceDomain>().GetInvoiceHistoryGridBuyerAsync(userId, id);
        }

        [HttpGet]
        public async Task<List<InvoiceApprovalHistoryGridViewModel>> InvoiceApprovalHistoryGrid(int id = 0)
        {
            return await ContextFactory.Current.GetDomain<InvoiceDomain>().GetInvoiceApprovalHistoryGridAsync(id);
        }

        [HttpGet]
        public async Task<InvoiceDetailViewModel> GetBuyerInvoiceStatusAsync(int id)
        {
            return await ContextFactory.Current.GetDomain<InvoiceDomain>().GetBuyerInvoiceStatusAsync(id);
        }

        [HttpGet]
        public async Task<StatementPdfViewModel> GetStatementPdfDetails(int id)
        {
            return await new BillingStatementDomain().GetStatementPdfDetailsAsync(id);
        }

        [HttpGet]
        public async Task<InvoiceDetailViewModel> GetInvoiceDetailSummary(int id)
        {
            return await new InvoiceDomain().GetInvoiceDetailSummary(id);
        }

        [HttpPost]
        public async Task<List<InvoiceApprovalHistoryGridViewModel>> GetBuyerWaitingForApprovalListAsync([FromUri()] int userId, [FromBody()] InvoiceFilterViewModel invoiceFilter)
        {
            return await ContextFactory.Current.GetDomain<InvoiceDomain>().GetBuyerWaitingForApprovalListAsync(userId, invoiceFilter);
        }

        [HttpPost]
        public async Task<List<MapViewModel>> GetMapDataAsync([FromUri()] int userId, [FromBody()] InvoiceFilterViewModel invoiceFilter)
        {
            return await ContextFactory.Current.GetDomain<InvoiceDomain>().GetMapDataAsync(userId, invoiceFilter);
        }

        [HttpPost]
        public async Task<InvoiceDetailViewModel> GetBuyerInvoiceDetailAsync([FromUri()] int id, [FromBody()] UserContext user)
        {
            return await ContextFactory.Current.GetDomain<InvoiceDomain>().GetBuyerInvoiceDetailAsync(id, user);
        }

        [HttpPost]
        public async Task<StatusViewModel> DeclineInvoice(DeclineInvoiceModel input)
        {
            return await ContextFactory.Current.GetDomain<InvoiceDomain>().DeclineInvoiceAsync(input.UserContext, input.Model);
        }

        [HttpPost]
        public async Task<StatusViewModel> ApproveInvoice([FromUri()] int id, [FromBody()] UserContext user)
        {
            return await ContextFactory.Current.GetDomain<InvoiceDomain>().ApproveInvoiceAsync(user, id);
        }

        [HttpPost]
        public async Task<DeclineInvoiceViewModel> GetDeclineInvoiceDetail([FromUri()] int id, [FromUri()] int statusId, [FromBody()] UserContext user)
        {
            return await ContextFactory.Current.GetDomain<InvoiceDomain>().GetDeclineInvoiceDetailAsync(user, id, statusId);
        }

        [HttpGet]
        public async Task<InvoicePdfViewModel> GetInvoicePdfNewAsync(int id, CompanyType companyType)
        {
            return await ContextFactory.Current.GetDomain<InvoiceDomain>().GetInvoicePdfNewAsync(id, companyType);
        }

        [HttpGet]
        public async Task<List<InvoiceBolEditGrid>> GetMarineInvoiceBolListAsync(int headerId, int companyId)
        {
            var spDomain = ContextFactory.Current.GetDomain<StoredProcedureDomain>();
            return await spDomain.GetMarineInvoiceBolListAsync(companyId, headerId);
        }

        [HttpGet]
        public async Task<List<MarineInspectionRequestVoucherViewModel>> GetMarineInspectionVoucherDocumentInfo(int invoiceHeaderId)
        {
            return await ContextFactory.Current.GetDomain<InvoiceDomain>().GetMarineInspectionVoucherDocumentInfo(invoiceHeaderId);
        }

        [HttpGet]
        public async Task<MarineTaxAffidavitPdfViewModel> GetMarineTaxAffidavitInfo(int invoiceHeaderId)
        {
            return await ContextFactory.Current.GetDomain<InvoiceDomain>().GetMarineTaxAffidavitInfo(invoiceHeaderId);
        }

        [HttpGet]
        public async Task<MarineBDNPdfViewModel> GetMarineBDNInfo(int invoiceHeaderId)
        {
            return await ContextFactory.Current.GetDomain<InvoiceDomain>().GetMarineBDNInfo(invoiceHeaderId);
        }

        [HttpGet]
        public async Task<MarineCGInspectionDocumentPdfViewModel> GetMarineCGInspectionDocumentInfo(int invoiceHeaderId)
        {
            return await ContextFactory.Current.GetDomain<InvoiceDomain>().GetMarineCGInspectionDocumentInfo(invoiceHeaderId);
        }

        [HttpPost]
        public async Task<StatusViewModel> ValidatePoNumberBulkFile(BulkUploadRequestModel input)
        {
            return await ContextFactory.Current.GetDomain<PoNumberBulkUploadDomain>().ValidatePoNumberBulkFile(input.UserContext, input.CsvText, input.CsvFilePath, input.CompanyType);
        }

        [HttpPost]
        public async Task<StatusViewModel> ValidateInvoiceBulkFile(BulkUploadRequestModel input)
        {
            var bulkDomain = ContextFactory.Current.GetDomain<InvoiceBulkUploadDomain>();

            var response = await bulkDomain.ValidateInvoiceFile(input.UserContext, input.CsvText, input.CsvFilePath);

            return response;
        }

        [HttpPost]
        public async Task<StatusViewModel> PayConfirm([FromUri()] int userId, [FromBody()] InvoiceDetailViewModel viewModel)
        {
            return await ContextFactory.Current.GetDomain<InvoiceDomain>().PayConfirmInvoiceAsync(viewModel, userId);
        }

        [HttpPost]
        public async Task<StatusViewModel> PoNumberBulkUpload(PoNumberBulkCsvViewModel viewModel, string csvText, string csvFilePath, HttpPostedFileBase csvFile, UserContext user)
        {
            var invoiceDomain = new PoNumberBulkUploadDomain();
            return await invoiceDomain.ValidatePoNumberBulkFile(user, csvText, csvFilePath, CompanyType.Buyer);
        }

        [HttpPost]
        public async Task<List<InvoiceBolGridViewModel>> BuyerBolInvoiceGrid(BolGridRequestModel input)
        {
            var dashboardDomain = new DashboardDomain();
            var invoiceDomain = new InvoiceDomain(dashboardDomain);
            input.RequestModel.GroupIds = dashboardDomain.DecryptData(input.RequestModel.GroupIds);

            return await invoiceDomain.GetBuyerBolInvoicesAsync(input.UserContext.Id, input.UserContext.CompanyId, input.RequestModel, input.View, input.UserContext.BrandedCompanyId);
        }

        [HttpPost]
        public async Task<List<InvoiceGridViewModel>> GetSupplierInvoiceGridAsync(InvoiceGridRequestModel model)
        {
            var invoiceDomain = new InvoiceDomain();
            var response = await invoiceDomain.GetSupplierInvoiceGridAsync(model.CompanyId, model.ViewModel, model.InvoiceType, model.View);
            return response;
        }

        [HttpPost]
        public async Task<StatusViewModel> EditInvoicePoNumber(EditInvoicePoNumberReqModel model)
        {
            StatusViewModel response = await ContextFactory.Current.GetDomain<InvoiceDomain>().EditInvoicePoNumberAsync(model.UserContext, model.InvoiceId, model.PoNumber);
            return response;
        }

        [HttpGet]
        public async Task<List<InvoiceHistoryGridViewModel>> InvoiceHistoryGrid(int id, int userId)
        {
            var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetInvoiceHistoryGridAsync(id, userId);
            return response;
        }

        [HttpPost]
        public async Task<InvoiceDetailViewModel> GetSupplierInvoiceDetail([FromUri()] int id, [FromBody()] UserContext userContext)
        {
            var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetSupplierInvoiceDetailAsync(id, userContext.CompanyId, userContext);
            return response;
        }

        [HttpGet]
        public async Task<InvoicePdfViewModel> PartialInvoicePdf(int id, CompanyType companyType)
        {
            var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetInvoicePdfNewAsync(id, companyType);
            return response;
        }

        [HttpGet]
        public async Task<int> GetInvoiceHeaderIdByIdAsync(int id)
        {
            var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetInvoiceHeaderIdByIdAsync(id);
            return response;
        }

        [HttpPost]
        public async Task<ConsolidatedInvoicePdfViewModel> GetConsolidatedInvoicePdfAsync(InvoicePdfRequestModel input)
        {
            var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetConsolidatedInvoicePdfAsync(input.InvoiceHeaderId, input.CompanyType, input.UserContext);
            return response;
        }

        [HttpPost]
        public async Task<BDRPdfViewModel> BDRPdf(InvoicePdfRequestModel input)
        {
            var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetBDRPdfAsync(input.InvoiceHeaderId, input.CompanyType, input.UserContext);
            return response;
        }

        [HttpPost]
        public async Task<InvoiceDetailViewModel> DownloadBDRSummary(InvoicePdfRequestModel input)
        {
            var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetSupplierInvoiceDetailAsync(input.InvoiceHeaderId, input.UserContext.CompanyId, input.UserContext);
            return response;
        }

        [HttpGet]
        public async Task<DryRunInvoiceViewModel> GetDryRunInvoice(int id, int currentUserId)
        {
            var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetDryRunInvoiceAsync(id, currentUserId);
            return response;
        }

        [HttpGet]
        public async Task<DryRunInvoiceViewModel> GetDryRunInvoiceForEdit(int id, int currentUserId)
        {
            var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetDryRunInvoiceForEditAsync(id, currentUserId);
            return response;
        }

        [HttpPost]
        public async Task<StatusViewModel> CreateDryRunInvoice(DryRunInvoiceViewModel viewModel)
        {
            var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().CreateDryRunInvoiceAsync(viewModel);
            return response;
        }

        [HttpGet]
        public async Task<List<AssignToOrderGridViewModel>> AssignToOrderGrid(int currentUserId)
        {
            var response = await ContextFactory.Current.GetDomain<OrderDomain>().GetAssignToOrderGridAsync(currentUserId);
            return response;
        }

        [HttpGet]
        public async Task<AssignToOrderPreviewViewModel> OrderPreView(int orderId, int invoiceId)
        {
            var response = await ContextFactory.Current.GetDomain<OrderDomain>().GetOrderPreviewAsync(orderId, invoiceId);
            return response;
        }

        [HttpGet]
        public async Task<AssignToOrderViewModel> AssignInvoiceToOrder(int orderId, int invoiceId, int currentUserId)
        {
            var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().AssignInvoiceToOrderAsync(orderId, invoiceId, currentUserId);
            return response;
        }

        [HttpGet]
        public async Task<ManualInvoiceViewModel> GetManualInvoice(int orderId)
        {
            var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetManualInvoiceAsync(orderId);
            return response;
        }

        [HttpGet]
        public async Task<ManualInvoiceViewModel> GetOrderDetailsForBalanceInvoice(int orderId)
        {
            var response = await ContextFactory.Current.GetDomain<BalanceInvoiceDomain>().GetManualInvoiceAsync(orderId);
            return response;
        }

        [HttpGet]
        public async Task<ManualInvoiceViewModel> GetOrderDetailsForTankRentalInvoice(int orderId)
        {
            var response = await ContextFactory.Current.GetDomain<TankRentalInvoiceDomain>().GetManualInvoiceAsync(orderId);
            return response;
        }

        [HttpGet]
        public async Task<ManualInvoiceViewModel> GetManualInvoiceForEdit(int id)
        {
            var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetManualInvoiceForEditAsync(id);
            return response;
        }

        [HttpPost]
        public async Task<StatusViewModel> CreateManualFtlInvoice(CreateInvoiceViewModel input)
        {
            var response = await ContextFactory.Current.GetDomain<InvoiceCreateDomain>().CreateManualFtlInvoiceAsync(input.UserContext, input.ViewModel);
            return response;
        }

        [HttpPost]
        public async Task<StatusViewModel> CreateConsolidatedInvoice(CreateConsolidateInvoiceModel input)
        {
            var domain = ContextFactory.Current.GetDomain<ConsolidatedInvoiceDomain>();
            return await domain.CreateManualInvoice(input.UserContext, input.ViewModel);
        }

        [HttpPost]
        public async Task<InvoiceCreateResponseViewModel> CreateBalanceInvoiceAsync(CreateInvoiceViewModel input)
        {
            var domain = ContextFactory.Current.GetDomain<BalanceInvoiceDomain>();
            return await domain.CreateBalanceInvoiceAsync(input.UserContext, input.ViewModel);
        }

        [HttpPost]
        public async Task<InvoiceCreateResponseViewModel> CreditRebillBalanceInvoiceAsync(CreateInvoiceViewModel input)
        {
            var domain = ContextFactory.Current.GetDomain<BalanceInvoiceDomain>();
            return await domain.CreditRebillBalanceInvoiceAsync(input.UserContext, input.ViewModel);
        }

        [HttpPost]
        public async Task<InvoiceCreateResponseViewModel> CreateTankRentalInvoiceAsync(CreateInvoiceViewModel input)
        {
            var domain = ContextFactory.Current.GetDomain<TankRentalInvoiceDomain>();
            return await domain.CreateTankRentalInvoiceAsync(input.UserContext, input.ViewModel);
        }

        [HttpPost]
        public async Task<InvoiceCreateResponseViewModel> RebillTankRentalInvoiceAsync(CreateInvoiceViewModel input)
        {
            var domain = ContextFactory.Current.GetDomain<TankRentalInvoiceDomain>();
            return await domain.RebillTankRentalInvoiceAsync(input.UserContext, input.ViewModel);
        }

        [HttpPost]
        public async Task<List<InvoiceBolGridViewModel>> GetSupplierBolInvoicesAsync(InvoiceGridRequestModel input)
        {
            var domain = ContextFactory.Current.GetDomain<InvoiceDomain>();
            return await domain.GetSupplierBolInvoicesAsync(input.CompanyId, input.ViewModel, input.View);
        }

        [HttpPost]
        public async Task<StatusViewModel> CreateManualInvoice(CreateInvoiceViewModel input)
        {
            var response = await ContextFactory.Current.GetDomain<InvoiceCreateDomain>().CreateManualInvoiceAsync(input.UserContext, input.ViewModel);
            return response;
        }

        [HttpPost]
        public async Task<ManualInvoiceViewModel> GetAssetsForInvoice(ManualInvoiceViewModel viewModel)
        {
            var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetAssetsForInvoice(viewModel);
            return response;
        }

        [HttpPost]
        public async Task<StatusViewModel> EditDraftDDTAsync(CreateInvoiceViewModel input)
        {
            var response = await ContextFactory.Current.GetDomain<InvoiceEditDomain>().EditDraftDDTAsync(input.UserContext, input.ViewModel);
            return response;
        }

        [HttpPost]
        public async Task<StatusViewModel> CreateInvoiceFromDropTicketForNonStandardFuel(CreateInvoiceViewModel input)
        {
            var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().CreateInvoiceFromDropTicketForNonStandardFuel(input.UserContext, input.UserContext.Id, input.ViewModel);
            return response;
        }

        [HttpPost]
        public async Task<StatusViewModel> CreateInvoiceFromDropTicketWithBol(CreateInvoiceViewModel input)
        {
            var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().CreateInvoiceFromDropTicketWithBol(input.ViewModel, input.UserContext);
            return response;
        }

        [HttpPost]
        public async Task<StatusViewModel> InvoiceEdit(CreateInvoiceViewModel input)
        {
            var response = await ContextFactory.Current.GetDomain<InvoiceEditDomain>().InvoiceEditAsync(input.UserContext, input.ViewModel);
            return response;
        }

        [HttpGet]
        public async Task<InvoiceViewModelNew> GetOriginalInvoiceDetails(int invoiceId, int companyId)
        {
            return await ContextFactory.Current.GetDomain<InvoiceDomain>().GetOriginalInvoiceDetails(invoiceId, companyId);
        }

        [HttpPost]
        public async Task<StatusViewModel> RebillInvoiceAsync(CreateInvoiceViewModel input)
        {
            var response = await ContextFactory.Current.GetDomain<CreditRebillInvoiceDomain>().RebillInvoiceAsync(input.UserContext, input.ViewModel);
            return response;
        }

        [HttpPost]
        public async Task<StatusViewModel> CancelDraftAsync(InvoicePdfRequestModel input)
        {
            var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().CancelDraftAsync(input.InvoiceHeaderId, input.UserContext);
            return response;
        }

        [HttpPost]
        public async Task<StatusViewModel> ConvertDdtToInvoiceManually(ConvertToInvoiceRequest input)
        {
            var response = await ContextFactory.Current.GetDomain<ConsolidatedDdtToInvoiceDomain>().ConvertDdtToInvoiceManually(input.UserContext, input.InvoiceId, input.IsConvertToInv);
            return response;
        }

        [HttpGet]
        public async Task<StatusViewModel> ValidateGravityByInvoiceId(int invoiceId, decimal gravity)
        {
            var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().ValidateGravityByInvoiceId(invoiceId, gravity);
            return response;
        }

        [HttpGet]
        public async Task<MFNConversionResponseViewModel> IsValidApiGravity(int invoiceId, decimal gravity)
        {
            var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().IsValidApiGravity(invoiceId, gravity);
            return response;
        }

        [HttpPost]
        public async Task<MFNConversionResponseViewModel> ValidateGravityAndConvertForMFN(MFNConversionRequestViewModel conversionRequest)
        {
            var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().ValidateGravityAndConvertForMFN(conversionRequest);
            return response;
        }

        [HttpPost]
        public async Task<NewsfeedMessagesViewModel> GetNewsfeed([FromBody()] UserContext userContext, [FromUri()] int entityId, [FromUri()] int currentPage, [FromUri()] int latestId, [FromUri()] EntityType entityTypeId)
        {
            var response = await ContextFactory.Current.GetDomain<NewsfeedDomain>().GetNewsfeed(userContext, entityTypeId, entityId, currentPage, latestId);
            return response;
        }

        [HttpPost]
        public async Task<StatusViewModel> EditInvoiceNumber([FromBody()] UserContext userContext, [FromUri()] int invoiceId, [FromUri()] string displayInvoiceNumber)
        {
            var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().EditInvoiceNumberAsync(userContext, invoiceId, displayInvoiceNumber);
            return response;
        }

        [HttpPost]
        public async Task<StatusViewModel> CreateInvoiceFromDraftDdt(CreateInvoiceViewModel input)
        {
            var response = await ContextFactory.Current.GetDomain<InvoiceCreateDomain>().CreateInvoiceFromDraftDdtAsync(input.UserContext, input.ViewModel);
            return response;
        }

        [HttpGet]
        public async Task<InvoiceDropViewModel> GetInvoiceDropModel(int orderId)
        {
            var invoiceCreate = new InvoiceCreateDomain();
            return await invoiceCreate.GetInvoiceDropModel(orderId);
        }

        [HttpGet]
        public async Task<List<AssetDropViewModel>> GetAssignedAssets(int orderId)
        {
            var invoiceCommonDomain = new InvoiceDomain();
            return await invoiceCommonDomain.GetAssignedAssetsAsync(orderId);
        }

        [HttpPost]
        public async Task<StatusViewModel> SaveBDNInvoiceDetails(UpdateBDNInvoiceDetailModel input)
        {
            return await ContextFactory.Current.GetDomain<ConsolidatedInvoiceDomain>().UpdateInvoices(input.Model, input.UserContext);
        }

        [HttpPost]
        public async Task<decimal> GetTerminalPrice(GetTerminalPriceModel input)
        {
            var invoiceCommonDomain = new InvoiceCommonDomain();
            var viewModel = await invoiceCommonDomain.GetFuelPricingRequestViewModel(input.OrderId, input.DeliveryDate);
            if (input.TerminalId > 0)
            {
                viewModel.TerminalId = input.TerminalId;
            }
            var result = await invoiceCommonDomain.GetFuelPriceAsync(viewModel);
            var response = viewModel.IsTerminalPrice() ? result.TerminalPrice : result.PricePerGallon;
            return response;
        }

        [HttpPost]
        public async Task<List<DropdownDisplayItem>> GetTerminals(GetTerminalsModel input)
        {
            var response = await ContextFactory.Current.GetDomain<ExternalPricingDomain>().GetClosestTerminals(input.OrderId, input.Terminal);
            return response;
        }


        [HttpPost]
        public async Task<List<DropdownDisplayItem>> GetBulkPlantsForAutoFreightMethod(GetBulkPlantsModel input)
        {
            var response = await ContextFactory.Current.GetDomain<ExternalPricingDomain>().GetBulkPlantsForAutoFreightMethod(input.OrderId, input.BulkPlant);
            return response;
        }

        [HttpPost]
        public async Task<InvoiceViewModelNew> GetPoDetailsToCreateInvoice([FromBody()] InvoiceServiceRequestModel user, [FromUri()] int orderId, [FromUri()] int trackableScheduleId)
        {
            var invoiceCreate = new InvoiceCreateDomain();
            return await invoiceCreate.GetPoDetailsToCreateInvoice(user.UserContext, orderId, trackableScheduleId);
        }

        [HttpPost]
        public async Task<StatusViewModel> ConvertToInvoice([FromBody] CreateConsolidateInvoiceModel invoiceViewModel, [FromUri] int ddtId)
        {
            var response = await ContextFactory.Current.GetDomain<ConsolidatedDdtToInvoiceDomain>().ConvertDdtToInvoiceWithBolManually(invoiceViewModel.UserContext, ddtId, invoiceViewModel.ViewModel);
            return response;
        }

        [HttpPost]
        public async Task<List<DropQuantityByPrePostDipResponseModel>> CalculateDropQuantitiesFromPrePostForCreateInvoice(List<AssetDropViewModel> assetInfoList)
        {
            var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().CalculateDropQuantitiesFromPrePostForCreateInvoice(assetInfoList);
            return response;
        }

        [HttpGet]
        public async Task<DecimalResponseModel> GetGallonsPerMetricTonAsync(decimal gravity)
        {
            DecimalResponseModel response = new DecimalResponseModel();
            if (gravity > 0)
            {
                response = await ContextFactory.Current.GetDomain<OrderDomain>().GetGallonsPerMetricTonAsync(Math.Round(gravity, 1));
            }
            return response;
        }

        [HttpGet]
        public async Task<StatusViewModel> DeleteBolForMarineInvoice(int invoiceHeaderId, int invoiceFtlDetailsId, int invoiceId)
        {
            var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().DeleteBolForMarineInvoice(invoiceHeaderId, invoiceFtlDetailsId, invoiceId);
            return response;
        }

        [HttpGet]
        public async Task<bool> TriggerDailyDataDumpReportCreation()
        {
            var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().CreateDeliveryDetailsDailyDataDumpReport();
            return response;
        }
    }
}
