using System;
using System.Text;

namespace SiteFuel.Exchange.FileGenerator.DTN
{
    public class InvoiceHeader
    {
        /// <summary>
        /// INVH = Invoice Header
        /// </summary>
        public string RecordType { get { return "INVH"; } }
        /// <summary>
        /// REQUIRED
        /// <para>
        /// Ref ID
        /// </para>
        /// </summary>
        public string Identifier { get; set; } = string.Empty;
        /// <summary>
        /// REQUIRED
        /// </summary>
        public string InvoiceNumber { get; set; } = string.Empty;
        /// <summary>
        /// REQUIRED if invoice number changed.
        /// <para>
        /// Used for Credit and Deferred Tax Only invoices.
        /// </para>
        /// </summary>
        public string OriginalInvoiceNumber { get; set; } = string.Empty;
        /// <summary>
        /// REQUIRED
        /// </summary>
        public DateTime InvoiceDate { get; set; }
        /// <summary>
        /// PR - Product Invoice
        /// </summary>
        public string DocumentType { get; set; }
        /// <summary>
        /// REQUIRED
        /// <para>
        /// Terms given on the invoice. If terms are not given, then "No Terms Available"
        /// </para>
        /// </summary>
        public string TermsDescription { get; set; } = "No Terms Available";
        /// <summary>
        /// REQUIRED: Document Grand "Gross" Total
        /// <para>
        /// Total of: [Products + Taxes/Fees + Deferred Taxes].
        /// </para>
        /// </summary>
        public decimal DocumentGrandTotal { get; set; }
        /// <summary>
        /// REQUIRED
        /// </summary>
        public DateTime InvoiceDueDate { get; set; }
        /// <summary>
        /// REQUIRED
        /// <para>
        /// Total of [Products + Taxes/Fees + Discount]. Deferred Taxes are not included in this total.
        /// </para>
        /// </summary>
        public decimal TotalInvoiceAmount { get; set; }
        /// <summary>
        /// Optional:
        /// <para>
        /// Required if discount is present otherwise ,, for place holder
        /// </para>
        /// </summary>
        public DateTime? DiscountDueDate { get; set; }
        /// <summary>
        /// Optional
        /// <para>
        /// Required if discount is present otherwise ,, for place holder
        /// </para>
        /// </summary>
        public decimal? Discount { get; set; }
        /// <summary>
        /// REQUIRED
        /// <para>
        /// If no discount this amount same as Total Invoice Amount
        /// </para>
        /// </summary>
        public decimal DiscountedAmountDue { get; set; }
        /// <summary>
        /// REQUIRED
        /// </summary>
        public string SellerName { get; set; } = string.Empty;
        /// <summary>
        /// Conditional: If no Sold To Number, Sold To Name is REQUIRED
        /// </summary>
        public string SoldToCustomerNumber { get; set; } = string.Empty;
        /// <summary>
        /// Conditional: If no Sold To Name, Sold To Number is REQUIRED
        /// </summary>
        public string SoldToName { get; set; } = string.Empty;
        /// <summary>
        /// optional
        /// </summary>
        public string PurchaseOrderNumber { get; set; } = string.Empty;
        /// <summary>
        /// Conditional: If no Ship To Number, Ship To Name is REQUIRED
        /// </summary>
        public string ShipToName { get; set; } = string.Empty;
        /// <summary>
        /// Conditional: If no Ship To Name, Ship To Number is REQUIRED
        /// </summary>
        public string ShipToNumber { get; set; } = string.Empty;
        public string ShipToAddress { get; set; } = string.Empty;
        public string ShipToAddress2 { get; set; } = string.Empty;
        public string ShipToCity { get; set; } = string.Empty;
        public string ShipToState { get; set; } = string.Empty;
        public string ShipToZip { get; set; } = string.Empty;
        /// <summary>
        /// Terminal Name. If Field 31 is TCN Number Fields 26-30 can be blank ,"",
        /// </summary>
        public string ShipFromName { get; set; } = string.Empty;
        /// <summary>
        /// Terminal Address 1. If TCN used, then ,"",
        /// </summary>
        public string ShipFromAddress { get; set; } = string.Empty;
        /// <summary>
        /// Terminal Address 2. If TCN used, then ,"",
        /// </summary>
        public string ShipFromAddress2 { get; set; } = string.Empty;
        /// <summary>
        /// Terminal City. If TCN used, then ,"",
        /// </summary>
        public string ShipFromCity { get; set; } = string.Empty;
        /// <summary>
        /// Terminal State. If TCN used, then ,"",
        /// </summary>
        public string ShipFromState { get; set; } = string.Empty;
        /// <summary>
        /// Terminal ZipCode. If TCN used, then ,"",
        /// </summary>
        public string ShipFromZip { get; set; } = string.Empty;
        /// <summary>
        /// Optional: Can be either TCN or SPLC.
        /// <para>
        /// TCN numbers are preferred over SPLC because TCN number are terminal specific,
        /// SPLC are City/State only. There can be multiple TCN numbers within the same City/State.
        /// </para>
        /// </summary>
        public string TCN_SPLC { get; set; } = string.Empty;
        public override string ToString()
        {
            var values = new StringBuilder();
            values.Append($"\"{RecordType}\",\"{Identifier}\",\"{InvoiceNumber}\",\"{OriginalInvoiceNumber}\",");
            values.Append($"{InvoiceDate.ToString(DtnConstants.DateFormat)},\"{DocumentType}\",\"{TermsDescription}\",");
            values.Append($"{DocumentGrandTotal.ToString(DtnConstants.NumberFormat2)},{InvoiceDueDate.ToString(DtnConstants.DateFormat)},{TotalInvoiceAmount.ToString(DtnConstants.NumberFormat2)},");
            values.Append($"{DiscountDueDate?.ToString(DtnConstants.DateFormat) ?? string.Empty},{Discount?.ToString() ?? string.Empty},");
            values.Append($"{DiscountedAmountDue.ToString(DtnConstants.NumberFormat2)},\"{SellerName}\",\"{SoldToCustomerNumber}\",\"{SoldToName}\",\"{PurchaseOrderNumber}\",");
            values.Append($"\"{ShipToName}\",\"{ShipToNumber}\",\"{ShipToAddress}\",\"{ShipToAddress2}\",\"{ShipToCity}\",");
            values.Append($"\"{ShipToState}\",\"{ShipToZip}\",\"{ShipFromName}\",\"{ShipFromAddress}\",\"{ShipFromAddress2}\",");
            values.Append($"\"{ShipFromCity}\",\"{ShipFromState}\",\"{ShipFromZip}\",\"{TCN_SPLC}\"");
            return values.ToString();
        }
    }
}