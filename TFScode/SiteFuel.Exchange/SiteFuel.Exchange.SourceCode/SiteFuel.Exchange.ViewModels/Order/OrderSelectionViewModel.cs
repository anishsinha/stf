using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class OrderSelectionViewModel : BaseViewModel
    {
        public OrderSelectionViewModel()
        {
           
        }

        public int OrderId { get; set; }

        public int UserId { get; set; }

        public int SelectedOrderId { get; set; }
    }
}
