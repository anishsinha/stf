using SiteFuel.BAL.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Models.ApiModels;
using SiteFuel.Models.Common.Enums;
using SiteFuel.Models.InvoiceException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.BAL.ExceptionEngine
{
    public class ApiException : IExceptionEngine
    {
        public ExceptionType Type { get; set; } = ExceptionType.InvoiceApiException;

        public GeneratedExceptionModel CheckException(InvoiceExceptionRequestModel model, EnabledExceptionModel enabledException)
        {
            GeneratedExceptionModel exceptionModel = null;
            try
            {
                if (!string.IsNullOrWhiteSpace(model.ExternalRefID))
                {
                    exceptionModel = new GeneratedExceptionModel();
                    exceptionModel.Status = ExceptionStatus.Raised;
                    exceptionModel.ExceptionTypeId = enabledException.ExceptionTypeId;
                    exceptionModel.OwnerCompanyId = enabledException.OwnerCompanyId;
                    exceptionModel.ApproverCompanyId = enabledException.ApproverCompanyId;
                    exceptionModel = model.ToGeneratedExceptionModel(exceptionModel);
                    var exceptionDetail = model.ToInvoiceExceptionModel();
                    exceptionModel.GeneratedExceptionDetail = exceptionDetail;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ApiException", "CheckException", ex.Message, ex);
                throw;
            }
            return exceptionModel;
        }
    }
}
