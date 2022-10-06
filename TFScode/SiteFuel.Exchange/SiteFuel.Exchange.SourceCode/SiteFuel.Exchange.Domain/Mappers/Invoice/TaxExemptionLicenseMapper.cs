using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class TaxExemptionLicenseMapper
    {
        public static TaxExemptLicens ToEntity(this TaxExemptionViewModel viewModel, TaxExemptLicens entity = null)
        {
            var accountCustomId = string.Empty;
            if (entity != null)
            {
                accountCustomId = entity.AccountCustomId;
            }
            if (entity == null)
                entity = new TaxExemptLicens();

            entity.BillOfLadingDate = viewModel.EffectiveDate;
            entity.EffectiveDate = viewModel.EffectiveDate;
            entity.IDType = viewModel.IDType;
            entity.IDCode = viewModel.IDCode;
            entity.BusinessSubType = viewModel.BusinessSubType;
            entity.TradeName = viewModel.TradeName;
            entity.LegalName = viewModel.LegalName;
            entity.CompanyId = viewModel.CompanyId;
            entity.Address = viewModel.Address;
            entity.City = viewModel.City;
            entity.StateId = viewModel.State.Id;
            entity.PostalCode = viewModel.ZipCode;
            entity.CountryId = viewModel.Country.Id;
            entity.LicenseNumber = string.IsNullOrWhiteSpace(viewModel.LicenseNumber) ? viewModel.IDCode : viewModel.LicenseNumber;
            entity.County = viewModel.County;
            entity.LicenseNumber = viewModel.LicenseNumber;
            entity.LicensePercentage = viewModel.LicensePercentage;
            entity.IsDefault = viewModel.IsDefault;
            entity.IsActive = true;
            entity.Status = (int)TaxExemptionLicenseStatus.Open;
            entity.UpdatedBy = viewModel.UserId;
            entity.CreatedDate = DateTimeOffset.Now;
            entity.UpdatedDate = DateTimeOffset.Now;
            entity.BusinessType = viewModel.BusinessType;
            entity.AccountCustomId = accountCustomId;
            entity.IsSameCompanyAddress = viewModel.IsSameCompanyAddress;
            return entity;
        }

        public static TaxExemptionViewModel ToViewModel(this TaxExemptLicens entity, TaxExemptionViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new TaxExemptionViewModel();
            viewModel.Id = entity.Id;
            viewModel.EntityCustomId = entity.EntityCustomId;
            viewModel.AccountCustomId = entity.AccountCustomId;
            viewModel.EffectiveDate = entity.EffectiveDate;
            viewModel.IDType = entity.IDType;
            viewModel.IDCode = entity.IDCode;
            viewModel.IsDefault = entity.IsDefault;
            viewModel.BusinessSubType = entity.BusinessSubType;
            viewModel.BusinessSubTypeVal = entity.MstBusinessSubType.Code;
            viewModel.TradeName = entity.TradeName;
            viewModel.LegalName = entity.LegalName;
            viewModel.CompanyId = entity.CompanyId;
            viewModel.Jurisdiction = entity.Jurisdiction;
            viewModel.ObsoleteDate = entity.ObsoleteDate;
            viewModel.Address = entity.Address;
            viewModel.City = entity.City;
            viewModel.State.Id = entity.StateId;
            viewModel.ZipCode = entity.PostalCode;
            viewModel.LicenseNumber = string.IsNullOrWhiteSpace(entity.LicenseNumber) ? entity.IDCode : entity.LicenseNumber;
            viewModel.County = entity.County;
            viewModel.LicenseNumber = entity.LicenseNumber;
            viewModel.IsSameCompanyAddress = entity.IsSameCompanyAddress;
            return viewModel;
        }
    }
}
