using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class FeeByQuantityMapper
    {
        public static FeeByQuantity ToEntity(this DeliveryFeeByQuantityViewModel viewModel)
        {
            var entity = new FeeByQuantity
            {
                Id = viewModel.Id,
                FeeTypeId = viewModel.FeeTypeId,
                FeeSubTypeId = viewModel.FeeSubTypeId,
                MinQuantity = viewModel.MinQuantity,
                MaxQuantity = viewModel.MaxQuantity,
                Fee = viewModel.Fee,
                MarginTypeId = viewModel.MarginTypeId,
                Margin = viewModel.Margin,
                FuelFeesId = viewModel.FuelFeesId,
                Currency = viewModel.Currency,
                UoM = viewModel.UoM
            };
            return entity;
        }

        public static List<FeeByQuantity> ToEntity(this List<FeesViewModel> viewModel, FeeByQuantity entity = null)
        {
            var entities = new List<FeeByQuantity>();

            foreach (var feeModel in viewModel)
            {
                int feeTypeId = 0;
                var isCommonFee = int.TryParse(feeModel.FeeTypeId, out feeTypeId);
                if (!isCommonFee)
                    feeTypeId = (int)FeeType.OtherFee;

                foreach (var deliveryFees in feeModel.DeliveryFeeByQuantity)
                {
                    entity = new FeeByQuantity();
                    entity.Id = deliveryFees.Id;
                    entity.FeeTypeId = feeTypeId;
                    entity.FeeSubTypeId = feeModel.FeeSubTypeId;
                    entity.MinQuantity = deliveryFees.MinQuantity;
                    entity.MaxQuantity = deliveryFees.MaxQuantity;
                    entity.Fee = deliveryFees.Fee;
                    entities.Add(entity);
                }
            }

            return entities;
        }

        public static List<FeeByQuantity> ToInvoiceFeesEntity(this List<FeesViewModel> viewModel, DateTime dateToCompare, FeeByQuantity entity = null)
        {
            var entities = new List<FeeByQuantity>();

            foreach (var feeModel in viewModel)
            {
                int feeTypeId = 0;
                var isCommonFee = int.TryParse(feeModel.FeeTypeId, out feeTypeId);
                if (!isCommonFee)
                    feeTypeId = (int)FeeType.OtherFee;

                var isFeeApplicable = true;
                if (viewModel.Any(t => t.FeeConstraintTypeId.HasValue))
                    isFeeApplicable = FuelRequestFeeMapper.CheckFeeApplicableConstraint(feeModel, viewModel, dateToCompare);

                if (isFeeApplicable)
                {
                    foreach (var deliveryFees in feeModel.DeliveryFeeByQuantity)
                    {
                        entity = new FeeByQuantity();
                        entity.Id = deliveryFees.Id;
                        entity.FeeTypeId = feeTypeId;
                        entity.FeeSubTypeId = feeModel.FeeSubTypeId;
                        entity.MinQuantity = deliveryFees.MinQuantity;
                        entity.MaxQuantity = deliveryFees.MaxQuantity;
                        entity.Fee = deliveryFees.Fee;
                        entities.Add(entity);
                    }
                }
            }

            return entities;
        }

        public static DeliveryFeeByQuantityViewModel ToViewModel(this UspGetFuelRequestFeeDetailViewModel entity)
        {
            DeliveryFeeByQuantityViewModel viewModel = new DeliveryFeeByQuantityViewModel(Status.Success);

            viewModel.Id = entity.FeeByQuantityId.Value;
            viewModel.FeeTypeId = entity.FeeByQuantityTypeId.Value;
            viewModel.FeeSubTypeId = entity.FeeByQuantitySubTypeId.Value;
            viewModel.MinQuantity = entity.FeeByQuantityMinQuantity.Value.GetPreciseValue(6);
            viewModel.MaxQuantity = entity.FeeByQuantityMaxQuantity.HasValue ? entity.FeeByQuantityMaxQuantity.Value.GetPreciseValue(6) : entity.FeeByQuantityMaxQuantity;
            viewModel.Fee = entity.FeeByQuantityFee.Value.GetPreciseValue(6);
            viewModel.Currency = entity.Currency;
            viewModel.UoM = entity.UoM;

            return viewModel;
        }

        public static DeliveryFeeByQuantityViewModel ToViewModel(this FeeByQuantity entity, DeliveryFeeByQuantityViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new DeliveryFeeByQuantityViewModel(Status.Success);

            viewModel.Id = entity.Id;
            viewModel.FeeTypeId = entity.FeeTypeId;
            viewModel.FeeSubTypeId = entity.FeeSubTypeId;
            viewModel.MinQuantity = entity.MinQuantity.GetPreciseValue(6);
            viewModel.MaxQuantity = entity.MaxQuantity.HasValue ? entity.MaxQuantity.Value.GetPreciseValue(6) : entity.MaxQuantity;
            viewModel.Fee = entity.Fee.GetPreciseValue(6);
            viewModel.Currency = entity.Currency;
            viewModel.UoM = entity.UoM;

            return viewModel;
        }

        public static DeliveryFeeByQuantityViewModel ToBrokerViewModel(this FeeByQuantity entity, DeliveryFeeByQuantityViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new DeliveryFeeByQuantityViewModel();

            viewModel.Id = entity.Id;
            viewModel.FeeTypeId = entity.FeeTypeId;
            viewModel.FeeSubTypeId = entity.FeeSubTypeId;
            viewModel.MinQuantity = entity.MinQuantity.GetPreciseValue(6);
            viewModel.MaxQuantity = entity.MaxQuantity.HasValue ? entity.MaxQuantity.Value.GetPreciseValue(6) : 0;
            viewModel.Fee = entity.Fee.GetPreciseValue(6);

            return viewModel;
        }
    }
}
