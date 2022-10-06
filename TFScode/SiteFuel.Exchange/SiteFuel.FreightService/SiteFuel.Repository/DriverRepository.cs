using MongoDB.Bson;
using MongoDB.Driver;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.FreightModels;
using SiteFuel.FreightModels.Driver;
using SiteFuel.FreightModels.ScheduleBuilder;
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
    public class DriverRepository : IDriverRepository
    {
        private readonly MdbContext mdbContext;
        public DriverRepository(MdbContext _context)
        {
            mdbContext = _context;
        }

        public async Task<StatusModel> CreateDriver(DriverObjectModel model)
        {
            var response = new StatusModel();
            var driver = model.ToEntity();
            await mdbContext.Drivers.InsertOneAsync(driver);
            var result = await AssignDriverToRegion(model);
            if (result.StatusCode == (int)Status.Failed)
            {
                response.StatusCode = (int)Status.Failed;
                return response;
            }
            response.StatusCode = (int)Status.Success;
            return response;
        }

        public async Task<StatusModel> UpdateDriver(DriverObjectModel model)
        {
            var response = new StatusModel();
            var driver = model.ToEntity();

            var updateFields = Builders<Driver>.Update
                .Set(t => t.DriverName, driver.DriverName)
                .Set(t => t.ProfilePhoto, driver.ProfilePhoto)
                .Set(t => t.LicenseNumber, driver.LicenseNumber)
                .Set(t => t.ShiftId, driver.ShiftId)
                .Set(t => t.TrailerType, driver.TrailerType)
                .Set(t => t.CardNumbers, driver.CardNumbers)
                .Set(t => t.ExpiryDate, driver.ExpiryDate)
                .Set(t => t.CompanyName, driver.CompanyName)
                .Set(t => t.LicenseTypeId, driver.LicenseTypeId)
                .Set(t => t.Regions, driver.Regions)
                .Set(t => t.IsActive, true)
                .Set(t => t.IsFilldAuthorized, driver.IsFilldAuthorized);
            var filter = Builders<Driver>.Filter.And(
                    Builders<Driver>.Filter.Where(t => t.DriverId == model.DriverId),
                    Builders<Driver>.Filter.Where(t => t.CompanyId == model.CompanyId)
                );

            if (mdbContext.Drivers.Find(filter).Any())
            {
                await mdbContext.Drivers.UpdateOneAsync(filter, updateFields);
                var result = await AssignDriverToRegion(model);
                if (result.StatusCode == (int)Status.Failed)
                {
                    response.StatusCode = (int)Status.Failed;
                    return response;
                }

            }
            else
            {
                await mdbContext.Drivers.InsertOneAsync(driver);
                var result = await AssignDriverToRegion(model);
                if (result.StatusCode == (int)Status.Failed)
                {
                    response.StatusCode = (int)Status.Failed;
                    return response;
                }
            }
            response.StatusCode = (int)Status.Success;
            return response;
        }

        public async Task<StatusModel> DeleteDriver(int driverId, int companyId)
        {
            var response = new StatusModel();
            var filter = Builders<Driver>.Filter.And(
                    Builders<Driver>.Filter.Where(t => t.DriverId == driverId),
                    Builders<Driver>.Filter.Where(t => t.CompanyId == companyId)
                );

            var updateFields = Builders<Driver>.Update
                .Set(t => t.IsActive, false)
                .Set(t => t.IsDeleted, true);

            await mdbContext.Drivers.UpdateOneAsync(filter, updateFields);
            response.StatusCode = (int)Status.Success;
            return response;
        }

        public async Task<DriverObjectModel> GetDriver(int driverId, int companyId)
        {
            DriverObjectModel response = null;
            var filter = Builders<Driver>.Filter.And(
                    Builders<Driver>.Filter.Where(t => t.DriverId == driverId),
                    Builders<Driver>.Filter.Where(t => t.CompanyId == companyId),
                    Builders<Driver>.Filter.Where(t => t.IsActive == true),
                    Builders<Driver>.Filter.Where(t => t.IsDeleted == false)
                );

            var driver = await mdbContext.Drivers.Find(filter).FirstOrDefaultAsync();
            if (driver != null)
            {
                response = driver.ToModel();
            }
            return response;
        }
        public async Task<DriverObjectModel> GetDriverById(int driverId)
        {
            DriverObjectModel response = null;
            var filter = Builders<Driver>.Filter.And(
                    Builders<Driver>.Filter.Where(t => t.DriverId == driverId),
                    Builders<Driver>.Filter.Where(t => t.IsDeleted == false)
                );

            var driver = await mdbContext.Drivers.Find(filter).FirstOrDefaultAsync();
            if (driver != null)
            {
                response = driver.ToModel();
            }
            return response;
        }


        public async Task<DriverAdditionalDetailsModel> GetDriverAdditionalDetails(int tfxDriverId)
        {
            var response = new DriverAdditionalDetailsModel();
            var drv = await mdbContext.Drivers.Find(t => t.DriverId == tfxDriverId && !t.IsDeleted)
                .Project(t => new { t.Id, t.DriverName, t.LicenseNumber })
                .FirstOrDefaultAsync();

            if (drv != null)
            {
                DateTime date = DateTime.Now.Date;
                response.Name = drv.DriverName;
                response.Id = drv.Id.ToString();
                response.License = drv.LicenseNumber;
                var trailerIds = new List<ObjectId>();
                var shiftIds = new List<ObjectId>();
                var trips = await mdbContext.ScheduleBuilders.Find(t => t.DateFilter == date
                                                                              && t.IsActive && !t.IsDeleted)
                                                                    .Project(x => new
                                                                    {
                                                                        trips = x.Trips.Where(z => z.TfxDrivers.Any(z1 => z1.Id == tfxDriverId))
                                                                                .Select(x1 => new { x1.Trailers, x1.ShiftId }).ToList(),

                                                                    })
                                                                    .ToListAsync();

                if (trips != null && trips.Count > 0)
                {
                    trips.ForEach(t =>
                    {
                        var oTrailers = t.trips.SelectMany(tp => tp.Trailers).ToList();
                        trailerIds.AddRange(oTrailers.Select(y1 => ObjectId.Parse(y1.Id)));
                        var shift = t.trips.Select(sh => sh.ShiftId).FirstOrDefault();
                        if (shift != null)
                        {
                            shiftIds.Add(ObjectId.Parse(shift));
                        }
                    });
                }
                //trailer.ForEach(y => trailerIds.AddRange(y.trips.Select(y1 => ObjectId.Parse(y1))));
                //trailer.ForEach(y => shiftIds.AddRange(y.Shift.Select(y1 => ObjectId.Parse(y1.ToString()))));
                trailerIds = trailerIds.Distinct().ToList();
                shiftIds = shiftIds.Distinct().ToList();
                if (shiftIds.Count > 0)
                    response.Shifts = await mdbContext.Shifts.Find(t => shiftIds.Contains(t.Id)).Project(t => $"{t.Name} {t.StartTime} - {t.EndTime}").ToListAsync();
                var truckRepository = new TruckRepository();
                response.Trailers = await truckRepository.GetTruckDetails(trailerIds);
            }
            return response;
        }

        public async Task<List<TrailerRetainDetails>> GetTrailerFuelReatinDetails(RetainRequets retainRequets)
        {
            var response = new List<TrailerRetainDetails>();

            DateTime FromDateFilter = DateTimeOffset.Now.Date;
            if (!string.IsNullOrWhiteSpace(retainRequets.fromDate))
            {
                FromDateFilter = Convert.ToDateTime(retainRequets.fromDate).Date;
            }
            DateTime ToDateFilter = DateTimeOffset.Now.Date;
            if (!string.IsNullOrWhiteSpace(retainRequets.toDate))
            {
                ToDateFilter = Convert.ToDateTime(retainRequets.toDate).Date;
            }
            var trailerIds = new List<ObjectId>();

            var trailers = await mdbContext.ScheduleBuilders.Find(t => t.IsActive && !t.IsDeleted && FromDateFilter <= t.DateFilter &&
                                                                                                     ToDateFilter >= t.DateFilter)
                                                            .Project(x => new
                                                            {
                                                                trips = x.Trips.Where(z => z.TfxDrivers.Any(z1 => retainRequets.driverIds.Contains(z1.Id)))
                                                                            .SelectMany(x1 => x1.Trailers.Select(x2 => x2.Id)).ToList()
                                                            })
                                                            .ToListAsync();


            trailers.ForEach(y => trailerIds.AddRange(y.trips.Select(y1 => ObjectId.Parse(y1))));
            trailerIds = trailerIds.Distinct().ToList();
            response = await new TruckRepository().GetTrailersRetainDetails(trailerIds);
            return response;
        }


        public async Task<StatusModel> AddDriverSchedule(DriverScheduleMappingViewModel model)
        {
            var response = new StatusModel();
            var driverSchedule = model.ToEntity();
            await mdbContext.DriverScheduleShiftMapping.InsertOneAsync(driverSchedule);
            response.StatusCode = (int)Status.Success;
            return response;
        }

        public async Task<StatusModel> AddTrailerSchedule(TrailerScheduleModel model)
        {
            var response = new StatusModel();
            var driverSchedule = model.ToEntity();
            await mdbContext.TrailerScheduleMapping.InsertOneAsync(driverSchedule);
            response.StatusCode = (int)Status.Success;
            return response;
        }

        public async Task<List<DriverObjectModel>> GetAllDrivers(int companyId)
        {
            List<DriverObjectModel> response = new List<DriverObjectModel>();

            var filter = Builders<Driver>.Filter.And(
                    Builders<Driver>.Filter.Where(t => t.CompanyId == companyId),
                    Builders<Driver>.Filter.Where(t => t.IsDeleted == false)
                );

            var drivers = await mdbContext.Drivers.Find(filter).ToListAsync();
            if (drivers != null && drivers.Count > 0)
            {
                drivers.ForEach(t => response.Add(t.ToModel()));
            }
            return response;
        }

        public async Task<DriverScheduleUpdateModel> UpdateDriverSchedule(List<DriverScheduleMappingViewModel> driverScheduleMappingViewModels)
        {
            var response = new DriverScheduleUpdateModel();
            response.DsbScheduleBuilderInfo = new List<ScheduleBuilderViewModel>();
            List<DriverScheduleInfoModel> driverRemoveScheduleInfo = new List<DriverScheduleInfoModel>();
            try
            {
                if (driverScheduleMappingViewModels != null && driverScheduleMappingViewModels.Count > 0)
                {
                    var IsDsbDriverSchedule = driverScheduleMappingViewModels.FirstOrDefault().IsDsbDriverSchedule;
                    foreach (var model in driverScheduleMappingViewModels)
                    {
                        var scheduleDriver = model.ToEntity();
                        if (IsDsbDriverSchedule)
                        {
                            driverRemoveScheduleInfo = await GetRemoveDriverScheduleInfo(scheduleDriver.Id, model.SelectedDate);
                        }
                        var updateFields = Builders<DriverScheduleShiftMapping>.Update
                            .Set(t => t.DriverScheduleList, scheduleDriver.DriverScheduleList);

                        var filter = Builders<DriverScheduleShiftMapping>.Filter.And(
                       Builders<DriverScheduleShiftMapping>.Filter.Where(x => x.Id == scheduleDriver.Id));

                        await mdbContext.DriverScheduleShiftMapping.UpdateOneAsync(filter, updateFields);
                        response.StatusCode = (int)Status.Success;

                        if (IsDsbDriverSchedule && driverRemoveScheduleInfo.Any())
                        {
                            var resetDriver_Response = await RemoveDriverFromDsbSchdule(driverRemoveScheduleInfo.FirstOrDefault().ShiftId,
                                        driverRemoveScheduleInfo.FirstOrDefault().DriverId,
                                        driverRemoveScheduleInfo.FirstOrDefault().ScheduleDate);
                            if (resetDriver_Response.Any())
                            {
                                response.DsbScheduleBuilderInfo.AddRange(resetDriver_Response);
                            }
                        }
                    }

                    response.StatusCode = (int)Status.Success;
                }
                else
                {
                    response.StatusCode = (int)Status.Failed;
                }
            }
            catch (Exception)
            {
                response.StatusCode = (int)Status.Failed;
            }

            return response;

        }


        public async Task<DriverScheduleUpdateModel> DeleteAllSchedulesOfDriver(List<DriverScheduleMappingViewModel> driverScheduleMappingViewModels)
        {
            var response = new DriverScheduleUpdateModel();
            List<DriverScheduleInfoModel> driverRemoveScheduleInfo = new List<DriverScheduleInfoModel>();
            try
            {
                if (driverScheduleMappingViewModels != null && driverScheduleMappingViewModels.Count > 0)
                {
                    var IsDsbDriverSchedule = driverScheduleMappingViewModels.FirstOrDefault().IsDsbDriverSchedule;
                    foreach (var model in driverScheduleMappingViewModels)
                    {
                        var scheduleDriver = model.ToEntity();
                        if (IsDsbDriverSchedule)
                        {
                            driverRemoveScheduleInfo = await GetRemoveDriverScheduleInfo(scheduleDriver.Id, model.SelectedDate);
                        }
                        var updateFields = Builders<DriverScheduleShiftMapping>.Update
                            .Set(t => t.DriverScheduleList, scheduleDriver.DriverScheduleList);

                        var filter = Builders<DriverScheduleShiftMapping>.Filter.And(
                       Builders<DriverScheduleShiftMapping>.Filter.Where(x => x.Id == scheduleDriver.Id));

                        await mdbContext.DriverScheduleShiftMapping.UpdateOneAsync(filter, updateFields);
                        response.StatusCode = (int)Status.Success;
                        if (IsDsbDriverSchedule && driverRemoveScheduleInfo.Any())
                        {
                            response.DsbScheduleBuilderInfo = await RemoveDriverFromDsbSchdule(driverRemoveScheduleInfo.FirstOrDefault().ShiftId,
                                        driverRemoveScheduleInfo.FirstOrDefault().DriverId,
                                        driverRemoveScheduleInfo.FirstOrDefault().ScheduleDate);
                        }
                    }
                }
                else
                {
                    response.StatusCode = (int)Status.Failed;
                }
            }
            catch (Exception)
            {
                response.StatusCode = (int)Status.Failed;
            }
            return response;
        }

        //Driver Schedule shift
        public List<DriverScheduleMappingViewModel> GetShiftByDrivers(string driverList, int scheduleType)
        {
            var response = new List<DriverScheduleMappingViewModel>();
            var driverSchedule = mdbContext.DriverScheduleShiftMapping.AsQueryable();
            var shifts = mdbContext.Shifts.AsQueryable();
            var driver = mdbContext.Drivers.AsQueryable();
            //convert string into list
            if (string.IsNullOrWhiteSpace(driverList))
                return response;
            var driversList = driverList.Split(',').Select(int.Parse).ToList();
            var list = driverSchedule.Where(w => w.IsActive == true && driversList.Contains(w.DriverId)).ToList().Select(s => new DriverScheduleShiftMapping
            {
                Id = s.Id,
                DriverId = s.DriverId,
                IsActive = s.IsActive,
                IsUnplanedSchedule = s.IsUnplanedSchedule,
                DriverScheduleList = s.DriverScheduleList.Select(x => new DriverSchedule()
                {
                    Id = x.Id,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    IsActive = x.IsActive,
                    RepeatDayList = x.RepeatDayList,
                    ShiftId = x.ShiftId,
                    Description = x.Description,
                    RepeatEveryDay = x.RepeatEveryDay,
                    TypeId = x.TypeId,

                }).ToList(),
            }).ToList();
            if (scheduleType == (int)DriverScheduleType.Planned || scheduleType == (int)DriverScheduleType.UnPlanned)
            {
                list = list.Where(w => w.IsUnplanedSchedule == (scheduleType == (int)DriverScheduleType.Planned ? false : true)).ToList();
            }
            foreach (var item in list)
            {
                var model = new DriverScheduleMappingViewModel()
                { Id = item.Id.ToString(), DriverId = item.DriverId, IsActive = item.IsActive, IsUnplanedSchedule = item.IsUnplanedSchedule, DriverName = driver.Where(d => d.DriverId == item.DriverId).Select(dr => dr.DriverName).FirstOrDefault() };
                var scheduleList = new List<DriverScheduleViewModel>();
                foreach (var schedule in item.DriverScheduleList)
                {
                    var driverSch = new DriverScheduleViewModel()
                    {
                        Id = schedule.Id.ToString(),
                        StartDate = schedule.StartDate.DateTime,
                        EndDate = schedule.EndDate.DateTime,
                        IsActive = schedule.IsActive,
                        StrEndDate = schedule.EndDate.DateTime.ToString(Resource.constFormatDate),
                        StrStartDate = schedule.StartDate.DateTime.ToString(Resource.constFormatDate),
                        RepeatDayStringList = schedule.RepeatDayList.Select(t => t.DateTime.ToString(Resource.constFormatDate)).ToList(),
                        RepeatDayList = schedule.RepeatDayList.Select(t => t.DateTime).ToList(),
                        ShiftId = schedule.ShiftId.ToString(),
                        Description = schedule.Description,
                        ShiftDetail = shifts.Where(w => w.Id == schedule.ShiftId).Select(t => new ShiftViewModel()
                        {
                            Id = schedule.ShiftId.ToString(),
                            StartTime = t.StartTime,
                            EndTime = t.EndTime,
                            IsActive = t.IsActive,
                            //   RegionName = regionList.Where(f => f.Id == t.RegionId).FirstOrDefault().Name,
                        }).FirstOrDefault(),
                        RepeatEveryDay = schedule.RepeatEveryDay,
                        TypeId = schedule.TypeId,
                    };
                    scheduleList.Add(driverSch);
                }
                model.ScheduleList = scheduleList;
                if (model.ScheduleList != null)
                    response.Add(model);
            }
            return response;
        }

        public async Task<StatusModel> AssignDriverToRegion(DriverObjectModel driverToUpdate)
        {
            var response = new StatusModel(Status.Failed);
            try
            {
                {
                    string regionIdUpdate = null;
                    if (driverToUpdate.Regions != null)
                        regionIdUpdate = driverToUpdate.Regions.FirstOrDefault();
                    //Find the region with given driver and company
                    var regionId = await GetRegionIdForDriver(driverToUpdate.DriverId, driverToUpdate.CompanyId);
                    if (regionId == regionIdUpdate)// No Assignment
                    {
                        response.StatusCode = (int)Status.Success;
                        return response;
                    }
                    //Implies No Region Assigned To Driver. Current Assignment will be removed
                    if ((regionIdUpdate == null || regionIdUpdate == string.Empty) && regionId != null)
                    {
                        var objectId = ObjectId.Parse(regionId);
                        var filter = Builders<Region>.Filter.And(
                                Builders<Region>.Filter.Where(x => x.Id == objectId),
                                Builders<Region>.Filter.Where(x => x.TfxCompanyId == driverToUpdate.CompanyId),
                                Builders<Region>.Filter.Where(x => x.IsActive),
                                Builders<Region>.Filter.Where(x => !x.IsDeleted)
                            );
                        var update = Builders<Region>.Update.PullFilter(p => p.TfxDrivers, f => f.Id == driverToUpdate.DriverId);


                        await mdbContext.Regions.FindOneAndUpdateAsync(filter, update);
                        response.StatusCode = (int)Status.Success;
                        response.StatusMessage = "Driver " + driverToUpdate.DriverName + " Succesfully Removed from Region";
                        return response;
                    }
                    if ((regionId != regionIdUpdate) && regionId != null)//Implies new Region is selected for given job 
                    {
                        //First Remove the given driver from existing region as one driver can belong to one region only 
                        var objectId = ObjectId.Parse(regionId);
                        var filter = Builders<Region>.Filter.And(
                                Builders<Region>.Filter.Where(x => x.Id == objectId),
                                Builders<Region>.Filter.Where(x => x.TfxCompanyId == driverToUpdate.CompanyId),
                                Builders<Region>.Filter.Where(x => x.IsActive),
                                Builders<Region>.Filter.Where(x => !x.IsDeleted)
                            );
                        var update = Builders<Region>.Update.PullFilter(p => p.TfxDrivers, f => f.Id == driverToUpdate.DriverId);
                        await mdbContext.Regions.FindOneAndUpdateAsync(filter, update);
                        return await AddDriverToRegion(driverToUpdate);
                    }
                    else
                    {
                        response = await AddDriverToRegion(driverToUpdate);//First time driver Assignment
                    }

                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DriverRepository", "AssignDriverToRegion", ex.Message, ex);
                response.StatusCode = (int)Status.Failed;
                response.StatusMessage = Status.Failed.ToString();
            }
            return response;
        }

        public async Task<string> GetRegionIdForDriver(int driverId, int companyId)
        {
            string regionId = string.Empty;
            try
            {
                var region = await (mdbContext.Regions.FindAsync(t => t.TfxCompanyId == companyId &&
                                                                t.IsActive && !t.IsDeleted &&
                                                                t.TfxDrivers.Any(t1 => t1.Id == driverId)));

                if (region != null)
                {
                    regionId = region.ToList().Select(t => t.Id.ToString()).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {

                LogManager.Logger.WriteException("DriverRepository", "GetRegionIdForDriver", ex.Message, ex);
            }
            return regionId;

        }

        public async Task<StatusModel> AddDriverToRegion(DriverObjectModel driverToUpdate)
        {
            var response = new StatusModel(Status.Failed);
            try
            {
                var regionIdToUpdate = driverToUpdate.Regions.FirstOrDefault();
                MdbDataAccess.Collections.DropdownDisplayItem TfxDriver = new MdbDataAccess.Collections.DropdownDisplayItem()
                {
                    Id = driverToUpdate.DriverId,
                    Code = null,
                    Name = driverToUpdate.DriverName
                };
                var filter = Builders<Region>.Filter.And(
                               Builders<Region>.Filter.Where(x => x.Id == ObjectId.Parse(regionIdToUpdate)),
                                Builders<Region>.Filter.Where(x => x.TfxCompanyId == driverToUpdate.CompanyId),
                                   Builders<Region>.Filter.Where(x => x.IsActive),
                                   Builders<Region>.Filter.Where(x => !x.IsDeleted)
                               );

                var update = Builders<Region>.Update.Push("TfxDrivers", TfxDriver);
                await mdbContext.Regions.FindOneAndUpdateAsync(filter, update);

                response.StatusCode = (int)Status.Success;
                response.StatusMessage = "Driver " + driverToUpdate.DriverName + " assigned successfully to Region";
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DriverRepository", "AddDriverToRegion", ex.Message, ex);

            }
            return response;
        }
        private async Task<List<DriverScheduleInfoModel>> GetRemoveDriverScheduleInfo(ObjectId scheduleId, string selectedDate)
        {
            List<DriverScheduleInfoModel> response = new List<DriverScheduleInfoModel>();
            var driverScheduleList = await mdbContext.DriverScheduleShiftMapping.Find(top => top.Id == scheduleId).FirstOrDefaultAsync();
            if (driverScheduleList != null)
            {
                DateTime dateFilter = DateTimeOffset.Now.Date;
                if (!string.IsNullOrWhiteSpace(selectedDate))
                {
                    dateFilter = Convert.ToDateTime(selectedDate).Date;
                }
                response.Add(new DriverScheduleInfoModel
                {
                    DriverId = driverScheduleList.DriverId,
                    ShiftId = driverScheduleList.DriverScheduleList.FirstOrDefault().ShiftId.ToString(),
                    ScheduleDate = dateFilter
                });
            }
            return response;
        }
        #region ResetDriverSchedule
        public async Task<List<ScheduleBuilderViewModel>> RemoveDriverFromDsbSchdule(string shiftId, int driverId, DateTime date)
        {
            ScheduleBuilderRepository scheduleBuilderRepository = new ScheduleBuilderRepository(mdbContext);
            var scheduleBuilderInfo = await scheduleBuilderRepository.RemoveDriverFromDsbSchdule(shiftId, driverId, date);
            return scheduleBuilderInfo;
        }
        #endregion
    }
}
