using FileHelpers;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
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
        public List<DipChartDetailsViewModel> DipChartDetails { get; set; }
    }

    [DelimitedRecord(",")]
    [IgnoreEmptyLines]
    public class DipChartDetailsViewModel
    {
        [FieldOptional]
        public decimal? Dip { get; set; }
        [FieldOptional]
        public decimal? Volume { get; set; }
        [FieldOptional]
        public decimal? Ullage { get; set; }
    }
}
