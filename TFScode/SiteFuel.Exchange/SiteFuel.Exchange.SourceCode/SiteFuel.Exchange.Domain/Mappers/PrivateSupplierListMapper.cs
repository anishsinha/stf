using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class PrivateSupplierListMapper
    {
        public static PrivateSupplierList ToEntity(this PrivateSupplierListViewModel viewModel, PrivateSupplierList entity = null)
        {
            if (entity == null)
                entity = new PrivateSupplierList();

            entity.Id = viewModel.Id ?? 0;
            entity.Name = viewModel.Name;
            entity.AddedBy = viewModel.AddedById;
            entity.CompanyId = viewModel.CompanyId;
            return entity;
        }
    }
}
