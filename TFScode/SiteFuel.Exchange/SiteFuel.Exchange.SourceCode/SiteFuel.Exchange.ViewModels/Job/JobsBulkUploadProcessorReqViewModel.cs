using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class JobsBulkUploadProcessorReqViewModel
    {
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public CompanyType CompanyType { get; set; }
        public int FileLineNumberToStart { get; set; }
        public string FileUploadPath { get; set; }
    }
    public class JobsBulkUploadErrorProcessorReqViewModel : JobsBulkUploadProcessorReqViewModel
    { 
        public string Errors { get; set; }
    }
}
