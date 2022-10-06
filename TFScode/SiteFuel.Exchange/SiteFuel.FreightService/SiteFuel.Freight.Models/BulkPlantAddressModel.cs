namespace SiteFuel.FreightModels
{
    public class BulkPlantAddressModel
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public DropdownDisplayItem State { get; set; } = new DropdownDisplayItem();
        public DropdownDisplayItem Country { get; set; } = new DropdownDisplayItem();
        public string ZipCode { get; set; }
        public string CountyName { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string TimeZoneName { get; set; }
        public string SiteName { get; set; }
        public int SiteId { get; set; }
    }
}
