using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.MobileAPI.TPD
{
    public class ApiResponseViewModel
    {
        public ApiResponseViewModel(Status status = Status.Failed)
        {
            if (status == Status.Success)
            {
                Status = Status.Success;
            }
            else
            {
                Status = Status.Failed;
            }
        }

        public Status Status { get; set; }

        public List<ApiCodeMessages> Messages { get; set; } = new List<ApiCodeMessages>();        

    }

    public class ApiCodeMessages
    {
        public string Message { get; set; }
        public string Code { get; set; }
        public string EntityId { get; set; }
        public int? OrderId { get; set; }
    }
}
