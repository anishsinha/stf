using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.InvoiceExceptions
{
    public class CompanyApprovalExceptionModel
    {
        public List<ExceptionApprovalResponseModel> GeneratedExceptions { get; set; } = new List<ExceptionApprovalResponseModel>();

        public List<DeliveryMismatchExceptionModel> DeliveryMismatchExceptions { get; set; } = new List<DeliveryMismatchExceptionModel>();
    }
    public class RaisedExceptionModel
    {
        public int Id { get; set; }
        public int ExceptionTypeId { get; set; }
        public string ParameterJson { get; set; }
        public TankExceptionDetailModel TankDetail { get; set; }
    }
    public class TankExceptionDetailModel
    {
        public string SiteId { get; set; }
        public string TankId { get; set; }
        public string StorageId { get; set; }
    }
}
