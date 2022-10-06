using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class AssignToOrderGridViewModel : BaseViewModel
    {
        public AssignToOrderGridViewModel()
        {
           
        }

        public AssignToOrderGridViewModel(Status status) : base(status)
        {
           
        }
        public int OrderId { get; set; }
        public string PoNumber { get; set; }
        public string CustomerName { get; set; }
        public string Location { get; set; }
        public string GallonsOrdered { get; set; }
        public string OrderUoM { get; set; }
    }
}
