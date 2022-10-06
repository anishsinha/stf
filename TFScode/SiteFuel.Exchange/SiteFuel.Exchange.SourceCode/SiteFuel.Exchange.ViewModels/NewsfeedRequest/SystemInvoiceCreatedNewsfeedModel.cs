using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class SystemInvoiceCreatedNewsfeedModel
    {
        public int JobId { get; set; }
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public int InvoiceTypeId { get; set; }
        public int OrderId { get; set; }
        public string PoNumber { get; set; }
        public int JobCompanyId { get; set; }
        public int BuyerCompanyId { get; set; }
        public int SupplierCompanyId { get; set; }
        public int DriverId { get; set; }
        public DateTimeOffset DropStartDate { get; set; }
        public DateTimeOffset DropEndDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public decimal DroppedGallons { get; set; }
        public decimal OrderMaxQuantity { get; set; }
        public int ApprovalUserId { get; set; }
        public int DeliveryTypeId { get; set; }
        public object UoM { get; set; }
        public int InvoiceHeaderId { get; set; }
    }
}
