namespace SiteFuel.Exchange.ViewModels
{
    public class SuperAdminOrderGridViewModel : StatusViewModel
    {
        public int Id { get; set; }

        public string PoNumber { get; set; }

        public string Job { get; set; }

        public string Customer { get; set; }

        public string Supplier { get; set; }

        public string FuelType { get; set; }

        public string StartDate { get; set; }

        public decimal Quantity { get; set; }

        public string PricePerGallon { get; set; }

        public string Status { get; set; }

        public int PricingTypeId { get; set; }

        public string RackOrPpg { get; set; }

        public string GallonsOrdered { get; set; }
    }
}
