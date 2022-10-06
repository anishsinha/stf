using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Infrastructure;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Domain.Mappers.TankRental;
using SiteFuel.Exchange.Domain.Services;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Invoice.Pdf;
using SiteFuel.Exchange.ViewModels.MobileAPI;
using SiteFuel.Exchange.ViewModels.MobileAPI.TPD;
using SiteFuel.Exchange.ViewModels.Queue;
using SiteFuel.Exchange.ViewModels.ThirdPartyOrder;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace SiteFuel.Exchange.Domain
{
    public class OrderDomain : FuelRequestDomain
    {
        public OrderDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public OrderDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task<List<OrderGridViewModel>> GetBuyerOrdersAsync(int userId, int companyId, OrderDataTableViewModel orderFilter, int BrandedCompanyId)
        {
            List<OrderGridViewModel> response = new List<OrderGridViewModel>();
            var helperDomain = new HelperDomain(this);

            try
            {
                var orders = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetBuyerOrders(companyId, userId, orderFilter, BrandedCompanyId);
                foreach (var order in orders)
                {
                    var data = new OrderGridViewModel();
                    data.AssetsAssigned = order.AssetsAssigned;
                    data.Eligibility = order.Eligibility;
                    data.FuelDeliveredPercentage = helperDomain.CheckQuantityValid(order.Quantity, order.FuelDeliveredPercentage);
                    data.FuelType = order.FuelType;
                    data.Id = order.Id;
                    data.PoNumber = order.PoNumber;
                    //data.TotalAmount = helperDomain.CheckQuantityValid(order.Quantity, order.TotalAmount);
                    if (order.Quantity != ApplicationConstants.QuantityNotSpecified)
                    {
                        data.Quantity = helperDomain.GetQuantityRequested(order.Quantity) + " " + helperDomain.GetUOM(order.UOM);
                    }
                    else
                    {
                        data.Quantity = helperDomain.GetQuantityRequested(order.Quantity);
                    }
                    data.PricePerGallon = order.PricePerGallon;
                    data.Status = order.Status;
                    data.Supplier = order.Supplier;
                    data.TotalCount = order.TotalCount;
                    data.DeliveryType = order.DeliveryType;
                    data.OrderType = order.OrderType;
                    data.OrderGroupId = order.OrderGroupId;
                    data.GroupPoNumber = string.IsNullOrWhiteSpace(order.GroupPoNumber) ? Resource.lblHyphen : order.GroupPoNumber;
                    data.VesselName = order.VesselName;
                    response.Add(data);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetBuyerOrdersAsync", ex.Message, ex);
            }

            return response;
        }
        public async Task<DecimalResponseModel> GetGallonsPerMetricTonAsync(decimal gravity)
        {
            var response = new DecimalResponseModel();
            try
            {
                var gallonsPerMetricTon = await Context.DataContext.MstGravityConversions
                                                                    .Where(t => t.Gravity == gravity)
                                                                    .Select(t => t.GallonsPerMetricTon)
                                                                    .FirstOrDefaultAsync();
                if (gallonsPerMetricTon > 0)
                {
                    response.Result = gallonsPerMetricTon;
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Status.Success.ToString();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetGallonsPerMetricTonAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<ProductModel> GetTfxProduct(int tfxProductId)
        {
            var response = new ProductModel();
            try
            {
                response = await Context.DataContext.MstTfxProducts
                                                                    .Where(t => t.Id == tfxProductId && t.IsActive)
                                                                    .Select(t => new ProductModel
                                                                    {
                                                                        Id = t.Id,
                                                                        Name = t.Name,
                                                                        DisplayGroupId = t.ProductDisplayGroupId,
                                                                        ProductTypeId = t.ProductTypeId,
                                                                        ProductTypeName = t.MstProductType.Name
                                                                    })
                                                                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "getTfxProduct", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<OrderGridViewModel>> GetBrokerOrders(int companyId, OrderDataTableViewModel requestModel)
        {
            var response = new List<OrderGridViewModel>();
            var helperDomain = new HelperDomain(this);

            try
            {
                var orders = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetBrokerOrders(companyId, requestModel);
                foreach (var order in orders)
                {
                    var data = new OrderGridViewModel();

                    data.Id = order.Id;
                    data.Eligibility = order.Eligibility;
                    data.FuelDeliveredPercentage = helperDomain.CheckQuantityValid(order.Quantity, order.FuelDeliveredPercentage);
                    data.FuelType = order.FuelType;
                    data.PoNumber = order.PoNumber;
                    data.PricePerGallon = order.PricePerGallon;
                    data.Quantity = helperDomain.GetQuantityRequested(order.Quantity);
                    //data.TotalAmount = helperDomain.CheckQuantityValid(order.Quantity, order.TotalAmount);
                    data.Status = order.Status;
                    data.Supplier = order.Supplier;
                    data.CustomerPoNumber = order.CustomerPoNumber ?? Resource.lblHyphen;
                    data.CustomerOrderId = order.CustomerOrderId;
                    data.TotalCount = order.TotalCount;
                    response.Add(data);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetBrokerOrders", ex.Message, ex);
            }
            return response;
        }

        public async Task<bool> IsBrokerOrderExist(int companyId)
        {
            var response = false;
            try
            {
                response = Context.DataContext.Orders.Any(t => t.BuyerCompanyId == companyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetBrokerOrders", ex.Message, ex);
            }
            return response;
        }

        public decimal GetRemaingScheduleQuantity(int orderId)
        {
            decimal response = 0;
            try
            {
                var order = Context.DataContext.Orders.SingleOrDefault(t => t.Id == orderId);
                var allschedules = GetOrderDeliverySchedules(order);
                decimal scheduelQuantity = 0;
                foreach (var schedule in allschedules)
                {
                    scheduelQuantity = scheduelQuantity + schedule.ScheduleQuantity * schedule.ScheduleDays.Count;
                }
                response = order.FuelRequest.MaxQuantity - scheduelQuantity;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetRemaingScheduleQuantity", ex.Message, ex);
            }
            return response;
        }

        public async Task<PickUpAddressViewModel> GetOrderPickUpLocationAsync(int orderId)
        {
            var response = new PickUpAddressViewModel();
            try
            {
                var existingAddress = await Context.DataContext.Orders.Where(t => t.Id == orderId)
                                            .Select(t => new
                                            {
                                                currency = t.FuelRequest.Currency,
                                                pickUpAddress = t.FuelDispatchLocations.FirstOrDefault(t1 => !t1.DeliveryScheduleId.HasValue
                                                                                        && !t1.TrackableScheduleId.HasValue
                                                                                        && t1.LocationType == (int)LocationType.PickUp && t1.IsActive)
                                            })
                                            .FirstOrDefaultAsync();
                //var existingPickUpAddress = await Context.DataContext.FuelDispatchLocations.FirstOrDefaultAsync(t => t.OrderId == orderId && !t.DeliveryScheduleId.HasValue && !t.TrackableScheduleId.HasValue && t.LocationType == (int)LocationType.PickUp && t.IsActive);
                if (existingAddress?.pickUpAddress != null)
                {
                    response = existingAddress.pickUpAddress.ToPickUpAddressViewModel();
                }
                else
                {
                    response.Address = new DispatchAddressViewModel() { State = new StateViewModel(), Country = new CountryViewModel() };
                    response.Currency = existingAddress?.currency ?? Currency.USD;
                    response.OrderId = orderId;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetOrderPickUpLocationAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> ModifyPickUpLocationAsync(UserContext userContext, PickUpAddressViewModel addressViewModel)
        {
            var response = new StatusViewModel();
            try
            {
                if (addressViewModel.Address.State.Name == Resource.lblDummy.ToString())
                {
                    var stateDetails = Context.DataContext.BulkPlantLocations
                                                .Where(t => t.CompanyId == userContext.CompanyId && t.IsActive && t.Name.Trim().ToLower() == addressViewModel.Address.SiteName.Trim().ToLower())
                                                .Select(x =>
                                                new DispatchAddressViewModel
                                                {
                                                    State = new StateViewModel { Id = x.StateId, Code = x.StateCode, Name = x.MstState.Name },
                                                }).FirstOrDefault();
                    if (stateDetails != null && stateDetails.State != null)
                    {
                        addressViewModel.Address.State = stateDetails.State;
                    }
                }
                await InActivePickUpLocationAsync(addressViewModel.OrderId);

                if (addressViewModel.Address.Country.Id == (int)Country.CAR && addressViewModel.Address.IsMissingAddress())
                {
                    var state = Context.DataContext.MstStates.First(t => t.Id == addressViewModel.Address.State.Id).ToViewModel();

                    addressViewModel.Address.Address = string.IsNullOrWhiteSpace(addressViewModel.Address.Address) ? state.Name : addressViewModel.Address.Address;
                    addressViewModel.Address.ZipCode = string.IsNullOrWhiteSpace(addressViewModel.Address.ZipCode) ? state.Name : addressViewModel.Address.ZipCode;
                    addressViewModel.Address.CountyName = string.IsNullOrWhiteSpace(addressViewModel.Address.CountyName) ? state.Name : addressViewModel.Address.CountyName;
                    addressViewModel.Address.City = string.IsNullOrWhiteSpace(addressViewModel.Address.City) ? state.Name : addressViewModel.Address.City;
                    addressViewModel.Address.State.Code = state.Code;
                    addressViewModel.Address.Country.Code = Country.CAR.ToString();
                }

                if (addressViewModel.Address.Latitude == 0 || addressViewModel.Address.Longitude == 0)
                {

                    SetBulkPlantPickupAddress(addressViewModel);
                }

                addressViewModel.CreatedBy = userContext.Id;
                var newPickUpAddress = addressViewModel.ToFuelDispatchLocationEntity();
                Context.DataContext.FuelDispatchLocations.Add(newPickUpAddress);
                await Context.CommitAsync();
                if (newPickUpAddress.Id > 0)
                {
                    var dispatchDomain = new DispatchDomain(this);
                    await dispatchDomain.SaveBulkPlantLocation(newPickUpAddress, userContext.CompanyId);
                }
                response.StatusCode = Status.Success;
            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = "Add/Update PickUp location Failed";
                LogManager.Logger.WriteException("OrderDomain", "ModifyPickUpLocationAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> InActivePickUpLocationAsync(int orderId)
        {
            var response = new StatusViewModel();
            try
            {
                var existingPickUpAddress = await Context.DataContext.FuelDispatchLocations.FirstOrDefaultAsync(t => t.OrderId == orderId && !t.DeliveryScheduleId.HasValue && !t.TrackableScheduleId.HasValue && t.LocationType == (int)LocationType.PickUp && t.IsActive);
                if (existingPickUpAddress != null)
                {
                    existingPickUpAddress.IsActive = false;
                    Context.DataContext.Entry(existingPickUpAddress).State = EntityState.Modified;
                    await Context.CommitAsync();
                }

                response.StatusCode = Status.Success;
            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = "Remove PickUp location Failed";
                LogManager.Logger.WriteException("OrderDomain", "InActivePickUpLocationAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> ChangePickUpLocationAsync(ChangePickupLocationViewModel request)
        {
            var response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    if (request.Orders == null || request.Orders.Count == 0)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = "Please provide Order details";
                    }

                    List<PickLocationOrderDetailResponseModel> pickups = null;
                    foreach (var order in request.Orders)
                    {
                        var existingPickUpAddress = await Context.DataContext.FuelDispatchLocations
                                                                 .FirstOrDefaultAsync(t => t.OrderId == order.OrderId && t.LocationType == (int)LocationType.PickUp && t.IsActive && !t.IsSkipped &&
                                                                                           (t.DeliveryScheduleId == (order.DeliveryScheduleId == 0 ? null : order.DeliveryScheduleId)) &&
                                                                                           (t.TrackableScheduleId == (order.TrackableScheduleId == 0 ? null : order.TrackableScheduleId))
                                                                                     );
                        FuelDispatchLocationViewModel newAddressModel = new FuelDispatchLocationViewModel();
                        if (existingPickUpAddress != null)
                        {
                            existingPickUpAddress.IsActive = false;
                            Context.DataContext.Entry(existingPickUpAddress).State = EntityState.Modified;
                            await Context.CommitAsync();

                            newAddressModel = existingPickUpAddress.ToViewModel(newAddressModel);
                        }
                        else
                        {
                            // save pick location if not exists
                            await SetPickLocationIfNotExists(request, newAddressModel, order);
                        }

                        var newAddress = new FuelDispatchLocation();
                        if (request.TerminalId != null && request.TerminalId.Value > 0)
                        {
                            // for terminal
                            await SetTerminalAddress(request, response, transaction, newAddress, newAddressModel);
                        }
                        else
                        {
                            // for bulk plant
                            newAddressModel.TerminalId = null;
                            newAddressModel.SiteName = string.IsNullOrWhiteSpace(request.BulkplantName) ? newAddressModel.SiteName : request.BulkplantName;
                            await SetBulkplantAddress(request, newAddress);
                        }

                        newAddress.CreatedBy = request.UserId;
                        newAddress = newAddressModel.ToEntity(newAddress);
                        Context.DataContext.FuelDispatchLocations.Add(newAddress);

                        await Context.CommitAsync();

                        if (pickups == null)
                        {
                            pickups = new List<PickLocationOrderDetailResponseModel>();
                        }

                        var obj = new PickLocationOrderDetailResponseModel { PickupLocationId = newAddress.Id, OrderId = order.OrderId, TerminalId = newAddress.TerminalId, Address = newAddress.Address, ZipCode = newAddress.ZipCode, City = newAddress.City, Latitude = newAddress.Latitude, Longitude = newAddress.Longitude };
                        pickups.Add(obj);
                    }

                    response.ResponseData = pickups;
                    response.StatusCode = Status.Success;
                    response.StatusMessage = "Updated successfully";

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = "Update pickUp location failed";
                    LogManager.Logger.WriteException("OrderDomain", "ChangePickUpLocationAsync", ex.Message, ex);
                }
            }
            return response;
        }

        private void SetBulkPlantPickupAddress(PickUpAddressViewModel request)
        {
            var point = GoogleApiDomain.GetGeocode($"{request.Address.Address} {request.Address.City} {request.Address.State.Code}{request.Address.Country.Code}{request.Address.ZipCode}");
            if (point != null)
            {
                request.Address.Latitude = Convert.ToDecimal(point.Latitude);
                request.Address.Longitude = Convert.ToDecimal(point.Longitude);
                request.Address.CountyName = point.CountyName == null ? request.Address.City : point.CountyName;
                request.Address.TimeZoneName = GoogleApiDomain.GetTimeZone(request.Address.Latitude, request.Address.Longitude);
            }
        }

        private async Task SetBulkplantAddress(ChangePickupLocationViewModel request, FuelDispatchLocation newAddress)
        {
            var point = GoogleApiDomain.GetGeocode($"{request.Address} {request.City} {request.StateCode} {request.ZipCode}");
            if (point != null)
            {
                newAddress.Latitude = Convert.ToDecimal(point.Latitude);
                newAddress.Longitude = Convert.ToDecimal(point.Longitude);
                newAddress.CountyName = point.CountyName == null ? request.City : point.CountyName;
            }

            newAddress.TerminalId = null;
            newAddress.SiteName = request.BulkplantName;
            newAddress.Address = request.Address;
            newAddress.City = request.City;
            newAddress.ZipCode = request.ZipCode;
            var state = await Context.DataContext.MstStates.Where(t => t.Code.ToLower() == request.StateCode.ToLower()).FirstOrDefaultAsync();
            newAddress.StateId = state != null ? state.Id : 0;
            newAddress.StateCode = request.StateCode;
            newAddress.CountryCode = state != null ? state.MstCountry.Code : string.Empty;
        }

        private async Task SetTerminalAddress(ChangePickupLocationViewModel request, StatusViewModel response, DbContextTransaction transaction, FuelDispatchLocation newAddress, FuelDispatchLocationViewModel newAddressModel)
        {
            var terminal = await Context.DataContext.MstExternalTerminals.Where(t => t.Id == request.TerminalId.Value && t.IsActive).FirstOrDefaultAsync();
            if (terminal != null)
            {
                newAddress.TerminalId = request.TerminalId;
                newAddressModel.TerminalId = request.TerminalId;
                newAddress.Address = terminal.Address;
                newAddress.City = terminal.City;
                newAddress.ZipCode = terminal.ZipCode;
                newAddress.StateId = terminal.StateId;
                newAddress.StateCode = terminal.StateCode;
                newAddress.CountryCode = terminal.CountryCode;
                newAddress.CountyName = terminal.CountyName;
                newAddress.Latitude = terminal.Latitude;
                newAddress.Longitude = terminal.Longitude;
            }
            else
            {
                transaction.Rollback();
                response.StatusCode = Status.Failed;
                response.StatusMessage = "Terminal not found";
            }
        }

        private async Task SetPickLocationIfNotExists(ChangePickupLocationViewModel request, FuelDispatchLocationViewModel newAddressModel, PickupLocationOrderDetailRequestModel orderDetail)
        {
            var order = await Context.DataContext.Orders
                                                .Where(t => t.Id == orderDetail.OrderId &&
                                                        t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open
                                                    )
                                                .FirstOrDefaultAsync();
            newAddressModel.LocationType = LocationType.PickUp;
            newAddressModel.OrderId = orderDetail.OrderId;
            newAddressModel.DeliveryScheduleId = orderDetail.DeliveryScheduleId;
            newAddressModel.TrackableScheduleId = orderDetail.TrackableScheduleId;
            newAddressModel.TerminalId = request.TerminalId;
            newAddressModel.IsFuturePickUp = false;
            newAddressModel.Currency = order.FuelRequest.Currency;
            newAddressModel.TimeZoneName = order.FuelRequest.Job.TimeZoneName;
            newAddressModel.DropStatus = DropAddressStatus.UnKnown;
            newAddressModel.IsJobLocation = false;
            newAddressModel.IsSkipped = false;

            var trackableSchedule = order.DeliveryScheduleXTrackableSchedules
                                         .Where(t => t.Id == orderDetail.TrackableScheduleId && t.IsActive &&
                                                     !t.IsScheduleCancelled).OrderByDescending(t => t.Id).FirstOrDefault();
            newAddressModel.DeliveryGroupId = trackableSchedule != null ? trackableSchedule.DeliveryGroupId : null;
        }

        public async Task<List<UspGetBrokerActivity>> GetBrokerActivityAsync(int companyId, string StartDate, string EndDate, Currency currency = Currency.USD, int countryId = (int)Country.USA)
        {
            var response = new List<UspGetBrokerActivity>();

            try
            {
                DateTimeOffset startDate = Convert.ToDateTime(ApplicationConstants.DateRangeFilterStartDate);
                if (!string.IsNullOrEmpty(StartDate))
                {
                    startDate = Convert.ToDateTime(StartDate).Date;
                }
                DateTimeOffset endDate = DateTimeOffset.Now.Date.AddDays(1);
                if (!string.IsNullOrEmpty(EndDate))
                {
                    endDate = Convert.ToDateTime(EndDate).Date.AddDays(1);
                }
                var brokeredOrders = await Context.DataContext.Orders.Where(t => t.BuyerCompanyId == companyId &&
                                                        t.AcceptedDate >= startDate &&
                                                        t.AcceptedDate <= endDate &&
                                                        t.FuelRequest.Currency == currency &&
                                                        t.FuelRequest.Job.CountryId == countryId
                                                        )
                                                     .OrderByDescending(t => t.Id).ToListAsync();
                HelperDomain helperDomain = new HelperDomain(this);
                foreach (var order in brokeredOrders)
                {
                    List<Order> buyerOrders = order.FuelRequest.GetParentFuelRequest().FuelRequest1.Orders.Where(t => t.AcceptedCompanyId == companyId).ToList();
                    List<Order> buyerOrders1 = new List<Order>();
                    if (!buyerOrders.Any())
                    {
                        var buyerRequest = order.Orders1.FirstOrDefault(t => t.BuyerCompanyId == companyId).FuelRequest;
                        buyerOrders = buyerRequest.GetParentFuelRequest().FuelRequest1.Orders.Where(t => t.AcceptedCompanyId == companyId).ToList();
                    }
                    foreach (var buyerOrder in buyerOrders) // to show orders in scenario B -> FR1 , O1  S1 -> FR2, O2 S2 -> FR3, O3 when S1 cancels O1 and S2 choose right side order
                    {
                        if (buyerOrder.Order1 != null && buyerOrder.Order1.AcceptedCompanyId == companyId)
                        {
                            buyerOrders1.Add(buyerOrder.Order1);
                        }
                    }
                    buyerOrders = buyerOrders.Union(buyerOrders1).OrderByDescending(t => t.Id).ToList();
                    foreach (var buyerOrder in buyerOrders)
                    {
                        UspGetBrokerActivity brokeredOrder = new UspGetBrokerActivity();
                        brokeredOrder.Id = order.Id;
                        brokeredOrder.ParentOrderId = buyerOrder.Id;
                        brokeredOrder.SupplierPoNumber = order.PoNumber;
                        brokeredOrder.BuyerPoNumber = buyerOrder.PoNumber;
                        brokeredOrder.SupplierCompany = order.Company.Name;
                        brokeredOrder.BuyerCompany = buyerOrder.BuyerCompany.Name;
                        brokeredOrder.SupplierCompanyId = order.Company.Id;
                        brokeredOrder.BuyerCompanyId = buyerOrder.BuyerCompany.Id;
                        brokeredOrder.FuelType = helperDomain.GetProductName(order.FuelRequest.MstProduct);
                        brokeredOrder.SupplierQuantity = helperDomain.GetQuantityRequested(order.BrokeredMaxQuantity, order.FuelRequest.MaxQuantity);
                        brokeredOrder.BuyerQuantity = helperDomain.GetQuantityRequested(buyerOrder.BrokeredMaxQuantity, buyerOrder.FuelRequest.MaxQuantity);
                        brokeredOrder.SupplierPPG = helperDomain.GetPricePerGallon(order.FuelRequest);
                        brokeredOrder.BuyerPPG = helperDomain.GetPricePerGallon(buyerOrder.FuelRequest);
                        int statusId = order.OrderXStatuses.FirstOrDefault(t => t.IsActive).StatusId;
                        if (buyerOrder != null && buyerOrder.OrderXStatuses.FirstOrDefault(t => t.IsActive).StatusId != (int)OrderStatus.Open)
                        {
                            statusId = buyerOrder.OrderXStatuses.FirstOrDefault(t => t.IsActive).StatusId;
                        }
                        brokeredOrder.Status = Convert.ToString((OrderStatus)((statusId == (int)OrderStatus.PartiallyCanceled ||
                                                statusId == (int)OrderStatus.PartiallyClosed) ? GetBrokeredOrderStatusId(order, statusId) : statusId));
                        response.Add(brokeredOrder);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetBrokerActivityAsync", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<OrderGridViewModel>> GetSupplierOrders(int companyId, DataTableSearchModel requestModel, OrderFilterViewModel filter = null)
        {
            var response = new List<OrderGridViewModel>();
            var helperDomain = new HelperDomain(this);

            if (filter == null)
            {
                filter = new OrderFilterViewModel();
            }

            try
            {
                var spDomain = new StoredProcedureDomain(this);
                var orders = await spDomain.GetSupplierOrders(companyId, requestModel, filter);
                foreach (var item in orders)
                {
                    var order = new OrderGridViewModel(Status.Success);
                    order.Id = item.Id;
                    order.PoNumber = item.PoNumber;
                    order.Eligibility = item.Eligibility;
                    order.Supplier = item.Supplier;
                    order.JobName = item.JobName;
                    //order.TotalAmount = helperDomain.CheckQuantityValid(item.Quantity, item.TotalAmount);
                    if (item.Quantity != ApplicationConstants.QuantityNotSpecified)
                    {
                        order.Quantity = helperDomain.GetQuantityRequested(item.Quantity) + " " + helperDomain.GetUOM(item.UOM);
                    }
                    else
                    {
                        order.Quantity = helperDomain.GetQuantityRequested(item.Quantity);
                    }

                    order.FuelType = item.FuelType;
                    order.StartDate = item.StartDate;
                    order.InvoiceCount = item.InvoiceCount;
                    order.DDTCount = item.DDTCount;
                    order.FuelDeliveredPercentage = helperDomain.CheckQuantityValid(item.Quantity, item.FuelDeliveredPercentage);
                    order.Status = item.Status;
                    order.BrokerFuelRequestId = item.BrokerFuelRequestId;
                    order.TotalCount = item.TotalCount;
                    order.Location = item.Location;
                    order.DeliveryType = item.DeliveryType;
                    order.OrderType = item.OrderType;
                    order.OrderGroupId = item.OrderGroupId;
                    order.GroupPoNumber = string.IsNullOrWhiteSpace(item.GroupPoNumber) ? Resource.lblHyphen : item.GroupPoNumber;
                    order.OrderName = item.OrderName;
                    order.VesselName = item.VesselName;
                    response.Add(order);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetSupplierOrders", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<NextDeliveryScheduleViewModel>> GetNextDeliveryScheduleDetails(int orderId)
        {
            var finalResponse = new List<NextDeliveryScheduleViewModel>();

            try
            {
                int datediff, scheduleDays;
                DateTimeOffset scheduleDate;
                var response = new List<NextDeliveryScheduleViewModel>();
                var order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == orderId &&
                    t.FuelRequest.FuelRequestDetail.DeliveryTypeId == (int)DeliveryType.MultipleDeliveries
                    && t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open);
                if (order != null)
                {
                    DateTimeOffset deliveryDate = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName);
                    DateTimeOffset deliveryStartDate = deliveryDate.Date;
                    DateTimeOffset deliveryEndDate = deliveryStartDate.AddDays(1);

                    var orderSchedules = GetLatestOrderDeliverySchedule(order.OrderDeliverySchedules);
                    if (orderSchedules.Any())
                    {
                        var allTrackableSchedules = order.DeliveryScheduleXTrackableSchedules.Where(Extensions.IsTrackableScheduleUnDeliveredFunc()).Where(t => t.IsActive && !t.IsDropped
                                        && t.Date >= deliveryStartDate && t.Date < deliveryEndDate && !t.IsScheduleCancelled).OrderByDescending(t => t.StartTime);
                        var unitOfMeasurement = order.FuelRequest.Job.CountryId == (int)Country.CAN ? Resource.lblLitres : Resource.lblGallons;
                        if (allTrackableSchedules.Any())
                        {
                            foreach (var item in allTrackableSchedules)
                            {
                                var deliveryDetails = new NextDeliveryScheduleViewModel();
                                deliveryDetails.ScheduleDate = item.Date.ToString(Resource.constFormatDate);
                                deliveryDetails.ScheduleTime = $"{Convert.ToDateTime(item.StartTime.ToString()).ToShortTimeString()} { Resource.lblSingleHyphen } {Convert.ToDateTime(item.EndTime.ToString()).ToShortTimeString()}";
                                deliveryDetails.Quantity = $"{item.Quantity.GetPreciseValue(2)} {unitOfMeasurement}";
                                deliveryDetails.Date = item.Date.Date.Add(item.StartTime);
                                response.Add(deliveryDetails);
                            }
                        }
                        else
                        {
                            TrackableScheduleDomain trackableScheduleDomain = new TrackableScheduleDomain(this);
                            // if delivery schedule is future date
                            var allschedules = GetOrderDeliverySchedules(order);
                            foreach (var schedule in allschedules)
                            {
                                if (schedule.ScheduleType == (int)DeliveryScheduleType.SpecificDates)
                                {
                                    if (schedule.ScheduleDate.Date > deliveryDate.Date)
                                    {
                                        var deliveryDetails = new NextDeliveryScheduleViewModel();
                                        deliveryDetails.ScheduleDate = schedule.StrScheduleDate;
                                        deliveryDetails.ScheduleTime = $"{schedule.ScheduleStartTime} {Resource.lblSingleHyphen} {schedule.ScheduleEndTime}";
                                        deliveryDetails.Quantity = $"{schedule.ScheduleQuantity.GetPreciseValue(2)} {unitOfMeasurement}";
                                        deliveryDetails.Date = schedule.ScheduleDate.Date.Add(schedule.StartTime);
                                        response.Add(deliveryDetails);
                                    }
                                }
                                else
                                {
                                    foreach (var item in schedule.AllScheduleDate)
                                    {
                                        scheduleDays = trackableScheduleDomain.GetDaysToAdd(schedule.ScheduleType);
                                        if (item.Date <= deliveryDate.Date)
                                        {
                                            datediff = Math.Abs(item.Subtract(deliveryDate).Days) % scheduleDays;
                                            scheduleDate = /*datediff == 0 ? deliveryDate :*/ deliveryDate.AddDays(scheduleDays - datediff);
                                        }
                                        else
                                        {
                                            scheduleDate = item.Date;
                                        }

                                        var deliveryDetails = new NextDeliveryScheduleViewModel();
                                        deliveryDetails.ScheduleDate = scheduleDate.ToString(Resource.constFormatDate);
                                        deliveryDetails.ScheduleTime = $"{schedule.ScheduleStartTime} {Resource.lblSingleHyphen} {schedule.ScheduleEndTime}";
                                        deliveryDetails.Quantity = $"{schedule.ScheduleQuantity.GetPreciseValue(2)} {unitOfMeasurement}";
                                        deliveryDetails.Date = scheduleDate.Date.Add(schedule.StartTime);
                                        response.Add(deliveryDetails);
                                    }
                                }
                            }
                        }

                        if (response.Count > 0)
                        {
                            var driverDropDetails = response.OrderBy(t => t.Date).FirstOrDefault();
                            finalResponse = response.Where(t => t.Date == driverDropDetails.Date).ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetNextDeliveryScheduleDetails", ex.Message, ex);
            }

            return finalResponse;
        }

        public async Task<ApiBolResponseViewModel> SaveBolDetailsAsync(ApiBolDetailsViewModel viewModel, int companyId)
        {
            var response = new ApiBolResponseViewModel();
            try
            {
                var dispatchDomain = new DispatchDomain(this);
                var carrier = await dispatchDomain.AddCarrierIfNotExists(viewModel.Carrier, viewModel.UserId, companyId);
                //var orderDetails = await Context.DataContext.OrderAdditionalDetails.Where(t => t.OrderId == viewModel.OrderId)
                //                                       .Select(t => new { t.Allowance, t.Order.FuelRequest.FuelRequestDetail.PricingQuantityIndicatorTypeId }).FirstOrDefaultAsync();
                ImageViewModel bolImage = null;
                if (!string.IsNullOrEmpty(viewModel.BolImage))
                {
                    bolImage = new ImageViewModel { Data = Convert.FromBase64String(viewModel.BolImage) };
                }
                var image = bolImage?.ToEntity();
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    if (!viewModel.IsDriverToUpdateBOL && image != null)
                    {
                        Context.DataContext.Images.Add(image);
                        await Context.CommitAsync();
                    }
                    else
                    {
                        var bolDetail = new InvoiceFtlDetail()
                        {
                            BolNumber = viewModel.BolNumber,
                            NetQuantity = viewModel.NetQuantity,
                            Carrier = carrier.Name,
                            CreatedBy = viewModel.UserId,
                            GrossQuantity = viewModel.GrossQuantity,
                            CreatedDate = DateTimeOffset.Now,
                            LiftDate = DateTimeOffset.FromUnixTimeMilliseconds(viewModel.LiftDate).ToOffset(TimeSpan.FromMinutes(viewModel.TimeZoneOffset)),
                            Image = image
                        };
                        Context.DataContext.InvoiceFtlDetails.Add(bolDetail);
                        await Context.CommitAsync();
                        response.BolId = bolDetail.Id;
                    }
                    transaction.Commit();
                }

                response.BolImageId = image != null ? image.Id : 0;
                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.errMessageSuccess;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "SaveBolDetailsAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<ApiBolFuelDetailsResponseModel> SaveBolFuelDetailsAsync(ApiBolFuelDetailsRequestModel viewModel, UserContext userContext)
        {
            var response = new ApiBolFuelDetailsResponseModel();
            try
            {
                var dispatchDomain = new DispatchDomain(this);
                var carrier = await dispatchDomain.AddCarrierIfNotExists(viewModel.Carrier, viewModel.UserId, userContext.CompanyId);

                ImageViewModel bolImage = null;
                if (viewModel.BolFile != null)
                {
                    bolImage = new ImageViewModel();
                    var result = await AzureStorageService.UploadImageToBlob(userContext, viewModel.BolFile.InputStream, "bol.pdf", BlobContainerType.InvoicePdfFiles);
                    if (result.StatusCode == Status.Success)
                    {
                        bolImage.FilePath = result.StatusMessage;
                        bolImage.IsPdf = true;
                    }
                }
                var bolImageEntity = bolImage?.ToEntity();

                ImageViewModel additionalImage = null;
                if (viewModel.AdditionalFile != null)
                {
                    additionalImage = new ImageViewModel();
                    var result = await AzureStorageService.UploadImageToBlob(userContext, viewModel.AdditionalFile.InputStream, "add-img.pdf", BlobContainerType.InvoicePdfFiles);
                    if (result.StatusCode == Status.Success)
                    {
                        additionalImage.FilePath = result.StatusMessage;
                        additionalImage.IsPdf = true;
                    }
                }
                var additionalImageEntity = additionalImage?.ToEntity();

                List<int> orderIds = null;
                if (viewModel.BolFuelDetails != null && viewModel.BolFuelDetails.Any())
                {
                    foreach (var bolInfo in viewModel.BolFuelDetails)
                    {
                        foreach (var schedule in bolInfo.Schedules)
                        {
                            if (schedule != null)
                            {
                                if (orderIds == null)
                                {
                                    orderIds = new List<int>();
                                }

                                orderIds.Add(schedule.OrderId);
                            }
                        }
                    }
                }

                // save BOL details by FuelType
                orderIds = orderIds.Distinct().ToList();
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    response.BolFuelDetailResponse = new List<BolFuelDetailsResponseViewModel>();
                    if (bolImageEntity != null)
                    {
                        Context.DataContext.Images.Add(bolImageEntity);
                        await Context.CommitAsync();
                    }
                    if (additionalImageEntity != null)
                    {
                        Context.DataContext.Images.Add(additionalImageEntity);
                        await Context.CommitAsync();
                    }

                    if (viewModel.IsDriverToUpdateBOL)
                    {
                        foreach (var orderId in orderIds)
                        {
                            var bolDetails = viewModel.BolFuelDetails.FirstOrDefault(t => t.Schedules != null && t.Schedules.Any(t1 => t1.OrderId == orderId));
                            var trackableScheduleId = bolDetails.Schedules.FirstOrDefault().TrackableScheduleId;
                            var orderDetails = Context.DataContext.Orders.Where(t => t.Id == orderId)
                                .Select(t => new
                                {
                                    t.OrderAdditionalDetail,
                                    location = t.FuelDispatchLocations.FirstOrDefault(t1 => t1.LocationType == (int)LocationType.PickUp && t1.TrackableScheduleId == trackableScheduleId && t1.IsActive),
                                    t.MstExternalTerminal,
                                    t.CityGroupTerminalId
                                })
                                .FirstOrDefault();
                            if (bolDetails != null)
                            {
                                var bolDetail = new InvoiceFtlDetail()
                                {
                                    Carrier = carrier.Name,
                                    CreatedBy = viewModel.UserId,
                                    CreatedDate = DateTimeOffset.Now,
                                    LiftDate = DateTimeOffset.Now.ToOffset(TimeSpan.FromMinutes(viewModel.Offset)),
                                    Image = bolImageEntity
                                };

                                if (viewModel.IsBulkPlant)
                                {
                                    bolDetail.LiftTicketNumber = viewModel.BolNumber;
                                }
                                else
                                {
                                    bolDetail.BolNumber = viewModel.BolNumber;
                                }
                                bolDetail.NetQuantity = bolDetails.NetQuantity;
                                bolDetail.GrossQuantity = bolDetails.GrossQuantity;
                                bolDetail.PickupLocation = viewModel.IsBulkPlant ? PickupLocationType.BulkPlant : PickupLocationType.Terminal;
                                bolDetail.IsDeleted = false;
                                bolDetail.IsActive = true;
                                bolDetail.FuelTypeId = bolDetails.FuelTypeId;
                                bolDetail.CityGroupTerminalId = orderDetails.CityGroupTerminalId;
                                bolDetail.TerminalId = orderDetails.location?.TerminalId ?? orderDetails.MstExternalTerminal?.Id;
                                bolDetail.TerminalName = orderDetails.location?.MstExternalTerminal?.Name ?? orderDetails.MstExternalTerminal?.Name;
                                if (orderDetails.location != null)
                                {
                                    bolDetail.Address = orderDetails.location.Address;
                                    bolDetail.City = orderDetails.location.City;
                                    bolDetail.CountryCode = orderDetails.location.CountryCode;
                                    bolDetail.CountyName = orderDetails.location.CountyName;
                                    bolDetail.Latitude = orderDetails.location.Latitude;
                                    bolDetail.Longitude = orderDetails.location.Longitude;
                                    bolDetail.SiteName = orderDetails.location.SiteName;
                                    bolDetail.StateCode = orderDetails.location.StateCode;
                                    bolDetail.StateId = orderDetails.location.StateId;
                                    bolDetail.ZipCode = orderDetails.location.ZipCode;
                                }
                                else if (orderDetails.location == null && orderDetails.MstExternalTerminal != null)
                                {
                                    var terminal = orderDetails.MstExternalTerminal;
                                    bolDetail.Address = terminal.Address;
                                    bolDetail.City = terminal.City;
                                    bolDetail.CountryCode = terminal.CountryCode;
                                    bolDetail.CountyName = terminal.CountyName;
                                    bolDetail.Latitude = terminal.Latitude;
                                    bolDetail.Longitude = terminal.Longitude;
                                    bolDetail.StateCode = terminal.StateCode;
                                    bolDetail.StateId = terminal.StateId;
                                    bolDetail.ZipCode = terminal.ZipCode;
                                }

                                //if (orderDetails != null) 
                                //{
                                //    orderDetails.OrderAdditionalDetail.IsDriverToUpdateBOL = viewModel.IsDriverToUpdateBOL; // ??
                                //}

                                Context.DataContext.InvoiceFtlDetails.Add(bolDetail);
                                await Context.CommitAsync();

                                var bolResponse = new BolFuelDetailsResponseViewModel();
                                bolResponse.BolId = bolDetail.Id;
                                bolResponse.AdditionalImageId = additionalImageEntity?.Id;
                                bolResponse.BolImageId = bolImageEntity?.Id;
                                bolResponse.FuelTypeId = bolDetails.FuelTypeId;
                                bolResponse.OrderId = orderId;

                                response.BolFuelDetailResponse.Add(bolResponse);
                            }
                        }
                    }
                    else
                    {
                        var bolResponse = new BolFuelDetailsResponseViewModel();
                        bolResponse.AdditionalImageId = additionalImageEntity?.Id;
                        bolResponse.BolImageId = bolImageEntity?.Id;
                        response.BolFuelDetailResponse.Add(bolResponse);
                    }

                    transaction.Commit();
                }

                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.errMessageSuccess;
            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = Resource.errMessagFailedToSaveDetails;
                LogManager.Logger.WriteException("OrderDomain", "SaveBolFuelDetailsAsync", ex.Message, ex);
            }

            return response;
        }

        public List<DeliveryScheduleViewModel> GetOrderDeliverySchedules(Order order)
        {
            List<DeliveryScheduleViewModel> response = new List<DeliveryScheduleViewModel>();
            try
            {
                if (order != null)
                {
                    var latestSchedules = GetLatestOrderDeliverySchedule(order.OrderDeliverySchedules);
                    if (latestSchedules != null)
                    {
                        response = latestSchedules.Where(t => t.DeliveryRequestId.HasValue)
                                                  .Select(t => t.DeliverySchedule)
                                                  .GroupBy(t => t.GroupId)
                                                  .Select(g => new { Items = g.ToList() })
                                                  .Select(t => t.Items.ToViewModel())
                                                  .OrderBy(t => t.Id)
                                                  .ToList();

                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetAcceptedDeliverySchedulesForOrderAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<OrderDetails>> GetDriverOrdersAsync(decimal latitude, decimal longitude, int companyId, int userId, int distance, long scheduleDate = 0, int buyerCompanyId = 0)
        {
            List<OrderDetails> response = new List<OrderDetails>();
            try
            {
                var driverAppExactOrderProximity = ApplicationDomain.ApplicationSettings.GetApplicationSettingValue(ApplicationConstants.KeyAppSettingDriverAppExactOrderProximity, 1);
                var storedProcedureDomain = new StoredProcedureDomain(this);
                var orders = await storedProcedureDomain.GetDriverOrdersForMobile(latitude, longitude, companyId, userId, distance, driverAppExactOrderProximity, scheduleDate, buyerCompanyId);
                //var orderIds = orders.Select(t => t.OrderId).ToList();
                //var enrouteData = Context.DataContext.EnrouteDeliveryHistories.Where(t => orderIds.Any(t1 => t1 == t.OrderId)).Select(t => new { t.TrackableScheduleId, t.EnrouteDate, t.OrderId, t.StatusId }).ToList();
                var requestIds = orders.Select(t => t.FuelId);
                var onsiteContactsAndSiteInstructions = Context.DataContext.Jobs.Where(t => t.FuelRequests.Any(t1 => requestIds.Contains(t1.Id))).Select(t => new
                { User = t.Users1.FirstOrDefault(t1 => t1.IsActive), t.Id, t.SiteInstructions }).ToList();
                var specialInstructions = Context.DataContext.FuelRequests.Where(t => requestIds.Contains(t.Id)).Select(t => new { Instructions = t.SpecialInstructions, t.Id }).ToList();
                foreach (var item in orders)
                {
                    var driverOrder = new OrderDetails();

                    driverOrder.OrderId = item.OrderId.ToString();
                    driverOrder.JobId = item.JobId;
                    driverOrder.CustomerOrderNumber = item.CustomerOrderNumber;
                    driverOrder.FuelId = item.FuelId;
                    driverOrder.FuelTypeName = item.FuelTypeName;
                    driverOrder.OrderName = item.OrderName;
                    driverOrder.CustomerName = item.CustomerName;
                    driverOrder.Quantity = item.Quantity;
                    driverOrder.TotalOrderQuantity = item.TotalOrderQuantity;
                    driverOrder.Latitude = item.Latitude.ToString();
                    driverOrder.Longitude = item.Longitude.ToString();
                    driverOrder.WetHosing = Convert.ToBoolean(item.WetHosing);
                    driverOrder.OverWater = Convert.ToBoolean(item.OverWater);
                    driverOrder.AssetCount = item.AssetCount;
                    driverOrder.IsAssetDropPicMandetory = item.IsAssetDropPicMandetory;
                    driverOrder.IsFTL = item.IsFTL;
                    driverOrder.IsDriverToUpdateBOL = item.IsDriverToUpdateBOL;
                    driverOrder.IsDropImageRequired = item.IsDropImageRequired;
                    driverOrder.IsBolImageRequired = item.IsBolImageRequired;
                    driverOrder.IsPrePostDipEnabled = item.IsPrePostDipRequired;
                    driverOrder.Address = item.Address;
                    driverOrder.City = item.City;
                    driverOrder.State = item.State;
                    driverOrder.SupplierName = item.SupplierName;
                    driverOrder.IsExactMatch = Convert.ToBoolean(item.IsExactMatch);
                    driverOrder.IsDeliveryScheduleAdded = Convert.ToBoolean(item.IsDeliveryScheduleAdded);
                    driverOrder.IsDriverAssigned = Convert.ToBoolean(item.IsDriverAssigned);
                    driverOrder.RunningMeterMode = (RunningMeterMode)item.RunningMeterMode;
                    driverOrder.StartDate = item.StartDate.Date.ToShortDateString();
                    driverOrder.StartTime = Convert.ToDateTime(item.StartTime.ToString()).ToShortTimeString();
                    driverOrder.EndTime = Convert.ToDateTime(item.EndTime.ToString()).ToShortTimeString();
                    driverOrder.UtcStartDate = item.StartDate.ToUnixTimeMilliseconds();
                    driverOrder.InitalMeterReading = item.InitalMeterReading;
                    driverOrder.FinalMeterReading = item.FinalMeterReading;
                    driverOrder.TrackableScheduleId = item.TrackableScheduleId;
                    driverOrder.DeliveryScheduleId = item.DeliveryScheduleId;
                    driverOrder.Distance = item.Distance;
                    driverOrder.BuyerCompanyId = item.BuyerCompanyId;
                    driverOrder.OrderDeliveryType = item.OrderDeliveryType;
                    driverOrder.IsAssetDropStatusEnabled = item.IsAssetDropStatusEnabled;
                    driverOrder.UnitOfMeasurement = item.UnitOfMeasurement;
                    driverOrder.Currency = item.Currency;
                    driverOrder.CustomerSignatureRequired = item.SignatureEnabled;
                    driverOrder.QuantityTypeId = item.QuantityTypeId;
                    driverOrder.ScheduleQuantityTypeId = (int)item.ScheduleQuantityType;
                    driverOrder.ScheduleQuantityTypeName = item.ScheduleQuantityType.ToString();

                    //Removing to improve performance
                    //int? enrouteDataStatus = enrouteData.Where(t => (item.TrackableScheduleId == 0 || t.TrackableScheduleId == item.TrackableScheduleId)
                    //												 && t.OrderId == item.OrderId && t.StatusId == (int)EnrouteDeliveryStatus.SplitTank).
                    //												 OrderByDescending(t => t.EnrouteDate).Select(t => t.StatusId).FirstOrDefault();
                    //driverOrder.IsSplitTank = enrouteDataStatus.HasValue;

                    //enrouteDataStatus = enrouteData.Where(t => (item.TrackableScheduleId == 0 || t.TrackableScheduleId == item.TrackableScheduleId)
                    //												 && t.OrderId == item.OrderId && t.StatusId == (int)EnrouteDeliveryStatus.FuelTruckRetain).
                    //												 OrderByDescending(t => t.EnrouteDate).Select(t => t.StatusId).FirstOrDefault();
                    //driverOrder.IsRentainFee = enrouteDataStatus.HasValue;

                    // add special instruction file upload details
                    driverOrder.SpecialInstruction = GetSpecialInstructionFileDetails(int.Parse(driverOrder.OrderId), item.FileDetails);

                    response.Add(driverOrder);
                }

                for (int i = 0; i < response.Count; i++)
                {
                    var onsiteContact = onsiteContactsAndSiteInstructions.FirstOrDefault(t => t.Id == response[i].JobId).User;
                    if (onsiteContact != null)
                    {
                        response[i].ContactEmail = onsiteContact.Email;
                        response[i].ContactNumber = onsiteContact.PhoneNumber;
                        response[i].ContactPerson = $"{onsiteContact.FirstName} {onsiteContact.LastName}";
                    }
                    response[i].UtcEndTime = response[i].StartTime.ToUnixTimeMilliseconds();
                    response[i].UtcStartTime = response[i].EndTime.ToUnixTimeMilliseconds();
                    response[i].SpecialInstructionsToDriver = specialInstructions.Where(t => t.Id == response[i].FuelId).SelectMany(t => t.Instructions.Select(t1 => t1.Instruction)).ToList();
                    response[i].SiteInstructions = onsiteContactsAndSiteInstructions.FirstOrDefault(t => t.Id == response[i].JobId).SiteInstructions;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetDriverOrdersAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<OrderDetails>> GetDriverOrdersForSchedulesAsync(int companyId, int userId, string orderIds, string scheduleIds, long scheduleDate = 0, int buyerCompanyId = 0)
        {
            List<OrderDetails> response = new List<OrderDetails>();
            try
            {
                if (!String.IsNullOrWhiteSpace(orderIds) && !String.IsNullOrWhiteSpace(scheduleIds))
                {
                    var storedProcedureDomain = new StoredProcedureDomain(this);
                    var orders = await storedProcedureDomain.GetDriverOrdersOfSchedulesForMobile(userId, orderIds, scheduleIds);
                    var requestIds = orders.Select(t => t.FuelId);
                    var onsiteContactsAndSiteInstructions = Context.DataContext.Jobs.Where(t => t.FuelRequests.Any(t1 => requestIds.Contains(t1.Id))).Select(t => new
                    { User = t.Users1.FirstOrDefault(t1 => t1.IsActive), t.Id, t.SiteInstructions }).ToList();
                    var specialInstructions = Context.DataContext.FuelRequests.Where(t => requestIds.Contains(t.Id)).Select(t => new { Instructions = t.SpecialInstructions, t.Id }).ToList();
                    foreach (var item in orders)
                    {
                        var driverOrder = new OrderDetails();

                        driverOrder.OrderId = item.OrderId.ToString();
                        driverOrder.JobId = item.JobId;
                        driverOrder.CustomerOrderNumber = item.CustomerOrderNumber;
                        driverOrder.FuelId = item.FuelId;
                        driverOrder.FuelTypeName = item.FuelTypeName;
                        driverOrder.OrderName = item.OrderName;
                        driverOrder.CustomerName = item.CustomerName;
                        driverOrder.Quantity = item.Quantity;
                        driverOrder.TotalOrderQuantity = item.TotalOrderQuantity;
                        driverOrder.Latitude = item.Latitude.ToString();
                        driverOrder.Longitude = item.Longitude.ToString();
                        driverOrder.WetHosing = Convert.ToBoolean(item.WetHosing);
                        driverOrder.OverWater = Convert.ToBoolean(item.OverWater);
                        driverOrder.AssetCount = item.AssetCount;
                        driverOrder.IsAssetDropPicMandetory = item.IsAssetDropPicMandetory;
                        driverOrder.IsFTL = item.IsFTL;
                        driverOrder.IsDriverToUpdateBOL = item.IsDriverToUpdateBOL;
                        driverOrder.IsDropImageRequired = item.IsDropImageRequired;
                        driverOrder.IsBolImageRequired = item.IsBolImageRequired;
                        driverOrder.Address = item.Address;
                        driverOrder.City = item.City;
                        driverOrder.State = item.State;
                        driverOrder.SupplierName = item.SupplierName;
                        driverOrder.IsDeliveryScheduleAdded = Convert.ToBoolean(item.IsDeliveryScheduleAdded);
                        driverOrder.IsDriverAssigned = Convert.ToBoolean(item.IsDriverAssigned);
                        driverOrder.RunningMeterMode = (RunningMeterMode)item.RunningMeterMode;
                        driverOrder.StartDate = item.StartDate.Date.ToShortDateString();
                        driverOrder.StartTime = Convert.ToDateTime(item.StartTime.ToString()).ToShortTimeString();
                        driverOrder.EndTime = Convert.ToDateTime(item.EndTime.ToString()).ToShortTimeString();
                        driverOrder.UtcStartDate = item.StartDate.ToUnixTimeMilliseconds();
                        driverOrder.InitalMeterReading = item.InitalMeterReading;
                        driverOrder.FinalMeterReading = item.FinalMeterReading;
                        driverOrder.TrackableScheduleId = item.TrackableScheduleId;
                        driverOrder.DeliveryScheduleId = item.DeliveryScheduleId;
                        driverOrder.BuyerCompanyId = item.BuyerCompanyId;
                        driverOrder.OrderDeliveryType = item.OrderDeliveryType;
                        driverOrder.IsAssetDropStatusEnabled = item.IsAssetDropStatusEnabled;
                        driverOrder.OrderUoM = item.UnitOfMeasurement;
                        if (item.UnitOfMeasurement == (int)UoM.Barrels || item.UnitOfMeasurement == (int)UoM.MetricTons)
                        {
                            if (item.CountryId == (int)Country.CAN)
                            {
                                driverOrder.UnitOfMeasurement = (int)UoM.Litres;
                            }
                            else
                            {
                                driverOrder.UnitOfMeasurement = (int)UoM.Gallons;
                            }
                        }
                        else
                        {
                            driverOrder.UnitOfMeasurement = item.UnitOfMeasurement;
                        }
                        driverOrder.Currency = item.Currency;
                        driverOrder.CustomerSignatureRequired = item.SignatureEnabled;
                        driverOrder.QuantityTypeId = item.QuantityTypeId;
                        driverOrder.ScheduleQuantityTypeId = (int)item.ScheduleQuantityType;
                        driverOrder.ScheduleQuantityTypeName = item.ScheduleQuantityType.ToString();

                        //Removing to improve performance
                        //int? enrouteDataStatus = enrouteData.Where(t => (item.TrackableScheduleId == 0 || t.TrackableScheduleId == item.TrackableScheduleId)
                        //												 && t.OrderId == item.OrderId && t.StatusId == (int)EnrouteDeliveryStatus.SplitTank).
                        //												 OrderByDescending(t => t.EnrouteDate).Select(t => t.StatusId).FirstOrDefault();
                        //driverOrder.IsSplitTank = enrouteDataStatus.HasValue;

                        //enrouteDataStatus = enrouteData.Where(t => (item.TrackableScheduleId == 0 || t.TrackableScheduleId == item.TrackableScheduleId)
                        //												 && t.OrderId == item.OrderId && t.StatusId == (int)EnrouteDeliveryStatus.FuelTruckRetain).
                        //												 OrderByDescending(t => t.EnrouteDate).Select(t => t.StatusId).FirstOrDefault();
                        //driverOrder.IsRentainFee = enrouteDataStatus.HasValue;

                        // add special instruction file upload details
                        driverOrder.SpecialInstruction = GetSpecialInstructionFileDetails(int.Parse(driverOrder.OrderId), item.FileDetails);

                        response.Add(driverOrder);
                    }

                    for (int i = 0; i < response.Count; i++)
                    {
                        var onsiteContact = onsiteContactsAndSiteInstructions.FirstOrDefault(t => t.Id == response[i].JobId).User;
                        if (onsiteContact != null)
                        {
                            response[i].ContactEmail = onsiteContact.Email;
                            response[i].ContactNumber = onsiteContact.PhoneNumber;
                            response[i].ContactPerson = $"{onsiteContact.FirstName} {onsiteContact.LastName}";
                        }
                        response[i].UtcEndTime = response[i].StartTime.ToUnixTimeMilliseconds();
                        response[i].UtcStartTime = response[i].EndTime.ToUnixTimeMilliseconds();
                        response[i].SpecialInstructionsToDriver = specialInstructions.Where(t => t.Id == response[i].FuelId).SelectMany(t => t.Instructions.Select(t1 => t1.Instruction)).ToList();
                        response[i].SiteInstructions = onsiteContactsAndSiteInstructions.FirstOrDefault(t => t.Id == response[i].JobId).SiteInstructions;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetDriverOrdersForSchedulesAsync", ex.Message, ex);
            }
            return response;
        }


        public async Task<OrderDetailsViewModel> GetBuyerOrderDetailsAsync(int orderId, UserContext userContext)
        {
            CheckEntityAccess(userContext, orderId, EntityType.Order);
            OrderDetailsViewModel response = new OrderDetailsViewModel();
            try
            {
                var order = await Context.DataContext.Orders.AsNoTracking()
                    .Where(t => t.Id == orderId).Select(t => new
                    {
                        ContactPersons = t.FuelRequest.Job.Users1.Select(t1 => new
                        {
                            t1.Id,
                            t1.FirstName,
                            t1.LastName,
                            t1.Email,
                            t1.PhoneNumber
                        }),
                        t.IsFTL,
                        t.FuelRequest.FuelRequestFees,
                        t.FuelRequest.Resales,
                        t.FuelRequest.ResaleCustomers,
                        t.OrderTaxDetails,
                        t.OrderAdditionalDetail,
                        t.FuelRequest.FuelRequestPricingDetail,
                        t.FuelRequest.FreightOnBoardTypeId
                    }).SingleOrDefaultAsync();

                StoredProcedureDomain storedProcedureDomain = new StoredProcedureDomain(this);
                var orderDetail = await storedProcedureDomain.GetBuyerOrderDetailAsync(userContext.CompanyId, orderId);
                if (order != null && orderDetail != null)
                {
                    var helperDomain = new HelperDomain(this);
                    response = new OrderDetailsViewModel(Status.Success);
                    response.Id = orderDetail.Id;
                    response.FuelRequestId = orderDetail.FuelRequestId;
                    response.CompanyId = orderDetail.BuyerCompanyId;
                    response.IsProFormaPo = orderDetail.IsProFormaPo;
                    response.PoNumber = orderDetail.PoNumber;
                    response.StatusId = orderDetail.StatusId;
                    response.StatusName = orderDetail.StatusName;
                    response.SiteInstructions = orderDetail.SiteInstructions;
                    response.JobName = orderDetail.JobName;
                    response.DisplayJobID = orderDetail.DisplayJobID;
                    response.JobId = orderDetail.JobId;
                    response.JobStateId = orderDetail.JobStateId;
                    response.IsJobResaleEnabled = orderDetail.IsResaleEnabled;
                    response.JobEndDate = orderDetail.JobEndDate;
                    response.SupplierCompanyName = orderDetail.SupplierCompanyName;
                    response.AccountingCompanyId = orderDetail.AccountingCompanyId;
                    response.CompanyCountryId = orderDetail.CompanyCountryId;
                    response.JobLocation = new AddressViewModel()
                    {
                        Address = orderDetail.JobAddress,
                        City = orderDetail.JobCity,
                        StateCode = orderDetail.JobStateCode,
                        ZipCode = orderDetail.JobZipCode,
                        LocationType = orderDetail.JobLocationType
                    };
                    response.Supplier = new ContactPersonViewModel()
                    {
                        Name = $"{orderDetail.SupplerFirstName} {orderDetail.SupplerLastName}",
                        Email = orderDetail.SupplerEmail,
                        PhoneNumber = orderDetail.SupplerPhoneNumber
                    };

                    response.DriverId = orderDetail.DriverId == null ? null : orderDetail.DriverId;
                    response.DriverName = orderDetail.DriverId == null ? Resource.lblNoDriverAssigned : $"{orderDetail.DriverFirstName} {orderDetail.DriverLastName}";

                    var splitDelimiter = new string[] { "::" };
                    response.ContactPersons = order.ContactPersons.Select(t => new ContactPersonViewModel() { Id = t.Id, Name = $"{t.FirstName} {t.LastName}", Email = t.Email, PhoneNumber = t.PhoneNumber }).ToList();
                    if (!string.IsNullOrWhiteSpace(orderDetail.Qualifications))
                    {
                        response.Qualifications = orderDetail.Qualifications.Split(splitDelimiter, StringSplitOptions.RemoveEmptyEntries).Select(t => Convert.ToInt32(t)).ToList();
                    }
                    response.AssetsAssigned = orderDetail.AssetsAssigned;
                    response.GallonsOrdered = orderDetail.BrokeredMaxQuantity ?? orderDetail.MaxQuantity;
                    response.InvoicedAmount = orderDetail.InvoicedAmount;
                    response.DropTicketAmount = orderDetail.DropTicketAmount;
                    response.OrderTotalAmount = orderDetail.OrderTotalAmount.ToString(ApplicationConstants.DecimalFormat2);
                    response.FuelDeliveryDetails = orderDetail.ToFuelDeliveryDetailsViewModel();
                    //response.PricePerGallon = helperDomain.GetPricePerGallon(orderDetail.PricePerGallon, orderDetail.PricingTypeId, orderDetail.RackAvgTypeId ?? 0);
                    response.PricePerGallon = orderDetail.DisplayPricePerGallon;
                    response.FuelRequestFees = order.FuelRequestFees.ToViewModel();
                    if (!string.IsNullOrWhiteSpace(orderDetail.SpecialInstructions))
                    {
                        response.FuelDeliveryDetails.SpecialInstructions = orderDetail.SpecialInstructions
                        .Split(splitDelimiter, StringSplitOptions.RemoveEmptyEntries)
                        .Select(t => new SpecialInstructionViewModel { Instruction = t }).ToList();
                    }

                    // get pricing details from pricing service
                    PricingRequestDetailResponseViewModel pricingDetails = new PricingRequestDetailResponseViewModel();  // need to check is this needed ?
                    if (orderDetail.RequestPriceDetailId > 0)
                    {
                        pricingDetails = await GetRequestPricingDetail(orderDetail.RequestPriceDetailId, (int)orderDetail.Currency, orderDetail.AcceptedCompanyId, orderDetail.FuelTypeId, orderDetail.JobStateId);
                        if (pricingDetails != null)
                        {
                            if (pricingDetails.TierPricings == null && !pricingDetails.TierPricings.Any())
                            {
                                orderDetail.PricingTypeId = pricingDetails.PricingTypeId;
                                orderDetail.RackAvgTypeId = pricingDetails.RackAvgTypeId;
                                orderDetail.PricePerGallon = pricingDetails.PricePerGallon;
                                orderDetail.SupplierCost = pricingDetails.SupplierCost;
                                orderDetail.PricingCodeId = response.PricingCodeId = pricingDetails.PricingCodeId;
                                orderDetail.PricingCode = response.PricingCode = pricingDetails.PricingCode;
                                response.FuelDetails.FuelPricing.FuelPricingDetails.PricingSourceId = pricingDetails.PricingSourceId;
                            }
                            else if (pricingDetails.TierPricings != null && pricingDetails.TierPricings.Any())
                            {
                                if (pricingDetails.TierPricings.First().CumulationTypeId.HasValue)
                                {
                                    var cumulationDetails = pricingDetails.TierPricings.First();
                                    response.FuelDetails.FuelPricing.TierPricing.ResetCumulationSetting.CumulationType = (CumulationType)cumulationDetails.CumulationTypeId;
                                    response.FuelDetails.FuelPricing.TierPricing.ResetCumulationSetting.Date = cumulationDetails.CumulationResetDate;
                                    response.FuelDetails.FuelPricing.TierPricing.ResetCumulationSetting.Day = (WeekDay)cumulationDetails.CumulationResetDay;
                                    var cumulationDisplayLabel = new HelperDomain().GetDisplayCumulationFrequencyLabel(cumulationDetails.CumulationTypeId.Value, cumulationDetails.CumulationResetDate.Value, cumulationDetails.CumulationResetDay.Value);
                                    response.FuelDetails.FuelPricing.TierPricing.DisplayCumulationFrequency = cumulationDisplayLabel;
                                }
                                response.FuelDetails.FuelPricing.TierPricing.Pricings = new List<PricingViewModel>();
                                response.FuelDetails.FuelPricing.TierPricing.TierPricingType = (TierPricingType)pricingDetails.TierPricings.FirstOrDefault().TierTypeId;
                                response.FuelDetails.TierPricing.TierPricingType = response.FuelDetails.FuelPricing.TierPricing.TierPricingType;

                                List<DropdownDisplayItem> externalTerminals = helperDomain.GetCityRackTerminalNameByIds(pricingDetails);
                                foreach (var item in pricingDetails.TierPricings)
                                {
                                    var model = new PricingViewModel();
                                    model.PricingSourceId = item.PricingSourceId;
                                    model.PricingTypeId = item.PricingTypeId;
                                    model.RackAvgTypeId = item.RackAvgTypeId;
                                    //model.RackTypeId = item.RackTypeId;
                                    model.PricingCode.Id = item.PricingCodeId;
                                    //model.SupplierCost = item.SupplierCost;
                                    model.PricePerGallon = item.PricePerGallon;
                                    model.BasePrice = item.BasePrice;
                                    model.BaseSupplierCost = item.BaseSupplierCost;
                                    model.FromQuantity = item.MinQuantity;
                                    model.ToQuantity = item.MaxQuantity;
                                    if (externalTerminals != null && externalTerminals.Any() && item.CityRackTerminalId != null && item.CityRackTerminalId != 0)
                                    {
                                        model.CityGroupTerminalName = externalTerminals.Where(w => w.Id == item.CityRackTerminalId).FirstOrDefault().Name;
                                    }

                                    var rackAvgTypeId = item.RackAvgTypeId.HasValue ? item.RackAvgTypeId.Value : 0;
                                    var pricingTypeId = item.PricingTypeId;
                                    if (item.PricingSourceId == (int)PricingSource.Axxis && item.PricingTypeId == (int)PricingType.RackAverage)
                                    {
                                        pricingTypeId = item.RackTypeId;
                                    }
                                    else if (item.PricingTypeId == (int)PricingType.Suppliercost)
                                    {
                                        rackAvgTypeId = item.SupplierCostTypeId ?? 0;
                                    }

                                    model.DisplayPrice = helperDomain.GetPricePerGallon(item.PricePerGallon, pricingTypeId, rackAvgTypeId);
                                    if (rackAvgTypeId != 0)
                                    {
                                        model.DisplayPrice = $"{model.DisplayPrice},{Enum.GetName(typeof(PricingSource), item.PricingSourceId)}";
                                    }
                                    response.FuelDetails.FuelPricing.TierPricing.Pricings.Add(model);
                                }
                                response.FuelDetails.IsTierPricing = true;

                            }
                            response.PricingCodeDescription = orderDetail.PricingCodeDescription;

                        }
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errorMessagePricingRequestDetailsNotAvailable;
                    }
                    var IsTierPricing = response.FuelDetails.IsTierPricing;
                    var tierPricings = new List<PricingViewModel>();
                    tierPricings = response.FuelDetails.IsTierPricing ? response.FuelDetails.FuelPricing.TierPricing.Pricings : null;
                    var cumulationFrequencydisplaylable = response.FuelDetails.FuelPricing.TierPricing.DisplayCumulationFrequency;
                    response.FuelDetails = orderDetail.ToFuelDetailsViewModel();
                    if (IsTierPricing && tierPricings != null
                        && tierPricings.Any())
                    {
                        response.FuelDetails.FuelPricing.TierPricing.Pricings = tierPricings;
                        if (!string.IsNullOrWhiteSpace(cumulationFrequencydisplaylable))
                        {
                            response.FuelDetails.FuelPricing.TierPricing.DisplayCumulationFrequency = cumulationFrequencydisplaylable;
                        }
                        response.FuelDetails.IsTierPricing = true;
                    }

                    if (pricingDetails != null)
                    {
                        response.FuelDetails.FuelPricing.FuelPricingDetails.PricingSourceId = pricingDetails.PricingSourceId;
                    }
                    response.FuelDetails.FuelPricing.FuelPricingDetails = order.FuelRequestPricingDetail.ToViewModel(response.FuelDetails.FuelPricing.FuelPricingDetails);

                    response.FuelDeliveredPercentage = helperDomain.CheckQuantityValid(response.GallonsOrdered, orderDetail.DeliveredPercentage);
                    response.TerminalName = orderDetail.TerminalName == null ? Resource.lblHyphen : orderDetail.TerminalName;

                    response.FuelDeliveryDetails.SpecialInstructionFiles = GetSpecialInstructionFileDetails(orderDetail.Id, orderDetail.FileDetails);
                    response.FuelDetails.IsFTLEnabled = order.IsFTL;
                    response.FuelDetails.FreightOnBoard = order.FreightOnBoardTypeId;

                    if (order.FuelRequestFees.Any())
                    {
                        response.FuelDeliveryDetails.FuelFees.FuelRequestFees = order.FuelRequestFees.ToFeesViewModel();
                    }

                    if (order.OrderTaxDetails != null)
                    {
                        response.TaxDetailsViewModel = order.OrderTaxDetails.Where(t => t.IsActive).ToList().ToViewModel();
                    }
                    response.IsTaxExempted = orderDetail.IsTaxExempted;
                    response.Distance = orderDetail.TerminalName == null ? 0 : orderDetail.Distance;
                    response.FuelType = orderDetail.FuelTypeName;
                    response.IsHidePricingEnabled = orderDetail.IsHidePricingEnabledForBuyer;
                    response.FuelRequestResale = GetFuelRequestResale(order.Resales, order.ResaleCustomers);
                    response.ResaleFee = order.FuelRequestFees.Where(t => t.FeeTypeId == (int)FeeType.ResaleFee).Select(t => t.ToResaleFeeViewModel()).ToList();
                    response.EstimatedGallonsPerDelivery = orderDetail.EstimateGallonsPerDelivery;
                    response.TypeOfFuel = orderDetail.ProductDisplayGroupId;
                    response.ProductDescription = orderDetail.FuelDescription;
                    response.OrderClosingThreshold = orderDetail.OrderClosingThreshold;
                    if (orderDetail.DeliveryTypeId == (int)DeliveryType.MultipleDeliveries)
                    {
                        response.NextDeliverySchedule = await GetNextDeliveryScheduleDetails(orderId);
                        if (orderDetail.IsProFormaPo)
                        {
                            response.AllowPoEdit = false;
                        }
                    }

                    if (order.OrderAdditionalDetail != null)
                    {
                        if (!string.IsNullOrWhiteSpace(order.OrderAdditionalDetail.Notes))
                        {
                            if (response.OrderAdditionalDetails == null)
                            {
                                response.OrderAdditionalDetails = new OrderAdditionalDetailsViewModel();
                            }

                            response.OrderAdditionalDetails.Notes = order.OrderAdditionalDetail.Notes;
                            response.OrderAdditionalDetails.SupplierAssignedProductName = orderDetail.SupplierAssignedProductName;
                        }
                    }
                    response = response.CorrectValues();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetBuyerOrderDetailsAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetNearestCustomerByFuelType(ApiNearestCustomerByFuelTypeModel viewModel)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var spDomain = new StoredProcedureDomain(this);
                response = await spDomain.GetNearestCustomerByFuelType(viewModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetNearestCustomerByFuelType", ex.Message, ex);
            }
            return response;
        }

        public async Task<ApiDriverOrderForJobViewModel> GetOrderForJobAsync(int jobId)
        {
            HelperDomain helperDomain = new HelperDomain(this);
            var response = new ApiDriverOrderForJobViewModel();
            try
            {
                var job = await Context.DataContext.Jobs.SingleOrDefaultAsync(t => t.Id == jobId);
                if (job != null)
                {
                    var allOrders = Context.DataContext.Orders.Where(t => t.IsActive && t.ParentId == null
                                    && t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open
                                    && t.FuelRequest.Job.Id == jobId);

                    var orders = new List<ApiOrderDetailsForJobViewModel>();
                    ApiOrderDetailsForJobViewModel orderDetail;
                    var filteredOrders = allOrders.OrderByDescending(t => t.Id).ToList();
                    foreach (var t in filteredOrders)
                    {
                        orderDetail = new ApiOrderDetailsForJobViewModel();
                        orderDetail.OrderId = t.Id;
                        orderDetail.FuelType = helperDomain.GetProductName(t.FuelRequest.MstProduct);
                        orderDetail.FuelTypeId = t.FuelRequest.FuelTypeId;
                        orderDetail.PoNumber = t.PoNumber;
                        orderDetail.TerminalId = t.TerminalId;
                        orderDetail.IsBOLImageRequired = t.OrderAdditionalDetail != null ? t.OrderAdditionalDetail.IsDriverToUpdateBOL : false;
                        orderDetail.IsSignatureRequired = t.SignatureEnabled;
                        orders.Add(orderDetail);
                    }

                    response.Orders = orders;
                    response.AssetCount = job.JobXAssets.Count(t => t.RemovedBy == null && t.Asset.Type == (int)AssetType.Asset);

                    var allProductTypeMapping = Context.DataContext.ProductTypeCompatibilityMappings.Select(t => new DropdownDisplayExtendedId { Id = t.ProductTypeId, CodeId = t.MappedToProductTypeId }).ToList();
                    var blendProductTypeMapping = Context.DataContext.MstBlendProductTypeMapping.Select(t => new DropdownDisplayExtendedId { Id = t.ProductTypeId, CodeId = t.MappedToProductTypeId }).ToList();
                    var allAssets = Context.DataContext.Assets.Include(t => t.AssetAdditionalDetail).Include(t => t.Image).Include(t => t.JobXAssets).Include("JobXAssets.Job")
                                       .Where(t => t.IsActive && t.Type == (int)AssetType.Tank && t.Company.Id == job.CompanyId);

                    allAssets = allAssets.Where(t => t.JobXAssets.Any(t1 => t1.JobId == jobId && t1.RemovedBy == null && t1.RemovedDate == null));

                    var tankIds = allAssets.Select(t => t.Id).ToList();
                    var tankAdditionalList = await new FreightServiceDomain(this).GetTankList(tankIds);

                    List<ApiTankDetailViewModel> tankList = new List<ApiTankDetailViewModel>();
                    foreach (var asset in allAssets)
                    {
                        ApiTankDetailViewModel tank = new ApiTankDetailViewModel();
                        if (tankAdditionalList != null)
                        {
                            var tankViewModel = tankAdditionalList.FirstOrDefault(t => t.AssetId == asset.Id);
                            if (tankViewModel != null)
                            {
                                //Set JobXAssetId and UoM
                                var jobXAssetId = 0;
                                var activeJobXAsset = asset.JobXAssets.FirstOrDefault(t => t.RemovedBy == null && t.RemovedDate == null);
                                if (activeJobXAsset != null)
                                {
                                    jobXAssetId = activeJobXAsset.Id;
                                    tank.UoM = activeJobXAsset.Job.UoM.ToString();
                                }

                                tank = tankViewModel.ToApiTankViewModel(tank.UoM);
                                tank.JobXAssetId = jobXAssetId;

                                //Set MappedToProductTypeId
                                tank.MappedToProductTypeId = allProductTypeMapping.Where(t => t.Id == tank.ProducTypeId).Select(t => t.CodeId).Distinct().ToList();
                                tank.MappedToBlendProductTypeId = blendProductTypeMapping.Where(t => t.Id == tank.ProducTypeId).Select(t => t.CodeId).Distinct().ToList();
                                tankList.Add(tank);
                            }
                        }
                    }

                    response.Tanks = tankList;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetOrderForJobAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> ChangeDriverAcknowledgement(int trackableScheduleId, DriverAcknowledgementStatus status, int userTimeOffset, int? groupId)
        {
            StatusViewModel result = new StatusViewModel(Status.Failed);
            try
            {
                if (trackableScheduleId > 0)
                {
                    var trackableSchedule = await Context.DataContext.DeliveryScheduleXTrackableSchedules.SingleOrDefaultAsync(t => t.Id == trackableScheduleId && t.IsActive);
                    trackableSchedule.DeliveryStatus = (int)status;
                    trackableSchedule.DeliveryStatusUpdatedDate = DateTimeOffset.Now.ToOffset(TimeSpan.FromMinutes(userTimeOffset));

                    Context.DataContext.Entry(trackableSchedule).State = EntityState.Modified;
                    await Context.CommitAsync();
                    if (!string.IsNullOrWhiteSpace(trackableSchedule.FrDeliveryRequestId) && trackableSchedule.DeliveryScheduleStatusId != (int)TrackableDeliveryScheduleStatus.Canceled && (status == DriverAcknowledgementStatus.Acknowledged || status == DriverAcknowledgementStatus.ReAcknowledgementNeeded))
                    {
                        var deliveryReqStatus = new DeliveryReqStatusUpdateModel() { DeliveryRequestId = trackableSchedule.FrDeliveryRequestId, ScheduleStatusId = (int)TrackableDeliveryScheduleStatus.Acknowledged, UserId = trackableSchedule.DriverId ?? 1 };
                        new ScheduleBuilderDomain(this).UpdateDeliveryRequestStatus(new List<DeliveryReqStatusUpdateModel>() { deliveryReqStatus });
                    }
                    result = new StatusViewModel(Status.Success);
                }
                else if (groupId.HasValue)
                {
                    var trackableSchedules = await Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(t => t.DeliveryGroupId == groupId && t.IsActive).ToListAsync();
                    if (trackableSchedules != null && trackableSchedules.Any())
                    {
                        var deliveryRequestStatuses = new List<DeliveryReqStatusUpdateModel>();
                        foreach (var trackableSchedule in trackableSchedules)
                        {
                            if (trackableSchedule.DeliveryScheduleStatusId != (int)TrackableDeliveryScheduleStatus.Canceled && !trackableSchedule.IsDropCompleted)
                            {
                                trackableSchedule.DeliveryStatus = (int)status;
                                trackableSchedule.DeliveryStatusUpdatedDate = DateTimeOffset.Now.ToOffset(TimeSpan.FromMinutes(userTimeOffset));
                                Context.DataContext.Entry(trackableSchedule).State = EntityState.Modified;
                                await Context.CommitAsync();
                                if (!string.IsNullOrWhiteSpace(trackableSchedule.FrDeliveryRequestId) && (status == DriverAcknowledgementStatus.Acknowledged || status == DriverAcknowledgementStatus.ReAcknowledgementNeeded))
                                {
                                    deliveryRequestStatuses.Add(new DeliveryReqStatusUpdateModel() { DeliveryRequestId = trackableSchedule.FrDeliveryRequestId, ScheduleStatusId = (int)TrackableDeliveryScheduleStatus.Acknowledged, UserId = trackableSchedule.DriverId ?? 1 });
                                }
                            }
                        }
                        if (deliveryRequestStatuses.Any())
                        {
                            new ScheduleBuilderDomain(this).UpdateDeliveryRequestStatus(deliveryRequestStatuses);
                        }
                        result = new StatusViewModel(Status.Success);
                    }
                    else
                    {
                        result.StatusMessage = Resource.errNoDeliverySchedulesFound;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "MarkDeliveryScheduleRead", ex.Message, ex);
            }

            return result;
        }

        public async Task<BuyerOrderStat> GetBuyerOrderStatAsync(int orderId)
        {
            var response = new BuyerOrderStat();
            try
            {
                var isProFormaAndSingleDelivery = false;
                var order = await Context.DataContext.Orders.Where(t => t.Id == orderId)
                        .Select(t => new
                        {
                            t.FuelRequest.FuelRequestDetail.DeliveryTypeId,
                            t.FuelRequest.QuantityTypeId,
                            t.IsProFormaPo,
                            IsInvoices = t.Invoices.Any(t1 => t1.InvoiceTypeId == (int)InvoiceType.Manual || t1.InvoiceTypeId == (int)InvoiceType.MobileApp)
                        }).FirstOrDefaultAsync();
                if (order.IsProFormaPo && order.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery && order.IsInvoices)
                {
                    isProFormaAndSingleDelivery = true;
                }
                response.IsInvoicesCreated = order.IsInvoices;
                StoredProcedureDomain storedProcedureDomain = new StoredProcedureDomain(this);
                var UspResponse = await storedProcedureDomain.GetBuyerOrderStatAsync(orderId, isProFormaAndSingleDelivery);
                if (isProFormaAndSingleDelivery || order.QuantityTypeId != (int)QuantityType.NotSpecified)
                {
                    response.GallonsOrdered = UspResponse.GallonsOrdered.GetPreciseValue(2).GetCommaSeperatedValue();
                    response.GallonsRemaining = UspResponse.GallonsRemaining.GetPreciseValue(2).GetCommaSeperatedValue();
                }
                else
                {
                    response.GallonsOrdered = Resource.lblHyphen;
                    response.GallonsRemaining = Resource.lblHyphen;
                }
                response.AvgGallonsPerDelivery = UspResponse.AvgGallonsPerDelivery.GetPreciseValue(2);
                response.AvgPricePerGallon = UspResponse.AvgPricePerGallon.GetPreciseValue(2);
                response.GallonsDelivered = UspResponse.GallonsDelivered.GetPreciseValue(2);
                response.TotalInvoicedAmount = UspResponse.TotalInvoicedAmount.GetPreciseValue(2);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetBuyerOrderStatAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<OrderDetailsViewModel> GetBuyerDeliveryDetailsAsync(int orderId, int userId)
        {
            OrderDetailsViewModel response = new OrderDetailsViewModel();
            try
            {
                var order = await Context.DataContext.Orders.Include(t => t.FuelRequest.FuelRequestDetail)
                    .Include(t => t.OrderDeliverySchedules).Include(t => t.OrderXDrivers).Include("OrderXDrivers.User").SingleOrDefaultAsync(t => t.Id == orderId);
                if (order != null)
                {
                    int statusId = order.OrderXStatuses.FirstOrDefault(t => t.IsActive).StatusId;
                    var helperDomain = new HelperDomain(this);
                    response = new OrderDetailsViewModel(Status.Success);
                    response.Id = orderId;
                    response.GallonsOrdered = order.BrokeredMaxQuantity ?? order.FuelRequest.MaxQuantity;
                    response.FuelDeliveryDetails.StartDate = order.FuelRequest.FuelRequestDetail.StartDate;
                    response.FuelDeliveryDetails.EndDate = order.FuelRequest.FuelRequestDetail.EndDate;
                    response.JobEndDate = order.FuelRequest.Job.EndDate;
                    if (statusId == (int)OrderStatus.PartiallyCanceled || statusId == (int)OrderStatus.PartiallyClosed)
                    {
                        if (helperDomain.CheckForOpenBrokerOrder(order))
                        {
                            response.StatusId = (int)OrderStatus.Open;
                        }
                        else if (statusId == (int)OrderStatus.PartiallyCanceled)
                        {
                            response.StatusId = (int)OrderStatus.Canceled;
                        }
                        else
                        {
                            response.StatusId = (int)OrderStatus.Closed;
                        }
                    }
                    else
                    {
                        response.StatusId = statusId;
                    }

                    //Delivery Schedules
                    var latestSchedules = GetLatestOrderDeliverySchedule(order.OrderDeliverySchedules);
                    if (latestSchedules.Count > 0)
                    {
                        response.FuelDeliveryDetails.OrderVersion = latestSchedules.FirstOrDefault().Version;
                        var scheduleIds = latestSchedules.Select(t => t.DeliveryRequestId).ToList();
                        var fuelDispatchLocations = Context.DataContext.FuelDispatchLocations.Where(t => t.OrderId == order.Id && t.LocationType == (int)LocationType.Drop && !t.IsJobLocation && scheduleIds.Contains(t.DeliveryScheduleId)).ToList();
                        response.CurrentOrderVersionToEdit = latestSchedules.ToViewModel(fuelDispatchLocations);
                        response.CurrentOrderVersionToEdit.DeliverySchedules.ForEach(t =>
                        {
                            t.DriverName = t.DriverId == null ? Resource.lblNoDriverAssigned : helperDomain.GetAssignedDriver(t.DriverId ?? 0);
                        });
                    }

                    if (response.DriverId != null && response.CurrentOrderVersionToEdit.DeliverySchedules.Count > 0)
                    {
                        if (response.CurrentOrderVersionToEdit.DeliverySchedules.Any(t => t.DriverId != null && t.DriverId != response.DriverId))
                        {
                            response.DriverName = Resource.lblMultipleDrivers;
                        }
                    }
                    if (response.CurrentOrderVersionToEdit != null)
                    {
                        response.CurrentOrderVersionToEdit.DeliverySchedules = helperDomain.GetUndeliveredSchedules(response.CurrentOrderVersionToEdit.DeliverySchedules);
                    }
                    var user = Context.DataContext.Users.Include(t => t.MstRoles).FirstOrDefault(t => t.Id == userId);
                    if (user.MstRoles.Any(t => t.Id == (int)UserRoles.OnsitePerson) && user.MstRoles.Any(t => t.Id != (int)UserRoles.Buyer))
                    {
                        response.CurrentOrderVersionToEdit.OnsiteDeliveryRequests = response.CurrentOrderVersionToEdit.DeliverySchedules.Where(t => t.CreatedBy == userId).ToList();
                        response.CurrentOrderVersionToEdit.DeliverySchedules = response.CurrentOrderVersionToEdit.DeliverySchedules.Where(t => t.CreatedBy != userId).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetBuyerDeliveryDetailsAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<OrderDetailsViewModel> GetBuyerOrderStatusAsync(int orderId, int userId)
        {
            OrderDetailsViewModel response = new OrderDetailsViewModel();
            try
            {
                var order = await Context.DataContext.Orders.Include(t => t.FuelRequest.FuelRequestDetail).SingleOrDefaultAsync(t => t.Id == orderId);
                if (order != null)
                {
                    int statusId = order.OrderXStatuses.FirstOrDefault(t => t.IsActive).StatusId;
                    var helperDomain = new HelperDomain(this);
                    response = new OrderDetailsViewModel(Status.Success);
                    response.Culture = helperDomain.SetEntityThreadCulture(order.FuelRequest.Currency);
                    response.Id = order.Id;
                    response.StatusId = (statusId == (int)OrderStatus.PartiallyCanceled || statusId == (int)OrderStatus.PartiallyClosed) ?
                                        (helperDomain.CheckForOpenBrokerOrder(order) ? (int)OrderStatus.Open
                                         : (statusId == (int)OrderStatus.PartiallyCanceled ? (int)OrderStatus.Canceled : (int)OrderStatus.Closed)) : statusId;

                    response.FuelDeliveryDetails = order.FuelRequest.FuelRequestDetail.ToViewModel();
                    response.IsProFormaPo = order.IsProFormaPo;
                    response.BuyerCompanyId = order.BuyerCompanyId;
                    response.FuelDetails.FuelQuantity.QuantityTypeId = order.FuelRequest.QuantityTypeId;
                    if (order.OrderTaxDetails != null)
                    {
                        response.TaxDetailsViewModel = order.OrderTaxDetails.Where(t => t.IsActive).ToList().ToViewModel();
                    }
                    response.UoM = order.FuelRequest.Job.CountryId == (int)Country.CAN ? UoM.Litres : UoM.Gallons;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetBuyerOrderStatusAsync", ex.Message, ex);
            }

            return response;
        }

        private FuelRequestResaleViewModel GetFuelRequestResale(ICollection<Resale> resales, ICollection<ResaleCustomer> resaleCustomers)
        {
            FuelRequestResaleViewModel response = new FuelRequestResaleViewModel();
            var resale = resales.FirstOrDefault();
            if (resale != null)
            {
                response.FuelPricing.PricingTypeId = resale.PricingTypeId;
                response.FuelPricing.RackAvgTypeId = resale.RackAvgTypeId;

                if (resale.PricingTypeId == (int)PricingType.PricePerGallon)
                {
                    response.FuelPricing.PricePerGallon = resale.PricePerGallon.GetPreciseValue(6);
                }
                else if (resale.PricingTypeId == (int)PricingType.RackAverage
                        || resale.PricingTypeId == (int)PricingType.RackLow
                        || resale.PricingTypeId == (int)PricingType.RackHigh)
                {
                    response.FuelPricing.RackPrice = resale.PricePerGallon.GetPreciseValue(6);
                }
                else if (resale.PricingTypeId == (int)PricingType.Suppliercost)
                {
                    response.FuelPricing.SupplierCostMarkupValue = resale.PricePerGallon.GetPreciseValue(6);
                }

                response.IsDropTicketEnabled = resale.IsDDTEnabled;
                response.ResaleCustomer = resaleCustomers.Select(t => t.ToViewModel()).ToList();
            }
            return response;
        }

        public async Task<List<ContactPersonViewModel>> GetCustomerContacts(int orderId)
        {
            var response = new List<ContactPersonViewModel>();
            var orderUsers = await Context.DataContext.Orders.Where(t => t.Id == orderId).SelectMany(t => t.OrderXUsers.Select(t1 => new { t1.Id, t1.PhoneNumber, t1.Email, t1.FirstName, t1.LastName })).ToListAsync();
            foreach (var user in orderUsers)
            {
                response.Add(new ContactPersonViewModel() { Id = user.Id, Name = user.FirstName + " " + user.LastName, PhoneNumber = user.PhoneNumber, Email = user.Email });
            }
            return response;
        }

        public async Task<OrderDetailsViewModel> GetSupplierOrderDetailsAsync(int orderId, int userId, UserContext userContext, bool isBrokeredRequest = false)
        {
            CheckEntityAccess(userContext, orderId, EntityType.Order);
            return await GetOrderDetails(orderId, userId, userContext, isBrokeredRequest);
        }

        public async Task<OrderDetailsViewModel> GetOrderDetails(int orderId, int userId, UserContext userContext, bool isBrokeredRequest)
        {
            OrderDetailsViewModel response = null;
            try
            {
                StoredProcedureDomain storedProcedureDomain = new StoredProcedureDomain(this);
                var spResponse = await storedProcedureDomain.GetSupplierOrderDetailsAsync(orderId, isBrokeredRequest, userContext.CompanyId);
                var settingsDomain = new SettingsDomain(this);
                var accountingCompanyId = settingsDomain.GetAccountingCompanyIdforOrder(spResponse.BuyerCompanyId, spResponse.JobId, userContext);
                int updatedBy = spResponse.StatusUpdatedBy;
                var culture = new HelperDomain(this).SetEntityThreadCulture(spResponse.Currency);
                response = new OrderDetailsViewModel()
                {
                    Id = spResponse.Id,
                    FuelRequestId = spResponse.FuelRequestId,
                    JobId = spResponse.JobId,
                    DisplayJobID = spResponse.DisplayJobID,
                    UoM = spResponse.UoM,
                    JobStateId = spResponse.StateId,
                    PoNumber = spResponse.PoNumber,
                    TerminalId = spResponse.TerminalId,
                    CityGroupTerminalId = spResponse.CityGroupTerminalId,
                    TerminalName = spResponse.TerminalName,
                    FuelType = spResponse.FuelType,
                    IsTaxExempted = spResponse.IsTaxExempted,
                    BuyerCompanyId = spResponse.BuyerCompanyId,
                    IsProFormaPo = spResponse.IsProFormaPo,
                    JobCompanyId = spResponse.JobCompanyId,
                    GallonsOrdered = spResponse.GallonsOrdered,
                    JobEndDate = spResponse.JobEndDate,
                    IsEndSupplier = spResponse.IsEndSupplier,
                    IsDefaultInvoiceTypeManual = spResponse.IsDefaultInvoiceTypeManual,
                    EstimatedGallonsPerDelivery = spResponse.EstimatedGallonsPerDelivery,
                    FuelRequestTypeId = spResponse.FuelRequestTypeId,
                    TypeOfFuel = spResponse.TypeOfFuel,
                    ProductTypeId = spResponse.ProductTypeId,
                    ProductDescription = spResponse.ProductDescription,
                    //CanCreateInvoice = spResponse.CanCreateInvoice,
                    //SuppplierCostTypeId = spResponse.SupplierCostTypeId,
                    FuelTypeId = spResponse.FuelTypeId,
                    TfxFuelTypeId = spResponse.TfxFuelTypeId,
                    //PricingTypeId = spResponse.PricingTypeId,
                    OrderClosingThreshold = spResponse.OrderClosingThreshold,
                    JobName = spResponse.JobName,
                    JobLocation = new AddressViewModel { Address = spResponse.Address, City = spResponse.City, StateCode = spResponse.StateCode, ZipCode = spResponse.ZipCode, Latitude = spResponse.Latitude, Longitude = spResponse.Longitude },
                    Country = new CountryViewModel { Id = spResponse.CountryId, Name = spResponse.CountryCode, Code = spResponse.CountryCode, Currency = spResponse.Currency },
                    IsFTLEnabled = spResponse.IsFTL,
                    IsRetailJob = spResponse.IsRetailJob,
                    DriverId = spResponse.DriverId,
                    DriverName = spResponse.DriverName,
                    OrderAdditionalDetails = new OrderAdditionalDetailsViewModel(),
                    PaymentTermId = spResponse.PaymentTermId,
                    NetDays = spResponse.NetDays,
                    PaymentTermName = spResponse.PaymentTermName,
                    StatusId = spResponse.StatusId,
                    AssetsAssigned = spResponse.AssignedAssetCount,
                    IsAssetHistoryAvailable = spResponse.IsAssetHistoryAvailable,
                    PaymentMethod = spResponse.PaymentMethod,
                    IsSignatureEnabled = spResponse.IsSignatureEnabled,
                    RequestPriceDetailId = spResponse.RequestPriceDetailId,
                    PricingCodeDescription = spResponse.PricingCodeDescription,
                    IsActive = spResponse.IsActive,
                    GroupPoNumber = spResponse.GroupPoNumber,
                    OrderGroupId = spResponse.OrderGroupId,
                    SiteInstructions = spResponse.SiteInstructions,
                    AccountingCompanyId = accountingCompanyId,
                    PreferencesSettingId = spResponse.PreferencesSettingId,
                    CarrierCompanyName = spResponse.CarrierCompanyName,
                    OrderName = spResponse.OrderName,
                    LeadRequestId = spResponse.LeadRequestId.HasValue ? spResponse.LeadRequestId : spResponse.LeadRequestId ?? 0,
                    Vessel = spResponse.Vessel,
                    IsSupressOrderPricing = spResponse.IsSupressOrderPricing,
                    IMONumber = spResponse.IMONumber,
                    BulkPlantId = spResponse.BulkPlantId,
                    CreditCheckTypeId = spResponse.CreditCheckTypeId
                };
                response.BillToInfo = spResponse.BillToInfoViewModel(response.BillToInfo);

                // get pricing details from pricing service

                if (!isBrokeredRequest)
                {
                    if (response.StatusId == (int)OrderStatus.PartiallyCanceled)
                    {
                        response.StatusId = (int)OrderStatus.Canceled;
                    }
                    else if (response.StatusId == (int)OrderStatus.PartiallyClosed)
                    {
                        response.StatusId = (int)OrderStatus.Closed;
                    }
                }

                int statusId = spResponse.StatusId;

                HelperDomain helperDomain = new HelperDomain(this);
                response.UpdatedBy = updatedBy;
                response.BuyerUserEmail = spResponse.CustomerEmail;
                response.BuyerUserId = spResponse.CustomerId;
                response.BuyerUserFirstName = spResponse.CustomerFirstName;
                response.BuyerUserLastName = spResponse.CustomerLastName;
                response.BuyerCompanyName = spResponse.CustomerCompany;

                response.FuelDetails = spResponse.ToFuelDetailViewModel(response);
                if (spResponse.RequestPriceDetailId > 0)
                {
                    await GetRequestPricingDetail(response, spResponse);
                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errorMessagePricingRequestDetailsNotAvailable;
                }

                if (spResponse.AdditionalDetailId.HasValue)
                {
                    spResponse.ToOrderAdditionalDetailViewModel(response);
                }
                response.SourceRegion = response.ToSourceRegionsViewModel();
                response.FuelDeliveredPercentage = spResponse.QuantityTypeId == (int)QuantityType.NotSpecified ? Resource.lblHyphen : helperDomain.CheckQuantityValid(response.GallonsOrdered, spResponse.FuelDeliveredPercentage) + Resource.constSymbolPercent;
                var orderTotalAmount = helperDomain.GetOrderTotalAmount(response.PricingTypeId, spResponse.QuantityTypeId, spResponse.GallonsOrdered, spResponse.PricePerGallon);
                response.OrderTotalAmount = orderTotalAmount.HasValue ? orderTotalAmount.Value.GetPreciseValue(2).GetCommaSeperatedValue() : Resource.lblHyphen;
                response.PricePerGallon = spResponse.DisplayPricePerGallon;
                response.FuelDeliveryDetails = spResponse.ToDeliveryDetailViewModel();

                response.Supplier = new ContactPersonViewModel()
                {
                    Name = $"{spResponse.CustomerFirstName} {spResponse.CustomerLastName}",
                    Email = spResponse.CustomerEmail,
                    PhoneNumber = spResponse.CustomerPhoneNumber
                };
                if (spResponse.IsCustomerContactExists)
                {
                    response.CustomerContacts = await GetCustomerContacts(response.Id);
                }
                response.IsHidePricingEnabled = false;
                if (spResponse.OrderXTogglePricingDetailId != null)
                {
                    response.IsHidePricingEnabled = (spResponse.FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest &&
                     spResponse.BuyerCompanyId == userContext.CompanyId) ? spResponse.IsHidePricingEnabledForBuyer.Value : spResponse.IsHidePricingEnabledForSupplier.Value;
                }

                response.CanSupplierChangeTerminal = spResponse.IsEndSupplier;
                response.IsBrokeredOrder = (spResponse.FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest
                                                || spResponse.FuelRequestTypeId == (int)FuelRequestType.FreightOnlyRequest)
                                            && spResponse.BuyerCompanyId == userContext.CompanyId;
                response.IsBrokerVisible = !(!spResponse.IsBrokerVisible || (spResponse.FuelRequestTypeId == (int)FuelRequestType.ThirdPartyRequest && (spResponse.ExternalBrokerId.HasValue
                                       || spResponse.ExternalBrokerBuySellDetailId != null)));


                if (!isBrokeredRequest && spResponse.ScheduleId > 0)
                {
                    response.ScheduleId = spResponse.ScheduleId;
                    response.ScheduleName = spResponse.ScheduleName;
                }
                if (response.JobLocation.Address == Resource.lblVarious)
                {
                    response.JobLocation = new AddressViewModel { Address = string.Empty, City = string.Empty, StateCode = spResponse.StateCode, ZipCode = string.Empty };
                }
                response.Culture = culture;
                response.FuelDeliveryDetails.IsOrderView = true;

                var fees = await GetFuelFees(response, spResponse.FuelRequestId, response.IsFTLEnabled, spResponse.Currency, spResponse.UoM);

                response.AllowPoEdit = false;
                if (spResponse.FuelRequestTypeId == (int)FuelRequestType.ThirdPartyRequest)
                {
                    if (spResponse.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery || (spResponse.DeliveryTypeId == (int)DeliveryType.MultipleDeliveries && !spResponse.IsProFormaPo))
                    {
                        response.AllowPoEdit = true;
                    }
                }
                else if (spResponse.FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest && spResponse.BuyerCompanyId == userContext.CompanyId && spResponse.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery)
                {
                    response.AllowPoEdit = true;
                }

                if (helperDomain.IsGasolineProduct(response.ProductTypeId) || response.ProductTypeId == (int)ProductTypes.ClearDiesel || response.ProductTypeId == (int)ProductTypes.ClearDiesel2 || response.ProductTypeId == (int)ProductTypes.RedDyeDiesel || response.ProductTypeId == (int)ProductTypes.RedDyeDiesel2)
                {
                    response.IsFuelSurchargeValid = true;
                }

                response.TaxDetailsViewModel = await GetOrderTaxDetailsAsync(orderId);
                if (!spResponse.IsMarineLocation)
                {
                    response.OrderBadgeDetails = await GetOrderBadgeDetailsAsync(orderId);
                }
                response.IsOtherFuelTypeTaxesGiven = response.TaxDetailsViewModel.Any();

                if (spResponse.ExternalBrokerOrderDetailId != null)
                {
                    response.ExternalBrokerId = spResponse.ExternalBrokerId ?? 0;
                    GetBrokerOrderDetails(response, spResponse);
                    response.ExternalBrokeredOrder.BrokeredOrderFee = fees.ToExternalBrokerViewModel();
                    response.IsThirdPartyHardwareUsed = true;
                }

                if (spResponse.ExternalBrokerBuySellDetailId != null)
                {
                    //what to display, confirm and then modity PPG
                    response.IsBuyAndSellOrder = true;
                    response.ExternalBrokerId = spResponse.BuySellBrokerId.Value;
                    response.ExternalBrokeredOrder.BrokerMarkUp = spResponse.BrokerMarkUp.Value;
                    response.ExternalBrokeredOrder.SupplierMarkUp = spResponse.SupplierMarkUp.Value;
                    response.ExternalBrokeredOrder.Currency = spResponse.Currency;
                }
                if (spResponse.ExternalBrokerCompany != null)
                {
                    response.ExternalBrokerCompanyName = spResponse.ExternalBrokerCompany;
                }

                if (spResponse.InvoicePreferenceId != null)
                {
                    response.IsSendFileToBroker = spResponse.InvoicePreferenceId == (int)InvoicePreference.SendBrokerDataFileToBroker;
                }

                response.FuelDeliveryDetails.SpecialInstructions = await GetFuelRequestSpecialInstructions(spResponse.FuelRequestId);
                response.FuelDeliveryDetails.SpecialInstructionFiles = GetSpecialInstructionFileDetails(spResponse.Id, spResponse.FileDetails);

                if (spResponse.PricingTypeId == (int)PricingType.Suppliercost)
                {
                    response.GlobalFuelCost = spResponse.GlobalFuelCost.GetValueOrDefault(0).GetPreciseValue(4);
                    response.CurrentFuelCost = spResponse.SupplierCost.GetValueOrDefault(0).GetPreciseValue(4);

                    if (spResponse.SupplierCostTypeId == (int)SupplierCostTypes.GlobalCost)
                    {
                        response.CalculatedPricePerGallon = helperDomain.GetCalculatedPricePerGallon(response.GlobalFuelCost, spResponse.PricePerGallon, spResponse.RackAvgTypeId ?? 0).GetPreciseValue(4);
                    }
                    else if (spResponse.SupplierCostTypeId == (int)SupplierCostTypes.SupplierCost)
                    {
                        response.CalculatedPricePerGallon = helperDomain.GetCalculatedPricePerGallon(response.CurrentFuelCost, spResponse.PricePerGallon, spResponse.RackAvgTypeId ?? 0).GetPreciseValue(4);
                    }
                }

                if (isBrokeredRequest || response.IsBrokeredOrder)
                {
                    response.StatusId = spResponse.StatusId;
                    response.Supplier = new ContactPersonViewModel()
                    {
                        Name = spResponse.SupplierName,
                        Email = spResponse.SupplierEmail,
                        PhoneNumber = spResponse.SupplierPhoneNumber
                    };
                    response.BuyerCompanyName = spResponse.SupplierCompany;
                }
                response.IsSingleDeliveryClosedOrderWithZeroPercent = (response.StatusId == (int)OrderStatus.Closed
                                                                        && response.FuelDeliveryDetails.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery
                                                                        && !spResponse.AnyInvoiceExists);

                DateTimeOffset currentDateTimeOffset = DateTimeOffset.Now.ToTargetDateTimeOffset(spResponse.TimeZoneName);
                DateTimeOffset currentDate = currentDateTimeOffset.Date;
                TimeSpan currentTime = currentDateTimeOffset.TimeOfDay;
                var orderCurrentVersion = await GetOrderLatestDeliverySchedulesAsync(orderId, currentDate, currentTime);
                if (orderCurrentVersion.Id > 0)
                {
                    response.FuelDeliveryDetails.OrderVersion = orderCurrentVersion.Version;
                    response.CurrentOrderVersionToEdit = orderCurrentVersion;
                    if (spResponse.DriverId != null && orderCurrentVersion.DeliverySchedules.Any(t => t.DriverId != spResponse.DriverId))
                    {
                        response.DriverName = Resource.lblMultipleDrivers;
                    }
                }

                if (!response.IsEndSupplier || response.IsBrokeredOrder || response.CurrentOrderVersionToEdit == null || response.IsRetailJob)
                {
                    response.DeliverySchedules = orderCurrentVersion.DeliverySchedules;
                }

                response = response.CorrectValues();
                if (response.CurrentOrderVersionToEdit != null)
                {
                    response.CurrentOrderVersionToEdit.DeliverySchedules.ForEach(t => t.IsFtlOrder = response.IsFTLEnabled);
                }
                if (statusId == (int)OrderStatus.Open)
                {
                    response.IsOriginalOrder = true;

                    if (spResponse.ParentOrderId != spResponse.Id && spResponse.ParentOrderId != null)
                    {
                        response.IsMultiOrder = false;
                        response.ParentOrderDetails = await GetSupplierOrderDetailsAsync(spResponse.ParentOrderId.Value, userId, UserContext.GetSystemUserContext(), false);
                        if (response.ParentOrderDetails != null && (response.ParentOrderDetails.StatusId == (int)OrderStatus.Canceled || response.ParentOrderDetails.StatusId == (int)OrderStatus.Closed) && response.ParentOrderDetails.UpdatedBy == userId)
                        {
                            response.IsMultiOrder = false;
                            response.StatusId = response.ParentOrderDetails.StatusId;
                        }
                        else if (response.ParentOrderDetails != null && (response.ParentOrderDetails.StatusId == (int)OrderStatus.Canceled || response.ParentOrderDetails.StatusId == (int)OrderStatus.Closed)) // different supplier than who canceled/closed the order
                        {
                            response.IsMultiOrder = true;
                            response.StatusId = response.ParentOrderDetails.StatusId;
                        }
                    }
                }
                if (response.PreferencesSettingId != null && response.PreferencesSettingId > 0)
                {
                    var preferencesSetting = await Context.DataContext.OnboardingPreferences
                                                                        .Where(t => t.Id == response.PreferencesSettingId)
                                                                        .Select(t => new
                                                                        {
                                                                            t.Id,
                                                                            t.DeliveryType,
                                                                            t.IsCustomerInvitationEnabled,
                                                                            t.IsBuySellEnabled,
                                                                            t.IsThirdPartyHardwareUsed,
                                                                            t.PreferencePricingMethod,
                                                                            t.FreightOnBoardType,
                                                                            t.IsSupressOrderPricing,
                                                                            t.IsDriverProdutDisplayEnable
                                                                        })
                                                                        .FirstOrDefaultAsync();
                    //response.IsSupressOrderPricing = (preferencesSetting != null && spResponse.FuelRequestTypeId == (int)FuelRequestType.ThirdPartyRequest) ? preferencesSetting.IsSupressOrderPricing : false;
                    response.IsSupressOrderPricing = spResponse.FuelRequestTypeId == (int)FuelRequestType.ThirdPartyRequest ? response.IsSupressOrderPricing : false;
                    response.IsDriverProdutDisplayEnable = preferencesSetting != null ? preferencesSetting.IsDriverProdutDisplayEnable : false;
                    if (response.IsSupressOrderPricing)
                    {
                        response.IsDefaultInvoiceTypeManual = false;
                    }
                    // var status = await SetDefaultInvoiceTypeForOrder(response.Id,false);
                }
                if (!spResponse.IsMarineLocation)
                {
                    var tanks = await Context.DataContext.FuelRequestTankRentalFrequencies
                                            .Where(t => t.FuelRequestId == response.FuelRequestId
                                            && (t.ActivationStatusId == (int)ActivationStatus.Created || t.ActivationStatusId == (int)ActivationStatus.Active)).ToListAsync();

                    tanks.ForEach(t => response.FuelDetails.TankFrequencies.Add(t.ToViewModel()));
                }

                if (response.FuelDetails.TankFrequencies != null && !response.FuelDetails.TankFrequencies.Any())
                {
                    var tank = new TankDetailsViewModel() { StartDate = DateTimeOffset.Now, FeeTaxDetails = new FeeTaxDetails() };
                    var tankFrequency = new TankRentalFrequencyViewModel() { FuelRequestId = response.FuelRequestId, FrequencyTypes = FrequencyTypes.Weekly };
                    tankFrequency.Tanks.Add(tank);
                    response.FuelDetails.TankFrequencies.Add(tankFrequency);
                }
                else
                {
                    response.IsTankRentalEnabled = true;
                }

                var job = await Context.DataContext.Jobs.FirstOrDefaultAsync(t => t.IsActive && t.Id == response.JobId);
                if (job != null)
                {
                    response.ContactPersons = job.Users1.Select(t => new ContactPersonViewModel() { Id = t.Id, Name = $"{t.FirstName} {t.LastName}", Email = t.Email, PhoneNumber = t.PhoneNumber }).ToList();
                }
                if (response.LeadRequestId != null && response.LeadRequestId.Value > 0)
                {
                    var generalNotes = Context.DataContext.LeadNotes.Where(l => l.LeadRequestId == response.LeadRequestId).ToList();
                    if (generalNotes != null && generalNotes.Any())
                    {
                        foreach (var notes in generalNotes)
                        {
                            var notesModel = new SourcingNotesViewModel();
                            notesModel.Id = notes.Id;
                            notesModel.LeadRequestId = notes.LeadRequestId;
                            notesModel.Note = notes.GeneralNote;
                            notesModel.CreatedBy = notes.CreatedBy;
                            notesModel.CreatedDate = Convert.ToDateTime(notes.CreatedDate.ToString()).ToShortDateString();

                            var userName = Context.DataContext.Users.Where(t => t.Id == notes.CreatedBy).Select(t => new { t.FirstName, t.LastName }).FirstOrDefault();
                            notesModel.UserName = string.Join(" ", userName.FirstName, userName.LastName);

                            response.GeneralNotesHistory.Add(notesModel);
                        }
                    }
                }
                response.StatusCode = Status.Success;


            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                LogManager.Logger.WriteException("OrderDomain", "GetSupplierOrderDetailsAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<PricingRequestDetailResponseViewModel> GetRequestPricingDetail(int requestPriceDetailId, int currency, int? acceptedCompanyId, int? fuelTypeId, int? stateId)
        {
            var pricingDetails = new PricingRequestDetailResponseViewModel();
            try
            {
                var request = new PricingDetailRequestViewModel { Id = requestPriceDetailId, AcceptedCompanyId = acceptedCompanyId, FuelTypeId = fuelTypeId, StateId = stateId, Currency = currency };
                var result = await ContextFactory.Current.GetDomain<PricingServiceDomain>().GetPricingRequestDetailByIdAsync(request);

                if (result != null && result.Status == Status.Success)
                {
                    pricingDetails = result.PricingRequestDetail;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetRequestPricingDetail", ex.Message, ex);
            }
            return pricingDetails;
        }

        private async Task GetRequestPricingDetail(OrderDetailsViewModel response, UspGetSupplierOrderDetail spResponse)
        {
            var pricingDetails = await GetRequestPricingDetail(spResponse.RequestPriceDetailId, (int)spResponse.Currency, spResponse.AcceptedCompanyId, spResponse.FuelTypeId, spResponse.StateId);
            if (pricingDetails != null)
            {
                response.SuppplierCostTypeId = pricingDetails.SupplierCostTypeId ?? (int)SupplierCostTypes.GlobalCost;
                response.PricingTypeId = spResponse.PricingTypeId = pricingDetails.PricingTypeId;
                response.PricingCodeId = pricingDetails.PricingCodeId;
                response.PricingCode = pricingDetails.PricingCode;
                spResponse.SupplierCostTypeId = pricingDetails.SupplierCostTypeId ?? (int)SupplierCostTypes.GlobalCost;
                response.SourceRegionJsonParameters = pricingDetails.SourceRegionJsonParameters;
                spResponse.PricePerGallon = pricingDetails.PricePerGallon;
                spResponse.SupplierCost = pricingDetails.SupplierCost;
                spResponse.PricingSourceId = pricingDetails.PricingSourceId;
                spResponse.FeedTypeId = pricingDetails.FeedTypeId;
                spResponse.PricingSourceQuantityIndicatorTypeId = pricingDetails.QuantityIndicatorId;
                spResponse.FuelClassTypeId = pricingDetails.FuelClassTypeId;
                spResponse.WeekendDropPricingDay = pricingDetails.WeekendPricingTypeId;
                spResponse.RackAvgTypeId = pricingDetails.RackAvgTypeId;

                response.FuelDetails.FuelPricing.FuelPricingDetails.PricingSourceId = pricingDetails.PricingSourceId;
                response.FuelDetails.FuelPricing.PricingTypeId = pricingDetails.PricingTypeId;
                response.FuelDetails.FuelPricing.RackAvgTypeId = pricingDetails.RackAvgTypeId;
                response.FuelDetails.FuelPricing.Currency = (Currency)pricingDetails.Currency;
                response.FuelDetails.FuelPricing.MarkertBasedPricingTypeId = pricingDetails.RackTypeId;
                response.FuelDetails.FuelPricing.FuelPricingDetails.PricingCode.Id = pricingDetails.PricingCodeId;
                response.FuelDetails.FuelPricing.FuelPricingDetails.RequestPriceDetailId = pricingDetails.RequestPriceDetailId;
                if (pricingDetails.PricingTypeId == (int)PricingType.PricePerGallon)
                {
                    response.FuelDetails.FuelPricing.PricePerGallon = pricingDetails.PricePerGallon.GetPreciseValue(6);
                }
                else if (pricingDetails.PricingTypeId == (int)PricingType.Suppliercost)
                {
                    response.FuelDetails.FuelPricing.SupplierCost = pricingDetails.SupplierCost;
                    response.FuelDetails.FuelPricing.SupplierCostMarkupTypeId = pricingDetails.RackAvgTypeId;
                    response.FuelDetails.FuelPricing.SupplierCostMarkupValue = pricingDetails.PricePerGallon.GetPreciseValue(6);
                }
                else if (pricingDetails.PricingTypeId == (int)PricingType.RackAverage)
                {
                    response.FuelDetails.FuelPricing.RackPrice = pricingDetails.PricePerGallon.GetPreciseValue(6);
                }

                if (pricingDetails.TierPricings != null && pricingDetails.TierPricings.Any())
                {
                    if (pricingDetails.TierPricings.First().CumulationTypeId.HasValue)
                    {
                        var cumulationDetails = pricingDetails.TierPricings.First();
                        response.FuelDetails.FuelPricing.TierPricing.ResetCumulationSetting.CumulationType = (CumulationType)cumulationDetails.CumulationTypeId;
                        response.FuelDetails.FuelPricing.TierPricing.ResetCumulationSetting.Date = cumulationDetails.CumulationResetDate;
                        response.FuelDetails.FuelPricing.TierPricing.ResetCumulationSetting.Day = (WeekDay)cumulationDetails.CumulationResetDay;
                        var cumulationDisplayLabel = new HelperDomain().GetDisplayCumulationFrequencyLabel(cumulationDetails.CumulationTypeId.Value, cumulationDetails.CumulationResetDate.Value, cumulationDetails.CumulationResetDay.Value);
                        response.FuelDetails.FuelPricing.TierPricing.DisplayCumulationFrequency = cumulationDisplayLabel;
                    }
                    response.FuelDetails.FuelPricing.TierPricing.Pricings = new List<PricingViewModel>();
                    response.FuelDetails.FuelPricing.TierPricing.TierPricingType = (TierPricingType)pricingDetails.TierPricings.FirstOrDefault().TierTypeId;
                    response.FuelDetails.TierPricing.TierPricingType = response.FuelDetails.FuelPricing.TierPricing.TierPricingType;
                    var helperDomain = new HelperDomain();
                    List<DropdownDisplayItem> externalTerminals = helperDomain.GetCityRackTerminalNameByIds(pricingDetails);

                    foreach (var item in pricingDetails.TierPricings)
                    {
                        var model = new PricingViewModel();
                        model.PricingSourceId = item.PricingSourceId;
                        model.PricingTypeId = item.PricingTypeId;
                        model.RackAvgTypeId = item.RackAvgTypeId;
                        //model.RackTypeId = item.RackTypeId;
                        model.PricingCode.Id = item.PricingCodeId;
                        //model.SupplierCost = item.SupplierCost;
                        model.PricePerGallon = item.PricePerGallon;
                        model.BasePrice = item.BasePrice;
                        model.BaseSupplierCost = item.BaseSupplierCost;
                        model.FromQuantity = item.MinQuantity;
                        model.ToQuantity = item.MaxQuantity;
                        if (externalTerminals != null && externalTerminals.Any() && item.CityRackTerminalId != null && item.CityRackTerminalId != 0)
                        {
                            model.CityGroupTerminalName = externalTerminals.Where(w => w.Id == item.CityRackTerminalId).FirstOrDefault().Name;
                        }

                        var rackAvgTypeId = item.RackAvgTypeId.HasValue ? item.RackAvgTypeId.Value : 0;
                        var pricingTypeId = item.PricingTypeId;
                        if (item.PricingSourceId == (int)PricingSource.Axxis && item.PricingTypeId == (int)PricingType.RackAverage)
                        {
                            pricingTypeId = item.RackTypeId;
                        }
                        else if (item.PricingTypeId == (int)PricingType.Suppliercost)
                        {
                            rackAvgTypeId = item.SupplierCostTypeId ?? 0;
                        }

                        model.DisplayPrice = helperDomain.GetPricePerGallon(item.PricePerGallon, pricingTypeId, rackAvgTypeId);
                        if (rackAvgTypeId != 0)
                        {
                            model.DisplayPrice = $"{model.DisplayPrice},{Enum.GetName(typeof(PricingSource), item.PricingSourceId)}";
                        }
                        response.FuelDetails.FuelPricing.TierPricing.Pricings.Add(model);
                    }
                    response.FuelDetails.IsTierPricing = true;
                }
                if (spResponse.IsActive && pricingDetails.PricingTypeId == (int)PricingType.Suppliercost && pricingDetails.SupplierCost != null)
                {
                    response.CanCreateInvoice = true;
                }
                else
                {
                    response.CanCreateInvoice = false;
                }
            }
            else
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = Resource.errorMessagePricingRequestDetailsNotAvailable;
            }
        }

        private async Task<List<UspGetFuelRequestFeeDetailViewModel>> GetFuelFees(OrderDetailsViewModel viewModel, int fuelRequestId, bool isFTLEnabled, Currency currency, UoM uoM)
        {
            var fees = await GetFuelRequestFeeDetailsAsync(fuelRequestId);
            viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees = fees.ToFeesViewModel();
            viewModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee = fees.ToSurchargeFreightFeesViewModel();
            viewModel.FuelDeliveryDetails.FuelFees.Currency = currency;
            viewModel.FuelDeliveryDetails.FuelFees.UoM = uoM;
            viewModel.FuelDeliveryDetails.FuelFees.TruckLoadType = isFTLEnabled ? TruckLoadTypes.FullTruckLoad : TruckLoadTypes.LessTruckLoad;

            if (isFTLEnabled)
            {
                viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.ForEach(t => t.TruckLoadType = (int)TruckLoadTypes.FullTruckLoad);
            }
            return fees;
        }

        public async Task<OrderDetailsViewModel> GetDriverOrderDetailsAsync(int orderId, int userId, UserContext userContext)
        {
            CheckEntityAccess(userContext, orderId, EntityType.Order);

            OrderDetailsViewModel response = null;
            try
            {
                StoredProcedureDomain storedProcedureDomain = new StoredProcedureDomain(this);
                var spResponse = await storedProcedureDomain.GetSupplierOrderDetailsAsync(orderId, false, userContext.CompanyId);
                int updatedBy = spResponse.StatusUpdatedBy;
                var culture = new HelperDomain(this).SetEntityThreadCulture(spResponse.Currency);
                response = new OrderDetailsViewModel()
                {
                    Id = spResponse.Id,
                    FuelRequestId = spResponse.FuelRequestId,
                    JobId = spResponse.JobId,
                    DisplayJobID = spResponse.DisplayJobID,
                    UoM = spResponse.UoM,
                    JobStateId = spResponse.StateId,
                    PoNumber = spResponse.PoNumber,
                    TerminalId = spResponse.TerminalId,
                    CityGroupTerminalId = spResponse.CityGroupTerminalId,
                    TerminalName = spResponse.TerminalName,
                    FuelType = spResponse.FuelType,
                    IsTaxExempted = spResponse.IsTaxExempted,
                    BuyerCompanyId = spResponse.BuyerCompanyId,
                    IsProFormaPo = spResponse.IsProFormaPo,
                    JobCompanyId = spResponse.JobCompanyId,
                    GallonsOrdered = spResponse.GallonsOrdered,
                    JobEndDate = spResponse.JobEndDate,
                    IsEndSupplier = spResponse.IsEndSupplier,
                    IsDefaultInvoiceTypeManual = spResponse.IsDefaultInvoiceTypeManual,
                    EstimatedGallonsPerDelivery = spResponse.EstimatedGallonsPerDelivery,
                    FuelRequestTypeId = spResponse.FuelRequestTypeId,
                    TypeOfFuel = spResponse.TypeOfFuel,
                    ProductDescription = spResponse.ProductDescription,
                    //CanCreateInvoice = spResponse.CanCreateInvoice,
                    //SuppplierCostTypeId = spResponse.SupplierCostTypeId,
                    FuelTypeId = spResponse.FuelTypeId,
                    //PricingTypeId = spResponse.PricingTypeId,
                    OrderClosingThreshold = spResponse.OrderClosingThreshold,
                    JobName = spResponse.JobName,
                    JobLocation = new AddressViewModel { Address = spResponse.Address, City = spResponse.City, StateCode = spResponse.StateCode, ZipCode = spResponse.ZipCode },
                    Country = new CountryViewModel { Id = spResponse.CountryId, Code = spResponse.CountryCode, Currency = spResponse.Currency },
                    IsFTLEnabled = spResponse.IsFTL,
                    DriverId = spResponse.DriverId,
                    DriverName = spResponse.DriverName,
                    OrderAdditionalDetails = new OrderAdditionalDetailsViewModel(),
                    PaymentTermId = spResponse.PaymentTermId,
                    NetDays = spResponse.NetDays,
                    PaymentTermName = spResponse.PaymentTermName,
                    StatusId = spResponse.StatusId,
                    AssetsAssigned = spResponse.AssignedAssetCount,
                    IsAssetHistoryAvailable = spResponse.IsAssetHistoryAvailable,
                    PaymentMethod = spResponse.PaymentMethod,
                    IsSignatureEnabled = spResponse.IsSignatureEnabled,
                    RequestPriceDetailId = spResponse.RequestPriceDetailId,
                    PricingCodeDescription = spResponse.PricingCodeDescription,
                    IsActive = spResponse.IsActive,
                    IsSupressOrderPricing = spResponse.IsSupressOrderPricing
                };

                // get pricing details from pricing service
                if (spResponse.RequestPriceDetailId > 0)
                {
                    await GetRequestPricingDetail(response, spResponse);
                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errorMessagePricingRequestDetailsNotAvailable;
                }

                if (response.StatusId == (int)OrderStatus.PartiallyCanceled)
                {
                    response.StatusId = (int)OrderStatus.Canceled;
                }
                else if (response.StatusId == (int)OrderStatus.PartiallyClosed)
                {
                    response.StatusId = (int)OrderStatus.Closed;
                }

                int statusId = spResponse.StatusId;

                HelperDomain helperDomain = new HelperDomain(this);
                response.UpdatedBy = updatedBy;
                response.BuyerUserEmail = spResponse.CustomerEmail;
                response.BuyerUserId = spResponse.CustomerId;
                response.BuyerUserFirstName = spResponse.CustomerFirstName;
                response.BuyerUserLastName = spResponse.CustomerLastName;
                response.BuyerCompanyName = spResponse.CustomerCompany;
                response.FuelDetails = spResponse.ToFuelDetailViewModel();
                if (spResponse.AdditionalDetailId.HasValue)
                {
                    spResponse.ToOrderAdditionalDetailViewModel(response);
                }
                response.FuelDeliveredPercentage = spResponse.QuantityTypeId == (int)QuantityType.NotSpecified ? Resource.lblHyphen : helperDomain.CheckQuantityValid(response.GallonsOrdered, spResponse.FuelDeliveredPercentage) + Resource.constSymbolPercent;
                response.OrderTotalAmount = spResponse.OrderTotalAmount.HasValue ? spResponse.OrderTotalAmount.Value.GetPreciseValue(2).GetCommaSeperatedValue() : Resource.lblHyphen;
                response.PricePerGallon = spResponse.DisplayPricePerGallon;
                response.FuelDeliveryDetails = spResponse.ToDeliveryDetailViewModel();

                response.Supplier = new ContactPersonViewModel()
                {
                    Name = $"{spResponse.CustomerFirstName} {spResponse.CustomerLastName}",
                    Email = spResponse.CustomerEmail,
                    PhoneNumber = spResponse.CustomerPhoneNumber
                };
                response.IsHidePricingEnabled = false;
                if (spResponse.OrderXTogglePricingDetailId != null)
                {
                    response.IsHidePricingEnabled = (spResponse.FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest &&
                     spResponse.BuyerCompanyId == userContext.CompanyId) ? spResponse.IsHidePricingEnabledForBuyer.Value : spResponse.IsHidePricingEnabledForSupplier.Value;
                }

                response.CanSupplierChangeTerminal = spResponse.IsEndSupplier;
                response.IsBrokeredOrder = (spResponse.FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest || spResponse.FuelRequestTypeId == (int)FuelRequestType.FreightOnlyRequest)
                                            && spResponse.BuyerCompanyId == userContext.CompanyId;
                response.IsBrokerVisible = !(!spResponse.IsBrokerVisible || (spResponse.FuelRequestTypeId == (int)FuelRequestType.ThirdPartyRequest && (spResponse.ExternalBrokerId.HasValue
                                       || spResponse.ExternalBrokerBuySellDetailId != null)));

                if (spResponse.ScheduleId > 0)
                {
                    response.ScheduleId = spResponse.ScheduleId;
                    response.ScheduleName = spResponse.ScheduleName;
                }
                if (response.JobLocation.Address == Resource.lblVarious)
                {
                    response.JobLocation = new AddressViewModel { Address = string.Empty, City = string.Empty, StateCode = spResponse.StateCode, ZipCode = string.Empty };
                }
                response.Culture = culture;
                response.FuelDeliveryDetails.IsOrderView = true;

                var fees = await GetFuelRequestFeeDetailsAsync(spResponse.FuelRequestId);
                response.FuelDeliveryDetails.FuelFees.FuelRequestFees = fees.ToFeesViewModel();
                response.FuelDeliveryDetails.FuelFees.Currency = spResponse.Currency;
                response.FuelDeliveryDetails.FuelFees.UoM = spResponse.UoM;
                response.FuelDeliveryDetails.FuelFees.TruckLoadType = response.IsFTLEnabled ? TruckLoadTypes.FullTruckLoad : TruckLoadTypes.LessTruckLoad;

                if (response.IsFTLEnabled)
                {
                    response.FuelDeliveryDetails.FuelFees.FuelRequestFees.ForEach(t => t.TruckLoadType = (int)TruckLoadTypes.FullTruckLoad);
                }

                response.AllowPoEdit = false;
                if (spResponse.FuelRequestTypeId == (int)FuelRequestType.ThirdPartyRequest)
                {
                    if (spResponse.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery || (spResponse.DeliveryTypeId == (int)DeliveryType.MultipleDeliveries && !spResponse.IsProFormaPo))
                    {
                        response.AllowPoEdit = true;
                    }
                }
                else if (spResponse.FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest && spResponse.BuyerCompanyId == userContext.CompanyId && spResponse.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery)
                {
                    response.AllowPoEdit = true;
                }

                response.TaxDetailsViewModel = await GetOrderTaxDetailsAsync(orderId);
                response.IsOtherFuelTypeTaxesGiven = response.TaxDetailsViewModel.Any();

                if (spResponse.ExternalBrokerOrderDetailId != null)
                {
                    response.ExternalBrokerId = spResponse.ExternalBrokerId ?? 0;
                    GetBrokerOrderDetails(response, spResponse);
                    response.ExternalBrokeredOrder.BrokeredOrderFee = fees.ToExternalBrokerViewModel();
                    response.IsThirdPartyHardwareUsed = true;
                }

                if (spResponse.ExternalBrokerBuySellDetailId != null)
                {
                    //what to display, confirm and then modity PPG
                    response.IsBuyAndSellOrder = true;
                    response.ExternalBrokerId = spResponse.BuySellBrokerId.Value;
                    response.ExternalBrokeredOrder.BrokerMarkUp = spResponse.BrokerMarkUp.Value;
                    response.ExternalBrokeredOrder.SupplierMarkUp = spResponse.SupplierMarkUp.Value;
                    response.ExternalBrokeredOrder.Currency = spResponse.Currency;
                }
                if (spResponse.ExternalBrokerCompany != null)
                {
                    response.ExternalBrokerCompanyName = spResponse.ExternalBrokerCompany;
                }

                if (spResponse.InvoicePreferenceId != null)
                {
                    response.IsSendFileToBroker = spResponse.InvoicePreferenceId == (int)InvoicePreference.SendBrokerDataFileToBroker;
                }

                response.FuelDeliveryDetails.SpecialInstructions = await GetFuelRequestSpecialInstructions(spResponse.FuelRequestId);

                if (spResponse.PricingTypeId == (int)PricingType.Suppliercost)
                {
                    var globalCost = spResponse.GlobalFuelCost;
                    response.GlobalFuelCost = globalCost.GetValueOrDefault(0).GetPreciseValue(2);
                    response.CurrentFuelCost = spResponse.SupplierCost.GetValueOrDefault(0).GetPreciseValue(2);

                    if (spResponse.SupplierCostTypeId == (int)SupplierCostTypes.GlobalCost)
                    {
                        response.CalculatedPricePerGallon = helperDomain.GetCalculatedPricePerGallon(response.GlobalFuelCost, spResponse.PricePerGallon, spResponse.RackAvgTypeId ?? 0).GetPreciseValue(4);
                    }
                    else if (spResponse.SupplierCostTypeId == (int)SupplierCostTypes.SupplierCost)
                    {
                        response.CalculatedPricePerGallon = helperDomain.GetCalculatedPricePerGallon(response.CurrentFuelCost, spResponse.PricePerGallon, spResponse.RackAvgTypeId ?? 0).GetPreciseValue(4);
                    }
                }

                if (response.IsBrokeredOrder)
                {
                    response.StatusId = spResponse.StatusId;
                    response.Supplier = new ContactPersonViewModel()
                    {
                        Name = spResponse.SupplierName,
                        Email = spResponse.SupplierEmail,
                        PhoneNumber = spResponse.SupplierPhoneNumber
                    };
                    response.BuyerCompanyName = spResponse.SupplierCompany;
                }
                response.IsSingleDeliveryClosedOrderWithZeroPercent = (response.StatusId == (int)OrderStatus.Closed
                                                                        && response.FuelDeliveryDetails.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery
                                                                        && !spResponse.AnyInvoiceExists);

                DateTimeOffset currentDateTimeOffset = DateTimeOffset.Now.ToTargetDateTimeOffset(spResponse.TimeZoneName);
                DateTimeOffset currentDate = currentDateTimeOffset.Date;
                TimeSpan currentTime = currentDateTimeOffset.TimeOfDay;
                var orderCurrentVersion = await GetOrderLatestDeliverySchedulesAsync(orderId, currentDate, currentTime);
                if (orderCurrentVersion.Id > 0)
                {
                    response.FuelDeliveryDetails.OrderVersion = orderCurrentVersion.Version;
                    response.CurrentOrderVersionToEdit = orderCurrentVersion;
                    if (spResponse.DriverId != null && orderCurrentVersion.DeliverySchedules.Any(t => t.DriverId != spResponse.DriverId))
                    {
                        response.DriverName = Resource.lblMultipleDrivers;
                    }
                }

                if (!response.IsEndSupplier || response.IsBrokeredOrder || response.CurrentOrderVersionToEdit == null)
                {
                    response.DeliverySchedules = orderCurrentVersion.DeliverySchedules;
                }

                response = response.CorrectValues();
                response.CurrentOrderVersionToEdit.DeliverySchedules.ForEach(t => t.IsFtlOrder = response.IsFTLEnabled);

                if (statusId == (int)OrderStatus.Open)
                {
                    response.IsOriginalOrder = true;

                    if (spResponse.ParentOrderId != spResponse.Id && spResponse.ParentOrderId != null)
                    {
                        response.IsMultiOrder = false;
                        response.ParentOrderDetails = await GetSupplierOrderDetailsAsync(spResponse.ParentOrderId.Value, userId, UserContext.GetSystemUserContext(), false);
                        if (response.ParentOrderDetails != null && (response.ParentOrderDetails.StatusId == (int)OrderStatus.Canceled || response.ParentOrderDetails.StatusId == (int)OrderStatus.Closed) && response.ParentOrderDetails.UpdatedBy == userId)
                        {
                            response.IsMultiOrder = false;
                            response.StatusId = response.ParentOrderDetails.StatusId;
                        }
                        else if (response.ParentOrderDetails != null && (response.ParentOrderDetails.StatusId == (int)OrderStatus.Canceled || response.ParentOrderDetails.StatusId == (int)OrderStatus.Closed)) // different supplier than who canceled/closed the order
                        {
                            response.IsMultiOrder = true;
                            response.StatusId = response.ParentOrderDetails.StatusId;
                        }
                    }
                }

                response.StatusCode = Status.Success;
            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                LogManager.Logger.WriteException("OrderDomain", "GetDriverOrderDetailsAsync", ex.Message, ex);
            }

            return response;
        }

        private static void GetBrokerOrderDetails(OrderDetailsViewModel response, UspGetSupplierOrderDetail spResponse)
        {
            response.ExternalBrokeredOrder.ProductCode = spResponse.ProductCode;
            response.ExternalBrokeredOrder.ShipTo = spResponse.ShipTo;
            response.ExternalBrokeredOrder.Source = spResponse.Source;
            response.ExternalBrokeredOrder.VendorId = spResponse.VendorId;
            response.ExternalBrokeredOrder.CustomerNumber = spResponse.CustomerNumber;
            response.ExternalBrokeredOrder.BrokerMarkUp = spResponse.BrokerMarkUp.Value;
            response.ExternalBrokeredOrder.SupplierMarkUp = spResponse.SupplierMarkUp.Value;
            response.ExternalBrokeredOrder.ThirdPartyNozzleId = spResponse.ThirdPartyNozzleId;
        }

        public async Task<SupplierOrderStat> GetSupplierOrderStat(int orderId)
        {
            var response = new SupplierOrderStat();
            try
            {
                var isProFormaAndSingleDelivery = false;
                var order = await Context.DataContext.Orders.Where(t => t.Id == orderId)
                        .Select(t => new
                        {
                            t.FuelRequest.FuelRequestDetail.DeliveryTypeId,
                            t.FuelRequest.QuantityTypeId,
                            t.IsProFormaPo,
                            t.FuelRequest.UoM,
                            IsInvoices = t.Invoices.Any(t1 => t1.InvoiceTypeId == (int)InvoiceType.Manual || t1.InvoiceTypeId == (int)InvoiceType.MobileApp)
                        }).FirstOrDefaultAsync();
                if (order.IsProFormaPo && order.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery && order.IsInvoices)
                {
                    isProFormaAndSingleDelivery = true;
                }

                StoredProcedureDomain storedProcedureDomain = new StoredProcedureDomain(this);
                var UspResponse = await storedProcedureDomain.GetSupplierOrderStatAsync(orderId, isProFormaAndSingleDelivery);
                if (isProFormaAndSingleDelivery || order.QuantityTypeId != (int)QuantityType.NotSpecified)
                {
                    response.GallonsOrdered = UspResponse.GallonsOrdered.GetPreciseValue(2).GetCommaSeperatedValue();
                    response.DisplayUoM = order.UoM.ToString();
                }
                else
                {
                    response.GallonsOrdered = Resource.lblHyphen;
                    response.DisplayUoM = String.Empty;
                }
                response.AvgGallonsPerDelivery = UspResponse.AvgGallonsPerDelivery.GetPreciseValue(2);
                response.AvgPricePerGallon = UspResponse.AvgPricePerGallon.GetPreciseValue(2);
                response.GallonsDelivered = UspResponse.GallonsDelivered.GetPreciseValue(2);

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetSupplierOrderStat", ex.Message, ex);
            }
            return response;
        }

        public async Task<int> GetOrdersBuyerCompanyIdAsync(int orderId)
        {
            var response = 0;
            try
            {
                var order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == orderId);
                if (order != null)
                {
                    response = order.BuyerCompanyId;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetOrderDetailsAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<OrderDetailsOutPutViewModel> GetOrderDetailsAsync(int orderId)
        {
            OrderDetailsOutPutViewModel response = new OrderDetailsOutPutViewModel();
            try
            {
                var order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == orderId);
                if (order != null)
                {
                    HelperDomain helperDomain = new HelperDomain(this);
                    response = new OrderDetailsOutPutViewModel()
                    {
                        Id = order.Id,
                        FuelType = helperDomain.GetProductName(order.FuelRequest.MstProduct),
                        FuelRequestType = order.FuelRequest.IsPublicRequest ? Resource.lblPublic : Resource.lblPrivate,
                        PoNumber = order.PoNumber,
                        GallonsOrdered = order.BrokeredMaxQuantity ?? order.FuelRequest.MaxQuantity,
                        GallonsDelivered = order.Invoices.Where(t => t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && !t.IsBuyPriceInvoice).Sum(t => t.DroppedGallons),
                        UnitOfMeasurement = (int)order.FuelRequest.UoM,
                        Currency = (int)order.FuelRequest.Currency,
                        PricePerGallon = helperDomain.GetPricePerGallon(order.FuelRequest),
                        DeliveryDetails = GetDeliveryDeliveryDetails(order),
                        QuantityTypeId = order.FuelRequest.QuantityTypeId
                    };
                    //if (order.FuelRequest.PricingTypeId == (int)PricingType.Tier)
                    //{
                    //    response.DifferentFuelPrices = order.FuelRequest.DifferentFuelPrices.Select(t => t.ToViewModel()).ToList();
                    //}

                    response = response.CorrectValues();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetOrderDetailsAsync", ex.Message, ex);
            }

            return response;
        }

        private FuelDeliveryDetailsViewModel GetDeliveryDeliveryDetails(Order order)
        {
            var response = order.FuelRequest.FuelRequestDetail.ToViewModel();
            response.SpecialInstructions = order.FuelRequest.SpecialInstructions.Select(t => t.ToViewModel()).ToList();
            response.FuelRequestFee = order.FuelRequest.FuelRequestFees.ToViewModel();
            //response.FuelRequestFee.DeliveryFeeByQuantity = order.FuelRequest.FeeByQuantities.Select(t => t.ToViewModel()).ToList();
            response.FuelRequestFee.AdditionalFee = order.FuelRequest.FuelRequestFees.ToAdditionalFeeViewModel().ToList();

            response.FuelFees.Currency = order.FuelRequest.Currency;
            response.FuelFees.UoM = order.FuelRequest.UoM;
            response.FuelFees.FuelRequestFees = order.FuelRequest.FuelRequestFees.ToFeesViewModel();

            //last accepted schedules
            var acceptedSchedules = Context.DataContext.OrderVersionXDeliverySchedules
                                                         .Include(t => t.DeliverySchedule)
                                                         .Where(t => t.OrderId == order.Id && t.IsActive);

            response.DeliverySchedules = acceptedSchedules.Select(t => t.DeliverySchedule)
                                                                    .GroupBy(t => t.GroupId)
                                                                    .Select(g => new { Items = g.ToList() })
                                                                    .ToList()
                                                                    .Select(t => t.Items.ToViewModel())
                                                                    .OrderBy(t => t.ScheduleDate)
                                                                    .ToList();

            HelperDomain helperDomain = new HelperDomain(this);
            response.DeliverySchedules = helperDomain.GetUndeliveredSchedules(response.DeliverySchedules);

            //history
            var invoices = order.Invoices.Where(t => t.OrderId == order.Id && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && !t.IsBuyPriceInvoice).OrderByDescending(t => t.Id);

            var DeliveryHistory = invoices.Select(t => new DeliveryHistoryViewModel()
            {
                InvoiceId = t.Id,
                InvoiceDisplayName = t.DisplayInvoiceNumber,
                Date = t.DropStartDate.Date,
                Time = t.DropStartDate.DateTime.ToShortTimeString(),
                Quantity = t.DroppedGallons
            }).ToList();

            response.DeliveryHistory = DeliveryHistory;

            return response;
        }

        public async Task<StatusViewModel> SaveDeliveryRequestAsync(UserContext userContext, CreateDeliveryRequestInputViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel();
            HelperDomain helperDomain = new HelperDomain(this);
            NewsfeedDomain newsfeedDomain = new NewsfeedDomain(this);
            NotificationDomain notificationDomain = new NotificationDomain(this);

            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var orderstatus = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == viewModel.OrderId);
                    if (orderstatus.OrderXStatuses.FirstOrDefault(t => t.IsActive).StatusId == (int)OrderStatus.Open)
                    {
                        if (orderstatus.FuelRequest.FuelRequestDetail.DeliveryTypeId == (int)DeliveryType.MultipleDeliveries)
                        {
                            int latestGroupNumber = 0;
                            var deliverySchedules = Context.DataContext.DeliverySchedules;
                            if (deliverySchedules.Any())
                            {
                                latestGroupNumber = deliverySchedules.Max(t => t.GroupId);
                            }

                            var deliverySchedule = new DeliverySchedule();
                            deliverySchedule.Date = viewModel.StartDate.Date;
                            deliverySchedule.StartTime = Convert.ToDateTime(viewModel.StartTime).TimeOfDay;
                            deliverySchedule.EndTime = Convert.ToDateTime(viewModel.EndTime).TimeOfDay;
                            deliverySchedule.Quantity = viewModel.Quantity;
                            deliverySchedule.Type = (int)DeliveryScheduleType.SpecificDates;
                            deliverySchedule.GroupId = ++latestGroupNumber;
                            deliverySchedule.WeekDayId = helperDomain.GetWeekDayId(viewModel.StartDate);
                            deliverySchedule.CreatedBy = viewModel.UserId;
                            deliverySchedule.StatusId = (int)DeliveryScheduleStatus.New;
                            Context.DataContext.DeliverySchedules.Add(deliverySchedule);
                            await Context.CommitAsync();

                            var order = await Context.DataContext.Orders.Include(t => t.OrderDeliverySchedules).Include(t => t.FuelRequest).SingleOrDefaultAsync(t => t.Id == viewModel.OrderId);
                            if (order != null)
                            {
                                var latestSchedules = GetLatestOrderDeliverySchedule(order.OrderDeliverySchedules);
                                latestSchedules.ForEach(t => t.IsActive = false);

                                if (latestSchedules.Count > 0)
                                {
                                    foreach (var item in latestSchedules)
                                    {
                                        var schedule = GetOrderDeliverySchedule(viewModel.OrderId, item.Version, viewModel.UserId, item.DeliveryRequestId);
                                        schedule.AdditionalNotes = viewModel.AdditionalNotes;
                                        order.OrderDeliverySchedules.Add(schedule);
                                    }
                                }
                                else
                                {
                                    var schedule = GetOrderDeliverySchedule(viewModel.OrderId, 0, viewModel.UserId, null);
                                    schedule.AdditionalNotes = viewModel.AdditionalNotes;
                                    order.OrderDeliverySchedules.Add(schedule);
                                }

                                var schedules = new List<DeliverySchedule>() { deliverySchedule };
                                TrackableScheduleDomain trackableScheduleDomain = new TrackableScheduleDomain(this);
                                await trackableScheduleDomain.ProcessTrackableSchedules(schedules, order);

                                List<int> brokerOrderIds = await GetBrokerOrderIdAsync(viewModel.OrderId, true);
                                foreach (var brokerOrderId in brokerOrderIds)
                                {
                                    await SaveBrokerDeliverySchedulesAsync(viewModel, brokerOrderId, schedules);
                                }

                                await Context.CommitAsync();
                                transaction.Commit();

                                await notificationDomain.AddNotificationEventAsync(EventType.DeliveryRequestCreated, order.OrderDeliverySchedules.Max(t => t.Id), userContext.Id);
                                await newsfeedDomain.SetDeliveryScheduleNewsfeed(userContext, order, NewsfeedEvent.BuyerOrderDeliveryScheduleAdded);

                                viewModel.Id = deliverySchedule.Id;
                                response.StatusCode = Status.Success;
                                response.StatusMessage = Resource.errMessageDeliverySchedulesSaveSuccess;
                            }
                        }
                        else
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errMessageDeliverySchedulesSaveFailedForSingleDeliveryType;
                        }
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = string.Format(Resource.errMessageDeliverySchedulesSaveFailed, orderstatus.OrderXStatuses.FirstOrDefault(t => t.IsActive).MstOrderStatus.Name);
                    }
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageCreateRequestFailed;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("OrderDomain", "SaveDeliveryRequestAsync", ex.Message, ex);
                }
            }
            return response;
        }

        private async Task<bool> SaveBrokerDeliverySchedulesAsync(CreateDeliveryRequestInputViewModel viewModel, int brokeredOrderId, IEnumerable<DeliverySchedule> schedules)
        {
            bool response = false;
            try
            {
                var brokerOrder = await Context.DataContext.Orders.Include(t => t.OrderDeliverySchedules).Include(t => t.FuelRequest).SingleOrDefaultAsync(t => t.Id == brokeredOrderId);
                if (brokerOrder != null)
                {
                    var brokerOrderDS = GetLatestOrderDeliverySchedule(brokerOrder.OrderDeliverySchedules);
                    foreach (var item in brokerOrderDS) { item.IsActive = false; }
                    if (brokerOrderDS.Count > 0)
                    {
                        foreach (var item in brokerOrderDS)
                        {
                            var orderSchedule = GetOrderDeliverySchedule(brokerOrder.Id, item.Version, viewModel.UserId, item.DeliveryRequestId);
                            orderSchedule.AdditionalNotes = viewModel.AdditionalNotes;
                            brokerOrder.OrderDeliverySchedules.Add(orderSchedule);
                        }
                    }
                    else
                    {
                        var orderSchedule = GetOrderDeliverySchedule(brokerOrder.Id, 0, viewModel.UserId, null);
                        orderSchedule.AdditionalNotes = viewModel.AdditionalNotes;
                        brokerOrder.OrderDeliverySchedules.Add(orderSchedule);
                    }

                    TrackableScheduleDomain trackableScheduleDomain = new TrackableScheduleDomain(this);
                    await trackableScheduleDomain.ProcessTrackableSchedules(schedules, brokerOrder);

                    await Context.CommitAsync();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "SaveBrokerDeliverySchedulesAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<OrderForJobViewModel>> GetOrderForJobAsync(int jobId, string searchCriteria, int offset = 0, int count = 0, int supplierCompanyId = 0)
        {
            HelperDomain helperDomain = new HelperDomain(this);
            var response = new List<OrderForJobViewModel>();
            try
            {
                var job = await Context.DataContext.Jobs.SingleOrDefaultAsync(t => t.Id == jobId);
                var allOrders = Context.DataContext.Orders.Where(t => t.IsActive
                                                                    && t.BuyerCompanyId == job.Company.Id
                                                                    && t.ParentId == null
                                                                    && t.FuelRequest.Job.Id == jobId);

                if (supplierCompanyId != 0)
                {
                    allOrders = allOrders.Where(t => t.AcceptedCompanyId == supplierCompanyId);
                }

                if (!string.IsNullOrEmpty(searchCriteria))
                {
                    allOrders = allOrders.Where(t => t.FuelRequest.MstProduct.MstTFXProduct.Name.ToLower().Contains(searchCriteria.ToLower())
                                                    || ((t.PoNumber).ToLower().Contains(searchCriteria.ToLower())));
                }

                OrderForJobViewModel ordersForJob;
                var filteredOrders = allOrders.OrderByDescending(t => t.Id).ToList();
                foreach (var t in filteredOrders)
                {
                    ordersForJob = new OrderForJobViewModel();
                    ordersForJob.OrderId = t.Id;
                    ordersForJob.FuelType = helperDomain.GetProductName(t.FuelRequest.MstProduct);
                    ordersForJob.GallonsOrdered = t.BrokeredMaxQuantity ?? t.FuelRequest.MaxQuantity;
                    ordersForJob.GallonsDelivered = t.Invoices.Where(t1 => t1.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && !t1.IsBuyPriceInvoice).Sum(t1 => t1.DroppedGallons);
                    ordersForJob.QuantityTypeId = t.FuelRequest.QuantityTypeId;
                    var statusId = t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId;
                    if (statusId == (int)OrderStatus.PartiallyCanceled)
                    {
                        ordersForJob.Status = OrderStatus.Canceled.ToString();
                    }
                    else if (statusId == (int)OrderStatus.PartiallyClosed)
                    {
                        ordersForJob.Status = OrderStatus.Closed.ToString();
                    }
                    else
                    {
                        ordersForJob.Status = t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).MstOrderStatus.Name;
                    }

                    ordersForJob.PoNumber = t.PoNumber;
                    ordersForJob.PricePerGallon = helperDomain.GetPricePerGallon(t.FuelRequest);
                    ordersForJob.StartDate = t.FuelRequest.FuelRequestDetail.StartDate;
                    response.Add(ordersForJob);
                }
                response = response.Skip(offset).Take(count > 0 ? count : int.MaxValue).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetOrderForJobAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DeliveryScheduleForOrderViewModel>> GetDeliverySchedulesForOrder(int orderId, long startDate = 0, long endDate = 0, int offset = 0, int count = 0)
        {
            var response = new List<DeliveryScheduleForOrderViewModel>();

            try
            {
                var order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == orderId && t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open);

                if (order != null)
                {
                    DateTimeOffset deliveryStartDate = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName).Date;
                    DateTimeOffset deliveryEndDate = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName).Date.AddDays(1);

                    if (startDate > 0)
                    {
                        deliveryStartDate = DateTimeOffset.FromUnixTimeMilliseconds(startDate).Date;
                    }

                    if (endDate > 0)
                    {
                        deliveryEndDate = DateTimeOffset.FromUnixTimeMilliseconds(endDate).Date.AddDays(1);
                    }

                    if (order.FuelRequest.FuelRequestDetail.DeliveryTypeId == (int)DeliveryType.MultipleDeliveries)
                    {
                        var allschedules = Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(Extensions.IsTrackableScheduleUnDelivered()).Where(t => t.IsActive && !t.Invoices.Any(t1 => t1.IsActive && t1.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active) && t.Date >= deliveryStartDate && t.Date < deliveryEndDate
                                                                                                && t.DeliveryScheduleStatusId != (int)TrackableDeliveryScheduleStatus.MissedAndCanceled
                                                                                                && t.DeliveryScheduleStatusId != (int)TrackableDeliveryScheduleStatus.Canceled && t.OrderId == orderId).ToList();
                        foreach (var schedule in allschedules)
                        {
                            DeliveryScheduleForOrderViewModel deliverySchedule = new DeliveryScheduleForOrderViewModel
                            {
                                TrackableScheduleId = schedule.Id,
                                DeliveryScheduleId = schedule.DeliveryScheduleId,
                                GallonsOrdered = schedule.Quantity,
                                ScheduleDate = schedule.Date.Date,
                                StartTime = Convert.ToDateTime(schedule.StartTime.ToString()).ToShortTimeString(),
                                EndTime = Convert.ToDateTime(schedule.EndTime.ToString()).ToShortTimeString(),
                                Date = schedule.Date.Date.Add(schedule.StartTime)
                            };
                            response.Add(deliverySchedule);
                        }
                    }
                    else
                    {
                        DeliveryScheduleForOrderViewModel deliverySchedule = new DeliveryScheduleForOrderViewModel
                        {
                            GallonsOrdered = (order.BrokeredMaxQuantity ?? order.FuelRequest.MaxQuantity).GetPreciseValue(6),
                            ScheduleDate = order.FuelRequest.FuelRequestDetail.StartDate,
                            StartTime = Convert.ToDateTime(order.FuelRequest.FuelRequestDetail.StartTime.ToString()).ToShortTimeString(),
                            EndTime = Convert.ToDateTime(order.FuelRequest.FuelRequestDetail.EndTime.ToString()).ToShortTimeString(),
                            Date = order.FuelRequest.FuelRequestDetail.StartDate.Date.Add(order.FuelRequest.FuelRequestDetail.StartTime)
                        };
                        response.Add(deliverySchedule);
                    }

                    response = response.OrderBy(t => t.Date).ToList();
                    response = response.Skip(offset).Take(count > 0 ? count : int.MaxValue).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetDeliverySchedulesForOrder", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DeliveryScheduleForJobViewModel>> GetDeliveryScheduleForJobAsync(int jobId, string searchCriteria, int offset = 0, int count = 0, long scheduleDate = 0, int supplierCompanyId = 0, int enrouteStatus = 0)
        {
            var response = new List<DeliveryScheduleForJobViewModel>();
            var helperDomain = new HelperDomain(this);
            try
            {
                var job = await Context.DataContext.Jobs.FirstOrDefaultAsync(t => t.Id == jobId);
                if (job != null)
                {
                    DateTimeOffset deliveryDate = DateTimeOffset.FromUnixTimeMilliseconds(scheduleDate);
                    DateTimeOffset deliveryStartDate = deliveryDate.Date;
                    DateTimeOffset deliveryEndDate = deliveryStartDate.AddDays(1);
                    var allOrders = Context.DataContext.Orders.Include("FuelRequest.Job").Include(t => t.OrderDeliverySchedules)
                                                                .Include("OrderDeliverySchedules.DeliverySchedule")
                                                                .Where(t => t.IsActive
                                                                            && t.BuyerCompanyId == job.Company.Id
                                                                            && t.ParentId == null
                                                                            && t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open
                                                                            && t.FuelRequest.Job.Id == jobId
                                                                            && t.IsEndSupplier);

                    if (supplierCompanyId != 0)
                    {
                        allOrders = allOrders.Where(t => t.AcceptedCompanyId == supplierCompanyId);
                    }

                    if (!string.IsNullOrEmpty(searchCriteria))
                    {
                        allOrders = allOrders.Where(t => t.FuelRequest.MstProduct.MstTFXProduct.Name.ToLower().Contains(searchCriteria.ToLower())
                                || ((t.PoNumber).ToLower().Contains(searchCriteria.ToLower())));
                    }

                    foreach (var order in allOrders)
                    {
                        DateTimeOffset maxDate = order.FuelRequest.FuelRequestDetail.EndDate ?? (order.FuelRequest.Job.EndDate ?? DateTimeOffset.MaxValue);
                        if (deliveryDate.Date >= order.FuelRequest.FuelRequestDetail.StartDate.Date && deliveryDate.Date <= maxDate.Date)
                        {
                            var company = order.Company.CompanyAddresses.FirstOrDefault(t => t.IsDefault);
                            if (order.FuelRequest.FuelRequestDetail.DeliveryTypeId == (int)DeliveryType.MultipleDeliveries)
                            {
                                var allschedules = order.DeliveryScheduleXTrackableSchedules.Where(Extensions.IsTrackableScheduleUnDeliveredFunc())
                                    .Where(t => t.IsActive && !t.Invoices.Any(t1 => t1.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && t.IsActive) && t.Date >= deliveryStartDate && t.Date < deliveryEndDate && !t.IsScheduleCancelled);
                                foreach (var schedule in allschedules)
                                {
                                    DeliveryScheduleForJobViewModel deliverySchedule = new DeliveryScheduleForJobViewModel
                                    {
                                        OrderId = order.Id,
                                        DeliveryRequestId = schedule.DeliveryScheduleId,
                                        GallonsOrdered = schedule.Quantity,
                                        ScheduleDate = deliveryDate.Date,
                                        ScheduleStartTime = Convert.ToDateTime(schedule.StartTime.ToString()).ToShortTimeString(),
                                        ScheduleEndTime = Convert.ToDateTime(schedule.EndTime.ToString()).ToShortTimeString(),
                                        FuelType = helperDomain.GetProductName(order.FuelRequest.MstProduct),
                                        PoNumber = order.PoNumber,
                                        DriverId = schedule.DriverId ?? 0,
                                        PhoneNumber = company != null ? company.PhoneNumber : order.User.PhoneNumber,
                                        CountryId = job.CountryId,
                                        QuantityTypeId = order.FuelRequest.QuantityTypeId
                                    };
                                    deliverySchedule.ScheduleDetails = $"{deliverySchedule.PoNumber} {"-"} {deliverySchedule.ScheduleStartTime} {deliverySchedule.ScheduleEndTime} {"-"} {helperDomain.GetQuantityRequested(deliverySchedule.GallonsOrdered)}";
                                    deliverySchedule.DeliveryDetails = await GetDriverDetailsForBuyerAsync(schedule.DriverId ?? 0, order.Id, schedule.DeliveryScheduleId, enrouteStatus);

                                    response.Add(deliverySchedule);
                                }
                            }
                            else
                            {
                                if (deliveryDate.Date >= order.FuelRequest.FuelRequestDetail.StartDate.Date && deliveryDate.Date <= maxDate.Date)
                                {
                                    DeliveryScheduleForJobViewModel deliverySchedule = new DeliveryScheduleForJobViewModel
                                    {
                                        OrderId = order.Id,
                                        GallonsOrdered = (order.BrokeredMaxQuantity ?? order.FuelRequest.MaxQuantity).GetPreciseValue(6),
                                        ScheduleDate = order.FuelRequest.FuelRequestDetail.StartDate,
                                        ScheduleStartTime = Convert.ToDateTime(order.FuelRequest.FuelRequestDetail.StartTime.ToString()).ToShortTimeString(),
                                        ScheduleEndTime = Convert.ToDateTime(order.FuelRequest.FuelRequestDetail.EndTime.ToString()).ToShortTimeString(),
                                        FuelType = helperDomain.GetProductName(order.FuelRequest.MstProduct),
                                        PoNumber = order.PoNumber,
                                        PhoneNumber = company != null ? company.PhoneNumber : order.User.PhoneNumber,
                                        CountryId = job.CountryId,
                                        QuantityTypeId = order.FuelRequest.QuantityTypeId
                                    };
                                    var currentDriver = order.OrderXDrivers.SingleOrDefault(t => t.IsActive);
                                    if (currentDriver != null)
                                    {
                                        deliverySchedule.DriverId = currentDriver.DriverId;
                                    }
                                    deliverySchedule.ScheduleDetails = $"{deliverySchedule.PoNumber} {"-"} {deliverySchedule.ScheduleStartTime} {deliverySchedule.ScheduleEndTime} {"-"} {helperDomain.GetQuantityRequested(deliverySchedule.GallonsOrdered)}";
                                    response.Add(deliverySchedule);
                                }
                            }
                        }
                    }

                    response = response.Skip(offset).Take(count > 0 ? count : int.MaxValue).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetDeliveryScheduleForJobAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<TrailerWithCompartments>> GetTrailerCompartmentsForMobile(int companyId)
        {
            var finalResult = new List<TrailerWithCompartments>() { };
            try
            {
                var apiResult = await ContextFactory.Current.GetDomain<FreightServiceDomain>().GetAllTruckDetails(companyId);
                if (apiResult.Any())
                {
                    apiResult.ForEach(r =>
                    {
                        var _compartments = new List<Compartment>() { };
                        r.Compartments.ForEach(c =>
                        {
                            _compartments.Add(new Compartment { CompartmentId = c.CompartmentId, Quantity = c.Quantity, Capacity = c.Capacity, FuelType = c.FuelType, PumpId = c.PumpId ?? "" });
                        });
                        finalResult.Add(new TrailerWithCompartments { TrailerId = r.Id, TrailerName = r.TruckId, Compartments = _compartments });
                    });
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetTrailerCompartmentsForMobile", ex.Message, ex);
            }
            return finalResult;
        }



        public List<TrailerWithCompartments> GetUniqueTrailerCompartmentFromString(string CompartmentInfo, List<TrailerWithCompartments> trailerDetails)
        {
            var response = new List<TrailerWithCompartments>() { };
            try
            {
                var _compList = JsonConvert.DeserializeObject<List<CompartmentsInfoViewModel>>(CompartmentInfo);

                if (_compList.Any())
                {
                    //GET UNIQUE TRAILER IDS FOR CURRENT SCHEDULE
                    var _uniqueTrailerIds = _compList.Select(_c => _c.TrailerId).Distinct().ToList();
                    //
                    foreach (var uniqueTtrailerId in _uniqueTrailerIds)
                    {
                        if (!string.IsNullOrEmpty(uniqueTtrailerId))
                        {
                            //COMMON TRAILER OBJECT
                            var trailerWithCompartments = new TrailerWithCompartments
                            {
                                TrailerName = trailerDetails.Where(td => td.TrailerId == uniqueTtrailerId).Select(n => n.TrailerName).FirstOrDefault(),
                                TrailerId = uniqueTtrailerId,
                                Compartments = new List<Compartment>() { }
                            };
                            //COMPARTMENTS FOR TRAILER
                            var _uniqueTrailerList = _compList.Where(h => h.TrailerId == uniqueTtrailerId).ToList();
                            foreach (var trl in _uniqueTrailerList)
                            {
                                trailerWithCompartments.Compartments.Add(new Compartment { CompartmentId = trl.CompartmentId, Quantity = trl.Quantity, PumpId = trl.PumpId ?? "" });
                            }

                            response.Add(trailerWithCompartments);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetUniqueTrailerCompartmentFromString", ex.Message, ex);
            }
            return response;
        }

        public List<TrailerWithCompartments> GetUniqueTrailerCompartmentFromStringList(List<string> CompartmentInfoStringList, List<TrailerWithCompartments> trailerDetails)
        {
            var response = new List<TrailerWithCompartments>() { };
            try
            {
                var _compList = new List<CompartmentsInfoViewModel>() { };
                CompartmentInfoStringList.ForEach(str =>
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        _compList.AddRange(JsonConvert.DeserializeObject<List<CompartmentsInfoViewModel>>(str));
                    }
                });
                //UNIQUE TRAILERS
                var _trailerIds = _compList.Select(c => c.TrailerId).Distinct().ToList();
                _trailerIds.ForEach(trailerId =>
                {
                    var _compartments = new List<Compartment>() { };
                    var _compartmentsRaw = _compList.Where(c => c.TrailerId == trailerId).ToList();

                    _compartmentsRaw.ForEach(c => { _compartments.Add(new Compartment { CompartmentId = c.CompartmentId, Quantity = c.Quantity }); });

                    response.Add(new TrailerWithCompartments
                    {
                        TrailerId = trailerId,
                        TrailerName = trailerDetails.Where(tr => tr.TrailerId == trailerId).Select(trl => trl.TrailerName).FirstOrDefault(),
                        Compartments = _compartments
                    });
                });

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetUniqueTrailerCompartmentFromStringList", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<DeliveryScheduleGroup>> GetDeliveryScheduleAsync(int userId, int companyId, long scheduleDate, int userTimeOffset)
        {
            List<DeliveryScheduleGroup> response = new List<DeliveryScheduleGroup>();
            try
            {
                var storedProcedureDomain = new StoredProcedureDomain(this);
                var orders = await storedProcedureDomain.GetDeliveryScheduleForMobile(userId, companyId, userTimeOffset, scheduleDate);
                var trackableScheduleIds = orders.Select(t => t.TrackableScheduleId).ToList();
                //var deliveryRequests = orders.Select(t => t.FrDeliveryRequestId).ToList();
                var deliveryRequests = orders.Where(t => t.FrDeliveryRequestId != null && t.FrDeliveryRequestId != "").Select(t => t.FrDeliveryRequestId).ToList();
                var optionalPickup = orders.Select(t => t.OptionalPickupInfo).FirstOrDefault();
                List<OptionalPickupInfo> optionalPickupInfo = new List<OptionalPickupInfo>();
                if (optionalPickup != null)
                {
                    await GetOptionalPickupDetails(optionalPickup, optionalPickupInfo);
                }
                var enrouteData = Context.DataContext.EnrouteDeliveryHistories.Where(t => trackableScheduleIds.Any(t1 => t1 == t.TrackableScheduleId)).ToList();
                var deliveryGroups = orders.GroupBy(t => t.DeliveryGroupId);
                var freightServiceDomain = new FreightServiceDomain(this);
                var scheduleOutputDetails = new ScheduleOutputDetails();
                var scheduleInputDetails = orders.Where(t => t.JobId > 0).Select(t => new ScheduleInputDetails() { DeliveryScheduleId = t.DeliveryScheduleId, TrackableScheduleId = t.TrackableScheduleId, JobId = t.JobId.Value, OrderId = t.OrderId ?? 0 }).ToList();
                if (orders.Any(t => t.JobId > 0))
                {
                    scheduleOutputDetails = await freightServiceDomain.GetTankDetailsBySchedule(scheduleInputDetails);
                }
                var deliveryRequestDetails = await freightServiceDomain.GetTbdDeliveryRequestDetails(deliveryRequests);
                var jobIds = orders.Select(t => t.JobId).ToList();
                var sequenceDefined = Context.DataContext.SupplierXProductSequencing.Where(t => t.SupplierCompanyId == companyId && (jobIds.Contains(t.JobId ?? 0) || t.SequenceCreationMethod == ProductSequencingCreationMethod.Account) && t.IsActive)
                                                                     .Select(t => new { t.JobId, t.ProductId, t.OrderId, t.SequenceNumber, t.SequenceCreationMethod }).ToList();
                var marineOrderIds = orders.Where(t => t.IsMarine).Select(t => t.OrderId).ToList();
                var marineVessles = Context.DataContext.JobXAssets.Where(t => marineOrderIds.Contains(t.OrderId)).Select(t => new { t.Asset.Name, t.OrderId }).ToList();

                ///GET UNIQUE TRAILERS AND NAMES
                List<TrailerWithCompartments> allTrailerDetails = await GetTrailerCompartmentsForMobile(companyId);
                var isEbolEnabled = Context.DataContext.OnboardingPreferences.Where(t => t.CompanyId == companyId && t.IsActive).Select(t => t.IsEbolWorkflowEnabled).FirstOrDefault();
                foreach (var deliveryGroup in deliveryGroups)
                {
                    var routeGroup = new DeliveryScheduleGroup();
                    routeGroup.GroupId = deliveryGroup.Key;
                    if (deliveryGroup.Key > 0)
                    {
                        routeGroup.RouteNote = !string.IsNullOrWhiteSpace(deliveryGroup.FirstOrDefault().RouteNote) ? deliveryGroup.FirstOrDefault().RouteNote : string.Empty;
                        routeGroup.LoadCode = deliveryGroup.FirstOrDefault().LoadCode;
                        var commonBadgeCount = deliveryGroup.Count(top => top.DeliveryGroupId == deliveryGroup.Key && top.IsCommonBadge);
                        if (commonBadgeCount == 0)
                        {
                            routeGroup.BadgeNo1 = string.Empty;
                            routeGroup.BadgeNo2 = string.Empty;
                            routeGroup.BadgeNo3 = string.Empty;
                            routeGroup.IsCommonBadge = false;
                            if (string.IsNullOrEmpty(routeGroup.LoadCode))
                            {
                                routeGroup.LoadCode = string.Empty;
                            }
                            // routeGroup.RouteNote = string.Empty;
                        }
                        else
                        {
                            var badgeNumberDetails = deliveryGroup.FirstOrDefault(top => top.DeliveryGroupId == deliveryGroup.Key && top.IsCommonBadge);
                            if (badgeNumberDetails != null)
                            {
                                routeGroup.BadgeNo1 = badgeNumberDetails.BadgeNo1;
                                routeGroup.BadgeNo2 = badgeNumberDetails.BadgeNo2;
                                routeGroup.BadgeNo3 = badgeNumberDetails.BadgeNo3;
                                routeGroup.IsCommonBadge = true;
                                IList<string> strLoadCodes = new List<string> { routeGroup.BadgeNo1, routeGroup.BadgeNo2, routeGroup.BadgeNo3 };
                                routeGroup.LoadCode = String.Join(",", strLoadCodes.Where(s => !String.IsNullOrEmpty(s)));
                                // routeGroup.RouteNote = badgeNumberDetails.DispatcherNote;
                            }
                        }
                        routeGroup.FsTrailerDisplayId = deliveryGroup.FirstOrDefault().FsTrailerDisplayId;
                        routeGroup.LoadNumber = deliveryGroup.FirstOrDefault().LoadNumber;
                    }
                    var orderMapping = deliveryGroup.GroupBy(t => t.OrderId);
                    foreach (var item in orderMapping)
                    {
                        var orderSchedules = item.GroupBy(t => t.TrackableScheduleId);
                        if (orderSchedules.Any())
                        {
                            foreach (var schedule in orderSchedules)
                            {
                                var schedules = schedule.ToList();
                                var trackableSchedule = schedule.FirstOrDefault();
                                var deliveryRequest = deliveryRequestDetails.FirstOrDefault(t => t.DeliveryRequestId == trackableSchedule.FrDeliveryRequestId);
                                var deliverySchedule = new DeliveryScheduleForDriverViewModel();
                                deliverySchedule.OrderId = trackableSchedule.OrderId;
                                deliverySchedule.PoNumber = trackableSchedule.PoNumber;
                                deliverySchedule.CompanyName = trackableSchedule.CompanyName;
                                deliverySchedule.JobId = trackableSchedule.JobId;
                                deliverySchedule.JobName = trackableSchedule.JobName;  //In case of Marine JobName is Port
                                if (trackableSchedule.IsAdditive)
                                {
                                    deliverySchedule.IsOnlyAdditiveGroup = orders.Where(t => t.BlendGroupId == trackableSchedule.BlendGroupId).All(t => t.IsAdditive);
                                }
                                if (trackableSchedule.IsMarine)
                                {
                                    deliverySchedule.Berth = trackableSchedule.Berth;
                                    var marineVessle = marineVessles.FirstOrDefault(t => t.OrderId == trackableSchedule.OrderId);
                                    deliverySchedule.Vessle = marineVessle != null ? marineVessle.Name : string.Empty;
                                }

                                deliverySchedule.JobAddress = trackableSchedule.JobAddress;
                                deliverySchedule.JobCity = trackableSchedule.JobCity;
                                deliverySchedule.JobState = trackableSchedule.JobState;
                                deliverySchedule.JobZip = trackableSchedule.JobZip;
                                deliverySchedule.LocationType = trackableSchedule.LocationType;
                                deliverySchedule.TrackableScheduleType = trackableSchedule.TrackableScheduleType;
                                deliverySchedule.PostLoadedForId = trackableSchedule.PostLoadedForId;
                                deliverySchedule.GallonsOrdered = trackableSchedule.GallonsOrdered;
                                deliverySchedule.StartTime = Convert.ToDateTime(trackableSchedule.StartTime.ToString()).ToShortTimeString();
                                deliverySchedule.EndTime = Convert.ToDateTime(trackableSchedule.EndTime.ToString()).ToShortTimeString();
                                deliverySchedule.UtcStartTime = trackableSchedule.StartTime.ToUnixTimeMilliseconds();
                                deliverySchedule.UtcEndTime = trackableSchedule.EndTime.ToUnixTimeMilliseconds();
                                deliverySchedule.DeliveryScheduleId = trackableSchedule.DeliveryScheduleId;
                                deliverySchedule.NetGrossTypeId = trackableSchedule.PricingQuantityIndicatorTypeId ?? 0;
                                deliverySchedule.TrackableScheduleId = trackableSchedule.TrackableScheduleId;
                                deliverySchedule.IsFTL = trackableSchedule.IsFTL;
                                deliverySchedule.DeliveryRequestId = !string.IsNullOrEmpty(trackableSchedule.FrDeliveryRequestId) ? trackableSchedule.FrDeliveryRequestId : string.Empty;
                                deliverySchedule.GroupedParentDrId = deliveryRequest != null ? deliveryRequest.GroupedParentDrId : string.Empty;
                                deliverySchedule.IsDriverToUpdateBOL = trackableSchedule.IsDriverToUpdateBOL;
                                deliverySchedule.IsDropImageRequired = trackableSchedule.IsDropImageRequired;
                                deliverySchedule.IsBolImageRequired = trackableSchedule.IsBolImageRequired;
                                deliverySchedule.IsBadgeMandatory = trackableSchedule.IsBadgeMandatory;
                                deliverySchedule.IsTBD = deliveryRequest != null && deliveryRequest.IsTBD;
                                deliverySchedule.FuelType = !string.IsNullOrWhiteSpace(trackableSchedule.FuelType) ? trackableSchedule.FuelType : deliveryRequest.FuelType;
                                deliverySchedule.FuelTypeId = trackableSchedule.FuelTypeId ?? deliveryRequest.FuelTypeId;
                                deliverySchedule.ProductTypeId = deliverySchedule.TankProductTypeId = trackableSchedule.ProductTypeId ?? deliveryRequest.ProductTypeId;
                                deliverySchedule.ProductTypeName = trackableSchedule.ProductTypeName;
                                deliverySchedule.OrderDeliveryType = trackableSchedule.OrderDeliveryType;
                                deliverySchedule.OrderUoM = trackableSchedule.UnitOfMeasurement ?? deliveryRequest.UoM;
                                deliverySchedule.BlendGroupId = trackableSchedule.BlendGroupId;
                                deliverySchedule.IsAdditive = trackableSchedule.IsAdditive;
                                if (trackableSchedule.UnitOfMeasurement == (int)UoM.Barrels || trackableSchedule.UnitOfMeasurement == (int)UoM.MetricTons)
                                {
                                    if (trackableSchedule.CountryId == (int)Country.CAN)
                                    {
                                        deliverySchedule.UnitOfMeasurement = (int)UoM.Litres;
                                    }
                                    else
                                    {
                                        deliverySchedule.UnitOfMeasurement = (int)UoM.Gallons;
                                    }
                                }
                                else
                                {
                                    deliverySchedule.UnitOfMeasurement = trackableSchedule.UnitOfMeasurement ?? deliveryRequest.UoM;
                                }
                                deliverySchedule.Currency = trackableSchedule.Currency ?? deliveryRequest.UoM;
                                deliverySchedule.CustomerSignatureRequired = trackableSchedule.CustomerSignatureRequired;
                                deliverySchedule.QuantityTypeId = trackableSchedule.QuantityTypeId;
                                deliverySchedule.SupplierSource = trackableSchedule.SupplierSource;
                                deliverySchedule.SupplierContract = trackableSchedule.SupplierContract;
                                deliverySchedule.LoadCode = trackableSchedule.LoadCode;
                                deliverySchedule.Carrier = trackableSchedule.CarrierName;
                                deliverySchedule.ScheduleQuantityTypeId = (int)trackableSchedule.ScheduleQuantityType;
                                if (deliverySchedule.ScheduleQuantityTypeId == (int)ScheduleQuantityType.Quantity || deliverySchedule.ScheduleQuantityTypeId == 0 || deliverySchedule.ScheduleQuantityTypeId == null)
                                {
                                    deliverySchedule.ScheduleQuantityTypeName = ScheduleQuantityType.Quantity.ToString();
                                }
                                else
                                {
                                    deliverySchedule.ScheduleQuantityTypeName = EnumHelperMethods.GetDisplayName((ScheduleQuantityType)deliverySchedule.ScheduleQuantityTypeId.Value);
                                }
                                deliverySchedule.DriverAcknowledgementStatus = (int)trackableSchedule.DeliveryAcknowledgementStatus;
                                deliverySchedule.ScheduleDate = trackableSchedule.Date.ToString(Resource.constFormatDate);
                                deliverySchedule.IsPrePostDipEnabled = trackableSchedule.IsPrePostDipRequired;
                                deliverySchedule.ShiftStartDate = trackableSchedule.ShiftStartDate == DateTimeOffset.MinValue ? trackableSchedule.Date.ToUnixTimeMilliseconds() : trackableSchedule.ShiftStartDate.ToUnixTimeMilliseconds();
                                //  await GetSplitDropAddressesAsync(trackableSchedule.OrderId, trackableSchedule.TrackableScheduleId, trackableSchedule.DeliveryScheduleId);
                                int? enrouteDataStatus = enrouteData.Where(t => (trackableSchedule.TrackableScheduleId == 0 || t.TrackableScheduleId == trackableSchedule.TrackableScheduleId)
                                                                                && (trackableSchedule.OrderId == null || (deliveryRequest != null && deliveryRequest.IsTBD && t.OrderId == null) || t.OrderId == trackableSchedule.OrderId)).OrderByDescending(t => t.EnrouteDate).Select(t => t.StatusId).FirstOrDefault();
                                deliverySchedule.EnrouteDeliveryStatus = enrouteDataStatus != null ? enrouteDataStatus.Value : 0;
                                deliverySchedule.BadgeNo1 = trackableSchedule.BadgeNo1;
                                deliverySchedule.BadgeNo2 = trackableSchedule.BadgeNo2;
                                deliverySchedule.BadgeNo3 = trackableSchedule.BadgeNo3;
                                deliverySchedule.DispatcherNote = trackableSchedule.DispatcherNote;
                                deliverySchedule.IsCommonBadge = trackableSchedule.IsCommonBadge;
                                deliverySchedule.IsFilldInvoke = trackableSchedule.IsFilldInvoke;
                                deliverySchedule.FilldStopId = trackableSchedule.FilldStopId;
                                deliverySchedule.FilldDriverId = trackableSchedule.FilldDriverId;
                                deliverySchedule.Notes = trackableSchedule.Notes;
                                deliverySchedule.IsEbolWorkflowEnabled = isEbolEnabled;
                                deliverySchedule.RouteAdditionalInfo = string.IsNullOrEmpty(trackableSchedule.RouteAdditionalInfo) ? null : JsonConvert.DeserializeObject<RouteInfoDetails>(trackableSchedule.RouteAdditionalInfo);


                                if (deliverySchedule.RouteAdditionalInfo != null)
                                {
                                    deliverySchedule.LocationSeqNo = deliverySchedule.RouteAdditionalInfo.LocationSeqNo;
                                }
                                else
                                {
                                    deliverySchedule.LocationSeqNo = int.MaxValue;
                                }
                                var json = JsonConvert.SerializeObject(scheduleInputDetails);
                                try
                                {
                                    if (scheduleOutputDetails != null && scheduleOutputDetails.ScheduleTank != null)
                                    {
                                        //Set TankId to Schedule
                                        var scheduleDetails = scheduleOutputDetails.ScheduleTank.FirstOrDefault(t => t.DeliveryScheduleId == trackableSchedule.DeliveryScheduleId && t.TrackableScheduleId == trackableSchedule.TrackableScheduleId);
                                        if (scheduleDetails != null)
                                        {
                                            deliverySchedule.AssetId = scheduleDetails.AssetId;
                                            deliverySchedule.TankProductTypeId = scheduleDetails.ProductTypeId;
                                        }
                                    }
                                }
                                catch (Exception ex1)
                                {
                                    LogManager.Logger.WriteException("OrderDomain", "ScheduleOutputDetails JsonObject: " + json, ex1.Message, ex1);
                                }
                                deliverySchedule.RecurringScheduleInfo = string.IsNullOrEmpty(trackableSchedule.RecurringScheduleInfo) ? null : JsonConvert.DeserializeObject<RecurringScheduleInfo>(trackableSchedule.RecurringScheduleInfo);
                                IList<string> strLoadCodes = new List<string> { deliverySchedule.BadgeNo1, deliverySchedule.BadgeNo2, deliverySchedule.BadgeNo3 };
                                deliverySchedule.LoadCode = String.Join(",", strLoadCodes.Where(s => !String.IsNullOrEmpty(s)));
                                DriverDeliveryDetailsViewModel driverDetails = new DriverDeliveryDetailsViewModel()
                                {
                                    CompanyName = trackableSchedule.CompanyName,
                                    JobName = trackableSchedule.JobName,
                                    JobAddress = trackableSchedule.JobAddress,
                                    JobCity = trackableSchedule.JobCity,
                                    JobState = trackableSchedule.JobState,
                                    JobZip = trackableSchedule.JobZip,
                                    JobLatitude = trackableSchedule.LocationType != (int)JobLocationTypes.Various ? trackableSchedule.Latitude : 0,
                                    JobLongitude = trackableSchedule.LocationType != (int)JobLocationTypes.Various ? trackableSchedule.Longitude : 0
                                };
                                if (trackableSchedule.JobId > 0)
                                {
                                    deliverySchedule.DropLocations = new List<ApiAddressViewModel>() { new ApiAddressViewModel()
                                    {
                                        Address = $"{trackableSchedule.JobAddress}, {trackableSchedule.JobCity}, {trackableSchedule.JobState}, {trackableSchedule.JobZip}",
                                        Id = trackableSchedule.JobId.Value,
                                        IsJobLocation = true,
                                        Latitude = trackableSchedule.LocationType != (int)JobLocationTypes.Various ? trackableSchedule.Latitude : 0,
                                        Longitude = trackableSchedule.LocationType != (int)JobLocationTypes.Various ? trackableSchedule.Longitude : 0,
                                        Status = DropAddressStatus.Pending
                                    } };
                                }
                                var specalInstructions = schedules.GroupBy(t => t.SpecialInstructionId).Where(t => t.Key != null).Select(t => t.FirstOrDefault());
                                driverDetails.SpecialInstructions = specalInstructions.Select(t1 => t1.Instruction).ToList();

                                var contactPersons = schedules.GroupBy(t => t.ContactPersonId).Where(t => t.Key != null).Select(t => t.FirstOrDefault());
                                driverDetails.ContactPersons = contactPersons.Select(t => new ContactPersonViewModel() { Id = t.ContactPersonId.Value, Name = t.ContactPersonName, Email = t.Email, PhoneNumber = t.PhoneNumber }).ToList();

                                deliverySchedule.DriverDeliveryDetails = driverDetails;
                                deliverySchedule.ProductSequence = ApplicationConstants.SequenceNotDefined;
                                deliverySchedule.FuelPickUpLocation = GetPickUpLocationDetails(trackableSchedule);

                                if (trackableSchedule.OrderId.HasValue)
                                {
                                    deliverySchedule.DriverDeliveryDetails.SpecialInstructionFiles = GetSpecialInstructionFileDetails(trackableSchedule.OrderId.Value, trackableSchedule.FileDetails);
                                }

                                if (sequenceDefined.Any(t => t.JobId == deliverySchedule.JobId))
                                {
                                    var productSequence = sequenceDefined.Where(t => t.JobId == deliverySchedule.JobId && (t.ProductId == deliverySchedule.TankProductTypeId || t.OrderId == deliverySchedule.OrderId)).FirstOrDefault();
                                    if (productSequence != null)
                                    {
                                        deliverySchedule.ProductSequence = productSequence.SequenceNumber;
                                    }
                                }
                                else if (sequenceDefined.Any(t => t.SequenceCreationMethod == ProductSequencingCreationMethod.Account))
                                {
                                    var accountLevelSequence = sequenceDefined.Where(t => t.ProductId == deliverySchedule.TankProductTypeId && t.SequenceCreationMethod == ProductSequencingCreationMethod.Account).FirstOrDefault();
                                    if (accountLevelSequence != null)
                                    {
                                        deliverySchedule.ProductSequence = accountLevelSequence.SequenceNumber;
                                    }
                                }

                                //SET TRAILER AND COMPARTMENT INFO
                                if (!string.IsNullOrEmpty(trackableSchedule.CompartmentInfo))
                                {
                                    deliverySchedule.CompartmentInfo = trackableSchedule.CompartmentInfo;
                                    deliverySchedule.TrailerWithCompartments = GetUniqueTrailerCompartmentFromString(trackableSchedule.CompartmentInfo, allTrailerDetails);
                                }
                                deliverySchedule.IsMarine = trackableSchedule.IsMarine;
                                //SET IsOptionalPickup
                                deliverySchedule.IsOptionalPickup = trackableSchedule.IsOptionalPickup;
                                if (trackableSchedule.IsOptionalPickup)
                                {
                                    deliverySchedule.OptionalPickupInfo = optionalPickupInfo.Where(x => x.FuelTypeId == trackableSchedule.FuelTypeId).ToList();
                                }
                                else
                                {
                                    deliverySchedule.OptionalPickupInfo = optionalPickupInfo.Where(x => x.FuelTypeId == trackableSchedule.FuelTypeId).ToList();
                                }
                                deliverySchedule.IsDispatcherDragDrop = trackableSchedule.IsDispatcherDragDrop;
                                deliverySchedule.DispatcherDragDropSequence = trackableSchedule.DispatcherDragDropSequence;
                                deliverySchedule.DeliveryLevelPO = trackableSchedule.DeliveryLevelPO;
                                routeGroup.DeliverySchedules.Add(deliverySchedule);
                            }
                        }
                    }
                    routeGroup.DeliverySchedules = routeGroup.DeliverySchedules.OrderBy(top => top.LocationSeqNo).ThenBy(top => top.ProductSequence).ThenBy(top => top.OrderId).ToList();

                    if (!string.IsNullOrEmpty(routeGroup.FsTrailerDisplayId))
                    {
                        var trailerList = routeGroup.FsTrailerDisplayId.Split(',').Select(t => t.Trim()).ToList();
                        if (trailerList.Count > 0)
                        {
                            routeGroup.TrailerWithCompartments = allTrailerDetails.Where(t => trailerList.Contains(t.TrailerName)).ToList();
                        }
                    }
                    response.Add(routeGroup);
                }

                List<int> postLoadedTrackableScheduleIds = new List<int>();
                foreach (var deliveryGroupItem in response)
                {
                    //ASSIGN OPTIONAL PICKUP INFO
                    AssignOptionalPickupInfo(optionalPickupInfo, deliveryGroupItem, optionalPickup);
                    foreach (var deliveryScheduleItem in deliveryGroupItem.DeliverySchedules)
                    {
                        if (deliveryScheduleItem.PostLoadedForId.HasValue)
                        {
                            postLoadedTrackableScheduleIds.Add(deliveryScheduleItem.TrackableScheduleId);
                        }
                    }
                }

                List<ApiPreLoadBolViewModel> allPreLoadBols = new List<ApiPreLoadBolViewModel>();
                if (postLoadedTrackableScheduleIds.Count > 0)
                {
                    allPreLoadBols = await storedProcedureDomain.GetPreLoadBolDetailsForMobile(companyId, userId, postLoadedTrackableScheduleIds);
                    //GET Retain Information
                    await SetRetainWorkFlow(allPreLoadBols);
                }


                var allProductTypeMapping = Context.DataContext.ProductTypeCompatibilityMappings.Select(t => new DropdownDisplayExtendedId { Id = t.ProductTypeId, CodeId = t.MappedToProductTypeId }).ToList();
                var blendProductTypeMapping = Context.DataContext.MstBlendProductTypeMapping.Select(t => new DropdownDisplayExtendedId { Id = t.ProductTypeId, CodeId = t.MappedToProductTypeId }).ToList();

                var assetIds = new List<int> { };
                if (scheduleOutputDetails != null && scheduleOutputDetails.TankDetailList != null)
                {
                    assetIds = scheduleOutputDetails.TankDetailList.Select(t => t.AssetId).Distinct().ToList();
                }
                var allAssets = Context.DataContext.Assets.Include(t => t.JobXAssets).Include("JobXAssets.Job").Where(t => t.IsActive && assetIds.Contains(t.Id));

                foreach (var deliveryGroupItem in response)
                {
                    if (deliveryGroupItem.DeliverySchedules != null)
                    {
                        var deliverySchedules = deliveryGroupItem.DeliverySchedules;
                        foreach (var deliveryScheduleItem in deliverySchedules)
                        {
                            //Set PreLoadBols to Schedule
                            if (allPreLoadBols.Count > 0)
                            {
                                var preLoadBols = allPreLoadBols.Where(t => t.TrackableScheduleId == deliveryScheduleItem.TrackableScheduleId).ToList();
                                if (preLoadBols.Count > 0)
                                {
                                    deliveryScheduleItem.PreLoadBols = preLoadBols;
                                    foreach (var _preloadBol in deliveryScheduleItem.PreLoadBols)
                                    {
                                        if (!string.IsNullOrEmpty(_preloadBol.CompartmentInfo))
                                        {
                                            _preloadBol.TrailerWithCompartments = GetUniqueTrailerCompartmentFromString(_preloadBol.CompartmentInfo, allTrailerDetails);
                                        }
                                    }
                                }
                                //deliveryScheduleItem.TrailerWithCompartments = GetUniqueTrailerCompartmentFromString(deliveryScheduleItem.CompartmentInfo, allTrailerDetails);
                            }
                        }

                        //Set Tanks
                        var deliveryGroupJobs = deliveryGroupItem.DeliverySchedules.Select(t => t.JobId).Distinct().ToList();
                        var jobTankList = new List<TankDetailViewModel>() { };
                        if (scheduleOutputDetails != null && scheduleOutputDetails.TankDetailList != null)
                        {
                            jobTankList = scheduleOutputDetails.TankDetailList.Where(t => deliveryGroupJobs.Contains(t.JobId)).ToList();
                            var apiTankViewModel = new List<ApiTankDetailViewModel>();
                            foreach (var item in jobTankList)
                            {
                                ApiTankDetailViewModel tank = new ApiTankDetailViewModel();

                                //Set JobXAssetId and UoM
                                var jobXAssetId = 0;
                                var filePath = string.Empty;
                                var asset = allAssets.FirstOrDefault(t => t.Id == item.AssetId);
                                if (asset != null)
                                {
                                    var activeJobXAsset = asset.JobXAssets.FirstOrDefault(t1 => t1.RemovedBy == null && t1.RemovedDate == null);
                                    if (activeJobXAsset != null)
                                    {
                                        jobXAssetId = activeJobXAsset.Id;
                                        tank.UoM = activeJobXAsset.Job.UoM.ToString();
                                        //tank.TankCreatedDate = activeJobXAsset.Asset.CreatedDate;
                                        filePath = asset.Image != null ? asset.Image.FilePath : string.Empty;
                                    }
                                }

                                if (jobXAssetId > 0)
                                {
                                    tank = item.ToApiTankViewModel(tank.UoM);
                                    tank.JobXAssetId = jobXAssetId;
                                    tank.TankImage = filePath;

                                    //Set MappedToProductTypeId
                                    tank.MappedToProductTypeId = allProductTypeMapping.Where(t => t.Id == item.FuelTypeId).Select(t => t.CodeId).Distinct().ToList();
                                    tank.MappedToBlendProductTypeId = blendProductTypeMapping.Where(t => t.Id == tank.ProducTypeId).Select(t => t.CodeId).Distinct().ToList();

                                    apiTankViewModel.Add(tank);
                                }
                            }

                            deliveryGroupItem.Tanks = apiTankViewModel;
                        }
                        if (scheduleOutputDetails != null && scheduleOutputDetails.JobDetails != null)
                        {
                            //Set Jobs
                            var jobList = scheduleOutputDetails.JobDetails.Where(t => deliveryGroupJobs.Contains(t.JobId)).ToList();
                            var apiJobViewModel = new List<ApiJobDetailViewModel>();
                            foreach (var item in jobList)
                            {
                                apiJobViewModel.Add(item.ToApiJobViewModel());
                            }
                            deliveryGroupItem.Jobs = apiJobViewModel;
                        }
                    }
                }
                //Order By DispatcherDragDropSequence.(IsDispatcherDragDrop)
                foreach (var deliveryGroupItem in response)
                {
                    deliveryGroupItem.DeliverySchedules = deliveryGroupItem.DeliverySchedules.OrderBy(x => x.DispatcherDragDropSequence).ToList();
                }
                //bind trailer and compartments on top
                //response.ForEach(r => { r.TrailerWithCompartments = allTrailerDetails; });
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetDeliveryScheduleAsync", ex.Message, ex);
            }
            return response;
        }

        private async Task SetRetainWorkFlow(List<ApiPreLoadBolViewModel> allPreLoadBols)
        {
            if (allPreLoadBols.Any())
            {
                allPreLoadBols.ForEach(x =>
                {
                    x.RetainCompartmentDetails = JsonConvert.DeserializeObject<List<CompartmentsInfoViewModel>>(x.CompartmentInfo);
                });
                //Get Pre BOL Retain Details.
                var preBOlRetainInfo = allPreLoadBols.Select(x => new PreBOLRetainModel { DeliveryReqId = x.DeliveryReqId, FuelTypeId = x.FuelTypeId, CompartmentInfo = x.RetainCompartmentDetails }).ToList();
                var preBOLRetainInfomation = await new FreightServiceDomain(this).GetPreBOLRetainInfo(preBOlRetainInfo);
                allPreLoadBols.ForEach(x =>
                {
                    var preBOLRetainInfo = preBOLRetainInfomation.Where(x1 => x1.DeliveryReqId == x.DeliveryReqId).ToList();
                    if (preBOLRetainInfo.Any() && preBOLRetainInfo.Sum(x1 => x1.RetainQuantity) == x.TotalRetainQty)
                    {
                        var retainIndex = x.BOLRetainDetails.FindIndex(x1 => x1.DeliveryReqId == x.DeliveryReqId);
                        if (retainIndex == -1)
                        {
                            x.BOLRetainDetails.AddRange(preBOLRetainInfo);
                        }
                    }
                    else
                    {
                        if (preBOLRetainInfo.Any())
                        {
                            var compartmentRetain = x.RetainCompartmentDetails;
                            foreach (var item in preBOLRetainInfo)
                            {
                                var compartRetainMatch = compartmentRetain.FirstOrDefault(x1 => x1.CompartmentId == item.CompartmentId && x1.TrailerId == item.TrailerId);
                                if (compartRetainMatch.Quantity != item.RetainQuantity)
                                {
                                    item.RetainQuantity = compartRetainMatch.Quantity;
                                }
                            }

                            var retainIndex = x.BOLRetainDetails.FindIndex(x1 => x1.DeliveryReqId == x.DeliveryReqId);
                            if (retainIndex == -1)
                            {
                                x.BOLRetainDetails.AddRange(preBOLRetainInfo);
                            }
                            foreach (var compitem in compartmentRetain)
                            {
                                var retainInfo = x.BOLRetainDetails.FirstOrDefault(x1 => x1.TrailerId == compitem.TrailerId && x1.CompartmentId == compitem.CompartmentId);
                                if (retainInfo == null)
                                {
                                    var bolRetain = x.BOLRetainDetails.FirstOrDefault(x1 => x1.TrailerId == compitem.TrailerId);
                                    if (bolRetain != null)
                                    {
                                        PreBOLRetainDeliveryDetailsModel preBOLRetainDeliveryDetail = new PreBOLRetainDeliveryDetailsModel();
                                        preBOLRetainDeliveryDetail.DeliveryReqId = bolRetain.DeliveryReqId;
                                        preBOLRetainDeliveryDetail.TrailerId = bolRetain.TrailerId;
                                        preBOLRetainDeliveryDetail.CompartmentId = compitem.CompartmentId;
                                        preBOLRetainDeliveryDetail.ProductType = bolRetain.ProductType;
                                        preBOLRetainDeliveryDetail.FuelTypeId = bolRetain.FuelTypeId;
                                        preBOLRetainDeliveryDetail.RetainQuantity = compitem.Quantity;
                                        preBOLRetainDeliveryDetail.IsTrailerRetain = false;
                                        x.BOLRetainDetails.Add(preBOLRetainDeliveryDetail);
                                    }

                                }
                            }
                        }
                    }
                    if (x.BOLRetainDetails.Any())
                    {
                        x.RetainQuantity = x.BOLRetainDetails.Sum(x1 => x1.RetainQuantity);
                        if (x.RetainQuantity != x.TotalRetainQty)
                        {
                            x.RetainQuantity = x.TotalRetainQty;
                        }
                    }
                });
            }
        }

        public async Task<List<DeliveryScheduleGroup>> GetScheduleAndOrdersAsync(int userId, int companyId, long scheduleDate, decimal latitude, decimal longitude, int userTimeOffset = -400, int buyerCompanyId = 0)
        {
            var deliveryGroups = await GetDeliveryScheduleAsync(userId, companyId, scheduleDate, userTimeOffset);

            scheduleDate = DateTimeOffset.FromUnixTimeMilliseconds(scheduleDate).AddMinutes(userTimeOffset).ToUnixTimeMilliseconds();
            string orderIds = string.Join(",", deliveryGroups.SelectMany(t => t.DeliverySchedules.Where(t1 => t1.OrderId != null).Select(t1 => t1.OrderId)).ToList());
            string scheduleIds = string.Join(",", deliveryGroups.SelectMany(t => t.DeliverySchedules.Select(t1 => t1.TrackableScheduleId)).ToList());
            var driverOrders = await GetDriverOrdersForSchedulesAsync(companyId, userId, orderIds, scheduleIds, scheduleDate, buyerCompanyId);

            deliveryGroups.SelectMany(t => t.DeliverySchedules).ToList().ForEach(t =>
            { t.OrderDetails = driverOrders.FirstOrDefault(x => x.OrderId == t.OrderId.ToString()); });

            return deliveryGroups;
        }

        private FuelPickUpLocationViewModel GetPickUpLocationDetails(DeliveryScheduleForDriverRequestViewModel trackableSchedule)
        {
            FuelPickUpLocationViewModel pickupDetails = new FuelPickUpLocationViewModel();
            if (trackableSchedule.SchedulePickupLocationId.HasValue)
            {
                pickupDetails.Longitude = trackableSchedule.SchedulePickUpLongitude;
                pickupDetails.Latitude = trackableSchedule.SchedulePickUpLatitude;
                pickupDetails.Address = trackableSchedule.SchedulePickupAddress;
                pickupDetails.City = trackableSchedule.SchedulePickupCity;
                pickupDetails.StateCode = trackableSchedule.SchedulePickUpStateCode;
                pickupDetails.ZipCode = trackableSchedule.SchedulePickUpZipCode;
                pickupDetails.CountryCode = trackableSchedule.SchedulePickUpCountryCode;
                pickupDetails.CountyName = trackableSchedule.SchedulePickUpCountyName;
                pickupDetails.TerminalName = trackableSchedule.SchedulePickUpTerminalName;
                if (trackableSchedule.SchedulePickUpTerminalId.HasValue)
                {
                    //Changed Terminal Details
                    pickupDetails.TerminalId = trackableSchedule.SchedulePickUpTerminalId.Value;
                }
                else
                {
                    //PickUp Location Details
                    pickupDetails.Id = trackableSchedule.SchedulePickupLocationId;
                    pickupDetails.IsPickUpLocation = true;
                }
                return pickupDetails;
            }
            else if (string.IsNullOrWhiteSpace(trackableSchedule.FrDeliveryRequestId) && (trackableSchedule.OrderPickUpLocationId.HasValue || trackableSchedule.OrderTerminalId.HasValue))
            {
                pickupDetails.Longitude = trackableSchedule.OrderPickUpLocationLongitude;
                pickupDetails.Latitude = trackableSchedule.OrderPickUpLocationLatitude;
                pickupDetails.Address = trackableSchedule.OrderPickUpLocationAddress;
                pickupDetails.City = trackableSchedule.OrderPickUpLocationCity;
                pickupDetails.StateCode = trackableSchedule.OrderPickUpLocationStateCode;
                pickupDetails.ZipCode = trackableSchedule.OrderPickUpLocationZipCode;
                pickupDetails.CountryCode = trackableSchedule.OrderPickUpLocationCountryCode;
                pickupDetails.CountyName = trackableSchedule.OrderPickUpLocationCountyName;

                if (trackableSchedule.OrderPickUpLocationId.HasValue)
                {
                    pickupDetails.TerminalName = trackableSchedule.OrderPickUpLocationTerminalName;
                    if (trackableSchedule.OrderPickUpLocationTerminalId.HasValue)
                    {
                        //Changed Terminal Details
                        pickupDetails.TerminalId = trackableSchedule.OrderPickUpLocationTerminalId.Value;
                    }
                    else
                    {
                        //PickUp Location Details
                        pickupDetails.Id = trackableSchedule.OrderPickUpLocationId;
                        pickupDetails.IsPickUpLocation = true;
                    }
                }
                else
                {
                    //Order Terminal Details
                    pickupDetails.TerminalId = trackableSchedule.OrderTerminalId.HasValue ? trackableSchedule.OrderTerminalId.Value : 0;
                    pickupDetails.TerminalName = trackableSchedule.OrderTerminalName;
                }
                return pickupDetails;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<DeliveryScheduleForDriverViewModel>> GetDropCompletedForDriverAsync(int userId, long scheduleDate, int offset)
        {
            //Get todays or specific date , drop completed for logged in Driver (for all Orders)
            var response = new List<DeliveryScheduleForDriverViewModel>();
            var helperDomain = new HelperDomain(this);
            try
            {
                var user = await Context.DataContext.Users.Include(t => t.Company).FirstOrDefaultAsync(t => t.Id == userId);
                if (user != null)
                {
                    DateTimeOffset dropStartDate = DateTimeOffset.FromUnixTimeMilliseconds(scheduleDate).AddMinutes(offset);
                    var allInvoices = Context.DataContext.Invoices.Include("Order").Where(t => t.Order.IsActive && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && t.IsActive
                                      && !t.IsBuyPriceInvoice && t.DriverId == user.Id && t.Order.AcceptedCompanyId == user.CompanyId
                    && (t.CreatedDate.Day == dropStartDate.Day && t.CreatedDate.Month == dropStartDate.Month && t.CreatedDate.Year == dropStartDate.Year));

                    foreach (var invoice in allInvoices)
                    {
                        DeliveryScheduleForDriverViewModel deliverySchedule = new DeliveryScheduleForDriverViewModel();
                        if (invoice.Order != null)
                        {
                            bool isSchedule = invoice.TrackableSchedule != null;
                            deliverySchedule.OrderId = invoice.Order.Id;
                            deliverySchedule.PoNumber = invoice.PoNumber;
                            deliverySchedule.FuelType = helperDomain.GetProductName(invoice.Order.FuelRequest.MstProduct);
                            deliverySchedule.GallonsOrdered = invoice.DroppedGallons;
                            deliverySchedule.CompanyName = invoice.Order.FuelRequest.Job.Company.Name;
                            deliverySchedule.JobId = invoice.Order.FuelRequest.Job.Id;
                            deliverySchedule.JobName = invoice.Order.FuelRequest.Job.Name;
                            deliverySchedule.JobAddress = invoice.Order.FuelRequest.Job.Address;
                            deliverySchedule.JobCity = invoice.Order.FuelRequest.Job.City;
                            deliverySchedule.JobState = invoice.Order.FuelRequest.Job.MstState.Name;
                            deliverySchedule.JobZip = invoice.Order.FuelRequest.Job.ZipCode;
                            deliverySchedule.StartTime = invoice.DropStartDate.ToString(Resource.constFormat12HourTime);
                            deliverySchedule.EndTime = invoice.DropEndDate.ToString(Resource.constFormat12HourTime);
                            deliverySchedule.UtcStartTime = invoice.DropStartDate.ToUnixTimeMilliseconds();
                            deliverySchedule.UtcEndTime = invoice.DropEndDate.ToUnixTimeMilliseconds();
                            deliverySchedule.UnitOfMeasurement = (int)invoice.UoM;
                            deliverySchedule.Currency = (int)invoice.Currency;
                            deliverySchedule.DeliveryRequestId = isSchedule ? invoice.TrackableSchedule.FrDeliveryRequestId : string.Empty;
                            deliverySchedule.CompletedType = (int)TrackableDeliveryScheduleStatus.Completed;
                            deliverySchedule.TrackableScheduleId = invoice.TrackableScheduleId > 0 ? invoice.TrackableScheduleId.Value : 0;
                            deliverySchedule.GroupedParentDrId = isSchedule && !string.IsNullOrWhiteSpace(invoice.TrackableSchedule.GroupParentDRId) ? invoice.TrackableSchedule.GroupParentDRId : string.Empty;
                            deliverySchedule.DeliveryLevelPO = isSchedule ? invoice.TrackableSchedule.DeliveryLevelPO : string.Empty;
                            response.Add(deliverySchedule);
                        }
                    }

                    var allPreLoadBolCompleted = await Context.DataContext.PreLoadBolDetails.Where(t => t.TrackableSchedule.DeliveryScheduleStatusId == (int)TrackableDeliveryScheduleStatus.PreLoadBolCompleted
                                                             && t.IsActive && t.TrackableSchedule.DriverId == userId && (t.PickupDate.Day == dropStartDate.Day && t.PickupDate.Month == dropStartDate.Month && t.PickupDate.Year == dropStartDate.Year)).GroupBy(t => t.TrackableScheduleId).ToListAsync();
                    var uniquePreLoadBolCompleted = allPreLoadBolCompleted.Select(g => g.Last()).ToList();
                    foreach (var preLoadBolCompleted in uniquePreLoadBolCompleted)
                    {
                        DeliveryScheduleForDriverViewModel deliverySchedule = new DeliveryScheduleForDriverViewModel();
                        var order = preLoadBolCompleted.TrackableSchedule.Order;
                        if (order != null)
                        {
                            deliverySchedule.OrderId = order.Id;
                            deliverySchedule.PoNumber = order.PoNumber;
                            deliverySchedule.FuelType = helperDomain.GetProductName(order.FuelRequest.MstProduct);
                            deliverySchedule.GallonsOrdered = 0;
                            deliverySchedule.CompanyName = order.FuelRequest.Job.Company.Name;
                            deliverySchedule.JobId = order.FuelRequest.Job.Id;
                            deliverySchedule.JobName = order.FuelRequest.Job.Name;
                            deliverySchedule.JobAddress = order.FuelRequest.Job.Address;
                            deliverySchedule.JobCity = order.FuelRequest.Job.City;
                            deliverySchedule.JobState = order.FuelRequest.Job.MstState.Name;
                            deliverySchedule.JobZip = order.FuelRequest.Job.ZipCode;
                            deliverySchedule.StartTime = preLoadBolCompleted.PickupDate.ToString(Resource.constFormat12HourTime);
                            deliverySchedule.EndTime = preLoadBolCompleted.PickupDate.ToString(Resource.constFormat12HourTime);
                            deliverySchedule.UtcStartTime = preLoadBolCompleted.PickupDate.ToUnixTimeMilliseconds();
                            deliverySchedule.UtcEndTime = preLoadBolCompleted.PickupDate.ToUnixTimeMilliseconds();
                            deliverySchedule.UnitOfMeasurement = (int)order.FuelRequest.Job.UoM;
                            deliverySchedule.Currency = (int)order.FuelRequest.Job.Currency;
                            deliverySchedule.CompletedType = (int)TrackableDeliveryScheduleStatus.PreLoadBolCompleted;
                            deliverySchedule.DeliveryRequestId = preLoadBolCompleted.TrackableSchedule != null ? preLoadBolCompleted.TrackableSchedule.FrDeliveryRequestId : string.Empty;
                            deliverySchedule.TrackableScheduleId = preLoadBolCompleted.TrackableScheduleId > 0 ? preLoadBolCompleted.TrackableScheduleId.Value : 0;
                            deliverySchedule.GroupedParentDrId = preLoadBolCompleted.TrackableSchedule != null && !string.IsNullOrWhiteSpace(preLoadBolCompleted.TrackableSchedule.GroupParentDRId) ? preLoadBolCompleted.TrackableSchedule.GroupParentDRId : string.Empty;
                            deliverySchedule.DeliveryLevelPO = preLoadBolCompleted.TrackableSchedule != null ? preLoadBolCompleted.TrackableSchedule.DeliveryLevelPO : string.Empty;
                            response.Add(deliverySchedule);
                        }
                    }

                    if (response.Any(t => !string.IsNullOrWhiteSpace(t.GroupedParentDrId)))
                    {
                        var groupedParentDrIds = response.Where(t => t.GroupedParentDrId != string.Empty).Select(t => t.GroupedParentDrId).ToList();
                        var trackableSchedules = await Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(t => groupedParentDrIds.Contains(t.GroupParentDRId) && t.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.Completed
                                                                         && t.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.CompletedLate
                                                                          && t.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.PreLoadBolCompleted
                                                                          && t.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.RescheduledCompleted
                                                                          && t.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.RescheduledLate
                                                                          && t.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.UnplannedDropCompleted
                                                                          && t.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.Canceled).Select(t => new { t.Id, t.GroupParentDRId }).ToListAsync();
                        foreach (var schedule in response.Where(t => t.GroupedParentDrId != string.Empty))
                        {
                            schedule.IsOnGoingScheduleExists = trackableSchedules.Any(t => t.GroupParentDRId == schedule.GroupedParentDrId && t.Id != schedule.TrackableScheduleId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetDropCompletedForDriverAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> SaveAppLocation(AppLocationViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel();
            try
            {
                if (!string.IsNullOrEmpty(viewModel.FCMAppId))
                {
                    AppLocation appLocation = await Context.DataContext.AppLocations.FirstOrDefaultAsync(t => t.UserId == viewModel.UserId && t.FCMAppId == viewModel.FCMAppId && t.AppTypeId == (int)viewModel.AppType);
                    if (appLocation != null)
                    {
                        appLocation.UpdatedDate = DateTime.Now;
                        appLocation.Latitude = viewModel.Latitude;
                        appLocation.Longitude = viewModel.Longitude;
                        appLocation.IsUserLogout = false;
                    }
                    else
                    {
                        appLocation = viewModel.ToEntity(appLocation);
                        Context.DataContext.AppLocations.Add(appLocation);
                    }

                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageSuccess;

                    await Context.CommitAsync();

                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "SaveAppLocation", ex.Message, ex);
            }

            return response;
        }

        public async Task<StatusViewModel> UpdateOrderLicenses(List<int> licenses, int orderId, int userId)
        {
            StatusViewModel response = new StatusViewModel();
            try
            {
                var supplier = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == userId);
                var order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == orderId);
                var existingLicenses = order.TaxExemptLicenses.ToList();
                existingLicenses.ForEach(t => order.TaxExemptLicenses.Remove(t));
                if (licenses != null && licenses.Count > 0)
                {
                    order.TaxExemptLicenses = supplier.Company.TaxExemptLicenses.Where(t => licenses.Contains(t.Id)).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "UpdateOrderLicenses", ex.Message, ex);
                return response;
            }
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    await Context.CommitAsync();
                    transaction.Commit();
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageSuccess;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("OrderDomain", "UpdateOrderLicenses", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusViewModel> EnrouteDelivery(EnrouteDeliveryViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel();

            try
            {
                response = await SaveAppLocationAndEnrouteDelivery(viewModel);

                if (response.StatusCode == Status.Success && viewModel.OrderId > 0)
                {
                    viewModel.Message.Notify = true;
                    response = await ContextFactory.Current.GetDomain<PushNotificationDomain>().NotificationToBuyer(viewModel.Message, viewModel.OrderId.Value);
                }

                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.errMessageSuccess;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "EnrouteDelivery", ex.Message, ex);
            }

            return response;
        }

        public async Task<StatusViewModel> SaveAppLocationAndEnrouteDelivery(EnrouteDeliveryViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    await SaveAppLocation(viewModel);

                    var enrouteDeliveryHistory = viewModel.ToEntity();
                    if (enrouteDeliveryHistory.StatusId == (int)EnrouteDeliveryStatus.Delayed)
                    {
                        var prevDeliveryStatus = Context.DataContext.EnrouteDeliveryHistories.Where(t => (enrouteDeliveryHistory.DeliveryScheduleId == 0 || t.DeliveryScheduleId == enrouteDeliveryHistory.DeliveryScheduleId) && t.OrderId == enrouteDeliveryHistory.OrderId).OrderByDescending(t => t.Id).FirstOrDefault();
                        if (prevDeliveryStatus != null && prevDeliveryStatus.StatusId == (int)EnrouteDeliveryStatus.OnTheWayToJob)
                        {
                            enrouteDeliveryHistory.StatusId = (int)EnrouteDeliveryStatus.StartAndDelay;
                        }
                    }
                    Context.DataContext.EnrouteDeliveryHistories.Add(enrouteDeliveryHistory);
                    await Context.CommitAsync();

                    if (enrouteDeliveryHistory.StatusId == (int)EnrouteDeliveryStatus.PreLoadBolCompleted && viewModel.TrackableScheduleId.HasValue)
                    {
                        var trackableSchedule = Context.DataContext.DeliveryScheduleXTrackableSchedules.SingleOrDefault(t => t.Id == viewModel.TrackableScheduleId.Value);
                        if (trackableSchedule != null)
                        {
                            trackableSchedule.DeliveryScheduleStatusId = (int)TrackableDeliveryScheduleStatus.PreLoadBolCompleted;
                            Context.DataContext.Entry(trackableSchedule).State = EntityState.Modified;
                            Context.Commit();
                        }
                    }

                    transaction.Commit();
                    new ScheduleBuilderDomain(this).UpdateDeliveryRequestStatus(viewModel.TrackableScheduleId, viewModel.StatusId, viewModel.UserId);
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageSuccess;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("OrderDomain", "SaveAppLocationAndEnrouteDelivery", ex.Message, ex);
                }
            }
            return response;
        }

        private async Task SaveAppLocation(EnrouteDeliveryViewModel viewModel)
        {
            try
            {
                var appLocation = Context.DataContext.AppLocations.FirstOrDefault(t => t.UserId == viewModel.UserId && t.FCMAppId == viewModel.FCMAppId);
                if (appLocation != null)
                {
                    if (appLocation.OrderId == viewModel.OrderId && viewModel.StatusId == (int)EnrouteDeliveryStatus.DriverCanceled)
                    {
                        appLocation.OrderId = null;
                        appLocation.DeliveryScheduleId = null;
                        appLocation.TrackableScheduleId = null;
                        appLocation.StatusId = viewModel.StatusId;
                    }

                    if (viewModel.StatusId == (int)EnrouteDeliveryStatus.OnTheWayToJob || viewModel.StatusId == (int)EnrouteDeliveryStatus.OnTheWayToTerminal
                        || viewModel.StatusId == (int)EnrouteDeliveryStatus.ArrivedAtJob || viewModel.StatusId == (int)EnrouteDeliveryStatus.ArrivedAtTerminal
                        || viewModel.StatusId == (int)EnrouteDeliveryStatus.CompletedDrop || viewModel.StatusId == (int)EnrouteDeliveryStatus.FuelTruckRetain)
                    {
                        appLocation.StatusId = viewModel.StatusId;
                        appLocation.OrderId = viewModel.OrderId;
                        appLocation.DeliveryScheduleId = viewModel.DeliveryScheduleId > 0 ? viewModel.DeliveryScheduleId : null;
                        appLocation.TrackableScheduleId = viewModel.TrackableScheduleId > 0 ? viewModel.TrackableScheduleId : null;
                        appLocation.Latitude = viewModel.Latitude ?? 0;
                        appLocation.Longitude = viewModel.Longitude ?? 0;
                        appLocation.UpdatedDate = DateTime.Now;
                        //AddEnrouteStatusWebNotification(viewModel);
                    }

                    Context.DataContext.Entry(appLocation).State = EntityState.Modified;
                }
                else
                {
                    appLocation = ToAppLocation(viewModel);
                    Context.DataContext.AppLocations.Add(appLocation);
                }

                if (viewModel.StatusId == (int)EnrouteDeliveryStatus.ArrivedAtJob)
                {
                    NotificationDomain notificationDomain = new NotificationDomain(this);
                    await notificationDomain.AddNotificationEventAsync(EventType.DriverArrivedJob, appLocation.Id, viewModel.UserId);
                }
                await Context.CommitAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "SaveAppLocation", ex.Message, ex);

            }
        }

        //private void AddEnrouteStatusWebNotification(EnrouteDeliveryViewModel viewModel)
        //{
        //    try
        //    {
        //        var dispatchDomain = new DispatchDomain(this);
        //        var terminalName = dispatchDomain.GetTerminalName(viewModel.TrackableScheduleId, viewModel.OrderId).Result;
        //        NotificationDispatchLocationViewModel dispatchLocation = new NotificationDispatchLocationViewModel()
        //        {
        //            DispatchNotificationType = DispatchNotificationType.EnrouteStatus,
        //            DeliveryScheduleId = viewModel.DeliveryScheduleId,
        //            TrackableScheduleId = viewModel.TrackableScheduleId,
        //            CurrentTerminalName = terminalName,
        //            Status = (EnrouteDeliveryStatus)viewModel.StatusId
        //        };

        //        dispatchDomain.ProcessDispatchLocationForWebNotifications(dispatchLocation, viewModel.OrderId, viewModel.UserId);
        //    }
        //    catch (Exception ex)
        //    {

        //        LogManager.Logger.WriteException("OrderDomain", "AddEnrouteStatusWebNotification", ex.Message, ex);
        //    }
        //}

        private AppLocation ToAppLocation(EnrouteDeliveryViewModel viewModel)
        {
            var appLocation = new AppLocation();
            appLocation.UserId = viewModel.UserId;
            appLocation.AppTypeId = (int)AppType.DriverApp;
            appLocation.FCMAppId = viewModel.FCMAppId;
            appLocation.Latitude = viewModel.Latitude ?? 0;
            appLocation.Longitude = viewModel.Longitude ?? 0;
            appLocation.UpdatedDate = DateTime.Now;
            appLocation.OrderId = viewModel.OrderId;
            appLocation.DeliveryScheduleId = viewModel.DeliveryScheduleId > 0 ? viewModel.DeliveryScheduleId : null;
            appLocation.TrackableScheduleId = viewModel.TrackableScheduleId > 0 ? viewModel.TrackableScheduleId : null;
            appLocation.StatusId = viewModel.StatusId;
            appLocation.IsUserLogout = false;
            return appLocation;
        }

        public async Task<List<DriverCalendarViewModel>> GetMonthlyDriverSchedule(int userId, long calendarDate)
        {
            var response = new List<DriverCalendarViewModel>();
            try
            {
                var user = await Context.DataContext.Users.Include(t => t.Company).SingleOrDefaultAsync(t => t.IsActive && t.Id == userId);
                if (user != null && user.Company != null)
                {
                    DateTimeOffset calendarDt = DateTimeOffset.FromUnixTimeMilliseconds(calendarDate); //calendarDate is today's date
                    var driverSchedules = await ContextFactory.Current.GetDomain<DashboardDomain>().GetDriverCalendarDataAsync(user.Company.Id, userId, calendarDt.Month, calendarDt.Year, calendarDt.Day);
                    driverSchedules = driverSchedules.Where(t => t.eventStatus != (int)TrackableDeliveryScheduleStatus.Canceled
                                                                    && t.eventStatus != (int)TrackableDeliveryScheduleStatus.MissedAndCanceled
                                                                    && t.eventStatus != (int)TrackableDeliveryScheduleStatus.MissedAndRescheduled
                                                                    && t.eventStatus != (int)TrackableDeliveryScheduleStatus.Rescheduled).ToList();
                    List<DateTimeOffset> allScheduleDates = new List<DateTimeOffset>();

                    foreach (var item in driverSchedules)
                    {
                        allScheduleDates.Add(Convert.ToDateTime(item.start));
                    }

                    allScheduleDates = allScheduleDates.Distinct().ToList();
                    allScheduleDates.ForEach(t => response.Add(
                                                    new DriverCalendarViewModel()
                                                    {
                                                        Date = t.Date,
                                                        CalendarDropType = (int)CalendarDropType.DropScheduled,
                                                        CalendarDate = t.ToUnixTimeMilliseconds()
                                                    }));

                    DateTime dropStartDate = new DateTime(calendarDt.Year, calendarDt.Month, 1);
                    DateTime dropEndDate = calendarDt.Date;
                    List<DateTimeOffset> allDropDates = new List<DateTimeOffset>();
                    var allInvoices = Context.DataContext.Invoices.Include("Order").Where(t => t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && t.IsActive && !t.IsBuyPriceInvoice
                                  && t.DriverId == user.Id && (t.DropStartDate >= dropStartDate.Date && t.DropEndDate < dropEndDate.Date)).ToList();

                    foreach (var item in allInvoices)
                    {
                        allDropDates.Add(item.DropStartDate.Date);
                    }
                    allDropDates = allDropDates.Distinct().ToList();

                    allDropDates.ForEach(t => response.Add(
                                                    new DriverCalendarViewModel()
                                                    {
                                                        Date = t.Date,
                                                        CalendarDropType = (int)CalendarDropType.DropCompleted,
                                                        CalendarDate = t.ToUnixTimeMilliseconds()
                                                    }));
                }

                response = response.OrderBy(t => t.Date).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetMonthlyDriverSchedule", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> AssignDriverToOrder(NotificationToBuyerViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel();
            NotificationDomain notificationDomain = new NotificationDomain(this);
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    AppLocation appLocation = Context.DataContext.AppLocations.FirstOrDefault(t => t.UserId == viewModel.DriverId && t.FCMAppId == viewModel.FCMAppId && t.AppTypeId == (int)AppType.DriverApp);
                    if (appLocation != null)
                    {
                        appLocation.OrderId = viewModel.OrderId;
                        if (viewModel.DeliveryScheduleId > 0)
                        {
                            appLocation.DeliveryScheduleId = viewModel.DeliveryScheduleId;
                        }

                        if (viewModel.TrackableScheduleId.HasValue && viewModel.TrackableScheduleId.Value > 0)
                        {
                            appLocation.TrackableScheduleId = viewModel.TrackableScheduleId;
                        }

                        appLocation.StatusId = (int)EnrouteDeliveryStatus.OnTheWayToJob;

                        Context.DataContext.Entry(appLocation).State = EntityState.Modified;
                        await Context.CommitAsync();

                        await notificationDomain.AddNotificationEventAsync(EventType.DriverOnWayToJob, appLocation.Id, viewModel.DriverId);
                    }

                    var enrouteViewModel = new EnrouteDeliveryViewModel();
                    enrouteViewModel.UserId = viewModel.DriverId;
                    enrouteViewModel.OrderId = viewModel.OrderId;

                    if (viewModel.DeliveryScheduleId > 0)
                    {
                        enrouteViewModel.DeliveryScheduleId = viewModel.DeliveryScheduleId;
                    }

                    if (viewModel.TrackableScheduleId.HasValue && viewModel.TrackableScheduleId.Value > 0)
                    {
                        enrouteViewModel.TrackableScheduleId = viewModel.TrackableScheduleId;
                    }

                    enrouteViewModel.StatusId = (int)EnrouteDeliveryStatus.OnTheWayToJob;

                    var enrouteDeliveryHistory = enrouteViewModel.ToEntity();
                    Context.DataContext.EnrouteDeliveryHistories.Add(enrouteDeliveryHistory);

                    await Context.CommitAsync();
                    transaction.Commit();

                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageSuccess;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("OrderDomain", "AssignDriverToOrder", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusViewModel> DriverUncanceledSchedule(NotificationToBuyerViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (var deliveryScheduleId in viewModel.UncanceledDeliveryScheduleId)
                    {
                        var enrouteViewModel = new EnrouteDeliveryViewModel();
                        enrouteViewModel.UserId = viewModel.DriverId;
                        enrouteViewModel.OrderId = viewModel.OrderId;
                        enrouteViewModel.DeliveryScheduleId = deliveryScheduleId;
                        enrouteViewModel.StatusId = (int)EnrouteDeliveryStatus.DriverUncanceled;

                        var enrouteDeliveryHistory = enrouteViewModel.ToEntity();
                        Context.DataContext.EnrouteDeliveryHistories.Add(enrouteDeliveryHistory);

                        await Context.CommitAsync();
                    }

                    transaction.Commit();

                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageSuccess;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("OrderDomain", "DriverUncanceledSchedule", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<DeliveryDetailsViewModel> GetDeliveryDetails(int orderId, int deliveryRequestId, long scheduleDate = 0)
        {
            var response = new DeliveryDetailsViewModel();
            int driverId = 0;
            try
            {
                DateTimeOffset deliveryDate = DateTimeOffset.FromUnixTimeMilliseconds(scheduleDate);
                var order = await Context.DataContext.Orders.Include(t => t.OrderDeliverySchedules)
                     .Include("OrderDeliverySchedules.DeliverySchedule").SingleOrDefaultAsync(t => t.Id == orderId);
                if (order != null)
                {
                    var latestSchedules = GetLatestOrderDeliverySchedule(order.OrderDeliverySchedules);
                    if (latestSchedules != null)
                    {
                        if (order.FuelRequest.FuelRequestDetail.DeliveryTypeId == (int)DeliveryType.MultipleDeliveries &&
                            latestSchedules.Count() > 0)
                        {
                            DeliveryScheduleViewModel deliverySchedule = latestSchedules.Where(t => t.DeliveryRequestId == deliveryRequestId)
                                                            .Select(t => t.DeliverySchedule)
                                                            .GroupBy(t => t.GroupId)
                                                            .Select(g => new { Items = g.ToList() })
                                                            .Select(t => t.Items.ToViewModel()).FirstOrDefault();
                            if (deliverySchedule != null)
                            {
                                response.ScheduleDate = deliveryDate.Date;
                                response.ScheduleStartTime = deliverySchedule.ScheduleStartTime;
                                response.ScheduleEndTime = deliverySchedule.ScheduleEndTime;
                                response.DriverName = deliverySchedule.DriverName;
                                response.PhoneNumber = deliverySchedule.PhoneNumber;
                                response.JobLatitude = order.FuelRequest.Job.Latitude;
                                response.JobLongitude = order.FuelRequest.Job.Longitude;
                                response.JobAddress = order.FuelRequest.Job.Address;
                                response.JobName = order.FuelRequest.Job.Name;
                                response.PONumber = order.PoNumber;
                                driverId = deliverySchedule.DriverId ?? 0;

                                DateTime startTime = Convert.ToDateTime(deliverySchedule.ScheduleStartTime);
                                DateTimeOffset ScheduleDateTime = new DateTimeOffset(deliveryDate.Year, deliveryDate.Month, deliveryDate.Day, startTime.Hour, startTime.Minute, startTime.Second, deliveryDate.Offset);
                                response.ScheduleDateTime = ScheduleDateTime.ToUnixTimeMilliseconds();
                            }
                        }
                        else
                        {
                            response.ScheduleDate = order.FuelRequest.FuelRequestDetail.StartDate;
                            response.ScheduleStartTime = Convert.ToDateTime(order.FuelRequest.FuelRequestDetail.StartTime.ToString()).ToShortTimeString();
                            response.ScheduleEndTime = Convert.ToDateTime(order.FuelRequest.FuelRequestDetail.EndTime.ToString()).ToShortTimeString();
                            response.JobLatitude = order.FuelRequest.Job.Latitude;
                            response.JobLongitude = order.FuelRequest.Job.Longitude;
                            response.JobAddress = order.FuelRequest.Job.Address;
                            response.JobName = order.FuelRequest.Job.Name;
                            response.PONumber = order.PoNumber;

                            DateTime startTime = Convert.ToDateTime(response.ScheduleStartTime);
                            DateTimeOffset ScheduleDateTime = new DateTimeOffset(deliveryDate.Year, deliveryDate.Month, deliveryDate.Day, startTime.Hour, startTime.Minute, startTime.Second, deliveryDate.Offset);
                            response.ScheduleDateTime = ScheduleDateTime.ToUnixTimeMilliseconds();

                            var driver = order.OrderXDrivers.FirstOrDefault(t1 => t1.IsActive);
                            if (driver != null)
                            {
                                response.DriverName = $"{driver.User.FirstName} {driver.User.LastName}";
                                response.PhoneNumber = $"{driver.User.PhoneNumber}";
                                driverId = driver.DriverId;
                            }
                        }

                        var driverLocation = Context.DataContext.AppLocations.Where(x => x.UserId == driverId).OrderByDescending(x => x.UpdatedDate).FirstOrDefault();
                        if (driverLocation != null)
                        {
                            response.DriverLatitude = driverLocation.Latitude;
                            response.DriverLongitude = driverLocation.Longitude;

                            bool isDriverStartedDelivery = IsDriverStartedDelivery(driverLocation, orderId, deliveryRequestId);
                            if (isDriverStartedDelivery)
                            {
                                response.IsDriverStartedDelivery = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetDeliveryDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<DriverDeliveryDetailsViewModel> GetDriverDeliveryDetails(int jobId = 0, int orderId = 0)
        {
            DriverDeliveryDetailsViewModel response = new DriverDeliveryDetailsViewModel();
            try
            {
                var job = await Context.DataContext.Jobs.FirstOrDefaultAsync(t => t.IsActive && t.Id == jobId);
                if (job != null)
                {
                    response.CompanyName = job.Company.Name;
                    response.JobName = job.Name;
                    response.JobAddress = job.Address;
                    response.JobCity = job.City;
                    response.JobState = job.MstState.Name;
                    response.JobZip = job.ZipCode;
                    response.JobLatitude = job.Latitude;
                    response.JobLongitude = job.Longitude;

                    response.ContactPersons = job.Users1.Select(t => new ContactPersonViewModel() { Id = t.Id, Name = $"{t.FirstName} {t.LastName}", Email = t.Email, PhoneNumber = t.PhoneNumber }).ToList();
                }

                if (jobId == 0)
                {
                    response.ContactPersons.Add(new ContactPersonViewModel(Status.Success));
                }

                var order = await Context.DataContext.Orders.FirstOrDefaultAsync(t => t.IsActive && t.Id == orderId);
                if (order != null)
                {
                    response.SpecialInstructions = order.FuelRequest.SpecialInstructions.Select(t1 => t1.Instruction).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetDriverDeliveryDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> CopyBrokeredOrderTaxesToNewOrder(Order newOrder, Order brokeredOrder, UserContext userContext)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    newOrder.OrderTaxDetails = new List<OrderTaxDetail>();
                    foreach (var item in brokeredOrder.OrderTaxDetails.Where(t => t.IsActive).ToList())
                    {
                        OrderTaxDetail orderTaxDetail = new OrderTaxDetail();
                        orderTaxDetail.AddedBy = userContext.Id;
                        orderTaxDetail.AddedDate = DateTimeOffset.Now;
                        orderTaxDetail.AddedByCompanyId = userContext.CompanyId;
                        orderTaxDetail.OrderId = newOrder.Id;
                        orderTaxDetail.OtherFuelTypeId = item.OtherFuelTypeId;
                        orderTaxDetail.TaxDescription = item.TaxDescription;
                        orderTaxDetail.TaxPricingTypeId = item.TaxPricingTypeId;
                        orderTaxDetail.TaxRate = item.TaxRate;
                        orderTaxDetail.IsActive = true;
                        newOrder.OrderTaxDetails.Add(orderTaxDetail);
                    }

                    await Context.CommitAsync();
                    transaction.Commit();

                    //Send response
                    response.StatusCode = Status.Success;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("OrderDomain", "CopyBrokeredOrderTaxesToNewOrder", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusViewModel> AssignNewTerminalToOrderAsync(int terminalId, int orderId, bool IsCityGroupTerminal = false)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == orderId);
                    if (order != null)
                    {
                        if (IsCityGroupTerminal)
                        {
                            //check if pricing available for city group terminal
                            var productMapping = Context.DataContext.MstProductMappings
                                                .FirstOrDefault(t => t.ProductId == order.FuelRequest.FuelTypeId && t.ExternalTerminalId == order.TerminalId);
                            if (productMapping != null)
                            {
                                PricingServiceDomain pricingServiceDomain = new PricingServiceDomain(this);
                                var isPriceAvailable = await pricingServiceDomain.IsCityRackPriceAvailable(productMapping.ExternalProductId, terminalId, PricingSource.Axxis, order.AcceptedDate.DateTime);
                                if (isPriceAvailable)
                                {
                                    if (terminalId == 0)
                                    {
                                        order.CityGroupTerminalId = null;
                                    }
                                    else
                                    {
                                        order.CityGroupTerminalId = terminalId;
                                    }
                                }
                                else
                                {
                                    response.StatusCode = Status.Failed;
                                    response.StatusMessage = Resource.errMessageTerminalPriceNotAvailable;
                                    return response;
                                }
                            }
                            else
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = Resource.errMessageTerminalPriceNotAvailable;
                                return response;
                            }
                        }
                        else
                        {
                            if (terminalId == 0)
                            {
                                order.TerminalId = null;
                            }
                            else
                            {
                                order.TerminalId = terminalId;
                                // var tfxProductTypeId = Context.DataContext.MstProducts.Where(t => t.Id == order.FuelRequest.FuelTypeId && t.IsActive).FirstOrDefault()?.TfxProductId ?? 0;
                                // order.OrderAdditionalDetail.SupplierAssignedProductName = helperDomain.GetSupplierAssignedProductName(order.AcceptedCompanyId, tfxProductTypeId, terminalId);
                            }
                        }

                        Context.DataContext.Entry(order).State = EntityState.Modified;
                        await Context.CommitAsync();
                        transaction.Commit();

                        if (order.FuelRequest.FuelRequest1 != null && order.FuelRequest.GetParentFuelRequest().FuelRequest1 != null)
                        {
                            var brokeredOrder = order.FuelRequest.GetParentFuelRequest().FuelRequest1.Orders.LastOrDefault();
                            if (brokeredOrder != null)
                            {
                                //assing new terminal to all chained orders in broker case
                                await AssignNewTerminalToOrderAsync(terminalId, brokeredOrder.Id, IsCityGroupTerminal);
                            }
                        }
                    }


                    //Send response
                    response.StatusCode = Status.Success;
                    response.StatusMessage = string.Format(Resource.errMessageTerminalAssignmentSuccess, IsCityGroupTerminal ? "City Rack" : string.Empty);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("OrderDomain", "AssignNewTerminalToOrderAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusViewModel> AssignNewPoContact(int poContactId, int orderId)
        {
            StatusViewModel response = new StatusViewModel();
            try
            {
                var order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == orderId);
                if (order != null)
                {
                    var fuelRequestDetail = order.FuelRequest.FuelRequestDetail;
                    fuelRequestDetail.PoContactId = poContactId;

                    Context.DataContext.Entry(fuelRequestDetail).State = EntityState.Modified;
                    await Context.CommitAsync();
                }

                //Send response
                response.StatusCode = Status.Success;
                response.StatusMessage = string.Format(Resource.errMessagePoContactAssignementSuccess);
            }
            catch (Exception ex)
            {
                response.StatusMessage = string.Format(Resource.errMessagePoContactAssignementFailed);
                LogManager.Logger.WriteException("OrderDomain", "AssignNewPoContact", ex.Message, ex);
            }

            return response;
        }

        public async Task<StatusViewModel> SetDefaultInvoiceTypeForOrder(int orderId, bool defaultInvoiceTypeManual)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == orderId);
                    if (order != null)
                    {
                        order.DefaultInvoiceType = defaultInvoiceTypeManual ? (int)InvoiceType.Manual : (int)InvoiceType.DigitalDropTicketManual;

                        Context.DataContext.Entry(order).State = EntityState.Modified;
                        await Context.CommitAsync();
                    }
                    transaction.Commit();

                    //Send response - no status message needed for success here as we don't show it
                    response.StatusCode = Status.Success;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("OrderDomain", "SetDefaultInvoiceTypeForOrder", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusViewModel> IsOrderHasInvoiceAsync(int orderId)
        {
            StatusViewModel response = new StatusViewModel();

            try
            {
                var order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == orderId);
                if (order != null && order.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId != (int)OrderStatus.Open)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageOrderCancelFailedOrderNotOpen;
                    return response;
                }

                //Send response
                response.StatusCode = Status.Success;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "CheckOrderHasInvoiceAsync", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<MapViewModel>> GetBrokerMap(int companyId, OrderFilterViewModel orderFilter = null)
        {
            List<MapViewModel> response = new List<MapViewModel>();

            try
            {
                response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetBrokerMapAsync(companyId, orderFilter);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetBrokerMap", ex.Message, ex);
            }

            return response;
        }
        public async Task<List<MapViewModel>> GetBuyerMapAsync(int userId, OrderFilterViewModel orderFilter = null)
        {
            List<MapViewModel> response = new List<MapViewModel>();
            var helperDomain = new HelperDomain(this);
            try
            {
                var groupIdslist = helperDomain.GetGroupList(orderFilter.GroupIds);
                var user = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == userId && t.IsActive);
                if (user != null && user.Company != null)
                {
                    var allOrders = Context.DataContext.Orders.Include(t => t.FuelRequest.Job)
                        .Include(t => t.FuelRequest.Job.MstState).Include(t => t.FuelRequest.Job.MstCountry)
                        .Include(t => t.FuelRequest.Job.Users1)
                        .Where(
                                t => t.IsActive &&
                                ((groupIdslist.Count == 0 && t.BuyerCompanyId == user.Company.Id) ||
                                 (groupIdslist.Count > 0 && t.BuyerCompany.SubCompanies.Any(t1 => t1.SubCompanyId == t.BuyerCompanyId && groupIdslist.Contains(t1.CompanyGroupId)))) &&
                                t.ParentId == null &&
                                ((groupIdslist.Count == 0 && t.FuelRequest.Job.Company.Id == user.Company.Id) ||
                                 (groupIdslist.Count > 0 && t.BuyerCompany.SubCompanies.Any(t1 => t1.SubCompanyId == t.FuelRequest.Job.Company.Id && groupIdslist.Contains(t1.CompanyGroupId)))) &&
                                t.FuelRequest.Job.CountryId == orderFilter.CountryId
                              );

                    allOrders = ApplyBuyerOrderFilter(orderFilter, allOrders).OrderByDescending(t => t.Id);

                    await allOrders.ForEachAsync(t => response.Add(new MapViewModel(Status.Success)
                    {
                        JobId = t.FuelRequest.Job.Id,
                        Name = t.FuelRequest.Job.Name,
                        Address = t.FuelRequest.Job.Address,
                        State = t.FuelRequest.Job.MstState.Code,
                        Country = t.FuelRequest.Job.MstCountry.Code,
                        City = t.FuelRequest.Job.City,
                        ZipCode = t.FuelRequest.Job.ZipCode,
                        Latitude = t.FuelRequest.Job.Latitude,
                        Longitude = t.FuelRequest.Job.Longitude,
                        ContactPersons = t.FuelRequest.Job.Users1.Select(t1 => new ContactPersonViewModel() { Id = t.Id, Name = $"{t1.FirstName} {t1.LastName}", Email = t1.Email, PhoneNumber = t1.PhoneNumber }).ToList()
                    }));
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetBuyerMap", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<MapViewModel>> GetSupplierMapAsync(int userId, OrderFilterViewModel orderFilter = null)
        {
            List<MapViewModel> response = new List<MapViewModel>();
            var helperDomain = new HelperDomain(this);
            try
            {
                var user = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == userId && t.IsActive);
                if (user != null && user.Company != null)
                {
                    var groupIdslist = helperDomain.GetGroupList(orderFilter.GroupIds);
                    IQueryable<Order> allOrders = Context.DataContext.Orders
                                                    .Include(t => t.FuelRequest).Include(t => t.FuelRequest.Job)
                                                    .Include(t => t.FuelRequest.FuelRequest1).Include(t => t.FuelRequest.Job.Users1)
                                                    .Where(t => ((groupIdslist.Count == 0 && t.AcceptedCompanyId == user.Company.Id) ||
                                                                 (groupIdslist.Count > 0 && t.Company.SubCompanies.Any(t1 => t1.SubCompanyId == t.AcceptedCompanyId && groupIdslist.Contains(t1.CompanyGroupId)))) &&
                                                                t.FuelRequest.Job.CountryId == orderFilter.CountryId);

                    var parentOrders = allOrders.Select(t => t.Id);
                    allOrders = allOrders.Where(t => t.ParentId == null || !parentOrders.Contains(t.ParentId ?? 0));
                    allOrders = ApplySupplierOrderFilter(orderFilter, allOrders);

                    var sqlQuery = allOrders.OrderByDescending(t => t.Id).Where(t => t.IsActive);
                    foreach (var item in sqlQuery)
                    {
                        var itemJob = item.FuelRequest.Job;
                        response.Add(new MapViewModel(Status.Success)
                        {
                            OrderId = item.Id,
                            PoNumber = item.PoNumber,
                            Name = itemJob.Name,
                            Address = itemJob.Address == Resource.lblVarious ? "" : itemJob.Address,
                            State = itemJob.MstState.Code,
                            City = itemJob.City == Resource.lblVarious ? "" : itemJob.City,
                            ZipCode = itemJob.ZipCode == Resource.lblVarious ? "" : itemJob.ZipCode,
                            Latitude = itemJob.Latitude,
                            Longitude = itemJob.Longitude,
                            ContactPersons = itemJob.Users1.Select(t1 => new ContactPersonViewModel() { Id = t1.Id, Name = $"{t1.FirstName} {t1.LastName}", Email = t1.Email, PhoneNumber = t1.PhoneNumber }).ToList(),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetSupplierMap", ex.Message, ex);
            }

            return response;
        }

        public async Task<StatusViewModel> CancelOrderAsync(UserContext userContext, CancelOrderViewModel viewModel, bool isChildOrder = false, bool isBuyer = false)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == viewModel.OrderId);
                    if (order != null)
                    {
                        bool cancelAllOrders = isChildOrder;
                        var user = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == viewModel.CanceledBy);

                        if (!isChildOrder)
                        {
                            if (order.IsEndSupplier && order.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest)
                            {
                                order.IsEndSupplier = false;
                                var parentOpenOrderId = GetOpenParentOrder(order);
                                if (parentOpenOrderId != order.Id)
                                {
                                    var parentOrder = Context.DataContext.Orders.FirstOrDefault(t => t.Id == parentOpenOrderId);
                                    parentOrder.IsEndSupplier = true;
                                }

                                var parentCanceledOrderId = GetCanceledParentOrder(order);
                                if (parentCanceledOrderId != order.Id) // means one of the parent order is canceled
                                {
                                    var parentCanceledOrder = Context.DataContext.Orders.FirstOrDefault(t => t.Id == parentCanceledOrderId);
                                    await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetSelectOrderCanceledNewsfeed(userContext, parentCanceledOrder);
                                }
                            }
                            if (order.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId != (int)OrderStatus.Open)
                            {
                                // order is already cancelled
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = Resource.errMessageOrderCancelFailedOrderNotOpen;
                                return response;
                            }

                            if (order.User.Company.Id == user.Company.Id && order.FuelRequest.FuelRequests1.Count > 0)
                            {

                                bool isQualificationMatches = CheckForQualificationMatch(order);
                                if (!isQualificationMatches)
                                {
                                    cancelAllOrders = true;
                                }
                            }
                        }
                        var isOpenBrokerOrder = ContextFactory.Current.GetDomain<HelperDomain>().CheckForOpenBrokerOrder(order);
                        //insert into OrderXCancelationReason table
                        if (order.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId != (int)OrderStatus.Closed || order.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId != (int)OrderStatus.Canceled)
                        {
                            order.OrderXCancelationReason = new OrderXCancelationReason
                            {
                                OrderId = viewModel.OrderId,
                                ReasonId = viewModel.ReasonId,
                                AdditionalNotes = viewModel.Reason,
                                IsAlreadyResubmittedFuel = viewModel.IsFuelRequestReSubmit,
                                CanceledBy = viewModel.CanceledBy
                            };

                            //update order status
                            order.OrderXStatuses.FirstOrDefault(t => t.IsActive).IsActive = false;
                            OrderXStatus orderStatus = new OrderXStatus();
                            if (order.User.Company.Id == user.Company.Id && isOpenBrokerOrder && !cancelAllOrders)
                            {
                                orderStatus.StatusId = (int)OrderStatus.PartiallyCanceled;
                            }
                            else
                            {
                                orderStatus.StatusId = (int)OrderStatus.Canceled;
                            }
                            orderStatus.IsActive = true;
                            orderStatus.UpdatedBy = viewModel.CanceledBy;
                            orderStatus.UpdatedDate = DateTimeOffset.Now;
                            order.OrderXStatuses.Add(orderStatus);

                            order.UpdatedBy = viewModel.CanceledBy;

                            Context.DataContext.Entry(order).State = EntityState.Modified;
                            await Context.CommitAsync();
                            transaction.Commit();

                            //Send response
                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.errMessageOrderCancelSuccess;

                            if (response.StatusCode == Status.Success)
                            {
                                bool isFuelResubmitted = false;
                                if (!isChildOrder)
                                {
                                    if (cancelAllOrders)
                                    {
                                        isFuelResubmitted = await ReSubmitFuelRequestAsync(order, userContext);
                                    }

                                    if (!isFuelResubmitted)
                                    {
                                        isFuelResubmitted = await ReSubmitFuelRequestAsync(order, userContext, true);
                                    }
                                }
                                if (!isFuelResubmitted)
                                {
                                    await ContextFactory.Current.GetDomain<NotificationDomain>()
                                          .AddNotificationEventAsync(EventType.OrderCancelled, order.Id, order.UpdatedBy);
                                }
                                else
                                {
                                    await ContextFactory.Current.GetDomain<NotificationDomain>()
                                          .AddNotificationEventAsync(EventType.OrderCanceledAndFuelRequestResubmitted, order.Id, order.UpdatedBy);
                                }

                                if (!isChildOrder)
                                {
                                    var brokeredFuelRequest = order.FuelRequest.FuelRequests1.OrderByDescending(t => t.Id).FirstOrDefault();
                                    if (brokeredFuelRequest != null && brokeredFuelRequest.GetFuelRequestLastOrder() != null)
                                    {
                                        var brokeredOrder = brokeredFuelRequest.GetFuelRequestLastOrder();
                                        if (brokeredOrder != null && brokeredOrder.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open)
                                        {
                                            // send newsfeed to next open brokered order
                                            var cancellationReason = await Context.DataContext.MstOrderCancelationReasons.SingleOrDefaultAsync(t => t.Id == viewModel.ReasonId);
                                            await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetBrokeredOrderCancelNewsfeed(userContext, order, brokeredOrder, string.IsNullOrWhiteSpace(viewModel.Reason) ? cancellationReason.Name : cancellationReason.Name + Resource.lblSingleHyphen + viewModel.Reason);
                                        }
                                    }
                                    else
                                    {
                                        var cancellationReason = await Context.DataContext.MstOrderCancelationReasons.SingleOrDefaultAsync(t => t.Id == viewModel.ReasonId);
                                        await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetOrderCancelNewsfeed(userContext, order, string.IsNullOrWhiteSpace(viewModel.Reason) ? cancellationReason.Name : cancellationReason.Name + Resource.lblSingleHyphen + viewModel.Reason);
                                    }

                                    if (order.FuelRequest.FuelRequestDetail.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery ||
                                        (order.FuelRequest.FuelRequestDetail.DeliveryTypeId == (int)DeliveryType.MultipleDeliveries &&
                                        GetLatestOrderDeliverySchedule(order.OrderDeliverySchedules).Count() == 0))
                                    {
                                        await ContextFactory.Current.GetDomain<PushNotificationDomain>().PushNotificationOrderCancel(userContext, order, isBuyer);

                                        var assignedDriver = order.OrderXDrivers.SingleOrDefault(t => t.IsActive);
                                        if (assignedDriver != null)
                                        {
                                            if (isBuyer)
                                            {
                                                await ContextFactory.Current.GetDomain<DispatchDomain>().RemoveOnMyWay(assignedDriver.DriverId, order.Id, (int)EnrouteDeliveryStatus.BuyerCanceled, 0);
                                            }
                                            else
                                            {
                                                await ContextFactory.Current.GetDomain<DispatchDomain>().RemoveOnMyWay(assignedDriver.DriverId, order.Id, (int)EnrouteDeliveryStatus.SupplierCanceled, 0);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            transaction.Commit();
                        }
                        if (order.User.Company.Id == user.Company.Id && isOpenBrokerOrder && !cancelAllOrders)
                        {
                            await ContextFactory.Current.GetDomain<NotificationDomain>()
                                  .AddNotificationEventAsync(EventType.OrderUpdated, GetOpenBrokerOrderId(order), order.UpdatedBy);
                        }
                        //cancel orders creted on brokered FR of this order
                        //FuelRequest.FuelRequests1 gives child FR details                        
                        if (order.BuyerCompanyId == user.Company.Id || cancelAllOrders)
                        {
                            if (order.FuelRequest.FuelRequests1.Count > 0)
                            {
                                foreach (var childRequest in order.FuelRequest.FuelRequests1)
                                {
                                    if (childRequest.FuelRequestXStatuses.FirstOrDefault(t => t.IsActive).StatusId == (int)FuelRequestStatus.Open)
                                    {
                                        await ContextFactory.Current.GetDomain<FuelRequestDomain>().CancelFuelRequestAsync(childRequest.Id, userContext);
                                    }

                                    var brokeredOrder = childRequest.GetFuelRequestLastOrder();
                                    if (brokeredOrder != null)
                                    {
                                        //assing new/child FR's order id and userid to viewmodel and create invoice for this
                                        viewModel.OrderId = brokeredOrder.Id;
                                        viewModel.CanceledBy = childRequest.User.Id;

                                        await CancelOrderAsync(userContext, viewModel, true, isBuyer);
                                        await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().DeleteDeliveryRequestOnOrderClose(new List<int> { brokeredOrder.Id }, userContext);
                                    }
                                }
                            }
                        }

                        //Close the ZTR Loop
                        HelperDomain helperDomain = new HelperDomain(this);
                        helperDomain.CloseZTRFuelRequestLoop(order.FuelRequest.Id);
                    }
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageOrderCancelFailed;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("OrderDomain", "CancelOrderAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusViewModel> CloseOrderAsync(UserContext userContext, int orderId, int userId, bool isChildOrder = false)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == orderId);
                    if (order != null)
                    {
                        var user = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == userId);
                        bool closeAllOrders = isChildOrder;
                        if (!isChildOrder)
                        {
                            if (order.IsEndSupplier && (order.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest || order.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.FreightOnlyRequest))
                            {
                                order.IsEndSupplier = false;
                                var parentOpenOrderId = GetOpenParentOrder(order);
                                if (parentOpenOrderId != order.Id)
                                {
                                    var parentOrder = Context.DataContext.Orders.FirstOrDefault(t => t.Id == parentOpenOrderId);
                                    parentOrder.IsEndSupplier = true;
                                }
                            }
                            if (order.OrderXStatuses.FirstOrDefault(t => t.IsActive).StatusId != (int)OrderStatus.Open)
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = Resource.errMessageOrderCloseFailedOrderNotOpen;
                                return response;
                            }
                            if (order.User.Company.Id == user.Company.Id && order.FuelRequest.FuelRequests1.Count > 0)
                            {
                                bool isQualificationMatches = CheckForQualificationMatch(order);
                                if (!isQualificationMatches)
                                {
                                    closeAllOrders = true;
                                }
                            }
                        }
                        var isOpenBrokerOrder = ContextFactory.Current.GetDomain<HelperDomain>().CheckForOpenBrokerOrder(order);
                        //update order status
                        if (order.OrderXStatuses.FirstOrDefault(t => t.IsActive).StatusId != (int)OrderStatus.Closed || order.OrderXStatuses.FirstOrDefault(t => t.IsActive).StatusId != (int)OrderStatus.Canceled)
                        {
                            order.OrderXStatuses.FirstOrDefault(t => t.IsActive).IsActive = false;
                            OrderXStatus orderStatus = new OrderXStatus();
                            if (order.AcceptedCompanyId == user.CompanyId && isOpenBrokerOrder && !closeAllOrders)
                            {
                                orderStatus.StatusId = (int)OrderStatus.PartiallyClosed;
                            }
                            else
                            {
                                orderStatus.StatusId = (int)OrderStatus.Closed;
                            }
                            orderStatus.IsActive = true;
                            orderStatus.UpdatedBy = userId;
                            orderStatus.UpdatedDate = DateTimeOffset.Now;
                            order.OrderXStatuses.Add(orderStatus);

                            order.UpdatedBy = userId;

                            Context.DataContext.Entry(order).State = EntityState.Modified;
                            await Context.CommitAsync();

                            var tankRentalDomain = new TankRentalInvoiceDomain(this);
                            await tankRentalDomain.AddTankRentalInvoiceCreateMessage((int)SystemUser.System, order.Id);

                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.errMessageOrderClosedSuccess;

                            transaction.Commit();

                            if (response.StatusCode == Status.Success)
                            {
                                bool isFuelResubmitted = false;
                                if (!isChildOrder)
                                {
                                    if (closeAllOrders)
                                    {
                                        isFuelResubmitted = await ReSubmitFuelRequestAsync(order, userContext);
                                    }

                                    if (!isFuelResubmitted)
                                    {
                                        isFuelResubmitted = await ReSubmitFuelRequestAsync(order, userContext, true);
                                    }
                                }
                                if (!isFuelResubmitted)
                                {
                                    await ContextFactory.Current.GetDomain<NotificationDomain>()
                                          .AddNotificationEventAsync(EventType.OrderClosed, order.Id, order.UpdatedBy);
                                }
                                else
                                {
                                    await ContextFactory.Current.GetDomain<NotificationDomain>()
                                          .AddNotificationEventAsync(EventType.OrderClosedAndFuelRequestResubmitted, order.Id, order.UpdatedBy);
                                }
                                if (!isChildOrder)
                                {
                                    await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetOrderClosedNewsfeed(userContext, order);
                                }
                            }
                        }
                        else
                        {
                            transaction.Commit();
                        }
                        if (order.User.Company.Id == user.Company.Id && isOpenBrokerOrder && !closeAllOrders)
                        {
                            await ContextFactory.Current.GetDomain<NotificationDomain>()
                                  .AddNotificationEventAsync(EventType.OrderUpdated, GetOpenBrokerOrderId(order), order.UpdatedBy);
                        }

                        //cancel orders creted on brokered FR of this order
                        //FuelRequest.FuelRequests1 gives child FR details                        
                        if (order.BuyerCompanyId == user.Company.Id || closeAllOrders)
                        {
                            if (order.FuelRequest.FuelRequests1.Count > 0)
                            {
                                foreach (var childRequest in order.FuelRequest.FuelRequests1)
                                {
                                    if (childRequest.FuelRequestXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)FuelRequestStatus.Open)
                                    {
                                        await ContextFactory.Current.GetDomain<FuelRequestDomain>().CancelFuelRequestAsync(childRequest.Id, userContext);
                                    }

                                    var brokeredOrder = childRequest.GetFuelRequestLastOrder();
                                    if (brokeredOrder != null)
                                    {
                                        //assing new/child FR's order id and userid to viewmodel and create invoice for this
                                        await CloseOrderAsync(userContext, brokeredOrder.Id, childRequest.User.Id, true);
                                        await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().DeleteDeliveryRequestOnOrderClose(new List<int> { brokeredOrder.Id }, userContext);
                                    }
                                }
                            }
                        }

                        //Close the ZTR Loop
                        //HelperDomain helperDomain = new HelperDomain(this);
                        //helperDomain.CloseZTRFuelRequestLoop(order.FuelRequest.Id);
                    }
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageOrderClosedFailed;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("OrderDomain", "CloseOrderAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<string> GetOrderPoNumber(int orderId)
        {
            string response = string.Empty;
            try
            {
                var order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == orderId);
                if (order != null)
                {
                    return order.PoNumber;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetOrderPoNumber", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> UpdateOrderAdditionalDetailsAsync(OrderAdditionalDetailsViewModel viewModel, bool bolImg, bool dropImg, bool signImg, int orderId, UserContext userContext, OrderAdditionalUpdateType updateType)
        {
            var response = new StatusViewModel();
            try
            {
                var orderAdditionalDetails = await Context.DataContext.OrderAdditionalDetails.FirstOrDefaultAsync(t => t.OrderId == orderId);
                if (orderAdditionalDetails != null)
                {
                    if (updateType == OrderAdditionalUpdateType.SupplierAllowance)
                    {
                        orderAdditionalDetails.Allowance = viewModel.Allowance;
                    }
                    else if (updateType == OrderAdditionalUpdateType.InvoiceNotificationPreference)
                    {
                        orderAdditionalDetails.BOLInvoicePreferenceId = (int)viewModel.BOLInvoicePreferenceTypes;
                    }
                    else if (updateType == OrderAdditionalUpdateType.Other)
                    {
                        var thirdPartyDomain = new ThirdPartyOrderDomain(this);
                        await thirdPartyDomain.SetCarrierAndSourceDetails(viewModel, userContext, orderAdditionalDetails);
                    }
                    orderAdditionalDetails.Order.FuelRequest.FuelRequestDetail.IsBolImageRequired = bolImg;
                    orderAdditionalDetails.Order.FuelRequest.FuelRequestDetail.IsDriverToUpdateBOL = viewModel.IsDriverToUpdateBOL;
                    orderAdditionalDetails.Order.FuelRequest.FuelRequestDetail.IsDropImageRequired = dropImg;
                    orderAdditionalDetails.Order.FuelRequest.Job.SignatureEnabled = signImg; //Update at job level also
                    orderAdditionalDetails.Order.SignatureEnabled = signImg;

                    Context.DataContext.Entry(orderAdditionalDetails).State = EntityState.Modified;

                    await Context.CommitAsync();
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.successMessageSuccessfullyUpdateDriverApplicationDetails;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "UpdateOrderAdditionalDetailsAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<OrderDetailsViewModel> GetDropInformationDetailsAsync(int orderId)
        {
            OrderDetailsViewModel response = new OrderDetailsViewModel();
            List<DropInformationViewModel> dropDetails = new List<DropInformationViewModel>();

            try
            {
                var order = await Context.DataContext.Orders.Include(t => t.FuelRequest.FuelRequestDetail).SingleOrDefaultAsync(t => t.Id == orderId);
                if (order != null)
                {
                    HelperDomain helperDomain = new HelperDomain(this);
                    var allInvoices = Context.DataContext.Invoices.Where(t => t.OrderId == orderId && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active
                                                && t.IsActive).OrderByDescending(t => t.Id).ToList();
                    foreach (var item in allInvoices)
                    {
                        var dropInformation = new DropInformationViewModel();
                        dropInformation.InvoiceId = item.Id;
                        dropInformation.PoNumber = item.PoNumber;
                        if (InvoiceDomain.IsDigitalDropTicket(item.InvoiceTypeId))
                        {
                            dropInformation.InvoiceNumber = Resource.lblHyphen;
                            dropInformation.DDTNumber = item.DisplayInvoiceNumber.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFDD);
                            dropInformation.InvoiceAmount = Resource.lblHyphen;
                            dropInformation.AllowPoEdit = false;
                        }
                        else
                        {
                            dropInformation.DDTNumber = Resource.lblHyphen;
                            if (item.Invoice1 != null)
                            {
                                dropInformation.DDTNumber = item.Invoice1.DisplayInvoiceNumber;
                            }
                            dropInformation.InvoiceNumber = item.DisplayInvoiceNumber;
                            if (item.OrderId != null)
                            {
                                dropInformation.InvoiceAmount = helperDomain.GetInvoiceAmount(item).GetInvoiceAmountValue(2, Resource.constSymbolCurrency);
                            }
                            else
                            {
                                dropInformation.InvoiceAmount = Resource.lblHyphen;
                            }
                            dropInformation.AllowPoEdit = item.InvoiceTypeId != (int)InvoiceType.CreditInvoice && item.InvoiceTypeId != (int)InvoiceType.PartialCredit;
                        }
                        dropInformation.DropDate = item.DropEndDate.ToString(Resource.constFormatDate);
                        dropInformation.Quantity = $"{item.DroppedGallons.GetPreciseValue(2).GetCommaSeperatedValue()} {item.UoM}";
                        dropDetails.Add(dropInformation);
                    }

                    response.FuelRequestId = order.FuelRequestId;
                    response.Id = orderId;
                    response.IsProFormaPo = order.IsProFormaPo;
                    response.BuyerCompanyId = order.BuyerCompanyId;
                    response.DropInformationDetails = dropDetails;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetDropInformationDetailsAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<OrderPoViewModel> GetOrderPoAsync(int orderId, int orderDetailVersionId = 0)
        {
            OrderPoViewModel response = new OrderPoViewModel();
            try
            {
                StoredProcedureDomain storedProcedureDomain = new StoredProcedureDomain(this);
                var order = await storedProcedureDomain.GetSupplierOrderPdfDetailsAsync(orderId, orderDetailVersionId);
                response = new OrderPoViewModel(Status.Success)
                {
                    OrderId = order.OrderId,
                    FuelRequestId = order.FuelRequestId,
                    PoNumber = order.PoNumber,
                    CompanyLogo = new ImageViewModel() { Data = order.Image, FilePath = order.CompanyLogoURL },
                    SupplierCompanyName = order.SupplierCompanyName,
                    SupplierLocation = { Address = order.SupplierAddress, City = order.SupplierCity,
                                                StateCode = order.SupplierStateCode, ZipCode = order.SupplierZipCode, CountryCode = order.SupplierCountryCode },
                    PhoneNumber = order.PhoneNumber,
                    JobName = order.JobName,
                    DisplayJobID = order.DisplayJobID,
                    DateOrdered = order.AcceptedDate.ToString(Resource.constFormatDate),
                    QbTxnDate = order.AcceptedDate,
                    CustomerId = ApplicationConstants.CustomerNumberPrefix + order.BuyerCompanyId.ToString(ApplicationConstants.SevenDigit),
                    PaymentTerm = order.PaymentTermId == (int)PaymentTerms.NetDays ? $"{Resource.lblNetDays} {order.NetDays}" : order.PaymentTerm,
                    BuyerCompanyName = order.BuyerCompanyName,
                    BuyerLocation = {
                                            Address = order.BuyerAddress,
                                            AddressLine2 = order.BuyerAddressLine2,
                                            AddressLine3 = order.BuyerAddressLine3,
                                            City = order.BuyerCity,
                                            StateCode = order.BuyerStateCode,
                                            ZipCode = order.BuyerZipCode,
                                            CountryCode = order.BuyerCountryCode,
                                            StateName = order.BuyerStateName,
                                            CountryName = order.BuyerCountryName,
                                            CountyName = order.BuyerCountyName
                                        },
                    ShippingLocation = { Address = order.JobAddress, AddressLine2 = order.JobAddressLine2, AddressLine3 = order.JobAddressLine3, City = order.JobCity, StateCode = order.JobStateCode, ZipCode = order.JobZipCode, CountryCode = order.JobCountryCode },
                    PoContact = new ContactPersonViewModel() { Name = order.PoContactName, Email = order.PoContactEmail, PhoneNumber = order.PoContactNumber, CompanyId = order.PoContactCompanyId, CompanyName = order.PoContactCompanyName },
                    OrderType = order.OrderType,
                    FuelName = order.FuelName,
                    DeliveryDetails = new FuelDeliveryDetailsViewModel() { StartDate = order.StartDate, EndDate = order.EndDate, StartTime = Convert.ToDateTime(order.StartTime.ToString()).ToShortTimeString(), EndTime = Convert.ToDateTime(order.EndTime.ToString()).ToShortTimeString() },
                    PricePerGallon = order.PricePerGallon,
                    GallonsOrdered = order.GallonsOrdered,
                    QuantityTypeId = order.QuantityTypeId,
                    TypeOfFuel = order.ProductDisplayGroupId,
                    PrductDescription = order.FuelDescription,
                    //OrderTotalAmount = order.OrderAmount.HasValue ? order.OrderAmount.Value.ToString(ApplicationConstants.DecimalFormat2) : Resource.lblHyphen,
                    SpecialInstructions = await GetFuelRequestSpecialInstructions(order.FuelRequestId),
                    IsAssetTrackingEnabled = order.IsAssetTracked,
                    Currency = order.Currency,
                    UoM = order.UoM,
                    IsActive = orderDetailVersionId == 0,
                    CreationTimeRackPPG = order.CreationTimeRackPPG,
                    RequestPriceDetailId = order.RequestPriceDetailId,
                    BillToAddress = order.BillToAddress,
                    BillToAddressLine2 = order.BillToAddressLine2,
                    BillToAddressLine3 = order.BillToAddressLine3,
                    BillToCity = order.BillToCity,
                    BillToCountryCode = order.BillToCountryCode,
                    BillToCountryId = order.BillToCountryId,
                    BillToName = order.BillToName,
                    BillToStateCode = order.BillToStateCode,
                    BillToStateId = order.BillToStateId,
                    BillToZipCode = order.BillToZipCode,
                    BillToCounty = order.BillToCounty,
                    BillToStateName = order.BillToStateName,
                    BillToCountryName = order.BillToCountryName,
                    IsBillToEnabled = order.IsBillToEnabled
                };
                if (!string.IsNullOrWhiteSpace(order.CustomAttribute))
                {
                    response.DeliveryDetails.CustomAttributeViewModel = JsonConvert.DeserializeObject<FuelRequestCustomAttributeViewModel>(order.CustomAttribute);
                }
                response.InvoiceDetails = await GetOrderDropDetails(orderId);
                var fees = await GetFuelRequestFeeDetailsAsync(order.FuelRequestId);
                response.FuelFees.FuelRequestFees = fees.ToFeesViewModel();

                decimal qbRequestPricePerGallon = 0;
                // get pricing details from pricing service
                PricingRequestDetailResponseViewModel pricingDetails;
                if (order.RequestPriceDetailId > 0)
                {
                    pricingDetails = await GetRequestPricingDetail(order.RequestPriceDetailId, (int)order.Currency, order.AcceptedCompanyId, order.FuelTypeId, order.StateId);
                    if (pricingDetails != null)
                    {
                        HelperDomain helperDomain = new HelperDomain(storedProcedureDomain);
                        qbRequestPricePerGallon = helperDomain.GetPricePerGallonForQb(pricingDetails.PricePerGallon, pricingDetails.PricingTypeId, order.CreationTimeRackPPG, pricingDetails.SupplierCost ?? 0, pricingDetails.RackAvgTypeId ?? 0);
                        var orderTotalAmount = helperDomain.GetOrderTotalAmount(pricingDetails.PricingTypeId, order.QuantityTypeId, order.GallonsOrdered, pricingDetails.PricePerGallon);
                        response.OrderTotalAmount = orderTotalAmount.HasValue ? orderTotalAmount.Value.GetPreciseValue(2).GetCommaSeperatedValue() : Resource.lblHyphen;
                    }
                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errorMessagePricingRequestDetailsNotAvailable;
                }

                response.QbRate = qbRequestPricePerGallon;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetOrderPoAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<OrderPoViewModel> GetProformaBDNPdfAsync(int orderId, int orderDetailVersionId = 0)
        {
            OrderPoViewModel response = new OrderPoViewModel();
            try
            {
                StoredProcedureDomain storedProcedureDomain = new StoredProcedureDomain(this);
                var order = await storedProcedureDomain.GetProformaBDNPdfAsync(orderId, orderDetailVersionId);
                response = new OrderPoViewModel(Status.Success)
                {
                    OrderId = order.OrderId,
                    FuelRequestId = order.FuelRequestId,
                    PoNumber = order.PoNumber,
                    CompanyLogo = new ImageViewModel() { Data = order.Image, FilePath = order.CompanyLogoURL },
                    SupplierCompanyName = order.SupplierCompanyName,
                    SupplierLocation = { Address = order.SupplierAddress,AddressLine2 = order.SupplierAddressLine2,AddressLine3 = order.SupplierAddressLine3, City = order.SupplierCity,
                                                StateCode = order.SupplierStateCode, ZipCode = order.SupplierZipCode, CountryCode = order.SupplierCountryCode },
                    PhoneNumber = order.PhoneNumber,
                    JobName = order.JobName,
                    DisplayJobID = order.DisplayJobID,
                    DateOrdered = order.AcceptedDate.ToString(Resource.constFormatDate),
                    QbTxnDate = order.AcceptedDate,
                    CustomerId = ApplicationConstants.CustomerNumberPrefix + order.BuyerCompanyId.ToString(ApplicationConstants.SevenDigit),
                    PaymentTerm = order.PaymentTermId == (int)PaymentTerms.NetDays ? $"{Resource.lblNetDays} {order.NetDays}" : order.PaymentTerm,
                    BuyerCompanyName = order.BuyerCompanyName,
                    BuyerLocation = {
                                            Address = order.BuyerAddress,
                                            AddressLine2 = order.BuyerAddressLine2,
                                            AddressLine3 = order.BuyerAddressLine3,
                                            City = order.BuyerCity,
                                            StateCode = order.BuyerStateCode,
                                            ZipCode = order.BuyerZipCode,
                                            CountryCode = order.BuyerCountryCode,
                                            StateName = order.BuyerStateName,
                                            CountryName = order.BuyerCountryName,
                                            CountyName = order.BuyerCountyName
                                        },
                    ShippingLocation = { Address = order.JobAddress, AddressLine2 = order.JobAddressLine2, AddressLine3 = order.JobAddressLine3, City = order.JobCity, StateCode = order.JobStateCode, ZipCode = order.JobZipCode, CountryCode = order.JobCountryCode },
                    PoContact = new ContactPersonViewModel() { Name = order.PoContactName, Email = order.PoContactEmail, PhoneNumber = order.PoContactNumber, CompanyId = order.PoContactCompanyId, CompanyName = order.PoContactCompanyName },
                    OrderType = order.OrderType,
                    FuelName = order.FuelName,
                    DeliveryDetails = new FuelDeliveryDetailsViewModel() { StartDate = order.StartDate, EndDate = order.EndDate, StartTime = Convert.ToDateTime(order.StartTime.ToString()).ToShortTimeString(), EndTime = Convert.ToDateTime(order.EndTime.ToString()).ToShortTimeString() },
                    PricePerGallon = order.PricePerGallon,
                    GallonsOrdered = order.GallonsOrdered,
                    QuantityTypeId = order.QuantityTypeId,
                    TypeOfFuel = order.ProductDisplayGroupId,
                    PrductDescription = order.FuelDescription,
                    //OrderTotalAmount = order.OrderAmount.HasValue ? order.OrderAmount.Value.ToString(ApplicationConstants.DecimalFormat2) : Resource.lblHyphen,
                    SpecialInstructions = await GetFuelRequestSpecialInstructions(order.FuelRequestId),
                    IsAssetTrackingEnabled = order.IsAssetTracked,
                    Currency = order.Currency,
                    UoM = order.UoM,
                    IsActive = orderDetailVersionId == 0,
                    CreationTimeRackPPG = order.CreationTimeRackPPG,
                    RequestPriceDetailId = order.RequestPriceDetailId,
                    BillToAddress = order.BillToAddress,
                    BillToAddressLine2 = order.BillToAddressLine2,
                    BillToAddressLine3 = order.BillToAddressLine3,
                    BillToCity = order.BillToCity,
                    BillToCountryCode = order.BillToCountryCode,
                    BillToCountryId = order.BillToCountryId,
                    BillToName = order.BillToName,
                    BillToStateCode = order.BillToStateCode,
                    BillToStateId = order.BillToStateId,
                    BillToZipCode = order.BillToZipCode,
                    BillToCounty = order.BillToCounty,
                    BillToStateName = order.BillToStateName,
                    BillToCountryName = order.BillToCountryName,
                    IsBillToEnabled = order.IsBillToEnabled,
                    AcceptedCompanyId = order.AcceptedCompanyId,
                    PdfFooterJson = order.PdfFooterJson,
                    Berth = order.Berth,
                    IMONumber = order.IMONumber,
                    Vessel = order.Vessel,
                    IsShowProductDescriptionOnInvoice = order.IsShowProductDescriptionOnInvoice,
                    SuperAdminProductDescription = order.SuperAdminProductDescription,
                    BDRNumber = order.BDRNumber
                };
                if (!string.IsNullOrWhiteSpace(order.CustomAttribute))
                {
                    response.DeliveryDetails.CustomAttributeViewModel = JsonConvert.DeserializeObject<FuelRequestCustomAttributeViewModel>(order.CustomAttribute);
                }

                response.InvoiceDetails = await GetOrderDropDetails(orderId);

                // get Pdf footer details
                if (!string.IsNullOrEmpty(response.PdfFooterJson))
                {
                    var pdfFooterDetailList = JsonConvert.DeserializeObject<InvoicePdfFooterViewModel>(response.PdfFooterJson);
                    if (pdfFooterDetailList != null)
                    {
                        var pdfFooterDetail = pdfFooterDetailList.InvoicePdfFooterList.FirstOrDefault(t => t.CompanyId == response.AcceptedCompanyId);
                        if (pdfFooterDetail != null)
                        {
                            response.PdfFooter = new PdfFooterModel()
                            {
                                CompanyId = pdfFooterDetail.CompanyId,
                                Description = pdfFooterDetail.Description,
                                BankingInstructions = pdfFooterDetail.BankingInstructions,
                                AdditionalDetails = pdfFooterDetail.AdditionalDetails,
                                CompanyName = pdfFooterDetail.CompanyName
                            };

                            var appDomain = new ApplicationDomain();
                            var siteFuelExchangeUrl = appDomain.GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingSiteFuelExchangeUrl);
                            response.PdfFooter.QRCodePath = siteFuelExchangeUrl + "/Content/images/QRCodeTropic.png";
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDomain", "GetProformaBDNPdfAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<OrderDropDetailsViewModel>> GetOrderDropDetails(int orderId)
        {
            List<OrderDropDetailsViewModel> response = null;
            StoredProcedureDomain spDomain = new StoredProcedureDomain(this);
            try
            {
                response = await spDomain.GetOrderDropDetails(orderId);
                response.ForEach(t => t.Amount = t.InvoiceAmount.HasValue ? $"{@Resource.constSymbolCurrency}{t.InvoiceAmount.Value.ToString(ApplicationConstants.DecimalFormat4)}" : Resource.lblHyphen);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetOrderDropDetails", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<DeliveryGridViewModel>> GetDeliveryGridDataAsync(int companyId, int driverId, string startDate, string endDate)
        {
            List<DeliveryGridViewModel> response = new List<DeliveryGridViewModel>();
            try
            {
                List<Invoice> invoices;
                DateTimeOffset StartDate = DateTimeOffset.Now.Date.AddDays(ApplicationConstants.DateFilterDefaultDays);
                DateTimeOffset EndDate = DateTimeOffset.Now.Date.AddDays(1);
                if (!string.IsNullOrEmpty(startDate))
                {
                    StartDate = Convert.ToDateTime(startDate).Date;
                }
                if (!string.IsNullOrEmpty(endDate))
                {
                    EndDate = Convert.ToDateTime(endDate).Date.AddDays(1);
                }

                if (driverId > 0)
                {
                    invoices = await Context.DataContext.InvoiceXAdditionalDetails
                        .Include(t => t.Invoice.Order).Include(t => t.Invoice.Order.FuelRequest)
                        .Where(t => t.Invoice.IsActive && t.Invoice.ParentId == null && t.Invoice.OrderId != null &&
                        t.Invoice.Order.Company.Id == companyId && t.Invoice.DriverId == driverId &&
                        t.Invoice.DropStartDate >= StartDate && t.Invoice.DropEndDate < EndDate).Select(t => t.Invoice).ToListAsync();
                }
                else
                {
                    invoices = await Context.DataContext.InvoiceXAdditionalDetails
                        .Include(t => t.Invoice.Order).Include(t => t.Invoice.Order.FuelRequest)
                        .Where(t => t.Invoice.IsActive && t.Invoice.ParentId == null && t.Invoice.OrderId != null &&
                        t.Invoice.Order.Company.Id == companyId &&
                        t.Invoice.DropStartDate >= StartDate && t.Invoice.DropEndDate < EndDate).Select(t => t.Invoice).ToListAsync();
                }

                foreach (var item in invoices)
                {
                    var delivery = new DeliveryGridViewModel(Status.Success);
                    delivery.InvoiceId = item.Id;
                    delivery.OrderId = item.OrderId ?? 0;
                    delivery.Invoice = item.DisplayInvoiceNumber;
                    delivery.Po = item.PoNumber;
                    delivery.DropDateTime = item.DropEndDate.DateTime.ToString();
                    delivery.AmountDropped = item.DroppedGallons;
                    delivery.Quantity = (item.Order.BrokeredMaxQuantity ?? item.Order.FuelRequest.MaxQuantity).GetCommaSeperatedValue();
                    delivery.Overage = item.Order.Invoices.Sum(t1 => t1.DroppedGallons) - (item.Order.BrokeredMaxQuantity ?? item.Order.FuelRequest.MaxQuantity);
                    delivery.DriverName = item.Driver == null ? Resource.lblHyphen : $"{item.Driver.FirstName} {item.Driver.LastName}";
                    delivery.Location = $"{item.Order.FuelRequest.Job.Address}, {item.Order.FuelRequest.Job.City}, {item.Order.FuelRequest.Job.MstState.Code} {item.Order.FuelRequest.Job.ZipCode}";
                    response.Add(delivery);
                }
                response.ForEach(t => t.OverageAmount = t.Overage > 0 ? t.Overage.GetCommaSeperatedValue() : Resource.lblHyphen);
                response = response.OrderByDescending(t => t.InvoiceId).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetDeliveryGridDataAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<DeliveryDetailsViewModel>> GetAllJobLocationsByUser(int userId, int countryId = (int)Country.All)
        {
            var response = new List<DeliveryDetailsViewModel>();
            try
            {
                var helperDomain = new HelperDomain(this);
                var jobIds = await helperDomain.GetJobIdsAsync(userId);
                if (jobIds != null)
                {
                    var jobs = Context.DataContext.Jobs.Where(t => t.IsActive &&
                                                            t.JobXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)JobStatus.Open
                                                            && (countryId == (int)Country.All || countryId == t.CountryId)
                                                            && jobIds.Contains(t.Id))
                                                            .ToList();

                    response = jobs.Where(t => t.EndDate == null || t.EndDate.Value.Date >= DateTimeOffset.Now.ToTargetDateTimeOffset(t.TimeZoneName).Date)
                                    .Select(t => new DeliveryDetailsViewModel { JobAddress = t.Address + ", " + t.City + ", " + t.MstState.Code + " " + t.ZipCode, JobLatitude = t.Latitude, JobLongitude = t.Longitude })
                                    .ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetAllJobLocationsByUser", ex.Message, ex);
            }

            return response;
        }

        public async Task<StatusViewModel> UpdateInvoiceNotesAsync(int orderId, string notes, UserContext userContext)
        {
            var response = new StatusViewModel();
            try
            {
                if (string.IsNullOrWhiteSpace(notes))
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.valMessageEnterInvoiceNotes;
                    return response;
                }
                else if (notes.Length > Constants.InvoiceNotesDefaultLength)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.valMessageInvalidInvoiceNotesLength;
                    return response;
                }

                var order = await Context.DataContext.Orders.FirstOrDefaultAsync(t => t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open && t.Id == orderId);
                if (order == null)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageCannotUpdateInvoiceNotesClosedOrders;
                    return response;
                }

                if (order.OrderAdditionalDetail == null)
                {
                    order.OrderAdditionalDetail = new OrderAdditionalDetail();
                }

                order.OrderAdditionalDetail.Notes = notes;
                Context.DataContext.Entry(order).State = EntityState.Modified;
                await Context.CommitAsync();

                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.successMessageSuccessfullyUpdateInvoiceNotes;
            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = Resource.errMessageUpdateInvoiceNotes;
                LogManager.Logger.WriteException("OrderDomain", "UpdateInvoiceNotesAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> SaveSpecialInstructionFilesAsync(int orderId, UserContext userContext, List<AttachmentViewModel> files)
        {
            StatusViewModel response = new StatusViewModel();
            try
            {
                if (orderId <= 0)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageOrderNotFound;
                    return response;
                }

                if (files == null || files.Count <= 0)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.btnLabelSelectFile;
                    return response;
                }

                var hasInvalidFile = files.Any(t => !VaidateSpecialInstructionFile(t.Name));
                if (hasInvalidFile)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageInValidFile;
                    return response;
                }

                var order = await Context.DataContext.Orders.FirstOrDefaultAsync(t => t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open && t.Id == orderId);
                if (order != null)
                {
                    // validation for max 5 file upload
                    List<AttachmentViewModel> existingFiles = new List<AttachmentViewModel>();
                    if (order.OrderAdditionalDetail != null && !string.IsNullOrWhiteSpace(order.OrderAdditionalDetail.FileDetails))
                    {
                        existingFiles = JsonConvert.DeserializeObject<List<AttachmentViewModel>>(order.OrderAdditionalDetail.FileDetails);
                        if ((existingFiles.Count + files.Count) > ApplicationConstants.SpecialInstructionsDefaultFileUploadCount)
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = string.Format(Resource.errMessageMaxFilesUploadAllowed, ApplicationConstants.SpecialInstructionsDefaultFileUploadCount);
                            return response;
                        }
                    }

                    await SaveSpecialInstructionFilesToBlob(orderId, files);

                    if (order.OrderAdditionalDetail == null)
                    {
                        order.OrderAdditionalDetail = new OrderAdditionalDetail();
                    }

                    // filter existing files and remove duplicates files
                    if (!string.IsNullOrWhiteSpace(order.OrderAdditionalDetail.FileDetails))
                    {
                        files.AddRange(existingFiles);
                        files = files.GroupBy(t => t.Url).Select(t => t.First()).ToList();
                    }

                    // create json for file paths/url
                    var fileBlobDetail = new List<FileDetailViewModel>();
                    foreach (var file in files)
                    {
                        FileDetailViewModel fileDtl = new FileDetailViewModel();
                        fileDtl.Container = BlobContainerType.SpecialInstructions.ToString().ToLower();
                        fileDtl.FileType = FileType.SpecialInstruction.ToString();
                        fileDtl.Url = file.Url;

                        fileBlobDetail.Add(fileDtl);
                    }
                    var fileJson = JsonConvert.SerializeObject(fileBlobDetail);

                    // save file upload json details
                    order.OrderAdditionalDetail.FileDetails = fileJson;
                    Context.DataContext.Entry(order).State = EntityState.Modified;
                    await Context.CommitAsync();

                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.successMsgSpecialInstructionFileUpload;
                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageCanNotUpdateClosedOrder;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = Resource.gridColumnFailed;
                LogManager.Logger.WriteException("OrderDomain", "SaveSpecialInstructionFilesAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<StatusViewModel> DeleteSpecialInstructionDocument(UserContext userContext, int orderId, string fileName)
        {
            StatusViewModel response = new StatusViewModel();
            try
            {
                var order = await Context.DataContext.Orders.FirstOrDefaultAsync(t => t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open && t.Id == orderId);
                if (order != null)
                {
                    List<AttachmentViewModel> existingFiles;
                    if (order.OrderAdditionalDetail != null && !string.IsNullOrWhiteSpace(order.OrderAdditionalDetail.FileDetails))
                    {
                        existingFiles = JsonConvert.DeserializeObject<List<AttachmentViewModel>>(order.OrderAdditionalDetail.FileDetails);
                        existingFiles = existingFiles.Where(t => t.Url != fileName).ToList();

                        var fileJson = JsonConvert.SerializeObject(existingFiles);

                        // save file upload json details
                        order.OrderAdditionalDetail.FileDetails = fileJson;
                        Context.DataContext.Entry(order).State = EntityState.Modified;
                        await Context.CommitAsync();

                        var azureBlob = new Core.Infrastructure.AzureBlobStorage();
                        await azureBlob.DeleteBlobAsync(fileName, BlobContainerType.SpecialInstructions.ToString().ToLower());

                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.successMsgFileDelete;
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = Resource.gridColumnFailed;
                LogManager.Logger.WriteException("OrderDomain", "DeleteSpecialInstructionDocument", ex.Message, ex);
            }

            return response;
        }

        private bool VaidateSpecialInstructionFile(string fileName)
        {
            string ext = System.IO.Path.GetExtension(fileName);
            switch (ext.ToLower())
            {
                case ".bmp":
                    return true;
                case ".jpg":
                    return true;
                case ".jpeg":
                    return true;
                case ".png":
                    return true;
                case ".pdf":
                    return true;
                case ".doc":
                    return true;
                case ".docx":
                    return true;
                default:
                    return false;
            }
        }

        private async Task SaveSpecialInstructionFilesToBlob(int orderId, List<AttachmentViewModel> files)
        {
            try
            {
                foreach (var file in files)
                {
                    var azureBlob = new Core.Infrastructure.AzureBlobStorage();

                    // replace multiple occurance of .(dot) and replace with hypen
                    var name = file.Name.Replace(".", "-");
                    int place = name.LastIndexOf("-");
                    // replace last index of hypen to .(dot)
                    name = name.Remove(place, "-".Length).Insert(place, ".");
                    string[] fileNameArr = name.Split('.');
                    var fileName = fileNameArr[0].Trim().Replace(" ", "");
                    var extension = fileNameArr[1].Trim();

                    file.Url = await azureBlob.SaveBlobAsync(file.FileStream, GetSpecialInstructionFileName(orderId, fileName, extension), BlobContainerType.SpecialInstructions.ToString().ToLower());
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "SaveSpecialInstructionFilesToBlob", $"OrderId : {orderId}. " + ex.Message, ex);
            }
        }

        private string GetSpecialInstructionFileName(int orderId, string fileName, string extension)
        {
            string name = "";
            try
            {
                name = string.Concat(values: fileName + Resource.lblSingleHyphen + orderId + "." + extension);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetSpecialInstructionFileName", ex.Message, ex);
            }

            return name;
        }

        private SpecialInstructionAttachmentViewModel GetSpecialInstructionFileDetails(int orderId, string jsonFileDetails)
        {
            SpecialInstructionAttachmentViewModel obj = new SpecialInstructionAttachmentViewModel();
            try
            {
                obj.Id = orderId;
                if (!string.IsNullOrWhiteSpace(jsonFileDetails))
                {
                    obj.Files = JsonConvert.DeserializeObject<List<AttachmentViewModel>>(jsonFileDetails);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetSpecialInstructionFileDetails", ex.Message, ex);
            }

            return obj;
        }

        //UPDATE BROKERED ORDERS
        public async Task UpdateBrokeredFreightOrderQuantity(List<OrderDropDetailsViewModel> orderWithDropQuantity)
        {
            try
            {
                if (orderWithDropQuantity.Any())
                {
                    var droppedOrderIds = orderWithDropQuantity.Select(o => o.OrderId).ToList();

                    var droppedOrders = await Context.DataContext.Orders.Where(
                                   t => droppedOrderIds.Contains(t.Id) &&
                                   t.FuelRequest.FuelRequestTypeId == (int)FuelRequestType.FreightOnlyRequest)
                                   .Select(t => new
                                   {
                                       OrderId = t.Id,
                                       JobId = t.FuelRequest.Job.Id,
                                       ParentOrders = t.FuelRequest.FuelRequest1.Orders.Select(t2 => t2.Id),
                                       SupplierCompanyId = t.BuyerCompanyId
                                   }).ToListAsync();

                    if (droppedOrders.Any())
                    {
                        FreightServiceDomain freightServiceDomain = new FreightServiceDomain(this);
                        StoredProcedureDomain spDomain = new StoredProcedureDomain();
                        List<CarrierOrderDetails> carrierCompanies = new List<CarrierOrderDetails>();
                        Dictionary<int, decimal> carrierOrders = new Dictionary<int, decimal>();
                        foreach (var order in droppedOrders)
                        {
                            int carrierCompanyId = await freightServiceDomain.GetAssignedCarrierCompanyId(order.SupplierCompanyId, order.JobId);
                            carrierCompanies.Add(new CarrierOrderDetails() { SupplierOrderId = order.ParentOrders.LastOrDefault(), CarrierOrderId = order.OrderId, CarrierCompanyId = carrierCompanyId });
                        }

                        foreach (var carrierCompany in carrierCompanies)
                        {
                            var childOrders = await spDomain.GetBrokeredChildOrders(carrierCompany.SupplierOrderId, (int)OrderStatus.Open, carrierCompany.CarrierCompanyId);
                            if (childOrders != null && childOrders.Any())
                            {
                                var carrierOrder = childOrders.Select(t => t.OrderId).OrderBy(t => t).FirstOrDefault();
                                var droppedQuantity = orderWithDropQuantity.Where(t => t.OrderId == carrierCompany.CarrierOrderId).Select(t => t.Quantity).FirstOrDefault();
                                carrierOrders.Add(carrierOrder, droppedQuantity);
                            }
                        }

                        var parentOrderIds = carrierOrders.Select(o => o.Key).ToList();
                        parentOrderIds = parentOrderIds.Except(droppedOrderIds).ToList();
                        var parentOrders = Context.DataContext.Orders.Where(t =>
                                       t.FuelRequest.QuantityTypeId != (int)QuantityType.NotSpecified &&
                                       parentOrderIds.Contains(t.Id)).Select(t => new { t.Id, t.FuelRequest }).ToList();

                        if (parentOrders.Any())
                        {
                            foreach (var parentOrder in parentOrders)
                            {
                                var dropQuantity = carrierOrders.Where(o1 => o1.Key == parentOrder.Id).Select(t => t.Value).FirstOrDefault();
                                if (dropQuantity > 0)
                                {
                                    if (parentOrder.FuelRequest.MaxQuantity - dropQuantity > 0)
                                    {
                                        parentOrder.FuelRequest.MaxQuantity = parentOrder.FuelRequest.MaxQuantity - dropQuantity;
                                    }
                                    else
                                    {
                                        parentOrder.FuelRequest.MaxQuantity = 0;
                                    }
                                    //Context.DataContext.Entry(parentOrder).State = EntityState.Modified;
                                }
                            }

                            await Context.CommitAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "UpdateBrokeredFreightOrderQuantity", $"orderWithDropQuantity : {string.Join(", ", orderWithDropQuantity.Select(x => " Order Id: " + x.OrderId.ToString() + ", Drop Quantity: " + x.Quantity.ToString()))}" + ex.Message, ex);
            }
        }
        //---------------------------------Mobile API------------------------------------------

        public async Task<List<DriverOrderAssetViewModel>> GetOrderAssetsAsync(List<int> orderId, int driverId = 0)
        {
            var response = new List<DriverOrderAssetViewModel>();
            try
            {
                var order = await Context.DataContext.Orders
                                .Select(x => new { x.Id, x.FuelRequest.JobId, x.FuelRequest.Job.IsMarine, x.BuyerCompanyId })
                                .Where(t => orderId.Contains(t.Id)).ToListAsync();
                if (order != null)
                {
                    //  if (order.IsMarine)
                    //  {
                    // var isAssetAssigned = await AssignAssetsForMarine(orderId, order.JobId, order.BuyerCompanyId);
                    //  if (isAssetAssigned.StatusCode == Status.Success)
                    // {
                    // SetAssetDetailsViewModel(orderId, response, order.JobId);
                    // }
                    //   }
                    //  else
                    // {
                    var isMarine = order.Select(t => t.IsMarine).Distinct().FirstOrDefault();
                    if (!isMarine)
                        SetAssetDetailsViewModel(orderId, response, order.Select(t => t.JobId).Distinct().ToList(), driverId);
                    else
                        SetAssetDetailsForMarineJobViewModel(orderId, response, order.Select(t => t.JobId).Distinct().ToList(), driverId);
                    // }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetOrderAssetsAsync", ex.Message, ex);
            }
            return response;
        }

        private async Task<StatusViewModel> AssignAssetsForMarine(int orderId, int jobId, int buyerCompanyId)
        {
            var response = new StatusViewModel();
            try
            {
                var assetIds = await Context.DataContext.Assets
                                                     .Where(t1 => t1.CompanyId == buyerCompanyId && t1.IsActive && t1.IsMarine)
                                                     .Select(x => x.Id).ToListAsync();

                var assignedAssets = await Context.DataContext.JobXAssets.Where(t => assetIds.Contains(t.AssetId)
                                                                        && t.RemovedBy == null
                                                                        && t.RemovedDate == null)
                                                                        .Select(t => new { t.JobId, t.AssetId })
                                                                        .ToListAsync();

                var allassignedAssetsIds = assignedAssets.Select(t => t.AssetId).ToList();
                var isAllAssetsAssignedToSameJob = assetIds.All(t => allassignedAssetsIds.Contains(t)) && assignedAssets.All(t => t.JobId == jobId);
                if (isAllAssetsAssignedToSameJob)
                {
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Status.Success.ToString();
                }
                else if (jobId > 0 && assetIds.Any())
                {
                    var assetDomain = new AssetDomain(this);
                    response = await assetDomain.AssignAssetsToJobAsync(new UserContext(), jobId, assetIds, true);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "AssignAssetsForMarine", ex.Message, ex);
                throw;
            }
            return response;
        }

        private void SetAssetDetailsViewModel(List<int> orderId, List<DriverOrderAssetViewModel> response, List<int> jobidForOrder, int driverId = 0)
        {
            var jobXAssets = Context.DataContext.JobXAssets.Where(t => jobidForOrder.Contains(t.JobId) && ((t.OrderId == null && !t.Job.IsMarine)) &&
                                                        t.RemovedBy == null && t.RemovedDate == null).
                                                        Select(x => new
                                                        {
                                                            x.Id,
                                                            FuelTypeName = x.Asset.MstProductType == null ? "" : x.Asset.MstProductType.Name,
                                                            x.AssetId,
                                                            AssetName = x.Asset.Name,
                                                            AssetDrops = x.AssetDrops.Where(t => t.InvoiceId == null && orderId.Contains(t.OrderId)).OrderBy(t => t.DropEndDate),
                                                            Spills = x.Asset.Spills.Where(t => t.InvoiceId == null && orderId.Contains(t.OrderId)).OrderByDescending(t => t.Id).FirstOrDefault(),
                                                            x.Asset.Type
                                                        }).ToList();

            foreach (var jobXAsset in jobXAssets.Where(t => t.Type == (int)AssetType.Asset || t.Type == (int)AssetType.Vessle))
            {
                var assetDrops = driverId == 0 ? jobXAsset.AssetDrops.ToList() : jobXAsset.AssetDrops.Where(t => t.DroppedBy == driverId).ToList();
                var driverOrderAsset = GetDriverOrderAssetAsync(jobXAsset, assetDrops, orderId);
                if (driverOrderAsset.StatusCode == Status.Success)
                {
                    //driverOrderAsset.FuelDrop.OrderId = orderId.FirstOrDefault();
                    response.Add(driverOrderAsset);
                }
            }
        }

        private void SetAssetDetailsForMarineJobViewModel(List<int> orderId, List<DriverOrderAssetViewModel> response, List<int> jobidForOrder, int driverId = 0)
        {
            var assets = Context.DataContext.JobXAssets.Where(t => jobidForOrder.Contains(t.JobId) && (t.Job.IsMarine && orderId.Contains(t.OrderId.Value)) &&
                                                        t.RemovedBy == null && t.RemovedDate == null).
                                                        Select(x => new
                                                        {
                                                            x.AssetId,
                                                            x.Asset.Type,
                                                            FuelTypeName = x.Asset.MstProductType == null ? "" : x.Asset.MstProductType.Name,
                                                            AssetName = x.Asset.Name,
                                                        }).Distinct().ToList();

            foreach (var asset in assets.Where(t => t.Type == (int)AssetType.Asset || t.Type == (int)AssetType.Vessle))
            {
                var driverOrderAsset = GetDriverOrderAssetForMarineAsync(asset.AssetId, asset.FuelTypeName, asset.AssetName, orderId, driverId);
                if (driverOrderAsset.StatusCode == Status.Success)
                {
                    //driverOrderAsset.FuelDrop.OrderId = orderId.FirstOrDefault();
                    response.Add(driverOrderAsset);
                }
            }
        }

        public async Task<StatusViewModel> AddFuelSpillAsync(DriverFuelSpillViewModel viewModel)
        {
            var response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var spill = await Context.DataContext.Spills.FirstOrDefaultAsync(t => t.Id == viewModel.Id);
                    if (spill == null)
                    {
                        spill = viewModel.ToEntity();
                        Context.DataContext.Spills.Add(spill);
                    }
                    else
                    {
                        Context.DataContext.Images.RemoveRange(spill.Images);

                        spill = viewModel.ToEntity(spill);
                        Context.DataContext.Entry(spill).State = EntityState.Modified;
                    }

                    await Context.CommitAsync();
                    transaction.Commit();

                    viewModel = spill.ToViewModel(viewModel);
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageSuccess;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("OrderDomain", "AddFuelSpillAsync", ex.Message, ex);
                }
            }

            return response;
        }

        public async Task<DriverFuelSpillViewModel> GetFuelSpillAsync(int assetId, int spillId)
        {
            var response = new DriverFuelSpillViewModel(Status.Success);
            try
            {
                var spill = await Context.DataContext.Spills.FirstOrDefaultAsync(t => t.Id == spillId && t.AssetId == assetId);
                if (spill != null)
                {
                    response = spill.ToViewModel(response);
                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageFailed;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetFuelSpillAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<StatusViewModel> DeleteFuelSpill(int spillId)
        {
            var response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var spill = await Context.DataContext.Spills.FirstOrDefaultAsync(t => t.Id == spillId);
                    if (spill != null)
                    {
                        Context.DataContext.Images.RemoveRange(spill.Images);
                        Context.DataContext.Spills.Remove(spill);

                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMessageSuccess;
                    }

                    await Context.CommitAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("OrderDomain", "DeleteFuelSpill", ex.Message, ex);
                }
            }

            return response;
        }

        public async Task<StatusViewModel> DeleteFuelSpillImage(int spillId, int imageId)
        {
            var response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var spill = await Context.DataContext.Spills.FirstOrDefaultAsync(t => t.Id == spillId);
                    if (spill != null)
                    {
                        var image = spill.Images.FirstOrDefault(t => t.Id == imageId);
                        if (image != null)
                        {
                            Context.DataContext.Images.Remove(image);
                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.errMessageSuccess;
                        }
                    }

                    await Context.CommitAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("OrderDomain", "DeleteFuelSpillImage", ex.Message, ex);
                }
            }

            return response;
        }

        public async Task<StatusViewModel> DeleteAssetDrop(DeleteAssetDropViewModel viewModel)
        {
            var response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (var assetDropId in viewModel.AssetDropId)
                    {
                        var assetDrop = await Context.DataContext.AssetDrops.FirstOrDefaultAsync(t => t.Id == assetDropId && t.InvoiceId == null);
                        if (assetDrop != null)
                        {
                            if (assetDrop.Image != null)
                            {
                                Context.DataContext.Images.Remove(assetDrop.Image);
                            }
                            Context.DataContext.AssetDrops.Remove(assetDrop);
                        }
                    }

                    foreach (var spillId in viewModel.SpillId)
                    {
                        var spill = await Context.DataContext.Spills.FirstOrDefaultAsync(t => t.Id == spillId && t.InvoiceId == null);
                        if (spill != null)
                        {
                            if (spill.Images != null)
                            {
                                Context.DataContext.Images.RemoveRange(spill.Images);
                            }
                            Context.DataContext.Spills.Remove(spill);
                        }
                    }
                    await Context.CommitAsync();
                    transaction.Commit();

                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageSuccess;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("OrderDomain", "DeleteAssetDrop", ex.Message, ex);
                }
            }

            return response;
        }

        public async Task<StatusViewModel> SetAssetDropStatus(AssetDropStatusViewModel viewModel)
        {
            var response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (var jobXAssetId in viewModel.JobXAssetId)
                    {
                        await SaveUnFilledAssetDrop(jobXAssetId, viewModel.OrderId, viewModel.UserId, viewModel.DropStatus);
                    }

                    await Context.CommitAsync();
                    transaction.Commit();

                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageSuccess;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("OrderDomain", "SetAssetDropStatus", ex.Message, ex);
                }
            }

            return response;
        }

        public async Task<DriverOrderAssetViewModel> GetFilledAssetAsync(int assetId, int orderId)
        {
            var response = new DriverOrderAssetViewModel();
            var order = await Context.DataContext.Orders.Select(x => new { x.Id, x.FuelRequest.JobId }).SingleOrDefaultAsync(t => t.Id == orderId);
            if (order != null)
            {
                var assetDrop = await Context.DataContext.AssetDrops.Where(t => t.JobXAsset.AssetId == assetId &&
               t.OrderId == orderId && t.InvoiceId == null && t.JobXAsset.RemovedBy == null && t.JobXAsset.RemovedDate == null).Select(
                    x => new
                    {
                        x.JobXAsset.Id,
                        x.JobXAsset.Asset.Name,
                        AssetDrops = x,
                        Spills = x.JobXAsset.Asset.Spills.Where(t => t.InvoiceId == null && t.OrderId == orderId).OrderByDescending(t => t.Id)
                    }).ToListAsync();

                if (assetDrop != null)
                {
                    List<int> orderIds = new List<int>();
                    orderIds.Add(orderId);
                    response = GetDriverOrderAssetAsync(assetDrop, assetDrop.Select(x => x.AssetDrops).ToList(), orderIds);
                    //if (response.StatusCode == Status.Success)
                    //{
                    //    response.FuelDrop.OrderId = orderId;
                    //}
                }
            }

            return response;
        }

        public async Task<DriverAssetDropHistoryViewModel> GetDriverAssetDropHistoryAsync(int jobId, int assetId, int companyId, int skipCount, int limit)
        {
            var response = new DriverAssetDropHistoryViewModel();

            var companyOrders = Context.DataContext.Orders.Where(t => t.AcceptedCompanyId == companyId);
            var jobOrders = companyOrders.Where(t => t.FuelRequest.Job.Id == jobId);
            if (jobOrders.Any())
            {
                var job = await Context.DataContext.Jobs.FirstOrDefaultAsync(t => t.Id == jobId);
                if (job != null)
                {
                    var jobXAsset = job.JobXAssets.SingleOrDefault(t => t.Asset.Id == assetId && t.RemovedBy == null && t.RemovedDate == null);
                    if (jobXAsset != null)
                    {
                        response.Asset = jobXAsset.Asset.ToViewModel();
                        response.DropHistory = jobXAsset.AssetDrops.Where(t => t.Order.FuelRequest.FuelRequestTypeId != (int)FuelRequestType.BrokeredFuelRequest)
                                                        .OrderByDescending(t => t.DropEndDate)
                                                        .Skip(skipCount).Take(limit).Select(t => t.ToAssetDropViewModel()).ToList();

                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMessageSuccess;
                    }
                }
            }

            return response;
        }

        public async Task<int> SaveAssetDropForFilld(AssetDropViewModel viewModel)
        {
            var response = 0;
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    AssetDrop assetDrop = null;
                    assetDrop = viewModel.ToEntity(assetDrop);
                    if (assetDrop.InvoiceId == 0)
                    {
                        assetDrop.InvoiceId = (int?)null;
                    }

                    var duplicateDrop = Context.DataContext.AssetDrops.FirstOrDefault(t => t.OrderId == viewModel.OrderId && t.JobXAssetId == viewModel.JobXAssetId
                                            && t.MeterStartReading == assetDrop.MeterStartReading && t.MeterEndReading == assetDrop.MeterEndReading && t.DroppedGallons == assetDrop.DroppedGallons
                                            && t.DroppedBy == viewModel.DroppedBy && t.DropStartDate == assetDrop.DropStartDate);

                    if (duplicateDrop == null)
                    {
                        Context.DataContext.AssetDrops.Add(assetDrop);
                    }
                    else if (duplicateDrop != null)
                    {
                        duplicateDrop.MeterStartReading = assetDrop.MeterStartReading;
                        duplicateDrop.MeterEndReading = assetDrop.MeterEndReading;
                        duplicateDrop.DropStartDate = assetDrop.DropStartDate;
                        duplicateDrop.DropEndDate = assetDrop.DropEndDate;
                        duplicateDrop.DroppedGallons = assetDrop.DroppedGallons;
                        duplicateDrop.Gravity = assetDrop.Gravity;
                        Context.DataContext.Entry(duplicateDrop).State = EntityState.Modified;
                    }

                    await Context.CommitAsync();
                    transaction.Commit();

                    if (duplicateDrop == null)
                    {
                        response = assetDrop.Id;
                    }
                    else
                    {
                        response = duplicateDrop.Id;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("OrderDomain", "SaveAssetDropForFilld", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusViewModel> CreateDeliveryRequest(List<ApiDeliveryRequestViewModel> viewModel, UserContext userContext)
        {
            var response = new StatusViewModel();
            try
            {
                var deliveryRequests = new List<RaiseDeliveryRequestInput>();
                foreach (var item in viewModel)
                {
                    var request = new RaiseDeliveryRequestInput();
                    request.Priority = item.Priority;
                    request.RequiredQuantity = item.RequiredQuantity;
                    request.SiteId = item.SiteId;
                    request.StorageId = item.StorageId;
                    request.TankId = item.TankId;
                    request.JobId = item.JobId;
                    request.DelReqSource = DRSource.BuyerApp;
                    request.ProductTypeId = item.ProductTypeId;
                    request.TankMaxFill = item.MaxFill; // added tank maxfill
                    deliveryRequests.Add(request);
                }

                response = await ContextFactory.Current.GetDomain<FreightServiceDomain>().RaiseDeliveryRequests(deliveryRequests, userContext);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "CreateDeliveryRequest", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> SaveDriverAssetDropAsync(DriverAssetFuelDropViewModel viewModel)
        {
            var response = new StatusViewModel(Status.Success);

            if (viewModel.FuelDrop.DropStatus == (int)DropStatus.NoFuelNeeded || viewModel.FuelDrop.DropStatus == (int)DropStatus.AssetNotAvailable)
            {
                int assetDropId = await SaveUnFilledAssetDrop(viewModel.FuelDrop.JobXAssetId, viewModel.FuelDrop.OrderId,
                                                            viewModel.Driver.UserId, viewModel.FuelDrop.DropStatus);
                viewModel.FuelDrop.AssetDropId = assetDropId;
            }
            else
            {
                if (viewModel.FuelDrop.Image != null)
                {
                    await viewModel.FuelDrop.Image.UploadImageToAzureBlobService(ApplicationConstants.AssetDropImageFileNamePrefix, BlobContainerType.JobFilesUpload);
                }

                int assetDropId = await SaveFilledAssetDrop(viewModel);
                viewModel.FuelDrop.AssetDropId = assetDropId;
            }

            try
            {
                var order = await Context.DataContext.Orders.Where(t => t.Id == viewModel.FuelDrop.OrderId).Select(t => new
                {
                    FuelType = t.FuelRequest.MstProduct.MstTFXProduct.Name,
                    ProductType = t.FuelRequest.MstProduct.MstProductType.Name
                }).FirstOrDefaultAsync();
                if (order != null)
                {
                    viewModel.FuelDrop.FuelType = order.FuelType;
                    viewModel.FuelDrop.ProductType = order.ProductType;
                }
                var assetDrop = Context.DataContext.AssetDrops.Where(t => t.OrderId == viewModel.FuelDrop.OrderId &&
                                                    t.InvoiceId == null && t.DroppedBy == viewModel.Driver.UserId &&
                                    t.JobXAssetId == viewModel.FuelDrop.JobXAssetId).Sum(t => t.DroppedGallons);
                viewModel.FuelDrop.PrimaryDrop = assetDrop;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "SaveDriverAssetDropAsync", ex.Message, ex);
            }
            return response;
        }

        private async Task<int> SaveFilledAssetDrop(DriverAssetFuelDropViewModel viewModel)
        {
            var response = 0;
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var order = await Context.DataContext.Orders.FirstOrDefaultAsync(t => t.Id == viewModel.FuelDrop.OrderId);
                    AssetDrop assetDrop = null;
                    bool addAssetDrop = true;
                    if (viewModel.FuelDrop.AssetDropId > 0)
                    {
                        assetDrop = Context.DataContext.AssetDrops.FirstOrDefault(t => t.Id == viewModel.FuelDrop.AssetDropId &&
                                                        t.DroppedBy == viewModel.Driver.UserId && t.DropStatus != (int)DropStatus.None);
                    }
                    else
                    {
                        assetDrop = Context.DataContext.AssetDrops.FirstOrDefault(t => t.JobXAssetId == viewModel.FuelDrop.JobXAssetId && t.InvoiceId == null &&
                                                    t.DroppedBy == viewModel.Driver.UserId && t.OrderId == viewModel.FuelDrop.OrderId && t.DroppedGallons == 0);
                    }

                    if (assetDrop != null)
                    {
                        viewModel.FuelDrop.AssetDropId = assetDrop.Id;
                        addAssetDrop = false;
                    }
                    assetDrop = viewModel.ToAssetDropEntity(assetDrop);

                    LogManager.Logger.WriteDebug("OrderDomain", "SaveFilledAssetDrop",
                                "DropStatus = " + viewModel.FuelDrop.DropStatus + " Gravity = " + viewModel.FuelDrop.Gravity
                                + " JobXAssetId = " + viewModel.FuelDrop.JobXAssetId
                                + " OrderId = " + viewModel.FuelDrop.OrderId + " DroppedGallons=" + viewModel.FuelDrop.PrimaryDrop
                                + " AssetDropId=" + viewModel.FuelDrop.AssetDropId + " DriverId=" + viewModel.Driver.UserId
                                + " DropStartDate = " + viewModel.DropStartDate.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName)
                                + " DropEndDate = " + viewModel.DropEndDate.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName));

                    assetDrop.DroppedGallons = viewModel.FuelDrop.DropStatus != (int)DropStatus.None ? 0 : viewModel.FuelDrop.PrimaryDrop;
                    assetDrop.Gravity = viewModel.FuelDrop.DropStatus != (int)DropStatus.None ? (decimal?)null : viewModel.FuelDrop.Gravity;
                    assetDrop.MeterStartReading = viewModel.FuelDrop.DropStatus != (int)DropStatus.None ? 0 : viewModel.FuelDrop.PrimaryMeterStartReading;
                    assetDrop.MeterEndReading = viewModel.FuelDrop.DropStatus != (int)DropStatus.None ? 0 : viewModel.FuelDrop.PrimaryMeterEndReading;
                    assetDrop.DropStartDate = viewModel.DropStartDate.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName);
                    assetDrop.DropEndDate = viewModel.DropEndDate.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName);

                    var subcontractors = (from jAsset in Context.DataContext.JobXAssets
                                          join a in Context.DataContext.Assets on jAsset.AssetId equals a.Id
                                          join aSub in Context.DataContext.AssetSubcontractors on a.Id equals aSub.AssetId into subContr
                                          from subContractor in subContr.DefaultIfEmpty()
                                          join s in Context.DataContext.Subcontractors on subContractor.SubcontractorId equals s.Id into contr
                                          from contractor in contr.DefaultIfEmpty()
                                          join ac in Context.DataContext.AssetContractNumbers on a.Id equals ac.AssetId into contractNum
                                          from contractNumbers in contractNum.DefaultIfEmpty()
                                          where viewModel.FuelDrop.JobXAssetId == jAsset.Id
                                          select new
                                          {
                                              Id = jAsset.Id,
                                              ContractNum = contractNumbers != null ? contractNumbers.ContractNumber : "",
                                              Timezone = jAsset.Job.TimeZoneName,
                                              AssignedDate = subContractor != null ? subContractor.AssignedDate : (DateTimeOffset?)null,
                                              RemovedDate = subContractor != null ? subContractor.RemovedDate : null,
                                              SubName = contractor != null ? contractor.Name : "",
                                              SubId = contractor != null ? contractor.Id : 0,
                                              AddedDate = contractNumbers != null ? contractNumbers.AddedDate : (DateTimeOffset?)null,
                                              DeletedDate = contractNumbers != null ? contractNumbers.RemovedDate : null,
                                          }).ToList();

                    var subcontractor = subcontractors.FirstOrDefault(t => t.Id == viewModel.FuelDrop.JobXAssetId && (t.AssignedDate == null || t.AssignedDate.Value.ToTargetDateTimeOffset(t.Timezone).DateTime <= assetDrop.DropEndDate.DateTime)
                                                                && (t.RemovedDate == null || t.RemovedDate.Value.ToTargetDateTimeOffset(t.Timezone).DateTime >= assetDrop.DropEndDate.DateTime));
                    if (subcontractor != null)
                    {
                        assetDrop.SubcontractorName = subcontractor.SubName;
                        assetDrop.SubcontractorId = subcontractor.SubId;
                    }
                    var contractNumber = subcontractors.FirstOrDefault(t => t.Id == viewModel.FuelDrop.JobXAssetId && (t.AddedDate == null || t.AddedDate.Value.ToTargetDateTimeOffset(t.Timezone).DateTime <= assetDrop.DropEndDate.DateTime)
                                                               && (t.DeletedDate == null || t.DeletedDate.Value.ToTargetDateTimeOffset(t.Timezone).DateTime >= assetDrop.DropEndDate.DateTime));
                    if (contractNumber != null && subcontractor != null)
                    {
                        assetDrop.ContractNumber = subcontractor.ContractNum;
                    }

                    var duplicateDrop = Context.DataContext.AssetDrops.FirstOrDefault(t => t.OrderId == viewModel.FuelDrop.OrderId && t.JobXAssetId == viewModel.FuelDrop.JobXAssetId
                                            && t.MeterStartReading == assetDrop.MeterStartReading && t.MeterEndReading == assetDrop.MeterEndReading && t.DroppedGallons == assetDrop.DroppedGallons
                                            && t.DroppedBy == viewModel.Driver.UserId && t.DropStartDate == assetDrop.DropStartDate);

                    if (duplicateDrop == null && addAssetDrop)
                    {
                        Context.DataContext.AssetDrops.Add(assetDrop);
                    }
                    else
                    {
                        if (duplicateDrop != null)
                        {
                            duplicateDrop.MeterStartReading = assetDrop.MeterStartReading;
                            duplicateDrop.MeterEndReading = assetDrop.MeterEndReading;
                            duplicateDrop.DropStartDate = assetDrop.DropStartDate;
                            duplicateDrop.DropEndDate = assetDrop.DropEndDate;
                            duplicateDrop.DroppedGallons = assetDrop.DroppedGallons;
                            duplicateDrop.Gravity = assetDrop.Gravity;
                            Context.DataContext.Entry(duplicateDrop).State = EntityState.Modified;
                        }
                        else
                        {
                            Context.DataContext.Entry(assetDrop).State = EntityState.Modified;
                        }
                    }

                    await Context.CommitAsync();
                    transaction.Commit();

                    if (duplicateDrop == null)
                    {
                        response = assetDrop.Id;
                    }
                    else
                    {
                        response = duplicateDrop.Id;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("OrderDomain", "SaveFilledAssetDrop", ex.Message, ex);
                }
            }
            return response;
        }

        private async Task<int> SaveUnFilledAssetDrop(int jobXAssetId, int orderId, int userId, int dropStatus)
        {
            var response = 0;
            try
            {
                var order = await Context.DataContext.Orders.FirstOrDefaultAsync(t => t.Id == orderId);
                var assetDrop = await Context.DataContext.AssetDrops.FirstOrDefaultAsync(t => t.JobXAssetId == jobXAssetId
                                        && t.OrderId == order.Id && t.DroppedBy == userId && t.InvoiceId == null);
                if (assetDrop != null)
                {
                    assetDrop.DropStatus = dropStatus;
                    assetDrop.DropStartDate = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName);
                    assetDrop.DropEndDate = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName);

                    Context.DataContext.Entry(assetDrop).State = EntityState.Modified;
                    await Context.CommitAsync();
                }
                else
                {
                    assetDrop = new AssetDrop();
                    assetDrop.OrderId = orderId;
                    assetDrop.JobXAssetId = jobXAssetId;
                    assetDrop.DropStartDate = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName);
                    assetDrop.DropEndDate = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName);
                    assetDrop.DroppedBy = userId;
                    assetDrop.DropStatus = dropStatus;
                    assetDrop.IsActive = true;
                    assetDrop.UpdatedBy = userId;
                    assetDrop.UpdatedDate = assetDrop.DropEndDate;
                    Context.DataContext.AssetDrops.Add(assetDrop);
                    await Context.CommitAsync();
                }
                response = assetDrop.Id;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "SaveUnFilledAssetDrop", ex.Message, ex);
            }
            return response;
        }

        private DriverOrderAssetViewModel GetDriverOrderAssetAsync(dynamic jobXAsset, List<AssetDrop> assetDrops, List<int> orderId)
        {
            var response = new DriverOrderAssetViewModel(Status.Success);
            if (jobXAsset != null)
            {
                var assetDrop = assetDrops.FirstOrDefault();

                response.FuelDrop.JobXAssetId = jobXAsset.Id;
                response.FuelDrop.InvoiceId = assetDrop == null ? 0 : assetDrop.InvoiceId ?? 0;

                if (assetDrop == null)
                    response.FuelDrop.OrderId = orderId.FirstOrDefault();
                else
                    response.FuelDrop.OrderId = assetDrop.OrderId;

                response.Asset = new AssetViewModel
                {
                    Id = jobXAsset.AssetId,
                    Name = jobXAsset.AssetName,
                    FuelType = new AssetFuelTypeViewModel { Name = jobXAsset.FuelTypeName }
                };
                response.FuelDrop.AssetDropId = assetDrop == null ? 0 : assetDrop.Id;
                response.FuelDrop.PrimaryDrop = assetDrop == null ? 0 : assetDrop.DroppedGallons;
                //response.FuelDrop.Gravity = assetDrop == null ? 0 : assetDrop.Gravity;
                response.FuelDrop.PrimaryMeterStartReading = assetDrop == null ? 0 : assetDrop.MeterStartReading;
                response.FuelDrop.PrimaryMeterEndReading = assetDrop == null ? 0 : assetDrop.MeterEndReading;
                response.FuelDrop.PrimaryDropId = assetDrop == null ? 0 : assetDrop.Id;
                response.FuelDrop.IsNoFuelNeeded = assetDrop == null ? false : assetDrops.All(t => t.DroppedGallons == 0);
                response.FuelDrop.DropStatus = assetDrop == null ? (int)DropStatus.None : assetDrop.DropStatus;
                response.FuelDrop.IsSpillOccurred = jobXAsset.Spills != null;
                response.FuelDrop.SpillId = jobXAsset.Spills == null ? 0 : jobXAsset.Spills.Id;
                response.FuelDrop.DroppedGallons = assetDrops.Sum(t => t.DroppedGallons);

                if (assetDrops.Count > 1)
                {
                    var secondary = assetDrops.Last();
                    response.FuelDrop.SecondaryDrop = secondary.DroppedGallons;
                    response.FuelDrop.SecondaryMeterStartReading = secondary.MeterStartReading;
                    response.FuelDrop.SecondaryMeterEndReading = secondary.MeterEndReading;
                    response.FuelDrop.SecondaryDropId = secondary.Id;
                }

                var lstAssetDropDetails = new List<AssetDropResponseViewModel>();
                foreach (var item in orderId)
                {
                    var assetDropDetail = new AssetDropResponseViewModel();
                    assetDropDetail.JobXAssetId = jobXAsset.Id;

                    var orderAssetDrop = assetDrops.Where(t => t.OrderId == item);
                    var firstOrderAssetDrop = orderAssetDrop.FirstOrDefault();
                    assetDropDetail.AssetDropId = firstOrderAssetDrop != null ? firstOrderAssetDrop.Id : 0;
                    assetDropDetail.Quantity = orderAssetDrop != null ? orderAssetDrop.Sum(t => t.DroppedGallons) : 0;

                    assetDropDetail.OrderId = item;
                    lstAssetDropDetails.Add(assetDropDetail);
                }
                response.FuelDrop.AssetDropDetail = lstAssetDropDetails;
            }
            else
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = Resource.errMessageFailed;
            }

            return response;
        }

        private DriverOrderAssetViewModel GetDriverOrderAssetForMarineAsync(int assetId, string fuelTypeName, string assetName, List<int> orderId, int driverId = 0)
        {
            var response = new DriverOrderAssetViewModel(Status.Success);
            var jobXAssets = Context.DataContext.JobXAssets.Where(t => t.AssetId == assetId && orderId.Contains(t.OrderId.Value)).
                                                        Select(x => new
                                                        {
                                                            x.Id,
                                                            x.OrderId,
                                                            AssetDrops = x.AssetDrops.Where(t => t.InvoiceId == null && orderId.Contains(t.OrderId)).OrderBy(t => t.DropEndDate),
                                                            Spills = x.Asset.Spills.Where(t => t.InvoiceId == null && orderId.Contains(t.OrderId)).OrderByDescending(t => t.Id).FirstOrDefault(),
                                                        }).ToList();

            var assetDrops = new List<AssetDrop>();
            var spills = new List<Spill>();
            if (jobXAssets != null)
            {
                foreach (var item in jobXAssets)
                {
                    var tempAssetDrops = driverId == 0 ? item.AssetDrops.ToList() : item.AssetDrops.Where(t => t.DroppedBy == driverId).ToList();
                    assetDrops.AddRange(tempAssetDrops);
                    spills.Add(item.Spills);
                }

                var assetDrop = assetDrops.FirstOrDefault();
                var spill = spills.FirstOrDefault();

                var jobXAsset = jobXAssets.FirstOrDefault();
                response.FuelDrop.JobXAssetId = jobXAsset != null ? jobXAsset.Id : 0;
                response.FuelDrop.InvoiceId = assetDrops.Count == 0 ? 0 : assetDrop.InvoiceId ?? 0;

                if (assetDrops.Count == 0)
                    response.FuelDrop.OrderId = orderId.FirstOrDefault();
                else
                    response.FuelDrop.OrderId = assetDrop.OrderId;

                response.Asset = new AssetViewModel
                {
                    Id = assetId,
                    Name = assetName,
                    FuelType = new AssetFuelTypeViewModel { Name = fuelTypeName }
                };
                response.FuelDrop.AssetDropId = assetDrops.Count == 0 ? 0 : assetDrop.Id;
                response.FuelDrop.PrimaryDrop = assetDrops.Count == 0 ? 0 : assetDrop.DroppedGallons;
                //response.FuelDrop.Gravity = assetDrop == null ? 0 : assetDrop.Gravity;
                response.FuelDrop.PrimaryMeterStartReading = assetDrops.Count == 0 ? 0 : assetDrop.MeterStartReading;
                response.FuelDrop.PrimaryMeterEndReading = assetDrops.Count == 0 ? 0 : assetDrop.MeterEndReading;
                response.FuelDrop.PrimaryDropId = assetDrops.Count == 0 ? 0 : assetDrop.Id;
                response.FuelDrop.IsNoFuelNeeded = assetDrops.Count == 0 ? false : assetDrops.All(t => t.DroppedGallons == 0);
                response.FuelDrop.DropStatus = assetDrops.Count == 0 ? (int)DropStatus.None : assetDrop.DropStatus;
                response.FuelDrop.IsSpillOccurred = spill != null;
                response.FuelDrop.SpillId = spill == null ? 0 : spill.Id;
                response.FuelDrop.DroppedGallons = assetDrops.Sum(t => t.DroppedGallons);

                if (assetDrops.Count > 1)
                {
                    var secondary = assetDrops.Last();
                    response.FuelDrop.SecondaryDrop = secondary.DroppedGallons;
                    response.FuelDrop.SecondaryMeterStartReading = secondary.MeterStartReading;
                    response.FuelDrop.SecondaryMeterEndReading = secondary.MeterEndReading;
                    response.FuelDrop.SecondaryDropId = secondary.Id;
                }

                var lstAssetDropDetails = new List<AssetDropResponseViewModel>();
                foreach (var item in orderId)
                {
                    var assetDropDetail = new AssetDropResponseViewModel();
                    var jobasset = jobXAssets.Where(t => t.OrderId == item).FirstOrDefault();
                    if (jobasset != null)
                    {
                        assetDropDetail.JobXAssetId = jobasset.Id;
                    }
                    else
                    {
                        //If no Asset for the selected order
                        var firstJobasset = jobXAssets.FirstOrDefault();
                        assetDropDetail.JobXAssetId = firstJobasset != null ? firstJobasset.Id : 0;
                    }
                    //if (item.OrderId.HasValue)
                    {
                        var orderAssetDrop = assetDrops.Where(t => t.OrderId == item);
                        var firstOrderAssetDrop = orderAssetDrop.FirstOrDefault();
                        assetDropDetail.AssetDropId = firstOrderAssetDrop != null ? firstOrderAssetDrop.Id : 0;
                        assetDropDetail.Quantity = orderAssetDrop != null ? orderAssetDrop.Sum(t => t.DroppedGallons) : 0;
                        assetDropDetail.OrderId = item;
                    }
                    lstAssetDropDetails.Add(assetDropDetail);
                }
                response.FuelDrop.AssetDropDetail = lstAssetDropDetails;
            }
            else
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = Resource.errMessageFailed;
            }

            return response;
        }

        public async Task<List<int>> GetOpenOrdersWithEndDateAsync()
        {
            var response = new List<int>();
            try
            {
                response = await Context.DataContext.Orders.Where(
                                               t =>
                                               t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open &&
                                               (t.FuelRequest.FuelRequestDetail.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery
                                                   || t.FuelRequest.FuelRequestDetail.EndDate != null) &&
                                               t.IsActive).Select(t => t.Id).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetOpenOrdersWithEndDateAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task ProcessOrderClosureAsync(int id)
        {
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var entity = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == id);
                    if (entity != null)
                    {
                        bool isExpired = false;
                        var currentDate = DateTimeOffset.Now.ToTargetDateTimeOffset(entity.FuelRequest.Job.TimeZoneName);
                        var fueldeliveryDetails = entity.FuelRequest.FuelRequestDetail;
                        int orderCloseWaitingPeriod = await ContextFactory.Current.GetDomain<MasterDomain>().GetOrderCloseWaitingPeriod();
                        var closeDate = (fueldeliveryDetails.EndDate.HasValue ? fueldeliveryDetails.EndDate.Value : fueldeliveryDetails.StartDate.AddHours(orderCloseWaitingPeriod)).Add(fueldeliveryDetails.EndTime);

                        if (fueldeliveryDetails.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery && closeDate.DateTime < currentDate.DateTime)
                        {
                            isExpired = true;
                        }
                        else if (fueldeliveryDetails.DeliveryTypeId == (int)DeliveryType.MultipleDeliveries && closeDate.Date < currentDate.Date)
                        {
                            isExpired = true;
                        }

                        if (isExpired)
                        {
                            entity.OrderXStatuses.FirstOrDefault(t => t.IsActive).IsActive = false;
                            OrderXStatus orderStatus = new OrderXStatus();
                            orderStatus.StatusId = (int)OrderStatus.Closed;
                            orderStatus.IsActive = true;
                            orderStatus.UpdatedBy = (int)SystemUser.System;
                            orderStatus.UpdatedDate = DateTimeOffset.Now;
                            entity.OrderXStatuses.Add(orderStatus);

                            Context.DataContext.Entry(entity).State = EntityState.Modified;
                            await Context.CommitAsync();

                            transaction.Commit();

                            //// set order closed newsfeed for single delivery
                            var avgGallonsPercentagePerDelivery = new HelperDomain(this).GetAverageFuelDropPercentagePerOrder(entity);
                            if (fueldeliveryDetails.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery && avgGallonsPercentagePerDelivery <= 0)
                            {
                                await ContextFactory.Current.GetDomain<NewsfeedDomain>().OrderClosedOnDateExpiredNewsfeed(entity);
                            }
                            UserContext userContext = new UserContext();
                            userContext.UserName = SystemUser.System.ToString();
                            var drCloseStatus = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().DeleteDeliveryRequestOnOrderClose(new List<int> { id }, userContext);
                            if (drCloseStatus.StatusCode != (int)Status.Success)
                            {
                                LogManager.Logger.WriteError("DeleteDeliveryRequestOnOrderClose", "ProcessOrderClosureAsync", drCloseStatus.StatusMessage);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("FuelRequestDomain", "ProcessOrderClosureAsync", ex.Message, ex);
                }
            }
        }

        public async Task<List<AssignToOrderGridViewModel>> GetAssignToOrderGridAsync(int userId)
        {
            List<AssignToOrderGridViewModel> response = new List<AssignToOrderGridViewModel>();
            HelperDomain helperDomain = new HelperDomain(this);
            try
            {
                var user = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == userId && t.IsActive);
                if (user != null && user.Company != null)
                {
                    response = user.Company.Orders.Where(t => t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open).Select(t => new AssignToOrderGridViewModel(Status.Success)
                    {
                        OrderId = t.Id,
                        PoNumber = t.PoNumber,
                        CustomerName = t.FuelRequest.GetCompany().Name,
                        Location = $"{t.FuelRequest.Job.Address} {t.FuelRequest.Job.City} {t.FuelRequest.Job.MstState.Name}",
                        GallonsOrdered = t.FuelRequest.QuantityTypeId != (int)QuantityType.NotSpecified ? $"{(t.BrokeredMaxQuantity ?? t.FuelRequest.MaxQuantity).GetPreciseValue(2).GetCommaSeperatedValue()} {t.FuelRequest.UoM}" : Resource.lblNotSpecified,
                        OrderUoM = t.FuelRequest.UoM.ToString(),
                    }).ToList();

                    //// add single delivery closed orders of 0 percent drop/0 gallons drop
                    var orders = user.Company.Orders.Where(t => t.FuelRequest.FuelRequestDetail.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery &&
                                                                t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Closed).ToList();
                    if (orders != null)
                    {
                        foreach (var order in orders)
                        {
                            var avgGallonsPercentagePerDelivery = helperDomain.GetAverageFuelDropPercentagePerOrder(order);
                            if (avgGallonsPercentagePerDelivery <= 0)
                            {
                                response.Add(new AssignToOrderGridViewModel()
                                {
                                    OrderId = order.Id,
                                    PoNumber = order.PoNumber,
                                    CustomerName = order.FuelRequest.GetCompany().Name,
                                    Location = $"{order.FuelRequest.Job.Address} {order.FuelRequest.Job.City} {order.FuelRequest.Job.MstState.Name}",
                                    GallonsOrdered = order.FuelRequest.QuantityTypeId != (int)QuantityType.NotSpecified ? $"{((order.BrokeredMaxQuantity ?? order.FuelRequest.MaxQuantity)).GetPreciseValue(2).GetCommaSeperatedValue()} {order.FuelRequest.UoM}" : Resource.lblNotSpecified,
                                    OrderUoM = order.FuelRequest.UoM.ToString(),
                                });
                            }
                        }
                    }

                    response = response.OrderByDescending(t => t.OrderId).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetAssignToOrderGridAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<AssignToOrderPreviewViewModel> GetOrderPreviewAsync(int orderId, int invoiceId)
        {
            AssignToOrderPreviewViewModel response = new AssignToOrderPreviewViewModel();
            try
            {
                var invoice = await Context.DataContext.Invoices.Where(t => t.Id == invoiceId)
                                                          .Select(t => new { t.DroppedGallons, t.DropStartDate, t.DropEndDate, t.Driver, t.IsWetHosingDelivery, t.IsOverWaterDelivery, t.UoM, t.InvoiceXAdditionalDetail })
                                                          .SingleOrDefaultAsync();
                if (invoice != null)
                {
                    response.InvoiceId = invoiceId;

                    var order = Context.DataContext.Orders.SingleOrDefault(t => t.Id == orderId);
                    if (order != null)
                    {
                        var job = order.FuelRequest.Job;
                        HelperDomain helperDomain = new HelperDomain(this);
                        response = new AssignToOrderPreviewViewModel(Status.Success);

                        response.OrderId = order.Id;
                        response.InvoiceId = invoiceId;
                        response.GallonsOrdered = order.BrokeredMaxQuantity ?? order.FuelRequest.MaxQuantity;
                        response.GallonsDropped = order.Invoices.Where(t => t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && !t.IsBuyPriceInvoice).Sum(t => t.DroppedGallons);
                        response.QuantityTypeId = order.FuelRequest.QuantityTypeId;
                        response.GallonsRemaining = response.GallonsOrdered - response.GallonsDropped;
                        response.DeliveryPercentage = helperDomain.GetFuelDeliveredPercentage(order, 0);
                        response.OrderTypeId = order.FuelRequest.OrderTypeId;
                        response.DeliveryTypeId = order.FuelRequest.FuelRequestDetail.DeliveryTypeId;
                        response.StartTime = Convert.ToDateTime(order.FuelRequest.FuelRequestDetail.StartTime.ToString()).ToShortTimeString();
                        response.EndTime = Convert.ToDateTime(order.FuelRequest.FuelRequestDetail.EndTime.ToString()).ToShortTimeString();
                        response.StartDate = order.AcceptedDate.ToString(Resource.constFormatDate);
                        response.Assets = order.FuelRequest.Job.JobXAssets.Count(t => t.RemovedBy == null && t.RemovedDate == null);
                        response.OrderUoM = order.FuelRequest.UoM;
                        response.Currency = order.FuelRequest.Currency;
                        var assignedDriver = order.OrderXDrivers.SingleOrDefault(t => t.IsActive);
                        response.AssignedDriver = assignedDriver == null ? Resource.lblNoDriverAssigned : $"{assignedDriver.User.FirstName} {assignedDriver.User.LastName}";

                        response.AssignToOrderGrid = new AssignToOrderGridViewModel
                        {
                            CustomerName = order.FuelRequest.User.Company.Name,
                            Location = $"{job.Address}, {job.City}, {job.MstState.Code}",
                            PoNumber = order.PoNumber
                        };
                        response.FuelRequestFee = order.FuelRequest.FuelRequestFees.ToViewModel();
                        response.DeliverySchedules = order.FuelRequest.DeliverySchedules.GroupBy(t => t.GroupId).Select(g => new { Items = g.ToList() }).Select(t => t.Items.ToViewModel()).ToList();
                    }
                    else
                    {
                        response.StartDate = Resource.lblHyphen;
                        response.AssignToOrderGrid.CustomerName = Resource.lblHyphen;
                        response.AssignToOrderGrid.Location = Resource.lblHyphen;
                        response.AssignToOrderGrid.PoNumber = Resource.lblHyphen;
                    }

                    response.DriverDroppedGallons = invoice.DroppedGallons;
                    response.DropStartDate = invoice.DropStartDate;
                    response.DropEndDate = invoice.DropEndDate;
                    response.DriverName = invoice.Driver == null ? Resource.lblHyphen : $"{invoice.Driver.FirstName} {invoice.Driver.LastName}";
                    response.IsWetHoseFee = invoice.IsWetHosingDelivery;
                    response.IsOverWaterFee = invoice.IsOverWaterDelivery;
                    response.InvoiceUoM = invoice.UoM;

                    if (invoice.InvoiceXAdditionalDetail != null)
                    {
                        response.AssetFilled = invoice.InvoiceXAdditionalDetail.AssetFilled;
                        response.DriverCustomerName = invoice.InvoiceXAdditionalDetail.DriverComment;
                        var point = GoogleApiDomain.GetAddress(Convert.ToDouble(invoice.InvoiceXAdditionalDetail.Latitude), Convert.ToDouble(invoice.InvoiceXAdditionalDetail.Longitude));
                        if (point != null)
                        {
                            response.DriverDropAddress = point.Address;
                            response.DriverDropCity = point.City;
                            response.DriverDropState = point.StateCode;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetOrderPreViewAsync", ex.Message, ex);
            }

            return response;
        }

        public OrderFilterViewModel GetOrderFilter(int jobId, OrderFilterType filter, int fuelTypeId = 0, int orderId = 0, string groupIds = "")
        {
            var response = new OrderFilterViewModel();
            try
            {
                response.JobId = jobId;
                response.Filter = filter;
                response.FuelTypeId = fuelTypeId;
                response.OrderId = orderId;
                response.GroupIds = groupIds;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetOrderFilter", ex.Message, ex);
            }
            return response;
        }

        private IQueryable<Order> ApplySupplierOrderFilter(OrderFilterViewModel orderFilter, IQueryable<Order> allOrders)
        {
            if (orderFilter != null)
            {
                if (orderFilter.Filter == OrderFilterType.Open)
                {
                    allOrders = allOrders.Where(t => t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open);
                }
                else if (orderFilter.Filter == OrderFilterType.Closed)
                {
                    allOrders = allOrders.Where(t => t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Closed || t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.PartiallyClosed);
                }
                else if (orderFilter.Filter == OrderFilterType.Canceled)
                {
                    allOrders = allOrders.Where(t => t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Canceled || t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.PartiallyCanceled);
                }
                else if (orderFilter.Filter == OrderFilterType.TotalDelivered)
                {
                    allOrders = allOrders.Where(t => t.Invoices.Count > 0);
                }
                else if (orderFilter.Filter == OrderFilterType.FiftyPlusDelivered)
                {
                    allOrders = allOrders.Where(t => ((t.FuelRequest.HedgeDroppedGallons + t.FuelRequest.SpotDroppedGallons) / (t.FuelRequest.MaxQuantity == 0 ? 1 : (t.BrokeredMaxQuantity ?? t.FuelRequest.MaxQuantity)) * 100) > 50);
                }

                DateTimeOffset StartDate = DateTimeOffset.Now.Date.AddDays(ApplicationConstants.DateFilterDefaultDays);
                DateTimeOffset EndDate = DateTimeOffset.Now.Date.AddDays(1);
                if (!string.IsNullOrEmpty(orderFilter.StartDate))
                {
                    StartDate = Convert.ToDateTime(orderFilter.StartDate).Date;
                }
                if (!string.IsNullOrEmpty(orderFilter.EndDate))
                {
                    EndDate = Convert.ToDateTime(orderFilter.EndDate).Date.AddDays(1);
                }

                allOrders = allOrders.Where(t => t.AcceptedDate >= StartDate && t.AcceptedDate < EndDate);
            }
            return allOrders;
        }

        private IQueryable<Order> ApplyBuyerOrderFilter(OrderFilterViewModel orderFilter, IQueryable<Order> allOrders)
        {
            if (orderFilter != null)
            {
                if (orderFilter.FuelTypeId > 0)
                {
                    allOrders = allOrders.Where(t => t.FuelRequest.FuelTypeId == orderFilter.FuelTypeId);
                }

                if (orderFilter.JobId > 0)
                {
                    allOrders = allOrders.Where(t => t.FuelRequest.Job.Id == orderFilter.JobId);
                }
                else
                {
                    if (!string.IsNullOrEmpty(orderFilter.StartDate))
                    {
                        var startDate = Convert.ToDateTime(orderFilter.StartDate).Date;
                        allOrders = allOrders.Where(t => t.AcceptedDate >= startDate);
                    }
                    if (!string.IsNullOrEmpty(orderFilter.EndDate))
                    {
                        var endDate = Convert.ToDateTime(orderFilter.EndDate).Date.AddDays(1);
                        allOrders = allOrders.Where(t => t.AcceptedDate <= endDate);
                    }
                }

                if (orderFilter.Filter == OrderFilterType.Open)
                {
                    allOrders = allOrders.Where(t => t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open || t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.PartiallyCanceled || t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.PartiallyClosed);
                }
                else if (orderFilter.Filter == OrderFilterType.Closed)
                {
                    allOrders = allOrders.Where(t => t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Closed);
                }
                else if (orderFilter.Filter == OrderFilterType.Canceled)
                {
                    allOrders = allOrders.Where(t => t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Canceled);
                }
                else if (orderFilter.Filter == OrderFilterType.TotalDelivered)
                {
                    allOrders = allOrders.Where(t => t.Invoices.Count > 0);
                }
                else if (orderFilter.Filter == OrderFilterType.FiftyPlusDelivered)
                {
                    allOrders = allOrders.Where(t => ((t.FuelRequest.HedgeDroppedGallons + t.FuelRequest.SpotDroppedGallons) / (t.FuelRequest.MaxQuantity == 0 ? 1 : (t.BrokeredMaxQuantity ?? t.FuelRequest.MaxQuantity)) * 100) > 50);
                }
            }

            return allOrders;
        }

        public async Task<StatusViewModel> SaveOtherPorductTypeTaxes(UserContext userContext, OrderDetailsViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == viewModel.Id);
                    order.OrderTaxDetails.ToList().ForEach(t => t.IsActive = false);

                    CurrencyRateDomain currencyRateDomain = new CurrencyRateDomain();
                    var exchangeRate = currencyRateDomain.GetCurrencyRate(order.FuelRequest.Currency, Currency.USD, DateTimeOffset.Now);

                    if (viewModel.IsOtherFuelTypeTaxesGiven)
                    {
                        foreach (var taxDetail in viewModel.TaxDetailsViewModel)
                        {
                            taxDetail.ExchangeRate = exchangeRate;
                            taxDetail.Currency = order.FuelRequest.Currency;
                            taxDetail.AddedBy = userContext.Id;
                            taxDetail.AddedByCompanyId = userContext.CompanyId;
                            order.OrderTaxDetails.Add(taxDetail.ToEntity(order.FuelRequest.FuelTypeId));
                        }
                    }
                    await Context.CommitAsync();
                    transaction.Commit();
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.msgOrderTaxesUpdatedSuccess;
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageSaveOtherProductTypeTaxesFailed;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("OrderDomain", "SaveOtherPorductTypeTaxes", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusViewModel> SaveBadgeDetails(UserContext userContext, OrderDetailsViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var orderBadgeDetail = new OrderBadgeDetail();
                    var order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == viewModel.Id);
                    if (order.OrderBadgeDetails != null && order.OrderBadgeDetails.Count > 0)
                    {
                        order.OrderBadgeDetails.ToList().ForEach(t => t.IsActive = false);
                        await Context.CommitAsync();
                    }

                    if (!string.IsNullOrEmpty(viewModel.OrderBadgeDetails.BadgeNo1) || !string.IsNullOrEmpty(viewModel.OrderBadgeDetails.BadgeNo2) ||
                            !string.IsNullOrEmpty(viewModel.OrderBadgeDetails.BadgeNo3))
                    {
                        orderBadgeDetail.BadgeNo1 = viewModel.OrderBadgeDetails.BadgeNo1;
                        orderBadgeDetail.BadgeNo2 = viewModel.OrderBadgeDetails.BadgeNo2;
                        orderBadgeDetail.BadgeNo3 = viewModel.OrderBadgeDetails.BadgeNo3;
                        orderBadgeDetail.IsCommonBadge = true;
                        orderBadgeDetail.CreatedBy = userContext.Id;
                        orderBadgeDetail.CreatedDate = DateTimeOffset.Now;
                        orderBadgeDetail.UpdatedBy = userContext.Id;
                        orderBadgeDetail.UpdatedDate = DateTimeOffset.Now;
                        orderBadgeDetail.PickupLocationType = PickupLocationType.None;
                        orderBadgeDetail.IsActive = true;
                        orderBadgeDetail.OrderId = viewModel.Id;
                        order.OrderBadgeDetails.Add(orderBadgeDetail);
                        await Context.CommitAsync();
                    }

                    if (viewModel.OrderBadgeDetails.TerminalBulkBadge != null)
                    {
                        foreach (var item in viewModel.OrderBadgeDetails.TerminalBulkBadge)
                        {
                            if ((item.TerminalId.HasValue || item.BulkPlantId.HasValue) &&
                                !string.IsNullOrEmpty(item.BadgeNo1) || !string.IsNullOrEmpty(item.BadgeNo2) || !string.IsNullOrEmpty(item.BadgeNo3))
                            {
                                orderBadgeDetail = new OrderBadgeDetail();
                                orderBadgeDetail.BadgeNo1 = item.BadgeNo1;
                                orderBadgeDetail.BadgeNo2 = item.BadgeNo2;
                                orderBadgeDetail.BadgeNo3 = item.BadgeNo3;
                                orderBadgeDetail.IsCommonBadge = false;
                                if (item.IsPickupTerminal)
                                {
                                    orderBadgeDetail.TerminalId = item.TerminalId;
                                    orderBadgeDetail.PickupLocationType = PickupLocationType.Terminal;
                                }
                                else
                                {
                                    orderBadgeDetail.BulkPlantId = item.BulkPlantId;
                                    orderBadgeDetail.PickupLocationType = PickupLocationType.BulkPlant;
                                }
                                orderBadgeDetail.CreatedBy = userContext.Id;
                                orderBadgeDetail.CreatedDate = DateTimeOffset.Now;
                                orderBadgeDetail.UpdatedBy = userContext.Id;
                                orderBadgeDetail.UpdatedDate = DateTimeOffset.Now;
                                orderBadgeDetail.IsActive = true;
                                orderBadgeDetail.OrderId = viewModel.Id;
                                order.OrderBadgeDetails.Add(orderBadgeDetail);
                                await Context.CommitAsync();
                            }
                        }
                    }

                    transaction.Commit();
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.msgOrderBadgeUpdatedSuccess;
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageSaveOrderBadgeFailed;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("OrderDomain", "SaveBadgeDetails", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusViewModel> SaveDeliverySchedulesAsync(UserContext userContext, OrderDetailsViewModel viewModel, bool isBuyer)
        {
            StatusViewModel response = new StatusViewModel();
            bool isNewVersion = false;
            bool isScheduleModified = false;
            viewModel.UpdatedBy = userContext.Id;
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var order = await Context.DataContext.Orders.Include(t => t.OrderDeliverySchedules)
                                                                .Include(t => t.FuelRequest)
                                                                .Include(t => t.FuelRequest.FuelRequestDetail)
                                                                .Include(t => t.DeliveryScheduleXTrackableSchedules)
                                                                .SingleOrDefaultAsync(t => t.Id == viewModel.Id);
                    if (order != null)
                    {
                        var orderDeliverySchedules = GetLatestOrderDeliverySchedule(order.OrderDeliverySchedules);
                        var latestVersionId = orderDeliverySchedules.Select(t => t.Version).FirstOrDefault();
                        if (latestVersionId != viewModel.FuelDeliveryDetails.OrderVersion)
                        {
                            response.StatusMessage = Resource.valMessageSchedulesFailedOnOldVersion;
                            return response;
                        }
                        var jobTime = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName);
                        var deliveryScheduleToUpdateFrom = order.FuelRequest.FuelRequestDetail.StartDate > jobTime ? order.FuelRequest.FuelRequestDetail.StartDate : jobTime;
                        viewModel.DeliverySchedules.ForEach(t => t.UoM = order.FuelRequest.UoM);

                        // update carriers
                        if (viewModel.DeliverySchedules != null && viewModel.DeliverySchedules.Any(t => t.Carrier != null))
                        {
                            viewModel.DeliverySchedules.ForEach(t => { t.Carrier.CompanyId = userContext.CompanyId; t.Carrier.CreatedBy = userContext.Id; });
                        }

                        bool isDriverModified = false;
                        var schedules = new List<DeliverySchedule>(); //all schedules
                        var addedScheduleList = new List<DeliverySchedule>(); // newly added schedules

                        var previousSchedules = orderDeliverySchedules.Where(t => t.DeliveryRequestId.HasValue)
                            .Select(t => new { t.DeliverySchedule, Drivers = t.DeliverySchedule.DeliveryScheduleXDrivers }).ToList();

                        var trackableScheduleIds = Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(t => t.OrderId == viewModel.Id &&
                        (t.DeliveryScheduleStatusId == (int)DeliveryScheduleStatus.Missed || t.DeliveryScheduleStatusId == (int)DeliveryScheduleStatus.RescheduledMissed))
                        .Select(t => t.DeliveryScheduleId).ToList();

                        var previousScheduleGroups = previousSchedules.Where(t => !trackableScheduleIds.Contains(t.DeliverySchedule.Id)).Select(t => t.DeliverySchedule.GroupId).Distinct();

                        // save carrier details
                        isScheduleModified = await ProcessScheduleCarriers(viewModel, previousSchedules.Select(t => t.DeliverySchedule).ToList());
                        await Context.CommitAsync();
                        isNewVersion = ProcessDeliverySchedulesAsync(schedules, addedScheduleList, deliveryScheduleToUpdateFrom, viewModel, orderDeliverySchedules, userContext.Id, order.FuelRequest.Currency, ref isDriverModified);

                        if (orderDeliverySchedules.Count > 0 && !isNewVersion)
                        {
                            var latestScheduleIds = viewModel.DeliverySchedules.Select(t => t.Id).ToList();
                            var currentSchedules = orderDeliverySchedules.Where(t => t.DeliveryRequestId.HasValue).Select(t => t.DeliverySchedule);

                            foreach (var request in currentSchedules)
                            {
                                if (request.Type == (int)DeliveryScheduleType.Weekly || request.Type == (int)DeliveryScheduleType.BiWeekly)
                                {
                                    if (!viewModel.DeliverySchedules.Any(t => t.GroupId == request.GroupId && t.ScheduleDays.Contains(request.WeekDayId)))
                                    {
                                        isNewVersion = true;
                                    }
                                }
                                else if (!(latestScheduleIds.Contains(request.Id)))
                                {
                                    isNewVersion = true;
                                }
                            }
                        }

                        var newsfeedDomain = new NewsfeedDomain(this);
                        var notificationDomain = new NotificationDomain(this);

                        if (isNewVersion)
                        {
                            if (orderDeliverySchedules != null && orderDeliverySchedules.Count > 0)
                            {
                                orderDeliverySchedules.ForEach(t => t.IsActive = false);
                            }

                            addedScheduleList.ForEach(t => Context.DataContext.DeliverySchedules.Add(t));

                            await Context.CommitAsync();

                            if (schedules.Count > 0)
                            {
                                foreach (var item in schedules)
                                {
                                    var ordertempSchedule = GetOrderDeliverySchedule(order.Id, latestVersionId, userContext.Id, item.Id);
                                    if (viewModel.CurrentOrderVersionToEdit != null)
                                    {
                                        ordertempSchedule.AdditionalNotes = viewModel.CurrentOrderVersionToEdit.AdditionalNotes;
                                    }
                                    order.OrderDeliverySchedules.Add(ordertempSchedule);
                                }
                            }
                            else
                            {
                                var ordertempSchedule = GetOrderDeliverySchedule(order.Id, latestVersionId, userContext.Id, null);
                                if (viewModel.CurrentOrderVersionToEdit != null)
                                {
                                    ordertempSchedule.AdditionalNotes = viewModel.CurrentOrderVersionToEdit.AdditionalNotes;
                                }
                                order.OrderDeliverySchedules.Add(ordertempSchedule);
                            }
                            UpdateFuelDispatchLocation(viewModel, schedules, order.FuelRequest.Currency);

                            await Context.CommitAsync();

                            var removedList = GetDeletedSchedules(order);
                            TrackableScheduleDomain trackableScheduleDomain = new TrackableScheduleDomain(this);
                            await trackableScheduleDomain.ProcessTrackableSchedules(schedules, order, removedList);

                            List<int> brokerOrderIds = await GetBrokerOrderIdAsync(viewModel.Id, isBuyer);
                            foreach (var id in brokerOrderIds)
                            {
                                await SaveBrokerDeliverySchedulesAsync(viewModel, userContext.Id, isBuyer, id, isNewVersion);
                            }

                            if (!isBuyer)
                            {
                                await SaveDeliverySchedulesInBrokerFRAsync(order);
                            }

                            await Context.CommitAsync();
                            transaction.Commit();

                            var messageDomain = new AppMessageDomain(this);
                            if (addedScheduleList.Any(t => t.StatusId == (int)DeliveryScheduleStatus.New))
                            {
                                await notificationDomain.AddNotificationEventAsync(EventType.DeliveryRequestCreated, order.OrderDeliverySchedules.Max(t => t.Id), userContext.Id);
                                var newSchedules = addedScheduleList.Where(t => t.StatusId == (int)DeliveryScheduleStatus.New);
                                await messageDomain.SendDeliveryScheduleAddedMessage(userContext, newSchedules, order);

                                var eventId = userContext.IsBuyerCompany || userContext.CompanySubTypeId == CompanyType.Buyer ? NewsfeedEvent.BuyerOrderDeliveryScheduleAdded : NewsfeedEvent.SupplierOrderDeliveryScheduleAdded;
                                await newsfeedDomain.SetDeliveryScheduleNewsfeed(userContext, order, eventId);
                            }
                            if (addedScheduleList.Any(t => t.StatusId == (int)DeliveryScheduleStatus.Modified))
                            {
                                await notificationDomain.AddNotificationEventAsync(EventType.DeliveryRequestUpdated, order.OrderDeliverySchedules.Max(t => t.Id), userContext.Id);
                                var modifiedSchedules = addedScheduleList.Where(t => t.StatusId == (int)DeliveryScheduleStatus.Modified);
                                await messageDomain.SendDeliveryScheduleModifiedMessage(userContext, schedules, modifiedSchedules, order);

                                var eventId = userContext.IsBuyerCompany || userContext.CompanySubTypeId == CompanyType.Buyer ? NewsfeedEvent.BuyerOrderDeliveryScheduleModified : NewsfeedEvent.SupplierOrderDeliveryScheduleModified;
                                await newsfeedDomain.SetDeliveryScheduleNewsfeed(userContext, order, eventId);
                            }
                            if (addedScheduleList.Any(t => t.StatusId == (int)DeliveryScheduleStatus.Rescheduled))
                            {
                                await notificationDomain.AddNotificationEventAsync(EventType.DeliveryRequestRescheduled, order.OrderDeliverySchedules.Max(t => t.Id), userContext.Id);
                                var rescheduledSchedules = addedScheduleList.Where(t => t.StatusId == (int)DeliveryScheduleStatus.Rescheduled);
                                await messageDomain.SendDeliveryScheduleRescheduledMessage(userContext, schedules, rescheduledSchedules, order);

                                var eventId = userContext.IsBuyerCompany || userContext.CompanySubTypeId == CompanyType.Buyer ? NewsfeedEvent.BuyerReschedulesSchedule : NewsfeedEvent.SupplierReschedulesSchedule;
                                await newsfeedDomain.SetDeliveryScheduleNewsfeed(userContext, order, eventId);

                                // set web notification for missed schedule reschedule. 
                                //var dispatchDomain = new DispatchDomain(newsfeedDomain);
                                //var rescheduleSchedule = rescheduledSchedules.First();
                                //NotificationDispatchLocationViewModel dispatchLocation = new NotificationDispatchLocationViewModel()
                                //{
                                //    DispatchNotificationType = DispatchNotificationType.Reschedule,
                                //    DeliveryScheduleId = rescheduleSchedule.Id,
                                //    TrackableScheduleId = rescheduleSchedule.DeliveryScheduleXTrackableSchedules.Any() ? rescheduleSchedule.DeliveryScheduleXTrackableSchedules.First().Id : 0
                                //};
                                //dispatchDomain.ProcessDispatchLocationForWebNotifications(dispatchLocation, order.Id, userContext.Id);

                            }
                            var currentScheduleGroups = viewModel.DeliverySchedules.Select(t => t.GroupId);
                            if (previousScheduleGroups.Any(t => !currentScheduleGroups.Contains(t)))
                            {
                                var eventId = userContext.IsBuyerCompany || userContext.CompanySubTypeId == CompanyType.Buyer ? NewsfeedEvent.BuyerOrderDeliveryScheduleRemoved : NewsfeedEvent.SupplierOrderDeliveryScheduleRemoved;
                                await newsfeedDomain.SetDeliveryScheduleNewsfeed(userContext, order, eventId);
                            }

                            var IsDeliveryIn24Hrs = IsDeliveryWithin24Hrs(addedScheduleList, order.Id, order.FuelRequest.Job.TimeZoneName);
                            if (IsDeliveryIn24Hrs)
                            {
                                Set24HoursWarning(userContext, response, addedScheduleList);
                            }
                            else
                            {
                                response.StatusCode = Status.Success;
                                response.StatusMessage = Resource.errMessageDeliverySchedulesSaveSuccess;
                            }
                        }
                        else if (isDriverModified)
                        {
                            UpdateFuelDispatchLocation(viewModel, schedules, order.FuelRequest.Currency);
                            await Context.CommitAsync();
                            transaction.Commit();

                            var currentSchedules = GetLatestOrderDeliverySchedule(order.OrderDeliverySchedules)
                                                    .Where(t => t.DeliveryRequestId.HasValue).Select(t => t.DeliverySchedule);
                            var driverModifiedSchedules = currentSchedules.Except(previousSchedules.Select(t1 => t1.DeliverySchedule)).Where(t => !trackableScheduleIds.Contains(t.Id));
                            if (driverModifiedSchedules.Any(t => t.StatusId == (int)DeliveryScheduleStatus.Reassigned))
                            {
                                await newsfeedDomain.SetDeliveryScheduleNewsfeed(userContext, order, NewsfeedEvent.SupplierDeliveryDriverReassigned);
                            }

                            bool isDriverAssigned = false, isDriverUnassigned = false;
                            foreach (var currentSchedule in currentSchedules)
                            {
                                var previousSchedule = previousSchedules.FirstOrDefault(t => t.DeliverySchedule.GroupId == currentSchedule.GroupId);
                                if (!previousSchedule.Drivers.Any(t => t.IsActive) && currentSchedule.DeliveryScheduleXDrivers.Any(t => t.IsActive))
                                {
                                    isDriverAssigned = true;
                                }
                                if (previousSchedule.Drivers.Any(t => t.IsActive) && !currentSchedule.DeliveryScheduleXDrivers.Any(t => t.IsActive))
                                {
                                    isDriverUnassigned = true;
                                }
                            }
                            if (isDriverAssigned)
                            {
                                await newsfeedDomain.SetDeliveryScheduleNewsfeed(userContext, order, NewsfeedEvent.SupplierDeliveryDriverAssigned);
                            }
                            if (isDriverUnassigned)
                            {
                                await newsfeedDomain.SetDeliveryScheduleNewsfeed(userContext, order, NewsfeedEvent.SupplierDeliveryDriverUnassigned);
                            }

                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.errMessageDeliverySchedulesSaveSuccess;
                        }
                        else if (isScheduleModified)
                        {
                            UpdateFuelDispatchLocation(viewModel, schedules, order.FuelRequest.Currency);
                            await Context.CommitAsync();
                            transaction.Commit();

                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.successMessageDeliveryScheduleSuccessfullyUpdated;
                        }
                        else
                        {
                            bool isLocationAdded = UpdateFuelDispatchLocation(viewModel, schedules, order.FuelRequest.Currency);
                            if (isLocationAdded)
                            {
                                await Context.CommitAsync();
                                transaction.Commit();
                                response.StatusCode = Status.Success;
                                response.StatusMessage = Resource.errMessageDeliverySchedulesSaveSuccess;
                            }
                            else
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = Resource.errMessageDeliverySchedulesNoNewChangesSaveFailed;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageSaveDeliverySchedulesFailed;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("OrderDomain", "SaveDeliverySchedulesAsync", ex.Message, ex);
                }
            }

            return response;
        }

        private bool UpdateFuelDispatchLocation(OrderDetailsViewModel viewModel, List<DeliverySchedule> currentSchedules, Currency currency)
        {
            bool isLocationChanged = false;
            var scheduleIds = currentSchedules.Select(t => t.Id).ToList();
            var dispatchLocations = Context.DataContext.FuelDispatchLocations.Where(t => t.OrderId == viewModel.Id && scheduleIds.Contains(t.DeliveryScheduleId ?? 0)).ToList();
            foreach (var schedule in currentSchedules)
            {
                foreach (var location in schedule.FuelDispatchLocations)
                {
                    location.CreatedBy = viewModel.UpdatedBy;
                    location.OrderId = viewModel.Id;
                    location.DeliveryScheduleId = schedule.Id;
                    location.Currency = currency;
                    if (location.Latitude == 0 && location.Longitude == 0)
                    {
                        UpdateFuelDispatchLocationLatLong(location);
                    }
                    else if (location.Address == null)
                    {
                        SetSplitLoadAddressByLatLong(location);
                    }
                    if (location.Id > 0)
                    {
                        var locationEntity = dispatchLocations.FirstOrDefault(t => t.Id == location.Id && t.DeliveryScheduleId == schedule.Id);
                        if (locationEntity == null)
                        {
                            Context.DataContext.FuelDispatchLocations.Add(location);
                            isLocationChanged = true;
                        }
                    }
                    else
                    {
                        Context.DataContext.FuelDispatchLocations.Add(location);
                        isLocationChanged = true;
                    }
                }
            }
            var currentLocations = currentSchedules.SelectMany(t => t.FuelDispatchLocations.Select(t1 => t1.Id)).ToList();
            var deletedLocations = dispatchLocations.Where(t => !currentLocations.Contains(t.Id) && (t.TrackableScheduleId == null || t.IsSkipped) && t.LocationType == (int)LocationType.Drop).ToList();
            if (deletedLocations.Any())
            {
                Context.DataContext.FuelDispatchLocations.RemoveRange(deletedLocations);
                isLocationChanged = true;
            }
            return isLocationChanged;
        }


        private async Task<bool> ProcessScheduleCarriers(OrderDetailsViewModel viewModel, List<DeliverySchedule> previousSchedules)
        {
            bool isScheduleModified = false;

            var dispatchDomain = new DispatchDomain(this);
            foreach (var item in viewModel.DeliverySchedules)
            {
                if (!string.IsNullOrWhiteSpace(item.Carrier.Name))
                {
                    item.Carrier = await dispatchDomain.AddCarrierIfNotExists(item.Carrier.Name, item.Carrier.CreatedBy, item.Carrier.CompanyId);
                }
                if (!string.IsNullOrWhiteSpace(item.SupplierSource.Name))
                {
                    item.SupplierSource = await dispatchDomain.AddSupplierSourceIfNotExists(item.SupplierSource, item.Carrier.CreatedBy, item.Carrier.CompanyId);
                }

                var existingSchedule = previousSchedules.SingleOrDefault(t => t.Id == item.Id);
                if (existingSchedule != null
                    && ((existingSchedule.CarrierId ?? 0) != item.Carrier.Id
                        || existingSchedule.SupplierSourceId != item.SupplierSource.Id
                        || existingSchedule.SupplierContract != item.SupplierSource.ContractNumber
                        || existingSchedule.LoadCode != item.LoadCode))
                {
                    existingSchedule.CarrierId = item.Carrier.Id;
                    existingSchedule.SupplierSourceId = item.SupplierSource.Id;
                    existingSchedule.SupplierContract = item.SupplierSource.ContractNumber;
                    existingSchedule.LoadCode = item.LoadCode;

                    foreach (var trackableSchedule in existingSchedule.DeliveryScheduleXTrackableSchedules)
                    {
                        trackableSchedule.CarrierId = item.Carrier.Id;
                        trackableSchedule.SupplierSourceId = item.SupplierSource.Id;
                        trackableSchedule.SupplierContract = item.SupplierSource.ContractNumber;
                        trackableSchedule.LoadCode = item.LoadCode;
                    }
                    Context.DataContext.Entry(existingSchedule).State = EntityState.Modified;
                    isScheduleModified = true;
                }

                if (existingSchedule != null && existingSchedule.QuantityTypeId != (int)item.ScheduleQuantityType)
                {
                    existingSchedule.QuantityTypeId = (int)item.ScheduleQuantityType;

                    if (existingSchedule.DeliveryScheduleXTrackableSchedules != null)
                    {
                        existingSchedule.DeliveryScheduleXTrackableSchedules.ToList().ForEach(t => t.QuantityTypeId = (int)item.ScheduleQuantityType);
                    }

                    Context.DataContext.Entry(existingSchedule).State = EntityState.Modified;
                    isScheduleModified = true;
                }
            }

            return isScheduleModified;
        }

        public async Task<bool> SaveDeliverySchedulesInBrokerFRAsync(Order order)
        {
            bool response = false;
            try
            {
                var brokeredFuelRequest = order.FuelRequest.FuelRequests1.OrderByDescending(t => t.Id).FirstOrDefault();
                if (brokeredFuelRequest != null && brokeredFuelRequest.FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest
                        && brokeredFuelRequest.FuelRequestXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)FuelRequestStatus.Open
                        && brokeredFuelRequest.FuelRequestDetail.DeliveryTypeId == (int)DeliveryType.MultipleDeliveries
                        && order.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open)
                {
                    brokeredFuelRequest.DeliverySchedules.Clear();
                    await Context.CommitAsync();

                    var orderDeliverySchedules = GetLatestOrderDeliverySchedule(order.OrderDeliverySchedules).Where(t => t.DeliveryRequestId.HasValue)
                                                .Select(t => t.DeliverySchedule).ToList();
                    if (orderDeliverySchedules.Count > 0)
                    {
                        brokeredFuelRequest.DeliverySchedules = orderDeliverySchedules;
                    }

                    brokeredFuelRequest.DeliverySchedules = brokeredFuelRequest.DeliverySchedules.Where(t => !(t.Type == (int)DeliveryScheduleType.SpecificDates
                                                                                                && t.DeliveryScheduleXTrackableSchedules.Any(t1 => t1.IsDropped))).ToList();
                    await Context.CommitAsync();
                }

                response = true;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "SaveDeliverySchedulesInBrokerFRAsync", ex.Message, ex);
            }
            return response;
        }

        private async Task<List<int>> GetBrokerOrderIdAsync(int orderId, bool isBuyer, List<int> brokerOrderIds = null)
        {
            try
            {
                if (brokerOrderIds == null)
                {
                    brokerOrderIds = new List<int>();
                }

                var order = await Context.DataContext.Orders.Include(t => t.FuelRequest).SingleOrDefaultAsync(t => t.Id == orderId);
                if (order != null)
                {
                    if (isBuyer)
                    {
                        if (order.FuelRequest.FuelRequests1.Count > 0)
                        {
                            foreach (var childRequest in order.FuelRequest.FuelRequests1)
                            {
                                if (childRequest.FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest && childRequest.GetFuelRequestFirstOrder() != null)
                                {
                                    int brokeredOrderId = childRequest.GetFuelRequestFirstOrder().Id;
                                    brokerOrderIds.Add(brokeredOrderId);
                                    await GetBrokerOrderIdAsync(brokeredOrderId, isBuyer, brokerOrderIds);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (order.FuelRequest.FuelRequest1 != null && order.FuelRequest.GetParentFuelRequest().FuelRequest1 != null)
                        {
                            var childRequest = order.FuelRequest.GetParentFuelRequest().FuelRequest1;
                            if (childRequest.Orders.Count > 0)
                            {
                                int brokeredOrderId = childRequest.Orders.FirstOrDefault().Id;
                                brokerOrderIds.Add(brokeredOrderId);
                                await GetBrokerOrderIdAsync(brokeredOrderId, isBuyer, brokerOrderIds);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetBrokerOrderIdAsync", ex.Message, ex);
            }
            return brokerOrderIds;
        }

        public async Task<bool> SaveBrokerDeliverySchedulesAsync(OrderDetailsViewModel viewModel, int userId, bool isBuyer, int brokeredOrderId, bool isNewVersion)
        {
            bool response = false;
            try
            {
                var brokerOrder = await Context.DataContext.Orders.Include(t => t.OrderDeliverySchedules)
                                                                    .Include(t => t.FuelRequest)
                                                                    .Include(t => t.DeliveryScheduleXTrackableSchedules)
                                                                    .SingleOrDefaultAsync(t => t.Id == brokeredOrderId);
                if (brokerOrder != null)
                {
                    var orderSchedules = GetLatestOrderDeliverySchedule(brokerOrder.OrderDeliverySchedules);

                    var schedules = await GetBrokeredDeliverySchedule(viewModel.Id); //all schedules
                    var removedList = new List<DeliverySchedule>(); // removed schedules       

                    if (orderSchedules.Count > 0)
                    {
                        var currentSchedules = orderSchedules.Where(t => t.DeliveryRequestId.HasValue).Select(t => t.DeliverySchedule);
                        foreach (var request in currentSchedules)
                        {
                            bool isRemoved = false;
                            var startTime = Convert.ToDateTime(request.StartTime.ToString()).ToShortTimeString();
                            var endTime = Convert.ToDateTime(request.EndTime.ToString()).ToShortTimeString();
                            if (request.Type == (int)DeliveryScheduleType.Weekly || request.Type == (int)DeliveryScheduleType.BiWeekly)
                            {
                                if (!viewModel.DeliverySchedules.Any(t => t.GroupId == request.GroupId
                                                                            && t.ScheduleDays.Contains(request.WeekDayId)
                                                                            && t.ScheduleStartTime == startTime
                                                                            && t.ScheduleEndTime == endTime
                                                                            && t.ScheduleQuantity == request.Quantity))
                                {
                                    isRemoved = true;
                                }
                            }
                            else if (!viewModel.DeliverySchedules.Any(t => t.Id == request.Id
                                                                            && t.ScheduleDate == request.Date
                                                                            && t.ScheduleStartTime == startTime
                                                                            && t.ScheduleEndTime == endTime
                                                                            && t.ScheduleQuantity == request.Quantity))
                            {
                                isRemoved = true;
                            }
                            if (isRemoved)
                            {
                                removedList.Add(request);
                            }
                        }
                    }

                    if (isNewVersion)
                    {



                        var brokerOrderDS = GetLatestOrderDeliverySchedule(brokerOrder.OrderDeliverySchedules);
                        if (brokerOrderDS != null && brokerOrderDS.Count > 0)
                        {
                            brokerOrderDS.ForEach(t => t.IsActive = false);
                        }

                        var latestVersionId = brokerOrderDS.FirstOrDefault().Version;
                        foreach (var item in schedules)
                        {
                            var ordertempSchedule = GetOrderDeliverySchedule(brokeredOrderId, latestVersionId, userId, item.Id);
                            if (viewModel.CurrentOrderVersionToEdit != null)
                            {
                                ordertempSchedule.AdditionalNotes = viewModel.CurrentOrderVersionToEdit.AdditionalNotes;
                            }
                            brokerOrder.OrderDeliverySchedules.Add(ordertempSchedule);
                        }

                        await Context.CommitAsync();

                        TrackableScheduleDomain trackableScheduleDomain = new TrackableScheduleDomain(this);
                        await trackableScheduleDomain.ProcessTrackableSchedules(schedules, brokerOrder, removedList);
                        await Context.CommitAsync();
                        response = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "SaveBrokerDeliverySchedulesAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DeliverySchedule>> GetBrokeredDeliverySchedule(int orderId)
        {
            List<DeliverySchedule> response = new List<DeliverySchedule>();

            try
            {
                var order = await Context.DataContext.Orders.Include(t => t.OrderDeliverySchedules)
                    .Include("OrderDeliverySchedules.DeliverySchedule").SingleOrDefaultAsync(t => t.Id == orderId);
                if (order != null)
                {
                    // do not take declined and cancelled schedule suggestions to edit
                    var orderSchedule = GetLatestOrderDeliverySchedule(order.OrderDeliverySchedules);
                    orderSchedule.ForEach(t => response.Add(t.DeliverySchedule));

                    return response;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetBrokeredDeliverySchedule", ex.Message, ex);
            }

            return response;
        }

        private bool ProcessDeliverySchedulesAsync(List<DeliverySchedule> schedules, List<DeliverySchedule> addedScheduleList, DateTimeOffset date,
            OrderDetailsViewModel viewModel, List<OrderVersionXDeliverySchedule> orderDeliverySchedules, int userId, Currency currency, ref bool isDriverModified)
        {
            bool response = false;
            try
            {
                HelperDomain helperDomain = new HelperDomain(this);
                int latestGroupNumber = 0;
                bool driverAssigned = false;
                if (Context.DataContext.DeliverySchedules.Any())
                {
                    latestGroupNumber = Context.DataContext.DeliverySchedules.Max(t => t.GroupId);
                }
                foreach (var schedule in viewModel.DeliverySchedules)
                {
                    ++latestGroupNumber;
                    if (schedule.ScheduleType == (int)DeliveryScheduleType.Weekly || schedule.ScheduleType == (int)DeliveryScheduleType.BiWeekly)
                    {
                        GetSchedulesByWeekDay(schedules, addedScheduleList, date, viewModel, orderDeliverySchedules, userId, ref isDriverModified, ref response, helperDomain, latestGroupNumber, ref driverAssigned, schedule);
                    }
                    else
                    {
                        GetSchedulesByDate(schedules, addedScheduleList, viewModel, userId, ref isDriverModified, ref response, helperDomain, latestGroupNumber, ref driverAssigned, schedule);
                    }
                }
                foreach (var schedule in addedScheduleList)
                {
                    foreach (var location in schedule.FuelDispatchLocations)
                    {
                        location.CreatedBy = viewModel.UpdatedBy;
                        location.OrderId = viewModel.Id;
                        location.Currency = currency;
                        if (location.Latitude == 0 && location.Longitude == 0)
                        {
                            UpdateFuelDispatchLocationLatLong(location);
                        }
                        else if (location.Address == null)
                        {
                            SetSplitLoadAddressByLatLong(location);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "ProcessDeliverSchedulesAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<TBDDropdownDisplayItem>> GetOtherProductsOfSupplier(int companyId)
        {

            return await Context.DataContext.Orders.Where(t => t.AcceptedCompanyId == companyId && t.FuelRequest.MstProduct.ProductTypeId == (int)ProductTypes.NonStandardFuel
                                                               && t.OrderXStatuses.Any(t1 => t1.StatusId == (int)OrderStatus.Open && t1.IsActive))
                                                    .Select(t => new TBDDropdownDisplayItem() { Id = t.FuelRequest.FuelTypeId, Name = t.FuelRequest.MstProduct.Name, ProductTypeId = t.FuelRequest.MstProduct.ProductTypeId, ProductTypeName = t.FuelRequest.MstProduct.MstProductType.Name }).Distinct().ToListAsync();
        }

        public void SetSplitLoadAddressByLatLong(FuelDispatchLocation location)
        {
            var point = GoogleApiDomain.GetAddress(Convert.ToDouble(location.Latitude), Convert.ToDouble(location.Longitude));
            if (point != null)
            {
                location.Address = point.Address;
                location.City = point.City;
                location.ZipCode = point.ZipCode;
                location.CountyName = point.CountyName;
                location.StateCode = point.StateCode;
                location.StateId = Context.DataContext.MstStates.Single(t => t.Code.ToLower() == point.StateCode.ToLower()).Id;
                var country = Context.DataContext.MstCountries.Single(t => t.Name.ToLower().Contains(point.CountryName.ToLower()));
                location.CountryCode = country != null ? country.Code : string.Empty;
            }
        }

        public void UpdateFuelDispatchLocationLatLong(FuelDispatchLocation location)
        {
            var point = GoogleApiDomain.GetGeocode($"{location.Address} {location.City} {location.StateCode} {location.CountryCode} {location.ZipCode}");
            if (point != null)
            {
                location.Latitude = Convert.ToDecimal(point.Latitude);
                location.Longitude = Convert.ToDecimal(point.Longitude);
                location.CountyName = point.CountyName;
                string timeZoneName = GoogleApiDomain.GetTimeZone(location.Latitude, location.Longitude);
                if (!string.IsNullOrEmpty(timeZoneName))
                {
                    location.TimeZoneName = timeZoneName;
                }
            }
        }

        private void GetSchedulesByDate(List<DeliverySchedule> schedules, List<DeliverySchedule> addedScheduleList, OrderDetailsViewModel viewModel, int userId, ref bool isDriverModified, ref bool response, HelperDomain helperDomain, int latestGroupNumber, ref bool driverAssigned, DeliveryScheduleViewModel schedule)
        {
            schedule.ScheduleDay = helperDomain.GetWeekDayId(schedule.ScheduleDate);

            if (schedule.RescheduledTrackableId.HasValue && schedule.RescheduledTrackableId.Value > 0)
            {
                var trackableSchedule = Context.DataContext.DeliveryScheduleXTrackableSchedules.SingleOrDefault(t => t.Id == schedule.RescheduledTrackableId.Value);
                if (trackableSchedule != null && trackableSchedule.IsActive)
                {
                    trackableSchedule.IsActive = false;
                    trackableSchedule.DeliveryScheduleStatusId = (int)TrackableDeliveryScheduleStatus.MissedAndRescheduled;
                    Context.DataContext.Entry(trackableSchedule).State = EntityState.Modified;
                    Context.Commit();
                }
                schedule.StatusId = (int)TrackableDeliveryScheduleStatus.Rescheduled;
            }

            var otherTypeSchedule = schedule.ToEntity();
            if (schedule.Id > 0)
            {
                var entity = Context.DataContext.DeliverySchedules.Where(t => t.Id == schedule.Id).ToList();
                if (entity != null && entity.Count > 0)
                {
                    var entityModel = entity.ToViewModel();
                    if (!Convert.ToBoolean(entityModel.CompareTo(schedule)))
                    {
                        otherTypeSchedule.Id = 0;
                        otherTypeSchedule.GroupId = schedule.GroupId > 0 ? schedule.GroupId : latestGroupNumber;
                        otherTypeSchedule.StatusId = (int)DeliveryScheduleStatus.Modified;
                        addedScheduleList.Add(otherTypeSchedule);
                        response = true;
                    }
                    schedules.Add(otherTypeSchedule);
                }
            }
            else
            {
                otherTypeSchedule.GroupId = latestGroupNumber;
                addedScheduleList.Add(otherTypeSchedule);
                schedules.Add(otherTypeSchedule);
                response = true;
            }
            if (userId > 0)
            {
                driverAssigned = helperDomain.AssignDeliveryLevelDriver(otherTypeSchedule, userId, schedule.DriverId, viewModel.Id, otherTypeSchedule.Id > 0);
                if (driverAssigned)
                {
                    isDriverModified = true;
                }
            }
        }

        private static void GetSchedulesByWeekDay(List<DeliverySchedule> schedules, List<DeliverySchedule> addedScheduleList, DateTimeOffset date, OrderDetailsViewModel viewModel, List<OrderVersionXDeliverySchedule> orderDeliverySchedules, int userId, ref bool isDriverModified, ref bool response, HelperDomain helperDomain, int latestGroupNumber, ref bool driverAssigned, DeliveryScheduleViewModel schedule)
        {
            foreach (var day in schedule.ScheduleDays)
            {
                schedule.ScheduleDay = day;
                int daysToAdd = (schedule.ScheduleDay - (int)date.DayOfWeek + 7) % 7;
                if (daysToAdd == 0 && Convert.ToDateTime(schedule.ScheduleEndTime).TimeOfDay < date.DateTime.TimeOfDay)
                {
                    daysToAdd = 7;
                }
                schedule.ScheduleDate = date.AddDays(daysToAdd);

                var daySchedule = schedule.ToEntity();
                if (schedule.Id > 0)//updated schedule
                {
                    var entity = orderDeliverySchedules.Where(t => t.DeliveryRequestId.HasValue && t.DeliverySchedule.GroupId == schedule.GroupId)
                                                                                .Select(t => t.DeliverySchedule).OrderBy(t => t.Id).ToList();
                    if (entity != null && entity.Count > 0)
                    {
                        var entityModel = entity.ToViewModel();
                        if (!Convert.ToBoolean(entityModel.CompareTo(schedule)))
                        {
                            daySchedule.Id = 0;
                            daySchedule.GroupId = schedule.GroupId > 0 ? schedule.GroupId : latestGroupNumber;
                            daySchedule.StatusId = (int)DeliveryScheduleStatus.Modified;
                            addedScheduleList.Add(daySchedule);
                            schedules.Add(daySchedule);
                            response = true;
                        }
                        else
                        {
                            if (schedules.Any(t => t.Id == daySchedule.Id))
                            {
                                var existingSchedule = entity.FirstOrDefault(t => t.WeekDayId == daySchedule.WeekDayId);
                                if (existingSchedule != null)
                                {
                                    UpdateLocationId(daySchedule, existingSchedule);
                                    daySchedule.Id = existingSchedule.Id;
                                }
                            }
                            schedules.Add(daySchedule);
                        }
                    }
                }
                else
                {
                    daySchedule.GroupId = latestGroupNumber;
                    addedScheduleList.Add(daySchedule);
                    schedules.Add(daySchedule);
                    response = true;
                }
                if (userId > 0)
                {
                    driverAssigned = helperDomain.AssignDeliveryLevelDriver(daySchedule, userId, schedule.DriverId, viewModel.Id, daySchedule.Id > 0);
                    if (driverAssigned)
                    {
                        isDriverModified = true;
                    }
                }
            }
        }

        private static void UpdateLocationId(DeliverySchedule daySchedule, DeliverySchedule existingSchedule)
        {
            var dispatchLocations = existingSchedule.FuelDispatchLocations.ToList();
            foreach (var location in daySchedule.FuelDispatchLocations)
            {
                if (location.Id > 0)
                {
                    location.Id = dispatchLocations.Where(t => t.Address == location.Address && t.City == location.City && t.StateId == location.StateId && t.ZipCode == location.ZipCode && t.CountyName == location.CountyName && !daySchedule.FuelDispatchLocations.Any(t1 => t1.Id == t.Id)).Select(t => t.Id).FirstOrDefault();
                }
            }
        }

        public async Task<List<int>> GetDeliveryRequestIdsAync()
        {
            var response = new List<int>();
            try
            {
                var nextDay = DateTimeOffset.Now.Date.AddDays(1);
                var orders = Context.DataContext.OrderVersionXDeliverySchedules.Include(t => t.Order)
                                             .Where(t => t.IsActive && t.Order.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open)
                                             .Select(t => t.OrderId);

                response = await Context.DataContext.OrderVersionXDeliverySchedules
                                                    .Include(t => t.DeliverySchedule)
                                                    .Where(t => t.IsActive && orders.Contains(t.OrderId) && t.DeliveryRequestId.HasValue &&
                                                           ((t.DeliverySchedule.Type == (int)DeliveryScheduleType.SpecificDates &&
                                                           DbFunctions.TruncateTime(t.DeliverySchedule.Date) == nextDay) ||
                                                           t.DeliverySchedule.Type != (int)DeliveryScheduleType.SpecificDates))
                                                    .Select(t => t.DeliveryRequestId.Value)
                                                    .Distinct()
                                                    .ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetDeliveryRequestIdsAync", ex.Message, ex);
            }
            return response;
        }

        public async Task ProcessDeliveryScheduleReminderAsync(int id)
        {
            using (var tracer = new Tracer("OrderDomain", "ProcessDeliveryScheduleReminderAsync"))
            {
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        bool isAdd = false;
                        var orderSchedules = Context.DataContext.OrderVersionXDeliverySchedules.Where(t => t.DeliveryRequestId == id && t.IsActive);
                        if (orderSchedules.Count() > 0)
                        {
                            var orderSchedule = orderSchedules.OrderByDescending(t => t.OrderId).FirstOrDefault();
                            Order order = orderSchedule.Order;
                            DateTimeOffset maxDate = order.FuelRequest.FuelRequestDetail.EndDate ?? (order.FuelRequest.Job.EndDate ?? DateTimeOffset.Now.Date.AddDays(1));
                            DateTimeOffset nextDay = DateTimeOffset.Now.Date.AddDays(1);
                            if (maxDate.Date >= nextDay.Date)
                            {
                                int percentThreshold = order.FuelRequest.OrderClosingThreshold ?? 100;
                                decimal orderAmount = (percentThreshold * (order.BrokeredMaxQuantity ?? order.FuelRequest.MaxQuantity)) / 100;

                                decimal droppedGallons = order.FuelRequest.HedgeDroppedGallons + order.FuelRequest.SpotDroppedGallons;
                                decimal todayDrops = 0;
                                var schedules = orderSchedule.Order.OrderDeliverySchedules.Where(x => x.DeliveryRequestId.HasValue && x.IsActive).Select(t => t.DeliverySchedule);
                                var currentSchedule = Context.DataContext.DeliverySchedules.SingleOrDefault(t => t.Id == id);
                                foreach (var schedule in schedules)
                                {
                                    schedule.Date = schedule.Date.Add(schedule.StartTime);
                                    if (schedule.Date >= DateTimeOffset.Now && schedule.Date.Date == DateTimeOffset.Now.Date)
                                    {
                                        todayDrops += schedule.Quantity;
                                    }
                                    else if (schedule.Type == (int)DeliveryScheduleType.Monthly)
                                    {
                                        var nextDate = GetNextDateForTotalDrops(schedule.Date, 30);
                                        if (nextDate.Date >= DateTimeOffset.Now && nextDate.Date == DateTimeOffset.Now.Date)
                                        {
                                            todayDrops += schedule.Quantity;
                                        }
                                    }
                                    else if (schedule.WeekDayId == ((int)DateTimeOffset.Now.DayOfWeek))
                                    {
                                        var nextDate = schedule.Type == (int)DeliveryScheduleType.BiWeekly ? GetNextDateForTotalDrops(schedule.Date, 14) : GetNextDateForTotalDrops(schedule.Date, 7);
                                        if (nextDate.Date >= DateTimeOffset.Now && nextDate.Date == DateTimeOffset.Now.Date)
                                        {
                                            todayDrops += schedule.Quantity;
                                        }
                                    }
                                }

                                if ((droppedGallons + todayDrops) < orderAmount)
                                {
                                    HelperDomain helperDomain = new HelperDomain(this);
                                    DeliverySchedule schedule = currentSchedule;
                                    DateTimeOffset estDate = schedule.Date.Date;

                                    if (estDate == nextDay)
                                    {
                                        isAdd = true;
                                    }
                                    else if (schedule.Type == (int)DeliveryScheduleType.Monthly)
                                    {
                                        var nextDate = helperDomain.GetNextDate(schedule.Date, 30);
                                        if (nextDate.Date == nextDay.Date)
                                        {
                                            isAdd = true;
                                        }
                                    }
                                    else if (schedule.WeekDayId == ((int)DateTimeOffset.Now.DayOfWeek + 1))
                                    {
                                        var nextDate = schedule.Type == (int)DeliveryScheduleType.BiWeekly ?
                                                                                    helperDomain.GetNextDate(schedule.Date, 14) :
                                                                                    helperDomain.GetNextDate(schedule.Date, 7);
                                        if (nextDate.Date == nextDay.Date)
                                        {
                                            isAdd = true;
                                        }
                                    }

                                    if (isAdd)
                                    {
                                        NotificationDomain notificationDomain = new NotificationDomain(this);
                                        await notificationDomain.AddNotificationEventAsync(EventType.DeliveryRequestReminder, id, order.User.Id);
                                        transaction.Commit();
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        LogManager.Logger.WriteException("OrderDomain", "ProcessDeliveryScheduleReminderAsync", ex.Message, ex);
                    }
                }
            }
        }

        public async Task<List<int>> GetOpenOrdersAync()
        {
            using (var tracer = new Tracer("OrderDomain", "GetOpenOrdersAync"))
            {
                List<int> openOrders = new List<int>();
                try
                {
                    StoredProcedureDomain storedProcedureDomain = new StoredProcedureDomain(this);
                    openOrders = await storedProcedureDomain.GetOpenOrdersHavingDeliverySchedule();
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderDomain", "GetOpenOrdersAync", ex.Message, ex);
                }
                return openOrders;
            }
        }

        public async Task<List<UspCarrierCustomerMapping>> GetAllCarrierCustomerData(UserContext userContext)
        {
            using (var tracer = new Tracer("OrderDomain", "GetAllCarrierCustomerData"))
            {
                List<UspCarrierCustomerMapping> carrierCustomers = new List<UspCarrierCustomerMapping>();
                try
                {
                    StoredProcedureDomain storedProcedureDomain = new StoredProcedureDomain(this);
                    carrierCustomers = await storedProcedureDomain.GetCarrierCustomerMapping(userContext.CompanyId);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderDomain", "GetAllCarrierCustomerData", ex.Message, ex);
                }
                return carrierCustomers;
            }
        }

        public async Task<List<DropdownDisplayItem>> GetBillingAddress(int companyId)
        {
            List<DropdownDisplayItem> dropdownDisplayItems = new List<DropdownDisplayItem>();
            try
            {
                dropdownDisplayItems = await (from u in Context.DataContext.BillingAddresses.Where(t => t.CompanyId == companyId && t.IsActive).OrderByDescending(t => t.IsDefault)
                                              select new DropdownDisplayItem
                                              {
                                                  Id = u.Id,
                                                  Name = u.Name
                                              }).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ThirdPartyOrderDomain", "GetBillingAddress", ex.Message, ex);
            }
            return dropdownDisplayItems;
        }

        public async Task<StatusViewModel> SaveCarrierCustomerMapping(UspCarrierCustomerMapping customerMapping, UserContext userContext)
        {
            StatusViewModel response = new StatusViewModel();
            using (var tracer = new Tracer("OrderDomain", "SaveCarrierCustomerMapping"))
            {
                CarrierCustomerMapping carrierCustomer = new CarrierCustomerMapping();

                if (customerMapping != null)
                {
                    carrierCustomer.CarrierCustomerId = customerMapping.BuyerCompanyId;
                    carrierCustomer.CarrierCompanyId = userContext.CompanyId;
                    carrierCustomer.CarrierAssignedCustomerId = string.IsNullOrEmpty(customerMapping.CarrierAssignedCustomerId) ? null : customerMapping.CarrierAssignedCustomerId.Trim();
                    carrierCustomer.CreatedBy = userContext.Id;
                    carrierCustomer.IsActive = true;
                    carrierCustomer.CreatedDate = DateTime.Now;

                }
                try
                {
                    if (customerMapping.Id != null)
                    {
                        var objCustomerMapping = Context.DataContext.CarrierCustomerMappings.FirstOrDefault(t => t.Id == customerMapping.Id.Value && t.IsActive == true);
                        objCustomerMapping.CarrierAssignedCustomerId = carrierCustomer.CarrierAssignedCustomerId;
                        response.StatusMessage = Resource.msgMyCustomerIdUpdate;
                    }
                    else
                    {
                        Context.DataContext.CarrierCustomerMappings.Add(carrierCustomer);
                        response.StatusMessage = Resource.msgMyCustomerIdSave;
                    }

                    response.StatusCode = Status.Success;


                    await Context.CommitAsync();
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderDomain", "SaveCarrierCustomerMapping", ex.Message, ex);
                }
                return response;
            }
        }


        public StatusViewModel CheckDuplicateCustomerId(UspCarrierCustomerMapping customerDetail, UserContext userContext)
        {
            StatusViewModel response = new StatusViewModel();
            using (var tracer = new Tracer("OrderDomain", "CheckDuplicateCustomerId"))
            {
                try
                {


                    if (!string.IsNullOrEmpty(customerDetail.CarrierAssignedCustomerId))
                    {
                        var objCustomerMapping = Context.DataContext.CarrierCustomerMappings
                                        .FirstOrDefault(t => t.CarrierAssignedCustomerId.ToLower().Trim() == customerDetail.CarrierAssignedCustomerId.ToLower().Trim()
                                                            && t.CarrierCompanyId == userContext.CompanyId && t.IsActive == true);

                        if (objCustomerMapping != null && objCustomerMapping.Id != customerDetail.Id)
                        {
                            response.StatusCode = Status.Warning;
                            response.StatusMessage = Resource.warningMyCustomerIdExist;
                        }
                        else { response.StatusCode = Status.Success; }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderDomain", "CheckDuplicateCustomerId", ex.Message, ex);
                }
                return response;
            }
        }


        public async Task<bool> ProcessDeliverySchedulesAsync(int orderId)
        {
            StoredProcedureDomain storedProcedureDomain = new StoredProcedureDomain(this);
            var order = await storedProcedureDomain.GetInfoToCreateTrackableSchedules(orderId);

            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var schedules = new List<int>();
                    if (order.ActiveOrderScheduleVersion > 0)
                    {
                        schedules = Context.DataContext.OrderVersionXDeliverySchedules.Where(t => t.IsActive && t.OrderId == order.OrderId && t.Version == order.ActiveOrderScheduleVersion && t.DeliveryRequestId.HasValue && t.DeliverySchedule.StatusId != (int)DeliveryScheduleStatus.Canceled && t.DeliverySchedule.DeliveryGroupId == null && t.DeliveryRequestId > 0).Select(t => t.DeliveryRequestId.Value).ToList();
                    }

                    TrackableScheduleDomain trackableScheduleDomain = new TrackableScheduleDomain(this);
                    var jobTime = DateTimeOffset.Now.ToTargetDateTimeOffset(order.TimeZoneName);
                    DateTime maxDate = jobTime.Date.AddDays(ApplicationConstants.FutureSchedulesAvailableFor);

                    var orderAmount = ((order.OrderClosingThreshold ?? 100) * (order.OrderMaxQuantity ?? order.FuelRequestMaxQuantity)) / 100;
                    order.RemainingQuantity = orderAmount - (order.DroppedQuantity + order.ExistingScheduleQuantity);
                    var schedulesToAdd = trackableScheduleDomain.GetSchedulesForAMonth(schedules, maxDate, order);
                    foreach (var schedule in schedulesToAdd)
                    {
                        var trackableSchedule = new DeliveryScheduleXTrackableSchedule();
                        trackableSchedule.Date = schedule.Date.Date;
                        trackableSchedule.ShiftStartDate = schedule.Date.Date;
                        trackableSchedule.StartTime = schedule.StartTime;
                        trackableSchedule.EndTime = schedule.EndTime;
                        trackableSchedule.Quantity = schedule.Quantity;
                        trackableSchedule.IsActive = true;
                        trackableSchedule.UoM = schedule.UoM;
                        trackableSchedule.OrderId = orderId;
                        trackableSchedule.DeliveryScheduleId = schedule.Id;
                        trackableSchedule.CarrierId = schedule.CarrierId;
                        trackableSchedule.SupplierSourceId = schedule.SupplierSourceId;
                        trackableSchedule.SupplierContract = schedule.SupplierContract;
                        trackableSchedule.LoadCode = schedule.LoadCode;
                        trackableSchedule.DeliveryScheduleStatusId = schedule.StatusId == (int)TrackableDeliveryScheduleStatus.Rescheduled ? (int)TrackableDeliveryScheduleStatus.Rescheduled : (int)TrackableDeliveryScheduleStatus.Accepted;
                        trackableSchedule.DriverId = schedule.DeliveryScheduleXDrivers.Any(t => t.IsActive) ? schedule.DeliveryScheduleXDrivers.First(t => t.IsActive).DriverId : (int?)null;
                        Context.DataContext.DeliveryScheduleXTrackableSchedules.Add(trackableSchedule);
                    }

                    await Context.CommitAsync();
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("OrderDomain", "ProcessDeliverySchedulesAsync:OrderId" + orderId, ex.Message, ex);
                }
                return false;
            }
        }

        public async Task<bool> ProcessDeliveryScheduleStatusAsync(UpdateScheduleStatusInputModel schedule, int waitingPeriod)
        {
            try
            {
                var isScheduleMissed = false;
                var delReqStatusUpdate = new List<DeliveryReqStatusUpdateModel>();
                List<ScheduleNotificationModel> notificationModel = new List<ScheduleNotificationModel>();

                var jobCurrentTime = DateTimeOffset.UtcNow;
                if (!string.IsNullOrWhiteSpace(schedule.TimeZoneName))
                {
                    jobCurrentTime = DateTimeOffset.Now.ToTargetDateTimeOffset(schedule.TimeZoneName);
                }
                else
                {
                    waitingPeriod = 2 * waitingPeriod;
                }

                var scheduleStartDate = schedule.Date.Add(schedule.StartTime);
                var scheduleEndDate = schedule.Date.Add(schedule.EndTime);
                var shiftEndDate = schedule.ShiftEndDateTime;
                if (shiftEndDate.HasValue)
                {
                    if (shiftEndDate.Value.AddHours(waitingPeriod) < jobCurrentTime.DateTime)
                    {
                        isScheduleMissed = true;
                    }
                }
                else
                {
                    if (schedule.Date.Date < jobCurrentTime.Date)
                    {
                        isScheduleMissed = true;
                    }
                    else if (schedule.Date.Date == jobCurrentTime.Date)
                    {
                        if (scheduleEndDate < scheduleStartDate)
                        {
                            scheduleEndDate = scheduleEndDate.AddDays(1);
                        }

                        if (scheduleEndDate.DateTime < jobCurrentTime.DateTime)
                        {
                            isScheduleMissed = true;
                        }
                    }
                }

                if (isScheduleMissed)
                {
                    using (var transaction = Context.DataContext.Database.BeginTransaction())
                    {
                        try
                        {
                            var trackableSchedule = await Context.DataContext.DeliveryScheduleXTrackableSchedules.FirstOrDefaultAsync(t => t.Id == schedule.Id);
                            if (trackableSchedule != null)
                            {
                                trackableSchedule.DeliveryScheduleStatusId = trackableSchedule.DeliveryScheduleStatusId == (int)TrackableDeliveryScheduleStatus.Rescheduled ? (int)TrackableDeliveryScheduleStatus.RescheduledMissed : (int)TrackableDeliveryScheduleStatus.Missed;
                                await Context.CommitAsync();
                                if (!string.IsNullOrWhiteSpace(trackableSchedule.FrDeliveryRequestId))
                                {
                                    delReqStatusUpdate.Add(new DeliveryReqStatusUpdateModel() { OrderStatusId = schedule.StatusId ?? 0, DeliveryRequestId = trackableSchedule.FrDeliveryRequestId, ScheduleStatusId = trackableSchedule.DeliveryScheduleStatusId, UserId = 1 });
                                    if (trackableSchedule.DriverId.HasValue)
                                    {
                                        var groupId = schedule.DeliveryGroupId.HasValue ? schedule.DeliveryGroupId.Value : 0;
                                        notificationModel.Add(new ScheduleNotificationModel() { DriverId = trackableSchedule.DriverId, GroupId = groupId, OrderId = trackableSchedule.OrderId ?? 0, ScheduleId = trackableSchedule.DeliveryScheduleId, TrackableScheduleId = schedule.Id, ScheduleStatus = (int)TrackableDeliveryScheduleStatus.Missed });
                                    }
                                }

                                if (delReqStatusUpdate.Any())
                                {
                                    new ScheduleBuilderDomain(this).UpdateDeliveryRequestStatus(delReqStatusUpdate);
                                }

                                await Context.CommitAsync();
                                transaction.Commit();
                            }
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            LogManager.Logger.WriteException("OrderDomain", "ProcessDeliveryScheduleStatusAsync:ScheduleId:" + schedule.Id, ex.Message, ex);
                        }
                    }
                }
                if (isScheduleMissed)
                {
                    if (!schedule.OrderId.HasValue)
                    {
                        NewsfeedDomain newsfeedDomain = new NewsfeedDomain(this);
                        await newsfeedDomain.SetSystemDeliveryScheduleMissedNewsfeed(schedule.OrderId ?? 0, schedule.JobId ?? 0, schedule.CompanyId ?? 0, schedule.AcceptedCompanyId ?? 0, schedule.TimeZoneName, schedule.PoNumber);
                    }
                    if (notificationModel.Any())
                    {
                        await new PushNotificationDomain(this).PushSbChangesNotificationToDriver(notificationModel, "System@mailinator.com");
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "ProcessDeliveryScheduleStatusAsync:ScheduleId:" + schedule.Id, ex.Message, ex);
            }
            return false;
        }

        public async Task<List<OrderVersionHistoryViewModel>> GetOrderVersionHistoryAsync(int orderId)
        {
            List<OrderVersionHistoryViewModel> response = new List<OrderVersionHistoryViewModel>();
            try
            {
                var versionHistory = await new StoredProcedureDomain(this).GetOrderVersionHistory(orderId);
                var historyVersions = versionHistory.Select(t => t.Version).Distinct().Count();
                if (historyVersions > 1)
                {
                    var scheduleVersions = versionHistory.OrderByDescending(t => t.CreatedDate).GroupBy(t => t.Version).Select(g => new { Items = g.ToList() }).ToList();
                    foreach (var scheduleVersion in scheduleVersions)
                    {
                        OrderVersionHistoryViewModel viewModel = new OrderVersionHistoryViewModel(Status.Success);
                        var schedule = scheduleVersion.Items.FirstOrDefault();
                        viewModel.CreatedUser = schedule.CreatedUser;
                        viewModel.CreatedDate = schedule.CreatedDate.ToString(Resource.constFormatDate);
                        var deliverySchedules = scheduleVersion.Items.Where(t => t.Id != null)
                                                            .GroupBy(t => t.GroupId)
                                                            .Select(g => new { Items = g.ToList() }).ToList();
                        foreach (var ds in deliverySchedules)
                        {
                            DeliveryScheduleViewModel dsModel = new DeliveryScheduleViewModel(Status.Success);
                            var firstDs = ds.Items.FirstOrDefault();
                            dsModel.ScheduleTypeName = firstDs.ScheduleType;
                            dsModel.Id = firstDs.Id.Value;
                            dsModel.ScheduleDayNames = ds.Items.Where(t => t.WeekDayCode != null).Select(t => t.WeekDayCode).ToList();
                            dsModel.ScheduleType = firstDs.Type.Value;
                            var scheduleQtyType = firstDs.QuantityTypeId == null ? ScheduleQuantityType.Quantity : (firstDs.QuantityTypeId.Value != ScheduleQuantityType.Quantity ? firstDs.QuantityTypeId.Value : ScheduleQuantityType.Quantity);
                            dsModel.ScheduleQuantityType = scheduleQtyType;
                            dsModel.ScheduleQuantityTypeText = scheduleQtyType.GetDisplayName();
                            dsModel.ScheduleQuantity = firstDs.Quantity.Value.GetPreciseValue(6);
                            dsModel.ScheduleStartTime = Convert.ToDateTime(firstDs.StartTime.ToString()).ToShortTimeString();
                            dsModel.ScheduleEndTime = Convert.ToDateTime(firstDs.EndTime.ToString()).ToShortTimeString();
                            dsModel.StrScheduleDate = firstDs.Date.Value.ToString(Resource.constFormatDate);
                            viewModel.DeliverySchedules.Add(dsModel);
                        }
                        viewModel.DeliverySchedules = viewModel.DeliverySchedules.OrderBy(t => t.Id).ToList();
                        response.Add(viewModel);
                    }
                    int version = response.Count;
                    response.ForEach(t => { t.Version = version; version--; });
                }


            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetOrderVersionHistoryAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<OrderHistoryGridViewModel>> GetBuyerOrderHistoryAsync(int orderId, int userId)
        {
            List<OrderHistoryGridViewModel> response = new List<OrderHistoryGridViewModel>();
            try
            {
                var order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == orderId);
                var user = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == userId);
                if (order != null)
                {
                    var childOrders = order.Orders1.Where(t => t.BuyerCompanyId == user.Company.Id).ToList();
                    HelperDomain helperDomain = new HelperDomain(this);
                    childOrders.ForEach(t => response.Add(new OrderHistoryGridViewModel()
                    {
                        Id = t.Id,
                        PoNumber = t.PoNumber,
                        Version = 1,
                        Supplier = $"{t.User.FirstName} {t.User.LastName}",
                        Phone = t.User.PhoneNumber,
                        Email = t.User.Email,
                        PricePerGallon = helperDomain.GetPricePerGallon(t.FuelRequest),
                        FuelDeliveredPercentage = helperDomain.GetFuelDeliveredPercentage(t),
                        Eligibility = helperDomain.GetDisadvantageBusinessEnterprise(t.FuelRequest.MstSupplierQualifications.ToList()),
                        DateModified = order.AcceptedDate.ToString(Resource.constFormatDate),
                        ModifiedBy = $"{order.User.FirstName} {order.User.LastName}"
                    }));
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetBuyerOrderHistoryAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<OrderHistoryGridViewModel>> GetSupplierOrderHistoryAsync(int orderId, int userId)
        {
            List<OrderHistoryGridViewModel> response = new List<OrderHistoryGridViewModel>();
            try
            {
                var order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == orderId);
                var user = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == userId);
                if (order != null)
                {
                    if (order.User.CompanyId == user.CompanyId)
                    {
                        var childOrders = order.Orders1.Where(t => t.User.CompanyId == user.CompanyId).ToList();
                        HelperDomain helperDomain = new HelperDomain(this);
                        foreach (var childOrder in childOrders)
                        {
                            var row = new OrderHistoryGridViewModel();
                            var supplier = childOrder.FuelRequest.GetParentFuelRequest().User;
                            if (supplier.CompanyId != childOrder.BuyerCompanyId) //when s2 cancels and s3 chose original order, s2 cancels and s3 chose original order, in s3 history customer is coming wrong
                            {
                                foreach (var cancelledOrder in childOrder.Orders1)
                                {
                                    supplier = cancelledOrder.FuelRequest.GetParentFuelRequest().User;
                                    if (supplier.CompanyId == childOrder.BuyerCompanyId)
                                    {
                                        break;
                                    }
                                }
                            }
                            row.Id = childOrder.Id;
                            row.PoNumber = childOrder.PoNumber;
                            row.Version = 1;
                            row.Supplier = $"{supplier.FirstName} {supplier.LastName}";
                            row.Phone = childOrder.FuelRequest.User.PhoneNumber;
                            row.Email = childOrder.FuelRequest.User.Email;
                            row.PricePerGallon = helperDomain.GetPricePerGallon(childOrder.FuelRequest);
                            row.FuelDeliveredPercentage = helperDomain.GetFuelDeliveredPercentage(childOrder);
                            row.Eligibility = helperDomain.GetDisadvantageBusinessEnterprise(childOrder.FuelRequest.MstSupplierQualifications.ToList());
                            row.DateModified = order.AcceptedDate.ToString(Resource.constFormatDate);
                            row.ModifiedBy = $"{order.User.FirstName} {order.User.LastName}";
                            response.Add(row);
                        }
                    }
                    else
                    {
                        response = await GetBuyerOrderHistoryAsync(orderId, userId);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetSupplierOrderHistoryAsync", ex.Message, ex);
            }

            return response;
        }

        public async Task<StatusViewModel> UpdateTogglePricingDetailsAsync(UserContext userContext, int orderId, bool isHidePricingEnabled, CompanyType companyType)
        {
            var response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == orderId);
                    if (order != null)
                    {
                        var orderPricingDetails = order.OrderXTogglePricingDetail;
                        if (orderPricingDetails != null)
                        {
                            if (companyType == CompanyType.Buyer)
                            {
                                orderPricingDetails.IsHidePricingEnabledForBuyer = isHidePricingEnabled;
                            }
                            if (companyType == CompanyType.Supplier)
                            {
                                orderPricingDetails.IsHidePricingEnabledForSupplier = isHidePricingEnabled;
                            }
                            Context.DataContext.Entry(order).State = EntityState.Modified;
                        }
                        else if (isHidePricingEnabled)
                        {
                            OrderXTogglePricingDetail pricingInfo = new OrderXTogglePricingDetail();
                            if (companyType == CompanyType.Buyer)
                            {
                                pricingInfo.IsHidePricingEnabledForBuyer = isHidePricingEnabled;
                            }
                            if (companyType == CompanyType.Supplier)
                            {
                                pricingInfo.IsHidePricingEnabledForSupplier = isHidePricingEnabled;
                            }
                            order.OrderXTogglePricingDetail = pricingInfo;
                        }

                        if (isHidePricingEnabled)
                        {
                            NewsfeedDomain newsfeedDomain = new NewsfeedDomain(this);
                            await newsfeedDomain.SetBuyerEnabledHidePricingNewsfeed(userContext, orderId, companyType == CompanyType.Buyer);
                        }
                        await Context.CommitAsync();
                        transaction.Commit();
                        response.StatusCode = Status.Success;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("OrderDomain", "UpdateTogglePricingDetailsAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusViewModel> UpdateIsPrePostDipRequired(UserContext userContext, int orderId, bool isPrePostDipRequired)
        {
            var response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == orderId);
                    if (order != null)
                    {
                        var frDetail = order.FuelRequest.FuelRequestDetail;
                        if (frDetail != null)
                        {
                            frDetail.IsPrePostDipRequired = isPrePostDipRequired;
                        }

                        await Context.CommitAsync();
                        transaction.Commit();
                        response.StatusCode = Status.Success;
                        response.StatusMessage = isPrePostDipRequired ? Resource.successMessagePrePostDipEnabled : Resource.successMessagePrePostDipDisabled;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("OrderDomain", "UpdateIsPrePostDipRequired:orderId" + orderId + " isPrePostDipRequired:" + isPrePostDipRequired + " userId:" + userContext.Id, ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusViewModel> UpdateFTLCheckDetailsAsync(UserContext userContext, int orderId, bool isFTL)
        {
            var response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == orderId);
                    if (order != null)
                    {
                        if (userContext.IsSupplierCompany)
                        {
                            order.IsFTL = isFTL;
                        }

                        await Context.CommitAsync();
                        transaction.Commit();
                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.successMessageFTLDetailsUpdated;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageUpdateFailed;
                    LogManager.Logger.WriteException("OrderDomain", "UpdateFTLCheckDetailsAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<DeliveryDetailsViewModel> GetDriverDetailsForBuyerAsync(int driverId, int orderId, int deliveryRequestId, int enrouteStatus = 0)
        {
            var response = new DeliveryDetailsViewModel();
            try
            {
                var user = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == driverId);
                if (user != null)
                {
                    response.DriverName = $"{user.FirstName} {user.LastName}";
                    response.PhoneNumber = $"{user.PhoneNumber}";

                    var applocation = user.AppLocations.Where(t => t.UserId == user.Id && (enrouteStatus == 0 || (t.StatusId.HasValue && t.StatusId.Value == enrouteStatus) || t.StatusId == null))
                                                                  .OrderByDescending(x => x.UpdatedDate).FirstOrDefault();
                    if (applocation != null)
                    {
                        response.DriverLatitude = applocation.Latitude;
                        response.DriverLongitude = applocation.Longitude;
                        response.StatusId = applocation.StatusId;

                        bool isDriverStartedDelivery = IsDriverStartedDelivery(applocation, orderId, deliveryRequestId);
                        if (isDriverStartedDelivery)
                        {
                            response.IsDriverStartedDelivery = true;
                        }
                    }

                    var order = Context.DataContext.Orders.FirstOrDefault(t => t.Id == orderId);
                    if (order != null)
                    {
                        response.JobLatitude = order.FuelRequest.Job.Latitude;
                        response.JobLongitude = order.FuelRequest.Job.Longitude;
                        response.JobAddress = order.FuelRequest.Job.Address;
                        response.CountryId = order.FuelRequest.Job.CountryId;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetDriverDetailsForBuyerAsync", ex.Message, ex);
            }
            return response;
        }

        private bool IsDriverStartedDelivery(AppLocation applocation, int orderId, int deliveryRequestId)
        {
            var appLocationOrderId = applocation.OrderId.HasValue ? applocation.OrderId.Value : 0;
            var appLocationDeliveryScheduleId = applocation.DeliveryScheduleId.HasValue ? applocation.DeliveryScheduleId.Value : 0;
            if (appLocationOrderId == orderId && (deliveryRequestId == 0 || appLocationDeliveryScheduleId == deliveryRequestId))
            {
                return true;
            }
            return false;
        }

        public List<DriverScheduleGridViewModel> GetDriverScheduleGridForBuyer(int driverId, int orderId, int deliveryRequestId, int enrouteStatus = 0)
        {
            List<DriverScheduleGridViewModel> response = new List<DriverScheduleGridViewModel>();
            try
            {
                HelperDomain helperDomain = new HelperDomain(this);
                var user = Context.DataContext.Users.SingleOrDefault(t => t.Id == driverId);

                if (user != null)
                {
                    var driverDetails = new DriverScheduleGridViewModel
                    {
                        DriverName = $"{user.FirstName} {user.LastName}",
                        PhoneNumber = $"{user.PhoneNumber}",
                        IsDeliverySchedule = Resource.lblNo,
                        ScheduleDate = Resource.lblHyphen,
                        ScheduleStartTime = Resource.lblHyphen,
                        ScheduleEndTime = Resource.lblHyphen,
                        PONumber = Resource.lblHyphen
                    };

                    var applocation = user.AppLocations.OrderByDescending(t => t.UpdatedDate).FirstOrDefault();
                    if (applocation != null)
                    {
                        driverDetails.StatusId = applocation.StatusId;
                        bool isDriverStartedDelivery = IsDriverStartedDelivery(applocation, orderId, deliveryRequestId);
                        if (isDriverStartedDelivery)
                        {
                            driverDetails.IsDeliverySchedule = Resource.lblYes;
                        }
                    }

                    var order = Context.DataContext.Orders.Include(x => x.OrderDeliverySchedules).FirstOrDefault(t => t.Id == orderId);
                    if (order != null)
                    {
                        var orderSchedules = GetLatestOrderDeliverySchedule(order.OrderDeliverySchedules);
                        if (order.FuelRequest.FuelRequestDetail.DeliveryTypeId == (int)DeliveryType.MultipleDeliveries &&
                                orderSchedules.Count > 0)
                        {
                            var allschedules = GetOrderDeliverySchedules(order);
                            foreach (var schedule in allschedules)
                            {
                                if (schedule.DriverId == user.Id && helperDomain.IsScheduleDateValid(schedule.ScheduleDate.Date, schedule.ScheduleType, DateTime.Now.Date, schedule.AllScheduleDate))
                                {
                                    driverDetails.ScheduleDate = schedule.ScheduleDate.ToString(Resource.constFormatDate);
                                    driverDetails.ScheduleStartTime = schedule.ScheduleStartTime;
                                    driverDetails.ScheduleEndTime = schedule.ScheduleEndTime;
                                    driverDetails.PONumber = order.PoNumber;
                                }
                            }
                        }
                        else if (order.FuelRequest.FuelRequestDetail.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery)
                        {
                            driverDetails.ScheduleDate = order.FuelRequest.FuelRequestDetail.StartDate.ToString(Resource.constFormatDate);
                            driverDetails.ScheduleStartTime = Convert.ToDateTime(order.FuelRequest.FuelRequestDetail.StartTime.ToString()).ToShortTimeString();
                            driverDetails.ScheduleEndTime = Convert.ToDateTime(order.FuelRequest.FuelRequestDetail.EndTime.ToString()).ToShortTimeString();
                            driverDetails.PONumber = order.PoNumber;
                        }
                        response.Add(driverDetails);
                    }

                    if (enrouteStatus > 0)
                    {
                        return response.Where(t => t.StatusId.HasValue && (t.StatusId.Value == enrouteStatus || t.StatusId.Value == (int)EnrouteDeliveryStatus.Unknown)).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetDriverScheduleGridForBuyer", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<CompanyViewModel>> GetTpoSuppliersAsync(int buyerCompanyId)
        {
            List<CompanyViewModel> response = new List<CompanyViewModel>();
            try
            {
                var companies = await Context.DataContext.Companies.Include(t => t.Orders).Where(t => t.Orders.Any(t1 => t1.BuyerCompanyId == buyerCompanyId)).OrderByDescending(t => t.Id).ToListAsync();
                if (companies != null)
                {
                    companies.ForEach(t =>
                        response.Add(new CompanyViewModel()
                        {
                            Id = t.Id,
                            Name = t.Name,
                            SupplierCode = t.SupplierCode
                        })
                        );
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetBuyerOrderHistoryAsync", ex.Message, ex);
            }

            return response;
        }

        private Order GetOriginalBuyerOrder(Order order)
        {
            var parentRequest = order.FuelRequest.FuelRequest1;
            if (parentRequest != null && order.FuelRequest.GetParentFuelRequest().FuelRequest1 != null)
            {
                var parentOrder = order.FuelRequest.GetParentFuelRequest().FuelRequest1.Orders.FirstOrDefault();
                return GetOriginalBuyerOrder(parentOrder);
            }
            else
            {
                return order;
            }
        }

        private decimal GetBrokeredDroppedGallons(Order order)
        {
            var droppedGallons = order.Invoices.Where(t => t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && t.IsActive && !t.IsBuyPriceInvoice).Sum(t => t.DroppedGallons);
            return order.Order1 != null ? droppedGallons += GetBrokeredDroppedGallons(order.Order1) : droppedGallons;
        }

        private decimal GetBrokerOrderedGallons(Order order)
        {
            var originalOrder = GetOriginalBuyerOrder(order);
            return originalOrder.FuelRequest.MaxQuantity - GetBrokeredDroppedGallons(originalOrder);
        }

        public DeliveryScheduleViewModel GetDefaultScheduleTime(int orderId, DeliveryScheduleViewModel deliverySchedule)
        {
            try
            {
                var cacheId = $"{Resource.lblScheduledTime}-{orderId}";
                DeliveryScheduleTime scheduleTime = CacheManager.Get<DeliveryScheduleTime>(cacheId);
                if (scheduleTime == null)
                {
                    var originalOrder = Context.DataContext.Orders.Where(t => t.Id == orderId)
                    .Select(t => new { t.FuelRequest.FuelRequestDetail.StartTime, t.FuelRequest.FuelRequestDetail.EndTime }).FirstOrDefault();
                    if (originalOrder != null)
                    {
                        deliverySchedule.ScheduleStartTime = Convert.ToDateTime(originalOrder.StartTime.ToString()).ToShortTimeString();
                        deliverySchedule.ScheduleEndTime = Convert.ToDateTime(originalOrder.EndTime.ToString()).ToShortTimeString();
                    }
                    DeliveryScheduleTime newScheduleTime = new DeliveryScheduleTime { StartTime = deliverySchedule.ScheduleStartTime, EndTime = deliverySchedule.ScheduleEndTime };
                    CacheManager.Set(cacheId, newScheduleTime, 14400);
                }
                else
                {
                    deliverySchedule.ScheduleStartTime = scheduleTime.StartTime;
                    deliverySchedule.ScheduleEndTime = scheduleTime.EndTime;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetDefaultScheduleTime", ex.Message, ex);
            }
            return deliverySchedule;
        }

        public async Task<OrderSelectionViewModel> ChooseOrderAsync(OrderSelectionViewModel viewModel, UserContext userContext)
        {
            var helperDomain = new HelperDomain(this);
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var selectedOrder = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == viewModel.SelectedOrderId);
                    Order order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == viewModel.OrderId);
                    var isCurrentOrderEndSupplier = order.IsEndSupplier;
                    if (isCurrentOrderEndSupplier)
                    {
                        order.IsEndSupplier = false;
                    }
                    Context.DataContext.Entry(order).State = EntityState.Modified;
                    int buyerCompanyId = GetBuyerCompanyId(order);
                    if (viewModel.OrderId == viewModel.SelectedOrderId)
                    {
                        var parentOrderId = GetParentOrder(order);
                        order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == parentOrderId);
                    }

                    Order updatedOrder = viewModel.OrderId == viewModel.SelectedOrderId ? order : selectedOrder;
                    if (updatedOrder.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.PartiallyCanceled)
                    {
                        order.OrderXStatuses.FirstOrDefault(t => t.IsActive).IsActive = false;
                        OrderXStatus orderStatus = new OrderXStatus();
                        orderStatus.StatusId = (int)OrderStatus.Canceled;
                        orderStatus.IsActive = true;
                        orderStatus.UpdatedBy = viewModel.UserId;
                        orderStatus.UpdatedDate = DateTimeOffset.Now;
                        order.OrderXStatuses.Add(orderStatus);

                        selectedOrder.OrderXStatuses.FirstOrDefault(t => t.IsActive).IsActive = false;
                        OrderXStatus selectedOrderStatus = new OrderXStatus();
                        selectedOrderStatus.StatusId = (int)OrderStatus.Canceled;
                        selectedOrderStatus.IsActive = true;
                        selectedOrderStatus.UpdatedBy = viewModel.UserId;
                        selectedOrderStatus.UpdatedDate = DateTimeOffset.Now;
                        selectedOrder.OrderXStatuses.Add(selectedOrderStatus);
                    }
                    else if (updatedOrder.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.PartiallyClosed)
                    {
                        order.OrderXStatuses.FirstOrDefault(t => t.IsActive).IsActive = false;
                        OrderXStatus orderStatus = new OrderXStatus();
                        orderStatus.StatusId = (int)OrderStatus.Closed;
                        orderStatus.IsActive = true;
                        orderStatus.UpdatedBy = viewModel.UserId;
                        orderStatus.UpdatedDate = DateTimeOffset.Now;
                        order.OrderXStatuses.Add(orderStatus);

                        selectedOrder.OrderXStatuses.FirstOrDefault(t => t.IsActive).IsActive = false;
                        OrderXStatus selectedOrderStatus = new OrderXStatus();
                        selectedOrderStatus.StatusId = (int)OrderStatus.Closed;
                        selectedOrderStatus.IsActive = true;
                        selectedOrderStatus.UpdatedBy = viewModel.UserId;
                        selectedOrderStatus.UpdatedDate = DateTimeOffset.Now;
                        selectedOrder.OrderXStatuses.Add(selectedOrderStatus);
                    }
                    var newOrder = new Order()
                    {
                        PoNumber = ApplicationConstants.PoNumberPrefix,
                        IsProFormaPo = selectedOrder.FuelRequest.Job.IsProFormaPoEnabled,
                        SignatureEnabled = selectedOrder.FuelRequest.Job.SignatureEnabled,
                        FuelRequestId = selectedOrder.FuelRequestId,
                        TerminalId = selectedOrder.TerminalId,
                        AcceptedCompanyId = userContext.CompanyId,
                        BuyerCompanyId = buyerCompanyId,
                        AcceptedBy = viewModel.UserId,
                        AcceptedDate = DateTimeOffset.Now,
                        IsActive = true,
                        UpdatedBy = viewModel.UserId,
                        UpdatedDate = DateTimeOffset.Now,
                        DefaultInvoiceType = selectedOrder.FuelRequest.MstProduct.ProductDisplayGroupId == (int)ProductDisplayGroups.OtherFuelType ? (int)InvoiceType.DigitalDropTicketManual : (int)InvoiceType.Manual,
                        IsEndSupplier = isCurrentOrderEndSupplier,
                        BrokeredMaxQuantity = selectedOrder.FuelRequest.QuantityTypeId == (int)QuantityType.NotSpecified ? ApplicationConstants.QuantityNotSpecified : GetBrokerOrderedGallons(order)
                    };

                    OrderXStatus orderXStatus = new OrderXStatus();
                    orderXStatus.StatusId = (int)OrderStatus.Open;
                    orderXStatus.IsActive = true;
                    orderXStatus.UpdatedBy = viewModel.UserId;
                    orderXStatus.UpdatedDate = DateTimeOffset.Now;
                    newOrder.OrderXStatuses.Add(orderXStatus);
                    newOrder.OrderAdditionalDetail = newOrder.ToOrderAdditionalDetailsEntityForChooseOrder();

                    var latestVersion = GetLatestOrderDeliverySchedule(selectedOrder.OrderDeliverySchedules);
                    if (latestVersion != null && latestVersion.Count > 0)
                    {
                        foreach (var item in latestVersion)
                        {
                            var latestSchedule = GetOrderDeliverySchedule(newOrder.Id, 0, selectedOrder.FuelRequest.CreatedBy, item.DeliveryRequestId);
                            latestSchedule.CreatedDate = selectedOrder.FuelRequest.CreatedDate;
                            latestSchedule.Version = 1;
                            newOrder.OrderDeliverySchedules.Add(latestSchedule);
                        }
                    }
                    else
                    {
                        var latestSchedule = GetOrderDeliverySchedule(newOrder.Id, 0, selectedOrder.FuelRequest.CreatedBy, null);
                        latestSchedule.CreatedDate = selectedOrder.FuelRequest.CreatedDate;
                        latestSchedule.Version = 1;
                        newOrder.OrderDeliverySchedules.Add(latestSchedule);
                    }
                    Context.DataContext.Orders.Add(newOrder);
                    await Context.CommitAsync();

                    newOrder.PoNumber = helperDomain.GetPoNumber(selectedOrder.FuelRequest, selectedOrder.IsProFormaPo, order.Id);
                    newOrder.TfxPoNumber = newOrder.PoNumber;
                    var orderDetailVersion = helperDomain.GetOrderDetailVersion(newOrder, selectedOrder.FuelRequest, userContext.Id);
                    newOrder.OrderDetailVersions.Add(orderDetailVersion);

                    if (newOrder.FuelRequest.FuelRequestDetail.PaymentMethod == PaymentMethods.CreditCard)
                    {
                        helperDomain.AddCreditCardProcessingFee(newOrder);
                    }

                    order.ParentId = newOrder.Id;
                    selectedOrder.ParentId = newOrder.Id;
                    await Context.CommitAsync();

                    //insert trackable schedules
                    var newOrderSchedules = latestVersion.Where(t => t.DeliveryRequestId.HasValue).Select(t => t.DeliverySchedule);
                    TrackableScheduleDomain trackableScheduleDomain = new TrackableScheduleDomain(this);
                    await trackableScheduleDomain.ProcessTrackableSchedules(newOrderSchedules, newOrder);

                    var canceledOrderNewsfeeds = await Context.DataContext.Newsfeeds.Where(t => t.EntityId == updatedOrder.Id && t.RecipientCompanyId == buyerCompanyId).OrderBy(t => t.Id).ToListAsync();
                    canceledOrderNewsfeeds.ForEach(t => t.EntityId = newOrder.Id);
                    Context.DataContext.Newsfeeds.AddRange(canceledOrderNewsfeeds);
                    await Context.CommitAsync();

                    var currentOrderNewsfeeds = await Context.DataContext.Newsfeeds.Where(t => t.EntityId == viewModel.OrderId && t.RecipientCompanyId == userContext.CompanyId).OrderBy(t => t.Id).ToListAsync();
                    currentOrderNewsfeeds.ForEach(t => t.EntityId = newOrder.Id);
                    Context.DataContext.Newsfeeds.AddRange(currentOrderNewsfeeds);
                    await Context.CommitAsync();

                    transaction.Commit();

                    int updatedOrderId = viewModel.OrderId == viewModel.SelectedOrderId ? order.Id : viewModel.SelectedOrderId;
                    NotificationDomain notificationDomain = new NotificationDomain(this);
                    await notificationDomain.AddNotificationEventAsync(EventType.OrderUpdated, updatedOrderId, viewModel.UserId);

                    var newsfeedDomain = new NewsfeedDomain(this);
                    await newsfeedDomain.SetOrderChosenNewsfeed(userContext, newOrder, viewModel.OrderId == viewModel.SelectedOrderId);

                    viewModel.SelectedOrderId = newOrder.Id;
                    viewModel.StatusCode = Status.Success;
                    viewModel.StatusMessage = Resource.errMessageChooseOrderSuccess;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("OrderDomain", "ChooseOrderAsync", ex.Message, ex);
                }
            }
            return viewModel;
        }

        public async Task<StatusViewModel> AssignDriverToOrder(UserContext userContext, int orderId, int? driverId)
        {
            var response = new StatusViewModel(Status.Failed);
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    response = await AssignDriverAsync(userContext, orderId, driverId);
                    if (response.StatusCode == Status.Failed)
                    {
                        response.StatusMessage = Resource.errMessageAssignDrivertoOrderFailed;
                        transaction.Rollback();
                        return response;
                    }
                    transaction.Commit();
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.successMessageAssignDrivertoOrder;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("OrderDomain", "AssignDriverToOrder", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusViewModel> AssignDriverAsync(UserContext userContext, int orderId, int? driverId)
        {
            var response = new StatusViewModel();
            try
            {
                var helperDomain = new HelperDomain(Context);

                var order = Context.DataContext.Orders.Include(t => t.OrderDeliverySchedules)
                                                    .Include("OrderDeliverySchedules.DeliverySchedule").SingleOrDefault(t => t.Id == orderId);

                var orderSchedules = GetLatestOrderDeliverySchedule(order.OrderDeliverySchedules);

                var previousDrivers = Context.DataContext.OrderXDrivers.Where(t => t.OrderId == orderId)
                                        .AsNoTracking().Select(t => new { Driver = t }).ToList();

                helperDomain.AssignOrderLevelDriver(order, userContext.Id, driverId);
                if (orderSchedules != null)
                {
                    foreach (var item in orderSchedules)
                    {
                        if (item.DeliverySchedule != null)
                        {
                            helperDomain.AssignDeliveryLevelDriver(item.DeliverySchedule, userContext.Id, driverId, orderId);
                        }
                    }
                }

                List<int> brokerOrderIds = await GetBrokerOrderIdAsync(order.Id, false);
                foreach (var brOrderId in brokerOrderIds)
                {
                    var brOrder = Context.DataContext.Orders.Include(t => t.OrderXDrivers).Where(t => t.Id == brOrderId).First();
                    helperDomain.AssignOrderLevelDriver(brOrder, userContext.Id, driverId, false);
                }

                var eventId = NewsfeedEvent.SupplierOrderDriverAssigned;
                if (previousDrivers.Any(t => t.Driver.IsActive) && driverId.HasValue)
                {
                    eventId = NewsfeedEvent.SupplierOrderDriverReassigned;
                }
                else if (previousDrivers.Any(t => t.Driver.IsActive) && !driverId.HasValue)
                {
                    eventId = NewsfeedEvent.SupplierOrderDriverUnassigned;
                }
                var newsfeedDomain = new NewsfeedDomain(this);
                await newsfeedDomain.SetDriverAssignmentOrderNewsfeed(userContext, order, eventId);

                Context.DataContext.SaveChanges();
                response.StatusCode = Status.Success;
            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                LogManager.Logger.WriteException("OrderDomain", "AssignDriverAsync", ex.Message, ex);
            }
            return response;
        }

        public OrderDetailVersion GetNewOrderDetailVersion(OrderDetailVersion version, int userId, string poNumber, int paymentTermId, int netDays, PaymentMethods paymentMethod, EditPropertyType EditPropertyType, dynamic orderViewModel)
        {
            OrderDetailVersion response = new OrderDetailVersion();
            if (version != null)
            {
                version.IsActive = false;
                response = new OrderDetailVersion()
                {
                    NetDays = netDays,
                    PaymentTermId = paymentTermId,
                    OrderId = version.Order.Id,
                    CreatedBy = userId,
                    CreatedDate = DateTimeOffset.Now,
                    PoNumber = string.IsNullOrWhiteSpace(poNumber) ? version.Order.PoNumber : poNumber,
                    IsActive = true,
                    PaymentMethod = paymentMethod,
                    Version = version.Version + 1,
                    EditPropertyType = EditPropertyType,
                    JsonOrderHistory = JsonConvert.SerializeObject(orderViewModel)
                };
            }
            return response;
        }

        public async Task<StatusViewModel> EditPoNumberAsync(UserContext userContext, int orderId, string poNumber)
        {
            var response = new StatusViewModel(Status.Failed);
            var newsfeedDomain = new NewsfeedDomain(this);

            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == orderId);
                    if (order != null)
                    {
                        var previousPoNumber = order.PoNumber;
                        if (!string.IsNullOrWhiteSpace(poNumber) && previousPoNumber != poNumber)
                        {
                            order.PoNumber = poNumber;
                            order.UpdatedDate = DateTimeOffset.Now;
                            var currentActiveVersion = order.OrderDetailVersions.LastOrDefault();
                            order.OrderDetailVersions.Add(GetNewOrderDetailVersion(currentActiveVersion, userContext.Id, poNumber, currentActiveVersion.PaymentTermId, currentActiveVersion.NetDays, currentActiveVersion.PaymentMethod, EditPropertyType.PO, new { PONumber = poNumber }));
                            Context.DataContext.Entry(order).State = EntityState.Modified;
                            await Context.CommitAsync();
                            transaction.Commit();

                            await newsfeedDomain.SetPONumberRenamedNewsfeed(userContext, order, poNumber, previousPoNumber);

                            if (order.FuelRequest.FuelRequestTypeId != (int)FuelRequestType.ThirdPartyRequest)
                            {
                                var message = new OrderMessageViewModel { PreviousPoNumber = previousPoNumber };
                                var jsonMessage = new JavaScriptSerializer().Serialize(message);
                                await ContextFactory.Current.GetDomain<NotificationDomain>().AddNotificationEventAsync(EventType.PoNumberChanged, order.Id, userContext.Id, null, jsonMessage);
                            }
                        }
                    }
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.successMessagePoUpdatedSuccessfully;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.StatusMessage = Resource.errMessageUpdateFailed;
                    LogManager.Logger.WriteException("OrderDomain", "EditPoNumberAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusViewModel> EditProFormaPoNumberAsync(UserContext userContext, int orderId, string poNumber)
        {
            var response = new StatusViewModel(Status.Success);
            var newsfeedDomain = new NewsfeedDomain(this);
            var invoiceEditDomain = new InvoiceEditDomain(this);
            var manualInvoiceViewModel = new ManualInvoiceViewModel();
            try
            {
                var order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == orderId && t.FuelRequest.FuelRequestDetail.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery);
                if (order != null)
                {
                    var previousPoNumber = order.PoNumber;
                    if (!string.IsNullOrWhiteSpace(poNumber) && previousPoNumber != poNumber)
                    {
                        var invoices = order.Invoices.Where(t => t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active
                            && t.IsActive && (t.InvoiceTypeId == (int)InvoiceType.Manual || t.InvoiceTypeId == (int)InvoiceType.MobileApp
                            || t.InvoiceTypeId == (int)InvoiceType.DryRun)).ToList();
                        foreach (var invoice in invoices)
                        {
                            response = await invoiceEditDomain.InvoiceEditForInvoicePoNumberAsync(userContext, invoice.Id, poNumber, invoice.OrderId);
                        }

                        if (response.StatusCode == (int)Status.Success)
                        {
                            order.PoNumber = poNumber;
                            order.UpdatedDate = DateTimeOffset.Now;
                            var currentActiveVersion = order.OrderDetailVersions.LastOrDefault();
                            order.OrderDetailVersions.Add(GetNewOrderDetailVersion(currentActiveVersion, userContext.Id, poNumber, currentActiveVersion.PaymentTermId, currentActiveVersion.NetDays, currentActiveVersion.PaymentMethod, EditPropertyType.ProformaPO, new { PONumber = poNumber }));
                            Context.DataContext.Entry(order).State = EntityState.Modified;
                            await Context.CommitAsync();

                            await newsfeedDomain.SetPONumberRenamedNewsfeed(userContext, order, poNumber, previousPoNumber);

                            var message = new OrderMessageViewModel { PreviousPoNumber = previousPoNumber, InvoiceId = manualInvoiceViewModel.InvoiceId, InvoiceNumber = manualInvoiceViewModel.InvoiceNumber.Number };
                            var jsonMessage = new JavaScriptSerializer().Serialize(message);
                            await ContextFactory.Current.GetDomain<NotificationDomain>().AddNotificationEventAsync(EventType.PoNumberChangedForSingleDeliveryOrder, order.Id, userContext.Id, null, jsonMessage);
                        }
                    }
                }

                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.successMessagePoUpdatedSuccessfully;
            }
            catch (Exception ex)
            {
                response.StatusMessage = Resource.errMessageUpdateFailed;
                LogManager.Logger.WriteException("OrderDomain", "EditProFormaPoNumberAsync", ex.Message, ex);
            }

            return response;
        }

        public int? GetParentOrderId(int orderId)
        {
            int? parentOrderId = null;
            try
            {
                var order = Context.DataContext.Orders.FirstOrDefault(t => t.Id == orderId);
                if (order != null)
                {
                    var parentId = GetParentOrder(order);
                    if (parentId > 0 && parentId != orderId)
                    {
                        parentOrderId = parentId;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetParentOrderId", ex.Message, ex);
            }
            return parentOrderId;
        }

        private DateTimeOffset GetNextDateForTotalDrops(DateTimeOffset currentDate, int days)
        {
            DateTimeOffset newDate = currentDate.Date.AddDays(days);
            if (newDate.Date != DateTimeOffset.Now.Date && newDate.Date < DateTimeOffset.Now.Date)
            {
                return GetNextDateForTotalDrops(newDate, days);
            }
            return newDate;
        }

        private bool CheckForQualificationMatch(Order order)
        {
            bool response = true;
            if (order.Orders1 != null && order.Orders1.Count > 0)
            {
                order = order.Orders1.FirstOrDefault(t => t.AcceptedCompanyId == order.AcceptedCompanyId);
            }
            var brokeredFuelRequest = order.FuelRequest.FuelRequests1.OrderByDescending(t => t.Id).FirstOrDefault();

            if (brokeredFuelRequest != null && brokeredFuelRequest.FuelRequestTypeId == (int)FuelRequestType.FreightOnlyRequest)
            {
                return response = false;
            }

            if (brokeredFuelRequest != null && brokeredFuelRequest.GetFuelRequestLastOrder() != null)
            {
                var brokeredOrder = brokeredFuelRequest.GetFuelRequestLastOrder();
                if (brokeredOrder != null && brokeredOrder.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open)
                {
                    var originalFR = GetFirstOpenParentRequest(order);
                    response = brokeredOrder.Company.CompanyAddresses.Any(t => t.IsActive && !originalFR.MstSupplierQualifications.Except(t.MstSupplierQualifications).Any());
                }
            }
            return response;
        }

        private Order CheckForPartiallyCancelledOrder(Order order)
        {
            if (order.FuelRequest.FuelRequest1 != null && order.FuelRequest.GetParentFuelRequest().FuelRequest1 != null)
            {
                var parentOrder = order.FuelRequest.GetParentFuelRequest().FuelRequest1.Orders.LastOrDefault();
                if (parentOrder != null)
                {
                    if (parentOrder.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.PartiallyCanceled || parentOrder.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.PartiallyClosed)
                    {
                        return parentOrder;
                    }
                    else if (parentOrder.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId != (int)OrderStatus.Open)
                    {
                        return CheckForPartiallyCancelledOrder(parentOrder);
                    }
                }
            }
            return null;
        }

        private int GetBuyerCompanyId(Order order)
        {
            var parentFuelRequest = order.FuelRequest.GetParentFuelRequest().FuelRequest1;
            int parentOrderId = GetParentOrder(order);
            if (parentOrderId > 0 && parentOrderId != order.Id)
            {
                var parentOrder = Context.DataContext.Orders.FirstOrDefault(t => t.Id == parentOrderId);
                if (parentOrder != null && parentOrder.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open)
                {
                    return parentOrder.AcceptedCompanyId;
                }
                else if (parentOrder != null)
                {
                    return GetBuyerCompanyId(parentOrder);
                }
                else
                {
                    return parentFuelRequest.User.Company.Id;
                }
            }
            return order.BuyerCompanyId;
        }

        private int GetBrokeredOrderStatusId(Order order, int parentStatusId)
        {
            int orderStatus = order.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.PartiallyCanceled ? (int)OrderStatus.Canceled : (int)OrderStatus.Closed;
            orderStatus = parentStatusId == (int)OrderStatus.PartiallyCanceled ? (int)OrderStatus.Canceled : parentStatusId == (int)OrderStatus.PartiallyClosed ? (int)OrderStatus.Closed : orderStatus;

            HelperDomain helperDomain = new HelperDomain(this);
            if (order.FuelRequest.FuelRequest1 != null && order.FuelRequest.GetParentFuelRequest().FuelRequest1 != null && order.FuelRequest.GetParentFuelRequest().FuelRequest1.Orders.Count > 0)
            {
                if (order.FuelRequest.GetParentFuelRequest().FuelRequest1.Orders.LastOrDefault().OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open && helperDomain.CheckForOpenBrokerOrder(order))
                {
                    orderStatus = (int)OrderStatus.Open;
                }
            }
            else if (helperDomain.CheckForOpenBrokerOrder(order))
            {
                orderStatus = (int)OrderStatus.Open;
            }

            return orderStatus;
        }

        private int GetOpenParentOrder(Order order)
        {
            if (order.FuelRequest.FuelRequest1 != null && order.FuelRequest.GetParentFuelRequest().FuelRequest1 != null)
            {
                var parentOrder = order.FuelRequest.GetParentFuelRequest().FuelRequest1.Orders.LastOrDefault();
                if (parentOrder != null)
                {
                    if (parentOrder.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open)
                    {
                        return parentOrder.Id;
                    }
                    else
                    {
                        return GetOpenParentOrder(parentOrder);
                    }
                }
                else
                {
                    return order.Id;
                }
            }
            return order.Id;
        }

        private int GetCanceledParentOrder(Order order)
        {
            if (order.FuelRequest.FuelRequest1 != null && order.FuelRequest.GetParentFuelRequest().FuelRequest1 != null)
            {
                var parentOrder = order.FuelRequest.GetParentFuelRequest().FuelRequest1.Orders.LastOrDefault();
                if (parentOrder != null)
                {
                    if (parentOrder.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Canceled ||
                        parentOrder.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.PartiallyCanceled)
                    {
                        return GetCanceledParentOrder(parentOrder);
                    }
                    else
                    {
                        return order.Id;
                    }
                }
                else
                {
                    return order.Id;
                }
            }
            return order.Id;
        }

        private int GetParentOrder(Order order)
        {
            if (order.FuelRequest.FuelRequest1 != null && order.FuelRequest.GetParentFuelRequest().FuelRequest1 != null)
            {
                var parentOrder = order.FuelRequest.GetParentFuelRequest().FuelRequest1.Orders.LastOrDefault();
                if (parentOrder != null)
                {
                    if (parentOrder.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.PartiallyCanceled || parentOrder.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.PartiallyClosed)
                    {
                        return parentOrder.Id;
                    }
                    else if (parentOrder.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId != (int)OrderStatus.Open)
                    {
                        return GetParentOrder(parentOrder);
                    }
                    else
                    {
                        return order.ParentId ?? order.Id;
                    }
                }
                else
                {
                    return order.ParentId ?? order.Id;
                }
            }
            return order.ParentId ?? order.Id;
        }

        private FuelRequest GetCustomerRequest(Order order, User user, int buyerCompanyId)
        {
            var customer = order.Orders1.FirstOrDefault(t => t.User.CompanyId != user.CompanyId);
            if (customer.FuelRequest.User.CompanyId == buyerCompanyId || !customer.Orders1.Any())
            {
                return customer.FuelRequest;
            }
            else
            {
                user = customer.User;
                return GetCustomerRequest(customer, user, customer.BuyerCompanyId);
            }
        }

        private int GetOpenBrokerOrderId(Order order)
        {
            var brokeredFuelRequest = order.FuelRequest.FuelRequests1.OrderByDescending(t => t.Id).FirstOrDefault();
            if (brokeredFuelRequest != null && brokeredFuelRequest.GetFuelRequestLastOrder() != null)
            {
                var brokeredOrder = brokeredFuelRequest.GetFuelRequestLastOrder();
                if (brokeredOrder != null && brokeredOrder.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open)
                {
                    return brokeredOrder.Id;
                }
                else
                {
                    if (brokeredOrder != null)
                    {
                        return GetOpenBrokerOrderId(brokeredOrder);
                    }
                    return 0;
                }
            }
            return 0;
        }

        private FuelRequest GetFirstOpenParentRequest(Order order)
        {
            if (order.FuelRequest.FuelRequest1 != null && order.FuelRequest.GetParentFuelRequest().FuelRequest1 != null)
            {
                if (order.FuelRequest.GetParentFuelRequest().FuelRequest1.Orders.LastOrDefault().OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId != (int)OrderStatus.Open)
                {
                    return GetFirstOpenParentRequest(order.FuelRequest.GetParentFuelRequest().FuelRequest1.Orders.LastOrDefault());
                }
                else
                {
                    return order.FuelRequest.GetParentFuelRequest().FuelRequest1;
                }
            }
            else
            {
                return order.FuelRequest;
            }
        }

        private async Task<bool> ReSubmitFuelRequestAsync(Order order, UserContext userContext, bool checkInvOrSchedule = false)
        {
            bool isFuelResubmitted = false;
            try
            {
                HelperDomain helperDomain = new HelperDomain(this);
                FuelRequest fuelRequest = helperDomain.GetFuelRequestConnectedWithBuyer(order);
                if (fuelRequest.Id > 0)
                {
                    var jobTime = DateTimeOffset.Now.ToTargetDateTimeOffset(fuelRequest.Job.TimeZoneName);
                    if (fuelRequest.Job.EndDate != null && fuelRequest.Job.EndDate.Value.Date <= jobTime.Date)
                    {
                        return isFuelResubmitted;
                    }
                    if (fuelRequest.FuelRequestDetail.EndDate != null && fuelRequest.FuelRequestDetail.EndDate.Value.Date <= jobTime.Date)
                    {
                        return isFuelResubmitted;
                    }
                    if (fuelRequest.ExpirationDate != null && fuelRequest.ExpirationDate.Value.Date <= jobTime.Date)
                    {
                        return isFuelResubmitted;
                    }

                    var newFuelRequest = fuelRequest.ToViewModel();
                    newFuelRequest.Id = 0;
                    newFuelRequest.FuelDetails.StatusId = (int)FuelRequestStatus.Open;
                    newFuelRequest.FuelDetails.CreatedDate = DateTimeOffset.Now;
                    var orders = fuelRequest.Orders.Select(t => t.Id).ToList();
                    var invoices = Context.DataContext.Invoices.Where(t => orders.Contains(t.OrderId ?? 0) &&
                                                                                t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active && !t.IsBuyPriceInvoice);
                    if (checkInvOrSchedule && fuelRequest.FuelRequestDetail.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery)
                    {
                        if (invoices.Any() || order.DeliveryScheduleXTrackableSchedules.Any())
                        {
                            return isFuelResubmitted = false;
                        }
                    }
                    else
                    {
                        if (invoices.Any())
                        {
                            if (fuelRequest.QuantityTypeId == (int)QuantityType.SpecificAmount)
                            {
                                newFuelRequest.FuelDetails.FuelQuantity.Quantity = fuelRequest.MaxQuantity - invoices.Sum(t => t.DroppedGallons);
                            }
                            else if (fuelRequest.QuantityTypeId == (int)QuantityType.Range)
                            {
                                newFuelRequest.FuelDetails.FuelQuantity.MaximumQuantity = fuelRequest.MaxQuantity - invoices.Sum(t => t.DroppedGallons);
                            }
                            else if (fuelRequest.QuantityTypeId == (int)QuantityType.NotSpecified)
                            {
                                newFuelRequest.FuelDetails.FuelQuantity.MaximumQuantity = ApplicationConstants.QuantityNotSpecified;
                            }
                        }
                    }

                    FuelRequestDomain fuelRequestDomain = new FuelRequestDomain(this);
                    var status = await fuelRequestDomain.SaveFuelRequestAsync(newFuelRequest, false, 0, userContext);
                    if (status.StatusCode == Status.Success)
                    {
                        fuelRequest.IsFuelAlreadyResubmitted = true;
                        Context.DataContext.Entry(fuelRequest).State = EntityState.Modified;
                        await Context.CommitAsync();

                        isFuelResubmitted = true;
                    }

                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "ReSubmitFuelRequestAsync", ex.Message, ex);
            }
            return isFuelResubmitted;
        }

        public async Task<List<MapViewModel>> GetDriverMapAsync(int companyId, int driverId, OrderFilterViewModel orderFilter = null)
        {
            List<MapViewModel> response = new List<MapViewModel>();
            try
            {
                StoredProcedureDomain storedProcedureDomain = new StoredProcedureDomain(this);
                var orderIds = storedProcedureDomain.GetOrdersAssignedToDriver(companyId, driverId, orderFilter.Filter);

                var user = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == driverId && t.IsActive);
                if (user != null && user.Company != null)
                {
                    IQueryable<Order> allOrders = Context.DataContext.Orders
                                                    .Include(t => t.FuelRequest).Include(t => t.FuelRequest.Job)
                                                    .Include(t => t.FuelRequest.FuelRequest1).Include(t => t.FuelRequest.Job.Users1)
                                                    .Where(t => t.AcceptedCompanyId == user.Company.Id && orderIds.Contains(t.Id));

                    var sqlQuery = allOrders.OrderByDescending(t => t.Id).Where(t => t.IsActive);
                    foreach (var item in sqlQuery)
                    {
                        var itemJob = item.FuelRequest.Job;
                        response.Add(new MapViewModel(Status.Success)
                        {
                            OrderId = item.Id,
                            PoNumber = item.PoNumber,
                            Name = itemJob.Name,
                            Address = itemJob.Address,
                            State = itemJob.MstState.Code,
                            City = itemJob.City,
                            ZipCode = itemJob.ZipCode,
                            Latitude = itemJob.Latitude,
                            Longitude = itemJob.Longitude,
                            ContactPersons = itemJob.Users1.Select(t1 => new ContactPersonViewModel() { Id = t1.Id, Name = $"{t1.FirstName} {t1.LastName}", Email = t1.Email, PhoneNumber = t1.PhoneNumber }).ToList()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetSupplierMap", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetOrderNumbersAsync(int companyId)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var query = await Context.DataContext.Orders.Where
                (
                    t =>
                    t.IsActive &&
                    (t.AcceptedCompanyId == companyId || t.BuyerCompanyId == companyId)
                ).ToListAsync();

                query.ForEach(t => response.Add(new DropdownDisplayItem
                {
                    Id = t.Id,
                    Name = t.PoNumber
                }));
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetOrderNumbersAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> CancelDeliverySchedule(UserContext userContext, int orderId, int trackableScheduleId = 0)
        {
            StatusViewModel response = new StatusViewModel();
            try
            {
                var order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == orderId);

                if (order.FuelRequest.FuelRequestDetail.DeliveryTypeId == (int)DeliveryType.MultipleDeliveries)
                {
                    response = await ProcessTrackableScheduleAsync(userContext, trackableScheduleId, (int)TrackableDeliveryScheduleStatus.Canceled, true);
                }
                else
                {
                    CancelOrderViewModel cancelOrder = new CancelOrderViewModel();
                    cancelOrder.CanceledBy = userContext.Id;
                    cancelOrder.OrderId = orderId;
                    cancelOrder.ReasonId = (int)OrderCancelReason.BuyerSpecifiedOtherReason;
                    cancelOrder.Reason = Resource.msgOrderCancelFromBuyerApp;
                    response = await CancelOrderAsync(userContext, cancelOrder, false, true);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "CancelDeliverySchedule", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> ProcessTrackableScheduleAsync(UserContext userContext, int trackableScheduleId, int deliveryScheduleStatusId, bool isBuyer)
        {
            var response = new StatusViewModel(Status.Failed);
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var trackableSchedule = Context.DataContext.DeliveryScheduleXTrackableSchedules.SingleOrDefault(t => t.Id == trackableScheduleId);
                    if (trackableSchedule != null)
                    {
                        trackableSchedule.DeliveryScheduleStatusId = deliveryScheduleStatusId;
                        if (deliveryScheduleStatusId == (int)DeliveryScheduleStatus.Canceled || deliveryScheduleStatusId == (int)TrackableDeliveryScheduleStatus.MissedAndCanceled)
                        {
                            trackableSchedule.DeliveryGroupId = null;
                        }
                        Context.DataContext.Entry(trackableSchedule).State = EntityState.Modified;
                        Context.Commit();
                        transaction.Commit();
                        if (!string.IsNullOrWhiteSpace(trackableSchedule.FrDeliveryRequestId) && (deliveryScheduleStatusId == (int)DeliveryScheduleStatus.Canceled || deliveryScheduleStatusId == (int)TrackableDeliveryScheduleStatus.MissedAndCanceled))
                        {
                            var delReqStatusUpdate = new List<DeliveryReqStatusUpdateModel>();
                            delReqStatusUpdate.Add(new DeliveryReqStatusUpdateModel() { DeliveryRequestId = trackableSchedule.FrDeliveryRequestId, ScheduleStatusId = deliveryScheduleStatusId, UserId = userContext.Id });
                            new ScheduleBuilderDomain(this).UpdateDeliveryRequestStatus(delReqStatusUpdate);
                        }
                        var newsfeedDomain = new NewsfeedDomain(this);
                        var eventId = userContext.IsBuyerCompany || userContext.CompanySubTypeId == CompanyType.Buyer ? NewsfeedEvent.BuyerCanceledSchedule : NewsfeedEvent.SupplierCanceledSchedule;
                        await newsfeedDomain.SetDeliveryScheduleNewsfeed(userContext, trackableSchedule.Order, eventId);

                        var pushNotificationDomain = new PushNotificationDomain(this);
                        if (trackableSchedule.OrderId != null)
                        {
                            await SendDeliveryCancelledMessage(userContext, trackableSchedule, isBuyer);
                        }
                        await pushNotificationDomain.PushNotificationScheduleCancel(userContext, trackableSchedule, isBuyer);

                        var dispatchDomain = new DispatchDomain(this);
                        //if (deliveryScheduleStatusId == (int)DeliveryScheduleStatus.Canceled || deliveryScheduleStatusId == (int)TrackableDeliveryScheduleStatus.MissedAndCanceled)
                        //{
                        //    NotificationDispatchLocationViewModel dispatchLocation = new NotificationDispatchLocationViewModel()
                        //    {
                        //        DispatchNotificationType = DispatchNotificationType.CancelDelivery,
                        //        DeliveryScheduleId = trackableSchedule.DeliveryScheduleId,
                        //        TrackableScheduleId = trackableSchedule.Id
                        //    };
                        //    dispatchDomain.ProcessDispatchLocationForWebNotifications(dispatchLocation, trackableSchedule.OrderId, userContext.Id);
                        //}
                        if (trackableSchedule.DriverId.HasValue)
                        {
                            await dispatchDomain.RemoveOnMyWay(trackableSchedule.DriverId.Value, trackableSchedule.OrderId, (int)EnrouteDeliveryStatus.SupplierCanceled, trackableSchedule.Id, trackableSchedule.DeliveryScheduleId);
                        }
                    }
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageDeliveryScheduleCancelSuccess;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("OrderDomain", "ProcessDeliverySchedule", ex.Message, ex);
                }
            }
            return response;
        }

        public DeliveryScheduleViewModel GetDeliveryScheduleByTrackableScheduleId(int trackableScheduleId)
        {
            DeliveryScheduleViewModel deliverySchedule = new DeliveryScheduleViewModel();
            try
            {
                var trackableSchedule = Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(t => t.Id == trackableScheduleId).Select(t => new
                {
                    t.Date,
                    t.StartTime,
                    t.EndTime,
                    t.SupplierContract,
                    t.SupplierSource,
                    t.LoadCode,
                    t.Carrier,
                    t.Quantity,
                    t.DriverId,
                    IsFTL = t.Order.IsFTL,
                    t.DeliveryScheduleId
                }).SingleOrDefault();
                if (trackableSchedule != null)
                {
                    deliverySchedule.ScheduleDate = trackableSchedule.Date;
                    deliverySchedule.ScheduleStartTime = $"{Convert.ToDateTime(trackableSchedule.StartTime.ToString()).ToShortTimeString()}";
                    deliverySchedule.ScheduleEndTime = $"{Convert.ToDateTime(trackableSchedule.EndTime.ToString()).ToShortTimeString()}";
                    deliverySchedule.ScheduleQuantity = trackableSchedule.Quantity.GetPreciseValue();
                    deliverySchedule.DriverId = trackableSchedule.DriverId;
                    deliverySchedule.LoadCode = trackableSchedule.LoadCode;
                    if (trackableSchedule.Carrier != null)
                    {
                        deliverySchedule.Carrier = trackableSchedule.Carrier.ToViewModel();
                    }

                    deliverySchedule.SupplierSource = new SupplierSourceViewModel() { ContractNumber = trackableSchedule.SupplierContract };
                    if (trackableSchedule.SupplierSource != null)
                    {
                        deliverySchedule.SupplierSource.Id = trackableSchedule.SupplierSource.Id;
                        deliverySchedule.SupplierSource.Name = trackableSchedule.SupplierSource.Name;
                    }
                    deliverySchedule.IsFtlOrder = trackableSchedule.IsFTL;
                    var splitLoadAddresses = Context.DataContext.FuelDispatchLocations.Where(t => t.TrackableScheduleId == trackableScheduleId && t.ParentId != null).Select(t => t.ParentId).ToList();
                    deliverySchedule.SplitLoadAddresses = Context.DataContext.FuelDispatchLocations.Where(t => (t.TrackableScheduleId == trackableScheduleId || t.DeliveryScheduleId == trackableSchedule.DeliveryScheduleId)
                                                                             && !t.IsSkipped && !splitLoadAddresses.Contains(t.Id)
                                                                             && !t.IsJobLocation && t.LocationType == (int)LocationType.Drop && t.IsActive).Select(t => new SplitLoadAddressViewModel()
                                                                             {
                                                                                 Id = t.Id,
                                                                                 Address = t.Address,
                                                                                 City = t.City,
                                                                                 StateCode = t.StateCode,
                                                                                 StateId = t.StateId.Value,
                                                                                 ZipCode = t.ZipCode,
                                                                                 CountryCode = t.CountryCode,
                                                                                 Latitude = t.Latitude,
                                                                                 Longitude = t.Longitude,
                                                                                 CountyName = t.CountyName,
                                                                                 Currency = t.Currency,
                                                                                 TimeZoneName = t.TimeZoneName,
                                                                                 SiteName = t.SiteName
                                                                             }).ToList();

                    deliverySchedule.IsSplitDrop = deliverySchedule.SplitLoadAddresses.Any();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetDeliveryScheduleByTrackableScheduleId", ex.Message, ex);
            }
            return deliverySchedule;
        }

        public async Task<StatusViewModel> SendDeliveryNotification(int orderId, string poNumber, int driverId, int groupId)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                var currentSchedules = await GetCurrentDeliverySchedules(orderId);
                var previousSchedules = await GetPreviousDeliverySchedules(orderId);

                if (currentSchedules.Any())
                {
                    var currentScheduleViewModel = GetDeliveryScheduleViewModel(groupId, currentSchedules);
                    var previousScheduleViewModel = GetDeliveryScheduleViewModel(groupId, previousSchedules);
                    if (currentScheduleViewModel != null)
                    {
                        currentScheduleViewModel.PONumber = poNumber;
                        var pushNotificationDomain = new PushNotificationDomain(this);
                        var viewModel = new DriverNotificationViewModel();

                        viewModel.DriverIds.Add(driverId);
                        viewModel.Message.Body = GetPushNotificationBody(currentScheduleViewModel, previousScheduleViewModel);

                        switch (currentScheduleViewModel.StatusId)
                        {
                            case 6:
                                viewModel.Message.Title = Resource.notificationToDriver_RescheduledDelivery_Title;
                                break;

                            case 14:
                            case 17:
                                viewModel.Message.Title = Resource.notificationToDriver_AssignedToDelivery_Title;
                                break;

                            case 15:
                                viewModel.Message.Title = Resource.notificationToDriver_ModifiedDelivery_Title;
                                break;

                            case 16:
                                viewModel.Message.Title = Resource.notificationToDriver_AssignedToDelivery_Title;
                                await SendNotificationToRemovedDriver(currentSchedules, currentScheduleViewModel);
                                break;
                        }
                        response = await pushNotificationDomain.NotificationToDriver(viewModel);
                    }
                }
                if (response.StatusCode == Status.Success)
                {
                    response.StatusMessage = Resource.errMessageNotificationSentSuccess;
                }
                else if (response.StatusMessage != Resource.errMessageDriverNotLoggedInApp)
                {
                    response.StatusMessage = Resource.errMessageNotificationSentFailed;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "SendDeliveryNotification", ex.Message, ex);
            }
            return response;
        }

        private async Task<List<DeliverySchedule>> GetCurrentDeliverySchedules(int orderId)
        {
            var response = new List<DeliverySchedule>();
            var currentVersion = await Context.DataContext.OrderVersionXDeliverySchedules.OrderByDescending(t => t.Id).FirstOrDefaultAsync(t => t.IsActive && t.OrderId == orderId);
            if (currentVersion != null)
            {
                response = currentVersion.Order.OrderDeliverySchedules.Where(t => t.Version == currentVersion.Version && t.DeliveryRequestId.HasValue).Select(t => t.DeliverySchedule).ToList();
            }
            return response;
        }

        private async Task<List<DeliverySchedule>> GetPreviousDeliverySchedules(int orderId)
        {
            var response = new List<DeliverySchedule>();
            var previousVersion = Context.DataContext.OrderVersionXDeliverySchedules.Where(t => t.OrderId == orderId && !t.IsActive).OrderByDescending(t => t.Version).Select(t1 => t1.Version).FirstOrDefault();
            var previousDeliverySchedules = await Context.DataContext.OrderVersionXDeliverySchedules.Where(t => t.Version == previousVersion && t.DeliveryRequestId.HasValue).Select(t => t.DeliverySchedule).ToListAsync();

            return previousDeliverySchedules ?? response;
        }

        private DeliveryScheduleViewModel GetDeliveryScheduleViewModel(int groupId, IEnumerable<DeliverySchedule> deliverySchedules)
        {
            var response = new DeliveryScheduleViewModel();
            try
            {
                var scheduleByGroup = deliverySchedules.Where(t => t.GroupId == groupId)
                        .GroupBy(t => t.GroupId).SelectMany(g => g.Select(t1 => t1)).ToList();

                if (scheduleByGroup.Any())
                {
                    response = scheduleByGroup.ToViewModel();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetDeliveryScheduleViewModel", ex.Message, ex);
            }
            return response;
        }

        private string GetPushNotificationBody(DeliveryScheduleViewModel currentSchedule, DeliveryScheduleViewModel previousSchedule)
        {
            var response = string.Empty;
            try
            {
                string currentDeliveryDate = string.Empty, previousDeliveryDate = string.Empty;
                if (currentSchedule.ScheduleType == (int)DeliveryScheduleType.Weekly || currentSchedule.ScheduleType == (int)DeliveryScheduleType.BiWeekly)
                {
                    currentDeliveryDate = string.Join(",", currentSchedule.ScheduleDayNames);
                    if (previousSchedule.ScheduleDayNames.Any())
                    {
                        previousDeliveryDate = string.Join(",", previousSchedule.ScheduleDayNames);
                    }
                }
                else
                {
                    currentDeliveryDate = currentSchedule.ScheduleDate.ToString(Resource.constFormatDate);
                    if (previousSchedule.ScheduleDate > DateTimeOffset.MinValue)
                    {
                        previousDeliveryDate = previousSchedule.ScheduleDate.ToString(Resource.constFormatDate);
                    }
                }

                switch (currentSchedule.StatusId)
                {
                    case 6:
                        var trackable = Context.DataContext.DeliveryScheduleXTrackableSchedules.FirstOrDefault(t => t.Id == currentSchedule.RescheduledTrackableId);
                        response = string.Format(Resource.notificationToDriver_RescheduledDelivery_Body,
                                                currentSchedule.PONumber,
                                                trackable.Date.ToString(Resource.constFormatDate),
                                                $"{trackable.Date.Add(trackable.StartTime).ToString(Resource.constFormat12HourTime)} to " +
                                                $"{trackable.Date.Add(trackable.EndTime).ToString(Resource.constFormat12HourTime)}",
                                                currentDeliveryDate,
                                                $"{currentSchedule.ScheduleStartTime} to " +
                                                $"{currentSchedule.ScheduleEndTime}");
                        break;

                    case 14:
                    case 16:
                        response = string.Format(Resource.notificationToDriver_AssignedToDelivery_Body,
                                                currentSchedule.PONumber,
                                                currentDeliveryDate,
                                                $"{currentSchedule.ScheduleStartTime} to " +
                                                $"{currentSchedule.ScheduleEndTime}");
                        break;

                    case 15:
                        response = string.Format(Resource.notificationToDriver_ModifiedDelivery_Body,
                                                currentSchedule.PONumber,
                                                previousDeliveryDate,
                                                $"{previousSchedule.ScheduleStartTime} to " +
                                                $"{previousSchedule.ScheduleEndTime}",
                                                previousSchedule.ScheduleQuantity.GetPreciseValue(2).GetCommaSeperatedValue(),
                                                currentDeliveryDate,
                                                $"{currentSchedule.ScheduleStartTime} to " +
                                                $"{currentSchedule.ScheduleEndTime}",
                                                currentSchedule.ScheduleQuantity.GetPreciseValue(2).GetCommaSeperatedValue());
                        break;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetPushNotificationBody", ex.Message, ex);
            }
            return response;
        }

        private async Task SendNotificationToRemovedDriver(IEnumerable<DeliverySchedule> currentSchedules, DeliveryScheduleViewModel currentScheduleViewModel)
        {
            var removedViewModel = new DriverNotificationViewModel();
            removedViewModel.Message.Title = Resource.notificationToDriver_RemovedFromDelivery_Title;
            removedViewModel.Message.Body = string.Format(Resource.notificationToDriver_RemovedFromDelivery_Body, currentScheduleViewModel.PONumber);

            var driverRemovedFromSchedule = currentSchedules.First(t => t.Id == currentScheduleViewModel.Id);
            var removedDriverId = driverRemovedFromSchedule.DeliveryScheduleXDrivers.Last(t => t.RemovedBy != null).DriverId;
            removedViewModel.DriverIds.Add(removedDriverId);

            var pushNotificationDomain = new PushNotificationDomain(this);
            await pushNotificationDomain.NotificationToDriver(removedViewModel);
        }

        private void Set24HoursWarning(UserContext userContext, StatusViewModel response, IEnumerable<DeliverySchedule> anyWinthin24Hrs)
        {
            response.StatusCode = Status.Warning;
            if (anyWinthin24Hrs.Any(t => t.StatusId == (int)DeliveryScheduleStatus.Modified))
            {
                if ((userContext.IsBuyerAndSupplierCompany && userContext.CompanySubTypeId == CompanyType.Buyer) || userContext.IsBuyerCompany)
                {
                    response.StatusMessage = Resource.warningMessageBuyerModifiedDeliveryIn24Hrs;
                }
                else
                {
                    response.StatusMessage = Resource.warningMessageSupplierModifiedDeliveryIn24Hrs;
                }
            }
            if (anyWinthin24Hrs.Any(t => t.StatusId == (int)DeliveryScheduleStatus.Rescheduled))
            {
                if ((userContext.IsBuyerAndSupplierCompany && userContext.CompanySubTypeId == CompanyType.Buyer) || userContext.IsBuyerCompany)
                {
                    response.StatusMessage = Resource.warningMessageBuyerRescheduledDeliveryIn24Hrs;
                }
                else
                {
                    response.StatusMessage = Resource.warningMessageSupplierRescheduledDeliveryIn24Hrs;
                }
            }
            if (anyWinthin24Hrs.Any(t => t.StatusId == (int)DeliveryScheduleStatus.New))
            {
                if ((userContext.IsBuyerAndSupplierCompany && userContext.CompanySubTypeId == CompanyType.Buyer) || userContext.IsBuyerCompany)
                {
                    response.StatusMessage = Resource.warningMessageBuyerNewDeliveryIn24Hrs;
                }
                else
                {
                    response.StatusMessage = Resource.warningMessageSupplierNewDeliveryIn24Hrs;
                }
            }
        }

        private async Task SendDeliveryCancelledMessage(UserContext userContext, DeliveryScheduleXTrackableSchedule trackableSchedule, bool isBuyer)
        {
            var helperDomain = new HelperDomain(this);
            var serverUrl = helperDomain.GetServerUrl();
            AppMessageDomain appMessageDomain = new AppMessageDomain(this);

            var quantityDelivered = ($"{trackableSchedule.Quantity.GetPreciseValue(2).GetCommaSeperatedValue()} {trackableSchedule.UoM}").ToLower();
            var composeMessageViewModel = new ComposeMessageViewModel
            {
                Id = 0,
                ComposeType = AppMessageComposeType.Compose,
                Type = AppMessageQueryType.Order,
                Number = trackableSchedule.OrderId ?? 0,
                Subject = string.Format(Resource.emailDeliveryCancelled_Buyer_SubjectText, userContext.Name,
                                                                            userContext.CompanyName),
                Message = string.Format(Resource.emailDeliveryCancelled_Buyer_BodyText, isBuyer ? $"{trackableSchedule.Order.User.FirstName} {trackableSchedule.Order.User.LastName}" : $"{trackableSchedule.Order.FuelRequest.User.FirstName} {trackableSchedule.Order.FuelRequest.User.LastName}",
                                                                            userContext.Name,
                                                                            userContext.CompanyName,
                                                                            trackableSchedule.Order.PoNumber,
                                                                            trackableSchedule.Date.ToString(Resource.constFormatDate),
                                                                            $"{Convert.ToDateTime(trackableSchedule.StartTime.ToString()).ToString(Resource.constFormat12HourTime)} - {Convert.ToDateTime(trackableSchedule.EndTime.ToString()).ToString(Resource.constFormat12HourTime)}",
                                                                            quantityDelivered,
                                                                            isBuyer ? serverUrl + "Supplier/Order/Details/" + trackableSchedule.OrderId : serverUrl + "Buyer/Order/Details/" + trackableSchedule.OrderId),
                TimeStamp = DateTimeOffset.Now
            };
            composeMessageViewModel.Recipients.Add(trackableSchedule.Order.FuelRequest.User.Id);
            if (trackableSchedule.DriverId != null)
            {
                composeMessageViewModel.Message = string.Format(Resource.emailDeliveryCancelled_Buyer_BodyText, $"{trackableSchedule.User.FirstName} {trackableSchedule.User.LastName}",
                                                                            userContext.Name,
                                                                            userContext.CompanyName,
                                                                            trackableSchedule.Order.PoNumber,
                                                                            trackableSchedule.Date.ToString(Resource.constFormatDate),
                                                                            $"{Convert.ToDateTime(trackableSchedule.StartTime.ToString()).ToString(Resource.constFormat12HourTime)} - {Convert.ToDateTime(trackableSchedule.EndTime.ToString()).ToString(Resource.constFormat12HourTime)}",
                                                                            quantityDelivered,
                                                                            serverUrl + "Driver/Order/Details/" + trackableSchedule.OrderId);
                composeMessageViewModel.Recipients.Add(trackableSchedule.DriverId ?? 0);
            }
            await appMessageDomain.SaveAppMessageAsync((int)SystemUser.System, composeMessageViewModel);
        }

        private void SetDeliveryWithin24Hrs(IEnumerable<DeliveryScheduleViewModel> schedules, int orderId, string timeZoneName)
        {
            DateTimeOffset currentDateTimeOffset = DateTimeOffset.Now.ToTargetDateTimeOffset(timeZoneName);
            DateTimeOffset todaysDate = currentDateTimeOffset.Date;
            TimeSpan currentTime = currentDateTimeOffset.TimeOfDay;

            var currentGroupIds = schedules.Select(t => t.GroupId).Distinct();
            var todaysSchedules = Context.DataContext.DeliveryScheduleXTrackableSchedules.Include(t => t.DeliverySchedule)
                                    .Where(t => t.OrderId == orderId && t.Date == todaysDate && t.IsActive
                                    && (t.StartTime >= currentTime || t.EndTime >= currentTime)
                                    && currentGroupIds.Contains(t.DeliverySchedule.GroupId)).ToList();

            foreach (var schedule in schedules)
            {
                schedule.IsDeliveryIn24Hrs = todaysSchedules.Where(Extensions.IsTrackableScheduleUnDeliveredFunc()).Any(t => t.DeliverySchedule.GroupId == schedule.GroupId && !t.IsDropped);
            }
            var futureSchedules = schedules.Where(t => !t.IsDeliveryIn24Hrs).ToList();
            foreach (var schedule in futureSchedules)
            {
                var trackableSchedule = todaysSchedules.Where(Extensions.IsTrackableScheduleDeliveredFunc()).FirstOrDefault(t => t.DeliverySchedule.GroupId == schedule.GroupId);
                if (trackableSchedule != null)
                {
                    schedule.ScheduleDays.Remove(trackableSchedule.DeliverySchedule.WeekDayId);
                }
                var scheduleDate = GetDeliveryNextDate(schedule.ScheduleType, schedule.ScheduleDate, schedule.ScheduleDays, todaysDate);
                var ScheduleStartHrs = scheduleDate.Date.Add(schedule.StartTime).Subtract(currentDateTimeOffset.DateTime).TotalHours;
                var ScheduleEndHrs = scheduleDate.Date.Add(schedule.EndTime).Subtract(currentDateTimeOffset.DateTime).TotalHours;
                if ((ScheduleStartHrs > 0 && ScheduleStartHrs <= ApplicationConstants.ScheduleWarningHours)
                    || (ScheduleEndHrs > 0 && ScheduleEndHrs <= ApplicationConstants.ScheduleWarningHours))
                {
                    schedule.IsDeliveryIn24Hrs = true;
                }
                if (trackableSchedule != null)
                {
                    schedule.ScheduleDays.Add(trackableSchedule.DeliverySchedule.WeekDayId);
                }
            }
        }

        private bool IsDeliveryWithin24Hrs(IEnumerable<DeliverySchedule> schedules, int orderId, string timeZoneName)
        {
            var response = false;
            DateTimeOffset currentDateTimeOffset = DateTimeOffset.Now.ToTargetDateTimeOffset(timeZoneName);
            DateTimeOffset todaysDate = currentDateTimeOffset.Date;

            var currentGroupIds = schedules.Select(t => t.GroupId).Distinct();
            var todaysSchedules = Context.DataContext.DeliveryScheduleXTrackableSchedules.Include(t => t.DeliverySchedule)
                                    .Where(t => t.OrderId == orderId && t.Date == todaysDate && t.IsActive
                                    && currentGroupIds.Contains(t.DeliverySchedule.GroupId)).ToList();

            foreach (var schedule in schedules)
            {
                response = todaysSchedules.Where(Extensions.IsTrackableScheduleUnDeliveredFunc()).Any(t => t.DeliverySchedule.Id == schedule.Id && !t.IsDropped);
                if (response)
                {
                    break;
                }
            }

            if (!response)
            {
                response = IsWithin24Hrs(schedules, todaysSchedules, currentDateTimeOffset);
            }

            return response;
        }

        private bool IsWithin24Hrs(IEnumerable<DeliverySchedule> schedules, IEnumerable<DeliveryScheduleXTrackableSchedule> trackableSchedules, DateTimeOffset currentDateTimeOffset)
        {
            var response = false;
            foreach (var schedule in schedules)
            {
                var trackableSchedule = trackableSchedules.Where(Extensions.IsTrackableScheduleDeliveredFunc()).FirstOrDefault(t => t.DeliverySchedule.GroupId == schedule.GroupId);
                if (trackableSchedule != null && trackableSchedule.DeliverySchedule.WeekDayId == schedule.WeekDayId)
                {
                    continue;
                }
                var scheduleDate = GetDeliveryNextDate(schedule.Type, schedule.Date, new List<int> { schedule.WeekDayId }, currentDateTimeOffset.Date);
                var ScheduleStartHrs = scheduleDate.Date.Add(schedule.StartTime).Subtract(currentDateTimeOffset.DateTime).TotalHours;
                var ScheduleEndHrs = scheduleDate.Date.Add(schedule.EndTime).Subtract(currentDateTimeOffset.DateTime).TotalHours;
                if ((ScheduleStartHrs > 0 && ScheduleStartHrs <= ApplicationConstants.ScheduleWarningHours)
                    || (ScheduleEndHrs > 0 && ScheduleEndHrs <= ApplicationConstants.ScheduleWarningHours))
                {
                    response = true;
                    break;
                }
            }
            return response;
        }

        private DateTimeOffset GetDeliveryNextDate(int scheduleType, DateTimeOffset scheduleDate, IEnumerable<int> scheduleDays, DateTimeOffset currentDate)
        {
            DateTimeOffset? nextDate = null;
            DateTimeOffset? previousScheduleDate = null;
            switch (scheduleType)
            {
                case 1:
                    foreach (var dayId in scheduleDays)
                    {
                        previousScheduleDate = nextDate;
                        nextDate = CalculateScheduleDate(currentDate, previousScheduleDate, dayId, 7);
                    }
                    break;
                case 2:
                    foreach (var dayId in scheduleDays)
                    {
                        previousScheduleDate = nextDate;
                        nextDate = CalculateScheduleDate(currentDate, previousScheduleDate, dayId, 14);
                    }
                    break;
                case 3:
                    if (scheduleDate < currentDate)
                    {
                        int datediff = Math.Abs(scheduleDate.Subtract(currentDate).Days) % 30;
                        nextDate = datediff == 0 ? currentDate.AddDays(30) : currentDate.AddDays(30 - datediff);
                    }
                    break;
            }
            if (!nextDate.HasValue)
            {
                nextDate = scheduleDate;
            }
            return nextDate.Value;
        }

        private DateTimeOffset CalculateScheduleDate(DateTimeOffset currentDate, DateTimeOffset? previousScheduleDate, int dayId, int dayModulus)
        {
            DateTimeOffset deliveryDate;
            int scheduleDayId = (dayId % 7), currentDayId = (int)currentDate.DayOfWeek;
            if (currentDayId != scheduleDayId)
            {
                int daysToAdd = (scheduleDayId - currentDayId + dayModulus) % dayModulus;
                deliveryDate = currentDate.AddDays(daysToAdd);
            }
            else
            {
                deliveryDate = currentDate;
            }
            var response = previousScheduleDate.HasValue && previousScheduleDate.Value < deliveryDate ? previousScheduleDate.Value : deliveryDate;
            return response;
        }

        private List<DeliverySchedule> GetDeletedSchedules(Order order)
        {
            var currentVersion = order.OrderDeliverySchedules.OrderByDescending(t => t.Id).Select(t => t.Version).FirstOrDefault();
            var currentSchedules = order.OrderDeliverySchedules.Where(t => t.Version == currentVersion && t.DeliveryRequestId.HasValue).Select(t => t.DeliverySchedule);
            var previousVersionSchedules = order.OrderDeliverySchedules.Where(t => (t.Version == currentVersion - 1) && t.DeliveryRequestId.HasValue).Select(t => t.DeliverySchedule);
            var deletedSchedules = previousVersionSchedules.Except(currentSchedules).ToList();
            return deletedSchedules;
        }

        public UserContext SetUserContextFromOrder(int orderId)
        {
            var order = Context.DataContext.Orders.SingleOrDefault(t => t.Id == orderId);
            if (order != null)
            {
                var userContext = new UserContext();
                userContext.Id = order.AcceptedBy;
                userContext.CompanyId = order.AcceptedCompanyId;
                userContext.CompanyName = order.Company.Name;
                userContext.Name = $"{order.User.FirstName} {order.User.LastName}";
                userContext.Email = order.User.Email;
                return userContext;
            }
            return null;
        }

        public async Task<StatusViewModel> UpdatePaymentTerms(UserContext userContext, int orderId, int paymentTermId, int netDays, PaymentMethods paymentMethod)
        {
            var helperDomain = new HelperDomain(this);
            StatusViewModel response = new StatusViewModel();
            var order = Context.DataContext.Orders.SingleOrDefault(t => t.Id == orderId);
            var currentActivePaymentTermsVersion = order.OrderDetailVersions.Last(); // currentActivePaymentTermsVersion will never be null as we save 1 version by default when order is created
            helperDomain.CheckForProcessingFee(paymentMethod, order, currentActivePaymentTermsVersion);
            OrderDetailVersion orderDetailVersion = new OrderDetailVersion
            {
                OrderId = orderId,
                PoNumber = currentActivePaymentTermsVersion.PoNumber,
                Version = currentActivePaymentTermsVersion.Version + 1,
                PaymentTermId = paymentTermId,
                PaymentMethod = paymentMethod,
                NetDays = netDays,
                IsActive = true,
                CreatedBy = userContext.Id,
                CreatedDate = DateTimeOffset.Now,
                EditPropertyType = EditPropertyType.PaymentTerms,
                JsonOrderHistory = JsonConvert.SerializeObject(new { PaymentTermId = paymentTermId, PaymentMethod = paymentMethod, NetDays = netDays })
            };

            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    currentActivePaymentTermsVersion.IsActive = false;
                    order.OrderDetailVersions.Add(orderDetailVersion);
                    await Context.CommitAsync();
                    transaction.Commit();

                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageUpdateOrderPaymentTermsSuccess;
                    // send newsfeed / email notifications here

                    var newsfeedDomain = new NewsfeedDomain(this);
                    await newsfeedDomain.SetOrderPaymentTermsUpdatedNewsfeed(userContext, order);

                    var notificationDomain = new NotificationDomain(this);
                    await notificationDomain.AddNotificationEventAsync(EventType.OrderPaymentTermsUpdated, order.Id, userContext.Id);
                }
                catch (Exception ex)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageUpdateOrderPaymentTermsFailed;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("OrderDomain", "UpdatePaymentTerms", ex.Message, ex);
                }
            }

            return response;
        }

        public async Task<StatusViewModel> UpdateFuelSurchargeFreightFee(UserContext userContext, OrderDetailsViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel();
            ThirdPartyOrderDomain thirdPartyOrderDomain = new ThirdPartyOrderDomain(this);
            var order = Context.DataContext.Orders.Where(t => t.Id == viewModel.Id).Select(t => new
            {
                t.Id,
                t.FuelRequestId,
                t.FuelRequest.Currency,
                t.FuelRequest.UoM,
                t.IsFTL,
                t.FuelRequest,
                t.BuyerCompanyId,
                t.OrderAdditionalDetail,
                t.PoNumber,
                t.FuelRequest.Job.TimeZoneName,
                t.FuelRequest.FuelRequestFees
            }).FirstOrDefault();
            bool oldFuelSurchargeStatus = false;
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    if (order.OrderAdditionalDetail != null)
                    {
                        oldFuelSurchargeStatus = order.OrderAdditionalDetail.IsFuelSurcharge;
                        order.OrderAdditionalDetail.IsFuelSurcharge = viewModel.OrderAdditionalDetails.IsFuelSurcharge;
                        order.OrderAdditionalDetail.FuelSurchagePricingType = viewModel.OrderAdditionalDetails.IsFuelSurcharge ? (int)viewModel.OrderAdditionalDetails.FuelSurchagePricingType : (int?)null;
                        Context.DataContext.Entry(order.OrderAdditionalDetail).State = EntityState.Modified;

                        var fuelFees = order.FuelRequest.FuelRequestFees.Where(t => t.FeeTypeId != (int)FeeType.SurchargeFreightFee).ToList();
                        var fuelSurchargeFreightFee = order.FuelRequest.FuelRequestFees.FirstOrDefault(t => t.FeeTypeId == (int)FeeType.SurchargeFreightFee);
                        if (fuelSurchargeFreightFee != null)
                        {
                            order.FuelRequest.FuelRequestFees.Remove(fuelSurchargeFreightFee);
                        }

                        if (viewModel.OrderAdditionalDetails.IsFuelSurcharge)
                        {
                            order.FuelRequest.FuelRequestFees = viewModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.ToFuelSurchargeEntity(fuelFees);
                        }
                    }
                    await Context.CommitAsync();
                    transaction.Commit();

                    if (viewModel.OrderAdditionalDetails.IsFuelSurcharge != oldFuelSurchargeStatus)
                    {
                        NewsfeedDomain newsfeedDomain = new NewsfeedDomain(this);
                        await thirdPartyOrderDomain.AddFuelSurchargeNotificationEvent(viewModel.OrderAdditionalDetails.IsFuelSurcharge, order.Id, userContext.Id);
                        await newsfeedDomain.SetFuelSurchargeEnableOrDisabledNewsfeed(order.Id, order.BuyerCompanyId, order.TimeZoneName, order.PoNumber, viewModel.OrderAdditionalDetails.IsFuelSurcharge, userContext);
                    }

                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageUpdateSuccess;
                }
                catch (Exception ex)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageUpdateFailed;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("OrderDomain", "UpdateFuelSurchargeFreightFee", ex.Message, ex);
                }
            }

            return response;
        }

        public async Task<StatusViewModel> UpdateFuelSurchargeForAuto(UserContext userContext, OrderDetailsViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel();
            ThirdPartyOrderDomain thirdPartyOrderDomain = new ThirdPartyOrderDomain(this);
            var order = Context.DataContext.Orders.Where(t => t.Id == viewModel.Id).Select(t => new
            {
                t.Id,
                t.FuelRequestId,
                t.FuelRequest.Currency,
                t.FuelRequest.UoM,
                t.IsFTL,
                t.FuelRequest,
                t.BuyerCompanyId,
                t.OrderAdditionalDetail,
                t.PoNumber,
                t.FuelRequest.Job.TimeZoneName,
                t.FuelRequest.FuelRequestFees
            }).FirstOrDefault();
            bool oldFuelSurchargeStatus = false;
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    oldFuelSurchargeStatus = order.OrderAdditionalDetail.IsFuelSurcharge;
                    if (order.OrderAdditionalDetail != null && viewModel.OrderAdditionalDetails.IsFuelSurcharge && viewModel.OrderAdditionalDetails.FuelSurchargeTableId.HasValue && viewModel.OrderAdditionalDetails.FuelSurchargeTableId.Value > 0)
                    {
                        var fuelSurcharge = Context.DataContext.FuelSurchargeIndexes.Where(t => t.Id == viewModel.OrderAdditionalDetails.FuelSurchargeTableId).FirstOrDefault();
                        if (fuelSurcharge != null)
                        {
                            viewModel.OrderAdditionalDetails.IsFuelSurcharge = true;

                            order.OrderAdditionalDetail.IsFuelSurcharge = viewModel.OrderAdditionalDetails.IsFuelSurcharge;
                            order.OrderAdditionalDetail.FuelSurchagePricingType = fuelSurcharge.FuelSurchargePeriod.Value;
                            order.OrderAdditionalDetail.FuelSurchargeTableType = viewModel.OrderAdditionalDetails.FuelSurchargeTableType;
                            order.OrderAdditionalDetail.FuelSurchargeTableId = viewModel.OrderAdditionalDetails.FuelSurchargeTableId;

                            var fuelFees = order.FuelRequest.FuelRequestFees.Where(t => t.FeeTypeId != (int)FeeType.SurchargeFreightFee).ToList();
                            var fuelSurchargeFreightFee = order.FuelRequest.FuelRequestFees.FirstOrDefault(t => t.FeeTypeId == (int)FeeType.SurchargeFreightFee);
                            if (fuelSurchargeFreightFee == null)
                            {
                                viewModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.FeeSubTypeId = (int)FeeSubType.FlatFee;
                                order.FuelRequest.FuelRequestFees = viewModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.ToFuelSurchargeEntity(fuelFees);
                            }
                        }

                        Context.DataContext.Entry(order.OrderAdditionalDetail).State = EntityState.Modified;
                    }
                    else
                    {
                        if (oldFuelSurchargeStatus)
                        {
                            order.OrderAdditionalDetail.IsFuelSurcharge = false;
                            order.OrderAdditionalDetail.FuelSurchargeTableType = null;
                            order.OrderAdditionalDetail.FuelSurchargeTableId = null;
                            order.OrderAdditionalDetail.FuelSurchagePricingType = null;

                            var fuelSurchargeFreightFee = order.FuelRequest.FuelRequestFees.FirstOrDefault(t => t.FeeTypeId == (int)FeeType.SurchargeFreightFee);
                            if (fuelSurchargeFreightFee != null)
                            {
                                order.FuelRequest.FuelRequestFees.Remove(fuelSurchargeFreightFee);
                            }
                        }
                    }
                    await Context.CommitAsync();
                    transaction.Commit();

                    if (viewModel.OrderAdditionalDetails.IsFuelSurcharge != oldFuelSurchargeStatus)
                    {
                        NewsfeedDomain newsfeedDomain = new NewsfeedDomain(this);
                        await thirdPartyOrderDomain.AddFuelSurchargeNotificationEvent(viewModel.OrderAdditionalDetails.IsFuelSurcharge, order.Id, userContext.Id);
                        await newsfeedDomain.SetFuelSurchargeEnableOrDisabledNewsfeed(order.Id, order.BuyerCompanyId, order.TimeZoneName, order.PoNumber, viewModel.OrderAdditionalDetails.IsFuelSurcharge, userContext);
                    }

                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageUpdateSuccess;
                }
                catch (Exception ex)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageUpdateFailed;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("OrderDomain", "UpdateFuelSurchargeForAuto", ex.Message, ex);
                }
            }

            return response;
        }

        public async Task<StatusViewModel> UpdateFreightRate(UserContext userContext, OrderDetailsViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel();
            var order = await Context.DataContext.Orders.Where(t => t.Id == viewModel.Id).Select(t => new
            {
                t.Id,
                t.OrderAdditionalDetail,
                t.FuelRequest,
            }).FirstOrDefaultAsync();
            bool oldFreightCostStatus = false;
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    oldFreightCostStatus = order.OrderAdditionalDetail.IsFreightCost;
                    if (order.OrderAdditionalDetail != null)
                    {
                        if (viewModel.OrderAdditionalDetails.FreightRateRuleType.HasValue && viewModel.OrderAdditionalDetails.IsFreightCost && viewModel.OrderAdditionalDetails.FreightRateRuleType.Value > 0)
                        {
                            order.OrderAdditionalDetail.IsFreightCost = viewModel.OrderAdditionalDetails.IsFreightCost;
                            order.OrderAdditionalDetail.FreightRateRuleType = viewModel.OrderAdditionalDetails.FreightRateRuleType;
                            order.OrderAdditionalDetail.FreightRateTableType = viewModel.OrderAdditionalDetails.FreightRateTableType;
                            order.OrderAdditionalDetail.FreightRateRuleId = viewModel.OrderAdditionalDetails.FreightRateRuleId;
                            Context.DataContext.Entry(order.OrderAdditionalDetail).State = EntityState.Modified;

                            var fuelFees = order.FuelRequest.FuelRequestFees.Where(t => t.FeeTypeId != (int)FeeType.FreightCost).ToList();
                            var freightCostFee = order.FuelRequest.FuelRequestFees.FirstOrDefault(t => t.FeeTypeId == (int)FeeType.FreightCost);
                            if (freightCostFee == null)
                            {
                                viewModel.FuelDeliveryDetails.FuelFees.FreightCostFee.FeeSubTypeId = (int)FeeSubType.FlatFee;
                                order.FuelRequest.FuelRequestFees = viewModel.FuelDeliveryDetails.FuelFees.FreightCostFee.ToFreightCostEntity(fuelFees);
                            }
                        }
                        else
                        {
                            if (oldFreightCostStatus)
                            {
                                order.OrderAdditionalDetail.IsFreightCost = false;
                                order.OrderAdditionalDetail.FreightRateRuleId = null;
                                order.OrderAdditionalDetail.FreightRateRuleType = null;
                                order.OrderAdditionalDetail.FreightRateTableType = null;

                                var freightCostFee = order.FuelRequest.FuelRequestFees.FirstOrDefault(t => t.FeeTypeId == (int)FeeType.FreightCost);
                                if (freightCostFee != null)
                                {
                                    order.FuelRequest.FuelRequestFees.Remove(freightCostFee);
                                }
                            }
                        }
                    }

                    await Context.CommitAsync();
                    transaction.Commit();

                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageUpdateSuccess;
                }
                catch (Exception ex)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageUpdateFailed;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("OrderDomain", "UpdateFreightRate", ex.Message, ex);
                }
            }

            return response;
        }

        public async Task<StatusViewModel> UpdateAccessorialFees(UserContext userContext, OrderDetailsViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel();
            ThirdPartyOrderDomain thirdPartyOrderDomain = new ThirdPartyOrderDomain(this);
            FuelFeesDomain fuelFeesDomain = new FuelFeesDomain(this);
            var order = await Context.DataContext.Orders.Where(t => t.Id == viewModel.Id).Select(t => new
            {
                t.Id,
                t.OrderAdditionalDetail,
                fuelRequestDetails = t.FuelRequest,
            }).FirstOrDefaultAsync();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    if (order.OrderAdditionalDetail != null)
                    {
                        if (viewModel.OrderAdditionalDetails.AccessorialFeeId.HasValue &&
                            order.OrderAdditionalDetail.AccessorialFeeId != viewModel.OrderAdditionalDetails.AccessorialFeeId.Value)
                        {
                            var userAddedFees = viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees;
                            var accessorialFees = await thirdPartyOrderDomain.GetAccessorialFee(viewModel.OrderAdditionalDetails.AccessorialFeeId.Value);
                            viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees = thirdPartyOrderDomain.ProcessAndRemoveDuplicateFees(userAddedFees, accessorialFees);

                            order.OrderAdditionalDetail.AccessorialFeeTableType = viewModel.OrderAdditionalDetails.AccessorialFeeTableType;
                            order.OrderAdditionalDetail.AccessorialFeeId = viewModel.OrderAdditionalDetails.AccessorialFeeId;
                            Context.DataContext.Entry(order.OrderAdditionalDetail).State = EntityState.Modified;
                        }

                        Context.DataContext.FuelRequestFees.RemoveRange(order.fuelRequestDetails.FuelRequestFees);

                        await fuelFeesDomain.SaveFuelFees(viewModel.FuelDeliveryDetails, order.fuelRequestDetails, userContext, false);

                        await Context.CommitAsync();
                        transaction.Commit();

                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMessageUpdateSuccess;
                    }
                }
                catch (Exception ex)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageUpdateFailed;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("OrderDomain", "UpdateAccessorialFees", ex.Message, ex);
                }
            }

            return response;
        }

        public List<OrderVersionsHistoryGridViewModel> GetOrderVersionsHistoryAsync(int orderId)
        {
            List<OrderVersionsHistoryGridViewModel> response = new List<OrderVersionsHistoryGridViewModel>();
            try
            {
                var orderVersions = Context.DataContext.OrderDetailVersions.Where(t => t.OrderId == orderId);
                if (orderVersions.Any(t => !t.IsActive)) // only if any edit has happened
                {
                    response = orderVersions.AsEnumerable()
                                .Join(Context.DataContext.Users, orderVersion => orderVersion.CreatedBy, user => user.Id,
                                (orderVersion, user) =>
                                new OrderVersionsHistoryGridViewModel
                                {
                                    OrderDetailVersionId = orderVersion.Id,
                                    NetDays = orderVersion.PaymentTermId == (int)PaymentTerms.NetDays ? orderVersion.NetDays.ToString() : Resource.lblHyphen,
                                    PaymentTerm = orderVersion.PaymentTerm.Name,
                                    PoNumber = orderVersion.Version > 0 ? orderVersion.PoNumber + " v(" + orderVersion.Version + ")" : orderVersion.PoNumber,
                                    ModifiedBy = user.FirstName + " " + user.LastName,
                                    DateModified = orderVersion.CreatedDate.ToString(Resource.constFormatDate)
                                }).OrderByDescending(t => t.OrderDetailVersionId).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetOrderVersionsHistoryAsync", ex.Message, ex);
            }

            return response;
        }

        public List<OrderVersionXDeliverySchedule> GetLatestOrderDeliverySchedule(IEnumerable<OrderVersionXDeliverySchedule> orderDeliverySchedules)
        {
            var maxversion = orderDeliverySchedules.OrderByDescending(t => t.Id).Select(t => t.Version).FirstOrDefault();
            if (maxversion > 0)
            {
                return (from OVDS in orderDeliverySchedules
                        where OVDS.IsActive &&
                        OVDS.Version == maxversion
                        select OVDS).ToList();
            }

            return new List<OrderVersionXDeliverySchedule>();
        }

        public OrderVersionXDeliverySchedule GetOrderDeliverySchedule(int orderId, int latestVersionId, int userId, int? deliveryScheduleId)
        {
            var orderDeliverySchedule = new OrderVersionXDeliverySchedule();

            orderDeliverySchedule.OrderId = orderId;
            orderDeliverySchedule.Version = latestVersionId + 1;
            orderDeliverySchedule.CreatedBy = userId;
            orderDeliverySchedule.CreatedDate = DateTimeOffset.Now;
            orderDeliverySchedule.IsActive = true;
            orderDeliverySchedule.DeliveryRequestId = deliveryScheduleId;

            return orderDeliverySchedule;
        }

        public async Task<StatusViewModel> UpdateWbsNumber(UserContext userContext, int fuelRequestId, string wbsNumber)
        {
            var response = new StatusViewModel(Status.Failed);

            try
            {
                var fuelRequestDetails = await Context.DataContext.FuelRequestDetails.SingleOrDefaultAsync(t => t.FuelRequestId == fuelRequestId);
                if (fuelRequestDetails != null)
                {
                    fuelRequestDetails.CustomAttribute = new CustomAttributeViewModel { WBSNumber = wbsNumber }.ToString();
                    Context.DataContext.Entry(fuelRequestDetails).State = EntityState.Modified;
                    await Context.CommitAsync();
                }
                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.successMessageWbsUpdatedSuccessfully;
            }
            catch (Exception ex)
            {
                response.StatusMessage = Resource.errMessageUpdateFailed;
                LogManager.Logger.WriteException("OrderDomain", "UpdateWbsNumber", ex.Message, ex);
            }
            return response;
        }

        public async Task<ThirdPartyOrderViewModel> GelCloneOrderDetails(int orderId, UserContext userContext)
        {
            var response = new ThirdPartyOrderViewModel();

            try
            {
                response = await InitializeTpoViewModel(userContext);
                var storedProcedureDomain = new StoredProcedureDomain(this);
                var spResponse = await storedProcedureDomain.GetTPOOrderDetails(orderId);
                response = spResponse.ToThirdPartyViewModel(response);
                response.IsCloneOrder = true;
                var fees = await Context.DataContext.Orders.Where(t => t.Id == orderId).Select(t => t.FuelRequest.FuelRequestFees).FirstOrDefaultAsync();
                if (fees != null)
                {
                    response.FuelDeliveryDetails.FuelFees.FuelRequestFees = fees.ToFeesViewModel();
                    if (response.FuelDeliveryDetails.FuelFees.FuelRequestFees.Any())
                    {
                        response.FuelDeliveryDetails.FuelFees.UoM = response.FuelDeliveryDetails.FuelFees.FuelRequestFees.FirstOrDefault().UoM;
                        response.FuelDeliveryDetails.FuelFees.Currency = response.FuelDeliveryDetails.FuelFees.FuelRequestFees.FirstOrDefault().Currency;
                    }
                }
            }
            catch (Exception ex)
            {
                response.StatusMessage = Resource.errMessageUpdateFailed;
                LogManager.Logger.WriteException("OrderDomain", "GelCloneOrderDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<ApiAddressViewModel>> GetSplitDropAddressesAsync(int orderId, int? trackableScheduleId, int? deliveryScheduleId)
        {
            List<ApiAddressViewModel> response = new List<ApiAddressViewModel>();
            try
            {
                trackableScheduleId = trackableScheduleId == 0 ? null : trackableScheduleId;
                deliveryScheduleId = deliveryScheduleId == 0 ? null : deliveryScheduleId;
                if (trackableScheduleId != null && deliveryScheduleId == null)
                {
                    deliveryScheduleId = Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(t => t.Id == trackableScheduleId).Select(t => t.DeliveryScheduleId).FirstOrDefault();
                }
                var dropAddress = await Context.DataContext.Orders.Where(t => t.Id == orderId).Select(t => new
                {
                    JobAddress = t.FuelRequest.Job.Address,
                    JobCity = t.FuelRequest.Job.City,
                    JobState = t.FuelRequest.Job.MstState.Name,
                    JobZip = t.FuelRequest.Job.ZipCode,
                    JobLatitude = t.FuelRequest.Job.Latitude,
                    JobLongitude = t.FuelRequest.Job.Longitude,
                    JobLocationType = t.FuelRequest.Job.LocationType,
                    DispatchAddresses = t.FuelDispatchLocations.Where(t1 =>
                                                    (
                                                        t1.DeliveryScheduleId == deliveryScheduleId
                                                        && (!t1.TrackableScheduleId.HasValue || t1.TrackableScheduleId == trackableScheduleId) && !t1.IsSkipped
                                                        && (!t.FuelDispatchLocations.Any(t2 => t2.ParentId == t1.Id))
                                                    )
                                            && t1.LocationType == (int)LocationType.Drop && t1.IsActive)
                                        .Select(t1 => new
                                        {
                                            t1.Id,
                                            Address = t1.Address,
                                            City = t1.City,
                                            State = t1.MstState.Name,
                                            Zip = t1.ZipCode,
                                            Latitude = t1.Latitude,
                                            Longitude = t1.Longitude,
                                            Status = t1.DropStatus,
                                            t1.IsJobLocation
                                        })
                }).FirstOrDefaultAsync();

                if (dropAddress != null)
                {
                    if (dropAddress.JobLocationType != JobLocationTypes.Various && !dropAddress.DispatchAddresses.Any(t => t.IsJobLocation))// check for isJobLocation
                    {
                        response.Add(new ApiAddressViewModel
                        {
                            Address = $"{dropAddress.JobAddress}, {dropAddress.JobCity}, {dropAddress.JobState}, {dropAddress.JobZip}",
                            Latitude = dropAddress.JobLatitude,
                            Longitude = dropAddress.JobLongitude,
                            Status = DropAddressStatus.Pending,
                            IsJobLocation = true
                        });
                    }

                    foreach (var item in dropAddress.DispatchAddresses)
                    {
                        response.Add(new ApiAddressViewModel
                        {
                            Id = item.Id,
                            Address = $"{item.Address}, {item.City}, {item.State}, {item.Zip}",
                            Latitude = item.Latitude,
                            Longitude = item.Longitude,
                            Status = item.Status,
                            IsJobLocation = item.IsJobLocation
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderController", "GetSplitDropAddressesAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> UpdateSplitDropAddressStatusAsync(ApiDispatchAddressViewModel addressViewModel)
        {
            var response = new StatusViewModel();
            try
            {
                addressViewModel.DeliveryScheduleId = addressViewModel.DeliveryScheduleId == 0 ? null : addressViewModel.DeliveryScheduleId;
                addressViewModel.TrackableScheduleId = addressViewModel.TrackableScheduleId == 0 ? null : addressViewModel.TrackableScheduleId;

                if (addressViewModel.OrderId > 0 && addressViewModel.Address != null)
                {
                    var order = await Context.DataContext.Orders.Include(t1 => t1.FuelDispatchLocations).Where(t => t.Id == addressViewModel.OrderId).FirstOrDefaultAsync();
                    //Update prev inprogress status to inactive
                    if (addressViewModel.Address.Status == DropAddressStatus.InProgress)
                    {
                        order.FuelDispatchLocations.Where(t => t.DeliveryScheduleId == addressViewModel.DeliveryScheduleId
                                                            && t.TrackableScheduleId == addressViewModel.TrackableScheduleId
                                                            && t.IsActive && t.LocationType == (int)LocationType.Drop
                                                            && t.DropStatus == DropAddressStatus.InProgress)
                                                    .ToList()
                                                    .ForEach(t => t.DropStatus = DropAddressStatus.Pending);
                    }
                    if (addressViewModel.Address.Id > 0)
                    {
                        var address = order.FuelDispatchLocations.Where(t => t.Id == addressViewModel.Address.Id).FirstOrDefault();
                        if (address != null)
                        {
                            if (address.TrackableScheduleId == addressViewModel.TrackableScheduleId)
                            {
                                address.DropStatus = addressViewModel.Address.Status;
                                Context.DataContext.Entry(address).State = EntityState.Modified;

                                response.EntityId = addressViewModel.Address.Id;
                            }
                            else
                            {
                                var newAddress = new FuelDispatchLocation(address);
                                newAddress.TrackableScheduleId = addressViewModel.TrackableScheduleId;
                                newAddress.ParentId = address.Id;
                                newAddress.DropStatus = addressViewModel.Address.Status;
                                Context.DataContext.FuelDispatchLocations.Add(newAddress);

                                response.EntityId = newAddress.Id;
                            }
                            await Context.CommitAsync();

                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.errMessageSuccess;
                        }
                    }
                    else if (addressViewModel.Address.IsJobLocation)
                    {
                        var jobAddress = new FuelDispatchLocation
                        {
                            LocationType = (int)LocationType.Drop,
                            DeliveryScheduleId = addressViewModel.DeliveryScheduleId,
                            TrackableScheduleId = addressViewModel.TrackableScheduleId,
                            CreatedBy = addressViewModel.UserId,
                            CreatedDate = DateTimeOffset.Now,
                            IsJobLocation = true,
                            DropStatus = addressViewModel.Address.Status,
                            Address = order.FuelRequest.Job.Address,
                            City = order.FuelRequest.Job.City,
                            StateId = order.FuelRequest.Job.StateId,
                            StateCode = order.FuelRequest.Job.MstState.Code,
                            ZipCode = order.FuelRequest.Job.ZipCode,
                            Currency = order.FuelRequest.Currency,
                            Latitude = order.FuelRequest.Job.Latitude,
                            Longitude = order.FuelRequest.Job.Longitude,
                            IsActive = true,
                            CountryCode = order.FuelRequest.Job.MstCountry.Code,
                            CountyName = order.FuelRequest.Job.CountyName
                        };
                        order.FuelDispatchLocations.Add(jobAddress);
                        await Context.CommitAsync();

                        response.EntityId = jobAddress.Id;
                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMessageSuccess;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderController", "UpdateSplitDropAddressStatusAsync", ex.Message, ex);
            }
            return response;
        }

        private async Task<List<OrderTaxDetailsViewModel>> GetOrderTaxDetailsAsync(int orderId)
        {
            StoredProcedureDomain storedProcedureDomain = new StoredProcedureDomain(this);
            var taxDetails = await storedProcedureDomain.GetOrderTaxDetailsAsync(orderId);
            taxDetails.ForEach(t => t.TaxRate = t.TaxRate.GetPreciseValue(6));
            return taxDetails;
        }

        private async Task<OrderBadgeViewModel> GetOrderBadgeDetailsAsync(int orderId)
        {
            var response = new OrderBadgeViewModel();
            var lstTerminalBulkBadge = new List<TerminalBulkBadgeViewModel>();
            var order = await Context.DataContext.OrderBadgeDetails.Where(t => t.OrderId == orderId && t.IsActive).ToListAsync();
            foreach (var item in order)
            {
                if (item.IsCommonBadge)
                {
                    response.BadgeNo1 = item.BadgeNo1;
                    response.BadgeNo2 = item.BadgeNo2;
                    response.BadgeNo3 = item.BadgeNo3;
                }
                else
                {
                    var terminalBulkBadge = new TerminalBulkBadgeViewModel();
                    terminalBulkBadge.BadgeNo1 = item.BadgeNo1;
                    terminalBulkBadge.BadgeNo2 = item.BadgeNo2;
                    terminalBulkBadge.BadgeNo3 = item.BadgeNo3;
                    if (item.PickupLocationType == PickupLocationType.Terminal)
                    {
                        terminalBulkBadge.TerminalId = item.TerminalId;
                        terminalBulkBadge.TerminalBulkPlantName = item.TerminalId.HasValue ? item.MstExternalTerminal.Name : string.Empty;
                        terminalBulkBadge.IsPickupTerminal = true;
                    }
                    else
                    {
                        terminalBulkBadge.BulkPlantId = item.BulkPlantId;
                        terminalBulkBadge.TerminalBulkPlantName = item.BulkPlantId.HasValue ? item.BulkPlantLocation.Name : string.Empty;
                        terminalBulkBadge.IsPickupTerminal = false;
                    }
                    lstTerminalBulkBadge.Add(terminalBulkBadge);
                }
            }
            response.TerminalBulkBadge = lstTerminalBulkBadge;
            return response;
        }

        private async Task<OrderVersionViewModel> GetOrderLatestDeliverySchedulesAsync(int orderId, DateTimeOffset currentDate, TimeSpan currentTime)
        {
            StoredProcedureDomain storedProcedureDomain = new StoredProcedureDomain(this);
            var scheduleDetails = await storedProcedureDomain.GetOrderDeliveryScheduleDetailsAsync(orderId, currentDate, currentTime);
            return scheduleDetails.ToViewModel();
        }

        public async Task<StatusViewModel> SaveTankSchedulesAsync(UserContext userContext, TankRentalFrequencyViewModel viewModel, bool isBuyer)
        {
            StatusViewModel response = new StatusViewModel();
            viewModel.CreatedBy = userContext.Id;
            viewModel.CreatedDate = DateTimeOffset.Now;

            var order = await Context.DataContext.FuelRequests.Where(t => t.Id == viewModel.FuelRequestId)
                                        .Select(t => new { t.TankRentals })
                                        .SingleOrDefaultAsync();

            if (order != null)
            {
                var existingFrequency = order.TankRentals.FirstOrDefault(t => t.FrequencyTypeId == (int)viewModel.FrequencyTypes);

                //if (existingFrequency != null && existingFrequency.ActivationStatusId == viewModel.ActivationStatusId)
                //{
                //    response.StatusMessage = "Schedule already exists for same frequency type. Please choose different frequency type.";
                //    return response;
                //}

                if (viewModel.Tanks == null || !viewModel.Tanks.Any())
                {
                    response.StatusMessage = "Please add at least one tank in the schedule.";
                    return response;
                }

                var tankFrequency = viewModel.ToEntity();

                if (tankFrequency != null)
                {
                    using (var transaction = Context.DataContext.Database.BeginTransaction())
                    {
                        try
                        {
                            if (viewModel.TankRentalFrequencyId > 0)
                            {
                                var updateExistingFreq = Context.DataContext.Database
                                        .ExecuteSqlCommand("UPDATE FuelRequestTankRentalFrequencies SET ActivationStatusId = {0}, UpdatedBy = {1}, UpdatedDate= {2} WHERE Id = {3}"
                                                        , (int)ActivationStatus.Deleted, viewModel.CreatedBy, viewModel.CreatedDate, viewModel.TankRentalFrequencyId);
                                var updateExistingTanks = Context.DataContext.Database
                                        .ExecuteSqlCommand("UPDATE TankDetails SET ActivationStatusId = {0}, UpdatedBy = {1}, UpdatedDate= {2} WHERE RentalFrequencyId = {3}"
                                                        , (int)ActivationStatus.Deleted, viewModel.CreatedBy, viewModel.CreatedDate, viewModel.TankRentalFrequencyId);
                                await Context.CommitAsync();
                            }
                            else if (existingFrequency != null)
                            {
                                var updateExistingFreq = Context.DataContext.Database
                                        .ExecuteSqlCommand("UPDATE FuelRequestTankRentalFrequencies SET ActivationStatusId = {0}, UpdatedBy = {1}, UpdatedDate= {2} WHERE Id = {3}"
                                                        , (int)ActivationStatus.Deleted, viewModel.CreatedBy, viewModel.CreatedDate, existingFrequency.Id);
                                var updateExistingTanks = Context.DataContext.Database
                                        .ExecuteSqlCommand("UPDATE TankDetails SET ActivationStatusId = {0}, UpdatedBy = {1}, UpdatedDate= {2} WHERE RentalFrequencyId = {3}"
                                                        , (int)ActivationStatus.Deleted, viewModel.CreatedBy, viewModel.CreatedDate, existingFrequency.Id);
                                await Context.CommitAsync();
                            }

                            Context.DataContext.FuelRequestTankRentalFrequencies.Add(tankFrequency);
                            await Context.CommitAsync();

                            transaction.Commit();

                            response.StatusCode = Status.Success;
                            response.StatusMessage = "Tank schedule updated sucessfully";

                            //add new frequencies
                        }
                        catch (Exception ex)
                        {
                            response.StatusMessage = Resource.errMessageSaveTankSchedulesFailed;
                            transaction.Rollback();
                            LogManager.Logger.WriteException("OrderDomain", "SaveTankSchedulesAsync", ex.Message, ex);
                        }
                    }
                }
            }

            return response;
        }

        public async Task<StatusViewModel> RemoveTankScheduleAsync(UserContext userContext, int freqId, int frId)
        {
            StatusViewModel response = new StatusViewModel();

            var tankFrequency = await Context.DataContext.FuelRequestTankRentalFrequencies
                                    .Where(t => t.FuelRequestId == frId && t.FrequencyTypeId == freqId &&
                                    (t.ActivationStatusId == (int)ActivationStatus.Created || t.ActivationStatusId == (int)ActivationStatus.Active))
                                    .SingleOrDefaultAsync();

            if (tankFrequency != null)
            {
                if (tankFrequency != null && tankFrequency.TankDetails.Any())
                {
                    using (var transaction = Context.DataContext.Database.BeginTransaction())
                    {
                        try
                        {

                            tankFrequency.ActivationStatusId = (int)ActivationStatus.Deleted;
                            tankFrequency.UpdatedBy = userContext.Id;
                            tankFrequency.UpdatedDate = DateTimeOffset.Now;

                            var updateExistingTanks = Context.DataContext.Database
                                        .ExecuteSqlCommand("UPDATE TankDetails SET ActivationStatusId = {0}, UpdatedBy = {1}, UpdatedDate= {2} WHERE RentalFrequencyId = {3}"
                                                        , (int)ActivationStatus.Deleted, userContext.Id, DateTimeOffset.Now, tankFrequency.Id);

                            Context.DataContext.Entry(tankFrequency).State = EntityState.Modified;
                            await Context.CommitAsync();

                            //CONFIRMED FROM SARAHA, WE DONT NEED TO WRITE THIS, BUT KEPT FOR FUTURE REFERENCE
                            //if (tankFrequency.ScheduleStartDate.Date < DateTimeOffset.Now.Date)
                            //{
                            //    var ord = tankFrequency.FuelRequest.Orders.FirstOrDefault();
                            //    if (ord != null)
                            //    {
                            //        var queueMsg = new TankRentalQueueMessage
                            //        {
                            //            OrderId = ord.Id,
                            //            RentalFrequencyId = tankFrequency.Id,
                            //            TimeZoneName = tankFrequency.FuelRequest.Job.TimeZoneName,
                            //            UserFirstName = userContext.Name,
                            //            UserLastName = userContext.Name,
                            //            CompanyName = userContext.CompanyName,
                            //            IsClosedOrder = false,
                            //            StartDate = tankFrequency.ScheduleStartDate,
                            //            EndDate = DateTimeOffset.Now.Date
                            //        };
                            //        var queueList = new List<TankRentalQueueMessage>
                            //        {
                            //            queueMsg
                            //        };

                            //        var queResutl = new TankRentalInvoiceDomain().SaveTankRentalInvoiceCreateMessage(queueList, userContext.Id);
                            //    }
                            //}

                            transaction.Commit();

                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.successMsgTankRemoved;

                            //add new frequencies
                        }
                        catch (Exception ex)
                        {
                            response.StatusMessage = Resource.errMessageSaveTankSchedulesFailed;
                            transaction.Rollback();
                            LogManager.Logger.WriteException("OrderDomain", "SaveTankSchedulesAsync", ex.Message, ex);
                        }
                    }
                }
            }
            else
            {
                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.successMsgTankRemoved;
            }

            return response;
        }

        public async Task<List<string>> CreateOrderTankMappingInFreightService(OrderTankMappingProcessorReqViewModel viewModel)
        {
            var errorInfo = new List<string>();
            using (var tracer = new Tracer("OrderDomain", "CreateOrderTankMappingInFreightService"))
            {
                StringBuilder processMessage = new StringBuilder();
                try
                {
                    if (viewModel.IsAssetTrackingEnabled)
                    {
                        //get order and tank details
                        var mappingsToUpdate = await GetOrderTankMappingDetails(viewModel);
                        //call freight service to add mapping
                        var saveMappings = await new FreightServiceDomain(this).CrateOrderTankMappings(mappingsToUpdate);
                    }
                }
                catch (Exception ex)
                {
                    if (!(ex is QueueMessageFatalException))
                    {
                        LogManager.Logger.WriteException("OrderDomain", "CreateOrderTankMappingInFreightService", ex.Message, ex);
                    }

                    if (processMessage.Length == 0)
                    {
                        processMessage.Append(Constants.RequestError);
                        errorInfo.Add(processMessage.ToString());
                    }
                    throw new QueueMessageFatalException(errorInfo[0], errorInfo);
                }
                return errorInfo;
            }
        }

        private async Task<List<OrderTankMappingViewModel>> GetOrderTankMappingDetails(OrderTankMappingProcessorReqViewModel viewModel)
        {
            var orderProductCategory = Context.DataContext.Orders.Where(t => t.Id == viewModel.OrderId).Select(t => t.FuelRequest.MstProduct.ProductTypeId).FirstOrDefault();

            var orderDetails = await Context.DataContext.JobXAssets.Where(t => t.Asset.Type == (int)AssetType.Tank
                                    && t.JobId == viewModel.JobId && t.Asset.MstProductType.Id == orderProductCategory)
                                .Select(t => new
                                {
                                    t.JobId,
                                    t.Asset.AssetAdditionalDetail.VehicleId,
                                    t.Asset.FuelType,
                                    ProductTypeId = t.Asset.MstProductType.Id,
                                }).ToListAsync();
            List<OrderTankMappingViewModel> mappings = new List<OrderTankMappingViewModel>();
            foreach (var item in orderDetails)
            {
                var mappingViewModel = new OrderTankMappingViewModel()
                {
                    FuelTypeId = item.FuelType ?? 0,
                    IsActive = true,
                    JobId = item.JobId,
                    OrderId = viewModel.OrderId,
                    ProductTypeId = (int)item.ProductTypeId.GetProductCategoryType(),
                    SupplierCompanyId = viewModel.SupplierCompanyId,
                    TankId = item.VehicleId,
                    CreatedBy = viewModel.CreatedBy,
                };
                mappings.Add(mappingViewModel);
            }

            return mappings;

        }

        public async Task<ThirdPartyOrderViewModel> InitializeTpoViewModel(UserContext userContext)
        {
            var response = new ThirdPartyOrderViewModel();
            try
            {
                var onboardingPreferencesSetting = await Context.DataContext.OnboardingPreferences
                                                                            .Where(t => t.IsActive && t.CompanyId == userContext.CompanyId)
                                                                            .OrderByDescending(t => t.Id)
                                                                            .Select(t => new
                                                                            {
                                                                                t.Id,
                                                                                t.DeliveryType,
                                                                                t.IsCustomerInvitationEnabled,
                                                                                t.IsBuySellEnabled,
                                                                                t.IsThirdPartyHardwareUsed,
                                                                                t.PreferencePricingMethod,
                                                                                t.FreightOnBoardType,
                                                                                t.IsSupressOrderPricing,
                                                                                t.IsDropTicketImageRequired,
                                                                                t.IsFreightOnlyOrderEnabled,
                                                                                t.IsDriverProdutDisplayEnable,
                                                                                t.LocationManagedType,
                                                                                t.IsBadgeMandatory,
                                                                                t.FreightPricingMethod,
                                                                                t.IsAdditiveBlendingEnabled
                                                                            })
                                                                            .FirstOrDefaultAsync();
                if (onboardingPreferencesSetting != null)
                {
                    response.PreferencesSetting = new OnboardingPreferenceViewModel();
                    response.PreferencesSetting.Id = onboardingPreferencesSetting.Id;
                    response.PreferencesSetting.PreferencePricingMethod = onboardingPreferencesSetting.PreferencePricingMethod;
                    response.PreferencesSetting.IsSupressOrderPricing = onboardingPreferencesSetting.IsSupressOrderPricing;
                    response.PreferencesSetting.IsDriverProdutDisplayEnable = onboardingPreferencesSetting.IsDriverProdutDisplayEnable;
                    response.PreferencesSetting.IsFreightOnlyOrderEnabled = onboardingPreferencesSetting.IsFreightOnlyOrderEnabled;
                    response.IsSupressOrderPricing = response.PreferencesSetting.IsSupressOrderPricing;
                    if (response.PreferencesSetting.IsSupressOrderPricing && response.OrderAdditionalDetailsViewModel != null)
                        response.OrderAdditionalDetailsViewModel.BOLInvoicePreferenceTypes = InvoiceNotificationPreferenceTypes.None;
                    response.IsOnboardingPreferenceExists = true;
                    response.FuelDetails.FuelPricing.FuelPricingDetails.FreightOnBoardTypes = onboardingPreferencesSetting.FreightOnBoardType;
                    response.FuelDetails.FuelPricing.FuelPricingDetails.TruckLoadTypes = onboardingPreferencesSetting.DeliveryType;
                    if (response.FuelDetails.FuelPricing.FuelPricingDetails.TruckLoadTypes == TruckLoadTypes.FullTruckLoad)
                    {
                        response.OrderAdditionalDetailsViewModel.IsDriverToUpdateBOL = true;
                    }
                    if (response.OrderAdditionalDetailsViewModel != null)
                        response.OrderAdditionalDetailsViewModel.FreightPricingMethod = onboardingPreferencesSetting.FreightPricingMethod;
                    response.IsInvitationEnabled = onboardingPreferencesSetting.IsCustomerInvitationEnabled;
                    response.IsBuyAndSellOrder = onboardingPreferencesSetting.IsBuySellEnabled;
                    response.IsThirdPartyHardwareUsed = onboardingPreferencesSetting.IsThirdPartyHardwareUsed;
                    response.FuelDeliveryDetails.IsDropImageRequired = onboardingPreferencesSetting.IsDropTicketImageRequired;
                    //response.OrderAdditionalDetailsViewModel.LocationManagedType = onboardingPreferencesSetting.LocationManagedType;
                    response.IsBadgeMandatory = onboardingPreferencesSetting.IsBadgeMandatory;
                    response.PreferencesSetting.IsAdditiveBlendingEnabled = onboardingPreferencesSetting.IsAdditiveBlendingEnabled;
                }
                else
                {
                    response.FuelDetails.FuelPricing.FuelPricingDetails.FreightOnBoardTypes = FreightOnBoardTypes.Destination;
                }

                var allowedSize = SiteFuel.Exchange.Core.Utilities.AppSettings.MaxAllowedUploadFileSize;
                response.MaxAllowedFileSize = allowedSize;
                response.FuelDetails.FuelDisplayGroupId = (int)ProductDisplayGroups.CommonFuelType;
                response.FuelDetails.FuelQuantity.QuantityTypeId = (int)QuantityType.NotSpecified;
                response.FuelDeliveryDetails.FuelRequestFee.DeliveryFeeByQuantity.Add(new DeliveryFeeByQuantityViewModel());
                response.OrderAdditionalDetailsViewModel.SupplierSource = new SupplierSourceViewModel();
                var masterDomain = new MasterDomain(this);
                var defaultaddresscountryId = masterDomain.GetDefaultServingCountry(userContext.CompanyId);
                var defaultCurrency = masterDomain.GetDefaultCurrencyForCompany(userContext.CompanyId);
                var defaultUoM = masterDomain.GetDefaultUoMforCompany(userContext.CompanyId);
                response.AddressDetails.Country.Id = defaultaddresscountryId;
                response.AddressDetails.Country.Currency = defaultCurrency;
                response.AddressDetails.Country.UoM = defaultUoM;
                response.ForcastingPreference.IsEditable = true;
                response.ForcastingServiceSetting.IsEditableTpo = true;

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "InitializeTpoViewModel", ex.Message, ex);
            }

            return response;
        }

        public InvoiceFilterViewModel GetUoMandCurrencyForOrder(int orderId)
        {
            var response = new InvoiceFilterViewModel();
            response.OrderId = orderId;
            try
            {
                var result = (from O in Context.DataContext.Orders
                              where O.Id == orderId
                              join fr in Context.DataContext.FuelRequests
                            on O.FuelRequestId equals
                            fr.Id
                              select new
                              {
                                  Currency = fr.Currency,
                                  UoM = fr.UoM
                              }).FirstOrDefault();
                if (result != null)
                {
                    response.Currency = result.Currency;
                    response.UoM = result.UoM;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetUoMandCurrencyForOrder", ex.Message, ex);

            }
            return response;
        }

        /// <summary>
        /// Gets the name of the assigned product.
        /// </summary>
        /// <param name="terminalId">The terminal identifier.</param>
        /// <param name="orderId">The order identifier.</param>
        /// <returns></returns>
        public async Task<ProductMappingViewModel> GetAssignedProductName(int terminalId, int orderId)
        {
            var response = new ProductMappingViewModel();
            var helperDomain = new HelperDomain(this);

            try
            {
                var order = await Context.DataContext.Orders.Where(t => t.Id == orderId)
                                                            .Select(t => new { t.AcceptedCompanyId, t.FuelRequest.FuelTypeId })
                                                            .FirstOrDefaultAsync();
                if (order != null)
                {
                    var tfxProductTypeId = Context.DataContext.MstProducts.Where(t => t.Id == order.FuelTypeId && t.IsActive)
                                                              .Select(t => t.TfxProductId)
                                                              .FirstOrDefault() ?? 0;
                    response = helperDomain.GetSupplierAssignedProductName(order.AcceptedCompanyId, tfxProductTypeId, terminalId);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetAssignedProductName", ex.Message, ex);
            }

            return response;
        }

        public async Task<StatusViewModel> UpdateAssetDropQuantity(UpdateAssetDropQuantityViewModel viewModel)
        {
            var response = new StatusViewModel(Status.Failed);
            var isValidAssetDropId = true;
            List<int> assetDropIds = new List<int>();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    if (viewModel.AssetDropDetail == null)
                    {
                        var assetDrop = await Context.DataContext.AssetDrops.FirstOrDefaultAsync(t => t.Id == viewModel.AssetDropId);
                        if (assetDrop != null)
                        {
                            LogManager.Logger.WriteDebug("OrderDomain", "UpdateAssetDropQuantity",
                               " Gravity = " + viewModel.Gravity + " DroppedGallons=" + viewModel.Quantity
                               + " AssetDropId=" + viewModel.AssetDropId + " DriverId=" + viewModel.DriverId);

                            assetDrop.DroppedGallons = viewModel.Quantity;
                            assetDrop.MeterStartReading = viewModel.PrimaryMeterStartReading;
                            assetDrop.MeterEndReading = viewModel.PrimaryMeterEndReading;
                            var gravity = viewModel.Gravity == null ? string.Empty : viewModel.Gravity.Replace(",", string.Empty);
                            assetDrop.Gravity = string.IsNullOrEmpty(gravity) ? (decimal?)null : Convert.ToDecimal(gravity);
                            assetDrop.UpdatedDate = DateTimeOffset.Now;
                            assetDrop.UpdatedBy = viewModel.DriverId;
                            Context.DataContext.Entry(assetDrop).State = EntityState.Modified;
                            await Context.CommitAsync();

                            transaction.Commit();
                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.successMessageDropQuantityUpdatedSuccessfully;
                        }
                        else
                        {
                            response.StatusMessage = string.Format(Resource.valMessageInvalid, "AssetDropId");
                        }
                    }
                    else
                    {
                        foreach (var item in viewModel.AssetDropDetail)
                        {
                            var assetDrop = await Context.DataContext.AssetDrops.FirstOrDefaultAsync(t => t.Id == item.AssetDropId);
                            if (assetDrop != null)
                            {
                                assetDrop.DroppedGallons = item.Quantity;
                                var gravity = item.Gravity == null ? string.Empty : item.Gravity.Replace(",", string.Empty);
                                assetDrop.Gravity = string.IsNullOrEmpty(gravity) ? (decimal?)null : Convert.ToDecimal(gravity);
                                assetDrop.UpdatedDate = DateTimeOffset.Now;
                                assetDrop.UpdatedBy = viewModel.DriverId;
                                Context.DataContext.Entry(assetDrop).State = EntityState.Modified;
                                await Context.CommitAsync();
                            }
                            else
                            {
                                isValidAssetDropId = false;
                                assetDropIds.Add(item.AssetDropId);
                            }
                        }

                        if (isValidAssetDropId)
                        {
                            transaction.Commit();
                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.successMessageDropQuantityUpdatedSuccessfully;
                        }
                        else
                        {
                            response.StatusMessage = string.Format(Resource.valMessageInvalid, "AssetDropIds:" + string.Join(",", assetDropIds));
                        }
                    }
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageUpdateFailed;
                    LogManager.Logger.WriteException("OrderDomain", "UpdateAssetDropQuantity", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusViewModel> UpdateOrderNameAsync(UserContext userContext, int orderId, string orderName)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                var order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == orderId);
                string currentOrderName = order.Name;
                if (order != null)
                {
                    var previousOrderName = order.Name;
                    if (previousOrderName != orderName)
                    {
                        order.Name = orderName;
                        order.UpdatedDate = DateTimeOffset.Now;
                        var currentOrderVersion = order.OrderDetailVersions.LastOrDefault(t => t.IsActive);
                        order.OrderDetailVersions.Add(GetNewOrderDetailVersion(currentOrderVersion, userContext.Id, order.PoNumber, order.FuelRequest.PaymentTermId, order.FuelRequest.NetDays, order.FuelRequest.FuelRequestDetail.PaymentMethod, EditPropertyType.OrderName, new { OrderName = orderName }));
                        Context.DataContext.Entry(order).State = EntityState.Modified;
                        await Context.CommitAsync();
                    }
                }
                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.successMessageOrderNameUpdatedSuccessfully;

                NewsfeedDomain newsfeedDomain = new NewsfeedDomain(this);
                await newsfeedDomain.SetThirdPartyOrderEditedNewsfeed(userContext, order, currentOrderName, order.Name);
            }
            catch (Exception ex)
            {
                response.StatusMessage = Resource.errMessageUpdateFailed;
                LogManager.Logger.WriteException("OrderDomain", "UpdateOrderNameAsync", ex.Message, ex);
            }
            return response;
        }
        public async Task<StatusViewModel> UpdateIsDispatchRetainedForBrokerOrders(int OrderId, bool IsDispatchRetained, UserContext userContext)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {

                var order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == OrderId);
                if (order != null)
                {
                    if (order.FuelRequest.FuelRequest1 != null && order.FuelRequest.GetParentFuelRequest().FuelRequest1 != null)
                    {
                        var parentOrder = order.FuelRequest.GetParentFuelRequest().FuelRequest1.Orders.LastOrDefault();
                        if (parentOrder != null)
                        {
                            parentOrder.FuelRequest.FuelRequestDetail.IsDispatchRetainedByCustomer = IsDispatchRetained;
                            parentOrder.UpdatedDate = DateTimeOffset.Now;
                            parentOrder.UpdatedBy = userContext.Id;
                            Context.DataContext.Entry(parentOrder).State = EntityState.Modified;

                            order.FuelRequest.FuelRequestDetail.IsDispatchRetainedByCustomer = IsDispatchRetained;
                            order.UpdatedDate = DateTimeOffset.Now;
                            order.UpdatedBy = userContext.Id;
                            Context.DataContext.Entry(order).State = EntityState.Modified;
                            await Context.CommitAsync();
                        }
                    }

                }
                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.successMessageIsDispatchRetained;
            }
            catch (Exception ex)
            {
                response.StatusMessage = Resource.errMessageUpdateFailed;
                LogManager.Logger.WriteException("OrderDomain", "UpdateIsDispatchRetainedForBrokerOrders", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<UspCarrierMapping>> GetCarrierData(UserContext userContext, int countryId)
        {
            using (var tracer = new Tracer("OrderDomain", "GetCarrierData"))
            {
                var carrierMapping = new List<UspCarrierMapping>();
                try
                {
                    var storedProcedureDomain = new StoredProcedureDomain(this);
                    carrierMapping = await storedProcedureDomain.GetCarrierMapping(userContext.CompanyId, countryId);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderDomain", "GetCarrierData", ex.Message, ex);
                }
                return carrierMapping;
            }
        }

        //public StatusViewModel CheckDuplicateCarrierId(UspCarrierMapping carrierMapping, UserContext userContext)
        //{
        //    var response = new StatusViewModel();
        //    using (var tracer = new Tracer("OrderDomain", "CheckDuplicateCarrierId"))
        //    {
        //        try
        //        {
        //            if (!string.IsNullOrEmpty(carrierMapping.AssignedCarrierId))
        //            {
        //                var mappingInfo = Context.DataContext.CarrierMappings.FirstOrDefault(t => t.AssignedCarrierId.ToLower().Trim() == carrierMapping.AssignedCarrierId.ToLower().Trim() && t.CompanyId == carrierMapping.CompanyId && t.IsActive);

        //                if (mappingInfo != null)
        //                {
        //                    response.StatusCode = Status.Warning;
        //                    response.StatusMessage = Resource.warningMsgCarrierIdExist;
        //                }
        //                else { response.StatusCode = Status.Success; }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            LogManager.Logger.WriteException("OrderDomain", "CheckDuplicateCarrierId", ex.Message, ex);
        //        }
        //        return response;
        //    }
        //}

        //public async Task<bool> CheckDuplicateCarrierMapping(List<int> TerminalIds, List<int> BulkPlantIds, int countryId, int carrierCompanyId)
        //{
        //    var terminalMappings = await Context.DataContext.CarrierMappings.Where(w => w.IsActive == true && w.CountryId == countryId && w.CarrierCompanyId == carrierCompanyId && (TerminalIds.Contains(w.TerminalId.Value) || BulkPlantIds.Contains(w.BulkPlantId.Value))).ToListAsync();
        //    if (terminalMappings != null && terminalMappings.Count() > 0)
        //    {
        //        return false;
        //    }
        //    return true;
        //}
        //public async Task<StatusViewModel> SaveAndUpdateCarrierMapping(UspCarrierMapping uspCarrierMapping, UserContext userContext)
        //{
        //    var response = new StatusViewModel();
        //    using (var tracer = new Tracer("OrderDomain", "SaveAndUpdateCarrierMapping"))
        //    {
        //        try
        //        {
        //            if (uspCarrierMapping.Id != null)
        //            {
        //                var mappingInfo = Context.DataContext.CarrierMappings.FirstOrDefault(t => t.Id == uspCarrierMapping.Id.Value && t.CompanyId == uspCarrierMapping.CompanyId && t.IsActive);
        //                mappingInfo.AssignedCarrierId = uspCarrierMapping.AssignedCarrierId.Trim();
        //                mappingInfo.IsActive = uspCarrierMapping.IsActive;
        //                mappingInfo.UpdatedBy = userContext.Id;
        //                mappingInfo.UpdatedDate = DateTime.Now;
        //                response.StatusMessage = Resource.msgCarrierMappingUpdate;
        //                response.StatusCode = Status.Success;
        //                await Context.CommitAsync();
        //                return response;
        //            }
        //            else
        //            {
        //                List<int> terminalIdList = new List<int>();
        //                List<int> bulkPlantIdList = new List<int>();
        //                if (uspCarrierMapping.TerminalIds != null)
        //                    terminalIdList = uspCarrierMapping.TerminalIds.Split(',').Select(int.Parse).ToList();

        //                if (uspCarrierMapping.BulkPlantIds != null)
        //                    bulkPlantIdList = uspCarrierMapping.BulkPlantIds.Split(',').Select(int.Parse).ToList();

        //                if (!(terminalIdList.Count() > 0 || bulkPlantIdList.Count() > 0))
        //                {
        //                    response.StatusCode = Status.Warning;
        //                    response.StatusMessage = Resource.msgCarrierMappingFailed;
        //                    return response;
        //                }
        //                else if (String.IsNullOrEmpty(uspCarrierMapping.AssignedCarrierId))
        //                {
        //                    response.StatusCode = Status.Warning;
        //                    response.StatusMessage = Resource.msgCarrierMappingFailed;
        //                    return response;
        //                }
        //                bool isDuplicate = await CheckDuplicateCarrierMapping(terminalIdList, bulkPlantIdList, uspCarrierMapping.CountryId, uspCarrierMapping.CarrierCompanyId);
        //                if (isDuplicate)
        //                {
        //                    var carrierMappingList = new List<CarrierMapping>();
        //                    if (uspCarrierMapping != null)
        //                    {
        //                        foreach (var item in terminalIdList)
        //                        {
        //                            var carriermapping = new CarrierMapping();
        //                            carriermapping.CarrierCompanyId = uspCarrierMapping.CarrierCompanyId;
        //                            carriermapping.CompanyId = userContext.CompanyId;
        //                            carriermapping.AssignedCarrierId = uspCarrierMapping.AssignedCarrierId.Trim();
        //                            carriermapping.CreatedBy = userContext.Id;
        //                            carriermapping.IsActive = true;
        //                            carriermapping.CreatedDate = DateTime.Now;
        //                            carriermapping.TerminalId = item;
        //                            carriermapping.CountryId = uspCarrierMapping.CountryId;
        //                            carrierMappingList.Add(carriermapping);
        //                        }
        //                        foreach (var item in bulkPlantIdList)
        //                        {
        //                            var carriermapping = new CarrierMapping();
        //                            carriermapping.CarrierCompanyId = uspCarrierMapping.CarrierCompanyId;
        //                            carriermapping.CompanyId = userContext.CompanyId;
        //                            carriermapping.AssignedCarrierId = uspCarrierMapping.AssignedCarrierId.Trim();
        //                            carriermapping.CreatedBy = userContext.Id;
        //                            carriermapping.IsActive = true;
        //                            carriermapping.CreatedDate = DateTime.Now;
        //                            carriermapping.BulkPlantId = item;
        //                            carriermapping.CountryId = uspCarrierMapping.CountryId;
        //                            carrierMappingList.Add(carriermapping);
        //                        }
        //                        Context.DataContext.CarrierMappings.AddRange(carrierMappingList);
        //                        response.StatusMessage = Resource.msgCarrierMappingSave;
        //                        response.StatusCode = Status.Success;
        //                        await Context.CommitAsync();
        //                    }
        //                }
        //                else
        //                {
        //                    response.StatusCode = Status.Warning;
        //                    response.StatusMessage = Resource.msgCarrierMappingExists;
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            response.StatusCode = Status.Failed;
        //            response.StatusMessage = Resource.msgCarrierMappingFailed;
        //            LogManager.Logger.WriteException("OrderDomain", "SaveAndUpdateCarrierMapping", ex.Message, ex);
        //        }
        //    }
        //    return response;
        //}



        public async Task<StatusViewModel> AssignNewTerminalForTierPricedOrder(int terminalId, int orderId)
        {

            var response = new StatusViewModel(Status.Failed);
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var requestPriceDetails = from O in Context.DataContext.Orders
                                              join FRPD in Context.DataContext.FuelRequestPricingDetails
                                              on O.FuelRequestId equals FRPD.FuelRequestId
                                              where O.Id == orderId
                                              select new
                                              {
                                                  requestPriceDetailsId = FRPD.RequestPriceDetailId,
                                                  order = O
                                              };

                    var requestPriceDetailsId = requestPriceDetails != null ? requestPriceDetails.FirstOrDefault().requestPriceDetailsId : 0;
                    var Order = requestPriceDetails != null ? requestPriceDetails.FirstOrDefault().order : null;


                    if (requestPriceDetails != null && requestPriceDetailsId > 0)
                    {
                        if (Order != null)
                        {
                            Order.TerminalId = terminalId;
                        }
                        var IsSuccess = await new PricingServiceDomain().AssignNewTerminalForTierPricedOrder(terminalId, requestPriceDetailsId);
                        if (IsSuccess)
                        {
                            Context.DataContext.Entry(Order).State = EntityState.Modified;
                            Context.Commit();
                            transaction.Commit();
                            response.StatusCode = Status.Success;
                            response.StatusMessage = string.Format(Resource.errMessageTerminalAssignmentSuccess, string.Empty);
                        }
                        else
                        {
                            transaction.Rollback();
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = string.Format(Resource.errMessageFailedToAssignTerminal, string.Empty);
                        }
                        //assing new terminal to all chained orders in broker case
                        if (IsSuccess && Order != null && Order.FuelRequest.FuelRequest1 != null
                            && Order.FuelRequest.GetParentFuelRequest().FuelRequest1 != null)
                        {
                            var brokeredOrder = Order.FuelRequest.GetParentFuelRequest().FuelRequest1.Orders.LastOrDefault();
                            if (brokeredOrder != null)
                            {
                                await AssignNewTerminalForTierPricedOrder(terminalId, brokeredOrder.Id);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = string.Format(Resource.errMessageFailedToAssignTerminal, string.Empty);
                    LogManager.Logger.WriteException("OrderDomain", "AssignNewTerminalForTierPricedOrder", ex.Message, ex);
                }

            }

            return response;
        }

        #region Edit/delete pre load bol details

        public async Task<StatusViewModel> UpdatePreLoadBolDetails(UserContext userContext, EditPreLoadBolViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    //inactivate current row
                    {
                        var bolDetailsOld = await Context.DataContext.PreLoadBolDetails.Where(b => b.Id == viewModel.Id).FirstOrDefaultAsync();

                        if (bolDetailsOld == null) { return response; }

                        bolDetailsOld.IsActive = false;
                        bolDetailsOld.IsDeleted = true;
                        bolDetailsOld.UpdatedBy = userContext.Id;
                        bolDetailsOld.UpdatedDate = DateTimeOffset.Now;
                        await Context.CommitAsync();
                    }
                    //add new row with updated fields
                    {
                        var bolDetailsNew = await Context.DataContext.PreLoadBolDetails.AsNoTracking().Where(b => b.Id == viewModel.Id).FirstOrDefaultAsync();

                        bolDetailsNew.BolNumber = viewModel.BolNumber;
                        bolDetailsNew.BadgeNumber = viewModel.BadgeNumber;
                        bolDetailsNew.Carrier = viewModel.Carrier;
                        bolDetailsNew.GrossQuantity = viewModel.GrossQuantity;
                        bolDetailsNew.LiftTicketNumber = viewModel.LiftTicketNumber;
                        bolDetailsNew.LiftQuantity = viewModel.LiftQuantity;
                        bolDetailsNew.NetQuantity = viewModel.NetQuantity;
                        bolDetailsNew.CreatedBy = userContext.Id;
                        bolDetailsNew.CreatedDate = DateTimeOffset.Now;
                        bolDetailsNew.UpdatedBy = null;
                        bolDetailsNew.UpdatedDate = null;
                        bolDetailsNew.IsActive = true;
                        bolDetailsNew.IsDeleted = false;
                        Context.DataContext.PreLoadBolDetails.Add(bolDetailsNew);
                        await Context.CommitAsync();
                    }
                    transaction.Commit();
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.msgSuccess;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("OrderDomain", "UpdatePreLoadBolDetails", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusViewModel> DeletePreLoadBolDetails(UserContext userContext, EditPreLoadBolViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel();
            try
            {
                var bol = await Context.DataContext.PreLoadBolDetails.Where(b => b.Id == viewModel.Id).FirstOrDefaultAsync();

                if (bol != null)
                {
                    bol.IsActive = false;
                    bol.IsDeleted = true;

                    await Context.CommitAsync();
                }
                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.msgSuccess;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "DeletePreLoadBolDetails", ex.Message, ex);
            }
            return response;
        }

        #endregion Edit/delete pre load bol details

        #region TPD API METHODS
        public async Task<List<TPDOrderDetails>> GetTPDPONumbers(string token)
        {
            var response = new List<TPDOrderDetails>();
            try
            {
                if (!string.IsNullOrWhiteSpace(token))
                {
                    //get userdetails from token
                    var authDomain = new AuthenticationDomain(this);
                    var apiUserContext = await authDomain.GetUserContextFromTokenAsync(token);
                    if (apiUserContext != null)
                    {
                        response = Context.DataContext.Orders.Where(t => t.AcceptedCompanyId == apiUserContext.CompanyId && t.IsActive
                                        && t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open)
                                        .Select(t => new TPDOrderDetails()
                                        {
                                            Customer = t.BuyerCompany.Name,
                                            PONumber = t.PoNumber,
                                            //Location = t.FuelRequest.Job.DisplayJobID == null ? t.FuelRequest.Job.Name : t.FuelRequest.Job.DisplayJobID, //Impediment - #24750
                                            Location = t.FuelRequest.Job.Name,
                                            Product = t.FuelRequest.MstProduct.DisplayName
                                        }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetTPDPONumbers", ex.Message, ex);
            }

            return response;
        }
        #endregion
        public async Task<List<DeliverySchedulesForBuyerViewModel>> GetDeliverySchedulesForBuyerApp(int buyerCompanyId, long scheduleDate = 0, int userTimeOffset = 0, int offset = 0, int count = 0, int brandedSuppCompId = 0)
        {
            var response = new List<DeliverySchedulesForBuyerViewModel>();
            var helperDomain = new HelperDomain(this);
            try
            {
                var orders = await Context.DataContext.Orders.Where(t => t.BuyerCompanyId == buyerCompanyId && (brandedSuppCompId == 0 || t.AcceptedCompanyId == brandedSuppCompId)
                                            && t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open)
                                    .Select(t => new { t.Id, t.FuelRequest.Job.TimeZoneName }).ToListAsync();

                if (orders != null && orders.Any())
                {
                    var orderIds = orders.Select(t => t.Id).ToList();
                    var allSchedules = Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(t => t.IsActive && t.OrderId != null && t.DeliveryScheduleStatusId != (int)TrackableDeliveryScheduleStatus.MissedAndCanceled
                                                                            && t.DeliveryScheduleStatusId != (int)TrackableDeliveryScheduleStatus.Canceled && orderIds.Contains(t.OrderId ?? 0))
                                                                             .Select(t => new
                                                                             {
                                                                                 t.Id,
                                                                                 t.OrderId,
                                                                                 t.DeliveryScheduleId,
                                                                                 t.Quantity,
                                                                                 t.Order.FuelRequest.UoM,
                                                                                 t.Date,
                                                                                 t.StartTime,
                                                                                 t.EndTime,
                                                                                 t.Order.PoNumber,
                                                                                 t.DeliveryScheduleStatusId,
                                                                                 t.DriverId,
                                                                                 t.Order.FuelRequest.MstProduct,
                                                                                 Dispatcher = t.Order.Company.Name,
                                                                                 DispatcherId = t.Order.Company.Id,
                                                                                 JobName = t.Order.FuelRequest.Job.Name,
                                                                                 JobId = t.Order.FuelRequest.Job.Id,
                                                                                 JobAddress = t.Order.FuelRequest.Job.Address,
                                                                                 JobCity = t.Order.FuelRequest.Job.City,
                                                                                 JobStateCode = t.Order.FuelRequest.Job.MstState.Code,
                                                                                 JobZipCode = t.Order.FuelRequest.Job.ZipCode
                                                                             }).ToList();
                    var enrouteData = Context.DataContext.EnrouteDeliveryHistories.Where(t => orderIds.Any(t1 => t1 == t.OrderId)).Select(t => new { t.DeliveryScheduleId, t.EnrouteDate, t.OrderId, t.StatusId }).ToList();

                    foreach (var order in orders)
                    {
                        DateTimeOffset deliveryStartDate = DateTimeOffset.Now.ToTargetDateTimeOffset(order.TimeZoneName).Date;
                        DateTimeOffset deliveryEndDate = DateTimeOffset.Now.ToTargetDateTimeOffset(order.TimeZoneName).Date.AddDays(1);
                        if (scheduleDate > 0)
                        {
                            deliveryStartDate = DateTimeOffset.FromUnixTimeMilliseconds(scheduleDate).AddMinutes(userTimeOffset).Date;
                        }

                        if (scheduleDate > 0)
                        {
                            deliveryEndDate = DateTimeOffset.FromUnixTimeMilliseconds(scheduleDate).AddMinutes(userTimeOffset).Date.AddDays(1);
                        }
                        if (allSchedules != null && allSchedules.Any())
                        {
                            var orderschedules = allSchedules.Where(t => t.Date.Date >= deliveryStartDate.Date && t.Date < deliveryEndDate.Date
                                                                                 && t.OrderId == order.Id)
                                                                                 .OrderBy(t => t.Date.Date).ThenBy(t => t.StartTime).ToList();

                            if (orderschedules != null && orderschedules.Any())
                            {
                                foreach (var schedule in orderschedules)
                                {
                                    DeliverySchedulesForBuyerViewModel deliverySchedule = new DeliverySchedulesForBuyerViewModel
                                    {
                                        TrackableScheduleId = schedule.Id,
                                        OrderId = schedule.OrderId ?? 0,
                                        DeliveryScheduleId = schedule.DeliveryScheduleId,
                                        GallonsOrdered = ($"{schedule.Quantity.GetPreciseValue(2).GetCommaSeperatedValue()} {(schedule.UoM == UoM.Gallons ? "G" : "L")}"),
                                        ScheduleDate = schedule.Date.Date,
                                        ScheduleStartTime = Convert.ToDateTime(schedule.StartTime.ToString()).ToShortTimeString(),
                                        ScheduleEndTime = Convert.ToDateTime(schedule.EndTime.ToString()).ToShortTimeString(),
                                        PoNumber = schedule.PoNumber,
                                        FuelType = helperDomain.GetProductName(schedule.MstProduct),
                                        FuelTypeId = schedule.MstProduct.Id,
                                        ProductType = schedule.MstProduct.MstProductType.Name,
                                        ProductTypeId = schedule.MstProduct.MstProductType.Id,
                                        PickUpAddress = GetPickUpLocationForBuyerApp(schedule.OrderId ?? 0, schedule.Id, schedule.DeliveryScheduleId),
                                        Dispatcher = schedule.Dispatcher,
                                        DispatcherId = schedule.DispatcherId,
                                        JobName = schedule.JobName,
                                        JobId = schedule.JobId,
                                        JobAddress = schedule.JobAddress + ", " + schedule.JobCity + ", " + schedule.JobStateCode + " " + schedule.JobZipCode,
                                        DriverId = schedule.DriverId.Value
                                    };
                                    var user = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == schedule.DriverId);
                                    if (user != null)
                                    {
                                        deliverySchedule.DriverName = $"{user.FirstName} {user.LastName}";
                                    }
                                    if (schedule.Date.Date >= DateTimeOffset.UtcNow.AddMinutes(userTimeOffset).Date && schedule.DeliveryScheduleStatusId == (int)DeliveryScheduleStatus.Accepted)
                                    {
                                        deliverySchedule.StatusId = schedule.DeliveryScheduleStatusId;
                                        deliverySchedule.Status = EnumHelperMethods.GetDisplayName((DeliveryScheduleStatus)schedule.DeliveryScheduleStatusId);
                                    }
                                    else if (schedule.DeliveryScheduleStatusId == (int)DeliveryScheduleStatus.Completed || schedule.DeliveryScheduleStatusId == (int)DeliveryScheduleStatus.CompletedLate || schedule.DeliveryScheduleStatusId == (int)DeliveryScheduleStatus.RescheduledCompleted)
                                    {
                                        deliverySchedule.StatusId = schedule.DeliveryScheduleStatusId;
                                        deliverySchedule.Status = EnumHelperMethods.GetDisplayName((DeliveryScheduleStatus)schedule.DeliveryScheduleStatusId);
                                    }
                                    else
                                    {
                                        int? enrouteDataStatus = enrouteData.Where(t => (deliverySchedule.TrackableScheduleId == 0 || (t.DeliveryScheduleId == deliverySchedule.DeliveryScheduleId))
                                   && t.OrderId == deliverySchedule.OrderId).OrderByDescending(t => t.EnrouteDate).Select(t => t.StatusId).FirstOrDefault();
                                        deliverySchedule.Status = enrouteDataStatus != null ? EnumHelperMethods.GetDisplayName((EnrouteDeliveryStatus)enrouteDataStatus) : EnumHelperMethods.GetDisplayName((DeliveryScheduleStatus)schedule.DeliveryScheduleStatusId);
                                        deliverySchedule.StatusId = enrouteDataStatus != null ? enrouteDataStatus.Value : schedule.DeliveryScheduleStatusId;
                                    }
                                    response.Add(deliverySchedule);
                                }
                            }
                        }
                    }
                    response = response.Skip(offset).Take(count > 0 ? count : int.MaxValue).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetDeliverySchedulesForBuyerApp", ex.Message, ex);
            }
            return response;
        }
        public string GetPickUpLocationForBuyerApp(int orderId, int? trackableScheduleId, int? deliveryScheduleId)
        {
            var response = string.Empty;

            var prevTerminals = Context.DataContext.FuelDispatchLocations.Where(t => t.OrderId == orderId && t.LocationType == (int)LocationType.PickUp
                                       && t.IsActive).Select(x => new
                                       {
                                           x.TerminalId,
                                           TerminalName = x.MstExternalTerminal != null ? x.MstExternalTerminal.Name : "",
                                           x.Address,
                                           x.City,
                                           x.CountryCode,
                                           x.CountyName,
                                           x.Latitude,
                                           x.Longitude,
                                           x.StateId,
                                           x.ZipCode,
                                           x.StateCode,
                                           x.TimeZoneName,
                                           x.DeliveryScheduleId,
                                           x.TrackableScheduleId,
                                           x.SiteName,
                                           SiteId = x.Id
                                       }).ToList();
            var prevAssignTerminal = prevTerminals.FirstOrDefault(t => t.DeliveryScheduleId == deliveryScheduleId && t.TrackableScheduleId == trackableScheduleId);
            if (prevAssignTerminal == null && prevTerminals != null)
            {
                prevAssignTerminal = prevTerminals.FirstOrDefault(t => t.DeliveryScheduleId == null && t.TrackableScheduleId == null);
            }

            if (prevAssignTerminal != null)
            {
                if (prevAssignTerminal.TerminalId.HasValue)
                {
                    response = prevAssignTerminal.TerminalName;
                }
                else
                {
                    response = prevAssignTerminal.SiteName + ", " + prevAssignTerminal.Address + ", " + prevAssignTerminal.City + ", " + prevAssignTerminal.StateCode + " " + prevAssignTerminal.ZipCode;
                }
            }
            else
            {
                var terminal = Context.DataContext.Orders.Where(t => t.Id == orderId).Select(t => new { t.TerminalId, t.MstExternalTerminal.Name }).FirstOrDefault();
                response = terminal.Name;
            }
            return response;
        }

        public async Task<OrderAPIResponseModel> GetOrdersAsBuyer(string token, string fromDate = "", string toDate = "", int userId = 0)
        {
            var response = new OrderAPIResponseModel();
            try
            {
                var authDomain = new AuthenticationDomain(this);
                var apiUserContext = Task.Run(() => authDomain.GetUserContextFromTokenAsync(token)).Result;
                if (apiUserContext != null)
                {
                    if (apiUserContext.CompanyTypeId == CompanyType.Buyer || apiUserContext.CompanyTypeId == CompanyType.BuyerAndSupplier || apiUserContext.CompanyTypeId == CompanyType.BuyerSupplierAndCarrier || apiUserContext.CompanyTypeId == CompanyType.SupplierAndCarrier || apiUserContext.CompanyTypeId == CompanyType.Supplier || apiUserContext.CompanyTypeId == CompanyType.Carrier)
                    {
                        var spDomain = new StoredProcedureDomain(this);
                        var result = await spDomain.GetOrdersAsBuyer(apiUserContext.CompanyId, userId, fromDate, toDate);

                        response.StatusCode = Status.Success;
                        if (result != null && result.Any())
                        {
                            response.ResponseData = result;
                            response.StatusMessage = Status.Success.ToString();
                        }
                        else
                        {
                            response.ResponseData = new List<BuyerOrderModel>();
                            response.StatusMessage = Resource.lblNoDataAvailable;
                        }
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMsgAccessDenied;
                    }
                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMsgInvalidToken;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetOrdersAsBuyer", ex.Message, ex);
                response.StatusCode = Status.Failed;
                response.StatusMessage = Status.Failed.ToString();
            }

            return response;
        }

        public async Task<OrderAPIResponseModel> GetOrdersAsSupplier(string token, string fromDate = "", string toDate = "", int userId = 0)
        {
            var response = new OrderAPIResponseModel();
            try
            {
                var authDomain = new AuthenticationDomain(this);
                var apiUserContext = Task.Run(() => authDomain.GetUserContextFromTokenAsync(token)).Result;
                if (apiUserContext != null)
                {
                    if (apiUserContext.CompanyTypeId != CompanyType.Buyer)
                    {
                        var spDomain = new StoredProcedureDomain(this);
                        var result = await spDomain.GetOrdersAsSupplier(apiUserContext.CompanyId, userId, fromDate, toDate);

                        response.StatusCode = Status.Success;
                        if (result != null && result.Any())
                        {
                            response.ResponseData = result;
                            response.StatusMessage = Status.Success.ToString();
                        }
                        else
                        {
                            response.ResponseData = new List<SupplierOrderModel>();
                            response.StatusMessage = Resource.lblNoDataAvailable;
                        }
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMsgAccessDenied;
                    }
                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMsgInvalidToken;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetOrdersAsSupplier", ex.Message, ex);
                response.StatusCode = Status.Failed;
                response.StatusMessage = Status.Failed.ToString();
            }

            return response;
        }
        public async Task<StatusViewModel> CancelBrokeredOrderAsync(CancelOrderViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == viewModel.OrderId);
                    if (order != null)
                    {
                        //insert into OrderXCancelationReason table
                        if (order.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId != (int)OrderStatus.Closed || order.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId != (int)OrderStatus.Canceled)
                        {
                            order.OrderXCancelationReason = new OrderXCancelationReason
                            {
                                OrderId = viewModel.OrderId,
                                ReasonId = 5,
                                AdditionalNotes = viewModel.Reason,
                                IsAlreadyResubmittedFuel = false,
                                CanceledBy = viewModel.CanceledBy
                            };

                            //update order status
                            order.OrderXStatuses.FirstOrDefault(t => t.IsActive).IsActive = false;
                            OrderXStatus orderStatus = new OrderXStatus();
                            orderStatus.StatusId = (int)OrderStatus.Canceled;
                            orderStatus.IsActive = true;
                            orderStatus.UpdatedBy = viewModel.CanceledBy;
                            orderStatus.UpdatedDate = DateTimeOffset.Now;
                            order.OrderXStatuses.Add(orderStatus);

                            order.UpdatedBy = viewModel.CanceledBy;

                            Context.DataContext.Entry(order).State = EntityState.Modified;
                            await Context.CommitAsync();
                            transaction.Commit();

                            //Send response
                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.errMessageOrderCancelSuccess;
                        }
                        else
                        {
                            transaction.Commit();
                        }
                    }
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageOrderCancelFailed;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("OrderDomain", "CancelOrderAsync", ex.Message, ex);
                }
            }
            return response;
        }
        #region OptionalPickup
        private async Task GetOptionalPickupDetails(string optionalPickup, List<OptionalPickupInfo> optionalPickupInfo)
        {
            var optionalPickupInfoModel = JsonConvert.DeserializeObject<OptionalPickupScheduleAdditionalInfo>(optionalPickup);
            if (optionalPickupInfoModel != null && !string.IsNullOrEmpty(optionalPickupInfoModel.ScheduleBuilderId) && !string.IsNullOrEmpty(optionalPickupInfoModel.ShiftId) && optionalPickupInfoModel.DriverColIndex >= 0)
            {
                //fetch the optional pickup information for particular schedule builder and that shift id and that driver col.
                DSBColumnOptionalPickupInfoModel dSBColumnOptionalPickupInfo = new DSBColumnOptionalPickupInfoModel();
                dSBColumnOptionalPickupInfo.ScheduleBuilderId = optionalPickupInfoModel.ScheduleBuilderId;
                dSBColumnOptionalPickupInfo.ShiftId = optionalPickupInfoModel.ShiftId;
                dSBColumnOptionalPickupInfo.DriverColIndex = optionalPickupInfoModel.DriverColIndex;
                await GetOptionalPickupDetails(optionalPickupInfo, dSBColumnOptionalPickupInfo);
            }
        }
        private async Task GetOptionalPickupDetails(List<OptionalPickupInfo> optionalPickupInfo, DSBColumnOptionalPickupInfoModel dSBColumnOptionalPickupInfo)
        {
            var getOptionalPickupDetails = await new FreightServiceDomain(this).GetOptionalPickupDetails(dSBColumnOptionalPickupInfo);
            if (getOptionalPickupDetails.Any())
            {
                var terminalIds = getOptionalPickupDetails.Where(x => x.DSBPickupLocationInfo.PickupLocationType == (int)PickupLocationType.Terminal).Select(x => x.DSBPickupLocationInfo.TfxTerminal.Id).ToList();
                var terminalInfo = Context.DataContext.MstExternalTerminals.Where(t => terminalIds.Contains(t.Id)).Select(t => new DropAddressViewModel()
                {
                    SiteId = t.Id,
                    SiteName = t.Name,
                    Address = t.Address,
                    City = t.City,
                    State = new StateViewModel() { Id = t.StateId, Code = t.StateCode },
                    ZipCode = t.ZipCode,
                    Country = new CountryViewModel() { Code = t.CountryCode },
                    CountyName = t.CountyName,
                    Latitude = t.Latitude,
                    Longitude = t.Longitude
                }).ToList();
                foreach (var item in getOptionalPickupDetails)
                {
                    if (optionalPickupInfo.FindIndex(x => x.FuelTypeId == item.TfxFuelTypeId) >= 0)
                    {
                        var optionalPickupItem = optionalPickupInfo.FirstOrDefault(x => x.FuelTypeId == item.TfxFuelTypeId);
                        if (optionalPickupItem != null)
                        {
                            var optionalPickupTerminal = IntializeOptionalPickup(optionalPickupInfo, terminalInfo, item);
                            optionalPickupItem.OptionalPickupTerminalInfo.Add(optionalPickupTerminal);
                        }

                    }
                    else
                    {
                        var optionalPickupInfoDetails = new OptionalPickupInfo();
                        optionalPickupInfoDetails.FuelTypeId = item.TfxFuelTypeId;
                        optionalPickupInfoDetails.FuelTypeName = item.TfxFuelTypeName;
                        optionalPickupInfoDetails.OptionalPickupTerminalInfo = new List<OptionalPickupTerminalInfo>();
                        optionalPickupInfoDetails.OptionalPickupTerminalInfo.Add(IntializeOptionalPickup(optionalPickupInfo, terminalInfo, item));
                        optionalPickupInfo.Add(optionalPickupInfoDetails);
                    }
                }
            }
        }
        private static OptionalPickupTerminalInfo IntializeOptionalPickup(List<OptionalPickupInfo> optionalPickupInfo, List<DropAddressViewModel> terminalInfo, DSBColumnOptionalPickupInfoModel item)
        {
            OptionalPickupTerminalInfo optionalPickupTerminal = new OptionalPickupTerminalInfo();

            if (item.DSBPickupLocationInfo.PickupLocationType == (int)PickupLocationType.Terminal)
            {
                var terminalPickupInfo = terminalInfo.FirstOrDefault(x => x.SiteId == item.DSBPickupLocationInfo.TfxTerminal.Id);
                if (terminalPickupInfo != null)
                {

                    optionalPickupTerminal.Id = item.Id;
                    optionalPickupTerminal.TerminalId = terminalPickupInfo.SiteId;
                    optionalPickupTerminal.TerminalName = terminalPickupInfo.SiteName;
                    optionalPickupTerminal.PickUpLocationLatitude = terminalPickupInfo.Latitude;
                    optionalPickupTerminal.PickUpLocationLongitude = terminalPickupInfo.Longitude;
                    optionalPickupTerminal.PickUpLocationStateCode = terminalPickupInfo.State.Code;
                    optionalPickupTerminal.PickUpLocationZipCode = terminalPickupInfo.ZipCode;
                    optionalPickupTerminal.PickUpLocationCountryCode = terminalPickupInfo.Country.Code;
                    optionalPickupTerminal.PickUpLocationCountyName = terminalPickupInfo.Country.Name;
                    optionalPickupTerminal.PickUpLocationCity = terminalPickupInfo.City;
                    optionalPickupTerminal.PickUpLocationAddress = terminalPickupInfo.Address;
                    optionalPickupTerminal.BadgeNo1 = item.DSBPickupLocationInfo.BadgeNo1;
                    optionalPickupTerminal.BadgeNo2 = item.DSBPickupLocationInfo.BadgeNo2;
                    optionalPickupTerminal.BadgeNo3 = item.DSBPickupLocationInfo.BadgeNo3;
                    optionalPickupTerminal.IsTerminal = true;

                }

            }
            else
            {
                optionalPickupTerminal.Id = item.Id;
                optionalPickupTerminal.TerminalId = item.DSBPickupLocationInfo.TfxBulkPlant.Id;
                optionalPickupTerminal.TerminalName = item.DSBPickupLocationInfo.TfxBulkPlant.SiteName.ToString();
                optionalPickupTerminal.PickUpLocationLatitude = item.DSBPickupLocationInfo.TfxBulkPlant.Latitude;
                optionalPickupTerminal.PickUpLocationLongitude = item.DSBPickupLocationInfo.TfxBulkPlant.Longitude;
                optionalPickupTerminal.PickUpLocationStateCode = item.DSBPickupLocationInfo.TfxBulkPlant.State.Code;
                optionalPickupTerminal.PickUpLocationZipCode = item.DSBPickupLocationInfo.TfxBulkPlant.ZipCode;
                optionalPickupTerminal.PickUpLocationCountryCode = item.DSBPickupLocationInfo.TfxBulkPlant.Country.Code;
                optionalPickupTerminal.PickUpLocationCountyName = item.DSBPickupLocationInfo.TfxBulkPlant.Country.Name;
                optionalPickupTerminal.PickUpLocationCity = item.DSBPickupLocationInfo.TfxBulkPlant.City;
                optionalPickupTerminal.PickUpLocationAddress = item.DSBPickupLocationInfo.TfxBulkPlant.Address;
                optionalPickupTerminal.BadgeNo1 = item.DSBPickupLocationInfo.BadgeNo1;
                optionalPickupTerminal.BadgeNo2 = item.DSBPickupLocationInfo.BadgeNo2;
                optionalPickupTerminal.BadgeNo3 = item.DSBPickupLocationInfo.BadgeNo3;
                optionalPickupTerminal.IsTerminal = false;

            }
            return optionalPickupTerminal;

        }
        private static void AssignOptionalPickupInfo(List<OptionalPickupInfo> optionalPickupInfo, DeliveryScheduleGroup deliveryGroupItem, string optionalPickup)
        {
            var fuelTypeIdList = deliveryGroupItem.DeliverySchedules.Select(x => x.FuelTypeId).ToList();
            if (fuelTypeIdList.Any())
            {
                var optioanlPickupList = optionalPickupInfo.Where(x => fuelTypeIdList.Contains(x.FuelTypeId)).ToList();
                deliveryGroupItem.OptionalPickupInfo = optioanlPickupList;
            }
            var IsOptionalPickup = deliveryGroupItem.DeliverySchedules.Any(x => x.IsOptionalPickup);
            if (IsOptionalPickup)
            {
                deliveryGroupItem.IsOptionalPickup = true;
            }
            else if (deliveryGroupItem.OptionalPickupInfo.Any())
            {
                deliveryGroupItem.IsOptionalPickup = true;
            }
            var optionalPickupInfoModel = JsonConvert.DeserializeObject<OptionalPickupScheduleAdditionalInfo>(optionalPickup);
            if (optionalPickupInfoModel != null && !string.IsNullOrEmpty(optionalPickupInfoModel.ScheduleBuilderId) && !string.IsNullOrEmpty(optionalPickupInfoModel.ShiftId) && optionalPickupInfoModel.DriverColIndex >= 0)
            {
                deliveryGroupItem.OptionalPickupAPIInfo = new OptionalPickupAPIInfo();
                deliveryGroupItem.OptionalPickupAPIInfo.ScheduleBuilderId = optionalPickupInfoModel.ScheduleBuilderId;
                deliveryGroupItem.OptionalPickupAPIInfo.ShiftId = optionalPickupInfoModel.ShiftId;
                deliveryGroupItem.OptionalPickupAPIInfo.DriverColIndex = optionalPickupInfoModel.DriverColIndex;
            }
        }
        public async Task<List<OptionalPickupInfo>> GetOptionalPickupDetails(OptionalPickupAPIInfo optionalPickupInfoModel, List<OptionalPickupInfo> optionalPickupInfo)
        {
            if (optionalPickupInfoModel != null && !string.IsNullOrEmpty(optionalPickupInfoModel.ScheduleBuilderId) && !string.IsNullOrEmpty(optionalPickupInfoModel.ShiftId) && optionalPickupInfoModel.DriverColIndex >= 0)
            {
                //fetch the optional pickup information for particular schedule builder and that shift id and that driver col.
                DSBColumnOptionalPickupInfoModel dSBColumnOptionalPickupInfo = new DSBColumnOptionalPickupInfoModel();
                dSBColumnOptionalPickupInfo.ScheduleBuilderId = optionalPickupInfoModel.ScheduleBuilderId;
                dSBColumnOptionalPickupInfo.ShiftId = optionalPickupInfoModel.ShiftId;
                dSBColumnOptionalPickupInfo.DriverColIndex = optionalPickupInfoModel.DriverColIndex;
                await GetOptionalPickupDetails(optionalPickupInfo, dSBColumnOptionalPickupInfo);
            }
            return optionalPickupInfo;
        }
        #endregion

        public async Task<StatusViewModel> ProcessUploadedOrderCsvFile(HttpPostedFileBase csvFile, string csvFilePath, UserContext userContext)
        {
            var response = new StatusViewModel();
            try
            {
                var orderList = new List<ThirdPartyOrderCsvViewModelNew>();
                int queueId = 0;
                using (var stream = new MemoryStream())
                {
                    //Validate Csv header
                    csvFile.InputStream.CopyTo(stream);
                    stream.Seek(0, SeekOrigin.Begin);
                    string csvText = new StreamReader(stream).ReadToEnd();
                    var csvHeaderLine = Regex.Matches(csvText.Trim(), @"^.*Company Name*.*\n").Cast<Match>().FirstOrDefault();

                    string[] lines = File.ReadAllLines(csvFilePath);
                    string headerLine = lines.FirstOrDefault();
                    if (csvHeaderLine != null && csvHeaderLine.Value.Trim() != headerLine)
                    {
                        //ToDo: this is temporary code which has been written to support multiple template.Once customer start using with new template we should revert code with perevious changeset 
                        int index = headerLine.IndexOf("Auto Freight Pricing Method(Yes/No)");
                        if (index >= 0)
                        {
                            headerLine = headerLine.Remove(index - 1, (headerLine.Length - index) + 1);
                            if (csvHeaderLine.Value.Trim() != headerLine)
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = Resource.errMessageBulkUploadHeaderMismatch;
                                return response;
                            }
                        }
                        else
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errMessageBulkUploadHeaderMismatch;
                            return response;
                        }

                    }

                    //Validate Empty File
                    stream.Seek(0, SeekOrigin.Begin);
                    orderList = ReadCSVFile<ThirdPartyOrderCsvViewModelNew>(stream, true);
                    if (orderList.Any())
                    {

                        //Upload File to blob and add queueevent
                        var azureBlob = new AzureBlobStorage();
                        var filePath = await azureBlob.SaveBlobAsync(csvFile.InputStream, GenerateFileName(userContext.Id), BlobContainerType.Orderbulkupload.ToString().ToLower());
                        if (!string.IsNullOrWhiteSpace(filePath))
                        {
                            var queueDomain = new QueueMessageDomain();
                            var queueRequest = GetEnqueueMessageRequestViewModel(userContext, filePath);
                            queueId = queueDomain.EnqeueMessage(queueRequest);

                            response.StatusCode = Status.Success;
                            response.StatusMessage = string.Format(Resource.successMessageOrderBulkWithRequestNo, string.Concat(Constants.SFXOrderBulkUploadSuffix, queueId.ToString("000")));
                        }
                        else
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errMessageErrorInAzureServer;
                        }
                    }
                    else
                    {
                        response.StatusMessage = Resource.errMessageOrderCSVFileEmpty;
                        response.StatusCode = Status.Failed;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "ProcessUploadedOrderCsvFile", "Order CSV upload failed", ex);
                response.StatusCode = Status.Failed;
                response.StatusMessage = "Order CSV upload failed";
            }
            return response;
        }

        private string GenerateFileName(int userId)
        {
            return string.Concat(values: Constants.OrderBulk + Resource.lblSingleHyphen + userId + Resource.lblSingleHyphen + DateTime.Now.Ticks + ".csv");
        }

        private QueueMessageViewModel GetEnqueueMessageRequestViewModel(UserContext userContext, string blobStoragePath)
        {
            var jsonViewModel = new ThirdPartyBulkUploadQueueMsg();
            jsonViewModel.FileLineNumberToStart = 0;
            jsonViewModel.SupplierId = userContext.Id;
            jsonViewModel.SupplierCompanyId = userContext.CompanyId;
            jsonViewModel.FileUploadPath = blobStoragePath;

            string json = JsonConvert.SerializeObject(jsonViewModel);

            return new QueueMessageViewModel()
            {
                CreatedBy = userContext.Id,
                QueueProcessType = QueueProcessType.ThirdPartyOrderBulkUpload,
                JsonMessage = json
            };
        }

        public async Task<StatusViewModel> SaveCarrierMapping(UspCarrierMapping uspCarrierMapping, UserContext userContext)
        {
            var response = new StatusViewModel();

            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    if (uspCarrierMapping.AssignedTerminalId == null || uspCarrierMapping.AssignedTerminalId.Id == 0)
                    {
                        response.StatusCode = Status.Warning;
                        response.StatusMessage = string.Format(Resource.valMessageRequired, Resource.gridColumnTerminalId);
                        return response;
                    }
                    if (string.IsNullOrWhiteSpace(uspCarrierMapping.CarrierName))
                    {
                        response.StatusCode = Status.Warning;
                        response.StatusMessage = string.Format(Resource.valMessageRequired, Resource.lblCarrierName);
                        return response;
                    }
                    if (string.IsNullOrEmpty(uspCarrierMapping.AssignedCarrierId))
                    {
                        response.StatusCode = Status.Warning;
                        response.StatusMessage = string.Format(Resource.valMessageRequired, Resource.lblLFVCarrierId);
                        return response;
                    }

                    if (uspCarrierMapping.Id > 0) //update mapping 
                    {
                        var existingMapping = await Context.DataContext.CarrierMappings.Where(t => t.Id == uspCarrierMapping.Id && t.IsActive).FirstOrDefaultAsync();
                        if (existingMapping != null)
                        {
                            existingMapping.IsActive = false;
                            existingMapping.UpdatedBy = userContext.Id;
                            existingMapping.UpdatedDate = DateTimeOffset.Now;
                            Context.DataContext.Entry(existingMapping).State = EntityState.Modified;

                            await Context.CommitAsync();

                            if (!IsDuplicateCarrierMapping(uspCarrierMapping.AssignedTerminalId.Id, uspCarrierMapping.AssignedCarrierId).Result)
                            {
                                var carrier = GetCarrierInfoByCarrierName(uspCarrierMapping.CarrierName, userContext);
                                var entity = new CarrierMapping();
                                entity.CompanyId = userContext.CompanyId; //created by companyId
                                entity.TerminalCompanyAliasId = existingMapping.TerminalCompanyAliasId;
                                if (carrier != null && carrier.Id > 0)
                                {
                                    entity.CarrierCompanyId = carrier.Id;
                                }
                                if (carrier != null)
                                {
                                    entity.CarrierName = carrier.Name;
                                }
                                entity.AssignedCarrierId = uspCarrierMapping.AssignedCarrierId;
                                entity.CreatedBy = userContext.Id;
                                entity.UpdatedBy = userContext.Id;
                                entity.CreatedDate = DateTimeOffset.Now;
                                entity.UpdatedDate = DateTimeOffset.Now;
                                entity.IsActive = true;
                                entity.CountryId = uspCarrierMapping.CountryId;
                                Context.DataContext.CarrierMappings.Add(entity);
                                await Context.CommitAsync();

                                transaction.Commit();
                                response.StatusCode = Status.Success;
                                response.StatusMessage = Resource.successMsgCarrierIDSaved;
                                return response;
                            }
                            else
                            {
                                transaction.Rollback();
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = Resource.errMsgCarrierIDValidation;

                            }
                        }

                    }
                    else // save new mapping
                    {
                        if (!IsDuplicateCarrierMapping(uspCarrierMapping.AssignedTerminalId.Id, uspCarrierMapping.AssignedCarrierId).Result)
                        {
                            var carrier = GetCarrierInfoByCarrierName(uspCarrierMapping.CarrierName, userContext);
                            var entity = new CarrierMapping();
                            entity.CompanyId = userContext.CompanyId; //created by companyId
                            entity.TerminalCompanyAliasId = uspCarrierMapping.AssignedTerminalId.Id;
                            if (carrier != null && carrier.Id > 0)
                            {
                                entity.CarrierCompanyId = carrier.Id;
                            }
                            if (carrier != null)
                            {
                                entity.CarrierName = carrier.Name;
                            }
                            entity.AssignedCarrierId = uspCarrierMapping.AssignedCarrierId;
                            entity.CreatedBy = userContext.Id;
                            entity.UpdatedBy = userContext.Id;
                            entity.CreatedDate = DateTimeOffset.Now;
                            entity.UpdatedDate = DateTimeOffset.Now;
                            entity.IsActive = true;
                            entity.CountryId = uspCarrierMapping.CountryId;
                            Context.DataContext.CarrierMappings.Add(entity);
                            await Context.CommitAsync();

                            transaction.Commit();
                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.successMsgCarrierIDSaved;
                            return response;
                        }
                        else
                        {
                            transaction.Rollback();
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errMsgCarrierIDValidation;

                        }
                    }
                }
                catch (Exception ex)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.msgCarrierMappingFailed;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("OrderDomain", "SaveCarrierMapping", ex.Message, ex);
                }
            }

            return response;
        }

        //TerminalId + CarrierID == UNIQUE when saving 
        public Task<bool> IsDuplicateCarrierMapping(int terminalCompanyAliasId, string carrierID, string carrierName = null)
        {
            if (string.IsNullOrWhiteSpace(carrierName))
            {
                var isValid = Context.DataContext.CarrierMappings.AnyAsync(t => t.TerminalCompanyAliasId == terminalCompanyAliasId
                               && t.AssignedCarrierId.ToLower().Trim() == carrierID.ToLower().Trim() && t.IsActive);
                return isValid;
            }
            else
            {
                var isValid = Context.DataContext.CarrierMappings.AnyAsync(t => t.TerminalCompanyAliasId == terminalCompanyAliasId
                              && t.AssignedCarrierId.ToLower().Trim() == carrierID.ToLower().Trim()
                              && t.CarrierName.ToLower().Trim() == carrierName.ToLower().Trim() && t.IsActive);
                return isValid;
            }
        }

        public DropdownDisplayItem GetCarrierInfoByCarrierName(string carrierName, UserContext userContext)
        {
            var response = new DropdownDisplayItem();
            try
            {
                if (!string.IsNullOrWhiteSpace(carrierName))
                {
                    var domain = new DispatcherDomain(this);
                    var existingCarriers = domain.GetCarriersForSupplierDashboard(userContext.CompanyId);
                    if (existingCarriers != null && existingCarriers.Any())
                    {
                        var existingCarrier = existingCarriers.Where(t => t.Name.ToLower().Trim() == carrierName.ToLower().Trim()).FirstOrDefault();
                        if (existingCarrier == null || existingCarrier.Id == 0) // non onboarded carrier
                        {
                            response.Id = 0;
                            response.Name = carrierName;
                        }
                        else
                        {
                            response = existingCarrier;
                        }
                    }
                    else
                    {
                        response.Id = 0;
                        response.Name = carrierName;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetCarrierInfoByCarrierName", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<UspCarrierMapping>> GetCarrierIDMappings(UserContext userContext, int countryId)
        {
            var carrierMapping = new List<UspCarrierMapping>();
            try
            {
                var storedProcedureDomain = new StoredProcedureDomain(this);
                carrierMapping = await storedProcedureDomain.GetCarrierIDMappings(userContext.CompanyId, countryId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetCarrierIDMappings", ex.Message, ex);
            }
            return carrierMapping;
        }

        public async Task<StatusViewModel> DeleteCarrierIDMapping(int mappingId, UserContext userContext)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                var existingMapping = await Context.DataContext.CarrierMappings.Where(t => t.Id == mappingId).FirstOrDefaultAsync();
                if (existingMapping != null)
                {
                    existingMapping.IsActive = false;
                    existingMapping.UpdatedBy = userContext.Id;
                    existingMapping.UpdatedDate = DateTimeOffset.Now;
                    Context.DataContext.Entry(existingMapping).State = EntityState.Modified;
                    await Context.CommitAsync();
                    response.StatusCode = Status.Success;
                    response.StatusMessage = "CarrierID mapping Succesfully Deleted";
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "DeleteCarrierIDMapping", ex.Message, ex);

            }
            return response;
        }

        public async Task<StatusViewModel> UpdateIncludePricingFlagForPDI(int orderId, bool isIncludePricing, UserContext userContext)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {

                var order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == orderId);
                if (order != null)
                {
                    var additionalDetail = order.OrderAdditionalDetail;
                    if (additionalDetail != null)
                    {
                        additionalDetail.IsIncludePricingInExternalObj = isIncludePricing;
                        order.UpdatedDate = DateTimeOffset.Now;
                        order.UpdatedBy = userContext.Id;
                        Context.DataContext.Entry(order).State = EntityState.Modified;
                        await Context.CommitAsync();
                    }
                    else
                    {
                        response.StatusMessage = Resource.errMessageOrderNotFound;
                    }
                }
                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.successMessageUpdatedSuccessfully;
            }
            catch (Exception ex)
            {
                response.StatusMessage = Resource.errMessageUpdateFailed;
                LogManager.Logger.WriteException("OrderDomain", "UpdateIncludePricingFlagForPDI", ex.Message, ex);
            }
            return response;
        }

        //FOR BLEND DR CREATION
        public async Task<List<AdditiveOrderViewModel>> GetAdditiveOrders(int companyId, string regionId)
        {
            var response = new List<AdditiveOrderViewModel>();
            try
            {
                if (!string.IsNullOrEmpty(regionId))
                {
                    var favProduct = await new FreightServiceDomain(this).GetRegionFavouriteProducts(null, regionId, companyId);
                    if (favProduct != null && favProduct.TfxFavProductTypeId != RegionFavProductType.None)
                    {
                        if (favProduct.TfxFavProductTypeId == RegionFavProductType.ProductType && favProduct.TfxProductTypeIds != null && favProduct.TfxProductTypeIds.Any())
                        {
                            var isAdditiveProductValid = favProduct.TfxProductTypeIds.Contains((int)ProductTypes.Additives);
                            if (!isAdditiveProductValid)
                            {
                                return response;
                            }
                        }
                    }
                }
                response = await Context.DataContext.Orders
                                    .Where(t => t.AcceptedCompanyId == companyId
                                        && t.IsActive
                                        && t.FuelRequest.MstProduct.ProductTypeId == (int)ProductTypes.Additives
                                        && t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open)
                                    .Select(t => new AdditiveOrderViewModel
                                    {
                                        Id = t.Id,
                                        BuyerCompanyId = t.BuyerCompanyId,
                                        Name = t.PoNumber + " (" + t.FuelRequest.MstProduct.Name + ")",
                                        JobId = t.FuelRequest.JobId,
                                        UoM = t.FuelRequest.UoM.ToString()
                                    }).ToListAsync();

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "GetAdditiveOrders", ex.Message, ex);
            }
            return response;
        }

        public async Task<DsbCalenderFiltersDataViewModel> GetFilterDataForCalenderView(UserContext userContext)
        {
            using (var tracer = new Tracer("OrderDomain", "GetFilterDataForCalenderView"))
            {
                var response = new DsbCalenderFiltersDataViewModel();
                try
                {
                    StoredProcedureDomain storedProcedureDomain = new StoredProcedureDomain(this);
                    response.CustomerList = await storedProcedureDomain.GetCarrierCustomerMapping(userContext.CompanyId);

                    var jobs = Context.DataContext.Orders.Where(t => t.AcceptedCompanyId == userContext.CompanyId && t.IsActive &&
                                         t.OrderXStatuses.FirstOrDefault().StatusId == (int)OrderStatus.Open)
                                         .Select(t => new
                                         {
                                             Id = t.FuelRequest.Job.Id,
                                             Name = t.FuelRequest.Job.Name,
                                             customerId = t.FuelRequest.Job.CompanyId,
                                             LocationType = t.FuelRequest.Job.IsMarine,
                                             Vessels = t.FuelRequest.Job.JobXAssets.Where(x => x.Job.IsMarine && x.RemovedBy == null && x.RemovedDate == null)
                                                                  .Select(x1 => new DropdownDisplayExtendedProperty { Id = x1.Asset.Id, Name = x1.Asset.Name, CodeId = x1.JobId })
                                         }).GroupBy(t => t.Id).Select(t => t.FirstOrDefault()).ToList();


                    if (jobs != null && jobs.Any())
                    {
                        response.Locations = jobs.Select(x => new DropdownDisplayExtendedProperty { Id = x.Id, Name = x.Name, CodeId = x.customerId, IsTrue = x.LocationType }).Distinct().ToList();
                        response.Vessels = jobs.SelectMany(t => t.Vessels).GroupBy(t => t.Id).Select(x => new DropdownDisplayExtendedProperty() { Id = x.Key, CodeId = x.Select(y => y.CodeId).FirstOrDefault(), Name = x.Select(y => y.Name).FirstOrDefault() }).ToList();
                    }

                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderDomain", "GetFilterDataForCalenderView", ex.Message, ex);
                }
                return response;
            }
        }
    }
}
