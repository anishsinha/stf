namespace SiteFuel.Exchange.ViewModels
{
    public class PickupLocationDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string ControlNumber { get; set; }
        public string Address { get; set; }
        public string StateCode { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string ZipCode { get; set; }
        public int StateId { get; set; }
        public int CountryId { get; set; }
        public string CountryCode { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public string TerminalOwner { get; set; }
        public int UpdatedBy { get; set; }
    }
}
