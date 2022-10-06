using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;

namespace SiteFuel.Exchange.Domain
{
    public class OrderGroupDomain : BaseDomain
    {
        public OrderGroupDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public OrderGroupDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task<OrderGroupViewModel> GetOrderGroupDetails(int groupId)
        {
            var response = new OrderGroupViewModel();
            try
            {
                if (groupId == 0)
                    return response;
                var orderGroup = await Context.DataContext.OrderGroups.Where(t => t.Id == groupId).Select(t => new
                {
                    response = new OrderGroupViewModel()
                    {
                        Id = t.Id,
                        BuyerCompanyId = t.BuyerCompanyId,
                        SupplierCompanyId = t.SupplierCompanyId,
                        JobId = t.JobId,
                        ProductType = (ProductCategory)t.ProductType,
                        StartDate = t.StartDate,
                        RenewalFrequency = (OrderGroupFrequency)t.RenewalFrequency,
                        RenewalCount = t.RenewalCount,
                        GroupType = (OrderGroupType)t.GroupType,
                        GroupPoNumber = ""
                    },
                    OrderList = t.OrderGroupXOrders.Where(t1 => t1.IsActive).Select(t1 => new
                    {
                        t1.OrderId,
                        MinVolume = t1.MinVolume,
                        MaxVolume = t1.MaxVolume,
                        t1.GroupPoNumber,
                        Order = new
                        {
                            OrderId = t1.OrderId,
                            TfxPoNumber = t1.Order.PoNumber,
                            FuelType = t1.Order.FuelRequest.MstProduct.DisplayName ?? t1.Order.FuelRequest.MstProduct.Name,
                            t1.Order.FuelRequest.FuelRequestPricingDetail.DisplayPrice,
                            t1.Order.FuelRequest.Currency,
                            t1.Order.FuelRequest.QuantityTypeId,
                            t1.Order.FuelRequest.MinQuantity,
                            t1.Order.FuelRequest.MaxQuantity,
                            t1.Order.FuelRequest.UoM
                        }
                    }).ToList()
                }).FirstOrDefaultAsync();
                response = orderGroup.response;
                response.GroupPoNumber = orderGroup.OrderList.Select(t => t.GroupPoNumber).FirstOrDefault();
                response.OrderList = orderGroup.OrderList.Select(t => new OrderGroupXOrderDetails()
                {
                    OrderId = t.OrderId,
                    MinVolume = t.MinVolume,
                    MaxVolume = t.MaxVolume,
                    Order = new OrderGroupDetailViewModel()
                    {
                        OrderId = t.OrderId,
                        TfxPoNumber = t.Order.TfxPoNumber,
                        FuelType = t.Order.FuelType,
                        DisplayPrice = $"{t.Order.DisplayPrice} {t.Order.Currency}",
                        Quantity = GetFuelRequestQuantity(t.Order.QuantityTypeId, t.Order.UoM, t.Order.MinQuantity, t.Order.MaxQuantity)
                    }
                }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderGroupDomain", "GetOrderGroupDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetCustomersForSupplier(int supplierCompanyId)
        {
            return await Context.DataContext.Orders.Where(t => t.AcceptedCompanyId == supplierCompanyId && (t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open || (t.FuelRequest.FuelRequestDetail.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery &&
                                                                        t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Closed && !t.Invoices.Any())))
                                                            .GroupBy(t => t.BuyerCompanyId)
                                                            .Select(t => t.FirstOrDefault())
                                                            .OrderByDescending(t => t.BuyerCompanyId)
                                                            .Select(t => new DropdownDisplayItem() { Id = t.BuyerCompanyId, Name = t.BuyerCompany.Name })
                                                            .ToListAsync();
        }

        public async Task<List<DropdownDisplayItem>> GetSuppliersForCustomer(int buyerCompanyId)
        {
            return await Context.DataContext.Orders.Where(t => t.BuyerCompanyId == buyerCompanyId && (t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open
                                                                        || (t.FuelRequest.FuelRequestDetail.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery && t.Company.IsActive &&
                                                                        t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Closed && !t.Invoices.Any())))
                                                            .GroupBy(t => t.AcceptedCompanyId)
                                                            .Select(t => t.FirstOrDefault())
                                                            .OrderByDescending(t => t.AcceptedCompanyId)
                                                            .Select(t => new DropdownDisplayItem() { Id = t.AcceptedCompanyId, Name = t.Company.Name })
                                                            .ToListAsync();
        }

        public async Task<List<DropdownDisplayItem>> GetJobsForCustomer(int supplierCompanyId, int buyerCompanyId)
        {
            List<DropdownDisplayItem> response = new List<DropdownDisplayItem>();
            var jobs = await Context.DataContext.Orders.Where(t => t.BuyerCompanyId == buyerCompanyId && t.AcceptedCompanyId == supplierCompanyId && t.FuelRequest.Job.IsActive && (t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open
                                                                        || (t.FuelRequest.FuelRequestDetail.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery &&
                                                                        t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Closed && !t.Invoices.Any())))
                                                            .Select(t => new { Id = t.FuelRequest.Job.Id, Name = t.FuelRequest.Job.Name, t.FuelRequest.Job.Address, t.FuelRequest.Job.City, t.FuelRequest.Job.MstState.Code, t.FuelRequest.Job.ZipCode })
                                                            .Distinct()
                                                            .ToListAsync();
            jobs.ForEach(t => response.Add(new DropdownDisplayItem() { Id = t.Id, Name = t.Address == Resource.lblVarious ? $"{t.Name} || {t.Code}" : $"{t.Name} || {t.Address}, {t.City}, {t.Code} {t.ZipCode}" }));
            return response;
        }

        public async Task<OrderBlendedGroupViewModel> GetBlendedGroupDetailsAsync(int groupId)
        {
            OrderBlendedGroupViewModel response = new OrderBlendedGroupViewModel();
            var group = await Context.DataContext.OrderGroups.Where(t => t.Id == groupId && t.IsActive).Select(t => new
            {
                t.Id,
                t.GroupType,
                t.JobId,
                t.SupplierCompanyId,
                t.BuyerCompanyId,
                t.CreatedBy,
                OrderGroupXOrders = t.OrderGroupXOrders.Where(t1 => t1.IsActive).Select(t1 => new
                {
                    t1.BlendPercentage,
                    t1.OrderGroupId,
                    t1.OrderId,
                    t1.Order.FuelRequest.MstProduct.DisplayName,
                    t1.Order.FuelRequest.MstProduct.TfxProductId,
                    t1.GroupPoNumber
                }).ToList()
            }).FirstOrDefaultAsync();

            if (group != null)
            {
                response.Jobs = await GetJobsForCustomer(group.SupplierCompanyId, group.BuyerCompanyId);
                response.FuelTypes = await GetFilteredFuelProductsAsync(group.SupplierCompanyId, group.BuyerCompanyId, group.JobId);
                int[] fuelTypeIds = group.OrderGroupXOrders.Where(t => t.TfxProductId.HasValue).Select(t => t.TfxProductId.Value).ToArray();
                response.FilteredOrders = await GetFilteredOrdersAsync(group.SupplierCompanyId, group.BuyerCompanyId, group.JobId, fuelTypeIds, group.Id);

                response.GroupDetails.BuyerCompanyId = group.BuyerCompanyId;
                response.GroupDetails.GroupType = (OrderGroupType)group.GroupType;
                response.GroupDetails.Id = group.Id;
                response.GroupDetails.JobId = group.JobId;
                response.GroupDetails.SupplierCompanyId = group.SupplierCompanyId;
                response.GroupDetails.OrderList = new List<OrderGroupXOrderDetails>();
                foreach (var item in group.OrderGroupXOrders)
                {
                    var orderGroup = new OrderGroupXOrderDetails();
                    orderGroup.BlendPercentage = item.BlendPercentage;
                    orderGroup.GroupId = item.OrderGroupId;
                    orderGroup.OrderId = item.OrderId;
                    orderGroup.Order = new OrderGroupDetailViewModel { FuelType = item.DisplayName };
                    response.GroupDetails.OrderList.Add(orderGroup);
                    response.GroupDetails.GroupPoNumber = item.GroupPoNumber;
                    response.FilteredOrders.Where(t => t.OrderId == item.OrderId).ToList().ForEach(t =>
                    {
                        t.BlendPercentage = item.BlendPercentage;
                        t.GroupId = item.OrderGroupId;
                    });
                }
            }
            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetJobsForCustomer(int buyerCompanyId, int fuelGroupType, int supplierCompanyId)
        {
            List<DropdownDisplayItem> response = new List<DropdownDisplayItem>();
            var jobs = await Context.DataContext.Orders.Where(t => t.BuyerCompanyId == buyerCompanyId && t.AcceptedCompanyId == supplierCompanyId && t.FuelRequest.Job.IsActive && (t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open
                                                                        || (t.FuelRequest.FuelRequestDetail.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery &&
                                                                        t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Closed && !t.Invoices.Any()))
                                                                         && ((fuelGroupType == (int)ProductCategory.Gasoline && 
                                                                         (t.FuelRequest.MstProduct.ProductTypeId == (int)ProductTypes.Unleaded || t.FuelRequest.MstProduct.ProductTypeId == (int)ProductTypes.ConventionalGas ||
                                                                         t.FuelRequest.MstProduct.ProductTypeId == (int)ProductTypes.MidgradeGas || t.FuelRequest.MstProduct.ProductTypeId == (int)ProductTypes.PremiumGas || 
                                                                         t.FuelRequest.MstProduct.ProductTypeId == (int)ProductTypes.RegularGas ||  t.FuelRequest.MstProduct.ProductTypeId == (int)ProductTypes.OtherGas)) ||
                                                                         (fuelGroupType == (int)ProductCategory.Diesel && (t.FuelRequest.MstProduct.ProductTypeId == (int)ProductTypes.ClearDiesel ||
                                                                         t.FuelRequest.MstProduct.ProductTypeId == (int)ProductTypes.RedDyeDiesel2
                                                                         || t.FuelRequest.MstProduct.ProductTypeId == (int)ProductTypes.ClearDiesel2 || t.FuelRequest.MstProduct.ProductTypeId == (int)ProductTypes.RedDyeDiesel
                                                            )))).Select(t => t.FuelRequest.Job)
                                                            .GroupBy(t => t.Id)
                                                            .Select(t => t.FirstOrDefault())
                                                            .OrderByDescending(t => t.Id)
                                                            .Select(t => new { Id = t.Id, Name = t.Name, t.Address, t.City, t.MstState.Code, t.ZipCode })
                                                            .ToListAsync();
            jobs.ForEach(t => response.Add(new DropdownDisplayItem() { Id = t.Id, Name = t.Address == Resource.lblVarious ? $"{t.Name} || {t.Code}" : $"{t.Name} | {t.Address}, {t.City}, {t.Code} {t.ZipCode}" }));
            return response;
        }

        public async Task<List<OrderGroupDetailViewModel>> GetOrdersForTierGroup(int buyerCompanyId, int fuelGroupType, int jobId, int supplierCompanyId)
        {
            List<OrderGroupDetailViewModel> response = new List<OrderGroupDetailViewModel>();
            var orders = await Context.DataContext.Orders.Where(t => t.BuyerCompanyId == buyerCompanyId && t.AcceptedCompanyId == supplierCompanyId && t.IsActive
                                                                        && !t.OrderGroupXOrders.Any(t1 => t1.IsActive && t1.OrderGroup.IsActive && !t1.OrderGroup.IsDeleted)
                                                                        && t.FuelRequest.JobId == jobId && (t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open
                                                                        || (t.FuelRequest.FuelRequestDetail.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery &&
                                                                        t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Closed && !t.Invoices.Any())))
                                                            .GroupBy(t => t.Id)
                                                            .Select(t => t.FirstOrDefault())
                                                            .OrderByDescending(t => t.Id)
                                                            .Select(t => new { Id = t.Id, PoNumber = t.PoNumber, ProductTypeId = t.FuelRequest.MstProduct.ProductTypeId, FuelType = t.FuelRequest.MstProduct.DisplayName ?? t.FuelRequest.MstProduct.Name, t.FuelRequest.MinQuantity, t.FuelRequest.MaxQuantity, t.FuelRequest.UoM, t.FuelRequest.QuantityTypeId, PricePerGallon = t.FuelRequest.FuelRequestPricingDetail.DisplayPrice, Currency = t.FuelRequest.Currency })
                                                            .ToListAsync();
            orders = orders.Where(t => (int)t.ProductTypeId.GetProductCategoryType() == fuelGroupType).ToList();
            orders.ForEach(t => response.Add(new OrderGroupDetailViewModel() { OrderId = t.Id, TfxPoNumber = t.PoNumber, FuelType = t.FuelType, Quantity = GetFuelRequestQuantity(t.QuantityTypeId, t.UoM, t.MinQuantity, t.MaxQuantity), DisplayPrice = $"{t.PricePerGallon} {t.Currency.ToString()}" }));
            return response;
        }

        public async Task<StatusViewModel> CreateGroup(OrderGroupViewModel groupViewModel, UserContext userContext)
        {
            var response = new StatusViewModel();
            if (groupViewModel.OrderList.Any() && groupViewModel.OrderList.Count > 1)
            {
                if (string.IsNullOrWhiteSpace(groupViewModel.GroupPoNumber) || !CheckForDuplicateGroupPoNumber(groupViewModel, userContext))
                {
                    using (var transaction = Context.DataContext.Database.BeginTransaction())
                    {
                        try
                        {
                            var group = groupViewModel.ToEntity(userContext);
                            Context.DataContext.OrderGroups.Add(group);
                            await Context.CommitAsync();
                            if (string.IsNullOrWhiteSpace(groupViewModel.GroupPoNumber))
                            {
                                await SetGroupNumber(groupViewModel, userContext, group);
                            }
                            transaction.Commit();
                            groupViewModel.Id = group.Id;

                            // update minvolume and maxvolume of fuelrequest for tier
                            await UpdateFuelRequestQuantityForTier(groupViewModel);

                            response.StatusCode = Utilities.Status.Success;
                            response.StatusMessage = Resource.successMessageOrderGroupCreated;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            response.StatusMessage = Resource.errMessageOrderGroupCreationFailed;
                            LogManager.Logger.WriteException("OrderGroupDomain", "CreateGroup", ex.Message, ex);
                        }
                    }
                }
                else
                    response.StatusMessage = Resource.errMessageGroupPoNumberAlreadyExist;
            }
            else
                response.StatusMessage = Resource.errMessageOrderGroupAtLeast2OrdersRequired;

            return response;
        }

        private async Task SetGroupNumber(OrderGroupViewModel groupViewModel, UserContext userContext, OrderGroup group)
        {
            groupViewModel.GroupPoNumber = ApplicationConstants.GroupNumberPrefix + group.Id.ToString().PadLeft(7, '0');
            bool isValidGroupNumber = !CheckForDuplicateGroupPoNumber(groupViewModel, userContext);
            if (!isValidGroupNumber)
            {
                groupViewModel.GroupPoNumber = OrderGroupViewModel.GetCustomGroupNumber(groupViewModel.GroupPoNumber);
            }
            group.OrderGroupXOrders.ToList().ForEach(t => t.GroupPoNumber = groupViewModel.GroupPoNumber);
            await Context.CommitAsync();
        }

        private async Task UpdateFuelRequestQuantityForTier(OrderGroupViewModel groupViewModel)
        {
            try
            {
                if (groupViewModel != null && groupViewModel.OrderList.Any() && groupViewModel.GroupType == OrderGroupType.Tier)
                {
                    foreach (var orderModel in groupViewModel.OrderList)
                    {
                        var order = await Context.DataContext.Orders.FirstOrDefaultAsync(t => t.IsActive && t.Id == orderModel.OrderId);
                        if (order != null)
                        {
                            order.FuelRequest.QuantityTypeId = (int)QuantityType.NotSpecified;
                            order.FuelRequest.MinQuantity = 0;
                            order.FuelRequest.MaxQuantity = ApplicationConstants.QuantityNotSpecified;

                            Context.DataContext.Entry(order).State = EntityState.Modified;
                            await Context.CommitAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderGroupDomain", "UpdateQuantityForTierGroup", ex.Message, ex);
            }
        }

        private bool CheckForDuplicateGroupPoNumber(OrderGroupViewModel groupViewModel, UserContext userContext)
        {
            return Context.DataContext.OrderGroups.Any(t => t.Id != groupViewModel.Id
                                    && t.IsActive
                                    && (userContext.IsBuyerCompany ? t.BuyerCompanyId == userContext.CompanyId : t.SupplierCompanyId == userContext.CompanyId)
                                    && t.OrderGroupXOrders.Any(t1 => t1.GroupPoNumber.ToLower().Equals(groupViewModel.GroupPoNumber.ToLower())));
        }


        public async Task<StatusViewModel> DeleteOrderGroup(int groupId, UserContext userContext)
        {
            var response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var group = await Context.DataContext.OrderGroups.Where(t => t.Id == groupId).SingleOrDefaultAsync();
                    if (group != null)
                    {
                        group.IsDeleted = true;
                        group.IsActive = false;
                        group.UpdatedBy = userContext.Id;
                        group.UpdatedDate = DateTimeOffset.Now;
                        group.OrderGroupXOrders.ToList().ForEach(t => t.IsActive = false);
                        Context.DataContext.Entry(group).State = EntityState.Modified;
                        await Context.CommitAsync();
                        transaction.Commit();

                        response.StatusCode = Utilities.Status.Success;
                        response.StatusMessage = Resource.successMessageOrderGroupDeleted;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.StatusMessage = Resource.errMessageOrderGroupEditingFailed;
                    LogManager.Logger.WriteException("OrderGroupDomain", "DeleteOrderGroup", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusViewModel> InactivateGroup(int groupId, UserContext userContext)
        {
            var response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var group = await Context.DataContext.OrderGroups.Where(t => t.Id == groupId).SingleOrDefaultAsync();
                    if (group != null)
                    {
                        group.IsActive = false;
                        group.UpdatedBy = userContext.Id;
                        group.UpdatedDate = DateTimeOffset.Now;
                        Context.DataContext.Entry(group).State = EntityState.Modified;
                        await Context.CommitAsync();
                        transaction.Commit();

                        response.StatusCode = Utilities.Status.Success;
                        response.StatusMessage = Resource.successMessageOrderGroupUpdated;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.StatusMessage = Resource.errMessageOrderGroupEditingFailed;
                    LogManager.Logger.WriteException("OrderGroupDomain", "InactivateGroup", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusViewModel> EditGroup(OrderGroupViewModel groupViewModel, UserContext userContext)
        {
            var response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(groupViewModel.GroupPoNumber) || !CheckForDuplicateGroupPoNumber(groupViewModel, userContext))
                    {
                        var group = await Context.DataContext.OrderGroups.Where(t => t.Id == groupViewModel.Id).SingleOrDefaultAsync();
                        if (group != null)
                        {
                            AddOrInactivateOrdersForGroup(groupViewModel, group);

                            if (group.OrderGroupXOrders.Count(t => t.IsActive) > 1)
                            {
                                group.UpdatedBy = userContext.Id;
                                group.UpdatedDate = DateTimeOffset.Now;
                                Context.DataContext.Entry(group).State = EntityState.Modified;
                                await Context.CommitAsync();

                                if (string.IsNullOrWhiteSpace(groupViewModel.GroupPoNumber))
                                {
                                    await SetGroupNumber(groupViewModel, userContext, group);
                                }

                                transaction.Commit();

                                response.StatusCode = Utilities.Status.Success;
                                response.StatusMessage = Resource.successMessageOrderGroupUpdated;
                            }
                            else
                            {
                                response.StatusMessage = Resource.errMessageOrderGroupAtLeast2OrdersRequired;
                            }
                        }
                    }
                    else
                        response.StatusMessage = Resource.errMessageGroupPoNumberAlreadyExist;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.StatusMessage = Resource.errMessageOrderGroupEditingFailed;
                    LogManager.Logger.WriteException("OrderGroupDomain", "EditGroup", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusViewModel> SaveBlendGroupAsync(UserContext userContext, OrderGroupDetailViewModel[] viewModel)
        {
            var response = new StatusViewModel();
            try
            {
                if (viewModel != null)
                {
                    var requestModel = new OrderGroupViewModel()
                    {
                        SupplierCompanyId = userContext.CompanyId,
                        GroupType = OrderGroupType.Blend,
                        OrderList = new List<OrderGroupXOrderDetails>()
                    };
                    foreach (var item in viewModel)
                    {
                        requestModel.OrderList.Add(new OrderGroupXOrderDetails { OrderId = item.OrderId, BlendPercentage = item.BlendPercentage });
                    }
                    response = await CreateGroup(requestModel, userContext);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderGroupDomain", "GetFilteredOrdersAsync", ex.Message, ex);
            }
            return response;
        }

        private static void AddOrInactivateOrdersForGroup(OrderGroupViewModel groupViewModel, OrderGroup group)
        {
            var existingOrderListForGroup = group.OrderGroupXOrders.ToList();
            var existingOrderIds = existingOrderListForGroup.Select(t => t.OrderId).ToList();

            var updatedOrderListForGroup = groupViewModel.OrderList.ToList();
            var updatedOrderIds = updatedOrderListForGroup.Select(t => t.OrderId).ToList();

            var needToAdd = updatedOrderIds.Except(existingOrderIds);
            var needToInactivate = existingOrderIds.Except(updatedOrderIds);

            //update order details
            foreach (var item in group.OrderGroupXOrders)
            {
                var updatedOrderDetails = groupViewModel.OrderList.FirstOrDefault(t => t.OrderId == item.OrderId);
                if (updatedOrderDetails != null)
                {
                    item.GroupPoNumber = groupViewModel.GroupPoNumber;
                    item.BlendPercentage = updatedOrderDetails.BlendPercentage;
                    item.MinVolume = updatedOrderDetails.MinVolume;
                    item.MaxVolume = updatedOrderDetails.MaxVolume;
                }
            }

            foreach (var item in needToAdd)
            {
                var addNewOrderToGroup = groupViewModel.OrderList.Where(t => t.OrderId == item).SingleOrDefault();
                if (addNewOrderToGroup != null)
                    group.OrderGroupXOrders.Add(addNewOrderToGroup.ToEntity(groupViewModel.GroupPoNumber));
            }

            foreach (var item in needToInactivate)
            {
                group.OrderGroupXOrders.SingleOrDefault(t => t.OrderId == item).IsActive = false;
            }
        }


        public async Task<List<OrderGroupDetailViewModel>> GetFilteredOrdersAsync(int SupplierCompanyId, int customerId, int jobId, int[] tfxProductIds, int groupId = 0)
        {
            var response = new List<OrderGroupDetailViewModel>();
            try
            {
                var fueltypFueltypes = tfxProductIds == null ? new List<int>() : new List<int>(tfxProductIds);
                var responseData = await Context.DataContext.Orders.Where(t => t.BuyerCompanyId == customerId && t.AcceptedCompanyId == SupplierCompanyId && t.FuelRequest.JobId == jobId
                                                                && (!fueltypFueltypes.Any() || fueltypFueltypes.Contains(t.FuelRequest.MstProduct.TfxProductId ?? 0))
                                                                && (!t.OrderGroupXOrders.Any(t1 => t1.IsActive && t1.OrderGroupId != groupId))
                                                                && (t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open
                                                                        || (t.FuelRequest.FuelRequestDetail.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery && t.Company.IsActive &&
                                                                        t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Closed && !t.Invoices.Any()))
                                                                && t.IsActive)
                                                       .Select(t => new
                                                       {
                                                           Id = t.Id,
                                                           DisplayPrice = t.FuelRequest.FuelRequestPricingDetail.DisplayPrice,
                                                           Currency = t.FuelRequest.Currency,
                                                           FuelType = t.FuelRequest.MstProduct.DisplayName,
                                                           QuantityTypeId = t.FuelRequest.QuantityTypeId,
                                                           TfxPoNumber = t.PoNumber,
                                                           UoM = t.FuelRequest.UoM,
                                                           t.FuelRequest.MaxQuantity,
                                                           t.FuelRequest.MinQuantity
                                                       }).ToListAsync();
                foreach (var item in responseData)
                {
                    response.Add(new OrderGroupDetailViewModel
                    {
                        OrderId = item.Id,
                        DisplayPrice = item.DisplayPrice + item.Currency.GetDisplayName(),
                        FuelType = item.FuelType,
                        Quantity = GetFuelRequestQuantity(item.QuantityTypeId, item.UoM, item.MinQuantity, item.MaxQuantity),
                        TfxPoNumber = item.TfxPoNumber
                    });
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderGroupDomain", "GetFilteredOrdersAsync", ex.Message, ex);
            }
            return response;
        }


        public async Task<List<DropdownDisplayItem>> GetFilteredFuelProductsAsync(int SupplierCompanyId, int customerId, int jobId)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = await Context.DataContext.Orders.Where(t => t.BuyerCompanyId == customerId && t.AcceptedCompanyId == SupplierCompanyId && t.FuelRequest.JobId == jobId && t.FuelRequest.MstProduct.TfxProductId.HasValue && t.IsActive)
                                                       .Select(t => new DropdownDisplayItem
                                                       {
                                                           Id = t.FuelRequest.MstProduct.TfxProductId ?? 0,
                                                           Name = t.FuelRequest.MstProduct.DisplayName
                                                       })
                                                       .Distinct()
                                                       .ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderGroupDomain", "GetFilteredFuelProductsAsync", ex.Message, ex);
            }
            return response;
        }

        private string GetFuelRequestQuantity(int quantityTypeId, UoM uom, decimal minQuantity, decimal maxQuantity)
        {
            switch (quantityTypeId)
            {
                case (int)QuantityType.NotSpecified:
                    return Resource.lblNotSpecified;
                case (int)QuantityType.Range:
                    return $"{minQuantity.GetPreciseValue(6)} - {maxQuantity.GetPreciseValue(6)} {uom.ToString()}";
                default:
                    return $"{maxQuantity.GetPreciseValue(6)} {uom.ToString()}";
            }
        }


        public async Task<OrderGroupDDLViewModel> FillOrderGroupDdlAsync(UserContext userContext, int groupTypeId)
        {
            var response = new OrderGroupDDLViewModel();
            try
            {
                response.IsBuyerCompany = userContext.IsBuyerCompany;
                response.IsSupplierCompany = userContext.IsSupplierCompany;

                //get group types
                response.GroupTypes = EnumHelperMethods.EnumToList<OrderGroupType>();

                if (userContext.IsBuyerCompany)
                {
                    //if (groupTypeId == (int)OrderGroupType.MultiProducts || groupTypeId == (int)OrderGroupType.Blend)
                    response.Companies = await GetSuppliersForCustomer(userContext.CompanyId);
                }
                else if (userContext.IsSupplierCompany)
                {
                    response.Companies = await GetCustomersForSupplier(userContext.CompanyId);
                }

                if (groupTypeId == (int)OrderGroupType.Tier)
                    response.ProductCategories = EnumHelperMethods.EnumToList<ProductCategory>();

                if (userContext.IsBuyerCompany && groupTypeId == (int)OrderGroupType.Tier)
                {
                    response.States = Context.DataContext.MstStates.Where(t => t.IsActive)
                                                            .Select(t1 => new DropdownDisplayExtendedItem
                                                            {
                                                                Id = t1.Id,
                                                                Code = t1.Code,
                                                                Name = t1.Name
                                                            }).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderGroupDomain", "FillOrderGroupDdlAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<ViewOrderGroupViewModel> ViewOrderGroupsAsync(UserContext userContext, OrderGroupSearchModel filterModel)
        {
            ViewOrderGroupViewModel response = new ViewOrderGroupViewModel();
            var spDomain = new StoredProcedureDomain(this);
            try
            {
                var orderGroups = new List<UspGetGroupOrders>();
                if (userContext.IsSupplierCompany)
                    orderGroups = await spDomain.GetSupplierOrderGroups(userContext.CompanyId, filterModel.SearchText, filterModel.JobId, filterModel.CompanyId, filterModel.GroupTypeId, filterModel.ProductCategoryId, filterModel.StateId);
                else
                    orderGroups = await spDomain.GetBuyerOrderGroups(userContext.CompanyId, filterModel.SearchText, filterModel.JobId, filterModel.CompanyId, filterModel.GroupTypeId, filterModel.ProductCategoryId, filterModel.StateId);

                List<int> orderGroupIds = orderGroups.Select(t => t.OrderGroupId).Distinct().ToList();
                foreach (var orderGroupId in orderGroupIds)
                {
                    GetMultiProductOrderGroups(response, orderGroups, orderGroupId);
                    GetTierOrderGroups(response, orderGroups, orderGroupId);
                    GetBlendOrderGroups(response, orderGroups, orderGroupId);
                }

                //search order group
                if(!string.IsNullOrWhiteSpace(filterModel.SearchText) && response.OrderGroupDetails != null && response.OrderGroupDetails.Any())
                {
                    var search = filterModel.SearchText.ToLower();
                    var filteredGroups = response.OrderGroupDetails
                                                .Where(t => t.CustomerPoNumber != null && t.CustomerPoNumber.ToLower().Contains(search) ||
                                                    t.GroupType.ToString().ToLower().Contains(search) ||
                                                    t.ProductType.ToString().ToLower().Contains(search) ||
                                                    t.FuelType != null && t.FuelType.ToLower().Contains(search) ||
                                                    t.JobName != null && t.JobName.ToLower().Contains(search) ||
                                                    t.JobAddress != null && t.JobAddress.ToLower().Contains(search) ||
                                                    t.RenewalFrequency != null && t.RenewalFrequency.ToLower().Contains(search) ||
                                                    t.DisplayBlendedGroupWeightedPPG != null && t.DisplayBlendedGroupWeightedPPG.ToLower().Contains(search) ||
                                                    (t.OrderDrops != null && 
                                                        t.OrderDrops.Any(t1 => t1.FuelType != null && t1.FuelType.ToLower().Contains(search) ||
                                                                                t1.MaxVolume.ToString().Contains(search) ||
                                                                                t1.MinVolume.ToString().Contains(search) ||
                                                                                t1.TfxPoNumber != null && t1.TfxPoNumber.ToLower().Contains(search) ||
                                                                                t1.DroppedGallons != null && t1.DroppedGallons.ToLower().Contains(search) ||
                                                                                t1.DisplayPPG != null && t1.DisplayPPG.ToLower().Contains(search)
                                                        )
                                                    )
                                                ).ToList();
                    response.OrderGroupDetails = filteredGroups;
                }

                // check for edit or delete group allowed
                if (response.OrderGroupDetails != null && response.OrderGroupDetails.Any())
                {
                    response.TotalGroupCount = response.OrderGroupDetails.Count;
                    response.ShowCount = response.OrderGroupDetails.Count;

                    if (response.TotalGroupCount > ApplicationConstants.OrderGroupsDefaultShowCount)
                    {
                        response.OrderGroupDetails = response.OrderGroupDetails.OrderByDescending(t => t.OrderGroupId).Take(ApplicationConstants.OrderGroupsDefaultShowCount).ToList();
                        response.ShowCount = ApplicationConstants.OrderGroupsDefaultShowCount;
                    }

                    foreach (var group in response.OrderGroupDetails)
                    {
                        var isEditAndDeleteAllowed = !group.OrderDrops.Any(t => t.DropPercentage != Resource.lblHyphen && t.FuelDeliveredPercentage > 0);
                        group.IsEditOrDeleteAllowed = isEditAndDeleteAllowed;

                        var groupCreatedByUserCompanyId = await Context.DataContext.Users.Where(t => t.IsActive && t.Id == group.CreatedBy)
                                                                    .Select(t1 => t1.CompanyId)
                                                                    .FirstOrDefaultAsync();
                        if (groupCreatedByUserCompanyId.HasValue && groupCreatedByUserCompanyId.Value == userContext.CompanyId)
                            group.CanCurrentUserEditOrDeleteGroup = true;

                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "ViewOrderGroupsAsync", ex.Message, ex);
            }

            return response;
        }

        private void GetBlendOrderGroups(ViewOrderGroupViewModel response, List<UspGetGroupOrders> orderGroups, int orderGroupId)
        {
            var blendGroups = orderGroups.Where(t => t.GroupType == OrderGroupType.Blend && t.OrderGroupId == orderGroupId)
                                                            .GroupBy(t => new { t.OrderGroupId, t.CustomerPoNumber, t.GroupType, t.CreatedBy })
                                                            .Select(t1 => new OrderGroupDetailModel() { OrderGroupId = t1.Key.OrderGroupId, CustomerPoNumber = t1.Key.CustomerPoNumber, GroupType = t1.Key.GroupType, DisplayGroupType = t1.Key.GroupType.GetDisplayName(), CreatedBy = t1.Key.CreatedBy }).Distinct().ToList();
            if (blendGroups != null && blendGroups.Any())
            {
                var helperDomain = new HelperDomain(this);
                decimal pricePerGallon = 0;
                StringBuilder displayPPG = new StringBuilder();
                foreach (var orderDrop in blendGroups)
                {
                    var drops = orderGroups.Where(t => t.OrderGroupId == orderDrop.OrderGroupId)
                                           .Select(t1 => new OrderDropDetailViewModel()
                                           {
                                               FuelType = t1.FuelType,
                                               DroppedGallons = helperDomain.GetQuantityRequested(t1.Quantity),
                                               UoM = t1.UoM.ToString(),
                                               TfxPoNumber = t1.TfxPoNumber,
                                               FuelDeliveredPercentage = t1.FuelDeliveredPercentage,
                                               DropPercentage = helperDomain.CheckQuantityValid(t1.Quantity, t1.FuelDeliveredPercentage),
                                               QuantityType = t1.QuantityType,
                                               MinVolume = t1.MinVolume,
                                               MaxVolume = t1.MaxVolume,
                                               BlendRatioPercentage = t1.BlendPercentage,
                                               DisplayPPG = t1.PricePerGallon,
                                           }).ToList();

                    if (drops != null && drops.Any())
                    {
                        var ppgList = drops.Select(t => t.DisplayPPG).ToList();
                        foreach (var ppg in ppgList)
                        {
                            //decimal price = 0;
                            //var item = ppg.Replace("$", "").Replace("Fuel Cost + ", "").Replace("Rack Avg + ", "");
                            //decimal.TryParse(item, out price);
                            //pricePerGallon += price;
                            if (displayPPG.ToString() == string.Empty)
                            {
                                displayPPG = new StringBuilder();
                                displayPPG.Append("(" + ppg);
                            }
                            else
                                displayPPG.Append("+" + ppg);
                        }
                        displayPPG.Append(")");


                        if (orderDrop.OrderDrops == null)
                            orderDrop.OrderDrops = new List<OrderDropDetailViewModel>();
                        orderDrop.OrderDrops.AddRange(drops);
                    }
                }

                // update ppg
                foreach (var item in blendGroups)
                {
                    item.DisplayBlendedGroupWeightedPPG = displayPPG.ToString();
                    item.BlendedGroupWeightedPPG = pricePerGallon;
                }

                if (response.OrderGroupDetails == null)
                    response.OrderGroupDetails = new List<OrderGroupDetailModel>();
                response.OrderGroupDetails.AddRange(blendGroups);
            }
        }

        private void GetTierOrderGroups(ViewOrderGroupViewModel response, List<UspGetGroupOrders> orderGroups, int orderGroupId)
        {
            var tierOrderGroups = orderGroups.Where(t => t.GroupType == OrderGroupType.Tier && t.OrderGroupId == orderGroupId)
                                                                    .GroupBy(t => new { t.OrderGroupId, t.CustomerPoNumber, t.ProductType, t.RenewalFrequency, t.GroupType, t.CreatedBy })
                                                                    .Select(t1 => new OrderGroupDetailModel()
                                                                    {
                                                                        OrderGroupId = t1.Key.OrderGroupId,
                                                                        CustomerPoNumber = t1.Key.CustomerPoNumber,
                                                                        DisplayProductType = t1.Key.ProductType.ToString(),
                                                                        ProductType = t1.Key.ProductType,
                                                                        RenewalFrequency = (int)t1.Key.RenewalFrequency == (int)OrderGroupFrequency.None ? "" : t1.Key.RenewalFrequency.ToString(),
                                                                        GroupType = t1.Key.GroupType,
                                                                        DisplayGroupType = t1.Key.GroupType.GetDisplayName(),
                                                                        CreatedBy = t1.Key.CreatedBy
                                                                    }).Distinct().ToList();

            if (tierOrderGroups != null && tierOrderGroups.Any())
            {
                var helperDomain = new HelperDomain(this);
                foreach (var orderDrop in tierOrderGroups)
                {
                    var drops = orderGroups.Where(t => t.OrderGroupId == orderDrop.OrderGroupId)
                                           .Select(t1 => new OrderDropDetailViewModel()
                                           {
                                               DroppedGallons = helperDomain.GetQuantityRequested(t1.Quantity),
                                               UoM = t1.UoM.ToString(),
                                               TfxPoNumber = t1.TfxPoNumber,
                                               FuelDeliveredPercentage = t1.FuelDeliveredPercentage,
                                               DropPercentage = helperDomain.CheckQuantityValid(t1.Quantity, t1.FuelDeliveredPercentage),
                                               QuantityType = t1.QuantityType,
                                               MinVolume = t1.MinVolume,
                                               MaxVolume = t1.MaxVolume,
                                               DisplayPPG = t1.PricePerGallon,
                                           }).OrderBy(o => o.MinVolume).ToList();

                    if (orderDrop.OrderDrops == null)
                        orderDrop.OrderDrops = new List<OrderDropDetailViewModel>();

                    orderDrop.OrderDrops.AddRange(drops);
                }

                if (response.OrderGroupDetails == null)
                    response.OrderGroupDetails = new List<OrderGroupDetailModel>();

                response.OrderGroupDetails.AddRange(tierOrderGroups);
            }
        }

        private void GetMultiProductOrderGroups(ViewOrderGroupViewModel response, List<UspGetGroupOrders> orderGroups, int orderGroupId)
        {
            var multiproductGroups = orderGroups.Where(t => t.GroupType == OrderGroupType.MultiProducts && t.OrderGroupId == orderGroupId)
                                                                    .GroupBy(t => new { t.OrderGroupId, t.CustomerPoNumber, t.JobName, t.JobAddress, t.GroupType, t.CreatedBy })
                                                                    .Select(t1 => new OrderGroupDetailModel() { OrderGroupId = t1.Key.OrderGroupId, CustomerPoNumber = t1.Key.CustomerPoNumber, JobName = t1.Key.JobName, JobAddress = t1.Key.JobAddress, GroupType = t1.Key.GroupType, DisplayGroupType = t1.Key.GroupType.GetDisplayName(), CreatedBy = t1.Key.CreatedBy }).Distinct().ToList();
            if (multiproductGroups != null && multiproductGroups.Any())
            {
                var helperDomain = new HelperDomain(this);
                foreach (var orderDrop in multiproductGroups)
                {
                    var drops = orderGroups.Where(t => t.OrderGroupId == orderDrop.OrderGroupId)
                                           .Select(t1 => new OrderDropDetailViewModel()
                                           {
                                               FuelType = t1.FuelType,
                                               DroppedGallons = helperDomain.GetQuantityRequested(t1.Quantity),
                                               UoM = t1.UoM.ToString(),
                                               TfxPoNumber = t1.TfxPoNumber,
                                               FuelDeliveredPercentage = t1.FuelDeliveredPercentage,
                                               DropPercentage = helperDomain.CheckQuantityValid(t1.Quantity, t1.FuelDeliveredPercentage),
                                               QuantityType = t1.QuantityType,
                                               MinVolume = t1.MinVolume,
                                               MaxVolume = t1.MaxVolume,
                                           }).ToList();

                    if (orderDrop.OrderDrops == null)
                        orderDrop.OrderDrops = new List<OrderDropDetailViewModel>();
                    orderDrop.OrderDrops.AddRange(drops);
                }

                if (response.OrderGroupDetails == null)
                    response.OrderGroupDetails = new List<OrderGroupDetailModel>();
                response.OrderGroupDetails.AddRange(multiproductGroups);
            }
        }
    }
}
