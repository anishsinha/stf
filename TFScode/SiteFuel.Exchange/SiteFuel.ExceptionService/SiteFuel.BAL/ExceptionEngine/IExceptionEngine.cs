using SiteFuel.Models.InvoiceException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiteFuel.Models.Common.Enums;
using SiteFuel.Models.ApiModels;

namespace SiteFuel.BAL.ExceptionEngine
{
    interface IExceptionEngine
    {
        ExceptionType Type { get; set; }

        GeneratedExceptionModel CheckException(InvoiceExceptionRequestModel model, EnabledExceptionModel enabledException);
    }
}
