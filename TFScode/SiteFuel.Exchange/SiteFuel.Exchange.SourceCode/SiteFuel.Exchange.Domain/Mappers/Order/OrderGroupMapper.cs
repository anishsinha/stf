using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class OrderGroupMapper
    {
        public static OrderGroup ToEntity(this OrderGroupViewModel viewModel, UserContext userContext, OrderGroup entity = null)
        {
            if (entity == null)
                entity = new OrderGroup();

            entity.BuyerCompanyId = viewModel.BuyerCompanyId;
            entity.CreatedBy = userContext.Id;
            entity.CreatedDate = DateTimeOffset.Now;
            entity.GroupType = viewModel.GroupType;
            entity.IsActive = true;
            entity.IsDeleted = false;
            entity.JobId = viewModel.JobId;
            entity.ProductType = viewModel.ProductType;
            entity.RenewalCount = viewModel.RenewalCount;
            entity.RenewalFrequency = (int)viewModel.RenewalFrequency;
            entity.StartDate = viewModel.StartDate;
            entity.SupplierCompanyId = viewModel.SupplierCompanyId;
            entity.UpdatedBy = userContext.Id;
            entity.UpdatedDate = DateTimeOffset.Now;

            entity.OrderGroupXOrders = viewModel.OrderList.ToEntity(viewModel.GroupPoNumber);

            if (viewModel.GroupType == OrderGroupType.Tier)
                entity.TermOrderGroupHistories = viewModel.ToTermGroupHistoryEntity();

            return entity;
        }

        public static List<TermOrderGroupHistory> ToTermGroupHistoryEntity(this OrderGroupViewModel viewModel, List<TermOrderGroupHistory> entites = null)
        {
            if (entites == null)
                entites = new List<TermOrderGroupHistory>();

            foreach (var order in viewModel.OrderList)
            {
                var entity = new TermOrderGroupHistory();
                entity.IsActive = true;
                entity.MaxVolume = order.MaxVolume;
                entity.MinVolume = order.MinVolume;
                entity.StartDate = viewModel.StartDate.Value;
                entity.EndDate = viewModel.StartDate.Value.LastDayOfMonth();
                entity.TotalDropped = 0;
                entity.OrderId = order.OrderId;
                entity.OrderGroupId = order.GroupId;
                entites.Add(entity);
            }

            return entites;
        }

        public static List<OrderGroupXOrder> ToEntity(this List<OrderGroupXOrderDetails> viewModel, string poNumber, List<OrderGroupXOrder> entites = null)
        {
            if (entites == null)
                entites = new List<OrderGroupXOrder>();

            foreach (var order in viewModel)
            {
                var entity = new OrderGroupXOrder();
                entity.BlendPercentage = order.BlendPercentage;
                entity.IsActive = true;
                entity.MaxVolume = order.MaxVolume;
                entity.MinVolume = order.MinVolume;
                entity.OrderId = order.OrderId;
                entity.OrderGroupId = order.GroupId;
                entity.GroupPoNumber = poNumber;
                entites.Add(entity);
            }

            return entites;
        }

        public static OrderGroupXOrder ToEntity(this OrderGroupXOrderDetails viewModel, string poNumber, OrderGroupXOrder entity = null)
        {
            if (entity == null)
            {
                entity = new OrderGroupXOrder();
            }
            entity.BlendPercentage = viewModel.BlendPercentage;
            entity.IsActive = true;
            entity.MaxVolume = viewModel.MaxVolume;
            entity.MinVolume = viewModel.MinVolume;
            entity.OrderId = viewModel.OrderId;
            entity.OrderGroupId = viewModel.GroupId;
            entity.GroupPoNumber = poNumber;

            return entity;
        }
    }
}
