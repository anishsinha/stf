using System;
using System.Collections.Generic;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.Notification;

namespace SiteFuel.Exchange.ViewModels
{
    public class NotificationDipTestViewModel : BaseNotificationViewModel
    {
        public int CompanyId { get; set; }
        public CompanyType CompanyType { get; set; }
        public List<DipTestSummaryViewModel> DipTest { get; set; } = new List<DipTestSummaryViewModel>();
        public List<NotificationUserViewModel> CompanyUsers { get; set; } = new List<NotificationUserViewModel>();
    }

    public class DipTestProcessViewModel
    {
        public CompanyType CompanyTypeId { get; set; }
        public InventoryDataCaptureType DipTestMethodInventoryDataCapture { get; set; }
    }
}
