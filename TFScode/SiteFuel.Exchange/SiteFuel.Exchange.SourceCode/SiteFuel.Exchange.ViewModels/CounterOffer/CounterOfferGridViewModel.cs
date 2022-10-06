using SiteFuel.Exchange.Utilities;
using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class CounterOfferGridViewModel : BaseViewModel
    {
        public CounterOfferGridViewModel()
        {
        }

        public CounterOfferGridViewModel(Status status)
            : base(status)
        {
        }

        public string CounterOfferRequestNumber { get; set; }

        public int FuelRequestId { get; set; }

        public string RequestNumber { get; set; }

        public string Job { get; set; }

        public string Price { get; set; }

        public string TotalGallons { get; set; }

        public string ViewDetails { get; set; }

        public int SupplierId { get; set; }

        public string Supplier { get; set; }

        public string Buyer { get; set; }

        public string CreatedDate { get; set; }

        public string Status { get; set; }

        public int? BuyerStatus { get; set; }

        public int? SupplierStatus { get; set; }

        public int OriginalFuelRequestId { get; set; }
    }
}
