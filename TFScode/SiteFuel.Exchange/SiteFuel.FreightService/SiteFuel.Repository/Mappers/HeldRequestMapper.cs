using SiteFuel.FreightModels;
using SiteFuel.MdbDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightRepository.Mappers
{
    public static class HeldRequestMapper
    {


        public static HeldDeliveryRequest ToHeldDeliveryRequest(this HeldDeliveryRequestModel model)
        {
            HeldDeliveryRequest entity = new HeldDeliveryRequest()
            {
                HeldDrId = model.HeldDrId,
                UniqueOrderNo = model.UniqueOrderNo,
                Sap_OrderNo = model.Sap_OrderNo,
                IsDREdited = model.IsDREdited,
                SiteId = model.SiteId,
                TankId = model.TankId,
                StorageId = model.StorageId,
                ScheduleQuantityType = model.ScheduleQuantityType,
                RequiredQuantity = model.RequiredQuantity,
                JobId = model.JobId,
                QuantityInPercent = model.QuantityInPercent,
                IsAdditive = model.IsAdditive,
                ProductTypeId = model.ProductTypeId,
                FuelTypeId = model.FuelTypeId,
                FuelType = model.FuelType,
                JobName = model.JobName,
                JobAddress = model.JobAddress,
                JobCity = model.JobCity,
                CustomerCompany = model.CustomerCompany,
                ProductType = model.ProductType,
                UoM = model.UoM,
                CreatedByRegionId = model.CreatedByRegionId,
                AssignedToRegionId = model.AssignedToRegionId,
                Priority = model.Priority,
                CurrentThreshold = model.CurrentThreshold,
                TankMaxFill = model.TankMaxFill,
                OrderId = model.OrderId,
                DelReqSource = model.DelReqSource,
                isRecurringSchedule = model.isRecurringSchedule,
                PoNumber = model.PoNumber,
                BuyerCompanyId = model.BuyerCompanyId,
                DispactherNote = model.DispactherNote,
                BrokeredDrId = model.BrokeredDrId,
                IsDispatchRetainedByCustomer = model.IsDispatchRetainedByCustomer,
                DeliveryRequestFor = model.DeliveryRequestFor,
                Notes = model.Notes,
                isRetainInfo = model.isRetainInfo,
                isTankExists = model.isTankExists,
                IsRetainButtonClick = model.IsRetainButtonClick,
                RetainTime = model.RetainTime,
                RetainDate = model.RetainDate,
                WindowStartTime = model.WindowStartTime,
                WindowStartDate = model.WindowStartDate,
                WindowEndTime = model.WindowEndTime,
                WindowEndDate = model.WindowEndDate,
                SupplierCompanyId = model.SupplierCompanyId,
                AssignedToCompanyId = model.AssignedToCompanyId,
                RequestFromBuyerWallyBoard = model.RequestFromBuyerWallyBoard,
                BadgeNo1 = model.BadgeNo1,
                BadgeNo2 = model.BadgeNo2,
                BadgeNo3 = model.BadgeNo3,
                DispatcherNote = model.DispatcherNote,
                PickupLocationType = model.PickupLocationType,
                Bulkplant = model.Bulkplant,
                IsAcceptNightDeliveries = model.IsAcceptNightDeliveries,
                HoursToCoverDistance = model.HoursToCoverDistance,
                JobTimeZoneOffset = model.JobTimeZoneOffset,
                IsMaxFillAllowed = model.IsMaxFillAllowed,
                IsReAssignToCarrier = model.IsReAssignToCarrier,
                IsTBD = model.IsTBD,
                TBDGroupId = model.TBDGroupId,
                UserId = model.UserId,
                DeliveryDateStartTime = model.DeliveryDateStartTime,
                Vessel = model.Vessel,
                Berth = model.Berth,
                IsMarine = model.IsMarine,
                DeliveryLevelPO = model.DeliveryLevelPO,
                ScheduleStartTime = model.ScheduleStartTime,
                ScheduleEndTime = model.ScheduleEndTime,
                IsBlendedRequest = model.IsBlendedRequest,
                IsCommonPickupForBlend = model.IsCommonPickupForBlend,
                BlendedGroupId = model.BlendedGroupId,
                BlendParentProductTypeId = model.BlendParentProductTypeId,
                SelectedDate = model.SelectedDate,
                IsFutureDR = model.IsFutureDR,
                IsCalendarView = model.IsCalendarView,
                NumOfSubDrs = model.NumOfSubDrs,
                IndicativePrice = model.IndicativePrice,
                CompanyTypeId = model.CompanyTypeId,
                CreatedBy = model.CreatedBy,
                CreatedByCompanyId = model.CreatedByCompanyId,
                CreatedOn = model.CreatedOn,
                Status = model.Status,
                UpdatedBy = model.UpdatedBy,
                UpdatedByCompanyId = model.UpdatedByCompanyId,
                UpdatedOn = model.UpdatedOn,
                IsActive = true,
                ValidationMessage = model.ValidationMessage
            };
            if(model.PickupLocationType == Exchange.Utilities.PickupLocationType.BulkPlant)
            {
                entity.Bulkplant = model.Bulkplant;
                entity.Terminal = new MdbDataAccess.Collections.DropdownDisplayItem();
            }
            else if (model.Terminal != null && model.Terminal.Id > 0)
            {
                entity.Terminal = new MdbDataAccess.Collections.DropdownDisplayItem()
                {
                    Id = model.Terminal.Id,
                    Name = model.Terminal.Name
                };
                entity.Bulkplant = new BulkPlantAddressModel();
            }
            return entity;
        }

        public static HeldDeliveryRequestModel ToHeldDeliveryRequestModel(this HeldDeliveryRequest model)
        {
            HeldDeliveryRequestModel entity = new HeldDeliveryRequestModel()
            {
                HeldDrId = model.Id.ToString(),
                UniqueOrderNo = model.UniqueOrderNo,
                IsDREdited = model.IsDREdited,
                Sap_OrderNo = model.Sap_OrderNo,
                SiteId = model.SiteId,
                TankId = model.TankId,
                StorageId = model.StorageId,
                CreditApprovalFilePath = model.CreditApprovalFilePath,
                FileName = model.FileName,
                ScheduleQuantityType = model.ScheduleQuantityType,
                RequiredQuantity = model.RequiredQuantity,
                JobId = model.JobId,
                QuantityInPercent = model.QuantityInPercent,
                IsAdditive = model.IsAdditive,
                ProductTypeId = model.ProductTypeId,
                FuelTypeId = model.FuelTypeId,
                FuelType = model.FuelType,
                JobName = model.JobName,
                JobAddress = model.JobAddress,
                JobCity = model.JobCity,
                CustomerCompany = model.CustomerCompany,
                ProductType = model.ProductType,
                UoM = model.UoM,
                CreatedByRegionId = model.CreatedByRegionId,
                AssignedToRegionId = model.AssignedToRegionId,
                Priority = model.Priority,
                CurrentThreshold = model.CurrentThreshold,
                TankMaxFill = model.TankMaxFill,
                OrderId = model.OrderId,
                DelReqSource = model.DelReqSource,
                isRecurringSchedule = model.isRecurringSchedule,
                PoNumber = model.PoNumber,
                BuyerCompanyId = model.BuyerCompanyId,
                DispactherNote = model.DispactherNote,
                BrokeredDrId = model.BrokeredDrId,
                IsDispatchRetainedByCustomer = model.IsDispatchRetainedByCustomer,
                DeliveryRequestFor = model.DeliveryRequestFor,
                Notes = model.Notes,
                isRetainInfo = model.isRetainInfo,
                isTankExists = model.isTankExists,
                IsRetainButtonClick = model.IsRetainButtonClick,
                RetainTime = model.RetainTime,
                RetainDate = model.RetainDate,
                WindowStartTime = model.WindowStartTime,
                WindowStartDate = model.WindowStartDate,
                WindowEndTime = model.WindowEndTime,
                WindowEndDate = model.WindowEndDate,
                SupplierCompanyId = model.SupplierCompanyId,
                AssignedToCompanyId = model.AssignedToCompanyId,
                RequestFromBuyerWallyBoard = model.RequestFromBuyerWallyBoard,
                BadgeNo1 = model.BadgeNo1,
                BadgeNo2 = model.BadgeNo2,
                BadgeNo3 = model.BadgeNo3,
                DispatcherNote = model.DispatcherNote,
                PickupLocationType = model.PickupLocationType,
                Bulkplant = model.Bulkplant,
                IsAcceptNightDeliveries = model.IsAcceptNightDeliveries,
                HoursToCoverDistance = model.HoursToCoverDistance,
                JobTimeZoneOffset = model.JobTimeZoneOffset,
                IsMaxFillAllowed = model.IsMaxFillAllowed,
                IsReAssignToCarrier = model.IsReAssignToCarrier,
                IsTBD = model.IsTBD,
                TBDGroupId = model.TBDGroupId,
                UserId = model.UserId,
                DeliveryDateStartTime = model.DeliveryDateStartTime,
                Vessel = model.Vessel,
                Berth = model.Berth,
                IsMarine = model.IsMarine,
                DeliveryLevelPO = model.DeliveryLevelPO,
                ScheduleStartTime = model.ScheduleStartTime,
                ScheduleEndTime = model.ScheduleEndTime,
                IsBlendedRequest = model.IsBlendedRequest,
                IsCommonPickupForBlend = model.IsCommonPickupForBlend,
                BlendedGroupId = model.BlendedGroupId,
                BlendParentProductTypeId = model.BlendParentProductTypeId,
                SelectedDate = model.SelectedDate,
                IsFutureDR = model.IsFutureDR,
                IsCalendarView = model.IsCalendarView,
                NumOfSubDrs = model.NumOfSubDrs,
                IndicativePrice = model.IndicativePrice,
                CompanyTypeId = model.CompanyTypeId,
                CreatedBy = model.CreatedBy,
                CreatedByCompanyId = model.CreatedByCompanyId,
                CreatedOn = model.CreatedOn,
                Status = model.Status,
                UpdatedBy = model.UpdatedBy,
                UpdatedByCompanyId = model.UpdatedByCompanyId,
                UpdatedOn = model.UpdatedOn,
                IsActive = true,
                ValidationMessage = model.ValidationMessage
            };
            if (model.Terminal != null)
            {
                entity.Terminal = new FreightModels.DropdownDisplayItem()
                {
                    Id = model.Terminal.Id,
                    Name = model.Terminal.Name
                };
            }

            return entity;
        }
    }
}
