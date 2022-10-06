using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class DsbLoadQueueViewModel
    {
        public int Id { get; set; }
        public string ScheduleBuilderId { get; set; }
        public string Date { get; set; }
        public string RegionId { get; set; }
        public string ShiftId { get; set; }
        public int ShiftIndex { get; set; }
        public int DriverColIndex { get; set; }
        public string TrailerInfo { get; set; }
        public List<TrailerInfo> TrailerDetails { get; set; } = new List<TrailerInfo>();
        public int TfxDriverId { get; set; }
        public string TfxDriverName { get; set; }
        public string DeliveryRequestInfo { get; set; }
        public List<string> DeliveryRequestDetails { get; set; } = new List<string>();
        public string DriverColJsonInfo { get; set; }
        public int TfxUserId { get; set; }
        public string TfxUserName { get; set; }
        public int TfxCompanyId { get; set; }
        public string UserLanguage { get; set; }
        public string DriverColJsonResponse { get; set; }
        public int CreatedBy { get; set; }


    }
    public class DsbLoadQueueStatusViewModel
    {
        public int ShiftIndex { get; set; }
        public int DriverColIndex { get; set; }
        public string TfxDriverName { get; set; }
        public int TfxUserId { get; set; }
        public int TfxCompanyId { get; set; }
        public List<LoadQueueStatus> Messages { get; set; } = new List<LoadQueueStatus>();
    }
    public class LoadQueueStatus
    {
        public string StatusMessage { get; set; }

    }
    public class DsbLoadQueueValidateViewModel
    {
        public int ShiftIndex { get; set; }
        public int DriverColIndex { get; set; }
        public string TrailerInfo { get; set; }
        public int TfxDriverId { get; set; }
        public string TfxDriverName { get; set; }
        public string DeliveryRequestInfo { get; set; }
        public int TfxUserId { get; set; }
        public string TfxUserName { get; set; }
        public int TfxCompanyId { get; set; }

    }
    public class TrailerInfo
    {
        public string Id { get; set; }
        public string TrailerId { get; set; }
    }
    public class DsbLoadQueueUserModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
    }
    public class DsbLoadQueueSuccessModel
    {
        public int Id { get; set; }
        public string DSBSaveModel { get; set; } = string.Empty;
    }
}
