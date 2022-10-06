using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Asset;
using SiteFuel.Exchange.ViewModels.BillingStatement;
using SiteFuel.Exchange.ViewModels.Dispatcher;
using SiteFuel.Exchange.ViewModels.ExternalEntityMappings;
using SiteFuel.Exchange.ViewModels.FuelPricing;
using SiteFuel.Exchange.ViewModels.Invoice.Pdf;
using SiteFuel.Exchange.ViewModels.Job;
using SiteFuel.Exchange.ViewModels.MFN;
using SiteFuel.Exchange.ViewModels.Offer;
using SiteFuel.Exchange.ViewModels.Order;
using SiteFuel.Exchange.ViewModels.Quickbooks;
using SiteFuel.Exchange.ViewModels.TankRental;
using SiteFuel.Exchange.ViewModels.WebNotification;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class StoredProcedureDomain : BaseDomain
    {
        public StoredProcedureDomain()
            : base(ContextFactory.Current.ConnectionString)
        {
        }

        public StoredProcedureDomain(BaseDomain domain)
            : base(domain)
        {
        }


        public List<SalesCalculatorGridViewModel> GetTerminalPricesForCalculator(SalesCalculatorInputViewModel model, int timeout = 30)
        {
            var response = new List<SalesCalculatorGridViewModel>();
            try
            {
                TimeSpan duration = new TimeSpan(0, 23, 59, 59, 999);
                model.PricingDate = model.PricingDate.Date.Add(duration);
                var input = SqlHelperMethods.GetStoredProcedure("GetTerminalPricesForCalculator", model);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = Context.DataContext.Database.SqlQuery<SalesCalculatorGridViewModel>(input.Query, input.Params.ToArray()).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetTerminalPricesForCalculator", ex.Message, ex);
            }
            return response;
        }

        public async Task<ScheduleStatusUpdateInput> GetInfoToCreateTrackableSchedules(int orderId, int timeout = 30)
        {
            var response = new ScheduleStatusUpdateInput();
            try
            {
                var inputmodel = new { OrderId = orderId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetInfoToCreateSchedules", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<ScheduleStatusUpdateInput>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetInfoToCreateTrackableSchedules", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<UspDeliveryDetails>> GetDeliveryDetailsAsync(int invoiceHeaderId, int timeout = 30)
        {
            var response = new List<UspDeliveryDetails>();
            var inputmodel = new { InvoiceHeaderId = invoiceHeaderId };
            var input = SqlHelperMethods.GetStoredProcedure("usp_GetDeliveryDetails", inputmodel);

            Context.DataContext.Database.CommandTimeout = timeout;
            response = await Context.DataContext.Database.SqlQuery<UspDeliveryDetails>(input.Query, input.Params.ToArray()).ToListAsync();

            return response;
        }

        public async Task<List<Usp_DeliveryScheduleHistoryViewModel>> GetOrderVersionHistory(int orderId, int timeout = 30)
        {
            var response = new List<Usp_DeliveryScheduleHistoryViewModel>();
            try
            {
                var inputmodel = new { @OrderId = orderId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetOrderScheduleHistory", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<Usp_DeliveryScheduleHistoryViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetOrderVersionHistory", ex.Message, ex);
            }
            return response;
        }

        public async Task<UspGetTerminalRackPrice> GetTerminalRackPrice(int TerminalId, DateTimeOffset PricingDate, int ProductId, int timeout = 30)
        {
            var response = new UspGetTerminalRackPrice();
            try
            {
                var inputmodel = new { TerminalId, PricingDate, ProductId };
                var input = SqlHelperMethods.GetStoredProcedure("GetTerminalRackPrice", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspGetTerminalRackPrice>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetTerminalPricesForCalculator", ex.Message, ex);
            }
            return response;
        }

        public async Task<UspTPOViewModel> GetTPOOrderDetails(int orderId, int timeout = 30)
        {
            var response = new UspTPOViewModel();
            try
            {
                var inputmodel = new { OrderId = orderId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetTPOOrderDetails", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspTPOViewModel>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetTPOOrderDetails", ex.Message, ex);
            }
            return response;
        }



        public async Task<List<DipTestSummaryViewModel>> GetLocationInventoryDiptestSummaryAsync(InventoryDataCaptureType inventoryCaptureType, string jobIds, int timeout = 30)
        {
            var response = new List<DipTestSummaryViewModel>();
            try
            {
                var inputmodel = new { lastUpdated = (int)inventoryCaptureType, jobIds = jobIds };

                var input = SqlHelperMethods.GetStoredProcedure("usp_GetLocationInventoryDiptestSummary", inputmodel);


                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<DipTestSummaryViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetLocationInventoryDiptestSummaryAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DipTestSummaryViewModel>> GetDiptestSummaryAsync(int lastUpdated, string jobIds, int timeout = 30)
        {
            var response = new List<DipTestSummaryViewModel>();
            try
            {
                var inputmodel = new { lastUpdated = lastUpdated, jobIds = jobIds };

                var input = SqlHelperMethods.GetStoredProcedure("usp_GetDiptestSummary", inputmodel);


                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<DipTestSummaryViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetDiptestSummaryAsync", ex.Message, ex);
            }
            return response;
        }


        public async Task<List<DipTestRequestModel>> GetLastUpdatedDiptestCompanies(int inventoryCaptureType, int timeout = 120)
        {
            var response = new List<DipTestRequestModel>();
            try
            {
                var inputmodel = new { inventoryCapture = inventoryCaptureType };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetLastUpdatedDiptestCompanies", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                LogManager.Logger.WriteDebug("StoredProcedureDomain", "GetLastUpdatedDiptestCompanies", "diptest method " + inventoryCaptureType.ToString());

                response = await Context.DataContext.Database.SqlQuery<DipTestRequestModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetLastUpdatedDiptestCompanies", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<UspStatementInvoiceDetails>> GetStatementInvoiceSummary(int statementId, int timeout = 30)
        {
            var response = new List<UspStatementInvoiceDetails>();
            try
            {
                var inputmodel = new { StatementId = statementId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetBillingStatementInvoiceDetails", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspStatementInvoiceDetails>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetStatementInvoiceSummary", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<UspStatementPdfInvoiceDetails>> GetStatementPdfInvoiceDetails(int statementId, int timeout = 30)
        {
            var response = new List<UspStatementPdfInvoiceDetails>();
            try
            {
                var inputmodel = new { StatementId = statementId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetStatementPdfInvoiceDetails", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspStatementPdfInvoiceDetails>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetStatementPdfInvoiceDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<OrderDropDetailsViewModel>> GetOrderDropDetails(int orderId, int timeout = 30)
        {
            var response = new List<OrderDropDetailsViewModel>();
            try
            {
                var inputmodel = new { OrderId = orderId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetOrderDropDetails", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<OrderDropDetailsViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetOrderDropDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<UspOrderPdfDetails> GetSupplierOrderPdfDetailsAsync(int orderId, int orderDetailVersionId = 0, int timeout = 30)
        {
            var response = new UspOrderPdfDetails();
            try
            {
                var inputmodel = new { OrderId = orderId, OrderDetailVersionId = orderDetailVersionId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetOrderPdfDetails", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspOrderPdfDetails>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetSupplierOrderPdfDetailsAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<UspFuelSurchargeSummary>> GetFuelSurchargeSummary(ViewFuelSurchargeInputViewModel viewModel, int supplierCompanyId, int timeout = 30)
        {
            var response = new List<UspFuelSurchargeSummary>();
            try
            {
                if (!string.IsNullOrEmpty(viewModel.BulkPlantIds))
                {
                    var bulkPlants = viewModel.BulkPlantIds.Split(',');
                    var bulkPlantIds = new List<int>();
                    foreach (var item in bulkPlants)
                    {
                        bulkPlantIds.Add(Int32.Parse(item.Split("_".ToCharArray())[1]));
                    }
                    viewModel.BulkPlantIds = string.Join(",", bulkPlantIds);
                }

                if (!string.IsNullOrEmpty(viewModel.TerminalIds))
                {
                    var terminals = viewModel.TerminalIds.Split(',');
                    var terminalIds = new List<int>();
                    foreach (var item in terminals)
                    {
                        terminalIds.Add(Int32.Parse(item.Split("_".ToCharArray())[1]));
                    }
                    viewModel.TerminalIds = string.Join(",", terminalIds);
                }

                var startDate = viewModel.StartDate;
                var endDate = viewModel.EndDate;


                var inputmodel = new
                {
                    SupplierCompanyId = supplierCompanyId,
                    TableTypeIds = viewModel.TableTypeIds,
                    CustomerIds = viewModel.CustomerIds,
                    CarrierIds = viewModel.CarrierIds,
                    SourceRegionIds = viewModel.SourceRegionIds,
                    TerminalIds = viewModel.TerminalIds,
                    BulkPlantIds = viewModel.BulkPlantIds,
                    IsArchived = viewModel.IsArchived,
                    StartDate = startDate,
                    EndDate = endDate
                };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetFuelSurchargeSummary", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspFuelSurchargeSummary>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetFuelSurchargeSummary", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<UspAccessorialFeeSummary>> GetAccessorialFeeSummary(ViewAccessorialFeeInputViewModel viewModel, int supplierCompanyId, int timeout = 30)
        {
            var response = new List<UspAccessorialFeeSummary>();
            try
            {
                if (!string.IsNullOrEmpty(viewModel.BulkPlantIds))
                {
                    var bulkPlants = viewModel.BulkPlantIds.Split(',');
                    var bulkPlantIds = new List<int>();
                    foreach (var item in bulkPlants)
                    {
                        bulkPlantIds.Add(Int32.Parse(item.Split("_".ToCharArray())[1]));
                    }
                    viewModel.BulkPlantIds = string.Join(",", bulkPlantIds);
                }

                if (!string.IsNullOrEmpty(viewModel.TerminalIds))
                {
                    var terminals = viewModel.TerminalIds.Split(',');
                    var terminalIds = new List<int>();
                    foreach (var item in terminals)
                    {
                        terminalIds.Add(Int32.Parse(item.Split("_".ToCharArray())[1]));
                    }
                    viewModel.TerminalIds = string.Join(",", terminalIds);
                }

                var startDate = viewModel.StartDate;
                var endDate = viewModel.EndDate;


                var inputmodel = new
                {
                    SupplierCompanyId = supplierCompanyId,
                    TableTypeIds = viewModel.TableTypeIds,
                    CustomerIds = viewModel.CustomerIds,
                    CarrierIds = viewModel.CarrierIds,
                    SourceRegionIds = viewModel.SourceRegionIds,
                    TerminalIds = viewModel.TerminalIds,
                    BulkPlantIds = viewModel.BulkPlantIds,
                    IsArchived = viewModel.IsArchived,
                    StartDate = startDate,
                    EndDate = endDate
                };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetAccessorialFeeSummary", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspAccessorialFeeSummary>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetAccessorialFeeSummary", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<UspFreightRateSummary>> GetFreightRateSummary(ViewFreightRateInputViewModel viewModel, int supplierCompanyId, int timeout = 30)
        {
            var response = new List<UspFreightRateSummary>();
            try
            {
                if (!string.IsNullOrEmpty(viewModel.BulkPlantIds))
                {
                    var bulkPlants = viewModel.BulkPlantIds.Split(',');
                    var bulkPlantIds = new List<int>();
                    foreach (var item in bulkPlants)
                    {
                        bulkPlantIds.Add(Int32.Parse(item.Split("_".ToCharArray())[1]));
                    }
                    viewModel.BulkPlantIds = string.Join(",", bulkPlantIds);
                }

                if (!string.IsNullOrEmpty(viewModel.TerminalIds))
                {
                    var terminals = viewModel.TerminalIds.Split(',');
                    var terminalIds = new List<int>();
                    foreach (var item in terminals)
                    {
                        terminalIds.Add(Int32.Parse(item.Split("_".ToCharArray())[1]));
                    }
                    viewModel.TerminalIds = string.Join(",", terminalIds);
                }

                var startDate = viewModel.StartDate;
                var endDate = viewModel.EndDate;


                var inputmodel = new
                {
                    SupplierCompanyId = supplierCompanyId,
                    TableTypeIds = viewModel.TableTypeIds,
                    FreightRateRuleTypeIds = viewModel.FreightRateRuleTypeIds,
                    CustomerIds = viewModel.CustomerIds,
                    CarrierIds = viewModel.CarrierIds,
                    SourceRegionIds = viewModel.SourceRegionIds,
                    TerminalIds = viewModel.TerminalIds,
                    BulkPlantIds = viewModel.BulkPlantIds,
                    IsArchived = viewModel.IsArchived,
                    StartDate = startDate,
                    EndDate = endDate
                };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetFreightRateSummary", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspFreightRateSummary>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetFreightRateSummary", ex.Message, ex);
            }
            return response;
        }


        public async Task<List<UspFuelGroupSummary>> GetFuelGroupSummary(int supplierCompanyId, int timeout = 30)
        {
            var response = new List<UspFuelGroupSummary>();
            try
            {
                var inputmodel = new
                {
                    SupplierCompanyId = supplierCompanyId,
                };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetFuelGroupSummary", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspFuelGroupSummary>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetFuelGroupSummary", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<BulkPlantViewModel>> GetNearestBulkPlantsForDriver(int supplierId, decimal latitude, decimal longitude, int timeout = 30)
        {
            var response = new List<BulkPlantViewModel>();
            try
            {
                var inputmodel = new { SupplierId = supplierId, Latitude = latitude, Longitude = longitude };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetNearestBulkPlant", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<BulkPlantViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetNearestBulkPlantsForDriver", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetBulkPlants(int companyId, string stateIds = "", string cityIds = "", int timeout = 30)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                if (stateIds == null)
                    stateIds = "";
                if (cityIds == null)
                    cityIds = "";

                var inputmodel = new { CompanyId = companyId, ServingStates = stateIds, ServingCities = cityIds };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetBulkPlants", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<DropdownDisplayItem>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetBulkPlants", ex.Message, ex);
            }
            return response;
        }

        public async Task<int> SaveNewProductAsync(string ProductName, int ProductId, int ProductTypeId, int ProductDisplayGroupId, int PricingSourceId, int? CompanyId = null, int? TfxProductId = null, string DisplayName = null, int timeout = 30)
        {
            var response = 0;
            try
            {
                var inputmodel = new { ProductName, ProductId, ProductTypeId, ProductDisplayGroupId, PricingSourceId, CompanyId, TfxProductId, DisplayName };
                var input = SqlHelperMethods.GetStoredProcedure("usp_SaveNewProduct", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<int>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "SaveNewProductAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<int> SaveNewTfxProductAsync(int ProductId, string DisplayName, string ProductCode, int ProductDisplayGroupId, int ProductTypeId, string ProductDescription, int timeout = 30)
        {
            var response = 0;
            try
            {
                var inputmodel = new { ProductId, DisplayName, ProductCode, ProductDisplayGroupId, ProductTypeId, ProductDescription };
                var input = SqlHelperMethods.GetStoredProcedure("usp_SaveNewtfxProduct", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<int>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "SaveNewProductAsync", ex.Message, ex);
            }
            return response;
        }

        /// <summary>
        /// Saves the new opis product asynchronous.
        /// </summary>
        /// <param name="ProductName">Name of the product.</param>
        /// <param name="ProductId">The product identifier.</param>
        /// <param name="ProductTypeId">The product type identifier.</param>
        /// <param name="ProductDisplayGroupId">The product display group identifier.</param>
        /// <param name="PricingSourceId">The pricing source identifier.</param>
        /// <param name="ProductCode">The product code.</param>
        /// <param name="TfxProductId">The TFX product identifier.</param>
        /// <param name="MstProductId">The MST product identifier.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns></returns>
        public async Task<int> SaveNewOPISProductAsync(string ProductName, int ProductId, int PricingSourceId, string ProductCode, int? TfxProductId, int? MstProductId, int timeout = 30)
        {
            var response = 0;
            try
            {
                var inputmodel = new
                {
                    ProductName,
                    ProductId,
                    PricingSourceId,
                    ProductCode,
                    TfxProductId,
                    MstProductId
                };
                var input = SqlHelperMethods.GetStoredProcedure("usp_SaveNewOPISProduct", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<int>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "SaveNewOPISProductAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<UspStatementDetails> GetStatementPdfDetails(int statementId, int timeout = 30)
        {
            var response = new UspStatementDetails();
            try
            {
                var inputmodel = new { StatementId = statementId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetStatementPdfDetails", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspStatementDetails>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetStatementPdfDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<UspGetSupplierOrderDetail> GetSupplierOrderDetailsAsync(int orderId, bool isBrokeredOrder, int companyId, int timeout = 30)
        {
            var response = new UspGetSupplierOrderDetail();
            try
            {
                var inputmodel = new { OrderId = orderId, IsBrokeredOrder = isBrokeredOrder, CompanyId = companyId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetSupplierOrderDetail", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspGetSupplierOrderDetail>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetSupplierOrderDetailsAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<UspGetFuelRequestFeeDetailViewModel>> GetFuelRequestFeeDetailsAsync(int fuelrequestId, int timeout = 30)
        {
            var response = new List<UspGetFuelRequestFeeDetailViewModel>();
            try
            {
                var inputmodel = new { FuelRequestId = fuelrequestId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetFuelRequestFeeDetails", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspGetFuelRequestFeeDetailViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetFuelRequestFeeDetailsAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<SpecialInstructionViewModel>> GetFuelRequestSpecialInstructionsAsync(int fuelrequestId, int timeout = 30)
        {
            var response = new List<SpecialInstructionViewModel>();
            try
            {
                var inputmodel = new { FuelRequestId = fuelrequestId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetFuelRequestSpecialInstructions", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<SpecialInstructionViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetFuelRequestSpecialInstructionsAsync", ex.Message, ex);
            }
            return response;
        }//GetFuelRequestSpecialInstructionsAsync

        public async Task<List<OrderTaxDetailsViewModel>> GetOrderTaxDetailsAsync(int orderId, int timeout = 30)
        {
            var response = new List<OrderTaxDetailsViewModel>();
            try
            {
                var inputmodel = new { OrderId = orderId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetOrderTaxDetails", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<OrderTaxDetailsViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetOrderTaxDetailsAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<UspGetOrderScheduleDetailsViewModel>> GetOrderDeliveryScheduleDetailsAsync(int orderId, DateTimeOffset currentDate, TimeSpan currentTime, int timeout = 30)
        {
            var response = new List<UspGetOrderScheduleDetailsViewModel>();
            try
            {
                var inputmodel = new { OrderId = orderId, CurrentDate = currentDate, CurrentTime = currentTime };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetOrderDeliveryScheduleDetails", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspGetOrderScheduleDetailsViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetOrderDeliveryScheduleDetailsAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<UspGenerateStatement>> GetGenerateStatementDetails(int billingScheduleId, DateTimeOffset stmtStartDate, DateTimeOffset stmtEndDate, int timeout = 30)
        {
            var response = new List<UspGenerateStatement>();
            try
            {
                var inputmodel = new { @ScheduleId = billingScheduleId, @StmtStartDate = stmtStartDate, @StmtEndDate = stmtEndDate };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetGenerateBillingStatementDetails", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspGenerateStatement>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetGenerateStatementDetails", ex.Message, ex);
            }
            return response;
        }


        public List<SalesCalculatorGridViewModel> GetTerminalPricesForAudit(SalesCalculatorInputViewModel model, int timeout = 30)
        {
            var response = new List<SalesCalculatorGridViewModel>();
            try
            {
                TimeSpan duration = new TimeSpan(0, 23, 59, 59, 999);
                model.PricingDate = model.PricingDate.Date.Add(duration);

                var inputModel = new { model.PricingDate, model.ExternalProductId, model.SrcLatitude, model.SrcLongitude, model.RecordCount };
                var input = SqlHelperMethods.GetStoredProcedure("GetTerminalPricesForAudit", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = Context.DataContext.Database.SqlQuery<SalesCalculatorGridViewModel>(input.Query, input.Params.ToArray()).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetTerminalPricesForAudit", ex.Message, ex);
            }
            return response;
        }

        public List<SalesCalculatorGridViewModel> GetCityRackTerminalPricesForCalculator(CityRackCalculatorInputViewModel model, int timeout = 30)
        {
            var response = new List<SalesCalculatorGridViewModel>();
            try
            {
                TimeSpan duration = new TimeSpan(0, 23, 59, 59, 999);
                model.PriceDate = model.PriceDate.Date.Add(duration);

                var input = SqlHelperMethods.GetStoredProcedure("GetCityRackTerminalPricesForCalculator", model);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = Context.DataContext.Database.SqlQuery<SalesCalculatorGridViewModel>(input.Query, input.Params.ToArray()).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetCityRackTerminalPricesForCalculator", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<UspSupplierQuotesGridViewModel>> GetSupplierQuotesGrid(USP_SupplierRequestsViewModel quoteRequestStat, int timeout = 30)
        {
            var response = new List<UspSupplierQuotesGridViewModel>();
            try
            {
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetSupplierQuoteRequests", quoteRequestStat);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspSupplierQuotesGridViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetSupplierQuotesGrid", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetAllProductsForCountry(int countryId, int timeout = 30)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var inputmodel = new { CountryId = countryId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetAllProductsForCountry", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<DropdownDisplayItem>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetAllProductsForCountry", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetAllOfferProductsForCountry(int companyId, int countryId, int sourceId = 1, int timeout = 30)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var inputmodel = new { CountryId = countryId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetAllOfferProductsForCountry", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<DropdownDisplayItem>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetAllOfferProductsForCountry", ex.Message, ex);
            }
            return response;
        }
        public List<ProductListViewModel> GetFuelProducts(int timeout = 30)
        {
            var response = new List<ProductListViewModel>();
            try
            {
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetAllProducts", new { });

                Context.DataContext.Database.CommandTimeout = timeout;
                response = Context.DataContext.Database.SqlQuery<ProductListViewModel>(input.Query, input.Params.ToArray()).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetFuelProducts", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<UspSupplierFuelRequestGridViewModel>> GetSupplierFuelRequestGrid(USP_SupplierRequestsViewModel fuelRequestStat, int timeout = 30)
        {
            var response = new List<UspSupplierFuelRequestGridViewModel>();
            try
            {
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetSupplierFuelRequests", fuelRequestStat);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspSupplierFuelRequestGridViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetSupplierFuelRequestGrid", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<UspSupplierFuelRequestGridViewModel>> GetLatestReceivedFuelRequestsAsync(int companyId, int userId, int timeout = 30)
        {
            var response = new List<UspSupplierFuelRequestGridViewModel>();
            try
            {
                var inputModel = new { CompanyId = companyId, UserId = userId, CurrentDate = DateTimeOffset.Now };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetLatestReceivedFuelRequests", inputModel);
                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspSupplierFuelRequestGridViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetLatestReceivedFuelRequestsAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<USP_SupplierFuelRequestStatViewModel>> GetSupplierFuelRequestStatForDashboard(int companyId, int userId, int fuelTypeId, int countryId, int currencyType, int timeout = 90)
        {
            var response = new List<USP_SupplierFuelRequestStatViewModel>();
            try
            {
                var inputModel = new { CompanyId = companyId, UserId = userId, FuelTypeId = fuelTypeId, CountryId = countryId, CurrencyType = currencyType };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetSupplierFuelRequestStatForDashboard", inputModel);
                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<USP_SupplierFuelRequestStatViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetSupplierFuelRequestStatForDashboard", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<UspBuyerFuelRequestGridViewModel>> GetBuyerFuelRequestGrid(USP_BuyerFRRequestViewModel fuelRequestStat, int timeout = 30)
        {
            var response = new List<UspBuyerFuelRequestGridViewModel>();
            try
            {
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetBuyerFuelRequests", fuelRequestStat);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspBuyerFuelRequestGridViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetBuyerFuelRequestGrid", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<UspBrokerFuelRequestGridViewModel>> GetBrokerFuelRequestGrid(int companyId, BrokerFuelRequestDataTableViewModel requestModel, int timeout = 30)
        {
            var response = new List<UspBrokerFuelRequestGridViewModel>();
            try
            {
                DataTableSearchModel dataTableSearchValues = new DataTableSearchModel(requestModel);
                DateTimeOffset startDate = Convert.ToDateTime(ApplicationConstants.DateRangeFilterStartDate);
                if (!string.IsNullOrEmpty(requestModel.StartDate))
                {
                    startDate = Convert.ToDateTime(requestModel.StartDate).Date;
                }
                DateTimeOffset endDate = DateTimeOffset.Now.Date.AddDays(1);
                if (!string.IsNullOrEmpty(requestModel.EndDate))
                {
                    endDate = Convert.ToDateTime(requestModel.EndDate).Date.AddDays(1);
                }

                object inputModel = new { CompanyId = companyId, StartDate = startDate, EndDate = endDate, requestModel.OrderId, CountryId = requestModel.CountryId, Currency = (int)requestModel.Currency, dataTableSearchValues };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetBrokeredFuelRequests", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspBrokerFuelRequestGridViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetBrokerFuelRequestGrid", ex.Message, ex);
            }
            return response;
        }


        public async Task<List<BrokerOrderDetails>> GetBrokerOrderDetails(int orderId)
        {
            var response = new List<BrokerOrderDetails>();
            try
            {
                object inputModel = new { OrderId = orderId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetBrokeredOrderDetails", inputModel);

                Context.DataContext.Database.CommandTimeout = 30;
                response = await Context.DataContext.Database.SqlQuery<BrokerOrderDetails>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetBrokerOrderDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<BaseballCardDetailsViewModel> GetBaseballCardDetailsAsync(int buyerCompanyId, int supplierCompanyId, int timeout = 30)
        {
            var response = new BaseballCardDetailsViewModel();
            try
            {
                var inputModel = new
                {
                    BuyerCompanyId = buyerCompanyId,
                    SupplierCompanyId = supplierCompanyId
                };

                var input = SqlHelperMethods.GetStoredProcedure("usp_GetSupplierBaseballCard", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<BaseballCardDetailsViewModel>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetBaseballCardDetailsAsync", ex.Message, ex);
            }
            return response;
        }

        public DashboardDriverOrdersViewModel GetDriverDashboardOrdersCount(int companyId, int userId, int timeout = 30)
        {
            var response = new DashboardDriverOrdersViewModel();
            try
            {
                var inputModel = new
                {
                    DriverId = userId,
                    CompanyId = companyId
                };

                var input = SqlHelperMethods.GetStoredProcedure("usp_GetDriverDashboardOrdersCount", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = Context.DataContext.Database.SqlQuery<DashboardDriverOrdersViewModel>(input.Query, input.Params.ToArray()).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetTerminalPricesForCalculator", ex.Message, ex);
            }
            return response;
        }

        public List<DeliveryScheduleViewModel> GetDriverDashboardDeliverySchedules(int companyId, int userId, int timeout = 30)
        {
            var response = new List<DeliveryScheduleViewModel>();
            try
            {
                var inputModel = new
                {
                    DriverId = userId,
                    CompanyId = companyId
                };

                var input = SqlHelperMethods.GetStoredProcedure("usp_GetDriverDashboardDeliverySchedules", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = Context.DataContext.Database.SqlQuery<DeliveryScheduleViewModel>(input.Query, input.Params.ToArray()).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetTerminalPricesForCalculator", ex.Message, ex);
            }
            return response;
        }

        public List<OrderGridViewModel> GetDriverOrders(int companyId, int driverId, OrderFilterViewModel orderFilter = null, int timeout = 30)
        {
            var response = new List<OrderGridViewModel>();
            try
            {
                var orderIds = GetOrdersAssignedToDriver(companyId, driverId, orderFilter.Filter);

                DateTimeOffset StartDate = DateTimeOffset.Now.Date.AddDays(ApplicationConstants.DateFilterDefaultDays);
                DateTimeOffset EndDate = DateTimeOffset.Now.Date.AddDays(1);
                if (!string.IsNullOrEmpty(orderFilter.StartDate))
                {
                    StartDate = Convert.ToDateTime(orderFilter.StartDate).Date;
                }
                if (!string.IsNullOrEmpty(orderFilter.EndDate))
                {
                    EndDate = Convert.ToDateTime(orderFilter.EndDate).Date.AddDays(1);
                }

                HelperDomain helperDomain = new HelperDomain(this);
                IQueryable<Order> allOrders = Context.DataContext.Orders
                                                .Include(t => t.FuelRequest).Include(t => t.FuelRequest.FuelRequestDetail).Include(t => t.Invoices)
                                                .Include(t => t.FuelRequest.FuelRequests1).Include(t => t.FuelRequest.MstSupplierQualifications)
                                                .Include(t => t.FuelRequest.Job).Include(t => t.FuelRequest.Job.Company).Include(t => t.FuelRequest.User.Company)
                                                .Where(t => t.AcceptedCompanyId == companyId && orderIds.Contains(t.Id) && t.AcceptedDate >= StartDate && t.AcceptedDate < EndDate);

                foreach (var order in allOrders)
                {
                    response.Add(new OrderGridViewModel(Status.Success)
                    {
                        Id = order.Id,
                        PoNumber = order.PoNumber,
                        Eligibility = helperDomain.GetDisadvantageBusinessEnterprise(order.FuelRequest.MstSupplierQualifications.ToList()),
                        Supplier = order.FuelRequest.GetCompany().Name,
                        //TotalAmount = helperDomain.GetOrderAmount(order.FuelRequest, order.BrokeredMaxQuantity),
                        Quantity = helperDomain.GetQuantityRequested(order.BrokeredMaxQuantity, order.FuelRequest.MaxQuantity),
                        StartDate = order.FuelRequest.FuelRequestDetail.StartDate.Date.ToString(Resource.constFormatDate),
                        InvoiceCount = order.Invoices.Count(t1 => t1.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && !t1.IsBuyPriceInvoice),
                        FuelDeliveredPercentage = helperDomain.GetFuelDeliveredPercentage(order, 0),
                        Status = (order.OrderXStatuses.FirstOrDefault(t => t.IsActive).StatusId == (int)OrderStatus.PartiallyCanceled) ? OrderStatus.Canceled.ToString() : (order.OrderXStatuses.FirstOrDefault(t => t.IsActive).StatusId ==
                                    (int)OrderStatus.PartiallyClosed ? OrderStatus.Closed.ToString() : order.OrderXStatuses.FirstOrDefault(t => t.IsActive).MstOrderStatus.Name),
                        DisplayUoM = order.FuelRequest.UoM.ToString(),
                        Currency = order.FuelRequest.Currency.ToString()
                    });
                }
                response = response.OrderByDescending(t => t.Id).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetDriverOrders", ex.Message, ex);
            }
            return response;
        }

        public List<int> GetOrdersAssignedToDriver(int companyId, int driverId, OrderFilterType filter = OrderFilterType.All, int timeout = 30)
        {
            var orderIds = new List<int>();
            try
            {
                var inputModel = new
                {
                    DriverId = driverId,
                    CompanyId = companyId,
                    Filter = filter.ToString()
                };

                var input = SqlHelperMethods.GetStoredProcedure("usp_GetDriverOrdersByfilter", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                orderIds = Context.DataContext.Database.SqlQuery<int>(input.Query, input.Params.ToArray()).ToList();

                return orderIds;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetOrdersAssignedToDriver", ex.Message, ex);
            }
            return orderIds;
        }

        public async Task<List<int>> GetOpenOrdersHavingDeliverySchedule(int timeout = 30)
        {
            var response = new List<int>();
            try
            {
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetOpenOrdersHavingDeliverySchedules", new { });
                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<int>(input.Query, input.Params.ToArray()).ToListAsync();
                return response;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetOpenOrdersHavingDeliverySchedule", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<UpdateScheduleStatusInputModel>> GetOpenSchedulesAync(int timeout = 30)
        {
            var response = new List<UpdateScheduleStatusInputModel>();
            try
            {
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetOpenTrackableSchedules", new { });
                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UpdateScheduleStatusInputModel>(input.Query, input.Params.ToArray()).ToListAsync();
                return response;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetOpenSchedulesAync", ex.Message, ex);
            }
            return response;
        }


        public List<AuditReportAxxis> GetAxxisAuditReport(string startDate, string endDate, int timeout = 30)
        {
            var response = new List<AuditReportAxxis>();
            try
            {
                DateTime StartDate = Convert.ToDateTime(ApplicationConstants.DateRangeFilterStartDate);
                DateTime EndDate = DateTimeOffset.Now.Date.AddDays(1);
                if (string.IsNullOrEmpty(startDate) && string.IsNullOrEmpty(endDate))
                {
                    StartDate = DateTime.Now.Date.AddDays(ApplicationConstants.DateFilterDefaultDays);
                }
                if (!string.IsNullOrEmpty(startDate))
                {
                    StartDate = Convert.ToDateTime(startDate).Date;
                }
                if (!string.IsNullOrEmpty(endDate))
                {
                    EndDate = Convert.ToDateTime(endDate).Date.AddDays(1);
                }

                object inputModel = new { StartDate, EndDate };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetAxxisAuditReport", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                var result = Context.DataContext.Database.SqlQuery<USP_AuditReportAxxis>(input.Query, input.Params.ToArray()).ToList();
                result.ForEach(t => response.Add(new AuditReportAxxis
                {
                    DeliveryDate = t.DeliveryDate.ToString(Resource.constFormatDateTime),
                    TerminalId = t.TerminalId,
                    TerminalName = t.TerminalName,
                    ProductId = t.ProductId,
                    ProductName = t.ProductName
                }));
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetAxxisAuditReport", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<UspGetSupplierOrders>> GetSupplierOrders(int CompanyId, DataTableSearchModel requestModel, OrderFilterViewModel filter = null, int timeout = 30)
        {
            var response = new List<UspGetSupplierOrders>();
            try
            {
                DateTimeOffset StartDate = Convert.ToDateTime(ApplicationConstants.DateRangeFilterStartDate).ToUniversalTime();
                if (!string.IsNullOrEmpty(filter.StartDate) && long.TryParse(filter.StartDate, out long lStartDate))
                {
                    //Convert Start Date Filter to UTC start Date
                    StartDate = DateTimeOffset.FromUnixTimeMilliseconds(lStartDate).ToUniversalTime();
                }
                DateTimeOffset EndDate = DateTimeOffset.Now.Date.AddDays(1).ToUniversalTime();
                if (!string.IsNullOrEmpty(filter.EndDate) && long.TryParse(filter.EndDate, out long lEndDate))
                {
                    //Convert endDate (To Date) Filter to UTC Date 
                    EndDate = DateTimeOffset.FromUnixTimeMilliseconds(lEndDate).AddDays(1);
                    EndDate = EndDate.ToUniversalTime();
                }
                var customerIds = filter.CustomerIds != null && filter.CustomerIds.Any() ? string.Join(",", filter.CustomerIds) : "";
                var locationIds = filter.LocationIds != null && filter.LocationIds.Any() ? string.Join(",", filter.LocationIds) : "";
                var vesselIds = filter.VesselIds != null && filter.VesselIds.Any() ? string.Join(",", filter.VesselIds) : "";
                var isMarine = filter.IsMarine;

                object inputModel = new { CompanyId, StartDate, EndDate, CountryId = filter.CountryId, CurrencyType = (int)filter.Currency, FilterType = (int)filter.Filter, GroupIds = filter.GroupIds, CustomerIds = customerIds, LocationIds = locationIds, VesselIds = vesselIds, IsMarine = isMarine, requestModel };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetSupplierOrders", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspGetSupplierOrders>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetSupplierOrders", ex.Message, ex);
            }

            return response;
        }
        public async Task<List<PickupLocationDetailViewModel>> GetTerminalForGridAsync(int CountryId, DataTableSearchModel requestModel, int timeout = 30)
        {
            var response = new List<PickupLocationDetailViewModel>();
            try
            {
                object inputModel = new { CountryId, requestModel };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetTerminalsForGrid", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<PickupLocationDetailViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetTerminalForGridAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<UspGetBuyerOrders>> GetBuyerOrders(int companyId, int userId, OrderDataTableViewModel filter, int BrandedCompanyId, int timeout = 30)
        {
            var response = new List<UspGetBuyerOrders>();
            try
            {
                DataTableSearchModel dataTableSearchValues = new DataTableSearchModel(filter);
                DateTimeOffset startDate = Convert.ToDateTime(ApplicationConstants.DateRangeFilterStartDate);
                if (!string.IsNullOrEmpty(filter.StartDate))
                {
                    startDate = Convert.ToDateTime(filter.StartDate).Date;
                }
                DateTimeOffset endDate = DateTimeOffset.Now.Date.AddDays(1);
                if (!string.IsNullOrEmpty(filter.EndDate))
                {
                    endDate = Convert.ToDateTime(filter.EndDate).Date.AddDays(1);
                }
                if (filter.JobId > 0)
                {
                    var job = Context.DataContext.Jobs.FirstOrDefault(t => t.Id == filter.JobId);
                    if (job != null)
                    {
                        filter.Currency = job.Currency;
                        filter.CountryId = job.CountryId;
                    }
                }
                var customerIds = filter.CustomerIds != null && filter.CustomerIds.Any() ? string.Join(",", filter.CustomerIds) : "";
                var locationIds = filter.LocationIds != null && filter.LocationIds.Any() ? string.Join(",", filter.LocationIds) : "";
                var vesselIds = filter.VesselIds != null && filter.VesselIds.Any() ? string.Join(",", filter.VesselIds) : "";
                var isMarine = filter.IsMarine;

                object inputModel = new { CompanyId = companyId, UserId = userId, filter.JobId, filter.OrderId, BrandedCompanyId, Filter = (int)filter.Filter, FuelTypeId = filter.FuelTypeId, StartDate = startDate, EndDate = endDate, CurrencyType = (int)filter.Currency, CountryId = filter.CountryId, GroupIds = filter.GroupIds, CustomerIds = customerIds, LocationIds = locationIds, VesselIds = vesselIds, IsMarine = isMarine, DataTableSearchValues = dataTableSearchValues };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetBuyerOrders", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspGetBuyerOrders>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetBuyerOrders", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<UspGetSupplierOrders>> GetBrokerOrders(int companyId, OrderDataTableViewModel requestModel, int timeout = 45)
        {
            var response = new List<UspGetSupplierOrders>();
            try
            {
                DataTableSearchModel dataTableSearchValues = new DataTableSearchModel(requestModel);
                DateTimeOffset startDate = Convert.ToDateTime(ApplicationConstants.DateRangeFilterStartDate);
                if (!string.IsNullOrEmpty(requestModel.StartDate))
                {
                    startDate = Convert.ToDateTime(requestModel.StartDate).Date;
                }
                DateTimeOffset endDate = DateTimeOffset.Now.Date.AddDays(1);
                if (!string.IsNullOrEmpty(requestModel.EndDate))
                {
                    endDate = Convert.ToDateTime(requestModel.EndDate).Date.AddDays(1);
                }

                object inputModel = new { CompanyId = companyId, StartDate = startDate, EndDate = endDate, CountryId = requestModel.CountryId, CurrencyType = (int)requestModel.Currency, dataTableSearchValues };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetBrokeredOrders", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspGetSupplierOrders>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetBrokerOrders", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<MapViewModel>> GetBrokerMapAsync(int companyId, OrderFilterViewModel filter, int timeout = 30)
        {
            var response = new List<MapViewModel>();
            try
            {
                DateTimeOffset startDate = Convert.ToDateTime(ApplicationConstants.DateRangeFilterStartDate);
                if (!string.IsNullOrEmpty(filter.StartDate))
                {
                    startDate = Convert.ToDateTime(filter.StartDate).Date;
                }
                DateTimeOffset endDate = DateTimeOffset.Now.Date.AddDays(1);
                if (!string.IsNullOrEmpty(filter.EndDate))
                {
                    endDate = Convert.ToDateTime(filter.EndDate).Date.AddDays(1);
                }

                object inputModel = new { CompanyId = companyId, StartDate = startDate, EndDate = endDate, Country = filter.CountryId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetBrokerMapView", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<MapViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetBrokerMapAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<UspGetBrokerActivity>> GetBrokerActivity(int companyId, string StartDate, string EndDate, int timeout = 30)
        {
            var response = new List<UspGetBrokerActivity>();
            try
            {
                DateTimeOffset startDate = Convert.ToDateTime(ApplicationConstants.DateRangeFilterStartDate);
                if (!string.IsNullOrEmpty(StartDate))
                {
                    startDate = Convert.ToDateTime(StartDate).Date;
                }
                DateTimeOffset endDate = DateTimeOffset.Now.Date.AddDays(1);
                if (!string.IsNullOrEmpty(EndDate))
                {
                    endDate = Convert.ToDateTime(EndDate).Date.AddDays(1);
                }

                object inputModel = new { CompanyId = companyId, StartDate = startDate, EndDate = endDate };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetBrokeredActivity", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspGetBrokerActivity>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetBrokerOrders", ex.Message, ex);
            }

            return response;
        }

        public async Task<IEnumerable<USP_GetSupplierActiveOrders>> GetSupplierLastActiveOrders(int CompanyId, int CountryId, int CurrencyType, string groupIds = "", int countOfActiveOrders = 5, int timeout = 30)
        {
            var response = new List<USP_GetSupplierActiveOrders>();
            try
            {
                object inputModel = new { CompanyId, countOfActiveOrders, CountryId, CurrencyType, GroupIds = groupIds };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetSupplierOrdersForDashboard", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<USP_GetSupplierActiveOrders>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetSupplierLastActiveOrders", ex.Message, ex);
            }

            return response;
        }

        public async Task<IEnumerable<UspGetBuyerActiveOrders>> GetBuyerLastActiveOrders(int companyId, int userId, int jobId, int countryId, int currency, string groupIds = "", int countOfActiveOrders = 5, int timeout = 30)
        {
            var response = new List<UspGetBuyerActiveOrders>();
            try
            {
                object inputModel = new { CompanyId = companyId, UserId = userId, jobId, CountryId = countryId, CurrencyType = currency, countOfActiveOrders, GroupIds = groupIds };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetBuyerOrdersForDashboard", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspGetBuyerActiveOrders>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetBuyerLastActiveOrders", ex.Message + " companyId: " + companyId + " jobId: " + jobId, ex);
            }

            return response;
        }

        public async Task<UspGetSupplierDashboardInvoices> GetSupplierDashboardInvoices(int CompanyId, int AllowedInvoiceType, int CountryId, int CurrencyType, string groupIds = "", int timeout = 30)
        {
            var response = new UspGetSupplierDashboardInvoices();
            try
            {
                object inputModel = new { CompanyId, AllowedInvoiceType, CountryId, CurrencyType, GroupIds = groupIds };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetSupplierInvoicesForDashboard", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspGetSupplierDashboardInvoices>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetSupplierDashboardInvoices", ex.Message, ex);
            }

            return response;
        }

        public async Task<UspGetSupplierDashboardInvoices> GetSupplierDashboardDropTickets(int CompanyId, int AllowedInvoiceType, int CountryId, int CurrencyType, string groupIds = "", int timeout = 30)
        {
            var response = new UspGetSupplierDashboardInvoices();
            try
            {
                object inputModel = new { CompanyId, AllowedInvoiceType, CountryId, CurrencyType, GroupIds = groupIds };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetSupplierDropTicketsForDashboard", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspGetSupplierDashboardInvoices>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetSupplierDashboardDropTickets", ex.Message, ex);
            }

            return response;
        }

        public async Task<UspGetSupplierInvoiceDetails> GetSupplierInvoiceDetails(int companyId, int invoiceId, int timeout = 65) //timeout=30
        {
            var response = new UspGetSupplierInvoiceDetails();
            try
            {
                object inputModel = new { @InvoiceId = invoiceId, @CompanyId = companyId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetSupplierInvoiceDetail", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspGetSupplierInvoiceDetails>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetSupplierInvoiceDetails", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<int>> GetSupplierQualifications(int fuelRequestId, int timeout = 30)
        {
            var response = new List<int>();
            try
            {
                object inputModel = new { @FuelRequestId = fuelRequestId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetSupplierQualifications", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<int>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetSupplierQualifications", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<InvoiceGridViewModel>> GetBuyerInvoicesAsync(int companyId, int userId, bool isBuyerAdmin, int InvoiceTypeId, InvoiceDataTableViewModel filter = null, int BrandedCompanyId = 0, int timeout = 30, string InvoiceTypeIdFilter = "")
        {
            var response = new List<InvoiceGridViewModel>();
            try
            {
                DataTableSearchModel dataTableSearchValues = new DataTableSearchModel(filter);
                DateTimeOffset StartDate = Convert.ToDateTime(ApplicationConstants.DateRangeFilterStartDate);
                DateTimeOffset EndDate = DateTimeOffset.Now.Date.AddDays(1);
                if (!string.IsNullOrEmpty(filter.StartDate))
                {
                    StartDate = Convert.ToDateTime(filter.StartDate).Date;
                }
                if (!string.IsNullOrEmpty(filter.EndDate))
                {
                    EndDate = Convert.ToDateTime(filter.EndDate).Date.AddDays(1);
                }
                if (filter.JobId > 0)
                {
                    var job = Context.DataContext.Jobs.FirstOrDefault(t => t.Id == filter.JobId);
                    if (job != null)
                    {
                        filter.Currency = job.Currency;
                        filter.CountryId = job.CountryId;
                    }
                }
                var locationIds = filter.LocationIds != null && filter.LocationIds.Any() ? string.Join(",", filter.LocationIds) : "";
                var vesselIds = filter.VesselIds != null && filter.VesselIds.Any() ? string.Join(",", filter.VesselIds) : "";
                var isMarine = filter.IsMarine;

                object inputModel = new { CompanyId = companyId, BrandedCompanyId, UserId = userId, IsBuyerAdmin = isBuyerAdmin, filter.JobId, InvoiceTypeId, InvoiceFilter = (int)filter.Filter, StartDate, EndDate, Currency = (int)filter.Currency, filter.CountryId, filter.GroupIds, InvoiceTypeIdFilter = InvoiceTypeIdFilter, LocationIds = locationIds, VesselIds = vesselIds, IsMarine = isMarine, DataTableSearchValues = dataTableSearchValues };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetBuyerInvoices", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<InvoiceGridViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetBuyerInvoicesAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<UserNotificationSettingViewModel>> GetUserNotificationSettingsAsync(int userId, int companyTypeId, int timeout = 30)
        {
            var response = new List<UserNotificationSettingViewModel>();
            try
            {
                object inputModel = new { UserId = userId, CompanyTypeId = companyTypeId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetUserNotifications", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UserNotificationSettingViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetUserNotificationSettingsAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<AuditViewModel>> GetAuditReportAsync(AuditDataTableViewModel auditDataViewModel, DataTableSearchModel dataTableSearchModel, int timeout = 30)
        {
            var response = new List<AuditViewModel>();
            try
            {
                DateTimeOffset StartDate = Convert.ToDateTime(ApplicationConstants.DateRangeFilterStartDate);
                DateTimeOffset EndDate = DateTimeOffset.Now.Date.AddDays(1);
                if (!string.IsNullOrEmpty(auditDataViewModel.FromDate))
                {
                    StartDate = Convert.ToDateTime(auditDataViewModel.FromDate).Date;
                }
                if (!string.IsNullOrEmpty(auditDataViewModel.ToDate))
                {
                    EndDate = Convert.ToDateTime(auditDataViewModel.ToDate).Date.AddDays(1);
                }
                object inputModel = new { jobId = auditDataViewModel.JobId, StartDate, EndDate, dataTableSearchModel };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetAuditReportForJob", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<AuditViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetAuditReportAsync", ex.Message, ex);
            }
            return response;
        }
        public async Task<ProgressReportViewModel> GetProgressReport(ProgressReportFilter filter, int timeout = 30)
        {
            var response = new ProgressReportViewModel();
            try
            {
                if (Convert.ToInt32(filter.EndDate.Subtract(filter.StartDate).TotalDays) <= 1)
                    response.Date = $"{filter.EndDate.DayOfWeek.ToString()}{","}{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(filter.EndDate.Month)} {filter.EndDate.Day}{","}{filter.EndDate.Year}";
                else
                    response.Date = $"{Resource.lblPeriod}{":"} {filter.StartDate.ToString(Resource.constFormatDate)} {Resource.lblTo} {filter.EndDate.ToString(Resource.constFormatDate)}";

                var demoAccounts = ContextFactory.Current.GetDomain<ApplicationDomain>().GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingDemoAccounts);

                object inputModel = new { DemoAccounts = demoAccounts, filter.StartDate, filter.EndDate, filter.MonthStartDate, filter.MonthEndDate, filter.AccountOwnerId, CountryId = filter.CountryId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetProgressReportCount", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response.ProgressReportCount = await Context.DataContext.Database.SqlQuery<ProgressReportCountViewModel>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();

                inputModel = new { demoAccounts, filter.StartDate, filter.EndDate, CountryId = filter.CountryId };
                input = SqlHelperMethods.GetStoredProcedure("usp_GetProgressReportNewAccount", inputModel);
                Context.DataContext.Database.CommandTimeout = timeout;
                var companies = await Context.DataContext.Database.SqlQuery<ProgressReportNewAccountsViewModel>(input.Query, input.Params.ToArray()).ToListAsync();

                response.NewBuyers = companies.Where(t => t.CompanyTypeId == (int)CompanyType.Buyer).ToList();
                response.NewSuppliers = companies.Where(t => t.CompanyTypeId == (int)CompanyType.Supplier).ToList();
                response.NewBuyerAndSuppliers = companies.Where(t => t.CompanyTypeId == (int)CompanyType.BuyerAndSupplier).ToList();
                response.Culture = new HelperDomain().SetCountryCulture(filter.CountryId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetProgressReport", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<AccountActivityGridViewModel>> GetAccountActivityReport(string startDate, string endDate, int timeout = 30)
        {
            var response = new List<AccountActivityGridViewModel>();
            try
            {
                DateTime StartDate = Convert.ToDateTime(ApplicationConstants.DateRangeFilterStartDate);
                DateTime EndDate = DateTimeOffset.Now.Date.AddDays(1);
                if (!string.IsNullOrEmpty(startDate))
                {
                    StartDate = Convert.ToDateTime(startDate).Date;
                }

                if (!string.IsNullOrEmpty(endDate))
                {
                    EndDate = Convert.ToDateTime(endDate).Date.AddDays(1);
                }

                TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
                DateTimeOffset StartDateOffset = new DateTimeOffset(StartDate, timeZoneInfo.GetUtcOffset(StartDate));
                DateTimeOffset EndDateOffset = new DateTimeOffset(EndDate, timeZoneInfo.GetUtcOffset(EndDate));
                DateTimeOffset currentDateTime = new DateTimeOffset(DateTimeOffset.Now.Date, timeZoneInfo.GetUtcOffset(DateTimeOffset.Now.Date));
                string Offset = string.Format(Resource.constOffsetFormat, currentDateTime.Offset.Hours, currentDateTime.Offset.Minutes);

                object inputModel = new { Offset, StartDateOffset, EndDateOffset };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetAccountActivity", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<AccountActivityGridViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetAccountActivityReport", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<SuperAdminTPOCompanyGridViewModel>> GetCompaniesCreatedByTPO(string startDate, string endDate, int timeout = 30)
        {
            var response = new List<SuperAdminTPOCompanyGridViewModel>();
            try
            {
                DateTimeOffset StartDate = Convert.ToDateTime(ApplicationConstants.DateRangeFilterStartDate);
                DateTimeOffset EndDate = DateTimeOffset.Now.Date.AddDays(1);
                if (!string.IsNullOrEmpty(startDate))
                {
                    StartDate = Convert.ToDateTime(startDate).Date;
                }
                if (!string.IsNullOrEmpty(endDate))
                {
                    EndDate = Convert.ToDateTime(endDate).Date.AddDays(1);
                }

                object inputModel = new { StartDate, EndDate };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetCompaniesCreatedByTPO", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<SuperAdminTPOCompanyGridViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetCompaniesCreatedByTPO", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<DiscountSummaryViewModel>> GetDiscountSummary(int invoiceId, int timeout = 30)
        {
            var response = new List<DiscountSummaryViewModel>();
            try
            {
                object inputModel = new { invoiceId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetDiscountSummary", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<DiscountSummaryViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetDiscountSummary", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetNearestCustomerByFuelType(ApiNearestCustomerByFuelTypeModel viewModel)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var fuelTypeIds = string.Join(",", viewModel.FuelTypeIds);
                object inputModel = new { fuelTypeIds, viewModel.SupplierCompanyId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetCustomerDetails", inputModel);

                Context.DataContext.Database.CommandTimeout = 30;
                response = await Context.DataContext.Database.SqlQuery<DropdownDisplayItem>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetNearestCustomerByFuelType", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<InvoiceGridViewModel>> GetSupplierInvoicesAsync(int companyId, int InvoiceTypeId, int ViewInvoices, InvoiceDataTableViewModel filter = null, int timeout = 120)
        {
            var response = new List<InvoiceGridViewModel>();
            try
            {
                DataTableSearchModel dataTableSearchValues = new DataTableSearchModel(filter);
                DateTimeOffset StartDate = Convert.ToDateTime(ApplicationConstants.DateRangeFilterStartDate);
                DateTimeOffset EndDate = DateTimeOffset.Now.Date.AddDays(1);
                if (!string.IsNullOrEmpty(filter.StartDate))
                {
                    StartDate = Convert.ToDateTime(filter.StartDate).Date;
                }
                if (!string.IsNullOrEmpty(filter.EndDate))
                {
                    EndDate = Convert.ToDateTime(filter.EndDate).Date.AddDays(1);
                }
                var customerIds = filter.CustomerIds != null && filter.CustomerIds.Any() ? string.Join(",", filter.CustomerIds) : "";
                var locationIds = filter.LocationIds != null && filter.LocationIds.Any() ? string.Join(",", filter.LocationIds) : "";
                var vesselIds = filter.VesselIds != null && filter.VesselIds.Any() ? string.Join(",", filter.VesselIds) : "";
                var isMarine = filter.IsMarine;

                object inputModel = new { CompanyId = companyId, filter.OrderId, InvoiceTypeId, InvoiceFilter = (int)filter.Filter, ViewInvoices, StartDate, EndDate, CountryId = filter.CountryId, CurrencyType = (int)filter.Currency, GroupIds = filter.GroupIds, CustomerIds = customerIds, LocationIds = locationIds, VesselIds = vesselIds, IsMarine = isMarine, DataTableSearchValues = dataTableSearchValues };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetSupplierInvoices", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<InvoiceGridViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
                if (response != null && response.Any())
                {
                    foreach (var item in response)
                    {
                        if ((string.IsNullOrWhiteSpace(item.PDIOrderId) || item.PDIOrderId == Resource.lblHyphen) && (!string.IsNullOrWhiteSpace(item.ExternalPDIException) && item.ExternalPDIException !=Resource.lblHyphen) )
                        {
                            item.PDIOrderId = Resource.lblViewException;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetSupplierInvoicesAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<InvoiceBolGridViewModel>> GetSupplierBolInvoicesAsync(int companyId, InvoiceDataTableViewModel filter = null, ViewInvoices view = ViewInvoices.All, int timeout = 30)
        {
            var response = new List<InvoiceBolGridViewModel>();
            try
            {
                DataTableSearchModel dataTableSearchValues = new DataTableSearchModel(filter);
                DateTimeOffset StartDate = DateTimeOffset.Now.Date.AddDays(ApplicationConstants.DateFilterDefaultPast30Days);
                DateTimeOffset EndDate = DateTimeOffset.Now.Date.AddDays(1);
                if (!string.IsNullOrEmpty(filter.StartDate))
                {
                    StartDate = Convert.ToDateTime(filter.StartDate).Date;
                }
                if (!string.IsNullOrEmpty(filter.EndDate))
                {
                    EndDate = Convert.ToDateTime(filter.EndDate).Date.AddDays(1);
                }

                object inputModel = new { CompanyId = companyId, InvoiceFilter = (int)filter.Filter, StartDate, EndDate, CountryId = filter.CountryId, CurrencyType = (int)filter.Currency, GroupIds = filter.GroupIds, DataTableSearchValues = dataTableSearchValues };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetSupplierBolInvoices", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<InvoiceBolGridViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetSupplierBolInvoicesAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<InvoiceBolGridViewModel>> GetBuyerBolInvoicesAsync(int userId, int companyId, InvoiceDataTableViewModel filter = null, ViewInvoices view = ViewInvoices.All, int BrandedCompanyId = 0, int timeout = 30)
        {
            var response = new List<InvoiceBolGridViewModel>();
            try
            {
                DataTableSearchModel dataTableSearchValues = new DataTableSearchModel(filter);
                DateTimeOffset StartDate = DateTimeOffset.Now.Date.AddDays(ApplicationConstants.DateFilterDefaultPast30Days);
                DateTimeOffset EndDate = DateTimeOffset.Now.Date.AddDays(1);
                if (!string.IsNullOrEmpty(filter.StartDate))
                {
                    StartDate = Convert.ToDateTime(filter.StartDate).Date;
                }
                if (!string.IsNullOrEmpty(filter.EndDate))
                {
                    EndDate = Convert.ToDateTime(filter.EndDate).Date.AddDays(1);
                }

                object inputModel = new { CompanyId = companyId, UserId = userId, InvoiceFilter = (int)filter.Filter, StartDate, EndDate, BrandedCompanyId, filter.CountryId, CurrencyType = (int)filter.Currency, GroupIds = filter.GroupIds, DataTableSearchValues = dataTableSearchValues };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetBuyerBolInvoices", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<InvoiceBolGridViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetBuyerBolInvoicesAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<USP_FuelRequestSummaryModel>> GetFuelRequestsSummary(DataTableSearchModel dataTableSearchModel, string startDate, string endDate, string fuelRequestStatuses = "", string fuelRequestTypes = "", int timeout = 30)
        {
            var response = new List<USP_FuelRequestSummaryModel>();
            try
            {
                HelperDomain helperDomain = new HelperDomain(this);
                DateTimeOffset StartDate = Convert.ToDateTime(ApplicationConstants.DateRangeFilterStartDate);
                DateTimeOffset EndDate = DateTimeOffset.Now.Date.AddDays(1);

                if (!string.IsNullOrEmpty(startDate))
                {
                    StartDate = Convert.ToDateTime(startDate).Date;
                }
                if (!string.IsNullOrEmpty(endDate))
                {
                    EndDate = Convert.ToDateTime(endDate).Date.AddDays(1);
                }

                object inputModel = new { StartDate = StartDate, EndDate = EndDate, FuelRequestTypes = fuelRequestTypes, FuelRequestStatuses = fuelRequestStatuses, DataTableSearchValues = dataTableSearchModel };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetFuelRequestsSummary", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<USP_FuelRequestSummaryModel>(input.Query, input.Params.ToArray()).ToListAsync();

                response.ForEach(t =>
                {
                    t.DateCreated = t.CreatedDate.ToTargetDateTimeOffset(t.TimeZoneName).ToString(Resource.constFormatDateTime);
                    t.DateAccepted = t.AcceptedDate.HasValue ? t.AcceptedDate.Value.ToTargetDateTimeOffset(t.TimeZoneName).ToString(Resource.constFormatDateTime) : Resource.lblHyphen;
                    t.PricePerGallon = t.PricePerGallon; //helperDomain.GetPricePerGallon(t.Price, t.PricingTypeId, t.RackAvgTypeId ?? 0);
                    t.GallonsOrdered = helperDomain.GetQuantityRequested(t.Quantity);
                    t.AboutToExpire = new FuelRequestDomain(helperDomain).GetFuelRequestExpiringIn7DaysStatus(t.TimeZoneName, t.ExpirationDate, t.StartDate, t.EndTime);
                });
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetFuelRequestsSummary", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<USP_TimeCardActionSummaryForAllDrivers>> GetTimeCardActionSummaryForAllDrivers(int companyId, string driverids, string startDate, string endDate, int pageNo = 0, int pageSize = 0, int sortId = 0, int timeout = 30)
        {
            var response = new List<USP_TimeCardActionSummaryForAllDrivers>();
            try
            {
                DateTimeOffset StartDate = startDate.GetFilterStartDateInDateTimeOffset();
                DateTimeOffset EndDate = endDate.GetFilterEndDateInDateTimeOffset();

                var inputModel = new { CompanyId = companyId, DriverIds = driverids, StartDate = StartDate, EndDate = EndDate, PageNo = pageNo, PageSize = pageSize, SortId = sortId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetTimeCardActionSummaryForAllDrivers", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<USP_TimeCardActionSummaryForAllDrivers>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetTimeCardActionSummaryForAllDrivers", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<UspCompletedDeliveriesViewModel>> GetCompletedDeliveriesForOrder(int orderId, string startDate, string endDate, int pageNo = 0, int pageSize = 0, int sortId = 0, int timeout = 30)
        {
            var response = new List<UspCompletedDeliveriesViewModel>();
            try
            {
                var inputModel = new { OrderId = orderId, PageNo = pageNo, PageSize = pageSize, SortId = sortId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetCompletedDeliveriesForOrder", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspCompletedDeliveriesViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetCompletedDeliveriesForOrder", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<UspCompletedDeliveriesViewModel>> GetCompletedDeliveriesForSupplierAsync(int companyId, string driverIds, string startDate, string endDate, Currency currency, int countryId, int pageNo = 0, int pageSize = 0, int sortId = 0, int timeout = 30)
        {
            var response = new List<UspCompletedDeliveriesViewModel>();
            try
            {
                DateTimeOffset StartDate = DateTimeOffset.Now.Date.AddDays(ApplicationConstants.DateFilterDefaultDays);
                DateTimeOffset EndDate = DateTimeOffset.Now.Date.AddDays(1);
                if (!string.IsNullOrEmpty(startDate))
                {
                    StartDate = Convert.ToDateTime(startDate).Date;
                }
                if (!string.IsNullOrEmpty(endDate))
                {
                    EndDate = Convert.ToDateTime(endDate).Date.AddDays(1);
                }

                var inputModel = new { CompanyId = companyId, DriverIds = driverIds, StartDate = StartDate, EndDate = EndDate, CountryId = countryId, CurrencyType = (int)currency, PageNo = pageNo, PageSize = pageSize, SortId = sortId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetCompletedDeliveriesForSupplier", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspCompletedDeliveriesViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetCompletedDeliveriesForSupplierAsync", ex.Message, ex);
            }
            return response;
        }

        public List<UspMissedSchedulesGridViewModel> GetMissedDeliverySchedules(int orderId, int pageId = 1, int pageSize = 10, int sortId = 0, int timeout = 30)
        {
            var response = new List<UspMissedSchedulesGridViewModel>();
            try
            {
                var inputModel = new { orderId, pageId, pageSize, sortId };
                //Missed, Rescheduled-Missed, MissedAndCanceled
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetMissedSchedulesForOrder", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = Context.DataContext.Database.SqlQuery<UspMissedSchedulesGridViewModel>(input.Query, input.Params.ToArray()).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetMissedDeliverySchedules", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<NewsfeedListViewModel>> GetNewsfeeds(int CompanyId, int UserId, int EntityTypeId, int EntityId, int CurrentPage, int PageSize = 10, int LatestId = 0, int timeout = 30)
        {
            var response = new List<NewsfeedListViewModel>();
            var inputModel = new { CompanyId, EntityTypeId, EntityId, CurrentPage, PageSize, LatestId, UserId };
            var input = SqlHelperMethods.GetStoredProcedure("usp_GetNewsfeed", inputModel);

            Context.DataContext.Database.CommandTimeout = timeout;
            response = await Context.DataContext.Database.SqlQuery<NewsfeedListViewModel>(input.Query, input.Params.ToArray()).ToListAsync();

            return response;
        }

        public async Task<List<NewsfeedListViewModel>> GetNewsfeedsForBuyerAndSupplier(int BuyerCompanyId, int SupplierCompanyId, int CurrentPage, int PageSize = 10, int LatestId = 0, int timeout = 30)
        {
            var response = new List<NewsfeedListViewModel>();
            try
            {
                var inputModel = new { BuyerCompanyId, SupplierCompanyId, CurrentPage, PageSize, LatestId };

                var input = SqlHelperMethods.GetStoredProcedure("usp_GetNewsfeedForBuyerAndSupplier", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<NewsfeedListViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetNewsfeedsForBuyerAndSupplier", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<Usp_SchedulesforDriversGridViewModel>> GetDeliverySchedulesforDriversAsync(int companyId, string driverIds, string startDate, string endDate, Currency currency, int countryId, string searchText = "", int pageId = 1, int pageSize = 10, int sortId = 0, int timeout = 30)
        {
            var response = new List<Usp_SchedulesforDriversGridViewModel>();
            try
            {
                var inputModel = new { CompanyId = companyId, DriverIds = driverIds, StartDate = startDate.GetFilterStartDateInDateTimeOffset(), EndDate = endDate.GetFilterStartDateInDateTimeOffset(), CurrentDate = DateTimeOffset.Now, CountryId = countryId, CurrencyType = (int)currency, SearchText = searchText };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetDeliverySchedulesforDrivers", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<Usp_SchedulesforDriversGridViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetDeliverySchedulesforDriversAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<ScheduleMapDataViewModel>> GetSchedulesMapDataAsync(string driverIds, int timeout = 30)
        {
            var response = new List<ScheduleMapDataViewModel>();
            try
            {
                var inputModel = new { DriverIds = driverIds };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetDeliverySchedulesMapData", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<ScheduleMapDataViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetSchedulesMapDataAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<USP_DashboardDropAveragesViewModel> GetSupplierDashboardDropAveragesAsync(int companyId, int userId, int buyerCompanyId, int fuelTypeId, int countryId, int currency, int timeout = 30)
        {
            var response = new USP_DashboardDropAveragesViewModel();
            try
            {
                var inputModel = new { CompanyId = companyId, UserId = userId, BuyerCompanyId = buyerCompanyId, FuelTypeId = fuelTypeId, CountryId = countryId, CurrencyType = currency };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetSupplierDashboardDropAverages", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<USP_DashboardDropAveragesViewModel>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetSupplierDashboardDropAveragesAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<USP_GetSupplierInvoices> GetDeleteRequestsInvoicesAsync(string invoiceNumber, int timeout = 30)
        {
            var response = new USP_GetSupplierInvoices();
            try
            {
                object inputModel = new { InvoiceNumber = invoiceNumber };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetDeleteRequestedInvoices", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<USP_GetSupplierInvoices>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetDeleteRequestsInvoicesAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<USP_GetTaxExemptionLicenses>> GetTaxExemptionLicenses(int CompanyId, bool isBuyer, int timeout = 30)
        {
            var response = new List<USP_GetTaxExemptionLicenses>();
            try
            {
                object inputModel = new { CompanyId, isBuyer };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetTaxExemptLicenses", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<USP_GetTaxExemptionLicenses>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetTaxExemptionLicenses", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<UspSupplierPerformanceViewModel>> GetYourSuppliersPerformanceData(int CompanyId, int timeout = 30)
        {
            var response = new List<UspSupplierPerformanceViewModel>();
            try
            {
                object inputModel = new { CompanyId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetSupplierPerformanceData", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspSupplierPerformanceViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetYourSuppliersPerformanceData", ex.Message + " -CompanyId: " + CompanyId, ex);
            }

            return response;
        }

        public async Task<List<UspBuyerPerformanceViewModel>> GetYourBuyerPerformanceData(int CompanyId, int timeout = 30)
        {
            var response = new List<UspBuyerPerformanceViewModel>();
            try
            {
                object inputModel = new { CompanyId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetBuyerPerformanceData", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspBuyerPerformanceViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetYourBuyerPerformanceData", ex.Message + " -CompanyId: " + CompanyId, ex);
            }

            return response;
        }
        public async Task<List<UspCarrierCustomerMapping>> GetCarrierCustomerMapping(int CompanyId, int timeout = 30)
        {
            var response = new List<UspCarrierCustomerMapping>();
            try
            {
                object inputModel = new { CompanyId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetCarrierCustomerMapping", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspCarrierCustomerMapping>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetCarrierCustomerMapping", ex.Message + " -CompanyId: " + CompanyId, ex);
            }

            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetAssignedTerminalIds(int CompanyId, int timeout = 30)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                object inputModel = new { CompanyId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetAssignedTerminalIdsForMapping", inputModel);
                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<DropdownDisplayItem>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetAssignedTerminalIds", ex.Message + " -CompanyId: " + CompanyId, ex);
            }
            return response;
        }

        public async Task<List<SuperAdminOrderGridViewModel>> GetOrdersByPoNumberAsync(string poNumber, int timeout = 30)
        {
            var response = new List<SuperAdminOrderGridViewModel>();
            try
            {
                object inputModel = new { PoNumber = poNumber };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetOrdersByPoNumber", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<SuperAdminOrderGridViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetOrdersByPoNumberAsync", ex.Message + " -PoNumber: " + poNumber, ex);
            }

            return response;
        }


        public List<DropdownDisplayItem> GetProductsInYourArea(int jobId = 0, decimal radius = 100, decimal latitude = 0, decimal longitude = 0, string countryCode = "USA", int timeout = 30)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                object inputModel = new { JobId = jobId, Latitude = latitude, Longitude = longitude, Radius = radius, CountryCode = countryCode };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetProductsInYourArea", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = Context.DataContext.Database.SqlQuery<DropdownDisplayItem>(input.Query, input.Params.ToArray()).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetProductsInYourArea", ex.Message, ex);
            }

            return response;
        }

        public List<DropdownDisplayItem> GetProductsByDisplayGroup(int jobId = 0, int displayGroupId = 1, string countryCode = "USA", int? companyId = null, int timeout = 30)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                object inputModel = new { ProductDisplayGroupId = displayGroupId, CountryCode = countryCode, JobId = jobId, CompanyId = companyId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetProductsByDisplayGroupId", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = Context.DataContext.Database.SqlQuery<DropdownDisplayItem>(input.Query, input.Params.ToArray()).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetProductsByDisplayGroup", ex.Message, ex);
            }

            return response;
        }

        public List<DropdownDisplayItem> GetFavoriteFuelTypes(int companyId, int jobId, string countryCode = "USA", int timeout = 30)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                object inputModel = new { CompanyId = companyId, CountryCode = countryCode, JobId = jobId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetFavoriteFuelTypes", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = Context.DataContext.Database.SqlQuery<DropdownDisplayItem>(input.Query, input.Params.ToArray()).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetFavoriteFuelTypes", ex.Message, ex);
            }

            return response;
        }

        public List<DropdownDisplayItem> GetSourceBasedProducts(PricingSource source, int timeout = 30)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                object inputModel = new { SourceId = (int)source };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetSourceBasedProducts", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = Context.DataContext.Database.SqlQuery<DropdownDisplayItem>(input.Query, input.Params.ToArray()).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetOpisProducts", ex.Message, ex);
            }

            return response;
        }

        public List<TerminalDetails> GetOpisTerminals(int cityRackId = 0, decimal latitude = 0, decimal longitude = 0, int countryId = 1, string searchStringTeminal = "", PricingSource source = PricingSource.Axxis, int companyCountryId = 0, int timeout = 30)
        {
            var response = new List<TerminalDetails>();
            try
            {
                object inputModel = new { CityRackTerminalId = cityRackId, Latitude = latitude, Longitude = longitude, CountryId = countryId, SourceId = (int)source, Terminal = searchStringTeminal, CompanyCountryId = companyCountryId > 0 ? companyCountryId : countryId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetOpisTerminals", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = Context.DataContext.Database.SqlQuery<TerminalDetails>(input.Query, input.Params.ToArray()).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetOpisTerminals", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<SupplierDetailViewModel>> SearchSuppliersByLocationAsync(SupplierGeoViewModel supplierModel, int accountTypeId, string fuelTypes = "", int timeout = 30)
        {
            var response = new List<SupplierDetailViewModel>();
            try
            {
                object paramModel = new { Latitude = supplierModel.Latitude, Longitude = supplierModel.Longitude, IncludeAllLocations = supplierModel.IncludeAllLocations, FuelTypes = fuelTypes, Radius = supplierModel.Radius, StateId = supplierModel.State.Id, AccountTypeId = accountTypeId, SupplierTypeId = supplierModel.SupplierType };
                var input = SqlHelperMethods.GetStoredProcedure("usp_SearchSuppliersByLocation", paramModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<SupplierDetailViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "SearchSuppliersByLocationAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task UpdateSupplierCostOfFuelRequests(UserContext userContext, int fuelTypeId, decimal supplierCost, int timeout = 30)
        {
            try
            {
                object paramModel = new { UserId = userContext.Id, FuelTypeId = fuelTypeId, SupplierCost = supplierCost };
                var input = SqlHelperMethods.GetStoredProcedure("usp_UpdateFuelRequestsSupplierCost", paramModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                await Context.DataContext.Database.SqlQuery<bool>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "UpdateSupplierCostOfFuelRequests", ex.Message, ex);
            }
        }

        public async Task<List<ExternalSupplierGridViewModel>> GetExternalSuppliersAsync(string offset, int timeout = 30)
        {
            var response = new List<ExternalSupplierGridViewModel>();
            try
            {
                object paramModel = new { Offset = offset };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetExternalSuppliers", paramModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<ExternalSupplierGridViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetExternalSuppliersAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<DriverOrderViewModel>> GetDriverOrdersForMobile(decimal latitude, decimal longitude, int companyId, int userId, int distance, int exactOrderProximity, long scheduleDate = 0, int buyerCompanyId = 0, int timeout = 30)
        {
            var response = new List<DriverOrderViewModel>();
            try
            {
                DateTimeOffset deliveryDate = DateTimeOffset.FromUnixTimeMilliseconds(scheduleDate);
                DateTimeOffset searchDate = deliveryDate.Date;

                var inputModel = new { @Latitude = latitude, @Longitude = longitude, @CompanyId = companyId, @UserId = userId, @Distance = distance, @ExactOrderProximity = exactOrderProximity, @SearchDate = searchDate, @BuyerCompanyId = buyerCompanyId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetDriverOrders", inputModel);
                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<DriverOrderViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetDriverOrdersForMobile", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DriverOrderViewModel>> GetDriverOrdersOfSchedulesForMobile(int userId, string orderIds, string scheduleIds, int timeout = 30)
        {
            var response = new List<DriverOrderViewModel>();
            try
            {
                var inputModel = new { @OrderIds = orderIds, @ScheduleIds = scheduleIds, @UserId = userId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetDriverOrdersForSchedules", inputModel);
                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<DriverOrderViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetDriverOrdersOfSchedulesForMobile", ex.Message, ex);
            }
            return response;
        }


        public async Task<List<ApiPreLoadBolViewModel>> GetPreLoadBolDetailsForMobile(int companyId, int userId, List<int> postLoadedTrackableScheduleIds, int timeout = 30)
        {
            var response = new List<ApiPreLoadBolViewModel>();
            try
            {
                string postLoadedTrackableSchedules = string.Join(",", postLoadedTrackableScheduleIds);

                var inputModel = new { @CompanyId = companyId, @UserId = userId, @PostLoadTrackableSchedules = postLoadedTrackableSchedules };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetPreLoadBolDetailsForMobile", inputModel);
                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<ApiPreLoadBolViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetPreLoadBolDetailsForMobile", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<NearestJobByCustomerViewModel>> GetNearestJobsByCustomer(int customerId, int supplierId, List<int> fuelTypeId, decimal latitude, decimal longitude, double radius, List<int> jobIds, int timeout = 30)
        {
            var response = new List<NearestJobByCustomerViewModel>();
            try
            {
                string driverJobs = string.Join(",", jobIds);
                string fuelTypeIds = string.Join(",", fuelTypeId);

                var inputModel = new { @BuyerCompanyId = customerId, @SupplierCompanyId = supplierId, @FuelTypeId = fuelTypeIds, @Latitude = latitude, @Longitude = longitude, @Radius = radius, @DriverJobs = driverJobs };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetNearestJobsByCustomer", inputModel);
                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<NearestJobByCustomerViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetNearestJobsByCustomer", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DeliveryScheduleForDriverRequestViewModel>> GetDeliveryScheduleForMobile(int userId, int companyId, int userTimeOffset, long scheduleDate = 0, int timeout = 30)
        {
            var response = new List<DeliveryScheduleForDriverRequestViewModel>();
            try
            {
                DateTimeOffset deliveryDate = DateTimeOffset.FromUnixTimeMilliseconds(scheduleDate).AddMinutes(userTimeOffset);
                DateTimeOffset searchDate = deliveryDate.Date;

                var inputModel = new { @UserId = userId, @CompanyId = companyId, @SearchDate = searchDate };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetDeliveryScheduleForMobile", inputModel);
                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<DeliveryScheduleForDriverRequestViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetDeliveryScheduleForMobile", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<UspDriverDropDetailsGridViewModel>> GetCurrentDriverDropDetails(List<int> driverIds, int currency, int countryId, int companyId, int timeout = 30)
        {
            var response = new List<UspDriverDropDetailsGridViewModel>();
            try
            {
                var inputModel = new { @Drivers = string.Join(",", driverIds), @Currency = currency, @CountryId = countryId, @CompanyId = companyId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetCurrentDriverDropDetails", inputModel);
                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspDriverDropDetailsGridViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetCurrentDriverDropDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<UspDriverDropDetailsGridViewModel>> GetNextDriverDropDetails(List<int> driverIds, int currency, int countryId, int companyId, DateTimeOffset deliveryDate, int timeout = 30)
        {
            var response = new List<UspDriverDropDetailsGridViewModel>();
            try
            {
                var inputModel = new { @Drivers = string.Join(",", driverIds), @Currency = currency, @CountryId = countryId, @CompanyId = companyId, @DeliveryDate = deliveryDate.Date };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetNextDriverDropDetails", inputModel);
                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspDriverDropDetailsGridViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetNextDriverDropDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<JobListViewModel>> GetJobsForSpecificScheduleDate(int userId, int supplierCompanyId, string searchCriteria, long scheduleDate = 0, int timeout = 30)
        {
            var response = new List<JobListViewModel>();
            try
            {
                DateTimeOffset deliveryDate = DateTimeOffset.FromUnixTimeMilliseconds(scheduleDate);
                DateTimeOffset searchDate = deliveryDate.Date;

                var inputModel = new { @UserId = userId, @SupplierCompanyId = supplierCompanyId, @SearchDate = searchDate, @SearchCriteria = searchCriteria };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetJobsForSpecificScheduleDate", inputModel);
                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<JobListViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetJobsForSpecificScheduleDate", ex.Message, ex);
            }
            return response;
        }

        public async Task<USP_FuelRequestStatsModel> GetAllFuelRequests(string startDt, string endDt, string fuelRequestTypes, string fuelRequestStatuses, int pageId = 1, int pageSize = 10, int sortId = 0, int timeout = 30)
        {
            var response = new USP_FuelRequestStatsModel();
            try
            {
                DateTimeOffset startDate = Convert.ToDateTime(ApplicationConstants.DateRangeFilterStartDate);
                DateTimeOffset endDate = DateTimeOffset.Now.Date.AddDays(1);
                if (string.IsNullOrEmpty(startDt) && string.IsNullOrEmpty(endDt))
                {
                    startDate = startDate.Date.AddDays(ApplicationConstants.DateFilterDefaultDays);
                }
                if (!string.IsNullOrEmpty(startDt))
                {
                    startDate = Convert.ToDateTime(startDt).Date;
                }
                if (!string.IsNullOrEmpty(endDt))
                {
                    endDate = Convert.ToDateTime(endDt).Date.AddDays(1);
                }

                //// pass comma seperated ids of fuel request statuses and fuel request types
                object inputModel = new { fuelRequestTypes, fuelRequestStatuses, startDate, endDate, pageId, pageSize, sortId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetAllFuelRequests", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<USP_FuelRequestStatsModel>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();


            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetAllFuelRequests", ex.Message, ex);
            }

            return response;
        }

        public List<TerminalDetails> GetClosestTerminals(int countryId, decimal fuelTypeId = 0, decimal latitude = 0, decimal longitude = 0, string terminal = "", int pricingSource = (int)PricingSource.Axxis, int timeout = 60)
        {
            var response = new List<TerminalDetails>();
            try
            {
                object inputModel = new { CountryId = countryId, FuelTypeId = fuelTypeId, Latitude = latitude, Longitude = longitude, Terminal = terminal.Trim(), PricingSource = pricingSource };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetClosestTerminals", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = Context.DataContext.Database.SqlQuery<TerminalDetails>(input.Query, input.Params.ToArray()).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetClosestTerminals", ex.Message, ex);
            }

            return response;
        }

        public List<TerminalDetails> GetClosestBulkPlantByDistance(BulkPlantRequestModel requestModel, int timeout = 60)
        {
            var response = new List<TerminalDetails>();
            try
            {
                var inputModel = new
                {
                    CompanyId = requestModel.CompanyId,
                    CountryId = requestModel.CountryId,
                    JobLatitude = requestModel.JobLatitude,
                    JobLongitude = requestModel.JobLongitude,
                    BulkPlantIds = requestModel.BulkPlantIds,
                    CompanyCountryId = requestModel.CompanyCountryId > 0 ? requestModel.CompanyCountryId : requestModel.CountryId,
                };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetClosestBulkPlantByDistance", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = Context.DataContext.Database.SqlQuery<TerminalDetails>(input.Query, input.Params.ToArray()).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetClosestBulkPlantByDistance", ex.Message, ex);
            }

            return response;
        }

        public async Task<InvoiceXAdditionalDetailViewModel> GetInvoiceAdditionalDetailAsync(int orderId, int invoiceId, bool isSellInvoice = false, int timeout = 60)
        {
            var response = new InvoiceXAdditionalDetailViewModel();
            try
            {
                object inputModel = new { InvoiceId = invoiceId, OrderId = orderId, IsSellInvoice = isSellInvoice };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetInvoiceAdditionalDatail", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<InvoiceXAdditionalDetailViewModel>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetInvoiceAdditionalDetailAsync", ex.Message, ex);
            }

            return response;
        }

        internal bool CheckAccessOnEntity(int id, int companyId, int companyTypeId, int entityType, int entityId, int roleId, int timeout)
        {
            var response = false;
            try
            {
                object inputModel = new
                {
                    UserId = id,
                    CompanyId = companyId,
                    CompanyTypeId = companyTypeId,
                    EntityType = entityType,
                    EntityId = entityId,
                    UserRoleId = roleId
                };
                var input = SqlHelperMethods.GetStoredProcedure("usp_CheckAccessOnEntity", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = Context.DataContext.Database.SqlQuery<bool>(input.Query, input.Params.ToArray()).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetClosestTerminals", ex.Message, ex);
            }

            return response;
        }

        public List<UspAtlasOilReportViewModel> GetAtlasOilReport(int companyId, DateTimeOffset startDate, DateTimeOffset endDate, int timeout = 60)
        {
            var response = new List<UspAtlasOilReportViewModel>();
            try
            {
                object inputModel = new { CompanyId = companyId, StartDate = startDate, EndDate = endDate };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetAtlasOilReport", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = Context.DataContext.Database.SqlQuery<UspAtlasOilReportViewModel>(input.Query, input.Params.ToArray()).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetAtlasOilReport", ex.Message, ex);
            }

            return response;
        }

        public List<UspAtlasOilCarrierReportViewModel> GetAtlasOilCarrierReport(int companyId, DateTimeOffset startDate, DateTimeOffset endDate, int timeout = 60)
        {
            var response = new List<UspAtlasOilCarrierReportViewModel>();
            try
            {
                object inputModel = new { CompanyId = companyId, StartDate = startDate, EndDate = endDate };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetAtlasOilCarrierReport", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = Context.DataContext.Database.SqlQuery<UspAtlasOilCarrierReportViewModel>(input.Query, input.Params.ToArray()).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetAtlasOilCarrierReport", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<USP_QuickBooksSummaryViewModel>> GetQbSummary(string FromDateTime, string ToDateTime, int CompanyId, int AccountingWorkflowId, DataTableSearchModel dataTableSearchValues, int timeout = 30)
        {
            var response = new List<USP_QuickBooksSummaryViewModel>();
            try
            {
                string StartDate = DateTime.Now.AddDays(ApplicationConstants.DateFilterDefaultDays).ToString();
                string EndDate = DateTime.Now.ToString();

                if (!string.IsNullOrEmpty(FromDateTime))
                {
                    StartDate = FromDateTime;
                }
                if (!string.IsNullOrEmpty(ToDateTime))
                {
                    EndDate = ToDateTime;
                }
                var inputModel = new { StartDate, EndDate, CompanyId, AccountingWorkflowId, dataTableSearchValues };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetQbSummary", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<USP_QuickBooksSummaryViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetQbSummary", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<QbReportViewModel>> GetQbReportGrid(string FromDateTime, string ToDateTime, int CompanyId, DataTableSearchModel dataTableSearchValues, int timeout = 30)
        {
            var response = new List<QbReportViewModel>();
            try
            {
                string StartDate = DateTime.Now.AddDays(ApplicationConstants.DateFilterDefaultDays).ToString();
                string EndDate = DateTime.Now.ToString();

                if (!string.IsNullOrEmpty(FromDateTime))
                {
                    StartDate = FromDateTime;
                }
                if (!string.IsNullOrEmpty(ToDateTime))
                {
                    EndDate = ToDateTime;
                }

                var inputModel = new { StartDate, EndDate, CompanyId, dataTableSearchValues };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetQbReport", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<QbReportViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetQbReportGrid", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<CompanyUserGridViewModel>> GetCompanyUsers(int CompanyId, DataTableSearchModel requestModel, CompanyUsersFilterType statusFilter = CompanyUsersFilterType.All, SiteFuelUserFilterType userRoleFilter = SiteFuelUserFilterType.All, int timeout = 30)
        {
            var response = new List<CompanyUserGridViewModel>();
            try
            {
                object inputModel = new { CompanyId = CompanyId, UserStatusId = (int)statusFilter, UserRoleId = (int)userRoleFilter, requestModel };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetCompanyUsers", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<CompanyUserGridViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetCompanyUsers", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<JobLocationDetailsViewModel>> GetJobLocationDetailsForSupplier(int companyId, string jobList = "", string inventoryCaptureTypeIds = "", int timeout = 30)
        {
            var response = new List<JobLocationDetailsViewModel>();
            try
            {
                object inputmodel = new
                {
                    @CompanyID = companyId,
                    @JobIds = string.IsNullOrEmpty(jobList) ? string.Empty : jobList,
                    @InventoryCaptureTypeIds = string.IsNullOrEmpty(inventoryCaptureTypeIds) ? string.Empty : inventoryCaptureTypeIds,
                };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetJobLocationDetails_By_UserName", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<JobLocationDetailsViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetJobLocationDetailsForSupplier", ex.Message, ex);
            }

            return response;
        }
        public async Task<List<JobLocationDetailsViewModel>> GetJobLocationDetailsForBuyer(int companyId, string jobList = "", string inventoryCaptureTypeIds = "", int timeout = 30)
        {
            var response = new List<JobLocationDetailsViewModel>();
            try
            {
                object inputmodel = new
                {
                    @CompanyID = companyId,
                    @JobIds = string.IsNullOrEmpty(jobList) ? string.Empty : jobList,
                    @InventoryDataCaptureTypeIds = string.IsNullOrEmpty(inventoryCaptureTypeIds) ? string.Empty : inventoryCaptureTypeIds,
                };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetBuyerJobLocationDetails", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<JobLocationDetailsViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetJobLocationDetailsForBuyer", ex.Message, ex);
            }

            return response;
        }
        public async Task<List<SuperAdminCompanyGridViewModel>> GetCompanies(DateTimeOffset startDate, DateTimeOffset endDate, CompanyDataTableViewModel requestModel, int companyTypeId = 0, int timeout = 30)
        {
            var response = new List<SuperAdminCompanyGridViewModel>();
            try
            {
                var dataTableSearchModel = new DataTableSearchModel(requestModel);
                object inputModel = new { StartDate = startDate, EndDate = endDate, CompanyTypeId = companyTypeId, DataTableSearchValues = dataTableSearchModel };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetCompanies", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<SuperAdminCompanyGridViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetCompanies", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<ImpersonationActivityLogViewModel>> GetImpersonedActivityLog(DateTimeOffset startDate, DateTimeOffset endDate, ImpersonateLogDataTableViewModel requestModel, int impersonatedBy = 0, int timeout = 30)
        {
            var response = new List<ImpersonationActivityLogViewModel>();
            try
            {
                var dataTableSearchModel = new DataTableSearchModel(requestModel);
                object inputModel = new { StartDate = startDate, EndDate = endDate, ImpersonatedBy = impersonatedBy, DataTableSearchValues = dataTableSearchModel };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetImpersonationActivityLogs", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<ImpersonationActivityLogViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetImpersonedActivityLog", ex.Message, ex);
            }

            return response;
        }

        public async Task<UspGetBuyerOrderDetail> GetBuyerOrderDetailAsync(int companyId, int orderId, int timeout = 30)
        {
            var response = new UspGetBuyerOrderDetail();
            try
            {
                object inputModel = new { CompanyId = companyId, OrderId = orderId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetBuyerOrderDetail", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspGetBuyerOrderDetail>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetBuyerOrderDetailAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<UspBuyerOrderStat> GetBuyerOrderStatAsync(int orderId, bool isProFormaAndSingleDelivery, int timeout = 30)
        {
            var response = new UspBuyerOrderStat();
            try
            {
                object inputModel = new { OrderId = orderId, IsProFormaAndSingleDelivery = isProFormaAndSingleDelivery };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetBuyerOrderStat", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspBuyerOrderStat>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetBuyerOrderStatAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<UspSupplierOrderStat> GetSupplierOrderStatAsync(int orderId, bool isProFormaAndSingleDelivery, int timeout = 30)
        {
            var response = new UspSupplierOrderStat();
            try
            {
                object inputModel = new { OrderId = orderId, IsProFormaAndSingleDelivery = isProFormaAndSingleDelivery };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetSupplierOrderStat", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspSupplierOrderStat>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetSupplierOrderStatAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<ChildCompanyViewModel>> GetParentCompanies(int parentCompanyId = 0, int timeout = 30)
        {
            var response = new List<ChildCompanyViewModel>();
            try
            {
                object inputModel = new { CompanyId = parentCompanyId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetParentCompanies", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<ChildCompanyViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetParentCompanies", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<CompanyGroupViewModel>> GetAllCompanyGroups(int parentCompanyId = 0, int timeout = 30)
        {
            var response = new List<CompanyGroupViewModel>();
            try
            {
                object inputModel = new { CompanyId = parentCompanyId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetCompanyGroups", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                var companyGroups = await Context.DataContext.Database.SqlQuery<CompanyGroupViewModel>(input.Query, input.Params.ToArray()).ToListAsync();

                if (companyGroups != null && companyGroups.Any())
                {
                    var parentCompanies = companyGroups.Where(t => t.ParentCompanyId == null).ToList();
                    if (parentCompanies != null && parentCompanies.Any())
                    {
                        foreach (var parent in parentCompanies)
                        {
                            var childs = companyGroups.Where(t => t.ParentCompanyId == parent.OwnerCompanyId).ToList();
                            CompanyGroupViewModel obj = new CompanyGroupViewModel();
                            obj.OwnerCompanyId = parent.OwnerCompanyId;
                            obj.CompanyType = parent.CompanyType;
                            obj.CompanyName = parent.CompanyName;
                            obj.OwnerCompanyType = parent.OwnerCompanyType;
                            StringBuilder companyName = new StringBuilder();
                            foreach (var child in childs)
                            {
                                companyName.Append(", " + child.CompanyName);
                            }

                            obj.SeletedChildCompanies = companyName.ToString().TrimStart(',');
                            response.Add(obj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetAllCompanyGroups", ex.Message, ex);
            }

            return response;
        }

        public async Task<CompanyGroupViewModel> GetCompanyGroupDetails(int parentCompanyId = 0, int timeout = 30)
        {
            var response = new CompanyGroupViewModel();
            try
            {
                object inputModel = new { CompanyId = parentCompanyId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetCompanyGroups", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                var companyGroups = await Context.DataContext.Database.SqlQuery<CompanyGroupViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
                if (companyGroups != null && companyGroups.Any())
                {
                    var parentCompany = companyGroups.FirstOrDefault(t => t.ParentCompanyId == null);
                    if (parentCompany != null)
                    {
                        response.OwnerCompanyId = parentCompany.OwnerCompanyId;
                        response.CompanyType = parentCompany.CompanyType;
                        response.CompanyName = parentCompany.CompanyName;
                        response.OwnerCompanyType = parentCompany.OwnerCompanyType;

                        StringBuilder companyName = new StringBuilder();
                        var childs = companyGroups.Where(t => t.ParentCompanyId == parentCompany.OwnerCompanyId).ToList();
                        foreach (var child in childs)
                        {
                            ChildCompanyViewModel obj = new ChildCompanyViewModel();
                            obj.Id = child.OwnerCompanyId;
                            obj.Name = child.CompanyName;
                            obj.ChildCompanyType = child.OwnerCompanyType;
                            response.Companies.Add(obj);

                            companyName.Append(", " + child.CompanyName);
                        }

                        response.SeletedChildCompanies = companyName.ToString().TrimStart(',');
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetCompanyGroupDetails", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<CompanyExternalIdGridViewModel>> GetExternalCompanyIdentifications(int companyId, int timeout = 30)
        {
            var response = new List<CompanyExternalIdGridViewModel>();
            try
            {
                var inputmodel = new { CompanyId = companyId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetExternalCompanyIdentifications", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<CompanyExternalIdGridViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetExternalCompanyIdentifications", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<int>> GetJobIdsOfUser(int userId, string groupIds = "", int timeout = 30)
        {
            var response = new List<int>();
            try
            {
                groupIds = string.IsNullOrWhiteSpace(groupIds) ? string.Empty : groupIds;
                var inputmodel = new { UserId = userId, GroupIds = groupIds };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetUserJobIds", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<int>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetJobIdsOfUser", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<UspJobProductModel>> GetBuyerJobsWithProductTypes(int userId, int companyId, int jobId, int timeout = 30)
        {
            var response = new List<UspJobProductModel>();
            try
            {
                var inputmodel = new { CompanyId = companyId, UserId = userId, JobId = jobId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetJobsWithProductTypes", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspJobProductModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetBuyerJobsWithProductTypes", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<InvoiceGridViewModel>> GetInvoiceGridAsync(InvoiceDataTableViewModel filter = null, int timeout = 30)
        {
            var response = new List<InvoiceGridViewModel>();
            try
            {
                DataTableSearchModel dataTableSearchValues = new DataTableSearchModel(filter);
                DateTimeOffset StartDate = Convert.ToDateTime(ApplicationConstants.DateRangeFilterStartDate);
                DateTimeOffset EndDate = DateTimeOffset.Now.Date.AddDays(1);
                if (!string.IsNullOrEmpty(filter.StartDate))
                {
                    StartDate = Convert.ToDateTime(filter.StartDate).Date;
                }
                if (!string.IsNullOrEmpty(filter.EndDate))
                {
                    EndDate = Convert.ToDateTime(filter.EndDate).Date.AddDays(1);
                }

                object inputModel = new { StartDate, EndDate, DataTableSearchValues = dataTableSearchValues };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetInvoices", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<InvoiceGridViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetInvoiceGridAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<InvoiceReportViewModel>> GetInvoiceReportAsync(InvoiceReportFilter filter = null, DataTableSearchModel dataTableSearchModel = null, int timeout = 30)
        {
            var response = new List<InvoiceReportViewModel>();
            try
            {
                DateTimeOffset startDate = Convert.ToDateTime(ApplicationConstants.DateRangeFilterStartDate);
                DateTimeOffset endDate = DateTimeOffset.Now.Date.AddDays(1);
                if (!string.IsNullOrEmpty(filter.StartDate))
                {
                    startDate = Convert.ToDateTime(filter.StartDate).Date;
                }
                if (!string.IsNullOrEmpty(filter.EndDate))
                {
                    endDate = Convert.ToDateTime(filter.EndDate).Date.AddDays(1);
                }

                object inputModel = new
                {
                    CompanyId = filter.CompanyId,
                    StartDate = startDate,
                    EndDate = endDate,
                    Suppliers = filter.SupplierCompanyIds != null ? string.Join(",", filter.SupplierCompanyIds) : string.Empty,
                    Customers = filter.CustomerCompanyIds != null ? string.Join(",", filter.CustomerCompanyIds) : string.Empty,
                    Jobs = filter.JobIds != null ? string.Join(",", filter.JobIds) : string.Empty,
                    CompanyProfile = filter.CompanyProfile,
                    DataTableSearchValues = dataTableSearchModel
                };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetInvoiceReport", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<InvoiceReportViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetInvoiceReportAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<InvoiceReconcilationViewModel>> GetInvoiceReconcilationAsync(InvoiceReportFilter filter = null, DataTableSearchModel dataTableSearchModel = null, int timeout = 30)
        {
            var response = new List<InvoiceReconcilationViewModel>();
            try
            {
                DateTimeOffset startDate = Convert.ToDateTime(ApplicationConstants.DateRangeFilterStartDate);
                DateTimeOffset endDate = DateTimeOffset.Now.Date.AddDays(1);
                if (!string.IsNullOrEmpty(filter.StartDate))
                {
                    startDate = Convert.ToDateTime(filter.StartDate).Date;
                }
                if (!string.IsNullOrEmpty(filter.EndDate))
                {
                    endDate = Convert.ToDateTime(filter.EndDate).Date.AddDays(1);
                }

                object inputModel = new
                {
                    CompanyId = filter.CompanyId,
                    StartDate = startDate,
                    EndDate = endDate,
                    Suppliers = filter.SupplierCompanyIds != null ? string.Join(",", filter.SupplierCompanyIds) : string.Empty,
                    Customers = filter.CustomerCompanyIds != null ? string.Join(",", filter.CustomerCompanyIds) : string.Empty,
                    Jobs = filter.JobIds != null ? string.Join(",", filter.JobIds) : string.Empty,
                    CompanyProfile = filter.CompanyProfile,
                    DataTableSearchValues = dataTableSearchModel
                };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetInvoiceReconcilation", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<InvoiceReconcilationViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetInvoiceReconcilationAsync", ex.Message, ex);
            }

            return response;
        }
        public async Task<List<UspGetQuickUpdateHistory>> GetQuickUpdateHistory(int companyId, int countryId, int timeout = 30)
        {
            var response = new List<UspGetQuickUpdateHistory>();
            try
            {
                var inputmodel = new { CompanyId = companyId, CountryId = countryId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetQuickUpdateHistory", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspGetQuickUpdateHistory>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetQuickUpdateHistory", ex.Message, ex);
            }
            return response;
        }

        public async Task<UspSourceBasedTerminalPrice> GetOpisTerminalPrice(FuelPricingRequestViewModel viewModel, int timeout = 30)
        {
            var response = new UspSourceBasedTerminalPrice();
            try
            {
                var inputmodel = new
                {
                    PriceDate = viewModel.DropEndDate,
                    CityRackTerminalId = viewModel.CityGroupTerminalId ?? 0,
                    ProductId = viewModel.FuelTypeId,
                    viewModel.FuelRequestPricingDetails.FeedTypeId,
                    RackTypeId = viewModel.PricingTypeId,
                    BrandTypeId = viewModel.FuelRequestPricingDetails.FuelClassTypeId,
                    PriceTypeId = viewModel.FuelRequestPricingDetails.PricingSourceQuantityIndicatorTypeId,
                    WeekendDropPricingDay = (int)viewModel.FuelRequestPricingDetails.WeekendDropPricingDay
                };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetOpisTerminalPrice", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspSourceBasedTerminalPrice>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetOpisTerminalPrice", ex.Message + $"[{viewModel.ToString()}]", ex);
            }
            return response;
        }

        public async Task<UspSourceBasedTerminalPrice> GetPlattsTerminalPrice(FuelPricingRequestViewModel viewModel, int timeout = 30)
        {
            var response = new UspSourceBasedTerminalPrice();
            try
            {
                var inputmodel = new
                {
                    PriceDate = viewModel.DropEndDate,
                    CityRackTerminalId = viewModel.CityGroupTerminalId ?? 0,
                    ProductId = viewModel.FuelTypeId,
                    WeekendPricingDropDay = (int)viewModel.FuelRequestPricingDetails.WeekendDropPricingDay
                };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetPlattsTerminalPrice", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspSourceBasedTerminalPrice>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetPlattsTerminalPrice", ex.Message + $"[{viewModel.ToString()}]", ex);
            }
            return response;
        }

        public async Task<List<UspGetSupplierOrderAssetDrop>> GetSupplierOrderAssetDrops(int orderId, int timeout = 30)
        {
            var response = new List<UspGetSupplierOrderAssetDrop>();
            try
            {
                var inputmodel = new { OrderId = orderId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetSupplierOrderAssetDrops", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspGetSupplierOrderAssetDrop>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetSupplierOrderAssetDrops", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<UspStatementSummary>> GetStatementSummary(int companyId, int userId, StatementSummaryDataViewModel filter, int timeout = 30)
        {
            var response = new List<UspStatementSummary>();
            try
            {
                DataTableSearchModel dataTableSearchValues = new DataTableSearchModel(filter);
                DateTimeOffset StartDate = Convert.ToDateTime(ApplicationConstants.DateRangeFilterStartDate);
                if (!string.IsNullOrEmpty(filter.StartDate))
                {
                    StartDate = Convert.ToDateTime(filter.StartDate).Date;
                }
                DateTimeOffset EndDate = DateTimeOffset.Now.Date.AddDays(1);
                if (!string.IsNullOrEmpty(filter.EndDate))
                {
                    EndDate = Convert.ToDateTime(filter.EndDate).Date.AddDays(1);
                }
                object inputModel = new
                {
                    CompanyId = companyId,
                    UserId = userId,
                    CustomerId = filter.CustomerId,
                    StatementId = !string.IsNullOrEmpty(filter.StatementId) && filter.StatementId.ToLower() != Resource.lblStatementId.ToLower() ? filter.StatementId : string.Empty,
                    CurrencyType = (int)filter.Currency,
                    CountryId = filter.CountryId,
                    StartDate = StartDate,
                    EndDate = EndDate,
                    DataTableSearchValues = dataTableSearchValues
                };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetStatementSummary", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspStatementSummary>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetStatementSummary", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<SalesCalculatorGridViewModel>> GetOpisTerminalPricesForCalculator(SalesCalculatorDatatableViewModel viewModel, DataTableSearchModel requestModel, int timeout = 30)
        {
            var response = new List<SalesCalculatorGridViewModel>();
            try
            {
                var feedTypeId = viewModel.FeedTypeId ?? 0;
                var inputmodel = new
                {
                    SourceId = viewModel.PricingSourceId,
                    PriceDate = viewModel.PriceDate,
                    CityRackTerminalIds = string.Join(",", viewModel.CityTerminalIds),
                    ProductId = viewModel.ProductId,
                    FeedTypeId = feedTypeId,
                    BrandTypeId = viewModel.BrandTypeId ?? 0,
                    PriceTypeId = viewModel.PriceTypeId ?? 0,
                    requestModel
                };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetOpisTerminalPricesForCalculator", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<SalesCalculatorGridViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetOpisTerminalPricesForCalculator", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<SalesCalculatorGridViewModel>> GetPlattsTerminalPricesForCalculator(SalesCalculatorViewModel viewModel, DataTableSearchModel requestModel, int timeout = 30)
        {
            var response = new List<SalesCalculatorGridViewModel>();
            try
            {
                var inputmodel = new
                {
                    SourceId = viewModel.FuelPricingDetails.PricingSourceId,
                    PriceDate = viewModel.PriceDate,
                    CityRackTerminalIds = string.Join(",", viewModel.CityTerminalIds),
                    ProductId = viewModel.CommonFuelTypeId,
                    requestModel
                };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetPlattsTerminalPricesForCalculator", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<SalesCalculatorGridViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetPlattsTerminalPricesForCalculator", ex.Message, ex);
            }
            return response;
        }

        public async Task<UspInvoicePdfDetail> GetInvoicePdfDetailsAsync(int invoiceId, int timeout = 30)
        {
            UspInvoicePdfDetail response = null;
            try
            {
                var inputmodel = new { InvoiceId = invoiceId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetInvoicePdfDetails", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspInvoicePdfDetail>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetInvoicePdfDetailsAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<UspConsolidatedInvoicePdfDetail> GetConsolidatedInvoicePdfAsync(int invoiceHeaderId, int timeout = 30)
        {
            UspConsolidatedInvoicePdfDetail response = null;
            var connection = Context.DataContext.Database.Connection;
            var command = connection.CreateCommand();
            try
            {
                
                command.CommandText = "[dbo].[usp_GetConsolidatedInvoicePdf]";
                command.CommandType = CommandType.StoredProcedure;

                command.CommandTimeout = timeout;

                // add parameters
                command.Parameters.Add(new SqlParameter("@InvoiceHeaderId", invoiceHeaderId));

                connection.Open();
                var reader = await command.ExecuteReaderAsync();

                UspPdfHeaderViewModel pdfHeaderDetail = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<UspPdfHeaderViewModel>(reader).FirstOrDefault();
                reader.NextResult();

                List<UspBolDetail> liftDetails = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<UspBolDetail>(reader).ToList();
                reader.NextResult();

                List<UspBolDetail> bolDetails = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<UspBolDetail>(reader).ToList();
                reader.NextResult();

                List<UspPickupAddressViewModel> pickupDetails = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<UspPickupAddressViewModel>(reader).ToList();
                reader.NextResult();

                List<UspPdfDetail> invoiceDropDetails = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<UspPdfDetail>(reader).ToList();
                reader.NextResult();

                List<UspInvoicePdfFuelFee> invoiceFees = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<UspInvoicePdfFuelFee>(reader).ToList();
                reader.NextResult();

                List<UspInvoicePdfTaxDetail> invoiceTaxes = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<UspInvoicePdfTaxDetail>(reader).ToList();
                reader.NextResult();

                List<UspInvoicePdfSpecialInstruction> specialInstructions = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<UspInvoicePdfSpecialInstruction>(reader).ToList();
                reader.NextResult();

                List<UspInvoicePdfAssetDrop> assetDrops = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<UspInvoicePdfAssetDrop>(reader).ToList();

                if (response == null)
                    response = new UspConsolidatedInvoicePdfDetail();

                response.InvoiceHeaderDetail = pdfHeaderDetail;
                response.InvoiceDropDetails = invoiceDropDetails;
                response.PickupLocations = pickupDetails;

                if (liftDetails != null)
                    response.LiftDetails = new List<UspBolDetail>();
                response.LiftDetails = liftDetails;

                if (bolDetails != null)
                    response.BolDetails = new List<UspBolDetail>();
                response.BolDetails = bolDetails;

                if (invoiceFees != null)
                    response.FuelFeeDetails = new List<UspInvoicePdfFuelFee>();
                response.FuelFeeDetails = invoiceFees;

                if (invoiceTaxes != null)
                    response.TaxDetails = new List<UspInvoicePdfTaxDetail>();
                response.TaxDetails = invoiceTaxes;

                if (specialInstructions != null)
                    response.SpecialInstructions = new List<UspInvoicePdfSpecialInstruction>();
                response.SpecialInstructions = specialInstructions;

                if (assetDrops != null)
                    response.AssetDetails = new List<UspInvoicePdfAssetDrop>();
                response.AssetDetails = assetDrops;

                reader.Close();
              
                return response;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetConsolidatedInvoicePdfAsync", ex.Message, ex);
            }
            finally
            {
                command.Dispose();
                connection.Close();
            }

            return response;
        }

        public async Task<UspOrderPdfDetails> GetProformaBDNPdfAsync(int orderId, int orderDetailVersionId = 0, int timeout = 30)
        {
            var response = new UspOrderPdfDetails();
            try
            {
                var inputmodel = new { OrderId = orderId, OrderDetailVersionId = orderDetailVersionId };
                var input = SqlHelperMethods.GetStoredProcedure("Usp_GetProformaBDNPdf", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspOrderPdfDetails>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetProformaBDNPdfAsync", ex.Message, ex);
            }
            return response;
        }
        public async Task<UspBDRPdfDetail> GetBDRPdfAsync(int invoiceHeaderId, int timeout = 30)
        {
            UspBDRPdfDetail response = null;
            var connection = Context.DataContext.Database.Connection;
            var command = connection.CreateCommand();
            try
            {
               
                command.CommandText = "[dbo].[usp_GetBDRPdf]";
                command.CommandType = CommandType.StoredProcedure;

                command.CommandTimeout = timeout;

                // add parameters
                command.Parameters.Add(new SqlParameter("@InvoiceHeaderId", invoiceHeaderId));

                connection.Open();
                var reader = await command.ExecuteReaderAsync();

                UspPdfHeaderViewModel pdfHeaderDetail = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<UspPdfHeaderViewModel>(reader).FirstOrDefault();
                reader.NextResult();

                List<UspPickupAddressViewModel> pickupDetails = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<UspPickupAddressViewModel>(reader).ToList();
                reader.NextResult();

                List<UspPdfDetail> invoiceDropDetails = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<UspPdfDetail>(reader).ToList();
                reader.NextResult();

                List<BDRDetailsModel> bDRDetails = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<BDRDetailsModel>(reader).ToList();
                reader.NextResult();

                if (response == null)
                    response = new UspBDRPdfDetail();

                response.InvoiceHeaderDetail = pdfHeaderDetail;
                response.InvoiceDropDetails = invoiceDropDetails;
                response.PickupLocations = pickupDetails;
                response.BDRDetailsModel = bDRDetails;

                reader.Close();
               

                return response;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetBDRPdfAsync", ex.Message, ex);
            }
            finally
            {
                command.Dispose();
                connection.Close();
            }

            return response;
        }


        public async Task<List<UspInvoicePdfAssetDrop>> GetInvoicePdfAssetDropsAsync(int invoiceId, int timeout = 30)
        {
            var response = new List<UspInvoicePdfAssetDrop>();
            try
            {
                var inputmodel = new { InvoiceId = invoiceId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetInvoicePdfAssetDrops", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspInvoicePdfAssetDrop>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetInvoicePdfAssetDropsAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<UspInvoicePdfFuelFee>> GetInvoicePdfFuelFeesAsync(int invoiceId, int timeout = 30)
        {
            var response = new List<UspInvoicePdfFuelFee>();
            try
            {
                var inputmodel = new { InvoiceId = invoiceId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetInvoicePdfFuelFees", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspInvoicePdfFuelFee>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetInvoicePdfFuelFeesAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<UspInvoicePdfFuelFee>> GetInvoiceDetailsFuelFeesAsync(int invoiceHeaderId, int timeout = 30)
        {
            var response = new List<UspInvoicePdfFuelFee>();
            try
            {
                var inputmodel = new { InvoiceHeaderId = invoiceHeaderId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetInvoiceDetailsFuelFees", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspInvoicePdfFuelFee>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetInvoiceDetailsFuelFeesAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<UspInvoicePdfSpecialInstruction>> GetInvoicePdfSpecialInstructionsAsync(int invoiceId, int timeout = 30)
        {
            var response = new List<UspInvoicePdfSpecialInstruction>();
            try
            {
                var inputmodel = new { InvoiceId = invoiceId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetInvoicePdfSpecialInstructions", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspInvoicePdfSpecialInstruction>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetInvoicePdfSpecialInstructionsAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<UspInvoicePdfTaxDetail>> GetInvoicePdfTaxDetailsAsync(int invoiceId, int timeout = 30)
        {
            var response = new List<UspInvoicePdfTaxDetail>();
            try
            {
                var inputmodel = new { InvoiceId = invoiceId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetInvoicePdfTaxDetails", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspInvoicePdfTaxDetail>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetInvoicePdfTaxDetailsAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<UspInvoicePdfTaxDetail>> GetConsolidatedInvoicePdfTaxesAsync(int invoiceHeaderId, int timeout = 30)
        {
            var response = new List<UspInvoicePdfTaxDetail>();
            try
            {
                var inputmodel = new { InvoiceHeaderId = invoiceHeaderId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetConsolidatedInvoicePdfTaxes", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspInvoicePdfTaxDetail>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetConsolidatedInvoicePdfTaxesAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<InvoiceNumberViewModel>> GetSplitLoadInvoicesAsync(string splitLoadChainId, int timeout = 30)
        {
            var response = new List<InvoiceNumberViewModel>();
            try
            {
                var inputmodel = new { SplitLoadChainId = splitLoadChainId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetSplitLoadInvoices", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<InvoiceNumberViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetSplitLoadInvoicesAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<AssetDropImagesViewModel>> GetAssetDropImagesAsync(int invoiceId, int timeout = 30)
        {
            var response = new List<AssetDropImagesViewModel>();
            try
            {
                var inputmodel = new { InvoiceId = invoiceId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetAssetDropImages", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<AssetDropImagesViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetAssetDropImagesAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<BolDetailViewModel>> GetBolDetailsAsync(int invoiceId, int timeout = 30)
        {
            var response = new List<BolDetailViewModel>();
            try
            {
                var inputmodel = new { InvoiceId = invoiceId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetBolDetails", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<BolDetailViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetBolDetailsAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<InvoiceLineItemDetailViewModel>> GetInvoiceLineItemDetailsAsync(int invoiceHeaderId, int timeout = 30)
        {
            var response = new List<InvoiceLineItemDetailViewModel>();
            try
            {
                var inputmodel = new { InvoiceId = invoiceHeaderId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetInvoiceLineItemDetails", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<InvoiceLineItemDetailViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
                response = response.OrderByDescending(x => x.TaxInvoiceId).ToList();
                if (response != null && response.Any(m => m.IsMarineLocation))
                {
                    var result = new List<InvoiceLineItemDetailViewModel>();
                    var marineRes = response.ToLookup(inlineItem => inlineItem.OrderId, inlineItem => inlineItem);
                    foreach (var item in marineRes)
                    {
                        result.Add(new InvoiceLineItemDetailViewModel()
                        {
                            OrderId = item.Key,
                            PoNumber = response.Where(w => w.OrderId == item.Key).FirstOrDefault().PoNumber,
                            TimeZoneName = response.Where(w => w.OrderId == item.Key).FirstOrDefault().TimeZoneName,
                            // AssetFilled = response.Where(w => w.OrderId == item.Key).FirstOrDefault().AssetFilled,
                            StatusId = response.Where(w => w.OrderId == item.Key).FirstOrDefault().StatusId,
                            AssetCount = response.Where(w => w.OrderId == item.Key).FirstOrDefault().AssetCount,
                            // Quantity = response.Where(w => w.OrderId == item.Key).Sum(s => s.Quantity),
                            Quantity = response.Where(w => w.OrderId == item.Key).FirstOrDefault().Quantity,
                            DroppedGallons = response.Where(w => w.OrderId == item.Key).Sum(s => s.DroppedGallons),
                            DropStartDate = response.Where(w => w.OrderId == item.Key).OrderBy(s => s.DropStartDate).FirstOrDefault().DropStartDate,
                            DropEndDate = response.Where(w => w.OrderId == item.Key).OrderByDescending(s => s.DropEndDate).FirstOrDefault().DropEndDate,
                            DriverName = String.Join(",", response.Where(w => w.OrderId == item.Key).Select(s => s.DriverName).Distinct().ToList()),
                            FuelType = String.Join(",", response.Where(w => w.OrderId == item.Key).Select(s => s.FuelType).Distinct().ToList()),
                            QuantityTypeId = response.Where(w => w.OrderId == item.Key).FirstOrDefault().QuantityTypeId,
                            UoM = response.Where(w => w.OrderId == item.Key).FirstOrDefault().UoM,
                            OrderTypeId = response.Where(w => w.OrderId == item.Key).FirstOrDefault().OrderTypeId,
                            IsPublicRequest = response.Where(w => w.OrderId == item.Key).FirstOrDefault().IsPublicRequest,
                            TaxInvoiceId = response.Where(w => w.OrderId == item.Key).FirstOrDefault().TaxInvoiceId,
                            FeesInvoiceId = response.Where(w => w.OrderId == item.Key).FirstOrDefault().FeesInvoiceId,
                            IsMarineLocation = true,
                            ConvertedQuantity = response.Where(w => w.OrderId == item.Key).Sum(s => s.ConvertedQuantity),
                            JobCountryId = response.Where(w => w.OrderId == item.Key).FirstOrDefault().JobCountryId,
                        }
                        );
                    }
                    result = result.OrderByDescending(x => x.TaxInvoiceId).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetInvoiceLineItemDetailsAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<TankRentalQueueMessage>> GetActiveTankRentalInvoiceMassage(DateTimeOffset currentDate, int orderStatusId, int orderId = 0, int timeout = 30)
        {
            var response = new List<TankRentalQueueMessage>();
            try
            {
                var inputmodel = new { CurrentDate = currentDate, OrderStatusId = orderStatusId, OrderId = orderId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetActiveTankRentalInvoiceMassage", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<TankRentalQueueMessage>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetActiveTankRentalInvoiceMassage", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetFreightRateTables(FreightRateInputViewModel freightRateInput, int timeout = 30)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                if (freightRateInput.FreightRateRuleType > 0 && freightRateInput.TableType > 0 && freightRateInput.FuelTypeId > 0 && freightRateInput.SourceRegionIds != null && freightRateInput.SourceRegionIds.Any())
                {
                    var inputmodel = new
                    {
                        FreightRateRuleType = freightRateInput.FreightRateRuleType,
                        TableType = freightRateInput.TableType,
                        CustomerId = freightRateInput.TableType == (int)TableTypes.CustomerSpecific ? freightRateInput.CustomerId : 0,
                        TerminalId = freightRateInput.TerminalId,
                        BulkPlantId = freightRateInput.BulkPlantId,
                        SupplierId = freightRateInput.SupplierId,
                        FuelTypeId = freightRateInput.FuelTypeId,
                        SourceRegionIds = freightRateInput.SourceRegionIds != null ? string.Join(",", freightRateInput.SourceRegionIds) : string.Empty,
                        SelectedTerminals = freightRateInput.SelectedTerminals != null ? string.Join(",", freightRateInput.SelectedTerminals) : string.Empty,
                        SelectedBulkPlants = freightRateInput.SelectedBulkPlants != null ? string.Join(",", freightRateInput.SelectedBulkPlants) : string.Empty,
                    };
                    var input = SqlHelperMethods.GetStoredProcedure("usp_GetFreightRateTables", inputmodel);

                    Context.DataContext.Database.CommandTimeout = timeout;
                    response = await Context.DataContext.Database.SqlQuery<DropdownDisplayItem>(input.Query, input.Params.ToArray()).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetFreightRateTables", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetFuelSurchargeTables(FreightRateInputViewModel freightRateInput, int timeout = 30)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                if (freightRateInput.TableType > 0 && freightRateInput.SourceRegionIds != null && freightRateInput.SourceRegionIds.Any())
                {
                    var inputmodel = new
                    {
                        TableType = freightRateInput.TableType,
                        CustomerId = freightRateInput.TableType == (int)TableTypes.CustomerSpecific ? freightRateInput.CustomerId : 0,
                        TerminalId = freightRateInput.TerminalId,
                        BulkPlantId = freightRateInput.BulkPlantId,
                        SupplierId = freightRateInput.SupplierId,
                        SourceRegionIds = freightRateInput.SourceRegionIds != null ? string.Join(",", freightRateInput.SourceRegionIds) : string.Empty,
                        SelectedTerminals = freightRateInput.SelectedTerminals != null ? string.Join(",", freightRateInput.SelectedTerminals) : string.Empty,
                        SelectedBulkPlants = freightRateInput.SelectedBulkPlants != null ? string.Join(",", freightRateInput.SelectedBulkPlants) : string.Empty,
                    };
                    var input = SqlHelperMethods.GetStoredProcedure("usp_GetFuelSurchargeTables", inputmodel);

                    Context.DataContext.Database.CommandTimeout = timeout;
                    response = await Context.DataContext.Database.SqlQuery<DropdownDisplayItem>(input.Query, input.Params.ToArray()).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetFuelSurchargeTables", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetFuelSurchargeTablesForInvoice(FreightRateInputViewModel freightRateInput, int timeout = 30)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var order = await Context.DataContext.Orders.Where(t => t.Id == freightRateInput.OrderId).Select(t => new
                {
                    t.BuyerCompanyId,
                }).FirstOrDefaultAsync();

                var inputmodel = new
                {
                    TableType = freightRateInput.TableType,
                    CustomerId = freightRateInput.TableType == (int)TableTypes.CustomerSpecific ? order.BuyerCompanyId : 0,
                    TerminalId = freightRateInput.TerminalId,
                    BulkPlantId = freightRateInput.BulkPlantId,
                    SupplierId = freightRateInput.SupplierId,
                };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetFuelSurchargeTablesForInvoice", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<DropdownDisplayItem>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetFuelSurchargeTablesForInvoice", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetFreightRateTablesForInvoice(FreightRateInputViewModel freightRateInput, int timeout = 30)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var order = await Context.DataContext.Orders.Where(t => t.Id == freightRateInput.OrderId).Select(t => new
                {
                    t.BuyerCompanyId,
                    t.FuelRequest.FuelTypeId
                }).FirstOrDefaultAsync();

                var inputmodel = new
                {
                    FreightRateRuleType = freightRateInput.FreightRateRuleType,
                    TableType = freightRateInput.TableType,
                    CustomerId = freightRateInput.TableType == (int)TableTypes.CustomerSpecific ? order.BuyerCompanyId : 0,
                    TerminalId = freightRateInput.TerminalId,
                    BulkPlantId = freightRateInput.BulkPlantId,
                    SupplierId = freightRateInput.SupplierId,
                    FuelTypeId = order.FuelTypeId,
                };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetFreightRateTablesForInvoice", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<DropdownDisplayItem>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetFreightRateTablesForInvoice", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetAccessorialFeeTablesForInvoice(FreightRateInputViewModel freightRateInput, int timeout = 30)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var inputmodel = new
                {
                    TableType = freightRateInput.TableType,
                    CustomerId = freightRateInput.TableType == (int)TableTypes.CustomerSpecific ? freightRateInput.CustomerId : 0,
                    TerminalId = freightRateInput.TerminalId,
                    BulkPlantId = freightRateInput.BulkPlantId,
                    SupplierId = freightRateInput.SupplierId
                };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetAccessorialFeeTablesForInvoice", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<DropdownDisplayItem>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetAccessorialFeeTablesForInvoice", ex.Message, ex);
            }
            return response;
        }

        public List<UspHistoricalPriceDetailsViewModel> GetFuelSurchargePriceHistory(int forPeriod, FuelSurchagePricingType surchargePricingType,
             SurchargeProductTypes surchargeProductType, FuelSurchageArea fuelSurchageArea = FuelSurchageArea.US, int timeout = 30)
        {
            var response = new List<UspHistoricalPriceDetailsViewModel>();
            try
            {
                var inputmodel = new
                {
                    ForPeriod = forPeriod,
                    SurchargePricingType = (int)surchargePricingType,
                    SurchargeProductType = (int)surchargeProductType,
                    FuelSurchageArea = (int)fuelSurchageArea
                };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetFuelSurchargePriceHistory", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = Context.DataContext.Database.SqlQuery<UspHistoricalPriceDetailsViewModel>(input.Query, input.Params.ToArray()).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetFuelSurchargePriceHistory", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetAccessorialFeeTables(FreightRateInputViewModel freightRateInput, int timeout = 30)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                if (freightRateInput.TableType > 0 && freightRateInput.SourceRegionIds != null && freightRateInput.SourceRegionIds.Any())
                {
                    var inputmodel = new
                    {
                        TableType = freightRateInput.TableType,
                        CustomerId = freightRateInput.TableType == (int)TableTypes.CustomerSpecific ? freightRateInput.CustomerId : 0,
                        TerminalId = freightRateInput.TerminalId,
                        BulkPlantId = freightRateInput.BulkPlantId,
                        SupplierId = freightRateInput.SupplierId,
                        SourceRegionIds = freightRateInput.SourceRegionIds != null ? string.Join(",", freightRateInput.SourceRegionIds) : string.Empty,
                        SelectedTerminals = freightRateInput.SelectedTerminals != null ? string.Join(",", freightRateInput.SelectedTerminals) : string.Empty,
                        SelectedBulkPlants = freightRateInput.SelectedBulkPlants != null ? string.Join(",", freightRateInput.SelectedBulkPlants) : string.Empty,
                    };
                    var input = SqlHelperMethods.GetStoredProcedure("usp_GetAccessorialFeeTables", inputmodel);

                    Context.DataContext.Database.CommandTimeout = timeout;
                    response = await Context.DataContext.Database.SqlQuery<DropdownDisplayItem>(input.Query, input.Params.ToArray()).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetAccessorialFeeTables", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetAccessorialFeeTablesForConsolidated(AccessorialFeeInvoiceInputViewModel accessorialFeeInput, int timeout = 30)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var inputmodel = new
                {
                    CustomerId = accessorialFeeInput.CustomerId,
                    TerminalId = accessorialFeeInput.TerminalId,
                    BulkPlantId = accessorialFeeInput.BulkPlantId,
                    SupplierId = accessorialFeeInput.SupplierId
                };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetAccessorialFeeTablesForConsolidated", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<DropdownDisplayItem>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetAccessorialFeeTablesForConsolidated", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetAccessorialFeeTablesForSelectedOrder(string orderIds, int timeout = 30)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var inputmodel = new
                {
                    OrderIds = orderIds
                };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetAccessorialFeeTablesForSelectedOrder", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<DropdownDisplayItem>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetAccessorialFeeTablesForSelectedOrder", ex.Message, ex);
            }
            return response;
        }

        public async Task<decimal> GetFreightCostForInvoice(FreightCostInputViewModel freightCostInput)
        {
            decimal response = 0;
            try
            {
                var freightRateRules = await Context.DataContext.FreightRateRules.Where(t => t.Id == freightCostInput.FreightRateRuleId).Select(t => new
                {
                    t.FreightRateRuleType,
                }).FirstOrDefaultAsync();

                if (freightRateRules != null)
                {
                    if (freightRateRules.FreightRateRuleType == FreightRateRuleType.Route)
                    {
                        response = await GetFreightCostForRoute(freightCostInput);
                    }
                    else if (freightRateRules.FreightRateRuleType == FreightRateRuleType.Range)
                    {
                        response = await GetFreightCostForRange(freightCostInput);
                    }
                    else if (freightRateRules.FreightRateRuleType == FreightRateRuleType.P2P)
                    {
                        response = await GetFreightCostForPtoP(freightCostInput);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetFreightCostForInvoice", ex.Message, ex);
            }
            return response;
        }

        public async Task<decimal> GetFreightCostForRoute(FreightCostInputViewModel freightCostInput, int timeout = 30)
        {
            decimal response = 0;
            var freightCostResponse = new List<FreightCostQuantityBasedOutputViewModel>();
            try
            {
                var order = await Context.DataContext.Orders.Where(t => t.Id == freightCostInput.OrderId).Select(t => new
                {
                    FuelTypeId = t.FuelRequest.MstProduct.Id,
                }).SingleOrDefaultAsync();

                if (order != null)
                {
                    var inputmodel = new
                    {
                        FreightRateRuleId = freightCostInput.FreightRateRuleId,
                        TerminalId = freightCostInput.TerminalId,
                        BulkPlantId = freightCostInput.BulkPlantId,
                        SupplierId = freightCostInput.SupplierId,
                        FuelTypeId = order.FuelTypeId,
                    };
                    var input = SqlHelperMethods.GetStoredProcedure("usp_GetFreightCostForRoute", inputmodel);

                    Context.DataContext.Database.CommandTimeout = timeout;
                    freightCostResponse = await Context.DataContext.Database.SqlQuery<FreightCostQuantityBasedOutputViewModel>(input.Query, input.Params.ToArray()).ToListAsync();

                    var setEndQty = freightCostResponse.Where(t => t.EndQuantity == null).FirstOrDefault();
                    setEndQty.EndQuantity = Decimal.MaxValue;

                    response = freightCostResponse.Where(t => t.StartQuantity <= freightCostInput.DeliveredQuantity && t.EndQuantity >= freightCostInput.DeliveredQuantity).Select(t => t.FuelCost).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetFreightCostForRoute", ex.Message, ex);
            }
            return response;
        }

        public async Task<decimal> GetFreightCostForRange(FreightCostInputViewModel freightCostInput, int timeout = 30)
        {
            decimal response = 0;
            var freightCostResponse = new List<FreightCostDistanceBasedOutputViewModel>();
            try
            {
                var order = await Context.DataContext.Orders.Where(t => t.Id == freightCostInput.OrderId).Select(t => new
                {
                    FuelTypeId = t.FuelRequest.MstProduct.Id,
                    t.BuyerCompanyId,
                }).SingleOrDefaultAsync();

                if (order != null)
                {
                    var inputmodel = new
                    {
                        FreightRateRuleId = freightCostInput.FreightRateRuleId,
                        TerminalId = freightCostInput.TerminalId,
                        BulkPlantId = freightCostInput.BulkPlantId,
                        SupplierId = freightCostInput.SupplierId,
                        FuelTypeId = order.FuelTypeId,
                    };
                    var input = SqlHelperMethods.GetStoredProcedure("usp_GetFreightCostForRange", inputmodel);

                    Context.DataContext.Database.CommandTimeout = timeout;
                    var tempFreightCostResponse = await Context.DataContext.Database.SqlQuery<FreightCostDistanceBasedOutputViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
                    int loopValue = 1;
                    decimal previousQuantity = 0;
                    foreach (var item in tempFreightCostResponse)
                    {
                        var freightCost = new FreightCostDistanceBasedOutputViewModel();
                        if (loopValue == 1)
                        {
                            freightCost.StartDistance = 1;
                            freightCost.EndDistance = item.EndDistance;
                            freightCost.FuelCost = item.FuelCost;
                            previousQuantity = item.EndDistance.Value;
                        }
                        else if (loopValue >= 1 && loopValue != tempFreightCostResponse.Count)
                        {
                            freightCost.StartDistance = previousQuantity + 1;
                            freightCost.EndDistance = item.EndDistance;
                            freightCost.FuelCost = item.FuelCost;
                            previousQuantity = item.EndDistance.Value;
                        }
                        else if (loopValue == tempFreightCostResponse.Count)
                        {
                            freightCost.StartDistance = previousQuantity + 1;
                            freightCost.EndDistance = decimal.MaxValue;
                            freightCost.FuelCost = item.FuelCost;
                        }
                        loopValue++;
                        freightCostResponse.Add(freightCost);
                    }

                    response = freightCostResponse.Where(t => t.StartDistance <= freightCostInput.Distance && t.EndDistance >= freightCostInput.Distance).Select(t => t.FuelCost).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetFreightCostForRange", ex.Message, ex);
            }
            return response;
        }

        public async Task<decimal> GetFreightCostForPtoP(FreightCostInputViewModel freightCostInput, int timeout = 30)
        {
            decimal response = 0;
            try
            {
                var order = await Context.DataContext.Orders.Where(t => t.Id == freightCostInput.OrderId).Select(t => new
                {
                    FuelTypeId = t.FuelRequest.MstProduct.Id,
                    t.FuelRequest.JobId
                }).SingleOrDefaultAsync();

                if (order != null)
                {
                    var inputmodel = new
                    {
                        FreightRateRuleId = freightCostInput.FreightRateRuleId,
                        TerminalId = freightCostInput.TerminalId,
                        BulkPlantId = freightCostInput.BulkPlantId,
                        SupplierId = freightCostInput.SupplierId,
                        FuelTypeId = order.FuelTypeId,
                        JobId = order.JobId
                    };
                    var input = SqlHelperMethods.GetStoredProcedure("usp_GetFreightCostForPtoP", inputmodel);

                    Context.DataContext.Database.CommandTimeout = timeout;
                    response = await Context.DataContext.Database.SqlQuery<decimal>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetFreightCostForPtoP", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<ProductViewModel>> GetProductMappingGridAsync(ProductDataTableViewModel requestModel, DataTableSearchModel model, int timeout = 30)
        {
            var response = new List<ProductViewModel>();
            try
            {
                var inputmodel = new { ProductId = requestModel.ProductId, model };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetProductDetails_v1", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<ProductViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetProductMappingGridAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<AssetListViewModel>> GetAssetList(int companyId, int start = 0, int length = 10, int timeout = 30)
        {
            var response = new List<AssetListViewModel>();
            try
            {
                object inputModel = new { companyId = companyId, start = start, length = length };
                var input = SqlHelperMethods.GetStoredProcedure("[usp_GetAssetList]", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<AssetListViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetAssetList", ex.Message, ex);
            }

            return response;
        }

        public async Task<UspDuplicateInvoice> CheckForDuplicateInvoiceAsync(DuplicateInvoiceViewModel model, int timeout = 30)
        {
            var response = new UspDuplicateInvoice();
            try
            {
                var inputModel = new { OrderId = model.OrderId, DropQuantity = model.DropQuantity, PricePerGallon = model.PricePerGallon, DropDate = model.DropDate, UserId = model.UserId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_CheckDuplicateInvoices", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspDuplicateInvoice>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "CheckForDuplicateInvoiceAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<UspGetGroupOrders>> GetBuyerOrderGroups(int buyerCompanyId, string searchText, int jobId = 0, int searchCompanyId = 0, int groupTypeId = 0, int prdCategoryId = 0, int stateId = 0, int timeout = 30)
        {
            var response = new List<UspGetGroupOrders>();
            try
            {
                object inputModel = new { BuyerCompanyId = buyerCompanyId, JobId = jobId, SearchCompanyId = searchCompanyId, GroupTypeId = groupTypeId, ProductCategoryId = prdCategoryId, StateId = stateId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_ViewBuyerOrderGroups", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspGetGroupOrders>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetBuyerOrderGroups", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<UspGetGroupOrders>> GetSupplierOrderGroups(int supplierCompanyId, string searchText = "", int jobId = 0, int searchCompanyId = 0, int groupTypeId = 0, int prdCategoryId = 0, int stateId = 0, int timeout = 30)
        {
            var response = new List<UspGetGroupOrders>();
            try
            {
                object inputModel = new { SupplierCompanyId = supplierCompanyId, JobId = jobId, SearchCompanyId = searchCompanyId, GroupTypeId = groupTypeId, ProductCategoryId = prdCategoryId, StateId = stateId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_ViewSupplierOrderGroups", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspGetGroupOrders>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetSupplierOrderGroups", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<UspJobViewModel>> GetBuyerJobsBySupplierAsync(int supplierCompanyId, int buyerCompanyId, int timeout = 30)
        {
            var response = new List<UspJobViewModel>();
            try
            {
                var inputModel = new { @BuyerCompanyId = buyerCompanyId, @SupplierCompanyId = supplierCompanyId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetBuyerJobsBySupplier", inputModel);
                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspJobViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetBuyerJobsBySupplierAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<int> GetAssetCountForBuyerCompany(int companyId, int assetTypeId)
        {
            int response = 0;
            try
            {
                var inputmodel = new { CompanyId = companyId, AssetTypeId = assetTypeId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetAssetCountForBuyerCompany", inputmodel);
                response = await Context.DataContext.Database.SqlQuery<int>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetAssetCountForBuyerCompany", ex.Message, ex);
            }
            return response;
        }
        public async Task<bool> GetSupplierDiptestAvailability(int companyId)
        {
            bool response = false;
            try
            {
                var inputmodel = new { CompanyId = companyId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetSupplierDiptestAvailability", inputmodel);
                int result = await Context.DataContext.Database.SqlQuery<int>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
                if (result > 0)
                {
                    response = true;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetSupplierDiptestAvailability", ex.Message, ex);
            }
            return response;
        }
        public async Task<int> GetAcceptedCompanyIdByJobId(int jobId)
        {
            int response = 0;
            try
            {
                var inputmodel = new { JobId = jobId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetAcceptedCompanyIdByJobId", inputmodel);
                response = await Context.DataContext.Database.SqlQuery<int>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetAcceptedCompanyIdByJobId", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<UspGetDispatcherDriverLocation>> GetOnGoingLoadsAsync(int companyId, WhereIsMyDriverInputModel loadInput)
        {
            var response = new List<UspGetDispatcherDriverLocation>();
            try
            {
                loadInput.ToDate = loadInput.ToDate.AddDays(1);
                loadInput.DriverSearch = loadInput.DriverSearch.Replace("-", string.Empty);
                var inputmodel = new
                {
                    CompanyId = companyId,
                    States = string.Join(",", loadInput.States),
                    loadInput.FromDate,
                    loadInput.ToDate,
                    loadInput.DriverSearch,
                    RegionId = loadInput.RegionId ?? string.Empty,
                    Priorities = string.Join(",", loadInput.Priorities),
                    DispacherId = string.IsNullOrEmpty(loadInput.DispacherId) ? string.Empty : loadInput.DispacherId,
                    CustomerId = (int)loadInput.CustomerId,
                    Carriers = loadInput.Carriers == null ? String.Empty : string.Join(",", loadInput.Carriers),
                    ShowCarrierManaged = loadInput.IsShowCarrierManaged,
                    InventoryCaptureTypeIds = loadInput.InventoryCaptureType == "" ? String.Empty : string.Join(",", loadInput.InventoryCaptureType),
                };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetDispatcherOnGoingLoads", inputmodel);
                response = await Context.DataContext.Database.SqlQuery<UspGetDispatcherDriverLocation>(input.Query, input.Params.ToArray()).ToListAsync();
                if (response != null && response.Count > 0)
                {
                    foreach (var item in response)
                    {
                        if (item.AppLastUpdatedDate != null)
                        {
                            DateTime ut = DateTime.SpecifyKind(Convert.ToDateTime(item.AppLastUpdatedDate), DateTimeKind.Utc);
                            DateTimeOffset date = new DateTimeOffset(ut);
                            date = date.ToTargetDateTimeOffset(item.TimeZoneName).DateTime;
                            DateTimeOffset currentDateTime = DateTimeOffset.Now.ToTargetDateTimeOffset(item.TimeZoneName).DateTime;
                            TimeSpan span = currentDateTime.Subtract(date);
                            if (span.TotalMinutes <= ApplicationConstants.DriverOnlineTimeInMinutes)
                                item.IsOnline = true;
                            item.AppLastUpdatedDate = date.ToString(Resource.constFormatDateTime);


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetOnGoingLoadsAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<UspGetDispatcherDriverLocation>> GetDispatcherLoadsAsync(int companyId, WhereIsMyDriverInputModel loadInput, DataTableSearchModel requestModel)
        {
            var response = new List<UspGetDispatcherDriverLocation>();
            try
            {
                loadInput.ToDate = loadInput.ToDate.AddDays(1);
                loadInput.DriverSearch = loadInput.DriverSearch.Replace("-", string.Empty);
                var inputmodel = new
                {
                    CompanyId = companyId,
                    States = string.Join(",", loadInput.States),
                    loadInput.FromDate,
                    loadInput.ToDate,
                    loadInput.DriverSearch,
                    Priority = (int)loadInput.Priority,
                    RegionId = string.IsNullOrEmpty(loadInput.RegionId) ? string.Empty : loadInput.RegionId,
                    DispacherId = string.IsNullOrEmpty(loadInput.DispacherId) ? string.Empty : loadInput.DispacherId,
                    CustomerId = (int)loadInput.CustomerId,
                    Carriers = loadInput.Carriers == null ? String.Empty : string.Join(",", loadInput.Carriers),
                    ShowCarrierManaged = loadInput.IsShowCarrierManaged,
                    InventoryCaptureTypeIds = string.IsNullOrEmpty(loadInput.InventoryCaptureType) ? string.Empty : loadInput.InventoryCaptureType,
                    requestModel
                };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetDispatcherLoads", inputmodel);
                response = await Context.DataContext.Database.SqlQuery<UspGetDispatcherDriverLocation>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetDispatcherLoadsAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<UspGetDispatcherDriverLocation>> GetOnGoingLoadsForMapAsync(int companyId, BuyerWhereIsMyDriverInputModel loadInput)
        {
            var response = new List<UspGetDispatcherDriverLocation>();
            try
            {
                loadInput.ToDate = loadInput.ToDate.AddDays(1);
                loadInput.DriverSearch = loadInput.DriverSearch.Replace("-", string.Empty);
                var inputmodel = new
                {
                    CompanyId = companyId,
                    States = string.Join(",", loadInput.States),
                    loadInput.FromDate,
                    loadInput.ToDate,
                    loadInput.DriverSearch,
                    Locations = string.IsNullOrWhiteSpace(loadInput.LocationIds) ? string.Empty : loadInput.LocationIds,
                    Priorities = string.Join(",", loadInput.Priorities),
                    Suppliers = string.IsNullOrEmpty(loadInput.SupplierCompanyIds) ? string.Empty : loadInput.SupplierCompanyIds,
                    Carriers = string.IsNullOrEmpty(loadInput.CarrierCompanyIds) ? string.Empty : loadInput.CarrierCompanyIds,
                    loadInput.IsRequestFromDashboard,
                    loadInput.CountryId,
                    ShowCarrierManaged = loadInput.IsShowCarrierManaged,
                    InventoryDataCaptureTypeIds = loadInput.InventoryCaptureType,
                };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetBuyerOnGoingLoads", inputmodel);
                response = await Context.DataContext.Database.SqlQuery<UspGetDispatcherDriverLocation>(input.Query, input.Params.ToArray()).ToListAsync();
                if (response != null && response.Count > 0)
                {
                    foreach (var item in response)
                    {
                        if (item.AppLastUpdatedDate != null)
                        {
                            DateTime ut = DateTime.SpecifyKind(Convert.ToDateTime(item.AppLastUpdatedDate), DateTimeKind.Utc);
                            DateTimeOffset date = new DateTimeOffset(ut);
                            date = date.ToTargetDateTimeOffset(item.TimeZoneName).DateTime;
                            DateTimeOffset currentDateTime = DateTimeOffset.Now.ToTargetDateTimeOffset(item.TimeZoneName).DateTime;
                            TimeSpan span = currentDateTime.Subtract(date);
                            if (span.TotalMinutes <= ApplicationConstants.DriverOnlineTimeInMinutes)
                                item.IsOnline = true;
                            item.AppLastUpdatedDate = date.ToString(Resource.constFormatDateTime);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetOnGoingLoadsForMapAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<UspWhereIsMyDriverBuyerApp>> GetOnGoingLoadsForBuyerAppAsync(WhereIsMyDriverBuyerAppInputModel loadInput)
        {
            var response = new List<UspWhereIsMyDriverBuyerApp>();
            try
            {
                DateTimeOffset FromDate = DateTimeOffset.Now.AddMinutes(loadInput.UserTimeOffset).Date;
                DateTimeOffset ToDate = DateTimeOffset.Now.AddMinutes(loadInput.UserTimeOffset).Date.AddDays(1);
                if (loadInput.ScheduleDate > 0)
                {
                    FromDate = DateTimeOffset.FromUnixTimeMilliseconds(loadInput.ScheduleDate).AddMinutes(loadInput.UserTimeOffset).Date;
                }

                if (loadInput.ScheduleDate > 0)
                {
                    ToDate = DateTimeOffset.FromUnixTimeMilliseconds(loadInput.ScheduleDate).AddMinutes(loadInput.UserTimeOffset).Date.AddDays(1);
                }

                var inputmodel = new
                {
                    CompanyId = loadInput.CompanyId,
                    FromDate,
                    ToDate,
                    Cities = string.Join(",", loadInput.Cities),
                    States = string.Join(",", loadInput.States),
                    Locations = string.Join(",", loadInput.LocationIds),
                    Suppliers = string.Join(",", loadInput.CarrierCompanyIds),
                    Carriers = string.Join(",", loadInput.SupplierCompanyIds)
                };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetOnGoingLoadsForBuyerApp", inputmodel);
                response = await Context.DataContext.Database.SqlQuery<UspWhereIsMyDriverBuyerApp>(input.Query, input.Params.ToArray()).ToListAsync();
                if (response != null && response.Count > 0)
                {
                    foreach (var item in response)
                    {
                        if (item.AppLastUpdatedDate != null)
                        {
                            DateTime ut = DateTime.SpecifyKind(Convert.ToDateTime(item.AppLastUpdatedDate), DateTimeKind.Utc);
                            DateTimeOffset date = new DateTimeOffset(ut);
                            date = date.ToTargetDateTimeOffset(item.TimeZoneName).DateTime;
                            DateTimeOffset currentDateTime = DateTimeOffset.Now.ToTargetDateTimeOffset(item.TimeZoneName).DateTime;
                            TimeSpan span = currentDateTime.Subtract(date);
                            if (span.TotalMinutes <= ApplicationConstants.DriverOnlineTimeInMinutes)
                                item.IsOnline = true;
                            item.AppLastUpdatedDate = date.ToString(Resource.constFormatDateTime);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetOnGoingLoadsForBuyerAppAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<UspGetDispatcherDriverLocation>> GetBuyerLoadsAsync(int companyId, BuyerWhereIsMyDriverInputModel loadInput, DataTableSearchModel requestModel)
        {
            var response = new List<UspGetDispatcherDriverLocation>();
            try
            {
                loadInput.ToDate = loadInput.ToDate.AddDays(1);
                loadInput.DriverSearch = loadInput.DriverSearch.Replace("-", string.Empty);
                var inputmodel = new
                {
                    CompanyId = companyId,
                    States = string.Join(",", loadInput.States),
                    loadInput.FromDate,
                    loadInput.ToDate,
                    loadInput.DriverSearch,
                    Locations = string.IsNullOrWhiteSpace(loadInput.LocationIds) ? string.Empty : loadInput.LocationIds,
                    Priority = (int)loadInput.Priority,
                    Priorities = string.Join(",", loadInput.Priorities),
                    Suppliers = string.IsNullOrWhiteSpace(loadInput.SupplierCompanyIds) ? string.Empty : loadInput.SupplierCompanyIds,
                    Carriers = string.IsNullOrEmpty(loadInput.CarrierCompanyIds) ? string.Empty : loadInput.CarrierCompanyIds,
                    ShowCarrierManaged = loadInput.IsShowCarrierManaged,
                    InventoryDataCaptureTypeIds = loadInput.InventoryCaptureType,
                    requestModel
                };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetBuyerWallyBoardLoads", inputmodel);
                response = await Context.DataContext.Database.SqlQuery<UspGetDispatcherDriverLocation>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetBuyerLoadsAsync", ex.Message, ex);
            }
            return response;
        }
        #region Email Notifications

        public async Task<List<UspQbSyncedReportData>> GetQbSyncedReportData(int companyId, DateTimeOffset startDate, DateTimeOffset endDate, string timeZone, int timeout = 30)
        {
            var response = new List<UspQbSyncedReportData>();
            try
            {
                var inputmodel = new { CompanyId = companyId, StartDate = startDate, EndDate = endDate, TimeZone = timeZone };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetQbSyncedReportData", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspQbSyncedReportData>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetQbSyncedReportData", ex.Message, ex);
            }
            return response;
        }

        #endregion

        #region Exception Logs
        // Exception Logs Methods
        public async Task<List<Usp_ExceptionLogsViewModel>> GetExceptionLogs(string FromDateTime, string ToDateTime, DataTableSearchModel dataTableSearchValues, int timeout = 30)
        {
            var response = new List<Usp_ExceptionLogsViewModel>();
            try
            {
                string StartDate = DateTime.Now.AddDays(ApplicationConstants.DateFilterDefaultDays).ToString();
                string EndDate = DateTime.Now.ToString();

                if (!string.IsNullOrEmpty(FromDateTime))
                {
                    StartDate = FromDateTime;
                }
                if (!string.IsNullOrEmpty(ToDateTime))
                {
                    EndDate = ToDateTime;
                }
                var inputModel = new { StartDate, EndDate, dataTableSearchValues };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetExceptionLogs", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<Usp_ExceptionLogsViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetExceptionLogs", ex.Message, ex);
            }
            return response;
        }

        public String GetParticularException(int id, int timeout = 30)
        {
            var response = "";
            try
            {
                var inputModel = new { id };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetParticularException", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = Context.DataContext.Database.SqlQuery<String>(input.Query, input.Params.ToArray()).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetParticularException", ex.Message, ex);
            }
            return response;
        }
        #endregion

        #region Stored Procedure Testing
        public class TestSPResult
        {
            public string TypeOfPhone { get; set; }
            public int Number { get; set; }
            public DateTime TodaysDate { get; set; }
        }

        public class TestSPParam
        {
            public int PhoneType { get; set; }
            public string PhoneNumber { get; set; }
        }

        public TestSPResult GetSPResult()
        {
            var a1 = SqlHelperMethods.GetStoredProcedure<TestSPParam>("TestStoredProc", new TestSPParam { PhoneType = 1, PhoneNumber = "9833450509" });
            return Context.DataContext.Database.SqlQuery<TestSPResult>(a1.Query, a1.Params.ToArray()).SingleOrDefault();
        }

        #endregion

        #region Offers

        public async Task<List<PricingTableGridViewModel>> GetSupplierPricingTableGrid(UspSupplierOfferGridRequestViewModel uspSupplierOfferGridRequest, int timeout = 30)
        {
            var response = new List<PricingTableGridViewModel>();
            try
            {
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetSupplierPricingTable", uspSupplierOfferGridRequest);
                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<PricingTableGridViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetSupplierPricingTableGrid", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<UspBuyerOfferGridViewModel>> GetBuyerOfferGrid(UspBuyerOfferGridRequestViewModel model, int timeout = 30)
        {
            var response = new List<UspBuyerOfferGridViewModel>();
            try
            {
                var inputmodel = new { model.CompanyId, model.JobId, model.OfferType, model.States, model.Cities, model.ZipCodes, model.FuelTypes, model.Quantity, model.Latitude, model.Longitude, model.CountryId, model.CurrencyType, model.PricingTypeId, model.LowPrice, model.AvgPrice, model.HighPrice, model.dataTableSearchValues };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetBuyerOffers", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspBuyerOfferGridViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetBuyerOfferGrid", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<UspBuyerOfferGridViewModel>> GetBuyerFtlOfferGrid(UspBuyerFtlOfferGridRequestViewModel model, int timeout = 30)
        {
            var response = new List<UspBuyerOfferGridViewModel>();
            try
            {
                string spName = model.PricingSource == (int)PricingSource.OPIS ? "usp_GetOpisBuyerOffers" : "usp_GetPlattsBuyerOffers";
                var inputmodel = new { model.CompanyId, model.JobId, model.OfferType, model.States, model.Cities, model.ZipCodes, model.FuelTypes, model.Quantity, model.Latitude, model.Longitude, model.CountryId, model.CurrencyType, model.PricingTypeId, model.TruckLoadType, model.PricingSource, model.CityGroupTerminalId, model.TerminalId, model.LowPrice, model.AvgPrice, model.HighPrice, model.dataTableSearchValues };
                var input = SqlHelperMethods.GetStoredProcedure(spName, inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspBuyerOfferGridViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetBuyerFtlOfferGrid", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<Usp_DeliveryStatisticsViewModel>> GetDeliveryStatisticsGrid(int buyerCompanyId, int selectedJobId, string groupIds, int timeout = 30)
        {
            var response = new List<Usp_DeliveryStatisticsViewModel>();
            try
            {
                var inputmodel = new { BuyerCompanyId = buyerCompanyId, JobId = selectedJobId, CompanyGroupIds = groupIds };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetDeliveryStatisticsSummary", inputmodel);
                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<Usp_DeliveryStatisticsViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetDeliveryStatisticsGrid", ex.Message, ex);
            }
            return response;
        }
        #endregion

        #region WebNotifications
        //public async Task<List<UspWebNotificationViewModel>> GetWebNotificationsForUser(int userId, int timeout = 30)
        //{
        //    var response = new List<UspWebNotificationViewModel>();
        //    try
        //    {
        //        var inputModel = new { UserId = userId };
        //        var input = SqlHelperMethods.GetStoredProcedure("usp_GetWebNotifications", inputModel);
        //        Context.DataContext.Database.CommandTimeout = timeout;
        //        response = await Context.DataContext.Database.SqlQuery<UspWebNotificationViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        LogManager.Logger.WriteException("StoredProcedureDomain", "GetWebNotificationsForUser", ex.Message, ex);
        //    }
        //    return response;
        //}
        #endregion

        #region BillingStatement
        public async Task<List<BillingScheduleGridViewModel>> GetBillingScheduleGridAsync(int companyId, BillingDataTableViewModel filter = null, int timeout = 30)
        {
            var response = new List<BillingScheduleGridViewModel>();
            try
            {
                DataTableSearchModel dataTableSearchValues = new DataTableSearchModel(filter);

                object inputModel = new { CompanyId = companyId, DataTableSearchValues = dataTableSearchValues };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetSupplierBillingSchedules", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<BillingScheduleGridViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetBillingScheduleGridAsync", ex.Message, ex);
            }

            return response;
        }
        #endregion

        #region LocationSummarySupplierSide    
        public async Task<List<UspJobViewModel>> GetCustomersAndJobDetailsForSupplier(int supplierCompanyId, int userId, int timeout = 100)
        {
            var response = new List<UspJobViewModel>();
            try
            {
                var inputModel = new { @SupplierCompanyId = supplierCompanyId, @UserId = userId };
                var input = SqlHelperMethods.GetStoredProcedure("Usp_GetCustomersAndJobDetailsForSupplier", inputModel);
                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspJobViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetCustomersAndJobDetailsForSupplier", ex.Message, ex);
            }
            return response;
        }
        #endregion

        /// <summary>
        /// Gets the supplier product mapping grid async.
        /// </summary>
        /// <param name="companyId">The company id.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns><see cref="List{MappedSupplierProductViewModel}"/>.</returns>
        public async Task<List<MappedSupplierProductViewModel>> GetSupplierProductMappingGridAsync(int companyId, int timeout = 30)
        {
            var response = new List<MappedSupplierProductViewModel>();
            try
            {
                var inputModel = new { companyId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetSupplierMappedProducts", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<MappedSupplierProductViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetSupplierProductMappingGridAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task InsertBrokerSchedules(List<int> trackableScheduleIds, int timeout = 30)
        {
            string trackableSchedule = string.Join(",", trackableScheduleIds);
            try
            {
                var inputModel = new { @TrackableScheduleIds = trackableSchedule };
                var input = SqlHelperMethods.GetStoredProcedure("usp_InsertBrokerSchedules", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                await Context.DataContext.Database.SqlQuery<MappedSupplierProductViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "InsertBrokerSchedules", ex.Message + " input: " + trackableSchedule, ex);
            }
        }

        public async Task<List<TerminalMappingGridViewModel>> GetTerminalMappingGridAsync(int companyId, int SelectedCountryId, int timeout = 30)
        {
            var response = new List<TerminalMappingGridViewModel>();
            try
            {

                string CountryId = Enum.GetName(typeof(Country), SelectedCountryId);
                var inputModel = new { companyId, CountryId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetTerminalMappingGrid", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<TerminalMappingGridViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetTerminalMappingGridAsync", ex.Message, ex);
            }

            return response;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="stateIds"></param>
        /// <param name="cityIds"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public async Task<List<DropdownDisplayItem>> GetSupplierTerminals(int companyId, string stateIds = "", string cityIds = "", int timeout = 30)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                if (stateIds == null)
                    stateIds = "";
                if (cityIds == null)
                    cityIds = "";

                var inputmodel = new { CompanyId = companyId, ServingStates = stateIds, ServingCities = cityIds };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetSupplierTerminals", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<DropdownDisplayItem>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetSupplierTerminals", ex.Message, ex);
            }
            return response;
        }


        public async Task<List<DropdownDisplayExtendedItem>> GetTerminalsForMapping(int companyId, int countryId, string stateIds = "", string cityIds = "", int timeout = 30)
        {
            var response = new List<DropdownDisplayExtendedItem>();
            try
            {
                if (stateIds == null)
                    stateIds = "";
                if (cityIds == null)
                    cityIds = "";

                var inputmodel = new { servingCountry = countryId, ServingStates = stateIds, ServingCities = cityIds, CompanyId = companyId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetTerminalsForMapping", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<DropdownDisplayExtendedItem>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetTerminalsForMapping", ex.Message, ex);
            }
            return response;
        }



        /// <summary>
        /// Gets the supplier fuel types.
        /// </summary>
        /// <param name="companyId">The company id.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns><see cref="List{DropdownDisplayItem}"/>.</returns>
        public async Task<List<DropdownDisplayItem>> GetSupplierFuelTypes(int companyId, int timeout = 30)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var inputmodel = new { CompanyId = companyId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetSupplierFuelTypes", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<DropdownDisplayItem>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetSupplierFuelTypes", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetSupplierFuelTypesForOpenOrder(int companyId, int timeout = 30)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var inputmodel = new { CompanyId = companyId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetSupplierFuelTypesForOpenOrders", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<DropdownDisplayItem>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetSupplierFuelTypesForOpenOrder", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DriverLocationViewModel>> GetDistpatchDriverLocationsAsync(List<int> driverIds, int enrouteStatusId, int timeout = 30)
        {
            var response = new List<DriverLocationViewModel>();
            try
            {
                var inputmodel = new { DriverIds = string.Join(",", driverIds), EnrouteStatusId = enrouteStatusId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetDispatchDriverLocations", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<DriverLocationViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetDistpatchDriverLocationsAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<ProductMappingGridViewModel>> GetProductMappingDetailsAsync(int companyId, string stateIds = "", string cityIds = "", string terminalIds = "", string fuelTypes = "", int countryId = (int)Country.USA, int timeout = 30)
        {
            var response = new List<ProductMappingGridViewModel>();
            try
            {
                var inputModel = new { CompanyId = companyId, StateIds = stateIds ?? "", CityIds = cityIds ?? "", TerminalIds = terminalIds ?? "", FuelTypeIds = fuelTypes ?? "", CountryId = countryId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetProductMappingDetails", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<ProductMappingGridViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetProductMappingDetailsAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<UspCarrierMapping>> GetCarrierMapping(int companyId, int countryId, int timeout = 30)
        {
            var response = new List<UspCarrierMapping>();
            try
            {
                var inputModel = new { companyId, countryId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_getCarrierTerminalBulkPlantMapping", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspCarrierMapping>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetCarrierMapping", ex.Message + " -CompanyId: " + companyId, ex);
            }

            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetCustomersBySupplierOrCarrier(int orderAcceptedCompanyId)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                object inputModel = new { orderAcceptedCompanyId };
                var input = SqlHelperMethods.GetStoredProcedure("Usp_GetCustomersBySupplierAndCarrier", inputModel);
                Context.DataContext.Database.CommandTimeout = 30;
                response = await Context.DataContext.Database.SqlQuery<DropdownDisplayItem>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetCustomers", ex.Message, ex);
            }

            return response;
        }
        public async Task<List<DropdownDisplayItem>> GetLocationByCustomerId(int companyId, int customerId, bool isRetailJob)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                object inputModel = new { companyId, customerId, isRetailJob };
                var input = SqlHelperMethods.GetStoredProcedure("Usp_GetLocationByCustomerId", inputModel);
                Context.DataContext.Database.CommandTimeout = 30;
                response = await Context.DataContext.Database.SqlQuery<DropdownDisplayItem>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetLocationByCustomerId", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetOrdersByCustomerAndLocationId(int companyId, int customerId, int locationid, int tfxProductId = 0)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                object inputModel = new { companyId, customerId, locationid, tfxProductId };
                var input = SqlHelperMethods.GetStoredProcedure("Usp_GetOrdersByCustomerIdAndLocationId", inputModel);
                Context.DataContext.Database.CommandTimeout = 30;
                response = await Context.DataContext.Database.SqlQuery<DropdownDisplayItem>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetOrdersByCustomerAndLocationId", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<JobTankDetailsViewModel>> GetTankDetailsByJobs(List<int> jobIds, int timeout = 30)
        {
            var response = new List<JobTankDetailsViewModel>();
            try
            {
                var strJobIds = string.Join(",", jobIds);
                var inputModel = new { @JobIds = strJobIds };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetTankVolumesForInventoryCheck", inputModel);
                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<JobTankDetailsViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetTankDetailsByJobs", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<JobTankDetailsViewModel>> GetTankDetailsByInvoice(int invoiceId, int timeout = 30)
        {
            var response = new List<JobTankDetailsViewModel>();
            try
            {
                var inputModel = new { @InvoiceId = invoiceId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetTankVolumesForInventoryCheckByInvoice", inputModel);
                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<JobTankDetailsViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetTankDetailsByInvoice", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<BolDataForLFV>> GetBolDataForLFV(int companyId, string bol = "", int timeout = 30)
        {
            var response = new List<BolDataForLFV>();
            try
            {   
                var inputModel = new { @CompanyId = companyId, @BolNumber = bol };
                var input = SqlHelperMethods.GetStoredProcedure("usp_LiftFileValidationRecords", inputModel);
                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<BolDataForLFV>(input.Query, input.Params.ToArray()).ToListAsync();

                if (!response.Any())
                {
                    LogManager.Logger.WriteException("LFVDomain", "SetLiftFileRecordStatus", $"NO Invoice or Exceptions found for company={companyId}", new Exception());
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetBolDataForLFV", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<UspGetBrokeredChildOrder>> GetBrokeredChildOrders(int orderId, int statusId = 0, int acceptedCompanyId = 0, int timeout = 30)
        {
            var response = new List<UspGetBrokeredChildOrder>();
            try
            {
                var inputModel = new { OrderId = orderId, StatusId = statusId, AcceptedCompanyId = acceptedCompanyId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetBrokeredChildOrders", inputModel);
                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspGetBrokeredChildOrder>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetBrokeredChildOrders", ex.Message, ex);
            }
            return response;
        }

        public List<usp_LFValidationViewModel> GetLFValidationGridData(int companyId, DateTimeOffset? startDate, DateTimeOffset? endDate, string carrierIds = "")
        {
            var response = new List<usp_LFValidationViewModel>();
            try
            {
                var timeOut = 30;
                if (startDate != null && endDate == null)
                    endDate = startDate;
                var inputModel = new { @CompanyId = companyId, @startDate = startDate, @endDate = endDate, @carrierIds = carrierIds };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetLiftFileValidationSummary", inputModel);
                Context.DataContext.Database.CommandTimeout = timeOut;
                response = Context.DataContext.Database.SqlQuery<usp_LFValidationViewModel>(input.Query, input.Params.ToArray()).ToList();
            }
            catch (Exception ex)
            {

                LogManager.Logger.WriteException("StoredProcedureDomain", "GetLFValidationGridData", ex.Message, ex);
            }
            return response;
        }

        public List<LFRecordsGridViewModel> GetLFRecordsGridData(int companyId, int recordStatus, int lfCallId, DateTimeOffset? startDate, DateTimeOffset? endDate, string carrierIds = "")
        {
            var response = new List<LFRecordsGridViewModel>();
            try
            {
                var timeOut = 30;
                var inputModel = new { @CompanyId = companyId, @RecordStatus = recordStatus, @CallId = lfCallId, @StartDate = startDate, @EndDate = endDate, @carrierIds = carrierIds };
                var input = SqlHelperMethods.GetStoredProcedure("usp_LiftFileRecordsByStatus", inputModel);
                Context.DataContext.Database.CommandTimeout = timeOut;
                var spResponse = Context.DataContext.Database.SqlQuery<LFRecordsGridViewModel>(input.Query, input.Params.ToArray()).ToList();
                var grpResult = spResponse.GroupBy(t => new { t.bol, t.TerminalName, t.LiftFileRecordId }).ToList();
                if (grpResult != null && grpResult.Any())
                {
                    foreach (var item in grpResult)
                    {
                        var lfRecord = spResponse.
                                       Where(t => t.bol == item.Key.bol && t.TerminalName == item.Key.TerminalName && t.LiftFileRecordId == item.Key.LiftFileRecordId).
                                       FirstOrDefault();
                        response.Add(lfRecord);
                    }
                }
            }
            catch (Exception ex)
            {

                LogManager.Logger.WriteException("StoredProcedureDomain", "GetLFRecordsGridData", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<NoDataExceptionDdtGridViewModel>> GetSupplierNoDataExceptionDdts(int companyId, NoDataExceptionDataTableViewModel requestModel = null, int timeout = 60)
        {
            var response = new List<NoDataExceptionDdtGridViewModel>();
            try
            {
                DataTableSearchModel dataTableSearchValues = new DataTableSearchModel(requestModel);
                DateTimeOffset StartDate = Convert.ToDateTime(ApplicationConstants.DateRangeFilterStartDate);
                DateTimeOffset EndDate = DateTimeOffset.Now.Date.AddDays(1);
                object inputModel = new { CompanyId = companyId, requestModel.OrderId, StartDate, EndDate, CountryId = requestModel.CountryId, CurrencyType = (int)requestModel.Currency, GroupIds = requestModel.GroupIds, DataTableSearchValues = dataTableSearchValues };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetSupplierNoDataExceptionDdts", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<NoDataExceptionDdtGridViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetSupplierNoDataExceptionDdts", ex.Message, ex);
            }

            return response;
        }

        //usp_GetBuyerNoDataExceptionDdts
        public async Task<List<NoDataExceptionDdtGridViewModel>> GetBuyerNoDataExceptionDdts(int companyId, NoDataExceptionDataTableViewModel requestModel = null, int timeout = 60)
        {
            var response = new List<NoDataExceptionDdtGridViewModel>();
            try
            {
                DataTableSearchModel dataTableSearchValues = new DataTableSearchModel(requestModel);
                DateTimeOffset StartDate = Convert.ToDateTime(ApplicationConstants.DateRangeFilterStartDate);
                DateTimeOffset EndDate = DateTimeOffset.Now.Date.AddDays(1);
                object inputModel = new { CompanyId = companyId, requestModel.OrderId, StartDate, EndDate, CountryId = requestModel.CountryId, CurrencyType = (int)requestModel.Currency, GroupIds = requestModel.GroupIds, DataTableSearchValues = dataTableSearchValues };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetBuyerNoDataExceptionDdts", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<NoDataExceptionDdtGridViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetBuyerNoDataExceptionDdts", ex.Message, ex);
            }

            return response;
        }

        public async Task<PedegreeConfigurationModel> GetPegedreeConfigurationInfo(string pedigreeIds)
        {
            var pedgreeConfig = new PedegreeConfigurationModel();
            try
            {
                var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString);
                var command = connection.CreateCommand();
                command.CommandText = "[dbo].[Usp_GETPedegreeConnectionInfo]";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("pedigreeIds", pedigreeIds);
                command.CommandTimeout = 30;
                connection.Open();
                var reader = await command.ExecuteReaderAsync();

                pedgreeConfig = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<PedegreeConfigurationModel>(reader).FirstOrDefault();
                reader.NextResult();

                pedgreeConfig.ProductTypeList = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<DropdownDisplayItem>(reader).ToList();
                reader.NextResult();
                pedgreeConfig.JobUOMList = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<JobUOMDropDwn>(reader).ToList();
                reader.Close();
                command.Dispose();
                connection.Close();

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetPegedreeConfigurationInfo", ex.Message, ex);
            }
            return pedgreeConfig;
        }

        public async Task<List<TerminalItemCodeMappingGridViewModel>> GetTerminalItemCodeMappings(ItemCodeMappingGridRequestViewModel requestModel, int timeout = 60)
        {
            var response = new List<TerminalItemCodeMappingGridViewModel>();
            try
            {
                DataTableSearchModel dataTableSearchValues = new DataTableSearchModel(requestModel);
                DateTimeOffset StartDate = Convert.ToDateTime(ApplicationConstants.DateRangeFilterStartDate);
                DateTimeOffset EndDate = DateTimeOffset.Now.Date.AddDays(1);
                object inputModel = new { CompanyId = requestModel.CompanyId, StartDate, EndDate, CountryId = (int)requestModel.Country, DataTableSearchValues = dataTableSearchValues };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetTerminalItemCodeMapping", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<TerminalItemCodeMappingGridViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetTerminalItemCodeMappings", ex.Message, ex);
            }

            return response;
        }

        public async Task<UspGetParentFuelRequest> GetParentFuelRequest(int fuelRequestId, int timeout = 60)
        {
            var response = new UspGetParentFuelRequest();
            try
            {
                object inputModel = new { FuelRequestId = fuelRequestId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetParentFuelRequest", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspGetParentFuelRequest>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetParentFuelRequest", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<int>> GetBrokerChainOrderListTillOriginalOrder(int orderId, List<int> divertedOrders, DateTimeOffset dropDate, int timeout = 60)
        {
            var response = new List<int>();
            try
            {
                object inputModel = new { OrderId = orderId, DivertedOrders = string.Join(",", divertedOrders), DropDate = dropDate };
                var input = SqlHelperMethods.GetStoredProcedure("usp_DryRun_GetBrokerChainOrderListTillOriginalOrder", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<int>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetBrokerChainOrderListTillOriginalOrder", ex.Message, ex);
            }
            return response;
        }

        public void InsertEBolDetails()
        {
            try
            {
                var input = SqlHelperMethods.GetStoredProcedure("usp_InsertEBolDetails", new { });
                Context.DataContext.Database.ExecuteSqlCommand(input.Query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ExternalCustomerMappingViewModel>> GetCustomersForExternalMapping(int companyId, int timeout = 30)
        {
            var response = new List<ExternalCustomerMappingViewModel>();
            try
            {
                object inputModel = new { CompanyId = companyId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetCustomersForExternalMapping", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<ExternalCustomerMappingViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetCustomersForExternalMapping", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<ExternalCustomerLocationMappingViewModel>> GetCustomerLocationsForExternalMapping(int companyId, int timeout = 30)
        {
            object inputModel = new { CompanyId = companyId };
            var response = new List<ExternalCustomerLocationMappingViewModel>();
            try
            {
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetCustomerLocationsForExternalMapping", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<ExternalCustomerLocationMappingViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetCustomerLocationsForExternalMapping", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<ExternalProductMappingViewModel>> GetProductsForExternalMapping(int companyId, int timeout = 30)
        {
            object inputModel = new { CompanyId = companyId };
            var response = new List<ExternalProductMappingViewModel>();
            try
            {
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetProductsForExternalMapping", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<ExternalProductMappingViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetProductsForExternalMapping", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<ExternalSupplierMappingViewModel>> GetSuppliersForExternalMapping(int companyId, int timeout = 30)
        {
            var response = new List<ExternalSupplierMappingViewModel>();
            try
            {
                object inputModel = new { CompanyId = companyId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetSuppliersForExternalMapping", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<ExternalSupplierMappingViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetSuppliersForExternalMapping", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<ExternalTerminalMappingViewModel>> GetTerminalsForExternalMapping(int companyId, int timeout = 30)
        {
            var response = new List<ExternalTerminalMappingViewModel>();
            try
            {
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetTerminalsForExternalMapping", new { CompanyId = companyId });

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<ExternalTerminalMappingViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetTerminalsForExternalMapping", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<ExternalBulkPlantMappingViewModel>> GetBulkPlantsForExternalMapping(int timeout = 30)
        {
            var response = new List<ExternalBulkPlantMappingViewModel>();
            try
            {
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetBulkPlantsForExternalMapping", new { });

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<ExternalBulkPlantMappingViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetBulkPlantsForExternalMapping", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<ExternalDriverMappingViewModel>> GetDriversForExternalMapping(int timeout = 30)
        {
            var response = new List<ExternalDriverMappingViewModel>();
            try
            {
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetDriversForExternalMapping", new { });

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<ExternalDriverMappingViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetDriversForExternalMapping", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<ExternalCarrierMappingViewModel>> GetCarriersForExternalMapping(int timeout = 30)
        {
            var response = new List<ExternalCarrierMappingViewModel>();
            try
            {
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetCarriersForExternalMapping", new { });

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<ExternalCarrierMappingViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetCarriersForExternalMapping", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<UspMfnGetDraftDDT>> GetMgnDraftDDTs(int timeout = 30)
        {
            var response = new List<UspMfnGetDraftDDT>();
            try
            {
                var input = SqlHelperMethods.GetStoredProcedure("usp_MFN_GetDraftDDTs", new { });

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspMfnGetDraftDDT>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetMgnDraftDDTs", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<LocationViewModel>> GetBuyerLocations(int companyId, int userId = 0, string fromDate = "", string toDate = "", int timeout = 30)
        {
            var response = new List<LocationViewModel>();
            try
            {
                if (string.IsNullOrWhiteSpace(fromDate))
                    fromDate = DateTimeOffset.Now.Date.AddDays(ApplicationConstants.DefaultDateRangeFromDate).ToString();
                if (string.IsNullOrWhiteSpace(toDate))
                    toDate = DateTimeOffset.Now.Date.ToString();

                object inputModel = new { UserId = userId, CompanyId = companyId, FromDate = fromDate, ToDate = toDate };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetBuyerLocations", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<LocationViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetBuyerLocations", ex.Message, ex);
                throw;
            }
            return response;
        }

        public async Task<List<LocationViewModel>> GetCustomerLocations(int companyId, int userId = 0, string fromDate = "", string toDate = "", int timeout = 30)
        {
            var response = new List<LocationViewModel>();
            try
            {
                if (string.IsNullOrWhiteSpace(fromDate))
                    fromDate = DateTimeOffset.Now.Date.AddDays(ApplicationConstants.DefaultDateRangeFromDate).ToString();
                if (string.IsNullOrWhiteSpace(toDate))
                    toDate = DateTimeOffset.Now.Date.ToString();

                object inputModel = new { UserId = userId, CompanyId = companyId, FromDate = fromDate, ToDate = toDate };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetCustomerLocationsForSupplier", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<LocationViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetCustomerLocations", ex.Message, ex);
                throw;
            }
            return response;
        }

        public async Task<List<AssetModel>> GetAssets(int companyId, int userId = 0, string fromDate = "", string toDate = "", AssetType type = AssetType.Asset, int timeout = 30)
        {
            var response = new List<AssetModel>();
            try
            {
                object inputModel = new { Type = (int)type, UserId = userId, CompanyId = companyId, FromDate = fromDate, ToDate = toDate };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetAssets", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<AssetModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetAssets", ex.Message, ex);
                throw;
            }
            return response;
        }

        public async Task<List<AssetModel>> GetCustomerAssets(int companyId, int userId = 0, string fromDate = "", string toDate = "", AssetType type = AssetType.Asset, int timeout = 30)
        {
            var response = new List<AssetModel>();
            try
            {
                object inputModel = new { Type = (int)type, CompanyId = companyId, FromDate = fromDate, ToDate = toDate };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetCustomerAssetsForSupplier", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<AssetModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetCustomerAssets", ex.Message, ex);
                throw;
            }
            return response;
        }

        public async Task<List<TankModel>> GetCustomerTanks(int companyId, int userId = 0, string fromDate = "", string toDate = "", AssetType type = AssetType.Tank, int timeout = 30)
        {
            var response = new List<TankModel>();
            try
            {
                object inputModel = new { Type = (int)type, CompanyId = companyId, FromDate = fromDate, ToDate = toDate };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetCustomerAssetsForSupplier", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<TankModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetCustomerTanks", ex.Message, ex);
                throw;
            }
            return response;
        }

        public async Task<List<TankModel>> GetTanks(int companyId, int userId = 0, string fromDate = "", string toDate = "", AssetType type = AssetType.Tank, int timeout = 30)
        {
            var response = new List<TankModel>();
            try
            {
                object inputModel = new { Type = (int)type, UserId = userId, CompanyId = companyId, FromDate = fromDate, ToDate = toDate };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetAssets", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<TankModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetTanks", ex.Message, ex);
                throw;
            }
            return response;
        }

        public async Task<List<FuelRequestModel>> GetFuelRequestsAsBuyer(int companyId, int userId = 0, string fromDate = "", string toDate = "", FuelRequestType type = FuelRequestType.All, int timeout = 30)
        {
            var response = new List<FuelRequestModel>();
            try
            {
                object inputModel = new { CompanyId = companyId, FromDate = fromDate, ToDate = toDate, Type = (int)type, UserId = userId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetBuyerFuelRequestsForAPI", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<FuelRequestModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetFuelRequestsAsBuyer", ex.Message, ex);
                throw;
            }
            return response;
        }

        public async Task<List<FuelRequestModel>> GetFuelRequestsAsSupplier(int companyId, int userId = 0, string fromDate = "", string toDate = "", FuelRequestType type = FuelRequestType.All, int timeout = 30)
        {
            var response = new List<FuelRequestModel>();
            try
            {
                object inputModel = new { CompanyId = companyId, FromDate = fromDate, ToDate = toDate, Type = (int)type, UserId = userId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetSupplierFuelRequestsForAPI", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<FuelRequestModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetFuelRequestsAsSupplier", ex.Message, ex);
                throw;
            }
            return response;
        }

        public List<LFRecordsGridViewModel> GetLFRecordsByBolFilenameGridData(string bol, string fileName, int companyId)
        {
            var response = new List<LFRecordsGridViewModel>();
            try
            {
                var timeOut = 30;
                var inputModel = new { @CompanyId = companyId, @Bol = bol, @FileName = fileName };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetLFrecordsByBolFilename", inputModel);
                Context.DataContext.Database.CommandTimeout = timeOut;
                response = Context.DataContext.Database.SqlQuery<LFRecordsGridViewModel>(input.Query, input.Params.ToArray()).ToList();

            }
            catch (Exception ex)
            {

                LogManager.Logger.WriteException("StoredProcedureDomain", "GetLFRecordsByBolFilenameGridData", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<BuyerOrderModel>> GetOrdersAsBuyer(int companyId, int userId = 0, string fromDate = "", string toDate = "", int timeout = 30)
        {
            var response = new List<BuyerOrderModel>();
            try
            {
                object inputModel = new { UserId = userId, CompanyId = companyId, FromDate = fromDate, ToDate = toDate };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetBuyerOrdersForAPI", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<BuyerOrderModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetOrdersAsBuyer", ex.Message, ex);
                throw;
            }
            return response;
        }

        public async Task<List<SupplierOrderModel>> GetOrdersAsSupplier(int companyId, int userId = 0, string fromDate = "", string toDate = "", int timeout = 30)
        {
            var response = new List<SupplierOrderModel>();
            try
            {
                object inputModel = new { CompanyId = companyId, FromDate = fromDate, ToDate = toDate, UserId = userId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetSupplierOrdersForAPI", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<SupplierOrderModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetOrdersAsSupplier", ex.Message, ex);
                throw;
            }
            return response;
        }

        public async Task<List<LFRecordsGridViewModel>> GetLfRecordsByDateTimeWindow(int companyId, DateTime startDate, DateTime endDate)
        {
            var response = new List<LFRecordsGridViewModel>();
            try
            {
                object inputModel = new { CompanyId = companyId, StartDate = startDate, EndDate = endDate };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetLiftFileRecordsByDateTimeWindow", inputModel);
                Context.DataContext.Database.CommandTimeout = 60;
                response = await Context.DataContext.Database.SqlQuery<LFRecordsGridViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetLfRecordsByDateTimeWindow", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<UspDdtApprovalListViewModel>> GetDdtApprovalListAsync(int companyId, int userId, bool isBuyerAdmin, int brandedCompanyId = 0, int timeout = 30)
        {
            var response = new List<UspDdtApprovalListViewModel>();
            try
            {
                DateTimeOffset StartDate = Convert.ToDateTime(ApplicationConstants.DateRangeFilterStartDate);
                DateTimeOffset EndDate = DateTimeOffset.Now.Date.AddDays(1);
                var InvoiceTypeIdFilter = string.Join(",", (int)InvoiceType.DigitalDropTicketManual, (int)InvoiceType.DigitalDropTicketMobileApp);
                object inputModel = new { @CompanyId = companyId, @BrandedCompanyId = brandedCompanyId, @UserId = userId, @IsBuyerAdmin = isBuyerAdmin, @InvoiceTypeId = 0, @StartDate = StartDate, @EndDate = EndDate, @CurrencyType = (int)Currency.None, @CountryId = (int)Country.All, @GroupIds = "", @JobId = 0, @InvoiceTypeIdFilter = InvoiceTypeIdFilter };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetDdtApprovalListForBuyerApp", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = Context.DataContext.Database.SqlQuery<UspDdtApprovalListViewModel>(input.Query, input.Params.ToArray()).ToListAsync().Result;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetDdtApprovalListAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<JobXCustomerBrandId>> GetCustomerBrandIdsByJobs(List<int> JobIds, int SupplierCompanyId)
        {
            var response = new List<JobXCustomerBrandId>();
            try
            {
                var jobIds = string.Join(",", JobIds);
                object inputModel = new { @JobIds = jobIds, @SupplierCompanyId = SupplierCompanyId };

                var input = SqlHelperMethods.GetStoredProcedure("usp_GetCustomerIdByJobs", inputModel);

                Context.DataContext.Database.CommandTimeout = 30;
                response = await Context.DataContext.Database.SqlQuery<JobXCustomerBrandId>(input.Query, input.Params.ToArray()).ToListAsync();

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetCustomerBrandIdsByJobs", ex.Message, ex);

            }
            return response;

        }

        public async Task<int> UpdateLiftFileForDuplicateRecords(int companyId, int timeout = 30)
        {
            int response = 0;
            try
            {
                var inputModel = new { CompanyId = companyId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_UpdateLiftFileForDuplicateRecords", inputModel);
                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<int>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "UpdateLiftFileForDuplicateRecords", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<TankInventoryDataCaptureResponseModel>> GetTankInventoryDetails(int companyId, int timeout = 30)
        {
            var response = new List<TankInventoryDataCaptureResponseModel>();
            try
            {
                var inputModel = new { CompanyId = companyId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetSalesDataOfTankInventory", inputModel);
                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<TankInventoryDataCaptureResponseModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetTankInventoryDetails", ex.Message, ex);
            }
            return response;
        }

        #region Buyer Dashboard New
        public async Task<List<JobBuyerDashboardViewModel>> GetJobDetailsForBuyerDashboard(int companyId, int countryId, int timeout = 30)
        {
            var response = new List<JobBuyerDashboardViewModel>();
            try
            {
                object inputmodel = new { @CompanyID = companyId, @CountryId = countryId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetJobDetailsForBuyerDashboard", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<JobBuyerDashboardViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetJobDetailsForBuyerDashboard", ex.Message, ex);
            }

            return response;
        }
        public async Task<List<UspBuyerLoadsForDashboard>> GetBuyerLoadsForDashboard(int companyId, BuyerLoadsForDashboardInputModel loadInput, int timeout = 30)
        {
            var response = new List<UspBuyerLoadsForDashboard>();
            try
            {
                loadInput.ToDate = loadInput.ToDate.AddDays(1);
                var inputmodel = new
                {
                    CompanyId = companyId,
                    CountryId = (int)loadInput.CountryId,
                    loadInput.FromDate,
                    loadInput.ToDate
                };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetBuyerLoadsForDashboard", inputmodel);
                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspBuyerLoadsForDashboard>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetBuyerLoadsForDashboard", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<InvoiceGridBuyerDashboardModel>> GetInvoiceGridForBuyerDashboardAsync(InvoiceGridBuyerDashboardInputModel requestModel, int timeout = 30)
        {
            var response = new List<InvoiceGridBuyerDashboardModel>();
            try
            {
                object inputModel = new
                {
                    requestModel.CompanyId,
                    requestModel.BrandedCompanyId,
                    requestModel.UserId,
                    requestModel.IsBuyerAdmin,
                    requestModel.InvoiceTypeId,
                    CurrencyTypeId = (int)requestModel.CurrencyTypeId,
                    requestModel.CountryId,
                    requestModel.GroupIds
                };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetBuyerInvoicesForDashboard", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<InvoiceGridBuyerDashboardModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetInvoiceGridForBuyerDashboardAsync", ex.Message, ex);
            }

            return response;
        }
        #endregion

        public async Task<SkybitzConfig> GetSkybitzConfiguration(int timeout = 30)
        {
            var response = new SkybitzConfig();
            try
            {
                var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString);
                var command = connection.CreateCommand();
                command.CommandText = "[dbo].[Usp_GetJobTimeZoneForSkybitz]";
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = timeout;
                connection.Open();
                var reader = await command.ExecuteReaderAsync();

                response.DropdownDisplayExtendedItems = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<DropdownDisplayExtendedItem>(reader).ToList();
                reader.NextResult();
                response.IsApi = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<int>(reader).FirstOrDefault();
                reader.NextResult();
                string skybitzConfigJson = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<string>(reader).FirstOrDefault();
                reader.Close();
                command.Dispose();
                connection.Close();
                if (response.DropdownDisplayExtendedItems != null && response.DropdownDisplayExtendedItems.Any())
                {
                    response.DropdownDisplayExtendedItems.FirstOrDefault().Code = skybitzConfigJson;
                }
                return response;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetSkybitzConfiguration", ex.Message, ex);
            }
            return response;
        }

        public async Task<ExternalTankConfigurationModel> GetIS360Configuration(int timeout = 30)
        {
            var response = new ExternalTankConfigurationModel();
            try
            {
                var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString);
                var command = connection.CreateCommand();
                command.CommandText = "[dbo].[Usp_GETIs360ConnectionInfo]";
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = timeout;
                connection.Open();
                var reader = await command.ExecuteReaderAsync();
                response.ConnectionInfo = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<string>(reader).FirstOrDefault();
                reader.NextResult();
                response.ProductTypeList = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<DropdownDisplayItem>(reader).ToList();
                reader.NextResult();
                response.JobUOMTimeZoneList = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<JobUOMDropDwn>(reader).ToList();
                reader.Close();
                command.Dispose();
                connection.Close();
                return response;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetIS360Configuration", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<RaiseDrJobInfo>> GetCreateDRLocationInfo(int companyId, List<long> jobIdList, List<String> jobSiteIdList, int timeout = 30)
        {
            var response = new List<RaiseDrJobInfo>();
            try
            {
                DataTable tblJobIds, tblDisplayJobIds;
                InitializeTableLongParameters(jobIdList, out tblJobIds);
                InitializeTableStringParameters(jobSiteIdList, out tblDisplayJobIds);

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    try
                    {
                        using (SqlCommand command = connection.CreateCommand())
                        {
                            command.CommandText = "[dbo].[Usp_GetLocationInfoToCreateDR]";
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandTimeout = timeout;
                            command.Parameters.Add(new SqlParameter("CompanyId", companyId));
                            command.Parameters.Add(new SqlParameter("LocationIds", tblJobIds));
                            command.Parameters.Add(new SqlParameter("@SiteIds", tblDisplayJobIds));
                            connection.Open();
                            SqlDataReader reader = await command.ExecuteReaderAsync();
                            response = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<RaiseDrJobInfo>(reader).ToList();
                            reader.Close();
                        }
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        connection.Close();
                        LogManager.Logger.WriteException("StoredProcedureDomain", "GetCreateDRLocationInfo", ex.Message, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetCreateDRLocationInfo", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<OrderPickupDetailModel>> GetOrderDetailsForEditDeliveryGroup(int jobId, int productTypeId, DateTimeOffset? loadDate, int companyId, bool isBlendReq, List<int> favFuels, int timeout = 30)
        {
            var response = new List<OrderPickupDetailModel>();
            try
            {
                DataTable tblFavFuelTypeIds;
                InitializeTableIntParameters(favFuels, out tblFavFuelTypeIds);
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    try
                    {
                        using (SqlCommand command = connection.CreateCommand())
                        {
                            command.CommandText = "[dbo].[usp_GetOrderDetailsForEditDeliveryGroup]";
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandTimeout = timeout;
                            command.Parameters.Add(new SqlParameter("ProductTypeId", productTypeId));
                            command.Parameters.Add(new SqlParameter("SupplierCompanyId", companyId));
                            command.Parameters.Add(new SqlParameter("JobId", jobId));
                            command.Parameters.Add(new SqlParameter("LoadDate", loadDate.HasValue ? loadDate : (object)DBNull.Value));
                            command.Parameters.Add(new SqlParameter("IsBlendReq", isBlendReq));
                            command.Parameters.Add(new SqlParameter("FavFuelTypeIds", tblFavFuelTypeIds));
                            connection.Open();
                            SqlDataReader reader = await command.ExecuteReaderAsync();
                            response = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<OrderPickupDetailModel>(reader).ToList();
                            reader.Close();
                        }
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        connection.Close();
                        LogManager.Logger.WriteException("StoredProcedureDomain", "GetOrderDetailsForEditDeliveryGroup", ex.Message, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetOrderDetailsForEditDeliveryGroup", ex.Message, ex);
            }
            return response;
        }

        public async Task<RaiseDrOrderInfoOfBuyerAndSupplier> GetOrdersForJobOfCustomerAndSupplier(int companyId, int buyerCompanyId, bool isEndSupplier, bool skipMarineConversion, List<int> jobIds, List<int> productIdsToExclude, List<int> favFuelTypeIds, List<int> favProductTypeIds, int timeout = 30)
        {
            var response = new RaiseDrOrderInfoOfBuyerAndSupplier();
            try
            {
                DataTable tblProductIdsToExclude, tblFavFuelTypeIds, tblFavProductTypeIds, tblJobIds;
                InitializeTableIntParameters(productIdsToExclude, out tblProductIdsToExclude);
                InitializeTableIntParameters(favFuelTypeIds, out tblFavFuelTypeIds);
                InitializeTableIntParameters(favProductTypeIds, out tblFavProductTypeIds);
                InitializeTableIntParameters(jobIds, out tblJobIds);
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    try
                    {
                        using (SqlCommand command = connection.CreateCommand())
                        {
                            command.CommandText = "[dbo].[Usp_GetOrdersForJobOfCustomerAndSupplier]";
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandTimeout = timeout;
                            command.Parameters.Add(new SqlParameter("BuyerCompanyId", buyerCompanyId));
                            command.Parameters.Add(new SqlParameter("IsEndSupplier", isEndSupplier));
                            command.Parameters.Add(new SqlParameter("SupplierCompanyId", companyId));
                            command.Parameters.Add(new SqlParameter("SkipMarineConversion", skipMarineConversion));
                            command.Parameters.Add(new SqlParameter("JobIds", tblJobIds));
                            command.Parameters.Add(new SqlParameter("ProductsToExclude", tblProductIdsToExclude));
                            command.Parameters.Add(new SqlParameter("FavFuelTypeIds", tblFavFuelTypeIds));
                            command.Parameters.Add(new SqlParameter("FavProductTypeIds", tblFavProductTypeIds));
                            connection.Open();
                            SqlDataReader reader = await command.ExecuteReaderAsync();
                            response.OrderPickupDetails = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<OrderPickupDetailModel>(reader).ToList();
                            reader.NextResult();
                            response.DeliveryReqInput = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<DemandModel>(reader).ToList();
                            reader.NextResult();
                            response.OrderList = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<DropdownDisplayItem>(reader).ToList();
                            reader.NextResult();
                            reader.Close();
                        }
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        connection.Close();
                        LogManager.Logger.WriteException("StoredProcedureDomain", "GetOrdersForJobOfCustomerAndSupplier", ex.Message, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetOrdersForJobOfCustomerAndSupplier", ex.Message, ex);
            }
            return response;
        }


        public async Task<SubmitDRInput> GetDataToCreateDR(List<int> orderIds, List<int> jobIds, List<string> tankIds, List<string> storageIds, int timeout = 30)
        {
            var response = new SubmitDRInput();
            try
            {
                DataTable tblOrderIds, tblTankIds, tblStorageIds, tblJobIds;
                InitializeTableIntParameters(orderIds, out tblOrderIds);
                InitializeTableStringParameters(tankIds, out tblTankIds);
                InitializeTableStringParameters(storageIds, out tblStorageIds);
                InitializeTableIntParameters(jobIds, out tblJobIds);
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    try
                    {
                        using (SqlCommand command = connection.CreateCommand())
                        {
                            command.CommandText = "[dbo].[Usp_GetDataToCreateDR]";
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandTimeout = timeout;
                            command.Parameters.Add(new SqlParameter("LocationIds", tblJobIds));
                            command.Parameters.Add(new SqlParameter("OrderIds", tblOrderIds));
                            command.Parameters.Add(new SqlParameter("TankIds", tblTankIds));
                            command.Parameters.Add(new SqlParameter("StorageIds", tblStorageIds));
                            connection.Open();
                            SqlDataReader reader = await command.ExecuteReaderAsync();
                            response.Jobs = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<SubmitDrJobInput>(reader).ToList();
                            reader.NextResult();
                            response.Orders = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<SubmitDrOrderInput>(reader).ToList();
                            reader.NextResult();
                            response.Assets = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<SubmitDrJobAssetInput>(reader).ToList();
                            reader.NextResult();
                            response.Vessels = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<SubmitDrVesselInput>(reader).ToList();
                            reader.NextResult();
                            reader.Close();
                        }
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        connection.Close();
                        LogManager.Logger.WriteException("StoredProcedureDomain", "GetDataToCreateDR", ex.Message, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetDataToCreateDR", ex.Message, ex);
            }
            return response;
        }



        public async Task<RaiseDrProductAndOrderInfo> GetProductAndOrderInfoToCreateDR(int companyId, DateTimeOffset? loadDate, List<long> jobIds, List<int> productTypeIds, List<int> favFuelTypeIds, List<int> favProductTypeIds, int timeout = 30)
        {
            var response = new RaiseDrProductAndOrderInfo();
            try
            {
                DataTable tblProductTypeIds, tblFavFuelTypeIds, tblFavProductTypeIds, tblJobIds;
                InitializeTableIntParameters(productTypeIds, out tblProductTypeIds);
                InitializeTableIntParameters(favFuelTypeIds, out tblFavFuelTypeIds);
                InitializeTableIntParameters(favProductTypeIds, out tblFavProductTypeIds);
                InitializeTableLongParameters(jobIds, out tblJobIds);
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    try
                    {
                        using (SqlCommand command = connection.CreateCommand())
                        {
                            command.CommandText = "[dbo].[Usp_GetProductAndOrderInfoToCreateDR]";
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandTimeout = timeout;
                            command.Parameters.Add(new SqlParameter("CompanyId", companyId));
                            command.Parameters.Add(new SqlParameter("LoadDate", loadDate));
                            command.Parameters.Add(new SqlParameter("LocationIds", tblJobIds));
                            command.Parameters.Add(new SqlParameter("ProductTypeIds", tblProductTypeIds));
                            command.Parameters.Add(new SqlParameter("FavFuelTypeIds", tblFavFuelTypeIds));
                            command.Parameters.Add(new SqlParameter("FavProductTypeIds", tblFavProductTypeIds));
                            connection.Open();
                            SqlDataReader reader = await command.ExecuteReaderAsync();
                            response.ProductMappings = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<ProductMappingModel>(reader).ToList();
                            reader.NextResult();
                            response.BlendProductMappings = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<ProductMappingModel>(reader).ToList();
                            reader.NextResult();
                            response.Orders = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<OrderPickupDetailModel>(reader).ToList();
                            reader.NextResult();
                            reader.Close();
                        }
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        connection.Close();
                        LogManager.Logger.WriteException("StoredProcedureDomain", "GetProductAndOrderInfoToCreateDR", ex.Message, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetProductAndOrderInfoToCreateDR", ex.Message, ex);
            }
            return response;
        }


        public async Task<RaiseDrLocationModel> GetRaiseDRLocationInfo(int companyId, bool isBuyerCompany, List<int> locationIds, List<int> carrierIds, int timeout = 30)
        {
            var response = new RaiseDrLocationModel();
            try
            {
                DataTable tblJobIds, tblCarrierIds;
                InitializeTableIntParameters(locationIds, out tblJobIds);
                InitializeTableIntParameters(carrierIds, out tblCarrierIds);

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    try
                    {
                        using (SqlCommand command = connection.CreateCommand())
                        {
                            command.CommandText = "[dbo].[Usp_GetRaiseDRLocationInfo]";
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandTimeout = timeout;
                            command.Parameters.Add(new SqlParameter("CompanyId", companyId));
                            command.Parameters.Add(new SqlParameter("IsBuyerCompany", isBuyerCompany));
                            command.Parameters.Add(new SqlParameter("LocationIds", tblJobIds));
                            command.Parameters.Add(new SqlParameter("CarrierIds", tblCarrierIds));
                            connection.Open();
                            SqlDataReader reader = await command.ExecuteReaderAsync();
                            response.JobDetails = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<JobDetail>(reader).ToList();
                            reader.NextResult();
                            response.CarrierJobDetails = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<CarrierJobDetail>(reader).ToList();
                            reader.NextResult();
                            response.JobIds = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<int>(reader).ToList();
                            reader.Close();
                        }
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        connection.Close();
                        LogManager.Logger.WriteException("StoredProcedureDomain", "GetRaiseDRLocationInfo", ex.Message, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetRaiseDRLocationInfo", ex.Message, ex);
            }
            return response;
        }

        private static void InitializeTableIntParameters(List<int> idList, out DataTable ids)
        {
            ids = CreateIntTableVariable();
            foreach (var item in idList.Distinct().ToList())
            {
                var row = ids.NewRow();
                row["Id"] = item;
                ids.Rows.Add(row);
            }
        }

        private static void InitializeTableLongParameters(List<long> idList, out DataTable ids)
        {
            ids = CreateLongTableVariable();
            foreach (var item in idList.Distinct().ToList())
            {
                var row = ids.NewRow();
                row["Id"] = item;
                ids.Rows.Add(row);
            }
        }

        private static void InitializeTableStringParameters(List<string> valueList, out DataTable values)
        {
            values = CreateStringTableVariable();
            foreach (var item in valueList.Distinct().ToList())
            {
                var row = values.NewRow();
                row["Value"] = item;
                values.Rows.Add(row);
            }
        }

        private static DataTable CreateIntTableVariable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            return dt;
        }

        private static DataTable CreateLongTableVariable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(long));
            return dt;
        }

        private static DataTable CreateStringTableVariable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Value", typeof(string));
            return dt;
        }

        public async Task<List<SupplierBOLReportViewModel>> GetLiftFileRecordsWithMissingTFXDeliveryDetails(int companyId, DateTimeOffset startDate, DateTimeOffset endDate)
        {

            var response = new List<SupplierBOLReportViewModel>();
            try
            {
                object inputModel = new { @CompanyId = companyId, @FromDate = startDate.Date, @ToDate = endDate.Date };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetLiftFileRecordsWithMissingTFXDeliveryDetails", inputModel);
                Context.DataContext.Database.CommandTimeout = 30;
                response = await Context.DataContext.Database.SqlQuery<SupplierBOLReportViewModel>(input.Query, input.Params.ToArray()).ToListAsync();

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetLiftFileRecordsWithMissingTFXDeliveryDetails", ex.Message, ex);

            }
            return response;

        }

        public async Task<List<CarrierBOLReportViewModel>> GetTFXDeliveryDetailsWithMissingLiftFileRecords(int companyId, DateTimeOffset startDate, DateTimeOffset endDate)
        {

            var response = new List<CarrierBOLReportViewModel>();
            try
            {
                object inputModel = new { @CompanyId = companyId, @FromDate = startDate.Date, @ToDate = endDate.Date };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetTFXDeliveryDetailsWithMissingLiftFileRecords", inputModel);
                Context.DataContext.Database.CommandTimeout = 30;
                response = await Context.DataContext.Database.SqlQuery<CarrierBOLReportViewModel>(input.Query, input.Params.ToArray()).ToListAsync();

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetTFXDeliveryDetailsWithMissingLiftFileRecords", ex.Message, ex);

            }
            return response;
        }

        public async Task<List<LFRecordsGridViewModel>> GetLiftFileRecordsScratchReport(UserContext context)
        {
            var response = new List<LFRecordsGridViewModel>();
            try
            {
                var currentDate = DateTimeOffset.Now;
                object inputModel = new { @CompanyId = context.CompanyId, @CurrentDate = currentDate.Date };
                var input = SqlHelperMethods.GetStoredProcedure("usp_LiftFileValidationScratchReport", inputModel);
                Context.DataContext.Database.CommandTimeout = 30;
                response = await Context.DataContext.Database.SqlQuery<LFRecordsGridViewModel>(input.Query, input.Params.ToArray()).ToListAsync();

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetLiftFileRecordsScratchReport", ex.Message, ex);

            }
            return response;
        }

        public async Task<List<LFRecordsGridViewModel>> GetLFVAccrualReportGrid(int companyId, AccrualReportGridInputViewModel input, DataTableSearchModel requestModel)
        {
            var response = new List<LFRecordsGridViewModel>();
            try
            {
                if (input.FromDate == null && input.ToDate == null)
                {
                    input.ToDate = DateTimeOffset.Now;
                }
                var inputModel = new
                {
                    @CompanyId = companyId,
                    @FromDate = input.FromDate,
                    @ToDATE = input.ToDate,
                    @ProductTypeIds = input.ProductTypeIds,
                    requestModel
                };
                var spInput = SqlHelperMethods.GetStoredProcedure("usp_GetLiftFileAccrualReport", inputModel);
                response = await Context.DataContext.Database.SqlQuery<LFRecordsGridViewModel>(spInput.Query, spInput.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetLFVAccrualReportGrid", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetLFVValidationStatsAndProductTypesDDL(int companyId, AccrualReportGridInputViewModel input)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                if (input.FromDate == null && input.ToDate == null)
                {
                    input.ToDate = DateTimeOffset.Now;
                }
                var inputModel = new
                {
                    @CompanyId = companyId,
                    @FromDate = input.FromDate,
                    @ToDATE = input.ToDate,
                };
                var spInput = SqlHelperMethods.GetStoredProcedure("usp_GetLFVProductTypes", inputModel);
                response = await Context.DataContext.Database.SqlQuery<DropdownDisplayItem>(spInput.Query, spInput.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetLFVValidationStatsAndProductTypesDDL", ex.Message, ex);
            }
            return response;
        }

        public async Task<DeleteTpoCompanyViewModel> DeleteTpoBuyerCompany(int buyerCompanyId, int supplierCompanyId, int deletedByUserId)
        {
            var response = new DeleteTpoCompanyViewModel();
            try
            {
                if (buyerCompanyId <= 0)
                {
                    response.StatusMessage = "Invalid buyer company";
                    return response;
                }
                var inputModel = new
                {
                    @BuyerCompanyId = buyerCompanyId,
                    @SupplierCompanyId = supplierCompanyId,
                    @DeletedByUserId = deletedByUserId,
                };
                var spInput = SqlHelperMethods.GetStoredProcedure("usp_DeleteTpoBuyerCompany", inputModel);
                response = await Context.DataContext.Database.SqlQuery<DeleteTpoCompanyViewModel>(spInput.Query, spInput.Params.ToArray()).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "DeleteTpoBuyerCompany", ex.Message, ex);
            }
            return response;
        }

        #region SalesUserSourcingRequest   
        public async Task<List<UspGetSoucingRequest>> GetSourcingRequestGrid(int companyId, bool isFromDashboard, int timeout = 15)
        {
            var response = new List<UspGetSoucingRequest>();
            try
            {
                var inputModel = new { companyId = companyId, isFromDashboard = isFromDashboard };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetSoucingRequests", inputModel);
                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspGetSoucingRequest>(input.Query, input.Params.ToArray()).ToListAsync();
                response = response.GroupBy(t => t.RequestNumber).Select(r => new { ListItem = r.ToList() }).Select(t => t.ListItem.ToViewModel()).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetSourcingRequestGrid", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<UspGetSourcingOrder>> GetSalesUserOrders(int CompanyId, int timeout = 30)
        {
            var response = new List<UspGetSourcingOrder>();
            try
            {
                var inputModel = new { CompanyId = CompanyId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetSalesUserOrders", inputModel);
                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspGetSourcingOrder>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetSalesUserOrders", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<UspGetGeneralNotesViewModel>> GetSourcingNotes(int id, int timeout = 30)
        {
            var response = new List<UspGetGeneralNotesViewModel>();
            try
            {
                var inputModel = new { Id = id };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetSourcingNotes", inputModel);
                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspGetGeneralNotesViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetSourcingNotes", ex.Message, ex);
            }
            return response;
        }


        #endregion

        public async Task<List<UspCarrierMapping>> GetCarrierIDMappings(int companyId, int countryId, int timeout = 30)
        {
            var response = new List<UspCarrierMapping>();
            try
            {
                var inputModel = new { companyId, countryId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetCarrierIDMappings", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspCarrierMapping>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetCarrierIDMappings", ex.Message + " -CompanyId: " + companyId, ex);
            }

            return response;
        }

        public async Task<List<CarrierInfoForUnAthorizedInventoryDataViewModel>> GetCarriersInfoForInventoryDataExport(int supplierCompanyId)
        {
            var response = new List<CarrierInfoForUnAthorizedInventoryDataViewModel>();
            try
            {

                var inputModel = new { @CompanyId = supplierCompanyId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetCarrierInfoForUnAthorizedInventoryData", inputModel);

                Context.DataContext.Database.CommandTimeout = 30;
                response = await Context.DataContext.Database.SqlQuery<CarrierInfoForUnAthorizedInventoryDataViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
                if (response != null && response.Any())
                {
                    response.ForEach(t => t.SupplierCompanyId = supplierCompanyId);
                }
            }
            catch (Exception ex)
            {

                LogManager.Logger.WriteException("StoredProcedureDomain", "GetCarrierInfoForInventoryDataExport", ex.Message, ex);
            }
            return response;
        }

        public async Task<MarineTaxAffidavitPdfViewModel> GetDetailsForMarineTaxAffidavitPdf(int invoiceHeaderId, UserContext userContext)
        {
            var response = new MarineTaxAffidavitPdfViewModel();
            try
            {
                var inputModel = new { @InvoiceHeaderId = invoiceHeaderId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetMarineTaxAffidavitDetails", inputModel);

                Context.DataContext.Database.CommandTimeout = 30;
                response = await Context.DataContext.Database.SqlQuery<MarineTaxAffidavitPdfViewModel>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
                if (response != null)
                {
                    response.InvoiceHeaderId = invoiceHeaderId;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetDetailsForMarineTaxAffidavitPdf", ex.Message, ex);
            }
            return response;
        }

        public async Task<MarineBDNPdfViewModel> GetDetailsForMarineBDNPdf(int invoiceHeaderId, UserContext userContext)
        {
            var response = new MarineBDNPdfViewModel();
            try
            {
                var inputModel = new { @InvoiceHeaderId = invoiceHeaderId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetMarineBDNDetails", inputModel);

                Context.DataContext.Database.CommandTimeout = 30;
                response = await Context.DataContext.Database.SqlQuery<MarineBDNPdfViewModel>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
                if (response != null)
                {
                    response.InvoiceHeaderId = invoiceHeaderId;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetDetailsForMarineBDNPdf", ex.Message, ex);
            }
            return response;
        }

        public async Task<MarineCGInspectionDocumentPdfViewModel> GetDetailsForMarineCGInspectionDocument(int invoiceHeaderId, UserContext userContext)
        {
            var response = new MarineCGInspectionDocumentPdfViewModel();
            try
            {
                var inputModel = new { @InvoiceHeaderId = invoiceHeaderId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetMarineCGInspectionDocumentDetails", inputModel);

                Context.DataContext.Database.CommandTimeout = 30;
                response = await Context.DataContext.Database.SqlQuery<MarineCGInspectionDocumentPdfViewModel>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
                if (response != null)
                {
                    response.InvoiceHeaderId = invoiceHeaderId;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetDetailsForMarineCGInspectionDocument", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<MarineInspectionRequestVoucherViewModel>> GetDetailsForMarineInspRequestVoucher(int invoiceHeaderId, UserContext userContext = null)
        {
            var response = new List<MarineInspectionRequestVoucherViewModel>();
            try
            {
                var inputModel = new { @InvoiceHeaderId = invoiceHeaderId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetInspectionRequestVoucherDetails", inputModel);

                Context.DataContext.Database.CommandTimeout = 30;
                response = await Context.DataContext.Database.SqlQuery<MarineInspectionRequestVoucherViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetDetailsForMarineInspRequestVoucher", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<AssetDropViewModel>> GetAssetDropsForInvoice(int invoiceId, int timeout = 30)
        {
            var response = new List<AssetDropViewModel>();
            try
            {
                var inputModel = new { @InvoiceId = invoiceId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetAssetDropsForInvoice", inputModel);

                Context.DataContext.Database.CommandTimeout = 30;
                response = await Context.DataContext.Database.SqlQuery<AssetDropViewModel>(input.Query, input.Params.ToArray()).ToListAsync();

            }
            catch (Exception ex)
            {

                LogManager.Logger.WriteException("StoredProcedureDomain", "GetAssetDropsForInvoice", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<InvoiceBolEditGrid>> GetMarineInvoiceBolListAsync(int companyId, int invoiceHeaderId, int timeout = 120)
        {
            var response = new List<InvoiceBolEditGrid>();
            try
            {
                object inputModel = new { CompanyId = companyId, InvoiceHeaderId = invoiceHeaderId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetMarineInvoiceBolList", inputModel);
                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<InvoiceBolEditGrid>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetMarineInvoiceBolListAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<JobBuyerDashboardViewModel>> GetMarinePortsForSuperAdmin(int countryId, UserContext userContext, int timeout = 30)
        {
            var response = new List<JobBuyerDashboardViewModel>();
            try
            {
                var companyId = userContext.IsSuperAdmin ? ApplicationConstants.SuperAdminCompanyId : userContext.CompanyId;
                object inputmodel = new { @CountryId = countryId, @CompanyId = companyId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetMarinePortForSuperAdmin", inputmodel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<JobBuyerDashboardViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetMarinePortsForSuperAdmin", ex.Message, ex);
            }

            return response;
        }
        public async Task<int> SupplierBrokeredOrderValidate(int orderId, int timeout = 30)
        {
            var response = 0;
            try
            {
                object inputModel = new { OrderId = orderId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_Get_SupplierBrokeredOrderValidate", inputModel);
                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<int>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "SupplierBrokeredOrderValidate", ex.Message, ex);
            }
            return response;
        }

        public async Task<bool> OPISPlattsPricingSyncStatus(bool setStatus, bool getStatus, int timeout = 30)
        {
            bool response = false;
            try
            {
                object inputModel = new { setStatus = setStatus, getStatus = getStatus };
                var input = SqlHelperMethods.GetStoredProcedure("usp_OPISPlattsPricingSyncStatus", inputModel);
                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<bool>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "OPISPlattsPricingSyncStatus", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<ServiceArea>> GetCityAndZipsByState(string stateIds, int timeout = 10)
        {
            var response = new List<ServiceArea>();
            try
            {
                if (!string.IsNullOrEmpty(stateIds))
                {
                    object inputmodel = new { @StateIds = stateIds };
                    var input = SqlHelperMethods.GetStoredProcedure("usp_GetZipByState", inputmodel);

                    Context.DataContext.Database.CommandTimeout = timeout;
                    response = await Context.DataContext.Database.SqlQuery<ServiceArea>(input.Query, input.Params.ToArray()).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetZipsByState", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<AssetViewModel>> GetMarineVesselsForSuperAdmin(UserContext userContext)
        {
            var response = new List<AssetViewModel>();
            try
            {
                var companyId = userContext.IsSuperAdmin ? ApplicationConstants.SuperAdminCompanyId : userContext.CompanyId;
                object inputmodel = new { @CompanyId = companyId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetMarineVesselsForSuperAdmin", inputmodel);
                Context.DataContext.Database.CommandTimeout = 30;
                response = await Context.DataContext.Database.SqlQuery<AssetViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetMarineVesselsForSuperAdmin", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<UspGetSourcingInvoice>> GetSalesInvoiceDashboard(int CompanyId, int InvoiceTypeId, int timeout = 15)
        {
            var response = new List<UspGetSourcingInvoice>();
            try
            {
                object inputmodel = new { @companyId = CompanyId, @InvoiceTypeId = InvoiceTypeId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetInvoicesForSalesUserDashboard", inputmodel);
                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspGetSourcingInvoice>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetSalesInvoiceDashboard", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayExtendedItem>> GetLiftFileCarrierDropDownForDashboard(int companyId, DateTimeOffset? fromDate, DateTimeOffset? toDate, int timeout = 30)
        {
            var response = new List<DropdownDisplayExtendedItem>();
            try
            {
                object inputModel = new { CompanyId = companyId, fromDate, toDate };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetLiftFileCarrierDetailsForDashboard", inputModel);
                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<DropdownDisplayExtendedItem>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetLiftFileCarrierDropDownForDashboard", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetReasonDescriptionList(int CompanyId)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                object inputModel = new { CompanyId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetReasonDescriptionList", inputModel);
                Context.DataContext.Database.CommandTimeout = 30;
                response = await Context.DataContext.Database.SqlQuery<DropdownDisplayItem>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetReasonDescriptionList", ex.Message, ex);
            }
            return response;
        }

        public async Task<BooleanResponseModel> DeleteBolForMarineInvoice(int invoiceHeaderId, int invoiceFtlDetailsId, int invoiceId)
        {
            var response = new BooleanResponseModel();
            try
            {
                object inputModel = new { @InvoiceHeaderId = invoiceHeaderId, @InvoiceId = invoiceId, @InvoiceFtlDetailsId = invoiceFtlDetailsId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_ValidateBolDeleteRequest", inputModel);
                Context.DataContext.Database.CommandTimeout = 30;
                response = await Context.DataContext.Database.SqlQuery<BooleanResponseModel>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "DeleteBolForMarineInvoice", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<Usp_CompanySpecificDeliveryDetails>> GetCompanySpecificDeliveryDetails(int companyId)
        {
            List<Usp_CompanySpecificDeliveryDetails> response = new List<Usp_CompanySpecificDeliveryDetails>();
            try
            {
                //.AddDays(-1) to get data for previous day, as job will be running at 2AM.
                DateTimeOffset startdate = new DateTimeOffset(DateTimeOffset.Now.Date.Year, DateTimeOffset.Now.Date.AddDays(-1).Month, DateTimeOffset.Now.Date.AddDays(-1).Day, 0, 0, 1, TimeSpan.Zero);
                DateTimeOffset enddate = new DateTimeOffset(DateTimeOffset.Now.Date.Year, DateTimeOffset.Now.Date.AddDays(-1).Month, DateTimeOffset.Now.AddDays(-1).Date.Day, 23, 59, 59, TimeSpan.Zero);
                object inputModel = new { @CompanyId = companyId, @StartDate = startdate, @EndDate = enddate };
                var input = SqlHelperMethods.GetStoredProcedure("Usp_GetCompanySpecificDeliveryDetails", inputModel);
                Context.DataContext.Database.CommandTimeout = 100;
                response = await Context.DataContext.Database.SqlQuery<Usp_CompanySpecificDeliveryDetails>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetCompanySpecificDeliveryDetails", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<DropdownDisplayItem>> GetCustomersForSupplier(int companyId, bool isMarine)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                object inputModel = new { @CompanyId = companyId, @IsMarine = isMarine };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetMarineCustomers", inputModel);
                Context.DataContext.Database.CommandTimeout = 30;
                response = await Context.DataContext.Database.SqlQuery<DropdownDisplayItem>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetCustomersForSupplier", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<DropdownDisplayExtendedProperty>> GetJobsForSupplier(int companyId, bool isMarine)
        {
            var response = new List<DropdownDisplayExtendedProperty>();
            try
            {
                object inputModel = new { @CompanyId = companyId, @IsMarine = isMarine };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetMarineCustomerJobs", inputModel);
                Context.DataContext.Database.CommandTimeout = 30;
                response = await Context.DataContext.Database.SqlQuery<DropdownDisplayExtendedProperty>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetJobsForSupplier", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<DropdownDisplayExtendedProperty>> GetCustomerJobs(string customerIds, bool IsMarine, int supplierCompanyId)
        {
            var response = new List<DropdownDisplayExtendedProperty>();
            try
            {
                object inputModel = new { @CustomerIds = customerIds, @IsMarine = IsMarine, @SupplierCompanyId = supplierCompanyId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetCustomerJobs", inputModel);
                Context.DataContext.Database.CommandTimeout = 30;
                response = await Context.DataContext.Database.SqlQuery<DropdownDisplayExtendedProperty>(input.Query, input.Params.ToArray()).ToListAsync();

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetCustomerJobs", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayExtendedProperty>> GetAssetAndTankForOrders(string jobIds)
        {
            var response = new List<DropdownDisplayExtendedProperty>();
            try
            {
                object inputModel = new { @JobIds = jobIds };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetAssetAndTankForOrders", inputModel);
                Context.DataContext.Database.CommandTimeout = 30;
                response = await Context.DataContext.Database.SqlQuery<DropdownDisplayExtendedProperty>(input.Query, input.Params.ToArray()).ToListAsync();

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MasterDomain", "GetAssetAndTankForOrders", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<PickupLocationDetailViewModel>> GetTerminalsForSuperadminGridAsync(int countryId)
        {
            var response = new List<PickupLocationDetailViewModel>();
            try
            {
                object inputModel = new { @CountryId = countryId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetTerminalsForGrid", inputModel);
                Context.DataContext.Database.CommandTimeout = 30;
                var spResponse = await Context.DataContext.Database.SqlQuery<PickupLocationDetailViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
                if (spResponse != null && spResponse.Any())
                {
                    response = spResponse.OrderByDescending(t => t.Id).ToList();
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetTerminalsForSuperadminGridAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<SAPDeliveryDetailsViewModel>> GetDeliveryDetailsForSAP(int invoiceHeaderId, int timeout = 30)
        {
            var response = new List<SAPDeliveryDetailsViewModel>();
            try
            {
                object inputModel = new { @InvoiceHeaderId = invoiceHeaderId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetDeliveryDetailsforSAP", inputModel);
                Context.DataContext.Database.CommandTimeout = 30;
                response = await Context.DataContext.Database.SqlQuery<SAPDeliveryDetailsViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetDeliveryDetailsForSAP" + invoiceHeaderId.ToString(), ex.Message, ex);
            }
            return response;
        }

        public async Task<List<TerminalProductMappingDetailsViewModel>> GetTerminalProductMappingDetails(int countryId, int pricingSourceId)
        {
            var response = new List<TerminalProductMappingDetailsViewModel>();
            try
            {
                Object inputModel = new { @CountryId = countryId, @PricingSourceId = pricingSourceId };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetTerminalProductAssignmentGrid", inputModel);
                Context.DataContext.Database.CommandTimeout = 30;
                response = await Context.DataContext.Database.SqlQuery<TerminalProductMappingDetailsViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetTerminalProductMappingDetails", ex.Message, ex);
            }
            return response;
        }
        public async Task<USPCustomerLoadQueueDetails> GetCustomerBrandANDLoadAttributeDetails(int customerId, List<int> jobIDs, int timeout = 30)
        {
            USPCustomerLoadQueueDetails response = new USPCustomerLoadQueueDetails();

            var connection = Context.DataContext.Database.Connection;
            var command = connection.CreateCommand();

            try
            {
                // add parameters
                command.CommandText = "[dbo].[usp_Get_CustomerBrand_AND_LoadAndDRQueue_Details]";
                command.CommandType = CommandType.StoredProcedure;

                command.CommandTimeout = timeout;

                var parameter = command.CreateParameter();
                var JobIds = CreateIntTableVariable();
                foreach (var item in jobIDs)
                {
                    var row = JobIds.NewRow();
                    row["Id"] = item;
                    JobIds.Rows.Add(row);
                }
                parameter.Value = JobIds;
                parameter.ParameterName = "@JobIds";

                command.Parameters.Add(parameter);
                command.Parameters.Add(new SqlParameter("@CompanyID", customerId));


                connection.Open();
                var reader = await command.ExecuteReaderAsync();


                List<UspJobDetails> jobDetails = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<UspJobDetails>(reader).ToList();
                reader.NextResult();

                List<UspCustomerBrandDetails> customerBrandDetails = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<UspCustomerBrandDetails>(reader).ToList();
                reader.NextResult();


                UspCustomerLoadQueueAttributes customerLoadQueueAttributes = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<UspCustomerLoadQueueAttributes>(reader).FirstOrDefault();
                reader.NextResult();

                if (jobDetails != null)
                {
                    response.jobDetails = jobDetails;
                }

                if (customerLoadQueueAttributes != null)
                {
                    response.customerLoadQueueAttributes = customerLoadQueueAttributes;
                }
                if (customerBrandDetails != null)
                {
                    response.customerBrandDetails = customerBrandDetails;
                }
                reader.Close();

                return response;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetCustomerBrandANDLoadAttributeDetails", ex.Message, ex);
            }
            finally
            {
                command.Dispose();
                connection.Close();
            }

            return response;
        }


        public async Task<TerminalProductMappingInput> GetTerminalProductMappingDetailsInput(int terminalId, List<DropdownDisplayItem> assignedProducts)
        {
            var response = new TerminalProductMappingInput();
            var connection = Context.DataContext.Database.Connection;
            var command = connection.CreateCommand();
          
            try
            {
                string assignedProductIds = string.Join(",", assignedProducts.Select(t => t.Id).ToList());

                object inputmodel = new { @TerminalId = @terminalId, @strProductIds = assignedProductIds };


                var input = SqlHelperMethods.GetStoredProcedure("usp_GetTerminalProductMappingDetailsInput", inputmodel);
                Context.DataContext.Database.CommandTimeout = 30;
                response = await Context.DataContext.Database.SqlQuery<TerminalProductMappingInput>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();

             
                command.CommandText = "[dbo].[usp_GetTerminalProductMappingDetailsInput]";
                command.CommandType = CommandType.StoredProcedure;

                command.CommandTimeout = 30;

                command.Parameters.Add(new SqlParameter("@TerminalId ", terminalId));

                command.Parameters.Add(new SqlParameter("@strProductIds ", assignedProductIds));

                connection.Open();
                var reader = await command.ExecuteReaderAsync();
                if (response == null)
                    response = new TerminalProductMappingInput();

                response.TerminalId = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<int>(reader).FirstOrDefault();
                reader.NextResult();

                response.NewProductIds = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<int>(reader).ToList();
                reader.NextResult();

                response.RemovedProductIds = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<int>(reader).ToList();
                reader.NextResult();

                response.ExistingMappedProductIds = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<int>(reader).ToList();
                reader.NextResult();

                response.ExternalProductIdXProductIds = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<ExternalProductIdXProductIdViewModel>(reader).ToList();
                reader.NextResult();

                response.ProductIdXMappingStatus = ((IObjectContextAdapter)Context.DataContext).ObjectContext.Translate<ProductIdXMappingStatusViewModel>(reader).ToList();
                reader.NextResult();
                reader.Close();
            }
            catch (Exception ex)
            {
               
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetTerminalProductMappingDetailsInput", ex.Message, ex);
            }
            finally
            {
               
                command.Dispose();
                connection.Close();
            }
            return response;
        }

        public async Task<UspGetSupplierDashboardInvoices> GetSupplierInvoicesAndDDTForDashboard(int CompanyId, int CountryId, int CurrencyType, string groupIds = "", int timeout = 30)
        {
            var response = new UspGetSupplierDashboardInvoices();
            try
            {
                object inputModel = new { CompanyId, CountryId, CurrencyType, GroupIds = groupIds };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetSupplierInvoicesAndDDTForDashboard", inputModel);

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<UspGetSupplierDashboardInvoices>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetSupplierInvoicesAndDDTForDashboard", ex.Message, ex);
            }

            return response;
        }

        public DashboardTotalGallonsCountViewModel GetGallonsDeliveredCount(int countryId, string startDate, string endDate)
        {
            var response = new DashboardTotalGallonsCountViewModel();
            try
            {
                DateTimeOffset? dtStartDate = null;
                DateTimeOffset? dtEndDate = null;
                if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
                {
                    dtStartDate = Convert.ToDateTime(startDate).ToUniversalTime();
                    dtEndDate = Convert.ToDateTime(endDate).ToUniversalTime();
                }
                else if (!string.IsNullOrEmpty(startDate))
                {
                    dtStartDate = Convert.ToDateTime(startDate).ToUniversalTime();
                }
                else if (!string.IsNullOrEmpty(endDate))
                {
                    dtEndDate = Convert.ToDateTime(endDate).ToUniversalTime();
                }

                Object inputModel = new { @CountryId = countryId, @StartDate = dtStartDate, dtEndDate = dtEndDate };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetGallonsDeliveredCount", inputModel);
                Context.DataContext.Database.CommandTimeout = 30;
                response = Context.DataContext.Database.SqlQuery<DashboardTotalGallonsCountViewModel>(input.Query, input.Params.ToArray()).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetGallonsDeliveredCount", ex.Message, ex);
            }
            return response;
        }

        public DashboardTotalGallonsCountViewModel GetGallonsOrderedCount(int countryId, string startDate, string endDate)
        {
            var response = new DashboardTotalGallonsCountViewModel();
            try
            {
                DateTimeOffset? dtStartDate = null;
                DateTimeOffset? dtEndDate = null;
                if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
                {
                    dtStartDate = Convert.ToDateTime(startDate).ToUniversalTime();
                    dtEndDate = Convert.ToDateTime(endDate).ToUniversalTime();
                }
                else if (!string.IsNullOrEmpty(startDate))
                {
                    dtStartDate = Convert.ToDateTime(startDate).ToUniversalTime();
                }
                else if (!string.IsNullOrEmpty(endDate))
                {
                    dtEndDate = Convert.ToDateTime(endDate).ToUniversalTime();
                }


                Object inputModel = new { @CountryId = countryId, @StartDate = dtStartDate, dtEndDate = dtEndDate };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetGallonsOrderedCount", inputModel);
                Context.DataContext.Database.CommandTimeout = 30;
                response = Context.DataContext.Database.SqlQuery<DashboardTotalGallonsCountViewModel>(input.Query, input.Params.ToArray()).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "usp_GetGallonsOrderedCount", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetCompanies(int timeout = 30)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetCompaniesDropDown", new { });

                Context.DataContext.Database.CommandTimeout = timeout;
                response = await Context.DataContext.Database.SqlQuery<DropdownDisplayItem>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetCompanies", ex.Message, ex);
            }
            return response;
        }

        public async void AddApiLogs(ApiLog apiLog)
        {
            try
            {
                var inputModel = new
                {
                    @Request = apiLog.Request,
                    @Response = apiLog.Response,
                    @Url = apiLog.Url,
                    @ExternalRefID = apiLog.ExternalRefID,
                    @Message = apiLog.Message,
                    @CompanyId = apiLog.CompanyId,
                    @CreatedBy = apiLog.CreatedBy,
                    @IsSuccessStatusCode = apiLog.IsSuccessStatusCode,
                    @RetryCount = apiLog.RetryCount
                };
                var input = SqlHelperMethods.GetStoredProcedure("usp_SaveApiLog", inputModel);
                Context.DataContext.Database.CommandTimeout = 30;
                await Context.DataContext.Database.SqlQuery<int>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "AddApiLogs", ex.Message, ex);

            }
        }


        public async Task<List<ApiLogViewModel>> GetApiLogsForCompany(int companyId=0,DateTimeOffset? fromDate=null,DateTimeOffset? toDate=null,int apiLogId = 0,string externalRefId=null,bool? statusCode= null)
        {
            var response = new List<ApiLogViewModel>();
            try
            {
                var inputModel = new
                {
                    @CompanyId = companyId,
                    @FromDate = fromDate,
                    @ToDate = toDate,
                    @ApiLogId = apiLogId,
                    @ExternalRefId = externalRefId,
                    @IsSuccesStatusCode = statusCode
                };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetApiLogsForCompany", inputModel);
                Context.DataContext.Database.CommandTimeout = 30;
                response = await Context.DataContext.Database.SqlQuery<ApiLogViewModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetApiLogsByCompany", ex.Message, ex);
            }
            return response;
        }
    }
}


