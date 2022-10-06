using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.TankRental
{
    public class TankRentalQueueMessage
    {
        public int OrderId { get; set; }
        public int RentalFrequencyId { get; set; }
        public int TankDetailId { get; set; }
        public string TimeZoneName { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string CompanyName { get; set; }
        public bool IsClosedOrder { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
    }

    public class TankRentalInterval
    {
        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset EndDate { get; set; }
    }
}
