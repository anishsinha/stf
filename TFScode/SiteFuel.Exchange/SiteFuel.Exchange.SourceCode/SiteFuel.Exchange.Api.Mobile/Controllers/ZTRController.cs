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
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Core.Logger;

namespace SiteFuel.Exchange.Api.Mobile.Controllers
{
#if DEBUG
    [ApiExplorerSettings(IgnoreApi = false)]
#else
    [ApiExplorerSettings(IgnoreApi = true)]
#endif

    [ValidateToken]
    public class ZTRController : ApiBaseController
    {
        [HttpPost]
        public async Task<CreateFuelRequestOutputViewModel> CreateFuelRequest(CreateFuelRequestInputViewModel fuelRequestViewModel)
        {
            using (var tracer = new Tracer("ZTRController", "CreateFuelRequest"))
            {
                var response = new CreateFuelRequestOutputViewModel();
                try
                {
                    var commonMethods = new CommonMethods();
                    if (ModelState.IsValid)
                    {
                        if (commonMethods.ValidateDate(fuelRequestViewModel.DateNeeded))
                        {
                            var frViewModel = ToFuelRequestModel(fuelRequestViewModel);

                            var assetValidation = ContextFactory.Current.GetDomain<AssetDomain>().ValidateAssetsAsync(fuelRequestViewModel.AssetFuelRequests, frViewModel.Job.JobId);
                            if (assetValidation.AssetFuelRequestsList.Count > 0)
                            {
                                frViewModel.FuelDetails.FuelQuantity.Quantity = assetValidation.AssetFuelRequestsList.Sum(t => t.QuantityRequired);

                                FuelRequestController fuelRequestController = new FuelRequestController();
                                response = await fuelRequestController.Create(frViewModel);
                                if (response.FuelRequestId > 0)
                                {
                                    //save assetfuel requests
                                    assetValidation.AssetFuelRequestsList.ForEach(t => t.FuelRequestId = response.FuelRequestId);
                                    await ContextFactory.Current.GetDomain<AssetDomain>().SaveAssetFuelRequests(assetValidation.AssetFuelRequestsList);
                                }

                                if (assetValidation.InvalidAssetList.Count > 0)
                                {
                                    response.StatusCode = Utilities.Status.Warning;
                                    //if (assetValidation.InvalidAssetList.Count == 0)
                                    //{
                                    //    var duplicateAssets = fuelRequestViewModel.AssetFuelRequests.GroupBy(x => x.AssetId).Where(g => g.Count() > 1).Select(y => y.Key).ToList();
                                    //    response.StatusMessage = response.StatusMessage + Environment.NewLine + 
                                    //        string.Format(StringResources.Resource.errMessageDuplicateAssets, string.Join(", ", duplicateAssets));
                                    //}
                                    response.StatusMessage = response.StatusMessage + " " + string.Format(Resource.errMessageInvalidAsset, string.Join(", ", assetValidation.InvalidAssetList));
                                }
                            }
                            else
                            {
                                response.StatusCode = Utilities.Status.Failed;
                                response.StatusMessage = assetValidation.StatusMessage;

                                if (assetValidation.InvalidAssetList.Count > 0)
                                    response.StatusMessage = string.Format(Resource.errMessageInvalidAsset, string.Join(", ", assetValidation.InvalidAssetList));
                            }
                        }
                        else
                        {
                            var validationResponse = commonMethods.GetDateValidationResponse();
                            response.StatusCode = validationResponse.StatusCode;
                            response.StatusMessage = validationResponse.StatusMessage;
                        }
                    }
                    else
                    {
                        var errorMessageResponse = commonMethods.GetErrorMessage(ModelState);
                        response.StatusCode = errorMessageResponse.StatusCode;
                        response.StatusMessage = errorMessageResponse.StatusMessage;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("ZTRController", "CreateFuelRequest", ex.Message, ex);
                }

                return response;
            }
        }

        private FuelRequestViewModel ToFuelRequestModel(CreateFuelRequestInputViewModel fuelRequestViewModel)
        {
            var response = GetModelFromFile();
            if (fuelRequestViewModel.JobId > 0)
            {
                response.Job.JobId = fuelRequestViewModel.JobId;
            }

            response.FuelDeliveryDetails.StartDate = fuelRequestViewModel.DateNeeded;
            //response.FuelDetails.FuelQuantity.Quantity = fuelRequestViewModel.AssetFuelRequests.Sum(t => t.Quantity);
            return response;
        }

        //to work this correctly, you need to create json file at below path
        private FuelRequestViewModel GetModelFromFile()
        {
            var model = new FuelRequestViewModel();
            try
            {
                // read JSON directly from a file
                var fileLocation = HostingEnvironment.MapPath("~\\ZTR-poc.json");
                using (StreamReader file = File.OpenText(fileLocation))
                {
                    string json = file.ReadToEnd();
                    model = JsonConvert.DeserializeObject<FuelRequestViewModel>(json);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ZTRController", "GetModelFromFile", ex.Message, ex);
                return null;
            }

            return model;
        }
    }
}
