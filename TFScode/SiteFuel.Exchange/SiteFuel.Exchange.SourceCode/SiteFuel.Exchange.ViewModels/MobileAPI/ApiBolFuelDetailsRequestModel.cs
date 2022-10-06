using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;
using System.Web;

namespace SiteFuel.Exchange.ViewModels
{
    public class ApiBolFuelDetailsRequestModel
    {
        public string BolNumber { get; set; }

        public string Carrier { get; set; }

        public bool IsDriverToUpdateBOL { get; set; }

        public bool IsBulkPlant { get; set; }

        public int UserId { get; set; }

        public int Offset { get; set; } = -400;

        public HttpPostedFile BolFile { get; set; }

        public HttpPostedFile AdditionalFile { get; set; }

        public List<BolFuelDetailViewModel> BolFuelDetails { get; set; }
    }

    public class BolFuelDetailViewModel
    {
        public decimal? GrossQuantity { get; set; }

        public decimal NetQuantity { get; set; }

        public int FuelTypeId { get; set; }

        public bool IsFtl { get; set; }

        public List<ScheduleModel> Schedules { get; set; }
    }

    public class ScheduleModel
    {
        public int ScheduleId { get; set; }

        public int TrackableScheduleId { get; set; }

        public int OrderId { get; set; }
    }

    public class ApiBolFuelDetailsResponseModel : StatusViewModel
    {
        public List<BolFuelDetailsResponseViewModel> BolFuelDetailResponse { get; set; }
    }

    public class BolFuelDetailsResponseViewModel
    {
        public int BolId { get; set; }

        public int OrderId { get; set; }

        public int? BolImageId { get; set; }

        public int? AdditionalImageId { get; set; }

        public int FuelTypeId { get; set; }
    }
}
