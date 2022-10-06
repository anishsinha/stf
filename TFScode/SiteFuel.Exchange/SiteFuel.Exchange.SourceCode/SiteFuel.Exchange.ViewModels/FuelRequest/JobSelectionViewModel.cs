using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class JobSelectionViewModel : BaseViewModel
    {
        public JobSelectionViewModel()
        {
            InstanceInitialize(Status.Failed);
        }

        public JobSelectionViewModel(Status status) : base(status)
        {
            InstanceInitialize(status);
        }

        private void InstanceInitialize(Status status)
        {
            State = new StateViewModel(status);
            Country = new CountryViewModel(status);
        }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblJobName), ResourceType = typeof(Resource))]
        public int JobId { get; set; }

        public string Name { get; set; }

        [Display(Name = nameof(Resource.headingJobLocation), ResourceType = typeof(Resource))]
        public string Address { get; set; }

        [Display(Name = nameof(Resource.lblCity), ResourceType = typeof(Resource))]
        public string City { get; set; }

        [Display(Name = nameof(Resource.lblState), ResourceType = typeof(Resource))]
        public StateViewModel State { get; set; }

        public CountryViewModel Country { get; set; }

        [Display(Name = nameof(Resource.lblZipcode), ResourceType = typeof(Resource))]
        public string ZipCode { get; set; }

        public bool IsTaxExempted { get; set; }

        public string JobStartDate { get; set; }

        public string JobEndDate { get; set; }

        public bool IsAssetTracked { get; set; }

        public bool IsProFormaPoEnabled { get; set; }

        public string ContractNumber { get; set; }

        public string CustomerName { get; set; }

        public string CustomerEmail { get; set; }

        public bool IsResaleEnabled { get; set; }
        public bool IsRetailJob { get; set; }

        public string JobLocationCurrentDate { get; set; }

        public int StatusId { get; set; }

        public string TimeZoneName { get; set; }

        public int LocationType { get; set; }

        public bool IsMarineLocation { get; set; }

        public int? AcknowledgementId { get; set; }

        [Display(Name = nameof(Resource.lblVessle), ResourceType = typeof(Resource))]
        public int? VessleId { get; set; }

        [Display(Name = nameof(Resource.lblIMONumber), ResourceType = typeof(Resource))]
        public string IMONumber { get; set; }

        [Display(Name = nameof(Resource.lblFlag), ResourceType = typeof(Resource))]
        public string Flag { get; set; }
    }
}
