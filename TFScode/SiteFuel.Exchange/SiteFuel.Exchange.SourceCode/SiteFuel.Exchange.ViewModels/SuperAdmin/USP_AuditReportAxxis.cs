using SiteFuel.Exchange.Core.StringResources;
using System;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class USP_AuditReportAxxis
    {
        public DateTimeOffset DeliveryDate { get; set; }

        public string TerminalId { get; set; }

        public string TerminalName { get; set; }

        public string ProductId { get; set; }

        public string ProductName { get; set; }
    }
}
