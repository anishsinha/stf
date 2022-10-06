using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Dispatcher;
using SiteFuel.Exchange.ViewModels.Job;
using SiteFuel.Exchange.ViewModels.Tank;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class DispatcherDomain : FreightServiceApiDomain
    {
        public DispatcherDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public DispatcherDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task<List<WhereIsMyDriverViewModel>> GetOnGoingLoadsAsync(UserContext userContext, WhereIsMyDriverInputModel input)
        {
            List<WhereIsMyDriverViewModel> response = new List<WhereIsMyDriverViewModel>();
            try
            {
                var spDomain = new StoredProcedureDomain(this);
                var driverLocations = await spDomain.GetOnGoingLoadsAsync(userContext.CompanyId, input);
                response = driverLocations.Select(t => t.ToViewModel(null)).ToList();
                if (response.Any())
                {
                    List<int> driverIds = response.Select(t => t.Id).Distinct().ToList();
                    DateTime fDate = DateTimeOffset.Now.Date;
                    DateTime eDate = DateTimeOffset.Now.Date;
                    string startDate = DateTimeOffset.Now.Date.ToString("MM/dd/yyyy");
                    string endDate = DateTimeOffset.Now.Date.ToString("MM/dd/yyyy");
                    if (DateTime.TryParse(input.FromDate.ToString(), out fDate))
                    {
                        startDate = fDate.ToString("MM/dd/yyyy");

                    }
                    if (DateTime.TryParse(input.ToDate.ToString(), out eDate))
                    {
                        endDate = eDate.ToString("MM/dd/yyyy");
                    }
                    var retainRequets = new { driverIds = driverIds, fromDate = startDate, toDate = endDate };
                    
                    var retainDetails = await ApiPostCall<List<TrailerRetainDetails>>(ApplicationConstants.UrlGetTrailerRetainDetailsByDriverIds, retainRequets);
                    if (retainDetails != null && retainDetails.Count > 0)
                    {
                        response.ForEach(t =>
                        {
                            t.FuelRetainCount = retainDetails.Where(res => t.Id == res.DriverId).Sum(x => x.RetainFuelCount);
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DispatchDomain", "GetOnGoingLoadsAsync", ex.Message, ex);
            }
            return response;

        }

        public async Task<StatusViewModel> SaveFilters(int userId, TfxModule moduleId, string filterInput)
        {
            StatusViewModel response = new StatusViewModel();
            try
            {
                if (moduleId != TfxModule.DSBShift)
                {
                    await SaveUserFilter(userId, moduleId, filterInput);
                }
                else
                {
                    await SaveDSBShiftFilter(userId, moduleId, filterInput);
                }
                await Context.CommitAsync();
                response.StatusCode = Status.Success;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DispatchDomain", "SaveFilters", ex.Message, ex);
            }
            return response;
        }

        private async Task SaveDSBShiftFilter(int userId, TfxModule moduleId, string filterInput)
        {
            List<RegionDSBModel> regionDSBModel = new List<RegionDSBModel>();
            var existingFilter = await Context.DataContext.UserFilterSettings.Where(t => t.UserId == userId && t.ModuleId == moduleId).ToListAsync();
            if (existingFilter != null && existingFilter.Any())
            {
                foreach (var item in existingFilter)
                {
                    var existingFilterInfo = JsonConvert.DeserializeObject<RegionDSBModel>(item.Filter);
                    existingFilterInfo.Id = item.Id;
                    regionDSBModel.Add(existingFilterInfo);
                }
                var filterInputInfo = JsonConvert.DeserializeObject<RegionDSBModel>(filterInput);
                if (filterInputInfo != null)
                {
                    var regionDSBModelFirstOrDefault = regionDSBModel.FirstOrDefault(x => x.RegionId == filterInputInfo.RegionId);
                    if (regionDSBModelFirstOrDefault != null)
                    {
                        var existingFilterDetails = await Context.DataContext.UserFilterSettings.FirstOrDefaultAsync(t => t.UserId == userId && t.ModuleId == moduleId && t.Id == regionDSBModelFirstOrDefault.Id);
                        if (existingFilterDetails != null)
                        {
                            existingFilterDetails.Filter = filterInput;
                            existingFilterDetails.UpdatedDate = DateTimeOffset.Now;
                            Context.DataContext.Entry(existingFilterDetails).State = EntityState.Modified;
                        }

                    }
                    else
                    {
                        SaveUserFilterSettings(userId, moduleId, filterInput);
                    }
                }

            }
            else
            {
                SaveUserFilterSettings(userId, moduleId, filterInput);

            }
        }

        private void SaveUserFilterSettings(int userId, TfxModule moduleId, string filterInput)
        {
            UserFilterSetting filterSetting = new UserFilterSetting()
            {
                ModuleId = moduleId,
                UserId = userId,
                UpdatedDate = DateTimeOffset.Now,
                Filter = filterInput
            };
            Context.DataContext.UserFilterSettings.Add(filterSetting);
        }

        private async Task SaveUserFilter(int userId, TfxModule moduleId, string filterInput)
        {
            var existingFilter = await Context.DataContext.UserFilterSettings.FirstOrDefaultAsync(t => t.UserId == userId && t.ModuleId == moduleId);
            if (existingFilter != null)
            {
                existingFilter.Filter = filterInput;
                existingFilter.UpdatedDate = DateTimeOffset.Now;
                Context.DataContext.Entry(existingFilter).State = EntityState.Modified;
            }
            else
            {
                UserFilterSetting filterSetting = new UserFilterSetting()
                {
                    ModuleId = moduleId,
                    UserId = userId,
                    UpdatedDate = DateTimeOffset.Now,
                    Filter = filterInput
                };
                Context.DataContext.UserFilterSettings.Add(filterSetting);
            }
        }

        public async Task<string> GetFilters(int userId, TfxModule moduleId)
        {
            string response = string.Empty;
            try
            {
                var filter = await Context.DataContext.UserFilterSettings.FirstOrDefaultAsync(t => t.UserId == userId && t.ModuleId == moduleId);
                if (filter != null && !string.IsNullOrWhiteSpace(filter.Filter))
                {
                    response = filter.Filter;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DispatchDomain", "GetFilters", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<WhereIsMyDriverViewModel>> GetDispatcherLoadsAsync(UserContext userContext, WhereIsMyDriverInputModel input, DataTableSearchModel requestModel)
        {
            List<WhereIsMyDriverViewModel> response = new List<WhereIsMyDriverViewModel>();
            try
            {
                var spDomain = new StoredProcedureDomain(this);
                var driverLocations = await spDomain.GetDispatcherLoadsAsync(userContext.CompanyId, input, requestModel);
                response = driverLocations.Select(t => t.ToViewModel(null)).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DispatchDomain", "GetDriverLocationAsync", ex.Message, ex);
            }
            return response;
        }
        #region JobLocationDetails

        public async Task<JobLocationReponse> GetJobLocationDetails(int companyID, bool isBuyerCompany = false, string jobList = "", string inventoryCaptureTypeIds = "")
        {
            var response = new JobLocationReponse();
            try
            {
                var jobLocationDetailsViewModel = new List<JobLocationDetailsViewModel>();
                var storedProcedureDomain = new StoredProcedureDomain(this);

                if (isBuyerCompany)
                {
                    jobLocationDetailsViewModel = await storedProcedureDomain.GetJobLocationDetailsForBuyer(companyID, jobList, inventoryCaptureTypeIds);
                }
                else
                {
                    jobLocationDetailsViewModel = await storedProcedureDomain.GetJobLocationDetailsForSupplier(companyID, jobList, inventoryCaptureTypeIds);
                }

                if (jobLocationDetailsViewModel.Any())
                {
                    response.jobLocationDetails = jobLocationDetailsViewModel;
                    //call the api -- find job related details for particular job and companyId
                    var JobIdArray = jobLocationDetailsViewModel.Select(top => top.JobID).Distinct().ToList();
                    var JobIds = string.Join(",", JobIdArray);

                    var getJobRelationDetails = await GetJobRelatedInfo(companyID, jobLocationDetailsViewModel, JobIds, isBuyerCompany);

                    GetJobAssetDetails(jobLocationDetailsViewModel, JobIds, getJobRelationDetails);
                    var jobDomain = new JobDomain(this);
                    foreach (var item in jobLocationDetailsViewModel)
                    {
                        if (isBuyerCompany)
                        {
                            var result = jobDomain.GetSuppliersCarriersForJob(new List<int> { item.JobID }, companyID);
                            if (result.supplierDetails.Any())
                            {
                                item.supplierDetails = result.supplierDetails;
                                item.carrierDetails = result.carrierDetails;
                            }
                        }


                        //if (item.jobDeliveryRequests.Any())
                        //{
                        //    int scheduleCreatedCount = item.jobDeliveryRequests.Count(top => top.Status == Convert.ToInt32(DeliveryReqStatus.ScheduleCreated));
                        //    if (scheduleCreatedCount == item.jobDeliveryRequests.Count)
                        //        item.ScheduleStatus = "Scheduled";
                        //    else
                        //        item.ScheduleStatus = "Not Scheduled";
                        //}
                        //else
                        //{
                        //    item.ScheduleStatus = "NA";
                        //}
                        var tStatus = Resource.valMessageNoDR;
                        if (item.jobDeliveryRequests.Any())
                        {
                            if (item.jobDeliveryRequests.All(t => t.Status == Convert.ToInt32(DeliveryReqStatus.ScheduleCreated))) { tStatus = Resource.lblScheduled; }
                            else tStatus = Resource.valMessageDRCreated;
                        }
                        item.ScheduleStatus = tStatus;
                        item.TankCount = item.jobAssetDetails.Count(top => top.AssetType == Convert.ToInt32(AssetType.Tank));
                        item.AssetCount = item.jobAssetDetails.Count(top => top.AssetType == Convert.ToInt32(AssetType.Asset));
                        if (!response.citiesDetails.Any(top => top.Id == item.CityId))
                        {
                            response.citiesDetails.Add(new JobCities { Id = item.CityId, Name = item.City });
                        }
                        if (!response.stateDetails.Any(top => top.Id == item.StateID))
                        {
                            response.stateDetails.Add(new JobStates { Id = item.StateID, Name = item.State });
                        }
                        if (!response.customerDetails.Any(top => top.Id == item.CustomerId))
                        {
                            response.customerDetails.Add(new JobCustomers { Id = item.CustomerId, Name = item.CustomerName });
                        }
                        if (item.FuelTypeNameList != null)
                        {
                            foreach (var fuelitem in item.FuelTypeNameList)
                            {
                                if (!string.IsNullOrEmpty(fuelitem) && !response.fuelTypeDetails.Any(top => top.ToString() == fuelitem.ToString()))
                                {
                                    response.fuelTypeDetails.Add(fuelitem.ToString());
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DispatcherDomain", "GetJobLocationDetails", ex.Message, ex);
            }

            return response;
        }
        public async Task<List<DispatcherDashboardRegionModel>> GetDispatcherRegionsAsync(UserContext userContext)
        {
            var response = new List<DispatcherDashboardRegionModel>();
            try
            {
                var freightServiceDomain = new FreightServiceDomain(this);
                response = await freightServiceDomain.GetDispatcherRegionsAsync(userContext.CompanyId, 0);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DispatcherDomain", "GetDispatcherRegionsAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<JobDipChartDetails>> GetDipTestDetails(string siteId, string tankId, int noOfDays)
        {
            var freightServiceDomain = new FreightServiceDomain(this);
            var diptestDetails = await freightServiceDomain.GetDipTestDetails(siteId, tankId, noOfDays);
            return diptestDetails;
        }
        public async Task<List<JobDipChartDetails>> GetJobDemandCaptureChartData(DemandCaptureChartData demandCaptureChartData)
        {
            var freightServiceDomain = new FreightServiceDomain(this);
            var diptestDetails = await freightServiceDomain.GetJobDemandCaptureChartData(demandCaptureChartData);
            return diptestDetails;
        }

        private async Task<JobLocationRelatedDetailsViewModel> GetJobRelatedInfo(int companyID, List<JobLocationDetailsViewModel> response, string JobIds, bool isBuyerCompany)
        {
            var freightServiceDomain = new FreightServiceDomain(this);
            var getJobRelationDetails = await freightServiceDomain.GetJobLocationRelatedDetails(companyID, JobIds, isBuyerCompany);
            if (getJobRelationDetails != null)
            {
                GetJobDeliveryRequestDetails(response, getJobRelationDetails);
            }
            return getJobRelationDetails;
        }


        private static void GetJobDeliveryRequestDetails(List<JobLocationDetailsViewModel> response, JobLocationRelatedDetailsViewModel getJobRelationDetails)
        {
            var deliveryRequestDetails = getJobRelationDetails.deliveryRequestViewModels;
            if (deliveryRequestDetails.Any())
            {
                foreach (var item in deliveryRequestDetails)
                {
                    var recordFound = response.FirstOrDefault(top => top.JobID == item.JobId);
                    if (recordFound != null)
                    {
                        recordFound.jobDeliveryRequests.Add(new JobDeliveryRequestsViewModel { CreatedRegionId = item.CreatedByRegionId, Id = item.Id, Priority = item.Priority, RequiredQuantity = item.RequiredQuantity, Status = item.Status, StorageId = item.StorageId, StorageTypeId = item.StorageId, TfxProductType = item.ProductType, TfxUoM = item.UoM, TfxJobId = item.JobId });
                    }
                }
            }
        }

        private void GetJobAssetDetails(List<JobLocationDetailsViewModel> response, string JobIds, JobLocationRelatedDetailsViewModel getJobRelationDetails)
        {
            try
            {
                var jobinputmodel = new { @JobIds = JobIds };
                var jobinput = SqlHelperMethods.GetStoredProcedure("usp_GetJobLocation_AssetDetails", jobinputmodel);

                Context.DataContext.Database.CommandTimeout = 30;
                var jobAssetDetails = Context.DataContext.Database.SqlQuery<JobAssetDetailsViewModel>(jobinput.Query, jobinput.Params.ToArray()).ToList();
                foreach (var job in response)
                {
                    var additionJobDetails = getJobRelationDetails.jobAdditionalDetailsModels.FirstOrDefault(t => t.JobId == job.JobID);
                    if (additionJobDetails != null)
                    {
                        string siteAvailability = string.Empty;
                        if (additionJobDetails.DeliveryDaysList != null)
                        {
                            foreach (var deliveryitem in additionJobDetails.DeliveryDaysList)
                            {
                                if (string.IsNullOrEmpty(siteAvailability))
                                    siteAvailability = deliveryitem.FinalString;
                                else
                                    siteAvailability += "," + deliveryitem.FinalString;
                            }
                            job.SiteAvailabilityTotalDays = additionJobDetails.DeliveryDaysList.Count;
                        }
                        job.SiteAvailability = siteAvailability;
                        if (!string.IsNullOrEmpty(additionJobDetails.SiteImageFilePath))
                        {
                            var siteImageModel = new ImageViewModel { FilePath = additionJobDetails.SiteImageFilePath };
                            job.SiteImageFilePath = siteImageModel.GetAzureFilePath(BlobContainerType.JobFilesUpload);
                        }
                        var jobAssetitems = jobAssetDetails.Where(t => t.JobId == job.JobID).ToList();

                        foreach (var jobAssetitem in jobAssetitems)
                        {
                            if (additionJobDetails.TankDetails.Any())
                            {
                                var tankrecordFound = additionJobDetails.TankDetails.Where(top => top.AssetId == jobAssetitem.AssetId).ToList();
                                foreach (var itemTankDetails in tankrecordFound)
                                {
                                    jobAssetitem.jobTankAdditionalDetails.Add(new JobAssetTankDetailsViewModel
                                    {
                                        DipTestMethod = itemTankDetails.DipTestMethod,
                                        FillType = itemTankDetails.FillType,
                                        StorageId = itemTankDetails.StorageId,
                                        TfxAssetId = itemTankDetails.AssetId,
                                        TfxProductTypeName = itemTankDetails.ProductTypeName,
                                        ThresholdDeliveryRequest = itemTankDetails.ThresholdDeliveryRequest,
                                        ManiFolded = itemTankDetails.ManiFolded,
                                        CaptureTime = itemTankDetails.CaptureTime,
                                        LastReading = itemTankDetails.LastReading,
                                        dipChartDetails = itemTankDetails.dipChartDetails,
                                        SiteId = additionJobDetails.TfxDisplayJobId,
                                        TankId = itemTankDetails.TankId,
                                        TankName = itemTankDetails.TankName,
                                        TankNumber = itemTankDetails.TankNumber,
                                        FuelCapacity = itemTankDetails.FuelCapacity,
                                        MaxFill = itemTankDetails.MaxFill,
                                        MinFill = itemTankDetails.MinFill,
                                        MinFillPercent = itemTankDetails.MinFillPercent,
                                        MaxFillPercent = itemTankDetails.MaxFillPercent,
                                        TankChartPath = itemTankDetails.TankChartPath
                                    });
                                }
                            }
                            job.jobAssetDetails.Add(jobAssetitem);
                            job.RegionId = additionJobDetails.RegionId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DispatcherDomain", "GetJobAssetDetails", ex.Message, ex);
            }

        }
        #endregion

        public async Task<DriverAdditionalDetailModel> GetDriverAdditionalDetailsAsync(int driverId,UserContext userContext)
        {
            var response = await ContextFactory.Current.GetDomain<FreightServiceDomain>().GetDriverAdditionalDetailsAsync(driverId);
            if (response != null)
            {
                response.ContactNumnber = Context.DataContext.Users.Where(t => t.Id == driverId).Select(t => t.PhoneNumber).FirstOrDefault();
                if(response.Trailers.Any())
                {
                    var FsDomain = ContextFactory.Current.GetDomain<FreightServiceDomain>();
                    int UoM = await FsDomain.getUoMByCompany(userContext.CompanyId);
                    response.Trailers.ForEach(t => { t.DefaultUOM = UoM; });
                }
            }
            return response;
        }
        public List<DropdownDisplayItem> GetCarriersForSupplierDashboard(int SupplierCompanyId)
        {
            var carrierDetails = new List<DropdownDisplayItem>();
            try
            {
                var jobIds = Context.DataContext.Orders
                         .Where(t => t.AcceptedCompanyId == SupplierCompanyId && t.IsActive
                             && t.OrderXStatuses.FirstOrDefault(t2 => t2.IsActive).StatusId == (int)OrderStatus.Open
                             )
                         .Select(t => t.FuelRequest.JobId)
                         .Distinct()
                         .ToList();


                if (jobIds != null && jobIds.Any())
                {
                    carrierDetails = GetJobsCarriers(jobIds, SupplierCompanyId);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DispatcherDomain", "GetCarriersForSupplierDashboard", ex.Message, ex);
            }
            return carrierDetails;
        }
        private List<DropdownDisplayItem> GetJobsCarriers(List<int> jobIds, int SupplierCompanyId)
        {
            var response = new List<DropdownDisplayItem>();
            var carrierDetails = Context.DataContext.JobCarrierDetails.Where(t => jobIds.Contains(t.JobId)
                                                                                   && t.IsActive && t.CreatedByCompanyId == SupplierCompanyId)
                                    .Select(t => new { t.CarrierCompanyId, CompanyName = t.Company.Name }).Distinct().ToList();
            if (carrierDetails != null && carrierDetails.Any())
            {
                foreach (var carrier in carrierDetails)
                {
                    var carrierCompanyId = carrier.CarrierCompanyId;
                    var carrierCompanyName = carrier.CompanyName;
                    response.Add(new DropdownDisplayItem { Id = carrierCompanyId, Name = carrierCompanyName });
                }
            }
            return response;
        }
        public async Task<string> GetDSBShiftFilters(int userId, TfxModule moduleId,string regionId)
        {
            string response = string.Empty;
            try
            {
                var filter = await Context.DataContext.UserFilterSettings.FirstOrDefaultAsync(t => t.UserId == userId && t.ModuleId == moduleId && t.Filter.Contains(regionId));
                if (filter != null && !string.IsNullOrWhiteSpace(filter.Filter))
                {
                    response = filter.Filter;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DispatchDomain", "GetDSBShiftFilters", ex.Message, ex);
            }
            return response;
        }
    }
}
