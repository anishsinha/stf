using System;
using System.Threading.Tasks;
using System.Web.Http;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.ViewModels;
using System.IO;
using Newtonsoft.Json;
using SiteFuel.Exchange.Api.Mobile.Common;
using System.Web.Http.Description;
using System.Linq;
using SiteFuel.Exchange.Api.Mobile.Attributes;
using System.Web.Hosting;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using System.Collections.Generic;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.Api.Mobile.Controllers
{
#if DEBUG
    [ApiExplorerSettings(IgnoreApi = false)]
#else
    [ApiExplorerSettings(IgnoreApi = true)]
#endif

    [ValidateToken]
    public class NotificationController : ApiBaseController
    {
        [HttpPost]
        //This function is OnMyWayStart
        public async Task<StatusViewModel> PushNotificationToBuyer(NotificationToBuyerViewModel viewModel)
        {
            using (var tracer = new Tracer("NotificationController", "PushNotificationToBuyer"))
            {
                StatusViewModel response = new StatusViewModel();
                try
                {
                    var messageViewModel = ToMessageViewModel(viewModel);
                    messageViewModel.NotificationCode = (int)NotificationCode.Start;
                    await ContextFactory.Current.GetDomain<PushNotificationDomain>().NotificationToBuyer(messageViewModel, viewModel.OrderId, viewModel.DeliveryScheduleId);

                    var orderDomain = ContextFactory.Current.GetDomain<OrderDomain>();
                    var appLocationViewModel = ToAppLocationViewModel(viewModel);
                    await orderDomain.SaveAppLocation(appLocationViewModel);
                    
                    response = await orderDomain.AssignDriverToOrder(viewModel);

                    if (viewModel.UncanceledDeliveryScheduleId.Any())
                    {
                        await orderDomain.DriverUncanceledSchedule(viewModel);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("NotificationController", "PushNotificationToBuyer", ex.Message, ex);
                }
                return response;
            }
        }

        private MessageViewModel ToMessageViewModel(NotificationToBuyerViewModel viewModel)
        {
            var messageViewModel = new MessageViewModel();
            messageViewModel.Body = viewModel.Body;
            messageViewModel.Sound = viewModel.Sound;
            messageViewModel.Title = viewModel.Title;
            messageViewModel.Notify = viewModel.Notify;
            return messageViewModel;
        }

        private AppLocationViewModel ToAppLocationViewModel(NotificationToBuyerViewModel viewModel)
        {
            var appLocationViewModel = new AppLocationViewModel();
            appLocationViewModel.UserId = viewModel.DriverId;
            appLocationViewModel.AppType = viewModel.AppType;
            appLocationViewModel.FCMAppId = viewModel.FCMAppId;
            appLocationViewModel.Latitude = viewModel.Latitude;
            appLocationViewModel.Longitude = viewModel.Longitude;
            appLocationViewModel.UpdatedDate = DateTime.Now;
            appLocationViewModel.OrderId = viewModel.OrderId;
            appLocationViewModel.DeliveryScheduleId = viewModel.DeliveryScheduleId;
            appLocationViewModel.TrackableScheduleId = viewModel.TrackableScheduleId;
            return appLocationViewModel;
        }

        [HttpPost]
        public async Task<StatusViewModel> OnMyWayArrival(NotificationToBuyerOnArrivalViewModel viewModel)
        {
            using (var tracer = new Tracer("NotificationController", "OnMyWayArrival"))
            {
                StatusViewModel response = new StatusViewModel();
                try
                {
                    viewModel.Message.NotificationCode = (int)NotificationCode.DriverArrived;
                    await ContextFactory.Current.GetDomain<PushNotificationDomain>().NotificationToBuyer(viewModel.Message, viewModel.OrderId);

                    response = await ContextFactory.Current.GetDomain<OrderDomain>().SaveAppLocation(viewModel.AppLocation);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("NotificationController", "OnMyWayArrival", ex.Message, ex);
                }
                return response;
            }
        }

        public async Task<StatusViewModel> SendNotificationToBuyer(NotificationToBuyerViewModel viewModel)
        {
            using (var tracer = new Tracer("NotificationController", "SendNotificationToBuyer"))
            {
                StatusViewModel response = new StatusViewModel();
                try
                {
                    var messageViewModel = ToMessageViewModel(viewModel);
                    await ContextFactory.Current.GetDomain<PushNotificationDomain>().NotificationToBuyer(messageViewModel, viewModel.OrderId, viewModel.DeliveryScheduleId);

                    var appLocationViewModel = ToAppLocationViewModel(viewModel);
                    await ContextFactory.Current.GetDomain<OrderDomain>().SaveAppLocation(appLocationViewModel);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("NotificationController", "SendNotificationToBuyer", ex.Message, ex);
                }
                return response;
            }
        }
    }
}
