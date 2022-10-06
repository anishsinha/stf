using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class MobileThemeViewModel : StatusViewModel
    {
        public int CompanyId { get; set; }

        public string CompanyLogo { get; set; }

        public string CompanyName { get; set; }

        public string SupplierCode { get; set; }

        public string HeaderBackgroundColor { get; set; }

        public string HeaderForeColor { get; set; }

        public string FooterBackgroundColor { get; set; }

        public string FooterForeColor { get; set; }

        public string PrimaryButtonBackgroundColor { get; set; }

        public string PrimaryButtonForeColor { get; set; }

        public string SecondaryButtonBackgroundColor { get; set; }

        public string SecondaryButtonForeColor { get; set; }
    }
}
