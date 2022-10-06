using MongoDB.Bson;
using SiteFuel.Exchange.Core.Infrastructure;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.MdbDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightRepository.Mappers
{
    public static class TankMakeAndModelMapper
    {
        public static TankModalType ToEntity(this TankModalTypeViewModel model)
        {
            var entity = new TankModalType();

            if (!string.IsNullOrEmpty(model.Id)) { entity.Id = ObjectId.Parse(model.Id); }
            entity.CreatedBy = model.CreatedBy;
            entity.BuyerCompanyId = model.BuyerCompanyId;
            entity.SupplierCompanyId = model.SupplierCompanyId;
            entity.CreatedOn = model.CreatedOn;
            entity.Modal = model.Modal;
            entity.ScaleMeasurement = model.ScaleMeasurement;
            entity.Name = model.Name;
            entity.IsActive = model.IsActive;
            entity.PdfFilePath = model.PdfFilePath;
            entity.DipChartDetails = new List<DipChartDetails>();
            entity.CreatedByCompanyId = model.CreatedByCompanyId;
            foreach (var item in model.DipChartDetails)
            {
                var current = new DipChartDetails
                {
                    Dip = item.Dip,
                    Ullage = item.Ullage,
                    Volume = item.Volume
                };
                entity.DipChartDetails.Add(current);
            }
            return entity;
        }

        public static TankModalTypeViewModel ToViewModel(this TankModalType entity)
        {
            var model = new TankModalTypeViewModel();

            if (entity.Id != ObjectId.Empty) { model.Id = entity.Id.ToString(); }
            model.CreatedBy = entity.CreatedBy;
            model.BuyerCompanyId = entity.BuyerCompanyId;
            model.SupplierCompanyId = entity.SupplierCompanyId;
            model.CreatedOn = entity.CreatedOn;
            model.Modal = entity.Modal;
            model.Name = entity.Name;
            model.ScaleMeasurement = entity.ScaleMeasurement;
            if (entity.ScaleMeasurement == (int)TankScaleMeasurement.Cm)
                model.ScaleMeasurementText = Resource.lblCm;
            else if (entity.ScaleMeasurement == (int)TankScaleMeasurement.Inches)
                model.ScaleMeasurementText = Resource.lblInches;
            model.IsActive = entity.IsActive;
            model.PdfFilePath = string.Format(ApplicationConstants.FilePathFormat, AzureBlobStorage.GetStorageAccountName(), BlobContainerType.TankTypeDipChart.ToString().ToLower(), entity.PdfFilePath.ToString(), AzureBlobStorage.GetSaS(BlobContainerType.TankTypeDipChart.ToString().ToLower()));
            model.CreatedByCompanyId = entity.CreatedByCompanyId;
            model.DipChartDetails = new List<TankTypeDipChartDetailsViewModel>();

            foreach (var item in entity.DipChartDetails)
            {
                var current = new TankTypeDipChartDetailsViewModel
                {
                    Dip = item.Dip,
                    Ullage = item.Ullage,
                    Volume = item.Volume
                };
                model.DipChartDetails.Add(current);
            }
            return model;
        }
    }
}
