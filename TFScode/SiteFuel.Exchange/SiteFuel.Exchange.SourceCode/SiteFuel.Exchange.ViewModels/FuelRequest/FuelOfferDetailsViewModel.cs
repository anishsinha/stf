using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Foolproof;
using System.Web.Mvc;
using SiteFuel.Exchange.ViewModels.CustomAttributes;

namespace SiteFuel.Exchange.ViewModels
{
    public class FuelOfferDetailsViewModel : BaseViewModel
    {
        public FuelOfferDetailsViewModel()
        {
            InstanceInitialize();
        }

        public FuelOfferDetailsViewModel(Status status) : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            PaymentTermId = (int)PaymentTerms.NetDays;
            PaymentDiscount = new PaymentDiscountViewModel();
            PrivateSupplierList = new PrivateSupplierListViewModel();
            PrivateSupplierList.Id = 0;
            SupplierQualifications = new List<int>();
            CloseOrderId = (int)CloseOrderType.OnCompleted;
        }

        [Display(Name = nameof(Resource.lblAutomaticallyCloseOrder), ResourceType = typeof(Resource))]
        public int CloseOrderId { get; set; }

        [Display(Name = nameof(Resource.lblAutomaticallyCloseOrder), ResourceType = typeof(Resource))]
        public Nullable<int> OrderClosingThreshold { get; set; }

        [Display(Name = nameof(Resource.lblPaymentTerms), ResourceType = typeof(Resource))]
        public int PaymentTermId { get; set; }

        public PaymentMethods PaymentMethod { get; set; }

        public string PaymentTermName { get; set; }

        [RequiredIf("PaymentTermId", (int)PaymentTerms.NetDays, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public int NetDays { get; set; }

        [Display(Name = nameof(Resource.lblSupplierNeedToQualify), ResourceType = typeof(Resource))]
        public List<int> SupplierQualifications { get; set; }

        public PaymentDiscountViewModel PaymentDiscount { get; set; }

        public PrivateSupplierListViewModel PrivateSupplierList { get; set; }
    }
}
