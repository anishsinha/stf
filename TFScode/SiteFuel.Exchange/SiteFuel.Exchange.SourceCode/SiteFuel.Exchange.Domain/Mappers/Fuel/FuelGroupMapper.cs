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
    public static class FuelGroupMapper
    {
        public static FuelGroup ToEntity(this FuelGroupViewModel viewModel, int companyId, int userId, FuelGroup entity = null)
        {
            if (entity == null)
                entity = new FuelGroup();

            entity.GroupName = viewModel.GroupName;
            entity.TableType = viewModel.TableType;
            entity.FuelGroupType = viewModel.FuelGroupType;
            entity.AssignedCompanyId = viewModel.AssignedCompanyId;
            entity.CreatedByCompanyId = companyId;
            entity.StatusId = viewModel.FreightTableStatus;
            entity.CreatedBy = userId;
            entity.CreatedDate = DateTimeOffset.Now;
            entity.IsActive = true;
            entity.UpdatedBy = userId;
            entity.UpdatedDate = entity.CreatedDate;

            return entity;
        }

        public static FuelGroupViewModel ToViewModel(this FuelGroup entity, FuelGroupViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new FuelGroupViewModel();

            viewModel.Id = entity.Id;
            viewModel.GroupName = entity.GroupName;
            viewModel.TableType = entity.TableType;
            viewModel.FreightTableStatus = entity.StatusId;
            viewModel.FuelGroupType = entity.FuelGroupType;

            if (viewModel.TableType == Utilities.TableTypes.CustomerSpecific || viewModel.TableType == Utilities.TableTypes.CarrierSpecific)
                viewModel.AssignedCompanyId = entity.AssignedCompanyId;

            viewModel.ProductTypeIds = entity.FuelGroupProductTypes.Select(t => t.ProductTypeId).ToList();
            viewModel.FuelTypeIds = entity.FuelGroupFuelTypes.Select(t => t.TfxProductId).ToList();

            return viewModel;
        }
    }
}
