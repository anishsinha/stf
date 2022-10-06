using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels
{
    public class TankModalTypeViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Modal { get; set; }
        public int ScaleMeasurement { get; set; }
        public string ScaleMeasurementText { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public int BuyerCompanyId { get; set; }
        public int SupplierCompanyId { get; set; }
        public int CreatedByCompanyId { get; set; }
        public bool IsActive { get; set; }
        public string PdfFilePath { get; set; }
        public List<TankTypeDipChartDetailsViewModel> DipChartDetails { get; set; }
    }
    public class TankTypeDipChartDetailsViewModel
    {
        public decimal Dip { get; set; }
        public decimal Volume { get; set; }
        public decimal Ullage { get; set; }
    }
}
