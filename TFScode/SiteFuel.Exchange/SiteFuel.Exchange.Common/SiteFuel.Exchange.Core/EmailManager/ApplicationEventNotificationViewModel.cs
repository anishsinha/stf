using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.EmailManager
{
    public class ApplicationEventNotificationViewModel
    {
        public ApplicationEventNotificationViewModel()
        {
            To = new List<string>();
            Cc = new List<string>();
            Bcc = new List<string>();
            ReplyTo = new List<string>();
            ShowUserSettingsLink = true;
            ShowFooterContent = true;
            ShowHelpLineInfo = true;
            ApplicationTemplateId = (int)ApplicationTemplate.TrueFill;
        }

        public string From { get; set; }

        public List<string> To { get; set; }

        public List<string> Cc { get; set; }

        public List<string> Bcc { get; set; }

        public List<string> ReplyTo { get; set; }

        public string Subject { get; set; }

        public string CompanyLogo { get; set; }

        public string CompanyText { get; set; }

        public string BodyLogo { get; set; }

        public string BodyText { get; set; }

        public string BodyButtonText { get; set; }

        public string BodyButtonUrl { get; set; }

        public string ServerUrl { get; set; }

        public List<Attachment> Attachments { get; set; }

        public bool ShowUserSettingsLink { get; set; }

        public bool ShowFooterContent { get; set; }

        public bool ShowHelpLineInfo { get; set; }

        public string BodyMaxWidth { get; set; } = "600px";

        public string SenderName { get; set; }

        public int ApplicationTemplateId { get; set; }
    }
}
