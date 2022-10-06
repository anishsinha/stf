using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class OrderHistoryGridViewModel : StatusViewModel
    {
        public OrderHistoryGridViewModel()
        {
            InstanceInitialize();
        }

        public OrderHistoryGridViewModel(Status status) 
            : base(status)
        {
            InstanceInitialize(status);
        }

        private void InstanceInitialize(Status status = Utilities.Status.Failed)
        {

        }
        public int Id { get; set; }

        public string PoNumber { get; set; }

        public string Supplier { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string PricePerGallon { get; set; }

        public string FuelDeliveredPercentage { get; set; }

        public string Eligibility { get; set; }

        public string DateModified { get; set; }

        public string ModifiedBy { get; set; }

        public int Version { get; set; }
    }
}
