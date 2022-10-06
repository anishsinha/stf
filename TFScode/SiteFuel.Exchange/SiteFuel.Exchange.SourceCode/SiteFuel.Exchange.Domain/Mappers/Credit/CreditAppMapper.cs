using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class CreditAppMapper
    {
        public static CreditAppViewModel ToViewModel(this CreditAppDetail entity, CreditAppViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new CreditAppViewModel(Status.Success);

            viewModel.Id = entity.Id;
            viewModel.Body = entity.EmailContent.Replace("</br>", Environment.NewLine);
            viewModel.FromEmail = entity.From;
            viewModel.IsEnabled = entity.Company.IsCreditAppEnabled;
            viewModel.Subject = entity.EmailSubject;
            viewModel.CompanyId = entity.CompanyId;

            return viewModel;
        }

        public static CreditAppDetail ToEntity(this CreditAppViewModel viewModel, CreditAppDetail entity = null)
        {
            if (entity == null)
                entity = new CreditAppDetail();

            entity.Id = viewModel.Id;
            entity.CompanyId = viewModel.CompanyId;
            entity.EmailContent = viewModel.Body.Replace(Environment.NewLine, "</br>");
            entity.From = viewModel.FromEmail;
            entity.EmailSubject = viewModel.Subject;
            if (entity.Company != null)
                entity.Company.IsCreditAppEnabled = viewModel.IsEnabled;

            entity.IsActive = viewModel.IsActive;
            entity.UpdatedBy = viewModel.UpdatedBy;
            entity.UpdatedDate = viewModel.UpdatedDate;

            return entity;
        }
    }
}
