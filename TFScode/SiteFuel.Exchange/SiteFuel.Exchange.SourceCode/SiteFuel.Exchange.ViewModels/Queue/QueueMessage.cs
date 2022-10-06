using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Queue
{
    public class QueueMessageViewModel
    {
        public int MessageId { get; set; }

        public string JsonMessage { get; set; }

        public QueueMessageStatus Status { get; set; }

        public int RetryCount { get; set; }

        public DateTimeOffset TimeRequested { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public QueueProcessType  QueueProcessType { get; set; }

        public List<QueueMessageResult> MessageResults { get; set; } = new List<QueueMessageResult>();
    }

    public class GroupDrInvoiceCreationViewModelForQueueService
    {
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public string GroupParentDrId { get; set; }
        public int InvoiceHeaderId { get; set; } //set only from Invoice manaully confirmed and without Tax processing
        public bool IsProcessWithoutTax { get; set; }
    }
}
