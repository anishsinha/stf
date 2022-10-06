using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class ProgressReportFilter
    {
        public ProgressReportFilter()
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            CountryId = (int)Country.USA;
        }

        [Display(Name = nameof(Resource.lblFrom), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public DateTimeOffset StartDate { get; set; }

        [Display(Name = nameof(Resource.lblTo), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public DateTimeOffset EndDate { get; set; }

        [Display(Name = nameof(Resource.lblStartTime), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public DateTime StartTime { get; set; }

        [Display(Name = nameof(Resource.lblEndTime), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public DateTime EndTime { get; set; }

        public DateTimeOffset MonthStartDate { get; set; }

        public DateTimeOffset MonthEndDate { get; set; }

        public int AccountOwnerId { get; set; }

        public int CountryId { get; set; }
    }

    public class ProgressReportMailingList
    {
        public string Country { get; set; }
        public string Emails { get; set; }
    }
    public class MailingList
    {
        public List<ProgressReportMailingList> ProgressReportMailingList { get; set; }
    }
}