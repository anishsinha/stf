using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.Web.Models
{
    public class FuelRequestDataTableModel : DataTableAjaxPostModel
    {
        public FuelRequestDataTableModel()
        {
            FuelRequestTypeFilter = FuelRequestType.All;
            Filter = FuelRequestFilterType.All;
            BrodcastType = BroadcastType.All;
        }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public int AddressId { get; set; }

        public int JobId { get; set; }

        public Currency Currency { get; set; }

        public int CountryId { get; set; }

        public string GroupIds { get; set; }

        public FuelRequestType FuelRequestTypeFilter { get; set; }

        public FuelRequestFilterType Filter { get; set; }

        public BroadcastType BrodcastType { get; set; }
    }
}