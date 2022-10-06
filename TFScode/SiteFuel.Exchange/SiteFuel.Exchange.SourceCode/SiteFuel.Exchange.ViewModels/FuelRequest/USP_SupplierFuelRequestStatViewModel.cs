using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class USP_SupplierFuelRequestStatViewModel
    {
        public int FuelRequestId { get; set; }
        public string FuelType { get; set; }
        public int FuelTypeId { get; set; }

        public int AcceptedFrCount { get; set; }
        public int MissedFrCount { get; set; }
        public int ExpiredFrCount { get; set; }
        public int DeclinedFrCount { get; set; }
        public int CounterOfferCount { get; set; }

        public decimal TotalRequestedGallons { get; set; }
        public decimal TotalAcceptedGallons { get; set; }
        public decimal TotalDeliveredGallons { get; set; }
        public decimal TotalMissedGallons { get; set; }
        public decimal TotalExpiredGallons { get; set; }

        public decimal BusinessYouWon { get; set; }
        public decimal BusinessYouMissed { get; set; }
        public decimal BusinessInYourArea { get; set; }

        public int TotalFrCount { get; set; }

        public int TotalQrCount { get; set; }
        public int OpenQrCount { get; set; }
        public int MissedQrCount { get; set; }
        public int AcceptedQrCount { get; set; }
        public int DeclinedQrCount { get; set; }

    }
}
