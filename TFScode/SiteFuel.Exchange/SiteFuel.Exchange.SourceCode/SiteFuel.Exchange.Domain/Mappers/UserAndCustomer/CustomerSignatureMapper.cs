using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Linq;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class CustomerSignatureMapper
    {
        public static CustomerSignatureViewModel ToViewModel(this Signature entity, CustomerSignatureViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new CustomerSignatureViewModel();

            viewModel.Id = entity.Id;
            viewModel.Name = entity.Signatory;
            viewModel.SignatoryAvailable = entity.SignatoryAvailable;
            viewModel.Image = entity.ImageId.HasValue ? new ImageViewModel { Id = entity.ImageId.Value, Data = entity.Image.Data, FilePath = entity.Image.FilePath } : new ImageViewModel();

            return viewModel;
        }

        public static CustomerSignatureViewModel ToSignatureViewModel(this UspGetSupplierInvoiceDetails entity)
        {
            var viewModel = new CustomerSignatureViewModel();

            viewModel.Id = entity.SignatureId.Value;
            viewModel.Name = entity.Signatory;
            viewModel.SignatoryAvailable = entity.SignatoryAvailable.Value;
            viewModel.Image = entity.SignImageId.HasValue ? new ImageViewModel { Id = entity.SignImageId.Value, FilePath = entity.CustomerSignaturePath } : new ImageViewModel();
            return viewModel;
        }

        public static CustomerSignatureViewModel ToViewModel(this SignatureData entity, CustomerSignatureViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new CustomerSignatureViewModel();

            viewModel.Id = entity.Id;
            viewModel.Name = entity.SignatoryName;
            viewModel.SignatoryAvailable = entity.SignatoryAvailable;
            if (!string.IsNullOrWhiteSpace(entity.SignatureImage))
                viewModel.Image = new ImageViewModel { Data = Convert.FromBase64String(entity.SignatureImage) };
            return viewModel;
        }

        public static Signature ToEntity(this CustomerSignatureViewModel viewModel, Signature entity = null)
        {
            if (entity == null)
                entity = new Signature();

            entity.Id = viewModel.Id;
            entity.Signatory = viewModel.Name;
            entity.SignatoryAvailable = viewModel.SignatoryAvailable;
            entity.IsActive = true;
            if (viewModel.Image != null && viewModel.Image.Id > 0)
            {
                entity.ImageId = viewModel.Image.Id;
            }
            else
            {
                entity.Image = viewModel.Image == null || string.IsNullOrWhiteSpace(viewModel.Image?.FilePath) || viewModel.Image.IsRemoved ? null : viewModel.Image.ToEntity();
            }

            return entity;
        }
    }
}
