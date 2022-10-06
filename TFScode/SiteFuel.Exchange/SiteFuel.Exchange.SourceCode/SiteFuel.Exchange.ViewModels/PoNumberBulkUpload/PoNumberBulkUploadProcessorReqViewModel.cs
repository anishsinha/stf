using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class PoNumberBulkUploadProcessorReqViewModel
    {
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public CompanyType CompanyType { get; set; }
        public int FileLineNumberToStart { get; set; }
        public string FileUploadPath { get; set; }
    }

    public class PoNumberBulkUploadErrorProcessorViewModel : PoNumberBulkUploadProcessorReqViewModel
    {
        public string Errors { get; set; }
    }
}