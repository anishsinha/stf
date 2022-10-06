namespace SiteFuel.Exchange.ViewModels
{
    public class ScheduleBuilderResponseModel : StatusViewModel
    {
        public string Id { get; set; }
        public long TimeStamp { get; set; }
        public int DeliveryRequestStatus { get; set; }
    }
}
