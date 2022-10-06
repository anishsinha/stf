using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class InvoiceNumberViewModel : BaseViewModel
    {
        public InvoiceNumberViewModel()
        {
            InstanceInitialize();
        }

        public InvoiceNumberViewModel(Status status)
            : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            Id = 0;
        }

        public int Id { get; set; }

        public string Number { get; set; }
    }
}
