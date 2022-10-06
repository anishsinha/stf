using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class TBDRequestDetailModel
    {
        public string DeliveryRequestId { get; set; }
        public int FuelTypeId { get; set; }
        public string FuelType { get; set; }
        public int ProductTypeId { get; set; }
        public int UoM { get; set; }
        public bool IsTBD { get; set; }
        public string GroupedParentDrId { get; set; }
    }
}
