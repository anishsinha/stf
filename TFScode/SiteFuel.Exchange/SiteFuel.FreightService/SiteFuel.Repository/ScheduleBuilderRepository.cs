using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Core.Utilities;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.FreightModels;
using SiteFuel.FreightModels.ScheduleBuilder;
using SiteFuel.FreightRepository.Mappers;
using SiteFuel.MdbDataAccess.Collections;
using SiteFuel.MdbDataAccess.DbContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using TrueFill.ExchangeDataAccess.DataAccess;
using DropdownDisplayItem = SiteFuel.FreightModels.DropdownDisplayItem;

namespace SiteFuel.FreightRepository
{
    public class ScheduleBuilderRepository : IScheduleBuilderRepository
    {
        private readonly MdbContext mdbContext;
        public ScheduleBuilderRepository()
        {
            if (mdbContext == null)
            {
                mdbContext = new MdbContext();
            }
        }
        public ScheduleBuilderRepository(MdbContext _context)
        {
            mdbContext = _context;
        }
        public async Task<List<DropdownDisplayExtended>> GetRegions(int userId)
        {
            List<DropdownDisplayExtended> response = new List<DropdownDisplayExtended>();
            var regionList = await mdbContext.Regions.Find(t => t.IsActive && !t.IsDeleted && t.TfxDispatchers.Any(t1 => t1.Id == userId)).Project(t => new { t.Id, t.Name }).SortBy(t => t.Name).ToListAsync();
            regionList.ForEach(t => response.Add(new DropdownDisplayExtended() { Id = t.Id.ToString(), Name = t.Name }));
            return response;
        }

        public async Task<StatusModel> CheckAndLockDrs(LockDrModel model)
        {
            StatusModel response = new StatusModel();

            List<ObjectId> drObjectIds = model.DrIds.Select(t => ObjectId.Parse(t)).ToList();
            var drs = await mdbContext.DeliveryRequests.Find(t => t.IsActive && !t.IsDeleted && drObjectIds.Contains(t.Id)).Project(t => new { t.Id, LockedBy = t.LockedBy }).ToListAsync();
            if (!drs.Any())
            {
                response.StatusCode = (int)Status.Failed;
                response.StatusMessage = Resource.valMessageDeliveryReqDeleted;
                return response;
            }
            if (drs.Any(t => t.LockedBy != null && t.LockedBy.Id != model.User.Id))
            {
                response.StatusCode = (int)Status.Failed;
                response.StatusMessage = string.Format(drs.FirstOrDefault(t => t.LockedBy != null).LockedBy.Name, "is working on DR(s) of this location. Multiple users can't work on same DR(s) at a time");
                return response;
            }
            else
            {
                using (var session = await mdbContext.Client.StartSessionAsync())
                {
                    try
                    {
                        session.StartTransaction();
                        var filter = Builders<DeliveryRequest>.Filter.And(
                                  Builders<DeliveryRequest>.Filter.Where(x => drObjectIds.Contains(x.Id)),
                                   Builders<DeliveryRequest>.Filter.Where(x => x.IsActive),
                                   Builders<DeliveryRequest>.Filter.Where(x => !x.IsDeleted)
                               );
                        var update = Builders<DeliveryRequest>.Update.Set("LockedBy", model.User);
                        await mdbContext.DeliveryRequests.UpdateManyAsync(filter, update);
                        session.CommitTransaction();
                        response.StatusCode = (int)Status.Success;
                    }
                    catch (Exception ex)
                    {
                        LogManager.Logger.WriteException("ScheduleBuilderDomain", "CheckAndLockDrs", ex.Message, ex);
                        session.AbortTransaction();
                    }
                }
            }

            return response;
        }

        public async Task<StatusModel> CheckAndReleaseDrs(LockDrModel model)
        {
            StatusModel response = new StatusModel();

            List<ObjectId> drObjectIds = model.DrIds.Select(t => ObjectId.Parse(t)).ToList();
            var drs = await mdbContext.DeliveryRequests.Find(t => t.IsActive && !t.IsDeleted && drObjectIds.Contains(t.Id)).Project(t => new { t.Id, LockedBy = t.LockedBy }).ToListAsync();
            if (!drs.Any())
            {
                response.StatusCode = (int)Status.Failed;
                response.StatusMessage = Resource.valMessageDeliveryReqDeleted;
                return response;
            }
            var lockedDrs = drs.Where(t => t.LockedBy != null && t.LockedBy.Id == model.User.Id).Select(t => t.Id).ToList();

            using (var session = await mdbContext.Client.StartSessionAsync())
            {
                try
                {
                    session.StartTransaction();
                    var filter = Builders<DeliveryRequest>.Filter.And(
                              Builders<DeliveryRequest>.Filter.Where(x => lockedDrs.Contains(x.Id)),
                               Builders<DeliveryRequest>.Filter.Where(x => x.IsActive),
                               Builders<DeliveryRequest>.Filter.Where(x => !x.IsDeleted)
                           );
                    var update = Builders<DeliveryRequest>.Update.Set("LockedBy", BsonNull.Value);
                    await mdbContext.DeliveryRequests.UpdateManyAsync(filter, update);
                    session.CommitTransaction();
                    response.StatusCode = (int)Status.Success;
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("ScheduleBuilderDomain", "CheckAndReleaseDrs", ex.Message, ex);
                    session.AbortTransaction();
                }
            }

            return response;
        }


        public async Task<RegionDetailModel> GetRegionDetails(string regionId)
        {
            RegionDetailModel regionModel = new RegionDetailModel();
            if (!string.IsNullOrEmpty(regionId))
            {
                ObjectId objectId = ObjectId.Parse(regionId);
                var region = await mdbContext.Regions.Find(t => t.Id == objectId).Project(t => new { t.TfxTrailers, t.TfxDrivers, t.Id, t.Name, t.SlotPeriod, t.TfxCompanyId }).FirstOrDefaultAsync();
                if (region != null)
                {
                    if (region.TfxTrailers != null && region.TfxTrailers.Any())
                    {
                        List<TrailerModel> trailers = GetTrailersForRegion(region.TfxTrailers);
                        if (trailers != null && trailers.Any())
                        {
                            regionModel.Trailers = trailers;
                            var TrailerIds = trailers.Select(t => t.Id).ToList();
                            var fuelRetainDetails = await GetTrailerFuelRetainDetails(TrailerIds);
                            if (fuelRetainDetails.Any())
                            {
                                Parallel.ForEach(regionModel.Trailers, t =>
                                {
                                    var retains = fuelRetainDetails.Where(retain => retain.TrailerId == t.TrailerId).ToList();
                                    t.RetainFuel = retains;
                                });
                            }
                        }
                    }
                    if (region.TfxDrivers != null && region.TfxDrivers.Any())
                    {
                        List<DriverAdditionalDetailsViewModel> drivers = GetDriversForRegion(region.TfxDrivers);
                        if (drivers != null && drivers.Any())
                        {
                            regionModel.Drivers = drivers;
                        }
                    }
                    regionModel.Id = region.Id.ToString();
                    regionModel.Name = region.Name;
                    regionModel.ShiftSlotTime = region.SlotPeriod;
                    regionModel.CreatedByCompanyId = region.TfxCompanyId;
                }
            }
            return regionModel;
        }

        public async Task<StatusModel> CancelSchedules(CancelScheduleModel model)
        {
            var response = new StatusModel();
            var parentObjectIds = new List<ObjectId>();
            var deliveryRequests = new List<ObjectId>();
            var deliveryRequestIds = model.DeliveryRequestIds.Where(t => !string.IsNullOrWhiteSpace(t)).ToList();
            var objectIds = deliveryRequestIds.Select(t => ObjectId.Parse(t)).ToList();
            if (model.IsCancelAll)
            {
                var groupedParentIds = model.GroupedParentDrIds.Where(t => !string.IsNullOrWhiteSpace(t)).ToList();
                parentObjectIds = groupedParentIds.Select(t => ObjectId.Parse(t)).ToList();
            }
            if (model.IsCancelAll)
            {
                deliveryRequests = await mdbContext.DeliveryRequests.Find(t => (objectIds.Contains(t.Id) || (t.GroupParentDRId != null && parentObjectIds.Contains(t.GroupParentDRId.Value)))
                && t.TfxTrackableScheduleId > 0
                && t.TfxScheduleEnrouteStatus != (int)EnrouteDeliveryStatus.CompletedDrop
                                                                      && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.Completed
                                                                      && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.CompletedLate
                                                                      && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.RescheduledCompleted
                                                                      && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.Canceled
                                                                      && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.PreLoadBolCompleted
                                                                      && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.Diverted).Project(t => t.Id).ToListAsync();
            }
            else
            {
                deliveryRequests = await mdbContext.DeliveryRequests.Find(t => (objectIds.Contains(t.Id))
                   && t.TfxTrackableScheduleId > 0
                   && t.TfxScheduleEnrouteStatus != (int)EnrouteDeliveryStatus.CompletedDrop
                                                                         && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.Completed
                                                                         && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.CompletedLate
                                                                         && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.RescheduledCompleted
                                                                         && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.Canceled
                                                                         && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.PreLoadBolCompleted
                                                                         && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.Diverted).Project(t => t.Id).ToListAsync();

            }
            var delfilter = Builders<DeliveryRequest>.Filter.Where(t => (objectIds.Contains(t.Id) || (t.GroupParentDRId != null && parentObjectIds.Contains(t.GroupParentDRId.Value)))
             && t.TfxTrackableScheduleId > 0 && t.TfxScheduleEnrouteStatus != (int)EnrouteDeliveryStatus.CompletedDrop
                                                                   && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.Completed
                                                                   && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.CompletedLate
                                                                   && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.RescheduledCompleted
                                                                   && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.Canceled
                                                                   && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.PreLoadBolCompleted
                                                                   && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.Diverted);

            var update = Builders<DeliveryRequest>.Update.Set(t => t.TfxScheduleStatus, (int)TrackableDeliveryScheduleStatus.Canceled)
                .Set(t => t.TfxScheduleStatusName, "Canceled")
                .Set(t => t.UpdatedBy, model.DriverId)
                .Set(t => t.UpdatedOn, DateTimeOffset.Now);
            await mdbContext.DeliveryRequests.UpdateManyAsync(delfilter, update);

            response.StatusCode = (int)Status.Success;
            response.StatusMessage = Resource.errMessageDeliveryScheduleCancelSuccess;
            response.EntityIds = deliveryRequests.Select(t => t.ToString()).ToList();
            return response;
        }

        public async Task<List<int>> GetCarrierSuppliersBySiteId(int jobId, int carrierCompanyId)
        {
            List<int> response = await mdbContext.CarrierJobs.Find(t => t.IsActive && t.TfxJobId == jobId && t.TfxCarrierCompanyId == carrierCompanyId).Project(t => t.TfxSupplierCompanyId).ToListAsync();
            return response;
        }

        public async Task<ScheduleBuilderViewModel> GetScheduleBuilderDetails(int companyId, int userId, string regionId, string date, int sbView, string scheduleBuilderId, int sbDsbView = 2, bool IsDsbDriverSchedule = false)
        {
            ScheduleBuilderViewModel response = new ScheduleBuilderViewModel();

            DateTime dateFilter = DateTimeOffset.Now.Date;
            if (!string.IsNullOrWhiteSpace(date))
            {
                dateFilter = Convert.ToDateTime(date).Date;
            }
            ScheduleBuilder scheduleBuilder = await mdbContext.ScheduleBuilders
                                    .Find(t => t.IsActive && !t.IsDeleted && t.TfxCompanyId == companyId
                                        && (regionId == null || regionId == "" || t.RegionId == regionId)
                                        && t.DateFilter == dateFilter).SortByDescending(t => t.Id).FirstOrDefaultAsync();

            response = await GetScheduleBuilderInfo(companyId, userId, regionId, date, sbView, sbDsbView, response, dateFilter, scheduleBuilder, IsDsbDriverSchedule);
            return response;
        }


        public async Task<DSBSaveModel> IsValidScheduleBuilder(DSBSaveModel scheduleBuilder)
        {
            StatusModel statusModel = null;
            if (scheduleBuilder.ObjectFilter == 2)
            {
                statusModel = await IsValidLoadTrailerView(scheduleBuilder);
            }
            else
            {
                statusModel = await IsValidLoadDriverView(scheduleBuilder);
            }
            scheduleBuilder.StatusCode = statusModel.StatusCode;
            scheduleBuilder.StatusMessage = statusModel.StatusMessage;
            return scheduleBuilder;
        }

        public async Task<ScheduleBuilderResponseModel> IsValidTimeStamp(string selectedDate, string regionid, int companyId, long lastTimeStamp)
        {
            ScheduleBuilderResponseModel response = new ScheduleBuilderResponseModel();
            DateTime date = Convert.ToDateTime(selectedDate).Date;
            var existingSbTimeStamp = await mdbContext.ScheduleBuilders.Find(t => t.TfxCompanyId == companyId
                                                                            && t.RegionId == regionid
                                                                            && t.DateFilter == date).Project(t => t.TimeStamp).FirstOrDefaultAsync();
            if (existingSbTimeStamp != 0 && existingSbTimeStamp != lastTimeStamp)
            {
                response.StatusMessage = Resource.valMessageSbNotRefreshed;
                response.StatusCode = (int)Status.Failed;
                response.TimeStamp = existingSbTimeStamp;
                return response;
            }
            response.StatusCode = (int)Status.Success;
            return response;
        }

        public async Task<List<DSBSaveModel>> SaveScheduleBuilder(List<DSBSaveModel> scheduleBuilders)
        {
            using (var session = await mdbContext.Client.StartSessionAsync())
            {
                try
                {
                    session.StartTransaction();
                    for (int index = 0; index < scheduleBuilders.Count; index++)
                    {
                        var scheduleBuilder = scheduleBuilders[index];
                        DateTime date = Convert.ToDateTime(scheduleBuilder.Date).Date;
                        StatusModel dsbViewstatus = await IsValidDSBView(scheduleBuilder);
                        if (dsbViewstatus.StatusCode != (int)Status.Success)
                        {
                            scheduleBuilder.StatusMessage = dsbViewstatus.StatusMessage;
                            scheduleBuilder.StatusCode = dsbViewstatus.StatusCode;
                            await session.AbortTransactionAsync();
                            return scheduleBuilders;
                        }
                        StatusModel status = await IsValidLoadDriverView(scheduleBuilder);
                        if (status.StatusCode != (int)Status.Success)
                        {
                            scheduleBuilder.StatusMessage = status.StatusMessage;
                            scheduleBuilder.StatusCode = status.StatusCode;
                            await session.AbortTransactionAsync();
                            return scheduleBuilders;
                        }
                        ScheduleBuilder dsbEntity = null;
                        if (string.IsNullOrWhiteSpace(scheduleBuilder.Id))
                        {
                            dsbEntity = await CreateScheduleBuilder(scheduleBuilder);
                        }
                        else
                        {
                            dsbEntity = await UpdateScheduleBuilder(scheduleBuilder);
                        }
                        await session.CommitTransactionAsync();
                        UpdateDsbModel(scheduleBuilder, dsbEntity);
                        scheduleBuilder.StatusCode = (int)Status.Success;
                        scheduleBuilder.StatusMessage = GetStatusMessage(scheduleBuilder);
                    }
                }
                catch (Exception)
                {
                    await session.AbortTransactionAsync();
                    scheduleBuilders.ForEach(t => t.StatusCode = (int)Status.Failed);
                    scheduleBuilders.ForEach(t => t.StatusMessage = Resource.valMessageSbSaveFailed);
                    throw;
                }
            }
            return scheduleBuilders;
        }

        public async Task<DSBSaveModel> AssignDriverAndTrailer(DSBSaveModel scheduleBuilder)
        {
            using (var session = await mdbContext.Client.StartSessionAsync())
            {
                session.StartTransaction();
                try
                {
                    StatusModel status = await IsValidLoadDriverView(scheduleBuilder);
                    if (status.StatusCode != (int)Status.Success)
                    {
                        scheduleBuilder.StatusMessage = status.StatusMessage;
                        scheduleBuilder.StatusCode = status.StatusCode;
                        await session.AbortTransactionAsync();
                        return scheduleBuilder;
                    }
                    ScheduleBuilder dsbEntity = null;

                    if (string.IsNullOrWhiteSpace(scheduleBuilder.Id))
                    {
                        dsbEntity = await CreateScheduleBuilder(scheduleBuilder);
                    }
                    else
                    {
                        dsbEntity = await UpdateScheduleBuilder(scheduleBuilder);
                    }
                    if (scheduleBuilder.Status == (int)DSBMethod.DriverAssignment)
                    {
                        await UpdateDeliveryReqCompartmentInfo(scheduleBuilder);
                    }
                    await session.CommitTransactionAsync();
                    UpdateDsbModel(scheduleBuilder, dsbEntity);
                    scheduleBuilder.StatusCode = (int)Status.Success;
                    scheduleBuilder.StatusMessage = Resource.valMessageDriverAssignmentSuccess;
                }
                catch (Exception)
                {
                    await session.AbortTransactionAsync();
                    scheduleBuilder.StatusCode = (int)Status.Failed;
                    scheduleBuilder.StatusMessage = Resource.valMessageSbSaveFailed;
                    throw;
                }
            }
            return scheduleBuilder;
        }

        private async Task UpdateDeliveryReqCompartmentInfo(DSBSaveModel scheduleBuilder)
        {
            var isTrailerExists = scheduleBuilder.Trips.Select(top => top.IsTrailerExists).FirstOrDefault();
            if (isTrailerExists)
            {
                foreach (var item in scheduleBuilder.Trips)
                {
                    foreach (var delitem in item.DeliveryRequests)
                    {
                        var delfilter = Builders<DeliveryRequest>.Filter.Where(t => t.Id == ObjectId.Parse(delitem.Id) && t.TfxScheduleEnrouteStatus != (int)EnrouteDeliveryStatus.PickUpCompleted && t.TfxScheduleEnrouteStatus != (int)EnrouteDeliveryStatus.CompletedDrop
                                                                   && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.Completed
                                                                   && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.CompletedLate
                                                                   && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.RescheduledCompleted
                                                                   && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.Canceled
                                                                   && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.RescheduledMissed
                                                                   && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.Missed
                                                                   && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.PreLoadBolCompleted
                                                                   && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.Diverted);
                        var compartmentInfo = delitem.Compartments.ToCloneEntity();
                        var update = Builders<DeliveryRequest>.Update.Set(t => t.Compartments, compartmentInfo);
                        await mdbContext.DeliveryRequests.UpdateOneAsync(delfilter, update);

                    }

                }
            }
        }
        public async Task<StatusModel> IsValidLoadDriverView(DSBSaveModel scheduleBuilder)
        {

            StatusModel response = new StatusModel(Status.Success);
            DateTime date = Convert.ToDateTime(scheduleBuilder.Date).Date;
            var existingDsb = await mdbContext.ScheduleBuilders.Find(t => t.TfxCompanyId == scheduleBuilder.CompanyId
                                                                            && t.RegionId == scheduleBuilder.RegionId
                                                                            && t.DateFilter == date).FirstOrDefaultAsync();
            if (existingDsb != null)
            {
                if (string.IsNullOrWhiteSpace(scheduleBuilder.Id))
                {
                    scheduleBuilder.Id = existingDsb.Id.ToString();
                }
                var drIdsFromDatabase = existingDsb.Trips.SelectMany(t => t.DeliveryRequests).ToList();
                var drIdsFromModel = scheduleBuilder.Trips.SelectMany(t => t.DeliveryRequests.Select(t1 => ObjectId.Parse(t1.Id))).ToList();
                var isSaveDsbFromLoadQueue = scheduleBuilder.Trips.Where(top => top.IsDsbLoadQueueBackgroundProcess == true).Count();
                var drsFromDatabase = await mdbContext.DeliveryRequests.Find(t => (drIdsFromModel.Contains(t.Id) || drIdsFromDatabase.Contains(t.Id)) && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.Canceled).Project(t => new { t.ScheduleBuilderId, t.TripId, t.Id, t.IsDeleted, t.IsReCreated, t.IsRecurringSchedule, t.TfxScheduleStatus }).ToListAsync();
                foreach (var trip in scheduleBuilder.Trips)
                {
                    var existingTrip = existingDsb.Trips.Where(t => t.ShiftId == trip.ShiftId && t.DriverRowIndex == trip.DriverRowIndex && t.DriverColIndex == trip.DriverColIndex).Select(t => new { t.TimeStamp, t.UpdatedByName, t.IsDsbLoadQueueBackgroundProcess }).FirstOrDefault();
                    if (existingTrip != null && isSaveDsbFromLoadQueue == 0 && existingTrip.TimeStamp != 0 && existingTrip.TimeStamp != trip.TimeStamp)
                    {
                        response.StatusMessage = string.Format(Resource.valMessageLoadUpdated, trip.DriverColIndex + 1, trip.DriverRowIndex + 1, trip.ShiftIndex + 1, existingTrip.UpdatedByName);
                        response.StatusCode = (int)Status.Failed;
                        break;
                    }
                    if (scheduleBuilder.Status != (int)DSBMethod.DriverAssignment)
                    {
                        if (trip != null && trip.Drivers.Count > 0 && trip.Drivers != null)
                        {
                            var driver = existingDsb.Trips.Where(t => t.ShiftId == trip.ShiftId && t.DriverRowIndex == trip.DriverRowIndex).SelectMany(t => t.TfxDrivers).FirstOrDefault();
                            if (driver != null && driver.Id > 0 && driver.Id != trip.Drivers[0].Id)
                            {
                                response.StatusMessage = Resource.valMessageSbNotRefreshed;
                                response.StatusCode = (int)Status.Failed;
                                break;
                            }
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(trip.TripId))
                    {
                        List<ObjectId> deliveryRequests = drsFromDatabase.Where(t => t.TripId == trip.TripId).Select(t => t.Id).ToList();
                        List<ObjectId> currentRequests = trip.DeliveryRequests.Select(t1 => ObjectId.Parse(t1.Id)).ToList();
                        var deletedRequests = deliveryRequests.Where(t => !currentRequests.Contains(t)).ToList();

                        if (drsFromDatabase.Any(t => deletedRequests.Contains(t.Id) && t.TfxScheduleStatus == (int)DeliveryScheduleStatus.Canceled))
                        {
                            response.StatusMessage = Resource.valMessageDeletedDelReqsCanceled;
                            response.StatusCode = (int)Status.Failed;
                            break;
                        }
                    }
                    foreach (var dr in trip.DeliveryRequests)
                    {
                        ObjectId DelReqId = ObjectId.Parse(dr.Id);
                        var existingDr = drsFromDatabase.FirstOrDefault(t => t.Id == DelReqId);
                        if (existingDr != null && existingDr.TfxScheduleStatus == (int)TrackableDeliveryScheduleStatus.Canceled && (dr.ScheduleStatus == (int)DeliveryScheduleStatus.New || dr.ScheduleStatus == (int)DeliveryScheduleStatus.Modified))
                        {
                            response.StatusMessage = Resource.valMessageDelReqsCanceled;
                            response.StatusCode = (int)Status.Failed;
                            break;
                        }
                        if (existingDr != null && existingDr.IsDeleted)
                        {
                            if (existingDr.IsReCreated)
                            {
                                response.StatusMessage = Resource.valMessageDelReqInUse;
                                response.StatusCode = (int)Status.Failed;
                                break;
                            }
                            else
                            {
                                response.StatusMessage = Resource.valMessageDeliveryReqDeleted;
                                response.StatusCode = (int)Status.Failed;
                                break;
                            }
                        }
                        if (existingDr != null && existingDr.ScheduleBuilderId != null && existingDr.ScheduleBuilderId != scheduleBuilder.Id)
                        {
                            response.StatusMessage = Resource.valMessageDelReqInUse;
                            response.StatusCode = (int)Status.Failed;
                            break;
                        }

                        if (existingDr != null && existingDr.TripId != null)
                        {
                            if (existingDr.TripId != trip.TripId)
                            {
                                if (scheduleBuilder.Status != (int)DSBMethod.DragDrop)
                                {
                                    response.StatusMessage = Resource.valMessageDelReqInUse;
                                    response.StatusCode = (int)Status.Failed;
                                    break;
                                }
                                else if (existingDr.TripId != dr.SourceTripId)
                                {
                                    response.StatusMessage = Resource.valMessageDelReqInUse;
                                    response.StatusCode = (int)Status.Failed;
                                    break;
                                }
                            }
                        }
                    }
                    if (response.StatusCode == (int)Status.Failed)
                    {
                        break;
                    }
                }

                if (response.StatusCode == (int)Status.Success)
                {
                    var shifts = scheduleBuilder.Trips.GroupBy(t => t.ShiftId).Select(t => t).ToList();
                    foreach (var shift in shifts)
                    {
                        var schedules = shift.GroupBy(t => t.DriverRowIndex).ToList();
                        foreach (var schedule in schedules)
                        {
                            TripViewModel tripModel = schedule.FirstOrDefault();
                            if (tripModel != null)
                            {
                                var driver = tripModel.Drivers.FirstOrDefault(t => t.Id > 0);
                                if (driver != null)
                                {
                                    int? existingDriverIndex = existingDsb.Trips.Where(t => t.ShiftId == tripModel.ShiftId && t.DriverRowIndex != tripModel.DriverRowIndex && t.TfxDrivers.Any(t1 => t1.Id == driver.Id)).Select(t => t.DriverRowIndex).FirstOrDefault();
                                    if (existingDriverIndex != null && existingDriverIndex >= 0)
                                    {
                                        response.StatusMessage = string.Format(Resource.valMessageDriverAlreadyAssigned, existingDriverIndex + 1);
                                        response.StatusCode = (int)Status.Failed;
                                        break;
                                    }
                                }
                                var trailers = tripModel.Trailers.Where(t => t.Id != null && t.Id != "").ToList();
                                foreach (var trailer in trailers)
                                {
                                    int? existingTrailerIndex = existingDsb.Trips.Where(t => t.ShiftId == tripModel.ShiftId && t.DriverRowIndex != tripModel.DriverRowIndex && t.Trailers.Any(t1 => t1.Id == trailer.Id)).Select(t => t.DriverRowIndex).FirstOrDefault();
                                    if (existingTrailerIndex != null && existingTrailerIndex >= 0)
                                    {
                                        response.StatusMessage = string.Format(Resource.valMessageTrailerAlreadyAssigned, existingTrailerIndex + 1);
                                        response.StatusCode = (int)Status.Failed;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return response;



        }
        public async Task<StatusModel> IsValidDSBView(DSBSaveModel scheduleBuilder)
        {
            StatusModel response = new StatusModel(Status.Success);
            if (!scheduleBuilder.IsPreloadSchedule)//we are not validate valid dsb view for across date pre load.
            {
                DateTime date = Convert.ToDateTime(scheduleBuilder.Date).Date;
                var existingDsb = await mdbContext.ScheduleBuilders.Find(t => t.TfxCompanyId == scheduleBuilder.CompanyId
                                                                               && t.RegionId == scheduleBuilder.RegionId
                                                                               && t.DateFilter == date).FirstOrDefaultAsync();
                if (existingDsb != null)
                {
                    var deliveryReqCount = existingDsb.Trips.SelectMany(top => top.DeliveryRequests).Count();
                    if (deliveryReqCount > 0 && existingDsb.DSBFilter != scheduleBuilder.DSBFilter)
                    {
                        var stringRowInfo = existingDsb.DSBFilter == 1 ? "Row" : "Grid";
                        response.StatusMessage = string.Format(Resource.valDSBView, stringRowInfo);
                        response.StatusCode = (int)Status.Failed;
                    }
                }
            }
            return response;
        }
        public async Task<StatusModel> IsValidLoadTrailerView(DSBSaveModel scheduleBuilder)
        {
            StatusModel response = new StatusModel(Status.Success);
            DateTime date = Convert.ToDateTime(scheduleBuilder.Date).Date;
            var existingDsb = await mdbContext.ScheduleBuilders.Find(t => t.TfxCompanyId == scheduleBuilder.CompanyId
                                                                            && t.RegionId == scheduleBuilder.RegionId
                                                                            && t.DateFilter == date).FirstOrDefaultAsync();
            if (existingDsb != null)
            {
                foreach (var trip in scheduleBuilder.Trips)
                {
                    string trailerId = trip.Trailers.Select(t => t.TrailerId).FirstOrDefault();
                    long existingTripTimeStamp = existingDsb.Trips.Where(t => t.Trailers.Any(t1 => trailerId == t1.TrailerId) && t.TrailerRowIndex == trip.TrailerRowIndex && t.TrailerColIndex == trip.TrailerColIndex).Select(t => t.TimeStamp).FirstOrDefault();
                    if (existingTripTimeStamp != 0 && existingTripTimeStamp != trip.TimeStamp)
                    {
                        response.StatusMessage = Resource.valMessageSbNotRefreshed;
                        response.StatusCode = (int)Status.Failed;
                        break;
                    }
                    foreach (var dr in trip.DeliveryRequests)
                    {
                        ObjectId DelReqId = ObjectId.Parse(dr.Id);
                        string existingDrLoadId = await mdbContext.DeliveryRequests.Find(t => t.Id == DelReqId).Project(t => t.TripId).FirstOrDefaultAsync();
                        if (existingDrLoadId != null && existingDrLoadId != trip.TripId)
                        {
                            response.StatusMessage = Resource.valMessageDelReqInUse;
                            dr.Status = DeliveryReqStatus.InUse;
                            response.StatusCode = (int)Status.Failed;
                        }
                    }
                }
            }
            return response;
        }

        private async Task<ScheduleBuilder> CreateScheduleBuilder(DSBSaveModel dsbModel)
        {
            ScheduleBuilder dsbEntity = dsbModel.ToEntity();
            dsbEntity.CreatedBy = dsbModel.UserId;
            dsbEntity.CreatedOn = DateTimeOffset.Now;
            dsbEntity.UpdatedOn = DateTimeOffset.Now;
            await mdbContext.ScheduleBuilders.InsertOneAsync(dsbEntity);
            if (dsbModel.Status != (int)DSBMethod.DriverAssignment)
            {
                await UpdateDeliveryRequests(dsbModel, dsbEntity);
                await InactiveRetainedFuels(dsbModel.UserId, dsbModel);
            }
            return dsbEntity;
        }

        private async Task<ScheduleBuilder> UpdateScheduleBuilder(DSBSaveModel viewModel)
        {
            var dsbId = ObjectId.Parse(viewModel.Id);
            var dsbEntity = await mdbContext.ScheduleBuilders.Find(t => t.Id == dsbId).FirstOrDefaultAsync();
            if (viewModel.Status != (int)DSBMethod.DriverAssignment)
            {
                await UpdateDeletedDeliveryRequests(viewModel, dsbEntity);
            }
            dsbEntity.TimeStamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var shifts = GetShifts(viewModel, dsbEntity);
            var trailers = GetTrailers(viewModel, dsbEntity);
            List<TripModel> trips = null;
            if (viewModel.Status != (int)DSBMethod.DriverAssignment)
            {
                trips = GetTrips(viewModel, dsbEntity);
            }
            else
            {
                trips = AssignDriverToTrips(viewModel, dsbEntity);
                foreach (var item in trips)
                {
                    var shiftInfo = shifts.FirstOrDefault(x => x.Id == item.ShiftId);
                    if (shiftInfo != null)
                    {
                        item.ShiftIndex = shiftInfo.OrderNo;
                    }
                }
            }
            var updateFields = Builders<ScheduleBuilder>.Update
                    .Set(t => t.Trips, trips)
                    .Set(t => t.Trailers, trailers)
                    .Set(t => t.Shifts, shifts)
                    .Set(t => t.DSBFilter, viewModel.DSBFilter)
                    .Set(t => t.TimeStamp, dsbEntity.TimeStamp)
                    .Set(t => t.Status, (int)DSBMethod.None)
                    .Set(t => t.UpdatedBy, viewModel.UserId)
                    .Set(t => t.UpdatedOn, DateTimeOffset.Now);

            ObjectId objectId = ObjectId.Parse(viewModel.Id);
            var filter = Builders<ScheduleBuilder>.Filter.And(
                    Builders<ScheduleBuilder>.Filter.Where(x => x.Id == objectId)
                );

            await mdbContext.ScheduleBuilders.UpdateOneAsync(filter, updateFields);
            if (viewModel.Status != (int)DSBMethod.DriverAssignment)
            {
                await UpdateDeliveryRequests(viewModel, dsbEntity);
                await InactiveRetainedFuels(viewModel.UserId, viewModel);
            }
            //update DispatcherDragDropSequence for deliveryRequests
            if (viewModel.Status != (int)DSBMethod.DriverAssignment)
            {
                await UpdateDispatcherDragDropSequence(viewModel.Trips);
            }
            return dsbEntity;
        }

        private async Task UpdateDispatcherDragDropSequence(List<TripViewModel> trips)
        {
            if (trips != null && trips.Any())
            {
                var tripsDetails = trips.Where(x => x.IsDispatcherDragDropSequence == true && x.IsDispatcherDragDropSequenceModified == true);

                foreach (var item in tripsDetails)
                {
                    int dispatcherDragDropSequence = 1;
                    foreach (var delitem in item.DeliveryRequests)
                    {
                        ObjectId delitemObj = ObjectId.Empty;
                        ObjectId.TryParse(delitem.Id, out delitemObj);
                        var delfilter = Builders<DeliveryRequest>.Filter.And(
                          Builders<DeliveryRequest>.Filter.Where(x => delitemObj == x.Id));
                        var update = Builders<DeliveryRequest>.Update
                            .Set(t => t.IsDispatcherDragDrop, true)
                            .Set(t => t.DispatcherDragDropSequence, dispatcherDragDropSequence);
                        await mdbContext.DeliveryRequests.UpdateOneAsync(delfilter, update);
                        dispatcherDragDropSequence = dispatcherDragDropSequence + 1;
                    }
                }
            }
        }

        private List<ShiftModel> GetShifts(DSBSaveModel viewModel, ScheduleBuilder dsbEntity)
        {
            var existingShifts = dsbEntity.Shifts;
            var modelShifts = viewModel.Trips.GroupBy(t => t.ShiftId).Select(t => t.FirstOrDefault());
            foreach (var item in existingShifts)
            {
                TimeSpan tspan;
                tspan = DateTime.ParseExact(item.StartTime, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
                item.Ticks = tspan.Ticks;
            }
            int shiftIndex = 0;
            foreach (var item in existingShifts.OrderBy(x => x.Ticks).ToList())
            {
                item.OrderNo = shiftIndex;
                shiftIndex++;
            }
            foreach (var shift in modelShifts)
            {
                if (!string.IsNullOrWhiteSpace(shift.ShiftId) && !existingShifts.Any(t => t.Id == shift.ShiftId))
                {
                    existingShifts.Add(new ShiftModel() { Id = shift.ShiftId, StartTime = shift.ShiftStartTime, EndTime = shift.ShiftEndTime, SlotPeriod = shift.SlotPeriod, OrderNo = shiftIndex });
                }
                var existingshiftIndex = existingShifts.FirstOrDefault(x => x.Id == shift.ShiftId);
                if (existingshiftIndex != null)
                {
                    shift.ShiftIndex = existingshiftIndex.OrderNo;
                }
            }
            return existingShifts;
        }

        private List<TrailerModel> GetTrailers(DSBSaveModel viewModel, ScheduleBuilder dsbEntity)
        {
            var existingTrailers = dsbEntity.Trailers;
            var modelTrailers = viewModel.Trips.SelectMany(t => t.Trailers).GroupBy(t => t.TrailerId).Select(t => t.FirstOrDefault());
            foreach (var trailer in modelTrailers)
            {
                if (!existingTrailers.Any(t => t.Id == trailer.Id))
                {
                    existingTrailers.Add(new TrailerModel() { Id = trailer.Id, TrailerId = trailer.TrailerId, TrailerType = trailer.TrailerType, Compartments = trailer.Compartments });
                }
            }
            return existingTrailers;
        }

        private List<TripModel> GetTrips(DSBSaveModel viewModel, ScheduleBuilder dsbEntity)
        {
            var existingTrips = dsbEntity.Trips;
            foreach (var trip in viewModel.Trips)
            {
                var newTripEntity = trip.ToEntity(viewModel, dsbEntity);
                if (!string.IsNullOrWhiteSpace(trip.TripId))
                {
                    var tripId = ObjectId.Parse(trip.TripId);
                    int existingTripIndex = existingTrips.FindIndex(t => t.TripId == tripId);
                    //if driver schedule reset then exclude driver assignment from existing schedule.
                    if (!viewModel.IsDriverScheduleReset)
                    {
                        var existingDrivers = existingTrips[existingTripIndex].TfxDrivers;
                        existingTrips[existingTripIndex].TfxDrivers = existingDrivers;
                    }
                    var existingTrailers = existingTrips[existingTripIndex].Trailers;
                    existingTrips[existingTripIndex] = newTripEntity;

                    //existingTrips[existingTripIndex].Trailers = existingTrailers;
                }
                else
                {
                    var rowFirstLoad = existingTrips.FirstOrDefault(t => t.ShiftId == trip.ShiftId && t.DriverRowIndex == trip.DriverRowIndex);
                    if (rowFirstLoad != null)
                    {
                        newTripEntity.TfxDrivers = rowFirstLoad.TfxDrivers;
                        //newTripEntity.Trailers = rowFirstLoad.Trailers;
                    }
                    existingTrips.Add(newTripEntity);
                }
            }
            // Reset trip properties if trip does not have any DR.
            for (int index = 0; index < existingTrips.Count; index++)
            {
                if (!existingTrips[index].DeliveryRequests.Any())
                {
                    ResetTripProperties(existingTrips[index]);
                }
            }
            return existingTrips;
        }

        private List<TripModel> AssignDriverToTrips(DSBSaveModel viewModel, ScheduleBuilder dsbEntity)
        {
            var existingTrips = dsbEntity.Trips;
            foreach (var trip in viewModel.Trips)
            {
                if (!string.IsNullOrWhiteSpace(trip.TripId))
                {
                    int existingTripIndex = existingTrips.FindIndex(t => t.ShiftId == trip.ShiftId && t.DriverRowIndex == trip.DriverRowIndex && t.DriverColIndex == trip.DriverColIndex);
                    if (existingTripIndex != -1)
                    {
                        existingTrips[existingTripIndex].TfxDrivers = trip.Drivers.ToDropItemEntity();
                        existingTrips[existingTripIndex].Trailers = trip.Trailers;
                        existingTrips[existingTripIndex].TimeStamp = dsbEntity.TimeStamp;
                        existingTrips[existingTripIndex].IsIncludeAllRegionDriver = trip.IsIncludeAllRegionDriver;
                    }
                }
                else
                {
                    var newTripEntity = trip.ToEmptyTripEntity(viewModel, dsbEntity);
                    existingTrips.Add(newTripEntity);
                }
            }
            return existingTrips;
        }

        private static void UpdateTripIdAndGroupId(DSBSaveModel scheduleBuilder, List<TripModel> trips)
        {
            scheduleBuilder.Trips.ForEach(t =>
            {
                if (scheduleBuilder.ObjectFilter == 1)
                {
                    TripModel trip = trips.FirstOrDefault(x => x.ShiftId == t.ShiftId
                                && x.DriverRowIndex == t.DriverRowIndex && x.DriverColIndex == t.DriverColIndex);
                    if (trip != null)
                    {
                        t.TripId = trip.TripId.ToString();
                        t.GroupId = trip.GroupId;
                        t.TimeStamp = trip.TimeStamp;
                    }
                }
                else
                {
                    TripModel trip = trips.FirstOrDefault(x => x.ShiftId == t.ShiftId
                                && x.Trailers.Any(y => t.Trailers.Select(z => z.Id).Contains(y.Id))
                                && x.TrailerRowIndex == t.TrailerRowIndex && x.TrailerColIndex == t.TrailerColIndex);
                    if (trip != null)
                    {
                        t.TripId = trip.TripId.ToString();
                        t.GroupId = trip.GroupId;
                        t.TimeStamp = trip.TimeStamp;
                    }
                }
            });
        }

        private void UpdateDsbModel(DSBSaveModel model, ScheduleBuilder entity)
        {
            model.Id = entity.Id.ToString();
            model.TimeStamp = entity.TimeStamp;
            UpdateTripIdAndGroupId(model, entity.Trips);
            if (model.Status != (int)DSBMethod.DriverAssignment)
            {
                model.Trips.ForEach(t => t.UpdateStatuses());
                foreach (var trip in model.Trips)
                {
                    var modifiedDelReqs = trip.DeliveryRequests.Where(t => t.ScheduleStatus == (int)DeliveryScheduleStatus.New || t.ScheduleStatus == (int)DeliveryScheduleStatus.Modified).Select(t => t).ToList();
                    modifiedDelReqs.ForEach(t => t.UpdateStatuses());
                    modifiedDelReqs.ForEach(t => t.TripId = trip.TripId);
                }
            }
        }

        private async Task UpdateDeletedDeliveryRequests(DSBSaveModel scheduleBuilder, ScheduleBuilder sbEntity)
        {
            ObjectId objectId = ObjectId.Parse(scheduleBuilder.Id);
            List<ObjectId> deletedRequests = new List<ObjectId>();
            if (sbEntity != null && sbEntity.Trips != null)
            {
                if (scheduleBuilder.Trips != null)
                {
                    foreach (var trip in scheduleBuilder.Trips)
                    {
                        List<ObjectId> deliveryRequests = sbEntity.Trips.Where(t => t.ShiftId == trip.ShiftId && t.DriverRowIndex == trip.DriverRowIndex && t.DriverColIndex == trip.DriverColIndex).SelectMany(t => t.DeliveryRequests).ToList();
                        List<ObjectId> currentRequests = trip.DeliveryRequests.Select(t1 => ObjectId.Parse(t1.Id)).ToList();
                        deletedRequests.AddRange(deliveryRequests.Where(t => !currentRequests.Contains(t)).ToList());
                    }
                }
            }
            await RestoreRetainedFuels(scheduleBuilder.UserId, deletedRequests);
            await UpdateDeletedDeliveryRequests(scheduleBuilder.UserId, deletedRequests);
        }

        private async Task RestoreRetainedFuels(int userId, List<ObjectId> deletedRequests)
        {
            if (deletedRequests != null && deletedRequests.Any())
            {
                var requestFilter = Builders<DeliveryRequest>.Filter.Where(x => deletedRequests.Contains(x.Id) && x.IsRetainFuelLoaded);
                var deliveryRequests = await mdbContext.DeliveryRequests.Find(requestFilter).Project(t => new { t.PostLoadedFor, t.Compartments }).ToListAsync();
                if (deliveryRequests != null && deliveryRequests.Any())
                {
                    foreach (var dr in deliveryRequests.Where(t => t.PostLoadedFor.HasValue))
                    {
                        var postLoadedFor = dr.PostLoadedFor.Value;
                        foreach (var compartment in dr.Compartments)
                        {
                            if (!string.IsNullOrEmpty(compartment.CompartmentId))
                            {
                                var filter = Builders<TrailerFuelRetain>.Filter.Where(t => t.DeliveryRequestId == postLoadedFor
                                            && t.TrailerId == compartment.TrailerId && t.CompartmentId == compartment.CompartmentId && !t.IsActive);
                                var update = Builders<TrailerFuelRetain>.Update.Set(t => t.IsActive, true)
                                            .Set(t => t.UpdatedOn, DateTimeOffset.Now).Set(t => t.UpdatedBy, userId);
                                await mdbContext.TrailerFuelRetains.UpdateOneAsync(filter, update);
                            }
                            else
                            {
                                var filter = Builders<TrailerFuelRetain>.Filter.Where(t => t.DeliveryRequestId == postLoadedFor
                                       && t.TrailerId == compartment.TrailerId && !t.IsActive);
                                var update = Builders<TrailerFuelRetain>.Update.Set(t => t.IsActive, true)
                                            .Set(t => t.UpdatedOn, DateTimeOffset.Now).Set(t => t.UpdatedBy, userId);
                                await mdbContext.TrailerFuelRetains.UpdateManyAsync(filter, update);
                            }
                        }
                    }
                }
            }
        }

        private async Task InactiveRetainedFuels(int userId, DSBSaveModel dsbModel)
        {
            var publishedDRs = dsbModel.Trips.SelectMany(t1 => t1.DeliveryRequests.Where(t2 => t2.IsRetainFuelLoaded
                                && !string.IsNullOrWhiteSpace(t2.PostLoadedFor) && t2.Status == DeliveryReqStatus.ScheduleCreated)).ToList();
            if (publishedDRs != null && publishedDRs.Any())
            {
                var publishedRetainedDRIds = publishedDRs.Select(t => new { PostLoadedFor = ObjectId.Parse(t.PostLoadedFor), t.Compartments }).ToList();
                foreach (var dr in publishedRetainedDRIds)
                {
                    var postLoadedFor = dr.PostLoadedFor;
                    foreach (var compartment in dr.Compartments)
                    {
                        var compartmentTrailerId = ObjectId.Parse(compartment.TrailerId);
                        if (!string.IsNullOrEmpty(compartment.CompartmentId))
                        {
                            var filter = Builders<TrailerFuelRetain>.Filter.Where(t => t.DeliveryRequestId == postLoadedFor
                                        && t.TrailerId == compartmentTrailerId && t.CompartmentId == compartment.CompartmentId && t.IsActive);
                            var update = Builders<TrailerFuelRetain>.Update.Set(t => t.IsActive, false)
                                        .Set(t => t.UpdatedOn, DateTimeOffset.Now).Set(t => t.UpdatedBy, userId);
                            await mdbContext.TrailerFuelRetains.UpdateOneAsync(filter, update);
                            var compartmentActiveInfo = await mdbContext.TrailerFuelRetains.Find(x => x.TrailerId == compartmentTrailerId && x.CompartmentId == compartment.CompartmentId && x.Quantity == compartment.Quantity && x.IsActive && !x.IsDeleted).ToListAsync();
                            if (compartmentActiveInfo.Any())
                            {
                                var trailerRetainIds = compartmentActiveInfo.Select(x => x.Id).ToList();
                                var compartmentfilter = Builders<TrailerFuelRetain>.Filter.Where(t => trailerRetainIds.Contains(t.Id));
                                var compartmentupdate = Builders<TrailerFuelRetain>.Update.Set(t => t.IsActive, false)
                                            .Set(t => t.UpdatedOn, DateTimeOffset.Now).Set(t => t.UpdatedBy, userId);
                                await mdbContext.TrailerFuelRetains.UpdateOneAsync(compartmentfilter, compartmentupdate);
                            }
                        }
                        else
                        {
                            var filter = Builders<TrailerFuelRetain>.Filter.Where(t => t.DeliveryRequestId == postLoadedFor
                                       && t.TrailerId == compartmentTrailerId && t.IsActive);
                            var update = Builders<TrailerFuelRetain>.Update.Set(t => t.IsActive, false)
                                        .Set(t => t.UpdatedOn, DateTimeOffset.Now).Set(t => t.UpdatedBy, userId);
                            await mdbContext.TrailerFuelRetains.UpdateManyAsync(filter, update);
                        }
                    }
                }
            }
        }

        private async Task UpdateDeletedDeliveryRequests(int userId, List<ObjectId> deletedRequests)
        {
            if (deletedRequests != null && deletedRequests.Any())
            {
                List<ObjectId> deletedBlendDrs = new List<ObjectId>();
                var blendGroupIds = await mdbContext.DeliveryRequests.Find(t => deletedRequests.Contains(t.Id)).Project(t => t.BlendedGroupId).ToListAsync();
                if (blendGroupIds.Any(t => !string.IsNullOrWhiteSpace(t)))
                {
                    var drs = await mdbContext.DeliveryRequests.Find(t => t.BlendedGroupId != null && blendGroupIds.Contains(t.BlendedGroupId) && t.IsActive && !t.IsDeleted && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.Completed && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.CompletedLate && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.RescheduledCompleted && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.UnplannedDropCompleted).Project(t => new { t.Id, t.BlendedGroupId }).ToListAsync();
                    drs.ForEach(t =>
                    {
                        if (deletedRequests.Any(t1 => t1 == t.Id))
                        {
                            var blendDrs = drs.Where(dr => dr.BlendedGroupId == t.BlendedGroupId).Select(t2 => t2.Id);
                            if (blendDrs.Except(deletedRequests).Any())
                            {
                                blendDrs.Intersect(deletedRequests).ToList().ForEach(b =>
                                {
                                    deletedRequests.Remove(b);
                                    deletedBlendDrs.Add(b);
                                });
                            }
                        }
                    });
                }
                var requestFilter = Builders<DeliveryRequest>.Filter.And(
                       Builders<DeliveryRequest>.Filter.Where(x => deletedRequests.Contains(x.Id) && x.TfxScheduleStatus != (int)DeliveryScheduleStatus.Completed && x.TfxScheduleStatus != (int)DeliveryScheduleStatus.CompletedLate && x.TfxScheduleStatus != (int)DeliveryScheduleStatus.RescheduledCompleted && x.TfxScheduleStatus != (int)DeliveryScheduleStatus.UnplannedDropCompleted)
                   );
                var requestUpdateFields = Builders<DeliveryRequest>.Update
                        .Set(t => t.ScheduleBuilderId, null)
                        .Set(t => t.ScheduleShiftEndDateTime, null)
                        .Set(t => t.TripId, null)
                        .Set(t => t.TfxDeliveryGroupId, null)
                        .Set(t => t.TfxDeliveryScheduleId, null)
                        .Set(t => t.TfxTrackableScheduleId, null)
                        .Set(t => t.TfxScheduleStatus, 0)
                        .Set(t => t.TfxScheduleStatusName, string.Empty)
                        .Set(t => t.TfxScheduleEnrouteStatus, 0)
                        .Set(t => t.Status, DeliveryReqStatus.Assigned)
                        .Set(t => t.UpdatedBy, userId)
                        .Set(t => t.UpdatedOn, DateTimeOffset.Now)
                        .Set(t => t.PreLoadedFor, null)
                        .Set(t => t.PreLoadInfo, null)
                        .Set(t => t.PostLoadedFor, null)
                        .Set(t => t.PostLoadInfo, null)
                        .Set(t => t.Compartments, new List<CompartmentsInfo>())
                        .Set(t => t.IsRetainFuelLoaded, false);
                await mdbContext.DeliveryRequests.UpdateManyAsync(requestFilter, requestUpdateFields);

                requestFilter = Builders<DeliveryRequest>.Filter.And(
                      Builders<DeliveryRequest>.Filter.Where(x => deletedBlendDrs.Contains(x.Id))
                  );
                requestUpdateFields = Builders<DeliveryRequest>.Update
                        .Set(t => t.IsActive, false)
                        .Set(t => t.IsDeleted, true)
                        .Set(t => t.UpdatedBy, userId)
                        .Set(t => t.UpdatedOn, DateTimeOffset.Now);
                await mdbContext.DeliveryRequests.UpdateManyAsync(requestFilter, requestUpdateFields);

                //for brokered dr scenario
                //Update parent dr status after reset load
                var _deletedRequests = deletedRequests.Select(x => x.ToString()).ToList();

                requestFilter = Builders<DeliveryRequest>.
                    Filter.And(Builders<DeliveryRequest>.
                    Filter.Where(x => x.IsActive && !x.IsDeleted && _deletedRequests.Contains(x.BrokeredChildId))
                );

                requestUpdateFields = Builders<DeliveryRequest>.Update.Set(t => t.Status, DeliveryReqStatus.Pending);

                await mdbContext.DeliveryRequests.UpdateManyAsync(requestFilter, requestUpdateFields);
            }
        }

        private async Task DeleteUnwantedRequests(int userId, List<ObjectId> deletedRequests)
        {
            if (deletedRequests != null && deletedRequests.Any())
            {
                var requestFilter = Builders<DeliveryRequest>.Filter.And(
                       Builders<DeliveryRequest>.Filter.Where(x => deletedRequests.Contains(x.Id))
                   );
                var requestUpdateFields = Builders<DeliveryRequest>.Update
                        .Set(t => t.IsActive, false)
                        .Set(t => t.IsDeleted, true)
                        .Set(t => t.UpdatedBy, userId)
                        .Set(t => t.UpdatedOn, DateTimeOffset.Now);
                await mdbContext.DeliveryRequests.UpdateManyAsync(requestFilter, requestUpdateFields);
            }
        }

        public async Task<List<DSBSaveModel>> DeleteTrip(List<DSBSaveModel> scheduleBuilders)
        {
            using (var session = await mdbContext.Client.StartSessionAsync())
            {
                session.StartTransaction();
                try
                {
                    for (int index = 0; index < scheduleBuilders.Count; index++)
                    {
                        var dsbModel = scheduleBuilders[index];
                        StatusModel status = await IsValidLoadDriverView(dsbModel);
                        if (status.StatusCode != (int)Status.Success)
                        {
                            dsbModel.StatusMessage = status.StatusMessage;
                            dsbModel.StatusCode = status.StatusCode;
                            session.AbortTransaction();
                            return scheduleBuilders;
                        }
                        ObjectId dsbId = ObjectId.Parse(dsbModel.Id);
                        var dsbEntity = mdbContext.ScheduleBuilders.Find(t => t.Id == dsbId).FirstOrDefault();
                        if (dsbEntity != null)
                        {
                            dsbEntity.TimeStamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                            var deletedDelReqIds = new List<ObjectId>();
                            var deliveryRequestInfo = new List<DeliveryRequest>();
                            foreach (var tripModel in dsbModel.Trips)
                            {
                                ObjectId tripId = ObjectId.Parse(tripModel.TripId);
                                var tripEntity = dsbEntity.Trips.FirstOrDefault(t => t.TripId == tripId);
                                if (tripEntity != null)
                                {
                                    var deletedTripDs = tripEntity.DeliveryRequests;
                                    List<int> enrouteCancelled = new List<int> { 2, 7, 8 };
                                    List<int> enrouteCompleted = new List<int> { 4 };
                                    List<int> trackCompleted = new List<int> { 7, 8, 9, 10, 22, 24 },
                                        trackCancelled = new List<int> { 5, 21 };
                                    List<int> dropDiversion = new List<int> { 25 };
                                    // reset calendar view flag
                                    ResetDrCalendarView(deletedTripDs);
                                    var moveDRQueueList = await mdbContext.DeliveryRequests.Find(x => deletedTripDs.Contains(x.Id)).Project(x => new { x.Id, x.TfxScheduleStatus, x.TfxScheduleEnrouteStatus, x.Status }).ToListAsync();
                                    foreach (var item in moveDRQueueList)
                                    {
                                        if (enrouteCancelled.Contains(item.TfxScheduleEnrouteStatus) || enrouteCompleted.Contains(item.TfxScheduleEnrouteStatus)
                                            || dropDiversion.Contains(item.TfxScheduleStatus) || trackCompleted.Contains(item.TfxScheduleStatus) || trackCancelled.Contains(item.TfxScheduleStatus))
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            deletedDelReqIds.Add(item.Id);
                                        }
                                    }
                                    ResetDSTripProperties(tripEntity, deletedDelReqIds);
                                    if (tripEntity.DeliveryRequests.Any())
                                    {
                                        var deliveryReqId = tripEntity.DeliveryRequests.ToList();
                                        deliveryRequestInfo = await mdbContext.DeliveryRequests.Find(x => deliveryReqId.Contains(x.Id)).ToListAsync();
                                    }
                                    if (tripEntity.DeliveryRequests.Count() == 0)
                                    {
                                        ResetTripProperties(tripEntity);
                                    }
                                    tripEntity.TimeStamp = dsbEntity.TimeStamp;
                                    tripEntity.UpdatedBy = dsbModel.UserId;
                                    tripEntity.UpdatedDate = DateTimeOffset.Now;
                                    mdbContext.ScheduleBuilders.ReplaceOne(c => c.Id == dsbId, dsbEntity);
                                }
                            }
                            if (deletedDelReqIds.Any())
                            {
                                await RestoreRetainedFuels(dsbModel.UserId, deletedDelReqIds);
                                await UpdateDeletedDeliveryRequests(dsbModel.UserId, deletedDelReqIds);
                            }
                            await session.CommitTransactionAsync();
                            for (int i = 0; i < dsbModel.Trips.Count; i++)
                            {
                                ObjectId tripId = ObjectId.Parse(dsbModel.Trips[i].TripId);
                                var tripEntity = dsbEntity.Trips.FirstOrDefault(t => t.TripId == tripId);
                                dsbModel.Trips[i] = tripEntity.ToViewModel(deliveryRequestInfo);
                                dsbModel.TimeStamp = dsbEntity.TimeStamp;
                            }
                            dsbModel.StatusCode = (int)Status.Success;
                            dsbModel.StatusMessage = Resource.valSuccessMessageResetLoad;
                        }
                    }

                }
                catch (Exception)
                {
                    await session.AbortTransactionAsync();
                    scheduleBuilders.ForEach(t => t.StatusCode = (int)Status.Failed);
                    throw;
                }
            }
            return scheduleBuilders;
        }

        private static void ResetTripProperties(TripModel tripEntity)
        {
            tripEntity.DeliveryRequests = new List<ObjectId>();
            tripEntity.GroupId = 0;
            tripEntity.LoadCode = null;
            tripEntity.RouteInfo = null;
            tripEntity.SupplierSource = null;
            tripEntity.TripStatus = TripStatus.Added;
            tripEntity.DeliveryGroupStatus = DeliveryGroupStatus.Draft;
            tripEntity.IsCommonPickup = false;
            tripEntity.PickupLocationType = PickupLocationType.Terminal;
            tripEntity.TfxBulkPlant = new BulkPlantAddressModel();
            tripEntity.TfxTerminal = new MdbDataAccess.Collections.DropdownDisplayItem();
        }

        private static void ResetDSTripProperties(TripModel tripEntity, List<ObjectId> deletedDelReqIds)
        {
            tripEntity.DeliveryRequests = tripEntity.DeliveryRequests.Where(x => !deletedDelReqIds.Contains(x)).ToList();
        }

        private void ResetDrCalendarView(List<ObjectId> deletedDelReqIds)
        {
            if (deletedDelReqIds != null && deletedDelReqIds.Any())
            {
                var requestFilter = Builders<DeliveryRequest>.Filter.Where(x => deletedDelReqIds.Contains(x.Id));
                var requestUpdateFields = Builders<DeliveryRequest>.Update
                                .Set(t => t.IsCalendarView, false);
                mdbContext.DeliveryRequests.UpdateMany(requestFilter, requestUpdateFields);
            }
        }
        private async Task UpdateDeliveryRequests(DSBSaveModel dsbModel, ScheduleBuilder dsbEntity)
        {
            foreach (var trip in dsbModel.Trips)
            {
                string tripId = GetTripId(dsbEntity, trip);
                SetChildDRStatus(trip);
                var modifiedRequests = trip.DeliveryRequests.Where(t => t.ScheduleStatus == (int)DeliveryScheduleStatus.New || t.ScheduleStatus == (int)DeliveryScheduleStatus.Modified).Select(t => t).ToList();
                foreach (var modifiedRequest in modifiedRequests)
                {
                    ObjectId id = ObjectId.Parse(modifiedRequest.Id);
                    var requestFilter = Builders<DeliveryRequest>.Filter.And(
                      Builders<DeliveryRequest>.Filter.Where(x => id == x.Id && x.TfxScheduleStatus != (int)DeliveryScheduleStatus.Completed && x.TfxScheduleStatus != (int)DeliveryScheduleStatus.CompletedLate && x.TfxScheduleStatus != (int)DeliveryScheduleStatus.RescheduledCompleted && x.TfxScheduleStatus != (int)DeliveryScheduleStatus.UnplannedDropCompleted));

                    DeliveryRequest deliveryRequest = new DeliveryRequest();
                    if (!modifiedRequest.IsAdditive)
                    {
                        if (modifiedRequest.Terminal != null && modifiedRequest.Terminal.Id > 0 && modifiedRequest.PickupLocationType != PickupLocationType.BulkPlant)
                        {
                            deliveryRequest.TfxTerminal = new MdbDataAccess.Collections.DropdownDisplayItem()
                            {
                                Id = modifiedRequest.Terminal.Id,
                                Name = modifiedRequest.Terminal.Name
                            };
                            modifiedRequest.BulkPlant = new BulkPlantAddressModel();
                        }
                        if (modifiedRequest.BulkPlant != null && !string.IsNullOrWhiteSpace(modifiedRequest.BulkPlant.SiteName) && modifiedRequest.PickupLocationType == PickupLocationType.BulkPlant)
                        {
                            deliveryRequest.TfxBulkPlant = modifiedRequest.BulkPlant;
                            modifiedRequest.Terminal = new FreightModels.DropdownDisplayItem();
                        }
                    }
                    else
                    {
                        modifiedRequest.Terminal = new FreightModels.DropdownDisplayItem();
                        modifiedRequest.BulkPlant = new BulkPlantAddressModel();
                    }
                    ObjectId? preloadedForId = null;
                    if (!string.IsNullOrWhiteSpace(modifiedRequest.PreLoadedFor))
                    {
                        preloadedForId = ObjectId.Parse(modifiedRequest.PreLoadedFor);
                    }
                    ObjectId? postloadedForId = null;
                    if (!string.IsNullOrWhiteSpace(modifiedRequest.PostLoadedFor))
                    {
                        postloadedForId = ObjectId.Parse(modifiedRequest.PostLoadedFor);
                    }
                    var compartmentInfo = new List<CompartmentsInfo>();
                    if (modifiedRequest.Compartments != null && modifiedRequest.Compartments.Count > 0)
                    {
                        modifiedRequest.Compartments = modifiedRequest.Compartments.Where(x => x.Quantity > 0).ToList();
                        compartmentInfo = modifiedRequest.Compartments.ToCloneEntity();
                    }

                    var updateList = new List<UpdateDefinition<DeliveryRequest>>();

                    TimeSpan? StartTime = null;
                    TimeSpan? EndTime = null;
                    if (!string.IsNullOrWhiteSpace(modifiedRequest.ScheduleStartTime))
                        StartTime = Convert.ToDateTime(modifiedRequest.ScheduleStartTime).TimeOfDay;
                    if (!string.IsNullOrWhiteSpace(modifiedRequest.ScheduleEndTime))
                        EndTime = Convert.ToDateTime(modifiedRequest.ScheduleEndTime).TimeOfDay;

                    var requestUpdateFields = Builders<DeliveryRequest>.Update
                            .Set(t => t.TfxJobAddress, modifiedRequest.JobAddress)
                            .Set(t => t.TfxJobCity, modifiedRequest.JobCity)
                            .Set(t => t.TfxJobName, modifiedRequest.JobName)
                            .Set(t => t.TfxUoM, modifiedRequest.UoM)
                            .Set(t => t.TfxFuelTypeId, modifiedRequest.FuelTypeId)
                            .Set(t => t.TfxJobId, modifiedRequest.JobId)
                            .Set(t => t.TfxCustomerCompany, modifiedRequest.CustomerCompany)
                            .Set(t => t.RequiredQuantity, modifiedRequest.RequiredQuantity)
                            .Set(t => t.TfxOrderId, modifiedRequest.OrderId)
                            .Set(t => t.IsFilldInvoke, modifiedRequest.IsFilldInvoke)
                            .Set(t => t.Status, modifiedRequest.Status)
                            .Set(t => t.TfxDeliveryGroupId, modifiedRequest.DeliveryGroupId)
                            .Set(t => t.TfxDeliveryScheduleId, modifiedRequest.DeliveryScheduleId)
                            .Set(t => t.TfxTrackableScheduleId, modifiedRequest.TrackableScheduleId)
                            .Set(t => t.TfxTerminal, deliveryRequest.TfxTerminal)
                            .Set(t => t.TfxBulkPlant, deliveryRequest.TfxBulkPlant)
                            .Set(t => t.TfxScheduleStatus, modifiedRequest.TrackScheduleStatus)
                            .Set(t => t.TfxScheduleStatusName, modifiedRequest.TrackScheduleStatusName)
                            .Set(t => t.TfxScheduleEnrouteStatus, 0)
                            .Set(t => t.TripId, tripId)
                            .Set(t => t.BadgeNo1, string.IsNullOrEmpty(modifiedRequest.BadgeNo1) ? string.Empty : modifiedRequest.BadgeNo1)
                            .Set(t => t.BadgeNo2, string.IsNullOrEmpty(modifiedRequest.BadgeNo2) ? string.Empty : modifiedRequest.BadgeNo2)
                            .Set(t => t.BadgeNo3, string.IsNullOrEmpty(modifiedRequest.BadgeNo3) ? string.Empty : modifiedRequest.BadgeNo3)
                            .Set(t => t.IsCommonBadge, modifiedRequest.IsCommonBadge)
                            .Set(t => t.DispactherNote, modifiedRequest.DispactherNote)
                            .Set(t => t.Notes, modifiedRequest.Notes)
                            .Set(t => t.ScheduleBuilderId, dsbEntity.Id.ToString())
                            .Set(t => t.ScheduleShiftEndDateTime, modifiedRequest.ScheduleShiftEndTime)
                            .Set(t => t.PreLoadedFor, preloadedForId)
                            .Set(t => t.PreLoadInfo, modifiedRequest.PreLoadInfo)
                            .Set(t => t.PostLoadedFor, postloadedForId)
                            .Set(t => t.PostLoadInfo, modifiedRequest.PostLoadInfo)
                            .Set(t => t.RouteInfo, modifiedRequest.RouteInfo)
                            .Set(t => t.ScheduleQuantityType, modifiedRequest.ScheduleQuantityType)
                            .Set(t => t.Compartments, compartmentInfo)
                            .Set(t => t.IsMaxFillAllowed, modifiedRequest.IsMaxFillAllowed)
                            .Set(t => t.IsRetainFuelLoaded, modifiedRequest.IsRetainFuelLoaded)
                            .Set(t => t.ScheduleStartTime, StartTime)
                            .Set(t => t.ScheduleEndTime, EndTime)
                            .Set(t => t.SelectedDate, DateTime.TryParse(modifiedRequest.SelectedDate, out DateTime sdate) ? sdate : (DateTime?)null)
                            .Set(t => t.DeliveryLevelPO, modifiedRequest.DeliveryLevelPO);

                    updateList.Add(requestUpdateFields);

                    if (modifiedRequest.IsTBD)
                        updateList.Add(Builders<DeliveryRequest>.Update.Set(r => r.JobTimeZoneOffset, modifiedRequest.JobTimeZoneOffset));

                    var finalUpdateFields = Builders<DeliveryRequest>.Update.Combine(updateList);
                    await mdbContext.DeliveryRequests.UpdateOneAsync(requestFilter, finalUpdateFields);

                    if (modifiedRequest.RouteInfo != null)
                    {
                        await UpdateRouteShiftInformation(trip, tripId, modifiedRequest, id, dsbModel.UserId);
                    }
                    if (modifiedRequest.isRecurringSchedule)
                    {
                        await UpdateRecurringDRShiftInfo(trip, modifiedRequest, deliveryRequest, dsbEntity.Id.ToString());
                    }
                    if (modifiedRequest.CarrierStatus == (int)BrokeredDrCarrierStatus.Accepted)
                    {
                        await UpdateParentDeliveryCarrierStatus(modifiedRequest.Id, trip.DeliveryGroupStatus);
                    }
                    if (modifiedRequest.DelReqSource == DRSource.OttoForecasting)
                    {
                        await SaveUpdateOttoDRInfo(dsbModel, trip, modifiedRequest, deliveryRequest);
                    }

                    try
                    {
                        if (trip.DeliveryGroupStatus == DeliveryGroupStatus.Published)
                        {
                            var deliveryRequestOtto = await mdbContext.DeliveryRequests.Find(requestFilter).FirstOrDefaultAsync();
                            if (deliveryRequestOtto != null && deliveryRequestOtto.DeliveryWindowInfo != null)
                            {
                                var deliveryWindowStart = deliveryRequestOtto.DeliveryWindowInfo.StartDate.Add(DateTime.Parse(deliveryRequestOtto.DeliveryWindowInfo.StartTime).TimeOfDay);
                                var deliveryWindowEnd = deliveryRequestOtto.DeliveryWindowInfo.EndDate.Add(DateTime.Parse(deliveryRequestOtto.DeliveryWindowInfo.EndTime).TimeOfDay);
                                var shiftStart = Convert.ToDateTime(trip.StartDate).Add(DateTime.Parse(trip.ShiftStartTime).TimeOfDay);
                                var shiftEnd = Convert.ToDateTime(trip.StartDate).Add(DateTime.Parse(trip.ShiftEndTime).TimeOfDay);
                                if ((deliveryWindowStart < shiftStart && deliveryWindowEnd < shiftStart) ||
                                    (deliveryWindowStart > shiftEnd && deliveryWindowEnd > shiftEnd))
                                {
                                    AddNotificationForDSOutsideDeliveryWindow(trip, deliveryRequestOtto, deliveryWindowStart, deliveryWindowEnd);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogManager.Logger.WriteException("ScheduleBuilderRepository", "UpdateDeliveryRequests-DSOutsideDeliveryWindow", ex.Message, ex);
                    }
                }
            }
        }

        private static void SetChildDRStatus(TripViewModel trip)
        {
            foreach (var item in trip.DeliveryRequests)
            {
                if (item.ScheduleStatus == (int)DeliveryScheduleStatus.None && !string.IsNullOrEmpty(item.GroupParentDRId))
                {
                    item.ScheduleStatus = (int)DeliveryScheduleStatus.Modified;
                }
            }
        }

        private void AddNotificationForDSOutsideDeliveryWindow(TripViewModel trip, DeliveryRequest deliveryRequestOtto,
            DateTime deliveryWindowStart, DateTime deliveryWindowEnd)
        {
            try
            {
                var message = new NotificationDSOutsideDeliveryWindowModel
                {
                    Location = deliveryRequestOtto.TfxJobName,
                    Product = deliveryRequestOtto.TfxProductType,
                    ScheduledQuantity = deliveryRequestOtto.RequiredQuantity + " " + (UoM)deliveryRequestOtto.TfxUoM,
                    EstimatedDeliveryWindow = deliveryWindowStart + " - " + deliveryWindowEnd,
                    ScheduledTiming = trip.StartDate + " " + trip.ShiftStartTime + " - " + trip.StartDate + " " + trip.ShiftEndTime,
                    OrderId = deliveryRequestOtto.TfxOrderId.Value,
                    CarrierCompanyId = deliveryRequestOtto.TfxCreatedByCompanyId
                };
                DeliveryRequestAccess drExchangeAccess = new DeliveryRequestAccess();
                var jsonMessage = new JavaScriptSerializer().Serialize(message);
                var createdBy = deliveryRequestOtto.CreatedBy > 0 ? deliveryRequestOtto.CreatedBy : (int)SystemUser.System;
                drExchangeAccess.AddNotificationEvent((int)EventType.DeliveryScheduledOutsideDeliveryWindow, createdBy, jsonMessage);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderRepository", "AddNotificationForDSOutsideDeliveryWindow", ex.Message, ex);
            }
        }

        private async Task UpdateRecurringDRShiftInfo(TripViewModel trip, DeliveryRequestViewModel modifiedRequest, DeliveryRequest deliveryRequest, string scheduleBuilderId)
        {
            if (trip.DeliveryGroupStatus == DeliveryGroupStatus.Published)
            {
                ObjectId objschebuleBuilderId = ObjectId.Empty;
                ObjectId.TryParse(scheduleBuilderId, out objschebuleBuilderId);
                var recurringId = ObjectId.Parse(modifiedRequest.RecurringScheduleId);
                var recurringSchedules = mdbContext.RecurringSchedules.Find(x => x.Id == recurringId && x.IsActive && !x.IsDeleted && x.ShiftInfo == null).ToList();
                var recurringDRDetails = recurringSchedules.SelectMany(x => x.DeliveryRequests).ToList();
                var recurringDrsCount = await mdbContext.DeliveryRequests.Find(top => top.IsRecurringSchedule && top.RecurringScheduleId == recurringId && top.IsActive && !top.IsDeleted).CountDocumentsAsync();
                if (recurringDRDetails.Count > 0 && recurringDrsCount > 0)
                {
                    await UpdateScheduleDeliveryInfo(modifiedRequest, deliveryRequest, recurringId, recurringDRDetails);

                }
                if (recurringDrsCount > 0)
                {
                    await UpdateScheduleShiftInfo(trip, modifiedRequest, recurringId, objschebuleBuilderId);
                }
            }
        }
        private async Task SaveUpdateOttoDRInfo(DSBSaveModel dsbModel, TripViewModel trip, DeliveryRequestViewModel modifiedRequest, DeliveryRequest deliveryRequest)
        {
            if (trip.DeliveryGroupStatus == DeliveryGroupStatus.Published)
            {
                var JobId = modifiedRequest.JobId;
                var TankId = modifiedRequest.TankId;
                var StorageId = modifiedRequest.StorageId;
                var SiteId = modifiedRequest.SiteId;

                var OttoScheduleCount = await mdbContext.OttoScheduleInfos.Find(top => top.TfxJobId == JobId && top.IsActive && top.TankId == TankId && top.SiteId == SiteId && top.StorageId == StorageId).CountDocumentsAsync();
                if (OttoScheduleCount == 0)
                {
                    await SaveOttoScheduleInfo(dsbModel, trip, modifiedRequest);
                }
                else if (OttoScheduleCount == 1)
                {
                    await UpdateOttoScheduleInfo(trip, modifiedRequest);
                }
            }
        }
        private async Task SaveOttoScheduleInfo(DSBSaveModel dsbModel, TripViewModel trip, DeliveryRequestViewModel modifiedRequest)
        {
            var assetDetails = mdbContext.JobAdditionalDetails.Find(item => item.TfxJobId == modifiedRequest.JobId && item.TfxDisplayJobId == modifiedRequest.SiteId && item.Tanks.Count() > 0).Project(top => top.Tanks).ToList();
            int tfxAssetId = 0;
            foreach (var item in assetDetails)
            {
                var tankDetails = item.Where(top => top.StorageId == modifiedRequest.StorageId && top.StorageTypeId == modifiedRequest.TankId).FirstOrDefault();
                if (tankDetails != null)
                {
                    tfxAssetId = tankDetails.TfxAssetId;
                    break;
                }
            }
            OttoScheduleInfo ottoScheduleInfo = new OttoScheduleInfo();
            ottoScheduleInfo.DeliveryReqId = modifiedRequest.Id.ToString();
            ottoScheduleInfo.ScheduleBuilderId = dsbModel.Id.ToString();
            ottoScheduleInfo.SiteId = modifiedRequest.SiteId;
            ottoScheduleInfo.TankId = modifiedRequest.TankId;
            ottoScheduleInfo.StorageId = modifiedRequest.StorageId;
            if (tfxAssetId > 0)
            {
                ottoScheduleInfo.TfxAssetId = tfxAssetId;
            }
            ottoScheduleInfo.TfxJobId = modifiedRequest.JobId;

            //set  ottoDeliveryRequestInfo
            OttoDeliveryRequestInfo ottoDeliveryRequestInfo = new OttoDeliveryRequestInfo();
            ottoDeliveryRequestInfo.BadgeNo1 = modifiedRequest.BadgeNo1;
            ottoDeliveryRequestInfo.BadgeNo2 = modifiedRequest.BadgeNo2;
            ottoDeliveryRequestInfo.BadgeNo3 = modifiedRequest.BadgeNo3;
            ottoDeliveryRequestInfo.IsCommonBadge = modifiedRequest.IsCommonBadge;
            ottoDeliveryRequestInfo.Notes = modifiedRequest.Notes;
            ottoDeliveryRequestInfo.RouteInfo = modifiedRequest.RouteInfo;
            if (modifiedRequest.Terminal != null && modifiedRequest.Terminal.Id > 0)
            {

                ottoDeliveryRequestInfo.PickupLocationType = PickupLocationType.Terminal;
                ottoDeliveryRequestInfo.TfxTerminal = new FreightModels.DropdownDisplayItem()
                {
                    Id = modifiedRequest.Terminal.Id,
                    Name = modifiedRequest.Terminal.Name
                };
            }
            else if (modifiedRequest.BulkPlant != null && !string.IsNullOrWhiteSpace(modifiedRequest.BulkPlant.SiteName))
            {
                ottoDeliveryRequestInfo.PickupLocationType = PickupLocationType.BulkPlant;
                ottoDeliveryRequestInfo.TfxBulkPlant = modifiedRequest.BulkPlant;
            }
            ottoScheduleInfo.DeliveryRequestInfo = ottoDeliveryRequestInfo;

            //set ottoShiftInfo
            OttoShiftInfo ottoShiftInfo = new OttoShiftInfo();
            ottoShiftInfo.ShiftId = trip.ShiftId;
            ottoShiftInfo.ShiftIndex = trip.ShiftIndex == null ? 0 : trip.ShiftIndex.Value;
            ottoShiftInfo.DriverRowIndex = trip.DriverRowIndex == null ? 0 : trip.DriverRowIndex.Value;
            ottoShiftInfo.DriverColIndex = trip.DriverColIndex == null ? 0 : trip.DriverColIndex.Value;
            ottoShiftInfo.RegionId = modifiedRequest.AssignedToRegionId;
            ottoShiftInfo.CompanyId = modifiedRequest.AssignedToCompanyId;
            ottoShiftInfo.StartTime = trip.ShiftStartTime;
            ottoShiftInfo.EndTime = trip.ShiftEndTime;
            ottoScheduleInfo.ShiftInfo = ottoShiftInfo;

            await mdbContext.OttoScheduleInfos.InsertOneAsync(ottoScheduleInfo);
        }
        private async Task UpdateOttoScheduleInfo(TripViewModel trip, DeliveryRequestViewModel modifiedRequest)
        {
            //update  ottoDeliveryRequestInfo
            OttoDeliveryRequestInfo ottoDeliveryRequestInfo = new OttoDeliveryRequestInfo();
            ottoDeliveryRequestInfo.BadgeNo1 = modifiedRequest.BadgeNo1;
            ottoDeliveryRequestInfo.BadgeNo2 = modifiedRequest.BadgeNo2;
            ottoDeliveryRequestInfo.BadgeNo3 = modifiedRequest.BadgeNo3;
            ottoDeliveryRequestInfo.IsCommonBadge = modifiedRequest.IsCommonBadge;
            ottoDeliveryRequestInfo.Notes = modifiedRequest.Notes;
            ottoDeliveryRequestInfo.RouteInfo = modifiedRequest.RouteInfo;
            if (modifiedRequest.Terminal != null && modifiedRequest.Terminal.Id > 0)
            {

                ottoDeliveryRequestInfo.PickupLocationType = PickupLocationType.Terminal;
                ottoDeliveryRequestInfo.TfxTerminal = new FreightModels.DropdownDisplayItem()
                {
                    Id = modifiedRequest.Terminal.Id,
                    Name = modifiedRequest.Terminal.Name
                };
            }
            else if (modifiedRequest.BulkPlant != null && !string.IsNullOrWhiteSpace(modifiedRequest.BulkPlant.SiteName))
            {
                ottoDeliveryRequestInfo.PickupLocationType = PickupLocationType.BulkPlant;
                ottoDeliveryRequestInfo.TfxBulkPlant = modifiedRequest.BulkPlant;
            }
            OttoShiftInfo ottoShiftInfo = new OttoShiftInfo();
            ottoShiftInfo.ShiftId = trip.ShiftId;
            ottoShiftInfo.ShiftIndex = trip.ShiftIndex == null ? 0 : trip.ShiftIndex.Value;
            ottoShiftInfo.DriverRowIndex = trip.DriverRowIndex == null ? 0 : trip.DriverRowIndex.Value;
            ottoShiftInfo.DriverColIndex = trip.DriverColIndex == null ? 0 : trip.DriverColIndex.Value;
            ottoShiftInfo.RegionId = modifiedRequest.AssignedToRegionId;
            ottoShiftInfo.CompanyId = modifiedRequest.AssignedToCompanyId;
            ottoShiftInfo.StartTime = trip.ShiftStartTime;
            ottoShiftInfo.EndTime = trip.ShiftEndTime;
            var ottoSchedule = Builders<OttoScheduleInfo>.Filter.And(
                         Builders<OttoScheduleInfo>.Filter.Where(t => t.SiteId == modifiedRequest.SiteId && t.TankId == modifiedRequest.TankId && t.StorageId == modifiedRequest.StorageId && t.TfxJobId == modifiedRequest.JobId));
            UpdateDefinition<OttoScheduleInfo> updateFields = null;
            updateFields = Builders<OttoScheduleInfo>.Update
                     .Set(t => t.ShiftInfo, ottoShiftInfo)
                     .Set(t => t.DeliveryRequestInfo, ottoDeliveryRequestInfo)
                     .Set(t => t.CreatedBy, modifiedRequest.CreatedBy)
                     .Set(t => t.CreatedDate, DateTime.Now);
            await mdbContext.OttoScheduleInfos.UpdateManyAsync(ottoSchedule, updateFields);
        }
        private async Task UpdateScheduleDeliveryInfo(DeliveryRequestViewModel modifiedRequest, DeliveryRequest deliveryRequest, ObjectId recurringId, List<RecurringDeliveryRequestDetails> recurringDRDetails)
        {
            foreach (var delItem in recurringDRDetails)
            {
                delItem.BadgeNo1 = modifiedRequest.BadgeNo1;
                delItem.BadgeNo2 = modifiedRequest.BadgeNo2;
                delItem.BadgeNo3 = modifiedRequest.BadgeNo3;
                delItem.IsCommonBadge = modifiedRequest.IsCommonBadge;
                delItem.TfxBulkPlant = deliveryRequest.TfxBulkPlant;
                delItem.TfxTerminal = deliveryRequest.TfxTerminal;
            }
            var recurringScheduleFilters = Builders<RecurringSchedules>.Filter.Eq(x => x.Id, recurringId);
            UpdateDefinition<RecurringSchedules> updateFields = null;
            updateFields = Builders<RecurringSchedules>.Update
                     .Set(t => t.DeliveryRequests, recurringDRDetails)
                    .Set(t => t.UpdatedBy, modifiedRequest.CreatedBy)
                    .Set(t => t.UpdatedOn, DateTime.Now);
            await mdbContext.RecurringSchedules.UpdateManyAsync(recurringScheduleFilters, updateFields);
        }

        private async Task UpdateScheduleShiftInfo(TripViewModel trip, DeliveryRequestViewModel modifiedRequest, ObjectId recurringId, ObjectId scheduleBuilderId)
        {
            RecurringShiftInfo recurringShiftInfo = new RecurringShiftInfo();
            recurringShiftInfo.ShiftId = trip.ShiftId;
            recurringShiftInfo.ShiftIndex = trip.ShiftIndex == null ? 0 : trip.ShiftIndex.Value;
            recurringShiftInfo.DriverRowIndex = trip.DriverRowIndex == null ? 0 : trip.DriverRowIndex.Value;
            recurringShiftInfo.DriverColIndex = trip.DriverColIndex == null ? 0 : trip.DriverColIndex.Value;
            recurringShiftInfo.RegionId = modifiedRequest.AssignedToRegionId;
            recurringShiftInfo.CompanyId = modifiedRequest.AssignedToCompanyId;
            recurringShiftInfo.StartTime = trip.ShiftStartTime;
            recurringShiftInfo.EndTime = trip.ShiftEndTime;
            var recurringScheduleFilters = Builders<RecurringSchedules>.Filter.Eq(x => x.Id, recurringId);
            UpdateDefinition<RecurringSchedules> updateFields = null;
            updateFields = Builders<RecurringSchedules>.Update
                    .Set(t => t.ScheduleBuilderId, scheduleBuilderId)
                    .Set(t => t.ShiftInfo, recurringShiftInfo)
                    .Set(t => t.UpdatedBy, modifiedRequest.CreatedBy)
                    .Set(t => t.UpdatedOn, DateTime.Now);
            await mdbContext.RecurringSchedules.UpdateManyAsync(recurringScheduleFilters, updateFields);
        }

        private async Task UpdateRouteShiftInformation(TripViewModel trip, string tripId, DeliveryRequestViewModel modifiedRequest, ObjectId id, int UserId)
        {
            var routeId = ObjectId.Parse(modifiedRequest.RouteInfo.Id);
            ShiftInfoViewModel shiftInfoModel = new ShiftInfoViewModel();
            shiftInfoModel.Id = trip.ShiftId;
            shiftInfoModel.TripId = tripId;
            shiftInfoModel.ShiftIndex = trip.ShiftIndex == null ? 0 : trip.ShiftIndex.Value + 1;
            shiftInfoModel.DriverRowIndex = trip.DriverRowIndex == null ? 0 : trip.DriverRowIndex.Value + 1;
            shiftInfoModel.DriverColIndex = trip.DriverColIndex == null ? 0 : trip.DriverColIndex.Value + 1;
            var ShiftInfo = shiftInfoModel.ToEntity();
            var routesFilters = Builders<RouteInformations>.Filter.Eq(x => x.Id, routeId);
            UpdateDefinition<RouteInformations> updateFields = null;
            updateFields = Builders<RouteInformations>.Update
                    .Set(t => t.ShiftInfo, ShiftInfo)
                    .Set(t => t.UpdatedBy, UserId)
                    .Set(t => t.UpdatedDate, DateTime.Now);
            await mdbContext.RouteInformations.UpdateManyAsync(routesFilters, updateFields);
        }
        private async Task UpdateParentDeliveryCarrierStatus(string Id, DeliveryGroupStatus tripStatus)
        {
            if (!string.IsNullOrEmpty(Id))
            {
                var deliveryFilters = Builders<DeliveryRequest>.Filter.And(
                          Builders<DeliveryRequest>.Filter.Where(t => t.BrokeredChildId == Id && !string.IsNullOrEmpty(t.BrokeredChildId)));
                UpdateDefinition<DeliveryRequest> updateFields = null;
                updateFields = Builders<DeliveryRequest>.Update
                        .Set(t => t.Status, tripStatus == DeliveryGroupStatus.Published ? DeliveryReqStatus.ScheduleCreated : DeliveryReqStatus.Draft);
                await mdbContext.DeliveryRequests.UpdateManyAsync(deliveryFilters, updateFields);
            }
        }
        private string GetTripId(ScheduleBuilder dsbEntity, TripViewModel trip)
        {
            string tripId = trip.TripId;
            var tripObjectId = dsbEntity.Trips.Where(t => t.ShiftId == trip.ShiftId && t.DriverRowIndex == trip.DriverRowIndex && t.DriverColIndex == trip.DriverColIndex).Select(t => t.TripId).FirstOrDefault();
            tripId = tripObjectId.ToString();
            return tripId;
        }

        private async Task<ScheduleBuilderViewModel> GetDefualtScheduleBuilder(int companyId, string regionId, string date, int sbView, List<RecurringShiftDetails> recurringShiftDetails)
        {
            ScheduleBuilderViewModel model = new ScheduleBuilderViewModel();
            model.RegionId = regionId;
            model.ObjectFilter = 1;
            model.RegionFilter = 1;
            //model.DateFilter = date.GetDateFilter();
            model.Date = date;
            model.CompanyId = companyId;
            model.TimeStamp = 0;
            model.Date = date;
            model.WindowMode = 1;
            model.Status = (int)DSBMethod.None;
            await GetDefaultShiftsForRegion(model, sbView, recurringShiftDetails);

            return model;
        }

        private async Task GetDefaultShiftsForRegion(ScheduleBuilderViewModel model, int sbView, List<RecurringShiftDetails> recurringShiftDetails)
        {
            ObjectId objectId = ObjectId.Parse(model.RegionId);
            var region = await mdbContext.Regions.Find(t => t.Id == objectId && t.IsActive && !t.IsDeleted).Project(t => new { t.SlotPeriod, t.TfxDrivers, t.TfxTrailers }).FirstOrDefaultAsync();
            var shifts = await mdbContext.Shifts.Find(t => t.RegionId == objectId && t.IsActive && !t.IsDeleted).Project(t => new ShiftModel() { Id = t.Id.ToString(), StartTime = t.StartTime, EndTime = t.EndTime }).ToListAsync();
            int slotPeriod = region.SlotPeriod;
            var shiftsNotInRegion = model.Shifts.Where(t => !shifts.Any(t1 => t1.Id.ToString() == t.Id)).ToList();
            shifts.AddRange(shiftsNotInRegion);
            //intialize shift order information.
            IntializeShiftOrderInfo(shifts);
            if (shifts != null && shifts.Any())
            {
                if (model.Shifts.Any())
                {
                    slotPeriod = model.Shifts[0].SlotPeriod;
                }
                shifts.ForEach(t => t.SlotPeriod = slotPeriod);
                var trailers = new List<TrailerModel>();
                var driversInRegion = new List<DriverAdditionalDetailsViewModel>();
                if (region.TfxTrailers != null && region.TfxTrailers.Any())
                {
                    trailers = GetTrailersForRegion(region.TfxTrailers);
                }
                var trailersNotInRegion = model.Trailers.Where(t => !trailers.Any(t1 => t1.Id == t.Id)).ToList();
                trailers.AddRange(trailersNotInRegion);
                if (region.TfxDrivers != null && region.TfxDrivers.Any())
                {
                    driversInRegion = GetDriversForRegion(region.TfxDrivers);
                }
                var driversInSbIds = model.Trips.SelectMany(t => t.Drivers).Where(t => t.Id > 0).GroupBy(t => t.Id).Select(t => t.FirstOrDefault().Id).ToList();
                var driversInSb = mdbContext.Drivers.Find(t => driversInSbIds.Contains(t.DriverId) && !t.IsDeleted)
                                                    .Project(t => new DriverAdditionalDetailsViewModel
                                                    {
                                                        Id = t.DriverId,
                                                        Name = t.DriverName,
                                                        IsFilldCompatible = t.IsFilldAuthorized
                                                    }).ToList();
                var driversNotInSb = driversInRegion.Where(t => !driversInSb.Any(t1 => t1.Id == t.Id)).ToList();
                var allDrivers = driversInSb.Concat(driversNotInSb).ToList();
                if ((!string.IsNullOrWhiteSpace(model.Id) && model.ObjectFilter == 2 && sbView == 0) || (sbView == 2 && !recurringShiftDetails.Any()))
                {
                    GetTrailers(model, shifts, slotPeriod, trailers);
                }
                else
                {
                    GetShifts(model, shifts, slotPeriod, trailers, allDrivers, recurringShiftDetails);
                }
            }
        }



        private List<DriverAdditionalDetailsViewModel> GetDriversForRegion(List<MdbDataAccess.Collections.DropdownDisplayItem> regionDrivers)
        {
            var driversInfo = regionDrivers.Select(x => x.Id).Distinct().ToList();
            var driversDetails = mdbContext.Drivers.Find(x => driversInfo.Contains(x.DriverId)).ToList();
            var response = new List<DriverAdditionalDetailsViewModel>();
            var drivers = (from r in driversDetails
                           select new
                           {
                               Id = r.DriverId,
                               Name = r.DriverName,
                               IsFilldAuthorized = r.IsFilldAuthorized,
                               Code = r.ShiftId != null ? r.ShiftId.Select(t => t.ToString()).ToList() : new List<string>(),
                           }).ToList();
            Parallel.ForEach(drivers, t =>
            {
                response.Add(new DriverAdditionalDetailsViewModel() { Id = t.Id, Name = t.Name, IsFilldCompatible = t.IsFilldAuthorized, Shifts = string.Join(",", t.Code) });
            });
            response = response.OrderBy(t => t.Name).ToList();
            return response;
        }

        private List<TrailerModel> GetTrailersForRegion(List<MdbDataAccess.Collections.DropdownDisplayItem> trailers)
        {
            List<TrailerModel> response = new List<TrailerModel>();
            var trailersCodes = trailers.Where(x => !string.IsNullOrEmpty(x.Code)).Select(x => ObjectId.Parse(x.Code)).Distinct().ToList();
            var truckDetails = mdbContext.TruckDetails.Find(x => trailersCodes.Contains(x.Id)).Project(x => new { x.Id, x.FuelCapacity, x.TrailerType, x.OptimizedCapacity, Compartments = x.Compartments.Select(t1 => t1.Capacity).ToList(), x.IsFilldCompatible }).ToList();
            Parallel.ForEach(truckDetails, t =>
            {
                response.Add(new TrailerModel { Id = t.Id.ToString(), TrailerId = trailers.FirstOrDefault(x1 => x1.Code == t.Id.ToString()) != null ? trailers.FirstOrDefault(x1 => x1.Code == t.Id.ToString()).Name : string.Empty, FuelCapacity = t.FuelCapacity, Compartments = t.Compartments.Count(), TrailerType = t.TrailerType.ToString(), IsFilldCompatible = t.IsFilldCompatible, OptimizedCapacity = t.OptimizedCapacity > 0 ? t.OptimizedCapacity : (t.Compartments.Sum() > 0 ? t.Compartments.Sum() : t.FuelCapacity) });
            });
            return response.OrderBy(t => t.TrailerId).ToList();
        }

        private static List<DriverAdditionalDetailsViewModel> GetDriverSequence(List<DriverAdditionalDetailsViewModel> regionDrivers, List<TripViewModel> trips, string shiftId)
        {
            List<DriverAdditionalDetailsViewModel> drivers = new List<DriverAdditionalDetailsViewModel>();
            var sbDrivers = trips.Where(t => t.ShiftId == shiftId).Where(t => t.Drivers.Any(t1 => t1.Id > 0)).Select(t => new { Driver = t.Drivers.First(), t.DriverRowIndex }).ToList();
            var notSbDrivers = regionDrivers.Where(t => !sbDrivers.Any(t1 => t1.Driver.Id == t.Id && t1.DriverRowIndex != null)).ToList();
            for (int i = 0; i < regionDrivers.Count; i++)
            {
                var sbDriver = sbDrivers.FirstOrDefault(t => t.DriverRowIndex != null && t.DriverRowIndex == i);
                if (sbDriver != null && sbDriver.Driver != null && sbDriver.Driver.Id > 0)
                {
                    drivers.Add(sbDriver.Driver);
                }
                else if (notSbDrivers.Count > 0)
                {
                    drivers.Add(notSbDrivers[0]);
                    notSbDrivers.RemoveAt(0);
                }
            }
            return drivers;
        }

        private static void GetShifts(ScheduleBuilderViewModel model, List<ShiftModel> shifts, int slotPeriod, List<TrailerModel> trailers, List<DriverAdditionalDetailsViewModel> drivers, List<RecurringShiftDetails> recurringShiftDetails)
        {
            var sbDate = Convert.ToDateTime(model.Date);
            int rowIndex, colIndex, shiftIndex = 0;
            var inputString = JsonConvert.SerializeObject(model.Trips);
            var sbTrips = JsonConvert.DeserializeObject<List<TripViewModel>>(inputString);
            model.Trips.Clear();
            foreach (var shift in shifts)
            {
                rowIndex = 0;
                if (drivers != null && drivers.Any())
                {
                    var shiftDrivers = GetDriverSequence(drivers, sbTrips, shift.Id);
                    foreach (var driver in shiftDrivers)
                    {
                        colIndex = 0;
                        int? maxColIndex = 0;
                        if (recurringShiftDetails != null && recurringShiftDetails.Count() > 0)
                        {
                            var maxColIndexDetails = recurringShiftDetails.Where(top => top.ShiftId == shift.Id && top.DriverRowIndex == rowIndex).FirstOrDefault();
                            if (maxColIndexDetails != null)
                            {
                                maxColIndex = maxColIndexDetails.DriverColIndex;
                            }
                            else
                            {
                                maxColIndex = sbTrips.Where(t => t.ShiftId == shift.Id && (t.Drivers.Any(t1 => t1.Id == driver.Id) || t.DriverRowIndex == rowIndex)).Max(t => t.DriverColIndex);
                            }
                        }
                        else
                        {
                            maxColIndex = sbTrips.Where(t => t.ShiftId == shift.Id && (t.Drivers.Any(t1 => t1.Id == driver.Id) || t.DriverRowIndex == rowIndex)).Max(t => t.DriverColIndex);
                        }
                        var scheduleStartTime = sbDate.Add(DateTime.Parse(shift.StartTime).TimeOfDay);
                        var scheduleEndTime = sbDate.Add(DateTime.Parse(shift.EndTime).TimeOfDay);
                        if (scheduleEndTime <= scheduleStartTime)
                        {
                            scheduleEndTime = scheduleEndTime.AddDays(1);
                        }

                        while (scheduleStartTime < scheduleEndTime || colIndex <= maxColIndex)
                        {
                            var trip = new TripViewModel();
                            trip.StartDate = scheduleStartTime.ToString(Resource.constFormatDate);
                            trip.StartTime = scheduleStartTime.ToShortTimeString();
                            trip.EndTime = scheduleStartTime.AddHours(slotPeriod).ToShortTimeString();
                            trip.SlotPeriod = shift.SlotPeriod;
                            var thisTripEndTime = scheduleStartTime.AddHours(slotPeriod);

                            var existingTrips = sbTrips.Where(t => t.ShiftId == shift.Id && t.DriverRowIndex == rowIndex && t.DriverColIndex == colIndex).ToList();
                            if (existingTrips.Any())
                            {
                                foreach (var existingTrip in existingTrips.OrderBy(t => t.DriverColIndex))
                                {
                                    UpdateIndexes(existingTrip, shiftIndex, rowIndex, colIndex);
                                    existingTrip.Trailers.ForEach(t =>
                                    {
                                        var loadTrailer = trailers.FirstOrDefault(t1 => t1.Id == t.Id);
                                        if (loadTrailer != null)
                                        {
                                            t.OptimizedCapacity = loadTrailer.OptimizedCapacity;
                                        }
                                    });
                                    model.Trips.Add(existingTrip);
                                    var startDate = existingTrip.StartDate;
                                    if (existingTrip.StartDate == null)
                                    {
                                        startDate = model.Date;
                                    }
                                    scheduleStartTime = GetEndDateTime(Convert.ToDateTime(startDate).Add(DateTime.Parse(existingTrip.StartTime).TimeOfDay), Convert.ToDateTime(startDate).Add(DateTime.Parse(existingTrip.EndTime).TimeOfDay));
                                    colIndex++;
                                }
                                continue;
                            }
                            scheduleStartTime = scheduleStartTime.AddHours(slotPeriod);
                            trip.DeliveryGroupStatus = DeliveryGroupStatus.Draft;
                            trip.ShiftId = shift.Id.ToString();
                            trip.ShiftStartTime = shift.StartTime;
                            trip.IsShiftCollapsed = false;
                            trip.ShiftEndTime = shift.EndTime;
                            trip.ShiftIndex = shiftIndex;
                            trip.DriverRowIndex = rowIndex;
                            trip.DriverColIndex = colIndex;
                            model.Trips.Add(trip);
                            colIndex++;
                        }
                        rowIndex++;
                    }
                }
                shiftIndex++;
                if (!model.Shifts.Any(t => t.Id == shift.Id.ToString()))
                {
                    model.Shifts.Add(new ShiftModel() { Id = shift.Id.ToString(), StartTime = shift.StartTime, EndTime = shift.EndTime, SlotPeriod = slotPeriod, OrderNo = shift.OrderNo });
                }
            }
            foreach (var trailer in trailers)
            {
                if (!model.Trailers.Any(t => t.Id == trailer.Id))
                {
                    model.Trailers.Add(new TrailerModel() { Id = trailer.Id, TrailerId = trailer.TrailerId, Compartments = trailer.Compartments, TrailerType = trailer.TrailerType, OptimizedCapacity = trailer.OptimizedCapacity });
                }
            }
        }

        private static void UpdateIndexes(TripViewModel existingTrip, int shiftIndex, int rowIndex, int colIndex)
        {
            if (existingTrip.ShiftIndex == null)
            {
                existingTrip.ShiftIndex = shiftIndex;
            }
            if (existingTrip.DriverRowIndex == null)
            {
                existingTrip.DriverRowIndex = rowIndex;
            }
            if (existingTrip.DriverColIndex == null)
            {
                existingTrip.DriverColIndex = colIndex;
            }
        }

        private static DateTime GetEndDateTime(DateTime startDateTime, DateTime endDateTime)
        {
            if (startDateTime > endDateTime)
            {
                endDateTime = endDateTime.AddDays(1);
            }
            return endDateTime;
        }

        private static void GetTrailers(ScheduleBuilderViewModel model, List<ShiftModel> shifts, int slotPeriod, List<TrailerModel> trailers)
        {
            var sbDate = Convert.ToDateTime(model.Date);
            int rowIndex, colIndex;
            var inputString = JsonConvert.SerializeObject(model.Trips);
            var sbTrips = JsonConvert.DeserializeObject<List<TripViewModel>>(inputString);
            model.Trips.Clear();
            if (trailers != null && trailers.Any())
            {
                foreach (var trailer in trailers)
                {
                    rowIndex = 0;
                    foreach (var shift in shifts)
                    {
                        colIndex = 0;
                        var tripMaxTime = sbTrips.Where(t => t.ShiftId == shift.Id && t.Trailers.Any(t1 => t1.Id == trailer.Id)).Select(t => GetEndDateTime(Convert.ToDateTime(t.StartDate).Add(DateTime.Parse(t.StartTime).TimeOfDay), Convert.ToDateTime(t.StartDate).Add(DateTime.Parse(t.EndTime).TimeOfDay))).OrderByDescending(t => t).FirstOrDefault();
                        var scheduleStartTime = sbDate.Add(DateTime.Parse(shift.StartTime).TimeOfDay);
                        var scheduleEndTime = sbDate.Add(DateTime.Parse(shift.EndTime).TimeOfDay);
                        if (scheduleEndTime <= scheduleStartTime)
                        {
                            scheduleEndTime = scheduleEndTime.AddDays(1);
                        }

                        if (tripMaxTime > scheduleEndTime)
                        {
                            scheduleEndTime = tripMaxTime;
                        }
                        while (scheduleStartTime < scheduleEndTime)
                        {
                            var trip = new TripViewModel();
                            trip.StartDate = scheduleStartTime.ToString(Resource.constFormatDate);
                            trip.StartTime = scheduleStartTime.ToShortTimeString();
                            trip.EndTime = scheduleStartTime.AddHours(slotPeriod).ToShortTimeString();
                            var thisTripEndTime = scheduleStartTime.AddHours(slotPeriod);
                            var existingTrips = sbTrips.Where(t => t.ShiftId == shift.Id && t.Trailers.Any(t1 => t1.Id == trailer.Id) && Convert.ToDateTime(t.StartDate).Add(DateTime.Parse(t.StartTime).TimeOfDay) < thisTripEndTime
                                                        && scheduleStartTime < GetEndDateTime(Convert.ToDateTime(t.StartDate).Add(DateTime.Parse(t.StartTime).TimeOfDay), Convert.ToDateTime(t.StartDate).Add(DateTime.Parse(t.EndTime).TimeOfDay))).ToList();
                            if (existingTrips.Any())
                            {
                                scheduleStartTime = existingTrips.Select(t => GetEndDateTime(Convert.ToDateTime(t.StartDate).Add(DateTime.Parse(t.StartTime).TimeOfDay), Convert.ToDateTime(t.StartDate).Add(DateTime.Parse(t.EndTime).TimeOfDay))).OrderByDescending(t => t).FirstOrDefault();
                                foreach (var existingTrip in existingTrips.OrderBy(t => Convert.ToDateTime(t.StartDate).Add(DateTime.Parse(t.StartTime).TimeOfDay)))
                                {
                                    existingTrip.TrailerRowIndex = rowIndex;
                                    existingTrip.TrailerColIndex = colIndex;
                                    if (existingTrip.Trailers.Count > 1)
                                    {
                                        var tripStr = JsonConvert.SerializeObject(existingTrip);
                                        trip = JsonConvert.DeserializeObject<TripViewModel>(tripStr);
                                        trip.Trailers.Clear();
                                        trip.Trailers.Add(new TrailerModel() { Id = trailer.Id, TrailerId = trailer.TrailerId, Compartments = trailer.Compartments, TrailerType = trailer.TrailerType });
                                        model.Trips.Add(trip);
                                    }
                                    else
                                    {
                                        model.Trips.Add(existingTrip);
                                    }
                                    colIndex++;
                                }
                                continue;
                            }
                            scheduleStartTime = scheduleStartTime.AddHours(slotPeriod);
                            trip.DeliveryGroupStatus = DeliveryGroupStatus.Draft;
                            trip.ShiftId = shift.Id.ToString();
                            trip.ShiftStartTime = shift.StartTime;
                            trip.IsShiftCollapsed = false;
                            trip.ShiftEndTime = shift.EndTime;
                            trip.TrailerRowIndex = rowIndex;
                            trip.TrailerColIndex = colIndex;
                            trip.DriverRowIndex = colIndex;
                            trip.DriverColIndex = rowIndex;
                            trip.Trailers.Add(new TrailerModel() { Id = trailer.Id, TrailerId = trailer.TrailerId, Compartments = trailer.Compartments, TrailerType = trailer.TrailerType });
                            model.Trips.Add(trip);
                            colIndex++;
                        }
                        rowIndex++;
                        if (!model.Shifts.Any(t => t.Id == shift.Id.ToString()))
                        {
                            model.Shifts.Add(new ShiftModel() { Id = shift.Id.ToString(), StartTime = shift.StartTime, EndTime = shift.EndTime, SlotPeriod = slotPeriod });
                        }
                    }
                    if (!model.Trailers.Any(t => t.Id == trailer.Id))
                    {
                        model.Trailers.Add(new TrailerModel() { Id = trailer.Id, TrailerId = trailer.TrailerId, Compartments = trailer.Compartments, TrailerType = trailer.TrailerType });
                    }
                }
            }
        }

        public async Task<List<Exchange.Utilities.DropdownDisplayItem>> GetAllDrivers(int companyId, List<string> trailerTypeId, string regionId, DateTimeOffset selectedDate)

        {
            var response = new List<Exchange.Utilities.DropdownDisplayItem>();
            var truckDrivers = new List<Exchange.Utilities.DropdownDisplayItem>();
            var regionDrivers = new List<Exchange.Utilities.DropdownDisplayItem>();
            try
            {
                ObjectId objregionId = new ObjectId(regionId);
                var regionDriverList = mdbContext.Regions.Find(top => top.Id == objregionId).Project(t => t.TfxDrivers).FirstOrDefault();
                if (regionDriverList != null)
                {
                    regionDrivers = regionDriverList.Select(t => new Exchange.Utilities.DropdownDisplayItem() { Id = t.Id, Name = t.Name }).ToList();
                }
                var tractorDetails = await mdbContext.TractorDetails.Find(x => x.TfxCompanyId == companyId && !x.IsDeleted).ToListAsync();
                if (tractorDetails != null)
                {
                    if (trailerTypeId != null)
                    {
                        List<ObjectId> truckIds = trailerTypeId.Select(t => ObjectId.Parse(t)).ToList();
                        var truckDetails = mdbContext.TruckDetails.Find(x => truckIds.Contains(x.Id) && !x.IsDeleted).Project(t => new { t.TrailerType, t.IsFilldCompatible }).ToList();
                        if (truckDetails != null && truckDetails.Any())
                        {
                            var trailerTypeStatus = truckDetails.Select(t => t.TrailerType).ToList();
                            var trailerAssociatedDriver = tractorDetails.Where(t => t.TrailerType.Intersect(trailerTypeStatus).Any()).SelectMany(t => t.Drivers).Where(t => t != null && t.TfxId > 0).GroupBy(t => t.TfxId).Select(t => t.FirstOrDefault()).ToList();
                            if (trailerAssociatedDriver != null && trailerAssociatedDriver.Any())
                            {
                                trailerAssociatedDriver.ForEach(t => truckDrivers.Add(new Exchange.Utilities.DropdownDisplayItem { Id = t.TfxId, Name = t.TfxName }));
                            }
                            response = regionDrivers.Where(t => truckDrivers.Any(t1 => t1.Id == t.Id)).ToList();
                        }
                    }
                    else
                    {
                        response = regionDrivers.Distinct().ToList();
                    }
                    return response;
                }
                if (response != null && response.Any())
                {
                    var tDriverIds = response.Select(t => t.Id).ToList();
                    var tDrivers = mdbContext.Drivers.Find(t => tDriverIds.Contains(t.DriverId) && !t.IsDeleted)
                                                            .Project(t => new { Id = t.DriverId, Name = t.DriverName, Code = t.IsFilldAuthorized.ToString() })
                                                            .ToList();
                }
            }
            catch (Exception ex)
            {
                Exchange.Logger.LogManager.Logger.WriteException("ScheduleBuilderRepository", "GetAllDrivers", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DriverAdditionalDetailsViewModel>> GetAllDriverDetailsAsync(int companyId, List<string> trailerTypeId, string regionId, DateTimeOffset selectedDate)

        {
            var response = new List<DriverAdditionalDetailsViewModel>();
            var allDrivers = new List<Exchange.Utilities.DropdownDisplayItem>();
            var truckDrivers = new List<Exchange.Utilities.DropdownDisplayItem>();
            var regionDrivers = new List<Exchange.Utilities.DropdownDisplayItem>();
            try
            {
                ObjectId objregionId = new ObjectId(regionId);
                var regionDriverList = mdbContext.Regions.Find(top => top.Id == objregionId).Project(t => t.TfxDrivers).FirstOrDefault();
                if (regionDriverList != null)
                {
                    regionDrivers = regionDriverList.Select(t => new Exchange.Utilities.DropdownDisplayItem() { Id = t.Id, Name = t.Name }).ToList();
                }
                var tractorDetails = await mdbContext.TractorDetails.Find(x => x.TfxCompanyId == companyId && !x.IsDeleted).ToListAsync();
                if (tractorDetails != null)
                {
                    if (trailerTypeId != null)
                    {
                        List<ObjectId> truckIds = trailerTypeId.Select(t => ObjectId.Parse(t)).ToList();
                        var truckDetails = mdbContext.TruckDetails.Find(x => truckIds.Contains(x.Id) && !x.IsDeleted).Project(t => new { t.TrailerType, t.IsFilldCompatible }).ToList();
                        if (truckDetails != null && truckDetails.Any())
                        {
                            var trailerTypeStatus = truckDetails.Select(t => t.TrailerType).ToList();
                            var trailerAssociatedDriver = tractorDetails.Where(t => t.TrailerType.Intersect(trailerTypeStatus).Any()).SelectMany(t => t.Drivers).Where(t => t != null && t.TfxId > 0).GroupBy(t => t.TfxId).Select(t => t.FirstOrDefault()).ToList();
                            if (trailerAssociatedDriver != null && trailerAssociatedDriver.Any())
                            {
                                trailerAssociatedDriver.ForEach(t => truckDrivers.Add(new Exchange.Utilities.DropdownDisplayItem { Id = t.TfxId, Name = t.TfxName }));
                            }
                            allDrivers = regionDrivers.Where(t => truckDrivers.Any(t1 => t1.Id == t.Id)).ToList();
                        }
                    }
                    else
                    {
                        allDrivers = regionDrivers.Distinct().ToList();
                    }
                }
                if (allDrivers != null && allDrivers.Any())
                {
                    var tDriverIds = allDrivers.Select(t => t.Id).ToList();
                    response = mdbContext.Drivers.Find(t => tDriverIds.Contains(t.DriverId) && !t.IsDeleted)
                                                            .Project(t => new DriverAdditionalDetailsViewModel { Id = t.DriverId, Name = t.DriverName, IsFilldCompatible = t.IsFilldAuthorized })
                                                            .ToList();
                }
            }
            catch (Exception ex)
            {
                Exchange.Logger.LogManager.Logger.WriteException("ScheduleBuilderRepository", "GetAllDriverDetailsAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DriverScheduleViewModel>> GetSelectedDateDriverScheduleByDriverId(int driverId, DateTimeOffset selectedDate)
        {
            var response = getSelectedDateScheduleByDriverId(driverId, selectedDate);
            return response;
        }

        private List<DriverScheduleViewModel> getSelectedDateScheduleByDriverId(int driverId, DateTimeOffset selectedDate)
        {
            var response = new List<DriverScheduleViewModel>();
            var driverSchedule = mdbContext.DriverScheduleShiftMapping.Find(x => x.IsActive && x.DriverId == driverId).ToList();
            if (!(selectedDate.Offset.Days == 0 && selectedDate.Offset.Hours == 0 && selectedDate.Offset.Minutes == 0))
            {
                selectedDate = selectedDate.ToUniversalTime();
            }

            var list = driverSchedule.Select(s => new DriverScheduleShiftMapping
            {
                Id = s.Id,
                DriverId = s.DriverId,
                IsActive = s.IsActive,

                DriverScheduleList = s.DriverScheduleList.Select(x => new DriverSchedule()
                {
                    Id = x.Id,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    IsActive = x.IsActive,
                    RepeatDayList = x.RepeatDayList,
                    ShiftId = x.ShiftId,

                }).ToList(),

            }).ToList();

            foreach (var item in list)
            {
                foreach (var schedule in item.DriverScheduleList)
                {
                    if (schedule.RepeatDayList != null)
                    {
                        DateTimeOffset sch = schedule.RepeatDayList.FirstOrDefault(f => f.ToString("MM/dd/YYYY") == selectedDate.ToString("MM/dd/YYYY"));
                        if (sch > DateTimeOffset.MinValue)
                        {
                            response.Add(schedule.ToDriverScheduleModel());
                        }
                    }

                }
            }
            return response;
        }

        private bool isSelectedDateShiftExist(int driverId, DateTimeOffset selectedDate, string shiftId)
        {
            List<DriverScheduleViewModel> model = getSelectedDateScheduleByDriverId(driverId, selectedDate);
            DriverScheduleViewModel todaySchedule = model.Where(w => w.ShiftId == shiftId).FirstOrDefault();
            if (todaySchedule != null)
            {
                return true;
            }

            return false;

        }
        private async Task CreateDriverSchedule(ScheduleBuilderViewModel scheduleBuilder)
        {
            DateTimeOffset datetime = Convert.ToDateTime(scheduleBuilder.Date);
            foreach (var trip in scheduleBuilder.Trips)
            {
                bool isShiftExist = isSelectedDateShiftExist(trip.Drivers.FirstOrDefault().Id, datetime, trip.ShiftId);
                if (String.IsNullOrEmpty(trip.DriverScheduleMappingId) && trip.DeliveryGroupPrevStatus != Exchange.Utilities.DeliveryGroupStatus.Published && !isShiftExist)
                {
                    var model = initializeDriverSchedule(scheduleBuilder, trip);
                    var driverSchedule = model.ToEntity();
                    await mdbContext.DriverScheduleShiftMapping.InsertOneAsync(driverSchedule);
                    trip.DriverScheduleMappingId = driverSchedule.Id.ToString();
                }
                else if (!String.IsNullOrEmpty(trip.DriverScheduleMappingId))
                {
                    var model = initializeDriverSchedule(scheduleBuilder, trip);
                    var driverSchedule = model.ToEntity();

                    ObjectId id = new ObjectId(trip.DriverScheduleMappingId);
                    var filter = Builders<DriverScheduleShiftMapping>.Filter.And(
                          Builders<DriverScheduleShiftMapping>.Filter.Where(t => t.Id == id)
                                         );
                    if (isShiftExist)
                    {
                        var updateFields = Builders<DriverScheduleShiftMapping>.Update
                          .Set(t => t.DriverId, driverSchedule.DriverId)
                          .Set(t => t.DriverScheduleList, driverSchedule.DriverScheduleList)
                           .Set(t => t.IsActive, false)
                           .Set(t => t.IsDeleted, true);
                        await mdbContext.DriverScheduleShiftMapping.UpdateOneAsync(filter, updateFields);
                    }
                    else
                    {
                        var updateFields = Builders<DriverScheduleShiftMapping>.Update
                        .Set(t => t.DriverId, driverSchedule.DriverId)
                        .Set(t => t.DriverScheduleList, driverSchedule.DriverScheduleList);
                        await mdbContext.DriverScheduleShiftMapping.UpdateOneAsync(filter, updateFields);
                    }
                }
            }
        }

        private DriverScheduleMappingViewModel initializeDriverSchedule(ScheduleBuilderViewModel scheduleBuilder, TripViewModel trip)
        {
            DateTimeOffset datetime = Convert.ToDateTime(scheduleBuilder.Date);
            var model = new DriverScheduleMappingViewModel();
            model.DriverId = trip.Drivers.FirstOrDefault().Id;
            model.IsActive = true;
            model.IsUnplanedSchedule = true;
            var schedulelist = new List<DriverScheduleViewModel>();
            // DateTimeOffset datetime = Convert.ToDateTime(scheduleBuilder.Date);
            DateTimeOffset startDate = datetime;
            DateTimeOffset endDate = datetime.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            var RepeatDayList = new List<DateTimeOffset>();
            RepeatDayList.Add(startDate);
            var driverschedule = new DriverScheduleViewModel()
            {
                StartDate = startDate.DateTime,
                EndDate = endDate.DateTime,
                IsActive = true,
                RepeatDayList = RepeatDayList.Select(t => t.DateTime).ToList(),
                Id = model.DriverId.ToString() + "_" + DateTimeOffset.Now.ToUnixTimeSeconds(),
                ShiftId = trip.ShiftId,
                Description = " Load Code (" + (trip.LoadCode) + " )",// "("+ trip.StartTime + " - " + trip.EndTime+")",

            };
            schedulelist.Add(driverschedule);
            model.ScheduleList = schedulelist;
            return model;
        }

        private async Task DeleteDriverSchedule(string driverScheduleMappingId)
        {
            ObjectId id = new ObjectId(driverScheduleMappingId);
            var filter = Builders<DriverScheduleShiftMapping>.Filter.And(
                  Builders<DriverScheduleShiftMapping>.Filter.Where(t => t.Id == id)
                                 );
            var updateFields = Builders<DriverScheduleShiftMapping>.Update
                .Set(t => t.IsActive, false)
                .Set(t => t.IsDeleted, true);

            await mdbContext.DriverScheduleShiftMapping.UpdateOneAsync(filter, updateFields);
        }

        public async Task<List<DeliveryRequestViewModel>> ValidateTrailerJobCompatibility(List<TrailerModel> trailers, List<DeliveryRequestViewModel> deliveryRequests)
        {
            List<DeliveryRequestViewModel> response = new List<DeliveryRequestViewModel>();
            try
            {
                var truckIds = trailers.Select(c => c.TrailerId).ToList();
                var jobIds = deliveryRequests.Select(c => c.JobId).ToList();
                var trucksDetails = await mdbContext.TruckDetails.Find(t => !t.IsDeleted && truckIds.Contains(t.TruckId)).ToListAsync();
                var jobsAdditionalDetails = await mdbContext.JobAdditionalDetails.Find(t => t.IsActive && jobIds.Contains(t.TfxJobId)).ToListAsync();

                foreach (var trailer in trailers)
                {
                    var trukDetails = trucksDetails.Where(t => t.TruckId == trailer.TrailerId && !t.IsDeleted).FirstOrDefault();
                    foreach (var deliveryRequest in deliveryRequests)
                    {
                        var jobAdditionalDetails = jobsAdditionalDetails.Where(t => t.TfxJobId == deliveryRequest.JobId && t.IsActive)
                                                                        .Select(a => new { a.TrailerType, a.TfxJobId }).FirstOrDefault();
                        if (jobAdditionalDetails == null)
                        {
                            response.Add(deliveryRequest);
                        }
                        else
                        {
                            if (jobAdditionalDetails.TrailerType == null)
                            {
                                response.Add(deliveryRequest);
                            }
                            if (trukDetails != null && jobAdditionalDetails != null && jobAdditionalDetails.TrailerType != null && !jobAdditionalDetails.TrailerType.Contains(trukDetails.TrailerType))
                            {
                                response.Add(deliveryRequest);
                            }
                            if (trukDetails == null || jobAdditionalDetails == null || jobAdditionalDetails.TrailerType == null)
                            {
                                LogManager.Logger.WriteDebug("ScheduleBuilderRepository", "ValidateTrailerJobCompatibility", " Trailers : " + JsonConvert.SerializeObject(trailers) + "dr Ids :" + string.Join(",", deliveryRequests.Select(t => t.Id).ToList()));
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Exchange.Logger.LogManager.Logger.WriteException("ScheduleBuilderRepository", "ValidateTrailerJobCompatibility", ex.Message + " Trailers : " + JsonConvert.SerializeObject(trailers) + "dr Ids :" + string.Join(",", deliveryRequests.Select(t => t.Id).ToList()), ex);
            }
            return response;
        }

        public async Task<List<TrailerJobNonCompatibleDrs>> ValidateTrailerJobCompatibilityForLoadQueue(List<TrailersDeliveryRequestViewModel> models)
        {
            List<TrailerJobNonCompatibleDrs> response = new List<TrailerJobNonCompatibleDrs>();
            try
            {
                foreach (var model in models)
                {
                    var temp = new TrailerJobNonCompatibleDrs();
                    temp.ShiftIndex = model.ShiftIndex;
                    temp.ScheduleIndex = model.ScheduleIndex;
                    temp.DrCount = 0;

                    var trailers = model.trailers;
                    var deliveryRequests = model.deliveryRequests;

                    var truckIds = trailers.Select(c => c.TrailerId).ToList();
                    var jobIds = deliveryRequests.Select(c => c.JobId).ToList();
                    var trucksDetails = await mdbContext.TruckDetails.Find(t => !t.IsDeleted && truckIds.Contains(t.TruckId)).ToListAsync();
                    var jobsAdditionalDetails = await mdbContext.JobAdditionalDetails.Find(t => t.IsActive && jobIds.Contains(t.TfxJobId)).ToListAsync();

                    foreach (var trailer in trailers)
                    {
                        var trukDetails = trucksDetails.Where(t => t.TruckId == trailer.TrailerId && !t.IsDeleted).FirstOrDefault();
                        foreach (var deliveryRequest in deliveryRequests)
                        {
                            var jobAdditionalDetails = jobsAdditionalDetails.Where(t => t.TfxJobId == deliveryRequest.JobId && t.IsActive)
                                                                            .Select(a => new { a.TrailerType, a.TfxJobId }).FirstOrDefault();
                            if (jobAdditionalDetails == null)
                            {
                                temp.DrCount++;
                            }
                            else
                            {
                                if (jobAdditionalDetails.TrailerType == null || !jobAdditionalDetails.TrailerType.Contains(trukDetails.TrailerType))
                                {
                                    temp.DrCount++;
                                }
                            }
                        }
                    }
                    response.Add(temp);
                }
            }

            catch (Exception ex)
            {
                Exchange.Logger.LogManager.Logger.WriteException("ScheduleBuilderRepository", "ValidateTrailerJobCompatibilityForLoadQueue", ex.Message, ex);
            }
            return response;
        }
        private string GetStatusMessage(DSBSaveModel model)
        {
            string statusMessage = Resource.valMessageSbSaveSuccess;
            if (model.Trips.Count == 1)
            {
                if (model.Trips[0].GroupId == 0)
                {
                    statusMessage = string.Format(model.DSBFilter == 1 ? Resource.valMessageLoadSaveSuccess : Resource.valMessageInColumnLoadSaveSuccess, model.Trips[0].DriverColIndex + 1, model.Trips[0].DriverRowIndex + 1, model.Trips[0].ShiftIndex + 1);
                }
                else
                {
                    statusMessage = string.Format(model.DSBFilter == 1 ? Resource.valMessageLoadPublishSuccess : Resource.valMessageInColumnLoadPublishSuccess, model.Trips[0].DriverColIndex + 1, model.Trips[0].DriverRowIndex + 1, model.Trips[0].ShiftIndex + 1);
                }
            }
            else if (model.Trips.Count == 2)
            {
                if (model.Trips.All(t => t.GroupId == 0))
                {
                    statusMessage = string.Format(model.DSBFilter == 1 ? Resource.valMessageLoadDragDropSaveSuccess : Resource.valMessageInColumnLoadDragDropSaveSuccess, model.Trips[0].DriverColIndex + 1, model.Trips[0].DriverRowIndex + 1, model.Trips[0].ShiftIndex + 1, model.Trips[1].DriverColIndex + 1, model.Trips[1].DriverRowIndex + 1, model.Trips[1].ShiftIndex + 1);
                }
                else if (model.Trips.All(t => t.GroupId > 0))
                {
                    statusMessage = string.Format(model.DSBFilter == 1 ? Resource.valMessageLoadDragDropPublishSuccess : Resource.valMessageInColumnLoadDragDropPublishSuccess, model.Trips[0].DriverColIndex + 1, model.Trips[0].DriverRowIndex + 1, model.Trips[0].ShiftIndex + 1, model.Trips[1].DriverColIndex + 1, model.Trips[1].DriverRowIndex + 1, model.Trips[1].ShiftIndex + 1);
                }
                else
                {
                    statusMessage = Resource.valMessageSbPublishSuccess;
                }
            }
            else if (model.Trips.All(t => t.GroupId > 0))
            {
                statusMessage = Resource.valMessageSbPublishSuccess;
            }
            return statusMessage;
        }

        public async Task<List<ScheduleBuilderViewModel>> GetScheduleBuildersByDrIds(List<string> drIds)
        {
            var response = new List<ScheduleBuilderViewModel>();
            var objectIds = drIds.Select(t => ObjectId.Parse(t)).ToList();
            var drFilter = Builders<DeliveryRequest>.Filter.And(
                    Builders<DeliveryRequest>.Filter.In(t => t.Id, objectIds),
                    Builders<DeliveryRequest>.Filter.Where(t => t.ScheduleBuilderId != null)
                );
            var dsbIds = mdbContext.DeliveryRequests.Find(drFilter).Project(t => t.ScheduleBuilderId).ToList();

            var dsbObjectIds = dsbIds.Distinct().Select(t => ObjectId.Parse(t)).ToList();
            var dsbFilter = Builders<ScheduleBuilder>.Filter.In(t => t.Id, dsbObjectIds);
            var scheduleBuilders = mdbContext.ScheduleBuilders.Find(dsbFilter).ToList();
            foreach (var scheduleBuilder in scheduleBuilders)
            {
                var dsbDate = scheduleBuilder.DateFilter.ToString(Resource.constFormatDate);
                var dsbModel = await GetScheduleBuilderViewModel(scheduleBuilder, scheduleBuilder.TfxCompanyId, scheduleBuilder.CreatedBy, scheduleBuilder.RegionId, dsbDate, scheduleBuilder.ObjectFilter, scheduleBuilder.DSBFilter, null, string.Empty);
                response.Add(dsbModel);
            }
            return response;
        }

        private StatusModel ValidateRegion(ProcessDSBCreation inputModel, List<DeliveryRequestViewModel> deliveryRequests)
        {
            StatusModel status = new StatusModel();
            var companyIds = inputModel.ScheduleDetails.Select(t => t.SupplierCompanyId).ToList();
            companyIds.Add(inputModel.CarrierCompanyId);
            var jobIds = inputModel.ScheduleDetails.Select(t => t.JobId).ToList();
            var filter = Builders<Region>.Filter.And(
                        Builders<Region>.Filter.Where(x => x.IsActive && !x.IsDeleted && x.TfxJobs.Any(x1 => jobIds.Contains(x1.Id))),
                        Builders<Region>.Filter.In(x => x.TfxCompanyId, companyIds)
                    );
            var regions = mdbContext.Regions.Find(filter).Project(t => new { t.Id, t.TfxJobs, t.TfxCompanyId, t.SlotPeriod, t.TfxDrivers }).SortByDescending(t => t.Id).ToList();
            List<string> carrierRegionIds = new List<string>();
            var carrierRegions = regions.Where(t => (t.TfxCompanyId == inputModel.CarrierCompanyId) && t.TfxJobs.Any(t1 => jobIds.Contains(t1.Id)));
            if (!carrierRegions.Any())
            {
                status.StatusMessage = Resource.valMessageJobNotAssignedToRegion;
                return status;
            }
            if (carrierRegions.Count() > 1)
            {
                status.StatusMessage = Resource.valMessageMultipleRegionsAvailable;
                return status;
            }
            var carrierRegion = carrierRegions.FirstOrDefault();
            var regionShifts = mdbContext.Shifts.Find(t => t.RegionId == carrierRegion.Id && !t.IsDeleted && t.IsActive).Project(t => new
            {
                t.Id,
                t.EndTime,
                t.StartTime
            }).ToList();
            if (!regionShifts.Any())
            {
                status.StatusMessage = Resource.valMessageNoShiftsAvailable;
                return status;
            }
            inputModel.CarrierRegionId = carrierRegion.Id.ToString();
            inputModel.SlotPeriod = carrierRegion.SlotPeriod;
            if (!carrierRegion.TfxDrivers.Any(t => t.Id == inputModel.Drivers.Id))
            {
                var regionFilter = Builders<Region>.Filter.Eq(e => e.Id, carrierRegion.Id);
                var driver = new MdbDataAccess.Collections.DropdownDisplayItem() { Id = inputModel.Drivers.Id, Name = inputModel.Drivers.Name };
                var update = Builders<Region>.Update
                        .Push<MdbDataAccess.Collections.DropdownDisplayItem>(e => e.TfxDrivers, driver);

                mdbContext.Regions.FindOneAndUpdate(regionFilter, update);
            }
            deliveryRequests.ForEach(t => { t.CreatedByRegionId = inputModel.CarrierRegionId; t.AssignedToRegionId = inputModel.CarrierRegionId; });
            regionShifts.ForEach(t => inputModel.Shifts.Add(new ShiftModel() { Id = t.Id.ToString(), SlotPeriod = inputModel.SlotPeriod, StartTime = t.StartTime, EndTime = t.EndTime }));
            foreach (var request in deliveryRequests)
            {
                var supplierRegion = regions.FirstOrDefault(t => (t.TfxCompanyId == request.SupplierCompanyId) && t.TfxJobs.Any(t1 => t1.Id == request.JobId));
                if (supplierRegion != null)
                {
                    request.CreatedByRegionId = supplierRegion.Id.ToString();
                }
            }
            status.StatusCode = (int)Status.Success;
            return status;
        }

        private async Task<DSBSaveModel> GetScheduleBuilderModel(ProcessDSBCreation inputModel, List<DeliveryRequestViewModel> deliveryRequests)
        {
            DSBSaveModel response = new DSBSaveModel();
            var status = ValidateRegion(inputModel, deliveryRequests);
            if (status.StatusCode == (int)Status.Failed)
            {
                response.StatusCode = status.StatusCode;
                response.StatusMessage = status.StatusMessage;
                return response;
            }

            DateTimeOffset loadDate = Convert.ToDateTime(inputModel.Date);
            TimeSpan loadStartTime = DateTime.Parse(inputModel.StartTime).TimeOfDay;
            var loadStartDateTime = loadDate.Add(loadStartTime);
            TimeSpan loadEndTime = DateTime.Parse(inputModel.EndTime).TimeOfDay;
            var loadEndDateTime = loadDate.Add(loadEndTime);
            if (loadEndDateTime < loadStartDateTime)
            {
                loadEndDateTime = loadEndDateTime.AddDays(1);
            }

            var shifts = new List<ShiftTimings>();
            foreach (var shift in inputModel.Shifts)
            {
                var shiftStartTime = loadDate.Add(DateTime.Parse(shift.StartTime).TimeOfDay);
                var shiftEndTime = loadDate.Add(DateTime.Parse(shift.EndTime).TimeOfDay);
                if (shiftEndTime <= shiftStartTime)
                {
                    shiftEndTime = shiftEndTime.AddDays(1);
                }

                shifts.Add(new ShiftTimings() { ShiftId = shift.Id.ToString(), ShiftStartTime = shiftStartTime, ShiftEndTime = shiftEndTime });
            }
            if (!shifts.Any(t => loadStartDateTime >= t.ShiftStartTime && loadStartDateTime < t.ShiftEndTime))
            {
                shifts.ForEach(t => { t.ShiftStartTime = t.ShiftStartTime.AddDays(-1); t.ShiftEndTime = t.ShiftEndTime.AddDays(-1); });
            }
            var loadShift = shifts.FirstOrDefault(t => loadStartDateTime >= t.ShiftStartTime && loadStartDateTime < t.ShiftEndTime);
            if (loadShift == null)
            {
                loadShift = shifts[0];
            }
            var shiftIndex = shifts.FindIndex(t => t.ShiftId == loadShift.ShiftId);
            var dsbDate = loadShift.ShiftStartTime.Date;
            response.Date = dsbDate.ToString(Resource.constFormatDate);
            response.Shifts = inputModel.Shifts;
            int rowIndex = 0, colIndex = 0;
            var existingDsb = mdbContext.ScheduleBuilders.Find(t => t.TfxCompanyId == inputModel.CarrierCompanyId && t.RegionId == inputModel.CarrierRegionId && t.DateFilter == dsbDate).FirstOrDefault();
            if (existingDsb != null)
            {
                response.Id = existingDsb.Id.ToString();
                var existingRow = existingDsb.Trips.Where(t => t.ShiftId == loadShift.ShiftId && t.TfxDrivers.Any(t1 => t1.Id == inputModel.Drivers.Id)).ToList();
                if (existingRow != null && existingRow.Any())
                {
                    rowIndex = existingRow[0].DriverRowIndex.Value;
                    colIndex = existingRow.Count;
                    var existingTrips = existingRow.Where(t => t.StartDate == loadDate && t.StartTime == loadStartTime && t.EndTime == loadEndTime).ToList();
                    if (existingTrips != null && existingTrips.Any())
                    {
                        foreach (var existingTrip in existingTrips)
                        {
                            List<ObjectId> deliveryRequestIds = existingTrip.DeliveryRequests;
                            List<DeliveryRequest> deliveryRequestEntities = await mdbContext.DeliveryRequests.Find(t => deliveryRequestIds.Contains(t.Id)).ToListAsync();
                            if (deliveryRequestEntities.Count != deliveryRequestIds.Count)
                            {
                                var archiveDrCollection = mdbContext.Database.GetCollection<DeliveryRequest>("Archive_DeliveryRequests");
                                var archiveDrs = await archiveDrCollection.Find(t => deliveryRequestIds.Contains(t.Id)).ToListAsync();
                                if (archiveDrs.Any())
                                {
                                    deliveryRequestEntities = deliveryRequestEntities.Union(archiveDrs).ToList();
                                }
                            }
                            var tripModel = existingTrip.ToViewModel(deliveryRequestEntities);
                            if (tripModel.DeliveryRequests.Select(t => t.JobId).Intersect(inputModel.ScheduleDetails.Select(t1 => t1.JobId)).Any())
                            {
                                response.Trips.Add(tripModel);
                            }
                        }
                    }
                }
                else
                {
                    int? maxRowIndex = existingDsb.Trips.Where(t => t.ShiftId == loadShift.ShiftId).Max(t => t.DriverRowIndex).GetValueOrDefault();
                    rowIndex = maxRowIndex.HasValue ? maxRowIndex.Value + 1 : 0;
                }
            }

            response.Trips.Add(new TripViewModel() { ShiftId = loadShift.ShiftId, ShiftStartTime = loadShift.ShiftStartTime.ToString(Resource.constFormat12HourTime), ShiftEndTime = loadShift.ShiftEndTime.ToString(Resource.constFormat12HourTime), ShiftIndex = shiftIndex, SlotPeriod = inputModel.SlotPeriod, DriverRowIndex = rowIndex, DriverColIndex = colIndex });
            response.StatusCode = (int)Status.Success;
            return response;
        }

        public async Task<DSBSaveModel> GetLoads(CreateScheduleModel scheduleInput)
        {
            var response = await GetScheduleBuilderModel(scheduleInput.CreateScheduleInput, scheduleInput.DeliveryRequests);
            if (response.StatusCode != (int)Status.Success)
            {
                return response;
            }
            await CreateLoads(scheduleInput.CreateScheduleInput, response, scheduleInput.DeliveryRequests);
            return response;
        }

        private async Task CreateLoads(ProcessDSBCreation inputModel, DSBSaveModel scheduleBuilder, List<DeliveryRequestViewModel> deliveryRequests)
        {
            deliveryRequests.ForEach(t => { t.CreatedBy = inputModel.UserId; t.CreatedOn = DateTimeOffset.Now; t.UpdatedBy = inputModel.UserId; t.UpdatedOn = DateTimeOffset.Now; t.IsActive = true; t.IsDeleted = false; t.DelReqSource = DRSource.API; });
            var deliveryRequestEntities = deliveryRequests.ToEntity();
            deliveryRequestEntities.ForEach(t => t.IsCommonBadge = false);
            await mdbContext.DeliveryRequests.InsertManyAsync(deliveryRequestEntities);
            deliveryRequests.Clear();
            deliveryRequestEntities.ForEach(t => deliveryRequests.Add(t.ToDeliveryRequestViewModel()));
            scheduleBuilder.UserId = inputModel.UserId;
            var newLoad = scheduleBuilder.Trips.FirstOrDefault(t => !t.DeliveryRequests.Any());
            scheduleBuilder.CompanyId = inputModel.CarrierCompanyId;
            scheduleBuilder.RegionId = inputModel.CarrierRegionId;

            foreach (var drs in deliveryRequests.GroupBy(t => t.JobId))
            {
                var load = scheduleBuilder.Trips.FirstOrDefault(t => t.DeliveryRequests.Any(t1 => t1.JobId == drs.Key));
                if (load != null)
                {
                    if (drs != null && drs.Any())
                    {
                        foreach (var dr in drs)
                        {
                            var existingDr = load.DeliveryRequests.FirstOrDefault(t => t.OrderId == dr.OrderId && t.PickupLocationType == dr.PickupLocationType
                                                                                                && ((t.PickupLocationType == PickupLocationType.BulkPlant && t.BulkPlant.Equals(dr.BulkPlant))
                                                                                                || (t.PickupLocationType == PickupLocationType.Terminal && t.Terminal.Id == dr.Terminal.Id)));
                            if (existingDr != null)
                            {
                                existingDr.ScheduleStatus = (int)DeliveryScheduleStatus.Modified;
                                existingDr.RequiredQuantity += dr.RequiredQuantity;
                                scheduleBuilder.ApiResponseModel.Add(new ScheduleApiResponse() { RequestCarrierOrderId = dr.CarrierOrderId, ResponseCarrierOrderId = existingDr.CarrierOrderId });
                                dr.IsActive = false;
                                dr.IsDeleted = true;
                            }
                            else
                            {
                                dr.Status = DeliveryReqStatus.ScheduleCreated;
                                dr.ScheduleStatus = (int)DeliveryScheduleStatus.New;
                                scheduleBuilder.ApiResponseModel.Add(new ScheduleApiResponse() { RequestCarrierOrderId = dr.CarrierOrderId, ResponseCarrierOrderId = existingDr.CarrierOrderId });
                                load.DeliveryRequests.Add(dr);
                            }
                        }
                        load.UpdatedByName = inputModel.UserName;
                        load.UpdatedBy = inputModel.UserId;
                        load.UpdatedDate = DateTimeOffset.Now;
                        load.DeliveryGroupStatus = DeliveryGroupStatus.Published;
                        load.TripStatus = TripStatus.Modified;
                    }
                }
                else
                {
                    drs.ToList().ForEach(t => { t.ScheduleStatus = (int)DeliveryScheduleStatus.New; t.Status = DeliveryReqStatus.ScheduleCreated; scheduleBuilder.ApiResponseModel.Add(new ScheduleApiResponse() { RequestCarrierOrderId = t.CarrierOrderId, ResponseCarrierOrderId = t.CarrierOrderId }); });
                    newLoad.DeliveryRequests.AddRange(drs);
                    newLoad.DeliveryGroupStatus = DeliveryGroupStatus.Published;
                    newLoad.UpdatedByName = inputModel.UserName;
                    newLoad.TripStatus = TripStatus.Added;
                    newLoad.StartTime = inputModel.StartTime;
                    newLoad.EndTime = inputModel.EndTime;
                    newLoad.Drivers.Add(inputModel.Drivers.ToDriverDetailModel());
                    newLoad.StartDate = inputModel.Date;
                    newLoad.Carrier = inputModel.CarrierCompanyName;
                    newLoad.UpdatedByName = inputModel.UserName;
                    newLoad.CreatedBy = newLoad.UpdatedBy = inputModel.UserId;
                    newLoad.CreatedDate = newLoad.UpdatedDate = DateTimeOffset.Now;
                }
            }
            await DeleteUnwantedRequests(inputModel.UserId, deliveryRequests.Where(t => !t.IsActive && t.IsDeleted).Select(t => ObjectId.Parse(t.Id)).ToList());
            scheduleBuilder.Trips.RemoveAll(t => !t.DeliveryRequests.Any());
            scheduleBuilder.StatusCode = (int)Status.Success;
        }

        public async Task<DSBSaveModel> CreateSchedules(DSBSaveModel scheduleBuilder)
        {
            using (var session = await mdbContext.Client.StartSessionAsync())
            {
                try
                {
                    session.StartTransaction();
                    ScheduleBuilder dsbEntity = null;
                    if (string.IsNullOrWhiteSpace(scheduleBuilder.Id))
                    {
                        dsbEntity = await CreateScheduleBuilder(scheduleBuilder);
                    }
                    else
                    {
                        dsbEntity = await UpdateScheduleBuilder(scheduleBuilder);
                    }
                    await session.CommitTransactionAsync();
                    UpdateDsbModel(scheduleBuilder, dsbEntity);
                    scheduleBuilder.StatusCode = (int)Status.Success;
                    scheduleBuilder.StatusMessage = GetStatusMessage(scheduleBuilder);

                }
                catch (Exception)
                {
                    await session.AbortTransactionAsync();
                    scheduleBuilder.StatusCode = (int)Status.Failed;
                    scheduleBuilder.StatusMessage = Resource.errMsgProcessRequestFailed;
                    throw;
                }
            }
            return scheduleBuilder;
        }

        public async Task<UnassignDriverViewModel> UnAssignDriverFromShift(UnassignDriverViewModel removeDriver)
        {
            var response = new UnassignDriverViewModel();
            using (var session = await mdbContext.Client.StartSessionAsync())
            {
                session.StartTransaction();
                try
                {
                    string trailerId = string.Empty;
                    string driverName = string.Empty;
                    string trailerName = string.Empty;
                    bool IsTrailerAvaliable = false;
                    int driverId = 0;
                    var loads = new List<TripModel>();
                    var sbobjectId = ObjectId.Parse(removeDriver.sbId);
                    var existingDsb = mdbContext.ScheduleBuilders.Find(t => t.Id == sbobjectId && t.Trips.Any(t1 => t1.DriverRowIndex == removeDriver.DriverRowIdx)).SingleOrDefault();
                    //check validation for trips time stamp
                    if (existingDsb != null)
                    {
                        foreach (var trip in removeDriver.Trips)
                        {
                            ObjectId tripObjectId = ObjectId.Empty;
                            ObjectId.TryParse(trip.TripId, out tripObjectId);
                            var existingTimeStampInDb = existingDsb.Trips.Where(t => t.TripId == tripObjectId).Select(t => t.TimeStamp).FirstOrDefault();
                            if (existingTimeStampInDb != 0 && existingTimeStampInDb != trip.TimeStamp)
                            {
                                response.StatusMessage = Resource.valMessageSbNotRefreshed;
                                response.StatusCode = (int)Status.Failed;
                                session.AbortTransaction();
                                return response;
                            }
                        }
                        var drExistsStatus = await CheckPublishedDrs(removeDriver);
                        if (drExistsStatus.StatusCode == (int)Status.Failed)
                        {
                            session.AbortTransaction();
                            return drExistsStatus;
                        }
                    }
                    if (removeDriver.Trailers != null && removeDriver.Trailers.Any())
                    {
                        trailerId = removeDriver.Trailers.FirstOrDefault().Id;
                    }
                    if (removeDriver.Drivers != null && removeDriver.Drivers.Any())
                    {
                        driverId = removeDriver.Drivers.FirstOrDefault().Id;
                    }
                    var filter = Builders<ScheduleBuilder>.Filter.Eq("Id", sbobjectId);

                    var arrayFilters = new List<ArrayFilterDefinition> { new JsonArrayFilterDefinition<ScheduleBuilder>("{'i.DriverRowIndex': " + removeDriver.DriverRowIdx + ",'i.ShiftIndex': " + removeDriver.shiftIdx + "}") };
                    var updateOptions = new UpdateOptions { ArrayFilters = arrayFilters };
                    if (removeDriver.Drivers != null && removeDriver.Drivers.Any())//Pull driver from trips
                    {
                        foreach (var item in removeDriver.Drivers)
                        {
                            item.Code = string.IsNullOrEmpty(item.Code) ? string.Empty : item.Code;
                        }
                        driverName = removeDriver.Drivers.FirstOrDefault().Name;

                        var update = Builders<ScheduleBuilder>.Update.PullFilter("Trips.$[i].TfxDrivers",
                                                   Builders<DropdownDisplayExtendedItem>.Filter.Eq("Id", removeDriver.Drivers[0].Id));
                        await mdbContext.ScheduleBuilders.UpdateOneAsync(filter, update, updateOptions);
                    }
                    if (removeDriver.Trailers != null && removeDriver.Trailers.Any())// Pull trailer from trips
                    {
                        trailerName = removeDriver.Trailers.FirstOrDefault().TrailerId;
                        IsTrailerAvaliable = true;
                        var update = Builders<ScheduleBuilder>.Update.PullFilter("Trips.$[i].Trailers",
                                                   Builders<TrailerModel>.Filter.Eq("Id", removeDriver.Trailers[0].Id));
                        await mdbContext.ScheduleBuilders.UpdateOneAsync(filter, update, updateOptions);
                    }
                    //Update trips 
                    if (existingDsb != null)
                    {
                        var dsb = mdbContext.ScheduleBuilders.Find(t => t.Id == sbobjectId).SingleOrDefault();
                        loads = SetUpdatedTripsValues(dsb.Trips, removeDriver);
                        if (loads != null && loads.Any())
                        {
                            var updateFields = Builders<ScheduleBuilder>.Update
                             .Set(t => t.Trips, loads);
                            await mdbContext.ScheduleBuilders.UpdateOneAsync(filter, updateFields);
                            //Update timestamp for each trip
                            var updatedTimeStamp = loads.Where(t => t.DriverRowIndex == removeDriver.DriverRowIdx && t.ShiftIndex == removeDriver.shiftIdx).Select(t => t.TimeStamp).FirstOrDefault();
                            removeDriver.Trips.ForEach(t => t.TimeStamp = updatedTimeStamp);
                            response.Trips = removeDriver.Trips;
                        }
                    }
                    //remove compartmentinfo in dsb.
                    await RemoveDeliveryReqCompartmentInfo(removeDriver, trailerId);
                    await session.CommitTransactionAsync();
                    response.StatusCode = (int)Status.Success;
                    response.StatusMessage = IsTrailerAvaliable ? string.Format(Resource.msgDriverTrailerUnAssignmentSuccessful, trailerName) : string.Format(Resource.msgDriverUnAssignementSuccessful, driverName);
                }

                catch (Exception ex)
                {
                    await session.AbortTransactionAsync();
                    int rowNumber = removeDriver.DriverRowIdx++;
                    response.StatusCode = (int)Status.Failed;
                    response.StatusMessage = "Failed to Unassign driver from Row" + rowNumber;
                    Exchange.Logger.LogManager.Logger.WriteException("ScheduleBuilderRepository", "UnAssignDriverFromShift", ex.Message, ex);
                }
            }
            return response;
        }

        private async Task<UnassignDriverViewModel> CheckPublishedDrs(UnassignDriverViewModel removeDriver)
        {
            var response = new UnassignDriverViewModel() { StatusCode = (int)Status.Success, StatusMessage = Status.Success.ToString() };
            var drIds = removeDriver.Trips.SelectMany(t1 => t1.DeliveryRequests.Select(t2 => t2.Id));
            var drIdsArray = drIds.Where(t => !string.IsNullOrWhiteSpace(t)).Select(t => ObjectId.Parse(t)).ToList();
            if (drIdsArray.Any())
            {
                var drFilter = Builders<DeliveryRequest>.Filter.And(
                    Builders<DeliveryRequest>.Filter.In(t => t.Id, drIdsArray),
                    Builders<DeliveryRequest>.Filter.Where(t => t.Status == DeliveryReqStatus.ScheduleCreated)
                );
                var isPublishedDrExists = await mdbContext.DeliveryRequests.Find(drFilter).AnyAsync();
                if (isPublishedDrExists)
                {
                    response.StatusMessage = Resource.errMessageLoadAleadyPublished;
                    response.StatusCode = (int)Status.Failed;
                }
            }
            return response;
        }

        private async Task RemoveDeliveryReqCompartmentInfo(UnassignDriverViewModel removeDriver, string trailerId)
        {
            if (removeDriver != null)
            {
                foreach (var item in removeDriver.Trips)
                {
                    var deliveryReqId = item.DeliveryRequests.Select(top => ObjectId.Parse(top.Id)).ToList();
                    var deliveryReqInfo = await mdbContext.DeliveryRequests.Find(t => deliveryReqId.Contains(t.Id)).Project(x => new { x.Id, x.Compartments }).ToListAsync();
                    foreach (var delitem in deliveryReqInfo)
                    {
                        var delfilter = Builders<DeliveryRequest>.Filter.Where(t => t.Id == delitem.Id);
                        var findIndex = delitem.Compartments.FindIndex(top => top.TrailerId == ObjectId.Parse(trailerId));
                        if (findIndex != -1)
                        {
                            delitem.Compartments.RemoveAt(findIndex);
                        }
                        var update = Builders<DeliveryRequest>.Update.Set(t => t.Compartments, delitem.Compartments);
                        await mdbContext.DeliveryRequests.UpdateOneAsync(delfilter, update);
                    }
                }
            }
        }

        private List<TripModel> SetUpdatedTripsValues(List<TripModel> trips, UnassignDriverViewModel removeDriver = null)
        {
            try
            {
                if (trips != null && trips.Any())
                {
                    if (removeDriver != null)
                    {
                        var updatedby = removeDriver.updatedBy;
                        var updatedDate = DateTimeOffset.Now;
                        var timeStamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                        var updatedByName = removeDriver.updatedByName;
                        var IsIncludeAllRegionDriver = false;
                        foreach (var trip in trips)
                        {
                            if (trip.ShiftIndex == removeDriver.shiftIdx && trip.DriverRowIndex == removeDriver.DriverRowIdx)
                            {
                                trip.UpdatedBy = updatedby;
                                trip.UpdatedDate = updatedDate;
                                trip.TimeStamp = timeStamp;
                                trip.UpdatedByName = updatedByName;
                                trip.IsIncludeAllRegionDriver = IsIncludeAllRegionDriver;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exchange.Logger.LogManager.Logger.WriteException("ScheduleBuilderRepository", "UnAssignDriverFromShift", ex.Message, ex);
            }
            return trips;
        }

        public async Task<List<CreateRecurringDRViewModel>> GetRecurringScheduleDetails(string dayOfWeek, int currentDay, string date)
        {
            var response = new List<CreateRecurringDRViewModel>();
            try
            {
                var recurringScheduleInfo = await mdbContext.RecurringSchedules.Find(top => (top.WeekDayId.Contains(dayOfWeek) || top.MonthDayId == currentDay)
                                                                        && top.IsActive && !top.IsDeleted && top.ShiftInfo != null).ToListAsync();
                if (recurringScheduleInfo.Any())
                {
                    foreach (var item in recurringScheduleInfo)
                    {
                        if (item.ScheduleType == (int)ScheduleTypes.BiWeekly)
                        {
                            var dt = Convert.ToDateTime(date); // your datetime
                            var weekNum = DateHelper.GetWeekNumberOfMonth(dt);
                            int biweeklyKey = weekNum % 2;
                            if (biweeklyKey == 0)//Bi-Weekly
                            {
                                response.Add(item.ToEntity());
                            }
                        }
                        else
                        {
                            response.Add(item.ToEntity());
                        }
                    }
                    response.SelectMany(top => top.DeliveryRequests).ToList().ForEach(x => x.CreatedOn = Convert.ToDateTime(date));
                    RemoveDuplicateSchedule(date, response);
                }
            }
            catch (Exception ex)
            {
                Exchange.Logger.LogManager.Logger.WriteException("ScheduleBuilderRepository", "GetRecurringScheduleDetails", ex.Message, ex);
                throw;
            }
            return response;
        }

        private void RemoveDuplicateSchedule(string date, List<CreateRecurringDRViewModel> response)
        {
            List<ObjectId> objectIds = new List<ObjectId>();
            response.Select(top => top.Id).ToList().ForEach(x => objectIds.Add(ObjectId.Parse(x)));

            var deliveryRequests = mdbContext.DeliveryRequests.Find(x => x.IsActive && !x.IsDeleted && x.IsRecurringSchedule && objectIds.Contains(x.RecurringScheduleId)).ToList();
            deliveryRequests = deliveryRequests.Distinct().Where(top => top.CreatedOn.ToString("MM/dd/yyyy") == date).ToList();
            foreach (var dritem in deliveryRequests)
            {
                int index = response.FindIndex(top => top.Id == dritem.RecurringScheduleId.ToString() && top.DeliveryRequests.Any(x => x.CreatedOn.ToString("MM/dd/yyyy") == date));
                if (index != -1)
                {
                    if (response.Count > 0)
                    {
                        response.RemoveAt(index);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        public async Task<ScheduleBuilderViewModel> GetScheduleBuilderDetails(int companyId, int userId, string regionId, string date, int sbView, int sbDsbView, string scheduleBuilderId, List<RecurringShiftDetails> recurringShiftDetails, string scheduleBuilderViewId, bool IsBackgroundJobScheduleCreation, bool IsDsbDriverSchedule)
        {
            ScheduleBuilderViewModel response = new ScheduleBuilderViewModel();

            DateTime dateFilter = DateTimeOffset.Now.Date;
            if (!string.IsNullOrWhiteSpace(date))
            {
                dateFilter = Convert.ToDateTime(date).Date;
            }
            ScheduleBuilder scheduleBuilder = await mdbContext.ScheduleBuilders
                                    .Find(t => t.IsActive && !t.IsDeleted && t.TfxCompanyId == companyId
                                        && (regionId == null || regionId == "" || t.RegionId == regionId)
                                        && t.DateFilter == dateFilter).SortByDescending(t => t.Id).FirstOrDefaultAsync();
            //set the dsbView for Recurring Schedule future.
            sbDsbView = await GetScheduleBuilderView(scheduleBuilderViewId);
            response = await GetScheduleBuilderInfo(companyId, userId, regionId, date, sbView, sbDsbView, recurringShiftDetails, scheduleBuilderViewId, response, dateFilter, scheduleBuilder, IsBackgroundJobScheduleCreation, IsDsbDriverSchedule);
            return response;
        }


        public async Task<List<TrailerCompartmentDetail>> GetTrailerCompartmentDetails(List<string> Id)
        {
            var response = new List<TrailerCompartmentDetail>();
            try
            {
                if (Id != null && Id.Count > 0)
                {
                    var objectId = Id.Select(top => ObjectId.Parse(top));
                    var trailerFuelRetains = (from trailerretain in mdbContext.TrailerFuelRetains.AsQueryable()
                                              join delreq in mdbContext.DeliveryRequests.AsQueryable()
                                              on trailerretain.DeliveryRequestId equals delreq.Id
                                              where objectId.Contains(trailerretain.TrailerId)
                                              && trailerretain.IsActive && !trailerretain.IsDeleted
                                              select new
                                              {
                                                  trailerretain.TrailerId,
                                                  trailerretain.CompartmentId,
                                                  trailerretain.ProductId,
                                                  trailerretain.OrderId,
                                                  trailerretain.Quantity,
                                                  trailerretain.ProductType,
                                                  delreq.TfxTerminal,
                                                  delreq.TfxBulkPlant,
                                                  DeliveryRequestId = delreq.Id,
                                                  delreq.TfxScheduleStatus
                                              }).ToList();

                    var trailerCompartments = await mdbContext.TruckDetails.Find(t => !t.IsDeleted && objectId.Contains(t.Id)).Project(t => new { t.Id, t.Compartments, t.Name }).ToListAsync();
                    foreach (var trailer in trailerCompartments)
                    {
                        var trailerFuelRetain = trailerFuelRetains.FirstOrDefault(x => x.TrailerId == trailer.Id);
                        if (trailerFuelRetain != null)
                        {
                            var thisTrailerRetainInfo = trailerFuelRetains.Where(delreq => delreq.TrailerId == trailer.Id).ToList();
                            var compartments = new List<CompartmentViewModel>();
                            if (trailer.Compartments.Any())
                            {
                                for (int i = 0; i < trailer.Compartments.Count; i++)
                                {
                                    var compartmentInfo = trailer.Compartments[i];
                                    var compartment = compartmentInfo.ToEntity();
                                    var retain = thisTrailerRetainInfo.Where(top => top.TrailerId == trailer.Id && top.CompartmentId == compartmentInfo.CompartmentId).FirstOrDefault();
                                    if (retain != null)
                                    {
                                        RetainInfo retainInfo = new RetainInfo();
                                        retainInfo.OrderId = retain.OrderId;
                                        retainInfo.ProductId = retain.ProductId;
                                        retainInfo.DeliveryReqId = retain.DeliveryRequestId.ToString();
                                        retainInfo.Quantity = retain.Quantity;
                                        if (retain.TfxTerminal != null && retain.TfxTerminal.Id > 0)
                                        {
                                            retainInfo.TfxTerminal = new FreightModels.DropdownDisplayItem()
                                            {
                                                Id = retain.TfxTerminal.Id,
                                                Name = retain.TfxTerminal.Name
                                            };
                                            retainInfo.PickupLocationType = PickupLocationType.Terminal;
                                        }
                                        else if (retain.TfxBulkPlant != null && !string.IsNullOrWhiteSpace(retain.TfxBulkPlant.SiteName))
                                        {
                                            retainInfo.TfxBulkPlant = retain.TfxBulkPlant;
                                            retainInfo.PickupLocationType = PickupLocationType.BulkPlant;
                                        }
                                        compartment.RetainInfo = retainInfo;
                                    }
                                    compartments.Add(compartment);
                                }
                                bool isRetain = false;
                                if (compartments.Any(t => t.RetainInfo != null))
                                {
                                    isRetain = true;

                                }
                                else if (thisTrailerRetainInfo.Any())
                                {
                                    isRetain = true;
                                }
                                response.Add(new TrailerCompartmentDetail { TrailerId = trailer.Id.ToString(), Compartments = compartments, IsFuelRetain = isRetain });
                            }
                            else
                            {
                                var retain = thisTrailerRetainInfo.Where(top => top.TrailerId == trailer.Id).FirstOrDefault();
                                if (retain != null)
                                {
                                    var compartment = new CompartmentViewModel();
                                    compartment.CompartmentId = trailer.Name;
                                    compartment.Capacity = thisTrailerRetainInfo.Where(top => top.TrailerId == trailer.Id).Sum(x => x.Quantity);
                                    compartment.FuelType = retain.ProductId;
                                    compartment.IsCompartmentAvailable = false;
                                    RetainInfo retainInfo = new RetainInfo();
                                    retainInfo.OrderId = retain.OrderId;
                                    retainInfo.ProductId = retain.ProductId;
                                    retainInfo.DeliveryReqId = retain.DeliveryRequestId.ToString();
                                    retainInfo.Quantity = thisTrailerRetainInfo.Where(top => top.TrailerId == trailer.Id).Sum(x => x.Quantity);
                                    if (retain.TfxTerminal != null && retain.TfxTerminal.Id > 0)
                                    {
                                        retainInfo.TfxTerminal = new FreightModels.DropdownDisplayItem()
                                        {
                                            Id = retain.TfxTerminal.Id,
                                            Name = retain.TfxTerminal.Name
                                        };
                                        retainInfo.PickupLocationType = PickupLocationType.Terminal;
                                    }
                                    else if (retain.TfxBulkPlant != null && !string.IsNullOrWhiteSpace(retain.TfxBulkPlant.SiteName))
                                    {
                                        retainInfo.TfxBulkPlant = retain.TfxBulkPlant;
                                        retainInfo.PickupLocationType = PickupLocationType.BulkPlant;
                                    }
                                    compartment.RetainInfo = retainInfo;
                                    compartments.Add(compartment);
                                }
                                bool isRetain = false;
                                if (compartments.Any(t => t.RetainInfo != null))
                                {
                                    isRetain = true;

                                }
                                else if (thisTrailerRetainInfo.Any())
                                {
                                    isRetain = true;
                                }
                                response.Add(new TrailerCompartmentDetail { TrailerId = trailer.Id.ToString(), Compartments = compartments, IsFuelRetain = isRetain, IsCompartmentAvailable = false });
                            }


                        }
                        else
                        {
                            response.Add(new TrailerCompartmentDetail { TrailerId = trailer.Id.ToString(), Compartments = trailer.Compartments.ToEntity(), IsFuelRetain = false });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exchange.Logger.LogManager.Logger.WriteException("ScheduleBuilderRepository", "GetTrailerCompartmentDetails", ex.Message, ex);
                throw;
            }
            return response;
        }

        public async Task<List<TrailerFuelRetainViewModel>> GetTrailerFuelRetainDetails(List<string> TrailerIds)
        {
            string defaultCompartmentId = "Compartment";
            var response = new List<TrailerFuelRetainViewModel>();
            try
            {
                var objectIds = TrailerIds.Select(top => ObjectId.Parse(top)).ToList();
                var trailerInfo = await mdbContext.TruckDetails.Find(x => objectIds.Contains(x.Id) && !x.IsDeleted).Project(x => new { x.Id, x.TruckId, x.Compartments }).ToListAsync();
                var trailerFuelRetainDetails = await mdbContext.TrailerFuelRetains.Find(x => objectIds.Contains(x.TrailerId) && !x.IsDeleted && x.IsActive).ToListAsync();
                foreach (var tFuelRetainDetails in trailerFuelRetainDetails)
                {
                    var trailerId = string.Empty;
                    decimal CompartmentCapacity = 0;
                    if (trailerInfo.Count() > 0 && trailerInfo.FirstOrDefault(top => top.Id == tFuelRetainDetails.TrailerId) != null)
                    {
                        var trailer = trailerInfo.FirstOrDefault(top => top.Id == tFuelRetainDetails.TrailerId);
                        if (trailer != null)
                        {
                            trailerId = trailer.TruckId;
                            if (trailer.Compartments.Any())
                                CompartmentCapacity = trailer.Compartments.Where(top => top.CompartmentId == tFuelRetainDetails.CompartmentId).Select(t => t.Capacity).FirstOrDefault();
                        }
                    }
                    else
                    {
                        trailerId = tFuelRetainDetails.TrailerId.ToString();
                        CompartmentCapacity = 0;
                    }
                    response.Add(new TrailerFuelRetainViewModel { TrailerId = trailerId, CompartmentId = tFuelRetainDetails.CompartmentId == defaultCompartmentId ? string.Empty : tFuelRetainDetails.CompartmentId, Quantity = tFuelRetainDetails.Quantity, ProductType = tFuelRetainDetails.ProductType, UOM = tFuelRetainDetails.UOM, Id = tFuelRetainDetails.Id.ToString(), CompartmentCapacity = CompartmentCapacity });
                }
            }
            catch (Exception ex)
            {
                Exchange.Logger.LogManager.Logger.WriteException("ScheduleBuilderRepository", "GetTrailerFuelRetainDetails", ex.Message, ex);
                throw;
            }
            return response;
        }
        private void GetRecurringScheduleInformation(List<DeliveryRequestViewModel> response)
        {
            var recurringScheduleInfo = response.Where(top => top.isRecurringSchedule).ToList();
            var recurringfilterInfo = response.Where(top => top.isRecurringSchedule).Select(top => ObjectId.Parse(top.RecurringScheduleId)).ToList();
            var filterDef = new FilterDefinitionBuilder<RecurringSchedules>();
            var filter = filterDef.In(x => x.Id, recurringfilterInfo);
            var collectionRecurringInfo = mdbContext.RecurringSchedules.Find(filter).ToList();
            foreach (var item in recurringScheduleInfo)
            {
                var scheduleInfo = collectionRecurringInfo.Where(top => top.Id == ObjectId.Parse(item.RecurringScheduleId)).FirstOrDefault();
                if (scheduleInfo != null)
                {
                    item.RecurringScheduleInfo += ((ScheduleTypes)scheduleInfo.ScheduleType).ToString();
                    if (scheduleInfo.ScheduleType == (int)ScheduleTypes.Weekly || scheduleInfo.ScheduleType == (int)ScheduleTypes.BiWeekly)
                    {
                        string weekname = string.Empty;
                        weekname = GetWeekInformation(scheduleInfo, weekname);
                        weekname = string.Join(",", weekname.Split(',').Select(s => s.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray());
                        item.RecurringScheduleInfo += ": " + weekname;
                    }
                    else
                    {
                        item.RecurringScheduleInfo += ": " + scheduleInfo.Date;
                    }
                    item.ScheduleQuantityTypeText = GetSchduleQtyTypeText(scheduleInfo);
                }
            }
        }

        public async Task<DrFilterPreferencesModel> SaveDrFilterPreferences(DrFilterPreferencesModel model)
        {
            var response = new DrFilterPreferencesModel();
            try
            {
                //delete if exist
                var filter = Builders<DrFilterPreferences>.Filter.Where(t => t.UserId == model.UserId);
                await mdbContext.DrFilterPreferences.DeleteManyAsync(filter);
                //create new entry
                var entity = model.ToEntity();
                await mdbContext.DrFilterPreferences.InsertOneAsync(model.ToEntity());
            }
            catch (Exception ex)
            {
                Exchange.Logger.LogManager.Logger.WriteException("ScheduleBuilderRepository", "GetTrailerFuelRetainDetails", ex.Message, ex);
            }
            return response;
        }
        public async Task<DrFilterPreferencesModel> GetDrFilterPreferences(int userId)
        {
            var response = new DrFilterPreferencesModel();
            try
            {
                var filterEntity = await mdbContext.DrFilterPreferences.Find(t => t.UserId == userId).FirstOrDefaultAsync();
                if (filterEntity != null)
                {
                    response = filterEntity.ToViewModel();
                    response.StatusCode = (int)Status.Success;
                }
            }
            catch (Exception ex)
            {
                Exchange.Logger.LogManager.Logger.WriteException("ScheduleBuilderRepository", "GetTrailerFuelRetainDetails", ex.Message, ex);
            }
            return response;
        }
        #region ScheduleBuilderFunctions

        public async Task<List<DriverAdditionalDetailsViewModel>> GetAllDriverDetailsAsync(int companyId, List<string> trailerTypeId, string regionId, string otherRegion, DateTimeOffset selectedDate, string shiftId, bool IsDsbDriverSchedule = false)
        {
            var response = new List<DriverAdditionalDetailsViewModel>();
            var regionDrivers = new List<Exchange.Utilities.DropdownDisplayItem>();
            var tDriverIds = new List<int>();
            try
            {
                if (IsDsbDriverSchedule == false)
                {
                    response = await GetAllDriverDetailsAsync(companyId, trailerTypeId, regionId, selectedDate);
                }
                else
                {
                    ObjectId objShiftId = ObjectId.Empty;
                    ObjectId.TryParse(shiftId, out objShiftId);

                    if (!string.IsNullOrEmpty(otherRegion)) // if otherRegion is null/empty,return available driver across the all region.
                    {
                        var regionDriverList = mdbContext.Regions.Find(top => top.IsActive && top.TfxCompanyId == companyId && !top.IsDeleted && top.TfxDrivers != null && top.TfxDrivers.Count > 0)
                            .Project(t => new { t.TfxDrivers, t.Name }).ToList();

                        if (regionDriverList == null && regionDriverList.Count() == 0)
                        {
                            return response;
                        }

                        regionDriverList.ForEach(item =>
                        {
                            if (item != null && item.TfxDrivers != null)
                            {
                                regionDrivers.AddRange(item.TfxDrivers.Select(t => new Exchange.Utilities.DropdownDisplayItem()
                                { Id = t.Id, Name = t.Name + " (Region -" + item.Name + ")" }).ToList());
                            }
                        });

                    }
                    else
                    {
                        ObjectId objregionId = new ObjectId(regionId);
                        var regionDriverList = mdbContext.Regions.Find(top => top.Id == objregionId && top.IsActive && !top.IsDeleted).Project(t => t.TfxDrivers).FirstOrDefault();

                        if (regionDriverList == null)
                        {
                            return response;
                        }

                        regionDrivers = regionDriverList.Select(t => new Exchange.Utilities.DropdownDisplayItem() { Id = t.Id, Name = t.Name }).ToList();

                    }
                    var driverInfo = regionDrivers.Select(x => x.Id).ToList();
                    //get driver schedule-shift information from DriverScheduleShiftMapping mongo db collection.
                    var driverScheduleInfo = await mdbContext.DriverScheduleShiftMapping.Find(x => driverInfo.Contains(x.DriverId) && x.IsActive && !x.IsDeleted)
                        .Project(top => new { top.DriverId, top.DriverScheduleList }).ToListAsync();

                    foreach (var item in driverScheduleInfo)
                    {

                        List<string> repeatDayList = new List<string>();

                        var driverDayShift = item.DriverScheduleList.Where(x => x.RepeatDayList != null && x.IsActive
                                             && x.RepeatDayList.Select(top => top.Date.ToString("dd/MM/yyyy"))
                                             .Contains(selectedDate.Date.ToString("dd/MM/yyyy"))).FirstOrDefault();

                        //get driver schedule information with date collections.
                        repeatDayList = item.DriverScheduleList.Where(x => x.RepeatDayList != null && x.IsActive)
                                        .SelectMany(top => top.RepeatDayList.Select(x => x.Date.ToString("dd/MM/yyyy"))).ToList();

                        var driverDayIndex = repeatDayList.Contains(selectedDate.Date.ToString("dd/MM/yyyy"));

                        if (driverDayIndex && driverDayShift != null)
                        {
                            if (driverDayShift.ShiftId == objShiftId)
                            {
                                tDriverIds.Add(item.DriverId);
                            }
                            else if (!string.IsNullOrEmpty(otherRegion)) // since shift id is not unique so we should include other region driver based on shift start,end and name
                            {
                                var sen = mdbContext.Shifts.Find(s => s.IsActive && !s.IsDeleted && s.Id == driverDayShift.ShiftId || s.Id == objShiftId)
                                          .Project(two => new { two.StartTime, two.EndTime, two.Name }).ToList();
                                if ((sen.Select(x => x.StartTime).Distinct().ToList()).Count == 1
                                    && (sen.Select(x => x.EndTime).Distinct().ToList()).Count == 1
                                    && (sen.Select(x => x.Name).Distinct().ToList()).Count == 1)
                                {
                                    tDriverIds.Add(item.DriverId); // include other region driver.
                                }
                            }

                        }

                    }

                    if (tDriverIds.Any())
                    {
                        tDriverIds.RemoveAll(r => GetOtherRegionAllocatedDrivers(companyId, regionId, tDriverIds, objShiftId, selectedDate).Contains(r));
                    }

                    if (tDriverIds.Any())
                    {
                        response = mdbContext.Drivers.Find(t => tDriverIds.Contains(t.DriverId) && !t.IsDeleted)
                                                                    .Project(t => new DriverAdditionalDetailsViewModel
                                                                    {
                                                                        Id = t.DriverId,
                                                                        // used ternary opeartor , when ask was to get the driver from all region 
                                                                        // than name format will be {drivername - (Region-RegionName)} other wise {drivername}
                                                                        Name = !string.IsNullOrEmpty(otherRegion) ?
                                                                        regionDrivers.Where(p => p.Id == t.DriverId).Select(p => p.Name).FirstOrDefault() : t.DriverName,
                                                                        IsFilldCompatible = t.IsFilldAuthorized
                                                                    })
                                                                    .ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Exchange.Logger.LogManager.Logger.WriteException("ScheduleBuilderRepository", "GetAllDriverDetailsAsync-GridView", ex.Message, ex);
            }
            return response;
        }

        /// <summary>
        ///  GetOtherRegionAllocatedDrivers
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="regionId"></param>
        /// <param name="tDriverIds"></param>
        /// <param name="objShiftId"></param>
        /// <returns></returns>
        /// <remarks>Don't include driver in current region which is already allocated in other region.</remarks>
        private List<int> GetOtherRegionAllocatedDrivers(int companyId, string regionId, List<int> tDriverIds, ObjectId objShiftId, DateTimeOffset selectedDate)
        {
            List<int> removeDrivers = new List<int>();
            var sen = mdbContext.Shifts.Find(s => s.IsActive && !s.IsDeleted && s.Id == objShiftId)
                                                       .Project(two => new { two.StartTime, two.EndTime, two.Name }).FirstOrDefault();

            var otherRegionDrivers = mdbContext.ScheduleBuilders.Find(t => t.RegionId != regionId && t.TfxCompanyId == companyId &&
                                                                    t.Trips.Any(x => x.StartDate == selectedDate && x.ShiftStartTime == sen.StartTime && x.ShiftEndTime == sen.EndTime && x.TfxDrivers.Any(t1 => tDriverIds.Contains(t1.Id)))
                                                                    && t.IsActive && !t.IsDeleted).ToList();
            if (otherRegionDrivers != null && otherRegionDrivers.Count() > 0)
            {
                otherRegionDrivers.ForEach(item =>
                {
                    if (item.Trips != null)
                    {
                        item.Trips.ForEach(trip =>
                    {
                        if (trip.TfxDrivers != null && trip.ShiftStartTime == sen.StartTime && trip.ShiftEndTime == sen.EndTime && trip.StartDate == selectedDate)
                        {
                            var driLis = trip.TfxDrivers.Where(driver => tDriverIds.Contains(driver.Id));
                            if (driLis.Any())
                            {
                                removeDrivers.AddRange(driLis.Select(dId => dId.Id));
                            }
                        }

                    });
                    }
                });
            }
            return removeDrivers;
        }

        private async Task GetDsbLoadQueueInfo(int companyId, int userId, string regionId, ScheduleBuilderViewModel response, DateTime dateFilter)
        {
            if (dateFilter.Date >= DateTime.Now.Date)
            {
                ObjectId objRegionId = ObjectId.Empty;
                ObjectId.TryParse(regionId, out objRegionId);
                var dsbLoadQueueInfo = await mdbContext.DSBLoadQueues
                                                    .Find(t => t.TfxCompanyId == companyId
                                                        && t.Date == dateFilter && t.TfxUserId == userId && t.RegionId == objRegionId).ToListAsync();
                if (dsbLoadQueueInfo.Any())
                {
                    response.DsbLoadQueueModel = dsbLoadQueueInfo.ToEntity();
                }
            }
        }
        private async Task<DriverScheduleInformation> GetDriverScheduleInformation(string regionId, int companyId, int sbDsbView, DateTime dateFilter)
        {
            DriverScheduleInformation driverScheduleObj = new DriverScheduleInformation();
            List<DriverShiftInfo> driverShiftInfo = new List<DriverShiftInfo>();
            List<DropdownDisplayItem> driverInformation = new List<DropdownDisplayItem>();
            try
            {
                ObjectId objRegionId = ObjectId.Empty;
                ObjectId.TryParse(regionId, out objRegionId);
                //get region drivers details.
                var regionDrivers = await mdbContext.Regions.Find(t => t.Id == objRegionId && t.IsActive && !t.IsDeleted).Project(t => new { t.TfxDrivers }).FirstOrDefaultAsync();
                //get region shift details.
                var shifts = await mdbContext.Shifts.Find(t => t.RegionId == objRegionId && t.IsActive && !t.IsDeleted).ToListAsync();
                if (regionDrivers != null && shifts.Any() && regionDrivers.TfxDrivers.Any())
                {
                    //get drivers ids collection.
                    var driverInfo = regionDrivers.TfxDrivers.Select(x => x.Id).ToList();
                    //get driver schedule-shift information from DriverScheduleShiftMapping mongo db collection.
                    var driverScheduleInfo = await mdbContext.DriverScheduleShiftMapping.Find(x => driverInfo.Contains(x.DriverId) && x.IsActive && !x.IsDeleted).Project(top => new { top.DriverId, top.DriverScheduleList }).ToListAsync();
                    foreach (var item in driverScheduleInfo)
                    {
                        //get driver schedule information with date collections.
                        List<DateTime> repeatDayList = new List<DateTime>();
                        repeatDayList = item.DriverScheduleList.Where(top => top.RepeatDayList != null && top.IsActive).SelectMany(top => top.RepeatDayList.Select(x => x.Date)).ToList();
                        var driverDayIndex = repeatDayList.FindIndex(x => x == dateFilter.Date);
                        if (driverDayIndex >= 0)
                        {
                            //get driver first shift information.
                            var driverDayShift = item.DriverScheduleList.Where(top => top.RepeatDayList != null && top.IsActive && top.RepeatDayList.Select(x => x.Date).Contains(dateFilter.Date)).FirstOrDefault();
                            if (driverDayShift != null)
                            {
                                //verify that shift exists in region shift information or not.
                                var shiftInfo = shifts.FirstOrDefault(x => x.Id == driverDayShift.ShiftId);
                                if (shiftInfo != null)
                                {
                                    //if drivers shift collections exists then add that driver in that shift.
                                    var driverShiftExists = driverShiftInfo.FirstOrDefault(x => x.Id == shiftInfo.Id.ToString());
                                    if (driverShiftExists != null)
                                    {
                                        List<DropdownDisplayItem> driverExistingInfoObj = driverShiftExists.DriverInformation;
                                        //get the driver name from region driver collection.
                                        var regionDriverName = regionDrivers.TfxDrivers.FirstOrDefault(x => x.Id == item.DriverId);
                                        if (regionDriverName != null)
                                        {
                                            var driverExistingIndex = driverExistingInfoObj.FindIndex(x => x.Id == item.DriverId);
                                            if (driverExistingIndex == -1)
                                            {
                                                DropdownDisplayItem driverInfoObj = new DropdownDisplayItem();
                                                driverInfoObj.Id = item.DriverId;
                                                driverInfoObj.Name = regionDriverName.Name;
                                                driverExistingInfoObj.Add(driverInfoObj);
                                            }

                                        }
                                    }
                                    else
                                    {
                                        //if drivers shift collections not exists then create that driver shift collection.
                                        driverInformation = new List<DropdownDisplayItem>();
                                        //get the driver name from region driver collection.
                                        var regionDriverName = regionDrivers.TfxDrivers.FirstOrDefault(x => x.Id == item.DriverId);
                                        if (regionDriverName != null)
                                        {
                                            DropdownDisplayItem driverInfoObj = new DropdownDisplayItem();
                                            driverInfoObj.Id = item.DriverId;
                                            driverInfoObj.Name = regionDriverName.Name;
                                            driverInformation.Add(driverInfoObj);
                                            driverShiftInfo.Add(new DriverShiftInfo { Id = shiftInfo.Id.ToString(), StartTime = shiftInfo.StartTime, EndTime = shiftInfo.EndTime, Name = shiftInfo.Name, CompanyId = shiftInfo.CompanyId, RegionId = shiftInfo.RegionId.ToString(), DriverInformation = driverInformation });
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
                foreach (var item in driverShiftInfo)
                {
                    ObjectId objShiftId = ObjectId.Empty;
                    ObjectId.TryParse(item.Id, out objShiftId);
                    if (item.DriverInformation != null && item.DriverInformation.Any())
                    {
                        List<int> drivers = GetOtherRegionAllocatedDrivers(companyId, regionId, item.DriverInformation.Select(x => x.Id).ToList(), objShiftId, dateFilter.Date);
                        if (drivers.Any())
                        {
                            item.DriverInformation.RemoveAll(d => drivers.Contains(d.Id));
                        }
                    }
                }

                driverScheduleObj.DriverShiftInfo = driverShiftInfo;
                driverScheduleObj.StatusCode = (int)Status.Success;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderRepository", "GetDriverScheduleInformation", ex.Message, ex);
            }

            return driverScheduleObj;//return shift object that contain shift info with driver collections.
        }
        private async Task<ScheduleBuilderViewModel> GetScheduleBuilderGridViewModel(ScheduleBuilder scheduleBuilder, int companyId, int userId, string regionId, string date, int sbView, int sbDsbView, List<RecurringShiftDetails> recurringShiftDetails, string scheduleBuilderViewId, DriverScheduleInformation driverschduleInfo, bool IsBackgroundJobScheduleCreation = false)
        {
            ScheduleBuilderViewModel response = new ScheduleBuilderViewModel();
            if (scheduleBuilder != null)
            {
                List<ObjectId> deliveryRequestIds = scheduleBuilder.Trips.SelectMany(t2 => t2.DeliveryRequests).ToList();
                List<DeliveryRequest> deliveryRequests = await mdbContext.DeliveryRequests.Find(t => deliveryRequestIds.Contains(t.Id)).ToListAsync();
                if (deliveryRequests.Count != deliveryRequestIds.Count)
                {
                    var archiveDrCollection = mdbContext.Database.GetCollection<DeliveryRequest>("Archive_DeliveryRequests");
                    var archiveDrs = await archiveDrCollection.Find(t => deliveryRequestIds.Contains(t.Id)).ToListAsync();
                    if (archiveDrs.Any())
                    {
                        deliveryRequests = deliveryRequests.Union(archiveDrs).ToList();
                    }
                }
                deliveryRequests = deliveryRequests.OrderBy(x => x.DispatcherDragDropSequence).ToList();
                response = scheduleBuilder.ToViewModel(deliveryRequests);
                GetJobCompatibilityInfo(deliveryRequests.Select(t => t.TfxJobId).Distinct().ToList(), response.Trips.SelectMany(t => t.DeliveryRequests).ToList(), date);
                var driverIds = response.Trips.SelectMany(t => t.Drivers.Select(t1 => t1.Id)).ToList();
                var driverdetails = mdbContext.Drivers.Find(t => driverIds.Contains(t.DriverId) && !t.IsDeleted).Project(t => new { t.DriverId, t.DriverName, t.IsFilldAuthorized }).ToList();
                foreach (var trip in response.Trips)
                {
                    foreach (var drv in trip.Drivers.Where(t => t.Id > 0))
                    {
                        var drvDetail = driverdetails.FirstOrDefault(t => t.DriverId == drv.Id);
                        drv.IsFilldCompatible = drvDetail?.IsFilldAuthorized ?? false;
                    }
                }
                foreach (var tripitem in response.Trips)
                {
                    if (tripitem.DeliveryRequests.Any())
                    {
                        GetRecurringScheduleInformation(tripitem.DeliveryRequests);
                    }
                }
                await GridViewDefaultShiftsForRegion(response, sbView, recurringShiftDetails, driverschduleInfo, IsBackgroundJobScheduleCreation);
                if (deliveryRequests.Count() == 0)
                {
                    response.DSBFilter = sbDsbView;
                }
            }
            else
            {
                if (String.IsNullOrWhiteSpace(regionId))
                {
                    ObjectId regionObjId = await mdbContext.Regions.Find(t => t.IsActive && !t.IsDeleted && t.TfxDispatchers.Any(t1 => t1.Id == userId)).SortBy(t => t.Name).Project(t => t.Id).FirstOrDefaultAsync();
                    if (regionObjId != BsonNull.Value && regionObjId != BsonObjectId.Empty)
                    {
                        regionId = regionObjId.ToString();
                    }
                }
                if (!string.IsNullOrWhiteSpace(regionId))
                {
                    response = await GetDefaultGridViewScheduleBuilder(companyId, regionId.ToString(), date, sbView, recurringShiftDetails, driverschduleInfo);
                    response.DSBFilter = sbDsbView;
                }
                //verify the Dsb view for recurring schedule--future schedule creation.
                if (recurringShiftDetails != null && recurringShiftDetails.Count > 0)
                {
                    await SetRecurringScheduleDsbview(scheduleBuilderViewId, response);
                }
            }
            response.StatusCode = (int)Status.Success;
            return response;
        }
        private async Task<ScheduleBuilderViewModel> GetScheduleBuilderInfo(int companyId, int userId, string regionId, string date, int sbView, int sbDsbView, List<RecurringShiftDetails> recurringShiftDetails, string scheduleBuilderViewId, ScheduleBuilderViewModel response, DateTime dateFilter, ScheduleBuilder scheduleBuilder, bool IsBackgroundJobScheduleCreation = false, bool IsDsbDriverSchedule = false)
        {
            if (dateFilter.Date >= DateTime.Now.Date)
            {
                //fetch the regions drivers shifts and schedules informations.
                if (IsDsbDriverSchedule)
                {
                    DriverScheduleInformation driverschduleInfo = await GetDriverScheduleInformation(regionId, companyId, sbDsbView, dateFilter);
                    response = await GetScheduleBuilderGridViewModel(scheduleBuilder, companyId, userId, regionId, date, sbView, sbDsbView, recurringShiftDetails, scheduleBuilderViewId, driverschduleInfo, IsBackgroundJobScheduleCreation);
                }
                else
                {
                    response = await GetScheduleBuilderViewModel(scheduleBuilder, companyId, userId, regionId, date, sbView, sbDsbView, recurringShiftDetails, scheduleBuilderViewId);
                }
                await GetDsbLoadQueueInfo(companyId, userId, regionId, response, dateFilter);
            }
            else
            {
                response = await GetScheduleBuilderViewModel(scheduleBuilder, companyId, userId, regionId, date, sbView, sbDsbView, recurringShiftDetails, scheduleBuilderViewId);
            }

            return response;
        }

        private async Task SetRecurringScheduleDsbview(string scheduleBuilderViewId, ScheduleBuilderViewModel response)
        {
            ObjectId objScheduleBuilderViewId = ObjectId.Empty;
            ObjectId.TryParse(scheduleBuilderViewId, out objScheduleBuilderViewId);
            var scheduleBuilderDsbView = await mdbContext.ScheduleBuilders
                               .Find(t => t.IsActive && !t.IsDeleted && t.Id == objScheduleBuilderViewId).FirstOrDefaultAsync();
            if (scheduleBuilderDsbView != null)
            {
                response.DSBFilter = scheduleBuilderDsbView.DSBFilter;
            }
        }
        private async Task<ScheduleBuilderViewModel> GetScheduleBuilderInfo(int companyId, int userId, string regionId, string date, int sbView, int sbDsbView, ScheduleBuilderViewModel response, DateTime dateFilter, ScheduleBuilder scheduleBuilder, bool IsDsbDriverSchedule = false)
        {
            if (dateFilter.Date >= DateTime.Now.Date)
            {
                //fetch the regions drivers shifts and schedules informations.
                if (IsDsbDriverSchedule)
                {
                    DriverScheduleInformation driverschduleInfo = await GetDriverScheduleInformation(regionId, companyId, sbDsbView, dateFilter);
                    response = await GetScheduleBuilderGridViewModel(scheduleBuilder, companyId, userId, regionId, date, sbView, sbDsbView, null, string.Empty, driverschduleInfo);
                }
                else
                {
                    response = await GetScheduleBuilderViewModel(scheduleBuilder, companyId, userId, regionId, date, sbView, sbDsbView, null, string.Empty);
                }
                await GetDsbLoadQueueInfo(companyId, userId, regionId, response, dateFilter);
            }
            else
            {
                response = await GetScheduleBuilderViewModel(scheduleBuilder, companyId, userId, regionId, date, sbView, sbDsbView, null, string.Empty);
            }

            return response;
        }


        private async Task<ScheduleBuilderViewModel> GetScheduleBuilderViewModel(ScheduleBuilder scheduleBuilder, int companyId, int userId, string regionId, string date, int sbView, int sbDsbView, List<RecurringShiftDetails> recurringShiftDetails, string scheduleBuilderViewId)
        {
            ScheduleBuilderViewModel response = new ScheduleBuilderViewModel();
            if (scheduleBuilder != null)
            {
                List<ObjectId> deliveryRequestIds = scheduleBuilder.Trips.SelectMany(t2 => t2.DeliveryRequests).ToList();
                List<DeliveryRequest> deliveryRequests = await mdbContext.DeliveryRequests.Find(t => deliveryRequestIds.Contains(t.Id) && !t.IsDeleted).ToListAsync();
                if (deliveryRequests.Count != deliveryRequestIds.Count)
                {
                    var archiveDrCollection = mdbContext.Database.GetCollection<DeliveryRequest>("Archive_DeliveryRequests");
                    var archiveDrs = await archiveDrCollection.Find(t => deliveryRequestIds.Contains(t.Id) && !t.IsDeleted).ToListAsync();
                    if (archiveDrs.Any())
                    {
                        deliveryRequests = deliveryRequests.Union(archiveDrs).ToList();
                    }
                }
                deliveryRequests = deliveryRequests.OrderBy(x => x.DispatcherDragDropSequence).ToList();
                response = scheduleBuilder.ToViewModel(deliveryRequests);
                GetJobCompatibilityInfo(deliveryRequests.Select(t => t.TfxJobId).Distinct().ToList(), response.Trips.SelectMany(t => t.DeliveryRequests).ToList(), date);
                var driverIds = response.Trips.SelectMany(t => t.Drivers.Select(t1 => t1.Id)).ToList();
                var driverdetails = mdbContext.Drivers.Find(t => driverIds.Contains(t.DriverId) && !t.IsDeleted).Project(t => new { t.DriverId, t.DriverName, t.IsFilldAuthorized }).ToList();
                foreach (var trip in response.Trips)
                {
                    foreach (var drv in trip.Drivers.Where(t => t.Id > 0))
                    {
                        var drvDetail = driverdetails.FirstOrDefault(t => t.DriverId == drv.Id);
                        drv.IsFilldCompatible = drvDetail?.IsFilldAuthorized ?? false;
                    }
                }
                foreach (var tripitem in response.Trips)
                {
                    if (tripitem.DeliveryRequests.Any())
                    {
                        GetRecurringScheduleInformation(tripitem.DeliveryRequests);
                    }
                }
                await GetDefaultShiftsForRegion(response, sbView, recurringShiftDetails);
                if (deliveryRequests.Count() == 0)
                {
                    response.DSBFilter = sbDsbView;
                }
            }
            else
            {
                if (String.IsNullOrWhiteSpace(regionId))
                {
                    ObjectId regionObjId = await mdbContext.Regions.Find(t => t.IsActive && !t.IsDeleted && t.TfxDispatchers.Any(t1 => t1.Id == userId)).SortBy(t => t.Name).Project(t => t.Id).FirstOrDefaultAsync();
                    if (regionObjId != BsonNull.Value && regionObjId != BsonObjectId.Empty)
                    {
                        regionId = regionObjId.ToString();
                    }
                }
                if (!string.IsNullOrWhiteSpace(regionId))
                {
                    response = await GetDefualtScheduleBuilder(companyId, regionId.ToString(), date, sbView, recurringShiftDetails);
                    response.DSBFilter = sbDsbView;
                }
                //verify the Dsb view for recurring schedule--future schedule creation.
                if (recurringShiftDetails != null && recurringShiftDetails.Count > 0)
                {
                    await SetRecurringScheduleDsbview(scheduleBuilderViewId, response);
                }
            }
            response.StatusCode = (int)Status.Success;
            return response;
        }

        #endregion
        #region DriverReformWorkflow
        private async Task<ScheduleBuilderViewModel> GetDefaultGridViewScheduleBuilder(int companyId, string regionId, string date, int sbView, List<RecurringShiftDetails> recurringShiftDetails, DriverScheduleInformation driverschduleInfo)
        {
            ScheduleBuilderViewModel model = new ScheduleBuilderViewModel();
            model.RegionId = regionId;
            model.ObjectFilter = 1;
            model.RegionFilter = 1;
            //model.DateFilter = date.GetDateFilter();
            model.Date = date;
            model.CompanyId = companyId;
            model.TimeStamp = 0;
            model.Date = date;
            model.WindowMode = 1;
            model.Status = (int)DSBMethod.None;
            await GridViewDefaultShiftsForRegion(model, sbView, recurringShiftDetails, driverschduleInfo);

            return model;
        }
        private async Task GridViewDefaultShiftsForRegion(ScheduleBuilderViewModel model, int sbView, List<RecurringShiftDetails> recurringShiftDetails, DriverScheduleInformation driverschduleInfo, bool IsBackgroundJobScheduleCreation = false)
        {
            var regionShiftOrderInfo = new List<ShiftOrderModel>();
            ObjectId objectId = ObjectId.Parse(model.RegionId);
            var region = await mdbContext.Regions.Find(t => t.Id == objectId && t.IsActive && !t.IsDeleted).Project(t => new { t.TfxTrailers, t.SlotPeriod }).FirstOrDefaultAsync();
            int slotPeriod = region.SlotPeriod;
            var shiftInformation = driverschduleInfo.DriverShiftInfo;
            var shiftDriverInfo = driverschduleInfo.DriverShiftInfo.SelectMany(x => x.DriverInformation).ToList();
            var shifts = shiftInformation.Select(t => new ShiftModel() { Id = t.Id.ToString(), StartTime = t.StartTime, EndTime = t.EndTime }).ToList();
            var shiftsNotInRegion = model.Shifts.Where(t => !shifts.Any(t1 => t1.Id.ToString() == t.Id)).ToList();
            var regionShiftInfo = await mdbContext.Shifts.Find(x => x.RegionId == objectId && x.IsActive && !x.IsDeleted).ToListAsync();
            foreach (var item in regionShiftInfo)
            {
                TimeSpan tspan;
                tspan = DateTime.ParseExact(item.StartTime, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
                regionShiftOrderInfo.Add(new ShiftOrderModel { Id = item.Id.ToString(), Ticks = tspan.Ticks, OrderNo = 0 });
            }
            int shiftIndex = 0;
            var shiftOrderInfo = regionShiftOrderInfo.OrderBy(x => x.Ticks).ToList();
            foreach (var item in shiftOrderInfo)
            {
                item.OrderNo = shiftIndex;
                shiftIndex++;
            }

            List<MdbDataAccess.Collections.DropdownDisplayItem> TfxDrivers = new List<MdbDataAccess.Collections.DropdownDisplayItem>();
            IntializeEmptyScheduleShift(model, shifts);
            List<TripViewModel> tempTrips = new List<TripViewModel>();
            if (shifts != null && shifts.Any())
            {
                if (model.Shifts.Any())
                {
                    AssignModelShiftInfo(model, shifts);
                    if (model.Shifts.Any())
                    {
                        slotPeriod = model.Shifts[0].SlotPeriod;
                    }
                }
                shifts.ForEach(t => t.SlotPeriod = slotPeriod);
                var trailers = new List<TrailerModel>();
                var driversInRegion = new List<DriverAdditionalDetailsViewModel>();
                if (region.TfxTrailers != null && region.TfxTrailers.Any())
                {
                    trailers = GetTrailersForRegion(region.TfxTrailers);
                }
                var trailersNotInRegion = model.Trailers.Where(t => !trailers.Any(t1 => t1.Id == t.Id)).ToList();
                trailers.AddRange(trailersNotInRegion);
                if (shiftDriverInfo.Any())
                {
                    shiftDriverInfo.ForEach(x =>
                    {
                        TfxDrivers.Add(new MdbDataAccess.Collections.DropdownDisplayItem { Id = x.Id, Name = x.Name, Code = x.Code });
                    });
                    driversInRegion = GetDriversForRegion(TfxDrivers);
                }
                var driversInSbIds = model.Trips.SelectMany(t => t.Drivers).Where(t => t.Id > 0).GroupBy(t => t.Id).Select(t => t.FirstOrDefault().Id).ToList();
                var driversInSb = mdbContext.Drivers.Find(t => driversInSbIds.Contains(t.DriverId) && !t.IsDeleted)
                                                    .Project(t => new DriverAdditionalDetailsViewModel
                                                    {
                                                        Id = t.DriverId,
                                                        Name = t.DriverName,
                                                        IsFilldCompatible = t.IsFilldAuthorized
                                                    }).ToList();
                var driversNotInSb = driversInRegion.Where(t => !driversInSb.Any(t1 => t1.Id == t.Id)).ToList();
                var allDrivers = driversInSb.Concat(driversNotInSb).ToList();
                if ((!string.IsNullOrWhiteSpace(model.Id) && model.ObjectFilter == 2 && sbView == 0) || sbView == 2)
                {
                    GetTrailers(model, shifts, slotPeriod, trailers);
                }
                else
                {
                    tempTrips.AddRange(model.Trips);
                    GridViewShifts(model, shifts, slotPeriod, trailers, allDrivers, recurringShiftDetails, driverschduleInfo, tempTrips, driversInRegion, shiftOrderInfo, IsBackgroundJobScheduleCreation);
                }
            }
            else
            {
                //verify the drivers shift schedule informations.
                ObjectId objRegionId = ObjectId.Empty;
                ObjectId.TryParse(model.RegionId, out objRegionId);
                var regionShifts = await mdbContext.Shifts.Find(t => t.RegionId == objRegionId && t.IsActive && !t.IsDeleted).ToListAsync();
                if (!shiftDriverInfo.Any())
                {
                    model.Trips.ForEach(x => x.IsDriverScheduleExists = false);
                }
                if (!driverschduleInfo.DriverShiftInfo.Any() && regionShifts.Any())
                {
                    model.IsNoDriverShiftFound = 1;
                }
                if (recurringShiftDetails != null && recurringShiftDetails.Any())
                {
                    tempTrips.AddRange(model.Trips);
                    var sbDate = Convert.ToDateTime(model.Date);
                    IntializeEmptyColRecurringScheduleModel(model, slotPeriod, recurringShiftDetails, sbDate, objRegionId);
                }
            }
        }
        /// <summary>
        /// Assign Shift Info to model.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="shifts"></param>
        private static void AssignModelShiftInfo(ScheduleBuilderViewModel model, List<ShiftModel> shifts)
        {
            var shiftInfo = new List<ShiftModel>();
            foreach (var item in shifts)
            {
                var modelShiftInfo = model.Shifts.FirstOrDefault(x => x.Id == item.Id);
                if (modelShiftInfo != null)
                {
                    shiftInfo.Add(modelShiftInfo);
                }
            }
            model.Shifts = shiftInfo;
        }

        /// <summary>
        /// Intialize empty shift if driver schedule removed.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="shifts"></param>
        private static void IntializeEmptyScheduleShift(ScheduleBuilderViewModel model, List<ShiftModel> shifts)
        {
            var tripsDRRecords = model.Trips.Where(top => top.DeliveryRequests.Count() > 0).GroupBy(x => new { x.ShiftId }).Select(t => t.FirstOrDefault()).ToList();
            if (tripsDRRecords.Any())
            {
                foreach (var item in tripsDRRecords)
                {
                    var shiftIndex = shifts.FindIndex(x => x.Id == item.ShiftId);
                    if (shiftIndex == -1)
                    {
                        shifts.Add(new ShiftModel { Id = item.ShiftId, StartTime = item.StartTime, EndTime = item.EndTime });
                    }
                }
            }
        }

        private void GridViewShifts(ScheduleBuilderViewModel model, List<ShiftModel> shifts, int slotPeriod, List<TrailerModel> trailers, List<DriverAdditionalDetailsViewModel> drivers, List<RecurringShiftDetails> recurringShiftDetails, DriverScheduleInformation driverschduleInfo, List<TripViewModel> tempDsbTrips, List<DriverAdditionalDetailsViewModel> regionDrivers, List<ShiftOrderModel> shiftOrderModel, bool IsBackgroundJobScheduleCreation = false)
        {
            var sbDate = Convert.ToDateTime(model.Date);
            int rowIndex, colIndex, shiftIndex = 0;
            var inputString = JsonConvert.SerializeObject(model.Trips);
            var sbTrips = JsonConvert.DeserializeObject<List<TripViewModel>>(inputString);
            List<DriverAdditionalDetailsViewModel> tempdriversInShift = new List<DriverAdditionalDetailsViewModel>();
            model.Trips.Clear();
            foreach (var shift in shifts)
            {
                rowIndex = 0;
                var shiftDriversInfo = driverschduleInfo.DriverShiftInfo.Where(x => x.Id == shift.Id).SelectMany(x => x.DriverInformation).ToList();
                var driversInShift = drivers.Where(t => shiftDriversInfo.Any(t1 => t1.Id == t.Id)).GroupBy(x => x.Id).Select(t => t.FirstOrDefault()).ToList();
                foreach (var drivershiftitem in driversInShift)
                {
                    tempdriversInShift.Add(new DriverAdditionalDetailsViewModel { Id = drivershiftitem.Id, IsFilldCompatible = drivershiftitem.IsFilldCompatible, Name = drivershiftitem.Name, Shifts = shift.Id });
                }
                if (driversInShift != null && driversInShift.Any())
                {
                    var shiftDrivers = GetDriverSequence(driversInShift, sbTrips, shift.Id);
                    foreach (var driver in shiftDrivers)
                    {
                        colIndex = 0;
                        int? maxColIndex = 0;

                        if (recurringShiftDetails != null && recurringShiftDetails.Count() > 0)
                        {
                            var maxColIndexDetails = recurringShiftDetails.Where(top => top.ShiftId == shift.Id && top.DriverRowIndex == rowIndex).FirstOrDefault();
                            if (maxColIndexDetails != null)
                            {
                                maxColIndex = maxColIndexDetails.DriverColIndex;
                            }
                            else
                            {
                                maxColIndex = sbTrips.Where(t => t.ShiftId == shift.Id && (t.Drivers.Any(t1 => t1.Id == driver.Id) || t.DriverRowIndex == rowIndex)).Max(t => t.DriverColIndex);
                            }
                        }
                        else
                        {
                            maxColIndex = sbTrips.Where(t => t.ShiftId == shift.Id && (t.Drivers.Any(t1 => t1.Id == driver.Id) || t.DriverRowIndex == rowIndex)).Max(t => t.DriverColIndex);
                        }
                        var scheduleStartTime = sbDate.Add(DateTime.Parse(shift.StartTime).TimeOfDay);
                        var scheduleEndTime = sbDate.Add(DateTime.Parse(shift.EndTime).TimeOfDay);
                        if (scheduleEndTime <= scheduleStartTime)
                        {
                            scheduleEndTime = scheduleEndTime.AddDays(1);
                        }

                        while (scheduleStartTime < scheduleEndTime || colIndex <= maxColIndex)
                        {
                            var trip = new TripViewModel();

                            trip.StartDate = scheduleStartTime.ToString(Resource.constFormatDate);
                            trip.StartTime = scheduleStartTime.ToShortTimeString();
                            trip.EndTime = scheduleStartTime.AddHours(slotPeriod).ToShortTimeString();
                            trip.SlotPeriod = shift.SlotPeriod;
                            var thisTripEndTime = scheduleStartTime.AddHours(slotPeriod);

                            var existingTrips = sbTrips.Where(t => t.ShiftId == shift.Id && t.DriverRowIndex == rowIndex && t.DriverColIndex == colIndex).ToList();
                            if (existingTrips.Any())
                            {
                                foreach (var existingTrip in existingTrips.OrderBy(t => t.DriverColIndex))
                                {
                                    if (existingTrip.Drivers.Any())
                                    {
                                        var driverId = existingTrip.Drivers.FirstOrDefault().Id;
                                        var driverIndex = regionDrivers.FindIndex(x => x.Id == driverId);
                                        if (driverIndex == -1)
                                        {
                                            existingTrip.IsDriverScheduleExists = false;
                                        }
                                    }

                                    UpdateIndexes(existingTrip, shiftIndex, rowIndex, colIndex);
                                    var existingTripsDriver = sbTrips.Where(x => x.ShiftId == shift.Id).SelectMany(x => x.Drivers).ToList();
                                    if (existingTrip.Drivers.Any())
                                    {
                                        if (existingTripsDriver.Any())
                                        {
                                            var driverExistsOtherTrips = existingTripsDriver.FirstOrDefault(x => x.Id == driver.Id);
                                            if (driverExistsOtherTrips == null)
                                            {
                                                existingTrip.Drivers.Add(driver);
                                            }
                                        }
                                        else
                                        {
                                            existingTrip.Drivers.Add(driver);
                                        }
                                    }
                                    else
                                    {
                                        if (existingTripsDriver.Any())
                                        {
                                            var driverExistsOtherTrips = existingTripsDriver.FirstOrDefault(x => x.Id == driver.Id);
                                            if (driverExistsOtherTrips == null)
                                            {
                                                existingTrip.Drivers.Add(driver);
                                            }
                                        }
                                        else
                                        {
                                            existingTrip.Drivers.Add(driver);
                                        }
                                    }
                                    model.Trips.Add(existingTrip);
                                    var startDate = existingTrip.StartDate;
                                    if (existingTrip.StartDate == null)
                                    {
                                        startDate = model.Date;
                                    }
                                    scheduleStartTime = GetEndDateTime(Convert.ToDateTime(startDate).Add(DateTime.Parse(existingTrip.StartTime).TimeOfDay), Convert.ToDateTime(startDate).Add(DateTime.Parse(existingTrip.EndTime).TimeOfDay));
                                    colIndex++;
                                }
                                continue;
                            }
                            else
                            {
                                if (trip.Drivers.Count() == 0)
                                {
                                    trip.Drivers.Add(driver);
                                }
                            }
                            scheduleStartTime = scheduleStartTime.AddHours(slotPeriod);
                            trip.DeliveryGroupStatus = DeliveryGroupStatus.Draft;
                            trip.ShiftId = shift.Id.ToString();
                            trip.ShiftStartTime = shift.StartTime;
                            trip.IsShiftCollapsed = false;
                            trip.ShiftEndTime = shift.EndTime;
                            trip.ShiftIndex = shiftIndex;
                            trip.DriverRowIndex = rowIndex;
                            trip.DriverColIndex = colIndex;
                            model.Trips.Add(trip);
                            colIndex++;
                        }
                        rowIndex++;
                    }
                }

                shiftIndex++;
                if (!model.Shifts.Any(t => t.Id == shift.Id.ToString()))
                {
                    int shiftOrderNo = 0;
                    var shiftOrderNumber = shiftOrderModel.FirstOrDefault(x => x.Id == shift.Id);
                    if (shiftOrderNumber != null)
                    {
                        shiftOrderNo = shiftOrderNumber.OrderNo;
                    }
                    model.Shifts.Add(new ShiftModel() { Id = shift.Id.ToString(), StartTime = shift.StartTime, EndTime = shift.EndTime, SlotPeriod = slotPeriod, OrderNo = shiftOrderNo });
                }
                else
                {
                    foreach (var item in model.Shifts)
                    {
                        int shiftOrderNo = 0;
                        var shiftOrderNumber = shiftOrderModel.FirstOrDefault(x => x.Id == item.Id);
                        if (shiftOrderNumber != null)
                        {
                            shiftOrderNo = shiftOrderNumber.OrderNo;
                        }
                        item.OrderNo = shiftOrderNo;
                    }
                }
            }
            IntializeEmptyDriverSchedule(model, shifts, slotPeriod, recurringShiftDetails, tempDsbTrips, sbDate, tempdriversInShift);
            if (recurringShiftDetails != null && recurringShiftDetails.Any())
            {
                ObjectId objRegionId = ObjectId.Empty;
                ObjectId.TryParse(model.RegionId, out objRegionId);
                IntializeEmptyColRecurringScheduleModel(model, slotPeriod, recurringShiftDetails, sbDate, objRegionId);
            }
            foreach (var trailer in trailers)
            {
                if (!model.Trailers.Any(t => t.Id == trailer.Id))
                {
                    model.Trailers.Add(new TrailerModel() { Id = trailer.Id, TrailerId = trailer.TrailerId, Compartments = trailer.Compartments, TrailerType = trailer.TrailerType });
                }
            }
            if (!IsBackgroundJobScheduleCreation)
            {
                IntializeScheduleColPriority(model, shifts, drivers, driverschduleInfo);
            }
        }
        /// <summary>
        /// Define Col Schedule Display Priority for get schedule builder.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="shifts"></param>
        /// <param name="drivers"></param>
        /// <param name="driverschduleInfo"></param>
        private static void IntializeScheduleColPriority(ScheduleBuilderViewModel model, List<ShiftModel> shifts, List<DriverAdditionalDetailsViewModel> drivers, DriverScheduleInformation driverschduleInfo)
        {
            foreach (var shift in shifts)
            {
                var shiftDriversInfo = driverschduleInfo.DriverShiftInfo.Where(x => x.Id == shift.Id).SelectMany(x => x.DriverInformation).ToList();
                var driversInShift = drivers.Where(t => shiftDriversInfo.Any(t1 => t1.Id == t.Id)).GroupBy(x => x.Id).Select(t => t.FirstOrDefault()).ToList();
                //DS column has highest priority.
                if (driversInShift.Count > 0)
                {
                    var dsbDSColPriority = model.Trips.Where(x => x.ShiftId == shift.Id && x.DeliveryRequests.Any()).OrderBy(x => x.DriverRowIndex).GroupBy(x => x.DriverRowIndex).Select(x => x.FirstOrDefault()).ToList();
                    if (dsbDSColPriority.Any())
                    {
                        var isDriverColPriorityCount = driversInShift.Count;
                        foreach (var item in dsbDSColPriority)
                        {
                            if (isDriverColPriorityCount >= 0)
                            {
                                var modelTripInfo = model.Trips.Where(x => x.DriverRowIndex == item.DriverRowIndex && x.ShiftId == shift.Id).ToList();
                                modelTripInfo.ForEach(x => { x.IsDriverColPriority = true; x.IsDriverScheduleExists = true; });
                            }
                            else
                            {
                                var modelTripInfo = model.Trips.Where(x => x.DriverRowIndex == item.DriverRowIndex && x.ShiftId == shift.Id).ToList();
                                modelTripInfo.ForEach(x => { x.IsDriverColPriority = true; x.IsDriverScheduleExists = false; });
                            }
                            isDriverColPriorityCount--;
                        }
                    }
                    //DS empty column has lowest priority.
                    var totalEmptyCol = driversInShift.Count - dsbDSColPriority.Count;
                    List<TripViewModel> dsbEmptyColPriority = new List<TripViewModel>();
                    var dsbEmptyDriverColPriority = model.Trips.Where(x => x.ShiftId == shift.Id && !x.DeliveryRequests.Any() && x.Drivers.Any() && x.IsDriverColPriority == false).OrderBy(x => x.DriverRowIndex).GroupBy(x => x.DriverRowIndex).Select(x => x.FirstOrDefault()).Take(totalEmptyCol).ToList();
                    var dsbEmptyColPriorityResult = model.Trips.Where(x => x.ShiftId == shift.Id && !x.DeliveryRequests.Any() && x.IsDriverColPriority == false).OrderBy(x => x.DriverRowIndex).GroupBy(x => x.DriverRowIndex).Select(x => x.FirstOrDefault()).Take(totalEmptyCol).ToList();
                    if (dsbEmptyDriverColPriority.Any())
                    {
                        dsbEmptyColPriority.AddRange(dsbEmptyDriverColPriority);
                    }
                    if (dsbEmptyColPriorityResult.Any())
                    {
                        foreach (var item in dsbEmptyColPriorityResult)
                        {
                            var emptyDsbIndex = dsbEmptyColPriority.FindIndex(x => x.ShiftId == item.ShiftId && x.DriverRowIndex == item.DriverRowIndex);
                            if (emptyDsbIndex == -1 && dsbEmptyColPriority.Count < totalEmptyCol)
                            {
                                dsbEmptyColPriority.Add(item);
                            }
                        }
                    }
                    if (dsbEmptyColPriority.Any())
                    {
                        foreach (var item in dsbEmptyColPriority)
                        {
                            var dsColprioItem = model.Trips.Where(x => x.DriverRowIndex == item.DriverRowIndex && x.IsDriverColPriority == false && x.ShiftId == shift.Id).OrderBy(x => x.DriverRowIndex).GroupBy(x => x.DriverRowIndex).ToList();
                            if (dsColprioItem.Count >= 0)
                            {
                                foreach (var dsitem in dsColprioItem)
                                {
                                    var modelTripInfo = model.Trips.Where(x => x.DriverRowIndex == dsitem.FirstOrDefault().DriverRowIndex && x.ShiftId == shift.Id).ToList();
                                    modelTripInfo.ForEach(x => x.IsDriverColPriority = true);
                                }
                            }
                        }
                    }
                }
                else
                {
                    //DS column has highest priority.
                    var dsbDSColPriority = model.Trips.Where(x => x.ShiftId == shift.Id && x.DeliveryRequests.Any()).GroupBy(x => x.DriverRowIndex).Select(x => x.FirstOrDefault()).ToList();
                    if (dsbDSColPriority.Any())
                    {
                        foreach (var item in dsbDSColPriority)
                        {
                            var modelTripInfo = model.Trips.Where(x => x.DriverRowIndex == item.DriverRowIndex && x.ShiftId == shift.Id).ToList();
                            modelTripInfo.ForEach(x => { x.IsDriverColPriority = true; });
                        }
                    }
                }
            }
            model.Trips = model.Trips.Where(top => top.IsDriverColPriority).OrderBy(x => x.DriverRowIndex).ToList();
        }

        //get schedule builder dsb view.
        private async Task<int> GetScheduleBuilderView(string scheduleBuilderViewId)
        {
            int dSBFilter = 2;
            ObjectId objScheduleBuilderViewId = ObjectId.Empty;
            ObjectId.TryParse(scheduleBuilderViewId, out objScheduleBuilderViewId);
            var scheduleBuilderDsbView = await mdbContext.ScheduleBuilders
                               .Find(t => t.IsActive && !t.IsDeleted && t.Id == objScheduleBuilderViewId).FirstOrDefaultAsync();
            if (scheduleBuilderDsbView != null)
            {
                dSBFilter = scheduleBuilderDsbView.DSBFilter;
            }
            return dSBFilter;
        }
        //IntializeEmptyDriverSchedule
        private static void IntializeEmptyDriverSchedule(ScheduleBuilderViewModel model, List<ShiftModel> shifts, int slotPeriod, List<RecurringShiftDetails> recurringShiftDetails, List<TripViewModel> tempDsbTrips, DateTime sbDate, List<DriverAdditionalDetailsViewModel> driversInShift)
        {
            foreach (var shift in shifts)
            {
                int recurringShiftIndex = 0;
                //initialize empty recurring schedule column.//driver removed
                IntializeEmptyColRecurringScheduleModel(model, slotPeriod, recurringShiftDetails, sbDate, recurringShiftIndex, shift);
                //initialize empty column schedule.//driver removed
                IntializeEmptyDriverColSchedule(model, recurringShiftDetails, tempDsbTrips, shift, driversInShift);
                recurringShiftIndex++;
            }

        }
        //IntializeEmptyDriverColSchedule
        //here we are find the driver schedule if any exists after removed from regions or schedule not exists
        private static void IntializeEmptyDriverColSchedule(ScheduleBuilderViewModel model, List<RecurringShiftDetails> recurringShiftDetails, List<TripViewModel> tempDsbTrips, ShiftModel shift, List<DriverAdditionalDetailsViewModel> driversInShift)
        {
            if (recurringShiftDetails == null || !recurringShiftDetails.Any())
            {
                //initialize empty column schedule.//driver removed
                var dsbDriverShiftInfo = tempDsbTrips.Where(top => top.ShiftId == shift.Id).ToList();
                var dsbDriverEmptyColumnInfo = dsbDriverShiftInfo.Where(top => top.DeliveryRequests.Count() > 0 || top.Drivers.Count() > 0).GroupBy(x => new { x.DriverRowIndex, x.DriverColIndex }).Select(t => t.FirstOrDefault()).ToList();
                if (dsbDriverEmptyColumnInfo.Any())
                {
                    foreach (var item in dsbDriverEmptyColumnInfo)
                    {
                        var tripIndex = model.Trips.FindIndex(top => top.DriverRowIndex == item.DriverRowIndex && top.ShiftId == shift.Id.ToString());
                        if (tripIndex == -1)
                        {
                            if (driversInShift.Any())
                            {
                                var tripDriverColList = dsbDriverShiftInfo.Where(x => x.DriverRowIndex == item.DriverRowIndex).ToList();
                                if (tripDriverColList.Any())
                                {
                                    var tripDriversIds = tripDriverColList.SelectMany(x => x.Drivers).Select(x => x.Id).Distinct().ToList();
                                    var driversInShiftIds = driversInShift.Where(top => top.Shifts == shift.Id).Select(x => x.Id).Distinct().ToList();
                                    var findIndex = driversInShiftIds.FindIndex(x => driversInShiftIds.Contains(x));
                                    if (findIndex == -1)
                                    {
                                        tripDriverColList.ForEach(x => x.IsDriverScheduleExists = false);
                                    }
                                    else if (!tripDriversIds.Any())
                                    {
                                        tripDriverColList.ForEach(x => x.IsDriverScheduleExists = false);
                                    }
                                    model.Trips.AddRange(tripDriverColList);
                                }
                            }
                            else
                            {
                                var tripDriverColList = dsbDriverShiftInfo.Where(x => x.DriverRowIndex == item.DriverRowIndex).ToList();
                                if (tripDriverColList.Any())
                                {
                                    tripDriverColList.ForEach(x => x.IsDriverScheduleExists = false);
                                    model.Trips.AddRange(tripDriverColList);
                                }

                            }
                        }
                    }

                }
            }
        }

        //here we are find the driver schedule if any exists after removed from regions or schedule not exists --- for recurring schedule
        private static void IntializeEmptyColRecurringScheduleModel(ScheduleBuilderViewModel model, int slotPeriod, List<RecurringShiftDetails> recurringShiftDetails, DateTime sbDate, int shiftIndex, ShiftModel shift)
        {
            if (recurringShiftDetails != null && recurringShiftDetails.Any())
            {
                var recurringDREmptySchedule = recurringShiftDetails.Where(top => top.ShiftId == shift.Id).ToList();
                foreach (var item in recurringDREmptySchedule)
                {
                    var recolIndex = 0;
                    var tripIndex = model.Trips.FindIndex(top => top.DriverRowIndex == item.DriverRowIndex && top.ShiftId == item.ShiftId);
                    if (tripIndex == -1)
                    {
                        var scheduleStartTime = sbDate.Add(DateTime.Parse(shift.StartTime).TimeOfDay);
                        var scheduleEndTime = sbDate.Add(DateTime.Parse(shift.EndTime).TimeOfDay);
                        if (scheduleEndTime <= scheduleStartTime)
                        {
                            scheduleEndTime = scheduleEndTime.AddDays(1);
                        }

                        while (scheduleStartTime < scheduleEndTime)
                        {
                            var trip = new TripViewModel();
                            trip.StartDate = scheduleStartTime.ToString(Resource.constFormatDate);
                            trip.StartTime = scheduleStartTime.ToShortTimeString();
                            trip.EndTime = scheduleStartTime.AddHours(slotPeriod).ToShortTimeString();
                            trip.SlotPeriod = shift.SlotPeriod;
                            var thisTripEndTime = scheduleStartTime.AddHours(slotPeriod);
                            scheduleStartTime = scheduleStartTime.AddHours(slotPeriod);
                            trip.DeliveryGroupStatus = DeliveryGroupStatus.Draft;
                            trip.ShiftId = shift.Id.ToString();
                            trip.ShiftStartTime = shift.StartTime;
                            trip.IsShiftCollapsed = false;
                            trip.ShiftEndTime = shift.EndTime;
                            trip.ShiftIndex = item.ShiftIndex;
                            trip.DriverRowIndex = item.DriverRowIndex;
                            trip.DriverColIndex = recolIndex;
                            model.Trips.Add(trip);
                            recolIndex++;
                        }
                    }
                }
            }
        }
        private async void IntializeEmptyColRecurringScheduleModel(ScheduleBuilderViewModel model, int slotPeriod, List<RecurringShiftDetails> recurringShiftDetails, DateTime sbDate, ObjectId objRegionId)
        {
            if (recurringShiftDetails != null && recurringShiftDetails.Any())
            {
                foreach (var item in recurringShiftDetails)
                {
                    ObjectId objShiftId = ObjectId.Empty;
                    ObjectId.TryParse(item.ShiftId, out objShiftId);
                    var shift = await mdbContext.Shifts.Find(x => x.Id == objShiftId).FirstOrDefaultAsync();
                    var region = await mdbContext.Regions.Find(t => t.Id == objRegionId).Project(t => new { t.SlotPeriod }).FirstOrDefaultAsync();
                    if (shift != null && region != null)
                    {
                        var recolIndex = 0;
                        var tripIndex = model.Trips.FindIndex(top => top.DriverRowIndex == item.DriverRowIndex && top.ShiftId == item.ShiftId);
                        if (tripIndex == -1)
                        {
                            var scheduleStartTime = sbDate.Add(DateTime.Parse(shift.StartTime).TimeOfDay);
                            var scheduleEndTime = sbDate.Add(DateTime.Parse(shift.EndTime).TimeOfDay);
                            if (scheduleEndTime <= scheduleStartTime)
                            {
                                scheduleEndTime = scheduleEndTime.AddDays(1);
                            }

                            while (scheduleStartTime < scheduleEndTime)
                            {
                                var trip = new TripViewModel();
                                trip.StartDate = scheduleStartTime.ToString(Resource.constFormatDate);
                                trip.StartTime = scheduleStartTime.ToShortTimeString();
                                trip.EndTime = scheduleStartTime.AddHours(slotPeriod).ToShortTimeString();
                                trip.SlotPeriod = region.SlotPeriod;
                                var thisTripEndTime = scheduleStartTime.AddHours(slotPeriod);
                                scheduleStartTime = scheduleStartTime.AddHours(slotPeriod);
                                trip.DeliveryGroupStatus = DeliveryGroupStatus.Draft;
                                trip.ShiftId = shift.Id.ToString();
                                trip.ShiftStartTime = shift.StartTime;
                                trip.IsShiftCollapsed = false;
                                trip.ShiftEndTime = shift.EndTime;
                                trip.ShiftIndex = item.ShiftIndex;
                                trip.DriverRowIndex = item.DriverRowIndex;
                                trip.DriverColIndex = recolIndex;
                                model.Trips.Add(trip);
                                recolIndex++;
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region UtilityFunctions
        private static string GetWeekInformation(RecurringSchedules scheduleInfo, string weekname)
        {
            foreach (var weekInfo in scheduleInfo.WeekDayId)
            {
                switch (weekInfo)
                {
                    case "1":
                        weekname += "MON,";
                        break;
                    case "2":
                        weekname += "TUE,";
                        break;
                    case "3":
                        weekname += "WED,";
                        break;
                    case "4":
                        weekname += "THU,";
                        break;
                    case "5":
                        weekname += "FRI,";
                        break;
                    case "6":
                        weekname += "SAT,";
                        break;
                    default:
                        weekname += "SUN,";
                        break;
                }

            }

            return weekname;
        }
        private static string GetSchduleQtyTypeText(RecurringSchedules scheduleInfo)
        {
            string qtyText = string.Empty;
            var enumerationType = typeof(ScheduleQuantityType);
            var name = Enum.GetName(enumerationType, scheduleInfo.ScheduleQuantityType);
            var memberInfo = enumerationType.GetMember(name);
            if (memberInfo.Count() > 0)
            {
                var attributes = memberInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);
                if (attributes.Count() > 0)
                {
                    var description = ((DisplayAttribute)attributes[0]).Name;
                    qtyText = description;
                }
            }
            return qtyText;
        }
        private static void IntializeShiftOrderInfo(List<ShiftModel> shifts)
        {
            foreach (var item in shifts)
            {
                TimeSpan tspan;
                tspan = DateTime.ParseExact(item.StartTime, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
                item.Ticks = tspan.Ticks;
            }
            int shiftIndex = 0;
            var shiftOrderInfo = shifts.OrderBy(x => x.Ticks).ToList();
            foreach (var item in shiftOrderInfo)
            {
                item.OrderNo = shiftIndex;
                shiftIndex++;
            }
        }
        #endregion
        #region ResetDriverSchedule
        public async Task<List<ScheduleBuilderViewModel>> RemoveDriverFromDsbSchdule(Region region, List<List<MdbDataAccess.Collections.DropdownDisplayItem>> regionDbDrivers)
        {
            List<ScheduleBuilderViewModel> scheduleBuilders = new List<ScheduleBuilderViewModel>();
            var regionFormDrivers = region.TfxDrivers.Select(top => top.Id).ToList();
            List<int> regionDbDriversIds = new List<int>();
            foreach (var item in regionDbDrivers)
            {
                regionDbDriversIds.AddRange(item.Select(x => x.Id));
            }
            var removedDriverInfo = regionDbDriversIds.Distinct().ToList().Except(region.TfxDrivers.Select(top => top.Id).ToList()).ToList();
            if (removedDriverInfo.Any())
            {
                var regionId = region.Id.ToString();
                DateTime dateFilter = DateTimeOffset.Now.Date;
                var scheduleBuildersInfo = await mdbContext.ScheduleBuilders
                                    .Find(t => t.IsActive && !t.IsDeleted && t.TfxCompanyId == region.TfxCompanyId
                                        && t.RegionId == regionId
                                        && t.DateFilter >= dateFilter && t.Trips.Any(i => i.TfxDrivers.Any(x => removedDriverInfo.Contains(x.Id)))).ToListAsync();
                foreach (var item in scheduleBuildersInfo)
                {
                    if (item.DSBFilter == 2)
                    {
                        var response = await GetScheduleBuilderViewModel(item, item.TfxCompanyId, item.UpdatedBy, regionId, item.DateFilter.ToString(Resource.constFormatDate), item.ObjectFilter, item.DSBFilter, null, string.Empty);
                        if (response.StatusCode == (int)Status.Success)
                        {
                            List<TripViewModel> tripViews = new List<TripViewModel>();
                            var trips = response.Trips;
                            foreach (var driverIds in removedDriverInfo)
                            {

                                var driverRowDetails = response.Trips.Where(x => x.Drivers.Any(top => top.Id == driverIds)).FirstOrDefault();
                                if (driverRowDetails != null)
                                {
                                    var driverRowTripDetails = response.Trips.Where(top => top.DriverRowIndex == driverRowDetails.DriverRowIndex && top.ShiftId == driverRowDetails.ShiftId).ToList();
                                    tripViews.AddRange(driverRowTripDetails);
                                }
                            }
                            if (tripViews.Any())
                            {
                                response.Trips.Clear();
                                response.Trips.AddRange(tripViews);
                            }

                            scheduleBuilders.Add(response);
                        }
                    }
                }

            }
            return scheduleBuilders;
        }
        public async Task<List<ScheduleBuilderViewModel>> RemoveDriverFromDsbSchdule(string shiftId, int driverId, DateTime date)
        {
            List<ScheduleBuilderViewModel> scheduleBuilders = new List<ScheduleBuilderViewModel>();

            DateTime currentDate = DateTimeOffset.Now.Date;
            if (date >= currentDate)
            {
                ObjectId objShiftId = ObjectId.Empty;
                ObjectId.TryParse(shiftId, out objShiftId);
                var sen = mdbContext.Shifts.Find(s => s.IsActive && !s.IsDeleted && s.Id == objShiftId)
                                                       .Project(two => new { two.StartTime, two.EndTime, two.Name }).FirstOrDefault();
                var scheduleBuildersInfo = await mdbContext.ScheduleBuilders
                                    .Find(t => t.IsActive && !t.IsDeleted
                                        && date == t.DateFilter && t.Trips.Any(i => i.TfxDrivers.Any(x => x.Id == driverId) && i.ShiftId == shiftId || (i.ShiftStartTime == sen.StartTime && i.ShiftEndTime == sen.EndTime && i.IsIncludeAllRegionDriver))).ToListAsync();
                foreach (var item in scheduleBuildersInfo)
                {
                    if (item.DSBFilter == 2)
                    {
                        var response = await GetScheduleBuilderViewModel(item, item.TfxCompanyId, item.UpdatedBy, item.RegionId.ToString(), item.DateFilter.ToString(Resource.constFormatDate), item.ObjectFilter, item.DSBFilter, null, string.Empty);
                        if (response.StatusCode == (int)Status.Success)
                        {
                            List<TripViewModel> tripViews = new List<TripViewModel>();
                            var trips = response.Trips;
                            var driverRowDetails = response.Trips.Where(x => x.Drivers.Any(top => top.Id == driverId) && x.ShiftId == shiftId).FirstOrDefault();
                            if (driverRowDetails != null)
                            {
                                var driverRowTripDetails = response.Trips.Where(top => top.DriverRowIndex == driverRowDetails.DriverRowIndex && top.ShiftId == driverRowDetails.ShiftId).ToList();
                                tripViews.AddRange(driverRowTripDetails);
                            }
                            if (tripViews.Any())
                            {
                                response.Trips.Clear();
                                response.Trips.AddRange(tripViews);
                            }

                            scheduleBuilders.Add(response);
                        }
                    }
                }
            }
            return scheduleBuilders;
        }

        #endregion
        public async Task<List<DriverScheduleViewModel>> GetSelectedDateDriverScheduleByDriverId(int driverId, DateTimeOffset selectedDate, string shiftId)
        {
            var response = new List<DriverScheduleViewModel>();
            var driverSchedule = await mdbContext.DriverScheduleShiftMapping.Find(x => x.DriverId == driverId && x.IsActive && !x.IsDeleted).ToListAsync();

            foreach (var item in driverSchedule)
            {
                var driverScheduleList = item.DriverScheduleList.Where(x => x.ShiftId.ToString() == shiftId && x.RepeatDayList != null && x.IsActive).ToList();
                foreach (var driverScheduleInfo in driverScheduleList)
                {
                    var repeatDaysList = driverScheduleInfo.RepeatDayList;
                    var scheduleIndex = repeatDaysList.FindIndex(f => f.Date == selectedDate.Date);
                    if (scheduleIndex >= 0)
                    {
                        response.Add(driverScheduleInfo.ToDriverScheduleModel());
                    }
                }
            }
            return response;
        }
        public async Task<ResetDeliveryGroupScheduleModel> RemoveScheduleBuilderDrs(ResetDeliveryGroupScheduleModel model)
        {
            using (var session = await mdbContext.Client.StartSessionAsync())
            {
                try
                {
                    session.StartTransaction();

                    var dsbFilter = Builders<ScheduleBuilder>.Filter.And(Builders<ScheduleBuilder>.Filter.Where(x => x.IsActive && x.IsDeleted != true && x.Id == ObjectId.Parse(model.ScheduleBuilderId)));

                    ScheduleBuilder scheduleBuilder = await mdbContext.ScheduleBuilders.Find(dsbFilter).FirstOrDefaultAsync();

                    if (scheduleBuilder != null)
                    {
                        List<ObjectId> assignedDrIds = model.DeliveryRequestIds.ConvertAll(x => ObjectId.Parse(x));
                        var assignedDrs = await mdbContext.DeliveryRequests.Find(dr => assignedDrIds.Contains(dr.Id)).ToListAsync();

                        foreach (var trip in scheduleBuilder.Trips)
                        {
                            //drs assigned from current trip
                            var drIdsRemovedFromTrip = trip.DeliveryRequests.FindAll(dr => assignedDrIds.Contains(dr)).ToList();

                            if (drIdsRemovedFromTrip.Any())
                            {
                                //all trip drs assigned to carrier
                                if (trip.DeliveryRequests.Count() == drIdsRemovedFromTrip.Count())
                                {
                                    trip.DeliveryGroupStatus = DeliveryGroupStatus.None;
                                    trip.GroupId = 0;
                                    trip.TripStatus = TripStatus.None;
                                    trip.TfxTerminal = null; //new DropdownDisplayItem();
                                    trip.TfxBulkPlant = null; //new DropAddressViewModel();
                                    trip.IsCommonPickup = false;
                                    trip.PickupLocationType = PickupLocationType.None;
                                    trip.UpdatedByName = model.UpdatedByName;
                                    trip.UpdatedBy = model.UserId;
                                    //trip is published
                                    if (trip.GroupId > 0)
                                    {
                                        model.DeliveryGroupIds.Add(trip.GroupId);
                                    }
                                }
                                //some trip drs assigned to carrier
                                else
                                {
                                    //published drs
                                    if (trip.GroupId > 0)
                                    {
                                        var removedDrsFromTrip = assignedDrs.FindAll(dr => drIdsRemovedFromTrip.Contains(dr.Id));
                                        model.DeliveryScheduleIds.AddRange(removedDrsFromTrip.Select(dr => dr.TfxDeliveryScheduleId.GetValueOrDefault()).ToList());
                                    }
                                }
                                trip.DeliveryRequests.RemoveAll(dr => assignedDrIds.Contains(dr));
                            }
                        }

                        //UPDATE DSB
                        var updateFieldsDsb = Builders<ScheduleBuilder>.Update
                        .Set(t => t.Trips, scheduleBuilder.Trips)
                        //.Set(t => t.TimeStamp, dsbEntity.TimeStamp)
                        .Set(t => t.UpdatedBy, model.UserId)
                        .Set(t => t.UpdatedOn, DateTimeOffset.Now);

                        await mdbContext.ScheduleBuilders.UpdateOneAsync(dsbFilter, updateFieldsDsb);

                        //UPDATE DRS
                        var updateFieldsDr = Builders<DeliveryRequest>.Update
                            .Set(t => t.CarrierStatus, (int)BrokeredDrCarrierStatus.Pending)
                            .Set(t => t.ScheduleBuilderId, null)
                            .Set(t => t.PreLoadedFor, null)
                            .Set(t => t.PostLoadedFor, null)
                            .Set(t => t.PreLoadInfo, null)
                            .Set(t => t.PostLoadInfo, null)
                            .Set(t => t.ScheduleShiftEndDateTime, null)
                            .Set(t => t.Status, DeliveryReqStatus.Assigned)
                            .Set(t => t.TfxDeliveryGroupId, null)
                            .Set(t => t.TfxDeliveryScheduleId, null)
                            .Set(t => t.TfxScheduleStatus, 0)
                            .Set(t => t.TfxScheduleEnrouteStatus, 0)
                            .Set(t => t.TfxScheduleStatusName, "")
                            .Set(t => t.TfxTrackableScheduleId, null)
                            .Set(t => t.TripId, null);

                        var filterDr = Builders<DeliveryRequest>.Filter.And(Builders<DeliveryRequest>.Filter.Where(x => assignedDrIds.Contains(x.Id)));

                        await mdbContext.DeliveryRequests.UpdateManyAsync(filterDr, updateFieldsDr);

                        await session.CommitTransactionAsync();

                        model.StatusCode = (int)Status.Success;
                        model.StatusMessage = Resource.msgSuccess;
                    }
                }
                catch (Exception ex)
                {
                    await session.AbortTransactionAsync();
                    LogManager.Logger.WriteException("DeliveryRequestRepository", "RemoveScheduleBuilderDrs", ex.Message + " DR IDS - " + string.Join(",", model.DeliveryRequestIds + "  DSB ID - " + model.ScheduleBuilderId), ex);
                }
            }

            return model;
        }

        public async Task<StatusModel> AddOptionalPickup(List<DSBColumnOptionalPickupInfoModel> model)
        {
            StatusModel response = new StatusModel();
            bool isSessionStarted = false;
            if (model != null && model.Any())
            {
                var optionalPickupInfo = model.ToEntity();
                using (var session = await mdbContext.Client.StartSessionAsync())
                {
                    try
                    {
                        session.StartTransaction();
                        isSessionStarted = true;
                        await mdbContext.DSBColumnOptionalPickupInfos.InsertManyAsync(optionalPickupInfo);
                        await session.CommitTransactionAsync();
                        response.StatusCode = (int)Status.Success;
                        response.StatusMessage = string.Format(Resource.successOptionalPickup, model.FirstOrDefault().DriverColIndex + 1, model.FirstOrDefault().ShiftOrderNumber);

                    }
                    catch (Exception ex)
                    {
                        if (isSessionStarted)
                        {
                            await session.AbortTransactionAsync();
                        }
                        response.StatusCode = (int)Status.Failed;
                        response.StatusMessage = Resource.errorDRCarrierSeq;
                        throw ex;
                    }
                }
            }
            return response;
        }

        public async Task<List<DSBColumnOptionalPickupInfoModel>> GetOptionalPickupDetails(DSBColumnOptionalPickupInfoModel dSBColumnOptionalPickup)
        {
            List<DSBColumnOptionalPickupInfoModel> dSBColumnOptionalPickupInfo = new List<DSBColumnOptionalPickupInfoModel>();
            if (dSBColumnOptionalPickup != null)
            {
                ObjectId objScheduleBuilderId = ObjectId.Empty;
                ObjectId.TryParse(dSBColumnOptionalPickup.ScheduleBuilderId, out objScheduleBuilderId);
                ObjectId objShiftId = ObjectId.Empty;
                ObjectId.TryParse(dSBColumnOptionalPickup.ShiftId, out objShiftId);
                var optionalPickupInfo = await mdbContext.DSBColumnOptionalPickupInfos
                        .Find(top => top.ScheduleBuilderId == objScheduleBuilderId && top.IsActive && !top.IsDeleted
                        && top.ShiftId == objShiftId && top.DriverColIndex == dSBColumnOptionalPickup.DriverColIndex).ToListAsync();
                if (optionalPickupInfo.Any())
                {
                    dSBColumnOptionalPickupInfo = optionalPickupInfo.ToEntity();
                }
            }
            return dSBColumnOptionalPickupInfo;
        }
        public async Task<StatusModel> DeleteOptionalPickupDetails(string Id)
        {
            StatusModel response = new StatusModel();
            bool isSessionStarted = false;
            ObjectId objectId = ObjectId.Empty;
            ObjectId.TryParse(Id, out objectId);
            using (var session = await mdbContext.Client.StartSessionAsync())
            {
                try
                {
                    session.StartTransaction();
                    isSessionStarted = true;
                    var filter = Builders<DSBColumnOptionalPickupInfo>.Filter.And(
                                           Builders<DSBColumnOptionalPickupInfo>.Filter.Eq(t => t.Id, objectId));
                    var updateFields = Builders<DSBColumnOptionalPickupInfo>.Update
                                                      .Set(t => t.IsActive, false)
                                                      .Set(t => t.IsDeleted, true);
                    await mdbContext.DSBColumnOptionalPickupInfos.UpdateOneAsync(filter, updateFields);
                    await session.CommitTransactionAsync();
                    response.StatusCode = (int)Status.Success;
                    response.StatusMessage = Resource.successOptionalPickupDelete;
                }
                catch (Exception ex)
                {
                    if (isSessionStarted)
                    {
                        await session.AbortTransactionAsync();
                    }
                    response.StatusCode = (int)Status.Failed;
                    response.StatusMessage = Resource.errorOptionalPickupDelete;
                    throw ex;
                }
            }
            return response;
        }

        public async Task<StatusModel> UpdateDROptionalPickupInfo(List<ScheduleOptionalPickupModel> scheduleOptionalPickups)
        {
            StatusModel response = new StatusModel();
            if (scheduleOptionalPickups != null && scheduleOptionalPickups.Any())
            {
                bool isSessionStarted = false;
                using (var session = await mdbContext.Client.StartSessionAsync())
                {
                    try
                    {
                        session.StartTransaction();
                        isSessionStarted = true;
                        foreach (var item in scheduleOptionalPickups)
                        {
                            var filter = Builders<DeliveryRequest>.Filter.And(
                                         Builders<DeliveryRequest>.Filter.Eq(t => t.TfxTrackableScheduleId, item.TrackableScheduleId));
                            var updateFields = Builders<DeliveryRequest>.Update
                                                              .Set(t => t.OptionalPickupIds, item.OptionalPickupIds);
                            await mdbContext.DeliveryRequests.UpdateOneAsync(filter, updateFields);
                        }
                        await session.CommitTransactionAsync();
                        response.StatusCode = (int)Status.Success;
                    }
                    catch (Exception ex)
                    {

                        if (isSessionStarted)
                        {
                            await session.AbortTransactionAsync();
                        }
                        response.StatusCode = (int)Status.Failed;
                        throw ex;
                    }

                }

            }
            return response;
        }
        public async Task<List<PreBOLRetainDeliveryDetailsModel>> GetPreLoadDSRetainInfo(List<PreBOLRetainModel> PreLoadBolDRs)
        {
            string defaultCompartmentId = "Compartment";
            var result = new List<PreBOLRetainDeliveryDetailsModel>();
            if (PreLoadBolDRs != null && PreLoadBolDRs.Any())
            {
                List<ObjectId> objDeliveryReqIds = new List<ObjectId>();
                var deliveryReqs = PreLoadBolDRs.Where(x => !string.IsNullOrEmpty(x.DeliveryReqId)).Select(x1 => x1.DeliveryReqId).ToList();
                var PreBoLCompartmentsDetails = PreLoadBolDRs.Where(x => !string.IsNullOrEmpty(x.DeliveryReqId)).SelectMany(x1 => x1.CompartmentInfo).ToList();
                if (PreBoLCompartmentsDetails != null && PreBoLCompartmentsDetails.Any())
                {
                    var PreBoLCompartmentIds = PreBoLCompartmentsDetails.Where(x1 => !string.IsNullOrEmpty(x1.CompartmentId)).Select(x1 => x1.CompartmentId).ToList();
                    var PreBoLTrailerIds = PreBoLCompartmentsDetails.Where(x1 => !string.IsNullOrEmpty(x1.TrailerId)).Select(x1 => x1.TrailerId).ToList();
                    deliveryReqs.ForEach(x => objDeliveryReqIds.Add(ObjectId.Parse(x.ToString())));
                    if (objDeliveryReqIds.Any())
                    {
                        var trailerFuelRetains = await mdbContext.TrailerFuelRetains.Find(x => objDeliveryReqIds.Contains(x.DeliveryRequestId) && !x.IsActive && !x.IsDeleted).ToListAsync();
                        if (PreBoLTrailerIds.Any())
                        {
                            trailerFuelRetains = trailerFuelRetains.Where(x => PreBoLTrailerIds.Contains(x.TrailerId.ToString())).ToList();
                        }
                        if (PreBoLCompartmentIds.Any())
                        {
                            trailerFuelRetains = trailerFuelRetains.Where(x => PreBoLCompartmentIds.Contains(x.CompartmentId.ToString())).ToList();
                        }
                        trailerFuelRetains = trailerFuelRetains.OrderByDescending(x => x.CreatedOn).GroupBy(l => new { l.DeliveryRequestId, l.TrailerId, l.CompartmentId }).Select(g => g.FirstOrDefault()).ToList();
                        foreach (var item in trailerFuelRetains)
                        {
                            var resultInfoIndex = result.FindIndex(x => x.DeliveryReqId == item.DeliveryRequestId.ToString() && x.TrailerId == item.TrailerId.ToString() && x.CompartmentId == item.CompartmentId && x.RetainQuantity == item.Quantity);
                            if (resultInfoIndex == -1)
                            {
                                PreBOLRetainDeliveryDetailsModel preBOLRetainDeliveryModel = new PreBOLRetainDeliveryDetailsModel();
                                preBOLRetainDeliveryModel.CompartmentId = item.CompartmentId;
                                preBOLRetainDeliveryModel.TrailerId = item.TrailerId.ToString();
                                preBOLRetainDeliveryModel.DeliveryReqId = item.DeliveryRequestId.ToString();
                                preBOLRetainDeliveryModel.FuelTypeId = item.ProductId;
                                preBOLRetainDeliveryModel.ProductType = item.ProductType;
                                preBOLRetainDeliveryModel.IsTrailerRetain = item.CompartmentId == defaultCompartmentId ? true : false;
                                preBOLRetainDeliveryModel.RetainQuantity = item.Quantity;
                                result.Add(preBOLRetainDeliveryModel);
                            }
                        }
                    }
                }
            }
            return result;
        }
        public async Task<List<DSBSaveModel>> CancelScheduleBuilder(List<DSBSaveModel> scheduleBuilders)
        {
            using (var session = await mdbContext.Client.StartSessionAsync())
            {
                try
                {
                    session.StartTransaction();
                    for (int index = 0; index < scheduleBuilders.Count; index++)
                    {
                        var scheduleBuilder = scheduleBuilders[index];
                        ScheduleBuilder dsbEntity = null;
                        if (!string.IsNullOrWhiteSpace(scheduleBuilder.Id))
                        {
                            dsbEntity = await CancelScheduleBuilder(scheduleBuilder);
                        }
                        await session.CommitTransactionAsync();
                        CancelScheduleDsbModel(scheduleBuilder, dsbEntity);
                        scheduleBuilder.StatusCode = (int)Status.Success;
                        scheduleBuilder.StatusMessage = GetCancelDSStatusMessage(scheduleBuilder);
                    }
                }
                catch (Exception)
                {
                    await session.AbortTransactionAsync();
                    scheduleBuilders.ForEach(t => t.StatusCode = (int)Status.Failed);
                    scheduleBuilders.ForEach(t => t.StatusMessage = Resource.valMessageSbSaveFailed);
                    throw;
                }
            }
            return scheduleBuilders;
        }
        private async Task<ScheduleBuilder> CancelScheduleBuilder(DSBSaveModel viewModel)
        {
            var dsbId = ObjectId.Parse(viewModel.Id);
            var dsbEntity = await mdbContext.ScheduleBuilders.Find(t => t.Id == dsbId).FirstOrDefaultAsync();
            dsbEntity.TimeStamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var shifts = GetShifts(viewModel, dsbEntity);
            var trailers = GetTrailers(viewModel, dsbEntity);
            List<TripModel> trips = null;
            if (viewModel.Status != (int)DSBMethod.DriverAssignment)
            {
                trips = GetTrips(viewModel, dsbEntity);
            }
            else
            {
                trips = AssignDriverToTrips(viewModel, dsbEntity);
                foreach (var item in trips)
                {
                    var shiftInfo = shifts.FirstOrDefault(x => x.Id == item.ShiftId);
                    if (shiftInfo != null)
                    {
                        item.ShiftIndex = shiftInfo.OrderNo;
                    }
                }
            }
            if (viewModel.Status != (int)DSBMethod.DriverAssignment)
            {
                await UpdateDeliveryRequestsToCancel(viewModel, dsbEntity);
            }
            return dsbEntity;
        }
        private async Task UpdateDeliveryRequestsToCancel(DSBSaveModel dsbModel, ScheduleBuilder dsbEntity)
        {

            foreach (var trip in dsbModel.Trips)
            {
                List<DeliveryRequest> cancellDRsStatus = new List<DeliveryRequest>();
                var modifiedRequestsDR = new List<DeliveryRequestViewModel>();
                var modifiedPostLoadRequestsDR = new List<DeliveryRequestViewModel>();
                List<int> enrouteInCompleted = new List<int> { 7, 8, 9, 22, 23, 24 };
                List<int> enrouteInScheduled = new List<int> { 3, 15, 23 };
                List<int> enrouteInProgress = new List<int> { 1, 3, 11, 12, 13, 14, 15, 16, 17, 18 };
                var modifiedRequests = trip.DeliveryRequests.Select(t => ObjectId.Parse(t.Id)).ToList();
                var cancelDRsList = await mdbContext.DeliveryRequests.Find(x => modifiedRequests.Contains(x.Id)).ToListAsync();
                foreach (var item in cancelDRsList)
                {
                    if (enrouteInScheduled.Contains(item.TfxScheduleStatus))
                    {
                        cancellDRsStatus.Add(item);
                    }
                    else if (enrouteInProgress.Contains(item.TfxScheduleEnrouteStatus) && !enrouteInCompleted.Contains(item.TfxScheduleStatus))
                    {
                        cancellDRsStatus.Add(item);
                    }
                }
                await FindPostLoadCancelDRs(trip, modifiedPostLoadRequestsDR, enrouteInScheduled, enrouteInProgress);
                cancellDRsStatus.ForEach(x => modifiedRequestsDR.Add(x.ToDeliveryRequestViewModel()));
                foreach (var modifiedRequest in modifiedRequestsDR)
                {
                    foreach (var item in trip.DeliveryRequests)
                    {
                        if (item.Id == modifiedRequest.Id)
                        {
                            item.ScheduleStatus = (int)DeliveryScheduleStatus.Canceled;
                            item.TrackScheduleStatus = (int)DeliveryScheduleStatus.Canceled;
                            item.TrackScheduleStatusName = DeliveryScheduleStatus.Canceled.ToString();
                            item.StatusClassId = (int)DeliveryScheduleStatus.Canceled;
                            ObjectId id = ObjectId.Parse(modifiedRequest.Id);
                            var requestFilter = Builders<DeliveryRequest>.Filter.And(
                              Builders<DeliveryRequest>.Filter.Where(x => id == x.Id));
                            var updateList = new List<UpdateDefinition<DeliveryRequest>>();

                            var requestUpdateFields = Builders<DeliveryRequest>.Update
                                    .Set(t => t.TfxScheduleStatus, (int)DeliveryScheduleStatus.Canceled)
                                    .Set(t => t.TfxScheduleStatusName, DeliveryScheduleStatus.Canceled.ToString());
                            updateList.Add(requestUpdateFields);
                            var finalUpdateFields = Builders<DeliveryRequest>.Update.Combine(updateList);
                            await mdbContext.DeliveryRequests.UpdateOneAsync(requestFilter, finalUpdateFields);
                        }
                    }

                }
                await UpdatePostLoadDRstoCancelStatus(modifiedPostLoadRequestsDR);
            }
        }

        private async Task FindPostLoadCancelDRs(TripViewModel trip, List<DeliveryRequestViewModel> modifiedPostLoadRequestsDR, List<int> enrouteInCompleted, List<int> enrouteInProgress)
        {
            var postLoadmodifiedRequests = trip.DeliveryRequests.Where(x1 => x1.PostLoadInfo != null).Select(t => t.PostLoadInfo).ToList();
            if (postLoadmodifiedRequests != null)
            {
                var postLoadmodifiedDRs = postLoadmodifiedRequests.Select(t => ObjectId.Parse(t.DrId)).ToList();
                var postLoadmodifiedDRsInfo = await mdbContext.DeliveryRequests.Find(x => postLoadmodifiedDRs.Contains(x.Id) && (enrouteInProgress.Contains(x.TfxScheduleEnrouteStatus) || enrouteInCompleted.Contains(x.TfxScheduleStatus))).ToListAsync();
                if (postLoadmodifiedDRsInfo.Any())
                {
                    postLoadmodifiedDRsInfo.ForEach(x => modifiedPostLoadRequestsDR.Add(x.ToDeliveryRequestViewModel()));

                }
            }
        }

        private async Task UpdatePostLoadDRstoCancelStatus(List<DeliveryRequestViewModel> modifiedPostLoadRequestsDR)
        {
            if (modifiedPostLoadRequestsDR.Any())
            {
                foreach (var item in modifiedPostLoadRequestsDR)
                {
                    item.ScheduleStatus = (int)DeliveryScheduleStatus.Canceled;
                    item.TrackScheduleStatus = (int)DeliveryScheduleStatus.Canceled;
                    item.TrackScheduleStatusName = DeliveryScheduleStatus.Canceled.ToString();
                    item.StatusClassId = (int)DeliveryScheduleStatus.Canceled;
                    ObjectId id = ObjectId.Parse(item.Id);
                    var requestFilter = Builders<DeliveryRequest>.Filter.And(
                      Builders<DeliveryRequest>.Filter.Where(x => id == x.Id));
                    var updateList = new List<UpdateDefinition<DeliveryRequest>>();

                    var requestUpdateFields = Builders<DeliveryRequest>.Update
                            .Set(t => t.TfxScheduleStatus, (int)DeliveryScheduleStatus.Canceled)
                            .Set(t => t.TfxScheduleStatusName, DeliveryScheduleStatus.Canceled.ToString());
                    updateList.Add(requestUpdateFields);
                    var finalUpdateFields = Builders<DeliveryRequest>.Update.Combine(updateList);
                    await mdbContext.DeliveryRequests.UpdateOneAsync(requestFilter, finalUpdateFields);
                }
            }
        }

        private void CancelScheduleDsbModel(DSBSaveModel model, ScheduleBuilder entity)
        {
            model.Trips.ForEach(t => t.UpdateStatuses());
            foreach (var trip in model.Trips)
            {
                var modifiedDelReqs = trip.DeliveryRequests.Where(t => t.ScheduleStatus == (int)DeliveryScheduleStatus.New || t.ScheduleStatus == (int)DeliveryScheduleStatus.Modified).Select(t => t).ToList();
                modifiedDelReqs.ForEach(t => t.UpdateStatusesToCancel());
                modifiedDelReqs.ForEach(t => t.TripId = trip.TripId);
            }
        }
        private string GetCancelDSStatusMessage(DSBSaveModel model)
        {
            string statusMessage = Resource.valMessageSbCancelSuccess;
            if (model.Trips.Count == 1)
            {
                if (model.Trips[0].GroupId == 0)
                {
                    statusMessage = string.Format(model.DSBFilter == 1 ? Resource.valMessageLoadSaveSuccess : Resource.valMessageLoadCancelDSSuccess, model.Trips[0].DriverColIndex + 1, model.Trips[0].DriverRowIndex + 1, model.Trips[0].ShiftIndex + 1);
                }
                else
                {
                    statusMessage = string.Format(model.DSBFilter == 1 ? Resource.valMessageLoadPublishSuccess : Resource.valMessageInColumnLoadCancelSuccess, model.Trips[0].DriverColIndex + 1, model.Trips[0].DriverRowIndex + 1, model.Trips[0].ShiftIndex + 1);
                }
            }
            else if (model.Trips.Count == 2)
            {
                if (model.Trips.All(t => t.GroupId == 0))
                {
                    statusMessage = string.Format(model.DSBFilter == 1 ? Resource.valMessageLoadDragDropSaveSuccess : Resource.valMessageInColumnLoadDragDropSaveSuccess, model.Trips[0].DriverColIndex + 1, model.Trips[0].DriverRowIndex + 1, model.Trips[0].ShiftIndex + 1, model.Trips[1].DriverColIndex + 1, model.Trips[1].DriverRowIndex + 1, model.Trips[1].ShiftIndex + 1);
                }
                else if (model.Trips.All(t => t.GroupId > 0))
                {
                    statusMessage = string.Format(model.DSBFilter == 1 ? Resource.valMessageLoadDragDropPublishSuccess : Resource.valMessageInColumnLoadDragDropPublishSuccess, model.Trips[0].DriverColIndex + 1, model.Trips[0].DriverRowIndex + 1, model.Trips[0].ShiftIndex + 1, model.Trips[1].DriverColIndex + 1, model.Trips[1].DriverRowIndex + 1, model.Trips[1].ShiftIndex + 1);
                }
                else
                {
                    statusMessage = Resource.valMessageSbCancelSuccess;
                }
            }
            else if (model.Trips.All(t => t.GroupId > 0))
            {
                statusMessage = Resource.valMessageSbCancelSuccess;
            }
            return statusMessage;
        }

        public async Task<ResetDeliveryGroupScheduleModel> RemoveDeliverySchedule(ResetDeliveryGroupScheduleModel model)
        {
            using (var session = await mdbContext.Client.StartSessionAsync())
            {
                try
                {
                    session.StartTransaction();

                    var dsbFilter = Builders<ScheduleBuilder>.Filter.And(Builders<ScheduleBuilder>.Filter.Where(x => x.IsActive && x.IsDeleted != true && x.Id == ObjectId.Parse(model.ScheduleBuilderId)));

                    ScheduleBuilder scheduleBuilder = await mdbContext.ScheduleBuilders.Find(dsbFilter).FirstOrDefaultAsync();

                    if (scheduleBuilder != null)
                    {
                        List<ObjectId> assignedDrIds = model.DeliveryRequestIds.ConvertAll(x => ObjectId.Parse(x));
                        var assignedDrs = await mdbContext.DeliveryRequests.Find(dr => assignedDrIds.Contains(dr.Id)).ToListAsync();

                        foreach (var trip in scheduleBuilder.Trips)
                        {
                            //drs assigned from current trip
                            var drIdsRemovedFromTrip = trip.DeliveryRequests.FindAll(dr => assignedDrIds.Contains(dr)).ToList();

                            if (drIdsRemovedFromTrip.Any())
                            {
                                //all trip drs
                                if (trip.DeliveryRequests.Count() == drIdsRemovedFromTrip.Count())
                                {
                                    trip.DeliveryGroupStatus = DeliveryGroupStatus.None;
                                    trip.GroupId = 0;
                                    trip.TripStatus = TripStatus.None;
                                    trip.TfxTerminal = null; //new DropdownDisplayItem();
                                    trip.TfxBulkPlant = null; //new DropAddressViewModel();
                                    trip.IsCommonPickup = false;
                                    trip.PickupLocationType = PickupLocationType.None;
                                    trip.UpdatedByName = model.UpdatedByName;
                                    trip.UpdatedBy = model.UserId;
                                    //trip is published
                                    if (trip.GroupId > 0)
                                    {
                                        model.DeliveryGroupIds.Add(trip.GroupId);
                                    }
                                }
                                //some trip drs assigned to carrier
                                else
                                {
                                    //published drs
                                    if (trip.GroupId > 0)
                                    {
                                        var removedDrsFromTrip = assignedDrs.FindAll(dr => drIdsRemovedFromTrip.Contains(dr.Id));
                                        model.DeliveryScheduleIds.AddRange(removedDrsFromTrip.Select(dr => dr.TfxDeliveryScheduleId.GetValueOrDefault()).ToList());
                                    }
                                }
                                trip.DeliveryRequests.RemoveAll(dr => assignedDrIds.Contains(dr));
                            }
                        }

                        //UPDATE DSB
                        var updateFieldsDsb = Builders<ScheduleBuilder>.Update
                        .Set(t => t.Trips, scheduleBuilder.Trips)
                        //.Set(t => t.TimeStamp, dsbEntity.TimeStamp)
                        .Set(t => t.UpdatedBy, model.UserId)
                        .Set(t => t.UpdatedOn, DateTimeOffset.Now);

                        await mdbContext.ScheduleBuilders.UpdateOneAsync(dsbFilter, updateFieldsDsb);

                        //UPDATE DRS
                        var updateFieldsDr = Builders<DeliveryRequest>.Update
                            .Set(t => t.ScheduleBuilderId, null)
                            .Set(t => t.PreLoadedFor, null)
                            .Set(t => t.PostLoadedFor, null)
                            .Set(t => t.PreLoadInfo, null)
                            .Set(t => t.PostLoadInfo, null)
                            .Set(t => t.ScheduleShiftEndDateTime, null)
                            .Set(t => t.Status, DeliveryReqStatus.None)
                            .Set(t => t.TfxDeliveryGroupId, null)
                            .Set(t => t.TfxDeliveryScheduleId, null)
                            .Set(t => t.TfxScheduleStatus, 0)
                            .Set(t => t.TfxScheduleEnrouteStatus, 0)
                            .Set(t => t.TfxScheduleStatusName, "")
                            .Set(t => t.TfxTrackableScheduleId, null)
                            .Set(t => t.TripId, null);

                        var filterDr = Builders<DeliveryRequest>.Filter.And(Builders<DeliveryRequest>.Filter.Where(x => assignedDrIds.Contains(x.Id)));

                        await mdbContext.DeliveryRequests.UpdateManyAsync(filterDr, updateFieldsDr);

                        await session.CommitTransactionAsync();

                        model.StatusCode = (int)Status.Success;
                        model.StatusMessage = Resource.msgSuccess;
                    }
                }
                catch (Exception ex)
                {
                    await session.AbortTransactionAsync();
                    LogManager.Logger.WriteException("DeliveryRequestRepository", "RemoveScheduleBuilderDrs", ex.Message + " DR IDS - " + string.Join(",", model.DeliveryRequestIds + "  DSB ID - " + model.ScheduleBuilderId), ex);
                }
            }

            return model;
        }
        protected void GetJobCompatibilityInfo(List<int> jobIds, List<DeliveryRequestViewModel> drs, string selectedDate)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(selectedDate) && drs != null && drs.Any() && jobIds != null && jobIds.Any())
                {
                    var date = DateTime.Parse(selectedDate);
                    var deliveryDay = (int)date.DayOfWeek;
                    deliveryDay = deliveryDay == 0 ? 7 : deliveryDay; // sunday is saved as 7 in our code but 0 in DayOfWeek enum
                    var compatibleTrailerTypes = new List<FreightModels.DropdownDisplayItem>();
                    bool IsAcceptNightDeliveries = false;
                    var jobsDetails = mdbContext.JobAdditionalDetails.Find(t => jobIds.Contains(t.TfxJobId)).Project(t => new { t.TfxJobId, t.TrailerType, t.DeliveryDaysList, t.DistanceCovered }).ToList();
                    foreach (var jobId in jobIds)
                    {
                        IsAcceptNightDeliveries = false;
                        compatibleTrailerTypes = new List<FreightModels.DropdownDisplayItem>();
                        var jobInfoModel = jobsDetails.Where(t => t.TfxJobId == jobId).Select(t => new
                        {
                            TrailerTypes = t.TrailerType,
                            t.DeliveryDaysList,
                            t.DistanceCovered
                        }).FirstOrDefault();
                        if (jobInfoModel != null)
                        {
                            if (jobInfoModel.TrailerTypes != null && jobInfoModel.TrailerTypes.Any())
                            {
                                //compatibleTrailerTypes = new List<FreightModels.DropdownDisplayItem>();
                                foreach (var trailer in jobInfoModel.TrailerTypes)
                                {
                                    var item = new FreightModels.DropdownDisplayItem();
                                    item.Id = (int)trailer;
                                    item.Name = trailer.GetDisplayName();
                                    compatibleTrailerTypes.Add(item);
                                }
                            }
                            if (jobInfoModel.DeliveryDaysList != null && jobInfoModel.DeliveryDaysList.Any())
                            {
                                var deliveryday = jobInfoModel.DeliveryDaysList.Where(t => t.DeliveryDays.HasValue && t.DeliveryDays == deliveryDay).FirstOrDefault();
                                if (deliveryday != null)
                                {
                                    IsAcceptNightDeliveries = deliveryday.IsAcceptNightDeliveries;
                                }
                            }
                            var setCompatibilityInfo = drs.FindAll(t => t.JobId == jobId).ToList();
                            if (setCompatibilityInfo != null && setCompatibilityInfo.Any())
                            {
                                setCompatibilityInfo.ForEach(t => t.TrailerTypes.AddRange(compatibleTrailerTypes));
                                setCompatibilityInfo.ForEach(t => t.IsAcceptNightDeliveries = IsAcceptNightDeliveries);
                                setCompatibilityInfo.ForEach(t => t.HoursToCoverDistance = jobInfoModel.DistanceCovered);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderRepository", "GetJobCompatibilityInfo", ex.Message, ex);
            }
        }
    }
}

