using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Foolproof;

namespace SiteFuel.Exchange.ViewModels
{
    public class PaymentDiscountViewModel : StatusViewModel
    {
        public PaymentDiscountViewModel()
        {
            InstanceInitialize();
        }

        public PaymentDiscountViewModel(Status status)
            : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            IsDiscountOnEarlyPayment = false;
        }

        public int Id { get; set; }

        [Display(Name = nameof(Resource.lblDiscountForEarlyPayment), ResourceType = typeof(Resource))]
        public bool IsDiscountOnEarlyPayment { get; set; }

        [RequiredIfTrue("IsDiscountOnEarlyPayment", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblDiscountPercent), ResourceType = typeof(Resource))]
        public int DiscountPercent { get; set; }

        [RequiredIfTrue("IsDiscountOnEarlyPayment", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblDaysAfterInvoice), ResourceType = typeof(Resource))]
        public int WithinDays { get; set; }
    }
}
