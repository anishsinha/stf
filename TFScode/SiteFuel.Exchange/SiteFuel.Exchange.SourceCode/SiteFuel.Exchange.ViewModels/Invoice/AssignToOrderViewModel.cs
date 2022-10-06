using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class AssignToOrderViewModel : BaseViewModel
    {
        public AssignToOrderViewModel()
        {
            
        }

        public AssignToOrderViewModel(Status status) : base(status)
        {
           
        }
        public int OrderId { get; set; }
        public int InvoiceId { get; set; }
        public UoM InvoiceUoM { get; set; }
    }
}
