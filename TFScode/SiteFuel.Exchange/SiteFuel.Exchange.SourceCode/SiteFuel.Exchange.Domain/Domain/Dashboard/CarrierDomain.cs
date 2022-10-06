using FileHelpers;
using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.EmailManager;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.DispatchScheduler;
using SiteFuel.Exchange.ViewModels.FreightOnlyOrder;
using SiteFuel.Exchange.ViewModels.Settings;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace SiteFuel.Exchange.Domain
{
    public class CarrierDomain : BaseDomain
    {
        public CarrierDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public CarrierDomain(BaseDomain domain) : base(domain)
        {
        }

        public string ProcessCreateFreightOnlyOrderQueueMsg(CreateFreightOnlyOrderQueueMsg bulkUploadMsg, List<string> errorInfo)
        {
            StringBuilder processMessage = new StringBuilder();

            try
            {
                if (bulkUploadMsg.CarrierCompanyId > 0 && bulkUploadMsg.FuelRequestId > 0)
                {
                    var user = Context.DataContext.Users.Where(t => t.CompanyId == bulkUploadMsg.CarrierCompanyId).FirstOrDefault();
                    if (user != null)
                    {
                        var carrierUserContext = new AuthenticationDomain(this).GetUserContextAsync(user.Id, CompanyType.Carrier).Result;
                        var supplierUserContext = new AuthenticationDomain(this).GetUserContextAsync(bulkUploadMsg.SupplierUserId, CompanyType.Supplier).Result;

                        var isAlreadyOrderCreated = Context.DataContext.Orders.Count(t => t.AcceptedCompanyId == bulkUploadMsg.CarrierCompanyId && t.PoNumber == bulkUploadMsg.PONumber);
                        if (isAlreadyOrderCreated == 0)
                        {
                            var frDomain = new FuelRequestDomain(this);

                            var brokerViewModel = frDomain.GetBrokerFuelRequestAsync(bulkUploadMsg.FuelRequestId, bulkUploadMsg.SupplierCompanyId, true).Result;
                            if (brokerViewModel != null)
                            {
                                brokerViewModel.Type = (int)FuelRequestType.FreightOnlyRequest;
                                brokerViewModel.Details.PrivateSupplierList.IsPublicRequest = true;
                                brokerViewModel.Details.FuelDeliveryDetails.PoContactId = bulkUploadMsg.SupplierUserId;
                                //using (var transaction = Context.DataContext.Database.BeginTransaction())
                                //{
                                try
                                {
                                    var response = frDomain.SaveBrokerFuelRequestAsync(supplierUserContext, brokerViewModel).Result;
                                    if (response.StatusCode == Status.Success)
                                    {
                                        var existingOrder = Context.DataContext.Orders.Where(t => t.Id == bulkUploadMsg.OrderId).FirstOrDefault();
                                        int? preferenceSettingId = existingOrder != null ? existingOrder.OrderAdditionalDetail.PreferencesSettingId : null;
                                        response = frDomain.AcceptFuelRequest(carrierUserContext, brokerViewModel.FuelRequestId, null, true, preferenceSettingId).Result;
                                        if (response.StatusCode == Status.Success || (response.StatusCode == Status.Warning && response.StatusMessage == Resource.errMessageTerminalIsNotAssigned))
                                        {
                                            // mark existing order IsEndSupplier Flag to false, as order is brokered to carrier
                                            // var existingOrder = Context.DataContext.Orders.Where(t => t.Id == bulkUploadMsg.OrderId).FirstOrDefault();
                                            if (existingOrder != null)
                                            {
                                                //existingOrder.IsEndSupplier = true;
                                                existingOrder.UpdatedBy = supplierUserContext.Id;
                                                existingOrder.UpdatedDate = DateTimeOffset.Now;
                                                Context.DataContext.Entry(existingOrder).State = EntityState.Modified;
                                                Context.DataContext.SaveChanges();
                                            }
                                            errorInfo.Add(SetSuccessProcessMessage(bulkUploadMsg.PONumber, carrierUserContext.CompanyName));
                                        }
                                        else
                                        {
                                            errorInfo.Add(response.StatusMessage);
                                            ////do we need to mark created FR as inactive?
                                            /// commented below code for #24826 impediment as brokered FR details were not showing
                                            //var newFr = Context.DataContext.FuelRequests.Where(t => t.Id == brokerViewModel.FuelRequestId).FirstOrDefault();
                                            //if (newFr != null)
                                            //{
                                            //    newFr.IsActive = false;
                                            //    Context.DataContext.Entry(newFr).State = EntityState.Modified;
                                            //    Context.DataContext.SaveChanges();
                                            //}
                                        }
                                    }
                                    else
                                        errorInfo.Add(response.StatusMessage);
                                }
                                catch (Exception ex)
                                {
                                    //transaction.Rollback();
                                    LogManager.Logger.WriteException("CarrierDomain", "CreateOrdersForCarrier", ex.Message, ex);
                                }
                                //}
                            }
                        }
                        else
                            errorInfo.Add(SetErrorProcessMessage(bulkUploadMsg.PONumber, carrierUserContext.CompanyName));
                    }
                }
            }
            catch (Exception ex)
            {
                if (!(ex is QueueMessageFatalException))
                    LogManager.Logger.WriteException("CarrierDomain", "ProcessCreateFreightOnlyOrderQueueMsg", ex.Message, ex);
                if (processMessage.Length == 0)
                {
                    processMessage.Append(Resource.errMessageCreateBrokeredFuelRequestFailed);
                    errorInfo.Add(processMessage.ToString());
                }
                throw new QueueMessageFatalException(errorInfo[0], errorInfo);
            }

            return processMessage.ToString();
        }

        public async Task<DeliveryRequestBrokerResponseViewModel> BrokerDeliveryRequestToCarrier(UserContext userContext, DeliveryRequestBrokerInfoViewModel viewModel)
        {
            var response = new DeliveryRequestBrokerResponseViewModel();
            try
            {
                if (viewModel.OrderId == 0)
                {
                    response.StatusMessage = "Please select an order to broker this deilvery request";
                    return response;
                }

                if (viewModel.CarrierCompanyId == 0)
                {
                    response.StatusMessage = "Please select a carrier to broker this deilvery request";
                    return response;
                }

                var freightServiceDomain = new FreightServiceDomain(this);
                var carrierAdminId = Context.DataContext.Users.Where(t => t.CompanyId == viewModel.CarrierCompanyId).Select(t => t.Id).FirstOrDefault();
                var authDomain = new AuthenticationDomain(this);
                var notificationDomain = new NotificationDomain(this);
                var companyType = CompanyType.Carrier;
                if (viewModel.IsDispatchRetainedByCustomer == true)
                {
                    companyType = CompanyType.Supplier;
                }
                var carrierUserContext = await authDomain.GetUserContextAsync(carrierAdminId, companyType);
                var spDomain = new StoredProcedureDomain(this);
                if (!string.IsNullOrEmpty(viewModel.BlendedGroupId))
                {
                    //get all the BlendedGroup DRs and Orders details.
                    List<DeliveryRequestViewModel> blendedDRsInfo = await freightServiceDomain.GetBlendedGroupDeliveryRequestDetails(new List<string> { viewModel.BlendedGroupId });
                    if (blendedDRsInfo.Any() && viewModel.IsDispatchRetainedByCustomer == false)
                    {
                        await CreateBlendedGroupBrokeredDRsCarrier(userContext, viewModel, response, freightServiceDomain, carrierAdminId, notificationDomain, companyType, carrierUserContext, blendedDRsInfo);
                        if (response.StatusCode == Status.Success && viewModel.DeliveryRequest != null)
                        {
                            var message = new BrokerDeliveryRequestMessageViewModel { EntityId = viewModel.DeliveryRequest.Id, AssignedTo = carrierAdminId, AssignedToCompanyId = viewModel.CarrierCompanyId, CompanyType = companyType, BlendedGroupId = viewModel.BlendedGroupId };
                            var jsonMessage = new JavaScriptSerializer().Serialize(message);
                            await notificationDomain.AddNotificationEventAsync(EventType.BrokerDeliveryRequestCreated, 0, userContext.Id, null, jsonMessage);
                        }
                    }
                    else if (blendedDRsInfo.Any() && viewModel.IsDispatchRetainedByCustomer == true)
                    {
                        // CreateBlendedGroupBrokeredDRsSupplier
                        foreach (var blendeditem in blendedDRsInfo)
                        {
                            var childOrders = await spDomain.GetBrokeredChildOrders(blendeditem.OrderId.GetValueOrDefault());
                            var OrderDetails = childOrders.Where(t => t.IsDispatchRetainedByCustomer).FirstOrDefault();
                            if (OrderDetails != null && OrderDetails.StatusId == (int)OrderStatus.Open)
                            {
                                response.OrderId = OrderDetails.OrderId;
                                response.StatusCode = Status.Success;
                                var blendedDRresponse = new DeliveryRequestBrokerResponseViewModel();
                                blendedDRresponse.OrderId = OrderDetails.OrderId;
                                blendedDRresponse.DeliveryRequestId = blendeditem.Id;
                                await CreateBlendedBrokeredDRs(userContext, viewModel, response, freightServiceDomain, carrierAdminId, notificationDomain, companyType, blendeditem, blendedDRresponse);
                            }
                            else if (OrderDetails != null && (OrderDetails.StatusId == (int)OrderStatus.Closed || OrderDetails.StatusId == (int)OrderStatus.Canceled))
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = Resource.errorSupplierBrokerOrderClosed;
                                return response;
                            }
                            else
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = Resource.errorSupplierBrokerorderNotFound;
                                return response;
                            }

                        }
                        if (response.StatusCode == Status.Success && viewModel.DeliveryRequest != null)
                        {
                            var message = new BrokerDeliveryRequestMessageViewModel { EntityId = viewModel.DeliveryRequest.Id, AssignedTo = carrierAdminId, AssignedToCompanyId = viewModel.CarrierCompanyId, CompanyType = companyType, BlendedGroupId = viewModel.BlendedGroupId };
                            var jsonMessage = new JavaScriptSerializer().Serialize(message);
                            await notificationDomain.AddNotificationEventAsync(EventType.BrokerDeliveryRequestCreated, 0, userContext.Id, null, jsonMessage);
                        }
                    }
                }
                else
                {
                    var childOrders = await spDomain.GetBrokeredChildOrders(viewModel.OrderId);
                    var targetOrderId = 0;

                    if (viewModel.IsDispatchRetainedByCustomer == false)
                    {
                        response = await CreateNewOrderForDeliveryRequest(userContext, carrierUserContext, viewModel);
                    }
                    else
                    {
                        var OrderDetails = childOrders.Where(t => t.IsDispatchRetainedByCustomer).FirstOrDefault();
                        if (OrderDetails != null && OrderDetails.StatusId == (int)OrderStatus.Open)
                        {
                            response.OrderId = OrderDetails.OrderId;
                            response.StatusCode = Status.Success;
                        }
                        else if (OrderDetails != null && (OrderDetails.StatusId == (int)OrderStatus.Closed || OrderDetails.StatusId == (int)OrderStatus.Canceled))
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errorSupplierBrokerOrderClosed;
                            return response;
                        }
                        else
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errorSupplierBrokerorderNotFound;
                            return response;
                        }
                    }
                    if (response.StatusCode != Status.Failed)
                    {
                        targetOrderId = response.OrderId;
                        var raiseDrModel = viewModel.DeliveryRequest.ToRaiseDrViewModel();
                        raiseDrModel.CreatedBy = userContext.Id;
                        raiseDrModel.AssignedTo = carrierAdminId;
                        raiseDrModel.OrderId = targetOrderId;
                        raiseDrModel.AssignedToRegionId = viewModel.CarrierRegionId;
                        raiseDrModel.AssignedToCompanyId = viewModel.CarrierCompanyId;
                        raiseDrModel.DispactherNote = viewModel.DispatcherNote;
                        raiseDrModel.BrokeredDrId = viewModel.DeliveryRequest.Id;
                        raiseDrModel.BrokeredOrderId = viewModel.OrderId;
                        raiseDrModel.CustomerCompany = userContext.CompanyName;
                        raiseDrModel.IsDispatchRetainedByCustomer = viewModel.IsDispatchRetainedByCustomer;
                        raiseDrModel.IsMaxFillAllowed = viewModel.DeliveryRequest.IsMaxFillAllowed;
                        raiseDrModel.AssignedOn = DateTimeOffset.Now;
                        raiseDrModel.UniqueOrderNo = viewModel.UniqueOrderNo;
                        var drResponse = await freightServiceDomain.BrokerDeliveryRequest(new List<RaiseDeliveryRequestInput>() { raiseDrModel });
                        if (drResponse.StatusCode == Status.Success)
                        {
                            response.StatusCode = drResponse.StatusCode;
                            response.StatusMessage = "Brokered delivery request created successfully";

                            foreach (var item in drResponse.EntityIds)
                            {
                                var message = new BrokerDeliveryRequestMessageViewModel { EntityId = item, AssignedTo = carrierAdminId, AssignedToCompanyId = viewModel.CarrierCompanyId, CompanyType = companyType };
                                var jsonMessage = new JavaScriptSerializer().Serialize(message);
                                await notificationDomain.AddNotificationEventAsync(EventType.BrokerDeliveryRequestCreated, 0, userContext.Id, null, jsonMessage);
                            }
                            //create DR Carrier Sequence
                            if (viewModel.IsDispatchRetainedByCustomer == false)
                            {
                                await SaveUpdateDRCarrierSequence(viewModel, response, freightServiceDomain, drResponse, userContext);

                            }
                        }
                        else
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = drResponse.StatusMessage;
                        }
                    }
                    else
                    {
                        response.StatusMessage = "Failed to create brokered delivery request for this order";
                    }
                }
                // }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CarrierDomain", "BrokerDeliveryRequestToCarrier", ex.Message, ex);
            }
            return response;
        }

        private async Task CreateBlendedGroupBrokeredDRsCarrier(UserContext userContext, DeliveryRequestBrokerInfoViewModel viewModel, DeliveryRequestBrokerResponseViewModel response, FreightServiceDomain freightServiceDomain, int carrierAdminId, NotificationDomain notificationDomain, CompanyType companyType, UserContext carrierUserContext, List<DeliveryRequestViewModel> blendedDRsInfo)
        {
            blendedDRsInfo.ForEach(x =>
            {
                x.UniqueOrderNo = viewModel.UniqueOrderNo;
            });
            foreach (var blendeditem in blendedDRsInfo)
            {
                viewModel.OrderId = blendeditem.OrderId.Value;
                viewModel.DeliveryRequest.RequiredQuantity = blendeditem.RequiredQuantity;
                var blendedDRresponse = await CreateNewOrderForDeliveryRequest(userContext, carrierUserContext, viewModel);
                if (blendedDRresponse.StatusCode != Status.Failed)
                {
                    await CreateBlendedBrokeredDRs(userContext, viewModel, response, freightServiceDomain, carrierAdminId, notificationDomain, companyType, blendeditem, blendedDRresponse);

                }
            }
            response.StatusCode = Status.Success;
            response.StatusMessage = "Brokered delivery request created successfully";
        }

        private static async Task CreateBlendedBrokeredDRs(UserContext userContext, DeliveryRequestBrokerInfoViewModel viewModel, DeliveryRequestBrokerResponseViewModel response, FreightServiceDomain freightServiceDomain, int carrierAdminId, NotificationDomain notificationDomain, CompanyType companyType, DeliveryRequestViewModel blendeditem, DeliveryRequestBrokerResponseViewModel blendedDRresponse)
        {
            var raiseDrModel = blendeditem.ToRaiseDrViewModel();
            raiseDrModel.CreatedBy = userContext.Id;
            raiseDrModel.AssignedTo = carrierAdminId;
            raiseDrModel.OrderId = blendedDRresponse.OrderId;
            raiseDrModel.AssignedToRegionId = viewModel.CarrierRegionId;
            raiseDrModel.AssignedToCompanyId = viewModel.CarrierCompanyId;
            raiseDrModel.DispactherNote = viewModel.DispatcherNote;
            raiseDrModel.BrokeredDrId = blendeditem.Id;
            raiseDrModel.BrokeredOrderId = blendeditem.OrderId.Value;
            raiseDrModel.CustomerCompany = userContext.CompanyName;
            raiseDrModel.IsDispatchRetainedByCustomer = viewModel.IsDispatchRetainedByCustomer;
            raiseDrModel.IsMaxFillAllowed = viewModel.DeliveryRequest.IsMaxFillAllowed;
            raiseDrModel.AssignedOn = DateTimeOffset.Now;
            //Blended Product Details
            raiseDrModel.FuelType = blendeditem.FuelType;
            raiseDrModel.FuelTypeId = blendeditem.FuelTypeId;
            raiseDrModel.IsBlendedRequest = blendeditem.IsBlendedRequest;
            raiseDrModel.BlendedGroupId = blendeditem.BlendedGroupId;
            raiseDrModel.IsAdditive = blendeditem.IsAdditive;
            raiseDrModel.IsActive = true;
            raiseDrModel.IsDeleted = false;
            raiseDrModel.UniqueOrderNo = viewModel.UniqueOrderNo;
            var drResponse = await freightServiceDomain.BrokerDeliveryRequest(new List<RaiseDeliveryRequestInput>() { raiseDrModel });
            if (drResponse.StatusCode == Status.Success)
            {
                //create DR Carrier Sequence
                if (viewModel.IsDispatchRetainedByCustomer == false)
                {
                    await SaveUpdateDRCarrierSequence(viewModel, response, freightServiceDomain, drResponse, userContext);

                }
            }
            else
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = drResponse.StatusMessage;
            }
        }

        public async Task<DeliveryRequestBrokerResponseViewModel> BrokerDeliveryRequestsToCarriers(UserContext userContext, DeliveryRequestBrokerInfoViewModel viewModel)
        {
            List<string> blendedDRIds = new List<string>();
            var response = new DeliveryRequestBrokerResponseViewModel();
            try
            {
                var firstBrokerModel = viewModel.BrokerDrModel.FirstOrDefault();

                if (viewModel.BrokerDrModel.Any(b => b.OrderId == 0))
                {
                    response.StatusMessage = "Please select an valid order for each deilvery request";
                    return response;
                }

                if (viewModel.BrokerDrModel.Any(b => b.CarrierCompanyId == 0))
                {
                    response.StatusMessage = "Please select a carrier to broker this deilvery request";
                    return response;
                }

                var carrierAdminId = Context.DataContext.Users.Where(t => t.CompanyId == firstBrokerModel.CarrierCompanyId).Select(t => t.Id).FirstOrDefault();

                var companyType = CompanyType.Carrier;
                if (firstBrokerModel.IsDispatchRetainedByCustomer == true)
                {
                    companyType = CompanyType.Supplier;
                }
                var carrierUserContext = await new AuthenticationDomain(this).GetUserContextAsync(carrierAdminId, companyType);
                if (firstBrokerModel.IsDispatchRetainedByCustomer == false)
                {
                    var drs = new List<RaiseDeliveryRequestInput>();
                    foreach (var brokerDrDetails in viewModel.BrokerDrModel)
                    {
                        if (string.IsNullOrEmpty(brokerDrDetails.BlendedGroupId))
                        {
                            viewModel.OrderId = brokerDrDetails.OrderId;
                            viewModel.DeliveryRequest = brokerDrDetails.DeliveryRequest;
                            response = await CreateNewOrderForDeliveryRequest(userContext, carrierUserContext, viewModel);

                            if (response.StatusCode != Status.Failed)
                            {
                                var raiseDrModel = brokerDrDetails.DeliveryRequest.ToRaiseDrViewModel();
                                raiseDrModel.CreatedBy = userContext.Id;
                                raiseDrModel.AssignedTo = carrierAdminId;
                                raiseDrModel.OrderId = response.OrderId;
                                raiseDrModel.AssignedToRegionId = brokerDrDetails.CarrierRegionId;
                                raiseDrModel.AssignedToCompanyId = brokerDrDetails.CarrierCompanyId;
                                raiseDrModel.DispactherNote = viewModel.DispatcherNote;
                                raiseDrModel.BrokeredDrId = brokerDrDetails.DeliveryRequest.Id;
                                raiseDrModel.BrokeredOrderId = brokerDrDetails.OrderId;
                                raiseDrModel.CustomerCompany = userContext.CompanyName;
                                raiseDrModel.IsDispatchRetainedByCustomer = brokerDrDetails.IsDispatchRetainedByCustomer;
                                raiseDrModel.IsMaxFillAllowed = brokerDrDetails.DeliveryRequest.IsMaxFillAllowed;
                                raiseDrModel.AssignedOn = DateTimeOffset.Now;
                                raiseDrModel.UniqueOrderNo = brokerDrDetails.UniqueOrderNo;
                                drs.Add(raiseDrModel);
                            }
                            else
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = "Failed to broker delivery requests to order.";
                                break;
                            }
                        }
                        else
                        {
                            //get all the BlendedGroup DRs and Orders details.
                            var freightServiceDomain = new FreightServiceDomain(this);
                            List<DeliveryRequestViewModel> blendedDRsInfo = await freightServiceDomain.GetBlendedGroupDeliveryRequestDetails(new List<string> { brokerDrDetails.BlendedGroupId });
                            if (blendedDRsInfo.Any())
                            {
                                blendedDRsInfo.ForEach(x =>
                                {
                                    x.UniqueOrderNo = brokerDrDetails.UniqueOrderNo;
                                });

                                blendedDRsInfo.ForEach(x => blendedDRIds.Add(x.Id));
                                response = await BlendedBrokerDeliveryRequestsToCarriers(userContext, viewModel, response, carrierAdminId, carrierUserContext, drs, brokerDrDetails, blendedDRsInfo);
                            }
                        }
                    }

                    if (response.StatusCode != Status.Failed)
                    {
                        //create new brokered drs - non blended DRs
                        var nonBlendedDRs = drs.Where(x => !x.IsBlendedRequest).ToList();
                        var drResponse = await new FreightServiceDomain(this).BrokerDeliveryRequest(nonBlendedDRs);
                        var blendedDRs = drs.Where(x => x.IsBlendedRequest).ToList();
                        var drBlendedResponse = new StatusViewModel();
                        drBlendedResponse.StatusCode = Status.Success;
                        if (blendedDRs.Any())
                        {
                            drBlendedResponse = await new FreightServiceDomain(this).BrokerDeliveryRequest(blendedDRs);
                        }
                        if (drResponse.StatusCode == Status.Success && drBlendedResponse.StatusCode == Status.Success)
                        {
                            foreach (var item in drResponse.EntityIds)
                            {
                                var message = new BrokerDeliveryRequestMessageViewModel { EntityId = item, AssignedTo = carrierAdminId, AssignedToCompanyId = firstBrokerModel.CarrierCompanyId, CompanyType = companyType };
                                var jsonMessage = new JavaScriptSerializer().Serialize(message);
                                await new NotificationDomain(this).AddNotificationEventAsync(EventType.BrokerDeliveryRequestCreated, 0, userContext.Id, null, jsonMessage);
                            }
                            var blendedDRsnotificationsInfo = blendedDRs.GroupBy(x => new { x.IsBlendedRequest, x.BlendedGroupId }).Select(x => x.Key.BlendedGroupId).ToList();
                            if (blendedDRsnotificationsInfo.Any())
                            {
                                foreach (var item in blendedDRsnotificationsInfo)
                                {
                                    var message = new BrokerDeliveryRequestMessageViewModel { EntityId = item, AssignedTo = carrierAdminId, AssignedToCompanyId = firstBrokerModel.CarrierCompanyId, CompanyType = companyType, BlendedGroupId = item };
                                    var jsonMessage = new JavaScriptSerializer().Serialize(message);
                                    await new NotificationDomain(this).AddNotificationEventAsync(EventType.BrokerDeliveryRequestCreated, 0, userContext.Id, null, jsonMessage);
                                }
                            }
                            //create DR Carrier Sequence
                            if (firstBrokerModel.IsDispatchRetainedByCustomer == false)
                            {
                                await SaveUpdateDRsCarrierSequence(viewModel, response, new FreightServiceDomain(this), drResponse, userContext);
                            }
                        }
                        else
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = drResponse.StatusMessage;
                        }
                        //reset dsb 
                        if (response.StatusCode == Status.Success)
                        {
                            var deliveryReqIds = viewModel.BrokerDrModel.Select(t => t.DeliveryRequest.Id).ToList();
                            if (blendedDRIds.Any())
                            {
                                deliveryReqIds.AddRange(blendedDRIds);
                            }
                            var model = new ResetDeliveryGroupScheduleModel
                            {
                                CompanyId = userContext.CompanyId,
                                UserId = userContext.Id,
                                ScheduleBuilderId = viewModel.ScheduleBuilderId,
                                DeliveryRequestIds = deliveryReqIds.Distinct().ToList()
                            };
                            model = await new ScheduleBuilderDomain(this).RemoveScheduleBuilderDrs(model);

                            if (model.StatusCode != Status.Failed)
                            {
                                //update schedules
                                if (model.DeliveryGroupIds.Any())
                                {
                                    var _delGroupResponse = await new ScheduleBuilderDomain(this).ResetDeliveryGroup(model.DeliveryScheduleIds, userContext);
                                    if (_delGroupResponse.StatusCode != Status.Success)
                                    {
                                        response.StatusCode = Status.Failed;
                                        response.StatusMessage = "Unable to update delivery group details.";
                                        return response;
                                    }
                                }
                                if (model.DeliveryScheduleIds.Any())
                                {
                                    var _delScheduleResponse = await new ScheduleBuilderDomain(this).ResetDeliverySchedules(model.DeliveryScheduleIds, userContext);
                                    if (_delScheduleResponse.StatusCode != Status.Success)
                                    {
                                        response.StatusCode = Status.Failed;
                                        response.StatusMessage = "Unable to update delivery schedule details.";
                                        return response;
                                    }
                                }
                                response.StatusCode = Status.Success;
                                response.StatusMessage = "Delivery request(s) assigned to carrier.";
                            }
                            else
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = "Unable to update schedule builder details.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CarrierDomain", "BrokerDeliveryRequestsToCarriers", ex.Message, ex);
            }
            return response;
        }

        private async Task<DeliveryRequestBrokerResponseViewModel> BlendedBrokerDeliveryRequestsToCarriers(UserContext userContext, DeliveryRequestBrokerInfoViewModel viewModel, DeliveryRequestBrokerResponseViewModel response, int carrierAdminId, UserContext carrierUserContext, List<RaiseDeliveryRequestInput> drs, BrokerDrModel brokerDrDetails, List<DeliveryRequestViewModel> blendedDRsInfo)
        {
            foreach (var blendedDRitem in blendedDRsInfo)
            {
                viewModel.OrderId = blendedDRitem.OrderId.GetValueOrDefault();
                viewModel.DeliveryRequest = blendedDRitem;
                response = await CreateNewOrderForDeliveryRequest(userContext, carrierUserContext, viewModel);

                if (response.StatusCode != Status.Failed)
                {
                    var raiseDrModel = blendedDRitem.ToRaiseDrViewModel();
                    raiseDrModel.CreatedBy = userContext.Id;
                    raiseDrModel.AssignedTo = carrierAdminId;
                    raiseDrModel.OrderId = response.OrderId;
                    raiseDrModel.AssignedToRegionId = brokerDrDetails.CarrierRegionId;
                    raiseDrModel.AssignedToCompanyId = brokerDrDetails.CarrierCompanyId;
                    raiseDrModel.DispactherNote = viewModel.DispatcherNote;
                    raiseDrModel.BrokeredDrId = blendedDRitem.Id;
                    raiseDrModel.BrokeredOrderId = blendedDRitem.OrderId.GetValueOrDefault();
                    raiseDrModel.CustomerCompany = userContext.CompanyName;
                    raiseDrModel.IsDispatchRetainedByCustomer = brokerDrDetails.IsDispatchRetainedByCustomer;
                    raiseDrModel.IsMaxFillAllowed = brokerDrDetails.DeliveryRequest.IsMaxFillAllowed;
                    raiseDrModel.AssignedOn = DateTimeOffset.Now;
                    //Blended Product Details
                    raiseDrModel.FuelType = blendedDRitem.FuelType;
                    raiseDrModel.FuelTypeId = blendedDRitem.FuelTypeId;
                    raiseDrModel.IsBlendedRequest = blendedDRitem.IsBlendedRequest;
                    raiseDrModel.BlendedGroupId = blendedDRitem.BlendedGroupId;
                    raiseDrModel.IsAdditive = blendedDRitem.IsAdditive;
                    raiseDrModel.UniqueOrderNo = blendedDRitem.UniqueOrderNo;
                    raiseDrModel.IsActive = true;
                    raiseDrModel.IsDeleted = false;
                    drs.Add(raiseDrModel);
                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = "Failed to broker delivery requests to order.";
                    break;
                }
            }

            return response;
        }

        private static async Task SaveUpdateDRsCarrierSequence(DeliveryRequestBrokerInfoViewModel viewModel, DeliveryRequestBrokerResponseViewModel response, FreightServiceDomain freightServiceDomain, StatusViewModel drResponse, UserContext userContext)
        {
            if (viewModel.CarrierInfo != null && viewModel.CarrierInfo.Any() && drResponse.StatusCode == Status.Success)
            {
                List<DRCarrierSequenceModel> dRCarrierSequenceList = new List<DRCarrierSequenceModel>();

                foreach (var brokerDrDetails in viewModel.BrokerDrModel)
                {
                    var dRCarrierSequenceModel = new DRCarrierSequenceModel
                    {
                        DeliveryRequestId = drResponse.EntityParentIds[brokerDrDetails.DeliveryRequest.Id],
                        RegionId = brokerDrDetails.CarrierRegionId,
                        TfxSupplierCompanyId = userContext.CompanyId,
                        TfxSupplierOrderId = brokerDrDetails.OrderId,
                        CarrierInfo = viewModel.CarrierInfo
                    };

                    dRCarrierSequenceList.Add(dRCarrierSequenceModel);
                }
                var drCarrierSeqResponse = await freightServiceDomain.SaveDRCarrierSequence(dRCarrierSequenceList);
                response.StatusCode = drCarrierSeqResponse.StatusCode;
                response.StatusMessage = drResponse.StatusMessage;
            }
        }

        public async Task<StatusViewModel> BrokerDeliveryRequestToNextCarrier(DeliveryRequestBrokerInfoViewModel viewModel, string deliveryReqId)
        {
            var statusResponse = new StatusViewModel();
            var response = new DeliveryRequestBrokerResponseViewModel();
            UserContext userContext = new UserContext();
            try
            {
                if (viewModel.OrderId == 0)
                {
                    response.StatusMessage = "Please select an order to broker this deilvery request";
                    return response;
                }

                if (viewModel.CarrierCompanyId == 0)
                {
                    response.StatusMessage = "Please select a carrier to broker this deilvery request";
                    return response;
                }

                var freightServiceDomain = new FreightServiceDomain(this);
                var carrierAdminId = Context.DataContext.Users.Where(t => t.CompanyId == viewModel.CarrierCompanyId).Select(t => t.Id).FirstOrDefault();
                var authDomain = new AuthenticationDomain(this);
                var companyType = CompanyType.Carrier;
                if (viewModel.IsDispatchRetainedByCustomer == true)
                {
                    companyType = CompanyType.Supplier;
                }
                var carrierUserContext = await authDomain.GetUserContextAsync(carrierAdminId, companyType);
                var targetOrderId = 0;
                var supplierAdminId = Context.DataContext.Users.Where(t => t.CompanyId == viewModel.DeliveryRequest.SupplierCompanyId && t.IsActive && !t.IsDeleted).Select(t => t.Id).FirstOrDefault();
                if (viewModel.IsDispatchRetainedByCustomer == false)
                {
                    userContext = await authDomain.GetUserContextAsync(supplierAdminId, companyType);
                    response = await CreateNewOrderForDeliveryRequest(userContext, carrierUserContext, viewModel);
                }
                if (response.StatusCode != Status.Failed)
                {
                    targetOrderId = response.OrderId;
                    var brokeredDRModel = new BrokeredDeliveryRequestInput();
                    brokeredDRModel.Id = deliveryReqId;
                    brokeredDRModel.CreatedBy = userContext.Id;
                    brokeredDRModel.AssignedTo = carrierAdminId;
                    brokeredDRModel.OrderId = targetOrderId;
                    brokeredDRModel.AssignedToRegionId = viewModel.CarrierRegionId;
                    brokeredDRModel.AssignedToCompanyId = viewModel.CarrierCompanyId;
                    var drResponse = await freightServiceDomain.UpdateBrokerDeliveryRequestInfo(brokeredDRModel);
                    if (drResponse.StatusCode == Status.Success)
                    {
                        statusResponse.StatusCode = drResponse.StatusCode;
                        statusResponse.StatusMessage = Resource.msgDrRejectedSuccessfully;
                    }
                    else
                    {
                        statusResponse.StatusCode = Status.Failed;
                        statusResponse.StatusMessage = drResponse.StatusMessage;
                    }
                }
                else
                {
                    statusResponse.StatusMessage = "Failed to create brokered delivery request for this order";
                }
                // }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CarrierDomain", "BrokerRejectedDeliveryRequestToNextCarrier", ex.Message, ex);
            }
            return statusResponse;
        }
        private static async Task SaveUpdateDRCarrierSequence(DeliveryRequestBrokerInfoViewModel viewModel, DeliveryRequestBrokerResponseViewModel response, FreightServiceDomain freightServiceDomain, StatusViewModel drResponse, UserContext userContext)
        {
            if (viewModel.CarrierInfo != null && viewModel.CarrierInfo.Any() && drResponse.StatusCode == Status.Success)
            {
                List<DRCarrierSequenceModel> dRCarrierSequenceList = new List<DRCarrierSequenceModel>();
                DRCarrierSequenceModel dRCarrierSequenceModel = new DRCarrierSequenceModel();
                dRCarrierSequenceModel.DeliveryRequestId = drResponse.EntityIds.FirstOrDefault();
                dRCarrierSequenceModel.BlendedGroupId = viewModel.BlendedGroupId;
                dRCarrierSequenceModel.RegionId = viewModel.CarrierRegionId;
                dRCarrierSequenceModel.TfxSupplierCompanyId = userContext.CompanyId;
                dRCarrierSequenceModel.TfxSupplierOrderId = viewModel.OrderId;
                dRCarrierSequenceModel.CarrierInfo = viewModel.CarrierInfo;
                dRCarrierSequenceList.Add(dRCarrierSequenceModel);
                var drCarrierSeqResponse = await freightServiceDomain.SaveDRCarrierSequence(dRCarrierSequenceList);
                response.StatusCode = drCarrierSeqResponse.StatusCode;
                response.StatusMessage = string.Format(Resource.successBrokeredDrToCarrier, viewModel.CarrierInfo[0].Name);

            }
        }

        private async Task<DeliveryRequestBrokerResponseViewModel> CreateNewOrderForDeliveryRequest(UserContext userContext, UserContext carrierUserContext, DeliveryRequestBrokerInfoViewModel viewModel)
        {
            var response = new DeliveryRequestBrokerResponseViewModel();
            var existingEntity = await Context.DataContext.Orders.Where(t => t.Id == viewModel.OrderId)
                                        .Select(t => new { Order = t, t.FuelRequest }).FirstOrDefaultAsync();
            var frDomain = new FuelRequestDomain(this);
            var fuelRequest = existingEntity.FuelRequest;
            var brokerViewModel = frDomain.GetBrokerFrDetails(userContext.CompanyId, fuelRequest, true, false);
            if (brokerViewModel != null)
            {
                brokerViewModel.Type = (int)FuelRequestType.FreightOnlyRequest;
                brokerViewModel.Details.PrivateSupplierList.IsPublicRequest = true;
                brokerViewModel.Details.FuelDeliveryDetails.PoContactId = userContext.Id;
                brokerViewModel.Details.FuelQuantity.QuantityTypeId = (int)QuantityType.SpecificAmount;
                brokerViewModel.Details.FuelQuantity.Quantity = viewModel.DeliveryRequest.RequiredQuantity;
                brokerViewModel.Details.FuelDeliveryDetails.DeliveryTypeId = (int)DeliveryType.OneTimeDelivery;
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        var saveResponse = await frDomain.SaveFreightOnlyBrokerFuelRequestAsync(userContext, brokerViewModel, fuelRequest.Id);
                        if (saveResponse.StatusCode != Status.Failed)
                        {
                            var acceptResponse = await frDomain.AcceptFreightOnlyFuelRequest(userContext, carrierUserContext, brokerViewModel.FuelRequestId);
                            if (acceptResponse.StatusCode != Status.Failed)
                            {
                                response.OrderId = acceptResponse.OrderId;
                                // mark existing order IsEndSupplier Flag to false, as order is brokered to carrier
                                var existingOrder = existingEntity.Order;
                                if (existingOrder != null)
                                {
                                    //existingOrder.IsEndSupplier = false;
                                    existingOrder.UpdatedBy = userContext.Id;
                                    existingOrder.UpdatedDate = DateTimeOffset.Now;
                                    Context.DataContext.Entry(existingOrder).State = EntityState.Modified;
                                    Context.DataContext.SaveChanges();
                                }
                                transaction.Commit();
                            }
                            response.StatusCode = acceptResponse.StatusCode;
                            response.StatusMessage = acceptResponse.StatusMessage;
                        }
                        else
                        {
                            response.StatusCode = saveResponse.StatusCode;
                            response.StatusMessage = saveResponse.StatusMessage;
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        LogManager.Logger.WriteException("CarrierDomain", "CreateNewOrderForDeliveryRequest", ex.Message, ex);
                    }
                }
            }
            return response;
        }

        private static string SetSuccessProcessMessage(string poNumber, string carrier)
        {
            StringBuilder processMessage = new StringBuilder();
            processMessage.Append("<p class='color-green'>").Append("<b>Freight-Only Order created successfully for </b>")
                            .Append($"PO#: {poNumber} for Carrier company: {carrier} </p><br>");
            return processMessage.ToString();
        }

        private static string SetErrorProcessMessage(string poNumber, string carrier)
        {
            StringBuilder processMessage = new StringBuilder();
            processMessage.Append("<p class='color-green'>").Append("<b>Freight-Only Order already created for </b>")
                            .Append($"PO#: {poNumber} for Carrier company: {carrier} </p><br>");
            return processMessage.ToString();
        }

        public async Task<StatusViewModel> CreateOrdersForCarrier(UserContext userContext, List<SupplierCarrierViewModel> carriers)
        {
            StatusViewModel response = new StatusViewModel();
            var frDomain = new FuelRequestDomain(this);

            //check open orders for each job
            //create FR - FreightOnly type
            //create brokered Order

            foreach (var carrier in carriers)
            {
                var user = Context.DataContext.Users.Where(t => t.CompanyId == carrier.Carrier.Id).FirstOrDefault();
                if (user != null)
                {
                    var carrierUserContext = await new AuthenticationDomain(this).GetUserContextAsync(user.Id, CompanyType.Carrier);

                    foreach (var job in carrier.Jobs)
                    {
                        var jobId = job.Job.Id;
                        var fuelrequestOfEachJob = await Context.DataContext.Jobs.Where(t => t.Id == jobId && t.IsActive)
                                                .SelectMany(t => t.FuelRequests)
                                                .Where(t => t.FuelRequestXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)FuelRequestStatus.Accepted)
                                                .Where(t => t.Orders.Any(t1 => t1.OrderXStatuses.FirstOrDefault(t2 => t2.IsActive).StatusId == (int)OrderStatus.Open))
                                                .ToListAsync();
                        foreach (var fr in fuelrequestOfEachJob)
                        {
                            var mainOrder = fr.Orders.FirstOrDefault(t1 => t1.OrderXStatuses.FirstOrDefault(t2 => t2.IsActive).StatusId == (int)OrderStatus.Open);
                            if (mainOrder != null)
                            {
                                var cloneModel = fr.ToCloneRequestViewModel();
                                using (var transaction = Context.DataContext.Database.BeginTransaction())
                                {
                                    try
                                    {
                                        FuelRequest cloneFuelRequest = await frDomain.CreateFRClone(cloneModel, carrierUserContext.Id, fr);
                                        cloneFuelRequest.FuelRequestTypeId = (int)FuelRequestType.FreightOnlyRequest;
                                        cloneFuelRequest.ParentId = fr.GetParentId();

                                        Context.DataContext.Entry(cloneFuelRequest).State = EntityState.Modified;
                                        await Context.CommitAsync();

                                        new ThirdPartyOrderDomain(this).SetFuelRequestStatus(carrierUserContext, cloneFuelRequest);

                                        //create clone of Order 
                                        var newFreightOnlyOrder = mainOrder.ToNewOrder(cloneFuelRequest, carrierUserContext);
                                        cloneFuelRequest.Orders.Add(newFreightOnlyOrder);
                                        await Context.CommitAsync();

                                        newFreightOnlyOrder.PoNumber = new HelperDomain(this).GetPoNumber(cloneFuelRequest, mainOrder.IsProFormaPo, newFreightOnlyOrder.Id);
                                        newFreightOnlyOrder.SetNewOrderAdditionalDetails(mainOrder, cloneFuelRequest, carrierUserContext);
                                        await Context.CommitAsync();

                                        transaction.Commit();
                                    }
                                    catch (Exception ex)
                                    {
                                        transaction.Rollback();
                                        LogManager.Logger.WriteException("CarrierDomain", "CreateOrdersForCarrier", ex.Message, ex);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return response;
        }

        public async Task<StatusViewModel> DeleteJobCarrierDetails(SupplierCarrierViewModel viewModel)
        {
            var response = new StatusViewModel();


            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {

                    if (viewModel.Jobs.Count > 0)
                    {
                        var jobIds = viewModel.Jobs.Select(t => t.Job.Id).ToList();
                        var jobCarrierDetailIds = Context.DataContext.JobCarrierDetails.Where(t =>
                                           jobIds.Contains(t.JobId) && t.IsActive == true).Select(t => t.Id).ToList();

                        var carrierUserEmailDetailIds = Context.DataContext.CarrierEmailSettings.Where(t =>
                                              jobIds.Contains(t.JobId) && t.IsActive).Select(t => t.Id)
                                              .ToList();

                        if (jobCarrierDetailIds != null && jobCarrierDetailIds.Any())
                        {

                            var jobCarrierDetails = Context.DataContext.JobCarrierDetails.Where(t =>
                                       jobCarrierDetailIds.Contains(t.Id)).ToList();
                            if (jobCarrierDetails != null && jobCarrierDetails.Any())
                            {
                                jobCarrierDetails.ForEach(t => t.IsActive = false);
                            }
                        }
                        if (carrierUserEmailDetailIds != null && carrierUserEmailDetailIds.Any())
                        {
                            var carrierUserEmailDetail = Context.DataContext.CarrierEmailSettings.Where(t =>
                                     carrierUserEmailDetailIds.Contains(t.Id)).ToList();
                            if (carrierUserEmailDetail != null && carrierUserEmailDetail.Any())
                            {
                                carrierUserEmailDetail.ForEach(t => t.IsActive = false);
                            }
                        }
                        await Context.DataContext.SaveChangesAsync();
                        transaction.Commit();
                        response.StatusCode = Status.Success;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("CarrierDomain", "DeleteJobCarrierDetails", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<List<CarrierJobDetailsModel>> GetAssignedCarrierUsers(int supplierCompanyId)
        {
            var response = new List<CarrierJobDetailsModel>();
            try
            {
                if (supplierCompanyId > 0)
                {
                    var fsDomain = new FreightServiceDomain(this);
                    var result = await fsDomain.GetAssignedCarrierUsers(supplierCompanyId);
                    if (result != null && result.StatusCode == Status.Success)
                    {
                        response = result.Carriers;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CarrierDomain", "GetAssignedCarrierUsers", ex.Message, ex);
            }
            return response;
        }
        public List<SupplierCarrierViewModel> GetCarrierUserEmails(List<SupplierCarrierViewModel> viewModel)
        {
            if (viewModel != null)
            {
                var carrierCompanyIds = viewModel.Select(t => t.Carrier.Id).ToList();
                var carrierJobWithEmail = Context.DataContext.CarrierEmailSettings.Where(t => carrierCompanyIds.Contains(t.CarrierCompanyId) && t.IsActive).
                                    Select(t => new
                                    CarrierJobWithEmailViewModel
                                    {
                                        JobId = t.JobId,
                                        UserId = t.User.Id,
                                        CarrierCompanyId = t.CarrierCompanyId,
                                        Email = t.User.FirstName + " " + t.User.LastName + " (" + t.User.Email + ")"
                                    })
                                    .ToList();


                foreach (var jobList in viewModel)
                {
                    if (jobList != null)
                    {
                        foreach (var item in jobList.Jobs)
                        {
                            item.Job.Emails = carrierJobWithEmail.Where(t => t.CarrierCompanyId == jobList.Carrier.Id && t.JobId == item.Job.Id).
                                                Select(t => new DropdownDisplayItem
                                                {
                                                    Id = t.UserId,
                                                    Name = t.Email
                                                }).ToList();
                        }
                    }
                }
            }
            return viewModel;
        }

        public async Task<StatusViewModel> UpdateJobCarrierDetails(SupplierCarrierViewModel viewModel, int UserId, int createdByCompanyID, bool isUpdatedByJobGrid = false)
        {
            var response = new StatusViewModel();


            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    List<SupplierCarrierViewModel> SupplierCarrierList = new List<SupplierCarrierViewModel>();
                    if (!isUpdatedByJobGrid)
                    {
                        var jobCarrierDetailList = Context.DataContext.JobCarrierDetails.Where(t =>
                                            t.CarrierCompanyId == viewModel.Carrier.Id && t.IsActive == true)
                                           .ToList();

                        var existingJobs = viewModel.Jobs.Where(t => jobCarrierDetailList.Any(t1 => t1.Job.Id == t.Job.Id)).ToList();
                        //var existingJobs = jobCarrierDetailList.Where(t => viewModel.Jobs == null || viewModel.Jobs.Any(t1 => t1.Job.Id == t.JobId)).ToList();
                        var existingJobsIds = existingJobs.Select(t => t.Job.Id).ToList();
                        var carrierExistingUserEmailDetail = Context.DataContext.CarrierEmailSettings.Where(t =>
                                                           t.CarrierCompanyId == viewModel.Carrier.Id
                                                        && existingJobsIds.Contains(t.JobId)
                                                        && t.IsActive)
                                                       .ToList();

                        if (carrierExistingUserEmailDetail.Any())
                        {
                            carrierExistingUserEmailDetail.ForEach(t => t.IsActive = false);
                        }

                        if (existingJobs != null && existingJobs.Any())
                        {
                            foreach (var job in existingJobs)
                            {
                                if (job.Job.Emails != null && job.Job.Emails.Any())
                                {
                                    foreach (var carrieruser in job.Job.Emails)
                                    {
                                        CarrierEmailSetting carrierEmailSetting = new CarrierEmailSetting();
                                        carrierEmailSetting.JobId = job.Job.Id;
                                        carrierEmailSetting.CarrierCompanyId = viewModel.Carrier.Id;
                                        carrierEmailSetting.UserId = carrieruser.Id;
                                        carrierEmailSetting.IsActive = true;
                                        carrierEmailSetting.CreatedBy = UserId;
                                        carrierEmailSetting.UpdatedBy = UserId;
                                        carrierEmailSetting.CreatedDate = DateTimeOffset.Now;
                                        carrierEmailSetting.UpdatedDate = DateTimeOffset.Now;
                                        Context.DataContext.CarrierEmailSettings.Add(carrierEmailSetting);
                                    }
                                }

                            }
                            //Context.DataContext.JobCarrierDetails.Add(jobCarrierDetail);
                            await Context.CommitAsync();
                        }

                        var deletedJobs = jobCarrierDetailList.Where(t => viewModel.Jobs == null || !viewModel.Jobs.Any(t1 => t1.Job.Id == t.JobId)).ToList();

                        if (deletedJobs != null && deletedJobs.Any())
                        {
                            deletedJobs.ForEach(t => t.IsActive = false);
                            response.StatusCode = Status.Success;
                        }

                        var deletedJobIds = deletedJobs.Select(t => t.JobId).ToList();
                        var carrierUserEmailDetail = Context.DataContext.CarrierEmailSettings.Where(t =>
                                                           t.CarrierCompanyId == viewModel.Carrier.Id
                                                        && deletedJobIds.Contains(t.JobId)
                                                        && t.IsActive)
                                                       .ToList();

                        if (carrierUserEmailDetail.Any())
                        {
                            carrierUserEmailDetail.ForEach(t => t.IsActive = false);
                        }

                        var newJobs = viewModel.Jobs.Where(t => !jobCarrierDetailList.Any(t1 => t1.JobId == t.Job.Id)).ToList();
                        if (newJobs != null && newJobs.Any())
                        {
                            var carrierViewModel = new SupplierCarrierViewModel { Jobs = newJobs, Carrier = viewModel.Carrier, SupplierCompanyId = viewModel.SupplierCompanyId, SupplierCompanyName = viewModel.SupplierCompanyName };
                            SupplierCarrierList.Add(carrierViewModel);
                            response = await ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().SaveJobCarrierDetailJob(SupplierCarrierList, UserId, createdByCompanyID);
                        }
                    }
                    else
                    {
                        var jobId = viewModel.Jobs?.FirstOrDefault().Job?.Id;
                        var jobCarrierDetails = Context.DataContext.JobCarrierDetails.Where(t => t.JobId == jobId && t.CreatedByCompanyId.Equals(createdByCompanyID) && t.IsActive == true).OrderByDescending(x => x.Id).FirstOrDefault();
                        if (jobCarrierDetails != null)
                        {
                            jobCarrierDetails.CarrierCompanyId = viewModel.Carrier.Id;
                        }
                        else
                        {
                            SupplierCarrierList.Add(viewModel);
                            response = await ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().SaveJobCarrierDetailJob(SupplierCarrierList, UserId, createdByCompanyID);
                        }
                    }
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Status.Success.ToString();
                    await Context.DataContext.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Status.Failed.ToString();
                    LogManager.Logger.WriteException("CarrierDomain", "UpdateJobCarrierDetails", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusViewModel> createFreightOrdersForAssignedCarrier(List<SupplierCarrierViewModel> carriers, UserContext userContext)
        {
            var response = new StatusViewModel(Status.Failed);
            response.StatusCode = Status.Success;
            try
            {
                foreach (var carrier in carriers)
                {
                    var carrierCompanyId = carrier.Carrier.Id;
                    if (carrier.Jobs.Any())
                    {
                        var carrierJobIds = carrier.Jobs.Select(t => t.Job.Id).ToList();
                        var Openorders = await Context.DataContext.Orders
                                                      .Where(t => t.AcceptedCompanyId == userContext.CompanyId && t.IsActive &&
                                                              t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open
                                                              && carrierJobIds.Contains(t.FuelRequest.JobId)
                                                          )
                                                        .Select(t => new
                                                        {
                                                            JobId = t.FuelRequest.Job.Id,
                                                            IsBrokerFr = t.FuelRequest.FuelRequests1.Any(t1 => t1.Orders.Any(t2 => t2.OrderXStatuses.FirstOrDefault(t3 => t3.IsActive).StatusId == (int)OrderStatus.Open)),
                                                            t.FuelRequestId,
                                                            t.Id,
                                                            t.PoNumber,
                                                            t.AcceptedBy,
                                                            t.AcceptedCompanyId
                                                        })
                                                    .ToListAsync();
                        var fuelrequestDomain = new FuelRequestDomain(this);
                        if (Openorders != null && Openorders.Any())
                        {
                            foreach (var jobId in carrierJobIds)
                            {
                                if (jobId > 0)
                                {
                                    var orderstoBroker = Openorders.Where(t => t.JobId == jobId && !t.IsBrokerFr).ToList();
                                    if (orderstoBroker != null && orderstoBroker.Any())
                                    {
                                        foreach (var order in orderstoBroker)
                                        {
                                            fuelrequestDomain.AddQueueMsgToCreateFreightOnlyOrder1(order, carrierCompanyId);
                                        }
                                        response.StatusCode = (int)Status.Success;
                                        response.StatusMessage = Resource.errMessageSuccess;
                                    }
                                    else
                                    {
                                        response.StatusCode = (int)Status.Success;
                                        response.StatusMessage = Resource.errMessageNoOpenOrders;
                                    }
                                }

                            }
                        }
                        else
                        {
                            response.StatusCode = (int)Status.Success;
                            response.StatusMessage = Resource.errMessageNoOpenOrders;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CarrierDomain", "createFreightOrdersForAssignedCarrier", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> EditFreightOnlyOrders(EditFreightOnlyOrderViewModel JobIdsToEdit, UserContext userContext)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                if (JobIdsToEdit.newJobsIds != null && JobIdsToEdit.newJobsIds.Any() && JobIdsToEdit.IsCreateOrder)
                {
                    //Create Freight Order for new job ids 
                    var carrierCompanyId = JobIdsToEdit.CarrierCompanyId;
                    //var jobIds = new List<int>();
                    //var orders = new List<Order>();

                    var Openorders = await Context.DataContext.Orders
                                                    .Where(t => t.AcceptedCompanyId == userContext.CompanyId && t.IsActive &&
                                                                JobIdsToEdit.newJobsIds.Contains(t.FuelRequest.JobId) &&
                                                            t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open
                                                        )
                                                    .Select(t => new
                                                    {
                                                        JobId = t.FuelRequest.Job.Id,
                                                        IsBrokerFr = t.FuelRequest.FuelRequests1.Any(t1 => t1.Orders.Any(t2 => t2.OrderXStatuses.FirstOrDefault(t3 => t3.IsActive).StatusId == (int)OrderStatus.Open)),
                                                        t.FuelRequestId,
                                                        t.Id,
                                                        t.PoNumber,
                                                        t.AcceptedBy,
                                                        t.AcceptedCompanyId
                                                    })
                                                    .ToListAsync();
                    var fuelrequestDomain = new FuelRequestDomain(this);
                    if (Openorders != null && Openorders.Any())
                    {
                        foreach (var jobId in JobIdsToEdit.newJobsIds)
                        {
                            //jobIds.Add(jobId);
                            if (jobId > 0)
                            {
                                var orderstoBroker = Openorders.Where(t => t.JobId == jobId && !t.IsBrokerFr).ToList();
                                if (orderstoBroker != null && orderstoBroker.Any())
                                {
                                    //var orderTobroker = ordersOngivenJob.Where(t => ).ToList();

                                    //foreach (var order in ordersOngivenJob)
                                    //{
                                    //    //Check for not creating freight-only-order if theres already brokered order exists
                                    //    //var isOpenBrokerOrderExists = IsOpenBrokerOrderExist(order);
                                    //    //if (!isOpenBrokerOrderExists)
                                    //    //{
                                    //    //    orders.Add(order);
                                    //    //}
                                    //}
                                    foreach (var order in orderstoBroker)
                                    {
                                        fuelrequestDomain.AddQueueMsgToCreateFreightOnlyOrder1(order, carrierCompanyId);
                                    }
                                    response.StatusCode = (int)Status.Success;
                                    response.StatusMessage = Resource.errMessageSuccess;
                                }
                                else
                                {
                                    response.StatusCode = (int)Status.Success;
                                    response.StatusMessage = Resource.errMessageNoOpenOrders;
                                }
                            }

                        }
                    }



                }
                if (JobIdsToEdit.removedJobsIds != null && JobIdsToEdit.removedJobsIds.Any())
                {
                    //close existing FOs assigned to given carrier companyId
                    var carrierCompanyId = JobIdsToEdit.CarrierCompanyId;
                    //var jobIds = new List<int>();
                    //var ordersToClose = new List<Order>();
                    var OpenordersToClose = await Context.DataContext.Orders
                                                   .Where(t => t.AcceptedCompanyId == carrierCompanyId && t.IsActive &&
                                                                JobIdsToEdit.removedJobsIds.Contains(t.FuelRequest.JobId) &&
                                                           t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open
                                                       )
                                                   .Select(t => new
                                                   {
                                                       t.Id,
                                                   })
                                                   .ToListAsync();
                    //if (Openorders.Any())
                    //{
                    //    //foreach (var jobId in JobIdsToEdit.removedJobsIds)
                    //    //{
                    //    //    //jobIds.Add(jobId);
                    //    //    if (jobId > 0)
                    //    //    {
                    //    //        var order = Openorders.Where(t => t.JobId == jobId).ToList();
                    //    //        //ordersToClose.AddRange(order);
                    //    //    }

                    //    //}
                    //}
                    if (OpenordersToClose.Any())
                    {
                        var orderIds = OpenordersToClose.Select(t => t.Id).ToList();
                        //foreach (Order order in OpenordersToClose)
                        //{
                        //    orderIds.Add(order.Id);
                        //}
                        var closeFreightOrderQueMsg = new CloseFreightOnlyOrderQueueMsg();

                        closeFreightOrderQueMsg.CarrierCompanyId = carrierCompanyId;
                        closeFreightOrderQueMsg.OrderIds = orderIds;
                        closeFreightOrderQueMsg.SupplierCompanyId = userContext.CompanyId;
                        closeFreightOrderQueMsg.SupplierUserId = userContext.Id;

                        var fuelRequestDomain = new FuelRequestDomain(this);
                        fuelRequestDomain.AddQueueMsgToCloseFreightOnlyOrder(closeFreightOrderQueMsg);

                        response.StatusCode = (int)Status.Success;
                        response.StatusMessage = Resource.errMessageSuccess;
                    }
                    else
                    {
                        response.StatusCode = (int)Status.Success;
                        response.StatusMessage = Resource.errMessageNoOpenOrders;
                    }

                }
                response.StatusCode = (int)Status.Success;

            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                LogManager.Logger.WriteException("CarrierDomain", "EditFreightOnlyOrders", ex.Message, ex);
            }

            return response;
        }

        public async Task<StatusViewModel> closeAssignedOrdersforCarrier(EditFreightOnlyOrderViewModel OrdersToClose, UserContext userContext)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                var CarrierCompanyId = OrdersToClose.CarrierCompanyId;
                var JobIds = OrdersToClose.removedJobsIds;

                var ordersToClose = await Context.DataContext.Orders
                                             .Where(t => t.AcceptedCompanyId == CarrierCompanyId && t.IsActive &&
                                                     t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open
                                                     && JobIds.Contains(t.FuelRequest.JobId)
                                                 )
                                                   .Select(t => new
                                                   {
                                                       t.Id,
                                                   })
                                             .ToListAsync();
                if (ordersToClose != null && ordersToClose.Any())
                {
                    var orderIds = ordersToClose.Select(t => t.Id).ToList();
                    var closeFreightOrderQueMsg = new CloseFreightOnlyOrderQueueMsg();

                    closeFreightOrderQueMsg.CarrierCompanyId = CarrierCompanyId;
                    closeFreightOrderQueMsg.OrderIds = orderIds;
                    closeFreightOrderQueMsg.SupplierCompanyId = userContext.CompanyId;
                    closeFreightOrderQueMsg.SupplierUserId = userContext.Id;

                    var fuelRequestDomain = new FuelRequestDomain(this);
                    fuelRequestDomain.AddQueueMsgToCloseFreightOnlyOrder(closeFreightOrderQueMsg);
                    response.StatusCode = (int)Status.Success;
                    response.StatusMessage = Resource.errMessageSuccess;
                }
                else
                {
                    response.StatusCode = (int)Status.Success;
                    response.StatusMessage = Resource.errMessageNoOpenOrders;
                }
                response.StatusCode = (int)Status.Success;

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CarrierDomain", "closeAssignedOrdersforCarrier", ex.Message, ex);
            }

            return response;
        }


        public StatusViewModel ProcessCloseFreightOnlyOrderQueueMsg(CloseFreightOnlyOrderQueueMsg bulkUploadMsg, List<string> errorInfo)
        {
            var response = new StatusViewModel(Status.Failed);

            try
            {
                if (bulkUploadMsg.CarrierCompanyId > 0 && bulkUploadMsg.OrderIds != null && bulkUploadMsg.OrderIds.Any())
                {
                    var user = Context.DataContext.Users.Where(t => t.CompanyId == bulkUploadMsg.CarrierCompanyId).FirstOrDefault();
                    if (user != null)
                    {
                        var supplierUserContext = new AuthenticationDomain(this).GetUserContextAsync(bulkUploadMsg.SupplierUserId, CompanyType.Supplier).Result;

                        var OrderIdsToClose = new List<int>();
                        OrderIdsToClose.AddRange(bulkUploadMsg.OrderIds);
                        var orderDomain = new OrderDomain(this);

                        foreach (var orderId in OrderIdsToClose)
                        {
                            // which context to pass, which user id to pass 
                            var result = orderDomain.CloseOrderAsync(supplierUserContext, orderId, supplierUserContext.Id).Result;
                            if (result.StatusCode == Status.Success)
                            {
                                errorInfo.Add("Freight Only Order with OrderId" + orderId + " closed successfully");
                            }
                            else
                            {
                                errorInfo.Add("Freight Only Order with OrderId" + orderId + "Failed to close");
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CarrierDomain", "ProcessCloseFreightOnlyOrderQueueMsg", ex.Message, ex);
            }
            return response;
        }


        public int GetCarrierByJobId(int jobId, int companyId)
        {
            int assignedCarrierId = 0;
            try
            {
                assignedCarrierId = Context.DataContext.JobCarrierDetails.Where(t => t.JobId == jobId && t.CreatedByCompanyId.Equals(companyId) && t.IsActive).OrderByDescending(x => x.Id).FirstOrDefault()?.CarrierCompanyId ?? 0;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CarrierDomain", "GetCarrierByJobId", ex.Message, ex);
            }

            return assignedCarrierId;
        }

        public async Task<StatusViewModel> RecallDeliveryRequest(UserContext userContext, DeliveryRequestViewModel model)
        {
            var response = new StatusViewModel();
            try
            {
                var freightServiceDomain = new FreightServiceDomain(this);
                if (string.IsNullOrEmpty(model.BlendedGroupId))
                {
                    var childDrInfo = await freightServiceDomain.GetChildDeliveryRequestInfo(model.Id);
                    if (childDrInfo.StatusCode == Status.Success)
                    {
                        response = await RecallDeliveryRequestOrderInfo(userContext, model, response, freightServiceDomain, childDrInfo);
                    }
                    else
                    {
                        response.StatusCode = childDrInfo.StatusCode;
                        response.StatusMessage = childDrInfo.StatusMessage;
                    }
                }
                else
                {
                    var childDrInfo = await freightServiceDomain.GetBlendedChildDeliveryRequestInfo(model.BlendedGroupId);
                    if (childDrInfo.Any())
                    {
                        foreach (var item in childDrInfo)
                        {
                            response = await RecallDeliveryRequestOrderInfo(userContext, model, response, freightServiceDomain, item);
                        }
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.valCantRecallScheduledDr;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CarrierDomain", "RecallDeliveryRequest", ex.Message, ex);
            }
            return response;
        }

        private async Task<StatusViewModel> RecallDeliveryRequestOrderInfo(UserContext userContext, DeliveryRequestViewModel model, StatusViewModel response, FreightServiceDomain freightServiceDomain, ChildDeliveryRequestInfoViewModel childDrInfo)
        {
            if (childDrInfo != null && childDrInfo.StatusCode == Status.Success)
            {
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (model.OrderId != 0 && model.OrderId != null)
                        {
                            var spDomain = new StoredProcedureDomain(this);
                            var childOrders = await spDomain.GetBrokeredChildOrders(model.OrderId.Value);

                            //FOR CARRIER
                            if (childOrders.All(t => t.IsDispatchRetainedByCustomer == false))
                            {
                                var orderStatus = await Context.DataContext.OrderXStatuses.FirstOrDefaultAsync(t => t.OrderId == childDrInfo.OrderId && t.IsActive && t.StatusId == (int)OrderStatus.Open);
                                if (orderStatus != null)
                                    orderStatus.IsActive = false;

                                Context.DataContext.OrderXStatuses.Add(new OrderXStatus()
                                {
                                    OrderId = childDrInfo.OrderId,
                                    StatusId = (int)OrderStatus.DrRecalled,
                                    UpdatedBy = userContext.Id,
                                    UpdatedDate = DateTimeOffset.Now,
                                    IsActive = true
                                });
                                await Context.CommitAsync();
                            }
                        }

                        var recallResult = await freightServiceDomain.RecallDeliveryRequest(childDrInfo.BrokeredParentId, childDrInfo.DrId, userContext.Id);
                        if (recallResult != null && recallResult.StatusCode == Status.Success)
                        {
                            transaction.Commit();
                            response = recallResult;
                        }
                    }
                    catch (Exception e)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = "Unable to get brokered delivery request info. Recall operation failed.";
                        transaction.Rollback();
                        LogManager.Logger.WriteException("CarrierDomain", "RecallDeliveryRequest", e.Message, e);
                        return response;
                    }
                }
            }
            else
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = "Unable to get brokered delivery request info. Recall operation failed.";
            }

            return response;
        }

        public bool IsOpenBrokerOrderExist(Order order, int orderId = 0)
        {
            try
            {
                var brokeredFuelRequests = order.FuelRequest.FuelRequests1.ToList();
                if (brokeredFuelRequests != null && brokeredFuelRequests.Any())
                {
                    foreach (var fuelReq in brokeredFuelRequests)
                    {
                        var brokeredOrder = Context.DataContext.Orders.Where(t => t.FuelRequestId == fuelReq.Id && t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open).ToList();
                        if (brokeredOrder != null && brokeredOrder.Any())
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CarrierDomain", "IsOpenBrokerOrderExist", ex.Message, ex);
            }
            return false;
        }

        public async Task<Tuple<List<SupplierCarrierViewModel>, StatusViewModel>> SaveCarrierAssignmentFileAsync(UserContext userContext, string csvText, List<SupplierCarrierViewModel> supplierCarrierViewModels)
        {
            using (var tracer = new Tracer("CarrierDomain", "SaveCarrierAssignmentFileAsync"))
            {
                StatusViewModel response = new StatusViewModel();
                try
                {
                    csvText = RemoveHeaderAndGuidelinesFromFile(csvText);
                    var engine = new FileHelperEngine<CarrierBulkCsvViewModel>();
                    var csvCarrierList = engine.ReadString(csvText).ToList();
                    response = await ValidateCarrierAssignmentFileAsync(userContext, csvCarrierList);
                    if (response.StatusCode == Status.Success)
                    {
                        var CarrierList = JsonConvert.DeserializeObject<List<CarrierBulkViewModel>>(JsonConvert.SerializeObject(csvCarrierList));
                        GetCarrierDataWithIds(CarrierList);

                        supplierCarrierViewModels = GetSupplierCarrierDetails(CarrierList, userContext);
                        var fsDomain = ContextFactory.Current.GetDomain<FreightServiceDomain>();
                        response = await fsDomain.AssignCarriers(supplierCarrierViewModels);
                        if (response.StatusCode == Status.Success)
                        {
                            await ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().SaveJobCarrierDetailJob(supplierCarrierViewModels, userContext.Id, userContext.CompanyId);
                        }

                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("CarrierDomain", "SaveCarrierAssignmentFileAsync", ex.Message, ex);
                }

                return new Tuple<List<SupplierCarrierViewModel>, StatusViewModel>(supplierCarrierViewModels, response);
            }
        }

        private void GetCarrierDataWithIds(List<CarrierBulkViewModel> CarrierList)
        {
            var carrierCompanies = GetCarrierCompaniesAsync(CarrierList);
            var carrierLocations = GetLocationsAsync(CarrierList);
            foreach (var item in CarrierList)
            {
                item.CarrierCompanyId = carrierCompanies.FirstOrDefault(t => t.Name.Trim().ToLower() == item.CarrierCompany.Trim().ToLower()).Id;
                item.Job = carrierLocations.FirstOrDefault(t => t.Job.Name.Trim().ToLower() == item.Location.Trim().ToLower());
            }
        }

        public List<DropdownDisplayItem> GetCarrierCompaniesAsync(List<CarrierBulkViewModel> CarrierList)
        {
            var distinctCarrierCompanies = CarrierList.GroupBy(t => t.CarrierCompany).Select(t => t.Key);
            var carrierCompanies = Context.DataContext.Companies.Where(t => distinctCarrierCompanies.Contains(t.Name) && t.IsActive).Select(t => new DropdownDisplayItem() { Id = t.Id, Name = t.Name }).ToList();
            return carrierCompanies;
        }
        public List<CarrierJobViewModel> GetLocationsAsync(List<CarrierBulkViewModel> CarrierList)
        {
            var distinctLocations = CarrierList.GroupBy(t => t.Location).Select(t => t.Key);
            var locations = Context.DataContext.Orders.Where(t => distinctLocations.Contains(t.FuelRequest.Job.Name))
                                                            .GroupBy(t => t.FuelRequest.JobId)
                                                            .Select(t => t.FirstOrDefault())
                                                            .OrderByDescending(t => t.FuelRequest.JobId)
                                                            .Select(t => new CarrierJobViewModel { Job = new JobWithEmails() { Id = t.FuelRequest.JobId, Name = t.FuelRequest.Job.Name }, BuyerCompanyId = t.FuelRequest.Job.CompanyId, BuyerCompanyName = t.FuelRequest.Job.Company.Name })
                                                            .ToList();
            if (locations != null && locations.Any())
            {
                foreach (var item in locations)
                {
                    item.Job.Emails = new List<DropdownDisplayItem>();
                    item.Job.Emails = GetEmailUsers(CarrierList, item.Job.Name);
                }
            }
            return locations;
        }
        public List<DropdownDisplayItem> GetEmailUsers(List<CarrierBulkViewModel> CarrierList, string JobName)
        {
            var emails = new List<string>();
            var emailList = new List<DropdownDisplayItem>();
            var locations = CarrierList.Where(t => t.Location.Trim().ToLower() == JobName.Trim().ToLower()).ToList();
            if (locations != null && locations.Any())
            {
                foreach (var item in locations)
                {
                    emails.AddRange(item.Email.Split(','));
                }
            }
            if (emails != null && emails.Any())
            {
                emails.ForEach(t => t.Trim().ToLower());
            }
            emailList = Context.DataContext.Users.Where(t => emails.Contains(t.Email.ToLower().Trim()) && t.IsActive).Select(t => new DropdownDisplayItem() { Id = t.Id, Name = t.Email }).ToList();
            return emailList;
        }

        public static List<SupplierCarrierViewModel> GetSupplierCarrierDetails(List<CarrierBulkViewModel> entity, UserContext userContext)
        {
            List<SupplierCarrierViewModel> supplierCarrierViewModels = new List<SupplierCarrierViewModel>();
            var CarrierList = entity.GroupBy(t => t.CarrierCompanyId);
            foreach (var item in CarrierList)
            {
                var supplierCarrierViewModel = new SupplierCarrierViewModel();
                supplierCarrierViewModel.Carrier = new DropdownDisplayItem { Id = item.First().CarrierCompanyId, Name = item.First().CarrierCompany };
                supplierCarrierViewModel.SupplierCompanyId = userContext.CompanyId;
                supplierCarrierViewModel.SupplierCompanyName = userContext.CompanyName;
                supplierCarrierViewModel.CreatedBy = userContext.Id;
                supplierCarrierViewModel.Jobs = item.Select(t => t.Job).ToList();
                supplierCarrierViewModels.Add(supplierCarrierViewModel);
            }
            return supplierCarrierViewModels;
        }
        public async Task<StatusViewModel> ValidateCarrierAssignmentFileAsync(UserContext userContext, List<CarrierBulkCsvViewModel> csvCarrierList)
        {
            using (var tracer = new Tracer("CarrierDomain", "ValidateCarrierAssignmentFileAsync"))
            {
                StatusViewModel response = new StatusViewModel();
                try
                {
                    StringBuilder errorList = new StringBuilder();
                    if (csvCarrierList != null && csvCarrierList.Any())
                    {
                        ValidateBaseRow(csvCarrierList, errorList);
                        if (errorList.Length <= 0)
                        {
                            ValidateInvalidRecords(csvCarrierList, errorList);
                        }
                        if (errorList.Length <= 0)
                        {
                            ValidateDuplicateData(csvCarrierList, errorList);
                        }
                        if (errorList.Length <= 0)
                        {
                            await ValidateAsignedDataAsync(csvCarrierList, errorList, userContext);
                        }
                        if (errorList.Length > 0)
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = errorList.ToString();
                            if (response.StatusMessage.Length > 1000)
                                response.StatusMessage = response.StatusMessage.Substring(0, 999) + ".... Too many errors in file";
                        }
                        else
                        {
                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.messageSuccessCarrierAssignmentBulkUpload;
                        }
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = "No Records found in csv.";
                    }
                }
                catch (Exception ex)
                {
                    response.StatusCode = Status.Failed;
                    LogManager.Logger.WriteException("CarrierDomain", "ValidateCarrierAssignmentFileAsync", ex.Message, ex);
                }

                return response;
            }

        }

        private static void ValidateDuplicateData(List<CarrierBulkCsvViewModel> csvCarrierList, StringBuilder errorList)
        {
            var duplicatesLocations = csvCarrierList.GroupBy(x => x.Location)
                                                 .Where(item => item.Count() > 1)
                                                 .Select(y => y.Key)
                                                 .ToList();
            if (duplicatesLocations != null && duplicatesLocations.Any())
            {
                foreach (var location in duplicatesLocations)
                {
                    errorList.Append("</br>");
                    errorList.AppendLine(string.Format(Resource.errMsgSameLocationToMultipleCarrier, location));
                }
            }
        }
        private static async Task ValidateAsignedDataAsync(List<CarrierBulkCsvViewModel> csvCarrierList, StringBuilder errorList, UserContext userContext)
        {
            var fsDomain = ContextFactory.Current.GetDomain<FreightServiceDomain>();
            var response = await fsDomain.GetAssignedCarriersForSupplier(userContext.CompanyId);
            if (response != null && response.Any())
            {
                foreach (var item in response)
                {
                    if (item.Jobs != null && item.Jobs.Any())
                    {
                        var existingAssignedLocation = csvCarrierList.Where(t => item.Jobs.Any(t1 => t1.Job.Name == t.Location)).FirstOrDefault();
                        if (existingAssignedLocation != null && !string.IsNullOrEmpty(existingAssignedLocation.Location))
                        {
                            errorList.Append("</br>");
                            errorList.AppendLine(string.Format(Resource.errMsgLocationAlreadyAssigned, existingAssignedLocation.Location));
                        }
                    }
                }
            }
        }
        private static void ValidateBaseRow(List<CarrierBulkCsvViewModel> csvCarrierList, StringBuilder errorList)
        {
            var csvInvalidRecords = csvCarrierList.Where(t => string.IsNullOrEmpty(t.CarrierCompany) || string.IsNullOrEmpty(t.Location));
            if (csvInvalidRecords != null && csvInvalidRecords.Any())
            {
                foreach (var item in csvInvalidRecords)
                {
                    if (string.IsNullOrEmpty(item.CarrierCompany) && string.IsNullOrEmpty(item.Location))
                    {
                        errorList.Append("</br>");
                        errorList.AppendLine(string.Format(Resource.errMsgAssignCarrierAndLocation));
                    }
                    else if (string.IsNullOrEmpty(item.CarrierCompany))
                    {
                        errorList.Append("</br>");
                        errorList.AppendLine(string.Format(Resource.errMsgAssignCarrierToLocation, item.Location));
                    }
                    else if (string.IsNullOrEmpty(item.Location))
                    {
                        errorList.Append("</br>");
                        errorList.AppendLine(string.Format(Resource.errMsgAssignLocationToCarrier, item.CarrierCompany));
                    }
                }
            }
        }
        public void ValidateInvalidRecords(List<CarrierBulkCsvViewModel> csvCarrierList, StringBuilder errorList)
        {
            var locations = csvCarrierList.Select(t => t.Location).Distinct();
            var validLocations = Context.DataContext.Jobs.Where(t => locations.Contains(t.Name) && t.IsActive).Select(t => t.Name).ToList();
            var invalidLocations = locations.Where(t => !validLocations.Contains(t)).ToList();
            //var invalidLocations =  Context.DataContext.Jobs.Where(t => csvCarrierList.Any(t1 => t1.Location.Trim().ToLower() != t.Name.Trim().ToLower() || (t1.Location.Trim().ToLower() == t.Name.Trim().ToLower() && !t.IsActive))).ToList();
            if (invalidLocations != null && invalidLocations.Any())
            {
                foreach (var item in invalidLocations)
                {
                    errorList.Append("</br>");
                    errorList.AppendLine(string.Format(Resource.errMsgCarrierLocationIsInvalid, item));
                }
            }
            var carriers = csvCarrierList.Select(t => t.CarrierCompany).Distinct();
            var validCarriers = Context.DataContext.Companies.Where(t => carriers.Contains(t.Name) && t.IsActive).Select(t => t.Name).ToList();
            var invalidCarrier = carriers.Where(t => !validCarriers.Contains(t)).ToList();
            if (invalidCarrier != null && invalidCarrier.Any())
            {
                foreach (var item in invalidCarrier)
                {
                    errorList.Append("</br>");
                    errorList.AppendLine(string.Format(Resource.errMsgCarrierLocationIsInvalid, item));
                }
            }
        }
        private string RemoveHeaderAndGuidelinesFromFile(string csvText)
        {
            csvText = Regex.Replace(csvText.Trim(), @"\A.*", string.Empty, RegexOptions.IgnoreCase);
            csvText = Regex.Replace(csvText.Trim(), @",\n", string.Empty, RegexOptions.IgnoreCase);
            csvText = csvText.TrimEnd(',');

            return csvText;
        }

        public async Task<bool> TriggerCarrierInventoryEmailExport()
        {
            var response = false;
            try
            {
                var supplierCompanyIds = await Context.DataContext.OnboardingPreferences
                                .Where(t => t.IssendInventoryExportEmail && t.IsActive)
                                .Select(t => t.CompanyId).Distinct().ToListAsync();
                bool canSendEmail = ShouldTriggerInventoryExport().Result;
                if (supplierCompanyIds != null && supplierCompanyIds.Any() && canSendEmail)
                {
                    foreach (var companyId in supplierCompanyIds)
                    {
                        await ProcessInventoryDataExportPerCompany(companyId);
                    }
                }


            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CarrierDomain", "TriggerCarrierInventoryEmailExport", ex.Message, ex);
            }
            return response;
        }

        //validates current time > last email sent time
        public async Task<bool> ShouldTriggerInventoryExport()
        {
            var canSend = false;
            try
            {
                var keyInventoryDataExportTimeWindow = ApplicationConstants.KeyCarrierInventoryDataExportTimeWindow;
                var keyLastInventoryExportSentMail = ApplicationConstants.KeyLastInventoryExportEmailDateTime;

                var exportTimeWindow = await Context.DataContext.MstAppSettings.Where(t => t.Key == keyInventoryDataExportTimeWindow)
                                        .Select(t => t.Value).FirstOrDefaultAsync();

                var strLastEmailsentDateTime = await Context.DataContext.MstAppSettings.Where(t => t.Key == keyLastInventoryExportSentMail)
                                            .Select(t => t.Value).FirstOrDefaultAsync();
                if (!string.IsNullOrWhiteSpace(exportTimeWindow) && !string.IsNullOrWhiteSpace(strLastEmailsentDateTime))
                {
                    if (DateTimeOffset.TryParse(strLastEmailsentDateTime, out DateTimeOffset lastEmailsentDateTime))
                    {
                        var diffInhours = (DateTimeOffset.Now - lastEmailsentDateTime).TotalHours;
                        if (diffInhours > Convert.ToInt32(exportTimeWindow))
                        {
                            canSend = true;
                        }
                    }
                }

            }

            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CarrierDomain", "ShouldTriggerInventoryExport", ex.Message, ex);

            }
            return canSend;
        }

        private async Task<bool> ProcessInventoryDataExportPerCompany(int supplierCompanyId)
        {
            var isProcessed = false;
            try
            {
                var domain = new StoredProcedureDomain();
                var carrierDetails = await domain.GetCarriersInfoForInventoryDataExport(supplierCompanyId);
                if (carrierDetails != null && carrierDetails.Any())
                {
                    foreach (var carrier in carrierDetails)
                    {
                        isProcessed = await TriggerInventoryDataExportEmailForCarrier(carrier);
                        if (isProcessed)
                        {

                            var query = $"UPDATE MstAppSettings SET Value = '{DateTimeOffset.Now.ToString()}' WHERE [Key] LIKE '%LastInventoryExportEmailDateTime%'";

                            Context.DataContext.Database.ExecuteSqlCommand(query);
                            Context.Commit();

                            var debugMessage = "Carrier inventory export email sent succesfully for carrierCompanyId"
                                               + carrier.CarrierCompanyId.ToString() +
                                               "and suppliercompanyId" + supplierCompanyId.ToString()
                                               + "on " + DateTimeOffset.Now.ToString();
                            LogManager.Logger.WriteDebug("LFVDomain", "ProcessInventoryDataExportPerCompany", debugMessage);
                        }
                        else
                        {
                            var debugMessage = "Falied to send Carrier inventory export for carrierCompanyId"
                                               + carrier.CarrierCompanyId.ToString() +
                                               "and suppliercompanyId" + supplierCompanyId.ToString()
                                               + "on " + DateTimeOffset.Now.ToString();
                            LogManager.Logger.WriteDebug("LFVDomain", "ProcessInventoryDataExportPerCompany", debugMessage);
                        }

                    }

                }

            }
            catch (Exception ex)
            {

                LogManager.Logger.WriteException("CarrierDomain", "ProcessInventoryDataExportPerCompany", ex.Message, ex);
            }
            return isProcessed;
        }

        private async Task<bool> TriggerInventoryDataExportEmailForCarrier(CarrierInfoForUnAthorizedInventoryDataViewModel carrierDetails)
        {
            var isSent = false;
            try
            {
                var subject = Resource.emailCarrierInventory_SubjectText;
                HelperDomain helperDomain = new HelperDomain();
                var serverBaseUrl = helperDomain.GetServerUrl();
                var encrptedUrl = GetEncryptedURLForUnauthorisedInventoryDataExport(carrierDetails.SupplierCompanyId, carrierDetails.CarrierCompanyId, serverBaseUrl);

                var body = string.Format(Resource.emailCarrierInventory_BodyText, encrptedUrl);
                var mailingList = new List<string>() { carrierDetails.CarrierAdminEmail };
                if (mailingList.Any() && !string.IsNullOrWhiteSpace(carrierDetails.CarrierAdminEmail))
                {
                    var companyLogo = helperDomain.GetAbsoluteServerUrl(serverBaseUrl, Resource.email_HeaderLogo);
                    var _emailTemplate = helperDomain.GetApplicationEventNotificationTemplate();
                    var emailModel = new ApplicationEventNotificationViewModel
                    {
                        To = mailingList,
                        Subject = subject,
                        CompanyLogo = companyLogo,
                        BodyText = body,
                        ShowFooterContent = false,
                        ShowHelpLineInfo = true

                    };
                    isSent = Email.GetClient().Send(_emailTemplate, emailModel);
                }
            }
            catch (Exception ex)
            {

                LogManager.Logger.WriteException("CarrierDomain", "TriggerInventoryDataExportForCarrier", ex.Message, ex);

            }
            return isSent;
        }

        private string GetEncryptedURLForUnauthorisedInventoryDataExport(int supplierCompanyId, int carrierCompanyId, string baseUrl)
        {
            var encryptedUrl = string.Empty;
            try
            {

                var token = carrierCompanyId.ToHexWithFixedNumber(); //encryted carriercompanyId
                var apiUrl = baseUrl + ApplicationConstants.UrlCarrierInventoryExport;
                encryptedUrl = string.Format(apiUrl, token, supplierCompanyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CarrierDomain", "GetEncryptedURLForUnauthorisedInventoryDataExport", ex.Message, ex);

            }
            return encryptedUrl;
        }
        public async Task<StatusViewModel> ResetDriverDeliveryRequest(ResetDeliveryGroupScheduleModel resetDeliveryGroup, UserContext userContext)
        {
            StatusViewModel status = new StatusViewModel();
            var model = await new ScheduleBuilderDomain(this).RemoveDeliverySchedule(resetDeliveryGroup);

            if (model.StatusCode != Status.Failed)
            {
                //update schedules
                if (model.DeliveryGroupIds.Any())
                {
                    var _delGroupResponse = await new ScheduleBuilderDomain(this).ResetDeliveryGroup(model.DeliveryScheduleIds, userContext);
                    if (_delGroupResponse.StatusCode != Status.Success)
                    {
                        status.StatusCode = Status.Failed;
                        status.StatusMessage = "Unable to update delivery group details.";
                        return status;
                    }
                }
                if (model.DeliveryScheduleIds.Any())
                {
                    var _delScheduleResponse = await new ScheduleBuilderDomain(this).ResetDeliverySchedules(model.DeliveryScheduleIds, userContext);
                    if (_delScheduleResponse.StatusCode != Status.Success)
                    {
                        status.StatusCode = Status.Failed;
                        status.StatusMessage = "Unable to update delivery schedule details.";
                        return status;
                    }
                }
                status.StatusCode = Status.Success;
                status.StatusMessage = Resource.valSuccessMessageResetLoad;
            }
            else
            {
                status.StatusCode = Status.Failed;
                status.StatusMessage = "Unable to update schedule builder details.";
            }
            return status;
        }
        private string GenerateUniqueDRId(DeliveryRequestViewModel requests, UserContext userContext, DeliveryRequestBrokerInfoViewModel brokerResponseViewModel)
        {
            string[] companyWordsDetails = userContext.CompanyName.Split(' ');
            var supplierName = GetCompanyWordInfo(userContext.CompanyName, companyWordsDetails);
            var companyDetails = Context.DataContext.Companies.Where(x => x.Id == brokerResponseViewModel.CarrierCompanyId).FirstOrDefault()?.Name;
            string[] customercompanyWordsDetails = companyDetails.Split(' ');
            var customerName = GetCompanyWordInfo(companyDetails, customercompanyWordsDetails);
            string productCode = "";
            if (!requests.IsBlendedRequest)
            {
                var productCodeInfo = Context.DataContext.MstProductTypes.Where(x => x.Id == requests.ProductTypeId).FirstOrDefault();
                productCode = productCodeInfo?.ProductCode;
            }
            else
            {
                productCode = "BL";
            }
            var dateFormat = DateTime.Now.ToString("MMddyy");
            var uniqueDRID = supplierName + customerName + productCode + dateFormat + GetUniqueKey();
            return uniqueDRID;
        }
        private void GenerateUniqueDRId(RaiseDeliveryRequestViewModel requests, UserContext userContext, DeliveryRequestBrokerInfoViewModel brokerResponseViewModel, IEnumerable<dynamic> carrierCompanyInfos, IEnumerable<dynamic> productTypeDetails)
        {
            string[] companyWordsDetails = userContext.CompanyName.Split(' ');
            var supplierName = GetCompanyWordInfo(userContext.CompanyName, companyWordsDetails);
            string[] customercompanyWordsDetails = carrierCompanyInfos.FirstOrDefault(x1 => x1.Id == requests.AssignedToCompanyId).Name.Split(' ');
            var customerName = GetCompanyWordInfo(carrierCompanyInfos.FirstOrDefault(x1 => x1.Id == requests.AssignedToCompanyId).Name, customercompanyWordsDetails);
            var productCodeInfo = productTypeDetails.FirstOrDefault(x => x.Id == requests.ProductTypeId);
            string productCode = "";
            if (productCodeInfo != null)
            {
                productCode = productCodeInfo.ProductCode;
            }
            var dateFormat = DateTime.Now.ToString("MMddyy");
            var uniqueDRID = supplierName + customerName + productCode + dateFormat + GetUniqueKey();
            requests.UniqueOrderNo = uniqueDRID;
        }

    }

}

