using Newtonsoft.Json;
using SiteFuel.Exchange.Core.StringResources;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class DashboardTileViewModel
    {
        [JsonProperty(PropertyName = "n")]
        public string TileName { get; set; }

        [JsonProperty(PropertyName = "r")]
        public int RowIdx { get; set; }

        [JsonProperty(PropertyName = "c")]
        public int ColIdx { get; set; }

        [JsonProperty(PropertyName = "m")]
        public bool IsCollapsed { get; set; }

        [JsonProperty(PropertyName = "x")]
        public bool IsClosed { get; set; }

        [JsonProperty(PropertyName = "d")]
        public string TileDisplayName { get; set; }
    }
}
