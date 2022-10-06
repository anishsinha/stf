using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;
using System.Net.Mail;

namespace SiteFuel.Exchange.ViewModels
{
    public class NotificationViewModel
    {

        public NotificationViewModel()
        {
            Attachments = new List<Attachment>();
            ShowHelpLineInfo = true;
            To = new List<string>();
            CC = new List<string>();
            BCC = new List<string>();
            SendNotificationType = (int)NotificationType.Email;
            ApplicationTemplateId = (int)ApplicationTemplate.TrueFill;
        }

        public string Subject { get; set; }

        public string CompanyLogo { get; set; }

        public string CompanyText { get; set; }

        public string BodyLogo { get; set; }

        public string BodyText { get; set; }

        public string BodyButtonText { get; set; }

        public string BodyButtonUrl { get; set; }

        public string SmsText { get; set; }

        public string ServerUrl { get; set; }

        public int? EventTypeId { get; set; }

        public List<Attachment> Attachments { get; set; }

        public bool ShowHelpLineInfo { get; set; }

        public int? NotificationId { get; set; }

        public int SendNotificationType { get; set; }

        public List<string> To { get; set; }

        public List<string> CC { get; set; }

        public List<string> BCC { get; set; }

        public int ApplicationTemplateId { get; set; }
        
    }
}
