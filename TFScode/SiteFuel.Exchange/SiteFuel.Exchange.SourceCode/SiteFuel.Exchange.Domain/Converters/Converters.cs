using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public static class MoneyConverter
    {
        public static decimal GetBaseAmount(Currency displayCurrency, decimal displayAmount, decimal xRate)
        {
            Money money = new Money(displayCurrency, displayAmount, xRate);
            var response = money.BaseAmount;
            return response;
        }
    }

    public static class VolumeConverter
    {
        public static decimal GetBaseQuantity(UoM displayUoM, decimal displayQuantity)
        {
            Quantity quantity = new Quantity(displayUoM, displayQuantity);
            var response = quantity.BaseQuantity;
            return response;
        }
    }

    public static class ScheduleBuilderConverter
    {
        public static ScheduleBuilderViewModel ConvertToEntity(SbDriverViewModel viewModel)
        {
            ScheduleBuilderViewModel sbModel = new ScheduleBuilderViewModel()
            {
                Id = viewModel.Id,
                CompanyId = viewModel.CompanyId,
                RegionId = viewModel.RegionId,
                ObjectFilter = viewModel.ObjectFilter,
                RegionFilter = viewModel.RegionFilter,
                DateFilter = viewModel.DateFilter,
                Date = viewModel.Date,
                TimeStamp = viewModel.TimeStamp,
                Status = viewModel.Status,
                UserId = viewModel.UserId,
                DeletedTripId = viewModel.DeletedTripId,
                DeletedGroupId = viewModel.DeletedGroupId,
                WindowMode = viewModel.WindowMode,
                ToggleRequestMode = viewModel.ToggleRequestMode,
                IsLoadReset = viewModel.IsLoadReset,
                DeletedDriverScheduleMappingId = viewModel.DeletedDriverScheduleMappingId,
                isCreateSchedule = viewModel.isCreateSchedule,
            };
            SetTrips(viewModel, sbModel);
            SetTrailers(viewModel, sbModel);
            return sbModel;
        }

        public static ScheduleBuilderViewModel ConvertToEntity(SbTrailerViewModel viewModel)
        {
            ScheduleBuilderViewModel sbModel = new ScheduleBuilderViewModel()
            {
                Id = viewModel.Id,
                CompanyId = viewModel.CompanyId,
                RegionId = viewModel.RegionId,
                ObjectFilter = viewModel.ObjectFilter,
                RegionFilter = viewModel.RegionFilter,
                DateFilter = viewModel.DateFilter,
                Date = viewModel.Date,
                TimeStamp = viewModel.TimeStamp,
                Status = viewModel.Status,
                UserId = viewModel.UserId,
                DeletedTripId = viewModel.DeletedTripId,
                DeletedGroupId = viewModel.DeletedGroupId,
                WindowMode = viewModel.WindowMode,
                ToggleRequestMode = viewModel.ToggleRequestMode,
                DeletedDriverScheduleMappingId = viewModel.DeletedDriverScheduleMappingId,
                isCreateSchedule = viewModel.isCreateSchedule,
            };
            SetTrips(viewModel, sbModel);
            SetTrailers(viewModel, sbModel);
            return sbModel;
        }

        public static SbDriverViewModel ConvertToDriverViewModel(ScheduleBuilderViewModel sbModel)
        {
            SbDriverViewModel driverViewModel = new SbDriverViewModel()
            {
                Id = sbModel.Id,
                CompanyId = sbModel.CompanyId,
                RegionId = sbModel.RegionId,
                ObjectFilter = sbModel.ObjectFilter,
                DSBFilter = sbModel.DSBFilter,
                RegionFilter = sbModel.RegionFilter,
                DateFilter = sbModel.DateFilter,
                Date = sbModel.Date,
                TimeStamp = sbModel.TimeStamp,
                Status = sbModel.Status,
                UserId = sbModel.UserId,
                DeletedTripId = sbModel.DeletedTripId,
                DeletedGroupId = sbModel.DeletedGroupId,
                WindowMode = sbModel.WindowMode,
                ToggleRequestMode = sbModel.ToggleRequestMode,
                IsNoDriverShiftFound = sbModel.IsNoDriverShiftFound,
                IsDsbDriverSchedule = sbModel.IsDsbDriverSchedule
            };
            GetShifts(sbModel, driverViewModel);
            return driverViewModel;
        }

        public static SbTrailerViewModel ConvertToTrailerViewModel(ScheduleBuilderViewModel sbModel)
        {
            SbTrailerViewModel trailerViewModel = new SbTrailerViewModel()
            {
                Id = sbModel.Id,
                CompanyId = sbModel.CompanyId,
                RegionId = sbModel.RegionId,
                ObjectFilter = sbModel.ObjectFilter,
                RegionFilter = sbModel.RegionFilter,
                DateFilter = sbModel.DateFilter,
                Date = sbModel.Date,
                TimeStamp = sbModel.TimeStamp,
                Status = sbModel.Status,
                UserId = sbModel.UserId,
                DeletedTripId = sbModel.DeletedTripId,
                DeletedGroupId = sbModel.DeletedGroupId,
                WindowMode = sbModel.WindowMode,
                ToggleRequestMode = sbModel.ToggleRequestMode,
            };
            trailerViewModel.Shifts = sbModel.Shifts;
            GetTrailers(sbModel, trailerViewModel);
            return trailerViewModel;
        }

        private static void GetTrailers(ScheduleBuilderViewModel sbModel, SbTrailerViewModel trailerViewModel)
        {
            foreach (var trailer in sbModel.Trailers)
            {
                TrailerViewModel trailerModel = new TrailerViewModel() { Id = trailer.Id, Compartments = trailer.Compartments, TrailerType = trailer.TrailerType, TrailerId = trailer.TrailerId };
                var trailerLoads = sbModel.Trips.Where(t => t.Trailers.Any(t1 => t1.Id == trailer.Id)).ToList();
                if (trailerLoads != null && trailerLoads.Any())
                {
                    var tripTimings = trailerLoads.Where(t => t.StartDate != null && t.StartDate != "" && t.StartTime != null && t.StartTime != "").Select(t => new { StartTime = Convert.ToDateTime(t.StartDate).Add(DateTime.Parse(t.StartTime).TimeOfDay), EndTime = Convert.ToDateTime(t.StartDate).Add(DateTime.Parse(t.EndTime).TimeOfDay) });
                    trailerModel.StartTime = tripTimings.Select(t => t.StartTime.ToShortTimeString()).FirstOrDefault();
                    trailerModel.EndTime = tripTimings.Select(t => t.EndTime.ToShortTimeString()).LastOrDefault();
                    var shifts = trailerLoads.GroupBy(t => t.ShiftId).OrderBy(t => t.Key).ToList();
                    foreach (var shift in shifts)
                    {
                        var shiftLoads = shift.OrderBy(t => t.TrailerColIndex);
                        var shiftDetail = shiftLoads.FirstOrDefault();
                        if (shiftDetail != null)
                        {
                            var shiftSlotPeriod = sbModel.Shifts.Where(t => t.Id == shiftDetail.ShiftId).Select(t => t.SlotPeriod).FirstOrDefault();
                            TrailerShiftModel trailerShiftModel = new TrailerShiftModel()
                            {
                                ShiftId = shiftDetail.ShiftId,
                                StartTime = shiftDetail.ShiftStartTime,
                                EndTime = shiftDetail.ShiftEndTime,
                                SlotPeriod = shiftSlotPeriod
                            };
                            foreach (var shiftLoad in shiftLoads)
                            {
                                InitializePickupLocation(shiftLoad);
                                trailerShiftModel.Trips.Add(shiftLoad);
                            }
                            trailerModel.Shifts.Add(trailerShiftModel);
                        }
                    }
                }
                trailerViewModel.Trailers.Add(trailerModel);
            }
        }

        public static void InitializePickupLocation(TripViewModel shiftLoad)
        {
            IntializeCommonBadgeNumberStatus(shiftLoad);
            IntializeBadgeNumber(shiftLoad);
            foreach (var dr in shiftLoad.DeliveryRequests)
            {
                if (shiftLoad.IsCommonPickup || dr.Terminal == null || dr.PickupLocationType == PickupLocationType.BulkPlant)
                {
                    dr.Terminal = new DropdownDisplayItem();
                }
                if (shiftLoad.IsCommonPickup || dr.BulkPlant == null || dr.PickupLocationType != PickupLocationType.BulkPlant)
                {
                    dr.BulkPlant = new DropAddressViewModel();
                }

                dr.GetDeliveryReqClassName(shiftLoad.DeliveryRequests);
            }
            if (shiftLoad.DeliveryRequests.Any() && (shiftLoad.DeliveryRequests.All(t => t.StatusClassId == 4)))
            {
                shiftLoad.IsEditable = false;
            }
            else
            {
                shiftLoad.IsEditable = true;
            }
            if (!shiftLoad.IsCommonPickup || shiftLoad.Terminal == null)
            {
                shiftLoad.Terminal = new DropdownDisplayItem();
            }
            if (!shiftLoad.IsCommonPickup || shiftLoad.BulkPlant == null)
            {
                shiftLoad.BulkPlant = new DropAddressViewModel();
            }
        }

        private static void IntializeCommonBadgeNumberStatus(TripViewModel shiftLoad)
        {
            if (shiftLoad.DeliveryRequests.Where(top => top.IsCommonBadge == true).Any())
            {
                shiftLoad.IsCommonBadge = true;
            }
            else
            {
                shiftLoad.IsCommonBadge = false;
            }
        }
        /// <summary>
        /// IntializeBadgeNumber
        /// </summary>
        /// <param name="shiftLoad"></param>
        private static void IntializeBadgeNumber(TripViewModel shiftLoad)
        {
            if (shiftLoad.DeliveryRequests != null)
            {
                var commonBadge = shiftLoad.DeliveryRequests.Where(top => top.IsCommonBadge == true).FirstOrDefault();
                if (commonBadge != null)
                {
                    shiftLoad.BadgeNo1 = commonBadge.BadgeNo1;
                    shiftLoad.BadgeNo2 = commonBadge.BadgeNo2;
                    shiftLoad.BadgeNo3 = commonBadge.BadgeNo3;
                    //shiftLoad.RouteInfo = commonBadge.DispactherNote;
                    shiftLoad.IsCommonBadge = true;
                }
                else
                {
                    shiftLoad.BadgeNo1 = string.Empty;
                    shiftLoad.BadgeNo2 = string.Empty;
                    shiftLoad.BadgeNo3 = string.Empty;
                    // shiftLoad.RouteInfo = string.Empty;
                }
            }
        }

        private static void GetShifts(ScheduleBuilderViewModel sbModel, SbDriverViewModel driverViewModel)
        {
            foreach (var shift in sbModel.Shifts)
            {
                bool isShiftCollapsed = sbModel.Trips.Where(t => t.ShiftId == shift.Id).All(t => t.IsShiftCollapsed);
                ScheduleShiftViewModel shiftModel = new ScheduleShiftViewModel()
                {
                    Id = shift.Id,
                    StartTime = shift.StartTime,
                    EndTime = shift.EndTime,
                    SlotPeriod = shift.SlotPeriod,
                    IsCollapsed = isShiftCollapsed,
                    OrderNo = shift.OrderNo
                };
                var schedules = sbModel.Trips.Where(t => t.ShiftId == shift.Id).GroupBy(t => t.DriverRowIndex).ToList();
                foreach (var scheduleGroup in schedules)
                {
                    var schedule = scheduleGroup.FirstOrDefault();
                    if (schedule != null)
                    {
                        var drivers = scheduleGroup.SelectMany(t => t.Drivers).GroupBy(t => t.Id).Select(t => t.FirstOrDefault()).ToList();
                        var trailers = scheduleGroup.SelectMany(t => t.Trailers).GroupBy(t => t.Id).Select(t => t.FirstOrDefault()).ToList();
                        DriverModel driverModel = new DriverModel() { Drivers = drivers, Trailers = trailers, DriverRowIndex = schedule.DriverRowIndex.Value };
                        var trips = scheduleGroup.OrderBy(t => t.DriverColIndex).ToList();
                        foreach (var trip in trips)
                        {
                            InitializePickupLocation(trip);
                            InitializeBadgeNumberForDeliveryRequests(trip);
                            trip.DeliveryRequests = trip.DeliveryRequests.OrderBy(x => x.DispatcherDragDropSequence).ToList();
                            driverModel.Trips.Add(trip);
                        }
                        if (scheduleGroup.Any(t => t.DeliveryGroupStatus == DeliveryGroupStatus.Published || t.DeliveryGroupPrevStatus == DeliveryGroupStatus.Published))
                        {
                            driverModel.AllowDriverChange = false;
                        }
                        else if (scheduleGroup.SelectMany(t => t.DeliveryRequests.Select(t1 => new { t1.PreLoadedFor, t1.PostLoadedFor })).Any(t => (t.PreLoadedFor != null
                                && t.PreLoadedFor.Trim() != "") || (t.PostLoadedFor != null && t.PostLoadedFor.Trim() != "")))
                        {
                            driverModel.AllowDriverChange = false;
                        }
                        IntializeDsbLoadQueueInfo(sbModel, shiftModel, schedule, driverModel);
                        shiftModel.Schedules.Add(driverModel);
                        if (trips.All(x => x.IsDriverScheduleExists == false))
                        {
                            driverModel.IsDriverScheduleExists = false;
                        }

                        if (trips.All(x => x.IsIncludeAllRegionDriver == true))
                        {
                            driverModel.IsIncludeAllRegionDriver = true;
                        }
                    }

                }
                driverViewModel.Shifts.Add(shiftModel);
            }
            driverViewModel.Shifts = driverViewModel.Shifts.OrderBy(x => x.OrderNo).ToList();
        }

        //verfy the column(s) collapsed or not(Dsb LoadQueue).
        private static void IntializeDsbLoadQueueInfo(ScheduleBuilderViewModel sbModel, ScheduleShiftViewModel shiftModel, TripViewModel schedule, DriverModel driverModel)
        {
            if (sbModel != null && sbModel.DsbLoadQueueModel.Any())
            {
                var dsbLoadQueueInfo = sbModel.DsbLoadQueueModel.FirstOrDefault(top => top.ShiftId == schedule.ShiftId && top.DriverRowIndex == schedule.DriverRowIndex);
                if (dsbLoadQueueInfo != null)
                {
                    driverModel.IsLoadQueueCollapsed = true;
                    driverModel.LoadQueueId = dsbLoadQueueInfo.Id;
                }
            }
        }

        public static void InitializeBadgeNumberForDeliveryRequests(TripViewModel trip)
        {
            if (trip.DeliveryRequests != null)
            {
                var commonBadge = trip.DeliveryRequests.Where(top => top.IsCommonBadge == true).FirstOrDefault();
                if (commonBadge != null)
                {
                    trip.BadgeNo1 = commonBadge.BadgeNo1;
                    trip.BadgeNo2 = commonBadge.BadgeNo2;
                    trip.BadgeNo3 = commonBadge.BadgeNo3;
                    //trip.RouteInfo = commonBadge.DispactherNote;
                }
                else
                {
                    trip.BadgeNo1 = string.Empty;
                    trip.BadgeNo2 = string.Empty;
                    trip.BadgeNo3 = string.Empty;
                    //trip.RouteInfo = string.Empty;
                }
            }
        }

        public static void GetDeliveryReqClassName(this DeliveryRequestViewModel dr, List<DeliveryRequestViewModel> loadDrs)
        {
            if (dr.PreviousStatus == 3)
            {
                List<int> enrouteInProgress = new List<int> { 1, 3, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 },
                    enrouteCancelled = new List<int> { 2, 7, 8 },
                    enrouteCompleted = new List<int> { 4 };
                List<int> trackCompleted = new List<int> { 7, 8, 9, 10, 22, 24 },
                    trackCancelled = new List<int> { 5, 11, 12, 13, 20, 21 };
                List<int> dropDiversion = new List<int> { 25 };

                if (dropDiversion.Contains(dr.TrackScheduleStatus))
                    dr.StatusClassId = 5;
                else if (trackCompleted.Contains(dr.TrackScheduleStatus) || enrouteCompleted.Contains(dr.TrackScheduleEnrouteStatus))
                    dr.StatusClassId = 4;
                else if (trackCancelled.Contains(dr.TrackScheduleStatus) || enrouteCancelled.Contains(dr.TrackScheduleEnrouteStatus))
                    dr.StatusClassId = 3;
                else if (enrouteInProgress.Contains(dr.TrackScheduleEnrouteStatus))
                    dr.StatusClassId = 2;
                else
                    dr.StatusClassId = 1;

                if (dr.IsBlendedRequest)
                {
                    var blendDrs = loadDrs.Where(t => t.BlendedGroupId == dr.BlendedGroupId);
                    if (blendDrs.All(t => trackCompleted.Contains(t.TrackScheduleStatus) ||
                           enrouteCompleted.Contains(t.TrackScheduleEnrouteStatus)))
                    {
                        dr.BlendDrScheduleStatus = 4;
                    }
                    else if (blendDrs.Any(t => trackCancelled.Contains(t.TrackScheduleStatus) ||
                           enrouteCancelled.Contains(t.TrackScheduleEnrouteStatus)))
                    {
                        dr.BlendDrScheduleStatus = 3;
                    }
                    else if (blendDrs.Any(t => enrouteInProgress.Contains(t.TrackScheduleEnrouteStatus)))
                    {
                        dr.BlendDrScheduleStatus = 2;
                    }
                    else if (blendDrs.Any(t => dropDiversion.Contains(t.TrackScheduleStatus)))
                    {
                        dr.BlendDrScheduleStatus = 5;
                    }
                    else
                    {
                        dr.BlendDrScheduleStatus = 1;
                    }
                }
            }
        }

        private static void SetTrailers(SbDriverViewModel viewModel, ScheduleBuilderViewModel sbModel)
        {
            var trailers = viewModel.Shifts.SelectMany(t => t.Schedules).SelectMany(t => t.Trailers).GroupBy(t => t.Id);
            foreach (var trailer in trailers)
            {
                var firstTrailer = trailer.FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(firstTrailer.Id))
                {
                    sbModel.Trailers.Add(new TrailerModel() { Id = firstTrailer.Id, TrailerId = firstTrailer.TrailerId, TrailerType = firstTrailer.TrailerType, Compartments = firstTrailer.Compartments });
                }
            }
        }

        private static void SetTrailers(SbTrailerViewModel viewModel, ScheduleBuilderViewModel sbModel)
        {
            if (viewModel.Trailers != null)
            {
                foreach (var trailer in viewModel.Trailers)
                {
                    sbModel.Trailers.Add(new TrailerModel() { Id = trailer.Id, TrailerId = trailer.TrailerId, TrailerType = trailer.TrailerType, Compartments = trailer.Compartments, StartTime = trailer.StartTime, EndTime = trailer.EndTime });
                }
            }
        }

        private static void SetTrips(SbDriverViewModel viewModel, ScheduleBuilderViewModel sbModel)
        {
            foreach (var shift in viewModel.Shifts)
            {
                foreach (var schedule in shift.Schedules)
                {
                    foreach (var trip in schedule.Trips)
                    {
                        if (trip.DeliveryRequests.Any())
                        {
                            trip.Drivers = schedule.Drivers;
                            trip.Trailers = schedule.Trailers;
                            trip.IsShiftCollapsed = shift.IsCollapsed;
                            trip.ShiftEndTime = shift.EndTime;
                            trip.ShiftStartTime = shift.StartTime;
                            trip.ShiftId = shift.Id;

                            sbModel.Trips.Add(trip);
                        }
                    }
                }
                sbModel.Shifts.Add(new ShiftModel() { Id = shift.Id, StartTime = shift.StartTime, EndTime = shift.EndTime, SlotPeriod = shift.SlotPeriod });
            }
        }

        private static void SetTrips(SbTrailerViewModel viewModel, ScheduleBuilderViewModel sbModel)
        {
            foreach (var trailer in viewModel.Trailers)
            {
                foreach (var schedule in trailer.Shifts)
                {
                    foreach (var trip in schedule.Trips)
                    {
                        if (trip.DeliveryRequests.Any())
                        {
                            trip.Trailers = new List<TrailerModel>() { new TrailerModel() { Id = trailer.Id, TrailerId = trailer.TrailerId, StartTime = trailer.StartTime, EndTime = trailer.EndTime, Compartments = trailer.Compartments, TrailerType = trailer.TrailerType } };
                            trip.ShiftId = schedule.ShiftId;
                            trip.ShiftStartTime = schedule.StartTime;
                            trip.ShiftEndTime = schedule.EndTime;
                            sbModel.Trips.Add(trip);
                        }
                    }
                    if (!sbModel.Shifts.Any(t => t.Id == schedule.ShiftId))
                    {
                        sbModel.Shifts.Add(new ShiftModel() { Id = schedule.ShiftId, StartTime = schedule.StartTime, EndTime = schedule.EndTime, SlotPeriod = schedule.SlotPeriod });
                    }
                }
            }
        }

        public static void UpdateDriverViewModel(this SbDriverViewModel driverModel, ScheduleBuilderViewModel sbModel)
        {
            if (driverModel != null && sbModel != null)
            {
                driverModel.Id = sbModel.Id;
                driverModel.TimeStamp = sbModel.TimeStamp;
                driverModel.Status = sbModel.Status;
                driverModel.DeletedTripId = sbModel.DeletedTripId;
                driverModel.DeletedGroupId = sbModel.DeletedGroupId;

                for (int idx1 = 0; idx1 < driverModel.Shifts.Count; idx1++)
                {
                    for (int idx2 = 0; idx2 < driverModel.Shifts[idx1].Schedules.Count; idx2++)
                    {
                        for (int idx3 = 0; idx3 < driverModel.Shifts[idx1].Schedules[idx2].Trips.Count; idx3++)
                        {
                            var driverTrip = driverModel.Shifts[idx1].Schedules[idx2].Trips[idx3];
                            var trip = sbModel.Trips.FirstOrDefault(t => t.ShiftId == driverTrip.ShiftId
                                                        && t.DriverRowIndex == driverTrip.DriverRowIndex
                                                        && t.DriverColIndex == driverTrip.DriverColIndex);
                            if (trip != null)
                            {
                                driverModel.Shifts[idx1].Schedules[idx2].Trips[idx3] = trip;
                            }
                            InitializePickupLocation(driverModel.Shifts[idx1].Schedules[idx2].Trips[idx3]);
                        }
                    }
                }
            }
        }

        public static void UpdateTrailerViewModel(this SbTrailerViewModel trailerModel, ScheduleBuilderViewModel sbModel)
        {
            if (trailerModel != null && sbModel != null)
            {
                trailerModel.Id = sbModel.Id;
                trailerModel.TimeStamp = sbModel.TimeStamp;
                trailerModel.Status = sbModel.Status;
                trailerModel.DeletedTripId = sbModel.DeletedTripId;
                trailerModel.DeletedGroupId = sbModel.DeletedGroupId;

                for (int idx1 = 0; idx1 < trailerModel.Trailers.Count; idx1++)
                {
                    for (int idx2 = 0; idx2 < trailerModel.Trailers[idx1].Shifts.Count; idx2++)
                    {
                        for (int idx3 = 0; idx3 < trailerModel.Trailers[idx1].Shifts[idx2].Trips.Count; idx3++)
                        {
                            var trailerTrip = trailerModel.Trailers[idx1].Shifts[idx2].Trips[idx3];
                            var trip = sbModel.Trips.FirstOrDefault(t => t.ShiftId == trailerTrip.ShiftId
                                                        && t.Trailers.Any(y => t.Trailers.Select(z => z.Id).Contains(y.Id))
                                                        && t.TrailerRowIndex == trailerTrip.TrailerRowIndex
                                                        && t.TrailerColIndex == trailerTrip.TrailerColIndex);
                            if (trip != null)
                            {
                                trailerModel.Trailers[idx1].Shifts[idx2].Trips[idx3] = trip;
                            }
                            InitializePickupLocation(trailerModel.Trailers[idx1].Shifts[idx2].Trips[idx3]);
                        }
                    }
                }
            }
        }
    }
}
