using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class DtnSupplierDetails
    {
        public List<DtnSuppliers> DtnSuppliers { get; set; }
        public string FtpUrl { get; set; }
        public List<string> NotifiedUsers { get; set; }
        public List<SiteNumber> SiteNumbers { get; set; }
    }

    public class DtnSuppliers
    {
        public int CompanyId { get; set; }
        public string RefId { get; set; }
        public string Password { get; set; }
        public string FtpUserName { get; set; }
        public string FtpPassword { get; set; }
        public string FtpPathToUpload { get; set; }
    }

    public class SiteNumber
    {
        public int BuyerCompanyId { get; set; }
        public int? SupplierCompanyId { get; set; }
        public string BuyerSiteNumber { get; set; }
    }
}
