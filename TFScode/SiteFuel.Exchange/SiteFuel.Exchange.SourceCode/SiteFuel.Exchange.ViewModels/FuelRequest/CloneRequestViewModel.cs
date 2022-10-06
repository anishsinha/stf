using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Foolproof;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class CloneRequestViewModel : StatusViewModel
    {
        public CloneRequestViewModel()
        {
            InstanceInitialize();
        }

        public CloneRequestViewModel(Status status) 
            : base(status)
        {
            InstanceInitialize(status);
        }

        private void InstanceInitialize(Status status = Status.Failed)
        {
            StartDate = DateTimeOffset.Now;
            StartTime = Convert.ToDateTime("08:00").ToShortTimeString();
            EndTime = Convert.ToDateTime("17:00").ToShortTimeString();
        }

        public int Id { get; set; }

        public int CompanyId { get; set; }

        [Display(Name = nameof(Resource.lblPoNumber), ResourceType = typeof(Resource))]
        [Remote("IsValidPONumberInCloneRequest", "Validation", AreaReference.UseRoot, AdditionalFields = "Id,CompanyId", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageAlreadyExist))]
        public string ExternalPoNumber { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblQuantity), ResourceType = typeof(Resource))]
        public decimal Quantity { get; set; }

        [LessThan("EndDate", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageLessThan), PassOnNull = true)]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblStartDate), ResourceType = typeof(Resource))]
        public DateTimeOffset StartDate { get; set; }

        [GreaterThan("StartDate", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageGreaterThan), PassOnNull = true)]
        [Display(Name = nameof(Resource.lblEnd), ResourceType = typeof(Resource))]
        public Nullable<DateTimeOffset> EndDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        //[Display(Name = nameof(Resource.lblDeliveryTime), ResourceType = typeof(Resource))]
        public string StartTime { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        //[Display(Name = nameof(Resource.lblDeliveryTime), ResourceType = typeof(Resource))]
        public string EndTime { get; set; }

        [LessThan("StartDate", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageLessThan), PassOnNull = true)]
        [Display(Name = nameof(Resource.lblExpiration), ResourceType = typeof(Resource))]
        public Nullable<DateTimeOffset> ExpirationDate { get; set; }

        public int QuantityTypeId { get; set; }

        public string JobStartDate { get; set; }

        public string JobEndDate { get; set; }

        public string JobLocationCurrentDateTime { get; set; }

        public bool IsProFormaPoEnabled { get; set; }
    }
}
