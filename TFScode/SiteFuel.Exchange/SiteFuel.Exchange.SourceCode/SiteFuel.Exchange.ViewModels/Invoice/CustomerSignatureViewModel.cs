namespace SiteFuel.Exchange.ViewModels
{
    public class CustomerSignatureViewModel : BaseViewModel
    {
        public CustomerSignatureViewModel()
        {
            Image = new ImageViewModel();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public ImageViewModel Image { get; set; }
        public bool SignatoryAvailable { get; set; }
        public bool IsJobSignatureEnabled { get; set; }
    }
}
