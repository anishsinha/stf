using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class DeliveryRequestMapper
    {
        public static RaiseDeliveryRequestViewModel ToRaiseDrViewModel(this DeliveryRequestViewModel model)
        {
            var entity = new RaiseDeliveryRequestViewModel();
            entity.SiteId = model.SiteId;
            entity.TankId = model.TankId;
            entity.StorageId = model.StorageId;
            entity.RequiredQuantity = model.RequiredQuantity;
            entity.ScheduleQuantityType = model.ScheduleQuantityType;
            entity.JobId = model.JobId;
            entity.ProductTypeId = model.ProductTypeId;
            entity.JobName = model.JobName;
            entity.JobAddress = model.JobAddress;
            entity.JobCity = model.JobCity;
            entity.CustomerCompany = model.CustomerCompany;
            entity.ProductType = model.ProductType;
            entity.UoM = model.UoM;
            entity.Terminal = model.Terminal != null && model.Terminal.Id > 0 ? model.Terminal : null;
            entity.CreatedByRegionId = model.CreatedByRegionId;
            entity.AssignedToRegionId = model.AssignedToRegionId;
            entity.Priority = model.Priority;
            //entity.CurrentThreshold  = model.
            entity.TankMaxFill = model.TankMaxFill;
            entity.OrderId = model.OrderId;
            entity.DelReqSource = DRSource.Manual;
            //entity.PoNumber  = model.
            //entity.BuyerCompanyId  = model.bu
            //entity.TankName  = model.t
            //entity.CreatedBy  = model.cre
            entity.CreatedByCompanyId = model.CreatedByCompanyId;
            entity.CreatedOn = DateTimeOffset.Now;
            entity.Status = DeliveryReqStatus.Pending;
            entity.IsActive = true;
            entity.IsDeleted = false;
            entity.Notes = model.Notes;
            //entity.AssignedTo = model.
            entity.AssignedToCompanyId = model.AssignedToCompanyId;
            entity.SupplierCompanyId = model.SupplierCompanyId;
            return entity;
        }

        public static HeldDeliveryRequestModel ToHeldDrViewModel(this RaiseDeliveryRequestInput input)
        {
            HeldDeliveryRequestModel model = new HeldDeliveryRequestModel()
            {
                AssignedToRegionId = input.AssignedToRegionId,
                BadgeNo1 = input.BadgeNo1,
                BadgeNo2 = input.BadgeNo2,
                BadgeNo3 = input.BadgeNo3,
                Berth = input.Berth,
                BrokeredDrId = input.BrokeredDrId,
                Bulkplant = input.Bulkplant,
                BuyerCompanyId = input.BuyerCompanyId,
                CreatedByRegionId = input.CreatedByRegionId,
                CurrentThreshold = input.CurrentThreshold,
                CustomerCompany = input.CustomerCompany,
                DeliveryDateStartTime = input.DeliveryDateStartTime,
                DeliveryLevelPO = input.DeliveryLevelPO,
                DeliveryRequestFor = input.DeliveryRequestFor,
                DelReqSource = input.DelReqSource,
                DispactherNote = input.DispactherNote,
                DispatcherNote = input.DispatcherNote,
                FuelType = input.FuelType,
                FuelTypeId = input.FuelTypeId,
                HoursToCoverDistance = input.HoursToCoverDistance,
                IndicativePrice = input.IndicativePrice,
                IsAcceptNightDeliveries = input.IsAcceptNightDeliveries,
                IsCalendarView = input.IsCalendarView,
                IsDispatchRetainedByCustomer = input.IsDispatchRetainedByCustomer,
                IsFutureDR = input.IsFutureDR,
                IsMarine = input.IsMarine,
                IsMaxFillAllowed = input.IsMaxFillAllowed,
                IsReAssignToCarrier = input.IsReAssignToCarrier,
                ScheduleQuantityType = input.ScheduleQuantityType,
                ScheduleEndTime = input.ScheduleEndTime,
                BlendedGroupId = input.BlendedGroupId,
                RetainTime = input.RetainTime,
                BlendedRequests = input.BlendedRequests,
                BlendParentProductTypeId = input.BlendParentProductTypeId,
                IsAdditive = input.IsAdditive,
                IsBlendedRequest = input.IsBlendedRequest,
                IsCommonPickupForBlend = input.IsCommonPickupForBlend,
                isRecurringSchedule = input.isRecurringSchedule,
                IsRetainButtonClick = input.IsRetainButtonClick,
                isRetainInfo = input.isRetainInfo,
                isTankExists = input.isTankExists,
                IsTBD = input.IsTBD,
                JobAddress = input.JobAddress,
                JobCity = input.JobCity,
                JobId = input.JobId,
                JobName = input.JobName,
                JobTimeZoneOffset = input.JobTimeZoneOffset,
                Notes = input.Notes,
                NumOfSubDrs = input.NumOfSubDrs,
                OrderId = input.OrderId,
                PickupLocationType = input.PickupLocationType,
                PoNumber = input.PoNumber,
                Priority = input.Priority,
                ProductType = input.ProductType,
                ProductTypeId = input.ProductTypeId,
                QuantityInPercent = input.QuantityInPercent,
                RecurringSchdule = input.RecurringSchdule,
                RequestFromBuyerWallyBoard = input.RequestFromBuyerWallyBoard,
                RequiredQuantity = input.RequiredQuantity,
                RetainDate = input.RetainDate,
                TBDGroupId = input.TBDGroupId,
                ScheduleStartTime = input.ScheduleStartTime,
                SelectedDate = input.SelectedDate,
                SiteId = input.SiteId,
                StorageId = input.StorageId,
                SupplierCompanyId = input.SupplierCompanyId,
                TankId = input.TankId,
                TankMaxFill = input.TankMaxFill,
                Terminal = input.Terminal,
                TrailerTypes = input.TrailerTypes,
                UoM = input.UoM,
                UserId = input.UserId,
                Vessel = input.Vessel,
                WindowEndDate = input.WindowEndDate,
                WindowEndTime = input.WindowEndTime,
                WindowStartDate = input.WindowStartDate,
                WindowStartTime = input.WindowStartTime,
            };
            return model;
        }
    }
}
