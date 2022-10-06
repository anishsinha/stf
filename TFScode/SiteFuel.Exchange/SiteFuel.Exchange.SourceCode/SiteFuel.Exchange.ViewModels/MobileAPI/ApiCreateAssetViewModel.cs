using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class ApiCreateAssetViewModel
    {
        public ApiCreateAssetViewModel()
        {

        }

        public int UserId { get; set; }

        public int Id { get; set; }

        public int CompanyId { get; set; }

        public int AssetId { get; set; }

        public Nullable<int> JobId { get; set; }

        public string Name { get; set; }

        public string Class { get; set; }

        public int? SubContractorId { get; set; }

        public decimal FuelCapacity { get; set; }

        public decimal? Make { get; set; }

        public decimal? Model { get; set; }

        public int? Year { get; set; }

        public string Color { get; set; }

        public int? LicensePlateStateId { get; set; }

        public string LicensePlate { get; set; }

        public decimal? VehicleId { get; set; }

        public string Vendor { get; set; }

        public string Description { get; set; }

        public string imageFile { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public AssetFuelTypeViewModel FuelType { get; set; }

        public ImageViewModel Image { get; set; }

        public AssetAdditionalDetailViewModel AssetAdditionalDetail { get; set; }

        public int JobxId { get; set; }
    }
}
