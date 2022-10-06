using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class TankRentalFrequencyViewModel : StatusViewModel
    {
        public int TankRentalFrequencyId { get; set; }

        public int FuelRequestId { get; set; }

        public string Name { get; set; }

        public FrequencyTypes FrequencyTypes { get; set; }

        public decimal RentalFee { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset? EndDate { get; set; }

        public DateTimeOffset LastRunDate { get; set; }

        public int ActivationStatusId { get; set; } = (int)ActivationStatus.Created;

        public DateTimeOffset? DeactivationDate { get; set; }

        public Currency Currency { get; set; }

        public UoM UoM { get; set; }

        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }

        public List<TankDetailsViewModel> Tanks { get; set; } = new List<TankDetailsViewModel>();

        public TankRentalFrequencyViewModel Clone()
        {
            var thisObject = (TankRentalFrequencyViewModel)this.MemberwiseClone();
            return thisObject;
        }
    }
}