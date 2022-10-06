using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System.Linq;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class InvoiceHeaderMapper
    {
        public static InvoiceHeaderViewModel ToViewModel(this InvoiceHeaderDetail entity, InvoiceHeaderViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new InvoiceHeaderViewModel(Status.Success);

            viewModel.Id = entity.Id;
            viewModel.InvoiceNumberId = entity.InvoiceNumberId;
            viewModel.TotalDroppedGallons = entity.TotalDroppedGallons;
            viewModel.TotalBasicAmount = entity.TotalBasicAmount;
            viewModel.TotalFeeAmount = entity.TotalFeeAmount;
            viewModel.TotalTaxAmount = entity.TotalTaxAmount;
            viewModel.TotalDiscountAmount = entity.TotalDiscountAmount;
            viewModel.Version = entity.Version;

            return viewModel;
        }

        public static InvoiceHeaderDetail ToEntity(this InvoiceHeaderViewModel viewModel, InvoiceHeaderDetail entity = null)
        {
            if (entity == null)
                entity = new InvoiceHeaderDetail();

            entity.Id = viewModel.Id;
            entity.InvoiceNumberId = viewModel.InvoiceNumberId;
            entity.TotalDroppedGallons = viewModel.TotalDroppedGallons;
            entity.TotalBasicAmount = viewModel.TotalBasicAmount;
            entity.TotalFeeAmount = viewModel.TotalFeeAmount;
            entity.TotalTaxAmount = viewModel.TotalTaxAmount;
            entity.TotalDiscountAmount = viewModel.TotalDiscountAmount;
            entity.Version = viewModel.Version;
            entity.CreatedBy = viewModel.CreatedBy;
            entity.CreatedDate = viewModel.CreatedDate;
            entity.UpdatedBy = viewModel.UpdatedBy;
            entity.UpdatedDate = viewModel.UpdatedDate;
            entity.IsActive = true;
            return entity;
        }
    }
}
