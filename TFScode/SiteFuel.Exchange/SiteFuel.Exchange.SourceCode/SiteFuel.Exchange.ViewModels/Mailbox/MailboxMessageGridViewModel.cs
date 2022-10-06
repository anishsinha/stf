using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class MailboxMessageGridViewModel
    {
        public int Id { get; set; }

        public string SenderName { get; set; }

        public string Subject { get; set; }
        
        public AppMessageStatus MessageStatusId { get; set; }
        
        public bool IsMarkedAsRead { get; set; }
        
        public bool IsMarkedAsImportant { get; set; }

        public DateTimeOffset TimeStamp { get; set; }
        public string MessageDeliveredTime { get; set; }
    }
}
