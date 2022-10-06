using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels.BillingStatement
{
    public class BillingScheduleViewModel
    {
        public BillingScheduleViewModel()
        {
            CountryId = (int)Country.USA;
            IsIncludePreviousStatement = false;
        }

        public int Id { get; set; }

        [Display(Name = nameof(Resource.lblStatementId), ResourceType = typeof(Resource))]
        [Remote("IsValidStatementId", "Validation", AreaReference.UseRoot, AdditionalFields = "Id,CreatedByCompanyId", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.errMessageBillingStatementIdAlreadyExists))]
        public string BillingStatementId { get; set; }

        [Required]
        [Display(Name = nameof(Resource.lblFrequency), ResourceType = typeof(Resource))]
        public int FrequencyTypeId { get; set; }

        [Required]
        [Display(Name = nameof(Resource.lblCustomer), ResourceType = typeof(Resource))]
        public int CustomerId { get; set; }

        [Required]
        [Display(Name = nameof(Resource.lblOrder), ResourceType = typeof(Resource))]
        public List<int> Orders { get; set; }

        [Required]
        [Display(Name = nameof(Resource.lblStartDate), ResourceType = typeof(Resource))]
        public DateTimeOffset StartDate { get; set; } = DateTimeOffset.Now;

        [Required]
        [Display(Name = nameof(Resource.lblPaymentTerms), ResourceType = typeof(Resource))]
        public int PaymentTermId { get; set; } = (int)PaymentTerms.DueOnReceipt;
        public int PaymentNetDays { get; set; }
        public bool IsActive { get; set; } = true;
        public int CreatedBy { get; set; }
        public int CreatedByCompanyId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public int VersionNumber { get; set; }
        public string ScheduleChainId { get; set; }

        [Required]
        [Display(Name = nameof(Resource.lblTimeZoneName), ResourceType = typeof(Resource))]
        public string TimeZone { get; set; }
        public string CompanyTimeZone { get; set; }

        [Display(Name = nameof(Resource.lblFrequencyDay), ResourceType = typeof(Resource))]
        public int? WeekDayId { get; set; }

        [Required]
        [Display(Name = nameof(Resource.lblUpdateFreqency), ResourceType = typeof(Resource))]
        public int UpdateFrequencyTypeId { get; set; }

        [Required]
        [Display(Name = nameof(Resource.lblStatementUpdateFrequencyValue), ResourceType = typeof(Resource))]
        public int? UpdateFrequencyValue { get; set; }

        [Display(Name = nameof(Resource.lblCountry), ResourceType = typeof(Resource))]
        public int CountryId { get; set; }

        public bool IsIncludePreviousStatement { get; set; }

        public bool IsStatmentExists { get; set; }
        public bool IsCountryDropdownRequired { get; set; }
    }
}
