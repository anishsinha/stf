using MongoDB.Bson;
using MongoDB.Driver;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.FreightModels.Dispatcher;
using SiteFuel.FreightRepository.Mappers;
using SiteFuel.MdbDataAccess.Collections;
using SiteFuel.MdbDataAccess.DbContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightRepository
{
    public class RegionRepository : IRegionRepository
    {
        private readonly MdbContext mdbContext;
        private readonly ScheduleBuilderRepository scheduleBuilderRepository;
        public RegionRepository()
        {
            if (mdbContext == null)
            {
                mdbContext = new MdbContext();
            }
        }
        public RegionRepository(MdbContext _context)
        {
            mdbContext = _context;
            scheduleBuilderRepository = new ScheduleBuilderRepository(mdbContext);
        }
        public async Task<bool> DeleteAllRegions()
        {
            var filter = MongoDB.Driver.FilterDefinition<Region>.Empty;
            await mdbContext.Regions.DeleteManyAsync(filter);
            return true;
        }

        public async Task<RegionResponseModel> CreateRegion(RegionViewModel model)
        {
            var response = new RegionResponseModel();
            try
            {
                //check for existing region with same regionname, dispatcher
                if (IsRegionExists(model))
                {
                    response.StatusCode = (int)Status.Failed;
                    response.StatusMessage = model.Name + " region already exists";
                    return response;
                }
                else
                {
                    var region = model.ToEntity();
                    await mdbContext.Regions.InsertOneAsync(region);
                    var result = await UpdateDriverAssignmentForRegion(region.TfxDrivers, region.Id.ToString(), region.TfxCompanyId);
                    if (result.StatusCode == (int)Status.Success)
                    {
                        response.RegionId = region.Id.ToString();
                        response.StatusCode = (int)Status.Success;
                    }
                    else
                    {
                        response.RegionId = region.Id.ToString();
                        response.StatusCode = (int)Status.Failed;
                        response.StatusMessage = "Failed to Assign Driver(s) to Region";
                    }
                    //response.RegionId = region.Id.ToString();
                    //response.StatusCode = (int)Status.Success;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionRepository", "CreateRegion - Name : " + model.Name, ex.Message, ex);
                response.StatusCode = (int)Status.Failed;
            }
            return response;
        }

        public bool IsJobExistsInOtherRegion(RegionViewModel model)
        {
            var response = false;
            if (model.Jobs == null || !model.Jobs.Any())
                return response;

            try
            {
                ObjectId.TryParse(model.Id, out ObjectId objId);
                var existingRegions = (from region in mdbContext.Regions.AsQueryable()
                                       where region.TfxCompanyId == model.CompanyId &&
                                              region.IsActive && !region.IsDeleted &&
                                              region.Id != objId &&
                                              region.TfxJobs != null
                                       select new
                                       {
                                           region.TfxJobs
                                       }).FirstOrDefault();

                if (existingRegions != null)
                {
                    var existingJobList = existingRegions.TfxJobs.Select(t => t.Id).ToList().Intersect(model.Jobs.Select(t1 => t1.Id).ToList());
                    if (existingJobList != null && existingJobList.Any())
                    {
                        response = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionRepository", "IsJobExistsInOtherRegion - Region Name : " + model.Name, ex.Message, ex);
            }
            return response;
        }
        public List<string> IsDriverAlreadyExists(RegionViewModel model)
        {
            List<string> response = new List<string>();

            try
            {
                List<int> modelDriverIds = new List<int>();
                if (model.Drivers != null)
                    modelDriverIds = model.Drivers.Select(top => top.Id).ToList();
                if (string.IsNullOrEmpty(model.Id))
                {
                    var result = (from region in mdbContext.Regions.AsQueryable()
                                  where region.TfxCompanyId == model.CompanyId &&
                                         region.IsActive && !region.IsDeleted &&
                                         region.TfxDrivers != null
                                  select new
                                  {
                                      region.Name,
                                      driverId = region.TfxDrivers.Select(top => top.Id)
                                  });
                    var driverDetails = result.ToList();
                    if (driverDetails != null)
                    {
                        foreach (var item in driverDetails)
                        {
                            var drivers = item.driverId.ToList();
                            var hasDuplicates = drivers.Intersect(modelDriverIds).Any();
                            if (hasDuplicates)
                            {
                                response.Add(item.Name);
                            }
                        }
                    }
                }
                else
                {
                    ObjectId objectId = new ObjectId(model.Id);
                    var result = (from region in mdbContext.Regions.AsQueryable()
                                  where region.TfxCompanyId == model.CompanyId &&
                                         region.IsActive && !region.IsDeleted &&
                                         region.TfxDrivers != null && region.Id != objectId
                                  select new
                                  {
                                      region.Name,
                                      driverId = region.TfxDrivers.Select(top => top.Id)
                                  });
                    var driverDetails = result.ToList();
                    if (driverDetails != null)
                    {
                        foreach (var item in driverDetails)
                        {
                            var drivers = item.driverId.ToList();
                            var hasDuplicates = drivers.Intersect(modelDriverIds).Any();
                            if (hasDuplicates)
                            {
                                response.Add(item.Name);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionRepository", "IsDriverAlreadyExists - Region Name : " + model.Name, ex.Message, ex);
            }
            return response;
        }

        private bool IsRegionExists(RegionViewModel model)
        {
            var isRegionExists = false;
            var existingRegion = (from region in mdbContext.Regions.AsQueryable()
                                  where region.TfxCompanyId == model.CompanyId &&
                                          region.IsActive && !region.IsDeleted &&
                                          region.Name.ToLower() == model.Name.ToLower()
                                  select new
                                  {
                                      region.Name
                                  }).FirstOrDefault();

            if (existingRegion != null)
                isRegionExists = true;

            return isRegionExists;
        }

        public async Task<RegionResponseModel> UpdateRegion(RegionViewModel model)
        {
            var response = new RegionResponseModel();
            try
            {
                var region = model.ToEntity();
                var routeresult = await UpdateRouteLocationInfo(model);
                if (routeresult.StatusCode == (int)Status.Success)
                {
                    var regionDbDrivers = mdbContext.Regions.Find(top => top.Id == region.Id && top.TfxCompanyId == region.TfxCompanyId).Project(x => x.TfxDrivers).ToList();
                    var updateFields = Builders<Region>.Update
                    .Set(t => t.Name, region.Name)
                    .Set(t => t.Description, region.Description)
                    .Set(t => t.SlotPeriod, region.SlotPeriod)
                    .Set(t => t.TfxJobs, region.TfxJobs)
                    .Set(t => t.TfxDrivers, region.TfxDrivers)
                    .Set(t => t.TfxDispatchers, region.TfxDispatchers)
                    .Set(t => t.TfxTrailers, region.TfxTrailers)
                    .Set(t => t.TfxStates, region.TfxStates)
                    .Set(t => t.TfxCarriers, region.TfxCarriers)
                    .Set(t => t.TfxProductTypeIds, region.TfxProductTypeIds)
                    .Set(t => t.TfxFuelTypeIds, region.TfxFuelTypeIds)
                    .Set(t => t.TfxFavProductTypeId, region.TfxFavProductTypeId);
                    var filter = Builders<Region>.Filter.And(
                            Builders<Region>.Filter.Where(x => x.Id == region.Id),
                            Builders<Region>.Filter.Where(x => x.TfxCompanyId == region.TfxCompanyId),
                            Builders<Region>.Filter.Where(x => x.IsActive),
                            Builders<Region>.Filter.Where(x => !x.IsDeleted)
                        );

                    await mdbContext.Regions.UpdateOneAsync(filter, updateFields);

                    var result = await UpdateDriverAssignmentForRegion(region.TfxDrivers, region.Id.ToString(), region.TfxCompanyId);
                    if (result.StatusCode == (int)Status.Success)
                    {
                        response.StatusCode = (int)Status.Success;
                        if (model.IsDsbDriverSchedule)
                        {
                            var scheduleBuilderInfo = await scheduleBuilderRepository.RemoveDriverFromDsbSchdule(region, regionDbDrivers);
                            if (scheduleBuilderInfo.Any())
                            {
                                response.ScheduleBuilderDetails = scheduleBuilderInfo;
                            }
                        }
                    }
                    else
                    {
                        response.StatusCode = (int)Status.Failed;
                        response.StatusMessage = "Failed to Assign Driver(s) to Region";
                    }
                }
                else
                {
                    response.StatusCode = (int)Status.Failed;
                    response.StatusMessage = routeresult.StatusMessage;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionRepository", "UpdateRegion - Name : " + model.Name, ex.Message, ex);
                response.StatusCode = (int)Status.Failed;
            }
            return response;
        }



        private async Task<StatusModel> UpdateRouteLocationInfo(RegionViewModel model)
        {
            var statusModel = new StatusModel();
            statusModel.StatusCode = (int)Status.Success;
            try
            {
                var routesInfo = (from item in mdbContext.RouteInformations.AsQueryable()
                                  where item.RegionId == ObjectId.Parse(model.Id) && item.IsActive && !item.IsDeleted
                                  select new
                                  {
                                      item.Id,
                                      item.TfxJobs
                                  });
                if (routesInfo.Count() > 0)
                {
                    // refresh the  regionLocationDetails
                    var regionLocation = mdbContext.Regions.Find(top => top.Id == ObjectId.Parse(model.Id)).FirstOrDefault();
                    if (regionLocation != null)
                    {
                        var regionLocInfo = regionLocation.TfxJobs;
                        if (model.Jobs != null)
                            regionLocInfo.RemoveAll(x => model.Jobs.Exists(y => y.Id == x.Id));
                        foreach (var regionitem in regionLocInfo)
                        {
                            var routeInfoLocation = routesInfo.Where(top => top.TfxJobs.Any(x => x.Id == regionitem.Id)).FirstOrDefault();
                            if (routeInfoLocation != null)
                            {
                                var index = routeInfoLocation.TfxJobs.FindIndex(x => x.Id == regionitem.Id);
                                if (index != -1)
                                {
                                    routeInfoLocation.TfxJobs.RemoveAt(index);
                                    int LocSeqNo = 0;
                                    foreach (var group in routeInfoLocation.TfxJobs)
                                    {
                                        LocSeqNo = LocSeqNo + 1;
                                        group.SequenceNo = LocSeqNo;
                                    }
                                }

                                var updateRouteFields = Builders<RouteInformations>.Update
                                                    .Set(t => t.UpdatedDate, DateTime.Now)
                                                    .Set(t => t.UpdatedBy, model.UpdatedBy)
                                                    .Set(t => t.TfxJobs, routeInfoLocation.TfxJobs);
                                var routefilter = Builders<RouteInformations>.Filter.And(
                                                  Builders<RouteInformations>.Filter.Where(x => x.Id == routeInfoLocation.Id),
                                                  Builders<RouteInformations>.Filter.Where(x => x.IsActive),
                                                  Builders<RouteInformations>.Filter.Where(x => !x.IsDeleted));
                                await mdbContext.RouteInformations.UpdateOneAsync(routefilter, updateRouteFields);

                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                statusModel.StatusMessage = "Error in update route location info.";
                statusModel.StatusCode = (int)Status.Failed;
                LogManager.Logger.WriteException("RegionRepository-Update", "UpdateRouteLocationInfo", ex.Message, ex);
            }
            return statusModel;
        }


        public async Task<string> GetRegionName(string regionId)
        {
            string name = null;
            try
            {
                var objectId = ObjectId.Parse(regionId);

                name = await mdbContext.Regions.Find(t => t.Id == objectId)
                                                     .Project(t => t.Name)
                                                     .FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionRepository", "GetRegionName - RegionId : " + regionId, ex.Message, ex);
            }
            return name;
        }

        public async Task<RegionViewModel> GetRegion(int companyId, string regionId)
        {
            RegionViewModel regionModel = null;
            try
            {
                var objectId = ObjectId.Parse(regionId);
                var filter = Builders<Region>.Filter.And(
                        Builders<Region>.Filter.Where(x => x.Id == objectId),
                        Builders<Region>.Filter.Where(x => x.TfxCompanyId == companyId),
                        Builders<Region>.Filter.Where(x => x.IsActive),
                        Builders<Region>.Filter.Where(x => !x.IsDeleted)
                    );

                var region = await mdbContext.Regions.Find(filter).FirstOrDefaultAsync();
                if (region != null)
                {
                    regionModel = region.ToModel();
                    var shifts = await mdbContext.Shifts.Find(t => t.RegionId == region.Id && t.IsActive && !t.IsDeleted).ToListAsync();
                    regionModel.Shifts = shifts.Select(t => t.ToModel()).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionRepository", "GetRegion - RegionId : " + regionId, ex.Message, ex);
                regionModel.StatusCode = (int)Status.Failed;
            }
            return regionModel;
        }

        public List<RegionViewModel> GetRegions(int companyId)
        {
            var response = new List<RegionViewModel>();
            try
            {
                var regionList = mdbContext.Regions.Find(t => t.IsActive && !t.IsDeleted && t.TfxCompanyId == companyId).ToList();
                var regionIds = regionList.Select(t => t.Id).ToList();
                var shifts = mdbContext.Shifts.Find(t => regionIds.Contains(t.RegionId)).ToList();
                Parallel.ForEach(regionList, item =>
                {
                    var model = new RegionViewModel
                    {
                        Id = item.Id.ToString(),
                        CompanyId = item.TfxCompanyId,
                        CreatedBy = item.CreatedBy,
                        CreatedOn = item.CreatedOn,
                        UpdatedOn = item.UpdatedOn,
                        UpdatedBy = item.UpdatedBy,
                        Description = item.Description,
                        SlotPeriod = item.SlotPeriod,
                        IsActive = item.IsActive,
                        IsDeleted = item.IsDeleted,
                        Name = item.Name,
                        ProductTypeIds = item.TfxProductTypeIds ?? new List<int>(),
                        FuelTypeIds = item.TfxFuelTypeIds != null ? item.TfxFuelTypeIds.Select(t => new FreightModels.DropdownDisplayItem { Id = t.Id, Code = t.Code, Name = t.Name }).ToList() : null,
                        FavProductTypeId = item.TfxFavProductTypeId ?? 0,
                        Jobs = item.TfxJobs != null ? item.TfxJobs.Select(t => new FreightModels.DropdownDisplayItem { Id = t.Id, Code = t.Code, Name = t.Name }).ToList() : null,
                        Drivers = item.TfxDrivers != null ? item.TfxDrivers.Select(t => new FreightModels.DropdownDisplayItem { Id = t.Id, Code = t.Code, Name = t.Name }).ToList() : null,
                        Dispatchers = item.TfxDispatchers != null ? item.TfxDispatchers.Select(t => new FreightModels.DropdownDisplayItem { Id = t.Id, Code = t.Code, Name = t.Name }).ToList() : null,
                        Trailers = item.TfxTrailers != null ? item.TfxTrailers.Select(t => new FreightModels.DropdownDisplayItem { Id = t.Id, Code = t.Code, Name = t.Name }).ToList() : null,
                        States = item.TfxStates != null ? item.TfxStates.Select(t => new FreightModels.DropdownDisplayItem { Code = t.Code, Name = t.Name }).ToList() : null,
                        Carriers = item.TfxCarriers != null ? item.TfxCarriers.Select(t => new FreightModels.TfxCarrierDropdownDisplayViewModelItem { Id = t.Id, Code = t.Code, Name = t.Name, SequenceNo = t.SequenceNo, RegionId = t.RegionId }).ToList() : new List<TfxCarrierDropdownDisplayViewModelItem>()
                    };

                    if (shifts != null)
                    {
                        model.Shifts = shifts.Where(t => t.RegionId == item.Id).Select(t => new ShiftViewModel
                        {
                            Id = t.Id.ToString(),
                            CompanyId = t.CompanyId,
                            CreatedBy = t.CreatedBy,
                            CreatedOn = t.CreatedOn,
                            EndTime = t.EndTime,
                            IsActive = t.IsActive,
                            IsDeleted = t.IsDeleted,
                            Name = t.Name,
                            RegionId = t.RegionId.ToString(),
                            StartTime = t.StartTime
                        }).ToList();
                    }

                    response.Add(model);
                });
                
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionRepository", "GetRegions - CompanyId : " + companyId, ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DispatcherDashboardRegionModel>> GetDispatcherDashboardRegions(int companyId, int dispatcherId)
        {
            var regions = await mdbContext.Regions.FindAsync(t => t.TfxCompanyId == companyId &&
                                (dispatcherId == 0 || t.TfxDispatchers.Any(t1 => t1.Id == dispatcherId))
                                && t.IsActive && !t.IsDeleted);



            var regionDetails = regions.ToList().Select(t => new DispatcherDashboardRegionModel()
            {
                Id = t.Id.ToString(),
                Name = t.Name,
                States = t.TfxStates.Select(x => new FreightModels.DropdownDisplayItem()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name
                }).ToList(),
                Dispatchers = t.TfxDispatchers.Select(x => new FreightModels.DropdownDisplayItem() { Id = x.Id, Name = x.Name }).ToList()
            }).ToList();
            return regionDetails;
        }

        public async Task<List<string>> GetDispatcherRegionIds(int companyId, int dispatcherId)
        {
            var response = new List<string>();
            var regionIds = await mdbContext.Regions.Find(t => t.TfxCompanyId == companyId &&
                                (dispatcherId == 0 || t.TfxDispatchers.Any(t1 => t1.Id == dispatcherId))
                                && t.IsActive && !t.IsDeleted).Project(t => t.Id).ToListAsync();
            if (regionIds != null && regionIds.Any())
            {
                response = regionIds.Select(t => t.ToString()).ToList();
            }
            return response;
        }

        public List<int> GetDriverDetailsByCompanyId(int companyId, int dispatcherId, string regionID)
        {
            List<int> driversId = new List<int>();
            if (string.IsNullOrEmpty(regionID))
            {
                var regions = (from region in mdbContext.Regions.AsQueryable()
                               where region.TfxCompanyId == companyId
                               && region.IsActive && !region.IsDeleted
                               select new
                               {
                                   region.TfxDrivers
                               }).ToList();

                foreach (var item in regions)
                {
                    foreach (var driveritem in item.TfxDrivers)
                    {
                        driversId.Add(driveritem.Id);
                    }
                }
            }
            else
            {
                ObjectId objectId = new ObjectId(regionID);
                var regions = (from region in mdbContext.Regions.AsQueryable()
                               where region.TfxCompanyId == companyId
                               && region.IsActive && !region.IsDeleted && region.Id != objectId
                               select new
                               {
                                   region.TfxDrivers
                               }).ToList();

                foreach (var item in regions)
                {
                    foreach (var driveritem in item.TfxDrivers)
                    {
                        driversId.Add(driveritem.Id);
                    }
                }
            }
            return driversId;
        }

        public async Task<StatusModel> DeleteRegion(string regionId, int deletedBy)
        {
            var response = new StatusModel();
            try
            {
                var filter = Builders<Region>.Filter.And(
                            Builders<Region>.Filter.Where(x => x.Id == ObjectId.Parse(regionId))
                        );

                var updateFields = Builders<Region>.Update
                    .Set(t => t.UpdatedBy, deletedBy)
                    .Set(t => t.UpdatedOn, DateTimeOffset.Now)
                    .Set(t => t.IsActive, false)
                    .Set(t => t.IsDeleted, true);
                await mdbContext.Regions.UpdateOneAsync(filter, updateFields);
                await DeleteRouteInfo(regionId, deletedBy);
                await DeleteRecurringSchedulenfo(regionId, deletedBy);
                response.StatusCode = (int)Status.Success;
                response.StatusMessage = "Deleted successfully";
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionRepository", "DeleteRegion - RegionId : " + regionId, ex.Message, ex);
                response.StatusCode = (int)Status.Failed;
                response.StatusMessage = Status.Failed.ToString();
            }
            return response;
        }

        private async Task DeleteRouteInfo(string regionId, int deletedBy)
        {
            var routefilter = Builders<RouteInformations>.Filter.And(
                       Builders<RouteInformations>.Filter.Where(x => x.RegionId == ObjectId.Parse(regionId))
                   );

            var routeupdateFields = Builders<RouteInformations>.Update
                .Set(t => t.UpdatedBy, deletedBy)
                .Set(t => t.UpdatedDate, DateTimeOffset.Now)
                .Set(t => t.IsActive, false)
                .Set(t => t.IsDeleted, true);
            await mdbContext.RouteInformations.UpdateManyAsync(routefilter, routeupdateFields);
        }
        private async Task DeleteRecurringSchedulenfo(string regionId, int deletedBy)
        {
            var recurringfilter = Builders<RecurringSchedules>.Filter.And(
                       Builders<RecurringSchedules>.Filter.Where(x => x.RegionId == ObjectId.Parse(regionId))
                   );

            var recurringFields = Builders<RecurringSchedules>.Update
                .Set(t => t.UpdatedBy, deletedBy)
                .Set(t => t.UpdatedOn, DateTimeOffset.Now)
                .Set(t => t.IsActive, false)
                .Set(t => t.IsDeleted, true);
            await mdbContext.RecurringSchedules.UpdateManyAsync(recurringfilter, recurringFields);
        }


        public async Task<StatusModel> AssignTPOJobToRegion(JobToRegionAssignViewModel jobToUpdate)
        {
            var response = new StatusModel(Status.Failed);
            try
            {
                //Find the region with given job and company

                var regionId = await GetRegionIdForJob(jobToUpdate.JobId, jobToUpdate.CompanyId);
                if (regionId == jobToUpdate.RegionId)// No Assignment
                {
                    response.StatusCode = (int)Status.Success;
                    return response;
                }
                if (!string.IsNullOrEmpty(regionId))
                {
                    var routeInfoExists = (from item in mdbContext.RouteInformations.AsQueryable()
                                           where item.TfxJobs.Any(x => x.Id == jobToUpdate.JobId) && item.RegionId == ObjectId.Parse(regionId)
                                           select new
                                           {
                                               item.Id
                                           }).FirstOrDefault();
                    if (routeInfoExists != null)
                    {
                        response.StatusCode = (int)Status.Failed;
                        response.StatusMessage = Resource.warningRouteExistsRegion;
                        return response;
                    }
                }
                //Implies No Region Assigned To Job. Current Assignment will be removed
                if ((jobToUpdate.RegionId == null || jobToUpdate.RegionId == string.Empty) && regionId != null)
                {
                    var objectId = ObjectId.Parse(regionId);
                    var filter = Builders<Region>.Filter.And(
                            Builders<Region>.Filter.Where(x => x.Id == objectId),
                            Builders<Region>.Filter.Where(x => x.TfxCompanyId == jobToUpdate.CompanyId),
                            Builders<Region>.Filter.Where(x => x.IsActive),
                            Builders<Region>.Filter.Where(x => !x.IsDeleted)
                        );
                    var update = Builders<Region>.Update.PullFilter(p => p.TfxJobs, f => f.Id == jobToUpdate.JobId);
                    await mdbContext.Regions.FindOneAndUpdateAsync(filter, update);
                    response.StatusCode = (int)Status.Success;
                    response.StatusMessage = "Job " + jobToUpdate.JobName + " Succesfully Removed from Region";
                    return response;
                }
                if ((regionId != jobToUpdate.RegionId) && regionId != null)//Implies new Region is selected for given job 
                {
                    //First Remove the given job from existing region as one job can belong to one region only 
                    var objectId = ObjectId.Parse(regionId);
                    var filter = Builders<Region>.Filter.And(
                            Builders<Region>.Filter.Where(x => x.Id == objectId),
                            Builders<Region>.Filter.Where(x => x.TfxCompanyId == jobToUpdate.CompanyId),
                            Builders<Region>.Filter.Where(x => x.IsActive),
                            Builders<Region>.Filter.Where(x => !x.IsDeleted)
                        );
                    var update = Builders<Region>.Update.PullFilter(p => p.TfxJobs, f => f.Id == jobToUpdate.JobId);
                    await mdbContext.Regions.FindOneAndUpdateAsync(filter, update);
                    return await AddJobToRegion(jobToUpdate);
                }
                else
                {
                    response = await AddJobToRegion(jobToUpdate);//First time job Assignment
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionRepository", "AssignTPOJobToRegion", ex.Message, ex);
                response.StatusCode = (int)Status.Failed;
                response.StatusMessage = Status.Failed.ToString();
            }
            return response;
        }

        public async Task<string> GetRegionIdForJob(int jobId, int companyId)
        {
            string regionId = string.Empty;
            try
            {
                var region = await (mdbContext.Regions.FindAsync(t => t.TfxCompanyId == companyId &&
                                                                t.IsActive && !t.IsDeleted &&
                                                                t.TfxJobs.Any(t1 => t1.Id == jobId)));

                if (region != null)
                {
                    regionId = region.ToList().Select(t => t.Id.ToString()).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {

                LogManager.Logger.WriteException("RegionRepository", "GetRegionIdForJob", ex.Message, ex);
            }
            return regionId;

        }

        public async Task<List<QuickEntryDRModels>> GetJobsForAllRegions(int companyId)
        {
            var regionId = new List<QuickEntryDRModels>();
            try
            {
                var region = await (mdbContext.Regions.FindAsync(t => t.TfxCompanyId == companyId &&
                                                                t.IsActive && !t.IsDeleted));

                if (region != null)
                {
                    regionId = region.ToList().Select(t =>
                        new QuickEntryDRModels()
                        {
                            RegionId = t.Id.ToString(),
                            CompanyId = t.TfxCompanyId,
                            Jobs = t.TfxJobs.Select(t1 => new FreightModels.DropdownDisplayItem() { Id = t1.Id, Name = t1.Name, Code = t1.Code }).ToList(),
                        }).ToList();
                }
            }
            catch (Exception ex)
            {

                LogManager.Logger.WriteException("JobRepository", "GetRegionIdForJob", ex.Message, ex);
            }
            return regionId;

        }

        public async Task<StatusModel> AddJobToRegion(JobToRegionAssignViewModel jobToUpdate)
        {
            var response = new StatusModel(Status.Failed);
            try
            {
                MdbDataAccess.Collections.DropdownDisplayItem TfxJob = new MdbDataAccess.Collections.DropdownDisplayItem()
                {
                    Id = jobToUpdate.JobId,
                    Code = null,
                    Name = jobToUpdate.JobName
                };
                var filter = Builders<Region>.Filter.And(
                               Builders<Region>.Filter.Where(x => x.Id == ObjectId.Parse(jobToUpdate.RegionId)),
                                Builders<Region>.Filter.Where(x => x.TfxCompanyId == jobToUpdate.CompanyId),
                                   Builders<Region>.Filter.Where(x => x.IsActive),
                                   Builders<Region>.Filter.Where(x => !x.IsDeleted)
                               );

                var update = Builders<Region>.Update.Push("TfxJobs", TfxJob);
                await mdbContext.Regions.FindOneAndUpdateAsync(filter, update);

                response.StatusCode = (int)Status.Success;
                response.StatusMessage = "Job " + jobToUpdate.JobName + " assigned successfully to Region";
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionRepository", "AddJobToRegion", ex.Message, ex);

            }
            return response;
        }

        public List<DropdownDisplayExtendedItem> GetRegionsDdl(int companyId)
        {
            var response = new List<DropdownDisplayExtendedItem>();
            try
            {
                var regionList = (from region in mdbContext.Regions.AsQueryable()
                                  where region.TfxCompanyId == companyId
                                  && region.IsActive && !region.IsDeleted
                                  orderby region.Name
                                  select new
                                  {
                                      Id = region.Id,
                                      Name = region.Name,
                                  }).ToList();
                regionList.ForEach(t => response.Add(new DropdownDisplayExtendedItem() { Code = t.Id.ToString(), Name = t.Name }));
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionRepository", "GetRegionsDdl - CompanyId : " + companyId, ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayExtendedItem> GetCarriersAssignedToRegion(string regionId)
        {
            var response = new List<DropdownDisplayExtendedItem>();
            try
            {
                var objectId = ObjectId.Parse(regionId);
                var regions = (from region in mdbContext.Regions.AsQueryable()
                               where region.Id == objectId
                               && region.IsActive && !region.IsDeleted
                               select region).FirstOrDefault();
                if (regions != null && regions.TfxCarriers != null)
                {
                    foreach (var carrier in regions.TfxCarriers)
                    {
                        response.Add(new DropdownDisplayExtendedItem() { Id = carrier.Id, Name = carrier.Name });
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionRepository", "GetCarriersAssignedToRegion - regionId : " + regionId, ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayExtendedItem> GetDispatchersAssignedToRegion(List<string> regionId)
        {
            var response = new List<DropdownDisplayExtendedItem>();
            try
            {
                var objectIds = regionId.Select(t => ObjectId.Parse(t)).ToList();
                var regions = (from region in mdbContext.Regions.AsQueryable()
                               where objectIds.Contains(region.Id)
                               && region.IsActive && !region.IsDeleted
                               select region).FirstOrDefault();
                if (regions != null && regions.TfxDispatchers != null)
                {
                    foreach (var dispatcher in regions.TfxDispatchers)
                    {
                        response.Add(new DropdownDisplayExtendedItem() { Id = dispatcher.Id, Name = dispatcher.Name });
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionRepository", "GetDispatchersAssignedToRegion", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<int>> GetJobsAssignedToRegions(int companyId)
        {
            List<int> RegionjobsIds = new List<int>();
            try
            {
                var regions = (from region in mdbContext.Regions.AsQueryable()
                               where region.TfxCompanyId == companyId
                               && region.IsActive && !region.IsDeleted
                               select region).ToList();
                if (regions.Any())
                {
                    foreach (var reg in regions)
                    {
                        if (reg.TfxJobs.Any())
                        {
                            foreach (var job in reg.TfxJobs)
                            {
                                RegionjobsIds.Add(job.Id);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionRepository", "GetJobsAssignedToRegions", ex.Message, ex);
            }
            return RegionjobsIds;
        }

        public async Task<List<int>> GetJobsAssignedToDriver(int driverId)
        {
            List<int> RegionjobsIds = new List<int>();
            try
            {
                var driverJobs = await mdbContext.Regions.Find(t => t.TfxDrivers.Any(t1 => t1.Id == driverId) && t.IsActive && !t.IsDeleted)
                                    .Project(t => t.TfxJobs.Select(t1 => t1.Id)).ToListAsync();

                if (driverJobs != null)
                {
                    driverJobs.ForEach(t => RegionjobsIds.AddRange(t));
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionRepository", "GetJobsAssignedToDriver", ex.Message, ex);
            }
            return RegionjobsIds;
        }

        //Input: regionId in string 
        //Output: all drivers from DRIVER Collection belonging to input regionId
        public async Task<List<DropdownDisplayExtendedItem>> GetAllDriversInRegion(string regionId)
        {
            List<DropdownDisplayExtendedItem> driverUsers = new List<DropdownDisplayExtendedItem>();

            try
            {
                var objectId = ObjectId.Parse(regionId);
                var drivers = await mdbContext.Drivers.FindAsync(t => t.Regions.Contains(objectId)).Result.ToListAsync();
                if (drivers.Any())
                {
                    foreach (Driver driver in drivers)
                    {
                        driverUsers.Add(new DropdownDisplayExtendedItem { Id = driver.DriverId, Code = null, Name = driver.DriverName });
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionRepository", "GetAllDriversInRegion", ex.Message, ex);
            }
            return driverUsers;
        }

        public async Task<StatusModel> UpdateDriverAssignmentForRegion(List<SiteFuel.MdbDataAccess.Collections.DropdownDisplayItem> driversList, string regionId, int companyId)
        {
            var response = new StatusModel(Status.Failed);
            try
            {
                List<int> alreadyAssignedDriverIds = new List<int>();
                List<int> newlyAssignedDriverIds = new List<int>();
                var objectId = ObjectId.Parse(regionId);
                var drivers = GetAllDriversInRegion(regionId).Result.ToList();// Get all drivers in given region
                if (drivers.Any())
                {
                    drivers.ForEach(d => alreadyAssignedDriverIds.Add(d.Id));
                }
                if (driversList.Any())
                {
                    driversList.ForEach(d => newlyAssignedDriverIds.Add(d.Id));
                }
                if (!drivers.Any() && driversList.Any())//No Previously assigned drivers.New drivers will be added 
                {
                    foreach (var driverId in newlyAssignedDriverIds)
                    {
                        var filter = Builders<Driver>.Filter.And(
                              Builders<Driver>.Filter.Where(x => x.DriverId == driverId),
                               Builders<Driver>.Filter.Where(x => x.CompanyId == companyId),
                               Builders<Driver>.Filter.Where(x => !x.IsDeleted)
                           );
                        var update = Builders<Driver>.Update.Push("Regions", objectId);
                        await mdbContext.Drivers.FindOneAndUpdateAsync(filter, update);

                    }
                    response.StatusCode = (int)Status.Success;
                    response.StatusMessage = "Driver(s) Assigned Succefully to region";
                    return response;
                }
                else if (drivers.Any() && !driversList.Any())//All drivers removed from Region -Create and Edit
                {
                    foreach (var driverId in alreadyAssignedDriverIds)
                    {
                        var filter = Builders<Driver>.Filter.And(
                              Builders<Driver>.Filter.Where(x => x.DriverId == driverId),
                               Builders<Driver>.Filter.Where(x => x.CompanyId == companyId),
                               Builders<Driver>.Filter.Where(x => !x.IsDeleted)
                           );
                        var update = Builders<Driver>.Update.Pull("Regions", objectId);
                        await mdbContext.Drivers.FindOneAndUpdateAsync(filter, update);
                    }
                    response.StatusCode = (int)Status.Success;
                    response.StatusMessage = "Driver(s) Removed Succesfully from region";
                    return response;
                }
                else
                {
                    // Remove all alreadyassignedDrivers
                    foreach (var driverId in alreadyAssignedDriverIds)
                    {
                        var filter = Builders<Driver>.Filter.And(
                              Builders<Driver>.Filter.Where(x => x.DriverId == driverId),
                               Builders<Driver>.Filter.Where(x => x.CompanyId == companyId),
                               Builders<Driver>.Filter.Where(x => !x.IsDeleted)
                           );
                        var update = Builders<Driver>.Update.Pull("Regions", objectId);
                        await mdbContext.Drivers.FindOneAndUpdateAsync(filter, update);
                    }
                    // Add all newlyassigned Drivers
                    foreach (var driverId in newlyAssignedDriverIds)
                    {
                        var filter = Builders<Driver>.Filter.And(
                              Builders<Driver>.Filter.Where(x => x.DriverId == driverId),
                               Builders<Driver>.Filter.Where(x => x.CompanyId == companyId),
                               Builders<Driver>.Filter.Where(x => !x.IsDeleted)
                           );
                        var update = Builders<Driver>.Update.Push("Regions", objectId);
                        await mdbContext.Drivers.FindOneAndUpdateAsync(filter, update);
                    }
                    response.StatusCode = (int)Status.Success;
                    response.StatusMessage = "Driver(s) Assigned Succefully to region";
                    return response;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionRepository", "UpdateDriverassignmentForRegion", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<RegionJobsModel>> GetRegionsForJobs(RegionInputModel request)
        {
            var response = new List<RegionJobsModel>();
            try
            {
                var regionIdList = new List<ObjectId>();
                if (!string.IsNullOrEmpty(request.RegionIds))
                {
                    regionIdList = request.RegionIds.Split(',').Select(t => ObjectId.Parse(t)).ToList();
                }

                var regionList = await (from region in mdbContext.Regions.AsQueryable()
                                          where region.TfxCompanyId == request.CompanyId
                                          && region.IsActive && !region.IsDeleted
                                          && (string.IsNullOrEmpty(request.RegionIds) || regionIdList.Contains(region.Id))
                                          orderby region.Name
                                          select new
                                          {
                                              Region = new
                                              {
                                                  Id = region.Id,
                                                  Name = region.Name,
                                                  Jobs = region.TfxJobs != null ? region.TfxJobs.Select(t => new FreightModels.DropdownDisplayItem { Id = t.Id, Code = t.Code, Name = t.Name }) : null,
                                              }
                                          }).ToListAsync();

                Parallel.ForEach(regionList, new ParallelOptions { MaxDegreeOfParallelism = 20 }, item =>
                {
                    var model = new RegionJobsModel
                    {
                        Id = item.Region.Id.ToString(),
                        Name = item.Region.Name,
                        Jobs = item.Region.Jobs != null ? item.Region.Jobs.ToList() : null,
                    };
                    response.Add(model);
                });
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionRepository", "GetRegionsForJobs - CompanyId : " + request.CompanyId + ", Regions :" + request.RegionIds, ex.Message, ex);
            }
            return response;
        }
        
        public List<TfxCarrierDropdownDisplayItem> GetRegionCarriers(string regionId)
        {
            var response = new List<TfxCarrierDropdownDisplayItem>();
            try
            {
                if (!string.IsNullOrEmpty(regionId))
                {
                    ObjectId objectId = ObjectId.Parse(regionId);
                    var regionList = (from region in mdbContext.Regions.AsQueryable()
                                      where region.Id == objectId
                                      && region.IsActive && !region.IsDeleted && region.TfxCarriers != null
                                      select new
                                      {
                                          region.TfxCarriers
                                      }).SelectMany(top => top.TfxCarriers).OrderBy(top => top.Name).ToList();
                    regionList.ForEach(t => response.Add(new TfxCarrierDropdownDisplayItem() { Id = t.Id, Name = t.Name, Code = t.Code, SequenceNo = t.SequenceNo, RegionId = t.RegionId }));

                    response = response.OrderBy(o => o.SequenceNo).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionRepository", "GetRegionCarriers - regionId : " + regionId, ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusModel> AddRegionSchedule(RegionScheduleModel model)
        {
            var response = new StatusModel();
            var RegionSchedule = model.ToEntity();
            await mdbContext.RegionScheduleMappings.InsertOneAsync(RegionSchedule);
            response.StatusCode = (int)Status.Success;
            return response;
        }
        public async Task<List<RegionScheduleModel>> GetRegionShiftSchedule(string regionId, string routeId)
        {
            var response = new List<RegionScheduleModel>();


            try
            {
                ObjectId RegionId = new ObjectId(regionId);
                ObjectId RouteId = new ObjectId(routeId);
                var regionScheduleMapping = (from reginShiftSchedule in mdbContext.RegionScheduleMappings.AsQueryable()
                                             where reginShiftSchedule.RegionId.Equals(RegionId)
                                             && reginShiftSchedule.RouteId.Equals(RouteId) && reginShiftSchedule.IsActive

                                             select new RegionScheduleMapping()
                                             {
                                                 Id = reginShiftSchedule.Id,
                                                 RegionId = reginShiftSchedule.RegionId,
                                                 RouteId = reginShiftSchedule.RouteId,
                                                 StartDate = reginShiftSchedule.StartDate,
                                                 EndDate = reginShiftSchedule.EndDate,
                                                 RepeatDayList = reginShiftSchedule.RepeatDayList,
                                                 RegionShiftDetail = reginShiftSchedule.RegionShiftDetail
                                             }).ToList();

                if (regionScheduleMapping != null && regionScheduleMapping.Count > 0)
                {
                    foreach (var ScheduleMapping in regionScheduleMapping)
                    {
                        var RegionScheduleModel = new RegionScheduleModel();

                        RegionScheduleModel.Id = ScheduleMapping.Id.ToString();
                        RegionScheduleModel.RegionId = ScheduleMapping.RegionId.ToString();
                        RegionScheduleModel.RouteId = ScheduleMapping.RouteId.ToString();
                        RegionScheduleModel.StartDate = ScheduleMapping.StartDate.ToString();
                        RegionScheduleModel.EndDate = ScheduleMapping.EndDate.ToString();
                        RegionScheduleModel.RepeatDayList = ScheduleMapping.RepeatDayList;
                        if (ScheduleMapping.RegionShiftDetail.Count > 0)
                        {
                            foreach (var mapping in ScheduleMapping.RegionShiftDetail)
                            {
                                RegionShiftDetailViewModel oModel = new RegionShiftDetailViewModel();
                                oModel.ColumnIndex = mapping.ColumnIndex;
                                oModel.ShiftId = mapping.ShiftId.ToString();
                                RegionScheduleModel.RegionShiftDetail.Add(oModel);
                            }
                        }
                        response.Add(RegionScheduleModel);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionRepository", "GetRegionShiftSchedule", ex.Message, ex);
            }
            return response;
        }

        public List<RegionScheduleMappingViewModel> GetSchedulesByRegion(string regionId, int scheduleType)
        {
            var response = new List<RegionScheduleMappingViewModel>();

            if (string.IsNullOrWhiteSpace(regionId))
                return response;

            ObjectId RegionId = new ObjectId(regionId);
            var regions = mdbContext.Regions.Find(r => r.Id == RegionId && r.IsActive).FirstOrDefault();
            var routes = mdbContext.RouteInformations.Find(r => r.RegionId == RegionId).ToList();
            var shifts = mdbContext.Shifts.Find(x => x.RegionId == RegionId).ToList();
            var driver = mdbContext.Drivers.AsQueryable();
            var RegionSchedule = mdbContext.RegionScheduleMappings.Find(x => x.RegionId == RegionId && x.IsActive).ToList();

            response = RegionSchedule.Select(s => new RegionScheduleMappingViewModel
            {
                Id = s.Id,
                RegionId = s.RegionId,
                RouteId = s.RouteId,
                RegionName = regions.Name,
                RouteName = routes.Where(r => r.Id == s.RouteId).Select(r => r.Name).FirstOrDefault(),
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                Description = s.Description,
                IsUnplanedSchedule = false,
                RepeatDayList = s.RepeatDayList.ToList(),
                ShiftDetail = s.RegionShiftDetail.Select(r => new ShiftDetailViewModel()
                {
                    ShiftId = r.ShiftId,
                    ShiftName = shifts.Where(x => x.Id == r.ShiftId).Select(a => a.Name + " (" + a.StartTime + "-" + a.EndTime + ")").FirstOrDefault(),
                    ColumnIndex = r.ColumnIndex,
                    ColumnName = "C" + r.ColumnIndex
                }).ToList()
            }).ToList();

            //if (scheduleType == (int)DriverScheduleType.Planned || scheduleType == (int)DriverScheduleType.UnPlanned)
            //{
            //    response = response.Where(w => w.IsUnplanedSchedule == (scheduleType == (int)DriverScheduleType.Planned ? false : true)).ToList();
            //}
            return response;
        }

        public async Task<List<CarrierRegionModel>> GetAllCarrierRegions(List<DropdownDisplayExtendedItem> Carriers)
        {
            var response = new List<CarrierRegionModel>();
            try
            {
                var carrierIds = Carriers.Select(x => x.Id).ToList();
                var regions = await mdbContext.Regions.Find(x => x.IsActive &&
                                                                !x.IsDeleted &&
                                                                carrierIds.Contains(x.TfxCompanyId)).Project(t =>
                                                                new { t.Id, t.Name, t.TfxCompanyId }).ToListAsync();
                if (regions.Any())
                {
                    foreach (var carrier in Carriers)
                    {
                        var carrierRegions = regions.Where(x => x.TfxCompanyId == carrier.Id).ToList();
                        if (carrierRegions.Any())
                        {
                            CarrierRegionModel carrierRegionModel = new CarrierRegionModel();
                            carrierRegionModel.Id = carrier.Id;
                            carrierRegionModel.Name = carrier.Name;
                            carrierRegionModel.Regions = new List<FreightModels.TfxCarrierRegionDetailsModel>();
                            carrierRegions.ForEach(t =>
                            {
                                var region = new FreightModels.TfxCarrierRegionDetailsModel();
                                region.Id = t.Id.ToString();
                                region.Name = t.Name;
                                carrierRegionModel.Regions.Add(region);
                            });
                            response.Add(carrierRegionModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionRepository", "GetCarriersRegions : ", ex.Message, ex);
                response = new List<CarrierRegionModel>();
            }
            return response;
        }
        public async Task<StatusModel> RemoveDriverFromRegion(RegionDriverRemoveModel model)
        {
            var response = new StatusModel();
            try
            {
                await RemoveRegionDrivers(model);
                await RemoveTractorsDrivers(model);
                await RemoveDriverScheduleIfExists(model);
                await RemoveDriverIfExists(model);
                if (model.IsScheduleExists)
                {
                    await RemoveDriverDSBIfExists(model);
                }
                response.StatusCode = (int)Status.Success;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionRepository", "RemoveDriverFromRegion", ex.Message, ex);
                response.StatusCode = (int)Status.Failed;
            }
            return response;
        }

        public async Task<List<InvitedDriverResponseModel>> CheckInvitedDriverScheduleExists(List<RegionDriverRemoveModel> model)
        {
            var response = new List<InvitedDriverResponseModel>();
            try
            {
                DateTime dateFilter = DateTimeOffset.Now.Date;
                var driverIDs = model.Select(x => x.DriverId).ToList();
                var scheduleBuildersInfo = await mdbContext.ScheduleBuilders
                                    .Find(t => t.IsActive && !t.IsDeleted
                                        && t.DateFilter >= dateFilter && t.Trips.Any(i => i.TfxDrivers.Any(x => driverIDs.Contains(x.Id)))).Project(x1 => new { x1.Trips, x1.Id }).ToListAsync();
                if (scheduleBuildersInfo.Any())
                {
                    foreach (var item in scheduleBuildersInfo)
                    {
                        var tripDriverDetails = (from tripDriver in item.Trips
                                                 where tripDriver.TfxDrivers.Any() && (tripDriver.DeliveryGroupStatus == DeliveryGroupStatus.None || tripDriver.DeliveryGroupStatus == DeliveryGroupStatus.Draft)
                                                 select new
                                                 {
                                                     DriverId = tripDriver.TfxDrivers.FirstOrDefault().Id,
                                                 }).Distinct().ToList();
                        if (tripDriverDetails.Any())
                        {
                            foreach (var tripDriverInfo in tripDriverDetails)
                            {
                                var tripDriverExists = response.FindIndex(x => x.DriverId == tripDriverInfo.DriverId);
                                if (tripDriverExists == -1)
                                {
                                    response.Add(new InvitedDriverResponseModel { DriverId = tripDriverInfo.DriverId, ScheduleBuilderIds = new List<string> { item.Id.ToString() }, UserId = model.FirstOrDefault().UserId });
                                }
                                else
                                {
                                    var driverInfo = response.FirstOrDefault(x => x.DriverId == tripDriverInfo.DriverId);
                                    if (driverInfo != null)
                                    {
                                        driverInfo.ScheduleBuilderIds.Add(item.Id.ToString());
                                    }
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionRepository", "CheckInvitedDriverScheduleExists", ex.Message, ex);

            }
            return response;
        }

        private async Task RemoveRegionDrivers(RegionDriverRemoveModel model)
        {
            List<RegionDriverRemoveViewModelModel> regionDriverRemove = new List<RegionDriverRemoveViewModelModel>();
            var regionDetails = await mdbContext.Regions.Find(x => x.IsActive && !x.IsDeleted).Project(x1 => new { x1.Id, x1.TfxDrivers }).ToListAsync();
            foreach (var item in regionDetails)
            {
                var driverindex = item.TfxDrivers.FindIndex(x1 => x1.Id == model.DriverId);
                if (driverindex >= 0)
                {
                    item.TfxDrivers.RemoveAt(driverindex);
                    var drivers = item.TfxDrivers;
                    regionDriverRemove.Add(new RegionDriverRemoveViewModelModel { Id = item.Id.ToString(), Drivers = drivers.Select(t => new FreightModels.DropdownDisplayItem { Id = t.Id, Code = t.Code, Name = t.Name }).ToList() });
                }
            }
            foreach (var item in regionDriverRemove)
            {
                ObjectId regionId = ObjectId.Empty;
                ObjectId.TryParse(item.Id, out regionId);
                var TfxDrivers = item.Drivers.Select(t => new MdbDataAccess.Collections.DropdownDisplayItem { Id = t.Id, Code = t.Code, Name = t.Name }).ToList();
                var updateFields = Builders<Region>.Update
                .Set(t => t.TfxDrivers, TfxDrivers)
                 .Set(t => t.UpdatedBy, model.UserId)
                  .Set(t => t.UpdatedOn, DateTime.Now);

                var filter = Builders<Region>.Filter.And(
                        Builders<Region>.Filter.Where(x => x.Id == regionId),
                        Builders<Region>.Filter.Where(x => x.IsActive),
                        Builders<Region>.Filter.Where(x => !x.IsDeleted)
                    );

                await mdbContext.Regions.UpdateOneAsync(filter, updateFields);
            }
        }
        private async Task RemoveTractorsDrivers(RegionDriverRemoveModel model)
        {
            List<RegionDriverRemoveViewModelModel> tractorDriverRemove = new List<RegionDriverRemoveViewModelModel>();
            var tractorDetails = await mdbContext.TractorDetails.Find(x => !x.IsDeleted).Project(x1 => new { x1.Id, x1.Drivers }).ToListAsync();
            foreach (var item in tractorDetails)
            {
                var driverindex = item.Drivers.FindIndex(x1 => x1.TfxId == model.DriverId);
                if (driverindex >= 0)
                {
                    item.Drivers.RemoveAt(driverindex);
                    var drivers = item.Drivers;
                    tractorDriverRemove.Add(new RegionDriverRemoveViewModelModel { Id = item.Id.ToString(), Drivers = drivers.Select(t => new FreightModels.DropdownDisplayItem { Id = t.TfxId, Name = t.TfxName }).ToList() });
                }
            }
            foreach (var item in tractorDriverRemove)
            {
                ObjectId tractorId = ObjectId.Empty;
                ObjectId.TryParse(item.Id, out tractorId);
                var TfxDrivers = item.Drivers.Select(t => new MdbDataAccess.Collections.DriverDetails { TfxId = t.Id, TfxName = t.Name }).ToList();
                var updateFields = Builders<TractorDetail>.Update
                    .Set(t => t.Drivers, TfxDrivers)
                    .Set(t => t.UpdatedDate, DateTime.Now);

                var filter = Builders<TractorDetail>.Filter.And(
                        Builders<TractorDetail>.Filter.Where(x => x.Id == tractorId),
                        Builders<TractorDetail>.Filter.Where(x => !x.IsDeleted)
                    );

                await mdbContext.TractorDetails.UpdateOneAsync(filter, updateFields);
            }
        }

        private async Task RemoveDriverScheduleIfExists(RegionDriverRemoveModel model)
        {
            var driverSCupdateFields = Builders<DriverScheduleShiftMapping>.Update
                .Set(t => t.IsActive, false)
                .Set(t => t.IsDeleted, true)
                 .Set(t => t.UpdatedBy, model.UserId)
                  .Set(t => t.UpdatedOn, DateTime.Now);

            var driverSCfilter = Builders<DriverScheduleShiftMapping>.Filter.And(
                    Builders<DriverScheduleShiftMapping>.Filter.Where(x => x.DriverId == model.DriverId),
                    Builders<DriverScheduleShiftMapping>.Filter.Where(x => x.IsActive),
                    Builders<DriverScheduleShiftMapping>.Filter.Where(x => !x.IsDeleted)
                );

            await mdbContext.DriverScheduleShiftMapping.UpdateOneAsync(driverSCfilter, driverSCupdateFields);
        }
        private async Task RemoveDriverIfExists(RegionDriverRemoveModel model)
        {
            var driverSCupdateFields = Builders<Driver>.Update
                .Set(t => t.IsActive, false)
                .Set(t => t.IsDeleted, true)
                 .Set(t => t.UpdatedBy, model.UserId)
                  .Set(t => t.UpdatedOn, DateTime.Now);

            var driverSCfilter = Builders<Driver>.Filter.And(
                    Builders<Driver>.Filter.Where(x => x.DriverId == model.DriverId),
                    Builders<Driver>.Filter.Where(x => x.IsActive),
                    Builders<Driver>.Filter.Where(x => !x.IsDeleted)
                );

            await mdbContext.Drivers.UpdateOneAsync(driverSCfilter, driverSCupdateFields);
        }
        private async Task RemoveDriverDSBIfExists(RegionDriverRemoveModel model)
        {
            try
            {

                List<ObjectId> objectIds = new List<ObjectId>();
                model.ScheduleBuilderIds.ForEach(x => objectIds.Add(ObjectId.Parse(x)));
                var scheduleBuildersInfo = await mdbContext.ScheduleBuilders
                                        .Find(t => t.IsActive && !t.IsDeleted && objectIds.Contains(t.Id) && t.Trips.Any(i => i.TfxDrivers.Any(x1 => x1.Id == model.DriverId))).Project(x1 => new { x1.Trips, x1.Id }).ToListAsync();
                foreach (var item in scheduleBuildersInfo)
                {
                    item.Trips.Where(x => x.TfxDrivers.Any(x1 => x1.Id == model.DriverId)).ToList().ForEach(x1 => x1.TfxDrivers = new List<DropdownDisplayExtendedItem>());
                    var driverSCupdateFields = Builders<ScheduleBuilder>.Update
                                  .Set(t => t.Trips, item.Trips)
                                   .Set(t => t.UpdatedBy, model.UserId)
                                    .Set(t => t.UpdatedOn, DateTime.Now);

                    var driverSCfilter = Builders<ScheduleBuilder>.Filter.And(
                            Builders<ScheduleBuilder>.Filter.Where(x => x.Id == item.Id),
                            Builders<ScheduleBuilder>.Filter.Where(x => x.IsActive),
                            Builders<ScheduleBuilder>.Filter.Where(x => !x.IsDeleted)
                        );

                    await mdbContext.ScheduleBuilders.UpdateOneAsync(driverSCfilter, driverSCupdateFields);
                }
            }
            catch (Exception ex)
            {

                LogManager.Logger.WriteException("RegionRepository", "RemoveDriverDSBIfExists", ex.Message, ex);
            }

        }
        public async Task<RegionFavProductModel> GetRegionFavouriteProducts(int? jobId, string regionId, int companyId)
        {
            var response = new RegionFavProductModel();
            try
            {
                if (!string.IsNullOrEmpty(regionId))
                {
                    var objectId = ObjectId.Parse(regionId);
                    response = await mdbContext.Regions.Find(t => t.Id == objectId && t.IsActive)
                                    .Project(t => new RegionFavProductModel
                                    {
                                        TfxProductTypeIds = t.TfxProductTypeIds,
                                        TfxFavProductTypeId = t.TfxFavProductTypeId ?? RegionFavProductType.None,
                                        TfxFuelTypeIds = t.TfxFuelTypeIds != null ? t.TfxFuelTypeIds.Select(t1 => new FreightModels.DropdownDisplayItem { Id = t1.Id, Code = t1.Code, Name = t1.Name }).ToList() : new List<FreightModels.DropdownDisplayItem>()
                                    }).FirstOrDefaultAsync();
                }
                else if (jobId > 0)
                {
                    response = await mdbContext.Regions.Find(t => t.TfxCompanyId == companyId && t.IsActive && !t.IsDeleted && t.TfxJobs.Any(x => x.Id == jobId))
                                    .Project(t => new RegionFavProductModel
                                    {
                                        TfxProductTypeIds = t.TfxProductTypeIds,
                                        TfxFavProductTypeId = t.TfxFavProductTypeId ?? RegionFavProductType.None,
                                        TfxFuelTypeIds = t.TfxFuelTypeIds != null ? t.TfxFuelTypeIds.Select(t1 => new FreightModels.DropdownDisplayItem { Id = t1.Id, Code = t1.Code, Name = t1.Name }).ToList() : new List<FreightModels.DropdownDisplayItem>()
                                    }).FirstOrDefaultAsync();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionRepository", "GetRegionFavouriteProducts", ex.Message, ex);
            }
            return response;
        }

        public bool IsPublishedDR(int companyId, string productIds, string orderIds)
        {
            long response = 0;
            try
            {
                if (string.IsNullOrEmpty(productIds) && string.IsNullOrEmpty(orderIds)) return false;

                DateTime currentDate = DateTime.Now.Date;
                var futureDR = mdbContext.ScheduleBuilders.Find(t => t.TfxCompanyId == companyId && t.IsActive && !t.IsDeleted && t.DateFilter >= currentDate)
                                            .Project(t => t.Trips.SelectMany(t1 => t1.DeliveryRequests).ToList()).ToList();
                var futureDRObjectIds = futureDR.SelectMany(t => t).ToList();
                if (futureDRObjectIds != null && futureDRObjectIds.Any())
                {
                    if (!string.IsNullOrEmpty(productIds))
                    {
                        var pList = productIds.Split(',').Select(x => int.Parse(x.Trim())).ToList();
                        response = mdbContext.DeliveryRequests.Find(t => t.TfxAssignedToCompanyId == companyId
                        && futureDRObjectIds.Contains(t.Id)
                        && t.IsActive
                        && pList.Contains(t.TfxProductTypeId)
                        && (t.Status == DeliveryReqStatus.ScheduleCreated || t.Status == DeliveryReqStatus.Draft)).CountDocuments();
                    }
                    else if (!string.IsNullOrEmpty(orderIds))
                    {
                        var oList = orderIds.Split(',').Select(x => int.Parse(x.Trim())).ToList();
                        response = mdbContext.DeliveryRequests.Find(t => t.TfxAssignedToCompanyId == companyId
                            && futureDRObjectIds.Contains(t.Id)
                            && t.IsActive
                            && (t.Status == DeliveryReqStatus.ScheduleCreated || t.Status == DeliveryReqStatus.Draft)
                            && t.TfxOrderId.HasValue
                            && oList.Contains(t.TfxOrderId.Value)).CountDocuments();
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionRepository", "IsPublishedDR", ex.Message, ex);
            }
            return response > 0;
        }

    }
}





