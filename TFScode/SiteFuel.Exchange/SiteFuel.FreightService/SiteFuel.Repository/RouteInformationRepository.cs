using MongoDB.Bson;
using MongoDB.Driver;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.FreightRepository.Mappers;
using SiteFuel.MdbDataAccess.Collections;
using SiteFuel.MdbDataAccess.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteFuel.FreightRepository
{
    public class RouteInformationRepository : IRouteInformationRepository
    {
        private readonly MdbContext mdbContext;
        public RouteInformationRepository(MdbContext _context)
        {
            mdbContext = _context;
        }
        public async Task<StatusModel> CreateRouteInformation(RouteInformationModel routeInformations)
        {
            var response = new StatusModel();
            var routeInfo = routeInformations.ToEntity();
            var routesDetails = (from routes in mdbContext.RouteInformations.AsQueryable()
                                 where routes.RegionId == routeInfo.RegionId && routes.IsActive && !routes.IsDeleted
                                 select routes
                                          ).ToList();
            if (routeInfo.TfxJobs.Count > 0)
            {
                using (var session = await mdbContext.Client.StartSessionAsync())
                {
                    session.StartTransaction();
                    try
                    {
                        int valid = ValidateRouteInformation(routeInfo, routesDetails);
                        if (valid == 1)
                        {
                            response.StatusCode = (int)Status.Warning;
                            response.StatusMessage = Resource.warningLocationExists;
                            return response;
                        }
                        else if (valid == 2)
                        {
                            response.StatusCode = (int)Status.Warning;
                            response.StatusMessage = Resource.warningRouteNameExists;
                            return response;
                        }
                        else
                        {
                            await mdbContext.RouteInformations.InsertOneAsync(routeInfo);
                            response.StatusCode = (int)Status.Success;
                            response.StatusMessage = Resource.routeCreationSuccessMsg;
                        }
                        await session.CommitTransactionAsync();
                    }
                    catch (Exception)
                    {
                        await session.AbortTransactionAsync();
                        throw;
                    }
                }

            }
            return response;
        }



        public async Task<StatusModel> DeleteRouteInformation(string RouteId, string RegionId, int CreatedBy)
        {
            var response = new StatusModel();
            using (var session = await mdbContext.Client.StartSessionAsync())
            {
                session.StartTransaction();
                try
                {
                    if (!string.IsNullOrEmpty(RouteId) && !string.IsNullOrEmpty(RegionId) && CreatedBy > 0)
                    {
                        await DeleteRouteInfo(RouteId, RegionId, CreatedBy);
                        response.StatusCode = (int)Status.Success;
                        response.StatusMessage = Resource.routeDeletionSuccessMsg;
                    }
                    await session.CommitTransactionAsync();
                }
                catch (Exception)
                {
                    await session.AbortTransactionAsync();
                    throw;
                }
            }
            return response;
        }

        private async Task DeleteRouteInfo(string RouteId, string RegionId, int CreatedBy)
        {
            //delete RouteInformations
            var routeInfoFilter = Builders<RouteInformations>.Filter.And(
                                   Builders<RouteInformations>.Filter
                                  .Where(x => x.Id == ObjectId.Parse(RouteId) && x.RegionId == ObjectId.Parse(RegionId)));
            UpdateDefinition<RouteInformations> updateFields = null;
            updateFields = Builders<RouteInformations>.Update
                    .Set(t => t.IsActive, false)
                    .Set(t => t.IsDeleted, true)
                    .Set(t => t.UpdatedBy, CreatedBy)
                    .Set(t => t.UpdatedDate, DateTime.Now);
            await mdbContext.RouteInformations.UpdateManyAsync(routeInfoFilter, updateFields);

            await DeleteRecurringDRRouteInfo(RouteId, RegionId);
        }
        private async Task DeleteRecurringDRRouteInfo(string RouteId, string RegionId)
        {
            var filter = Builders<RecurringSchedules>.Filter.Where(x => x.DeliveryRequests.Any(i => i.RouteInfo.Id == RouteId)
                            && x.DeliveryRequests.Any(i => i.CreatedRegionId == ObjectId.Parse(RegionId))
                            && x.DeliveryRequests.Count > 0 && x.DeliveryRequests.Any(i => i.RouteInfo != null)
                            );
            var update = Builders<RecurringSchedules>.Update.Set(x => x.DeliveryRequests[-1].RouteInfo, null);
            await mdbContext.RecurringSchedules.UpdateManyAsync(filter, update);
        }
        public List<FreightModels.DropdownDisplayItem> GetRegionLocationDetails(int companyId, string regionId)
        {
            var locationDetails = new List<FreightModels.DropdownDisplayItem>();
            var regionLocationDetails = (from regions in mdbContext.Regions.AsQueryable()
                                         where regions.Id == ObjectId.Parse(regionId) && regions.TfxCompanyId == companyId && regions.IsActive && !regions.IsDeleted
                                         select regions
                                           ).SelectMany(top => top.TfxJobs).ToList();
            var routeJobsDetails = (from regions in mdbContext.RouteInformations.AsQueryable()
                                    where regions.RegionId == ObjectId.Parse(regionId) && regions.TfxCompanyId == companyId && regions.IsActive && !regions.IsDeleted
                                    select regions
                                           ).SelectMany(top => top.TfxJobs).ToList();
            // refresh the  regionLocationDetails
            regionLocationDetails.RemoveAll(x => routeJobsDetails.Exists(y => y.Id == x.Id));
            if (regionLocationDetails.Count > 0)
            {
                locationDetails = regionLocationDetails.Select(t => new FreightModels.DropdownDisplayItem { Id = t.Id, Code = t.Code, Name = t.Name }).ToList();
            }
            return locationDetails.OrderBy(top => top.Name).ToList();
        }

        public List<RouteInformationModel> GetRouteInformations(int companyId, string regionId)
        {
            List<RouteInformationModel> routeInformations = new List<RouteInformationModel>();
            if (!string.IsNullOrEmpty(regionId))
            {
                routeInformations = mdbContext.RouteInformations.Find(top => top.RegionId == ObjectId.Parse(regionId) && top.IsActive == true && top.IsDeleted == false && top.TfxCompanyId == companyId)
                                 .Project(t => new RouteInformationModel
                                 {
                                     Id = t.Id.ToString(),
                                     Name = t.Name,
                                     RegionId = t.RegionId.ToString(),
                                     CreatedBy = t.CreatedBy,
                                     TfxCompanyId = t.TfxCompanyId,
                                     TfxJobs = t.TfxJobs.Select(top => new TfxJobsDetailsModel { Id = top.Id, SequenceNo = top.SequenceNo, Name = top.Name, Code = top.Code }).OrderBy(top => top.SequenceNo).ToList(),
                                     ShiftInfo = t.ShiftInfo != null ? t.ShiftInfo.ToEntity() : null,
                                 }).SortByDescending(top => top.CreatedDate).ToList();
                routeInformations.ForEach(item => item.ShiftInfoDetails = item.ShiftInfo != null ? string.Format(Resource.valRouteShiftInfo, item.ShiftInfo.DriverRowIndex, item.ShiftInfo.DriverColIndex) : string.Empty);
            }
            return routeInformations;
        }

        public async Task<StatusModel> UpdateRouteInformation(RouteInformationModel routeInformations)
        {
            var response = new StatusModel();
            using (var session = await mdbContext.Client.StartSessionAsync())
            {
                session.StartTransaction();
                try
                {
                    var routeInfo = routeInformations.ToEntity();
                    var routesDetails = (from routes in mdbContext.RouteInformations.AsQueryable()
                                         where routes.RegionId == routeInfo.RegionId && routes.IsActive && !routes.IsDeleted
                                         && routes.Id != routeInfo.Id
                                         select routes
                                                  ).ToList();
                    int valid = ValidateRouteInformation(routeInfo, routesDetails);
                    if (valid == 1)
                    {
                        response.StatusCode = (int)Status.Warning;
                        response.StatusMessage = Resource.warningLocationExists;
                        return response;
                    }
                    else if (valid == 2)
                    {
                        response.StatusCode = (int)Status.Warning;
                        response.StatusMessage = Resource.warningRouteNameExists;
                        return response;
                    }
                    else
                    {
                        //update RouteInformations
                        var routeInfoFilter = Builders<RouteInformations>.Filter.And(
                                               Builders<RouteInformations>.Filter
                                              .Where(x => x.Id == routeInfo.Id && x.RegionId == routeInfo.RegionId));
                        UpdateDefinition<RouteInformations> updateFields = null;
                        updateFields = Builders<RouteInformations>.Update
                                .Set(t => t.UpdatedBy, routeInfo.CreatedBy)
                                .Set(t => t.Name, routeInfo.Name)
                                .Set(t => t.TfxJobs, routeInfo.TfxJobs)
                                .Set(t => t.UpdatedDate, DateTime.Now);
                        await mdbContext.RouteInformations.UpdateManyAsync(routeInfoFilter, updateFields);

                        //update RouteInfo(name of route) in deliveryRequests
                        await UpateDeliveryRequestRouteInfo(routeInfo);
                        await UpateRecurringDRRouteInfo(routeInfo);
                        //update Location Sequence number in Delivery requests if any changes
                        await UpateDeliveryRequestRouteLocationSeqInfo(routeInfo);
                        await UpateRecurringDRRouteInfoLocationSeqInfo(routeInfo);
                        response.StatusCode = (int)Status.Success;
                        response.StatusMessage = Resource.routeUpdateSuccessMsg;
                    }
                    await session.CommitTransactionAsync();
                }
                catch (Exception)
                {
                    await session.AbortTransactionAsync();
                    throw;
                }
            }
            return response;
        }

        private async Task UpateDeliveryRequestRouteInfo(RouteInformations routeInfo)
        {
            var deliveryReqFilter = Builders<DeliveryRequest>.Filter.And(
                                                           Builders<DeliveryRequest>.Filter
                                                          .Where(x => x.RouteInfo.Id == routeInfo.Id.ToString() && x.RouteInfo != null
                                                           && x.IsActive && !x.IsDeleted
                                                          ));
            UpdateDefinition<DeliveryRequest> updateDeliveryFields = null;
            updateDeliveryFields = Builders<DeliveryRequest>.Update
                    .Set(t => t.RouteInfo.Name, routeInfo.Name);
            await mdbContext.DeliveryRequests.UpdateManyAsync(deliveryReqFilter, updateDeliveryFields);
        }
        private async Task UpateRecurringDRRouteInfo(RouteInformations routeInfo)
        {
            var filter = Builders<RecurringSchedules>.Filter.Where(x => x.DeliveryRequests.Any(i => i.RouteInfo.Id == routeInfo.Id.ToString())
                                        && x.DeliveryRequests.Count > 0 && x.DeliveryRequests.Any(i => i.RouteInfo != null));
            var update = Builders<RecurringSchedules>.Update.Set(x => x.DeliveryRequests[-1].RouteInfo.Name, routeInfo.Name);
            await mdbContext.RecurringSchedules.UpdateManyAsync(filter, update);

        }
        private async Task UpateRecurringDRRouteInfoLocationSeqInfo(RouteInformations routeInfo)
        {
            foreach (var item in routeInfo.TfxJobs)
            {
                var filter = Builders<RecurringSchedules>.Filter.Where(x => x.DeliveryRequests.Any(i => i.RouteInfo.Id == routeInfo.Id.ToString())
                                                       && x.DeliveryRequests.Count > 0 && x.DeliveryRequests.Any(i => i.RouteInfo != null && i.TfxJobId == item.Id)
                                                       && x.IsActive && !x.IsDeleted);
                var update = Builders<RecurringSchedules>.Update.Set(x => x.DeliveryRequests[-1].RouteInfo.LocationSeqNo, item.SequenceNo);
                await mdbContext.RecurringSchedules.UpdateManyAsync(filter, update);
            }


        }

        private async Task UpateDeliveryRequestRouteLocationSeqInfo(RouteInformations routeInfo)
        {

            foreach (var item in routeInfo.TfxJobs)
            {
                var filter = Builders<DeliveryRequest>.Filter.Where(x => x.RouteInfo != null && x.RouteInfo.Id == routeInfo.Id.ToString()
                                                        && x.TfxJobId == item.Id && x.IsActive && !x.IsDeleted);
                var update = Builders<DeliveryRequest>.Update.Set(x => x.RouteInfo.LocationSeqNo, item.SequenceNo);
                await mdbContext.DeliveryRequests.UpdateManyAsync(filter, update);
            }

        }
        public List<DropdownDisplayExtended> GetRouteInformations(string regionId)
        {
            var routeInfoDetails = mdbContext.RouteInformations.Find(top => top.RegionId == ObjectId.Parse(regionId) && top.IsActive == true && top.IsDeleted == false)
                             .Project(t => new DropdownDisplayExtended
                             {
                                 Id = t.Id.ToString(),
                                 Name = t.Name,
                             }).ToList();
            return routeInfoDetails.OrderBy(top => top.Name).ToList();
        }
        public List<RouteCustomerLocationModel> GetRouteInformations(List<string> regionId)
        {
            List<ObjectId> objectIds = new List<ObjectId>();
            regionId.ForEach(x => objectIds.Add(ObjectId.Parse(x)));
            var routeInfoDetails = mdbContext.RouteInformations.Find(top => objectIds.Contains(top.RegionId) && top.IsActive == true && top.IsDeleted == false)
                            .Project(t => new RouteCustomerLocationModel
                            {
                                Id = t.Id.ToString(),
                                Name = t.Name,
                                RegionId = t.RegionId.ToString(),
                                TfxJobs = t.TfxJobs.Select(top => new TfxJobsDetailsModel { Id = top.Id, SequenceNo = top.SequenceNo, Name = top.Name }).ToList(),
                            }).ToList();
            return routeInfoDetails;
        }
        public async Task<StatusModel> AssignTPOJobToRoute(JobToRegionAssignViewModel jobToUpdate)
        {
            var response = new StatusModel(Status.Failed);
            try
            {
                //Find the routeId with given job and company
                var routeId = await GetRoutesIdForJob(jobToUpdate.JobId, jobToUpdate.CompanyId);
                if (routeId == jobToUpdate.RouteId)// No Assignment
                {
                    response.StatusCode = (int)Status.Success;
                    return response;
                }
                //Implies No Routes Assigned To Job. Current Assignment will be removed
                if ((jobToUpdate.RouteId == null || jobToUpdate.RouteId == string.Empty) && routeId != null)
                {
                    await UpdateRouteInformation(jobToUpdate, routeId);
                    response.StatusCode = (int)Status.Success;
                    response.StatusMessage = "Job " + jobToUpdate.JobName + " Succesfully Removed from Routes";
                    return response;
                }
                if ((routeId != jobToUpdate.RouteId) && routeId != null)//Implies new Routes is selected for given job 
                {
                    //First Remove the given job from existing region as one job can belong to one region only 
                    await UpdateRouteInformation(jobToUpdate, routeId);
                    return await AddJobToRouteInfo(jobToUpdate);
                }
                else
                {
                    response = await AddJobToRouteInfo(jobToUpdate);//First time job Assignment
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RouteInformationRepository", "AssignTPOJobToRoute", ex.Message, ex);
                response.StatusCode = (int)Status.Failed;
                response.StatusMessage = Status.Failed.ToString();
            }
            return response;
        }
        private static int ValidateRouteInformation(RouteInformations routeInfo, List<RouteInformations> routesDetails)
        {
            var TfxJobsDetails = routesDetails.SelectMany(top => top.TfxJobs).ToList();
            bool hasMatch = TfxJobsDetails.Intersect(routeInfo.TfxJobs).Any();
            bool hasNameMatch = routesDetails.Any(top => top.Name.Trim().ToLower() == routeInfo.Name.Trim().ToLower());
            if (hasMatch)
            {
                return 1;
            }
            else if (hasNameMatch)
            {

                return 2;
            }
            return 0;

        }
        public async Task<string> GetRoutesIdForJob(int jobId, int companyId)
        {
            string routeId = string.Empty;
            try
            {
                var routesDetails = await (mdbContext.RouteInformations.FindAsync(t => t.TfxCompanyId == companyId &&
                                                                t.IsActive && !t.IsDeleted &&
                                                                t.TfxJobs.Any(t1 => t1.Id == jobId)));

                if (routesDetails != null)
                {
                    routeId = routesDetails.ToList().Select(t => t.Id.ToString()).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {

                LogManager.Logger.WriteException("RouteInformationRepository", "GetRoutesIdForJob", ex.Message, ex);
            }
            return routeId;

        }
        private async Task UpdateRouteInformation(JobToRegionAssignViewModel jobToUpdate, string routeId)
        {
            var objectId = ObjectId.Parse(routeId);
            var filter = Builders<RouteInformations>.Filter.And(
                    Builders<RouteInformations>.Filter.Where(x => x.Id == objectId),
                    Builders<RouteInformations>.Filter.Where(x => x.TfxCompanyId == jobToUpdate.CompanyId),
                    Builders<RouteInformations>.Filter.Where(x => x.IsActive),
                    Builders<RouteInformations>.Filter.Where(x => !x.IsDeleted)
                );
            var update = Builders<RouteInformations>.Update.PullFilter(p => p.TfxJobs, f => f.Id == jobToUpdate.JobId);
            await mdbContext.RouteInformations.FindOneAndUpdateAsync(filter, update);
        }
        public async Task<StatusModel> AddJobToRouteInfo(JobToRegionAssignViewModel jobToUpdate)
        {
            var response = new StatusModel(Status.Failed);
            try
            {
                int sequenceNo = 0;
                var routeInfo = mdbContext.RouteInformations.Find(top => top.Id == ObjectId.Parse(jobToUpdate.RouteId)).FirstOrDefault();
                if (routeInfo != null)
                {
                    sequenceNo = routeInfo.TfxJobs.Max(top => top.SequenceNo) + 1;
                }
                MdbDataAccess.Collections.TfxJobsDetails TfxJob = new MdbDataAccess.Collections.TfxJobsDetails()
                {
                    Id = jobToUpdate.JobId,
                    Code = null,
                    Name = jobToUpdate.JobName,
                    SequenceNo = sequenceNo
                };
                var filter = Builders<RouteInformations>.Filter.And(
                                   Builders<RouteInformations>.Filter.Where(x => x.Id == ObjectId.Parse(jobToUpdate.RouteId)),
                                    Builders<RouteInformations>.Filter.Where(x => x.TfxCompanyId == jobToUpdate.CompanyId),
                                       Builders<RouteInformations>.Filter.Where(x => x.IsActive),
                                       Builders<RouteInformations>.Filter.Where(x => !x.IsDeleted)
                                   );
                var update = Builders<RouteInformations>.Update.Push("TfxJobs", TfxJob);
                await mdbContext.RouteInformations.FindOneAndUpdateAsync(filter, update);

                response.StatusCode = (int)Status.Success;
                response.StatusMessage = "Job " + jobToUpdate.JobName + " assigned successfully to Route Info.";

            }
            catch (Exception ex)
            {
                response.StatusCode = (int)Status.Failed;
                LogManager.Logger.WriteException("RouteInformationRepository", "AddJobToRouteInfo", ex.Message, ex);

            }
            return response;
        }
        public List<FreightModels.DropdownDisplayItem> GetRouteLocationDetails(string Id, string regionId)
        {
            var locationDetails = new List<FreightModels.DropdownDisplayItem>();
            var regionLocationDetails = (from regions in mdbContext.Regions.AsQueryable()
                                         where regions.Id == ObjectId.Parse(regionId) && regions.IsActive && !regions.IsDeleted
                                         select regions
                                           ).SelectMany(top => top.TfxJobs).ToList();
            var routeJobsDetails = (from routes in mdbContext.RouteInformations.AsQueryable()
                                    where routes.RegionId == ObjectId.Parse(regionId) && routes.IsActive && !routes.IsDeleted
                                    select routes
                                           ).SelectMany(top => top.TfxJobs).ToList();
            // refresh the  regionLocationDetails
            var getrouteJobsDetails = (from routes in mdbContext.RouteInformations.AsQueryable()
                                       where routes.Id == ObjectId.Parse(Id) && routes.IsActive && !routes.IsDeleted
                                       select routes
                                           ).SelectMany(top => top.TfxJobs).ToList();
            regionLocationDetails.RemoveAll(x => routeJobsDetails.Exists(y => y.Id == x.Id));
            if (regionLocationDetails.Count > 0)
            {
                locationDetails = regionLocationDetails.Select(t => new FreightModels.DropdownDisplayItem { Id = t.Id, Code = t.Code, Name = t.Name }).ToList();
            }
            getrouteJobsDetails.ForEach(x => locationDetails.Add(new FreightModels.DropdownDisplayItem { Id = x.Id, Name = x.Name, Code = x.Code }));
            return locationDetails.OrderBy(top => top.Name).ToList();
        }
        public async Task<StatusModel> UpdateShiftInfo(RouteInformationModel jobToUpdate)
        {
            var response = new StatusModel();
            using (var session = await mdbContext.Client.StartSessionAsync())
            {
                session.StartTransaction();
                try
                {
                    if (!string.IsNullOrEmpty(jobToUpdate.RegionId) && jobToUpdate.ShiftInfo != null)
                    {
                        //update RouteInformations
                        var objectId = ObjectId.Parse(jobToUpdate.Id);
                        var filter = Builders<RouteInformations>.Filter.And(
                                Builders<RouteInformations>.Filter.Where(x => x.Id == objectId),
                                Builders<RouteInformations>.Filter.Where(x => x.IsActive),
                                Builders<RouteInformations>.Filter.Where(x => !x.IsDeleted)
                            );
                        var shiftInfo = jobToUpdate.ShiftInfo.ToEntity();

                        UpdateDefinition<RouteInformations> updateFields = null;
                        updateFields = Builders<RouteInformations>.Update
                                .Set(t => t.ShiftInfo, shiftInfo)
                                .Set(t => t.UpdatedBy, jobToUpdate.CreatedBy)
                                .Set(t => t.UpdatedDate, DateTime.Now);
                        await mdbContext.RouteInformations.UpdateManyAsync(filter, updateFields);
                    }
                    await session.CommitTransactionAsync();
                    response.StatusCode = (int)Status.Success;
                    response.StatusMessage = Resource.routeShiftInfoSuccessMsg;
                }
                catch (Exception)
                {
                    await session.AbortTransactionAsync();
                    throw;
                }
            }
            return response;
        }
        public List<InvoiceRouteInfo> GetInvoiceRouteInfo(List<string> deliveryReqId)
        {
            var response = new List<InvoiceRouteInfo>();
            List<ObjectId> objectIds = new List<ObjectId>();
            deliveryReqId.ForEach(x => objectIds.Add(ObjectId.Parse(x)));
            var deliveryReqRouteInfo = (from deliveryReq in mdbContext.DeliveryRequests.AsQueryable()
                                        where objectIds.Contains(deliveryReq.Id) && deliveryReq.IsActive && !deliveryReq.IsDeleted
                                        && deliveryReq.RouteInfo != null
                                        select new
                                        {
                                            deliveryReq.Id,
                                            deliveryReq.RouteInfo
                                        }).ToList();
            deliveryReqRouteInfo.ForEach(x => response.Add(new InvoiceRouteInfo { DeliveryReqId = x.Id.ToString(), Id = x.RouteInfo.Id.ToString(), Name = x.RouteInfo.Name }));
            return response;

        }
        public async Task<string> GetRouteIdForJob(int jobId, int companyId, string regionId)
        {
            ObjectId regionObjId = string.IsNullOrEmpty(regionId) ? ObjectId.Empty : ObjectId.Parse(regionId);
            string routeId = string.Empty;
            if (!string.IsNullOrEmpty(regionId))
            {
                var routeInfo = await (mdbContext.RouteInformations.FindAsync(t => t.TfxCompanyId == companyId && t.RegionId == regionObjId &&
                                                                   t.IsActive && !t.IsDeleted &&
                                                                   t.TfxJobs.Any(t1 => t1.Id == jobId)));
                if (routeInfo != null)
                {
                    routeId = routeInfo.ToList().Select(t => t.Id.ToString()).FirstOrDefault();
                }
            }
            return routeId;
        }
    }
}

