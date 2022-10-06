namespace SiteFuel.Exchange.ViewModels.WebNotification
{
    public class NotificationOfferQueMsg
    {
        public int OfferId { get; set; }
        public string OfferNumber { get; set; }
        public int OfferTypeId { get; set; }
        public int FuelTypeId { get; set; }
        public string CreatedByUserName { get; set; }
        public string CreatedByCompanyName { get; set; }
        public int CreatedByCompanyId { get; set; }
    }
}
