using MongoDB.Bson;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.FreightModels.DeliveryRequest;
using SiteFuel.FreightModels.ScheduleBuilder;
using SiteFuel.MdbDataAccess.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SiteFuel.FreightRepository.Mappers
{
    public static class DeliveryRequestMapper
    {
        public static List<DeliveryRequest> ToEntity(this List<DeliveryRequestViewModel> requests)
        {
            var entityList = new List<DeliveryRequest>();
            foreach (var model in requests)
            {
                if ((model.ScheduleQuantityType == (int)ScheduleQuantityType.Quantity
                        && model.RequiredQuantity > 0) || model.ScheduleQuantityType != (int)ScheduleQuantityType.Quantity)
                {
                    var entity = model.ToEntity();
                    entityList.Add(entity);
                }
            }
            return entityList;
        }

        public static DeliveryRequest ToEntity(this DeliveryRequestViewModel model)
        {
            var entity = new DeliveryRequest();
            entity.TfxJobAddress = model.JobAddress;
            entity.TfxJobCity = model.JobCity;
            entity.TfxJobName = model.JobName;
            entity.TfxProductType = model.ProductType;
            entity.TfxUoM = model.UoM;
            entity.BlendedGroupId = model.BlendedGroupId;
            entity.BlendParentProductTypeId = model.BlendParentProductTypeId;
            entity.IsBlendedRequest = model.IsBlendedRequest;
            entity.DelReqSource = model.DelReqSource;
            entity.IsAdditive = model.IsAdditive;
            entity.TfxCreatedByCompanyId = model.CreatedByCompanyId;
            entity.TfxAssignedToCompanyId = model.AssignedToCompanyId;
            entity.TfxSupplierCompanyId = model.SupplierCompanyId;
            entity.TfxDisplayJobId = model.SiteId;
            entity.CreditApprovalFilePath = model.CreditApprovalFilePath;
            entity.StorageId = model.StorageId;
            entity.StorageTypeId = model.TankId;
            entity.NumOfSubDrs = model.NumOfSubDrs;
            entity.Sap_OrderNo = model.Sap_OrderNo;
            entity.UniqueOrderNo = model.UniqueOrderNo;
            if (!string.IsNullOrWhiteSpace(model.CreatedByRegionId))
            {
                entity.CreatedRegionId = ObjectId.Parse(model.CreatedByRegionId);
            }
            entity.TfxProductTypeId = model.ProductTypeId;
            entity.TfxFuelTypeId = model.FuelTypeId;
            entity.FuelType = model.FuelType;
            entity.QuantityInPercent = model.QuantityInPercent;
            entity.DRCreationLevel = model.CurrentQuantity;
            entity.RequiredQuantity = model.RequiredQuantity;
            entity.Priority = model.Priority;
            entity.CurrentThreshold = model.CurrentThreshold;
            entity.Status = model.Status;
            entity.TfxAssignedToRegionId = model.AssignedToRegionId;
            //entity.TfxAssignedToRegionId = ObjectId.Parse(model.AssignedToRegionId);
            entity.TfxJobId = model.JobId;
            entity.JobTimeZoneOffset = model.JobTimeZoneOffset;
            entity.TfxCustomerCompany = model.CustomerCompany;
            entity.TfxAssignedToUserId = model.CreatedBy;
            entity.TfxOrderId = model.OrderId;
            entity.CreatedBy = model.CreatedBy;
            entity.CreatedOn = model.CreatedOn;
            entity.UpdatedBy = model.CreatedBy;
            entity.UpdatedOn = model.CreatedOn;
            entity.AssignedOn = DateTimeOffset.Now.Add(model.JobTimeZoneOffset);
            entity.IsActive = model.IsActive;
            entity.IsDeleted = model.IsDeleted;
            entity.TfxDeliveryGroupId = model.DeliveryGroupId;
            entity.TfxDeliveryScheduleId = model.DeliveryScheduleId;
            entity.TfxTrackableScheduleId = model.TrackableScheduleId;
            entity.AutoDRStatus = model.AutoDRStatus;
            entity.AutoCreatedOn = model.AutoCreatedOn;
            entity.AutoUpdatedOn = model.AutoUpdatedOn;
            entity.ParentId = model.ParentId;
            entity.TankMaxFill = model.TankMaxFill;
            entity.IsMaxFillAllowed = model.IsMaxFillAllowed;
            entity.CarrierOrderId = model.CarrierOrderId;
            entity.ExternalRefId = model.ExternalRefId;
            if (!model.IsAdditive)
            {
                if (model.Terminal != null && model.Terminal.Id > 0)
                {
                    entity.TfxTerminal = new MdbDataAccess.Collections.DropdownDisplayItem()
                    {
                        Id = model.Terminal.Id,
                        Name = model.Terminal.Name
                    };
                }
                if (model.BulkPlant != null && !string.IsNullOrWhiteSpace(model.BulkPlant.SiteName))
                {
                    entity.TfxBulkPlant = model.BulkPlant;
                }
            }
            else
            {
                entity.TfxTerminal = new MdbDataAccess.Collections.DropdownDisplayItem();
                entity.TfxBulkPlant = new BulkPlantAddressModel();
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
            if (!string.IsNullOrWhiteSpace(model.PreLoadedFor))
            {
                entity.PreLoadedFor = ObjectId.Parse(model.PreLoadedFor);
            }
            if (!string.IsNullOrWhiteSpace(model.PostLoadedFor))
            {
                entity.PostLoadedFor = ObjectId.Parse(model.PostLoadedFor);
            }
            entity.PreLoadInfo = model.PreLoadInfo;
            entity.PostLoadInfo = model.PostLoadInfo;
            if (String.IsNullOrEmpty(model.RecurringScheduleId))
            {
                model.RecurringScheduleId = ObjectId.Empty.ToString();
            }
            entity.RecurringScheduleId = !model.isRecurringSchedule ? ObjectId.Empty : ObjectId.Parse(model.RecurringScheduleId);
            entity.RouteInfo = model.RouteInfo;
            entity.IsRecurringSchedule = model.isRecurringSchedule;
            entity.ScheduleQuantityType = model.ScheduleQuantityType;
            entity.BrokeredParentId = model.BrokeredDrId;
            entity.IsDispatchRetainedByCustomer = model.IsDispatchRetainedByCustomer;
            entity.Compartments = new List<CompartmentsInfo>();
            entity.DeliveryRequestFor = model.DeliveryRequestFor;
            entity.Notes = model.Notes;
            entity.DeliveryWindowInfo = model.DeliveryWindowInfo?.ToEntity();
            ObjectId groupParentId = ObjectId.Empty;
            bool status = ObjectId.TryParse(model.GroupParentDRId, out groupParentId);
            if (status)
            {
                entity.GroupParentDRId = groupParentId;
            }
            else
            {
                entity.GroupParentDRId = null;
            }
            if (model.GroupChildDRs.Any())
            {
                model.GroupChildDRs.ForEach(x => entity.GroupChildDRs.Add(ObjectId.Parse(x)));
            }
            entity.IsTBD = model.IsTBD;
            entity.TBDGroupId = model.TBDGroupId;
            //Marine Nomination Changes.
            entity.DeliveryDateStartTime = model.DeliveryDateStartTime;
            entity.Vessel = model.Vessel;
            entity.Berth = model.Berth;
            entity.IsMarine = model.IsMarine;
            entity.SelectedDate = DateTime.TryParse(model.SelectedDate, out DateTime sdate) ? sdate : (DateTime?)null;
            entity.IsFutureDR = model.IsFutureDR;
            entity.IsCalendarView = model.IsCalendarView;
            entity.IsDispatcherDragDrop = model.IsDispatcherDragDrop;
            entity.DispatcherDragDropSequence = model.DispatcherDragDropSequence;
            entity.DeliveryLevelPO = model.DeliveryLevelPO;
            if (!string.IsNullOrWhiteSpace(model.ScheduleStartTime))
                entity.ScheduleStartTime = Convert.ToDateTime(model.ScheduleStartTime).TimeOfDay;
            if (!string.IsNullOrWhiteSpace(model.ScheduleEndTime))
                entity.ScheduleEndTime = Convert.ToDateTime(model.ScheduleEndTime).TimeOfDay;
            entity.IndicativePrice = model.IndicativePrice;
            return entity;
        }

        public static DeliveryWindowInfo ToEntity(this DeliveryWindowInfoModel model)
        {
            var entity = new DeliveryWindowInfo();
            entity.RetainDate = model.RetainDate.Date;
            entity.RetainTime = model.RetainTime;
            entity.StartDate = model.StartDate.Date;
            entity.StartTime = model.StartTime;
            entity.EndDate = model.EndDate.Date;
            entity.EndTime = model.EndTime;
            return entity;
        }

        public static DeliveryRequest CloneEntity(this DeliveryRequest model)
        {
            var entity = new DeliveryRequest();
            entity.TfxJobAddress = model.TfxJobAddress;
            entity.TfxJobCity = model.TfxJobCity;
            entity.TfxJobName = model.TfxJobName;
            entity.TfxProductType = model.TfxProductType;
            entity.TfxUoM = model.TfxUoM;
            entity.BlendParentProductTypeId = model.BlendParentProductTypeId;
            entity.IsBlendedRequest = model.IsBlendedRequest;
            entity.IsAdditive = model.IsAdditive;
            entity.QuantityInPercent = model.QuantityInPercent;
            entity.TfxCreatedByCompanyId = model.TfxCreatedByCompanyId;
            entity.TfxDisplayJobId = model.TfxDisplayJobId;
            entity.StorageTypeId = model.StorageTypeId;
            entity.StorageId = model.StorageId;
            entity.TfxProductTypeId = model.TfxProductTypeId;
            entity.DRCreationLevel = model.DRCreationLevel;
            entity.RequiredQuantity = model.RequiredQuantity;
            entity.Priority = model.Priority;
            entity.TfxFuelTypeId = model.TfxFuelTypeId;
            entity.FuelType = model.FuelType;
            entity.IsTBD = model.IsTBD;
            entity.TBDGroupId = model.TBDGroupId;
            entity.CurrentThreshold = model.CurrentThreshold;
            entity.Status = model.Status;
            entity.JobTimeZoneOffset = model.JobTimeZoneOffset;
            entity.TfxScheduleStatus = model.TfxScheduleStatus;
            entity.TfxScheduleEnrouteStatus = model.TfxScheduleEnrouteStatus;
            entity.TfxAssignedToUserId = model.TfxAssignedToUserId;
            entity.TfxAssignedToCompanyId = model.TfxAssignedToCompanyId;
            entity.TfxAssignedToRegionId = model.TfxAssignedToRegionId;
            entity.TfxSupplierCompanyId = model.TfxSupplierCompanyId;
            entity.CreatedRegionId = model.CreatedRegionId;
            entity.ParentId = model.ParentId;
            entity.TfxJobId = model.TfxJobId;
            entity.TfxCustomerCompany = model.TfxCustomerCompany;
            entity.TfxDeliveryGroupId = model.TfxDeliveryGroupId;
            entity.TfxDeliveryScheduleId = model.TfxDeliveryScheduleId;
            entity.TfxTrackableScheduleId = model.TfxTrackableScheduleId;
            entity.TfxOrderId = model.TfxOrderId;
            entity.TfxTerminal = model.TfxTerminal;
            entity.TfxBulkPlant = model.TfxBulkPlant;
            entity.AutoDRStatus = model.AutoDRStatus;
            entity.AutoCreatedOn = model.AutoCreatedOn;
            entity.AutoUpdatedOn = model.AutoUpdatedOn;
            entity.DelReqSource = DRSource.PreLoad;
            entity.TfxScheduleStatusName = model.TfxScheduleStatusName;
            entity.TankMaxFill = model.TankMaxFill;
            entity.IsMaxFillAllowed = model.IsMaxFillAllowed;
            entity.DeliveryRequestType = model.DeliveryRequestType;
            entity.PostLoadedFor = model.PostLoadedFor;
            entity.PreLoadedFor = model.PreLoadedFor;
            entity.BadgeNo1 = string.IsNullOrEmpty(model.BadgeNo1) ? string.Empty : model.BadgeNo1;
            entity.BadgeNo2 = string.IsNullOrEmpty(model.BadgeNo2) ? string.Empty : model.BadgeNo2;
            entity.BadgeNo3 = string.IsNullOrEmpty(model.BadgeNo3) ? string.Empty : model.BadgeNo3;
            entity.IsCommonBadge = model.IsCommonBadge;
            entity.DispactherNote = model.DispactherNote;
            entity.RouteInfo = model.RouteInfo;
            entity.BrokeredParentId = model.BrokeredParentId;
            entity.BrokeredChildId = model.BrokeredChildId;
            entity.CarrierStatus = model.CarrierStatus;
            entity.CarrierOrderId = model.CarrierOrderId;
            entity.ScheduleQuantityType = model.ScheduleQuantityType;
            entity.Compartments = model.Compartments;
            entity.DeliveryRequestFor = model.DeliveryRequestFor;
            entity.GroupParentDRId = model.GroupParentDRId;
            entity.GroupChildDRs = model.GroupChildDRs;
            entity.IsActive = true;
            entity.IsDeleted = false;
            entity.DeliveryLevelPO = model.DeliveryLevelPO;
            entity.IndicativePrice = model.IndicativePrice;
            return entity;
        }

        public static DeliveryRequestViewModel ToDeliveryRequestViewModel(this DeliveryRequest entity)
        {
            var model = new DeliveryRequestViewModel();
            if (entity == null)
            {
                return model;
            }
            if (entity.Id != BsonNull.Value)
            {
                model.Id = entity.Id.ToString();
            }
            model.ScheduleShiftEndTime = entity.ScheduleShiftEndDateTime;
            model.JobAddress = entity.TfxJobAddress;
            model.IsAdditive = entity.IsAdditive;
            model.Sap_OrderNo = entity.Sap_OrderNo;
            model.UniqueOrderNo = entity.UniqueOrderNo;
            model.IsBlendedRequest = entity.IsBlendedRequest;
            model.BlendedGroupId = entity.BlendedGroupId;
            model.CreditApprovalFilePath = entity.CreditApprovalFilePath;
            model.BlendParentProductTypeId = entity.BlendParentProductTypeId;
            model.QuantityInPercent = entity.QuantityInPercent;
            model.JobCity = string.IsNullOrEmpty(entity.TfxJobCity) ? string.Empty : entity.TfxJobCity;
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
            model.CreatedByRegionId = entity.CreatedRegionId.ToString();
            model.ProductTypeId = entity.TfxProductTypeId;
            model.FuelTypeId = entity.TfxFuelTypeId;
            model.FuelType = entity.FuelType;
            model.CurrentQuantity = entity.DRCreationLevel;
            model.RequiredQuantity = entity.RequiredQuantity;
            model.PreviousStatus = entity.Status;
            model.Status = entity.Status;
            model.CurrentThreshold = entity.CurrentThreshold ?? 0;
            model.SchedulePreviousStatus = entity.Status == DeliveryReqStatus.ScheduleCreated ? (int)DeliveryScheduleStatus.New : (int)DeliveryScheduleStatus.None;
            model.ParentId = entity.ParentId;
            model.CustomerCompany = entity.TfxCustomerCompany;
            model.JobId = entity.TfxJobId;
            model.AssignedToRegionId = string.IsNullOrEmpty(entity.TfxAssignedToRegionId) ? string.Empty : entity.TfxAssignedToRegionId;
            model.OrderId = entity.TfxOrderId;
            model.IsFilldInvoke = entity.IsFilldInvoke;
            model.DeliveryGroupId = entity.TfxDeliveryGroupId;
            model.DeliveryScheduleId = entity.TfxDeliveryScheduleId;
            model.TrackableScheduleId = entity.TfxTrackableScheduleId;
            model.TrackScheduleStatus = entity.TfxScheduleStatus;
            model.TrackScheduleEnrouteStatus = entity.TfxScheduleEnrouteStatus;
            model.TrackScheduleStatusName = entity.TfxScheduleStatusName;
            model.TankMaxFill = entity.TankMaxFill;
            model.IsMaxFillAllowed = entity.IsMaxFillAllowed;
            model.WindowMode = 1;
            model.QueueMode = 1;
            model.BadgeNo1 = string.IsNullOrEmpty(entity.BadgeNo1) ? string.Empty : entity.BadgeNo1;
            model.BadgeNo2 = string.IsNullOrEmpty(entity.BadgeNo2) ? string.Empty : entity.BadgeNo2;
            model.BadgeNo3 = string.IsNullOrEmpty(entity.BadgeNo3) ? string.Empty : entity.BadgeNo3;
            model.IsCommonBadge = entity.IsCommonBadge;
            model.DispactherNote = entity.DispactherNote;
            model.PreLoadInfo = entity.PreLoadInfo;
            model.PostLoadInfo = entity.PostLoadInfo;
            model.CarrierOrderId = entity.CarrierOrderId;
            model.ExternalRefId = entity.ExternalRefId;
            model.JobTimeZoneOffset = entity.JobTimeZoneOffset;
            if (entity.PreLoadedFor.HasValue)
            {
                model.PreLoadedFor = entity.PreLoadedFor.ToString();
            }
            if (entity.PostLoadedFor.HasValue)
            {
                model.PostLoadedFor = entity.PostLoadedFor.ToString();
            }
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
            model.isRecurringSchedule = entity.IsRecurringSchedule;
            model.RecurringScheduleId = entity.RecurringScheduleId.ToString();
            model.ScheduleQuantityType = entity.ScheduleQuantityType;
            if (entity.IsRecurringSchedule)
            {

                model.ScheduleQuantityTypeText = EnumHelperMethods.GetDisplayName((ScheduleQuantityType)entity.ScheduleQuantityType);
            }
            else
            {
                if (model.ScheduleQuantityType > 1)
                {
                    model.ScheduleQuantityTypeText = EnumHelperMethods.GetDisplayName((ScheduleQuantityType)entity.ScheduleQuantityType);
                }
                else
                {
                    model.ScheduleQuantityTypeText = string.Empty;
                }
            }
            model.RecurringScheduleInfo = string.Empty;
            model.BrokeredDrId = entity.BrokeredChildId;
            model.RouteInfo = entity.RouteInfo;
            model.CarrierStatus = entity.CarrierStatus;
            model.IsDispatchRetainedByCustomer = entity.IsDispatchRetainedByCustomer;
            model.DeliveryRequestFor = entity.DeliveryRequestFor;
            model.Compartments = new List<CompartmentsInfoViewModel>();
            if (entity.Compartments.Count > 0)
            {
                model.Compartments = entity.Compartments.ToCloneEntity();
            }
            model.Notes = entity.Notes;
            model.IsRetainFuelLoaded = entity.IsRetainFuelLoaded;
            if (entity.DeliveryWindowInfo != null)
            {
                if (!string.IsNullOrEmpty(entity.DeliveryWindowInfo.StartTime) && !string.IsNullOrEmpty(entity.DeliveryWindowInfo.EndTime) && !string.IsNullOrEmpty(entity.DeliveryWindowInfo.RetainTime))
                {
                    model.DeliveryWindow = entity.DeliveryWindowInfo.StartDate.ToString(Resource.constFormatDate)
                                            + " " + Convert.ToDateTime(entity.DeliveryWindowInfo.StartTime.ToString()).ToShortTimeString()
                                            + " - " + Convert.ToDateTime(entity.DeliveryWindowInfo.EndTime.ToString()).ToShortTimeString();
                    model.DeliveryWindowInfo = new DeliveryWindowInfoModel()
                    {
                        RetainDate = entity.DeliveryWindowInfo.RetainDate,
                        RetainTime = entity.DeliveryWindowInfo.RetainTime,
                        StartDate = entity.DeliveryWindowInfo.StartDate,
                        StartTime = entity.DeliveryWindowInfo.StartTime,
                        EndDate = entity.DeliveryWindowInfo.EndDate,
                        EndTime = entity.DeliveryWindowInfo.EndTime
                    };
                }
            }
            if (!string.IsNullOrEmpty(model.PreLoadedFor))
            {
                if (model.TrackScheduleEnrouteStatus == 16 || model.TrackScheduleStatus == 7 || model.TrackScheduleStatus == 8 || model.TrackScheduleStatus == 9 || model.TrackScheduleEnrouteStatus == 21 || model.TrackScheduleStatus == 25)
                {
                    model.IsPreloadDisable = true;
                }
            }
            model.DelReqSource = entity.DelReqSource;
            model.GroupParentDRId = string.Empty;
            model.CurrentInventory = Resource.lblHyphen;
            model.Ullage = "0";
            if (entity.GroupParentDRId != null)
            {
                model.GroupParentDRId = entity.GroupParentDRId.ToString();
            }
            if (entity.GroupChildDRs.Any())
            {
                entity.GroupChildDRs.ForEach(x => model.GroupChildDRs.Add(x.ToString()));
            }
            model.CreatedOn = entity.CreatedOn;
            model.AssignedOn = entity.AssignedOn;
            model.IsTBD = entity.IsTBD;
            model.TBDGroupId = entity.TBDGroupId;
            SetSubDRVisibleFlag(entity, model);
            //Marine Nomination.
            model.DeliveryDateStartTime = entity.DeliveryDateStartTime;
            model.Vessel = entity.Vessel;
            model.Berth = entity.Berth;
            model.IsMarine = entity.IsMarine;
            model.SelectedDate = entity.SelectedDate?.Date.ToString("MM/dd/yyyy");
            model.IsFutureDR = entity.IsFutureDR;
            model.IsCalendarView = entity.IsCalendarView;
            model.IsDispatcherDragDrop = entity.IsDispatcherDragDrop;
            model.DispatcherDragDropSequence = entity.DispatcherDragDropSequence;
            model.DeliveryLevelPO = entity.DeliveryLevelPO;
            model.IndicativePrice = entity.IndicativePrice;
            if (entity.ScheduleStartTime != null)
            {
                model.ScheduleStartTime = Convert.ToDateTime(entity.ScheduleStartTime.ToString()).ToShortTimeString();
            }
            if (entity.ScheduleEndTime != null)
            {
                model.ScheduleEndTime = Convert.ToDateTime(entity.ScheduleEndTime.ToString()).ToShortTimeString();
            }
            model.UniqueOrderNo = entity.UniqueOrderNo;
            model.BrokeredParentId = entity.BrokeredParentId;
            return model;
        }

        public static void GetBlendDRInfo(this List<DeliveryRequestViewModel> clonedDrs, List<DeliveryRequestViewModel> drModels, bool isGetDrs)
        {
            drModels.ForEach(dr =>
            {
                if (!dr.IsBlendedRequest || !clonedDrs.Any(t => t.BlendedGroupId == dr.BlendedGroupId))
                {
                    if (dr.IsBlendedRequest)
                    {
                        var blendedDrs = drModels.Where(t => t.BlendedGroupId == dr.BlendedGroupId);
                        if (!isGetDrs || !blendedDrs.All(t => t.IsAdditive))
                        {
                            foreach (var blendDr in blendedDrs.Where(t => !t.IsAdditive))
                            {
                                SetBlendDRInfo(clonedDrs, blendedDrs, blendDr);
                            }
                            foreach (var additiveDr in blendedDrs.Where(t => t.IsAdditive))
                            {
                                SetBlendDRInfo(clonedDrs, blendedDrs, additiveDr);
                            }
                        }
                        else if (blendedDrs.All(t => t.IsAdditive))
                        {
                            foreach (var additive in blendedDrs)
                            {
                                SetBlendDRInfo(clonedDrs, blendedDrs, additive);
                            }
                        }
                    }
                    else
                    {
                        clonedDrs.Add(dr);
                    }
                }
            });
        }

        private static void SetBlendDRInfo(List<DeliveryRequestViewModel> clonedDrs, IEnumerable<DeliveryRequestViewModel> blendedDrs, DeliveryRequestViewModel blendDr)
        {
            if (!clonedDrs.Any(t => t.BlendedGroupId == blendDr.BlendedGroupId) && (!blendDr.IsAdditive || !blendedDrs.Any(t => t.BlendedGroupId == blendDr.BlendedGroupId && !t.IsAdditive)))
            {
                blendDr.IsBlendedDrParent = true;
            }
            blendDr.BlendedProductName = string.Join(", ", blendedDrs.Where(t => !t.IsAdditive).Select(t => t.ProductType).Distinct().ToList());
            blendDr.AdditiveProductName = string.Join(", ", blendedDrs.Where(t => t.IsAdditive).Select(t => t.FuelType).ToList());
            blendDr.TotalBlendedQuantity = blendedDrs.Sum(t => t.RequiredQuantity);
            clonedDrs.Add(blendDr);
        }

        private static void SetSubDRVisibleFlag(DeliveryRequest entity, DeliveryRequestViewModel model)
        {
            if (entity.ScheduleQuantityType == (int)ScheduleQuantityType.Quantity && entity.GroupChildDRs.Count() > 0)
            {
                model.IsSpiltDRIconVisible = false;
            }
            else if (entity.IsRecurringSchedule)
            {
                model.IsSpiltDRIconVisible = false;
            }
            else if (entity.GroupParentDRId != null)
            {
                model.IsSpiltDRIconVisible = false;
            }
            else if (model.ScheduleQuantityType > 1)
            {
                model.IsSpiltDRIconVisible = false;
            }
        }

        public static DeliveryRequest ToEntity(this DeliveryRequest model)
        {
            var entity = new DeliveryRequest();
            entity.TfxJobAddress = model.TfxJobAddress;
            entity.TfxJobCity = model.TfxJobCity;
            entity.TfxJobName = model.TfxJobName;
            entity.TfxProductType = model.TfxProductType;
            entity.TfxUoM = model.TfxUoM;
            entity.IsAdditive = model.IsAdditive;
            entity.IsBlendedRequest = model.IsBlendedRequest;
            entity.BlendParentProductTypeId = model.BlendParentProductTypeId;
            entity.QuantityInPercent = model.QuantityInPercent;
            entity.TfxCreatedByCompanyId = model.TfxCreatedByCompanyId;
            entity.TfxAssignedToCompanyId = model.TfxAssignedToCompanyId;
            entity.TfxSupplierCompanyId = model.TfxSupplierCompanyId;
            entity.TfxDisplayJobId = model.TfxDisplayJobId;
            entity.StorageId = model.StorageId;
            entity.StorageTypeId = model.StorageTypeId;
            entity.CreatedRegionId = model.CreatedRegionId;
            entity.TfxProductTypeId = model.TfxProductTypeId;
            entity.TfxFuelTypeId = model.TfxFuelTypeId;
            entity.FuelType = model.FuelType;
            entity.IsTBD = model.IsTBD;
            entity.TBDGroupId = model.TBDGroupId;
            entity.DRCreationLevel = model.DRCreationLevel;
            entity.RequiredQuantity = model.RequiredQuantity;
            entity.Priority = model.Priority;
            entity.CurrentThreshold = model.CurrentThreshold;
            entity.Status = DeliveryReqStatus.Assigned;
            entity.TfxAssignedToRegionId = model.TfxAssignedToRegionId;
            entity.TfxJobId = model.TfxJobId;
            entity.TfxCustomerCompany = model.TfxCustomerCompany;
            entity.TfxAssignedToUserId = model.TfxAssignedToUserId;
            entity.TfxOrderId = model.TfxOrderId;
            entity.CreatedBy = model.CreatedBy;
            entity.CreatedOn = DateTimeOffset.Now;
            entity.UpdatedBy = model.CreatedBy;
            entity.UpdatedOn = DateTimeOffset.Now;
            entity.IsActive = true;
            entity.IsDeleted = false;
            entity.TfxTerminal = model.TfxTerminal;
            entity.TfxBulkPlant = model.TfxBulkPlant;
            entity.BadgeNo1 = model.BadgeNo1;
            entity.BadgeNo2 = model.BadgeNo2;
            entity.BadgeNo3 = model.BadgeNo3;
            entity.DispactherNote = model.DispactherNote;
            entity.Notes = model.Notes;
            entity.AutoDRStatus = model.AutoDRStatus;
            entity.AutoCreatedOn = model.AutoCreatedOn;
            entity.AutoUpdatedOn = model.AutoUpdatedOn;
            entity.TankMaxFill = model.TankMaxFill;
            entity.IsMaxFillAllowed = model.IsMaxFillAllowed;
            entity.ParentId = model.Id.ToString();
            entity.DelReqSource = DRSource.MissedDR;
            entity.ScheduleQuantityType = model.ScheduleQuantityType;
            entity.DeliveryRequestFor = model.DeliveryRequestFor;
            entity.IsFilldInvoke = model.IsFilldInvoke;
            entity.Notes = model.Notes;
            entity.JobTimeZoneOffset = model.JobTimeZoneOffset;
            entity.IsCommonBadge = model.IsCommonBadge;
            entity.GroupParentDRId = model.GroupParentDRId;
            //Marine Nomination Changes.
            entity.DeliveryDateStartTime = model.DeliveryDateStartTime;
            entity.Vessel = model.Vessel;
            entity.Berth = model.Berth;
            entity.IsMarine = model.IsMarine;
            entity.DeliveryLevelPO = model.DeliveryLevelPO;
            entity.IndicativePrice = model.IndicativePrice;

            return entity;
        }

        public static void UpdateStatuses(this DeliveryRequestViewModel model)
        {
            if (model != null)
            {
                model.PreviousStatus = model.Status;
                model.SchedulePreviousStatus = model.Status == Exchange.Utilities.DeliveryReqStatus.ScheduleCreated ?
                                                (int)Exchange.Utilities.DeliveryScheduleStatus.New :
                                                (int)Exchange.Utilities.DeliveryScheduleStatus.None;
                model.ScheduleStatus = (int)Exchange.Utilities.DeliveryScheduleStatus.None;
            }
        }

        public static void UpdateStatuses(this TripViewModel model)
        {
            if (model != null)
            {
                model.TripPrevStatus = model.TripStatus == Exchange.Utilities.TripStatus.None && !string.IsNullOrWhiteSpace(model.TripId) ? Exchange.Utilities.TripStatus.Added : model.TripStatus;
                if (model.DeliveryGroupStatus == Exchange.Utilities.DeliveryGroupStatus.None)
                {
                    if (model.DeliveryGroupPrevStatus == Exchange.Utilities.DeliveryGroupStatus.None)
                    {
                        if (model.GroupId > 0)
                        {
                            model.DeliveryGroupPrevStatus = Exchange.Utilities.DeliveryGroupStatus.Published;
                        }
                        else if (!string.IsNullOrWhiteSpace(model.TripId))
                        {
                            model.DeliveryGroupPrevStatus = Exchange.Utilities.DeliveryGroupStatus.Draft;
                        }
                    }
                }
                else
                {
                    model.DeliveryGroupPrevStatus = model.DeliveryGroupStatus;
                }
                model.DeliveryGroupStatus = Exchange.Utilities.DeliveryGroupStatus.None;
                model.TripStatus = Exchange.Utilities.TripStatus.None;
            }
        }
        public static List<RecurringDeliveryRequestDetails> ToRecurringDREntity(this DeliveryRequestViewModel model)
        {
            List<RecurringDeliveryRequestDetails> recurringDeliveryRequestDetails = new List<RecurringDeliveryRequestDetails>();
            var entity = new RecurringDeliveryRequestDetails();
            entity.TfxJobAddress = model.JobAddress;
            entity.TfxJobCity = model.JobCity;
            entity.TfxJobName = model.JobName;
            entity.TfxProductType = model.ProductType;
            entity.TfxUoM = model.UoM;
            entity.TfxCreatedByCompanyId = model.CreatedByCompanyId;
            entity.TfxAssignedToCompanyId = model.AssignedToCompanyId;
            entity.TfxSupplierCompanyId = model.SupplierCompanyId;
            entity.TfxDisplayJobId = model.SiteId;
            entity.StorageId = model.StorageId;
            entity.StorageTypeId = model.TankId;
            entity.DeliveryRequestFor = model.DeliveryRequestFor;
            if (!string.IsNullOrWhiteSpace(model.CreatedByRegionId))
            {
                entity.CreatedRegionId = ObjectId.Parse(model.CreatedByRegionId);
            }
            entity.TfxProductTypeId = model.ProductTypeId;
            entity.DRCreationLevel = model.CurrentQuantity;
            entity.RequiredQuantity = model.RequiredQuantity;
            entity.Priority = model.Priority;
            entity.CurrentThreshold = model.CurrentThreshold;
            entity.Status = model.Status;
            entity.TfxAssignedToRegionId = model.AssignedToRegionId;
            //entity.TfxAssignedToRegionId = ObjectId.Parse(model.AssignedToRegionId);
            entity.TfxJobId = model.JobId;
            entity.JobTimeZoneOffset = model.JobTimeZoneOffset;
            entity.TfxCustomerCompany = model.CustomerCompany;
            entity.TfxAssignedToUserId = model.CreatedBy;
            entity.TfxOrderId = model.OrderId;
            entity.TfxDeliveryGroupId = model.DeliveryGroupId;
            entity.TfxDeliveryScheduleId = model.DeliveryScheduleId;
            entity.TfxTrackableScheduleId = model.TrackableScheduleId;
            entity.AutoDRStatus = model.AutoDRStatus;
            entity.AutoCreatedOn = model.AutoCreatedOn;
            entity.AutoUpdatedOn = model.AutoUpdatedOn;
            entity.ParentId = model.ParentId;
            entity.TankMaxFill = model.TankMaxFill;
            if (model.Terminal != null && model.Terminal.Id > 0)
            {
                entity.TfxTerminal = new MdbDataAccess.Collections.DropdownDisplayItem()
                {
                    Id = model.Terminal.Id,
                    Name = model.Terminal.Name
                };
            }
            if (model.BulkPlant != null && !string.IsNullOrWhiteSpace(model.BulkPlant.SiteName))
            {
                entity.TfxBulkPlant = model.BulkPlant;
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
            entity.CarrierOrderId = model.CarrierOrderId;
            entity.RouteInfo = model.RouteInfo;
            entity.Notes = model.Notes;
            entity.DeliveryLevelPO = model.DeliveryLevelPO;

            //Blended DRs.
            entity.BlendedGroupId = model.BlendedGroupId;
            entity.IsBlendedRequest = model.IsBlendedRequest;
            entity.BlendParentProductTypeId = model.BlendParentProductTypeId;
            entity.IsAdditive = model.IsAdditive;
            entity.QuantityInPercent = model.QuantityInPercent;
            entity.TfxFuelTypeId = model.FuelTypeId;
            entity.FuelType = model.FuelType;
            recurringDeliveryRequestDetails.Add(entity);
            return recurringDeliveryRequestDetails;
        }
        public static DeliveryRequest CloneRECEntity(this DeliveryRequestViewModel model)
        {
            var entity = new DeliveryRequest();
            entity.Id = ObjectId.GenerateNewId();
            entity.TfxJobAddress = model.JobAddress;
            entity.TfxJobCity = model.JobCity;
            entity.TfxJobName = model.JobName;
            entity.JobTimeZoneOffset = model.JobTimeZoneOffset;
            entity.TfxProductType = model.ProductType;
            entity.TfxUoM = model.UoM;
            entity.DelReqSource = model.DelReqSource;
            entity.TfxCreatedByCompanyId = model.CreatedByCompanyId;
            entity.TfxAssignedToCompanyId = model.AssignedToCompanyId;
            entity.TfxSupplierCompanyId = model.SupplierCompanyId;
            entity.TfxDisplayJobId = model.SiteId;
            entity.StorageId = model.StorageId;
            entity.StorageTypeId = model.TankId;
            if (!string.IsNullOrWhiteSpace(model.CreatedByRegionId))
            {
                entity.CreatedRegionId = ObjectId.Parse(model.CreatedByRegionId);
            }
            entity.TfxProductTypeId = model.ProductTypeId;
            entity.DRCreationLevel = model.CurrentQuantity;
            entity.RequiredQuantity = model.RequiredQuantity;
            entity.Priority = model.Priority;
            entity.CurrentThreshold = model.CurrentThreshold;
            entity.Status = model.Status;
            entity.TfxAssignedToRegionId = model.AssignedToRegionId;
            //entity.TfxAssignedToRegionId = ObjectId.Parse(model.AssignedToRegionId);
            entity.TfxJobId = model.JobId;
            entity.TfxCustomerCompany = model.CustomerCompany;
            entity.TfxAssignedToUserId = model.CreatedBy;
            entity.TfxOrderId = model.OrderId;
            entity.CreatedBy = model.CreatedBy;
            entity.CreatedOn = model.CreatedOn;
            entity.UpdatedBy = model.CreatedBy;
            entity.UpdatedOn = model.CreatedOn;
            entity.TfxDeliveryGroupId = model.DeliveryGroupId;
            entity.TfxDeliveryScheduleId = model.DeliveryScheduleId;
            entity.TfxTrackableScheduleId = model.TrackableScheduleId;
            entity.AutoDRStatus = model.AutoDRStatus;
            entity.AutoCreatedOn = model.AutoCreatedOn;
            entity.AutoUpdatedOn = model.AutoUpdatedOn;
            entity.ParentId = model.ParentId;
            entity.TankMaxFill = model.TankMaxFill;
            entity.IsMaxFillAllowed = model.IsMaxFillAllowed;
            entity.CarrierOrderId = model.CarrierOrderId;
            entity.ExternalRefId = model.ExternalRefId;
            if (model.Terminal != null && model.Terminal.Id > 0)
            {
                entity.TfxTerminal = new MdbDataAccess.Collections.DropdownDisplayItem()
                {
                    Id = model.Terminal.Id,
                    Name = model.Terminal.Name
                };
            }
            if (model.BulkPlant != null && !string.IsNullOrWhiteSpace(model.BulkPlant.SiteName))
            {
                entity.TfxBulkPlant = model.BulkPlant;
            }
            entity.BadgeNo1 = model.BadgeNo1 ?? string.Empty;
            entity.BadgeNo2 = model.BadgeNo2 ?? string.Empty;
            entity.BadgeNo3 = model.BadgeNo3 ?? string.Empty;
            entity.DispactherNote = model.DispactherNote ?? string.Empty;
            entity.IsCommonBadge = true;
            if (!string.IsNullOrWhiteSpace(model.PreLoadedFor))
            {
                entity.PreLoadedFor = ObjectId.Parse(model.PreLoadedFor);
            }
            if (!string.IsNullOrWhiteSpace(model.PostLoadedFor))
            {
                entity.PostLoadedFor = ObjectId.Parse(model.PostLoadedFor);
            }
            entity.PreLoadInfo = model.PreLoadInfo;
            entity.PostLoadInfo = model.PostLoadInfo;
            entity.RecurringScheduleId = model.isRecurringSchedule == false ? ObjectId.Empty : ObjectId.Parse(model.RecurringScheduleId);
            entity.RouteInfo = model.RouteInfo;
            entity.IsRecurringSchedule = true;
            entity.IsActive = true;
            entity.IsDeleted = false;
            entity.ScheduleQuantityType = model.ScheduleQuantityType;
            entity.RecurringScheduleId = ObjectId.Parse(model.RecurringScheduleId);
            //entity.DeliveryLevelPO = model.DeliveryLevelPO;
            //Blended DRs.
            entity.BlendedGroupId = model.BlendedGroupId;
            entity.IsBlendedRequest = model.IsBlendedRequest;
            entity.BlendParentProductTypeId = model.BlendParentProductTypeId;
            entity.IsAdditive = model.IsAdditive;
            entity.QuantityInPercent = model.QuantityInPercent;
            entity.TfxFuelTypeId = model.FuelTypeId;
            entity.FuelType = model.FuelType;
            return entity;
        }
        public static void UpdateModifiedPostLoadedDrValues(this DeliveryRequestViewModel entity, DeliveryRequestViewModel model)
        {
            entity.BadgeNo1 = model.BadgeNo1;
            entity.BadgeNo2 = model.BadgeNo2;
            entity.BadgeNo3 = model.BadgeNo3;
            entity.DispactherNote = model.DispactherNote;
            entity.BulkPlant = model.BulkPlant;
            entity.Terminal = model.Terminal;
            entity.UoM = model.UoM;
            entity.OrderId = model.OrderId;
            entity.PickupLocationType = model.PickupLocationType;
            entity.RequiredQuantity = model.RequiredQuantity;
            entity.ScheduleStatus = (int)DeliveryScheduleStatus.Modified;
            entity.Compartments = model.Compartments;
        }

        public static DeliveryRequestReportGridViewModel ToDeliveryRequestReportGridViewModel(this DeliveryRequest entity)
        {
            var model = new DeliveryRequestReportGridViewModel();

            model.DrId = entity.Id.ToString();
            model.CustomerName = entity.TfxCustomerCompany;
            model.RegionId = entity.TfxAssignedToRegionId;
            model.Location = entity.TfxJobName;
            model.LocationId = entity.TfxDisplayJobId;
            model.OrderId = entity.TfxOrderId.HasValue ? entity.TfxOrderId.Value : 0;
            model.ProductType = entity.TfxProductType;
            model.TfxJobId = entity.TfxJobId;
            model.RequestedQuantity = entity.RequiredQuantity;
            model.Priority = entity.Priority;
            model.ProductTypeId = entity.TfxProductTypeId;
            if ((entity.AutoDRStatus == AutoDrStatus.Create) || (entity.AutoDRStatus == AutoDrStatus.CreateAndUpdate))
            {
                model.IsAutoDR = true;
            }
            model.IsRecurringSchedule = entity.IsRecurringSchedule;
            if (entity.TfxUoM == (int)UoM.Gallons)
            {
                model.UoM = Resource.lblGallonsShortName;
            }
            if (entity.TfxUoM == (int)UoM.Litres)
            {
                model.UoM = Resource.lblLitresShortName;
            }
            else if (entity.TfxUoM == (int)UoM.MetricTons)
            {
                model.UoM = Resource.lblMetricTonsShortName;
            }
            else if (entity.TfxUoM == (int)UoM.Barrels)
            {
                model.UoM = Resource.lblBarrelsShortName;
            }

            return model;
        }
        public static void UpdateStatusesToCancel(this DeliveryRequestViewModel model)
        {
            if (model != null)
            {
                model.PreviousStatus = model.Status;
                model.SchedulePreviousStatus = model.Status == Exchange.Utilities.DeliveryReqStatus.ScheduleCreated ?
                                                (int)Exchange.Utilities.DeliveryScheduleStatus.New :
                                                (int)Exchange.Utilities.DeliveryScheduleStatus.None;
                model.ScheduleStatus = (int)Exchange.Utilities.DeliveryScheduleStatus.Canceled;
            }
        }
    }
}
