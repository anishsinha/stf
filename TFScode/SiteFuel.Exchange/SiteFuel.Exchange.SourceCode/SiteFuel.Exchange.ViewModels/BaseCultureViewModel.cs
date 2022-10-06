using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;

namespace SiteFuel.Exchange.ViewModels
{
    public abstract class BaseCultureViewModel : BaseViewModel
    {
        public BaseCultureViewModel(Status status = Status.Failed) 
            : base (status)
        {
            Culture = ApplicationConstants.Culture_USA;
        }

        public string Culture { get; set; }
    }
}
