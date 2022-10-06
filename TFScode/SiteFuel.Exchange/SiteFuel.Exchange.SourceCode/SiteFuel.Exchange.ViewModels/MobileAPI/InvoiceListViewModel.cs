using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class InvoiceListViewModel
    {
        public InvoiceListViewModel()
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            Filter = InvoiceFilterType.All;
        }

        public InvoiceFilterType Filter { get; set; }

        public int UserId { get; set; }

        public int CompanyId { get; set; }

        public int Start { get; set; }

        public int Length { get; set; }

        public Currency Currency { get; set; }

        public int CountryId { get; set; }

        public string GroupIds { get; set; }

        public int OrderId { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

    }
}
