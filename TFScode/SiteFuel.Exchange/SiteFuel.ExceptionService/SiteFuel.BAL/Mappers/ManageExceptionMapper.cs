using SiteFuel.Models.ApiModels;
using SiteFuel.Models.CompanyException;
using SiteFuel.Models.CustomerException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.BAL.Mappers
{
    public static class ManageExceptionMapper
    {
        public static CompanyExceptionModel ToCompanyExceptionModel(this UspGetCompanyException entity, CompanyExceptionModel model = null)
        {
            if (model == null)
                model = new CompanyExceptionModel();

            model.TypeId = entity.TypeId;
            model.TypeName = entity.TypeName;
            model.ApproverId = entity.ExceptionApproverId;
            model.AutoApprovalDays = entity.AutoApprovalDays;
            model.DelayInvoiceCreationTime = entity.DelayInvoiceCreationTime;
            model.Threshold = entity.Threshold;
            model.IsActive = entity.IsActive;

            return model;
        }

        public static CustomerExceptionModel ToCustomerExceptionModel(this UspGetCustomerException entity, CustomerExceptionModel model = null)
        {
            if (model == null)
                model = new CustomerExceptionModel();

            model.ExceptionTypeId = entity.ExceptionTypeId;
            model.ExceptionTypeName = entity.ExceptionTypeName;
            model.IsEnabled = entity.IsEnabled;
            model.IsActive = entity.IsActive;

            return model;
        }
    }
}
