using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class CompanyMapper
    {
        public static CompanyViewModel ToViewModel(this Company entity, CompanyViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new CompanyViewModel(Status.Success);

            viewModel.Id = entity.Id;
            viewModel.Name = entity.Name;
            viewModel.CompanyTypeId = entity.CompanyTypeId;
            //viewModel.CompanySizeId = entity.CompanySizeId;
            //viewModel.BudgetAlertPercentage = entity.BudgetAlertPercentage;
            //viewModel.BusinessTenureId = entity.BusinessTenureId;
            //viewModel.FuelQuantityId = entity.FuelQuantityId;
            viewModel.IsAssetTrackingEnabled = entity.IsAssetTrackingEnabled;
            viewModel.IsResaleEnabled = entity.IsResaleEnabled;
            viewModel.SupplierCode = entity.SupplierCode;

            if (entity.Image != null)
            {
                viewModel.CompanyLogo = new ImageViewModel(Status.Success)
                {
                    Id = entity.Image.Id,
                    Data = entity.Image.Data,
                    FilePath = entity.Image.FilePath
                };
            }
            else
            {
                viewModel.CompanyLogo = new ImageViewModel(Status.Success);
            }
            viewModel.IsActive = entity.IsActive;
            viewModel.UpdatedBy = entity.UpdatedBy;
            viewModel.UpdatedDate = entity.UpdatedDate;
            viewModel.WorkPreference = entity.WorkPreference;
            if (entity.FleetInformations != null && entity.FleetInformations.Any())
            {
                viewModel.FleetInfo = entity.FleetInformations.ToList().ToViewModel();
            }
            return viewModel;
        }

        public static Company ToEntity(this CompanyViewModel viewModel, Company entity = null)
        {
            if (entity == null)
                entity = new Company();

            entity.Id = viewModel.Id;
            entity.Name = viewModel.Name;
            entity.CompanyTypeId = viewModel.CompanyTypeId;
            //entity.CompanySizeId = viewModel.CompanySizeId;
            //entity.BusinessTenureId = viewModel.BusinessTenureId;
            //entity.FuelQuantityId = viewModel.FuelQuantityId;
            entity.BudgetAlertPercentage = viewModel.BudgetAlertPercentage;
            entity.IsAssetTrackingEnabled = viewModel.IsAssetTrackingEnabled;
            entity.IsResaleEnabled = viewModel.IsResaleEnabled;
            entity.SupplierCode = viewModel.SupplierCode;
            entity.AccountOwnerId = viewModel.AccountOwnerId;
            if (viewModel.CompanyLogo != null)
            {
                if (viewModel.CompanyLogo.Id > 0 && !viewModel.CompanyLogo.IsRemoved)
                {
                    entity.CompanyLogoId = viewModel.CompanyLogo.Id;
                }
                if (viewModel.CompanyLogo.IsRemoved)
                {
                    entity.CompanyLogoId = null;
                }
            }

            entity.IsActive = viewModel.IsActive;
            entity.UpdatedBy = viewModel.UpdatedBy;
            entity.UpdatedDate = viewModel.UpdatedDate;
            entity.WorkPreference = viewModel.WorkPreference;
            return entity;
        }

        public static OnboardingPreference ToEntity(this OnboardingPreferenceViewModel viewModel, OnboardingPreference entity = null)
        {
            if (entity == null)
                entity = new OnboardingPreference();

            entity.Id = viewModel.Id;
            entity.CompanyId = viewModel.CompanyId;
            entity.UserId = viewModel.UserId;
            entity.AllowCustomerDriverChat = viewModel.AllowChatWithDrivers;
            entity.IsBuyerReceiptEnabled = viewModel.IsBuyerReceiptEnabled;
            entity.IsExceptionEnabled = viewModel.IsExceptionEnabled;
            entity.IsUnscheduledDeliveryAllowed = viewModel.IsUnscheduledDeliveryAllowed;
            entity.IsUnscheduledPickupAllowed = viewModel.IsUnscheduledPickupAllowed;
            entity.PreferencePricingMethod = viewModel.PreferencePricingMethod;
            entity.DeliveryType = viewModel.IsFtl ? TruckLoadTypes.FullTruckLoad : TruckLoadTypes.LessTruckLoad;
            entity.FreightOnBoardType = viewModel.FreightOnBoardType;
            entity.IsBuySellEnabled = viewModel.IsBuySellEnabled;
            entity.CreditCheckType = viewModel.CreditCheckType;
            entity.IsDropTicketImageRequired = viewModel.IsDropTicketImageRequired;
            entity.IsThirdPartyHardwareUsed = viewModel.IsThirdPartyHardwareUsed;
            entity.IsSuppressOrderNotifications = viewModel.IsSuppressOrderNotifications;
            entity.IsCustomerInvitationEnabled = viewModel.IsCustomerInvitationEnabled;
            entity.IsSupressOrderPricing = viewModel.IsSupressOrderPricing;
            entity.IsFreightOnlyOrderEnabled = viewModel.IsFreightOnlyOrderEnabled;
            entity.CreatedBy = viewModel.CreatedBy;
            entity.CreatedDate = viewModel.CreatedDate;
            entity.UpdatedBy = viewModel.UpdatedBy;
            entity.UpdatedDate = viewModel.UpdatedDate;
            entity.IsActive = viewModel.IsActive;
            entity.IsDriverProdutDisplayEnable = viewModel.IsDriverProdutDisplayEnable;
            entity.IsCustomUnScheduleDelivery = viewModel.IsCustomUnScheduleDelivery;
            entity.DeliveryDays = viewModel.DeliveryDaysInString;
            entity.ShiftStartTime = Convert.ToDateTime(viewModel.ShiftStartTime).TimeOfDay;
            entity.ShiftEndTime = Convert.ToDateTime(viewModel.ShiftEndTime).TimeOfDay;
            entity.IsBrandMyWebsite = viewModel.IsBrandMyWebsite;
            entity.BackgroundColor = viewModel.BackgroundColor;
            entity.FontColor = viewModel.FontColor;
            entity.ForegroundColor = viewModel.ForegroundColor;
            entity.IconColor = viewModel.IconColor;
            entity.HeaderColor = viewModel.HeaderColor;
            entity.ButtonColor = viewModel.ButtonColor;
            entity.ImageFilePath = viewModel.ImageFilePath;
            entity.FaviconImageFilePath = viewModel.FaviconFilePath;
            entity.CarrierOnboardingImageFilePath = viewModel.CarrierOnboardingImageFilePath;
            entity.URLName = string.IsNullOrEmpty(viewModel.URLName) ? string.Empty : viewModel.URLName.Trim().ToUpper();
            entity.NotificationPeriod = viewModel.NotificationPeriod;
            entity.DipTestMethod = viewModel.DipTestMethod;
            entity.LocationManagedType = viewModel.LocationManagedType;
            entity.BackgroundImageFilePath = viewModel.BackgroundImageFilePath;
            entity.IsBadgeMandatory = viewModel.IsBadgeMandatory;
            entity.IsShowProductDescriptionOnInvoice = viewModel.IsShowProductDescriptionOnInvoice;
            entity.IsEbolWorkflowEnabled = viewModel.IsEbolWorkflowEnabled;
            entity.IsLiftFileValidationEnabled = viewModel.IsLiftFileValidationEnabled;
            entity.IsSequencingEnabled = viewModel.IsProductSequencingEnabled;
            entity.LoadQueueAttributes = viewModel.LoadQueueAttributesValue;
            entity.DRQueueAttributes = viewModel.DRQueueAttributesValue;
            entity.RetainThreshold = viewModel.RetainThreshold;
            entity.UOM = viewModel.UOM;
            if (viewModel.IsLiftFileValidationEnabled)
            {
                if (viewModel.LfvInputParameter != null)
                    entity.LiftFileValidationParameters.Add(viewModel.LfvInputParameter.ToEntity(LFVParameterType.Input));

                if (viewModel.LfvOutputParameter != null)
                    entity.LiftFileValidationParameters.Add(viewModel.LfvOutputParameter.ToEntity(LFVParameterType.Output));
            }
            entity.IsDSBDriverSchedule = viewModel.IsDSBDriverSchedule;
            entity.IssendInventoryExportEmail = viewModel.IsSendInventoryExportEmail;
            entity.IsCarrierTileEmailNotification = viewModel.IsCarrierTileEmailNotification;
            entity.FreightPricingMethod = viewModel.FreightPricingMethod;
            entity.IsThirdPartyInvitationEnabled = viewModel.IsThirdPartyInvitationEnabled;
            entity.PaymentDueDateType = viewModel.PaymentDueDateType;
            entity.IsAdditiveBlendingEnabled = viewModel.IsAdditiveBlendingEnabled;
            entity.IsAssignReasonCodeEnabled = viewModel.IsReasonCodesEnabled;
            entity.IsLoadOptimization = viewModel.IsLoadOptimization;
            return entity;
        }

        public static BuyerXOnboardingPreference ToEntity(this BuyerXOnboardingPreferenceViewModel viewModel, BuyerXOnboardingPreference entity = null)
        {
            if (entity == null)
                entity = new BuyerXOnboardingPreference();

            entity.Id = viewModel.Id;
            entity.OnboardingPreferenceId = viewModel.OnboardingPreferenceId;
            entity.BuyerCompanyId = viewModel.BuyerCompanyId;
            entity.IsActive = viewModel.IsActive;

            return entity;
        }

        public static OnboardingPreferenceViewModel ToViewModel(this OnboardingPreference entity, OnboardingPreferenceViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new OnboardingPreferenceViewModel();

            viewModel.Id = entity.Id;
            viewModel.CompanyId = entity.CompanyId;
            viewModel.UserId = entity.UserId;
            viewModel.IsBuyerReceiptEnabled = entity.IsBuyerReceiptEnabled;
            viewModel.IsExceptionEnabled = entity.IsExceptionEnabled;
            viewModel.IsUnscheduledDeliveryAllowed = entity.IsUnscheduledDeliveryAllowed;
            viewModel.IsUnscheduledPickupAllowed = entity.IsUnscheduledPickupAllowed;
            viewModel.PreferencePricingMethod = entity.PreferencePricingMethod;
            viewModel.IsFtl = entity.DeliveryType == TruckLoadTypes.FullTruckLoad ? true : false;
            viewModel.FreightOnBoardType = entity.FreightOnBoardType;
            viewModel.IsBuySellEnabled = entity.IsBuySellEnabled;
            viewModel.IsDropTicketImageRequired = entity.IsDropTicketImageRequired;
            viewModel.IsThirdPartyHardwareUsed = entity.IsThirdPartyHardwareUsed;
            viewModel.CreditCheckType = entity.CreditCheckType;
            viewModel.IsCreditCheckEnabled = entity.CreditCheckType != CreditCheckTypes.None;
            viewModel.IsSuppressOrderNotifications = entity.IsSuppressOrderNotifications;
            viewModel.IsCustomerInvitationEnabled = entity.IsCustomerInvitationEnabled;
            viewModel.IsSupressOrderPricing = entity.IsSupressOrderPricing;
            viewModel.IsFreightOnlyOrderEnabled = entity.IsFreightOnlyOrderEnabled;
            viewModel.AllowChatWithDrivers = entity.AllowCustomerDriverChat;
            viewModel.CreatedBy = entity.CreatedBy;
            viewModel.CreatedDate = entity.CreatedDate;
            viewModel.UpdatedBy = entity.UpdatedBy;
            viewModel.UpdatedDate = entity.UpdatedDate;
            viewModel.IsActive = entity.IsActive;
            viewModel.IsDriverProdutDisplayEnable = entity.IsDriverProdutDisplayEnable;
            viewModel.IsCustomUnScheduleDelivery = entity.IsCustomUnScheduleDelivery;

            viewModel.IsBrandMyWebsite = entity.IsBrandMyWebsite;
            viewModel.BackgroundColor = entity.BackgroundColor;
            viewModel.FontColor = entity.FontColor;
            viewModel.ForegroundColor = entity.ForegroundColor;
            viewModel.IconColor = entity.IconColor;
            viewModel.HeaderColor = entity.HeaderColor;
            viewModel.ButtonColor = entity.ButtonColor;
            viewModel.ImageFilePath = string.IsNullOrEmpty(entity.ImageFilePath) ? string.Empty : entity.ImageFilePath;
            viewModel.URLName = string.IsNullOrEmpty(entity.URLName) ? string.Empty : entity.URLName;
            viewModel.hdnImageFilePath = string.IsNullOrEmpty(entity.ImageFilePath) ? string.Empty : entity.ImageFilePath;
            viewModel.LocationManagedType = entity.LocationManagedType;
            viewModel.DipTestMethod = entity.DipTestMethod;
            viewModel.NotificationPeriod = entity.NotificationPeriod ?? ApplicationConstants.DefaultNotificationPeriod;
            viewModel.BackgroundImageFilePath = string.IsNullOrEmpty(entity.BackgroundImageFilePath) ? string.Empty : entity.BackgroundImageFilePath;
            viewModel.hdnBackgroundImageFilePath = string.IsNullOrEmpty(entity.BackgroundImageFilePath) ? string.Empty : entity.BackgroundImageFilePath;
            viewModel.IsBadgeMandatory = entity.IsBadgeMandatory;
            viewModel.IsLiftFileValidationEnabled = entity.IsLiftFileValidationEnabled;
            viewModel.IsProductSequencingEnabled = entity.IsSequencingEnabled;
            viewModel.IsEbolWorkflowEnabled = entity.IsEbolWorkflowEnabled;
            viewModel.IsShowProductDescriptionOnInvoice = entity.IsShowProductDescriptionOnInvoice;
            viewModel.RetainThreshold = entity.RetainThreshold;
            viewModel.UOM = entity.UOM;
            viewModel.FaviconFilePath = string.IsNullOrEmpty(entity.FaviconImageFilePath) ? string.Empty : entity.FaviconImageFilePath;
            viewModel.hdnfaviconFilePath = viewModel.FaviconFilePath;
            viewModel.CarrierOnboardingImageFilePath = string.IsNullOrEmpty(entity.CarrierOnboardingImageFilePath) ? string.Empty : entity.CarrierOnboardingImageFilePath;
            viewModel.hdnCarrierOnboardingImageFilePath = viewModel.CarrierOnboardingImageFilePath;
            if (entity.DeliveryDays == null)
            {
                viewModel.DeliveryDaysInList = null;
            }
            else
            {
                viewModel.DeliveryDaysInList = new List<int>(entity.DeliveryDays.Split(',').Select(int.Parse));
                viewModel.DeliveryDaysInString = entity.DeliveryDays;
                viewModel.ShiftStartTime = entity.ShiftStartTime.ToString();
                viewModel.ShiftEndTime = entity.ShiftEndTime.ToString();
            }

            viewModel.LoadQueueAttributesValue = entity.LoadQueueAttributes;
            viewModel.DRQueueAttributesValue = entity.DRQueueAttributes;

            if (entity.BuyerXOnboardingPreferences != null && entity.BuyerXOnboardingPreferences.Any())
            {
                viewModel.BuyersToSendReceipts = new System.Collections.Generic.List<BuyerXOnboardingPreferenceViewModel>();
                foreach (var buyer in entity.BuyerXOnboardingPreferences)
                {
                    var buyerToSendReceipt = buyer.ToViewModel();
                    viewModel.BuyersToSendReceipts.Add(buyerToSendReceipt);
                }
            }
            viewModel.IsDSBDriverSchedule = entity.IsDSBDriverSchedule;
            viewModel.IsSendInventoryExportEmail = entity.IssendInventoryExportEmail;
            viewModel.IsCarrierTileEmailNotification = entity.IsCarrierTileEmailNotification;
            viewModel.FreightPricingMethod = entity.FreightPricingMethod;
            viewModel.IsThirdPartyInvitationEnabled = entity.IsThirdPartyInvitationEnabled;
            viewModel.PaymentDueDateType = entity.PaymentDueDateType;
            viewModel.IsAdditiveBlendingEnabled = entity.IsAdditiveBlendingEnabled;
            viewModel.IsReasonCodesEnabled = entity.IsAssignReasonCodeEnabled;
            viewModel.IsLoadOptimization = entity.IsLoadOptimization;
            return viewModel;
        }

        public static BuyerXOnboardingPreferenceViewModel ToViewModel(this BuyerXOnboardingPreference entity, BuyerXOnboardingPreferenceViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new BuyerXOnboardingPreferenceViewModel();

            viewModel.Id = entity.Id;
            viewModel.OnboardingPreferenceId = entity.OnboardingPreferenceId;
            viewModel.BuyerCompanyId = entity.BuyerCompanyId;
            viewModel.IsActive = entity.IsActive;

            return viewModel;
        }

        public static LfvParameterViewModel ToViewModel(this LiftFileValidationParameter entity, LfvParameterViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new LfvParameterViewModel();

            if (entity != null)
            {
                viewModel.IsBolReq = entity.IsBolReq;
                viewModel.IsCINReq = entity.IsCarrierIdReq;
                viewModel.IsCarrierNameReq = entity.IsCarrierNameReq;
                viewModel.IsCorrectedQtyRes = entity.IsCorrectedQtyReq;
                viewModel.IsGrossReq = entity.IsGrossReq;
                viewModel.IsLoadDateReq = entity.IsLoadDateReq;
                viewModel.IsTerminalCodeReq = entity.IsTerminalCodeReq;
                viewModel.IsTermItemCodeReq = entity.IsTermItemCodeReq;
                viewModel.IsIgnoreSelfHauling = entity.IsIgnoreSelfHauling;
                viewModel.IsReplacePoWithAccoutingId = entity.IsReplacePoWithAccoutingId;
                viewModel.DaysToContinueMatchProcess = entity.NoMatchRecordDays == 0 ? ApplicationConstants.DefaultNoMatchRecordDays : entity.NoMatchRecordDays;
                viewModel.IsIgnoreWholesaleBadge = entity.IsIgnoreWholesalebadge;
                viewModel.IsIgnoreNonRegisteredCarriers = entity.IsIgnoreNonRegisteredCarriers;
                viewModel.IsIgnoreQuebecBillingBadges = entity.IsIgnoreQuebecBillingBadges;
                viewModel.IsNeedToTruncateLeadingZeros = entity.IsUnsupportedDataToIncludeInCleanRecords;
            }

            return viewModel;
        }

        public static LiftFileValidationParameter ToEntity(this LfvParameterViewModel viewModel, LFVParameterType lFVParameterType, LiftFileValidationParameter entity = null)
        {
            if (entity == null)
                entity = new LiftFileValidationParameter();

            if(viewModel != null)
            {
                entity.IsActive = true;
                entity.IsBolReq = viewModel.IsBolReq;
                entity.IsCarrierIdReq = viewModel.IsCINReq; //hvant changed db column name to CINReq
                entity.IsCarrierNameReq = viewModel.IsCarrierNameReq;
                entity.IsCorrectedQtyReq = viewModel.IsCorrectedQtyRes;
                entity.IsGrossReq = viewModel.IsGrossReq;
                entity.IsLoadDateReq = viewModel.IsLoadDateReq;
                entity.IsTerminalCodeReq = viewModel.IsTerminalCodeReq;
                entity.IsTermItemCodeReq = viewModel.IsTermItemCodeReq;
                entity.IsUnsupportedDataToIncludeInCleanRecords = viewModel.IsNeedToTruncateLeadingZeros; //instead of adding new column we will use this and rename it
                entity.IsIgnoreSelfHauling = viewModel.IsIgnoreSelfHauling;
                entity.IsReplacePoWithAccoutingId = viewModel.IsReplacePoWithAccoutingId;
                entity.ParameterType = lFVParameterType;
                entity.NoMatchRecordDays = viewModel.DaysToContinueMatchProcess;
                entity.IsIgnoreWholesalebadge = viewModel.IsIgnoreWholesaleBadge;
                entity.IsIgnoreNonRegisteredCarriers = viewModel.IsIgnoreNonRegisteredCarriers;
                entity.IsIgnoreQuebecBillingBadges = viewModel.IsIgnoreQuebecBillingBadges;
            }

            return entity;
        }
        public static FleetInfo ToViewModel(this List<FleetInformation> entity, FleetInfo viewModel = null)
        {
            if (viewModel == null)
                viewModel = new FleetInfo();

            foreach (var fleetInfo in entity)
            {
                if (fleetInfo.FleetType != FleetType.DEF)
                {
                    var fuelAsset = new FleetTrailers();
                    fuelAsset.FleetType = fleetInfo.FleetType;
                    fuelAsset.FuelTrailerServiceTypeFTL = ((FuelTrailerAssetType)fleetInfo.TrailerServiceType);
                    fuelAsset.Capacity = fleetInfo.Capacity;
                    fuelAsset.TrailerHasPump = fleetInfo.DoesTrailerHasPump;
                    fuelAsset.IsTrailerMetered = fleetInfo.IsTrailerMetered;
                    fuelAsset.Count = fleetInfo.Count;
                    viewModel.FuelAssets.Add(fuelAsset);
                }
                else
                {
                    var defAsset = new FleetTrailers();
                    defAsset.FleetType = fleetInfo.FleetType;
                    defAsset.DEFTrailerServiceType = ((DefTrailerAssetType)fleetInfo.TrailerServiceType);
                    defAsset.Capacity = fleetInfo.Capacity;
                    defAsset.TrailerHasPump = fleetInfo.DoesTrailerHasPump;
                    defAsset.IsTrailerMetered = fleetInfo.IsTrailerMetered;
                    defAsset.Count = fleetInfo.Count;
                    defAsset.PackagedGoods = fleetInfo.IsPackagedGoods;
                    viewModel.DefAssets.Add(defAsset);
                }
            }
            return viewModel;
        }
    }
}
    
