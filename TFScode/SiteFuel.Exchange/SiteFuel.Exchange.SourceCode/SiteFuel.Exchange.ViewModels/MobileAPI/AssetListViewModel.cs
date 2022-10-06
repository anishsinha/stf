using SiteFuel.Exchange.Core.StringResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class AssetListViewModel : StatusViewModel
    {
        public AssetListViewModel()
        {

        }

        public int AssetId { get; set; }

        public String AssetName { get; set; }

        public int? ImageId { get; set; }

        public int? jobXAssetsId { get; set; }

        public String CurrentJobName { get; set; }

        public int? CurrentJobId { get; set; }
        public string FilePath { get; set; }

    }
}
