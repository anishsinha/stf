using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class AssetJobAssignmentViewModel : StatusViewModel
    {
        public AssetJobAssignmentViewModel()
        {
           
        }

        public AssetJobAssignmentViewModel(Status status)
            : base(status)
        {
           
        }

        public int Id { get; set; }

        public int AssetId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblJobName), ResourceType = typeof(Resource))]
        public int JobId { get; set; }

        public int AssignedBy { get; set; }

        public DateTimeOffset AssignedDate { get; set; }

        public Nullable<int> RemovedBy { get; set; }

        public Nullable<DateTimeOffset> RemovedDate { get; set; }
    }
}
