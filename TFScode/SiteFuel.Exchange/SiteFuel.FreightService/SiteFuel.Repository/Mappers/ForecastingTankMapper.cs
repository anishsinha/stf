using SiteFuel.FreightModels.ForcastingHelpers;
using SiteFuel.MdbDataAccess.Collections;
using System.Collections.Generic;

namespace SiteFuel.FreightRepository.Mappers
{
    public static class ForecastingTankMapper
    {
        public static ForecastingTankInformation ToEntity(this ForecastingTankInformationModel input)
        {
            var entity = new ForecastingTankInformation();
            entity.SiteId = input.SiteId;
            entity.TankId = input.TankId;
            entity.StorageId = input.StorageId;
            entity.DaysLeft = input.DaysLeft;
            entity.EstimatedCurrentInventory = input.EstimatedCurrentInventory;
            entity.TankInformation = input.TankInformation.ToTankInfoEntity();
            return entity;
        }
        public static TankInformation ToTankInfoEntity(this TankInformationModel input)
        {
            var entity = new TankInformation();
            entity.Date = input.Date;
            entity.BandNumber = input.BandNumber;
            entity.SaleTankId = input.SaleTankId;
            entity.TotalSale = input.TotalSale;
            entity.AverageSale = input.AverageSale;
            return entity;
        }
        public static ForecastingTankInformationModel ToEntity(this ForecastingTankInformation input)
        {
            var entity = new ForecastingTankInformationModel();
            entity.SiteId = input.SiteId;
            entity.TankId = input.TankId;
            entity.StorageId = input.StorageId;
            entity.DaysLeft = input.DaysLeft;
            entity.EstimatedCurrentInventory = input.EstimatedCurrentInventory;
            entity.TankInformation = input.TankInformation.ToTankInfoEntity();
            return entity;
        }
        public static TankInformationModel ToTankInfoEntity(this TankInformation input)
        {
            var entity = new TankInformationModel();
            entity.Date = input.Date;
            entity.BandNumber = input.BandNumber;
            entity.SaleTankId = input.SaleTankId;
            entity.TotalSale = input.TotalSale;
            entity.AverageSale = input.AverageSale;
            return entity;
        }
    }
}
