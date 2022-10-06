using SiteFuel.Exchange.Utilities;
using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class AssetFuelTypeViewModel : BaseViewModel
    {
        public AssetFuelTypeViewModel()
        {
        }

        public AssetFuelTypeViewModel(Status status)
            : base(status)
        {
        }

        public Nullable<int> Id { get; set; }

        public string Name { get; set; }
    }
}
