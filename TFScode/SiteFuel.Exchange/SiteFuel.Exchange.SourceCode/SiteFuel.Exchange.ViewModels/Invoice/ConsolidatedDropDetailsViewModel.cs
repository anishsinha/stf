using Foolproof;
using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class ConsolidatedDropDetailsViewModel
    {
        public string PONumber { get; set; }

        public string DropTicketNumber { get; set; }

        public string DropArrivalDate { get; set; }

        public string DropArrivalTime { get; set; }

        public DateTime DropArrivalDateTime { get; set; }

        public int FuelTypeId { get; set; }

        public bool IsBolImageRequired { get; set; }
    }
}
