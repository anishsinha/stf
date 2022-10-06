using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class OrderStatusRequestModel
    {
        public SalesOrderStatusModel[] SalesOrderStatus { get; set; }
    }

    public class SalesOrderStatusModel
    {
        public string SAP_Order_No { get; set; }
        public string ExternalOrderNo { get; set; }
        public string SAP_Order_Status { get; set; }
        public SapProductModel[] Products { get; set; }
    }

    public class SalesOrderStatusRequestModel
    {
        public int Id { get; set; }
        public string JsonRequest { get; set; }
        public bool IsProcessed { get; set; }
    }

    public class OverrideCreditCheckApprovalModel
    {
        public int UserId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string HeldDRId { get; set; }
    }
}
