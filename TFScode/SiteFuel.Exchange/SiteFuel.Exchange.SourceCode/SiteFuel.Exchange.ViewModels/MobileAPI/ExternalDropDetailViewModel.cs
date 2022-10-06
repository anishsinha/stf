using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class ExternalDropDetailViewModel : StatusViewModel
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int? TrackableScheduleId { get; set; }

        public decimal DropStartLatitude { get; set; }

        public decimal DropStartLongitude { get; set; }

        public decimal DropEndLatitude { get; set; }

        public decimal DropEndLongitude { get; set; }

        public long DropStartDate { get; set; }

        public long DropEndDate { get; set; }

        public int UserId { get; set; }
    }
}
