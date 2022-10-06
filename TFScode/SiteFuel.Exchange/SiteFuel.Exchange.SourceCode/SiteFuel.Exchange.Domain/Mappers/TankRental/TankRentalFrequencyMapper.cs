using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System.Linq;

namespace SiteFuel.Exchange.Domain.Mappers.TankRental
{
    public static class TankRentalFrequencyMapper
    {
        public static TankRentalFrequencyViewModel ToViewModel(this FuelRequestTankRentalFrequency entity, TankRentalFrequencyViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new TankRentalFrequencyViewModel();

            viewModel.TankRentalFrequencyId = entity.Id;
            viewModel.ActivationStatusId = entity.ActivationStatusId;
            viewModel.FrequencyTypes = (FrequencyTypes)entity.FrequencyTypeId;
            viewModel.FuelRequestId = entity.FuelRequestId;
            viewModel.CreatedBy = entity.CreatedBy;
            entity.TankDetails.Where(t => t.ActivationStatusId != (int)ActivationStatus.Deleted).ToList().ForEach(t => viewModel.Tanks.Add(t.ToViewModel()));

            return viewModel;
        }

        public static FuelRequestTankRentalFrequency ToEntity(this TankRentalFrequencyViewModel viewModel, FuelRequestTankRentalFrequency entity = null)
        {
            if (entity == null)
                entity = new FuelRequestTankRentalFrequency();

            entity.Id = viewModel.TankRentalFrequencyId;
            entity.ActivationStatusId = viewModel.ActivationStatusId;
            entity.CreatedBy = viewModel.CreatedBy;
            entity.CreatedDate = viewModel.CreatedDate;
            entity.ScheduleStartDate = viewModel.Tanks.Min(t => t.StartDate);
            entity.FrequencyTypeId = (int)viewModel.FrequencyTypes;
            entity.FuelRequestId = viewModel.FuelRequestId;
            entity.UpdatedBy = viewModel.CreatedBy;
            entity.UpdatedDate = viewModel.CreatedDate;
            
            foreach (var tank in viewModel.Tanks)
            {
                var tankEntity = new TankDetail();
                tankEntity.ActivationStatusId = viewModel.ActivationStatusId;
                tankEntity.CreatedBy = viewModel.CreatedBy;
                tankEntity.CreatedDate = viewModel.CreatedDate;
                tankEntity.EndDate = tank.EndDate;
                tankEntity.FeeDescription = tank.Description;
                tankEntity.RentalFee = tank.RentalFee;
                tankEntity.RentalFrequencyId = (int)viewModel.FrequencyTypes;
                tankEntity.StartDate = tank.StartDate;
                tankEntity.TaxAmount = tank.FeeTaxDetails.Amount;
                tankEntity.TaxDescription = tank.FeeTaxDetails.Description;
                tankEntity.TaxPercentage = tank.FeeTaxDetails.Percentage;
                tankEntity.UpdatedBy = viewModel.CreatedBy;
                tankEntity.UpdatedDate = viewModel.CreatedDate;

                entity.TankDetails.Add(tankEntity);
            }

            return entity;
        }
    }
}
