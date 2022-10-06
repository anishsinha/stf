using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class FirebaseConfiguration
    {
        public string ProjectId { get; set; }
        public string ServiceAccountJson { get; set; }
        public string CollectionName { get; set; }
        public string PreLoadBolCollectionName { get; set; }
        public DateTimeOffset LastUpdatedDateTime { get; set; }
        public DateTimeOffset PreLoadBolLastUpdatedDateTime { get; set; }
        //CONFIG FOR EDIT PRE LOAD BOL DETAILS
        public string EditedPreLoadBolCollectionName { get; set; }
        public DateTimeOffset EditedPreLoadBolLastUpdatedDateTime { get; set; }
        //CONFIG FOR DELETE PRE LOAD BOL DETAILS
        public string DeletedPreLoadBolCollectionName { get; set; }
        public DateTimeOffset DeletedPreLoadBolLastUpdatedDateTime { get; set; }
        public string FuelRetainCollectionName { get; set; }
        public DateTimeOffset FuelRetainLastUpdatedDateTime { get; set; }

        public string PickupBOLRetainCollectionName { get; set; }
        public DateTimeOffset PickupBOLRetainLastUpdatedDateTime { get; set; }

        public string CancelledScheduleCollectionName { get; set; }
        public DateTimeOffset CancelledScheduleLastUpdatedDateTime { get; set; }
    }
}
