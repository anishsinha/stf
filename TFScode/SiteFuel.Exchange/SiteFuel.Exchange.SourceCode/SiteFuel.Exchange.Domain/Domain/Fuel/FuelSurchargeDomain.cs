using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Domain.Mappers.BillingStatement;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.BillingStatement;
using SiteFuel.Exchange.ViewModels.FuelSurcharge;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class FuelSurchargeDomain : BaseDomain
    {
        public FuelSurchargeDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public FuelSurchargeDomain(BaseDomain domain) : base(domain)
        {
        }

        public CreateFuelSurchargeInputViewModel GetCreateFuelSurchargeInput(int companyId)
        {
            CreateFuelSurchargeInputViewModel viewModel = new CreateFuelSurchargeInputViewModel();
            var address = Context.DataContext.CompanyAddresses.FirstOrDefault(t => t.CompanyId == companyId && t.IsActive && t.IsDefault);
            if (address != null)
            {
                string timeZoneName = GoogleApiDomain.GetTimeZone(address.Latitude, address.Longitude);
                if (!string.IsNullOrEmpty(timeZoneName))
                {
                    viewModel.StartDate = DateTimeOffset.Now.ToTargetDateTimeOffset(timeZoneName);
                }
            }
            return viewModel;
        }

        public ViewFuelSurchargeInputViewModel GetViewFuelSurchargeInput(int companyId)
        {
            ViewFuelSurchargeInputViewModel viewModel = new ViewFuelSurchargeInputViewModel();
            var address = Context.DataContext.CompanyAddresses.FirstOrDefault(t => t.CompanyId == companyId && t.IsActive && t.IsDefault);
            if (address != null)
            {
                string timeZoneName = GoogleApiDomain.GetTimeZone(address.Latitude, address.Longitude);
                if (!string.IsNullOrEmpty(timeZoneName))
                {
                    viewModel.StartDate = DateTimeOffset.Now.ToTargetDateTimeOffset(timeZoneName);
                }
            }
            return viewModel;
        }

        public List<FuelSurchargeTableViewModel> GenerateTable(CreateFuelSurchargeInputViewModel viewModel)
        {
            List<FuelSurchargeTableViewModel> response = new List<FuelSurchargeTableViewModel>();
            decimal startValue = viewModel.PriceRangeStartValue;
            decimal surchargePercentage = viewModel.FuelSurchargeStartPercentage;
            decimal endValue = 0;
            while (startValue <= viewModel.PriceRangeEndValue)
            {
                endValue = viewModel.PriceRangeInterval == 0 ? viewModel.PriceRangeEndValue : startValue + viewModel.PriceRangeInterval;
                if (endValue > viewModel.PriceRangeEndValue)
                {
                    endValue = viewModel.PriceRangeEndValue;
                }
                response.Add(new FuelSurchargeTableViewModel() { FuelSurchargeStartPercentage = surchargePercentage, PriceRangeStartValue = startValue, PriceRangeEndValue = endValue });
                startValue = GetPriceStartValue(endValue);
                surchargePercentage += viewModel.SurchargeInterval;
            }
            return response;
        }


        public List<FuelSurchargeTableViewModel> GenerateTable(decimal priceRangeStartValue, decimal priceRangeEndValue, decimal priceRangeInterval, decimal surchargeInterval, decimal fuelSurchargeStartPercentage)
        {
            List<FuelSurchargeTableViewModel> response = new List<FuelSurchargeTableViewModel>();
            decimal startValue = priceRangeStartValue;
            decimal surchargePercentage = fuelSurchargeStartPercentage;
            decimal endValue = 0;
            while (startValue <= priceRangeEndValue)
            {
                endValue = priceRangeInterval == 0 ? priceRangeEndValue : startValue + priceRangeInterval;
                if (endValue > priceRangeEndValue)
                {
                    endValue = priceRangeEndValue;
                }
                response.Add(new FuelSurchargeTableViewModel() { FuelSurchargeStartPercentage = surchargePercentage, PriceRangeStartValue = startValue, PriceRangeEndValue = endValue });
                startValue = GetPriceStartValue(endValue);
                surchargePercentage += surchargeInterval;
            }
            return response;
        }

        public async Task<Nullable<Int32>> GetExistingFuelSurchargeId(FuelSurchargeIndexViewModel viewModel, int companyId)
        {
            var fs = await Context.DataContext.FuelSurchargeIndexes.FirstOrDefaultAsync(f => f.Name.Trim().ToLower().Equals(viewModel.TableName.Trim().ToLower())
                     && f.SupplierCompanyId.Value == companyId && f.IsActive);
            if (fs != null) return fs.Id;
            return null;
        }

        public async Task<StatusViewModel> UpdateFuelSurchargeTableAsync(FuelSurchargeIndexViewModel viewModel, int userId, int companyId, int entityId)
        {

            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var entity = await Context.DataContext.FuelSurchargeIndexes.FirstOrDefaultAsync(t => t.Id == entityId);
                    if (entity != null && viewModel != null)
                    {
                        // routine 1 : sanity check
                        if (entity.StatusId == FreightTableStatus.Published && (FreightTableStatus)viewModel.StatusId == FreightTableStatus.Draft)
                        {
                            response.StatusMessage = viewModel.TableName + " is already in published mode. Publish mode to Draft mode is not allowed.";
                            response.StatusCode = Status.Failed;
                            return response;
                        }

                        if (!viewModel.FuelSurchargeIndexId.HasValue) // must be duplicate
                        {
                            response.StatusMessage = "Fuel surcharge table " + viewModel.TableName + " is all ready exist.";
                            response.StatusCode = Status.Failed;
                            return response;
                        }

                        // routine 2: by reference update of entity.

                        entity = viewModel.ToEntity(entity); // update existing entity
                        entity.SupplierCompanyId = companyId;
                        entity.UpdatedBy = userId;
                        Context.DataContext.Entry(entity).State = EntityState.Modified;
                        await Context.CommitAsync();

                        //routine 3 : delete all related collections from bottom to top to avoid primary and foregin key voilations.
                        while (entity.FreightTablePickupLocations.Count > 0)
                        {
                            var item = entity.FreightTablePickupLocations.First();
                            entity.FreightTablePickupLocations.Remove(item);
                            Context.DataContext.Entry(item).State = EntityState.Deleted;
                            await Context.CommitAsync();
                        }

                        while (entity.FreightTableSourceRegions.Count > 0)
                        {
                            var item = entity.FreightTableSourceRegions.First();
                            entity.FreightTableSourceRegions.Remove(item);
                            Context.DataContext.Entry(item).State = EntityState.Deleted;
                            await Context.CommitAsync();
                        }

                        while (entity.FreightTableCompanies.Count > 0)
                        {
                            var item = entity.FreightTableCompanies.First();
                            entity.FreightTableCompanies.Remove(item);
                            Context.DataContext.Entry(item).State = EntityState.Deleted;
                            await Context.CommitAsync();
                        }

                        while (entity.FuelSurchargeGeneratedTables.Count > 0)
                        {
                            var item = entity.FuelSurchargeGeneratedTables.First();
                            entity.FuelSurchargeGeneratedTables.Remove(item);
                            Context.DataContext.Entry(item).State = EntityState.Deleted;
                            await Context.CommitAsync();
                        }


                        // routine 4 : fill into FreightTablePickupLocations collection

                        if (viewModel.TerminalsAndBulkPlants != null && viewModel.TerminalsAndBulkPlants.Count > 0)
                        {
                            Context.DataContext.FreightTablePickupLocations.AddRange(
                            viewModel.TerminalsAndBulkPlants.Select(t => new FreightTablePickupLocation
                            {
                                //Todo : should not be hard coded string...
                                FuelSurchargeIndexId = entity.Id,
                                //very tricky situation , 
                                //since terminal id and bulkplant id may be duplicate and angular2-multiselect control does work with duplicate Id
                                //refer 1. "create-fuel-surcharge.component" and "FuelSurchargeDomain-CreateFuelSurchargeTableAsync,GetTerminalsAndBulkPlantsAsync" 
                                //refer 2. "FuelSurchargeMapper-ToViewModel"
                                //so have to managed to make unique in  Get and save functionality i.e BulkPlants_Id,Terminals_Id.    
                                BulkPlantId = t.Code == "Bulk Plants" ? Int32.Parse(t.Id.Split("_".ToCharArray())[1]) : new Nullable<int>(),
                                TerminalId = t.Code == "Terminals" ? Int32.Parse(t.Id.Split("_".ToCharArray())[1]) : new Nullable<int>(),
                                IsActive = true
                            }));

                            await Context.CommitAsync();
                        }

                        // routine 5 : fill into SourceRegions collection
                        if (viewModel.SourceRegions != null && viewModel.SourceRegions.Count > 0)
                            Context.DataContext.FreightTableSourceRegions.AddRange(
                            viewModel.SourceRegions.Select(t => new FreightTableSourceRegion
                            {
                                FuelSurchargeIndexId = entity.Id,
                                SourceRegionId = t.Id
                            }));
                        await Context.CommitAsync();

                        // routine 6 : fill into FuelSurchargeCompanies collection

                        if (viewModel.Customers != null && viewModel.Customers.Any())
                            Context.DataContext.FreightTableCompanies.AddRange(viewModel.Customers.Select(t => new FreightTableCompany
                            {
                                FuelSurchargeIndexId = entity.Id,
                                AssignedCompanyId = t.Id, // customer ids
                                AssignedCompanyType = AssignedCompanyType.Customer,
                                IsActive = true
                            }));
                        await Context.CommitAsync();

                        // routine 6.1 : fill into FuelSurchargeCompanies collection
                        if (viewModel.Carriers != null && viewModel.Carriers.Any())
                            Context.DataContext.FreightTableCompanies.AddRange(viewModel.Carriers.Select(t => new FreightTableCompany
                            {
                                FuelSurchargeIndexId = entity.Id,
                                AssignedCompanyId = t.Id, // carrier ids
                                AssignedCompanyType = AssignedCompanyType.Carrier,
                                IsActive = true
                            }));
                        await Context.CommitAsync();


                        // routine 7 : fill into FuelSurchargeGeneratedTables collection

                        if (viewModel.GeneratedSurchargeTable != null && viewModel.GeneratedSurchargeTable.Count > 0)
                            Context.DataContext.FuelSurchargeGeneratedTables.AddRange(
                            viewModel.GeneratedSurchargeTable.Select(t => new FuelSurchargeGeneratedTable
                            {
                                FuelSurchargeIndexId = entity.Id,
                                PriceRangeStartValue = t.PriceRangeStartValue.Value,
                                PriceRangeEndValue = t.PriceRangeEndValue.Value,
                                FuelSurchargePercentage = t.FuelSurchargeStartPercentage.Value
                            }));


                        await Context.CommitAsync();
                        transaction.Commit();

                        response.StatusMessage = "Fuel surcharge table " + viewModel.TableName + " has been updated successfully.";
                        response.StatusCode = Status.Success;
                    }
                }
                catch (Exception ex)
                {
                    string tblName = viewModel != null ? viewModel.TableName : string.Empty;
                    response.StatusMessage = "Fuel surcharge table " + tblName + " failed to update.";
                    response.StatusCode = Status.Failed;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("FuelSurchargeDomain", "UpdateFuelSurchargeTableAsync", ex.Message, ex);
                }
                return response;
            }
        }

        public async Task<StatusViewModel> CreateFuelSurchargeTableAsync(FuelSurchargeIndexViewModel viewModel, int userId, int companyId)
        {
            var response = new StatusViewModel(Status.Success);

            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {

                    //1 : routine  
                    var entity = viewModel.ToEntity(new FuelSurchargeIndex { SupplierCompanyId = companyId, CreatedBy = userId, UpdatedBy = userId });
                    Context.DataContext.FuelSurchargeIndexes.Add(entity);
                    await Context.CommitAsync();

                    //2 : routine
                    if (viewModel.Customers != null && viewModel.Customers.Any())
                        Context.DataContext.FreightTableCompanies.AddRange(viewModel.Customers.Select(t => new FreightTableCompany
                        {
                            FuelSurchargeIndexId = entity.Id,
                            AssignedCompanyId = t.Id, // customer ids
                            AssignedCompanyType = AssignedCompanyType.Customer,
                            IsActive = true
                        }));
                    await Context.CommitAsync();

                    //2.1: routine
                    if (viewModel.Carriers != null && viewModel.Carriers.Any())
                        Context.DataContext.FreightTableCompanies.AddRange(viewModel.Carriers.Select(t => new FreightTableCompany
                        {
                            FuelSurchargeIndexId = entity.Id,
                            AssignedCompanyId = t.Id, // carrier ids
                            AssignedCompanyType = AssignedCompanyType.Carrier,
                            IsActive = true
                        }));
                    await Context.CommitAsync();

                    //3 : routine
                    if (viewModel.SourceRegions != null && viewModel.SourceRegions.Count > 0)
                        Context.DataContext.FreightTableSourceRegions.AddRange(
                        viewModel.SourceRegions.Select(t => new FreightTableSourceRegion
                        {
                            FuelSurchargeIndexId = entity.Id,
                            SourceRegionId = t.Id
                        }));
                    await Context.CommitAsync();

                    //4 : routine  
                    if (viewModel.TerminalsAndBulkPlants != null && viewModel.TerminalsAndBulkPlants.Count > 0)
                        Context.DataContext.FreightTablePickupLocations.AddRange(
                        viewModel.TerminalsAndBulkPlants.Select(t => new FreightTablePickupLocation
                        {
                            //Todo : should not be hard coded string...
                            FuelSurchargeIndexId = entity.Id,
                            //very tricky situation , 
                            //since terminal id and bulkplant id may be duplicate and angular2-multiselect control does work with duplicate Id
                            //refer 1. "create-fuel-surcharge.component" and "FuelSurchargeDomain-CreateFuelSurchargeTableAsync,GetTerminalsAndBulkPlantsAsync" 
                            //refer 2. "FuelSurchargeMapper-ToViewModel"
                            //so have to managed to make unique in  Get and save functionality i.e BulkPlants_Id,Terminals_Id.    
                            BulkPlantId = t.Code == "Bulk Plants" ? Int32.Parse(t.Id.Split("_".ToCharArray())[1]) : new Nullable<int>(),
                            TerminalId = t.Code == "Terminals" ? Int32.Parse(t.Id.Split("_".ToCharArray())[1]) : new Nullable<int>(),
                            IsActive = true
                        }));
                    await Context.CommitAsync();

                    //5 : routine 
                    if (viewModel.GeneratedSurchargeTable != null && viewModel.GeneratedSurchargeTable.Count > 0)
                        Context.DataContext.FuelSurchargeGeneratedTables.AddRange(
                        viewModel.GeneratedSurchargeTable.Select(t => new FuelSurchargeGeneratedTable
                        {
                            FuelSurchargeIndexId = entity.Id,
                            PriceRangeStartValue = t.PriceRangeStartValue.Value,
                            PriceRangeEndValue = t.PriceRangeEndValue.Value,
                            FuelSurchargePercentage = t.FuelSurchargeStartPercentage.Value
                        }));

                    await Context.CommitAsync();
                    transaction.Commit();
                    response.StatusMessage = "Created successfully.";
                }

                catch (Exception ex)
                {
                    response = new StatusViewModel(Status.Failed);
                    transaction.Rollback();
                    LogManager.Logger.WriteException("FuelSurchargeDomain", "CreateFuelSurchargeTableAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<FuelSurchargeIndexViewModel> GetFuelSurchargeTableAsync(int fuelSurchargeIndexId, int userId, int companyId)
        {
            var viewModel = new FuelSurchargeIndexViewModel();

            try
            {
                var fsIndex = await Context.DataContext.FuelSurchargeIndexes.FirstOrDefaultAsync(t => t.Id == fuelSurchargeIndexId);
                fsIndex.ToViewModel(viewModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelSurchargeDomain", "GetFuelSurchargeTableAsync", ex.Message, ex);
            }

            return viewModel;
        }

        public async Task<FuelSurchargeIndexEIAViewModel> GetFuelSurchargeTableForAutoFreightMethod(int fuelSurchargeIndexId)
        {
            var viewModel = new FuelSurchargeIndexEIAViewModel();
            try
            {
                var fsIndex = await Context.DataContext.FuelSurchargeIndexes.FirstOrDefaultAsync(t => t.Id == fuelSurchargeIndexId);
                if (fsIndex != null)
                {
                    viewModel.SurchargePricingType = (FuelSurchagePricingType)fsIndex.FuelSurchargePeriod.Value;
                    viewModel.SurchargeProductType = (SurchargeProductTypes)fsIndex.FuelSurchargeProduct.Value;
                    viewModel.FuelSurchageArea = (FuelSurchageArea)fsIndex.FuelSurchargeArea.Value;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelSurchargeDomain", "GetFuelSurchargeTableForAutoFreightMethod", ex.Message, ex);
            }

            return viewModel;
        }

        public HistoricalPriceViewModel GetHistoricalPrice(int fuelSurchargeIndexId, int forPeriod)
        {
            var response = new HistoricalPriceViewModel();
            var fuelSurchargeIndex = Context.DataContext.FuelSurchargeIndexes.FirstOrDefault(t => t.Id == fuelSurchargeIndexId);
            if (fuelSurchargeIndex != null)
            {
                response.IndexType = (int)fuelSurchargeIndex.IndexType;
                response.IndexTypeName = fuelSurchargeIndex.IndexType.GetDisplayName();
                if (fuelSurchargeIndex.IndexType == IndexType.API)
                {
                    response.IndexPeriod = fuelSurchargeIndex.FuelSurchargePeriod.Name;
                    response.IndexArea = fuelSurchargeIndex.FuelSurchargeArea.Name;
                    response.IndexProduct = fuelSurchargeIndex.FuelSurchargeProduct.Name;

                    if (fuelSurchargeIndex.FuelSurchargePeriod.Value == (int)FuelSurchagePricingType.Weekly)
                    {
                        response.PeriodName = "Weeks";
                    }
                    else if (fuelSurchargeIndex.FuelSurchargePeriod.Value == (int)FuelSurchagePricingType.Monthly)
                    {
                        response.PeriodName = "Months";
                    }
                    else if (fuelSurchargeIndex.FuelSurchargePeriod.Value == (int)FuelSurchagePricingType.Daily)
                    {
                        response.PeriodName = "Days";
                    }
                    else if (fuelSurchargeIndex.FuelSurchargePeriod.Value == (int)FuelSurchagePricingType.Annualy)
                    {
                        response.PeriodName = "Years";
                    }

                    var surchargePricingType = (FuelSurchagePricingType)fuelSurchargeIndex.FuelSurchargePeriod.Value;
                    var surchargeProductType = (SurchargeProductTypes)fuelSurchargeIndex.FuelSurchargeProduct.Value;
                    var surchargeArea = (FuelSurchageArea)fuelSurchargeIndex.FuelSurchargeArea.Value;

                    var listHistoricalPriceDetails = new List<HistoricalPriceDetailsViewModel>();
                    var eiaPrices = ContextFactory.Current.GetDomain<StoredProcedureDomain>()
                                      .GetFuelSurchargePriceHistory(forPeriod, surchargePricingType, surchargeProductType, surchargeArea);
                    foreach (var item in eiaPrices)
                    {
                        var historicalPriceDetails = new HistoricalPriceDetailsViewModel();
                        historicalPriceDetails.PublishDate = item.PublishDate.ToShortDateString();
                        historicalPriceDetails.Price = item.Price.GetPreciseValue(3).ToString();
                        listHistoricalPriceDetails.Add(historicalPriceDetails);
                    }
                    response.HistoricalPriceDetails = listHistoricalPriceDetails;
                }
                else
                {
                    response.ManualIndexPriceDate = fuelSurchargeIndex.IndexPriceDate.ToString(Resource.constFormatDate);
                    response.ManualIndexPrice = fuelSurchargeIndex.IndexPrice.GetPreciseValue(3).ToString();
                }
            }
            return response;
        }

        public async Task<StatusViewModel> CreateFuelSurchargeTableAsync(CreateFuelSurchargeInputViewModel viewModel, int userId, int companyId)
        {
            var response = new StatusViewModel(Status.Success);
            ValidateGivenDateRange(viewModel, companyId, response);
            if (response.StatusCode == Status.Failed)
            {
                return response;
            }
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    if (!viewModel.BuyerCompanyIds.Any())
                    {
                        viewModel.BuyerCompanyIds.Add(0);
                    }

                    List<FuelSurchargeTable> fuelSurchargeTables = new List<FuelSurchargeTable>();
                    foreach (var buyerCompany in viewModel.BuyerCompanyIds)
                    {
                        int? buyerCompanyId = buyerCompany == 0 ? (int?)null : buyerCompany;
                        DateTimeOffset? surchargeEndDate = null;
                        if (viewModel.SurchargeTable.Any())
                        {
                            if (viewModel.EndDate == null)
                            {
                                surchargeEndDate = Context.DataContext.FuelSurchargeTables.Where(t => t.TableType == viewModel.TableType && t.SupplierCompanyId == companyId && t.BuyerCompanyId == buyerCompanyId && t.StartDate > viewModel.StartDate).OrderBy(t => t.StartDate).Select(t => t.StartDate).FirstOrDefault();
                                if (surchargeEndDate == DateTimeOffset.MinValue)
                                {
                                    surchargeEndDate = null;
                                }
                                else if (surchargeEndDate.HasValue)
                                {
                                    surchargeEndDate = surchargeEndDate.Value.AddDays(-1);
                                }
                            }
                            var recordsToUpdateEndDate = Context.DataContext.FuelSurchargeTables.Where(t => t.TableType == viewModel.TableType && t.SupplierCompanyId == companyId && t.BuyerCompanyId == buyerCompanyId && t.StartDate < viewModel.StartDate && t.EndDate == null).ToList();
                            recordsToUpdateEndDate.ForEach(t => t.EndDate = viewModel.StartDate.AddDays(-1));
                        }
                        foreach (var row in viewModel.SurchargeTable)
                        {
                            FuelSurchargeTable fuelSurchargeTable = new FuelSurchargeTable();
                            fuelSurchargeTable.PriceRangeStartValue = row.PriceRangeStartValue;
                            fuelSurchargeTable.PriceRangeEndValue = row.PriceRangeEndValue;
                            fuelSurchargeTable.FuelSurchargePercentage = row.FuelSurchargeStartPercentage;
                            fuelSurchargeTable.BuyerCompanyId = buyerCompanyId;
                            fuelSurchargeTable.EndDate = viewModel.EndDate == null ? surchargeEndDate : viewModel.EndDate;
                            fuelSurchargeTables.Add(fuelSurchargeTable);
                        }
                    }
                    fuelSurchargeTables.ForEach(t =>
                    {
                        t.TableType = viewModel.TableType;
                        t.ProductType = viewModel.ProductType;
                        t.StartDate = viewModel.StartDate;
                        t.SupplierCompanyId = companyId;
                        t.CreatedBy = userId;
                        t.UpdatedBy = userId;
                        t.CreatedDate = DateTimeOffset.Now;
                        t.UpdatedDate = DateTimeOffset.Now;
                        t.IsActive = true;
                    });
                    Context.DataContext.FuelSurchargeTables.AddRange(fuelSurchargeTables);
                    await Context.CommitAsync();
                    transaction.Commit();

                    response.StatusMessage = Resource.errMessageCreateSurchargeTableSuccess;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("FuelSurchargeDomain", "CreateFuelSurchargeTableAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusViewModel> EditSurchargeTable(List<FuelSurchargeTableViewModel> viewModel, int companyId, int userId)
        {
            var response = new StatusViewModel(Status.Success);
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var surchargeIds = viewModel.Select(t => t.Id).ToList();
                    var surcharges = await Context.DataContext.FuelSurchargeTables.Where(t => surchargeIds.Contains(t.Id) && t.SupplierCompanyId == companyId).ToListAsync();
                    foreach (var surcharge in surcharges)
                    {
                        var surchargePercent = viewModel.Where(t => t.Id == surcharge.Id).Select(t => t.FuelSurchargeStartPercentage).FirstOrDefault();
                        if (surchargePercent != surcharge.FuelSurchargePercentage)
                        {
                            surcharge.FuelSurchargePercentage = surchargePercent;
                            surcharge.UpdatedBy = userId;
                            surcharge.UpdatedDate = DateTimeOffset.Now;
                        }
                    }
                    await Context.CommitAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("FuelSurchargeDomain", "EditSurchargeTable", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<List<FuelSurchargeGridViewModel>> GetFuelSurchargeSummary(ViewFuelSurchargeInputViewModel input, int customerCompanyId, int supplierCompanyId)
        {
            var response = new List<FuelSurchargeGridViewModel>();
            try
            {
                var fuelSurcharges = await Context.DataContext.FuelSurchargeTables.Where(t => (t.SupplierCompanyId == null || t.SupplierCompanyId == supplierCompanyId)
                                                                                                && (t.BuyerCompanyId == null || t.BuyerCompanyId == customerCompanyId)
                                                                                                && ((input.EndDate == null && (t.EndDate == null || t.EndDate >= input.StartDate))
                                                                                                || (input.EndDate.HasValue && (t.EndDate == null || (t.EndDate >= input.StartDate && t.EndDate <= input.EndDate))))
                                                                                                && (input.ProductTypes.Contains(t.ProductType)
                                                                                                && (!input.TableTypes.Any() || input.TableTypes.Contains(t.TableType)))
                                                                                                && (input.PriceRangeStartValue == null || t.PriceRangeStartValue >= input.PriceRangeStartValue)
                                                                                                && (input.PriceRangeEndValue == null || t.PriceRangeEndValue <= input.PriceRangeEndValue)
                                                                                                && (input.FuelSurchargeStartPercentage == null || t.FuelSurchargePercentage >= input.FuelSurchargeStartPercentage)
                                                                                        ).GroupBy(t => new { t.TableType, t.BuyerCompanyId, t.SupplierCompanyId, t.StartDate, t.EndDate, t.ProductType }).ToListAsync();

                foreach (var fuelSurcharge in fuelSurcharges)
                {
                    var key = fuelSurcharge.Key;
                    var data = new FuelSurchargeGridViewModel();

                    data.TableType = key.TableType;
                    data.ProductType = key.ProductType.ToString();
                    data.StartDate = key.StartDate.ToString();
                    data.EndDate = key.EndDate != null ? key.EndDate.ToString() : null;
                    data.StartValue = Resource.constSymbolCurrency + fuelSurcharge.Min(t => t.PriceRangeStartValue).GetPreciseValue(6);
                    data.EndValue = Resource.constSymbolCurrency + fuelSurcharge.Max(t => t.PriceRangeEndValue).GetPreciseValue(6);
                    data.PriceInterval = (fuelSurcharge.Select(t => t.PriceRangeEndValue).FirstOrDefault() - fuelSurcharge.Select(t => t.PriceRangeStartValue).FirstOrDefault()).GetPreciseValue(6);
                    data.DateRange = key.StartDate.ToString(Resource.constFormatDate) + (key.EndDate.HasValue ? (" - " + key.EndDate.Value.ToString(Resource.constFormatDate)) : "");
                    data.SurchargePercentage = fuelSurcharge.Select(t => t.FuelSurchargePercentage).FirstOrDefault().GetPreciseValue(6);
                    response.Add(data);
                }
                if (input.PriceRangeInterval != null)
                {
                    response = response.Where(t => t.PriceInterval == input.PriceRangeInterval).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelSurchargeDomain", "GetFuelSurchargeSummary", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<FuelSurchargeGridViewModel>> GetFuelSurchargeSummaryNew(ViewFuelSurchargeInputViewModel input, int supplierCompanyId)
        {
            var response = new List<FuelSurchargeGridViewModel>();
            try
            {
                var storedProcedureDomain = new StoredProcedureDomain(this);
                var fuelSurchargeSummary = await storedProcedureDomain.GetFuelSurchargeSummary(input, supplierCompanyId);
                var fuelSurchageMapping = fuelSurchargeSummary.GroupBy(t => t.Id);
                foreach (var item in fuelSurchageMapping)
                {
                    var fuelSurcharge = item.FirstOrDefault();
                    var data = new FuelSurchargeGridViewModel();

                    data.Id = fuelSurcharge.Id;
                    data.TableName = fuelSurcharge.TableName;
                    data.TableTypeNew = fuelSurcharge.TableType.GetDisplayName();
                    data.StatusName = fuelSurcharge.StatusId.GetDisplayName();
                    if (fuelSurcharge.StatusId == FreightTableStatus.Archived)
                        data.IsArchived = true;
                    data.ProductType = fuelSurcharge.ProductType.ToString();
                    data.StartDate = fuelSurcharge.StartDate.ToString(Resource.constFormatDate);
                    data.EndDate = fuelSurcharge.EndDate != null ? fuelSurcharge.EndDate.Value.ToString(Resource.constFormatDate) : null;
                    data.DateRange = fuelSurcharge.StartDate.ToString(Resource.constFormatDate) + (fuelSurcharge.EndDate.HasValue ? (" - " + fuelSurcharge.EndDate.Value.ToString(Resource.constFormatDate)) : "");
                    data.IndexArea = !string.IsNullOrEmpty(fuelSurcharge.IndexArea) ? fuelSurcharge.IndexArea : Resource.lblHyphen;
                    data.IndexPeriod = !string.IsNullOrEmpty(fuelSurcharge.IndexPeriod) ? fuelSurcharge.IndexPeriod : Resource.lblHyphen;
                    data.IndexProduct = !string.IsNullOrEmpty(fuelSurcharge.IndexProduct) ? fuelSurcharge.IndexProduct : Resource.lblHyphen;
                    data.IndexType = fuelSurcharge.IndexType != IndexType.None ? fuelSurcharge.IndexType.GetDisplayName() : Resource.lblHyphen;

                    data.SourceRegion = Resource.lblHyphen;
                    var sourceRegions = Context.DataContext.FreightTableSourceRegions.Where(t => t.FuelSurchargeIndexId == fuelSurcharge.Id).Select(t => t.SourceRegion.Name).Distinct().ToList();
                    if (sourceRegions != null && sourceRegions.Count > 0)
                    {
                        data.SourceRegion = string.Join(", ", sourceRegions);
                    }

                    data.Customer = Resource.lblHyphen;
                    var customers = Context.DataContext.FreightTableCompanies.Where(t => t.FuelSurchargeIndexId == fuelSurcharge.Id && t.AssignedCompanyType == AssignedCompanyType.Customer).Select(t => t.AssignedCompany.Name).Distinct().ToList();
                    if (customers != null && customers.Count > 0)
                    {
                        data.Customer = string.Join(", ", customers);
                    }

                    data.Carrier = Resource.lblHyphen;
                    var carriers = Context.DataContext.FreightTableCompanies.Where(t => t.FuelSurchargeIndexId == fuelSurcharge.Id && t.AssignedCompanyType == AssignedCompanyType.Carrier).Select(t => t.AssignedCompany.Name).Distinct().ToList();
                    if (carriers != null && carriers.Count > 0)
                    {
                        data.Carrier = string.Join(", ", carriers);
                    }

                    if (fuelSurcharge.TableType == TableTypes.Master)
                    {
                        data.Carrier = Resource.lblHyphen;
                        data.Customer = Resource.lblHyphen;
                    }

                    data.Terminal = Resource.lblHyphen;
                    var terminals = Context.DataContext.FreightTablePickupLocations.Where(t => t.FuelSurchargeIndexId == fuelSurcharge.Id && t.TerminalId != null).Select(t => t.MstExternalTerminal.Name).Distinct().ToList();
                    if (terminals != null && terminals.Count > 0)
                    {
                        data.Terminal = string.Join(", ", terminals);
                    }

                    data.BulkPlant = Resource.lblHyphen;
                    var bulkPlants = Context.DataContext.FreightTablePickupLocations.Where(t => t.FuelSurchargeIndexId == fuelSurcharge.Id && t.BulkPlantId != null).Select(t => t.BulkPlantLocation.Name).Distinct().ToList();
                    if (bulkPlants != null && bulkPlants.Count > 0)
                    {
                        data.BulkPlant = string.Join(", ", bulkPlants);
                    }

                    response.Add(data);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelSurchargeDomain", "GetFuelSurchargeSummary", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<FuelSurchargeTableViewModel>> GetSurchargeTable(CreateFuelSurchargeInputViewModel input, int customerCompanyId, int supplierCompanyId)
        {
            var response = new List<FuelSurchargeTableViewModel>();
            try
            {
                var fuelSurcharges = await Context.DataContext.FuelSurchargeTables.Where(t => (t.SupplierCompanyId == null || t.SupplierCompanyId == supplierCompanyId)
                                                                                                && (t.BuyerCompanyId == null || t.BuyerCompanyId == customerCompanyId)
                                                                                                && t.StartDate == input.StartDate && t.EndDate == input.EndDate
                                                                                                && input.ProductType == t.ProductType
                                                                                                && input.TableType == t.TableType
                                                                                                && t.PriceRangeStartValue >= input.PriceRangeStartValue
                                                                                                && t.PriceRangeEndValue <= input.PriceRangeEndValue
                                                                                                && t.FuelSurchargePercentage >= input.FuelSurchargeStartPercentage
                                                                                        ).ToListAsync();

                FuelSurchargeTableViewModel data = null;

                foreach (var fuelSurcharge in fuelSurcharges)
                {
                    data = new FuelSurchargeTableViewModel();

                    data.PriceRangeStartValue = fuelSurcharge.PriceRangeStartValue.GetPreciseValue(6);
                    data.PriceRangeEndValue = fuelSurcharge.PriceRangeEndValue.GetPreciseValue(6);
                    data.FuelSurchargeStartPercentage = fuelSurcharge.FuelSurchargePercentage.GetPreciseValue(6);
                    data.PriceRangeInterval = data.PriceRangeEndValue - data.PriceRangeStartValue;
                    data.SupplierId = fuelSurcharge.SupplierCompanyId;
                    data.Id = fuelSurcharge.Id;
                    response.Add(data);
                }

                response = response.Take(response.Count - 1).Where(t => t.PriceRangeInterval == input.PriceRangeInterval).ToList();
                if (data != null)
                    response.Add(data);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelSurchargeDomain", "GetSurchargeTable", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<FuelSurchargeTableViewModel>> GetSurchargeTableNew(int fuelSurchargeIndexId, int supplierCompanyId)
        {
            var response = new List<FuelSurchargeTableViewModel>();
            try
            {
                var fuelSurcharges = await Context.DataContext.FuelSurchargeGeneratedTables.Where(t => t.FuelSurchargeIndexId == fuelSurchargeIndexId).ToListAsync();

                FuelSurchargeTableViewModel data = null;

                foreach (var fuelSurcharge in fuelSurcharges)
                {
                    data = new FuelSurchargeTableViewModel();

                    data.PriceRangeStartValue = fuelSurcharge.PriceRangeStartValue.GetPreciseValue(6);
                    data.PriceRangeEndValue = fuelSurcharge.PriceRangeEndValue.GetPreciseValue(6);
                    data.FuelSurchargeStartPercentage = fuelSurcharge.FuelSurchargePercentage.GetPreciseValue(6);
                    data.PriceRangeInterval = data.PriceRangeEndValue - data.PriceRangeStartValue;
                    data.SupplierId = supplierCompanyId;
                    data.Id = fuelSurcharge.Id;
                    response.Add(data);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelSurchargeDomain", "GetSurchargeTableNew", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> ArchiveFuelSurchargeTable(int fuelSurchargeIndexId, int userId)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                var fuelSurcharge = await Context.DataContext.FuelSurchargeIndexes.FirstOrDefaultAsync(t => t.Id == fuelSurchargeIndexId);
                if (fuelSurcharge != null)
                {
                    fuelSurcharge.StatusId = FreightTableStatus.Archived;
                    fuelSurcharge.UpdatedDate = DateTimeOffset.Now;
                    fuelSurcharge.UpdatedBy = userId;
                    Context.DataContext.Entry(fuelSurcharge).State = EntityState.Modified;
                    await Context.CommitAsync();

                    response.StatusCode = Status.Success;
                }
            }
            catch (Exception ex)
            {
                response.StatusMessage = Resource.errMessageUpdateFailed;
                LogManager.Logger.WriteException("FuelSurchargeDomain", "ArchiveFuelSurchargeTable", ex.Message, ex);
            }
            return response;
        }

        private decimal GetPriceStartValue(decimal endValue)
        {
            int precision = 0;
            var text = endValue.ToString(System.Globalization.CultureInfo.InvariantCulture);
            var decpoint = text.IndexOf('.');
            if (decpoint > 0)
                precision = text.Length - decpoint - 1;
            else
                precision = 1;
            int multiplyBy = Convert.ToInt16("1" + string.Concat(Enumerable.Repeat("0", precision)));
            var valueWithOutDecimal = endValue * multiplyBy;
            var nextValue = valueWithOutDecimal + 1;
            return nextValue / multiplyBy;

        }

        private void ValidateGivenDateRange(CreateFuelSurchargeInputViewModel viewModel, int companyId, StatusViewModel response)
        {
            if (viewModel.TableType == TableTypes.Master)
            {
                bool isMasterTableExists = Context.DataContext.FuelSurchargeTables.Any(t => t.SupplierCompanyId == companyId && t.IsActive && t.ProductType == viewModel.ProductType && t.TableType == TableTypes.Master
                && (t.StartDate == viewModel.StartDate || (t.StartDate <= viewModel.StartDate && t.EndDate >= viewModel.StartDate))
                && (viewModel.EndDate == null || t.StartDate <= viewModel.EndDate));
                if (isMasterTableExists)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageMasterTableAlreadyExists;
                }
            }
            else
            {
                List<string> customerNames = Context.DataContext.FuelSurchargeTables.Where(t => t.SupplierCompanyId == companyId && t.IsActive && t.ProductType == viewModel.ProductType && t.TableType == TableTypes.CustomerSpecific
                                                    && viewModel.BuyerCompanyIds.Contains(t.BuyerCompanyId ?? 0)
                                                    && (t.StartDate == viewModel.StartDate || (t.StartDate <= viewModel.StartDate && t.EndDate >= viewModel.StartDate))
                                                    && (viewModel.EndDate == null || t.StartDate <= viewModel.EndDate))
                                                    .Select(t => t.CustomerCompany.Name).Distinct().ToList();
                if (customerNames.Any())
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = string.Format(Resource.errMessageCustomerSpecificTableAlreadyExists, string.Join(", ", customerNames));
                }
            }
        }

        public FuelSurchargeTableViewModel GetFuelSurchargeRecordForEaiPrice(decimal eaiPrice, int supplierCompanyId, int buyerCompanyId, DateTimeOffset dropDate, SurchargeProductTypes productType)
        {
            var onlyDate = dropDate.Date;

            var surchargeTableRecords = Context.DataContext.FuelSurchargeTables.Where(t => t.IsActive
                                        && t.ProductType == productType
                                        && t.StartDate <= onlyDate && ((t.EndDate.HasValue && t.EndDate >= onlyDate) || !t.EndDate.HasValue)
                                        && t.PriceRangeStartValue <= eaiPrice && t.PriceRangeEndValue >= eaiPrice)
                                        .Select(t => new
                                        {
                                            t.SupplierCompanyId,
                                            t.BuyerCompanyId,
                                            t.PriceRangeStartValue,
                                            t.PriceRangeEndValue,
                                            t.FuelSurchargePercentage
                                        }).ToList();

            var record = surchargeTableRecords.SingleOrDefault(t => t.SupplierCompanyId == supplierCompanyId && t.BuyerCompanyId == buyerCompanyId);
            if (record == null)
            {
                record = surchargeTableRecords.SingleOrDefault(t => t.SupplierCompanyId == supplierCompanyId && t.BuyerCompanyId == null);
                if (record == null)
                    record = surchargeTableRecords.SingleOrDefault(t => t.SupplierCompanyId == null && t.BuyerCompanyId == null);
            }

            if (record != null)
            {
                var result = new FuelSurchargeTableViewModel();

                result.PriceRangeStartValue = record.PriceRangeStartValue;
                result.PriceRangeEndValue = record.PriceRangeEndValue;
                result.FuelSurchargeStartPercentage = record.FuelSurchargePercentage;
                return result;
            }
            return null;
        }

        public FuelSurchargeTableViewModel GetFuelSurchargeRecordForEaiPriceForAutoFreightMethod(decimal eaiPrice, int fuelSurchargeIndexId)
        {
            var record = Context.DataContext.FuelSurchargeGeneratedTables.FirstOrDefault(t => t.FuelSurchargeIndexId == fuelSurchargeIndexId
                                        && t.PriceRangeStartValue <= eaiPrice && t.PriceRangeEndValue >= eaiPrice);
            if (record != null)
            {
                var result = new FuelSurchargeTableViewModel();

                result.PriceRangeStartValue = record.PriceRangeStartValue;
                result.PriceRangeEndValue = record.PriceRangeEndValue;
                result.FuelSurchargeStartPercentage = record.FuelSurchargePercentage;
                return result;
            }
            return null;
        }

        public async Task<List<DropdownDisplayExtended>> GetTerminalsAndBulkPlantsAsync(List<int> sourceRegionIds)
        {
            List<DropdownDisplayExtended> response = null;
            try
            {
                //very trick situation , 
                //since terminal id and bulkplant id may be duplicate and angular2-multiselect control does work with duplicate Id
                //refer 1. "create-fuel-surcharge.component" and "FuelSurchargeDomain-CreateFuelSurchargeTableAsync,GetTerminalsAndBulkPlantsAsync" 
                //refer 2. "FuelSurchargeMapper-ToViewModel"
                //so have to managed to make unique in  Get and save functionality i.e BulkPlants_Id,Terminals_Id.   
                response = await Context.DataContext.SourceRegionPickupLocations
                                               .Include(t => t.MstExternalTerminal)
                                               .Include(t => t.BulkPlantLocation)
                                               .Where(t => sourceRegionIds.Contains(t.SourceRegionId))
                                               .Select(t => new DropdownDisplayExtended
                                               {
                                                   Id = t.TerminalId.HasValue && t.MstExternalTerminal.IsActive ? "Terminals_" + t.MstExternalTerminal.Id.ToString() : t.BulkPlantId.HasValue && t.BulkPlantLocation.IsActive ? "BulkPlants_" + t.BulkPlantLocation.Id.ToString() : string.Empty,
                                                   Name = t.TerminalId.HasValue && t.MstExternalTerminal.IsActive ? t.MstExternalTerminal.Name : t.BulkPlantId.HasValue && t.BulkPlantLocation.IsActive ? t.BulkPlantLocation.Name : string.Empty,
                                                   Code = t.TerminalId.HasValue && t.MstExternalTerminal.IsActive ? "Terminals" : t.BulkPlantId.HasValue && t.BulkPlantLocation.IsActive ? "Bulk Plants" : string.Empty,

                                               }).GroupBy(t => t.Name).Select(x => x.FirstOrDefault()).OrderByDescending(t => t.Code).ThenBy(t => t.Name).ToListAsync();
                if (response.Count > 0 && response.Where(r => r.Id == string.Empty || r.Name == string.Empty || r.Code == string.Empty).ToList().Count > 0)
                {
                    throw new Exception("terminal id and bulkplant id should not null at same time");
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelSurchargeDomain", "GetTerminalsAndBulkPlants", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<DropdownDisplayExtendedId>> GetSourceRegionAsync(SourceRegionInputViewModel sourceRegionInput, UserContext userContext)
        {
            List<DropdownDisplayExtendedId> response = null;

            try
            {
                if (sourceRegionInput != null && sourceRegionInput.TableType == (int)TableTypes.Master)
                {
                    response = await Context.DataContext.SourceRegions.Where(t => t.CompanyId == userContext.CompanyId && t.IsActive)
                                                        .Select(t => new DropdownDisplayExtendedId
                                                        {
                                                            Id = t.Id,
                                                            Name = t.Name
                                                        }).Distinct().OrderByDescending(t => t.Name).ToListAsync();
                }
                else if (sourceRegionInput != null && sourceRegionInput.TableType == (int)TableTypes.CarrierSpecific
                    && (sourceRegionInput.CustomerId == null || sourceRegionInput.CustomerId.Count == 0)) //for carrier only
                {

                    if (sourceRegionInput.CarrierId != null && sourceRegionInput.CarrierId.Any())
                    {
                        response = await Context.DataContext.SourceRegionCarriers.Where(t => t.SourceRegion.CompanyId == userContext.CompanyId
                                                            && sourceRegionInput.CarrierId.Contains(t.CarrierId)
                                                            && t.SourceRegion.IsActive)
                                                            .Select(t => new DropdownDisplayExtendedId
                                                            {
                                                                Id = t.SourceRegion.Id,
                                                                CodeId = t.CarrierId,
                                                                Name = t.SourceRegion.Name
                                                            }).ToListAsync();
                    }

                    return GetValidSourceRegions(response);


                }
                else if (sourceRegionInput != null && sourceRegionInput.TableType == (int)TableTypes.CustomerSpecific
                    && (sourceRegionInput.CarrierId == null || sourceRegionInput.CarrierId.Count == 0)
                    && sourceRegionInput.CustomerId != null && sourceRegionInput.CustomerId.Count > 0) // for customer only
                {
                    var requestPriceDetailIds = await GetRequestPriceDetailIdsForCustomer(sourceRegionInput.CustomerId);
                    if (requestPriceDetailIds != null && requestPriceDetailIds.Count > 0)
                    {
                        var lstSourceRegion = await GetSourceRegionForCustomers(requestPriceDetailIds);
                        if (lstSourceRegion != null && lstSourceRegion.Count > 0)
                        {
                            response = new List<DropdownDisplayExtendedId>();
                            foreach (var item in lstSourceRegion)
                            {
                                var sIds = item.Name.Split(',').Select(int.Parse).ToList();
                                response.AddRange(await Context.DataContext.SourceRegions.Where(t1 => sIds.Contains(t1.Id))
                                .Select(t => new DropdownDisplayExtendedId { Name = t.Name, Id = t.Id, CodeId = item.CodeId }).ToListAsync()); //item.CodeId
                            }
                            return response.Count > 0 ? GetValidSourceRegions(response) : null;
                        }
                    }

                }
                else if (sourceRegionInput != null
                    && (sourceRegionInput.CarrierId != null && sourceRegionInput.CarrierId.Count > 0)
                    && (sourceRegionInput.CustomerId != null && sourceRegionInput.CustomerId.Count > 0)) //for both
                {
                    List<DropdownDisplayExtendedId> response1 = null;
                    List<DropdownDisplayExtendedId> response2 = new List<DropdownDisplayExtendedId>();

                    response1 = await Context.DataContext.SourceRegionCarriers.Where(t => t.SourceRegion.CompanyId == userContext.CompanyId
                                                                && sourceRegionInput.CarrierId.Contains(t.CarrierId)
                                                                && t.SourceRegion.IsActive)
                                                                .Select(t => new DropdownDisplayExtendedId
                                                                {
                                                                    Id = t.SourceRegion.Id,
                                                                    CodeId = t.CarrierId,
                                                                    Name = t.SourceRegion.Name
                                                                }).ToListAsync();

                    if (response1 != null && response1.Count > 0)
                    {
                        var requestPriceDetailIds = await GetRequestPriceDetailIdsForCustomer(sourceRegionInput.CustomerId);
                        if (requestPriceDetailIds != null && requestPriceDetailIds.Count > 0)
                        {
                            var lstSourceRegion = await GetSourceRegionForCustomers(requestPriceDetailIds);
                            if (lstSourceRegion != null && lstSourceRegion.Count > 0)
                            { 
                                foreach (var item in lstSourceRegion)
                                {
                                    var sIds = item.Name.Split(',').Select(int.Parse).ToList();
                                    response2.AddRange(await Context.DataContext.SourceRegions.Where(t1 => sIds.Contains(t1.Id))
                                    .Select(t => new DropdownDisplayExtendedId { Name = t.Name, Id = t.Id, CodeId = item.CodeId }).ToListAsync()); //item.CodeId
                                }

                            }
                        }
                    }

                    if (response1 != null && response1.Count > 0 && response2 != null && response2.Count > 0)
                    {
                        response1.AddRange(response2);
                        response = response1;
                        return GetValidSourceRegions(response);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelSurchargeDomain", "GetSourceRegionAsync", ex.Message, ex);
            }
            
            return response;
        }

        private List<DropdownDisplayExtendedId> GetValidSourceRegions(List<DropdownDisplayExtendedId> response)
        {
            if (response != null && response.Count > 0)
            {
                var groups = response.GroupBy(orderDetail => orderDetail.CodeId)
                             .ToList();
                var validSRIds = groups.First().Select(t1 => t1.Id).ToList();
                foreach (var g in groups.Skip(0))
                {
                    var otherGroupItemIds = g.Select(t1 => t1.Id).ToList();
                    validSRIds = validSRIds.Intersect(otherGroupItemIds).ToList();
                }
                if (validSRIds.Count > 0)
                    return response.FindAll(t1 => validSRIds.Contains(t1.Id)).GroupBy(t2 => new { t2.Id, t2.Name }).Select(g => g.First()).OrderByDescending(t => t.Name).ToList();
            }
            return null;
        }

        private async Task<List<DropdownDisplayExtendedId>> GetRequestPriceDetailIdsForCustomer(List<int> customerIds)
        {
            List<DropdownDisplayExtendedId> response = null;
            try
            {
                if (customerIds.Any())
                {
                    response = await Context.DataContext.Orders.Where(t => customerIds.Contains(t.BuyerCompanyId)
                                    && t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open &&
                                    t.OrderAdditionalDetail.FreightPricingMethod == FreightPricingMethod.Auto)
                                    .Select(t => new DropdownDisplayExtendedId
                                    {
                                        CodeId = t.BuyerCompanyId,
                                        Id = t.FuelRequest.FuelRequestPricingDetail.RequestPriceDetailId
                                    }).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelSurchargeDomain", "GetSourceRegionForSelectedCustomer", ex.Message, ex);
            }

            if (response != null && response.Count > 0 && response.Select(t1 => t1.CodeId).Distinct().ToList().Count() == customerIds.Count())
            {
                return response;
            }
            return null;

        }


        private async Task<List<DropdownDisplayExtendedId>> GetSourceRegionForCustomers(List<DropdownDisplayExtendedId> requestPriceDetailIds)
        {
            var pricingDomain = new PricingServiceDomain(this);
            return await pricingDomain.GetSourceRegionForCustomers(requestPriceDetailIds);
        }

    }
}