using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class NotificationTankHitRunoutLevelModel 
    {
        public NotificationTankHitRunoutLevelModel()
        {
        }

        public int SupplierCompanyId { get; set; }

        public string Location { get; set; }

        public string Tank { get; set; }

        public string TankStorageId { get; set; }

        public string CurrentInventory { get; set; }

        public string Ullage { get; set; }

        public int DaysRemaining { get; set; }
    }
}
