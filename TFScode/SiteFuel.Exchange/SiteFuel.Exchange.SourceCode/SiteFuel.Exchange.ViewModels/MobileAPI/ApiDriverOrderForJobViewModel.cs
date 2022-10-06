using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class ApiDriverOrderForJobViewModel
    {
        public List<ApiOrderDetailsForJobViewModel> Orders { get; set; }

        public List<ApiTankDetailViewModel> Tanks { get; set; }

        public int AssetCount { get; set; }
    }
}
