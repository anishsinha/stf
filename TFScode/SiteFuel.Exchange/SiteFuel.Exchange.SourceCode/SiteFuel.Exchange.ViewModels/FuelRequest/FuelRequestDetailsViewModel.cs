using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class FuelRequestDetailsViewModel : BaseCultureViewModel
    {
        public FuelRequestDetailsViewModel()
        {
            InstanceInitialize();
        }

        public FuelRequestDetailsViewModel(Status status)
            : base(status)
        {
            InstanceInitialize(status);
        }

        private void InstanceInitialize(Status status = Status.Failed)
        {
            Supplier = new ContactPersonViewModel(status);
            ContactPersons = new List<ContactPersonViewModel>();
            FuelRequest = new FuelRequestViewModel(status);
            FuelDeliveryDetails = new FuelDeliveryDetailsViewModel(status);
        }

        public ContactPersonViewModel Supplier { get; set; }

        public IList<ContactPersonViewModel> ContactPersons { get; set; }

        public FuelRequestViewModel FuelRequest { get; set; }

        public int ExternalBrokerId { get; set; }

        public TPOBrokeredOrderViewModel BrokeredOrder { get; set; }

        public UserViewModel User { get; set; }

        public FuelDeliveryDetailsViewModel FuelDeliveryDetails { get; set; }
    }
}
