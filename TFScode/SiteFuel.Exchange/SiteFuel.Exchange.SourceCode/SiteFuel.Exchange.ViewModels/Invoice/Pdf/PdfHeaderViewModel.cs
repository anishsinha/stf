using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.Invoice.Pdf;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class PdfHeaderViewModel
    {
        public PdfHeaderViewModel()
        {
            this.PaymentDueDate = DateTimeOffset.Now;
            this.InvoiceDate = DateTimeOffset.Now;
        }
        public string SupplierCompanyName { get; set; }
        public string BuyerCompanyName { get; set; }
        public string CustomerId { get; set; }
        public string Carrier { get; set; }
        public int PaymentTermId { get; set; }
        public string PaymentTerm { get; set; }
        public int NetDays { get; set; }
        public string SupplierPhoneNumber { get; set; }
        public string DisplayInvoiceNumber { get; set; }
        public string JobName { get; set; }
        public int InvoiceHeaderId { get; set; }
        public Currency Currency { get; set; }
        public UoM UoM { get; set; }
        public DateTimeOffset InvoiceDate { get; set; }
        public DateTimeOffset PaymentDueDate { get; set; }
        public AddressViewModel BuyerLocation { get; set; } = new AddressViewModel();
        public AddressViewModel SupplierLocation { get; set; } = new AddressViewModel();
        public ContactPersonViewModel PoContact { get; set; } = new ContactPersonViewModel();
        public AddressViewModel ShippingLocation { get; set; } = new AddressViewModel();
        public ImageViewModel CompanyLogo { get; set; } = new ImageViewModel();
        public string BrokerInvoiceNumber { get; set; }
        public string BrokerCompany { get; set; }
        public JobSpecificBillingInfoViewModel JobSpecificBillTo { get; set; } = new JobSpecificBillingInfoViewModel();
        public string AccountingCompanyId { get; set; }
        public string CarrierOrderId { get; set; }
        public string EditedInvoiceNote { get; set; }
        public string DeliveryRequestId { get; set; }
        public string RouteName { get; set; } = string.Empty;
        public int JobCountryId { get; set; }
        public string InvoiceFooterJson { get; set; }
        public int SupplierCompanyId { get; set; }
        public string Vessel { get; set; }
        public string Berth { get; set; }
        public string BDRNumber { get; set; }
    }
}

