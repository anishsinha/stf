using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class SapOrderCreationRequest
    {
        public SapOrderCreationRequest()
        {
            SalesOrderDetails = new SapOrderDetails();
        }
        public SapOrderDetails SalesOrderDetails { get; set; }
    }

    public class SapOrderDetails
    {
        public string ExternalOrderNo { get; set; }
        public string TerminalControl { get; set; }
        public string CustomerID { get; set; }
        public string LocationID { get; set; }
        public string LiftDate { get; set; }
        public string SAP_DocNumber { get; set; }
        public SapProductModel[] Products { get; set; }
    }

    public class SapProductModel
    {
        public string ProductID { get; set; }
        public string OrderQuantity { get; set; }
        public string Price { get; set; }
    }
}
