using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class CounterOfferDetailsViewModel : BaseViewModel
    {
        public CounterOfferDetailsViewModel()
        {   
        }

        public CounterOfferDetailsViewModel(Status status)
            : base(status)
        {  
        }

        public int Id { get; set; }

        public int FuelRequestId { get; set; }

        public int BuyerId { get; set; }

        public int? BuyerStatus { get; set; }

        public int SupplierId { get; set; }

        public int? SupplierStatus { get; set; }

        public string CreatedBy { get; set; }
    }
}