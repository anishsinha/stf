using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class UserPageSettingViewModel : BaseCultureViewModel
    {
        public UserPageSettingViewModel()
        {
            TileDetails = new List<DashboardTileViewModel>();
        }

        public int Id { get; set; }

        public int UserId { get; set; }

        public string PageId { get; set; }

        public List<DashboardTileViewModel> TileDetails { get; set; }
    }
}
