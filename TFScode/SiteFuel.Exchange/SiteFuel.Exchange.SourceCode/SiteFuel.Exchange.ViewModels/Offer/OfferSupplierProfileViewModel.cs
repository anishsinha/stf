using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class OfferSupplierProfileViewModel
    {
        public OfferSupplierProfileViewModel()
        {
            BaseBallCardDetails = new BaseballCardDetailsViewModel();
        }

        public string PhoneNumber { get; set; }

        public string MemberSince { get; set; }

        public string ActiveHours { get; set; }

        public int TotalOrders { get; set; }

        public decimal GallonsDelivered { get; set; }
        
        public string Address { get; set; }

        public BaseballCardDetailsViewModel BaseBallCardDetails { get; set; }
    }
}
