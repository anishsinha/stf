using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;
using System.Net.Mail;

namespace SiteFuel.Exchange.ViewModels
{
    public class ApplicationTemplateViewModel
    {
        public int ApplicationTemplateId { get; set; }

        public string BrandedCompanyName { get; set; }

        public string URLName { get; set; }

        public string SenderName { get; set; }

        public string FromEmail { get; set; }

        public string CompanyLogo { get; set; }

        public string Template { get; set; }
    }
}
