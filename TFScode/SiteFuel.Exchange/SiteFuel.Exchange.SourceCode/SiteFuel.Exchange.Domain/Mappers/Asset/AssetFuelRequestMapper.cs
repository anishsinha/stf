using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class AssetFuelRequestMapper
    {
        public static List<AssetDropRequest> ToEntity(this List<AssetFuelRequestInputViewModel> viewModel, List<AssetDropRequest> entities = null)
        {
            if (entities == null)
                entities = new List<AssetDropRequest>();

            foreach (var item in viewModel)
            {
                AssetDropRequest entity = new AssetDropRequest()
                {
                    AssetId = item.AssetId,
                    AssetExternalId = item.AssetExternalId,
                    Latitude = item.Latitude,
                    Longitude = item.Longitude,
                    QuantityRequired = item.QuantityRequired,
                    FuelRequestId = item.FuelRequestId,
                    IsThisRequestClosed = item.IsThisRequestClosed
                };
                entities.Add(entity);
            }

            return entities;
        }
    }
}
