using MongoDB.Bson;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.MdbDataAccess.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightRepository.Mappers
{
    public static class RecurringSchedulesMapper
    {
        public static List<RecurringSchedules> ToRecurringDREntity(this List<DeliveryRequestViewModel> requests)
        {
            var entityList = new List<RecurringSchedules>();
            foreach (var model in requests)
            {
                var entity = model.ToRecurringEntity();
                entityList.AddRange(entity);
            }
            return entityList;
        }
        public static List<RecurringSchedules> ToRecurringEntity(this DeliveryRequestViewModel model)
        {
            var entity = new List<RecurringSchedules>();
            foreach (var item in model.RecurringSchdule.ToList())
            {
                RecurringSchedules recurringSchdule = new RecurringSchedules();
                if (item.Id != null)
                {
                    recurringSchdule.Id = ObjectId.Parse(item.Id);
                }
                recurringSchdule.ScheduleType = item.ScheduleType;
                recurringSchdule.WeekDayId = item.ScheduleType == (int)ScheduleTypes.Monthly ? new List<string>() : item.WeekDayId;
                recurringSchdule.Date = item.ScheduleType == (int)ScheduleTypes.Monthly ? item.Date : string.Empty;
                if (!string.IsNullOrEmpty(recurringSchdule.Date))
                {
                    DateTime time = DateTime.Parse(recurringSchdule.Date);
                    recurringSchdule.MonthDayId = time.Day;
                }
                recurringSchdule.ScheduleQuantityType = item.ScheduleQuantityType;
                recurringSchdule.RequiredQuantity = item.ScheduleQuantityType == (int)ScheduleQuantityType.Quantity ? item.RequiredQuantity : 0;
                recurringSchdule.OrderId = model.OrderId;
                recurringSchdule.PoNumber = model.PoNumber;
                recurringSchdule.SiteId = model.SiteId;
                recurringSchdule.JobId = model.JobId;
                recurringSchdule.TankName = !string.IsNullOrEmpty(item.TankName) ? item.TankName : string.Empty;
                recurringSchdule.AssetId = item.AssetId;
                recurringSchdule.TfxSupplierCompanyId = model.SupplierCompanyId;
                recurringSchdule.TfxCompanyName = model.CustomerCompany;
                recurringSchdule.BuyerCompanyId = model.BuyerCompanyId;
                recurringSchdule.TfxUserId = model.CreatedBy;
                recurringSchdule.AssignedToCompanyId = model.AssignedToCompanyId;
                recurringSchdule.IsActive = true;
                recurringSchdule.IsDeleted = false;
                recurringSchdule.CreatedOn = DateTime.Now;
                recurringSchdule.CreatedBy = model.CreatedBy;
                recurringSchdule.isIgnoreRecord = item.isIgnoreRecord;
                recurringSchdule.DeliveryRequestFor = model.DeliveryRequestFor;
                recurringSchdule.ProductTypeId = model.ProductTypeId;
                recurringSchdule.RegionId = string.IsNullOrEmpty(model.AssignedToRegionId) ? ObjectId.Empty : ObjectId.Parse(model.AssignedToRegionId);
                recurringSchdule.DeliveryRequests = model.ToRecurringDREntity();
                //set the required quantity based on ScheduleQuantityType type.
                recurringSchdule.DeliveryRequests.Where(x => x.IsBlendedRequest == false).ToList().ForEach(xiTem =>
                    {
                        xiTem.RequiredQuantity = item.ScheduleQuantityType == (int)ScheduleQuantityType.Quantity ? item.RequiredQuantity : 0;
                    });
                recurringSchdule.BlendedGroupId = model.BlendedGroupId;
                recurringSchdule.IsBlendedRequest = model.IsBlendedRequest;
                recurringSchdule.RecurringBlendedGroupId = item.RecurringBlendedGroupId;
                recurringSchdule.DeliveryLevelPO = item.DeliveryLevelPO;
                entity.Add(recurringSchdule);
            }
            return entity;
        }
        public static RecurringDRSchdule ToRecurringEntity(this RecurringSchedules item)
        {
            var recurringItem = new RecurringDRSchdule();
            recurringItem.Id = item.Id.ToString();
            recurringItem.ScheduleType = item.ScheduleType;
            recurringItem.WeekDayId = item.WeekDayId;
            recurringItem.MonthDayId = item.MonthDayId;
            recurringItem.Date = item.Date;
            recurringItem.ScheduleQuantityType = item.ScheduleQuantityType;
            recurringItem.RequiredQuantity = item.RequiredQuantity;
            recurringItem.OrderId = item.OrderId;
            recurringItem.PoNumber = item.PoNumber;
            recurringItem.SiteId = item.SiteId;
            recurringItem.JobId = item.JobId;
            recurringItem.TfxSupplierCompanyId = item.TfxSupplierCompanyId;
            recurringItem.TfxCompanyName = item.TfxCompanyName;
            recurringItem.TfxUserId = item.TfxUserId;
            recurringItem.AssignedToCompanyId = item.AssignedToCompanyId;
            recurringItem.BuyerCompanyId = item.BuyerCompanyId;
            recurringItem.TankName = item.TankName;
            recurringItem.AssetId = item.AssetId;
            recurringItem.ProductTypeId = item.ProductTypeId;
            recurringItem.OrderId = item.OrderId == null ? 0 : item.OrderId.Value;
            recurringItem.IsBlendedRequest = item.IsBlendedRequest;
            recurringItem.BlendedGroupId = item.BlendedGroupId;
            recurringItem.DeliveryLevelPO = item.DeliveryLevelPO;
            return recurringItem;
        }
        public static CreateRecurringDRViewModel ToEntity(this RecurringSchedules item)
        {
            var recurringItem = new CreateRecurringDRViewModel();
            recurringItem.Id = item.Id.ToString();
            recurringItem.Date = item.Date;
            recurringItem.JobId = item.JobId;
            recurringItem.MonthDayId = item.MonthDayId;
            recurringItem.ScheduleType = item.ScheduleType;
            recurringItem.TfxUserId = item.TfxUserId;
            recurringItem.WeekDayId = item.WeekDayId;
            recurringItem.RegionId = item.ShiftInfo.RegionId;
            recurringItem.TfxCompanyId = item.AssignedToCompanyId;
            recurringItem.ShiftInfo = item.ShiftInfo.ToRecurringShiftEntity();
            recurringItem.DeliveryRequests = new List<DeliveryRequestViewModel>();
            foreach (var delitem in item.DeliveryRequests)
            {
                recurringItem.DeliveryRequests.Add(delitem.ToCreateRecurringDREntity());
            }
            foreach (var delRCitem in recurringItem.DeliveryRequests)
            {
                delRCitem.isRecurringSchedule = true;
                delRCitem.RecurringScheduleId = item.Id.ToString();
                delRCitem.ScheduleQuantityType = item.ScheduleQuantityType;
            }
            recurringItem.ScheduleBuilderId = item.ScheduleBuilderId.ToString();
            recurringItem.OrderId = item.OrderId != null ? item.OrderId.Value : 0;
            recurringItem.DeliveryLevelPO = item.DeliveryLevelPO;
            recurringItem.DeliveryRequests.ForEach(x => x.DeliveryLevelPO = recurringItem.DeliveryLevelPO);
            return recurringItem;
        }
        public static RecurringShiftInfoViewModel ToRecurringShiftEntity(this RecurringShiftInfo item)
        {
            var recurringShiftInfo = new RecurringShiftInfoViewModel();
            recurringShiftInfo.CompanyId = item.CompanyId;
            recurringShiftInfo.DriverColIndex = item.DriverColIndex;
            recurringShiftInfo.DriverRowIndex = item.DriverRowIndex;
            recurringShiftInfo.EndTime = item.EndTime;
            recurringShiftInfo.StartTime = item.StartTime;
            recurringShiftInfo.RegionId = item.RegionId;
            recurringShiftInfo.ShiftId = item.ShiftId;
            recurringShiftInfo.ShiftIndex = item.ShiftIndex;
            return recurringShiftInfo;
        }
        public static RecurringDeliveryRequestDetailsViewModel ToRecurringDREntity(this RecurringDeliveryRequestDetails model)
        {
            var entity = new RecurringDeliveryRequestDetailsViewModel();

            entity.TfxJobAddress = model.TfxJobAddress;
            entity.TfxJobCity = model.TfxJobCity;
            entity.TfxJobName = model.TfxJobName;
            entity.TfxProductType = model.TfxProductType;
            entity.TfxUoM = model.TfxUoM;
            entity.TfxCreatedByCompanyId = model.TfxCreatedByCompanyId;
            entity.TfxAssignedToCompanyId = model.TfxAssignedToCompanyId;
            entity.TfxSupplierCompanyId = model.TfxSupplierCompanyId;
            entity.TfxDisplayJobId = model.TfxDisplayJobId;
            entity.StorageId = model.StorageId;
            entity.StorageTypeId = model.StorageTypeId;
            entity.CreatedRegionId = model.CreatedRegionId.ToString();
            entity.TfxProductTypeId = model.TfxProductTypeId;
            entity.DRCreationLevel = model.DRCreationLevel;
            entity.RequiredQuantity = model.RequiredQuantity;
            entity.Priority = model.Priority;
            entity.CurrentThreshold = model.CurrentThreshold;
            entity.Status = model.Status;
            entity.TfxAssignedToRegionId = model.TfxAssignedToRegionId;
            //entity.TfxAssignedToRegionId = ObjectId.Parse(model.AssignedToRegionId);
            entity.TfxJobId = model.TfxJobId;
            entity.TfxCustomerCompany = model.TfxCustomerCompany;
            entity.TfxAssignedToUserId = model.TfxAssignedToUserId;
            entity.TfxOrderId = model.TfxOrderId;
            entity.TfxDeliveryGroupId = model.TfxDeliveryGroupId;
            entity.TfxDeliveryScheduleId = model.TfxDeliveryScheduleId;
            entity.TfxTrackableScheduleId = model.TfxTrackableScheduleId;
            entity.AutoDRStatus = model.AutoDRStatus;
            entity.AutoCreatedOn = model.AutoCreatedOn;
            entity.AutoUpdatedOn = model.AutoUpdatedOn;
            entity.ParentId = model.ParentId;
            entity.TankMaxFill = model.TankMaxFill;
            if (model.TfxTerminal != null && model.TfxTerminal.Id > 0)
            {
                entity.TfxTerminal = new FreightModels.DropdownDisplayItem()
                {
                    Id = model.TfxTerminal.Id,
                    Name = model.TfxTerminal.Name
                };
            }
            if (model.TfxBulkPlant != null && !string.IsNullOrWhiteSpace(model.TfxBulkPlant.SiteName))
            {
                entity.TfxBulkPlant = model.TfxBulkPlant;
            }
            entity.BadgeNo1 = model.BadgeNo1;
            entity.BadgeNo2 = model.BadgeNo2;
            entity.BadgeNo3 = model.BadgeNo3;
            entity.DispactherNote = model.DispactherNote;
            entity.IsCommonBadge = model.IsCommonBadge;
            entity.RouteInfo = model.RouteInfo;
            return entity;
        }
        public static DeliveryRequestViewModel ToCreateRecurringDREntity(this RecurringDeliveryRequestDetails entity)
        {
            var model = new DeliveryRequestViewModel();
            model.JobAddress = entity.TfxJobAddress;
            model.JobCity = entity.TfxJobCity;
            model.JobName = entity.TfxJobName;
            model.ProductType = entity.TfxProductType;
            model.UoM = entity.TfxUoM;
            model.Priority = entity.Priority;
            model.CreatedByCompanyId = entity.TfxCreatedByCompanyId;
            model.AssignedToCompanyId = entity.TfxAssignedToCompanyId;
            model.SupplierCompanyId = entity.TfxSupplierCompanyId;
            model.SiteId = entity.TfxDisplayJobId;
            model.TankId = entity.StorageTypeId;
            model.StorageId = entity.StorageId;
            model.JobTimeZoneOffset = entity.JobTimeZoneOffset;
            model.CreatedByRegionId = entity.CreatedRegionId.ToString();
            model.ProductTypeId = entity.TfxProductTypeId;
            model.CurrentQuantity = entity.DRCreationLevel;
            model.RequiredQuantity = entity.RequiredQuantity;
            model.PreviousStatus = entity.Status;
            model.Status = entity.Status;
            model.SchedulePreviousStatus = entity.Status == DeliveryReqStatus.ScheduleCreated ? (int)DeliveryScheduleStatus.New : (int)DeliveryScheduleStatus.None;
            model.ParentId = entity.ParentId;
            model.CustomerCompany = entity.TfxCustomerCompany;
            model.JobId = entity.TfxJobId;
            model.AssignedToRegionId = string.IsNullOrEmpty(entity.TfxAssignedToRegionId) ? string.Empty : entity.TfxAssignedToRegionId;
            model.OrderId = entity.TfxOrderId;
            model.DeliveryGroupId = entity.TfxDeliveryGroupId;
            model.DeliveryScheduleId = entity.TfxDeliveryScheduleId;
            model.TrackableScheduleId = entity.TfxTrackableScheduleId;
            model.TrackScheduleStatus = entity.TfxScheduleStatus;
            model.TrackScheduleEnrouteStatus = entity.TfxScheduleEnrouteStatus;
            model.TrackScheduleStatusName = entity.TfxScheduleStatusName;
            model.TankMaxFill = entity.TankMaxFill;
            model.WindowMode = 1;
            model.QueueMode = 1;
            model.BadgeNo1 = string.IsNullOrEmpty(entity.BadgeNo1) ? string.Empty : entity.BadgeNo1;
            model.BadgeNo2 = string.IsNullOrEmpty(entity.BadgeNo2) ? string.Empty : entity.BadgeNo2;
            model.BadgeNo3 = string.IsNullOrEmpty(entity.BadgeNo3) ? string.Empty : entity.BadgeNo3;
            model.IsCommonBadge = entity.IsCommonBadge;
            model.DispactherNote = entity.DispactherNote;
            model.CarrierOrderId = entity.CarrierOrderId;

            if (entity.TfxTerminal != null && entity.TfxTerminal.Id > 0)
            {
                model.Terminal = new FreightModels.DropdownDisplayItem()
                {
                    Id = entity.TfxTerminal.Id,
                    Name = entity.TfxTerminal.Name
                };
                model.PickupLocationType = Exchange.Utilities.PickupLocationType.Terminal;
            }
            if (entity.TfxBulkPlant != null && !string.IsNullOrWhiteSpace(entity.TfxBulkPlant.SiteName))
            {
                model.BulkPlant = entity.TfxBulkPlant;
                model.PickupLocationType = Exchange.Utilities.PickupLocationType.BulkPlant;
            }
            if ((entity.AutoDRStatus == AutoDrStatus.Create) || (entity.AutoDRStatus == AutoDrStatus.CreateAndUpdate))
            {
                model.IsAutoCreatedDR = true;
            }
            model.RecurringScheduleId = string.Empty;
            model.RecurringScheduleInfo = string.Empty;
            model.RouteInfo = entity.RouteInfo;
            //Blended DRs.
            model.BlendedGroupId = entity.BlendedGroupId;
            model.IsBlendedRequest = entity.IsBlendedRequest;
            model.IsAdditive = entity.IsAdditive;
            model.QuantityInPercent = entity.QuantityInPercent;
            model.FuelTypeId = entity.TfxFuelTypeId;
            model.FuelType = model.FuelType;
            model.DeliveryLevelPO = entity.DeliveryLevelPO;

            return model;
        }
    }
}
