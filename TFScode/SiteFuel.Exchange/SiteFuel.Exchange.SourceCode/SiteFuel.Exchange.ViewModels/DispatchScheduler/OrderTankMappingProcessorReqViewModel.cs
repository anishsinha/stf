using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class OrderTankMappingProcessorReqViewModel
    {
        public int OrderId { get; set; }
        public int CreatedBy { get; set; }
        public bool IsAssetTrackingEnabled { get; set; }
        public int JobId { get; set; }
        public int SupplierCompanyId { get; set; }
        public string DisplayJobId { get; set; }
        public int ProductTypeId { get; set; }

    }

    public class OrderTankMappingViewModel
    {
        public int JobId { get; set; }
        public string TankId { get; set; }
        public int OrderId { get; set; }
        public int FuelTypeId { get; set; }
        public int ProductTypeId { get; set; }
        public int SupplierCompanyId { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
    }

}
