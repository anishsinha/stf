using MongoDB.Bson;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.FreightModels.ScheduleBuilder;
using SiteFuel.MdbDataAccess.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SiteFuel.FreightRepository.Mappers
{
    public static class ScheduleBuilderMapper
    {
        public static ScheduleBuilderViewModel ToViewModel(this ScheduleBuilder entity, List<DeliveryRequest> deliveryRequests)
        {
            var date = entity.DateFilter.ToString(Resource.constFormatDate);
            var model = new ScheduleBuilderViewModel();
            model.Id = entity.Id.ToString();
            model.RegionId = entity.RegionId;
            model.ObjectFilter = entity.ObjectFilter;
            model.RegionFilter = entity.RegionFilter;
            model.DSBFilter = entity.DSBFilter;
            //model.DateFilter = date.GetDateFilter();
            model.Date = date;
            model.CompanyId = entity.TfxCompanyId;
            model.TimeStamp = entity.TimeStamp;
            model.Status = entity.Status;
            model.Shifts = entity.Shifts;
            model.Trailers = entity.Trailers;

            entity.Trips.ForEach(t => model.Trips.Add(t.ToViewModel(deliveryRequests)));
            return model;
        }

        public static ScheduleBuilder ToEntity(this DSBSaveModel model)
        {
            var entity = new ScheduleBuilder();
            entity.TimeStamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            entity.RegionId = model.RegionId;
            entity.ObjectFilter = model.ObjectFilter;
            entity.RegionFilter = model.RegionFilter;
            entity.DSBFilter = model.DSBFilter;
            entity.DateFilter = Convert.ToDateTime(model.Date).Date;
            entity.TfxCompanyId = model.CompanyId;
            entity.Status = model.Status;
            entity.UpdatedBy = model.UserId;
            entity.UpdatedOn = DateTimeOffset.Now;
            entity.IsActive = true;
            entity.IsDeleted = false;
            if (string.IsNullOrWhiteSpace(model.Id))
            {
                entity.Shifts = model.Shifts.Where(t => t.Id != null).ToList();
            }
            entity.Trips = new List<TripModel>();

            model.ToTripEntity(entity);

            return entity;
        }

        private static void ToTripEntity(this DSBSaveModel sbModel, ScheduleBuilder entity)
        {
            if (sbModel.Status == (int)Exchange.Utilities.DSBMethod.DriverAssignment)
            {
                foreach (var tripModel in sbModel.Trips.Where(t => t.TripId == null || t.TripId.Trim() == ""))
                {
                    var trip = tripModel.ToEmptyTripEntity(sbModel, entity);
                    entity.Trips.Add(trip);
                }
            }
            else
            {
                foreach (var tripModel in sbModel.Trips.Where(t => t.TripId == null || t.TripId.Trim() == ""))
                {
                    var trip = tripModel.ToEntity(sbModel, entity);
                    entity.Trips.Add(trip);
                }
            }
        }

        public static TripModel ToEntity(this TripViewModel tripModel, DSBSaveModel sbModel, ScheduleBuilder entity)
        {
            var trip = new TripModel()
            {
                TripId = !string.IsNullOrWhiteSpace(tripModel.TripId) ? ObjectId.Parse(tripModel.TripId) : ObjectId.GenerateNewId(),
                GroupId = tripModel.GroupId,
                LoadCode = tripModel.LoadCode,
                RouteInfo = tripModel.RouteInfo,
                TripStatus = tripModel.TripStatus == Exchange.Utilities.TripStatus.None ? tripModel.TripPrevStatus : tripModel.TripStatus,
                DeliveryGroupStatus = tripModel.DeliveryGroupStatus == Exchange.Utilities.DeliveryGroupStatus.None ? tripModel.DeliveryGroupPrevStatus : tripModel.DeliveryGroupStatus,
                IsCommonPickup = tripModel.IsCommonPickup,
                PickupLocationType = tripModel.PickupLocationType,
                IsShiftCollapsed = tripModel.IsShiftCollapsed,
                ShiftEndTime = tripModel.ShiftEndTime,
                ShiftStartTime = tripModel.ShiftStartTime,
                ShiftId = tripModel.ShiftId,
                SlotPeriod = tripModel.SlotPeriod,
                ShiftIndex = tripModel.ShiftIndex,
                DriverRowIndex = tripModel.DriverRowIndex,
                DriverColIndex = tripModel.DriverColIndex,
                TrailerRowIndex = tripModel.TrailerRowIndex,
                TrailerColIndex = tripModel.TrailerColIndex,
                TimeStamp = entity.TimeStamp,
                DriverScheduleMappingId = tripModel.DriverScheduleMappingId,
                UpdatedBy = sbModel.UserId,
                UpdatedByName = tripModel.UpdatedByName,
                UpdatedDate = entity.UpdatedOn,
                TfxDrivers = tripModel.Drivers.ToDropItemEntity(),
                Trailers = tripModel.Trailers,
                IsIncludeAllRegionDriver = tripModel.IsIncludeAllRegionDriver,
                IsDispatcherDragDropSequence=tripModel.IsDispatcherDragDropSequence,

            };
            if (string.IsNullOrWhiteSpace(tripModel.TripId))
            {
                trip.CreatedBy = sbModel.UserId;
                trip.CreatedDate = entity.UpdatedOn;
            }
            if (!string.IsNullOrWhiteSpace(tripModel.StartDate))
                trip.StartDate = Convert.ToDateTime(tripModel.StartDate);
            if (!string.IsNullOrWhiteSpace(tripModel.StartTime))
                trip.StartTime = Convert.ToDateTime(tripModel.StartTime).TimeOfDay;
            if (!string.IsNullOrWhiteSpace(tripModel.EndTime))
                trip.EndTime = Convert.ToDateTime(tripModel.EndTime).TimeOfDay;
            if (!string.IsNullOrWhiteSpace(tripModel.SupplierSource))
            {
                trip.SupplierSource = new DropdownDisplayItem() { Name = tripModel.SupplierSource };
            }
            if (!string.IsNullOrWhiteSpace(tripModel.Carrier))
            {
                trip.Carrier = new DropdownDisplayItem() { Name = tripModel.Carrier };
            }
            if (tripModel.Terminal != null && tripModel.Terminal.Id > 0)
            {
                trip.TfxTerminal = new DropdownDisplayItem() { Id = tripModel.Terminal.Id, Name = tripModel.Terminal.Name };
            }
            else
            {
                tripModel.Terminal = new FreightModels.DropdownDisplayItem();
            }
            if (tripModel.BulkPlant != null && !string.IsNullOrWhiteSpace(tripModel.BulkPlant.SiteName))
            {
                trip.TfxBulkPlant = tripModel.BulkPlant;
            }
            else
            {
                tripModel.BulkPlant = new FreightModels.BulkPlantAddressModel();
            }
            tripModel.DeliveryRequests.Where(t => t.Status != Exchange.Utilities.DeliveryReqStatus.Deleted).ToList().ForEach(t => trip.DeliveryRequests.Add(ObjectId.Parse(t.Id)));
            foreach (var trailer in tripModel.Trailers)
            {
                if (!entity.Trailers.Any(t => t.Id == trailer.Id))
                {
                    entity.Trailers.Add(new TrailerModel() { Id = trailer.Id, TrailerId = trailer.TrailerId, TrailerType = trailer.TrailerType, Compartments = trailer.Compartments });
                }
            }
            if (!string.IsNullOrWhiteSpace(tripModel.ShiftId) && !entity.Shifts.Any(t => t.Id == tripModel.ShiftId))
            {
                entity.Shifts.Add(new ShiftModel() { Id = tripModel.ShiftId, StartTime = tripModel.ShiftStartTime, EndTime = tripModel.ShiftEndTime, SlotPeriod = tripModel.SlotPeriod });
            }
            return trip;
        }
        public static List<Exchange.Utilities.DropdownDisplayExtendedItem> ToDropItemEntity(this List<DriverAdditionalDetailsViewModel> driverDetails)
        {
            var entities = new List<Exchange.Utilities.DropdownDisplayExtendedItem>();
            if (driverDetails != null)
            {
                driverDetails.ForEach(t => entities.Add(new Exchange.Utilities.DropdownDisplayExtendedItem { Id = t.Id, Name = t.Name, Code = t.Shifts }));
            }
            return entities;
        }
        public static Exchange.Utilities.DropdownDisplayExtendedItem ToDropItemEntity(this DriverAdditionalDetailsViewModel driverDetail)
        {
            var entity = new Exchange.Utilities.DropdownDisplayExtendedItem();
            if (driverDetail != null)
            {
                entity.Id = driverDetail.Id;
                entity.Code = driverDetail.Shifts;
                entity.Name = driverDetail.Name;
            }
            return entity;
        }

        public static DriverAdditionalDetailsViewModel ToDriverDetailModel(this Exchange.Utilities.DropdownDisplayExtendedItem entity)
        {
            var model = new DriverAdditionalDetailsViewModel();
            if (entity != null)
            {
                model.Id = entity.Id;
                model.Shifts = entity.Code;
                model.Name = entity.Name;
            }
            return model;
        }

        public static TripModel ToEmptyTripEntity(this TripViewModel tripModel, DSBSaveModel sbModel, ScheduleBuilder entity)
        {
            var trip = new TripModel()
            {
                TripId = !string.IsNullOrWhiteSpace(tripModel.TripId) ? ObjectId.Parse(tripModel.TripId) : ObjectId.GenerateNewId(),
                TripStatus = tripModel.TripStatus == Exchange.Utilities.TripStatus.None ? tripModel.TripPrevStatus : tripModel.TripStatus,
                DeliveryGroupStatus = tripModel.DeliveryGroupStatus == Exchange.Utilities.DeliveryGroupStatus.None ? tripModel.DeliveryGroupPrevStatus : tripModel.DeliveryGroupStatus,
                IsCommonPickup = false,
                PickupLocationType = SiteFuel.Exchange.Utilities.PickupLocationType.Terminal,
                TfxDrivers = tripModel.Drivers.ToDropItemEntity(),
                Trailers = tripModel.Trailers,
                IsShiftCollapsed = tripModel.IsShiftCollapsed,
                ShiftEndTime = tripModel.ShiftEndTime,
                ShiftStartTime = tripModel.ShiftStartTime,
                ShiftId = tripModel.ShiftId,
                SlotPeriod = tripModel.SlotPeriod,
                ShiftIndex = tripModel.ShiftIndex,
                DriverRowIndex = tripModel.DriverRowIndex,
                DriverColIndex = tripModel.DriverColIndex,
                TrailerRowIndex = tripModel.TrailerRowIndex,
                TrailerColIndex = tripModel.TrailerColIndex,
                TimeStamp = entity.TimeStamp,
                DriverScheduleMappingId = tripModel.DriverScheduleMappingId,
                UpdatedBy = sbModel.UserId,
                UpdatedByName = tripModel.UpdatedByName,
                UpdatedDate = entity.UpdatedOn,
                IsIncludeAllRegionDriver = tripModel.IsIncludeAllRegionDriver,
                IsDispatcherDragDropSequence=tripModel.IsDispatcherDragDropSequence,
            };
            if (string.IsNullOrWhiteSpace(tripModel.TripId))
            {
                trip.CreatedBy = sbModel.UserId;
                trip.CreatedDate = entity.UpdatedOn;
            }
            if (!string.IsNullOrWhiteSpace(tripModel.StartDate))
                trip.StartDate = Convert.ToDateTime(tripModel.StartDate);
            if (!string.IsNullOrWhiteSpace(tripModel.StartTime))
                trip.StartTime = Convert.ToDateTime(tripModel.StartTime).TimeOfDay;
            if (!string.IsNullOrWhiteSpace(tripModel.EndTime))
                trip.EndTime = Convert.ToDateTime(tripModel.EndTime).TimeOfDay;

            tripModel.Terminal = new FreightModels.DropdownDisplayItem();
            tripModel.BulkPlant = new FreightModels.BulkPlantAddressModel();
            foreach (var trailer in tripModel.Trailers)
            {
                if (!entity.Trailers.Any(t => t.Id == trailer.Id))
                {
                    entity.Trailers.Add(new TrailerModel() { Id = trailer.Id, TrailerId = trailer.TrailerId, TrailerType = trailer.TrailerType, Compartments = trailer.Compartments });
                }
            }
            if (!string.IsNullOrWhiteSpace(tripModel.ShiftId) && !entity.Shifts.Any(t => t.Id == tripModel.ShiftId))
            {
                entity.Shifts.Add(new ShiftModel() { Id = tripModel.ShiftId, StartTime = tripModel.ShiftStartTime, EndTime = tripModel.ShiftEndTime, SlotPeriod = tripModel.SlotPeriod });
            }
            return trip;
        }
       
        public static TripViewModel ToViewModel(this TripModel trip, List<DeliveryRequest> deliveryRequests = null)
        {
            var tripModel = new TripViewModel()
            {
                TripId = trip.TripId.ToString(),
                GroupId = trip.GroupId,
                LoadCode = trip.LoadCode,
                RouteInfo = trip.RouteInfo,
                TripPrevStatus = trip.TripStatus,
                TripStatus = Exchange.Utilities.TripStatus.None,
                DeliveryGroupStatus = Exchange.Utilities.DeliveryGroupStatus.None,
                DeliveryGroupPrevStatus = trip.DeliveryGroupStatus,
                IsCommonPickup = trip.IsCommonPickup,
                PickupLocationType = trip.PickupLocationType,
                Trailers = trip.Trailers,
                IsShiftCollapsed = trip.IsShiftCollapsed,
                ShiftEndTime = trip.ShiftEndTime,
                ShiftStartTime = trip.ShiftStartTime,
                ShiftId = trip.ShiftId,
                SlotPeriod = trip.SlotPeriod,
                ShiftIndex = trip.ShiftIndex,
                DriverRowIndex = trip.DriverRowIndex,
                DriverColIndex = trip.DriverColIndex,
                TimeStamp = trip.TimeStamp,
                DriverScheduleMappingId = trip.DriverScheduleMappingId,
                IsIncludeAllRegionDriver = trip.IsIncludeAllRegionDriver,
                IsDispatcherDragDropSequence=trip.IsDispatcherDragDropSequence
            };
            if (trip.StartDate != null)
            {
                tripModel.StartDate = trip.StartDate.Value.ToString(Resource.constFormatDate);
            }
            if (trip.StartTime != null)
            {
                tripModel.StartTime = Convert.ToDateTime(trip.StartTime.ToString()).ToShortTimeString();
            }
            if (trip.EndTime != null)
            {
                tripModel.EndTime = Convert.ToDateTime(trip.EndTime.ToString()).ToShortTimeString();
            }
            trip.TfxDrivers.ForEach(t => tripModel.Drivers.Add(new DriverAdditionalDetailsViewModel { Id = t.Id, Shifts = t.Code, Name = t.Name }));
            if (trip.SupplierSource != null && !string.IsNullOrWhiteSpace(trip.SupplierSource.Name))
            {
                tripModel.SupplierSource = trip.SupplierSource.Name;
            }
            if (trip.Carrier != null && !string.IsNullOrWhiteSpace(trip.Carrier.Name))
            {
                tripModel.Carrier = trip.Carrier.Name;
            }
            tripModel.PickupLocationType = trip.PickupLocationType;
            if (trip.IsCommonPickup)
            {

                if (trip.TfxTerminal != null && trip.TfxTerminal.Id > 0)
                {
                    tripModel.Terminal = new FreightModels.DropdownDisplayItem() { Id = trip.TfxTerminal.Id, Name = trip.TfxTerminal.Name };
                }
                if (trip.TfxBulkPlant != null && !string.IsNullOrWhiteSpace(trip.TfxBulkPlant.SiteName))
                {
                    tripModel.BulkPlant = trip.TfxBulkPlant;
                }
            }
            if (trip.DeliveryRequests != null && deliveryRequests != null)
            {
                var drs = deliveryRequests.Where(t => trip.DeliveryRequests.Contains(t.Id)).Select(t => t.ToDeliveryRequestViewModel()).ToList();
                tripModel.DeliveryRequests.GetBlendDRInfo(drs, false);
            }
            return tripModel;
        }

        public static DSBSaveModel ToDsbSaveModel(this ScheduleBuilderViewModel viewModel)
        {
            var model = new DSBSaveModel();
            model.CompanyId = viewModel.CompanyId;
            model.Date = viewModel.Date;
            model.DateFilter = viewModel.DateFilter;
            model.DeletedDriverScheduleMappingId = viewModel.DeletedDriverScheduleMappingId;
            model.DeletedGroupId = viewModel.DeletedGroupId;
            model.DeletedTripId = viewModel.DeletedTripId;
            model.Id = viewModel.Id;
            model.ObjectFilter = viewModel.ObjectFilter;
            model.RegionFilter = viewModel.RegionFilter;
            model.RegionId = viewModel.RegionId;
            model.Shifts = viewModel.Shifts;
            model.TimeStamp = viewModel.TimeStamp;
            model.UserId = model.UserId;
            model.WindowMode = viewModel.WindowMode;
            return model;
        }
    }
}
