using System;

namespace SiteFuel.Exchange.ViewModels.EIAPriceData
{
    public class EIAPriceViewModel
    {
        public int Id { get; set; }
        public int ProductTypeId { get; set; }
        public string FuelSurchargePricing { get; set; }
        public string SeriesId { get; set; }
        public string SeriesName { get; set; }
        public decimal Price { get; set; }
        public DateTime PriceUpdatedDate { get; set; }
        public DateTimeOffset PriceAddedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
