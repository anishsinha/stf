using Google.Cloud.Firestore;

namespace SiteFuel.Exchange.CJob.DeliveryAlerts.Firebase
{
    [FirestoreData]
    public class EditPreLoadBolFireBaseViewModel
    {
        [FirestoreProperty]
        public int Id { get; set; }
        [FirestoreProperty]
        public int UserId { get; set; }
        [FirestoreProperty]
        public int CompanyId { get; set; }
        [FirestoreProperty]
        public string BolNumber { get; set; }
        [FirestoreProperty]
        public string LiftTicketNumber { get; set; }
        [FirestoreProperty]
        public string BadgeNumber { get; set; }
        [FirestoreProperty]
        public string Carrier { get; set; }
        [FirestoreProperty]
        public double NetQuantity { get; set; }
        [FirestoreProperty]
        public double GrossQuantity { get; set; }
        [FirestoreProperty]
        public double LiftQuantity { get; set; }
    }
}
