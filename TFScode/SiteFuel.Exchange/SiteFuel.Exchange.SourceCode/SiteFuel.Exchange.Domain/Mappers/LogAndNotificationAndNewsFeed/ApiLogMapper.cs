using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class ApiLogMapper
    {
        public static ApiLog ToEntity(this ApiLogViewModel viewModel, ApiLog entity = null)
        {
            if (entity == null)
                entity = new ApiLog();

            entity.Id = viewModel.Id;
            entity.Request = viewModel.Request;
            entity.Response = viewModel.Response;
            entity.Message = viewModel.Message;
            entity.Url = viewModel.Url;
            entity.CreatedBy = viewModel.CreatedBy;
            entity.CreatedDate = viewModel.CreatedDate;
            entity.ExternalRefID = viewModel.ExternalRefID;
            entity.CompanyId = viewModel.CompanyId;
            return entity;
        }
        public static ApiLogViewModel ToViewModel(this ApiLog entity, ApiLogViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new ApiLogViewModel();

            viewModel.Id = entity.Id;
            //viewModel.Request = entity.Request;
           // viewModel.Response = entity.Response;
            viewModel.Message = entity.Message;
            viewModel.Url = entity.Url;
            viewModel.CreatedBy = entity.CreatedBy;
            viewModel.CreatedDate = entity.CreatedDate;
            viewModel.ExternalRefID = entity.ExternalRefID;
            viewModel.CompanyId = entity.CompanyId;
            return viewModel;
        }

    }
}
