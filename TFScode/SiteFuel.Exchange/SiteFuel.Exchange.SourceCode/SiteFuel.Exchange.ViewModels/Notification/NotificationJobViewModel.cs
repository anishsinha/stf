using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.Notification;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class NotificationJobViewModel : BaseNotificationViewModel
    {
        public NotificationJobViewModel()
        {
            ApproverUser = new NotificationUserViewModel();
            Creator = new NotificationUserViewModel();
            OnsitePersons = new List<NotificationUserViewModel>();
            PreviousApprover = new NotificationUserViewModel();
            AssignedTo = new List<NotificationUserViewModel>();
        }
        public NotificationUserViewModel Creator { get; set; }
        public NotificationUserViewModel ApproverUser { get; set; }
        public List<NotificationUserViewModel> OnsitePersons { get; set; }
        public List<NotificationUserViewModel> AssignedTo { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public bool IsApprovalWorkflowEnabled { get; set; }
        public NotificationUserViewModel PreviousApprover { get; set; }
        public bool IsProFormaPoEnabled { get; set; }
        public InventoryDataCaptureType InventoryDataCaptureType { get; set; }
    }
}
