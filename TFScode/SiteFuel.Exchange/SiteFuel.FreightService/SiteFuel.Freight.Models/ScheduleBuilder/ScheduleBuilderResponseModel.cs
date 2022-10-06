namespace SiteFuel.FreightModels.ScheduleBuilder
{
    public class ScheduleBuilderResponseModel : StatusModel
    {
        public string Id { get; set; }
        public long TimeStamp { get; set; }
        public int DeliveryRequestStatus { get; set; }
    }
}
