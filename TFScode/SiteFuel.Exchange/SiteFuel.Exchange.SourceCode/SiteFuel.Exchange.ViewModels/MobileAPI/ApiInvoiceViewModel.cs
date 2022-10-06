using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class ApiInvoiceViewModel 
    {
        public ApiInvoiceViewModel()
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            StatusId = (int)InvoiceStatus.Received;
            InvoiceNumber = new InvoiceNumberViewModel();
        }

        public InvoiceNumberViewModel InvoiceNumber { get; set; }

        public int InvoiceTypeId { get; set; }

        public decimal DroppedGallons { get; set; }

        public decimal PricePerGallon { get; set; }

        public decimal RackPrice { get; set; }

        public DateTimeOffset DropStartDate { get; set; }

        public DateTimeOffset DropEndDate { get; set; }

        public decimal BasicAmount { get; set; }

        public int StatusId { get; set; }

        public decimal TotalTaxAmount { get; set; }

        public decimal TotalFees { get; set; }

        public int UnitOfMeasurement { get; set; }

        public int Currency { get; set; }

        public int InvoiceHeaderId { get; set; }
    }
}
