using Google.Cloud.Firestore;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.CJob.DeliveryAlerts.Firebase
{
    [FirestoreData]
    public class MobileFuelRetainViewModel
    {
        [FirestoreProperty]
        public int DriverId { get; set; }
        [FirestoreProperty]
        public List<MobileTrailerFuelRetainViewModel> TrailerFuelRetain { get; set; } = new List<MobileTrailerFuelRetainViewModel>();
        [FirestoreProperty]
        public List<MobileInvoiceBolViewModel> BolDetails { get; set; } = new List<MobileInvoiceBolViewModel>();
        [FirestoreProperty]
        public List<MobileInvoiceLiftTicketViewModel> TicketDetails { get; set; } = new List<MobileInvoiceLiftTicketViewModel>();
        [FirestoreProperty]
        public string TraceId { get; set; }
    }

    [FirestoreData]
    public class MobileTrailerFuelRetainViewModel
    {
        [FirestoreProperty]
        public string TrailerId { get; set; }
        [FirestoreProperty]
        public string CompartmentId { get; set; }
        [FirestoreProperty]
        public double Quantity { get; set; }
        [FirestoreProperty]
        public int? TrackableScheduleId { get; set; }
        [FirestoreProperty]
        public string ProductType { get; set; }
        [FirestoreProperty]
        public int ProductId { get; set; }
        [FirestoreProperty]
        public int UoM { get; set; }
    }
}
