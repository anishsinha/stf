using MongoDB.Bson;
using MongoDB.Driver;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
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
    public class ShiftRepository : IShiftRepository
    {
        private readonly MdbContext mdbContext;
        public ShiftRepository()
        {
            if (mdbContext == null)
            {
                mdbContext = new MdbContext();
            }
        }
        public ShiftRepository(MdbContext _context)
        {
            mdbContext = _context;
        }
        public async Task<bool> DeleteAllShifts()
        {
            var filter = MongoDB.Driver.FilterDefinition<Shift>.Empty;
            await mdbContext.Shifts.DeleteManyAsync(filter);
            return true;
        }

        public async Task<ShiftResponseModel> CreateShift(ShiftViewModel model)
        {
            var response = new ShiftResponseModel();
            var shift = model.ToEntity();
            await mdbContext.Shifts.InsertOneAsync(shift);
            response.ShiftId = shift.Id.ToString();
            response.StatusCode = (int)Status.Success;
            return response;
        }

        public async Task<List<ShiftViewModel>> CreateShifts(List<ShiftViewModel> models)
        {
            var response = new List<ShiftViewModel>();
            var shifts = models.Select(t => t.ToEntity()).ToList();
            await mdbContext.Shifts.InsertManyAsync(shifts);
            response = shifts.Select(t => t.ToModel()).ToList();
            return response;
        }

        public async Task<ShiftResponseModel> UpdateShift(ShiftViewModel model)
        {
            var response = new ShiftResponseModel();
            var shift = model.ToEntity();
            var updateFields = Builders<Shift>.Update
                .Set(t => t.Name, shift.Name)
                .Set(t => t.StartTime, shift.StartTime)
                .Set(t => t.EndTime, shift.EndTime)
                .Set(t => t.IsActive, shift.IsActive)
                .Set(t => t.IsDeleted, shift.IsDeleted);

            var filter = Builders<Shift>.Filter.And(
                    Builders<Shift>.Filter.Where(x => x.Id == shift.Id),
                    Builders<Shift>.Filter.Where(x => x.CompanyId == shift.CompanyId)
                );

            if (shift.Id == ObjectId.Empty)
                await mdbContext.Shifts.InsertOneAsync(shift);
            else
                await mdbContext.Shifts.UpdateOneAsync(filter, updateFields);

            response.StatusCode = (int)Status.Success;
            return response;
        }

        public async Task<ShiftResponseModel> DeleteShift(int companyId, string shiftId)
        {
            var response = new ShiftResponseModel();
            var updateFields = Builders<Shift>.Update
                .Set(t => t.IsActive, false)
                .Set(t => t.IsDeleted, true);

            var objectId = ObjectId.Parse(shiftId);
            var filter = Builders<Shift>.Filter.And(
                    Builders<Shift>.Filter.Where(x => x.Id == objectId),
                    Builders<Shift>.Filter.Where(x => x.IsActive == false),
                    Builders<Shift>.Filter.Where(x => x.IsDeleted == true)
                );

            await mdbContext.Shifts.UpdateOneAsync(filter, updateFields);
            response.StatusCode = (int)Status.Success;
            return response;
        }

        public async Task<ShiftResponseModel> DeleteShifts(int companyId, string regionId)
        {
            var response = new ShiftResponseModel();
            await mdbContext.Shifts.DeleteManyAsync(t => t.RegionId == ObjectId.Parse(regionId));
            response.StatusCode = (int)Status.Success;
            return response;
        }

        public async Task<ShiftViewModel> GetShift(int companyId, string shiftId)
        {
            var objectId = ObjectId.Parse(shiftId);
            var filter = Builders<Shift>.Filter.And(
                    Builders<Shift>.Filter.Where(t => t.Id == objectId),
                   Builders<Shift>.Filter.Where(t => t.CompanyId == companyId),
                   Builders<Shift>.Filter.Where(t => t.IsActive == true),
                   Builders<Shift>.Filter.Where(t => t.IsDeleted == false)
               );

            var shift = await mdbContext.Shifts.Find(filter).FirstOrDefaultAsync();
            return shift?.ToModel();
        }

        public async Task<List<ShiftViewModel>> GetShifts(int companyId)
        {
            var filter = Builders<Shift>.Filter.And(
                    Builders<Shift>.Filter.Where(t => t.CompanyId == companyId),
                    Builders<Shift>.Filter.Where(t => t.IsActive == true),
                    Builders<Shift>.Filter.Where(t => t.IsDeleted == false)
                );

            var shifts = await mdbContext.Shifts.Find(filter).ToListAsync();
            var response = shifts.Select(t => t.ToModel()).OrderBy(t => t.StartTime).ToList();

            return response;
        }

        public async Task<List<ShiftViewModel>> GetShifts(int companyId, string regionId)
        {
            var objectId = ObjectId.Parse(regionId);

            var regionfilter = Builders<Shift>.Filter.And(
                    Builders<Shift>.Filter.Where(t => t.RegionId == objectId),
                    //Builders<Shift>.Filter.Where(t => t.CompanyId == companyId),
                    Builders<Shift>.Filter.Where(t => t.IsActive == true),
                    Builders<Shift>.Filter.Where(t => t.IsDeleted == false)
                );

            var shifts = await mdbContext.Shifts.Find(regionfilter).ToListAsync();
            var response = shifts.Select(t => t.ToModel()).OrderByDescending(t => t.CreatedOn).ToList();

            return response;
        }

        public List<DropdownDisplayExtendedItem> GetShiftDdl(int companyId)
        {
            var regions = mdbContext.Regions.AsQueryable();
            var shifts = mdbContext.Shifts.AsQueryable();

            var shiftList = regions.Join(shifts, r => r.Id, s => s.RegionId, (r, s) => new { Region = r, Shift = s })
                            .Where(t => t.Region.TfxCompanyId == companyId && t.Region.IsActive && !t.Region.IsDeleted && t.Shift.IsActive && !t.Shift.IsDeleted)
                            .ToList().Select(t => new DropdownDisplayExtendedItem()
                            {
                                Code = t.Shift.Id.ToString(),
                                Name = $"{t.Region.Name}: {t.Shift.Name} {t.Shift.StartTime} - {t.Shift.EndTime}"
                            }).ToList();
            if (shiftList == null)
            {
                shiftList = new List<DropdownDisplayExtendedItem>();
            }
            return shiftList;
        }
        public async Task<List<ShiftViewModel>> GetDriversShifts(int companyId, string regionId, string SelectedDate, bool IsDsbDriverSchedule)
        {
            List<ShiftViewModel> shiftViews = new List<ShiftViewModel>();
            DateTime dateFilter = DateTimeOffset.Now.Date;
            if (!string.IsNullOrWhiteSpace(SelectedDate))
            {
                dateFilter = Convert.ToDateTime(SelectedDate).Date;
            }
            var objectId = ObjectId.Parse(regionId);
            //get region drivers details.
            var regionDrivers = await mdbContext.Regions.Find(t => t.Id == objectId && t.IsActive && !t.IsDeleted).Project(t => new { t.TfxDrivers }).FirstOrDefaultAsync();
            //get region shift details.
            var shifts = await mdbContext.Shifts.Find(t => t.RegionId == objectId && t.IsActive && !t.IsDeleted).ToListAsync();
            if (IsDsbDriverSchedule == true)
            {
                if (regionDrivers != null && shifts.Any() && regionDrivers.TfxDrivers.Any())
                {
                    //get drivers ids collection.
                    var driverInfo = regionDrivers.TfxDrivers.Select(x => x.Id).ToList();
                    //get driver schedule-shift information from DriverScheduleShiftMapping mongo db collection.
                    var driverScheduleInfo = await mdbContext.DriverScheduleShiftMapping.Find(x => driverInfo.Contains(x.DriverId) && x.IsActive && !x.IsDeleted).Project(top => new { top.DriverId, top.DriverScheduleList }).ToListAsync();
                    foreach (var item in driverScheduleInfo)
                    {
                        //get driver schedule information with date collections.
                        var repeatDayList = item.DriverScheduleList.Where(x => x.RepeatDayList != null && x.IsActive).SelectMany(top => top.RepeatDayList.Select(x => x.Date)).ToList();
                        //find the driver schedule for dsb schedule date.
                        var driverDayIndex = repeatDayList.FindIndex(x => x == dateFilter);
                        if (driverDayIndex >= 0)
                        {
                            var driverDayShift = item.DriverScheduleList.Where(x => x.RepeatDayList != null && x.IsActive).FirstOrDefault();
                            if (driverDayShift != null)
                            {
                                //verify that shift exists in region shift information or not.
                                var shiftInfo = shifts.FirstOrDefault(x => x.Id == driverDayShift.ShiftId);
                                if (shiftInfo != null)
                                {
                                    var shiftIndex = shiftViews.FindIndex(x => x.Id == shiftInfo.Id.ToString());
                                    if (shiftIndex == -1)
                                    {
                                        shiftViews.Add(shiftInfo.ToModel());
                                    }
                                }
                            }
                        }
                    }

                    shiftViews = shiftViews.OrderByDescending(t => t.CreatedOn).ToList();
                }
            }
            else
            {
                shifts.ForEach(x =>
                {
                    shiftViews.Add(x.ToModel());
                });
                shiftViews = shiftViews.OrderByDescending(t => t.CreatedOn).ToList();
            }
            return shiftViews;
        }
    }
}
