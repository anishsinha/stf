using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.ViewModels.Forcasting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Mappers.Forcasting
{
    public static class ForcastingServiceSettingMapper
    {
        public static ForcastingServiceSetting ToEntity(this ForcastingServiceSettingViewModel viewModel, ForcastingServiceSetting entity = null)
        {
            if (entity == null)
                entity = new ForcastingServiceSetting();

            entity.AverageLoadQty = viewModel.AverageLoad.GetValueOrDefault();
            entity.BandPeriod = viewModel.BandPeriod.GetValueOrDefault();
            entity.CreatedBy = viewModel.CreatedBy;
            entity.CreatedDate = viewModel.CreatedDate;
            entity.EndBuffer = viewModel.EndBuffer.GetValueOrDefault();
            entity.EndBufferUOM = viewModel.EndBufferUOM.GetValueOrDefault();
            entity.Id = viewModel.Id;
            entity.InventoryPriorityType = viewModel.ForcastingType.GetValueOrDefault();
            entity.InventoryUOM = viewModel.InventoryUOM.GetValueOrDefault();
            entity.IsAutoDRCreation = viewModel.IsAutoDRCreation;
            entity.LeadTime = viewModel.LeadTime.GetValueOrDefault();
            entity.LeadTimeUOM = viewModel.LeadTimeUOM.GetValueOrDefault();
            entity.MinimumLoadQty = viewModel.MinimumLoad.GetValueOrDefault();
            entity.RetainCouldGo = viewModel.Retain.GetValueOrDefault();
            entity.RetainTimeBuffer = viewModel.RetainTimeBuffer.GetValueOrDefault();
            entity.RetainTimeBufferUOM = viewModel.RetainTimeBufferUOM.GetValueOrDefault();
            entity.RunoutLevelMustGo = viewModel.RunoutLevel.GetValueOrDefault();
            entity.SafetyStockShouldGo = viewModel.SafetyStock.GetValueOrDefault();
            entity.StartBuffer = viewModel.StartBuffer.GetValueOrDefault();
            entity.StartBufferUOM = viewModel.StartBufferUOM.GetValueOrDefault();
            entity.SupplierLead = viewModel.SupplierLead.GetValueOrDefault();
            entity.SupplierLeadUOM = viewModel.SupplierLeadUOM.GetValueOrDefault();
            entity.IsOttoAutoDRCreation = viewModel.IsOttoAutoDRCreation;
            entity.IsOttoScheduleCreation = viewModel.IsOttoScheduleCreation;
            if (viewModel.IsOttoAutoDRCreationAllCarrier == 1)
                entity.IsAllCarrierEnabled = true;
            else
                entity.IsAllCarrierEnabled = false;
            entity.StartTiming = Convert.ToDateTime(viewModel.StartTime).TimeOfDay;
            entity.UpdatedBy = viewModel.UpdatedBy;
            entity.UpdatedDate = viewModel.UpdatedDate;

            return entity;
        }
        public static ForcastingServiceSetting ToCloneEntity(this ForcastingServiceSettingViewModel viewModel, ForcastingServiceSetting entity = null)
        {
            if (entity == null)
                entity = new ForcastingServiceSetting();
            entity.AverageLoadQty = viewModel.AverageLoad.GetValueOrDefault();
            entity.BandPeriod = viewModel.BandPeriod.GetValueOrDefault();
            entity.EndBuffer = viewModel.EndBuffer.GetValueOrDefault();
            entity.EndBufferUOM = viewModel.EndBufferUOM.GetValueOrDefault();
            entity.InventoryPriorityType = viewModel.ForcastingType.GetValueOrDefault();
            entity.InventoryUOM = viewModel.InventoryUOM.GetValueOrDefault();
            entity.IsAutoDRCreation = viewModel.IsAutoDRCreation;
            entity.LeadTime = viewModel.LeadTime.GetValueOrDefault();
            entity.LeadTimeUOM = viewModel.LeadTimeUOM.GetValueOrDefault();
            entity.MinimumLoadQty = viewModel.MinimumLoad.GetValueOrDefault();
            entity.RetainCouldGo = viewModel.Retain.GetValueOrDefault();
            entity.RetainTimeBuffer = viewModel.RetainTimeBuffer.GetValueOrDefault();
            entity.RetainTimeBufferUOM = viewModel.RetainTimeBufferUOM.GetValueOrDefault();
            entity.RunoutLevelMustGo = viewModel.RunoutLevel.GetValueOrDefault();
            entity.SafetyStockShouldGo = viewModel.SafetyStock.GetValueOrDefault();
            entity.StartBuffer = viewModel.StartBuffer.GetValueOrDefault();
            entity.StartBufferUOM = viewModel.StartBufferUOM.GetValueOrDefault();
            entity.SupplierLead = viewModel.SupplierLead.GetValueOrDefault();
            entity.SupplierLeadUOM = viewModel.SupplierLeadUOM.GetValueOrDefault();
            entity.IsOttoAutoDRCreation = viewModel.IsOttoAutoDRCreation;
            entity.IsOttoScheduleCreation = viewModel.IsOttoScheduleCreation;
            if (viewModel.IsOttoAutoDRCreationAllCarrier == 1)
                entity.IsAllCarrierEnabled = true;
            else
                entity.IsAllCarrierEnabled = false;
            var finalString = string.Empty;
            finalString = ValidateStartTimining(viewModel, finalString);
            DateTime dateTime = DateTime.ParseExact(finalString,
                                    "hh:mm tt", CultureInfo.InvariantCulture);
            entity.StartTiming = dateTime.TimeOfDay;

            entity.CreatedBy = viewModel.CreatedBy;
            entity.CreatedDate = DateTime.Now;
            return entity;
        }

        private static string ValidateStartTimining(ForcastingServiceSettingViewModel viewModel, string finalString)
        {
            if (!string.IsNullOrEmpty(viewModel.StartTime))
            {
                var splitRecord = viewModel.StartTime.Split(':');
                if (splitRecord.Length > 0)
                {

                    if (splitRecord[0].Length == 1)
                    {
                        finalString = "0" + splitRecord[0].ToString();
                        finalString = finalString + ":" + splitRecord[1].ToString();
                    }
                    else
                    {
                        finalString = viewModel.StartTime;
                    }
                }
            }

            return finalString;
        }

        public static ForcastingServiceSettingViewModel ToViewModel(this ForcastingServiceSetting entity, ForcastingServiceSettingViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new ForcastingServiceSettingViewModel();

            viewModel.AverageLoad = entity.AverageLoadQty;
            viewModel.BandPeriod = entity.BandPeriod;
            viewModel.CreatedBy = entity.CreatedBy;
            viewModel.CreatedDate = entity.CreatedDate;
            viewModel.EndBuffer = entity.EndBuffer;
            viewModel.EndBufferUOM = entity.EndBufferUOM;
            viewModel.Id = entity.Id;
            viewModel.ForcastingType = entity.InventoryPriorityType;
            viewModel.InventoryUOM = entity.InventoryUOM;
            viewModel.IsAutoDRCreation = entity.IsAutoDRCreation;
            viewModel.LeadTime = entity.LeadTime;
            viewModel.LeadTimeUOM = entity.LeadTimeUOM;
            viewModel.MinimumLoad = entity.MinimumLoadQty;
            viewModel.Retain = entity.RetainCouldGo;
            viewModel.RetainTimeBuffer = entity.RetainTimeBuffer;
            viewModel.RetainTimeBufferUOM = entity.RetainTimeBufferUOM;
            viewModel.RunoutLevel = entity.RunoutLevelMustGo;
            viewModel.SafetyStock = entity.SafetyStockShouldGo;
            viewModel.StartBuffer = entity.StartBuffer;
            viewModel.StartBufferUOM = entity.StartBufferUOM;
            viewModel.StartTime = Convert.ToDateTime(entity.StartTiming.ToString()).ToShortTimeString();
            viewModel.UpdatedBy = entity.UpdatedBy;
            viewModel.UpdatedDate = entity.UpdatedDate;

            viewModel.SupplierLead = entity.SupplierLead;
            viewModel.SupplierLeadUOM = entity.SupplierLeadUOM;
            viewModel.IsOttoAutoDRCreation = entity.IsOttoAutoDRCreation;
            viewModel.IsOttoScheduleCreation = entity.IsOttoScheduleCreation;
            if (entity.IsAllCarrierEnabled)
            {
                viewModel.IsOttoAutoDRCreationAllCarrier = 1;
            }
            else
            {
                if (entity.ForcastingServiceXCarriers.Count() > 0)
                {
                    viewModel.IsOttoAutoDRCreationAllCarrier = 2;
                }
                else
                {
                    viewModel.IsOttoAutoDRCreationAllCarrier = -1;
                }
            }
            if (viewModel.IsOttoAutoDRCreationAllCarrier == 2)
            {
                foreach (var item in entity.ForcastingServiceXCarriers)
                {
                    viewModel.SelectedCarrierList.Add(item.CarrierId);
                }
            }
            return viewModel;
        }
    }
}
