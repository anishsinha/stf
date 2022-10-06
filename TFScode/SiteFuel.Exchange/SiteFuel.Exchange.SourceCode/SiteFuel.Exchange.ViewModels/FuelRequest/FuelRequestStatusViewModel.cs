namespace SiteFuel.Exchange.ViewModels
{
    public class FuelRequestStatusViewModel : StatusViewModel
    {
        public bool IsFirstTimeBuyer { get; set; }

        public string ToUser { get; set; }

        public string ToUserEmail { get; set; }

        public int OrderId { get; set; }
    }
}
