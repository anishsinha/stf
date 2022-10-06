using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class ExternalMeterDataUploadQueueMsg
    {
        public int SupplierCompanyId { get; set; }

        public int BuyerCompanyId { get; set; }

        public string FileUploadPath { get; set; }
    }
}
