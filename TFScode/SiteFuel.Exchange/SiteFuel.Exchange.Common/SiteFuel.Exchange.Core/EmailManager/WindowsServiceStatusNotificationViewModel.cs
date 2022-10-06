using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.EmailManager
{
    public class WindowsServiceStatusNotificationViewModel
    {
        public WindowsServiceStatusNotificationViewModel(string serviceName, bool isServiceStarted = false)
        {
            To = new List<string>();
            Cc = new List<string>();
            Bcc = new List<string>();
            ReplyTo = new List<string>();
            ServiceName = serviceName;
            EventDateTime = DateTimeOffset.Now;
            ServiceStatus = isServiceStarted ? "Started" : "Stopped";
            Subject = $"TrueFill : {serviceName} is {ServiceStatus.ToLower()}";
        }

        public string From { get; set; }

        public List<string> To { get; set; }

        public List<string> Cc { get; set; }

        public List<string> Bcc { get; set; }

        public List<string> ReplyTo { get; set; }

        public string Subject { get; set; }

        public string ServiceName { get; set; }

        public DateTimeOffset EventDateTime { get; set; }

        public string ServiceStatus { get; set; }
    }
}
