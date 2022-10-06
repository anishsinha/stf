using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Foolproof;
using SiteFuel.Exchange.ViewModels.CustomAttributes;

namespace SiteFuel.Exchange.ViewModels
{
    public class QuoteRequestDetailsViewModel : BaseCultureViewModel
    {
        public QuoteRequestDetailsViewModel()
        {
            Documents = new List<DocumentViewModel>();
        }

        public int Id { get; set; }

        public int QuotesReceived { get; set; }

        public int QuotesNeeded { get; set; }

        public string JobName { get; set; }

        public int JobId { get; set; }

        public int JobStateId { get; set; }

        public string Address { get; set; }

        public int FuelTypeId { get; set; }

        public string FuelType { get; set; }

        public string QuoteNumber { get; set; }

        public string QuoteDueDate { get; set; }

        public DateTimeOffset QuoteDueDateUpdated { get; set; }

        public string RequestType { get; set; }

        public string OrderType { get; set; }

        public decimal Quantity { get; set; }

        public int QuotesNeededUpdated { get; set; }

        public string DeliveryStartDate { get; set; }

        public string EndDate { get; set; }

        public string DeliveryType { get; set; }

        public string EstimatedGallonsPerDelivery { get; set; }

        public bool IncludeFees { get; set; }

        public List<string> SupplierDBE { get; set; }

        public string Notes { get; set; }

        public bool IsEdit { get; set; }

        public List<DocumentViewModel> Documents { get; set; }

        public string Status { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public bool IsQuotationCreated { get; set; }

        public bool IsQuoteRequestDeclined { get; set; }

        public Currency Currency { get; set; }

        public UoM UoM { get; set; }

        public int PaymentTermId { get; set; }

        public string PaymentTermName { get; set; }

        public int NetDays { get; set; }
    }
}
