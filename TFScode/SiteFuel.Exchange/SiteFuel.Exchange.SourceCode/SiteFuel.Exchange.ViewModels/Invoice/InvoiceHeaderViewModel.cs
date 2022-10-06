using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class InvoiceHeaderViewModel : BaseViewModel
    {
        public InvoiceHeaderViewModel()
        {
            InstanceInitialize();
        }

        public InvoiceHeaderViewModel(Status status)
            : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            Id = 0;
        }

        public int Id { get; set; }

        public int InvoiceNumberId { get; set; }

        public decimal TotalDroppedGallons { get; set; }

        public decimal TotalBasicAmount { get; set; }

        public decimal TotalFeeAmount { get; set; }

        public decimal TotalTaxAmount { get; set; }

        public decimal TotalDiscountAmount { get; set; }

        public int Version { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
    }
}
