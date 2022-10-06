using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class AuditLogViewModel
    {
        public AuditLogViewModel()
        {
            CreatedDate = DateTimeOffset.Now;
        }

        public int Id { get; set; }

        public string AuditEventType { get; set; }

        public string AuditEntityType { get; set; }

        public int AuditEntityId { get; set; }

        public string Message { get; set; }

        public string MachineName { get; set; }

        public string RemoteAddress { get; set; }

        public string Url { get; set; }

        public string CallSite { get; set; }

        public string JsonMessage { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public int CreatedBy { get; set; }
    }
}
