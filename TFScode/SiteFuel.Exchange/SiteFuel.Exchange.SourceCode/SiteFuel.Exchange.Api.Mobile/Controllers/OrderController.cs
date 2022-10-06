using Easy.Common;
using Easy.Common.Interfaces;
using Newtonsoft.Json;
using SiteFuel.Exchange.Api.Mobile.Attributes;
using SiteFuel.Exchange.Api.Mobile.Common;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Common;
using SiteFuel.Exchange.ViewModels.Dispatcher;
using SiteFuel.Exchange.ViewModels.FilldService;
using SiteFuel.Exchange.ViewModels.MobileAPI;
using SiteFuel.Exchange.ViewModels.MobileAPI.TPD;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace SiteFuel.Exchange.Api.Mobile.Controllers
{
#if DEBUG
    [ApiExplorerSettings(IgnoreApi = false)]
#else
    [ApiExplorerSettings(IgnoreApi = true)]
#endif

    [ValidateToken]
    public class OrderController : ApiBaseController
    {

        #region Old Style

        [HttpGet]
        [ApiLog(Enabled = true)]
        public HttpResponseMessage GetOrders(decimal latitude, decimal longitude, int companyId, int userId = 0, long scheduleDate = 0, int offset = -400)
        {
            using (var tracer = new Tracer("OrderController", "GetOrders"))
            {
                BaseResponse returnObj = new BaseResponse
                {
                    Message = ValidateGetOrdersInput(latitude, longitude, companyId)
                };
                try
                {
                    if (string.IsNullOrWhiteSpace(returnObj.Message))
                    {
                        if (scheduleDate == 0)
                        {
                            scheduleDate = DateTimeOffset.Now.ToUnixTimeMilliseconds(); //if scheduleDate is passed 0 , consider today's date
                        }

                        scheduleDate = DateTimeOffset.FromUnixTimeMilliseconds(scheduleDate).AddMinutes(offset).ToUnixTimeMilliseconds();
                        var distance = ApplicationDomain.ApplicationSettings.GetApplicationSettingValue(ApplicationConstants.KeyAppSettingDriverAppGetOrdersProximity, 500);
                        var orders = Task.Run(() => GetOrdersNew(latitude, longitude, companyId, userId, distance, scheduleDate)).Result;
                        if (orders.Count > 0)
                        {
                            returnObj.Status = Constants.OKStatus;
                            returnObj.Message = Constants.Success;
                        }
                        else
                        {
                            returnObj.Status = Constants.OKStatus;
                            returnObj.Message = Constants.NoJobsFound;
                        }

                        return Request.CreateResponse(HttpStatusCode.OK, new { returnObj, data = orders });
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "GetOrders", ex.Message, ex);
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest, returnObj);
            }
        }

        [HttpGet]
        [ApiLog(Enabled = true)]
        public HttpResponseMessage GetOrdersForCompany(decimal latitude, decimal longitude, int companyId, int buyerCompanyId, int userId = 0, long scheduleDate = 0, int distance = 25)
        {
            using (var tracer = new Tracer("OrderController", "GetOrdersForCompany"))
            {
                BaseResponse returnObj = new BaseResponse
                {
                    Message = ValidateGetOrdersInput(latitude, longitude, companyId)
                };
                try
                {
                    if (string.IsNullOrWhiteSpace(returnObj.Message))
                    {
                        if (scheduleDate == 0)
                        {
                            scheduleDate = DateTimeOffset.Now.ToUnixTimeMilliseconds(); //if scheduleDate is passed 0 , consider today's date
                        }

                        var orders = Task.Run(() => GetOrdersNew(latitude, longitude, companyId, userId, distance, scheduleDate, buyerCompanyId)).Result;
                        if (orders.Count > 0)
                        {
                            returnObj.Status = Constants.OKStatus;
                            returnObj.Message = Constants.Success;
                        }
                        else
                        {
                            returnObj.Status = Constants.OKStatus;
                            returnObj.Message = Constants.NoJobsFound;
                        }

                        return Request.CreateResponse(HttpStatusCode.OK, new { returnObj, data = orders });
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "GetOrdersForCompany", ex.Message, ex);
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest, returnObj);
            }
        }

        [HttpGet]
        [ApiLog(Enabled = true)]
        public HttpResponseMessage GetAssetLevelOrderDetails(int orderId, int companyId, int driverId = 0)
        {
            using (var tracer = new Tracer("OrderController", "GetAssetLevelOrderDetails"))
            {
                BaseResponse returnObj = new BaseResponse
                {
                    Message = (orderId == 0) ? Constants.InvalidOrder : string.Empty
                };
                try
                {
                    if (string.IsNullOrWhiteSpace(returnObj.Message))
                    {
                        List<int> orderIds = new List<int>();
                        orderIds.Add(orderId);
                        var orderAssetList = Task.Run(() => GetOrderAssets(orderIds, driverId)).Result;
                        var orderAssets = orderAssetList.Select(t => t.ToAssetDropDetails()).ToList();
                        returnObj.Status = Constants.OKStatus;
                        returnObj.Message = Constants.Success;

                        return Request.CreateResponse(HttpStatusCode.OK, new { returnObj, data = orderAssets });
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "GetAssetLevelOrderDetails", ex.Message, ex);
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest, returnObj);
            }
        }

        [HttpPost]
        [ApiLog(Enabled = true)]
        public HttpResponseMessage GetAssetLevelOrderDetails(AssetLevelDropRequestViewModel viewModel)
        {
            using (var tracer = new Tracer("OrderController", "GetAssetLevelOrderDetails"))
            {
                BaseResponse returnObj = new BaseResponse();
                try
                {
                    List<DriverOrderAssetViewModel> orderAssetList = new List<DriverOrderAssetViewModel>();
                    var response = Task.Run(() => GetOrderAssets(viewModel.OrderId, viewModel.DriverId)).Result;
                    orderAssetList.AddRange(response);

                    var orderAssets = orderAssetList.Select(t => t.ToAssetDropDetails()).ToList();
                    returnObj.Status = Constants.OKStatus;
                    returnObj.Message = Constants.Success;

                    return Request.CreateResponse(HttpStatusCode.OK, new { returnObj, data = orderAssets });
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "GetAssetLevelOrderDetails", ex.Message, ex);
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest, returnObj);
            }
        }

        [HttpPost]
        [ApiLog(Enabled = true)]
        public HttpResponseMessage addNewAsset([FromBody] NewAssetDetails newAsset)
        {
            using (var tracer = new Tracer("OrderController", "addNewAsset"))
            {
                BaseResponse returnObj = new BaseResponse
                {
                    Message = ValidateAddNewAssetInput(newAsset)
                };
                try
                {
                    if (string.IsNullOrWhiteSpace(returnObj.Message))
                    {
                        var viewModel = new AssetViewModel
                        {
                            UserId = newAsset.UserID,
                            Name = newAsset.Name,
                            CreatedDate = DateTimeOffset.FromUnixTimeMilliseconds(newAsset.AssetAddedDateTime),
                            FuelType = (newAsset.FuelType == null || newAsset.FuelType == 0) ? null : new AssetFuelTypeViewModel { Id = newAsset.FuelType },
                            Type = (int)AssetType.Asset,
                            AssetAdditionalDetail = new AssetAdditionalDetailViewModel
                            {
                                Make = newAsset.Make,
                                Model = newAsset.Model,
                                Year = newAsset.Year,
                                Color = newAsset.Color,
                                TelematicsProvider = newAsset.TelematicsProvider,
                                LicensePlateState = newAsset.LicensePlateState,
                                LicensePlate = newAsset.LicensePlate,
                                Class = newAsset.CatClass,
                                Vendor = newAsset.Vendor,
                                VehicleId = newAsset.VehicleId,
                                Description = newAsset.AdditionalDetails,
                                FuelCapacity = newAsset.FuelCapacity
                            }
                        };

                        if (!string.IsNullOrWhiteSpace(newAsset.Image))
                        {
                            viewModel.Image = new ImageViewModel() { Data = Convert.FromBase64String(newAsset.Image) };
                        }

                        var result = Task.Run(() => AddNewAsset(viewModel, newAsset.JobDetailId)).Result;
                        if (result != null)
                        {
                            if (result.StatusCode == Utilities.Status.Success)
                            {
                                var insertResponse = new InsertResponseModel
                                {
                                    Status = Constants.OKStatus,
                                    Message = Constants.Success,
                                    NewInsertedId = result.AssetId,
                                    AssetImageId = result.ImageId,
                                    JobXAssignmentId = result.JobXAssetId
                                };

                                returnObj.Status = insertResponse.Status;
                                returnObj.Message = insertResponse.Message;

                                return Request.CreateResponse(HttpStatusCode.OK, new { returnObj, data = insertResponse });
                            }
                            else
                            {
                                returnObj.Status = Constants.FailStatus;
                                returnObj.Message = result.StatusMessage;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "AddNewAsset", ex.Message, ex);
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest, returnObj);
            }
        }

        [HttpPost]
        [ApiLog(Enabled = true)]
        public HttpResponseMessage addSpillFuelDetails([FromBody] SpillFuelDetails newSpill)
        {
            using (var tracer = new Tracer("OrderController", "addSpillFuelDetails"))
            {
                BaseResponse returnObj = new BaseResponse
                {
                    Message = CheckIsValidNewSpillFuelRequest(newSpill)
                };
                try
                {
                    if (string.IsNullOrWhiteSpace(returnObj.Message))
                    {
                        var viewModel = newSpill.ToDriverFuelSpillViewModel();
                        var response = Task.Run(() => AddFuelSpill(viewModel)).Result;
                        if (response.StatusCode == Utilities.Status.Success)
                        {
                            returnObj.Status = Constants.OKStatus;
                            returnObj.Message = Constants.Success;
                            var data = new { SpillId = viewModel.Id, imageListId = viewModel.SpillImages.Select(t => t.Id).ToList() };

                            return Request.CreateResponse(HttpStatusCode.OK, new { returnObj, data });
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "addSpillFuelDetails", ex.Message, ex);
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest, returnObj);
            }
        }

        [HttpGet]
        public HttpResponseMessage getSpillFuelDetails(int assetId, int spillId)
        {
            using (var tracer = new Tracer("OrderController", "getSpillFuelDetails"))
            {
                BaseResponse returnObj = new BaseResponse
                {
                    Message = CheckIsValidGetSpillFuelRequest(assetId, spillId)
                };
                try
                {
                    if (string.IsNullOrWhiteSpace(returnObj.Message))
                    {
                        var response = Task.Run(() => GetFuelSpill(assetId, spillId)).Result;
                        if (response.StatusCode == Utilities.Status.Success)
                        {
                            returnObj.Status = Constants.OKStatus;
                            returnObj.Message = Constants.Success;
                            var spillDetails = response.ToSpillFuelDetailsViewModel();

                            return Request.CreateResponse(HttpStatusCode.OK, new { returnObj, data = spillDetails });
                        }
                        else
                        {
                            returnObj.Message = Constants.NoSpillFound;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "getSpillFuelDetails", ex.Message, ex);
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest, returnObj);
            }
        }

        [HttpPost]
        [ApiLog(Enabled = true)]
        public HttpResponseMessage deleteSpillFuel([FromBody] DeleteSpillDetails deleteSpill)
        {
            using (var tracer = new Tracer("OrderController", "deleteSpillFuel"))
            {
                BaseResponse returnObj = new BaseResponse
                {
                    Message = CheckIsValidDeleteSpillRequest(deleteSpill.deleteSpillId)
                };
                try
                {
                    if (string.IsNullOrWhiteSpace(returnObj.Message))
                    {
                        var response = Task.Run(() => DeleteFuelSpill(deleteSpill.deleteSpillId)).Result;
                        if (response.StatusCode == Utilities.Status.Success)
                        {
                            returnObj.Status = Constants.OKStatus;
                            returnObj.Message = Constants.Success;

                            return Request.CreateResponse(HttpStatusCode.OK, new { returnObj });
                        }
                        else
                        {
                            returnObj.Message = Constants.NoSpillFound;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "deleteSpillFuel", ex.Message, ex);
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest, returnObj);
            }
        }

        [HttpPost]
        [ApiLog(Enabled = true)]
        public async Task<StatusViewModel> UpdateAssetDropQuantity(UpdateAssetDropQuantityViewModel viewModel)
        {
            var response = new StatusViewModel();
            try
            {
                if (ModelState.IsValid)
                {
                    response = await ContextFactory.Current.GetDomain<OrderDomain>().UpdateAssetDropQuantity(viewModel);
                }
                else
                {
                    var geterror = new CommonMethods().GetErrorMessage(ModelState);
                    response.StatusCode = geterror.StatusCode;
                    response.StatusMessage = geterror.StatusMessage;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderController", "UpdateAssetDropQuantity", ex.Message, ex);
            }
            return response;
        }

        [HttpPost]
        [ApiLog(Enabled = true)]
        public async Task<StatusViewModel> DeleteAssetDrop(DeleteAssetDropViewModel viewModel)
        {
            var response = new StatusViewModel();
            try
            {
                if (ModelState.IsValid)
                {
                    response = await ContextFactory.Current.GetDomain<OrderDomain>().DeleteAssetDrop(viewModel);
                }
                else
                {
                    var geterror = new CommonMethods().GetErrorMessage(ModelState);
                    response.StatusCode = geterror.StatusCode;
                    response.StatusMessage = geterror.StatusMessage;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderController", "DeleteAssetDrop", ex.Message, ex);
            }
            return response;
        }

        [HttpPost]
        [ApiLog(Enabled = true)]
        public async Task<StatusViewModel> SetAssetDropStatus(AssetDropStatusViewModel viewModel)
        {
            var response = new StatusViewModel();
            try
            {
                if (ModelState.IsValid)
                {
                    response = await ContextFactory.Current.GetDomain<OrderDomain>().SetAssetDropStatus(viewModel);
                }
                else
                {
                    var geterror = new CommonMethods().GetErrorMessage(ModelState);
                    response.StatusCode = geterror.StatusCode;
                    response.StatusMessage = geterror.StatusMessage;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderController", "SetAssetDropStatus", ex.Message, ex);
            }
            return response;
        }

        [HttpPost]
        [ApiLog(Enabled = true)]
        public HttpResponseMessage deleteSpillImage([FromBody] DeleteSpillImage deleteImage)
        {
            using (var tracer = new Tracer("OrderController", "deleteSpillImage"))
            {
                BaseResponse returnObj = new BaseResponse
                {
                    Message = CheckIsValidDeleteImageRequest(deleteImage.imageId)
                };
                try
                {
                    var response = Task.Run(() => DeleteFuelSpillImage(deleteImage.spillId, deleteImage.imageId)).Result;
                    if (response.StatusCode == Status.Success)
                    {
                        returnObj.Status = Constants.OKStatus;
                        returnObj.Message = Constants.Success;

                        return Request.CreateResponse(HttpStatusCode.OK, new { returnObj });
                    }
                    else
                    {
                        //returnObj.Message = Constants.NoImageFound; [KS::SHOULD HAVE NoImageFound IN RESPONSE]
                        returnObj.Message = Constants.RequestError;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "deleteSpillImage", ex.Message, ex);
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest, returnObj);
            }
        }

        [HttpGet]
        public HttpResponseMessage getImage(int imageId)
        {
            using (var tracer = new Tracer("OrderController", "getImage"))
            {
                BaseResponse returnObj = new BaseResponse
                {
                    Message = imageId > 0 ? string.Empty : Constants.DataMissing
                };
                try
                {
                    if (string.IsNullOrWhiteSpace(returnObj.Message))
                    {
                        var result = Task.Run(() => GetImageNew(imageId)).Result;
                        if (result.StatusCode == Status.Success)
                        {
                            MemoryStream memoryStream = new MemoryStream(result.Data);
                            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                            response.Content = new StreamContent(memoryStream);
                            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");

                            return response;
                        }
                        else
                        {
                            returnObj.Message = Constants.NoImageFound;

                            return Request.CreateResponse(HttpStatusCode.OK, returnObj);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "getImage", ex.Message, ex);
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest, returnObj);
            }
        }

        [HttpGet]
        [ApiLog(Enabled = true)]
        public HttpResponseMessage getStates()
        {
            using (var tracer = new Tracer("OrderController", "getStates"))
            {
                BaseResponse returnObj = new BaseResponse
                {
                    Message = Constants.NoStateFound
                };
                try
                {
                    var response = Task.Run(() => GetStatesNew()).Result;
                    if (response != null && response.Count > 0)
                    {
                        var statelist = new List<StateDetails>();
                        response.ForEach(t => statelist.Add(new StateDetails { StateId = t.Id, StateCode = t.Code, StateName = t.Name }));
                        returnObj.Status = Constants.OKStatus;
                        returnObj.Message = Constants.Success;

                        return Request.CreateResponse(HttpStatusCode.OK, new { returnObj, data = statelist });
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "getStates", ex.Message, ex);
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest, returnObj);
            }
        }

        [HttpGet]
        [ApiLog(Enabled = true)]
        public HttpResponseMessage getAssetFuelTypes()
        {
            using (var tracer = new Tracer("OrderController", "getAssetFuelTypes"))
            {
                BaseResponse returnObj = new BaseResponse
                {
                    Message = Constants.NoFuelTypeFound
                };
                try
                {
                    var response = Task.Run(() => GetAssetFuelTypesNew()).Result;
                    if (response != null && response.Count > 0)
                    {
                        var fuelList = new List<AssetFuelTypeDetails>();
                        response.ForEach(t => fuelList.Add(new AssetFuelTypeDetails { FuelTypeId = t.Id, FuelTypeName = t.Name }));
                        returnObj.Status = Constants.OKStatus;
                        returnObj.Message = Constants.Success;

                        return Request.CreateResponse(HttpStatusCode.OK, new { returnObj, data = fuelList });
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "getAssetFuelTypes", ex.Message, ex);
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest, returnObj);
            }
        }

        [HttpGet]
        [ApiLog(Enabled = true)]
        public HttpResponseMessage getAssetDropHistoryDetails(int jobId, int assetId, int companyId, int skipCount, int limit)
        {
            using (var tracer = new Tracer("OrderController", "getAssetDropHistoryDetails"))
            {
                BaseResponse returnObj = new BaseResponse
                {
                    Message = CheckIsValidAssetDropHistoryRequest(jobId, assetId, companyId)
                };
                try
                {
                    var response = Task.Run(() => GetAssetDropHistory(jobId, assetId, companyId, skipCount, limit)).Result;
                    if (response.StatusCode == Status.Success)
                    {
                        var dropHistory = response.ToDriverAssetHistory();
                        returnObj.Status = Constants.OKStatus;
                        returnObj.Message = Constants.Success;
                        return Request.CreateResponse(HttpStatusCode.OK, new { returnObj, data = dropHistory });
                    }
                    else
                    {
                        returnObj.Message = Constants.NoAssetFound;
                        return Request.CreateResponse(HttpStatusCode.OK, returnObj);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "getAssetDropHistoryDetails", ex.Message, ex);
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest, returnObj);
            }
        }

        [HttpGet]
        [ApiLog(Enabled = true)]
        public HttpResponseMessage getFilledAssetDetails(int assetId, int orderId)
        {
            using (var tracer = new Tracer("OrderController", "getFilledAssetDetails"))
            {
                BaseResponse returnObj = new BaseResponse
                {
                    Message = CheckIsValidAssetDropsRequest(assetId, orderId)
                };
                try
                {
                    if (string.IsNullOrWhiteSpace(returnObj.Message))
                    {
                        var response = Task.Run(() => GetFilledAsset(assetId, orderId)).Result;
                        if (response.StatusCode == Status.Success)
                        {
                            var filledAsset = response.ToFilledAssetDetails();
                            returnObj.Status = Constants.OKStatus;
                            returnObj.Message = Constants.Success;

                            return Request.CreateResponse(HttpStatusCode.OK, new { returnObj, data = filledAsset });
                        }
                        else
                        {
                            returnObj.Message = Constants.NoAssetFound;

                            return Request.CreateResponse(HttpStatusCode.OK, returnObj);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "getFilledAssetDetails", ex.Message, ex);
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest, returnObj);
            }
        }

        [HttpPost]
        [ApiLog(Enabled = true)]
        public HttpResponseMessage createAssetLevelDrop([FromBody] AssetDropRequestModel newAssetDropRequest)
        {
            using (var tracer = new Tracer("OrderController", "createAssetLevelDrop"))
            {
                BaseResponse returnObj = new BaseResponse
                {
                    Message = CheckIsValidAssetDropRequest(newAssetDropRequest)
                };
                try
                {
                    if (string.IsNullOrWhiteSpace(returnObj.Message))
                    {
                        bool isBlend = false;
                        bool isSaveSuccess = false;
                        int assetDropId = 0;
                        List<AssetDropResponseViewModel> assetDropResponseData = new List<AssetDropResponseViewModel>();
                        if (newAssetDropRequest.data.AssetDropDetail != null && newAssetDropRequest.data.AssetDropDetail.Count > 0)
                        {
                            isBlend = true;
                        }

                        var viewModel = newAssetDropRequest.ToDriverAssetFuelDropViewModel();
                        foreach (var item in viewModel)
                        {
                            if (ValidateSaveAssetDropInput(item, isBlend))
                            {
                                var response = Task.Run(() => SaveAssetDrop(item)).Result;
                                if (response.StatusCode == Status.Success)
                                {
                                    returnObj.Message = Constants.Success;
                                    returnObj.Status = Constants.OKStatus;
                                    isSaveSuccess = true;

                                    if (isBlend)
                                        assetDropResponseData.Add(new AssetDropResponseViewModel
                                        {
                                            AssetDropId = item.FuelDrop.AssetDropId,
                                            OrderId = item.FuelDrop.OrderId,
                                            Quantity = item.FuelDrop.PrimaryDrop,
                                            FuelType = item.FuelDrop.FuelType,
                                            ProductType = item.FuelDrop.ProductType,
                                            JobXAssetId = item.FuelDrop.JobXAssetId
                                        });
                                    else
                                        assetDropId = item.FuelDrop.AssetDropId;
                                }
                            }
                            else
                            {
                                returnObj.Message = Constants.MeterReadingError;
                            }
                        }

                        if (isSaveSuccess)
                        {
                            if (isBlend)
                            {
                                var data = new { assetDropResponseData };
                                return Request.CreateResponse(HttpStatusCode.OK, new { returnObj, data });
                            }
                            else
                            {
                                var data = new { assetDropId = assetDropId };
                                return Request.CreateResponse(HttpStatusCode.OK, new { returnObj, data });
                            }
                        }
                        else
                        {
                            returnObj.Message = Constants.RequestError;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "createAssetLevelDrop", ex.Message, ex);
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest, returnObj);
            }
        }

        [HttpPost]
        [ApiLog(Enabled = true)]
        public HttpResponseMessage createNewOrder([FromBody] NewOrderRequestModel newOrder)
        {
            using (var tracer = new Tracer("OrderController", "createNewOrder"))
            {
                BaseResponse returnObj = new BaseResponse
                {
                    Message = CheckIsValidCreateRequest(newOrder)
                };
                try
                {
                    if (string.IsNullOrWhiteSpace(returnObj.Message))
                    {
                        var viewModel = newOrder.ToDriverDropOrderViewModel();
                        var response = Task.Run(() => CreateUnassignedInvoice(viewModel)).Result;
                        if (response.StatusCode == Status.Success)
                        {
                            returnObj.Message = Constants.Success;
                            returnObj.Status = Constants.OKStatus;

                            return Request.CreateResponse(HttpStatusCode.OK, returnObj);
                        }
                        else
                        {
                            returnObj.Message = Constants.RequestError;
                            return Request.CreateResponse(HttpStatusCode.OK, returnObj);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "createNewOrder", ex.Message, ex);
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest, returnObj);
            }
        }

        private async Task<bool> GetExternalResponse(int invoiceId)
        {
            var serverUrl = ContextFactory.Current.GetDomain<HelperDomain>().GetServerUrl();
            var url = $"{serverUrl}/InvoiceBase/SendResaleCustomerNotification?InvoiceId={invoiceId}";
            try
            {
                using (IRestClient client = new RestClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderController", "GetExternalResponse", ex.Message, ex);
            }
            return false;
        }

        [HttpPost]
        [ApiLog(Enabled = true)]
        public async Task<HttpResponseMessage> dropOrder([FromBody] DropRequestModel dropRequest)
        {
            using (var tracer = new Tracer("OrderController", "dropOrder"))
            {
                BaseResponse returnObj = new BaseResponse
                {
                    Message = CheckIsValidDropRequest(dropRequest)
                };
                try
                {
                    if (string.IsNullOrWhiteSpace(returnObj.Message))
                    {
                        var viewModel = dropRequest.ToDriverDropOrderViewModel();
                        var response = await CreateMobileInvoice(viewModel);
                        if (response.StatusCode == Status.Success)
                        {
                            await GetExternalResponse(viewModel.InvoiceId);
                            returnObj.Status = Constants.OKStatus;
                            returnObj.Message = Constants.Success;

                            await ContextFactory.Current.GetDomain<PushNotificationDomain>().NotificationToApprover_DropCompleted(viewModel.OrderId);
                            await ContextFactory.Current.GetDomain<NotificationDomain>().AddNotificationEventAsync(EventType.DeliveryCompleted, response.EntityId, viewModel.Driver.UserId);

                            return Request.CreateResponse(HttpStatusCode.OK, returnObj);
                        }
                        else
                        {
                            returnObj.Message = Constants.RequestError;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "dropOrder", ex.Message, ex);
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest, returnObj);
            }
        }


        [HttpPost]
        [ApiLog(Enabled = true)]
        public async Task<HttpResponseMessage> FtlDropOrder([FromBody] FtlDropRequestModel dropRequest)
        {
            using (var tracer = new Tracer("OrderController", "FtlDropOrder"))
            {
                FtlDropResponseModel returnObj = new FtlDropResponseModel
                {
                    Message = CheckIsValidDropRequest(dropRequest)
                };
                try
                {
                    var response = new StatusViewModel();
                    if (string.IsNullOrWhiteSpace(returnObj.Message))
                    {
                        var viewModel = dropRequest.ToFtlDriverDropOrderViewModel();

                        if (viewModel.IsSplitLoad || !string.IsNullOrEmpty(viewModel.SplitLoadChainId))
                        {
                            // need to hit split draft ddt
                            returnObj = await ContextFactory.Current.GetDomain<InvoiceCreateDomain>().CreateMobileSplitLoadDraftDdtAsync(viewModel);
                            response.StatusCode = (Status)returnObj.Status;
                        }
                        else
                        {
                            response = await CreateFtlMobileInvoice(viewModel);
                        }

                        if (response.StatusCode == Status.Success)
                        {
                            if (!viewModel.IsSplitLoad)
                            {
                                await GetExternalResponse(viewModel.InvoiceId);
                                await ContextFactory.Current.GetDomain<PushNotificationDomain>().NotificationToApprover_DropCompleted(viewModel.OrderId);
                                await ContextFactory.Current.GetDomain<NotificationDomain>().AddNotificationEventAsync(EventType.DeliveryCompleted, response.EntityId, viewModel.Driver.UserId);
                            }
                            returnObj.Status = Constants.OKStatus;
                            returnObj.Message = Constants.Success;

                            return Request.CreateResponse(HttpStatusCode.OK, returnObj);
                        }
                        else
                        {
                            returnObj.Message = Constants.RequestError;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "FtlDropOrder", ex.Message, ex);
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest, returnObj);
            }
        }
        #endregion

        #region New Style

        [HttpGet]
        [NonAction]
        public async Task<List<OrderDetails>> GetOrdersNew(decimal latitude, decimal longitude, int companyId, int userId, int distance, long scheduleDate = 0, int buyerCompanyId = 0)
        {
            using (var tracer = new Tracer("OrderController", "GetOrdersNew"))
            {
                List<OrderDetails> response = new List<OrderDetails>();
                try
                {
                    if (latitude != 0 && longitude != 0 && companyId != 0 && userId != 0)
                    {
                        response = await ContextFactory.Current.GetDomain<OrderDomain>().GetDriverOrdersAsync(latitude, longitude, companyId, userId, distance, scheduleDate, buyerCompanyId);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "GetOrdersNew", ex.Message, ex);
                }
                return response;
            }
        }

        [HttpGet]
        [NonAction]
        public async Task<List<DriverOrderAssetViewModel>> GetOrderAssets(List<int> orderId, int driverId = 0)
        {
            using (var tracer = new Tracer("OrderController", "GetOrderAssets"))
            {
                List<DriverOrderAssetViewModel> response = new List<DriverOrderAssetViewModel>();
                try
                {
                    if (orderId.Count > 0)
                    {
                        response = await ContextFactory.Current.GetDomain<OrderDomain>().GetOrderAssetsAsync(orderId, driverId);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "GetOrderAssets", ex.Message, ex);
                }
                return response;
            }
        }

        [HttpPost]
        [NonAction]
        public async Task<DriverAddAssetResponseViewModel> AddNewAsset(AssetViewModel viewModel, int jobId)
        {
            using (var tracer = new Tracer("OrderController", "AddNewAsset"))
            {
                var response = new DriverAddAssetResponseViewModel();
                try
                {
                    var assetDomain = ContextFactory.Current.GetDomain<AssetDomain>();
                    var result = await assetDomain.SaveMobileAssetAsync(viewModel, jobId);
                    if (result.StatusCode == Status.Success)
                    {
                        var jobXAsset = new AssetJobAssignmentViewModel
                        {
                            JobId = jobId,
                            AssetId = viewModel.Id,
                            AssignedBy = viewModel.UserId,
                            AssignedDate = DateTimeOffset.Now
                        };
                        var userContext = await GetUserContext(viewModel.UserId);
                        result = await assetDomain.AssignToJobAsync(userContext, jobXAsset);
                        if (result.StatusCode == Status.Success)
                        {
                            response = new DriverAddAssetResponseViewModel
                            {
                                AssetId = viewModel.Id,
                                ImageId = viewModel.Image == null ? 0 : viewModel.Image.Id,
                                JobXAssetId = jobXAsset.Id,
                                StatusCode = Utilities.Status.Success,
                                StatusMessage = string.Format(Resource.errMessageAssetsAssignedSuccess, ((AssetType)viewModel.Type).GetDisplayName())
                            };
                        }
                    }
                    else
                    {
                        response.StatusMessage = result.StatusMessage;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "AddNewAsset", ex.Message, ex);
                }
                return response;
            }
        }

        [HttpPost]
        [ApiLog(Enabled = true)]
        public async Task<StatusViewModel> AddFuelSpill(DriverFuelSpillViewModel viewModel)
        {
            using (var tracer = new Tracer("OrderController", "AddFuelSpill"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().AddFuelSpillAsync(viewModel);
                return response;
            }
        }

        [HttpGet]
        [NonAction]
        public async Task<DriverFuelSpillViewModel> GetFuelSpill(int assetId, int spillId)
        {
            using (var tracer = new Tracer("OrderController", "GetFuelSpill"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().GetFuelSpillAsync(assetId, spillId);
                return response;
            }
        }

        [HttpPost]
        [NonAction]
        public async Task<StatusViewModel> DeleteFuelSpill(int spillId)
        {
            using (var tracer = new Tracer("OrderController", "DeleteFuelSpill"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().DeleteFuelSpill(spillId);
                return response;
            }
        }

        [HttpPost]
        [NonAction]
        public async Task<StatusViewModel> DeleteFuelSpillImage(int spillId, int imageId)
        {
            using (var tracer = new Tracer("OrderController", "DeleteFuelSpillImage"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().DeleteFuelSpillImage(spillId, imageId);
                return response;
            }
        }

        [HttpGet]
        [NonAction]
        public async Task<ImageViewModel> GetImageNew(int imageId)
        {
            using (var tracer = new Tracer("OrderController", "GetImageNew"))
            {
                var response = await ContextFactory.Current.GetDomain<HelperDomain>().GetImage(imageId);
                return response;
            }
        }

        [HttpGet]
        [NonAction]
        public List<StateDropdownExtendedItem> GetStatesNew()
        {
            using (var tracer = new Tracer("OrderController", "GetStatesNew"))
            {
                var response = ContextFactory.Current.GetDomain<MasterDomain>().GetStatesEx();
                return response;
            }
        }

        [HttpGet]
        [NonAction]
        public List<DropdownDisplayItem> GetAssetFuelTypesNew()
        {
            using (var tracer = new Tracer("OrderController", "GetAssetFuelTypesNew"))
            {
                var response = ContextFactory.Current.GetDomain<MasterDomain>().GetAssetFuelTypes();
                return response;
            }
        }

        [HttpGet]
        [NonAction]
        public async Task<DriverOrderAssetViewModel> GetFilledAsset(int assetId, int orderId)
        {
            using (var tracer = new Tracer("OrderController", "GetFilledAsset"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().GetFilledAssetAsync(assetId, orderId);

                return response;
            }
        }

        [HttpGet]
        [NonAction]
        public async Task<DriverAssetDropHistoryViewModel> GetAssetDropHistory(int jobId, int assetId, int companyId, int skipCount, int limit)
        {
            using (var tracer = new Tracer("OrderController", "GetAssetDropHistory"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().GetDriverAssetDropHistoryAsync(jobId, assetId, companyId, skipCount, limit);
                return response;
            }
        }

        [HttpPost]
        [NonAction]
        public async Task<StatusViewModel> SaveAssetDrop(DriverAssetFuelDropViewModel viewModel)
        {
            using (var tracer = new Tracer("OrderController", "SaveAssetDrop"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().SaveDriverAssetDropAsync(viewModel);
                return response;
            }
        }

        [HttpPost]
        [NonAction]
        public async Task<StatusViewModel> CreateUnassignedInvoice(DriverDropOrderViewModel viewModel)
        {
            using (var tracer = new Tracer("OrderController", "CreateUnassignedInvoice"))
            {
                var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().CreateUnassignedInvoiceAsync(viewModel);
                return response;
            }
        }

        [HttpPost]
        [NonAction]
        public async Task<StatusViewModel> CreateMobileInvoice(DriverDropOrderViewModel viewModel)
        {
            using (var tracer = new Tracer("OrderController", "CreateMobileInvoice"))
            {
                //bool isBuySellOrder = ContextFactory.Current.GetDomain<InvoiceDomain>().IsBuySellOrder(viewModel.OrderId);
                //StatusViewModel response;
                //if (isBuySellOrder)
                //{
                //    response = await ContextFactory.Current.GetDomain<InvoiceDomain>().CreateMobileInvoiceAsync(viewModel);
                //}
                //else
                //{
                //    response = await ContextFactory.Current.GetDomain<InvoiceCreateDomain>().CreateMobileInvoiceAsync(viewModel);
                //}
                var response = await ContextFactory.Current.GetDomain<InvoiceCreateDomain>().CreateMobileInvoiceAsync(viewModel);
                return response;
            }
        }

        [HttpPost]
        [NonAction]
        public async Task<StatusViewModel> CreateFtlMobileInvoice(FtlDriverDropOrderViewModel viewModel)
        {
            using (var tracer = new Tracer("OrderController", "CreateFtlMobileInvoice"))
            {
                //bool isBuySellOrder = ContextFactory.Current.GetDomain<InvoiceDomain>().IsBuySellOrder(viewModel.OrderId);
                //StatusViewModel response;
                //if (isBuySellOrder)
                //{
                //    response = await ContextFactory.Current.GetDomain<InvoiceCreateDomain>().CreateMobileInvoiceAsync(viewModel);
                //}
                //else
                //{
                //    response = await ContextFactory.Current.GetDomain<InvoiceCreateDomain>().CreateFtlMobileInvoiceAsync(viewModel);
                //}
                var response = await ContextFactory.Current.GetDomain<InvoiceCreateDomain>().CreateFtlMobileInvoiceAsync(viewModel);
                return response;
            }
        }

        [HttpGet]
        [ApiLog(Enabled = true)]
        public async Task<DeliveryDetailsViewModel> GetDeliveryDetails(int orderId, int deliveryRequestId, long scheduleDate = 0)
        {

            DeliveryDetailsViewModel response = new DeliveryDetailsViewModel();
            try
            {
                if (orderId > 0)
                {
                    if (scheduleDate == 0)
                    {
                        scheduleDate = DateTimeOffset.Now.ToUnixTimeMilliseconds(); //if scheduleDate is passed 0 , consider today's date
                    }

                    response = await ContextFactory.Current.GetDomain<OrderDomain>().GetDeliveryDetails(orderId, deliveryRequestId, scheduleDate);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderController", "GetDeliveryDetails", ex.Message, ex);
            }

            var res = JsonConvert.SerializeObject(response);
            LogManager.Logger.WriteDebug("WhereIsMyDriver", "GetDeliveryDetails", $"OrderId:{orderId}, DeliverReqId:{deliveryRequestId}, scheduleDate: {scheduleDate}, Resp:{res}");
            return response;
        }

        [HttpGet]
        [ApiLog(Enabled = true)]
        public async Task<DriverDeliveryDetailsViewModel> GetDriverDeliveryDetails(int jobId, int orderId = 0)
        {
            using (var tracer = new Tracer("OrderController", "GetDriverDeliveryDetails"))
            {
                DriverDeliveryDetailsViewModel response = new DriverDeliveryDetailsViewModel();
                try
                {
                    if (jobId > 0)
                    {
                        response = await ContextFactory.Current.GetDomain<OrderDomain>().GetDriverDeliveryDetails(jobId, orderId);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "GetDriverDeliveryDetails", ex.Message, ex);
                }
                return response;
            }
        }

        [HttpPost]
        [ApiLog(Enabled = true)]
        public async Task<List<TerminalViewModel>> GetNearestTerminalsForDriver(ApiInputTerminalViewModel apiInputTerminal)
        {
            List<TerminalViewModel> response = new List<TerminalViewModel>();
            try
            {
                if (apiInputTerminal.OrderIds.Count > 0)
                {
                    response = await ContextFactory.Current.GetDomain<ExternalPricingDomain>().GetClosestTerminals(apiInputTerminal.OrderIds, apiInputTerminal.Latitude, apiInputTerminal.Longitude);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderController", "GetNearestTerminalsForDriver", ex.Message, ex);
            }
            return response;
        }

        [HttpPost]
        [ApiLog(Enabled = true)]
        public async Task<List<BulkPlantViewModel>> GetNearestBulkPlantsForDriver(ApiInputTerminalViewModel apiInputTerminal)
        {
            List<BulkPlantViewModel> response = new List<BulkPlantViewModel>();
            try
            {
                if (apiInputTerminal.OrderIds.Count > 0)
                {
                    response = await ContextFactory.Current.GetDomain<ExternalPricingDomain>().GetBulkPlantsForAutoFreightMethod(apiInputTerminal.OrderIds, apiInputTerminal.Latitude, apiInputTerminal.Longitude);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderController", "GetNearestBulkPlantsForDriver", ex.Message, ex);
            }
            return response;
        }

        [HttpGet]
        [ApiLog(Enabled = true)]
        public async Task<List<BulkPlantViewModel>> GetNearestBulkPlantsForDriver(int supplierId, decimal latitude, decimal longitude)
        {
            List<BulkPlantViewModel> response = new List<BulkPlantViewModel>();
            try
            {
                response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetNearestBulkPlantsForDriver(supplierId, latitude, longitude);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderController", "GetNearestBulkPlantsForDriver", ex.Message, ex);
            }
            return response;
        }

        [HttpGet]
        [ApiLog(Enabled = true)]
        public async Task<ApiDriverOrderForJobViewModel> GetDriverOrdersForJob(int jobId)
        {
            using (var tracer = new Tracer("OrderController", "GetDriverOrdersForJob"))
            {
                ApiDriverOrderForJobViewModel response = new ApiDriverOrderForJobViewModel();
                try
                {
                    if (jobId > 0)
                    {
                        response = await ContextFactory.Current.GetDomain<OrderDomain>().GetOrderForJobAsync(jobId);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "GetDriverOrdersForJob", ex.Message, ex);
                }
                return response;
            }
        }

        [HttpGet]
        [ApiLog(Enabled = true)]
        public async Task<List<DropdownDisplayItem>> GetNearestCustomerByFuelType(int fuelTypeId, int supplierCompanyId)
        {
            using (var tracer = new Tracer("OrderController", "GetNearestCustomerByFuelType"))
            {
                var response = new List<DropdownDisplayItem>();
                try
                {
                    if (fuelTypeId > 0 && supplierCompanyId > 0)
                    {
                        var viewModel = new ApiNearestCustomerByFuelTypeModel();
                        viewModel.FuelTypeIds.Add(fuelTypeId);
                        viewModel.SupplierCompanyId = supplierCompanyId;
                        response = await ContextFactory.Current.GetDomain<OrderDomain>().GetNearestCustomerByFuelType(viewModel);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "GetNearestCustomerByFuelType", ex.Message, ex);
                }
                return response;
            }
        }

        [HttpPost]
        [ApiLog(Enabled = true)]
        public async Task<List<DropdownDisplayItem>> GetNearestCustomerByFuelTypes(ApiNearestCustomerByFuelTypeModel viewModel)
        {
            using (var tracer = new Tracer("OrderController", "GetNearestCustomerByFuelTypes"))
            {
                var response = new List<DropdownDisplayItem>();
                try
                {
                    if (viewModel != null)
                    {
                        response = await ContextFactory.Current.GetDomain<OrderDomain>().GetNearestCustomerByFuelType(viewModel);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "GetNearestCustomerByFuelTypes", ex.Message, ex);
                }
                return response;
            }
        }

        [HttpGet]
        public async Task<List<OrderForJobViewModel>> SearchOrders(int jobId, string searchCriteria, int offset = 0, int count = 0, int supplierCompanyId = 0)
        {
            using (var tracer = new Tracer("OrderController", "SearchOrders"))
            {
                List<OrderForJobViewModel> response = new List<OrderForJobViewModel>();
                try
                {
                    if (jobId > 0)
                    {
                        var brandedSupCompanyId = GetBrandedSupplierCompId();
                        if (brandedSupCompanyId > 0) supplierCompanyId = brandedSupCompanyId;
                        response = await ContextFactory.Current.GetDomain<OrderDomain>().GetOrderForJobAsync(jobId, searchCriteria, offset, count, supplierCompanyId);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "SearchOrders", ex.Message, ex);
                }
                return response;
            }
        }

        [HttpGet]
        [ApiLog(Enabled = true)]
        public async Task<List<DeliveryScheduleForJobViewModel>> SearchDeliveryScheduleForJobAsync(int jobId, string searchCriteria, int offset = 0, int count = 0, long scheduleDate = 0, int supplierCompanyId = 0)
        {
            var response = new List<DeliveryScheduleForJobViewModel>();
            try
            {
                if (jobId > 0)
                {
                    if (scheduleDate == 0)
                    {
                        scheduleDate = DateTimeOffset.Now.ToUnixTimeMilliseconds(); //if scheduleDate is passed 0 , consider today's date
                    }

                    var brandedSupCompanyId = GetBrandedSupplierCompId();
                    if (brandedSupCompanyId > 0) supplierCompanyId = brandedSupCompanyId;
                    response = await ContextFactory.Current.GetDomain<OrderDomain>().GetDeliveryScheduleForJobAsync(jobId, searchCriteria, offset, count, scheduleDate, supplierCompanyId);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "SearchDeliveryScheduleForJobAsync", ex.Message, ex);
            }
            return response;
        }


        [HttpPost]
        [ApiLog(Enabled = true)]
        public async Task<StatusViewModel> ChangeDriverAcknowledgement(DeliveryScheduleAcknowledgeViewModel viewModel)
        {
            using (var tracer = new Tracer("OrderController", "ReadDeliverySchedule"))
            {
                StatusViewModel response = new StatusViewModel(Status.Failed);
                try
                {
                    if ((viewModel.TrackableScheduleId > 0) || (viewModel.GroupId.HasValue && viewModel.GroupId > 0))
                    {
                        response = await ContextFactory.Current.GetDomain<OrderDomain>().ChangeDriverAcknowledgement(viewModel.TrackableScheduleId, viewModel.Status, viewModel.UserTimeOffset, viewModel.GroupId);
                    }
                    else
                    {
                        response.StatusMessage = "Invalid TrackableScheduleId";
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "GetDeliveryScheduleAsync", ex.Message, ex);
                }
                return response;
            }
        }

        [HttpGet]
        [ApiLog(Enabled = true)]
        public async Task<List<DeliveryScheduleGroup>> GetDeliveryScheduleAsync(int userId, long scheduleDate, int userTimeOffset = -400, int companyId = 0)
        {
            using (var tracer = new Tracer("OrderController", "GetDeliveryScheduleAsync"))
            {
                List<DeliveryScheduleGroup> response = new List<DeliveryScheduleGroup>();
                try
                {
                    if (userId > 0)
                    {
                        if (companyId == 0)
                        {
                            var userContext = await GetUserContext(userId, CompanyType.Supplier);
                            companyId = userContext.CompanyId;
                        }
                        response = await ContextFactory.Current.GetDomain<OrderDomain>().GetDeliveryScheduleAsync(userId, companyId, scheduleDate, userTimeOffset);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "GetDeliveryScheduleAsync", ex.Message, ex);
                }
                return response;
            }
        }

        [HttpGet]
        [ApiLog(Enabled = true)]
        public async Task<List<DeliveryScheduleGroup>> GetScheduleAndOrdersAsync(int userId, int companyId, long scheduleDate, decimal latitude, decimal longitude, int userTimeOffset = -400, int buyerCompanyId = 0)
        {
            using (var tracer = new Tracer("OrderController", "GetScheduleAndOrdersAsync"))
            {
                List<DeliveryScheduleGroup> response = new List<DeliveryScheduleGroup>();
                try
                {
                    if (userId > 0)
                    {
                        //if (companyId == 0)
                        //{
                        //    var userContext = await GetUserContext(userId, CompanyType.Supplier);
                        //    companyId = userContext.CompanyId;
                        //}
                        var domain = ContextFactory.Current.GetDomain<OrderDomain>();
                        response = await domain.GetScheduleAndOrdersAsync(userId, companyId, scheduleDate, latitude, longitude, userTimeOffset, buyerCompanyId);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "GetScheduleAndOrdersAsync", ex.Message, ex);
                }
                return response;
            }
        }

        [HttpGet]
        [ApiLog(Enabled = true)]
        public async Task<DecimalResponseModel> GetGallonsPerMetricTonAsync(decimal gravity)
        {

            DecimalResponseModel response = new DecimalResponseModel();
            try
            {
                response.StatusMessage = Resource.errMessageInvalidGravity;
                if (gravity > 0)
                {
                    var domain = ContextFactory.Current.GetDomain<OrderDomain>();
                    response = await domain.GetGallonsPerMetricTonAsync(gravity);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderController", "GetGallonsPerMetricTonAsync", ex.Message, ex);
            }
            return response;
        }

        [HttpPost]
        [ApiLog(Enabled = true)]
        public async Task<ApiBolResponseViewModel> SaveBolDetails([FromBody] ApiBolDetailsViewModel viewModel)
        {
            using (var tracer = new Tracer("OrderController", "SaveBolDetails"))
            {
                ApiBolResponseViewModel response = new ApiBolResponseViewModel();
                response.StatusMessage = ValidateBolDetailsInput(viewModel);
                try
                {
                    if (string.IsNullOrWhiteSpace(response.StatusMessage))
                    {
                        var userContext = await GetUserContext(viewModel.UserId, CompanyType.Supplier);
                        response = await ContextFactory.Current.GetDomain<OrderDomain>().SaveBolDetailsAsync(viewModel, userContext.CompanyId);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "SaveBolDetails", ex.Message, ex);
                }
                return response;
            }
        }

        [HttpPost]
        [ApiLog(Enabled = true)]
        public async Task<ApiBolFuelDetailsResponseModel> SaveBolFuelDetails(ApiBolFuelDetailsRequestModel request)
        {
            using (var tracer = new Tracer("OrderController", "SaveBolFuelDetails"))
            {
                ApiBolFuelDetailsResponseModel response = new ApiBolFuelDetailsResponseModel();
                try
                {
                    var requestJson = string.Empty;
                    if (request == null)
                    {
                        requestJson = Convert.ToString(HttpContext.Current.Request.Form["request"]) ?? string.Empty;
                        request = JsonConvert.DeserializeObject<ApiBolFuelDetailsRequestModel>(requestJson);
                    }

                    if (request != null)
                    {
                        var uploadedFiles = HttpContext.Current.Request.Files;
                        if (uploadedFiles != null && uploadedFiles.Count > 0)
                        {
                            request.BolFile = HttpContext.Current.Request.Files["bolFile"] as HttpPostedFile;
                            request.AdditionalFile = HttpContext.Current.Request.Files["additionalFile"] as HttpPostedFile;
                        }

                        response.StatusMessage = ValidateBolDetailsInput(request);
                        if (string.IsNullOrWhiteSpace(response.StatusMessage))
                        {
                            var userContext = await GetUserContext(request.UserId, CompanyType.Supplier);
                            response = await ContextFactory.Current.GetDomain<OrderDomain>().SaveBolFuelDetailsAsync(request, userContext);
                        }
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessagFailedToSaveDetails;
                    }
                }
                catch (Exception ex)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessagFailedToSaveDetails;
                    LogManager.Logger.WriteException("OrderController", "SaveBolFuelDetails", ex.Message, ex);
                }
                response.DisplayMode = PageDisplayMode.Create;
                return response;
            }
        }

        [HttpPost]
        [ApiLog(Enabled = true)]
        public async Task<StatusViewModel> UpdateSplitDropAddressStatus(ApiDispatchAddressViewModel viewModel)
        {
            using (var tracer = new Tracer("OrderController", "UpdateSplitDropAddressStatus"))
            {
                StatusViewModel response = new StatusViewModel();

                try
                {
                    if (viewModel.Address.Id == 0 && viewModel.Address.IsJobLocation == false)
                    {
                        response.StatusMessage = "AddressId " + Constants.DataMissing;
                    }
                    else
                    {
                        response = await ContextFactory.Current.GetDomain<OrderDomain>().UpdateSplitDropAddressStatusAsync(viewModel);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "UpdateSplitDropAddressStatus", ex.Message, ex);
                }
                return response;
            }
        }


        [HttpPost]
        [ApiLog(Enabled = true)]
        public async Task<StatusViewModel> AddSplitDropAddress(ApiDispatchAddressViewModel viewModel)
        {
            using (var tracer = new Tracer("OrderController", "AddSplitDropAddress"))
            {
                var response = new StatusViewModel();
                if (viewModel.Address == null)
                {
                    response.StatusMessage = "Address " + Constants.DataMissing;
                    return response;
                }
                response = await ContextFactory.Current.GetDomain<DispatchDomain>().AddSplitDropAddressAsync(viewModel);
                return response;
            }
        }

        [HttpPost]
        [ApiLog(Enabled = true)]
        public async Task<StatusViewModel> CreateTankDeliveryRequest(List<ApiDeliveryRequestViewModel> viewModel)
        {
            using (var tracer = new Tracer("OrderController", "CreateTankDeliveryRequest"))
            {
                var response = await CheckTankDRValidation(viewModel);

                if (response.StatusCode == Status.Success)
                {
                    var userContext = await GetUserContext(viewModel.FirstOrDefault().UserId, CompanyType.Buyer);

                    response = await ContextFactory.Current.GetDomain<OrderDomain>().CreateDeliveryRequest(viewModel, userContext);
                    if (response.StatusMessage == "Invalid SupplierCompanyId")
                    {
                        response.StatusMessage = @Resource.warningMessageNoOrdersFoundForGivenProductType;
                    }
                }
                return response;
            }
        }

        private async Task<StatusViewModel> CheckTankDRValidation(List<ApiDeliveryRequestViewModel> viewModel)
        {
            using (var tracer = new Tracer("OrderController", "CheckTankDRValidation"))
            {
                var response = new StatusViewModel();
                try
                {
                    var tankIds = viewModel.Select(t => t.AssetId).ToList();
                    var tankAdditionalList = await new FreightServiceDomain().GetTankList(tankIds);
                    List<ApiTankDetailViewModel> tankList = new List<ApiTankDetailViewModel>();
                    foreach (var item in tankAdditionalList)
                    {
                        var tank = item.ToApiTankViewModel(string.Empty);
                        tankList.Add(tank);
                    }

                    foreach (var item in viewModel)
                    {
                        var tank = tankList.FirstOrDefault(t => t.Id == item.AssetId);
                        if (tank != null)
                        {
                            if (item.RequiredQuantity <= 0)
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format(Resource.valMessageTankQuantityRequired, tank.TankName);
                                return response;
                            }


                            if (tank.MaxFill.HasValue && item.RequiredQuantity > tank.MaxFill)
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format(Resource.valMessageMaxFillQuantity, tank.TankName);
                                return response;
                            }
                        }
                    }

                    response.StatusCode = Status.Success;
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "CheckTankDRValidation", ex.Message, ex);
                }
                return response;
            }
        }

        [HttpGet]
        [ApiLog(Enabled = true)]
        public async Task<List<DeliveryScheduleForDriverViewModel>> GetDropCompletedForDriverAsync(int userId, long scheduleDate, int offset = -400)
        {
            using (var tracer = new Tracer("OrderController", "GetDropCompletedForDriverAsync"))
            {
                List<DeliveryScheduleForDriverViewModel> response = new List<DeliveryScheduleForDriverViewModel>();
                try
                {
                    if (userId > 0)
                    {
                        response = await ContextFactory.Current.GetDomain<OrderDomain>().GetDropCompletedForDriverAsync(userId, scheduleDate, offset);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "GetDropCompletedForDriverAsync", ex.Message, ex);
                }
                return response;
            }
        }

        [HttpPost]
        // [ApiLog(Enabled = true)]
        public async Task<StatusViewModel> SaveAppLocation(AppLocationViewModel viewModel)
        {
            using (var tracer = new Tracer("OrderController", "SaveAppLocation"))
            {
                StatusViewModel response = new StatusViewModel();
                try
                {
                    response = await ContextFactory.Current.GetDomain<OrderDomain>().SaveAppLocation(viewModel);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "SaveAppLocation", ex.Message, ex);
                }
                return response;
            }
        }

        [HttpPost]
        [ApiLog(Enabled = true)]
        public async Task<StatusViewModel> SaveExternalDrop(ExternalDropDetailViewModel viewModel)
        {
            using (var tracer = new Tracer("OrderController", "SaveExternalDrop"))
            {
                StatusViewModel response = new StatusViewModel();
                try
                {
                    response = await ContextFactory.Current.GetDomain<InvoiceDomain>().SaveExternalDrop(viewModel);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "SaveExternalDrop", ex.Message, ex);
                }
                return response;
            }
        }

        [HttpGet]
        [ApiLog(Enabled = true)]
        public async Task<List<DriverCalendarViewModel>> GetMonthlyDriverSchedule(int userId, long scheduleDate)
        {
            using (var tracer = new Tracer("OrderController", "GetMonthlyDriverSchedule"))
            {
                List<DriverCalendarViewModel> response = new List<DriverCalendarViewModel>();
                try
                {
                    if (userId > 0)
                    {
                        response = await ContextFactory.Current.GetDomain<OrderDomain>().GetMonthlyDriverSchedule(userId, scheduleDate);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "GetMonthlyDriverSchedule", ex.Message, ex);
                }
                return response;
            }
        }

        [HttpGet]
        [ApiLog(Enabled = true)]
        public async Task<OrderDetailsOutPutViewModel> GetOrderDetails(int orderId)
        {
            using (var tracer = new Tracer("OrderController", "GetOrderDetails"))
            {
                OrderDetailsOutPutViewModel response = new OrderDetailsOutPutViewModel();
                try
                {
                    if (orderId > 0)
                    {
                        response = await ContextFactory.Current.GetDomain<OrderDomain>().GetOrderDetailsAsync(orderId);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "GetOrderDetails", ex.Message, ex);
                }
                return response;
            }
        }

        [HttpGet]
        [ApiLog(Enabled = true)]
        public async Task<List<DeliveryScheduleForOrderViewModel>> GetDeliverySchedulesForOrder(int orderId, long startDate = 0, long endDate = 0, int offset = 0, int count = 0)
        {
            using (var tracer = new Tracer("OrderController", "GetDeliverySchedulesForOrder"))
            {
                List<DeliveryScheduleForOrderViewModel> response = new List<DeliveryScheduleForOrderViewModel>();
                try
                {
                    if (orderId > 0)
                    {
                        response = await ContextFactory.Current.GetDomain<OrderDomain>().GetDeliverySchedulesForOrder(orderId, startDate, endDate, offset, count);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "GetDeliverySchedulesForOrder", ex.Message, ex);
                }
                return response;
            }
        }

        [HttpPost]
        [ApiLog(Enabled = true)]
        public async Task<StatusViewModel> CancelDeliverySchedule(int userId, int orderId, int trackableScheduleId = 0)
        {
            using (var tracer = new Tracer("OrderController", "CancelDeliverySchedule"))
            {
                var userContext = await GetUserContext(userId, CompanyType.Buyer);
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().CancelDeliverySchedule(userContext, orderId, trackableScheduleId);
                return response;
            }
        }

        [HttpPost]
        [ApiLog(Enabled = true)]
        public async Task<CreateDeliveryRequestOutputViewModel> CreateDeliveryRequest(CreateDeliveryRequestInputViewModel deliveryRequestViewModel)
        {
            using (var tracer = new Tracer("OrderController", "CreateDeliveryRequest"))
            {
                var response = new CreateDeliveryRequestOutputViewModel();
                try
                {
                    if (ModelState.IsValid)
                    {
                        response = await CheckValidation(deliveryRequestViewModel);

                        if (response.StatusCode == Status.Success)
                        {
                            UserContext UserContext = await GetUserContext(deliveryRequestViewModel.UserId, CompanyType.Buyer);
                            var deliveryResponse = await ContextFactory.Current.GetDomain<OrderDomain>().SaveDeliveryRequestAsync(UserContext, deliveryRequestViewModel);
                            response.DeliveryRequestId = deliveryRequestViewModel.Id;
                            response.StatusCode = deliveryResponse.StatusCode;
                            response.StatusMessage = deliveryResponse.StatusMessage;
                        }
                    }
                    else
                    {
                        var geterror = new CommonMethods().GetErrorMessage(ModelState);
                        response.StatusCode = geterror.StatusCode;
                        response.StatusMessage = geterror.StatusMessage;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "CreateDeliveryRequest", ex.Message, ex);
                }
                return response;
            }
        }

        [HttpGet]
        [ApiLog(Enabled = true)]
        public async Task<List<CompanyViewModel>> GetTpoSuppliers(int buyerCompanyId)
        {
            using (var tracer = new Tracer("OrderController", "GetTpoSuppliers"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().GetTpoSuppliersAsync(buyerCompanyId);
                return response;
            }
        }

        // gets called from Driver app only
        public async Task<StatusViewModel> EnrouteDelivery(EnrouteDeliveryViewModel viewModel)
        {
            using (var tracer = new Tracer("OrderController", "EnrouteDelivery"))
            {
                StatusViewModel response = new StatusViewModel();
                try
                {
                    response = await ContextFactory.Current.GetDomain<OrderDomain>().EnrouteDelivery(viewModel);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "EnrouteDelivery", ex.Message, ex);
                }
                return response;
            }
        }

        private async Task<CreateDeliveryRequestOutputViewModel> CheckValidation(CreateDeliveryRequestInputViewModel deliveryRequestViewModel)
        {
            using (var tracer = new Tracer("OrderController", "CheckValidation"))
            {
                var response = new CreateDeliveryRequestOutputViewModel();
                try
                {
                    int fuelRequestId = await ContextFactory.Current.GetDomain<FuelRequestDomain>().GetFuelRequestIdAsync(deliveryRequestViewModel.OrderId);
                    var fuelRequestResponse = await ContextFactory.Current.GetDomain<FuelRequestDomain>().GetFuelRequestAsync(fuelRequestId);

                    if (fuelRequestResponse.FuelDeliveryDetails.DeliveryTypeId == (int)DeliveryType.MultipleDeliveries)
                    {
                        //Compare with FR Start and End Date
                        if (deliveryRequestViewModel.StartDate.Date < fuelRequestResponse.FuelDeliveryDetails.StartDate.Date)
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.valMessageDeliveryScheduleDateLessThanFRStartDate;
                            return response;
                        }

                        if (fuelRequestResponse.FuelDeliveryDetails.EndDate != null && deliveryRequestViewModel.StartDate.Date > fuelRequestResponse.FuelDeliveryDetails.EndDate.Value)
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.valMessageDeliveryScheduleDateGreaterThanFRStartDate;
                            return response;
                        }
                        //Compare with FR Start and End Date


                        //Compare with Job Start and End Date
                        if (deliveryRequestViewModel.StartDate.Date < Convert.ToDateTime(fuelRequestResponse.Job.JobStartDate).Date)
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.valMessageLessThanJobStartDate;
                            return response;
                        }

                        if (!string.IsNullOrEmpty(fuelRequestResponse.Job.JobEndDate))
                        {
                            if (deliveryRequestViewModel.StartDate.Date > Convert.ToDateTime(fuelRequestResponse.Job.JobEndDate).Date)
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = Resource.valMessageLessThanJobEndDate;
                                return response;
                            }
                        }
                        //Compare with Job Start and End Date

                        decimal quantity = 0;
                        quantity = ContextFactory.Current.GetDomain<OrderDomain>().GetRemaingScheduleQuantity(deliveryRequestViewModel.OrderId);

                        if (deliveryRequestViewModel.Quantity > quantity)
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.valMessageScheduleQuantity;
                            return response;
                        }
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageDeliverySchedulesSaveFailedForSingleDeliveryType;
                        return response;
                    }
                    response.StatusCode = Status.Success;
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "CheckValidation", ex.Message, ex);
                }
                return response;
            }
        }

        [HttpPost]
        [ApiLog(Enabled = true)]
        public async Task<StatusViewModel> ChangePickupLocation(ChangePickupLocationViewModel request)
        {
            StatusViewModel response = new StatusViewModel();
            var json = JsonConvert.SerializeObject(request);
            using (var tracer = new Tracer("OrderController", $"ChangePickupLocation(request: {json})"))
            {
                try
                {
                    if (ValidateChangePickupLocationRequest(request, response))
                    {
                        response = await ContextFactory.Current.GetDomain<OrderDomain>().ChangePickUpLocationAsync(request);
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                    }
                }
                catch (Exception ex)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = "Failed";
                    LogManager.Logger.WriteException("OrderController", "ChangePickupLocation", ex.Message, ex);
                }

                return response;
            }
        }
        #endregion

        #region UN-SCHEDULE DROP
        [HttpGet]
        [ApiLog(Enabled = true)]
        public async Task<OnboardingPreferenceViewModel> GetPreferencesSettingBySupplierAsync(int supplierCompanyId)
        {
            using (var tracer = new Tracer("OrderController", "GetPreferencesSettingBySupplierAsync"))
            {
                var response = new OnboardingPreferenceViewModel();
                try
                {
                    response = await ContextFactory.Current.GetDomain<CompanyDomain>().GetPreferencesSettingBySupplierAsync(supplierCompanyId);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "GetPreferencesSettingBySupplierAsync", ex.Message, ex);
                }
                return response;
            }
        }
        #endregion UN-SCHEDULE DROP

        #region Private Functions New Style

        private bool ValidateSaveAssetDropInput(DriverAssetFuelDropViewModel viewModel, bool isBlend)
        {
            bool response = false;
            if (viewModel != null)
            {
                if (viewModel.FuelDrop.IsNoFuelNeeded || isBlend)
                {
                    return true;
                }

                switch (viewModel.FuelDrop.RunningMeterMode)
                {
                    case RunningMeterMode.None:
                    case RunningMeterMode.No:
                        {
                            response = (viewModel.FuelDrop.PrimaryMeterEndReading == 0 && viewModel.FuelDrop.SecondaryMeterEndReading == 0);
                        }
                        break;
                    case RunningMeterMode.Yes:
                        {
                            response = (viewModel.FuelDrop.PrimaryMeterEndReading > 0 && viewModel.FuelDrop.SecondaryMeterEndReading >= 0);
                        }
                        break;
                }
            }
            return response;
        }

        private bool ValidateChangePickupLocationRequest(ChangePickupLocationViewModel request, StatusViewModel status)
        {
            bool response = true;
            if (request != null)
            {
                if (request.Orders == null || request.Orders.Count == 0)
                {
                    status.StatusMessage = "Please provide Order details";
                    response = false;
                }
                else if (request.Orders.Any(t => t.OrderId <= 0))
                {
                    status.StatusMessage = "OrderId required";
                    response = false;
                }
                else if ((request.TerminalId == null || request.TerminalId == 0) && string.IsNullOrWhiteSpace(request.ZipCode))
                {
                    status.StatusMessage = "Terminal or ZipCode required";
                    response = false;
                }
                else if (request.UserId <= 0)
                {
                    status.StatusMessage = "UserId required";
                    response = false;
                }
            }
            else
            {
                status.StatusMessage = "Invalid request";
                response = false;
            }

            return response;
        }

        #endregion


        #region Private Functions Old Style

        private string ValidateGetOrdersInput(decimal latitude, decimal longitude, int companyId)
        {
            string response = string.Empty;
            if (/*latitude == 0 || longitude == 0 || */companyId == 0)
            {
                response = "ComapnyId " + Constants.DataMissing;
            }
            return response;
        }

        private string ValidateAddNewAssetInput(NewAssetDetails newAsset)
        {
            if (newAsset.CompanyID == 0)
            {
                return "Companyid " + Constants.DataMissing;
            }
            else if (newAsset.UserID == 0)
            {
                return "UserID " + Constants.DataMissing;
            }
            if (newAsset.JobDetailId == 0)
            {
                return "JobDetailId " + Constants.DataMissing;
            }
            else if (string.IsNullOrWhiteSpace(newAsset.Name))
            {
                return "Name " + Constants.DataMissing;
            }
            return string.Empty;
        }

        private string ValidateBolDetailsInput(ApiBolDetailsViewModel bolDetails)
        {
            if (string.IsNullOrWhiteSpace(bolDetails.BolImage))
            {
                return "BolImage " + Constants.DataMissing;
            }
            else if (bolDetails.IsDriverToUpdateBOL)
            {
                if (bolDetails.UserId == 0)
                {
                    return "UserId " + Constants.DataMissing;
                }
                if (bolDetails.GrossQuantity == 0)
                {
                    return "GrossQuantity " + Constants.DataMissing;
                }
                else if (bolDetails.NetQuantity == 0)
                {
                    return "NetQuantity " + Constants.DataMissing;
                }
            }
            return string.Empty;
        }

        private string ValidateBolDetailsInput(ApiBolFuelDetailsRequestModel bolDetails)
        {
            if (bolDetails.UserId == 0)
            {
                return "User Id " + Constants.DataMissing;
            }

            if (bolDetails.IsDriverToUpdateBOL)
            {
                if (bolDetails.IsBulkPlant) // consider BolNumber, NetQuantity as LiftNumber, LiftQuantity in case of bulk plant
                {
                    if (bolDetails.BolFuelDetails != null && bolDetails.BolFuelDetails.Any(t => t.IsFtl))
                    {
                        foreach (var item in bolDetails.BolFuelDetails.Where(t => t.IsFtl))
                        {
                            if (bolDetails.BolFile == null)
                            {
                                return "Lift Ticket Image " + Constants.DataMissing;
                            }
                            if (string.IsNullOrWhiteSpace(bolDetails.Carrier))
                            {
                                return "Carrier " + Constants.DataMissing;
                            }
                        }
                    }

                    if (bolDetails.BolFuelDetails != null)
                    {
                        if (bolDetails.BolFuelDetails.Any(t => t.IsFtl && t.NetQuantity == 0))
                        {
                            return "Net Quantity " + Constants.DataMissing;
                        }
                        if (bolDetails.BolFuelDetails.Any(t => t.IsFtl && (t.GrossQuantity == null || t.GrossQuantity == 0)))
                        {
                            return "Gross Quantity " + Constants.DataMissing;
                        }
                    }
                    if (bolDetails.BolFuelDetails != null && bolDetails.BolFuelDetails.Any(t => t.IsFtl && (bolDetails.BolNumber == string.Empty || bolDetails.BolNumber == null)))
                    {
                        return "Lift Ticket " + Constants.DataMissing;
                    }
                }
                else
                {
                    if (bolDetails.BolFuelDetails != null && bolDetails.BolFuelDetails.Any(t => t.IsFtl))
                    {
                        foreach (var item in bolDetails.BolFuelDetails.Where(t => t.IsFtl))
                        {
                            if (bolDetails.BolFile == null)
                            {
                                return "Bol Image " + Constants.DataMissing;
                            }
                            if (string.IsNullOrWhiteSpace(bolDetails.BolNumber))
                            {
                                return "Bol Number " + Constants.DataMissing;
                            }
                            if (string.IsNullOrWhiteSpace(bolDetails.Carrier))
                            {
                                return "Carrier " + Constants.DataMissing;
                            }
                        }
                    }
                    if (bolDetails.BolFuelDetails != null && bolDetails.BolFuelDetails.Any(t => t.IsFtl && t.NetQuantity == 0))
                    {
                        return "Net Quantity " + Constants.DataMissing;
                    }
                    if (bolDetails.BolFuelDetails != null && bolDetails.BolFuelDetails.Any(t => t.IsFtl && t.GrossQuantity == null || (t.GrossQuantity.HasValue && t.GrossQuantity.Value == 0)))
                    {
                        return "Gross Quantity " + Constants.DataMissing;
                    }
                }
            }

            return string.Empty;
        }

        private string CheckIsValidNewSpillFuelRequest(SpillFuelDetails newSpill)
        {
            if (newSpill.AssetId == 0)
            {
                return "AssetId " + Constants.DataMissing;
            }

            if (newSpill.OrderId == 0)
            {
                return "OrderId " + Constants.DataMissing;
            }

            if (newSpill.CompanyID == 0)
            {
                return "CompanyID " + Constants.DataMissing;
            }

            if (newSpill.UserId == 0)
            {
                return "UserId " + Constants.DataMissing;
            }

            if (newSpill.SpillTime < 1)
            {
                return "SpillTimeSpan " + Constants.DataMissing;
            }

            if (string.IsNullOrWhiteSpace(newSpill.Notes))
            {
                return "Notes " + Constants.DataMissing;
            }

            return string.Empty;
        }

        private string CheckIsValidGetSpillFuelRequest(int assetId, int spillId)
        {
            if (assetId == 0)
            {
                return "assetId " + Constants.DataMissing;
            }

            if (spillId == 0)
            {
                return "spillId " + Constants.DataMissing;
            }

            return string.Empty;
        }

        private string CheckIsValidDeleteSpillRequest(int deleteSpillId)
        {
            if (deleteSpillId == 0)
            {
                return "deleteSpillId " + Constants.DataMissing;
            }

            return string.Empty;

        }

        private string CheckIsValidDeleteImageRequest(int imageId)
        {
            if (imageId == 0)
            {
                return "imageId " + Constants.DataMissing;
            }

            return string.Empty;
        }

        private string CheckIsValidAssetDropHistoryRequest(int jobId, int assetId, int companyId)
        {
            if (jobId == 0)
            {
                return "jobId " + Constants.DataMissing;
            }

            if (assetId == 0)
            {
                return "assetId " + Constants.DataMissing;
            }

            if (companyId == 0)
            {
                return "companyId " + Constants.DataMissing;
            }

            return string.Empty;
        }

        private string CheckIsValidAssetDropsRequest(int assetId, int orderId)
        {
            if (assetId == 0)
            {
                return "assetId " + Constants.DataMissing;
            }

            if (orderId == 0)
            {
                return "orderId " + Constants.DataMissing;
            }

            return string.Empty;
        }

        private string CheckIsValidAssetDropRequest(AssetDropRequestModel dropRequest)
        {
            if (dropRequest == null)
            {
                return Constants.InvalidDropRequest;
            }

            if (dropRequest.data == null)
            {
                return "Data: " + Constants.DataMissing;
            }

            //if (dropRequest.data.AssetDropId == 0)
            //    if (string.IsNullOrWhiteSpace(dropRequest.receipt))
            //        return "Receipt " + Constants.DataMissing;

            if (string.IsNullOrWhiteSpace(dropRequest.data.quantity))
            {
                return "Quantity " + Constants.DataMissing;
            }

            if (dropRequest.data.timeStamps == null)
            {
                return "Timestapms " + Constants.DataMissing;
            }

            if (dropRequest.data.timeStamps.assetStartDropTime == 0)
            {
                return "AssetStartDropTime " + Constants.DataMissing;
            }

            if (dropRequest.data.timeStamps.assetEndDropTime == 0)
            {
                return "AssetEndDropTime " + Constants.DataMissing;
            }

            if (dropRequest.data.AssetId == 0)
            {
                return "AssetId " + Constants.DataMissing;
            }

            if (dropRequest.data.JobXAssignmentId == 0)
            {
                return "JobXAssignmentId " + Constants.DataMissing;
            }

            decimal quantity;
            if (!decimal.TryParse(dropRequest.data.quantity, out quantity))
            {
                return "Quantity " + Constants.ConversionError;
            }

            if (dropRequest.data.userid == 0)
            {
                return "UserID " + Constants.DataMissing;
            }

            if (dropRequest.data.companyid == 0)
            {
                return "Companyid " + Constants.DataMissing;
            }

            return string.Empty;
        }

        private string CheckIsValidCreateRequest(NewOrderRequestModel newOrder)
        {
            if (newOrder == null || newOrder.data == null)
            {
                return Constants.InvalidCreateRequest;
            }

            if (newOrder.data.userid == 0)
            {
                return "UserID " + Constants.DataMissing;
            }

            if (string.IsNullOrWhiteSpace(newOrder.data.quantity))
            {
                return "Quantity " + Constants.DataMissing;
            }

            if (string.IsNullOrWhiteSpace(newOrder.data.customerName))
            {
                return "CustomerName " + Constants.DataMissing;
            }

            if (string.IsNullOrWhiteSpace(newOrder.receipt))
            {
                return "Receipt " + Constants.DataMissing;
            }

            return null;
        }

        private string CheckIsValidDropRequest(DropRequestModel dropRequest)
        {
            if (dropRequest == null)
            {
                return Constants.InvalidDropRequest;
            }

            if (dropRequest.data == null)
            {
                return "Data: " + Constants.DataMissing;
            }

            if (string.IsNullOrWhiteSpace(dropRequest.data.quantity))
            {
                return "Quantity " + Constants.DataMissing;
            }

            if (dropRequest.data.timeStamps == null)
            {
                return "Timestamps " + Constants.DataMissing;
            }

            if (string.IsNullOrWhiteSpace(dropRequest.data.fuelId))
            {
                return "FuelID " + Constants.DataMissing;
            }

            if (string.IsNullOrWhiteSpace(dropRequest.data.orderId))
            {
                return "OrderID " + Constants.DataMissing;
            }

            int orderID, fuelID;
            decimal quantity;
            if (!Int32.TryParse(dropRequest.data.orderId.Replace(ApplicationConstants.SFPO, ""), out orderID))
            {
                return "OrderID " + Constants.ConversionError;
            }

            if (!Int32.TryParse(dropRequest.data.fuelId.Replace(",", ""), out fuelID))
            {
                return "FuelID " + Constants.ConversionError;
            }

            if (!decimal.TryParse(dropRequest.data.quantity, out quantity))
            {
                return "Quantity " + Constants.ConversionError;
            }

            if (dropRequest.data.userid == 0)
            {
                return "UserID " + Constants.DataMissing;
            }

            if (dropRequest.data.companyid == 0)
            {
                return "Companyid " + Constants.DataMissing;
            }

            var isDropImageRequired = ContextFactory.Current.GetDomain<HelperDomain>().GetDropImageRequiredFromOrder(Convert.ToInt32(dropRequest.data.orderId));
            if (dropRequest.data.InvoiceStatusId != (int)InvoiceStatus.Draft && string.IsNullOrWhiteSpace(dropRequest.receipt) && isDropImageRequired)
            {
                LogManager.Logger.WriteException("OrderController", "CheckIsValidDropRequest", string.Format("Drop image is missing orderId {0} userId {1}",
                    dropRequest.data.orderId, dropRequest.data.userid), new Exception());
            }

            return string.Empty;
        }

        #endregion

        #region TPD API
        [HttpGet]
        [ApiLog(Enabled = true, TPDLogEnabled = true)]
        public async Task<List<TPDOrderDetails>> GetTfxPoNumbers(int maxCount = 0)
        {
            var token = ActionContext.Request.Headers.FirstOrDefault(t => t.Key.ToLower() == ApplicationConstants.Token).Value.First();
            var orders = await ContextFactory.Current.GetDomain<OrderDomain>().GetTPDPONumbers(token);
            return orders;
        }
        #endregion

        [HttpPost]
        [ApiLog(Enabled = true)]
        public async Task<List<EBolAPIResponseModel>> GetEBolDetails(EBolAPIRequestModel eBOLApiRequest)
        {
            var response = new List<EBolAPIResponseModel>();
            using (var tracer = new Tracer("OrderController", "GetEBolDetails"))
            {
                response = await ContextFactory.Current.GetDomain<EBolDomain>().GetEBol(eBOLApiRequest);
                return response;
            }
        }

        [HttpGet]
        [ApiLog(Enabled = true)]
        public async Task<List<DeliverySchedulesForBuyerViewModel>> GetDeliverySchedulesForBuyerApp(int buyerCompanyId, long scheduleDate = 0, int userTimeOffset = 0, int offset = 0, int count = 0)
        {
            using (var tracer = new Tracer("OrderController", "GetDeliverySchedulesForBuyerApp"))
            {
                List<DeliverySchedulesForBuyerViewModel> response = new List<DeliverySchedulesForBuyerViewModel>();
                try
                {
                    var brandedSupCompanyId = GetBrandedSupplierCompId();
                    response = await ContextFactory.Current.GetDomain<OrderDomain>()
                                    .GetDeliverySchedulesForBuyerApp(buyerCompanyId, scheduleDate, userTimeOffset, offset, count, brandedSupCompanyId);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "GetDeliverySchedulesForBuyerApp", ex.Message, ex);
                }
                return response;
            }
        }
        [HttpPost]
        public async Task<WhereIsMyDriverBuyerAppViewModel> GetOnGoingLoadsForBuyerAppAsync(WhereIsMyDriverBuyerAppInputModel input)
        {
            using (var tracer = new Tracer("OrderController", "GetOnGoingLoadsForBuyerAppAsync"))
            {
                var brandedSuppCompanyId = GetBrandedSupplierCompId();
                if (brandedSuppCompanyId > 0)
                {
                    input.SupplierCompanyIds.Clear();
                    input.SupplierCompanyIds.Add(brandedSuppCompanyId);
                }

                var response = await ContextFactory.Current.GetDomain<WallyBoardDomain>().GetOnGoingLoadsForBuyerAppAsync(input);
                return response;
            }
        }
        [HttpGet]
        [ApiLog(Enabled = true)]
        public async Task<IntegerResponseModel> GetUnreadNotificationsCount(int userId, int notificationCode = 0, DateTimeOffset? createdDate = null, int appTypeId = 0)
        {
            IntegerResponseModel response = new IntegerResponseModel();
            using (var tracer = new Tracer("OrderController", "GetUnreadNotificationsCount"))
            {
                try
                {
                    response = await ContextFactory.Current.GetDomain<PushNotificationDomain>().GetUnreadNotificationsCount(userId, notificationCode, createdDate, appTypeId);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "GetUnreadNotificationsCount", ex.Message, ex);
                }
                return response;
            }
        }
        [HttpGet]
        [ApiLog(Enabled = true)]
        public async Task<List<PushNotificationViewModel>> GetPushNotificationLogs(int userId, int notificationCode = 0, DateTimeOffset? createdDate = null, int appTypeId = 0)
        {
            List<PushNotificationViewModel> response = new List<PushNotificationViewModel>();
            using (var tracer = new Tracer("OrderController", "GetPushNotificationLogs"))
            {
                try
                {
                    response = await ContextFactory.Current.GetDomain<PushNotificationDomain>().GetPushNotificationLogs(userId, notificationCode, createdDate, appTypeId);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "GetPushNotificationLogs", ex.Message, ex);
                }
                return response;
            }
        }
        [HttpPost]
        [ApiLog(Enabled = true)]
        public async Task<StatusViewModel> ClearNotificationForBuyerApp(ClearNotificationInputModel inputModel)
        {
            var response = new StatusViewModel();
            using (var tracer = new Tracer("OrderController", "ClearNotificationForBuyerApp"))
            {
                try
                {
                    response = await ContextFactory.Current.GetDomain<PushNotificationDomain>().ClearNotificationForBuyerApp(inputModel);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OrderController", "ClearNotificationForBuyerApp", ex.Message, ex);
                }
                return response;
            }
        }

        [HttpPost]
        [ApiLog(Enabled = true)]
        public async Task OrderStatus(OrderStatusRequestModel viewModel)
        {
            if (viewModel != null && viewModel.SalesOrderStatus != null)
            {
                await ContextFactory.Current.GetDomain<HeldDrQueueDomain>().PushDRStatusToQueue(viewModel);
            }
        }
    }
}