using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels
{
    public class ExternalVehicleMappingViewModel
    {
        public string Id { get; set; }
        public string TruckId { get; set; }
        public string TruckName { get; set; }
        public string TargetVehicleValue { get; set; }
        public int ThirdPartyId { get; set; }
        public int UserId { get; set; }
    }
    public class ExternalVehicleMappingInputModel
    {
        public int UserId { get; set; }
        public List<ExternalVehicleMappingViewModel> ListExternalVehicles { get; set; }
    }

}
