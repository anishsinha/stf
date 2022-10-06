using SiteFuel.BAL.Mappers;
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
    public class DeliveredQuantityVariance : IExceptionEngine
    {
        public ExceptionType Type { get; set; } = ExceptionType.DeliveredQuantityVariance;

        public GeneratedExceptionModel CheckException(InvoiceExceptionRequestModel model, EnabledExceptionModel enabledException)
        {
            GeneratedExceptionModel exceptionModel = null;
            try
            {
                var quantityTolerance = enabledException.Threshold * model.BolQuantity / 100;
                var quantityVarience = model.BolQuantity - quantityTolerance;
                if (model.DroppedQuantity < quantityVarience)
                {
                    exceptionModel = new GeneratedExceptionModel();
                    exceptionModel.Status = ExceptionStatus.Raised;
                    exceptionModel.ExceptionTypeId = enabledException.ExceptionTypeId;
                    exceptionModel.OwnerCompanyId = enabledException.OwnerCompanyId;
                    exceptionModel.ApproverCompanyId = enabledException.ApproverCompanyId;
                    exceptionModel = model.ToGeneratedExceptionModel(exceptionModel);
                    var exceptionDetail = model.ToGeneratedExceptionDetailModel();
                    exceptionDetail.Tolerance = quantityTolerance;
                    exceptionDetail.Varience = quantityVarience;
                    exceptionModel.GeneratedExceptionDetail = exceptionDetail;
                }
            }
            catch
            {
                throw;
            }
            return exceptionModel;
        }
    }
}
