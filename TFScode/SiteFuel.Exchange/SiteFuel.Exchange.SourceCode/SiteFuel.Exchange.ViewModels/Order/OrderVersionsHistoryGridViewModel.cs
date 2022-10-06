using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class OrderVersionsHistoryGridViewModel : StatusViewModel
    {
        public OrderVersionsHistoryGridViewModel()
        {
           
        }

        public OrderVersionsHistoryGridViewModel(Status status) 
            : base(status)
        {
          
        }


        public int OrderDetailVersionId { get; set; }

        public string Version { get; set; }

        public string PoNumber { get; set; }

        public string PaymentTerm { get; set; }

        public string NetDays { get; set; }

        public string DateModified { get; set; }

        public string ModifiedBy { get; set; }
    }
}
