using SiteFuel.Exchange.Utilities;
using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class PickUpAddressViewModel
    {
        public int OrderId { get; set; }
        public DispatchAddressViewModel Address { get; set; }
        public Currency Currency { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.Now;
    }
}
