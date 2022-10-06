using MongoDB.Bson;
using SiteFuel.MdbDataAccess.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightRepository.Mappers
{
   public static class RecurringDeliveryScheduleMapper
    {
        public static List<DeliveryRequest> ToEntity(this List<RecurringDeliveryRequestDetails> requests)
        {
            var entityList = new List<DeliveryRequest>();
            foreach (var model in requests)
            {
                var entity = model.ToEntity();
                entityList.Add(entity);
            }
            return entityList;
        }
        public static DeliveryRequest ToEntity(this RecurringDeliveryRequestDetails model)
        {
            var entity = new DeliveryRequest();
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
            entity.CreatedRegionId = model.CreatedRegionId;
            entity.TfxProductTypeId = model.TfxProductTypeId;
            entity.DRCreationLevel = model.DRCreationLevel;
            entity.RequiredQuantity = model.RequiredQuantity;
            entity.Priority = model.Priority;
            entity.CurrentThreshold = model.CurrentThreshold;
            entity.Status = model.Status;
            entity.TfxAssignedToRegionId = model.TfxAssignedToRegionId;
            //entity.TfxAssignedToRegionId = ObjectId.Parse(model.AssignedToRegionId);
            entity.TfxJobId = model.TfxJobId;
            entity.JobTimeZoneOffset = model.JobTimeZoneOffset;
            entity.TfxCustomerCompany = model.TfxCustomerCompany;
            entity.TfxAssignedToUserId = model.TfxAssignedToUserId;
            entity.TfxOrderId = model.TfxOrderId;
            entity.CreatedOn = DateTime.Now;
            entity.UpdatedOn =DateTime.Now;
            entity.IsActive = true;
            entity.IsDeleted = false;
            entity.TfxDeliveryGroupId = model.TfxDeliveryGroupId;
            entity.TfxDeliveryScheduleId = model.TfxDeliveryScheduleId;
            entity.TfxTrackableScheduleId = model.TfxTrackableScheduleId;
            entity.AutoDRStatus = model.AutoDRStatus;
            entity.AutoCreatedOn = model.AutoCreatedOn;
            entity.AutoUpdatedOn = model.AutoUpdatedOn;
            entity.ParentId = model.ParentId;
            entity.TankMaxFill = model.TankMaxFill;
            entity.IsMaxFillAllowed = model.IsMaxFillAllowed;
            if (model.TfxTerminal != null && model.TfxTerminal.Id > 0)
            {
                entity.TfxTerminal = new DropdownDisplayItem()
                {
                    Id = model.TfxTerminal.Id,
                    Name = model.TfxTerminal.Name
                };
            }
            if (model.TfxBulkPlant != null && !string.IsNullOrWhiteSpace(model.TfxBulkPlant.SiteName))
            {
                entity.TfxBulkPlant = model.TfxBulkPlant;
            }
            entity.BadgeNo1 = model.BadgeNo1 ?? string.Empty;
            entity.BadgeNo2 = model.BadgeNo2 ?? string.Empty;
            entity.BadgeNo3 = model.BadgeNo3 ?? string.Empty;
            entity.DispactherNote = model.DispactherNote ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(entity.BadgeNo1) || !string.IsNullOrWhiteSpace(entity.BadgeNo2) || !string.IsNullOrWhiteSpace(entity.BadgeNo3))
            {
                entity.IsCommonBadge = false;
            }
            else
            {
                entity.IsCommonBadge = true;
            }
            entity.RouteInfo = model.RouteInfo;
            entity.DeliveryRequestFor = model.DeliveryRequestFor;
            entity.Notes = model.Notes;
            //Blended DRs.
            entity.BlendedGroupId = model.BlendedGroupId;
            entity.IsBlendedRequest = model.IsBlendedRequest;
            entity.IsAdditive = model.IsAdditive;
            entity.QuantityInPercent = model.QuantityInPercent;
            entity.TfxFuelTypeId = model.TfxFuelTypeId;
            entity.FuelType = model.FuelType;

            return entity;
        }

    }
}
