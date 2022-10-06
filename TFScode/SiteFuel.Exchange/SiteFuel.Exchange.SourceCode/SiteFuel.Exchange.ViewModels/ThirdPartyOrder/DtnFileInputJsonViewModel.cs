using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class DtnFileProcessingRequestViewModel
    {
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public string RefId { get; set; }
        public string Password { get; set; }
        public string SiteNumber { get; set; }
        public string FtpPassword { get; set; }
        public string FtpUserName { get; set; }
        public string FtpUrl { get; set; }
        public string PathToUpload { get; set; }
        public List<string> ReceiversEmail { get; set; }
    }
}
