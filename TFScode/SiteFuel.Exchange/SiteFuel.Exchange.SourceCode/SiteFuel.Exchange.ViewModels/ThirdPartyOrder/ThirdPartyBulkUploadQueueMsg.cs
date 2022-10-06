using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.ThirdPartyOrder
{
    public class ThirdPartyBulkUploadQueueMsg
    {
        public int SupplierId { get; set; }
        public int SupplierCompanyId { get; set; }
        public int FileLineNumberToStart { get; set; }
        public string FileUploadPath { get; set; }

    }
}
