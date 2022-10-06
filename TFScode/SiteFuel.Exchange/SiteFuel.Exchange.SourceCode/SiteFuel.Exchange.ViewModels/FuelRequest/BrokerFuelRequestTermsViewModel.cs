using System;
using System.Collections.Generic;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.ComponentModel.DataAnnotations;
using Foolproof;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class BrokerFuelRequestTermsViewModel : StatusViewModel
    {
        public BrokerFuelRequestTermsViewModel()
        {
            InstanceInitialize();
        }
        public BrokerFuelRequestTermsViewModel(Status status) : base(status)
        {
            InstanceInitialize(status);
        }
        private void InstanceInitialize(Status status = Status.Failed)
        {
            FuelRequestFee = new BrokerFuelRequestFeeViewModel();
            SpecialInstructions = new List<SpecialInstructionViewModel>();
            StatusId = (int)FuelRequestStatus.Open;
            CreatedDate = DateTime.Now;
            PaymentTermId = (int)PaymentTerms.NetDays;
            PaymentDiscount = new PaymentDiscountViewModel();
            SupplierQualifications = new List<int>();
        }

        public int OrderId { get; set; }

        public BrokerFuelRequestFeeViewModel FuelRequestFee { get; set; }
        public List<SpecialInstructionViewModel> SpecialInstructions { get; set; }
        public int Id { get; set; }
        public int CompanyId { get; set; }

        public bool IsProFormaPoEnabled { get; set; }

        [Display(Name = nameof(Resource.lblPoNumber), ResourceType = typeof(Resource))]
        [Remote("IsValidPONumberInBroker", "Validation", AreaReference.UseRoot, AdditionalFields = "Id,CompanyId,IsProFormaPoEnabled", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageAlreadyExist))]
        public string ExternalPoNumber { get; set; }

        public string RequestNumber { get; set; }

        [Display(Name = nameof(Resource.lblStatus), ResourceType = typeof(Resource))]
        public int StatusId { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public Nullable<int> TerminalId { get; set; }

        public Nullable<int> OrderClosingThreshold { get; set; }

        [Display(Name = nameof(Resource.lblPaymentTerms), ResourceType = typeof(Resource))]
        public int PaymentTermId { get; set; }

        public string PaymentTermName { get; set; }

        [RequiredIf("PaymentTermId", (int)PaymentTerms.NetDays, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public int NetDays { get; set; }

        [Display(Name = nameof(Resource.lblSupplierNeedToQualify), ResourceType = typeof(Resource))]
        public List<int> SupplierQualifications { get; set; }
        public PaymentDiscountViewModel PaymentDiscount { get; set; }
    }

}
