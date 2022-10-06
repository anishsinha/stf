using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class ResaleGridViewModel : BaseViewModel
    {
        public ResaleGridViewModel()
        {
            
        }

        public ResaleGridViewModel(Utilities.Status status)
            : base(status)
        {
            
        }

        public int Id { get; set; }

        public int JobId { get; set; }

        public string CustomerName { get; set; }

        public string ContractNumber { get; set; }

        public string OrderNumber { get; set; }

        public string FuelType { get; set; }

        public int Assets { get; set; }

        public string Quantity { get; set; }

        public string RackPPGPaid { get; set; }

        public string RackPPGSold { get; set; }

        public string CreatedBy { get; set; }

        public string Status { get; set; }
    }
}
