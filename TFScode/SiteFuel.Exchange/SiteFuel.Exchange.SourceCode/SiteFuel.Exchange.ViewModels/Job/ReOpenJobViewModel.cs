using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class ReOpenJobViewModel : StatusViewModel
    {
        public ReOpenJobViewModel()
        {
          
        }

        public ReOpenJobViewModel(Status status) 
            : base(status)
        {
           
        }

       

        public int JobId { get; set; }

        public bool IsEndDate { get; set; }

        public string StartDate { get; set; }

        [RequiredIfTrue("IsEndDate", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public Nullable<DateTimeOffset> EndDate { get; set; }
    }
}
