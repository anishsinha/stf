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
    public class MailboxMessageFilterViewModel
    {
        public MailboxMessageFilterViewModel()
        {
            StartCount = 1;
            CurrentPage = 1;
            EndCount = ApplicationConstants.MessagesDefaultPageSize;
            Messages = new List<MailboxMessageGridViewModel>();
        }

        public int UserId { get; set; }

        public AppMessageFilterType Type { get; set; }

        public int StartCount { get; set; }

        public int EndCount { get; set; }

        public int TotalCount { get; set; }

        public int CurrentPage { get; set; }

        public int LastPage { get; set; }

        public List<MailboxMessageGridViewModel> Messages { get; set; }

        public TimeSpan TimeZoneOffset { get; set; }
    }
}
