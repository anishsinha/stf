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
    public class MailboxMessageCountViewModel
    {
        public int UnreadInboxCount { get; set; }

        public int TotalInboxCount { get; set; }

        public int TotalDraftsCount { get; set; }
        public int ImportantsCount { get; set; }
        public int TrashCount { get; set; }

    }
}
