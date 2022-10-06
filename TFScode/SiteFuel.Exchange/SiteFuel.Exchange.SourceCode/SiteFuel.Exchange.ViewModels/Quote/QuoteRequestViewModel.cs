using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Foolproof;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class QuoteRequestViewModel : BaseViewModel
    {
        public QuoteRequestViewModel()
        {
            InstanceInitialize();
        }

        public QuoteRequestViewModel(Status status) : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            Job = new JobSelectionViewModel();
            FuelDetails = new FuelDetailsViewModel();
            PrivateSupplierList = new PrivateSupplierListViewModel();
            PrivateSupplierList.Id = 0;
            DeliveryTypeId = (int)DeliveryType.OneTimeDelivery;
            IsExistingJob = true;
            Qualifications = new List<int>();
            Documents = new List<DocumentViewModel>();
            CountryId = 1;
            FuelOfferDetails = new FuelOfferDetailsViewModel();
        }

        public int Id { get; set; }

        public bool IsExistingJob { get; set; }

        public JobSelectionViewModel Job { get; set; }

        [Display(Name = nameof(Resource.lblName), ResourceType = typeof(Resource))]
        [RequiredIfNot("IsExistingJob", true, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Remote("IsValidNewJobName", "Validation", AreaReference.UseRoot, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageAlreadyExist))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        public string NewJobName { get; set; }

        [Display(Name = nameof(Resource.lblState), ResourceType = typeof(Resource))]
        [RequiredIfNot("IsExistingJob", true, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string StateId { get; set; }

        [Display(Name = nameof(Resource.lblCountry), ResourceType = typeof(Resource))]
        public int CountryId { get; set; }

        [Display(Name = nameof(Resource.lblCountyName), ResourceType = typeof(Resource))]
        //[RequiredIfNot("IsExistingJob", true, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string CountyName { get; set; }

        [Display(Name = nameof(Resource.lblZipcode), ResourceType = typeof(Resource))]
       // [RequiredIfNot("IsExistingJob", true, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string ZipCode { get; set; }

        public FuelDetailsViewModel FuelDetails { get; set; }

        public FuelOfferDetailsViewModel FuelOfferDetails { get; set; }

        public bool AddToFavorite { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblQuotesNeeded), ResourceType = typeof(Resource))]
        public int QuotesNeeded { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblQuotesDueDate), ResourceType = typeof(Resource))]
        public DateTimeOffset QuoteDueDate { get; set; }

        [Display(Name = nameof(Resource.lblPrice), ResourceType = typeof(Resource))]
        public int PricingTypeId { get; set; }

        [Display(Name = nameof(Resource.lblQuantity), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Range(typeof(Decimal), ApplicationConstants.DecimalMinValue, ApplicationConstants.DecimalMaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public decimal Quantity { get; set; }

        [LessThanOrEqualTo("EndDate", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageLessThan), PassOnNull = true)]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblStartDate), ResourceType = typeof(Resource))]
        public DateTimeOffset StartDate { get; set; }

        public bool IsOrderEndDateRequired { get; set; }

        [RequiredIf("IsOrderEndDateRequired", true, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [GreaterThanOrEqualTo("StartDate", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageGreaterThan), PassOnNull = true)]
        [Display(Name = nameof(Resource.lblEndDate), ResourceType = typeof(Resource))]
        public Nullable<System.DateTimeOffset> EndDate { get; set; }

        public int DeliveryTypeId { get; set; }

        [Display(Name = nameof(Resource.lblType), ResourceType = typeof(Resource))]
        public SingleDeliverySubTypes SingleDeliverySubTypes { get; set; }

        [Display(Name = nameof(Resource.lblEstimatedQuantityPerDelivery), ResourceType = typeof(Resource))]
        public int? EstimatedGallonsPerDelivery { get; set; }

        public bool IncludeFees { get; set; }

        public string RequestNumber { get; set; }

        [Display(Name = nameof(Resource.lblDBE), ResourceType = typeof(Resource))]
        public List<int> Qualifications { get; set; }

        public PrivateSupplierListViewModel PrivateSupplierList { get; set; }

        public string Notes { get; set; }

        public List<DocumentViewModel> Documents { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        [Display(Name = nameof(Resource.lblCountryGroup), ResourceType = typeof(Resource))]
        public int? CountryGroupId { get; set; }
    }
}
