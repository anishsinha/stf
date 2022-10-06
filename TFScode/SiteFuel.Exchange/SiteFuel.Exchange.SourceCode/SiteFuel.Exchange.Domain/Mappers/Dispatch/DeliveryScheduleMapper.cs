using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class DeliveryScheduleMapper
    {
        public static DeliverySchedule ToEntity(this DeliveryScheduleViewModel viewModel, DeliverySchedule entity = null)
        {
            if (entity == null)
                entity = new DeliverySchedule();

            entity.Id = viewModel.Id;
            entity.Date = viewModel.ScheduleDate.Date;
            entity.StartTime = Convert.ToDateTime(viewModel.ScheduleStartTime).TimeOfDay;
            entity.EndTime = Convert.ToDateTime(viewModel.ScheduleEndTime).TimeOfDay;
            entity.Quantity = viewModel.ScheduleQuantityType != ScheduleQuantityType.Quantity ? 0 : viewModel.ScheduleQuantity;
            entity.WeekDayId = viewModel.ScheduleDay;
            entity.GroupId = viewModel.GroupId;
            entity.Type = viewModel.ScheduleType;
            entity.CreatedBy = viewModel.CreatedBy;
            entity.StatusId = viewModel.StatusId;
            entity.IsRescheduled = viewModel.IsRescheduled;
            entity.RescheduledTrackableId = viewModel.RescheduledTrackableId;
            entity.UoM = viewModel.UoM;
            entity.LoadCode = viewModel.LoadCode;
            entity.QuantityTypeId = (int)viewModel.ScheduleQuantityType;
            if (viewModel.Carrier != null && viewModel.Carrier.Id > 0)
            {
                entity.CarrierId = viewModel.Carrier.Id;
            }
            if (viewModel.SupplierSource != null)
            {
                entity.SupplierContract = viewModel.SupplierSource.ContractNumber;
                entity.SupplierSourceId = viewModel.SupplierSource.Id;
            }
            if (viewModel.SplitLoadAddresses != null && viewModel.IsSplitDrop)
                viewModel.SplitLoadAddresses.ForEach(t => entity.FuelDispatchLocations.Add(t.ToFuelDispatchLocationEntity()));

            return entity;
        }

        public static DeliveryScheduleViewModel ToViewModel(this List<DeliverySchedule> entity, List<FuelDispatchLocation> dropLocations = null, DeliveryScheduleViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new DeliveryScheduleViewModel(Status.Success);

            if (entity != null && entity.Count > 0)
            {
                var firstSchedule = entity.First();
                var driver = firstSchedule.DeliveryScheduleXDrivers.SingleOrDefault(t => t.IsActive);
                viewModel.Id = firstSchedule.Id;
                viewModel.ScheduleDate = firstSchedule.Date;
                viewModel.ScheduleStartTime = Convert.ToDateTime(firstSchedule.StartTime.ToString()).ToShortTimeString();
                viewModel.ScheduleEndTime = Convert.ToDateTime(firstSchedule.EndTime.ToString()).ToShortTimeString();
                viewModel.StartTime = firstSchedule.StartTime;
                viewModel.EndTime = firstSchedule.EndTime;
                var scheduleQtyType = firstSchedule.QuantityTypeId == null ? ScheduleQuantityType.Quantity : (firstSchedule.QuantityTypeId.Value != (int)ScheduleQuantityType.Quantity ? (ScheduleQuantityType)firstSchedule.QuantityTypeId.Value : ScheduleQuantityType.Quantity);
                viewModel.ScheduleQuantityType = scheduleQtyType;
                viewModel.ScheduleQuantityTypeText = scheduleQtyType.GetDisplayName();
                viewModel.ScheduleQuantity = firstSchedule.Quantity.GetPreciseValue(6);
                viewModel.ScheduleDays = entity.Select(t => t.WeekDayId).ToList();
                viewModel.ScheduleDayNames = entity.Where(t => t.MstWeekDay != null).Select(t => t.MstWeekDay.Code).ToList();
                viewModel.ScheduleType = firstSchedule.Type;
                viewModel.ScheduleTypeName = firstSchedule.MstDeliveryScheduleType.Name;
                viewModel.StrScheduleDate = firstSchedule.Date.ToString(Resource.constFormatDate);
                viewModel.AllScheduleDate = entity.Select(t => t.Date).ToList();
                viewModel.GroupId = firstSchedule.GroupId;
                viewModel.CreatedBy = firstSchedule.CreatedBy;
                viewModel.DriverId = driver == null ? (int?)null : driver.DriverId;
                viewModel.DriverName = driver == null || driver.User == null ? Resource.lblNoDriverAssigned : $"{driver.User.FirstName} {driver.User.LastName}";
                viewModel.PhoneNumber = driver == null || driver.User == null ? Resource.lblHyphen : $"{driver.User.PhoneNumber}";
                viewModel.StatusId = firstSchedule.StatusId;
                viewModel.IsRescheduled = firstSchedule.IsRescheduled;
                viewModel.RescheduledTrackableId = firstSchedule.RescheduledTrackableId;
                viewModel.UoM = firstSchedule.UoM;
                viewModel.LoadCode = firstSchedule.LoadCode;
                viewModel.Carrier = firstSchedule.Carrier.ToViewModel();
                viewModel.SupplierSource = new SupplierSourceViewModel() { ContractNumber = firstSchedule.SupplierContract };
                if (firstSchedule.SupplierSourceId.HasValue)
                {
                    viewModel.SupplierSource.Id = firstSchedule.SupplierSourceId;
                    viewModel.SupplierSource.Name = firstSchedule.SupplierSource.Name;
                }
                
                if (dropLocations != null && dropLocations.Any())
                {
                    var fuelDropLocation = dropLocations.Where(t => t.DeliveryScheduleId == viewModel.Id).ToList();
                    if (fuelDropLocation.Any())
                    {
                        viewModel.IsSplitDrop = true;
                        fuelDropLocation.ForEach(t => viewModel.SplitLoadAddresses.Add(t.ToViewModel()));
                    }
                }
            }
            return viewModel;
        }

        public static OrderVersionHistoryViewModel ToViewModel(this IEnumerable<OrderVersionXDeliverySchedule> entity, OrderVersionHistoryViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new OrderVersionHistoryViewModel(Status.Success);

            if (entity != null)
            {
                var user = entity.FirstOrDefault().User;
                viewModel.CreatedUser = $"{user.FirstName} {user.LastName}";
                viewModel.CreatedDate = entity.FirstOrDefault().CreatedDate.ToString(Resource.constFormatDate);
                viewModel.DeliverySchedules = entity.Where(t => t.DeliveryRequestId.HasValue).Select(t => t.DeliverySchedule)
                                                    .GroupBy(t => t.GroupId)
                                                    .Select(g => new { Items = g.ToList() })
                                                    .Select(t => t.Items.ToViewModel())
                                                    .OrderBy(t => t.Id)
                                                    .ToList();

            }
            return viewModel;
        }

        public static OrderVersionViewModel ToViewModel(this IEnumerable<OrderVersionXDeliverySchedule> entity, List<FuelDispatchLocation> dropLocations = null, OrderVersionViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new OrderVersionViewModel(Status.Success);

            if (entity != null)
            {
                viewModel.Id = entity.FirstOrDefault().OrderId;
                viewModel.AdditionalNotes = entity.FirstOrDefault().AdditionalNotes;
                viewModel.CreatedBy = entity.FirstOrDefault().CreatedBy;
                viewModel.Version = entity.FirstOrDefault().Version;
                viewModel.DeliverySchedules = entity.Where(t => t.DeliveryRequestId.HasValue).Select(t => t.DeliverySchedule)
                                                    .GroupBy(t => t.GroupId)
                                                    .Select(g => new { Items = g.ToList() })
                                                    .Select(t => t.Items.ToViewModel(dropLocations))
                                                    .OrderBy(t => t.Id)
                                                    .ToList();
            }
            return viewModel;
        }

        public static OrderVersionViewModel ToViewModel(this List<UspGetOrderScheduleDetailsViewModel> entity)
        {
            OrderVersionViewModel viewModel = new OrderVersionViewModel(Status.Success);

            if (!entity.Any())
                return viewModel;

            var version = entity.First();
            viewModel.Id = version.OrderId;
            viewModel.AdditionalNotes = version.AdditionalNotes;
            viewModel.Version = version.Version;
            viewModel.DeliverySchedules = entity.Where(t => t.Id.HasValue)
                                                .GroupBy(t => t.GroupId)
                                                .Select(g => new { Items = g.ToList() })
                                                .Select(t => t.Items.ToScheduleViewModel())
                                                .OrderBy(t => t.Id)
                                                .ToList();

            return viewModel;
        }

        public static DeliveryScheduleViewModel ToScheduleViewModel(this List<UspGetOrderScheduleDetailsViewModel> entity)
        {
            DeliveryScheduleViewModel viewModel = new DeliveryScheduleViewModel(Status.Success);

            if (entity != null && entity.Any())
            {
                var schedule = entity.First();
                viewModel.Id = schedule.Id.Value;
                viewModel.ScheduleDate = schedule.Date.Value;
                viewModel.ScheduleStartTime = Convert.ToDateTime(schedule.StartTime.Value.ToString()).ToShortTimeString();
                viewModel.ScheduleEndTime = Convert.ToDateTime(schedule.EndTime.Value.ToString()).ToShortTimeString();
                viewModel.StartTime = schedule.StartTime.Value;
                viewModel.EndTime = schedule.EndTime.Value;
                viewModel.ScheduleQuantity = schedule.Quantity.Value.GetPreciseValue(6);
                viewModel.ScheduleQuantityType = schedule.ScheduleQuantityTypeId;
                viewModel.ScheduleDays = entity.Select(t => t.WeekDayId.Value).ToList();
                viewModel.ScheduleDayNames = entity.Where(t => t.WeekDayCode != null).Select(t => t.WeekDayCode).Distinct().ToList();
                viewModel.ScheduleType = schedule.Type.Value;
                viewModel.ScheduleTypeName = schedule.ScheduleTypeName;
                viewModel.StrScheduleDate = schedule.Date.Value.ToString(Resource.constFormatDate);
                viewModel.AllScheduleDate = entity.Select(t => t.Date.Value).ToList();
                viewModel.GroupId = schedule.GroupId.Value;
                viewModel.CreatedBy = schedule.CreatedBy.Value;
                viewModel.DriverId = schedule.DriverId;
                viewModel.DriverName = schedule.DriverId == null ? Resource.lblNoDriverAssigned : schedule.DriverName;
                viewModel.PhoneNumber = schedule.PhoneNumber;
                viewModel.StatusId = schedule.StatusId.Value;
                viewModel.IsRescheduled = schedule.IsRescheduled.Value;
                viewModel.RescheduledTrackableId = schedule.RescheduledTrackableId;
                viewModel.UoM = schedule.UoM.Value;
                viewModel.LoadCode = schedule.LoadCode;
                viewModel.IsDeliveryIn24Hrs = entity.Any(t => t.IsDeliveryIn24Hrs);
                if (schedule.CarrierId.HasValue)
                {
                    viewModel.Carrier.Id = schedule.CarrierId.Value;
                    viewModel.Carrier.Name = schedule.CarrierName;
                }
                viewModel.SupplierSource = new SupplierSourceViewModel() { ContractNumber = schedule.SupplierContract };
                if (schedule.SupplierSourceId.HasValue)
                {
                    viewModel.SupplierSource.Id = schedule.SupplierSourceId;
                    viewModel.SupplierSource.Name = schedule.SourceName;
                }
                var fuelDispatchLocations = entity.Where(t => t.Id == viewModel.Id && t.LocationId != null && !entity.Any(t1 => t.TrackableScheduleId == null && t1.ParentId == t.LocationId 
                                                && t.IsSkipped) && !t.IsSkipped && t.IsActive).ToList();
                if (fuelDispatchLocations.Any())
                {
                    viewModel.IsSplitDrop = true;
                    fuelDispatchLocations.ForEach(t => viewModel.SplitLoadAddresses.Add(t.ToViewModel()));
                }
            }
            return viewModel;
        }

        public static CarrierViewModel ToViewModel(this Carrier entity, CarrierViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new CarrierViewModel();
            if (entity != null)
            {
                viewModel.Id = entity.Id;
                viewModel.Name = entity.Name;
            }
            return viewModel;
        }

        public static Carrier ToEntity(this CarrierViewModel viewModel, Carrier entity = null)
        {
            if (entity == null)
                entity = new Carrier();

            entity.Id = viewModel.Id;
            entity.Name = viewModel.Name;
            entity.CreatedBy = viewModel.CreatedBy;
            entity.CompanyId = viewModel.CompanyId;
            return entity;
        }
    }
}
