using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{

    [DelimitedRecord(",")]
    [IgnoreEmptyLines]
    public class EBolFileModel
    {

        [FieldQuoted]
        public string TransmissionID { get; set; }

        [FieldQuoted]
        public string TransmissionDate { get; set; }

        [FieldQuoted]
        public string TransmissionTime { get; set; }

        [FieldQuoted]
        public string Type { get; set; }

        [FieldQuoted]
        public string Supplier { get; set; }

        [FieldQuoted]
        public string TerminalName { get; set; }

        [FieldQuoted]
        public string TerminalControlNumber { get; set; }

        [FieldQuoted]
        public string SPLCCode { get; set; }

        [FieldQuoted]
        public string CarrierName { get; set; }

        [FieldQuoted]
        public string CarrierSCAC { get; set; }

        [FieldQuoted]
        public string CarrierFEIN { get; set; }

        [FieldQuoted]
        public string DriverNumber { get; set; }

        [FieldQuoted]
        public string VehicleType { get; set; }

        [FieldQuoted]
        public string VehicleNumber { get; set; }

        [FieldQuoted]
        public string DestinationCustomerName { get; set; }

        [FieldQuoted]
        public string DestinationCustomerNumber { get; set; }

        [FieldQuoted]
        public string DestinationAddress { get; set; }

        [FieldQuoted]
        public string DestinationCity { get; set; }

        [FieldQuoted]
        public string DestinationState { get; set; }

        [FieldQuoted]
        public string DestinationCode { get; set; }

        [FieldQuoted]
        public string CustomerNumberAdditional { get; set; }

        [FieldQuoted]
        public string BOLNumber { get; set; }

        [FieldQuoted]
        public string BolDate { get; set; }

        [FieldQuoted]
        public string PurchaseOrderNumber { get; set; }

        [FieldQuoted]
        public string InvoiceNumber { get; set; }

        [FieldQuoted]
        public string ReleaseNumber { get; set; }

        [FieldQuoted]
        public string GateInDate { get; set; }

        [FieldQuoted]
        public string GateInTime { get; set; }
        public string StartLoadDate { get; set; }

        [FieldQuoted]
        public string StartLoadTime { get; set; }

        [FieldQuoted]
        public string EndLoadDate { get; set; }

        [FieldQuoted]
        public string EndLoadTime { get; set; }

        [FieldQuoted]
        public string ProductDescription { get; set; }

        [FieldQuoted]
        public string ProductCode { get; set; }

        [FieldQuoted]
        public string UnitOfMeasure { get; set; }

        [FieldQuoted]
        public string GrossGallons { get; set; }

        [FieldQuoted]
        public string NetGallons { get; set; }

        [FieldQuoted]
        public string Temprature { get; set; }

        [FieldQuoted]
        public string Gravity { get; set; }

        [FieldQuoted]
        public string APIGravity { get; set; }

        [FieldQuoted]
        public string SpecificGravity { get; set; }

        [FieldQuoted]
        public string DTNOwnerName { get; set; }

        [FieldQuoted]
        public string SupplierDefinedTerminal { get; set; }

        [FieldQuoted]
        public string DTNBrandIndicator { get; set; }

        [FieldQuoted]
        public string SupplierDefinedProduct { get; set; }

        [FieldQuoted]
        public string ContainerNumber1 { get; set; }

        [FieldQuoted]
        public string ContainerNumber2 { get; set; }

        [FieldQuoted]
        public string ConsignorName { get; set; }

        [FieldQuoted]
        public string ReservedForFuture1 { get; set; }

        [FieldQuoted]
        public string ReservedForFuture2 { get; set; }

        [FieldQuoted]
        public string ReservedForFuture3 { get; set; }

        [FieldQuoted]
        public string ReservedForFuture4 { get; set; }

        [FieldQuoted]
        public string ReservedForFuture5 { get; set; }

        [FieldQuoted]
        public string ReservedForFuture6 { get; set; }
    }
}
