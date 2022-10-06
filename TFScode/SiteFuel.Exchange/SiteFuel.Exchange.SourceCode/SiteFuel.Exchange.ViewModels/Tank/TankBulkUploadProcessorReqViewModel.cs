using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class TankBulkUploadProcessorReqViewModel
    {
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public CompanyType CompanyType { get; set; }
        public int FileLineNumberToStart { get; set; }
        public string FileUploadPath { get; set; }
        public bool IsTpoTank { get; set; }
    }

    public class TankBulkUploadErrorProcessorViewModel : TankBulkUploadProcessorReqViewModel
    {
        public string Errors { get; set; }
    }
}