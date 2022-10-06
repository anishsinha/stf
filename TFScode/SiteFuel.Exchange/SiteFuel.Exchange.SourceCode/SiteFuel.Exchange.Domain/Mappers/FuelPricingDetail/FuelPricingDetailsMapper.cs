using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.FuelPricingDatail;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class FuelPricingDetailsMapper
    {
        public static FuelRequestPricingDetailsViewModel ToViewModel(this FuelRequestPricingDetail entity, FuelRequestPricingDetailsViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new FuelRequestPricingDetailsViewModel();

            if (entity == null) 
                return null;

            viewModel.RequestPriceDetailId = entity.RequestPriceDetailId;
            viewModel.FuelRequestId = entity.FuelRequestId;
            viewModel.PricingCode = new PricingCodeDetailViewModel { Code = entity.PricingCode, Description = entity.DisplayPriceCode };

            if (entity.FuelRequest.FuelRequestDetail.TruckLoadTypeId == (int)TruckLoadTypes.FullTruckLoad)
            {
                viewModel.StateDefaultQuantityIndicatorId = entity.FuelRequest.Job.MstState.QuantityIndicatorTypeId;
                if (entity.FuelRequest.FreightOnBoardTypeId.HasValue)
                    viewModel.FreightOnBoardTypes = (FreightOnBoardTypes)entity.FuelRequest.FreightOnBoardTypeId;

            }
            return viewModel;
        }

        public static FuelRequestPricingDetailsViewModel ToFuelRequestViewModel(this FuelRequestPricingDetail entity, FuelRequestPricingDetailsViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new FuelRequestPricingDetailsViewModel();

            if (entity == null) 
            {
                viewModel.PricingSourceId = (int)PricingSource.Axxis;
            }
            else
            {
                viewModel.RequestPriceDetailId = entity.RequestPriceDetailId;
                viewModel.FuelRequestId = entity.FuelRequestId;
                viewModel.PricingCode = new PricingCodeDetailViewModel { Code = entity.PricingCode, Description = entity.DisplayPriceCode };
                if (entity.FuelRequest.FuelRequestDetail.TruckLoadTypeId == (int)TruckLoadTypes.FullTruckLoad)
                {
                    viewModel.StateDefaultQuantityIndicatorId = entity.FuelRequest.Job.MstState.QuantityIndicatorTypeId;
                    if (entity.FuelRequest.FreightOnBoardTypeId.HasValue)
                        viewModel.FreightOnBoardTypes = (FreightOnBoardTypes)entity.FuelRequest.FreightOnBoardTypeId;
                }
            }

            return viewModel;
        }

        public static FuelRequestPricingDetail ToPricingDetailsEntity(this FuelRequestPricingDetailsViewModel viewModel, bool isMarketPricing, FuelRequestPricingDetail entity = null)
        {
            if (entity == null)
                entity = new FuelRequestPricingDetail();

            entity.PricingCode = viewModel.PricingCode.Code;
            if (viewModel.RequestPriceDetailId > 0)
                entity.RequestPriceDetailId = viewModel.RequestPriceDetailId;
            entity.PricingCodeId = viewModel.PricingCode.Id;
            return entity;
        }

        public static FuelRequestPricingDetail ToPricingDetailsEntityForTPO(this ThirdPartyOrderViewModel viewModel, FuelRequestPricingDetail entity = null)
        {
            if (entity == null)
                entity = new FuelRequestPricingDetail();

            entity.FuelRequestId = viewModel.FuelRequestId;
            entity.PricingCodeId = viewModel.PricingDetails.FuelPricingDetails.PricingCode.Id;
            entity.PricingCode = viewModel.PricingDetails.FuelPricingDetails.PricingCode.Code;
            entity.RequestPriceDetailId = viewModel.PricingDetails.FuelPricingDetails.RequestPriceDetailId;
            return entity;
        }
    }
}
