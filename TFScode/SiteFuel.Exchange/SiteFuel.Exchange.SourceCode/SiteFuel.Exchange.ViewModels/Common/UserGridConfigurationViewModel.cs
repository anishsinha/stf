using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;
using System.Web;

namespace SiteFuel.Exchange.ViewModels
{
    public class UserGridConfigurationViewModel : BaseViewModel
    {
        public int UserId { get; set; }

        public GridName GridId { get; set; }

        public List<GridSetting> Setting { get; set; }
    }

    public class GridSetting
    {
        public string Key { get; set; }

        public string Value { get; set; }
    }
}
