using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class DeclineInvoiceViewModel
    {
        public DeclineInvoiceViewModel()
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            DeclineReason = new DeclineReasonViewModel();
            UoM = UoM.Gallons;
        }
        public int UserId { get; set; }

        public DeclineReasonViewModel DeclineReason { get; set; }

        public UoM UoM { get; set; }
    }
}
