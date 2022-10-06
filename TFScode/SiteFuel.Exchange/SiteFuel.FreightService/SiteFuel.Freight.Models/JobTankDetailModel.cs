using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels
{
    public class JobTankDetailModel
    {
        public int AssetId { get; set; }
        public string SiteId { get; set; }
        public string TankId { get; set; }
        public string StorageId { get; set; }
        public string TankName { get; set; }
        public string TankNumber { get; set; }
        public int ProductTypeId { get; set; }
        public int JobId { get; set; }
    }
    public class JobTankAdditionalDetailModel
    {
        public int AssetId { get; set; }
        public string SiteId { get; set; }
        public string TankId { get; set; }
        public string StorageId { get; set; }
        public string TankName { get; set; }
        public string TankNumber { get; set; }
        public decimal MaxFill { get; set; }
        public int FillType { get; set; }
        public decimal MinFill { get; set; }
        public decimal RunOut { get; set; }
        public int ProductTypeId { get; set; }
        public string TfxProductTypeName { get; set; }
        public string JobName { get; set; }
        public int JobId { get; set; }
        public bool ISRunOut { get; set; } = false;
        public decimal FuelCapacity { get; set; }
        public string DaysRemaining { get; set; }
    }
}
