using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.ViewModels.Forcasting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Mappers.Forcasting
{
    public static class ForcastingPreferenceMapper
    {
        public static ForcastingPreference ToEntity(this ForcastingPreferenceViewModel viewModel, ForcastingPreference entity = null)
        {
            if (entity == null)
                entity = new ForcastingPreference();


            entity.BuyerCompanyId = viewModel.BuyerCompanyId;
            entity.CreatedBy = viewModel.CreatedBy;
            entity.CreatedDate = viewModel.CreatedDate;
            entity.EntityId = viewModel.EntityId;
            entity.ForcastingServicePreference = viewModel.ForcastingServicePreference;
            entity.ForcastingServiceSetting = viewModel.ForcastingServiceSetting.ToEntity();
            entity.ForcastingSettingId = viewModel.ForcastingSettingId;
            entity.ForcastingSettingLevel = viewModel.ForcastingSettingLevel;
            entity.Id = viewModel.Id;
            entity.IsActive = viewModel.IsActive;
            entity.IsDeleted = viewModel.IsDeleted;
            entity.SupplierCompanyId = viewModel.SupplierCompanyId;
            entity.UpdatedBy = viewModel.UpdatedBy;
            entity.UpdatedDate = viewModel.UpdatedDate;

            return entity;
        }
        public static ForcastingPreference ToCloneEntity(this ForcastingPreferenceViewModel viewModel, ForcastingPreference entity = null)
        {
            if (entity == null)
                entity = new ForcastingPreference();

            entity.BuyerCompanyId = viewModel.BuyerCompanyId;
            entity.SupplierCompanyId = viewModel.SupplierCompanyId;
            entity.ForcastingServicePreference = viewModel.ForcastingServicePreference;
            entity.ForcastingSettingLevel = viewModel.ForcastingSettingLevel;
            entity.ForcastingServiceSetting = new ForcastingServiceSetting();
            entity.EntityId = viewModel.EntityId;
            entity.IsActive = true;
            entity.IsDeleted = false;
            entity.CreatedDate = DateTime.Now;
            entity.CreatedBy = viewModel.CreatedBy;
            return entity;
        }
        public static ForcastingPreference ToCloneAccountEntity(this ForcastingPreferenceViewModel viewModel, ForcastingPreference entity = null)
        {
            if (entity == null)
                entity = new ForcastingPreference();

            entity.BuyerCompanyId = viewModel.BuyerCompanyId;
            entity.SupplierCompanyId = viewModel.SupplierCompanyId;
            entity.ForcastingServicePreference = viewModel.ForcastingServicePreference;
            entity.ForcastingSettingLevel = viewModel.ForcastingSettingLevel;
            entity.EntityId = viewModel.EntityId;
            entity.IsActive = true;
            entity.IsDeleted = false;
            entity.CreatedDate = DateTime.Now;
            entity.CreatedBy = viewModel.CreatedBy;
            return entity;
        }

        public static ForcastingPreferenceViewModel ToViewModel(this ForcastingPreference entity, ForcastingPreferenceViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new ForcastingPreferenceViewModel();

            viewModel.BuyerCompanyId = entity.BuyerCompanyId;
            viewModel.CreatedBy = entity.CreatedBy;
            viewModel.CreatedDate = entity.CreatedDate;
            viewModel.EntityId = entity.EntityId;
            viewModel.ForcastingServicePreference = entity.ForcastingServicePreference;
            viewModel.ForcastingServiceSetting = entity.ForcastingServiceSetting.ToViewModel();
            viewModel.ForcastingSettingId = entity.ForcastingSettingId == null ? 0 : entity.ForcastingSettingId.Value;
            viewModel.ForcastingSettingLevel = entity.ForcastingSettingLevel;
            viewModel.Id = entity.Id;
            viewModel.IsActive = entity.IsActive;
            viewModel.IsDeleted = entity.IsDeleted;
            viewModel.SupplierCompanyId = entity.SupplierCompanyId;
            viewModel.UpdatedBy = entity.UpdatedBy;
            viewModel.UpdatedDate = entity.UpdatedDate;

            return viewModel;
        }
    }
}
