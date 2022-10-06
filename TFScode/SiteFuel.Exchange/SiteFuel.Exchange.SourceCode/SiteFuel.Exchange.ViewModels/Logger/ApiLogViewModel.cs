using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
   public class ApiLogViewModel
    {
        public ApiLogViewModel()
        {
            CreatedDate = DateTimeOffset.Now;
        }

        public int Id { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public string Url { get; set; }
        public string ExternalRefID { get; set; }
        public string Message { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int CompanyId { get; set; }
        public DateTimeOffset FromDate { get; set; }
        public DateTimeOffset ToDate { get; set; }

        public int ViewType { get; set; }
        public int ReqResType { get; set; }

       
    }

    public class ApiDetailLogViewModel {
        public int TotalCall { get; set; }
        public int SuccessCall { get; set; }
        public int FailedCall { get; set; }
        public List<ApiLogViewModel> ApiLogList { get; set; }
    }
}
