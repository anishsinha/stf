using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Forcasting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class SalesDomain : FreightServiceApiDomain
    {
        public SalesDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public SalesDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task<List<SalesDataModel>> GetSalesDataAsync(int companyId, string regionId, string customerId, string jobId, int priority, int SelectedTab, bool isShowCarrierManaged = false, string carriers = "", bool isRetailJob = true, string inventoryCaptureType = "", bool isFromExchangeApiForDataExpose = false)
        {
            var response = new List<SalesDataModel>();
            try
            {
                List<CustomerJobsModel> allJobs = new List<CustomerJobsModel>();
                List<int> customerIds = new List<int>();
                List<int> jobIds = new List<int>();
                List<int> carrierIds = new List<int>();
                List<int> inventoryCaptureTypeIds = new List<int>();

                //STEP 1: Read all inputs 
                Parallel.Invoke(
                                (() =>
                                {
                                    if (!String.IsNullOrEmpty(inventoryCaptureType))
                                        inventoryCaptureTypeIds = inventoryCaptureType.Split(',').Select(int.Parse).ToList();
                                }),
                                (() =>
                                {
                                    if (!String.IsNullOrEmpty(customerId))
                                        customerIds = customerId.Split(',').Select(int.Parse).ToList();
                                }),
                                (() =>
                                {
                                    if (!String.IsNullOrEmpty(jobId))
                                        jobIds = jobId.Split(',').Select(int.Parse).ToList();
                                }),
                                (() =>
                                {
                                    if (!String.IsNullOrEmpty(carriers))
                                        carrierIds = carriers.Split(',').Select(int.Parse).ToList();
                                }));


                //STEP 2: Get all orders
                allJobs = await Context.DataContext.Orders.Where(t => t.AcceptedCompanyId == companyId
                                        && !t.BuyerCompany.IsDeleted
                                        && (customerId == "" || customerIds.Contains(t.BuyerCompanyId))
                                        && (jobId == "" || jobIds.Contains(t.FuelRequest.JobId))
                                        && (inventoryCaptureType == "" || inventoryCaptureTypeIds.Contains((int)t.FuelRequest.Job.InventoryDataCaptureType))
                                        && t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open
                                        && t.IsActive
                                        &&
                                        (
                                        (isRetailJob && t.FuelRequest.Job.JobXAssets.Any(x => x.Asset.Type == (int)AssetType.Tank && x.RemovedBy == null && x.RemovedDate == null))
                                        ||
                                        (!isRetailJob && t.FuelRequest.Job.JobXAssets.Any(x => x.Asset.Type == (int)AssetType.Asset && x.RemovedBy == null && x.RemovedDate == null))
                                        ))
                                        .Select(t => new CustomerJobsModel
                                        {
                                            CustomerId = t.BuyerCompanyId,
                                            CustomerName = t.BuyerCompany.Name,
                                            JobId = t.FuelRequest.JobId,
                                            JobAddress = t.FuelRequest.Job.Address,
                                            City = t.FuelRequest.Job.City,
                                            StateCode = t.FuelRequest.Job.MstState.Code,
                                            ZipCode = t.FuelRequest.Job.ZipCode,
                                            LocationTypeId = t.FuelRequest.Job.LocationType,
                                            TimeZoneName = t.FuelRequest.Job.TimeZoneName,
                                            LocationName = t.FuelRequest.Job.Name,
                                            InventoryDataCaptureType = t.FuelRequest.Job.InventoryDataCaptureType,
                                            LocationManagedType = (int)t.FuelRequest.Job.LocationManagedType
                                        })
                                        .Distinct()
                                        .ToListAsync();

                var alljobIds = allJobs.Select(t => t.JobId).ToList();
                var carrierJobIds = await Context.DataContext.JobCarrierDetails.Where(t => alljobIds.Contains(t.JobId)
                                                                                && t.IsActive
                                                                                && t.CreatedByCompanyId == companyId
                                                                                && (carriers == "" || carrierIds.Contains(t.CarrierCompanyId)))
                                                                                .Select(t => t.JobId).Distinct().ToListAsync();
                
                if (isShowCarrierManaged && allJobs != null)
                {
                    if (carrierJobIds != null && carrierJobIds.Any())
                    {
                        allJobs = allJobs.Where(t => carrierJobIds.Contains(t.JobId)).ToList();
                        //TODO: LINQ foreach and Parallel foreach to be compared for performance instead of normal C# foreach
                        Parallel.ForEach(allJobs, new ParallelOptions { MaxDegreeOfParallelism = 20 }, res =>
                        {
                            res.LocationManagedType = (int)LocationManagedType.FullyCarrierManaged;
                        });
                    }
                    else
                    {
                        allJobs = new List<CustomerJobsModel>();
                    }
                }
                else if (allJobs != null)
                {
                    //TODO: LINQ foreach and Parallel foreach to be compared for performance instead of normal C# foreach
                    Parallel.ForEach(allJobs, new ParallelOptions { MaxDegreeOfParallelism = 20 }, res =>
                   {
                       res.LocationManagedType = carrierJobIds.Contains(res.JobId) ? (int)LocationManagedType.FullyCarrierManaged : (int)LocationManagedType.NotSpecified;
                   });
                }

                if (isRetailJob)
                {
                    var reqData = new SalesDataRequestModel { RegionId = regionId, Priority = priority, CompanyId = companyId, Jobs = allJobs, SelectedTab = SelectedTab, isFromExchangeApiForDataExpose = isFromExchangeApiForDataExpose };

                    var respData = await GetSales(reqData);
                    if (respData != null && respData.StatusCode == Status.Success)
                    {
                        response = respData.SalesData;
                    }
                }
                else
                {
                    var allJobIds = allJobs.Select(t => t.JobId).Distinct().ToList();
                    if (priority == 0)
                    {
                        response = await GetSalesForNonRetailJob(companyId, allJobIds, regionId);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SalesDomain", "GetSalesDataAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<SalesDataModel>> GetSalesDataForExternalResourceAsync(int companyId, string regionId, string customerId,
                                                string jobId, int priority, int SelectedTab, string carriers = "", bool isRetailJob = true,
                                                string inventoryCaptureType = "", bool isFromExchangeApiForDataExpose = false,
                                                List<DropdownDisplayItem> supplierList = null)
        {
            var response = new List<SalesDataModel>();
            try
            {
                List<CustomerJobsModel> allJobs = new List<CustomerJobsModel>();
                List<int> customerIds = new List<int>();
                List<int> jobIds = new List<int>();
                List<int> carrierIds = new List<int>();
                List<int> inventoryCaptureTypeIds = new List<int>();
                if (!String.IsNullOrEmpty(inventoryCaptureType))
                    inventoryCaptureTypeIds = inventoryCaptureType.Split(',').Select(int.Parse).ToList();
                if (!String.IsNullOrEmpty(customerId))
                    customerIds = customerId.Split(',').Select(int.Parse).ToList();
                if (!String.IsNullOrEmpty(jobId))
                    jobIds = jobId.Split(',').Select(int.Parse).ToList();
                if (!String.IsNullOrEmpty(carriers))
                    carrierIds = carriers.Split(',').Select(int.Parse).ToList();

                allJobs = await Context.DataContext.Orders.Where(t => t.AcceptedCompanyId == companyId
                                         && !t.BuyerCompany.IsDeleted
                                         && (customerId == "" || customerIds.Contains(t.BuyerCompanyId))
                                         && (jobId == "" || jobIds.Contains(t.FuelRequest.JobId))
                                          && (inventoryCaptureType == "" || inventoryCaptureTypeIds.Contains((int)t.FuelRequest.Job.InventoryDataCaptureType))
                                         && t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open
                                         && t.IsActive
                                         &&
                                         (
                                            (isRetailJob && t.FuelRequest.Job.JobXAssets.Any(x => x.Asset.Type == (int)AssetType.Tank && x.RemovedBy == null && x.RemovedDate == null))
                                            ||
                                            (!isRetailJob && t.FuelRequest.Job.JobXAssets.Any(x => x.Asset.Type == (int)AssetType.Asset && x.RemovedBy == null && x.RemovedDate == null))
                                         ))
                                        .Select(t => new CustomerJobsModel
                                        {
                                            CustomerId = t.BuyerCompanyId,
                                            CustomerName = t.BuyerCompany.Name,
                                            JobId = t.FuelRequest.JobId,
                                            JobAddress = t.FuelRequest.Job.Address,
                                            City = t.FuelRequest.Job.City,
                                            StateCode = t.FuelRequest.Job.MstState.Code,
                                            ZipCode = t.FuelRequest.Job.ZipCode,
                                            LocationTypeId = t.FuelRequest.Job.LocationType,
                                            TimeZoneName = t.FuelRequest.Job.TimeZoneName,
                                            LocationName = t.FuelRequest.Job.Name,
                                            InventoryDataCaptureType = t.FuelRequest.Job.InventoryDataCaptureType,
                                            LocationManagedType = (int)t.FuelRequest.Job.LocationManagedType
                                        })
                                        .Distinct()
                                        .ToListAsync();

                if (isRetailJob)
                {
                    var reqData = new SalesDataRequestModel { RegionId = regionId, Priority = priority, CompanyId = companyId, Jobs = allJobs, SelectedTab = SelectedTab, isFromExchangeApiForDataExpose = isFromExchangeApiForDataExpose };

                    var respData = await GetSales(reqData);
                    if (respData != null && respData.StatusCode == Status.Success)
                    {
                        response = respData.SalesData;
                    }
                }
                else
                {
                    var allJobIds = allJobs.Select(t => t.JobId).Distinct().ToList();
                    if (priority == 0)
                    {
                        response = await GetSalesForNonRetailJob(companyId, allJobIds, regionId);
                    }
                }

                //USED only for unauthenticated page
                if (supplierList != null)
                {
                    foreach (var item in allJobs)
                    {
                        supplierList.Add(new DropdownDisplayItem() { Id = item.CustomerId, Name = item.CustomerName });
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SalesDomain", "GetSalesDataForExternalResourceAsync", ex.Message, ex);
            }
            return response;
        }

        private async Task<List<SalesDataModel>> GetSalesForNonRetailJob(int companyId, List<int> jobIds, string regionIds)
        {
            var respData = new List<SalesDataModel>();

            var freightServiceDomain = new FreightServiceDomain(this);
            var regions = new List<RegionJobsModel>();

            if (!string.IsNullOrEmpty(regionIds))
            {
                regions = await freightServiceDomain.GetRegionsForJobs(companyId, regionIds);
                if (regions != null && regions.Any())
                {
                    var regionJobIds = new List<int>();
                    regionJobIds = regions.SelectMany(t => t.Jobs).Select(t => t.Id).ToList();
                    jobIds = jobIds.Where(t => regionJobIds.Contains(t)).Distinct().ToList();
                }
                else
                {
                    jobIds = new List<int>();
                }
            }

            respData = await Context.DataContext.Jobs.Where(t => jobIds.Contains(t.Id)
                        && t.JobXAssets.Any(t1 => t1.Asset.Type == (int)AssetType.Asset && t1.RemovedBy == null && t1.RemovedDate == null))
                        .Select(t => new SalesDataModel
                        {
                            Assets = t.JobXAssets.Where(t1 => t1.Asset.Type == (int)AssetType.Asset && t1.RemovedBy == null && t1.RemovedDate == null).Select(t2 => t2.Asset.Name).ToList(),
                            CompanyId = t.CompanyId,
                            CompanyName = t.Company.Name,
                            SiteId = t.DisplayJobID,
                            Location = t.Name,
                            LocationName = t.Name,
                            StorageId = Resource.lblHyphen,
                            Status = Resource.lblHyphen,
                            TfxJobId = t.Id,
                            AvgSale = Resource.lblHyphen,
                            DaysRemaining = Resource.lblHyphen,
                            Inventory = Resource.lblHyphen,
                            LastDeliveredQuantity = Resource.lblHyphen,
                            LastDeliveryDate = Resource.lblHyphen,
                            PrevSale = Resource.lblHyphen,
                            Ullage = Resource.lblHyphen,
                            WeekAgoSale = Resource.lblHyphen,
                            LastReadingTime = Resource.lblHyphen,
                            InventoryDataCaptureType = t.InventoryDataCaptureType,
                        }).ToListAsync();

            if (respData != null && respData.Any())
            {
                respData.ForEach(t => { t.TankName = string.Join(",", t.Assets?.Select(t1 => t1).ToList()); t.InventoryDataCaptureTypeName = t.InventoryDataCaptureType.GetDisplayName(); });
            }
            return respData;
        }
        
        public async Task<List<LocationTankDetailsModel>> GetBuyerLocationsAsync(int companyId, string sjobId, bool isShowCarrierManaged = false, string carriers = "", string inventoryCaptureType = "")
        {
            var response = new List<LocationTankDetailsModel>();
            try
            {
                List<int> jobIds = new List<int>();
                List<int> carrierIds = new List<int>();
                List<int> InventoryCaptureTypeIdsList = new List<int>();
                if (!String.IsNullOrEmpty(sjobId))
                    jobIds = sjobId.Split(',').Select(int.Parse).ToList();
                if (!String.IsNullOrEmpty(carriers))
                    carrierIds = carriers.Split(',').Select(int.Parse).ToList();
                if (!String.IsNullOrEmpty(inventoryCaptureType))
                    InventoryCaptureTypeIdsList = inventoryCaptureType.Split(',').Select(int.Parse).ToList();

                response = await Context.DataContext.Jobs.Where(t => t.CompanyId == companyId && !string.IsNullOrEmpty(t.DisplayJobID)
                                                        && (sjobId == "" || jobIds.Contains(t.Id))
                                                         && (inventoryCaptureType == "" || InventoryCaptureTypeIdsList.Contains((int)t.InventoryDataCaptureType))
                                                        && t.IsActive && t.JobXAssets.Any(t1 => t1.Asset.Type == (int)AssetType.Tank && !t1.RemovedBy.HasValue) && t.IsRetailJob)
                                                    .Select(t => new LocationTankDetailsModel
                                                    {
                                                        JobId = t.Id,
                                                        SiteId = t.DisplayJobID,
                                                        LocationName = t.Name,
                                                        Tanks = t.JobXAssets.Where(t1 => t1.Asset.Type == (int)AssetType.Tank && !t1.RemovedBy.HasValue)
                                                            .Select(t1 => new TankDetailModel
                                                            {
                                                                Name = t1.Asset.Name,
                                                                StorageId = t1.Asset.AssetAdditionalDetail.Vendor,
                                                                TankId = t1.Asset.AssetAdditionalDetail.VehicleId
                                                            }).ToList()
                                                    })
                                                    .ToListAsync();
                if (isShowCarrierManaged && response != null)
                {
                    var allJobIds = response.Select(t => t.JobId).Distinct().ToList();
                    var carrierJobIds = await Context.DataContext.JobCarrierDetails.Where(t => allJobIds.Contains(t.JobId)
                                                                                    && t.IsActive
                                                                                    && (carriers == "" || carrierIds.Contains(t.CarrierCompanyId)))
                                                                                    .Select(t => t.JobId).Distinct().ToListAsync();
                    if (carrierJobIds != null && carrierJobIds.Any())
                    {
                        response = response.Where(t => carrierJobIds.Contains(t.JobId)).ToList();
                    }
                    else
                    {
                        response = new List<LocationTankDetailsModel>();
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SalesDomain", "GetLocationTanksAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<LocationTankDetailsModel>> GetLocationTanksAsync(int companyId, string regionIds, string customerId, bool isShowCarrierManaged = false, string carriers = "", string inventoryCaptureType = "")
        {
            var response = new List<LocationTankDetailsModel>();
            try
            {
                List<int> allJobsIds = new List<int>();
                var allJobs = new List<LocationTankDetailsModel>();
                List<int> customerIds = new List<int>();
                List<int> carrierIds = new List<int>();
                List<int> inventoryCaptureTypeIds = new List<int>();
                if (!string.IsNullOrEmpty(customerId))
                {
                    customerIds = customerId.Split(',').Select(t => Convert.ToInt32(t.Trim())).ToList();
                }
                if (!String.IsNullOrEmpty(carriers))
                    carrierIds = carriers.Split(',').Select(int.Parse).ToList();
                if (!String.IsNullOrEmpty(inventoryCaptureType))
                    inventoryCaptureTypeIds = inventoryCaptureType.Split(',').Select(int.Parse).ToList();

                allJobs = await Context.DataContext.Orders.Where(t => t.AcceptedCompanyId == companyId
                                      && !t.BuyerCompany.IsDeleted
                                      && (inventoryCaptureType == "" || inventoryCaptureTypeIds.Contains((int)t.FuelRequest.Job.InventoryDataCaptureType))
                                      && (customerId == "" || customerIds.Contains(t.BuyerCompanyId))
                                      && t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open
                                      && t.IsActive && t.FuelRequest.Job.IsRetailJob)
                                      .Select(t => new LocationTankDetailsModel
                                      {
                                          JobId = t.FuelRequest.JobId,
                                          LocationName = t.FuelRequest.Job.Name
                                      }).Distinct().ToListAsync();

                if (allJobs.Any())
                    allJobsIds = allJobs.Select(x => x.JobId).ToList();

                if (isShowCarrierManaged && allJobs != null)
                {
                    var carrierJobIds = await Context.DataContext.JobCarrierDetails.Where(t => allJobsIds.Contains(t.JobId)
                                                                                    && t.IsActive
                                                                                    && t.CreatedByCompanyId == companyId
                                                                                    && (carriers == "" || carrierIds.Contains(t.CarrierCompanyId)))
                                                                                    .Select(t => t.JobId).Distinct().ToListAsync();
                    if (carrierJobIds != null && carrierJobIds.Any())
                    {
                        allJobs = allJobs.Where(t => carrierJobIds.Contains(t.JobId)).ToList();
                    }
                    else
                    {
                        allJobsIds = new List<int>();
                    }
                }
                var reqData = new LocationTanksRequestModel { JobIds = allJobsIds, RegionIds = regionIds, CompanyId = companyId };
                var apiUrl = string.Format(ApplicationConstants.UrlGetLocationTanks);
                var respData = await ApiPostCall<LocationTanksResponseModel>(apiUrl, reqData);
                if (respData != null && respData.StatusCode == Status.Success)
                {
                    response = respData.LocationDetails;
                    if (response.Any())
                    {
                        foreach (var oLocation in response)
                        {
                            oLocation.LocationName = allJobs.Where(a => a.JobId == oLocation.JobId).Select(x => x.LocationName).FirstOrDefault();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SalesDomain", "GetLocationTanksAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<SalesGraphDataModel>> GetSalesDataForGraphAsync(int jobId, int noOfDays)
        {
            var response = new List<SalesGraphDataModel>();
            try
            {
                if (jobId > 0)
                {
                    var apiUrl = string.Format(ApplicationConstants.UrlGetSalesDataForGraph, jobId, noOfDays);
                    var respData = await ApiGetCall<SalesGraphRespDataModel>(apiUrl);
                    if (respData.StatusCode == Status.Success)
                    {
                        response = respData.Sales;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SalesDomain", "GetSalesDataForGraphAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<ForecastingTankViewModel>> GetForecastingTankDetails(int jobId, string tankId, string storageId, UserContext userContext)
        {
            var response = new List<ForecastingTankViewModel>();
            try
            {
                var jobInfo = Context.DataContext.Jobs.Where(t => t.Id == jobId).Select(t => new { t.UoM, t.SiteInstructions, t.TimeZoneName, t.LocationManagedType }).FirstOrDefault();
                if (jobInfo != null)
                {

                    var carrierCompany = Context.DataContext.JobCarrierDetails.Where(t => t.JobId == jobId && t.CarrierCompanyId != userContext.CompanyId && t.CreatedByCompanyId == userContext.CompanyId && t.IsActive).OrderByDescending(x => x.Id)
                                                                              .Select(t => new { CompanyId = t.CarrierCompanyId, t.Company.Name })
                                                                              .FirstOrDefault();
                    var locationManagedType = carrierCompany != null ? (int)LocationManagedType.FullyCarrierManaged : (int)LocationManagedType.NotSpecified;

                    var unit = jobInfo.UoM == UoM.Gallons ? "G" : "L";
                    var apiUrl = string.Format(ApplicationConstants.UrlGetForecastingTankDetails, jobId, tankId, storageId, unit, jobInfo.TimeZoneName);
                    response = await ApiGetCall<List<ForecastingTankViewModel>>(apiUrl);
                    if (response != null && response.Any())
                    {
                        response.ForEach(x => x.SiteInstructions = string.IsNullOrEmpty(jobInfo.SiteInstructions) ? "N/A" : jobInfo.SiteInstructions);
                        foreach (var res in response)
                        {
                            res.LocationManagedType = locationManagedType;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SalesDomain", "GetForecastingTankDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<ForecastingEstimatedUsageViewModel>> GetForecastingTankEstimatedUsageDetails(int jobId, string startDate, string endDate, string tankId, string storageId)
        {
            var response = new List<ForecastingEstimatedUsageViewModel>();
            try
            {
                var uoM = Context.DataContext.Jobs.Where(t => t.Id == jobId).Select(t => new { t.UoM }).FirstOrDefault().UoM;
                var unit = uoM == UoM.Gallons ? "G" : "L";
                var apiUrl = string.Format(ApplicationConstants.UrlGetForecastingTankEstimatedUsageDetails, jobId, startDate, endDate, tankId, storageId, unit);
                response = await ApiGetCall<List<ForecastingEstimatedUsageViewModel>>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SalesDomain", "GetForecastingTankEstimatedUsageDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<ForecastingInventoryViewModel>> GetForecastingTankInventoryDetails(int jobId, string tankId, string storageId)
        {
            var response = new List<ForecastingInventoryViewModel>();
            try
            {
                var uoM = Context.DataContext.Jobs.Where(t => t.Id == jobId).Select(t => new { t.UoM }).FirstOrDefault().UoM;
                var unit = uoM == UoM.Gallons ? "G" : "L";
                var apiUrl = string.Format(ApplicationConstants.UrlGetForecastingTankInventoryDetails, jobId, tankId, storageId, unit);
                response = await ApiGetCall<List<ForecastingInventoryViewModel>>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SalesDomain", "GetForecastingTankInventoryDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<ForecastingDeliveryViewModel>> GetForecastingTankDeliveryDetails(int jobId, string tankId, string storageId)
        {
            var response = new List<ForecastingDeliveryViewModel>();
            try
            {
                var uoM = Context.DataContext.Jobs.Where(t => t.Id == jobId).Select(t => new { t.UoM }).FirstOrDefault().UoM;
                var reqData = new { jobId = jobId, tankId = tankId, storageId = storageId, uOM = uoM };
                var apiUrl = string.Format(ApplicationConstants.UrlGetForecastingTankDeliveryDetails);
                response = await ApiPostCall<List<ForecastingDeliveryViewModel>>(apiUrl, reqData);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SalesDomain", "GetForecastingTankDeliveryDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<ForecastingExistingScheduleViewModel>> GetForecastingTankScheduleDetails(int jobId, string tankId, string storageId)
        {
            var response = new List<ForecastingExistingScheduleViewModel>();
            try
            {
                var uoM = Context.DataContext.Jobs.Where(t => t.Id == jobId).Select(t => new { t.UoM }).FirstOrDefault().UoM;
                var reqData = new { jobId = jobId, tankId = tankId, storageId = storageId, uOM = uoM };
                var apiUrl = string.Format(ApplicationConstants.UrlGetForecastingTankScheduleDetails);
                response = await ApiPostCall<List<ForecastingExistingScheduleViewModel>>(apiUrl, reqData);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SalesDomain", "GetForecastingTankScheduleDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<ForecastingTankDataForChartViewModel> GetForecastingTankDataForChart(int jobId, string startDtTm, string tankId = "", string storageId = "")
        {
            var response = new ForecastingTankDataForChartViewModel();
            try
            {
                var uoM = Context.DataContext.Jobs.Where(t => t.Id == jobId).Select(t => new { t.UoM }).FirstOrDefault().UoM;
                var reqData = new { jobId = jobId, tankId = tankId, storageId = storageId, uOM = uoM, startDtTm = startDtTm };
                var apiUrl = string.Format(ApplicationConstants.UrlGetForecastingTankDataForChart);
                response = await ApiPostCall<ForecastingTankDataForChartViewModel>(apiUrl, reqData);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SalesDomain", "GetForecastingTankDataForChart", ex.Message, ex);
            }
            return response;
        }
        public async Task<TankRetainInfo> CalculateTankRetainWindowInfo(TankRetainWindowInfo tankRetainWindowInfo)
        {
            var response = new TankRetainInfo();
            try
            {

                var apiUrl = string.Format(ApplicationConstants.UrlPostCalculateTankRetainWindowInfo);
                response = await ApiPostCall<TankRetainInfo>(apiUrl, tankRetainWindowInfo);
            }
            catch (Exception ex)
            {
                response = null;
                LogManager.Logger.WriteException("SalesDomain", "CalculateTankRetainWindowInfo", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<DeliveryDetailsModel>> GetExistingSchedulesAsync(int jobId, int productTypeId, int companyId)
        {
            var response = new List<DeliveryDetailsModel>();
            try
            {
                if (jobId > 0)
                {
                    var apiUrl = string.Format(ApplicationConstants.UrlGetExistingSchedules, jobId, productTypeId, companyId);
                    var respData = await ApiGetCall<DeliveryDetailsRespModel>(apiUrl);
                    if (respData.StatusCode == Status.Success)
                    {
                        var scheduledDeliveries = respData.DeliveryDetails.Where(t => t.TrackableScheduleId.HasValue && t.TrackableScheduleId.Value > 0).ToList();
                        if (scheduledDeliveries.Any())
                        {
                            var trIds = scheduledDeliveries.Select(t => t.TrackableScheduleId).ToList();
                            var delDetails = Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(t => trIds.Contains(t.Id))
                                                .Select(t => new
                                                {
                                                    t.Id,
                                                    t.ShiftStartDate,
                                                    t.StartTime,
                                                    t.EndTime,
                                                    Carrier = (t.CarrierId.HasValue ? t.Carrier.Name : Resource.lblHyphen),
                                                    t.User.FirstName,
                                                    t.User.LastName,
                                                    t.Quantity,
                                                    t.QuantityTypeId
                                                })
                                                .ToList();
                            foreach (var del in delDetails)
                            {
                                var delivery = new DeliveryDetailsModel
                                {
                                    TrackableScheduleId = del.Id,
                                    Quantity = del.Quantity,
                                    ScheduleDate = del.ShiftStartDate.ToString("MM/dd/yyyy"),
                                    ScheduleTime = $"{del.StartTime.GetTimeInAmPmFormat()} - {del.EndTime.GetTimeInAmPmFormat()}",
                                    Carrier = del.Carrier,
                                    Driver = $"{del.FirstName} {del.LastName}",
                                    QuantityTypeId = del.QuantityTypeId == null ? 0 : del.QuantityTypeId.Value,

                                };
                                if (delivery != null)
                                {
                                    var QuantityTypeId = del.QuantityTypeId == null ? 0 : del.QuantityTypeId.Value;
                                    if (QuantityTypeId == 0 || QuantityTypeId == 1)
                                    {
                                        delivery.QuantityTypeName = ScheduleQuantityType.Quantity.ToString();
                                    }
                                    else
                                    {
                                        delivery.QuantityTypeName = EnumHelperMethods.GetDisplayName((ScheduleQuantityType)QuantityTypeId);
                                    }
                                }
                                response.Add(delivery);
                            }
                        }

                        var unScheduledDeliveries = respData.DeliveryDetails.Where(t => !t.TrackableScheduleId.HasValue).ToList();
                        if (unScheduledDeliveries.Any())
                        {
                            foreach (var item in unScheduledDeliveries)
                            {
                                if (item.QuantityTypeId == 0 || item.QuantityTypeId == 1)
                                {
                                    item.QuantityTypeName = ScheduleQuantityType.Quantity.ToString();
                                }
                                else
                                {
                                    item.QuantityTypeName = EnumHelperMethods.GetDisplayName((ScheduleQuantityType)item.QuantityTypeId);
                                }
                            }
                            response.AddRange(unScheduledDeliveries);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SalesDomain", "GetExistingSchedulesAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> RaiseDeliveryRequest(RaiseDeliveryRequestInput raiseDelivery, UserContext userContext)
        {
            var response = new StatusViewModel();
            try
            {
                var freightServiceDomain = new FreightServiceDomain(this);
                var raiseDr = new List<RaiseDeliveryRequestInput> { raiseDelivery };
                response = await freightServiceDomain.RaiseDeliveryRequests(raiseDr, userContext);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SalesDomain", "RaiseDeliveryRequest", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<SalesDataModel>> GetBuyerSalesDataAsync(int companyId, string jobId, int priority, int SelectedTab, int CountryId = 0, bool isShowCarrierManaged = false, string carriers = "", string suppliers = "", bool isRetailJob = true, string InventoryCaptureTypeIds = "", bool isFromExchangeApiForDataExpose = false)
        {
            var response = new List<SalesDataModel>();
            try
            {
                List<CustomerJobsModel> allJobs = new List<CustomerJobsModel>();
                List<int> carrierIds = new List<int>();
                List<int> jobIds = new List<int>();
                List<int> supplierIds = new List<int>();
                List<int> inventoryCaptureIdList = new List<int>();
                int inventoryIdfromStr = 0;
                int jobIdfromStr = 0;
                int carrierIdfromStr = 0;

                if (!String.IsNullOrEmpty(InventoryCaptureTypeIds))
                    inventoryCaptureIdList = InventoryCaptureTypeIds.Split(',')
                                                .Where(t => int.TryParse(t, out inventoryIdfromStr)).Select(t => inventoryIdfromStr).ToList();

                if (!String.IsNullOrEmpty(jobId))
                    jobIds = jobId.Split(',').Where(t => int.TryParse(t, out jobIdfromStr)).Select(t => jobIdfromStr).ToList();

                if (!String.IsNullOrEmpty(carriers))
                    carrierIds = carriers.Split(',').Where(t => int.TryParse(t, out carrierIdfromStr)).Select(t => carrierIdfromStr).ToList();

                if (!String.IsNullOrEmpty(suppliers))
                {
                    supplierIds = suppliers.Split(',').Select(int.Parse).ToList();
                    allJobs = await Context.DataContext.Orders
                                              .Where(t => t.OrderXStatuses.FirstOrDefault(t2 => t2.IsActive).StatusId == (int)OrderStatus.Open
                                                  && t.BuyerCompanyId == companyId && supplierIds.Contains(t.AcceptedCompanyId)
                                                  && t.FuelRequest.Job.IsActive
                                                  && (jobId == "" || jobIds.Contains(t.FuelRequest.JobId))
                                                  && (InventoryCaptureTypeIds == "" || inventoryCaptureIdList.Contains((int)t.FuelRequest.Job.InventoryDataCaptureType))
                                                  && (CountryId == 0 || t.FuelRequest.Job.CountryId == CountryId))
                                               .Select(t => new CustomerJobsModel { JobId = t.FuelRequest.JobId, JobAddress = t.FuelRequest.Job.Address, City = t.FuelRequest.Job.City, StateCode = t.FuelRequest.Job.MstState.Code, ZipCode = t.FuelRequest.Job.ZipCode, LocationTypeId = t.FuelRequest.Job.LocationType, TimeZoneName = t.FuelRequest.Job.TimeZoneName, InventoryDataCaptureType = t.FuelRequest.Job.InventoryDataCaptureType, LocationName = t.FuelRequest.Job.Name })
                                 .Distinct()
                                 .ToListAsync();
                }
                else
                {
                    allJobs = await Context.DataContext.Jobs.Where(t => t.CompanyId == companyId
                                     && t.IsActive
                                     && (jobId == "" || jobIds.Contains(t.Id))
                                      && (InventoryCaptureTypeIds == "" || inventoryCaptureIdList.Contains((int)t.InventoryDataCaptureType))
                                     && t.IsActive
                                     && (CountryId == 0 || t.CountryId == CountryId))
                                     .Select(t => new CustomerJobsModel { JobId = t.Id, JobAddress = t.Address, City = t.City, StateCode = t.MstState.Code, ZipCode = t.ZipCode, LocationTypeId = t.LocationType, TimeZoneName = t.TimeZoneName, LocationName = t.Name, InventoryDataCaptureType = t.InventoryDataCaptureType })
                                     .Distinct()
                                     .ToListAsync();
                }

                var alljobIds = allJobs.Select(t => t.JobId).ToList();
                var carrierJobIds = await Context.DataContext.JobCarrierDetails.Where(t => alljobIds.Contains(t.JobId)
                                                                                && t.IsActive
                                                                                && (carriers == "" || carrierIds.Contains(t.CarrierCompanyId)))
                                                                                .Select(t => t.JobId).Distinct().ToListAsync();

                if (isShowCarrierManaged && allJobs != null)
                {
                    if (carrierJobIds != null && carrierJobIds.Any())
                    {
                        allJobs = allJobs.Where(t => carrierJobIds.Contains(t.JobId)).ToList();
                        foreach (var res in allJobs)
                        {
                            res.LocationManagedType = (int)LocationManagedType.FullyCarrierManaged;
                        }
                    }
                    else
                    {
                        allJobs = new List<CustomerJobsModel>();
                    }
                }
                else if (allJobs != null)
                {
                    foreach (var res in allJobs)
                    {
                        res.LocationManagedType = carrierJobIds.Contains(res.JobId) ? (int)LocationManagedType.FullyCarrierManaged : (int)LocationManagedType.NotSpecified;
                    }
                }
                if (isRetailJob)
                {
                    var reqData = new SalesDataRequestModel { Priority = priority, Jobs = allJobs, CompanyId = companyId, SelectedTab = SelectedTab, isFromExchangeApiForDataExpose = isFromExchangeApiForDataExpose };
                    SalesDataResponseModel respData = await GetSales(reqData);
                    if (respData != null && respData.StatusCode == Status.Success)
                    {
                        response = respData.SalesData;
                    }
                }
                else
                {
                    var allJobIds = allJobs.Select(t => t.JobId).Distinct().ToList();
                    if (priority == 0)
                    {
                        response = await GetSalesForNonRetailJob(0, allJobIds, string.Empty);
                    }
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SalesDomain", "GetBuyerSalesDataAsync", ex.Message, ex);
            }
            return response;
        }
        private async Task<SalesDataResponseModel> GetSales(SalesDataRequestModel reqData)
        {
            var apiUrl = string.Format(ApplicationConstants.UrlGetSalesData);
            var respData = await ApiPostCall<SalesDataResponseModel>(apiUrl, reqData);
            return respData;
        }

        public async Task<InventoryDataResponseModel> GetInventoryDataForDashboard(InventoryDataViewModel model)
        {
            var apiUrl = string.Format(ApplicationConstants.UrlGetInventoryDataForDashboard);
            var respData = await ApiPostCall<InventoryDataResponseModel>(apiUrl, model);
            return respData;
        }

        public async Task<StatusViewModel> GetForecastingSetting(UserContext userContext)
        {
            var response = new StatusViewModel();
            response.IsForecatingAccountLevel = 0;
            try
            {
                var forcastingAccountDetails = await Context.DataContext.ForcastingPreferences.Where(top => top.SupplierCompanyId == userContext.CompanyId && top.ForcastingSettingLevel == (int)ForcastingSettingLevel.Account && top.IsActive && !top.IsDeleted).FirstOrDefaultAsync();
                if (forcastingAccountDetails != null)
                {
                    response.IsForecatingAccountLevel = 1;
                }
            }
            catch (Exception ex)
            {
                response.IsForecatingAccountLevel = -1;
                LogManager.Logger.WriteException("SalesDomain", "GetForecastingSeeting", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<LocationTankDetailsModel>> GetLocationTanksInfo(int companyId, string regionIds, string customerId, bool isShowCarrierManaged = false, string carriers = "", string inventoryCaptureType = "", bool isRateOfConsumption = false, UserContext userContext = null)
        {
            var response = new List<LocationTankDetailsModel>();
            try
            {
                List<int> allJobs = new List<int>();
                List<int> customerIds = new List<int>();
                List<int> carrierIds = new List<int>();
                List<int> buyerCompanyIds = new List<int>();
                List<int> inventoryCaptureTypeIds = new List<int>();
                if (!string.IsNullOrEmpty(customerId))
                {
                    customerIds = customerId.Split(',').Select(t => Convert.ToInt32(t.Trim())).ToList();
                }
                if (!String.IsNullOrEmpty(carriers))
                    carrierIds = carriers.Split(',').Select(int.Parse).ToList();
                if (!String.IsNullOrEmpty(inventoryCaptureType))
                    inventoryCaptureTypeIds = inventoryCaptureType.Split(',').Select(int.Parse).ToList();

                var allJobsInfo = await Context.DataContext.Orders.Where(t => t.AcceptedCompanyId == companyId
                                        && !t.BuyerCompany.IsDeleted
                                         && (string.IsNullOrEmpty(inventoryCaptureType) || inventoryCaptureTypeIds.Contains((int)t.FuelRequest.Job.InventoryDataCaptureType))
                                        && (string.IsNullOrEmpty(customerId) || customerIds.Contains(t.BuyerCompanyId))
                                        && t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open
                                        && t.IsActive)
                                        .Select(t => new
                                        {
                                            t.FuelRequest.JobId,
                                            CustomerName = t.BuyerCompany.Name,
                                            t.BuyerCompanyId,
                                            t.FuelRequest.Job.Name,
                                            t.FuelRequest.Job.LocationManagedType
                                        })
                                        .ToListAsync();

                if (allJobsInfo.Any())
                {
                    buyerCompanyIds = allJobsInfo.Select(top => top.BuyerCompanyId).Distinct().ToList();
                    allJobs = allJobsInfo.Select(top => top.JobId).Distinct().ToList();
                }
                var customerIdInfo = await Context.DataContext.SupplierXBuyerSettings.
                                          Where(t => t.SupplierCompanyId == companyId && buyerCompanyIds.Contains(t.BuyerCompanyId)).
                                          Select(t => new { t.SupplierCompanyId, t.BuyerCompanyId, t.CustomerId }).ToListAsync();

                var carrierJobIds = await Context.DataContext.JobCarrierDetails.Where(t => allJobs.Contains(t.JobId)
                                                                                  && t.IsActive
                                                                                  && t.CreatedByCompanyId == companyId
                                                                                  && (carriers == "" || carrierIds.Contains(t.CarrierCompanyId)))
                                                                                  .Select(t => t.JobId).Distinct().ToListAsync();
                if (isShowCarrierManaged && allJobs != null)
                {
                    if (carrierJobIds != null && carrierJobIds.Any())
                    {
                        allJobs = allJobs.Where(t => carrierJobIds.Contains(t)).ToList();
                    }
                    else
                    {
                        allJobs = new List<int>();
                    }
                }
                var reqData = new LocationTanksRequestModel { JobIds = allJobs, RegionIds = regionIds, IsRateOfConsumption = isRateOfConsumption, CompanyId = companyId };
                var apiUrl = string.Format(ApplicationConstants.UrlGetLocationTanks);
                var respData = await ApiPostCall<LocationTanksResponseModel>(apiUrl, reqData);
                if (respData != null && respData.LocationDetails != null && respData.LocationDetails.Any())
                {
                    var noDaysRemainingInfo = respData.LocationDetails.Where(top => top.DaysRemaining == null).ToList();
                    response = respData.LocationDetails.Where(top => top.DaysRemaining != null).OrderBy(top => top.DaysRemaining).ToList();
                    if (noDaysRemainingInfo.Any())
                    {
                        response.AddRange(noDaysRemainingInfo);
                    }

                    foreach (var item in response)
                    {
                        var customerInfo = allJobsInfo.FirstOrDefault(top => top.JobId == item.JobId);

                        if (customerInfo != null)
                        {
                            item.LocationName = customerInfo.Name;
                            if (carrierJobIds != null && carrierJobIds.Any())
                                item.LocationManagedType = carrierJobIds.Contains(item.JobId) ? (int)LocationManagedType.FullyCarrierManaged : (int)LocationManagedType.NotSpecified;
                            else
                            {

                                var carrierCompany = Context.DataContext.JobCarrierDetails.Where(t => t.JobId == item.JobId && t.CarrierCompanyId != userContext.CompanyId && t.CreatedByCompanyId == userContext.CompanyId && t.IsActive).OrderByDescending(x => x.Id)
                                                                                            .Select(t => new { CompanyId = t.CarrierCompanyId, t.Company.Name })
                                                                                            .FirstOrDefault();
                                if (carrierCompany != null)
                                {
                                    item.LocationManagedType = (int)LocationManagedType.FullyCarrierManaged;
                                }
                            }
                            if (customerInfo.BuyerCompanyId > 0)
                            {
                                var customerBrandId = customerIdInfo.Where(t => t.BuyerCompanyId == customerInfo.BuyerCompanyId && t.SupplierCompanyId == companyId).Select(t => t.CustomerId).FirstOrDefault();
                                if (!string.IsNullOrEmpty(customerBrandId))
                                {
                                    item.CustomerInfo = customerBrandId;
                                }
                                else
                                {
                                    item.CustomerInfo = customerInfo.CustomerName;
                                }

                            }
                            else
                            {
                                item.CustomerInfo = customerInfo.CustomerName;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SalesDomain", "GetLocationTanksInfo", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<LocationTankDetailsModel>> GetBuyerLocationTanksInfo(int companyId, string regionIds, string customerId, bool isShowCarrierManaged = false, string carriers = "", string inventoryCaptureType = "", bool isRateOfConsumption = false)
        {
            var response = new List<LocationTankDetailsModel>();
            try
            {
                List<int> allJobs = new List<int>();
                List<int> customerIds = new List<int>();
                List<int> carrierIds = new List<int>();
                List<int> buyerCompanyIds = new List<int>();
                List<int> inventoryCaptureTypeIds = new List<int>();
                if (!string.IsNullOrEmpty(customerId))
                {
                    customerIds = customerId.Split(',').Select(t => Convert.ToInt32(t.Trim())).ToList();
                }
                if (!String.IsNullOrEmpty(carriers))
                    carrierIds = carriers.Split(',').Select(int.Parse).ToList();
                if (!String.IsNullOrEmpty(inventoryCaptureType))
                    inventoryCaptureTypeIds = inventoryCaptureType.Split(',').Select(int.Parse).ToList();

                var allJobsInfo = await Context.DataContext.Jobs.Where(t => t.CompanyId == companyId && !string.IsNullOrEmpty(t.DisplayJobID)
                                                           && (string.IsNullOrEmpty(inventoryCaptureType) || inventoryCaptureTypeIds.Contains((int)t.InventoryDataCaptureType))
                                                          && t.IsActive && t.JobXAssets.Any(t1 => t1.Asset.Type == (int)AssetType.Tank && !t1.RemovedBy.HasValue) && t.IsRetailJob)
                                                      .Select(t => new
                                                      {
                                                          JobId = t.Id,
                                                          SiteId = t.DisplayJobID,
                                                          LocationName = t.Name,
                                                          CustomerName = t.Company.Name,
                                                          BuyerCompanyId = t.Company.Id,

                                                      })
                                                      .ToListAsync();

                if (allJobsInfo.Any())
                {
                    allJobs = allJobsInfo.Select(top => top.JobId).Distinct().ToList();
                }
                var customerIdInfo = await Context.DataContext.SupplierXBuyerSettings.
                                          Where(t => t.SupplierCompanyId == companyId && buyerCompanyIds.Contains(t.BuyerCompanyId)).
                                          Select(t => new { t.SupplierCompanyId, t.BuyerCompanyId, t.CustomerId }).ToListAsync();

                var carrierJobIds = await Context.DataContext.JobCarrierDetails.Where(t => allJobs.Contains(t.JobId)
                                                                                  && t.IsActive
                                                                                  // && t.CreatedByCompanyId == companyId
                                                                                  && (string.IsNullOrEmpty(carriers) || carrierIds.Contains(t.CarrierCompanyId)))
                                                                                  .Select(t => t.JobId).Distinct().ToListAsync();
                if (isShowCarrierManaged && allJobs != null)
                {
                    if (carrierJobIds != null && carrierJobIds.Any())
                    {
                        allJobs = allJobs.Where(t => carrierJobIds.Contains(t)).ToList();
                    }
                    else
                    {
                        allJobs = new List<int>();
                    }
                }
                var reqData = new LocationTanksRequestModel { JobIds = allJobs, RegionIds = regionIds, IsRateOfConsumption = isRateOfConsumption, CompanyId = companyId };
                var apiUrl = string.Format(ApplicationConstants.UrlGetLocationTanks);
                var respData = await ApiPostCall<LocationTanksResponseModel>(apiUrl, reqData);
                if (respData != null && respData.LocationDetails != null && respData.LocationDetails.Any())
                {
                    var noDaysRemainingInfo = respData.LocationDetails.Where(top => top.DaysRemaining == null).ToList();
                    response = respData.LocationDetails.Where(top => top.DaysRemaining != null).OrderBy(top => top.DaysRemaining).ToList();
                    if (noDaysRemainingInfo.Any())
                    {
                        response.AddRange(noDaysRemainingInfo);
                    }
                    foreach (var item in response)
                    {
                        var customerInfo = allJobsInfo.FirstOrDefault(top => top.JobId == item.JobId);
                        if (customerInfo != null)
                        {
                            item.LocationName = customerInfo.LocationName;
                            item.LocationManagedType = carrierJobIds.Contains(item.JobId) ? (int)LocationManagedType.FullyCarrierManaged : (int)LocationManagedType.NotSpecified;
                            if (customerInfo.BuyerCompanyId > 0)
                            {
                                var customerBrandId = customerIdInfo.Where(t => t.BuyerCompanyId == customerInfo.BuyerCompanyId && t.SupplierCompanyId == companyId).Select(t => t.CustomerId).FirstOrDefault();
                                if (!string.IsNullOrEmpty(customerBrandId))
                                {
                                    item.CustomerInfo = customerBrandId;
                                }
                                else
                                {
                                    item.CustomerInfo = customerInfo.CustomerName;
                                }

                            }
                            else
                            {
                                item.CustomerInfo = customerInfo.CustomerName;
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SalesDomain", "GetLocationTanksInfo", ex.Message, ex);
            }
            return response;
        }
    }
}
