using System.Collections.Generic;

namespace SiteFuel.Exchange.SmsManager
{
    public class ApplicationEventSmsNotificationViewModel
    {
        public ApplicationEventSmsNotificationViewModel()
        {
            To = new List<string>();
        }

        public string From { get; set; }

        public List<string> To { get; set; }

        public string SmsText { get; set; }

        public string TwilioAccountSid { get; set; }

        public string TwilioAuthToken { get; set; }

        public string TwilioFromPhoneNumber { get; set; }

        public string SmsSendingCountryCode { get; set; }

        public string Url { get; set; }
    }
}
