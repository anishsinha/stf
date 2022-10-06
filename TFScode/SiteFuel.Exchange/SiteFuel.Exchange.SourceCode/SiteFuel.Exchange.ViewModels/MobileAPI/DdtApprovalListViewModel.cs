namespace SiteFuel.Exchange.ViewModels
{
    public class DdtApprovalListViewModel
    {
        public DdtApprovalListViewModel()
        {

        }

        public string Id { get; set; }

        public string InvoiceNumber { get; set; }

        public string AssignedTo { get; set; }

        public string ApprovedBy { get; set; }

        public string ApprovedDate { get; set; }

        public string ApprovedTime { get; set; }

        public string DeliveryDate { get; set; }

        public string DeliveryTime { get; set; }

        public string CreatedDate { get; set; }

        public string CreatedTime { get; set; }

        public string RejectedDate { get; set; }

        public string RejectedTime { get; set; }

        public string Status { get; set; }

        public bool IsApprovalUser { get; set; }

        public string JobName { get; set; }

        public string PoNumber { get; set; }

        public string Quantity { get; set; }

        public int StatusId { get; set; }

        public int UnitOfMeasurement { get; set; }

        public int Currency { get; set; }
        public string FuelType { get; set; } //product displayname from MstProducts
        public string PaymentDueDate { get; set; }
        public decimal InvoiceAmount { get; set; }
        public int AssetCount { get; set; }
        public string DropDate { get; set; }
        public string DropTime { get; set; }
        public string DroppedGallons { get; set; }
        public int HeaderId { get; set; }

    }

    public class UspDdtApprovalListViewModel
    {
        public UspDdtApprovalListViewModel()
        {

        }

        public string Id { get; set; }

        public string InvoiceNumber { get; set; }

        public string AssignedTo { get; set; }

        public string DeliveryTime { get; set; }

        public string Status { get; set; }

        public bool IsApprovalUser { get; set; }

        public string JobName { get; set; }

        public string PoNumber { get; set; }

        public int StatusId { get; set; }

        public int UnitOfMeasurement { get; set; }

        public int Currency { get; set; }
        public string FuelType { get; set; } //product displayname from MstProducts
        public string PaymentDueDate { get; set; }
        public decimal InvoiceAmount { get; set; }
        public int AssetCount { get; set; }
        public string DropDate { get; set; }
        public string DroppedGallons { get; set; }
        public int InvoiceHeaderId { get; set; }
        public string InvoiceUpdatedDate { get; set; }
        public string InvoiceUpdateTime { get; set; }


    }
}
