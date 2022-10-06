using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class SAPDeliveryDetailsViewModel
    {
        public string TFXOrderNo { get; set; }

        public int UoM { get; set; }
        public string LocationID { get; set; }
        public string LiftTicketNo { get; set; }

        public string DropTicketNo { get; set; }

        public string LiftDate { get; set; }

        public string LiftStartTime { get; set; }

        public string LiftEndTime { get; set; }

        public decimal TotalDropQuantity { get; set; }

        public string ProductID { get; set; }

        public decimal Price { get; set; }

        public string CustomerID { get; set; }

        public string TerminalControl { get; set; }
        public string SAPOrdNumber { get; set; }
        public string TruckId { get; set; }
    }

    public class SAPDrXml
    {
        public List<SAPDeliveryDetailsXMLViewModel> DeliveryProcessingDetails = new List<SAPDeliveryDetailsXMLViewModel>();
    }

    public class SAPDeliveryDetailsXMLViewModel
    {
        public string ExternalOrderNo { get; set; }

        public string LocationID { get; set; }
        public string LiftTicketNo { get; set; }

        public string DropTicketNo { get; set; }

        public string LiftDate { get; set; }

        public string LiftStartTime { get; set; }

        public string LiftEndTime { get; set; }

        public string CustomerID { get; set; }

        public string TerminalControl { get; set; }

        public string SAP_DocNumber { get; set; }
        public string TruckID { get; set; }

        public List<SAPXmlProductModel> Products { get; set; } = new List<SAPXmlProductModel>();

    }

    public class SAPXmlProductModel
    {
        public string ProductID { get; set; }
        public decimal TotalDropQtyGross { get; set; }
        public decimal TotalDropQtyNet { get; set; }
        //public string UoM { get; set; }
        public decimal Price { get; set; }
    }
}
