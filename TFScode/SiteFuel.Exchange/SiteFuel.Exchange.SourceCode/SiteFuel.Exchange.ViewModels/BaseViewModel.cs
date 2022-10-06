using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;

namespace SiteFuel.Exchange.ViewModels
{
    public abstract class BaseViewModel : StatusViewModel
    {
        public BaseViewModel(Status status = Status.Failed) 
            : base (status)
        {
            IsActive = true;
            UpdatedBy = (int)SystemUser.System;
            UpdatedDate = DateTimeOffset.Now;
        }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }
    }
}
