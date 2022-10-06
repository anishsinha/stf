using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class UspCarrierMapping
    {
        public int CompanyId { get; set; }
        public int CarrierCompanyId { get; set; }
        public string CarrierName { get; set; }
        public int TotalOrders { get; set; }
        public string AssignedCarrierId { get; set; }
        public int Id { get; set; }
        public string TerminalIds { get; set; }
        public string BulkPlantIds { get; set; }
        public int CountryId { get; set; }
        public string TerminalBulkPlant { get; set; }
        public bool IsActive { get; set; }
        public DropdownDisplayItem AssignedTerminalId { get; set; }
        public string AssignedTerminalIdName { get; set; }

        public int TerminalCompanyAliasId { get; set; }


    }
}
