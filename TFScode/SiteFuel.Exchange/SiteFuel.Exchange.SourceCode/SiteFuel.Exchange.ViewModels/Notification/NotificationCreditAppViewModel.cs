using System.Collections.Generic;
using System.Net.Mail;

namespace SiteFuel.Exchange.ViewModels
{
    public class NotificationCreditAppViewModel
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string CompanyLogo { get; set; }
        public List<Attachment> Attachments { get; set; }
    }
}
