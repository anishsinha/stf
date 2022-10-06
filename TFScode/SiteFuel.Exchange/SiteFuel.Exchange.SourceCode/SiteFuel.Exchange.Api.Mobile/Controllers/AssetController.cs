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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;

namespace SiteFuel.Exchange.Api.Mobile.Controllers
{
#if DEBUG
    [ApiExplorerSettings(IgnoreApi = false)]
#else
    [ApiExplorerSettings(IgnoreApi = true)]
#endif

    [ValidateToken]
    public class AssetController : ApiBaseController
    {
        [HttpGet]
        [ApiLog(Enabled = true)]
        public async Task<List<AssetGridViewModel>> GetBuyerAssetList(int jobId, int buyerId)
        {
            var response = new List<AssetGridViewModel>();

                try
                {
                    var userContext = await GetUserContext(buyerId, CompanyType.Buyer);

                    AssetFilterViewModel assetFilter = new AssetFilterViewModel
                    {
                        JobId = jobId
                    };

                    response = await ContextFactory.Current.GetDomain<AssetDomain>().GetAssetGridAsync(userContext, assetFilter);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AssetController", "GetBuyerAssetList", ex.Message, ex);
                }

                return response;
        }

        [HttpGet]
        [ApiLog(Enabled = true)]
        public async Task<bool> IsValidAssetName(int assetId, string Name, int companyId)
        {
            bool result = false;
            try
            {
                result =  ContextFactory.Current.GetDomain<HelperDomain>().IsValidAssetName(assetId, Name, companyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AssetController", "IsValidAssetName", ex.Message, ex);
            }
            return result;
        }

        [HttpPost]
        [ApiLog(Enabled = true)]
        public async Task<StatusViewModel> CreateAndUpdateAsset(ApiCreateAssetViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel();
            try
            {
                    var assetNameCheck = await IsValidAssetName(viewModel.Id, viewModel.Name, viewModel.CompanyId);

                    if (assetNameCheck)
                    {
                        var userContext = await GetUserContext(viewModel.UserId, CompanyType.Buyer);
                        viewModel.CreatedDate = DateTimeOffset.Now;
         
                        AssetViewModel assetViewModel = new AssetViewModel()
                        {
                            Id = viewModel.Id,
                            CompanyId = viewModel.CompanyId,
                            CreatedDate = viewModel.CreatedDate,
                            Name = viewModel.Name,
                            AssetAdditionalDetail = viewModel.AssetAdditionalDetail,
                            FuelType = viewModel.FuelType,
                            UserId = viewModel.UserId,
                            JobId = viewModel.JobId,
                            Type = (int)AssetType.Asset
                        };

                        if (!string.IsNullOrEmpty(viewModel.imageFile))
                        {
                            assetViewModel.Image = new ImageViewModel { Data = Convert.FromBase64String(viewModel.imageFile) };
                        }

                        if (viewModel.Id > 0)
                        {
                            response = await ContextFactory.Current.GetDomain<AssetDomain>().UpdateAssetAsync(userContext, assetViewModel);
                        }
                        else
                        {
                            response = await ContextFactory.Current.GetDomain<AssetDomain>().SaveAssetAsync(assetViewModel);
                        }

                        if (response.StatusCode == Status.Success)
                        {
                            if (assetViewModel.JobId > 0)
                            {
                                 AssetJobAssignmentViewModel assignJobViewModel = new AssetJobAssignmentViewModel()
                                {
                                    Id = viewModel.JobxId,
                                    AssignedBy = assetViewModel.UserId,
                                    AssetId = assetViewModel.Id,
                                    AssignedDate = assetViewModel.CreatedDate,
                                    JobId = (int)assetViewModel.JobId
                                };
                                var res = await AssignAsset(assignJobViewModel);
                                if (res.StatusCode == Status.Success)
                                {
                                    response.StatusMessage = string.Format(Resource.errMessageAssetCreatedAndAssignedToJob, viewModel.Name, ((AssetType)assetViewModel.Type).GetDisplayName());
                                }
                                else
                                {
                                    response.StatusMessage = string.Format(Resource.errMessageAssetCreatedAndFailedToAssignedJob, viewModel.Name, ((AssetType)assetViewModel.Type).GetDisplayName());
                                }
                            }
                        }   
                    }
                    else
                    {
                       response.StatusCode = Status.Failed;
                       response.StatusMessage = string.Format(Resource.errMessageAssetNameAlreadyExists, viewModel.Name);
                    }
                
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AssetController", "CreateAndUpdateAsset", ex.Message, ex);
            }

            return response;
        }


        [HttpGet]
        [ApiLog(Enabled = true)]
        public async Task<AssetViewModel> GetAssetDetail(int id)
        {
            AssetViewModel response = new AssetViewModel();
            try
            {
                response = await ContextFactory.Current.GetDomain<AssetDomain>().GetAssetAsync(id);
                using (var tracer = new Tracer("AssetController", "GetAssetDetail"))
                {
                    response.MaxAllowedFileSize = SiteFuel.Exchange.Core.Utilities.AppSettings.MaxAllowedUploadFileSize;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AssetController", "GetAssetDetail", ex.Message, ex);
            }
            return response; 
        }


        [HttpPost]
        [ApiLog(Enabled = true)]
        public async Task<StatusViewModel> AssignAssets(AssignAssetsViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel();
            try
            {
                var userContext = await GetUserContext(viewModel.BuyerId, CompanyType.Buyer); //AssignedBy = BuyerID
                using (var tracer = new Tracer("AssetController", "AssignAssets"))
                {
                     response = await ContextFactory.Current.GetDomain<AssetDomain>().AssignAssetsToJobAsync(userContext, viewModel.JobId, viewModel.Assets);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AssetController", "AssignAssets", ex.Message, ex);
            }
            return response;
        }

        [HttpPost]
        [ApiLog(Enabled = true)]
        public async Task<StatusViewModel> AssignAsset(AssetJobAssignmentViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel();
            try
            {
                using (var tracer = new Tracer("AssetController", "AssignAsset"))
                {
                    var userContext = await GetUserContext(viewModel.AssignedBy, CompanyType.Buyer); //AssignedBy = BuyerID

                    var assetDomain = ContextFactory.Current.GetDomain<AssetDomain>();
                    if (viewModel.Id > 0)
                    {
                      await assetDomain.RemoveFromJobAsync(userContext, viewModel.Id, viewModel.JobId);
                    }
                    response = await assetDomain.AssignToJobAsync(userContext, viewModel);

                    return response;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AssetController", "AssignAsset", ex.Message, ex);
            }
            return response;
        }

        [HttpGet]
        [ApiLog(Enabled = true)]
        public async Task<StatusViewModel> UnAssignAsset(int jobXAssetsId, int buyerId)
        {
            StatusViewModel response = new StatusViewModel();
            try
            {
                using (var tracer = new Tracer("AssetController", "UnAssignAsset"))
                {
                    var userContext = await GetUserContext(buyerId, CompanyType.Buyer);
                    response = await ContextFactory.Current.GetDomain<AssetDomain>().RemoveFromJobAsync(userContext, jobXAssetsId);
                    return response;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AssetController", "UnAssignAsset", ex.Message, ex);
            }
            return response;
        }

        [HttpPost]
        [ApiLog(Enabled = true)]
        public async Task<StatusViewModel> DeleteAssets(DeleteAssetsViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel();
            try
            {
                using (var tracer = new Tracer("AssetController", "DeleteAssets"))
                {
                    response = await ContextFactory.Current.GetDomain<AssetDomain>().DeleteAssetAsync(viewModel.BuyerId, viewModel.Assets);
                    return response;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AssetController", "DeleteAssets", ex.Message, ex);
            }
            return response;
        }

        [HttpGet]
        public List<DropdownDisplayItem> GetSubContractors(int companyId)
        {
            var response = new List<DropdownDisplayItem>(); 
            try
            {
                response =  ContextFactory.Current.GetDomain<AssetDomain>().GetSubContractors(0, companyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AssetController", "GetSubContractors", ex.Message, ex);
            }
            return response;
        }

        [HttpGet]
        [ApiLog(Enabled = true)]
        public async Task<List<AssetListViewModel>> GetAssetList(int buyerId, int start, int length)
        {
            var response = new List<AssetListViewModel>();

            using (var tracer = new Tracer("AssetController", "GetAssetList"))
            {
                try
                {
                    var userContext = await GetUserContext(buyerId, CompanyType.Buyer);
                    var brandedCompanyId = GetBrandedSupplierCompId();
                    response = await ContextFactory.Current.GetDomain<AssetDomain>().GetAssetListMobileAsync(userContext, start, length, brandedCompanyId);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AssetController", "GetAssetList", ex.Message, ex);
                }

                return response;
            }
        }

        [HttpPost]
        [ApiLog(Enabled = true)]
        public async Task<TankVolumeAndUllageViewModel> GetTankVolumeAndUllage(TankVolumeAndUllageInputModel requestModel)
        {
            TankVolumeAndUllageViewModel response = new TankVolumeAndUllageViewModel();
            try
            {
                response = await new FreightServiceDomain().GetTankVolumeAndUllage(requestModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AssetController", "GetTankVolumeAndUllage", ex.Message, ex);
            }
            return response;
        }
    }
}