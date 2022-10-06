using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class AssetDetailViewModel : BaseViewModel
    {
        public AssetDetailViewModel()
        {
            InstanceInitialize(Status.Failed);
        }

        public AssetDetailViewModel(Status status): base(status)
        {
            InstanceInitialize(status);
        }
        private void InstanceInitialize(Status status)
        {
            Asset = new AssetViewModel(status);
            Currency = Currency.USD;
            UoM = UoM.Gallons;
        }


        public AssetViewModel Asset { get; set; }

        [Display(Name = nameof(Resource.lblJobName), ResourceType = typeof(Resource))]
        public string JobName { get; set; }

        [Display(Name = nameof(Resource.lblDateAssigned), ResourceType = typeof(Resource))]
        public Nullable<DateTimeOffset> DateAssigned { get; set; }

        [Display(Name = nameof(Resource.lblTotalDrops), ResourceType = typeof(Resource))]
        public int TotalDrops { get; set; }

        [Display(Name = nameof(Resource.lblTotalFuel), ResourceType = typeof(Resource))]
        public decimal TotalFuel { get; set; }

        [Display(Name = nameof(Resource.lblTotalFuelCost), ResourceType = typeof(Resource))]
        public decimal TotalFuelCost { get; set; }

        public Currency Currency { get; set; }
        
        public UoM UoM { get; set; }
    }
}
