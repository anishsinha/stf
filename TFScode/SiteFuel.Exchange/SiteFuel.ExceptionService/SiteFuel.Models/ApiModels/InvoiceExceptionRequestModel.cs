using System;
using System.Collections.Generic;

namespace SiteFuel.Models.ApiModels
{
    public class InvoiceExceptionRequestModel : InvoiceExceptionModel
    {
        public int BuyerCompanyId { get; set; }
        public string BuyerCompanyName { get; set; }
        public int SupplierCompanyId { get; set; }
        public string SupplierCompanyName { get; set; }
        public string CarrierName { get; set; }
        public string JobName { get; set; }
        public decimal OrderedQuantity { get; set; }
        public decimal BolQuantity { get; set; }
        public string ScheduledLocation { get; set; }
        public string DroppedLocation { get; set; }
        public int ExceptionTypeId { get; set; }
        public decimal Ullage { get; set; }
        public string ParameterJson { get; set; }
        public string ExternalRefID { get; set; }
        public bool IsInventoryVerified { get; set; }
        public InvoiceExceptionModel OrigionalInvoice { get; set; }
        public List<BrokeredOrdersModel> BrokeredOrders { get; set; }
    }

    public class InvoiceExceptionModel
    {
        public int? InvoiceId { get; set; }
        public int OrderId { get; set; }
        public string PoNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal DroppedQuantity { get; set; }
        public DateTimeOffset DropDate { get; set; }
        public decimal PricePerGallon { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public int? DriverId { get; set; }
        public string DriverName { get; set; }
        public string UOM { get; set; }
    }

    public class BrokeredOrdersModel
    {
        public int BuyerCompanyId { get; set; }
        public int SupplierCompanyId { get; set; }
        public int SequenceFromEndSupplier { get; set; }
        public int OrderId { get; set; }
    }
}
