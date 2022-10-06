using FileHelpers;
using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class USP_GetSupplierInvoices
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public string Supplier { get; set; }

        public string FuelType { get; set; }

        public string InvoiceNumber { get; set; }

        public string PoNumber { get; set; }

        public decimal InvoiceAmount { get; set; }

        public DateTimeOffset DropStartDate { get; set; }

        public DateTimeOffset DropDate { get; set; }

        public DateTimeOffset InvoiceDate { get; set; }

        public DateTimeOffset PaymentDueDate { get; set; }

        public string Status { get; set; }

        public int InvoiceNumberId { get; set; }

        public string TerminalName { get; set; }

        public string DriverFName { get; set; }

        public string DriverLName { get; set; }

        public int InvoiceTypeId { get; set; }

        public string City { get; set; }

        public string StateCode { get; set; }

        public string Zip { get; set; }

        public decimal PricePerGallon { get; set; }

        public int PricingTypeId { get; set; }

        public int? RackAvgTypeId { get; set; }
    }

    public class Usp_CompanySpecificDeliveryDetails
    {
        public string ParentDropTicket { get; set; }
        public string SubDropTicket { get; set; }
        public string PONumber { get; set; }
        public string BolNumber { get; set; }
        public string LiftTicketNumber { get; set; }
        public string LiftDate { get; set; }
        public string Carrier { get; set; }
        public string Badge { get; set; }
        public string Customer { get; set; }
        public string CustomerLocation { get; set; }
        public string LocationAddress { get; set; }
        public string LocationCity { get; set; }
        public string LocationState { get; set; }
        public string Zip { get; set; }
        public string FuelType { get; set; }
        public string Tank { get; set; }
        public string GallonsDelivered { get; set; }
        public string Terminal { get; set; }
        public string DropDate { get; set; }
        public string DropTime { get; set; }
        public decimal Pricing { get; set; }
        public string DropTicketDate { get; set; }
        public string DriverName { get; set; }
        public string BulkplantName { get; set; }
        public string PickupLocation { get; set; }
        public string PDIOrder { get; set; }
        public string PDIException { get; set; }
        public string NetQuantity { get; set; }
        public string GrossQuantity { get; set; }
        public string WaitingForStatus { get; set; }
        public string Vessel { get; set; }
        public string Berth { get; set; }
        public string OrderType { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal InvoiceAmount { get; set; }
        public string API { get; set; }
        public string MTQty { get; set; }
        public string MarineFlag { get; set; }
        public string OrderCurrentStatus { get; set; }
        public string OrderLastUpdatedBy { get; set; }
        public string OrderLastUpdatedDate { get; set; }
        public string DDTInvUpdatedDate { get; set; }
        public string DDTModifiedby { get; set; }
        public string SupplierCompanyName { get; set; }
        public string UniqueOrderNo { get; set; }
    }

    [DelimitedRecord(",")]
    public class CompanySpecificDeliveryDetailsCsvViewModel
    {
        [FieldOrder(0), FieldQuoted]
        public string SupplierCompanyName { get; set; }
        [FieldOrder(1), FieldQuoted]
        public string ParentDropTicket { get; set; }
        [FieldOrder(2), FieldQuoted]
        public string SubDropTicket { get; set; }
        [FieldOrder(3), FieldQuoted]
        public string PONumber { get; set; }
        [FieldOrder(4), FieldQuoted]
        public string BolNumber { get; set; }
        [FieldOrder(5), FieldQuoted]
        public string LiftTicketNumber { get; set; }
        [FieldOrder(6), FieldQuoted]
        public string LiftDate { get; set; }
        [FieldOrder(7), FieldQuoted]
        public string Carrier { get; set; }
        [FieldOrder(8), FieldQuoted]
        public string Badge { get; set; }
        [FieldOrder(9), FieldQuoted]
        public string Customer { get; set; }
        [FieldOrder(10), FieldQuoted]
        public string CustomerLocation { get; set; }
        [FieldOrder(11), FieldQuoted]
        public string LocationAddress { get; set; }
        [FieldOrder(12), FieldQuoted]
        public string LocationCity { get; set; }
        [FieldOrder(13), FieldQuoted]
        public string LocationState { get; set; }
        [FieldOrder(14), FieldQuoted]
        public string Zip { get; set; }
        [FieldOrder(15), FieldQuoted]
        public string FuelType { get; set; }
        [FieldOrder(16), FieldQuoted]
        public string Tank { get; set; }
        [FieldOrder(17), FieldQuoted]
        public string GallonsDelivered { get; set; }
        [FieldOrder(18), FieldQuoted]
        public string Terminal { get; set; }
        [FieldOrder(19), FieldQuoted]
        public string DropDate { get; set; }
        [FieldOrder(20), FieldQuoted]
        public string DropTime { get; set; }
        [FieldOrder(21), FieldQuoted]
        public string Pricing { get; set; }
        [FieldOrder(22), FieldQuoted]
        public string DropTicketDate { get; set; }
        [FieldOrder(23), FieldQuoted]
        public string DriverName { get; set; }
        [FieldOrder(24), FieldQuoted]
        public string BulkplantName { get; set; }
        [FieldOrder(25), FieldQuoted]
        public string PickupLocation { get; set; }
        [FieldOrder(26), FieldQuoted]
        public string PDIOrder { get; set; }
        [FieldOrder(27), FieldQuoted]
        public string PDIException { get; set; }
        [FieldOrder(28), FieldQuoted]
        public string NetQuantity { get; set; }
        [FieldOrder(29), FieldQuoted]
        public string GrossQuantity { get; set; }
        [FieldOrder(30), FieldQuoted]
        public string WaitingForStatus { get; set; }
        [FieldOrder(31), FieldQuoted]
        public string Vessel { get; set; }
        [FieldOrder(32), FieldQuoted]
        public string Berth { get; set; }
        [FieldOrder(33), FieldQuoted]
        public string OrderType { get; set; }
        [FieldOrder(34), FieldQuoted]
        public string InvoiceNumber { get; set; }
        [FieldOrder(35), FieldQuoted]
        public string InvoiceAmount { get; set; }
        [FieldOrder(36), FieldQuoted]
        public string API { get; set; }
        [FieldOrder(37), FieldQuoted]
        public string MTQty { get; set; }
        [FieldOrder(38), FieldQuoted]
        public string MarineFlag { get; set; }
        [FieldOrder(39), FieldQuoted]
        public string OrderCurrentStatus { get; set; }
        [FieldOrder(40), FieldQuoted]
        public string OrderLastUpdatedBy { get; set; }
        [FieldOrder(41), FieldQuoted]
        public string OrderLastUpdatedDate { get; set; }
        [FieldOrder(42), FieldQuoted]
        public string DDTInvUpdatedDate { get; set; }
        [FieldOrder(43), FieldQuoted]
        public string DDTModifiedby { get; set; }
        [FieldOrder(44), FieldQuoted]
        public string DRID { get; set; }
    }
}
