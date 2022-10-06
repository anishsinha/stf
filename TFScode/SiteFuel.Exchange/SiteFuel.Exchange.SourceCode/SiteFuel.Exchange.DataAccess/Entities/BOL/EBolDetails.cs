using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    [Table("EBolDetails")]
    public partial class EBolDetails
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string TransmissionID { get; set; }

        [Required]
        public DateTimeOffset TransmissionDate { get; set; }

        [Required]
        [StringLength(128)]
        public string TransmissionTime { get; set; }

        [Required]
        [StringLength(256)]
        public string Supplier { get; set; }

        [StringLength(256)]
        public string TerminalName { get; set; }

        [StringLength(256)]
        public string TerminalControlNumber { get; set; }

        [StringLength(128)]
        public string SPLCCode { get; set; }

        [StringLength(256)]
        public string CarrierName { get; set; }

        [StringLength(128)]
        public string CarrierSCAC  { get; set; }

        [StringLength(128)]
        public string CarrierFEIN  { get; set; }

        [StringLength(128)]
        public string DriverNumber { get; set; }

        [StringLength(128)]
        public string VehicleType { get; set; }

        [StringLength(128)]
        public string VehicleNumber { get; set; }

        [StringLength(128)]
        public string DestinationCustomerName { get; set; }

        [StringLength(256)]
        public string DestinationAddress { get; set; }

        [StringLength(128)]
        public string DestinationCity { get; set; }

        [StringLength(128)]
        public string DestinationState { get; set; }

        [Required]
        [StringLength(128)]
        public string BOLNumber { get; set; }

        public Nullable<DateTimeOffset> BolDate { get; set; }

        [StringLength(256)]
        public string PurchaseOrderNumber { get; set; }

        [StringLength(128)]
        public string InvoiceNumber { get; set; }
        public Nullable<DateTimeOffset> GateInDate { get; set; }

        [StringLength(256)]
        public string GateInTime { get; set; }
        public Nullable<DateTimeOffset> StartLoadDate { get; set; }

        [StringLength(128)]
        public string StartLoadTime { get; set; }
        public Nullable<DateTimeOffset> EndLoadDate { get; set; }

        [StringLength(128)]
        public string EndLoadTime { get; set; }

        [Required]
        [StringLength(256)]
        public string ProductDescription { get; set; }

        [StringLength(128)]
        public string ProductCode  { get; set; }

        [Required]
        [StringLength(128)]
        public string UnitOfMeasure { get; set; }

        [Required]
        public Decimal GrossGallons { get; set; }

        [Required]
        public decimal NetGallons { get; set; }

        [StringLength(256)]
        public string SupplierDefinedTerminal { get; set; }

        [StringLength(256)]
        public string SupplierDefinedProduct { get; set; }

        [StringLength(256)]
        public string ContainerNumber1 { get; set; }

        [StringLength(256)]
        public string ContainerNumber2 { get; set; }

        [StringLength(256)]
        public string ConsignorName { get; set; }

        public Nullable<DateTimeOffset> CreatedDate { get; set; }
        public string HexString { get; set; }
    }
}
