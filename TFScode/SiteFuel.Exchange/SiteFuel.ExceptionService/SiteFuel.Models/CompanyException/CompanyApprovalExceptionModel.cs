using SiteFuel.Models.InvoiceException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Models.CompanyException
{
    public class CompanyApprovalExceptionModel
    {
        public List<GeneratedExceptionApprovalModel> GeneratedExceptions { get; set; }
    }

    public class RaisedExceptionModel
    {
        public int Id { get; set; }
        public int ExceptionTypeId { get; set; }
        public string ParameterJson { get; set; }
    }
}
