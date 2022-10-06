using System;

namespace SiteFuel.Exchange.ViewModels.FilldService
{
    public class FilldTerritoryViewModel
    {
        public string external_territory_id { get; set; }
        public string short_name { get; set; }
        public string name { get; set; }
        public string country { get; set; }
        public string currency { get; set; }
        public string delivery_unit { get; set; }
        public string locale { get; set; }
        public string timezone { get; set; }
    }

    public class FilldStatusViewModel
    {
        public bool Status { get; set; }
        public string Status_message { get; set; }
        public int Status_code { get; set; }
    }
    public class TerritoryDataViewModel
    {
        public int Id { get; set; }
        public int Dealer_id { get; set; }
        public string Uuid { get; set; }
        public string Status { get; set; }
        public DateTimeOffset Created_at { get; set; }
        public DateTimeOffset Updated_at { get; set; }
    }

    public class FilldTerritoryResponseViewModel : FilldStatusViewModel
    {
        public TerritoryDataViewModel Data { get; set; }
    }
}
