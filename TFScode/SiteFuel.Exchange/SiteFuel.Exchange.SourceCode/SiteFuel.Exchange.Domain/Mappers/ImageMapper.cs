using SiteFuel.Exchange.Core.Infrastructure;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class ImageMapper
    {
        public static ImageViewModel ToViewModel(this Image entity, ImageViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new ImageViewModel(Status.Success);

            viewModel.Id = entity.Id;
            viewModel.Data = entity.Data;
            if (!string.IsNullOrWhiteSpace(entity.FilePath))
            {

                viewModel.FilePath = entity.FilePath;
            }
            return viewModel;
        }

        public static Image ToEntity(this ImageViewModel viewModel, Image entity = null)
        {
            if (entity == null)
                entity = new Image();

            entity.Id = viewModel.Id;

            //dummy data for pdf
            entity.Data = new byte[] { 0x20 };
            entity.FilePath = viewModel.FilePath;
            return entity;
        }

        public static CustomerSignatureViewModel ToCustomerSignature(this ImageViewModel entity, CustomerSignatureViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new CustomerSignatureViewModel();

            viewModel.Id = entity.Id;
            viewModel.Image = entity;
            viewModel.Name = entity.SignatureName;
            viewModel.SignatoryAvailable = true;
            viewModel.IsJobSignatureEnabled = true;

            return viewModel;
        }

        public static List<ImageViewModel> ToBreakFilePathToMany( this ImageViewModel imageViewModel , List<ImageViewModel> images=null)
        {
                images = imageViewModel.FilePath.Split(ApplicationConstants.FilePathSeparation, System.StringSplitOptions.RemoveEmptyEntries).
                    Select(x => new ImageViewModel { FilePath = x, IsPdf = ImageViewModel.IsFilePathTypeIsNonImage(x) }).ToList();

            return images;
        }
    }
}
