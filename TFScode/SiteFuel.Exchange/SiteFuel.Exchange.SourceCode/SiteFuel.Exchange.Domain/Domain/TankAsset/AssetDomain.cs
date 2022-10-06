using FileHelpers;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Infrastructure;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SiteFuel.Exchange.ViewModels.Asset;
using System.Text;
using SiteFuel.Exchange.ViewModels.Queue;
using Newtonsoft.Json;

namespace SiteFuel.Exchange.Domain
{
    public class AssetDomain : BaseDomain
    {
        public AssetDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public AssetDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task<StatusViewModel> SaveBulkAssetsAsync(string csvText, int userId, int companyId, int jobId = 0)
        {
            using (var tracer = new Tracer("AssetDomain", "SaveBulkAssetsAsync"))
            {
                var response = new StatusViewModel();
                try
                {
                    LogManager.Logger.WriteInfo("AssetDomain", "SaveBulkAssetsAsync", "\n\n[" + csvText + "]\n\n");
                    csvText = Regex.Replace(csvText.Trim(), @"^.*AssetName.*\n", string.Empty, RegexOptions.IgnoreCase);

                    int userIdToUse = userId;
                    if (jobId > 0)
                    {
                        var jobDetail = Context.DataContext.Jobs.SingleOrDefault(t => t.Id == jobId);
                        if (jobDetail != null)
                            userIdToUse = jobDetail.CreatedBy;
                    }

                    var engine = new FileHelperEngine<AssetCsvRecordViewModel>();
                    var csvAssetList = engine.ReadString(csvText).ToList();
                    csvAssetList = csvAssetList.Where(t => !string.IsNullOrWhiteSpace(t.Name)).ToList();
                    var productTypesList = new MasterDomain(this).GetProductTypes();
                    List<Asset> assets = new List<Asset>();
                    foreach (var item in csvAssetList)
                    {
                        var productType = productTypesList.FirstOrDefault(t => t.Name.ToLower() == item.FuelType.ToLower());
                        assets.Add(GetAssetObject(item, userId, companyId, productType));
                    }
                    response = await SaveAssetList(userIdToUse, jobId, assets);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AssetDomain", "SaveBulkAssetsAsync", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<StatusViewModel> SaveUpdatedAssetsAsync(int userId, AssetDuplicateGridViewModel updatedAssets)
        {
            using (var tracer = new Tracer("AssetDomain", "SaveBulkAssetsAsync"))
            {
                var response = new StatusViewModel();
                try
                {
                    var jobId = updatedAssets.JobId;
                    var assets = new List<Asset>();
                    var updatedList = updatedAssets.AssetDuplicates.Where(t => t.IsSelected).ToList();
                    if (updatedList.Count > 0)
                    {
                        var updatedIds = updatedList.Select(t => t.Id);
                        var duplicates = Context.DataContext.AssetDuplicates.Where(t => updatedIds.Contains(t.Id)).Select(t => t).ToList();
                        for (int index = 0; index < updatedList.Count; index++)
                        {
                            var toUpdate = duplicates[index];
                            toUpdate.Name = updatedList[index].Name;
                            var asset = toUpdate.ToAssetEntity();
                            if (!string.IsNullOrWhiteSpace(toUpdate.Subcontractor))
                            {
                                var subContractor = Context.DataContext.Subcontractors.FirstOrDefault(t => t.Name.ToLower() == toUpdate.Subcontractor.ToLower());
                                if (subContractor == null)
                                {
                                    subContractor = new Subcontractor() { Name = toUpdate.Subcontractor, IsActive = true, UpdatedBy = userId, UpdatedDate = DateTimeOffset.Now };
                                    Context.DataContext.Subcontractors.Add(subContractor);
                                }
                                AssetSubcontractor assetSubcontractor = new AssetSubcontractor() { SubcontractorId = subContractor.Id, AssignedBy = userId, AssignedDate = DateTimeOffset.Now, IsActive = true };
                                asset.AssetSubcontractors.Add(assetSubcontractor);
                            }
                            if (!string.IsNullOrWhiteSpace(toUpdate.Color))
                            {
                                AssetContractNumber assetContractNumber = new AssetContractNumber() { ContractNumber = toUpdate.Color, AddedBy = userId, AddedDate = DateTimeOffset.Now, IsActive = true };
                                asset.AssetContractNumbers.Add(assetContractNumber);
                            }
                            var fuelType = Context.DataContext.MstProductTypes.FirstOrDefault(t => t.Name.ToLower() == toUpdate.FuelType.ToLower());
                            if (fuelType != null)
                            {
                                asset.FuelType = fuelType.Id;
                            }

                            var licenseeState = Context.DataContext.MstStates.FirstOrDefault(t => t.Code.ToLower() == toUpdate.LicensePlateState.ToLower());
                            if (licenseeState != null)
                            {
                                asset.AssetAdditionalDetail.LicensePlateStateId = licenseeState.Id;
                            }

                            asset.IsActive = true;
                            asset.UpdatedBy = userId;
                            asset.UpdatedDate = DateTimeOffset.Now;
                            assets.Add(asset);
                            Context.DataContext.Entry(toUpdate).State = EntityState.Deleted;
                        }
                        response = await SaveAssetList(userId, jobId, assets);
                        await Context.CommitAsync();
                    }
                    else
                    {
                        response.StatusMessage = Resource.errMessageNoAssetSelectedToUpdate;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AssetDomain", "SaveUpdatedAssetsAsync", ex.Message, ex);
                }
                return response;
            }
        }

        public async Task<StatusViewModel> DeleteDuplicateAssetsAsync(int companyId, AssetDuplicateGridViewModel selectedAssets)
        {
            var response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var company = await Context.DataContext.Companies.Include(t => t.AssetDuplicates).FirstOrDefaultAsync(t => t.Id == companyId);
                    if (company != null)
                    {
                        var selectedIds = selectedAssets.AssetDuplicates.Where(t => t.IsSelected).Select(t => t.Id).ToList();
                        if (selectedIds.Count > 0)
                        {
                            var deleteList = Context.DataContext.AssetDuplicates.Where(t => selectedIds.Contains(t.Id)).ToList();
                            deleteList.ForEach(t => Context.DataContext.Entry(t).State = EntityState.Deleted);
                            await Context.CommitAsync();
                            transaction.Commit();

                            response.StatusCode = Status.Success;
                            response.StatusMessage = string.Format(Resource.errMessageAssetDeleteSuccess, ((AssetType.Asset).GetDisplayName()));
                        }
                        else
                        {
                            response.StatusMessage = Resource.errMessageNoAssetSelectedToDelete;
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("AssetDomain", "DeleteDuplicateAssetsAsync", ex.Message, ex);
                }
            }

            return response;
        }

        public async Task<AssetDuplicateGridViewModel> GetDuplicateAssets(int companyId)
        {
            var response = new AssetDuplicateGridViewModel();
            try
            {
                var company = await Context.DataContext.Companies.Include(t => t.AssetDuplicates).FirstOrDefaultAsync(t => t.Id == companyId);
                if (company != null)
                {
                    var duplicates = company.AssetDuplicates.ToList();
                    response.AssetDuplicates = duplicates.Select(t => t.ToViewModel()).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AssetDomain", "GetDuplicateAssets", ex.Message, ex);
            }
            return response;
        }

        public async Task<AssetViewModel> GetAssetAsync(int id = 0, int jobId = 0)
        {
            using (var tracer = new Tracer("AssetDomain", "GetAssetAsync"))
            {
                AssetViewModel response = new AssetViewModel();

                try
                {
                    var asset = await Context.DataContext.Assets.Include(t => t.AssetAdditionalDetail).Include(t => t.Image)
                                      .Include(t => t.Company).Include(t => t.MstProductType).FirstOrDefaultAsync(t => t.Id == id);
                    if (asset != null)
                    {
                        response = asset.ToViewModel(response);

                        if (asset.Type == (int)AssetType.Tank)
                        {
                            var tankResponse = await new FreightServiceDomain(this).GetTankDetails(id);
                            ToTankViewModel(response, tankResponse);
                        }

                        response.DisplayMode = PageDisplayMode.Edit;
                    }
                    else
                    {
                        SetMinMaxFillValues(response);
                    }

                    SetUoM(response, jobId, asset);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AssetDomain", "GetAssetAsync", ex.Message, ex);
                }

                return response;
            }
        }

        private void SetMinMaxFillValues(AssetViewModel response)
        {
            if (response.AssetAdditionalDetail.MinFill == null && response.AssetAdditionalDetail.MaxFill == null && response.AssetAdditionalDetail.ThresholdDeliveryRequest == null)
            {
                var keys = new List<string> { ApplicationConstants.KeyAppSettingTankMinFillPercent, ApplicationConstants.KeyAppSettingTankMaxFillPercent, ApplicationConstants.KeyAppSettingTankReorderLevel, ApplicationConstants.KeyAppSettingTankShouldGoLevel };
                var appSettings = Context.DataContext.MstAppSettings.Where(t => keys.Contains(t.Key) && t.IsActive).Select(t => new { t.Key, t.Value }).ToList();
                var minFillPercent = appSettings.FirstOrDefault(t => t.Key == ApplicationConstants.KeyAppSettingTankMinFillPercent);
                if (!string.IsNullOrWhiteSpace(minFillPercent.Value))
                {
                    response.AssetAdditionalDetail.MinFill = Convert.ToDecimal(minFillPercent.Value); //MUST GO
                }
                var maxFillPercent = appSettings.FirstOrDefault(t => t.Key == ApplicationConstants.KeyAppSettingTankMaxFillPercent);
                if (!string.IsNullOrWhiteSpace(maxFillPercent.Value))
                {
                    response.AssetAdditionalDetail.MaxFill = Convert.ToDecimal(maxFillPercent.Value);
                }
                var reorderLevel = appSettings.FirstOrDefault(t => t.Key == ApplicationConstants.KeyAppSettingTankReorderLevel);
                if (!string.IsNullOrWhiteSpace(reorderLevel.Value))
                {
                    response.AssetAdditionalDetail.ThresholdDeliveryRequest = Convert.ToDecimal(reorderLevel.Value); //SHOULD GO
                }

                var runOutLevel = appSettings.FirstOrDefault(t => t.Key == ApplicationConstants.KeyAppSettingTankShouldGoLevel);
                if (!string.IsNullOrWhiteSpace(reorderLevel.Value))
                {
                    response.AssetAdditionalDetail.RunOutLevel = Convert.ToDecimal(runOutLevel.Value); //COULD GO
                }
            }
        }

        private void SetUoM(AssetViewModel model, int jobId, Asset asset)
        {
            int localjobId = 0;
            if (jobId > 0)
            {
                localjobId = jobId;
            }
            else if (jobId == 0 && asset != null && asset.JobXAssets.Any(t => t.RemovedBy == null))
            {
                var job = asset.JobXAssets.FirstOrDefault(t => t.RemovedBy == null);
                if (job != null)
                    localjobId = job.JobId;
            }

            if (localjobId > 0)
            {
                var job = Context.DataContext.Jobs.Where(t => t.Id == localjobId).Select(t =>
                    new
                    {
                        t.UoM,
                        t.CompanyId,
                        t.InventoryDataCaptureType
                    }).FirstOrDefault();
                model.UoM = job.UoM;
                model.CompanyId = job.CompanyId;
                model.InventoryDataCaptureType = job.InventoryDataCaptureType;
            }
        }

        public async Task<DipTestMethod> GetDefaultDiptest(int companyId)
        {
            using (var tracer = new Tracer("AssetDomain", "GetDefaultDiptest"))
            {
                DipTestMethod response = DipTestMethod.Select;

                try
                {
                    var defaultDipTest = await Context.DataContext.OnboardingPreferences.Where(t => t.CompanyId == companyId && t.IsActive).Select(t => t.DipTestMethod).FirstOrDefaultAsync();
                    response = defaultDipTest;
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AssetDomain", "GetDefaultDiptest", ex.Message, ex);
                }

                return response;
            }
        }


        public List<int> AddDefaultTankAcceptDays()
        {
            List<int> tankAcceptDays = new List<int>();
            tankAcceptDays.Add((int)WeekDay.Monday);
            tankAcceptDays.Add((int)WeekDay.Tuesday);
            tankAcceptDays.Add((int)WeekDay.Wednesday);
            tankAcceptDays.Add((int)WeekDay.Thursday);
            tankAcceptDays.Add((int)WeekDay.Friday);
            tankAcceptDays.Add((int)WeekDay.Saturday);
            tankAcceptDays.Add((int)WeekDay.Sunday);
            return tankAcceptDays;
        }

        private void ToTankViewModel(AssetViewModel viewModel, TankAdditionalDetailViewModel tankViewModel)
        {
            if (tankViewModel != null)
            {
                viewModel.AssetAdditionalDetail.TankId = tankViewModel.TankId;
                viewModel.AssetAdditionalDetail.StorageId = tankViewModel.StorageId;
                viewModel.AssetAdditionalDetail.TankNumber = tankViewModel.TankNumber;
                viewModel.AssetAdditionalDetail.TankType = tankViewModel.TankType;
                viewModel.AssetAdditionalDetail.TankModelTypeId = tankViewModel.TankModelTypeId;
                viewModel.AssetAdditionalDetail.DipTestMethod = tankViewModel.DipTestMethod;
                viewModel.AssetAdditionalDetail.ThresholdDeliveryRequest = tankViewModel.ThresholdDeliveryRequest;
                viewModel.AssetAdditionalDetail.FillType = tankViewModel.FillType;
                viewModel.AssetAdditionalDetail.MinFill = tankViewModel.FillType == FillType.UoM ? tankViewModel.MinFill : tankViewModel.MinFillPercent;
                //viewModel.AssetAdditionalDetail.MinFillPercent = tankViewModel.FillType == FillType.Percent ? tankViewModel.MinFill : null;
                viewModel.AssetAdditionalDetail.MaxFill = tankViewModel.FillType == FillType.UoM ? tankViewModel.MaxFill : tankViewModel.MaxFillPercent;
                //viewModel.AssetAdditionalDetail.MaxFillPercent = tankViewModel.MaxFillPercent;
                viewModel.AssetAdditionalDetail.PhysicalPumpStop = tankViewModel.PhysicalPumpStop;
                viewModel.AssetAdditionalDetail.WaterLevel = tankViewModel.WaterLevel;
                viewModel.AssetAdditionalDetail.RunOutLevel = tankViewModel.RunOutLevel;
                viewModel.AssetAdditionalDetail.Manufacturer = tankViewModel.Manufacturer;
                viewModel.AssetAdditionalDetail.ManiFolded = tankViewModel.ManiFolded;
                viewModel.AssetAdditionalDetail.TankConstruction = tankViewModel.TankConstruction;
                viewModel.AssetAdditionalDetail.NotificationUponUsageSwing = tankViewModel.NotificationUponUsageSwing;
                viewModel.AssetAdditionalDetail.NotificationUponUsageSwingValue = tankViewModel.NotificationUponUsageSwingValue;
                viewModel.AssetAdditionalDetail.NotificationUponInventorySwing = tankViewModel.NotificationUponInventorySwing;
                viewModel.AssetAdditionalDetail.NotificationUponInventorySwingValue = tankViewModel.NotificationUponInventorySwingValue;
                if (!string.IsNullOrEmpty(tankViewModel.TankAcceptDelivery))
                    viewModel.AssetAdditionalDetail.TankAcceptDelivery = tankViewModel.TankAcceptDelivery.Split(',').Select(x => Convert.ToInt32(x)).Distinct().ToList();
                viewModel.AssetAdditionalDetail.TanksConnected = tankViewModel.TanksConnected;
                viewModel.AssetAdditionalDetail.TankSequence = tankViewModel.TankSequence;
                viewModel.AssetAdditionalDetail.PedigreeAssetDBID = tankViewModel.PedigreeAssetDBID;
                viewModel.AssetAdditionalDetail.SkyBitzRTUID = tankViewModel.SkyBitzRTUID;
                viewModel.AssetAdditionalDetail.IsStopATGPolling = tankViewModel.IsStopATGPolling;
                if (tankViewModel.DipTestMethod != null && tankViewModel.DipTestMethod.Value == DipTestMethod.Insight360)
                {
                    viewModel.AssetAdditionalDetail.Insight360TankId = tankViewModel.ExternalTankId;
                }
            }
        }

        public async Task<AssetDetailViewModel> GetAssetDetailAsync(int id = 0)
        {
            using (var tracer = new Tracer("AssetDomain", "GetAssetDetailAsync"))
            {
                AssetDetailViewModel response = new AssetDetailViewModel(Status.Success);

                try
                {
                    var asset = await Context.DataContext.Assets.Include(t => t.AssetAdditionalDetail).Include(t => t.Image)
                                      .Include(t => t.Company).Include(t => t.MstProductType).Include(t => t.JobXAssets).FirstOrDefaultAsync(t => t.Id == id);
                    if (asset != null)
                    {
                        response.Asset = asset.ToViewModel();

                        var jobAsset = asset.JobXAssets.Where(t => t.AssetId == id && t.RemovedDate == null).OrderByDescending(x => x.AssignedDate).FirstOrDefault();
                        if (jobAsset != null)
                        {
                            response.JobName = jobAsset.Job.Name;
                            response.DateAssigned = jobAsset.AssignedDate;
                            response.TotalDrops = jobAsset.AssetDrops.Count;
                            response.TotalFuel = jobAsset.AssetDrops.Sum(t => t.DroppedGallons).GetPreciseValue(2);
                            response.TotalFuelCost = jobAsset.AssetDrops.Sum(t => (t.Invoice.InvoiceXBolDetails.Select(t1 => t1.InvoiceFtlDetail.PricePerGallon).First() * t.DroppedGallons)).GetPreciseValue(2);
                            response.Currency = jobAsset.Job.Currency;
                            response.UoM = jobAsset.Job.UoM;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AssetDomain", "GetAssetDetailAsync", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<StatusViewModel> SaveAssetAsync(AssetViewModel viewModel)
        {
            using (var tracer = new Tracer("AssetDomain", "SaveAssetAsync"))
            {
                StatusViewModel response = new StatusViewModel();
                bool freightResponse = true;
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (viewModel.AssetAdditionalDetail.DipTestMethod == DipTestMethod.Manual && viewModel.AssetAdditionalDetail.TankModelTypeId == null)
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = string.Format(Resource.errorTankMakeModelRequiredForManual);
                            return response;
                        }
                        
                        var user = Context.DataContext.Users.FirstOrDefault(t => t.Id == viewModel.UserId);
                        if (user != null)
                        {
                            // validate pedigree asset DB ID already exists for tank - Pedigree Dip
                            if (!string.IsNullOrWhiteSpace(viewModel.AssetAdditionalDetail.PedigreeAssetDBID))
                            {
                                var existingPedigreeAssetDBIdTank = user.Company.Assets.Where(t => t.CompanyId == user.Company.Id && t.IsActive && (t.AssetAdditionalDetail.PedigreeAssetDBId != null && t.AssetAdditionalDetail.PedigreeAssetDBId != "" && t.AssetAdditionalDetail.PedigreeAssetDBId.ToLower() == viewModel.AssetAdditionalDetail.PedigreeAssetDBID.ToLower())).FirstOrDefault();
                                if (existingPedigreeAssetDBIdTank != null)
                                {
                                    response.StatusCode = Status.Failed;
                                    response.StatusMessage = string.Format(Resource.errMessagePedigreeAssetDBIDAlreadyExists, existingPedigreeAssetDBIdTank.Name);
                                    return response;
                                }
                            }

                            if (viewModel.AssetAdditionalDetail.DipTestMethod == DipTestMethod.VeederRoot)
                            {
                                var existingVeederRootTank = Context.DataContext.Assets
                                                           .Where(t => t.CompanyId == user.Company.Id && t.IsActive && (t.AssetAdditionalDetail.DipTestMethod != null &&
                                                                       t.AssetAdditionalDetail.DipTestMethod.Value == (int)DipTestMethod.VeederRoot) &&
                                                                       (
                                                                        t.AssetAdditionalDetail.ExternalTankId != null && t.AssetAdditionalDetail.ExternalTankId != "" &&
                                                                        t.AssetAdditionalDetail.ExternalTankId.ToLower() == viewModel.AssetAdditionalDetail.VeederRootTankID.ToLower() &&
                                                                        t.AssetAdditionalDetail.VeederRootIPAddress == viewModel.AssetAdditionalDetail.VeederRootIPAddress &&
                                                                        t.AssetAdditionalDetail.Port == viewModel.AssetAdditionalDetail.Port
                                                                       )
                                                                )
                                                           .Select(t => new { t.Name, t.AssetAdditionalDetail.ExternalTankId, t.AssetAdditionalDetail.VeederRootIPAddress, t.AssetAdditionalDetail.Port })
                                                           .FirstOrDefault();

                                if (existingVeederRootTank != null)
                                {
                                    response.StatusCode = Status.Failed;
                                    response.StatusMessage = string.Format(Resource.errMessageVeederRootTankIDAlreadyExists, existingVeederRootTank.Name, existingVeederRootTank.ExternalTankId, existingVeederRootTank.VeederRootIPAddress, existingVeederRootTank.Port);
                                    return response;
                                }
                            }

                            var asset = viewModel.ToEntity();
                            user.Company.Assets.Add(asset);
                           

                            if (asset.Type == (int)AssetType.Tank)
                            {
                                viewModel.AssetAdditionalDetail.AssetId = asset.Id;
                                freightResponse = await new FreightServiceDomain().SaveTankDetails(viewModel);
                            }

                            if (freightResponse)
                            {
                                await Context.CommitAsync();
                                transaction.Commit();

                                viewModel = asset.ToViewModel(viewModel);
                                response.EntityId = asset.Id;
                                response.StatusCode = Status.Success;
                                response.StatusMessage = string.Format(Resource.errMessageAssetCreatedSuccess, asset.Name, ((AssetType)viewModel.Type).GetDisplayName());
                                return response;
                            }
                            else
                            {
                                transaction.Rollback();
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format(Resource.errMessageAssetCreatedFailed, ((AssetType)viewModel.Type).GetDisplayName());
                                return response;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        LogManager.Logger.WriteException("AssetDomain", "SaveAssetAsync", ex.Message, ex);
                    }
                }

                return response;
            }
        }


        public async Task<StatusViewModel> SaveTruckDetailsAsync(UserContext userContext, TruckDetailViewModel viewModel)
        {
            using (var tracer = new Tracer("AssetDomain", "SaveTruckDetailsAsync"))
            {
                StatusViewModel response = new StatusViewModel();
                if (viewModel != null)
                {
                    if (string.IsNullOrEmpty(viewModel.Id))
                        response = await CreateTruckAsync(userContext, viewModel, response);
                    else
                        response = await UpdateTruckAsync(userContext, viewModel, response);
                }
                return response;
            }
        }

        private static async Task<StatusViewModel> CreateTruckAsync(UserContext userContext, TruckDetailViewModel viewModel, StatusViewModel response)
        {
            viewModel.TfxCreatedBy = userContext.Id;
            viewModel.TfxCompanyId = userContext.CompanyId;
            response = await new FreightServiceDomain().SaveTruckDetails(viewModel);
            if (response != null && response.StatusCode == Status.Success)
                response.StatusMessage = string.Format($"{viewModel.TruckId} trailer created successfully");
            return response;
        }

        private static async Task<StatusViewModel> UpdateTruckAsync(UserContext userContext, TruckDetailViewModel viewModel, StatusViewModel response)
        {
            viewModel.TfxCreatedBy = userContext.Id;
            viewModel.TfxCompanyId = userContext.CompanyId;
            var freightDomain = new FreightServiceDomain();
            response = await freightDomain.UpdateTruckDetails(viewModel);
            if (response != null && response.StatusCode == Status.Success)
            {
                response.StatusMessage = string.Format($"{viewModel.TruckId} trailer updated successfully"); 
            }
            return response;
        }

        public async Task<StatusViewModel> SavejobXAssetAsync(UserContext userContext, AssetViewModel viewModel, int jobId)
        {
            using (var tracer = new Tracer("AssetDomain", "SavejobXAssetAsync"))
            {
                StatusViewModel response = new StatusViewModel();
                bool freightResponse = true;
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        bool isAssetTrackingEnabled = false;
                        var user = Context.DataContext.Users.FirstOrDefault(t => t.Id == viewModel.UserId);
                        if (user != null)
                        {
                            bool isValidAssetName = new HelperDomain(this).IsValidAssetName(viewModel.Id, viewModel.Name, user.CompanyId.Value, viewModel.Type, viewModel.JobId);
                            if (!isValidAssetName)
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format(Resource.errMessageAssetOrTankNameAlreadyExists, ((AssetType)viewModel.Type).GetDisplayName());
                                return response;
                            }


                            // validate pedigree asset DB ID already exists for tank - Pedigree Dip
                            if (!string.IsNullOrWhiteSpace(viewModel.AssetAdditionalDetail.PedigreeAssetDBID))
                            {
                                var existingPedigreeAssetDBIdTank = user.Company.Assets.Where(t => t.CompanyId == user.Company.Id && t.IsActive && t.AssetAdditionalDetail != null && (t.AssetAdditionalDetail.PedigreeAssetDBId != null && t.AssetAdditionalDetail.PedigreeAssetDBId != "" && t.AssetAdditionalDetail.PedigreeAssetDBId.ToLower() == viewModel.AssetAdditionalDetail.PedigreeAssetDBID.ToLower())).FirstOrDefault();
                                if (existingPedigreeAssetDBIdTank != null)
                                {
                                    response.StatusCode = Status.Failed;
                                    response.StatusMessage = string.Format(Resource.errMessagePedigreeAssetDBIDAlreadyExists, existingPedigreeAssetDBIdTank.Name);
                                    return response;
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(viewModel.AssetAdditionalDetail.SkyBitzRTUID))
                            {
                                var existingSkyBitzRTUID = user.Company.Assets.Where(t => t.CompanyId == user.Company.Id && t.IsActive && t.AssetAdditionalDetail != null && (t.AssetAdditionalDetail.SkyBitzRTUID != null && t.AssetAdditionalDetail.SkyBitzRTUID != "" && t.AssetAdditionalDetail.SkyBitzRTUID.ToLower() == viewModel.AssetAdditionalDetail.SkyBitzRTUID.ToLower())).FirstOrDefault();
                                if (existingSkyBitzRTUID != null)
                                {
                                    response.StatusCode = Status.Failed;
                                    response.StatusMessage = string.Format(Resource.errMessageSkyBitzRTUIDAlreadyExists, existingSkyBitzRTUID.Name);
                                    return response;
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(viewModel.AssetAdditionalDetail.Insight360TankId))
                            {

                                var existingInsightTankId = user.Company.Assets.Where(t => t.CompanyId == user.Company.Id && t.IsActive && t.AssetAdditionalDetail != null && (t.AssetAdditionalDetail.DipTestMethod.HasValue && t.AssetAdditionalDetail.DipTestMethod.Value == (int)DipTestMethod.Insight360)&&(t.AssetAdditionalDetail.ExternalTankId != null && t.AssetAdditionalDetail.ExternalTankId != "" && t.AssetAdditionalDetail.ExternalTankId.ToLower() == viewModel.AssetAdditionalDetail.Insight360TankId.ToLower())).FirstOrDefault();
                                if (existingInsightTankId != null)
                                {
                                    response.StatusCode = Status.Failed;
                                    response.StatusMessage = string.Format(Resource.errMessageInsight360TankIDAlreadyExists, existingInsightTankId.Name);
                                    return response;
                                }
                            }
                            if (viewModel.AssetAdditionalDetail.DipTestMethod == DipTestMethod.VeederRoot)
                            {
                                var existingVeederRootTank = user.Company.Assets
                                                          .Where(t => t.IsActive && (t.AssetAdditionalDetail.DipTestMethod != null &&
                                                                      t.AssetAdditionalDetail.DipTestMethod.Value == (int)DipTestMethod.VeederRoot) &&
                                                                      (
                                                                       t.AssetAdditionalDetail.ExternalTankId != null && t.AssetAdditionalDetail.ExternalTankId != "" &&
                                                                       t.AssetAdditionalDetail.ExternalTankId.ToLower() == viewModel.AssetAdditionalDetail.VeederRootTankID.ToLower() &&
                                                                       t.AssetAdditionalDetail.VeederRootIPAddress == viewModel.AssetAdditionalDetail.VeederRootIPAddress &&
                                                                       t.AssetAdditionalDetail.Port == viewModel.AssetAdditionalDetail.Port
                                                                      )
                                                               )
                                                          .Select(t => new { t.Name, t.AssetAdditionalDetail.ExternalTankId, t.AssetAdditionalDetail.VeederRootIPAddress, t.AssetAdditionalDetail.Port })
                                                          .FirstOrDefault();

                                if (existingVeederRootTank != null)
                                {
                                    response.StatusCode = Status.Failed;
                                    response.StatusMessage = string.Format(Resource.errMessageVeederRootTankIDAlreadyExists, existingVeederRootTank.Name, existingVeederRootTank.ExternalTankId, existingVeederRootTank.VeederRootIPAddress, existingVeederRootTank.Port);
                                    return response;
                                }
                            }

                            var asset = viewModel.ToEntity();

                            user.Company.Assets.Add(asset);
                            await Context.CommitAsync();

                            Context.DataContext.JobXAssets.Add(new JobXAsset() { AssetId = asset.Id, JobId = jobId, AssignedBy = userContext.Id, AssignedDate = DateTime.Now });
                            var job = Context.DataContext.Jobs.FirstOrDefault(t => t.Id == jobId);
                            if (!job.JobBudget.IsAssetTracked && viewModel.Type == (int)AssetType.Asset)
                            {
                                job.JobBudget.IsAssetTracked = true;
                                isAssetTrackingEnabled = true;
                            }

                            if (!job.JobBudget.IsTankAvailable && viewModel.Type == (int)AssetType.Tank)
                            {
                                job.JobBudget.IsTankAvailable = true;
                            }
                            await Context.CommitAsync();

                            if (viewModel.Type == (int)AssetType.Tank)
                            {
                                viewModel.AssetAdditionalDetail.AssetId = asset.Id;
                                viewModel.JobDisplayId = job.DisplayJobID;
                                freightResponse = await new FreightServiceDomain().SaveTankDetails(viewModel);
                            }

                            if (freightResponse)
                            {
                                transaction.Commit();

                                viewModel = asset.ToViewModel(viewModel);

                                if (isAssetTrackingEnabled)
                                {
                                    await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetAssetTrackingEnableDisableNewsfeed(userContext, job, isAssetTrackingEnabled);
                                }
                                await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetAssetAddedRemovedNewsfeed(userContext, job, 1, true);
                                response.EntityId = asset.Id;
                                response.CustomerCompanyId = job.CompanyId;
                                response.StatusCode = Status.Success;
                                response.StatusMessage = isAssetTrackingEnabled ? string.Format(Resource.errMessageAssetCreatedAndAssignedSuccess, asset.Name, job.Name, ((AssetType)viewModel.Type).GetDisplayName()) : string.Format(Resource.errMessageAssetCreatedSuccess, asset.Name, ((AssetType)viewModel.Type).GetDisplayName());
                            }
                            else
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format(Resource.errMessageAssetCreatedFailed, ((AssetType)viewModel.Type).GetDisplayName());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        LogManager.Logger.WriteException("AssetDomain", "SavejobXAssetAsync", ex.Message, ex);
                    }
                }

                return response;
            }
        }

        public async Task<StatusViewModel> SaveMobileAssetAsync(AssetViewModel viewModel, int jobId)
        {
            using (var tracer = new Tracer("AssetDomain", "SaveMobileAssetAsync"))
            {
                StatusViewModel response = new StatusViewModel();
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        // From mobile API, they are sending state-name instead of state-id
                        if (viewModel.AssetAdditionalDetail.LicensePlateStateId == null &&
                            !string.IsNullOrWhiteSpace(viewModel.AssetAdditionalDetail.LicensePlateState))
                        {
                            var state = await ContextFactory.Current.GetDomain<HelperDomain>().GetState(viewModel.AssetAdditionalDetail.LicensePlateState);
                            if (state != null && state.Id > 0)
                                viewModel.AssetAdditionalDetail.LicensePlateStateId = state.Id;
                        }
                        if (viewModel.Image != null)
                            await viewModel.Image.UploadImageToAzureBlobService(ApplicationConstants.AssetDropImageFileNamePrefix, BlobContainerType.JobFilesUpload);

                        var asset = viewModel.ToEntity();
                        var job = Context.DataContext.Jobs.Include(t => t.Company.Assets).FirstOrDefault(t => t.Id == jobId);
                        if (job != null)
                        {
                            if (job.Company.Assets.Any(t => t.Name.Trim().ToLower() == asset.Name.Trim().ToLower()))
                            {
                                response.StatusMessage = Resource.errMessageAssetNameAlreadyExists;
                            }
                            else
                            {
                                job.Company.Assets.Add(asset);
                                await Context.CommitAsync();

                                transaction.Commit();
                                viewModel = asset.ToViewModel(viewModel);

                                response.StatusCode = Status.Success;
                                response.StatusMessage = Resource.errMessageSuccess;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        LogManager.Logger.WriteException("AssetDomain", "SaveMobileAssetAsync", ex.Message, ex);
                    }
                }

                return response;
            }
        }

        public async Task<List<string>> GetAllTankTypeNameForDipChart(int companyId, string searchValue)
        {
            var response = new List<string>();
            using (var tracer = new Tracer("AssetDomain", "GetAllTankTypeNameForDipChart"))
            {
                response = await new FreightServiceDomain().GetAllTankTypeNameForDipChart(companyId, searchValue);
            }
            return response;
        }

        public async Task<StatusViewModel> UpdateAssetAsync(UserContext userContext, AssetViewModel viewModel)
        {
            using (var tracer = new Tracer("AssetDomain", "UpdateAssetAsync"))
            {
                StatusViewModel response = new StatusViewModel();
                bool freightResponse = true;
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (viewModel.AssetAdditionalDetail.DipTestMethod == DipTestMethod.Manual && viewModel.AssetAdditionalDetail.TankModelTypeId == null)
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = string.Format(Resource.errorTankMakeModelRequiredForManual);
                            return response;
                        }
                        var asset = Context.DataContext.Assets.FirstOrDefault(t => t.Id == viewModel.Id);
                        if (asset != null)
                        {
                            // validate pedigree asset DB ID already exists for tank - Pedigree Dip
                            if (!string.IsNullOrWhiteSpace(viewModel.AssetAdditionalDetail.PedigreeAssetDBID))
                            {
                                var existingPedigreeAssetDBIdTank = Context.DataContext.Assets.Where(t => t.Id != asset.Id && t.IsActive && t.CompanyId == asset.CompanyId && (t.AssetAdditionalDetail.PedigreeAssetDBId != null && t.AssetAdditionalDetail.PedigreeAssetDBId != "" && t.AssetAdditionalDetail.PedigreeAssetDBId.ToLower() == viewModel.AssetAdditionalDetail.PedigreeAssetDBID.ToLower())).FirstOrDefault();
                                if (existingPedigreeAssetDBIdTank != null)
                                {
                                    response.StatusCode = Status.Failed;
                                    response.StatusMessage = string.Format(Resource.errMessagePedigreeAssetDBIDAlreadyExists, existingPedigreeAssetDBIdTank.Name);
                                    return response;
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(viewModel.AssetAdditionalDetail.SkyBitzRTUID))
                            {
                                var existingSkyBitzRTUID = Context.DataContext.Assets.Where(t => t.Id != asset.Id && t.IsActive && t.CompanyId == asset.CompanyId && (t.AssetAdditionalDetail.SkyBitzRTUID != null && t.AssetAdditionalDetail.SkyBitzRTUID != "" && t.AssetAdditionalDetail.SkyBitzRTUID.ToLower() == viewModel.AssetAdditionalDetail.SkyBitzRTUID.ToLower())).FirstOrDefault();
                                if (existingSkyBitzRTUID != null)
                                {
                                    response.StatusCode = Status.Failed;
                                    response.StatusMessage = string.Format(Resource.errMessageSkyBitzRTUIDAlreadyExists, existingSkyBitzRTUID.Name);
                                    return response;
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(viewModel.AssetAdditionalDetail.Insight360TankId))
                            {

                                var existingInsightTankId = Context.DataContext.Assets.Where(t => t.Id != asset.Id && t.IsActive && t.CompanyId == asset.CompanyId && (t.AssetAdditionalDetail.DipTestMethod != null && t.AssetAdditionalDetail.DipTestMethod.Value == (int)DipTestMethod.Insight360) && (t.AssetAdditionalDetail.ExternalTankId != null && t.AssetAdditionalDetail.ExternalTankId != "" && t.AssetAdditionalDetail.ExternalTankId.ToLower() == viewModel.AssetAdditionalDetail.Insight360TankId.ToLower())).FirstOrDefault();
                                if (existingInsightTankId != null)
                                {
                                    response.StatusCode = Status.Failed;
                                    response.StatusMessage = string.Format(Resource.errMessageInsight360TankIDAlreadyExists, existingInsightTankId.Name);
                                    return response;
                                }
                            }
                            if (viewModel.AssetAdditionalDetail.DipTestMethod == DipTestMethod.VeederRoot)
                            {
                                var existingVeederRootTank = Context.DataContext.Assets
                                                           .Where(t => t.Id != asset.Id && t.IsActive && t.CompanyId == asset.CompanyId && (t.AssetAdditionalDetail.DipTestMethod != null &&
                                                                       t.AssetAdditionalDetail.DipTestMethod.Value == (int)DipTestMethod.VeederRoot) &&
                                                                       (
                                                                        t.AssetAdditionalDetail.ExternalTankId != null && t.AssetAdditionalDetail.ExternalTankId != "" &&
                                                                        t.AssetAdditionalDetail.ExternalTankId.ToLower() == viewModel.AssetAdditionalDetail.VeederRootTankID.ToLower() &&
                                                                        t.AssetAdditionalDetail.VeederRootIPAddress == viewModel.AssetAdditionalDetail.VeederRootIPAddress &&
                                                                        t.AssetAdditionalDetail.Port == viewModel.AssetAdditionalDetail.Port
                                                                       )
                                                                )
                                                           .Select(t => new { t.Name, t.AssetAdditionalDetail.ExternalTankId, t.AssetAdditionalDetail.VeederRootIPAddress, t.AssetAdditionalDetail.Port })
                                                           .FirstOrDefault();

                                if (existingVeederRootTank != null)
                                {
                                    response.StatusCode = Status.Failed;
                                    response.StatusMessage = string.Format(Resource.errMessageVeederRootTankIDAlreadyExists, existingVeederRootTank.Name, existingVeederRootTank.ExternalTankId, existingVeederRootTank.VeederRootIPAddress, existingVeederRootTank.Port);
                                    return response;
                                }
                            }

                            asset = viewModel.ToEntity(asset);
                            if (viewModel.Image != null && viewModel.Image.IsRemoved)
                            {
                                var image = await Context.DataContext.Images.FirstOrDefaultAsync(t => t.Id == viewModel.Image.Id);
                                if (image != null)
                                {
                                    asset.Image = null;
                                    Context.DataContext.Images.Remove(image);
                                }
                            }

                         

                            if (asset.Type == (int)AssetType.Tank)
                            {
                                var jobAsset = asset.JobXAssets.FirstOrDefault(t => t.RemovedBy == null);
                                if (jobAsset != null)
                                    viewModel.JobDisplayId = jobAsset.Job.DisplayJobID;
                                freightResponse = await new FreightServiceDomain().UpdateTankDetails(viewModel);
                            }

                            if (freightResponse)
                            {
                                Context.DataContext.Entry(asset).State = EntityState.Modified;
                                await Context.CommitAsync();
                                transaction.Commit();

                                if (asset.JobXAssets.Any(t => t.RemovedBy == null))
                                {
                                    await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetAssetModifiedNewsfeed(userContext, asset.JobXAssets.First(t => t.RemovedBy == null).Job, 1);
                                }

                                response.StatusCode = Status.Success;
                                response.StatusMessage = string.Format(Resource.errMessageAssetUpdateSuccess, asset.Name, ((AssetType)viewModel.Type).GetDisplayName());
                                return response;
                            }
                            else
                            {
                                transaction.Rollback();
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format(Resource.errMessageAssetUpdatedFailed, ((AssetType)viewModel.Type).GetDisplayName());
                                return response;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        LogManager.Logger.WriteException("AssetDomain", "UpdateAssetAsync", ex.Message, ex);
                    }
                }

                return response;
            }
        }

        public async Task<List<AssetViewModel>> GetTpoAssetGridAsync(UserContext user, int jobId, int type)
        {
            using (var tracer = new Tracer("AssetDomain", "GetTpoAssetGridAsync"))
            {
                var response = new List<AssetViewModel>();
                try
                {
                    var jobDetails = Context.DataContext.Jobs.Where(t => t.Id == jobId)
                                    .Select(t => new { t.CompanyId, t.Company.IsActive, CreatedCompanyId = t.Company.Orders1.Select(t1 => t1.AcceptedCompanyId).FirstOrDefault() }).SingleOrDefault();

                    if (jobDetails != null)
                    {
                        var allAssets = await Context.DataContext.Assets.Where(t => t.IsActive && t.Type == type && t.CompanyId == jobDetails.CompanyId && (t.Type == (int)AssetType.Asset || t.JobXAssets.Any(t1 => t1.JobId == jobId && t1.RemovedBy == null && t1.RemovedDate == null)))
                                        .Select(t => new
                                        {
                                            Asset = new
                                            {
                                                t.Id,
                                                t.Name,
                                                t.ImageId,
                                                FilePath = t.Image == null ? string.Empty : t.Image.FilePath,
                                                t.Type,
                                                TFXFuelType = t.MstTfxProduct.Name,
                                            },
                                            AssetAdditionalDetail = new
                                            {
                                                t.AssetAdditionalDetail.Make,
                                                t.AssetAdditionalDetail.Model,
                                                t.AssetAdditionalDetail.Year,
                                                t.AssetAdditionalDetail.AssetClass,
                                                t.AssetAdditionalDetail.FuelCapacity,
                                                t.AssetAdditionalDetail.TankType,
                                                t.AssetAdditionalDetail.ThresholdDeliveryRequest,
                                                t.AssetAdditionalDetail.DipTestMethod,
                                                t.AssetAdditionalDetail.VehicleId,
                                                t.AssetAdditionalDetail.Vendor
                                            },
                                            FuelType = t.MstProductType == null ? null : new
                                            {
                                                t.MstProductType.Name
                                            },
                                            IsAssetAssigned = t.JobXAssets.Any(t1 => t1.JobId == jobId && t1.RemovedBy == null)
                                        }).ToListAsync();
                        bool isEditAllowed = jobDetails.CreatedCompanyId == user.CompanyId;

                        var tankAdditionalList = new List<TankDetailViewModel>();

                        if (type == (int)AssetType.Tank)
                        {
                            var tankIds = allAssets.Select(t => t.Asset.Id).ToList();
                            tankAdditionalList = await new FreightServiceDomain(this).GetTankList(tankIds);
                        }
                        foreach (var item in allAssets)
                        {
                            var assetItem = new AssetViewModel();
                            assetItem.Id = item.Asset.Id;
                            assetItem.Name = item.Asset.Name;
                            var assetAdditionalDetail = new AssetAdditionalDetailViewModel();
                            assetAdditionalDetail.Make = string.IsNullOrWhiteSpace(item.AssetAdditionalDetail.Make) ? null : item.AssetAdditionalDetail.Make;
                            assetAdditionalDetail.Model = string.IsNullOrWhiteSpace(item.AssetAdditionalDetail.Model) ? null : item.AssetAdditionalDetail.Model;
                            assetAdditionalDetail.Year = string.IsNullOrWhiteSpace(item.AssetAdditionalDetail.Year) ? null : item.AssetAdditionalDetail.Year;
                            assetAdditionalDetail.Class = string.IsNullOrWhiteSpace(item.AssetAdditionalDetail.AssetClass) ? null : item.AssetAdditionalDetail.AssetClass;
                            assetAdditionalDetail.FuelCapacity = item.AssetAdditionalDetail.FuelCapacity;
                            assetAdditionalDetail.TankId = !string.IsNullOrWhiteSpace(item.AssetAdditionalDetail.VehicleId) ? item.AssetAdditionalDetail.VehicleId : string.Empty;
                            assetAdditionalDetail.VehicleId = !string.IsNullOrWhiteSpace(item.AssetAdditionalDetail.VehicleId) ? item.AssetAdditionalDetail.VehicleId : string.Empty;
                            assetAdditionalDetail.StorageId = !string.IsNullOrWhiteSpace(item.AssetAdditionalDetail.Vendor) ? item.AssetAdditionalDetail.Vendor : string.Empty;
                            assetAdditionalDetail.Vendor = !string.IsNullOrWhiteSpace(item.AssetAdditionalDetail.Vendor) ? item.AssetAdditionalDetail.Vendor : string.Empty;
                            bool isTankDetailFound = false;
                            if (item.Asset.Type == (int)AssetType.Tank && tankAdditionalList != null)
                            {
                                var tankViewModel = tankAdditionalList.FirstOrDefault(t => t.AssetId == item.Asset.Id);
                                if (tankViewModel != null)
                                {
                                    isTankDetailFound = true;
                                    assetAdditionalDetail.TankTypeName = !string.IsNullOrEmpty(tankViewModel.TankMakeModel) ? tankViewModel.TankMakeModel : Resource.lblHyphen;
                                    assetAdditionalDetail.Threshold = tankViewModel.ThresholdDeliveryRequest.HasValue ? $"{tankViewModel.ThresholdDeliveryRequest.Value.GetPreciseValue(2)}{Resource.constSymbolPercent}" : Resource.lblHyphen;
                                    assetAdditionalDetail.DipTestMethodName = tankViewModel.DipTestMethod.HasValue ? ((DipTestMethod)tankViewModel.DipTestMethod).GetDisplayName() : Resource.lblHyphen;
                                    assetAdditionalDetail.TankChart = !string.IsNullOrEmpty(tankViewModel.TankChart) ? tankViewModel.TankChart : Resource.lblHyphen;
                                    if (tankViewModel.TanksConnected != null && tankViewModel.TanksConnected.Any())
                                        assetAdditionalDetail.TanksConnectedNames = GetTanksName(tankViewModel.TanksConnected, tankAdditionalList);
                                    assetAdditionalDetail.TankSequence = tankViewModel.TankSequence;
                                    assetAdditionalDetail.PedigreeAssetDBID = tankViewModel.PedigreeAssetDBID;
                                    assetAdditionalDetail.IsStopATGPolling = tankViewModel.IsStopATGPolling;
                                }
                            }

                            if (item.Asset.Type == (int)AssetType.Tank && !isTankDetailFound)
                            {
                                assetAdditionalDetail.TankTypeName = Resource.lblHyphen;
                                assetAdditionalDetail.Threshold = Resource.lblHyphen;
                                assetAdditionalDetail.DipTestMethodName = Resource.lblHyphen;
                                assetAdditionalDetail.TankChart = Resource.lblHyphen;
                            }

                            assetItem.AssetAdditionalDetail = assetAdditionalDetail;

                            assetItem.Image = item.Asset.ImageId == null ? new ImageViewModel() : new ImageViewModel() { Id = item.Asset.ImageId ?? 0, FilePath = item.Asset.FilePath };
                            if (assetItem.Image.Id > 0)
                            {
                                assetItem.Image.FilePath = assetItem.Image.GetAzureFilePath(BlobContainerType.JobFilesUpload);
                            }

                            assetItem.FuelType = item.FuelType == null ? new AssetFuelTypeViewModel() : new AssetFuelTypeViewModel() { Name = item.FuelType.Name };
                            assetItem.TFXFuelType = item.Asset.TFXFuelType;
                            assetItem.IsAssetAssigned = item.IsAssetAssigned;
                            assetItem.JobId = item.IsAssetAssigned ? jobId : (int?)null;
                            assetItem.DisplayMode = isEditAllowed ? PageDisplayMode.Edit : PageDisplayMode.Details;
                            response.Add(assetItem);
                        }
                        if (response != null && response.Any())
                        {
                            response = response.OrderBy(t1 => t1.AssetAdditionalDetail.TankSequence == null || t1.AssetAdditionalDetail.TankSequence == 0 ? 99999 : t1.AssetAdditionalDetail.TankSequence).ToList();
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AssetDomain", "GetTpoAssetGridAsync", ex.Message, ex);
                }
                return response;
            }
        }

        public async Task<List<AssetViewModel>> GetAssetInfoGridAsync(int userId, int jobId, int type)
        {
            using (var tracer = new Tracer("AssetDomain", "GetAssetInfoGridAsync"))
            {
                var response = new List<AssetViewModel>();
                try
                {
                    var jobDetails = Context.DataContext.Jobs.Where(t => t.Id == jobId).Select(t => new { t.CompanyId }).SingleOrDefault();
                    if (jobDetails != null)
                    {
                        var assignedAssets = await Context.DataContext.JobXAssets.Where(t => t.JobId == jobId
                                                && t.Asset.Type == type
                                                && t.Asset.CompanyId == jobDetails.CompanyId
                                                && t.RemovedBy == null && t.RemovedDate == null)
                                                .Select(t => new
                                                {
                                                    Asset = new
                                                    {
                                                        t.Asset.Id,
                                                        t.Asset.Name,
                                                        t.Asset.ImageId,
                                                        FilePath = t.Asset.Image == null ? string.Empty : t.Asset.Image.FilePath,
                                                        t.Asset.Type,
                                                        TFXFuelType = t.Asset.MstTfxProduct.Name,
                                                    },
                                                    AssetAdditionalDetail = new
                                                    {
                                                        t.Asset.AssetAdditionalDetail.Make,
                                                        t.Asset.AssetAdditionalDetail.Model,
                                                        t.Asset.AssetAdditionalDetail.Year,
                                                        t.Asset.AssetAdditionalDetail.AssetClass,
                                                        t.Asset.AssetAdditionalDetail.FuelCapacity,
                                                        t.Asset.AssetAdditionalDetail.VehicleId,
                                                        t.Asset.AssetAdditionalDetail.Vendor
                                                    },
                                                    FuelType = t.Asset.MstProductType == null ? null : new
                                                    {
                                                        t.Asset.MstProductType.Name
                                                    }
                                                }).ToListAsync();

                        var tankAdditionalList = new List<TankDetailViewModel>();
                        if (type == (int)AssetType.Tank)
                        {
                            var tankIds = assignedAssets.Select(t => t.Asset.Id).ToList();
                            tankAdditionalList = await new FreightServiceDomain(this).GetTankList(tankIds);
                        }

                        foreach (var item in assignedAssets)
                        {
                            var assetItem = new AssetViewModel();
                            assetItem.Id = item.Asset.Id;
                            assetItem.Name = item.Asset.Name;
                            var assetAdditional = new AssetAdditionalDetailViewModel();

                            assetAdditional.Make = string.IsNullOrWhiteSpace(item.AssetAdditionalDetail.Make) ? null : item.AssetAdditionalDetail.Make;
                            assetAdditional.Model = string.IsNullOrWhiteSpace(item.AssetAdditionalDetail.Model) ? null : item.AssetAdditionalDetail.Model;
                            assetAdditional.Year = string.IsNullOrWhiteSpace(item.AssetAdditionalDetail.Year) ? null : item.AssetAdditionalDetail.Year;
                            assetAdditional.Class = string.IsNullOrWhiteSpace(item.AssetAdditionalDetail.AssetClass) ? null : item.AssetAdditionalDetail.AssetClass;
                            assetAdditional.FuelCapacity = item.AssetAdditionalDetail.FuelCapacity;

                            assetAdditional.TankId = !string.IsNullOrWhiteSpace(item.AssetAdditionalDetail.VehicleId) ? item.AssetAdditionalDetail.VehicleId : string.Empty;
                            assetAdditional.VehicleId = !string.IsNullOrWhiteSpace(item.AssetAdditionalDetail.VehicleId) ? item.AssetAdditionalDetail.VehicleId : string.Empty;
                            assetAdditional.StorageId = !string.IsNullOrWhiteSpace(item.AssetAdditionalDetail.Vendor) ? item.AssetAdditionalDetail.Vendor : string.Empty;
                            assetAdditional.Vendor = !string.IsNullOrWhiteSpace(item.AssetAdditionalDetail.Vendor) ? item.AssetAdditionalDetail.Vendor : string.Empty;

                            bool isTankDetailFound = false;
                            if (item.Asset.Type == (int)AssetType.Tank && tankAdditionalList != null)
                            {
                                var tankViewModel = tankAdditionalList.FirstOrDefault(t => t.AssetId == assetItem.Id);
                                if (tankViewModel != null)
                                {
                                    isTankDetailFound = true;
                                    assetAdditional.TankTypeName = !string.IsNullOrEmpty(tankViewModel.TankMakeModel) ? tankViewModel.TankMakeModel : Resource.lblHyphen;
                                    assetAdditional.Threshold = tankViewModel.ThresholdDeliveryRequest.HasValue ? $"{tankViewModel.ThresholdDeliveryRequest.Value.GetPreciseValue(2)}{Resource.constSymbolPercent}" : Resource.lblHyphen;
                                    assetAdditional.DipTestMethodName = tankViewModel.DipTestMethod.HasValue ? ((DipTestMethod)tankViewModel.DipTestMethod).GetDisplayName() : Resource.lblHyphen;
                                    assetAdditional.TankChart = !string.IsNullOrEmpty(tankViewModel.TankChart) ? tankViewModel.TankChart : Resource.lblHyphen;
                                    if (assetAdditional.TanksConnected != null && assetAdditional.TanksConnected.Any())
                                        assetAdditional.TanksConnectedNames = GetTanksName(assetAdditional.TanksConnected, tankAdditionalList);
                                    assetAdditional.TankSequence = tankViewModel.TankSequence;
                                    assetAdditional.PedigreeAssetDBID = tankViewModel.PedigreeAssetDBID;
                                }
                            }

                            if (item.Asset.Type == (int)AssetType.Tank && !isTankDetailFound)
                            {
                                assetAdditional.TankTypeName = Resource.lblHyphen;
                                assetAdditional.Threshold = Resource.lblHyphen;
                                assetAdditional.DipTestMethodName = Resource.lblHyphen;
                                assetAdditional.TankChart = Resource.lblHyphen;
                            }

                            assetItem.AssetAdditionalDetail = assetAdditional;
                            assetItem.Image = item.Asset.ImageId == null ? new ImageViewModel() : new ImageViewModel() { Id = item.Asset.ImageId ?? 0, FilePath = item.Asset.FilePath };
                            if (assetItem.Image.Id > 0)
                            {
                                assetItem.Image.FilePath = assetItem.Image.GetAzureFilePath(BlobContainerType.JobFilesUpload);
                            }
                            assetItem.FuelType = item.FuelType == null ? new AssetFuelTypeViewModel() : new AssetFuelTypeViewModel() { Name = item.FuelType.Name };//product type id
                          
                                assetItem.TFXFuelType = item.Asset.TFXFuelType;
                                response.Add(assetItem);
                        }
                        if (response != null && response.Any())
                        {
                            response = response.OrderBy(t1 => t1.AssetAdditionalDetail.TankSequence == null || t1.AssetAdditionalDetail.TankSequence == 0 ? 99999 : t1.AssetAdditionalDetail.TankSequence).ToList();
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AssetDomain", "GetAssetInfoGridAsync", ex.Message, ex);
                }
                return response;
            }
        }

        public async Task<List<SupplierAssetDropHistoryViewModel>> GetSupplierAssetHistoryViewAsync(int orderId)
        {
            using (var tracer = new Tracer("AssetDomain", "GetSupplierAssetHistoryViewAsync"))
            {
                var response = new List<SupplierAssetDropHistoryViewModel>();
                try
                {
                    var spDomain = new StoredProcedureDomain(this);
                    var assetDrops = await spDomain.GetSupplierOrderAssetDrops(orderId);                    
                    foreach (var item in assetDrops)
                    {
                        var history = new SupplierAssetDropHistoryViewModel();
                        history.CustomerName = item.Customer;
                        history.PONumber = item.PoNumber;
                        history.AssetContractNumber = item.ContractNumber;
                        history.AssetId = item.VehicleId;
                        history.AssetName = item.AssetName;
                        history.DropDate = item.DropStartDate.Date.ToString(Resource.constFormatDate);
                        history.DropStartTime = item.DropStartDate.DateTime.ToShortTimeString();
                        history.DropEndTime = item.DropEndDate.DateTime.ToShortTimeString();
                        history.GallonsDelivered = item.DroppedGallons;
                        history.InvoiceId = item.InvoiceId;
                        history.InvoiceNumber = item.InvoiceNumber;
                        history.InvoiceTypeId = item.InvoiceTypeId;
                        if (history.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketManual && history.InvoiceTypeId != (int)InvoiceType.DigitalDropTicketMobileApp)
                        {
                            history.PricingFormat = item.DisplayPrice;
                            history.TotalCostForAsset = (item.PricePerGallon * item.DroppedGallons).GetPreciseValue(4);
                            history.UnitCost = item.PricePerGallon.GetPreciseValue(4);
                            history.Currency = item.Currency;
                        }
                        response.Add(history);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AssetDomain", "GetSupplierAssetHistoryViewAsync", ex.Message, ex);
                }
                return response;
            }
        }


        public async Task<List<AssetListViewModel>> GetAssetListMobileAsync(UserContext userContext, int start = 0, int length = 10, int brandedSuppCompanyId = 0)
        {
            using (var tracer = new Tracer("AssetDomain", "GetAssetListMobileAsync"))
            {
                var response = new List<AssetListViewModel>();
                try
                {
                    var spDomain = new StoredProcedureDomain(this);
                    response = await spDomain.GetAssetList(userContext.CompanyId, start, length);
                    if(brandedSuppCompanyId > 0)
                    {
                        var jobIdList = response.Where(t => t.CurrentJobId.HasValue && t.CurrentJobId.Value > 0)
                                            .Select(t => t.CurrentJobId.Value).Distinct().ToList();

                        jobIdList = ContextFactory.Current.GetDomain<JobDomain>().GetBrandedSupplierLocations(brandedSuppCompanyId, true, jobIdList);

                        response = response.Where(t => t.CurrentJobId.HasValue
                                                && jobIdList.Contains(t.CurrentJobId.Value)).ToList();
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AssetDomain", "GetAssetListMobileAsync", ex.Message, ex);
                }
                return response;
            }
        }

        public async Task<List<AssetGridViewModel>> GetAssetGridAsync(UserContext userContext, AssetFilterViewModel assetFilter = null, bool assignAssets = false, int brandedCompanyId = -1)
        {
            using (var tracer = new Tracer("AssetDomain", "GetAssetGridAsync"))
            {
                var response = new List<AssetGridViewModel>();
                try
                {
                    var allAssets = Context.DataContext.Assets.Include(t => t.AssetAdditionalDetail)
                                    .Include(t => t.Image).Include(t => t.JobXAssets).Include("JobXAssets.Job")
                                    .Where(t => t.IsActive && t.Type == (int)assetFilter.Type && t.Company.Id == userContext.CompanyId);

                    if (assetFilter != null && !assignAssets)
                    {
                        if (assetFilter.JobId > 0)
                        {
                            allAssets = allAssets.Where(t => t.JobXAssets.Any(t1 => t1.JobId == assetFilter.JobId &&
                                                                                t1.RemovedBy == null && t1.RemovedDate == null));
                        }

                        DateTimeOffset startDate = Convert.ToDateTime(ApplicationConstants.DateRangeFilterStartDate);
                        DateTimeOffset endDate = DateTimeOffset.Now.Date.AddDays(1);

                        if (!string.IsNullOrEmpty(assetFilter.StartDate))
                        {
                            startDate = Convert.ToDateTime(assetFilter.StartDate).Date;
                        }
                        if (!string.IsNullOrEmpty(assetFilter.EndDate))
                        {
                            endDate = Convert.ToDateTime(assetFilter.EndDate).Date.AddDays(1);
                        }
                        allAssets = allAssets.Where(t => t.CreatedDate >= startDate && t.CreatedDate < endDate);
                    }

                    var helperDomain = new HelperDomain(this);
                    List<int> assignedJobIds = new List<int>();
                    if (userContext.IsBuyer || userContext.IsOnsitePerson)
                    {
                        assignedJobIds = await Task.Run(() => helperDomain.GetJobIdsAsync(userContext.Id));
                        allAssets = allAssets.Where(t => assignedJobIds.Any(t1 => t.JobXAssets.Any(t2 => t2.JobId == t1
                                                                    && t2.RemovedDate == null
                                                                    && t2.RemovedBy == null)));
                    }
                    var numberOfAssignedJobs = assignedJobIds.Count;

                    var filteredAssetList = await allAssets.OrderByDescending(t => t.Id).ToListAsync();

                    var tankAdditionalList = new List<TankDetailViewModel>();
                    if (assetFilter.Type == AssetType.Tank)
                    {
                        var tankIds = filteredAssetList.Select(t => t.Id).ToList();
                        tankAdditionalList = await new FreightServiceDomain(this).GetTankList(tankIds);
                    }

                    foreach (var asset in filteredAssetList)
                    {
                        AssetGridViewModel assetGridViewModel = new AssetGridViewModel();
                        var activeJob = asset.JobXAssets.FirstOrDefault(t => t.RemovedBy == null && t.RemovedDate == null);
                        var inactiveJob = asset.JobXAssets.OrderByDescending(t => t.AssignedDate).FirstOrDefault(t => t.RemovedBy != null && t.RemovedDate != null);
                        var subcontractor = asset.AssetSubcontractors.FirstOrDefault(t => t.IsActive);
                        assetGridViewModel.Id = asset.Id;
                        assetGridViewModel.Name = asset.Name;
                        assetGridViewModel.IsActive = asset.IsActive;
                        assetGridViewModel.JobXAssetsId = activeJob == null ? 0 : activeJob.Id;
                        assetGridViewModel.Class = asset.AssetAdditionalDetail.AssetClass ?? Resource.lblHyphen;
                        assetGridViewModel.DateAdded = activeJob == null ? Resource.lblHyphen : activeJob.AssignedDate.ToTargetDateTimeOffset(activeJob.Job.TimeZoneName).DateTime.ToString(Resource.constFormatDateTime);
                        assetGridViewModel.ImageId = asset.Image == null ? 0 : asset.Image.Id;
                        assetGridViewModel.AzureBlobImageURL = asset.Image == null ? string.Empty : string.Format("https://{0}/{1}/{2}{3}", AzureBlobStorage.GetStorageAccountName(), BlobContainerType.JobFilesUpload.ToString().ToLower(), asset.Image.FilePath, AzureBlobStorage.GetSaS(BlobContainerType.JobFilesUpload.ToString().ToLower()));
                        assetGridViewModel.FuelCapacity = asset.AssetAdditionalDetail.FuelCapacity.HasValue ? asset.AssetAdditionalDetail.FuelCapacity.Value.GetPreciseValue(6).GetCommaSeperatedValue() : Resource.lblHyphen;
                        assetGridViewModel.VehicleId = asset.AssetAdditionalDetail.VehicleId;
                        assetGridViewModel.Vendor = string.IsNullOrWhiteSpace(asset.AssetAdditionalDetail.Vendor) ? Resource.lblHyphen : asset.AssetAdditionalDetail.Vendor;
                        assetGridViewModel.CurrentJobId = activeJob == null ? 0 : activeJob.Job.Id;
                        assetGridViewModel.LastJobId = inactiveJob != null ? inactiveJob.Job.Id : 0;
                        assetGridViewModel.NumberOfAssignedJobs = numberOfAssignedJobs;
                        assetGridViewModel.FuelType = asset.MstProductType != null ? asset.MstProductType.Name : Resource.lblHyphen;
                        assetGridViewModel.TFXFuelType = asset.MstTfxProduct != null ? asset.MstTfxProduct.Name : Resource.lblHyphen;
                        //if (activeJob != null && activeJob.Job != null && activeJob.Job.FuelRequests != null && brandedCompanyId > 0)
                        //{

                        //    assetGridViewModel.SupplierCompanyIds = (from fuelitem in activeJob.Job.FuelRequests
                        //                                             join orderinfo in Context.DataContext.Orders
                        //                                             on fuelitem.Id equals orderinfo.FuelRequestId
                        //                                             where orderinfo.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open
                        //                                             select orderinfo.AcceptedCompanyId
                        //                                             ).Distinct().ToList();
                        //}
                        bool isTankDetailFound = false;
                        if (asset.Type == (int)AssetType.Asset)
                        {
                            assetGridViewModel.CurrentJobName = activeJob == null ? Resource.lblHyphen : activeJob.Job.Name;
                            assetGridViewModel.LastJobName = inactiveJob != null ? inactiveJob.Job.Name : Resource.lblHyphen;
                            assetGridViewModel.SubcontractorId = subcontractor != null ? subcontractor.SubcontractorId : -1;
                        }
                        else if (asset.Type == (int)AssetType.Tank && tankAdditionalList != null)
                        {
                            var tankViewModel = tankAdditionalList.FirstOrDefault(t => t.AssetId == asset.Id);
                            if (tankViewModel != null)
                            {
                                isTankDetailFound = true;
                                assetGridViewModel.TankType = !string.IsNullOrEmpty(tankViewModel.TankMakeModel) ? tankViewModel.TankMakeModel : Resource.lblHyphen;
                                assetGridViewModel.Threshold = tankViewModel.ThresholdDeliveryRequest.HasValue ? $"{tankViewModel.ThresholdDeliveryRequest.Value.GetPreciseValue(2)}{Resource.constSymbolPercent}" : Resource.lblHyphen;
                                assetGridViewModel.DipTestMethod = tankViewModel.DipTestMethod.HasValue ? ((DipTestMethod)tankViewModel.DipTestMethod).GetDisplayName() : Resource.lblHyphen;
                                assetGridViewModel.TankChart = !string.IsNullOrEmpty(tankViewModel.TankChart) ? tankViewModel.TankChart : Resource.lblHyphen;
                                if (tankViewModel.TanksConnected != null && tankViewModel.TanksConnected.Any())
                                    assetGridViewModel.TanksConnectedNames = GetTanksName(tankViewModel.TanksConnected, tankAdditionalList);
                                assetGridViewModel.TankSequence = tankViewModel.TankSequence;
                                assetGridViewModel.IsStopATGPolling = tankViewModel.IsStopATGPolling;
                            }
                        }

                        if (asset.Type == (int)AssetType.Tank && !isTankDetailFound)
                        {
                            assetGridViewModel.TankType = Resource.lblHyphen;
                            assetGridViewModel.Threshold = Resource.lblHyphen;
                            assetGridViewModel.DipTestMethod = Resource.lblHyphen;
                            assetGridViewModel.TankChart = Resource.lblHyphen;
                        }

                        response.Add(assetGridViewModel);
                    }
                    if (response != null && response.Any())
                    {
                        //if (brandedCompanyId > 0)
                        //{
                        //    response = response.Where(top => top.SupplierCompanyIds.Contains(brandedCompanyId) && top.SupplierCompanyIds.Count() > 0).ToList();
                        //}
                        response = response.OrderBy(t1 => t1.TankSequence == null || t1.TankSequence == 0 ? 99999 : t1.TankSequence).ToList();
                    }
                    int? jobId = null;
                    if (assetFilter != null && assetFilter.JobId > 0)
                    {
                        jobId = assetFilter.JobId;
                        var subcontractors = GetSubContractors(jobId, userContext.CompanyId);
                        response.ForEach(t => t.Subcontractors = subcontractors);
                    }
                    else
                    {
                        response.ForEach(t => t.Subcontractors = GetSubContractors(t.CurrentJobId, userContext.CompanyId));
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AssetDomain", "GetAssetGridAsync", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<List<AssetGridViewModel>> GetAssetGridByJobAsync(int jobId)
        {
            using (var tracer = new Tracer("AssetDomain", "GetAssetGridByJobAsync"))
            {
                var response = new List<AssetGridViewModel>();
                try
                {
                    var job = await Context.DataContext.Jobs.SingleOrDefaultAsync(t => t.Id == jobId);
                    if (job != null)
                    {
                        response = job.JobXAssets.Where(t => t.Asset.IsActive && t.RemovedBy == null && t.RemovedDate == null).Select(t => new AssetGridViewModel()
                        {
                            Id = t.AssetId,
                            Name = t.Asset.Name,
                            Class = t.Asset.AssetAdditionalDetail.AssetClass ?? Resource.lblHyphen,
                            DateAdded = t.AssignedDate.DateTime.ToString(),
                            Vendor = t.Asset.AssetAdditionalDetail.Vendor ?? Resource.lblHyphen,
                            ImageId = t.Asset.Image == null ? 0 : t.Asset.Image.Id,
                            CurrentJobName = t.Job.Name,
                            CurrentJobId = t.Job.Id,
                            LastJobName = Context.DataContext.JobXAssets.Where(x => x.AssetId == t.Asset.Id && x.RemovedDate != null)
                                              .OrderByDescending(x => x.RemovedDate).Select(x => x.Job.Name).FirstOrDefault(),
                        }).OrderByDescending(t => t.Id).ToList();
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AssetDomain", "GetAssetGridByJobAsync", ex.Message, ex);
                }

                return response;
            }
        }

        public List<AssetHistoryGridViewModel> GetAssetHistoryGrid(int assetId)
        {
            using (var tracer = new Tracer("AssetDomain", "GetAssetHistoryGrid"))
            {
                var response = new List<AssetHistoryGridViewModel>();
                try
                {
                    var sqlQuery = Context.DataContext.JobXAssets.Where(t => t.AssetId == assetId).OrderByDescending(t => t.AssignedDate);
                    foreach (var item in sqlQuery)
                    {
                        var assetDropsWithInvoice = item.AssetDrops.Where(x => x.Invoice != null && (x.Order.FuelRequest.FuelRequestTypeId != (int)FuelRequestType.BrokeredFuelRequest));
                        response.Add(new AssetHistoryGridViewModel(Status.Success)
                        {
                            Id = item.Id,
                            JobId = item.JobId,
                            JobName = item.Job.Name,
                            AssignedDate = item.AssignedDate.ToString(Resource.constFormatDate),
                            RemovedDate = item.RemovedDate.HasValue ? item.RemovedDate.Value.ToString(Resource.constFormatDate) : Resource.lblHyphen,
                            StartDate = assetDropsWithInvoice.Any() ? item.AssetDrops.OrderBy(x => x.DropEndDate).First().DropEndDate.ToString(Resource.constFormatDate) : Resource.lblHyphen,
                            EndDate = assetDropsWithInvoice.Any() ? item.AssetDrops.OrderByDescending(x => x.DropEndDate).First().DropEndDate.ToString(Resource.constFormatDate) : Resource.lblHyphen,
                            TotalDrops = assetDropsWithInvoice.Count(),
                            TotalFuel = assetDropsWithInvoice.Sum(x => x.DroppedGallons).GetPreciseValue(2).GetCommaSeperatedValue(),
                            TotalCost = Resource.constSymbolCurrency + (assetDropsWithInvoice.Sum(x => (x.Invoice.InvoiceXBolDetails.Select(t => t.InvoiceFtlDetail.PricePerGallon).First() * x.DroppedGallons)).GetPreciseValue(2)).GetCommaSeperatedValue(),
                            AssetDropHistory = assetDropsWithInvoice.Select(x => x.ToAssetDropViewModel()).OrderByDescending(x => x.DropDate).ThenByDescending(x => x.DropTime).ToList()
                        });
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AssetDomain", "GetAssetHistoryGrid", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<StatusViewModel> DeleteAssetAsync(int userId, int assetId)
        {
            using (var tracer = new Tracer("AssetDomain", "DeleteAssetAsync"))
            {
                StatusViewModel response = new StatusViewModel();
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        var user = await Context.DataContext.Users.FirstOrDefaultAsync(t => t.Id == userId);
                        if (user != null)
                        {
                            var asset = user.Company.Assets.FirstOrDefault(t => t.Id == assetId);
                            if (asset != null)
                            {
                                var jobAsset = asset.JobXAssets.Any(t => t.RemovedBy == null && t.RemovedDate == null);
                                if (jobAsset)
                                {
                                    response.StatusMessage = Resource.errMessageAssetUsed;
                                }
                                else
                                {
                                    asset.IsActive = false;
                                    asset.UpdatedBy = userId;
                                    asset.UpdatedDate = DateTimeOffset.Now;

                                    Context.DataContext.Entry(asset).State = EntityState.Modified;

                                    response.StatusCode = Status.Success;
                                    response.StatusMessage = string.Format(Resource.errMessageAssetDeleteSuccess, ((AssetType)asset.Type).GetDisplayName());
                                }
                            }
                            await Context.CommitAsync();
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        response.StatusMessage = Resource.errMessageAssetDeleteFailed;
                        transaction.Rollback();
                        LogManager.Logger.WriteException("AssetDomain", "DeleteAssetAsync", ex.Message, ex);
                    }
                }

                return response;
            }
        }

        public async Task<StatusViewModel> AssignToJobAsync(UserContext userContext, AssetJobAssignmentViewModel viewModel)
        {
            LogManager.Logger.WriteDebug("AssetDomain", "AssignToJobAsync", "Start");
            StatusViewModel response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var jobAsset = viewModel.ToEntity();
                    //removing subcontractor assignment to asset if subcontractor company doesn't belongs to this job
                    var asset = Context.DataContext.Assets.FirstOrDefault(t => t.Id == viewModel.AssetId);
                    if (asset != null)
                    {
                        var job = Context.DataContext.Jobs.FirstOrDefault(t => t.Id == viewModel.JobId);
                        var subcontractor = asset.AssetSubcontractors.FirstOrDefault(t => t.IsActive);
                        if (subcontractor != null && !job.Subcontractors.Any(t => t.Id == subcontractor.Id))
                        {
                            subcontractor.RemovedBy = userContext.Id;
                            subcontractor.RemovedDate = DateTimeOffset.Now;
                            subcontractor.IsActive = false;
                        }

                        await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetAssetAddedRemovedNewsfeed(userContext, job, 1, true);
                    }
                    Context.DataContext.JobXAssets.Add(jobAsset);

                    response.StatusCode = Status.Success;
                    if (asset != null)
                        response.StatusMessage = string.Format(Resource.errMessageAssetsAssignedSuccess, ((AssetType)asset.Type).GetDisplayName());

                    await Context.CommitAsync();
                    transaction.Commit();

                    viewModel = jobAsset.ToViewModel(viewModel);
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageAssetsAssignedFailed;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("AssetDomain", "AssignToJobAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusViewModel> RemoveFromJobAsync(UserContext userContext, int id, int newJobId = 0)
        {
            using (var tracer = new Tracer("AssetDomain", "RemoveFromJobAsync"))
            {
                StatusViewModel response = new StatusViewModel();
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        var jobAssets = Context.DataContext.JobXAssets.Where(t => t.Id == id && t.RemovedBy == null && t.RemovedDate == null);
                        if (jobAssets != null)
                        {
                            var jobAsset = jobAssets.OrderByDescending(t => t.AssignedDate).FirstOrDefault();
                            if (jobAsset != null)
                            {
                                jobAsset.RemovedBy = userContext.Id;
                                jobAsset.RemovedDate = DateTimeOffset.Now;

                                Context.DataContext.Entry(jobAsset).State = EntityState.Modified;

                                await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetAssetAddedRemovedNewsfeed(userContext, jobAsset.Job, 1, false);
                                if (newJobId != 0)
                                {
                                    var job = Context.DataContext.Jobs.Where(t => t.Id == newJobId).FirstOrDefault();
                                    await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetAssetReassignedNewsfeed(userContext, jobAsset.Job, 1, job.Name);
                                }
                                response.StatusCode = Status.Success;
                                response.StatusMessage = string.Format(Resource.errMessageRemoveAssetAssignmentSuccess, ((AssetType)jobAsset.Asset.Type).GetDisplayName());
                            }
                        }

                        await Context.CommitAsync();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        response.StatusMessage = Resource.errMessageRemoveAssetAssignmentFailed;
                        transaction.Rollback();
                        LogManager.Logger.WriteException("AssetDomain", "RemoveAssetFromJob", ex.Message, ex);
                    }
                }

                return response;
            }
        }

        public async Task<StatusViewModel> SaveImageAsync(int assetId, byte[] imageData)
        {
            using (var tracer = new Tracer("AssetDomain", "SaveImageAsync"))
            {
                StatusViewModel response = new StatusViewModel();
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        var asset = await Context.DataContext.Assets.FirstOrDefaultAsync(t => t.Id == assetId);
                        if (asset != null)
                        {
                            if (asset.Image != null)
                            {
                                asset.Image.Data = imageData;
                            }
                            else
                            {
                                asset.Image = new Image { Data = imageData };
                            }
                            Context.DataContext.Entry(asset).State = EntityState.Modified;

                            await Context.CommitAsync();
                            transaction.Commit();

                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.errMessageAssetImageSaveSuccess;
                        }
                    }
                    catch (Exception ex)
                    {
                        response.StatusMessage = Resource.errMessageAssetImageSaveFailed;
                        transaction.Rollback();
                        LogManager.Logger.WriteException("AssetDomain", "SaveImageAsync", ex.Message, ex);
                    }
                }

                return response;
            }
        }

        public async Task<StatusViewModel> RemoveImageAsync(int assetId, int imageId)
        {
            using (var tracer = new Tracer("AssetDomain", "RemoveImageAsync"))
            {
                StatusViewModel response = new StatusViewModel();
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        var asset = await Context.DataContext.Assets.FirstOrDefaultAsync(t => t.Id == assetId);
                        var image = await Context.DataContext.Images.FirstOrDefaultAsync(t => t.Id == imageId);
                        if (asset != null && image != null)
                        {
                            asset.Image = null;
                            Context.DataContext.Entry(asset).State = EntityState.Modified;
                            Context.DataContext.Images.Remove(image);

                            await Context.CommitAsync();
                            transaction.Commit();

                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.errMessageAssetImageRemoveSuccess;
                        }
                    }
                    catch (Exception ex)
                    {
                        response.StatusMessage = Resource.errMessageAssetImageRemoveFailed;
                        transaction.Rollback();
                        LogManager.Logger.WriteException("AssetDomain", "RemoveImageAsync", ex.Message, ex);
                    }
                }

                return response;
            }
        }
        public StatusViewModel ValidateTPOAssetCsvHeader(string csvText, string csvFilePath)
        {
            using (var tracer = new Tracer("AssetDomain", "ValidateTPOAssetCsvHeader"))
            {
                StatusViewModel response = new StatusViewModel();
                try
                {
                    var csvHeaderLine = Regex.Matches(csvText.Trim(), @"^.*Asset Name.*\n").Cast<Match>().FirstOrDefault();
                    string[] lines = File.ReadAllLines(csvFilePath);
                    string headerLine = lines.FirstOrDefault();
                    if (csvHeaderLine != null && csvHeaderLine.Value.Trim() == headerLine)
                    {
                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMessageSuccess;
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageBulkUploadHeaderMismatch;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AssetDomain", "ValidateTPOAssetCsvHeader", ex.Message, ex);
                }

                return response;
            }
        }
        public StatusViewModel ValidateCsvHeader(string csvText, string csvFilePath)
        {
            using (var tracer = new Tracer("AssetDomain", "ValidateCsvHeader"))
            {
                StatusViewModel response = new StatusViewModel();
                try
                {
                    var csvHeaderLine = Regex.Matches(csvText.Trim(), @"^.*AssetName.*\n").Cast<Match>().FirstOrDefault();
                    string[] lines = File.ReadAllLines(csvFilePath);
                    string headerLine = lines.FirstOrDefault();
                    if (csvHeaderLine.Value.Trim() == headerLine)
                    {
                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMessageSuccess;
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageBulkUploadHeaderMismatch;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AssetDomain", "ValidateCsvHeader", ex.Message, ex);
                }

                return response;
            }
        }
        private Asset GetAssetObject(AssetTPOBulkRecordViewModel csvRecord, int userId, int companyId, DropdownDisplayItem productType)
        {
            using (var tracer = new Tracer("AssetDomain", "GetAssetObject"))
            {
                Asset entity = new Asset { Name = csvRecord.Name, CreatedDate = DateTimeOffset.Now, IsActive = true, UpdatedDate = DateTimeOffset.Now, Type = (int)AssetType.Asset };
                entity.AssetAdditionalDetail = new AssetAdditionalDetail
                {
                    Make = csvRecord.Make,
                    Model = csvRecord.Model,
                    Year = csvRecord.Year,
                    TelematicsProvider = csvRecord.TelematicsProvider,
                    LicensePlate = csvRecord.LicensePlate,
                    AssetClass = csvRecord.Class,
                    Vendor = csvRecord.Vendor,
                    VehicleId = csvRecord.AssetID,
                    Description = csvRecord.Description,
                    IsActive = true,
                    UpdatedDate = DateTimeOffset.Now
                };

                if (!string.IsNullOrEmpty(csvRecord.FuelType) && productType != null)
                {
                    entity.FuelType = productType.Id;
                    if (csvRecord.FuelType.Equals("marine", StringComparison.InvariantCultureIgnoreCase))
                    {
                        entity.IsMarine = true;
                    }
                }

                var licenseeState = Context.DataContext.MstStates.FirstOrDefault(t => t.Code.ToLower() == csvRecord.LicensePlateState.ToLower());
                if (licenseeState != null)
                {
                    entity.AssetAdditionalDetail.LicensePlateStateId = licenseeState.Id;
                    entity.AssetAdditionalDetail.MstState = licenseeState;
                }

                var subContractor = Context.DataContext.Subcontractors.FirstOrDefault(t => t.Name.ToLower() == csvRecord.Subcontractor.ToLower() && t.Jobs.Any(t1 => t1.CompanyId == companyId));
                if (subContractor != null)
                {
                    AssetSubcontractor assetSubcontractor = new AssetSubcontractor() { Subcontractor = subContractor, SubcontractorId = subContractor.Id, AssignedBy = userId, AssignedDate = DateTimeOffset.Now, IsActive = true };
                    entity.AssetSubcontractors.Add(assetSubcontractor);
                }

                if (!string.IsNullOrWhiteSpace(csvRecord.Contract))
                {
                    AssetContractNumber assetContractNumber = new AssetContractNumber() { ContractNumber = csvRecord.Contract, AddedBy = userId, AddedDate = DateTimeOffset.Now, IsActive = true };
                    entity.AssetContractNumbers.Add(assetContractNumber);
                }

                decimal fuelCapacity = 0;
                if (decimal.TryParse(csvRecord.FuelCapacity, out fuelCapacity))
                {
                    entity.AssetAdditionalDetail.FuelCapacity = fuelCapacity;
                }

                return entity;
            }
        }

        private Asset GetAssetObject(AssetCsvRecordViewModel csvRecord, int userId, int companyId, DropdownDisplayItem productType)
        {
            using (var tracer = new Tracer("AssetDomain", "GetAssetObject"))
            {
                Asset entity = new Asset { Name = csvRecord.Name, CreatedDate = DateTimeOffset.Now, IsActive = true, UpdatedDate = DateTimeOffset.Now, Type = (int)AssetType.Asset };
                entity.AssetAdditionalDetail = new AssetAdditionalDetail
                {
                    Make = csvRecord.Make,
                    Model = csvRecord.Model,
                    Year = csvRecord.Year,
                    TelematicsProvider = csvRecord.TelematicsProvider,
                    LicensePlate = csvRecord.LicensePlate,
                    AssetClass = csvRecord.Class,
                    Vendor = csvRecord.Vendor,
                    VehicleId = csvRecord.AssetID,
                    Description = csvRecord.Description,
                    IsActive = true,
                    UpdatedDate = DateTimeOffset.Now
                };

                if (!string.IsNullOrEmpty(csvRecord.FuelType) && productType != null)
                {
                    entity.FuelType = productType.Id;
                    if(csvRecord.FuelType.Equals("marine", StringComparison.InvariantCultureIgnoreCase))
                    {
                        entity.IsMarine = true;
                    }
                }

                var licenseeState = Context.DataContext.MstStates.FirstOrDefault(t => t.Code.ToLower() == csvRecord.LicensePlateState.ToLower());
                if (licenseeState != null)
                {
                    entity.AssetAdditionalDetail.LicensePlateStateId = licenseeState.Id;
                    entity.AssetAdditionalDetail.MstState = licenseeState;
                }

                var subContractor = Context.DataContext.Subcontractors.FirstOrDefault(t => t.Name.ToLower() == csvRecord.Subcontractor.ToLower() && t.Jobs.Any(t1 => t1.CompanyId == companyId));
                if (subContractor != null)
                {
                    AssetSubcontractor assetSubcontractor = new AssetSubcontractor() { Subcontractor = subContractor, SubcontractorId = subContractor.Id, AssignedBy = userId, AssignedDate = DateTimeOffset.Now, IsActive = true };
                    entity.AssetSubcontractors.Add(assetSubcontractor);
                }

                if (!string.IsNullOrWhiteSpace(csvRecord.Color))
                {
                    AssetContractNumber assetContractNumber = new AssetContractNumber() { ContractNumber = csvRecord.Color, AddedBy = userId, AddedDate = DateTimeOffset.Now, IsActive = true };
                    entity.AssetContractNumbers.Add(assetContractNumber);
                }

                decimal fuelCapacity = 0;
                if (decimal.TryParse(csvRecord.FuelCapacity, out fuelCapacity))
                {
                    entity.AssetAdditionalDetail.FuelCapacity = fuelCapacity;
                }

                return entity;
            }
        }

        private async Task<StatusViewModel> SaveAssetList(int userId, int jobId, List<Asset> assets)
        {
            var response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var job = Context.DataContext.Jobs.FirstOrDefault(t => t.Id == jobId);
                    var user = Context.DataContext.Users.FirstOrDefault(t => t.Id == userId);

                    var uniqueAssets = assets.GroupBy(i => new { i.Name }).SelectMany(g => g.Take(1)).ToList();
                    var duplicates = assets.GroupBy(i => new { i.Name }).SelectMany(g => g.Skip(1)).ToList();

                    // Add unique assets exists in the csv file
                    foreach (var asset in uniqueAssets)
                    {
                        if (!user.Company.Assets.Any(t => t.IsActive && t.Name.ToLower() == asset.Name.ToLower()))
                        {
                            asset.UpdatedBy = userId;
                            asset.AssetAdditionalDetail.UpdatedBy = userId;
                            user.Company.Assets.Add(asset);
                            if (job != null)
                            {
                                job.JobXAssets.Add(new JobXAsset { Asset = asset, JobId = jobId, AssignedBy = userId, AssignedDate = DateTimeOffset.Now });
                            }
                        }
                        else if (!duplicates.Any(t => t.Name.ToLower() == asset.Name.ToLower()))
                        {
                            duplicates.Add(asset);
                        }
                    }

                    if (duplicates.Any())
                    {
                        foreach (var duplicate in duplicates)
                        {
                            var existing = user.Company.AssetDuplicates.FirstOrDefault(t => t.Name.ToLower() == duplicate.Name.ToLower());
                            if (existing != null && existing.Id > 0)
                            {
                                existing = duplicate.ToAssetDuplicate(existing);
                            }
                            user.Company.AssetDuplicates.Add(duplicate.ToAssetDuplicate());
                        }
                    }

                    await Context.CommitAsync();
                    transaction.Commit();

                    if (duplicates.Count > 0)
                    {
                        response.StatusCode = Status.Warning;
                        response.StatusMessage = Resource.errMessageDuplicateAssetName;
                    }
                    else
                    {
                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMessageBulkUploadSuccess;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.StatusMessage = Constants.ErrorWhileProcessingBulkOrder;
                    LogManager.Logger.WriteException("AssetDomain", "SaveBulkAssetsAsync", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<int> GetCurrentJobId(int id)
        {
            using (var tracer = new Tracer("AssetDomain", "GetCurrentJobId"))
            {
                int response = 0;
                try
                {
                    var asset = await Context.DataContext.JobXAssets.FirstOrDefaultAsync(t => t.Id == id && t.RemovedBy == null && t.RemovedDate == null);
                    if (asset != null)
                    {
                        response = asset.Job.Id;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AssetDomain", "GetCurrentJobId", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<StatusViewModel> AssignAssetsToJobAsync(UserContext userContext, int jobId, List<int> assets, bool IsSupplierUser = false)
        {
            using (var tracer = new Tracer("AssetDomain", "AssignAssetsToJobAsync"))
            {
                StatusViewModel response = new StatusViewModel();
                var type = (int)AssetType.Asset;

                if (assets != null && assets.Any())
                    type = Context.DataContext.Assets.Where(t => assets.Contains(t.Id)).FirstOrDefault().Type;

                using (var trasaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (jobId > 0)
                        {
                            var job = await Context.DataContext.Jobs.SingleOrDefaultAsync(t => t.Id == jobId);
                            if (job != null)
                            {
                                int userIdToUse = IsSupplierUser ? job.CreatedBy : userContext.Id;

                                int assetCount = 0;
                                bool isAdded = false;
                                int existingAssetCount = job.JobXAssets.Count(t => t.RemovedBy == null);
                                if (assets == null && existingAssetCount > 0)
                                {
                                    assetCount = existingAssetCount;
                                }
                                else if (assets != null && assets.Count > 0 && (existingAssetCount == 0 || assets.Count > existingAssetCount))
                                {
                                    assetCount = assets.Count - existingAssetCount; isAdded = true;
                                }
                                else if (assets != null && assets.Count > 0 && assets.Count < existingAssetCount)
                                {
                                    assetCount = existingAssetCount - assets.Count;
                                }
                                if (assetCount > 0 && !IsSupplierUser)
                                    await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetAssetAddedRemovedNewsfeed(userContext, job, assetCount, isAdded);

                                if (assets == null)
                                {
                                    assets = new List<int>();
                                }

                                var assignedAssets = Context.DataContext
                                                            .JobXAssets
                                                            .Where
                                                                    (
                                                                        t => t.JobId == jobId &&
                                                                        t.Asset.Type == type &&
                                                                        t.RemovedBy == null &&
                                                                        t.RemovedDate == null
                                                                    )
                                                            .Select
                                                                    (
                                                                        t => t.AssetId
                                                                    ).ToList();

                                var removedAssets = assignedAssets.Except(assets).ToList();
                                var newAssets = assets.Except(assignedAssets).ToList();

                                Context.DataContext.JobXAssets
                                                .Where(t => (newAssets.Contains(t.AssetId) ||
                                                            removedAssets.Contains(t.AssetId)) &&
                                                            t.RemovedBy == null &&
                                                            t.RemovedDate == null
                                                    ).ToList()
                                                .ForEach(t =>
                                                {
                                                    t.RemovedBy = userIdToUse;
                                                    t.RemovedDate = DateTimeOffset.Now;
                                                });

                                await Context.CommitAsync();

                                newAssets.ForEach(
                                                    t => Context.DataContext.JobXAssets.Add(new JobXAsset()
                                                    {
                                                        JobId = jobId,
                                                        AssetId = t,
                                                        AssignedBy = userIdToUse,
                                                        AssignedDate = DateTimeOffset.Now
                                                    })
                                                  );

                                if (type == (int)AssetType.Asset)
                                {
                                    //removing subcontractor assignment to asset if subcontractor company doesn't belongs to this job
                                    var assetList = Context.DataContext.Assets;
                                    foreach (var assetId in newAssets)
                                    {
                                        var asset = assetList.FirstOrDefault(t => t.Id == assetId);
                                        if (asset != null)
                                        {
                                            var subcontractor = asset.AssetSubcontractors.FirstOrDefault(t => t.IsActive);
                                            if (subcontractor != null && !job.Subcontractors.Any(t => t.Id == subcontractor.SubcontractorId))
                                            {
                                                subcontractor.RemovedDate = DateTimeOffset.Now;
                                                subcontractor.RemovedBy = userIdToUse;
                                                subcontractor.IsActive = false;
                                            }
                                        }
                                    }

                                    if (!job.JobBudget.IsAssetTracked && job.JobXAssets.Any(t => t.RemovedBy == null && t.RemovedDate == null))
                                    {
                                        job.JobBudget.IsAssetTracked = true;
                                    }
                                }

                                await Context.CommitAsync();
                            }
                        }

                        trasaction.Commit();

                        response.StatusCode = Status.Success;
                        response.StatusMessage = string.Format(Resource.errMessageAssetsAssignedSuccess, ((AssetType)type).GetDisplayName());
                    }
                    catch (Exception ex)
                    {
                        trasaction.Rollback();
                        LogManager.Logger.WriteException("AssetDomain", "AssignAssetsToJobAsync", ex.Message, ex);
                    }
                }

                return response;
            }
        }

        public async Task<StatusViewModel> DeleteAssetAsync(int userId, List<int> assets)
        {
            using (var tracer = new Tracer("AssetDomain", "DeleteAssetAsync"))
            {
                StatusViewModel response = new StatusViewModel();
                int type = (int)AssetType.Asset;
                bool assetTankDeleteStatus = false;

                using (var trasaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (assets != null)
                        {
                            var tanks = Context.DataContext.JobXAssets
                                                           .Where(t => assets.Contains(t.AssetId) && t.RemovedBy == null &&
                                                                       t.RemovedDate == null && t.Asset.Type == (int)AssetType.Tank)
                                                           .ToList();
                            var tanksToDelete = new DeleteTanksModel();
                            if (tanks.Any())
                            {
                                tanksToDelete.JobIds = tanks.Select(t => t.JobId).Distinct().ToList();
                                tanksToDelete.TankIds = tanks.Select(t => t.AssetId).Distinct().ToList();
                                tanks.ForEach(t =>
                                {
                                    t.RemovedBy = userId;
                                    t.RemovedDate = DateTimeOffset.Now;
                                });
                                Context.Commit();
                            }

                            var selectedAssetsTanks = Context.DataContext.Assets
                                                                         .Where(t => assets.Contains(t.Id) &&
                                                                                     !t.JobXAssets.Any(t1 => t1.RemovedBy == null && t1.RemovedDate == null))
                                                                         .ToList();
                            if (selectedAssetsTanks.Any())
                            {
                                selectedAssetsTanks.ForEach(t =>
                                                   {
                                                       type = t.Type;
                                                       t.IsActive = false;
                                                       t.UpdatedBy = userId;
                                                       t.UpdatedDate = DateTimeOffset.Now;
                                                   });
                                await Context.CommitAsync();
                                assetTankDeleteStatus = true;
                            }
                            if (tanks.Any())
                            {
                                
                                var isAllRemoved = Context.DataContext.JobXAssets.Where(j => tanksToDelete.JobIds.Contains(j.JobId)
                                                                                        && j.RemovedBy == null && j.RemovedDate == null
                                                                                        && j.Asset.Type == (int)AssetType.Tank).Count()>0;
                                if (!isAllRemoved)
                                {
                                    var locations = Context.DataContext.Jobs.Where(j => tanksToDelete.JobIds.Contains(j.Id)).ToList();
                                    locations.ForEach(t =>
                                    {
                                        t.JobBudget.IsTankAvailable = false;
                                    });
                                    Context.Commit();

                                }

                                assetTankDeleteStatus = await new FreightServiceDomain(this).DeleteJobTanks(tanksToDelete);
                            }
                            if (assetTankDeleteStatus)
                            {
                                trasaction.Commit();
                                response.StatusCode = Status.Success;
                                response.StatusMessage = string.Format(Resource.errMessageAssetDeleteSuccess, ((AssetType)type).GetDisplayName());
                            }
                            else
                            {
                                trasaction.Rollback();
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = string.Format(Resource.errMessageAssetDeleteFailed, ((AssetType)type).GetDisplayName());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        trasaction.Rollback();
                        LogManager.Logger.WriteException("AssetDomain", "DeleteAssetAsync", ex.Message, ex);
                    }
                }
                return response;
            }
        }

        public AssetFilterViewModel GetAssetFilterAsync(int jobId, int companyId, AssetFilterType filter)
        {
            LogManager.Logger.WriteDebug("AssetDomain", "GetAssetFilterAsync", "Start");
            var response = new AssetFilterViewModel();
            
            try
            {
                response.JobId = jobId;
                var appDomain = new ApplicationDomain(this);
                var jobAssetBulkUploadCompany = appDomain.GetApplicationSettingValue<int>(ApplicationConstants.KeyAppSettingJobAssetBulkUplaodCompany);
                response.IsJobAssetBulkUploadEnabled = jobAssetBulkUploadCompany == companyId;
                response.DuplicateExists = Context.DataContext.AssetDuplicates.Any(t => t.CompanyId == companyId);
                if (filter > 0)
                {
                    response.Filter = filter;
                }

                if (jobId > 0)
                    response.IsRetailJob = Context.DataContext.Jobs.Where(t => t.Id == jobId).Select(t => t.IsRetailJob).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("JobDomain", "GetAssetFilterAsync", ex.Message, ex);
            }
            LogManager.Logger.WriteDebug("AssetDomain", "GetAssetFilterAsync", "End");
            return response;
        }

        public AssetValidationViewModel ValidateAssetsAsync(List<AssetFuelRequestViewModel> assetFuelRequests, int jobId)
        {
            using (var tracer = new Tracer("AssetDomain", "ValidateAssetsAsync"))
            {
                AssetValidationViewModel response = new AssetValidationViewModel();
                try
                {
                    //var checkUniqueness = assetFuelRequests.Select(t => t.AssetId).Distinct().Count() == assetFuelRequests.Count();
                    //if (checkUniqueness)
                    //{
                    var jobXassets = Context.DataContext.JobXAssets.Include(t => t.Asset).Include(t => t.Asset.AssetAdditionalDetail)
                                        .Where
                                        (
                                            t => t.JobId == jobId &&
                                            t.Asset.IsActive &&
                                            t.RemovedDate == null &&
                                            t.RemovedBy == null
                                        )
                                        .Select(t => new
                                        {
                                            TelematicsProvider = t.Asset.AssetAdditionalDetail.TelematicsProvider,
                                            AssetId = t.Asset.Id
                                        }).ToList();

                    if (jobXassets != null && jobXassets.Count > 0)
                    {
                        foreach (var item in assetFuelRequests)
                        {
                            var assetDetails = jobXassets.SingleOrDefault(t => t.TelematicsProvider.Equals(item.AssetId));

                            if (assetDetails != null)
                            {
                                response.AssetFuelRequestsList.Add(new AssetFuelRequestInputViewModel
                                {
                                    AssetId = assetDetails.AssetId,
                                    AssetExternalId = item.AssetId,
                                    Latitude = item.Latitude,
                                    Longitude = item.Longitude,
                                    QuantityRequired = item.Quantity,
                                    IsValidAsset = true,
                                });
                            }
                            else
                            {
                                response.InvalidAssetList.Add(item.AssetId);
                            }
                        }
                    }
                    else
                    {
                        response.StatusMessage = string.Format(Resource.errMessageInvalidJob, jobId);
                    }
                    //}
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AssetDomain", "ValidateAssetsAsync", ex.Message, ex);
                }
                return response;
            }
        }

        public async Task<ResponseViewModel> SaveAssetFuelRequests(List<AssetFuelRequestInputViewModel> assetFuelRequests)
        {
            using (var tracer = new Tracer("AssetDomain", "SaveAssetFuelRequests"))
            {
                var response = new ResponseViewModel();
                try
                {
                    List<AssetDropRequest> assetDrops = new List<AssetDropRequest>();
                    assetDrops = assetFuelRequests.ToEntity(assetDrops);
                    if (assetDrops.Count > 0)
                    {
                        using (var transaction = Context.DataContext.Database.BeginTransaction())
                        {
                            try
                            {
                                Context.DataContext.AssetDropRequests.AddRange(assetDrops);
                                await Context.CommitAsync();
                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                LogManager.Logger.WriteException("AssetDomain", "SaveAssetFuelRequests", ex.Message, ex);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AssetDomain", "SaveAssetFuelRequests", ex.Message, ex);
                }
                return response;
            }
        }

        public async Task<StatusViewModel> SaveTankTypes(TankModalTypeViewModel tankTypes)
        {
            var response = new StatusViewModel();
            using (var tracer = new Tracer("AssetDomain", "SaveTankTypes"))
            {
                response = await new FreightServiceDomain().SaveTankTypes(tankTypes);
            }
            return response;
        }
        public async Task<ResponseViewModel> AssignAssetSubcontractor(UserContext userContext, int assetId, int subcontractorId, int jobId)
        {
            using (var tracer = new Tracer("AssetDomain", "AssignAssetSubcontractor"))
            {
                var response = new ResponseViewModel(Status.Success);
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        var asset = await Context.DataContext.Assets.SingleOrDefaultAsync(t => t.Id == assetId);
                        if (asset != null)
                        {
                            var job = await Context.DataContext.Jobs.FirstOrDefaultAsync(t => t.Id == jobId);
                            if (subcontractorId > 0)
                            {
                                bool isSubcontractorExist = false;
                                string prevSubcontractorName = string.Empty;
                                var assetSubcontractors = asset.AssetSubcontractors.Where(t => !(t.SubcontractorId == subcontractorId && t.IsActive) && t.IsActive).ToList();
                                if (assetSubcontractors.Any())
                                {
                                    isSubcontractorExist = true;
                                    prevSubcontractorName = assetSubcontractors.FirstOrDefault().Subcontractor.Name;
                                    //// set existing subcontractor as inactive
                                    assetSubcontractors.ForEach(t => { t.RemovedBy = userContext.Id; t.RemovedDate = DateTimeOffset.Now; t.IsActive = false; });
                                }

                                if (!asset.AssetSubcontractors.Any(t => t.SubcontractorId == subcontractorId && t.IsActive))
                                {
                                    AssetSubcontractor assetSubcontractor = new AssetSubcontractor() { SubcontractorId = subcontractorId, AssignedBy = userContext.Id, AssignedDate = DateTimeOffset.Now, IsActive = true };
                                    asset.AssetSubcontractors.Add(assetSubcontractor);
                                    var subcontractor = Context.DataContext.Subcontractors.FirstOrDefault(t => t.Id == subcontractorId);
                                    response.StatusMessage = string.Format(Resource.errMessageSubcontractorAssignmentSuccess, new object[] { subcontractor.Name, asset.Name });

                                    //// newsfeed for update subcontractor
                                    if (isSubcontractorExist)
                                    {
                                        await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetSubcontractorAssetAssignNewsfeed(userContext, NewsfeedEvent.SubContractorUpdatedForAsset, job, subcontractor.Name, asset.Name, prevSubcontractorName);
                                    }
                                    else
                                    {
                                        //// newsfeed for add subcontractor
                                        await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetSubcontractorAssetAssignNewsfeed(userContext, NewsfeedEvent.SubContractorAddedToAsset, job, subcontractor.Name, asset.Name);
                                    }
                                }
                            }
                            else
                            {
                                var subcontractor = asset.AssetSubcontractors.FirstOrDefault(t => t.IsActive);
                                subcontractor.RemovedBy = userContext.Id;
                                subcontractor.RemovedDate = DateTimeOffset.Now;
                                subcontractor.IsActive = false;
                                response.StatusMessage = string.Format(Resource.errMessageRemovingSubcontractorSuccess, new object[] { subcontractor.Subcontractor.Name, asset.Name });

                                //// newsfeed for removing subcontractor
                                await ContextFactory.Current.GetDomain<NewsfeedDomain>().SetSubcontractorAssetAssignNewsfeed(userContext, NewsfeedEvent.SubContractorRemovedFromAsset, job, subcontractor.Subcontractor.Name, asset.Name);
                            }


                            Context.DataContext.Entry(asset).State = EntityState.Modified;
                            await Context.CommitAsync();
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageSubcontractorAssignmentFailed;
                        LogManager.Logger.WriteException("AssetDomain", "AssignAssetSubcontractor", ex.Message, ex);
                    }
                }
                return response;
            }
        }

        public List<DropdownDisplayItem> GetSubContractors(int? jobId, int companyId)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                response = Context.DataContext
                                                .Subcontractors
                                                .Where(t => t.IsActive && (jobId == null || jobId == 0 || t.Jobs.Any(t1 => t1.Id == jobId))
                                                            && t.Jobs.Any(t1 => t1.CompanyId == companyId))
                                                .Select(t => new DropdownDisplayItem
                                                {
                                                    Id = t.Id,
                                                    Name = t.Name
                                                }).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AssetDomain", "GetSubContractors", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> AddAmpDropStreamAssets(AmpJobViewModel viewModel, Job job, int userId)
        {
            var response = new StatusViewModel();
            try
            {
                var AmpDropStreamAssetNames = viewModel.Drops.Select(t => t.AssetName);
                var jobExistingAssets = Context.DataContext.Assets.Where(t => t.IsActive && t.CompanyId == viewModel.BuyerCompanyId && AmpDropStreamAssetNames.Contains(t.Name)).ToList();
                var jobExistingAssetNames = jobExistingAssets.Select(t => new { t.Name, t.Id });
                var assetsTobeAdded = AmpDropStreamAssetNames.Where(t => !jobExistingAssetNames.Select(t1 => t1.Name).Contains(t))
                    .Select(t => new Asset { Name = t, IsActive = true, AssetAdditionalDetail = new AssetAdditionalDetail() }).ToList();
                var result = await SaveAssetList(job.CreatedBy, job.Id, assetsTobeAdded);
                if (result.StatusCode == Status.Success)
                {
                    jobExistingAssets = Context.DataContext.Assets.Where(t => t.IsActive && t.CompanyId == viewModel.BuyerCompanyId && AmpDropStreamAssetNames.Contains(t.Name)).ToList();
                    var AssetsToRemoveFromOtherJobs = jobExistingAssets.SelectMany(t => t.JobXAssets.Where(t1 => t1.RemovedBy == null && t1.JobId != job.Id));
                    foreach (var jobXAsset in AssetsToRemoveFromOtherJobs)
                    {
                        jobXAsset.RemovedBy = userId;
                        jobXAsset.RemovedDate = DateTimeOffset.Now;
                        Context.DataContext.Entry(jobXAsset).State = EntityState.Modified;
                    }
                    var jobExistingAssetIds = jobExistingAssets.Select(t => t.Id);
                    var jobExistingAssignedAssets = Context.DataContext.JobXAssets.Where(t => jobExistingAssetIds.Contains(t.AssetId) && t.JobId == job.Id && t.RemovedBy == null);
                    var assetTobeAssigned = jobExistingAssetIds.Where(t => !jobExistingAssignedAssets.Select(t1 => t1.AssetId).Contains(t)).ToList();
                    assetTobeAssigned.ForEach(t => job.JobXAssets.Add(new JobXAsset { AssetId = t, AssignedBy = userId, AssignedDate = DateTimeOffset.Now }));
                    await Context.CommitAsync();
                    response.StatusCode = Status.Success;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AssetDomain", "AddAmpDropStreamAssets", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<TankModalTypeViewModel>> GetTankTypesByCompanyAsync(int companyId)
        {
            using (var tracer = new Tracer("AssetDomain", "GetTankTypesByCompanyAsync"))
            {
                var response = new List<TankModalTypeViewModel>();

                try
                {
                    response = await new FreightServiceDomain(this).GetTankTypesByCompany(companyId);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AssetDomain", "GetTankTypesByCompanyAsync", ex.Message, ex);
                }

                return response;
            }
        }
        public async Task<StatusViewModel> DeleteTankDipChartById(string id)
        {
            using (var tracer = new Tracer("AssetDomain", "DeleteTankDipChartById"))
            {
                var response = new StatusViewModel();

                try
                {
                    response = await new FreightServiceDomain(this).DeleteTankDipChartById(id);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AssetDomain", "DeleteTankDipChartById", ex.Message, ex);
                }

                return response;
            }
        }



        public async Task<StatusViewModel> SaveTractorDetailsAsync(UserContext userContext, TractorDetailViewModel viewModel)
        {
            using (var tracer = new Tracer("AssetDomain", "SaveTractorDetailsAsync"))
            {
                StatusViewModel response = new StatusViewModel();
                if (viewModel != null)
                {
                    if (string.IsNullOrEmpty(viewModel.Id))
                        response = await CreateTractorAsync(userContext, viewModel, response);
                    else
                        response = await UpdateTractorAsync(userContext, viewModel, response);
                }
                return response;
            }
        }
        private static async Task<StatusViewModel> CreateTractorAsync(UserContext userContext, TractorDetailViewModel viewModel, StatusViewModel response)
        {
            viewModel.TfxCreatedBy = userContext.Id;
            viewModel.TfxCompanyId = userContext.CompanyId;
            response = await new FreightServiceDomain().SaveTractorDetails(viewModel);
            if (response != null && response.StatusCode == Status.Success)
                response.StatusMessage = string.Format($"{viewModel.TractorId} tractor created successfully");
            return response;
        }
        private static async Task<StatusViewModel> UpdateTractorAsync(UserContext userContext, TractorDetailViewModel viewModel, StatusViewModel response)
        {
            viewModel.TfxCreatedBy = userContext.Id;
            viewModel.TfxCompanyId = userContext.CompanyId;
            response = await new FreightServiceDomain().UpdateTractorDetails(viewModel);
            if (response != null && response.StatusCode == Status.Success)
                response.StatusMessage = string.Format($"{viewModel.TractorId} tractor updated successfully");
            return response;
        }

        public List<DropdownDisplayItem> GetAssetOrTankListAsync(int userId, int? jobId, int type, int assetId = 0)
        {
            using (var tracer = new Tracer("AssetDomain", "GetAssetOrTankListAsync"))
            {
                List<DropdownDisplayItem> response = new List<DropdownDisplayItem>();
                try
                {
                    var jobDetails = Context.DataContext.Jobs.Where(t => t.Id == jobId).Select(t => new { t.CompanyId }).SingleOrDefault();
                    if (jobDetails != null)
                    {
                        var assignedAssets = Context.DataContext.JobXAssets.Where(t => t.JobId == jobId
                                             && t.Asset.Type == type
                                             && t.Asset.CompanyId == jobDetails.CompanyId
                                             && t.RemovedBy == null && t.RemovedDate == null && t.Asset.Id != assetId)
                                                .Select(t => new
                                                {
                                                    t.Asset.Id,
                                                    t.Asset.Name,
                                                    t.Asset.ImageId,
                                                    t.Asset.Type,
                                                }).ToList();

                        assignedAssets.ForEach(t => response.Add(new DropdownDisplayItem { Id = t.Id, Name = t.Name }));
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AssetDomain", "GetAssetOrTankListAsync", ex.Message, ex);
                }
                return response;
            }
        }
        public string GetTanksName(List<int> TanksId, List<TankDetailViewModel> tankAdditionalList)
        {
            string response = "";
            using (var tracer = new Tracer("AssetDomain", "GetTanksName"))
            {
                try
                {
                    var assignedAssets = tankAdditionalList.Where(t => TanksId.Any(item => item == t.AssetId))
                                            .Select(t => new
                                            {
                                                t.TankName
                                            }).ToList();
                    if (assignedAssets != null && assignedAssets.Any())
                    {
                        foreach (var item in assignedAssets)
                        {
                            if (response == "")
                            {
                                response = item.TankName;
                            }
                            else
                            {
                                response += ", " + item.TankName;
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AssetDomain", "GetTanksName", ex.Message, ex);
                }
                return response;
            }
        }

        public int GetBuyerCompanyId(int jobId)
        {
            int response = 0;
            using (var tracer = new Tracer("AssetDomain", "GetBuyerCompanyId"))
            {

                try
                {
                    var jobDetails = Context.DataContext.Jobs.Where(t => t.Id == jobId).Select(t => new { t.CompanyId }).SingleOrDefault();
                    if (jobDetails != null)
                    {
                        response = jobDetails.CompanyId;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AssetDomain", "GetBuyerCompanyId", ex.Message, ex);
                }
                return response;
            }
        }

        public async Task<AssetResponseModel> GetAssets(string token, string fromDate = "", string toDate = "", int userId = 0)
        {
            var response = new AssetResponseModel();
            try
            {
                var authDomain = new AuthenticationDomain(this);
                var apiUserContext = Task.Run(() => authDomain.GetUserContextFromTokenAsync(token)).Result;
                if (apiUserContext != null)
                {
                    if (apiUserContext.CompanyTypeId == CompanyType.Buyer || apiUserContext.CompanyTypeId == CompanyType.BuyerAndSupplier || apiUserContext.CompanyTypeId == CompanyType.BuyerSupplierAndCarrier)
                    {
                        var spDomain = new StoredProcedureDomain(this);
                        var result = await spDomain.GetAssets(apiUserContext.CompanyId, userId, fromDate, toDate);

                        response.StatusCode = Status.Success;
                        if (result != null && result.Any())
                        {
                            response.ResponseData = result;
                            response.StatusMessage = Status.Success.ToString();
                        }
                        else
                        {
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
                LogManager.Logger.WriteException("AssetDomain", "GetAssets", ex.Message, ex);
                response.StatusCode = Status.Failed;
                response.StatusMessage = Status.Failed.ToString();
            }

            return response;
        }

        public async Task<AssetResponseModel> GetCustomerAssets(string token, string fromDate = "", string toDate = "", int userId = 0)
        {
            var response = new AssetResponseModel();
            try
            {
                var authDomain = new AuthenticationDomain(this);
                var apiUserContext = Task.Run(() => authDomain.GetUserContextFromTokenAsync(token)).Result;
                if (apiUserContext != null)
                {
                    if (apiUserContext.CompanyTypeId != CompanyType.Buyer)
                    {
                        var spDomain = new StoredProcedureDomain(this);
                        var result = await spDomain.GetCustomerAssets(apiUserContext.CompanyId, userId, fromDate, toDate, AssetType.Asset);

                        response.StatusCode = Status.Success;
                        if (result != null && result.Any())
                        {
                            response.ResponseData = result;
                            response.StatusMessage = Status.Success.ToString();
                        }
                        else
                        {
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
                LogManager.Logger.WriteException("AssetDomain", "GetCustomerAssets", ex.Message, ex);
                response.StatusCode = Status.Failed;
                response.StatusMessage = Status.Failed.ToString();
            }

            return response;
        }

        public async Task<AssetResponseModel> GetTanks(string token, string fromDate = "", string toDate = "", int userId = 0)
        {
            var response = new AssetResponseModel();
            try
            {
                var authDomain = new AuthenticationDomain(this);
                var apiUserContext = Task.Run(() => authDomain.GetUserContextFromTokenAsync(token)).Result;
                if (apiUserContext != null)
                {
                    if (apiUserContext.CompanyTypeId == CompanyType.Buyer || apiUserContext.CompanyTypeId == CompanyType.BuyerAndSupplier || apiUserContext.CompanyTypeId == CompanyType.BuyerSupplierAndCarrier)
                    {
                        var spDomain = new StoredProcedureDomain(this);
                        var tanks = await spDomain.GetTanks(apiUserContext.CompanyId, userId, fromDate, toDate, AssetType.Tank);

                        if (tanks != null && tanks.Any())
                        {
                            var tankIds = tanks.Select(t => t.AssetId).ToList();
                            var tankAdditionalList = await new FreightServiceDomain(this).GetTankList(tankIds);
                            if (tankAdditionalList != null && tankAdditionalList.Any())
                            {
                                foreach (var tankDetail in tankAdditionalList)
                                {
                                    var tank = tanks.Where(t => t.AssetId == tankDetail.AssetId).FirstOrDefault();
                                    if (tank != null)
                                    {
                                        tank.Make = !string.IsNullOrEmpty(tankDetail.TankMake) ? tankDetail.TankMake : Resource.lblHyphen;
                                        tank.Model = !string.IsNullOrEmpty(tankDetail.TankModel) ? tankDetail.TankModel : Resource.lblHyphen;
                                        tank.Threshold = tankDetail.ThresholdDeliveryRequest.HasValue ? $"{tankDetail.ThresholdDeliveryRequest.Value.GetPreciseValue(2)}{Resource.constSymbolPercent}" : Resource.lblHyphen;
                                        tank.DipTestMethod = tankDetail.DipTestMethod.HasValue ? ((int)tankDetail.DipTestMethod) : (int)DipTestMethod.Select;
                                        tank.DipTestMethodType = tankDetail.DipTestMethod.HasValue ? ((DipTestMethod)tankDetail.DipTestMethod).GetDisplayName() : Resource.lblHyphen;
                                        tank.TankChart = !string.IsNullOrEmpty(tankDetail.TankChart) ? tankDetail.TankChart : Resource.lblHyphen;
                                        if (tankDetail.TanksConnected != null && tankDetail.TanksConnected.Any())
                                            tank.TanksConnectedNames = GetTanksName(tankDetail.TanksConnected, tankAdditionalList);
                                        tank.TankSequence = tankDetail.TankSequence;
                                        tank.Manufacturer = tankDetail.Manufacturer;
                                        tank.IsManifold = tankDetail.ManiFolded == null || tankDetail.ManiFolded == ManiFolded.Select ? "NA" : (tankDetail.ManiFolded == ManiFolded.Yes ? "Y" : "N");
                                        tank.ConstructionType = tankDetail.TankConstruction == null || tankDetail.TankConstruction == TankConstruction.Select ? Resource.lblHyphen : tankDetail.TankConstruction.GetDisplayName();
                                    }
                                }
                            }

                            response.ResponseData = tanks;
                            response.StatusCode = Status.Success;
                            response.StatusMessage = Status.Success.ToString();
                        }
                        else
                        {
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
                LogManager.Logger.WriteException("AssetDomain", "GetTanks", ex.Message, ex);
                response.StatusCode = Status.Failed;
                response.StatusMessage = Status.Failed.ToString();
            }

            return response;
        }

        public async Task<AssetResponseModel> GetCustomerTanks(string token, string fromDate = "", string toDate = "", int userId = 0)
        {
            var response = new AssetResponseModel();
            try
            {
                var authDomain = new AuthenticationDomain(this);
                var apiUserContext = Task.Run(() => authDomain.GetUserContextFromTokenAsync(token)).Result;
                if (apiUserContext != null)
                {
                    if (apiUserContext.CompanyTypeId != CompanyType.Buyer)
                    {
                        var spDomain = new StoredProcedureDomain(this);
                        var customerTanks = await spDomain.GetCustomerTanks(apiUserContext.CompanyId, userId, fromDate, toDate, AssetType.Tank);

                        if (customerTanks != null && customerTanks.Any())
                        {
                            var tankIds = customerTanks.Select(t => t.AssetId).ToList();
                            var tankAdditionalList = await new FreightServiceDomain(this).GetTankList(tankIds);
                            if (tankAdditionalList != null && tankAdditionalList.Any())
                            {
                                foreach (var tankDetail in tankAdditionalList)
                                {
                                    var tank = customerTanks.Where(t => t.AssetId == tankDetail.AssetId).FirstOrDefault();
                                    if (tank != null)
                                    {
                                        tank.Make = !string.IsNullOrEmpty(tankDetail.TankMake) ? tankDetail.TankMake : Resource.lblHyphen;
                                        tank.Model = !string.IsNullOrEmpty(tankDetail.TankModel) ? tankDetail.TankModel : Resource.lblHyphen;
                                        tank.Threshold = tankDetail.ThresholdDeliveryRequest.HasValue ? $"{tankDetail.ThresholdDeliveryRequest.Value.GetPreciseValue(2)}{Resource.constSymbolPercent}" : Resource.lblHyphen;
                                        tank.DipTestMethod = tankDetail.DipTestMethod.HasValue ? ((int)tankDetail.DipTestMethod) : (int)DipTestMethod.Select;
                                        tank.DipTestMethodType = tankDetail.DipTestMethod.HasValue ? ((DipTestMethod)tankDetail.DipTestMethod).GetDisplayName() : Resource.lblHyphen;
                                        tank.TankChart = !string.IsNullOrEmpty(tankDetail.TankChart) ? tankDetail.TankChart : Resource.lblHyphen;
                                        if (tankDetail.TanksConnected != null && tankDetail.TanksConnected.Any())
                                            tank.TanksConnectedNames = GetTanksName(tankDetail.TanksConnected, tankAdditionalList);
                                        tank.TankSequence = tankDetail.TankSequence;
                                        tank.Manufacturer = tankDetail.Manufacturer;
                                        tank.IsManifold = tankDetail.ManiFolded == null || tankDetail.ManiFolded == ManiFolded.Select ? "NA" : (tankDetail.ManiFolded == ManiFolded.Yes ? "Y" : "N");
                                        tank.ConstructionType = tankDetail.TankConstruction == null || tankDetail.TankConstruction == TankConstruction.Select ? Resource.lblHyphen : tankDetail.TankConstruction.GetDisplayName();
                                    }
                                }
                            }

                            response.ResponseData = customerTanks;
                            response.StatusCode = Status.Success;
                            response.StatusMessage = Status.Success.ToString();
                        }
                        else
                        {
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
                LogManager.Logger.WriteException("AssetDomain", "GetCustomerTanks", ex.Message, ex);
                response.StatusCode = Status.Failed;
                response.StatusMessage = Status.Failed.ToString();
            }

            return response;
        }

        public async Task<TankInventoryResponseModel> GetTankInventoryDetails(string token,bool isFromExchangeApiForDataExpose)
        {
            var response = new TankInventoryResponseModel();
            try
            {
                var salesData = new List<SalesDataModel>();
                var authDomain = new AuthenticationDomain(this);
                var salesDomain = new SalesDomain(authDomain);
                var apiUserContext = Task.Run(() => authDomain.GetUserContextFromTokenAsync(token)).Result;
                if (apiUserContext != null)
                {
                    if(apiUserContext.CompanyTypeId == CompanyType.Buyer || apiUserContext.CompanyTypeId == CompanyType.BuyerAndSupplier || apiUserContext.CompanyTypeId == CompanyType.BuyerSupplierAndCarrier)
                    {
                        if (apiUserContext.CompanyTypeId == CompanyType.Buyer)
                        {
                            salesData = await salesDomain.GetBuyerSalesDataAsync(apiUserContext.CompanyId,"", 0, 1,0,false,"","",true,"", isFromExchangeApiForDataExpose);
                        }
                        else
                        {
                            salesData = await salesDomain.GetBuyerSalesDataAsync(apiUserContext.CompanyId,"", 0, 1,0, false, "", "", true, "", isFromExchangeApiForDataExpose);
                            var salesDataForSupplier = await salesDomain.GetSalesDataForExternalResourceAsync(apiUserContext.CompanyId,"", "", "", 0, 1, "",true,"", isFromExchangeApiForDataExpose);
                            if (salesDataForSupplier.Any())
                                salesData.AddRange(salesDataForSupplier);
                        }
                    }
                    else if(apiUserContext.CompanyTypeId == CompanyType.Carrier)
                    {
                        salesData = await salesDomain.GetSalesDataForExternalResourceAsync(apiUserContext.CompanyId, "", "", "", 0, 1, "",true,"", isFromExchangeApiForDataExpose);
                    }
                    else
                    {
                        salesData = await salesDomain.GetSalesDataForExternalResourceAsync(apiUserContext.CompanyId, "", "", "", 0, 1, "",true,"", isFromExchangeApiForDataExpose);
                    }

                    //var spDomain = new StoredProcedureDomain(this);
                    //var tankInventoryDetails = await spDomain.GetTankInventoryDetails(apiUserContext.CompanyId);

                    if (salesData != null && salesData.Any())
                    {
                        var tankInventoryDetails = new List<TankInventoryDataCaptureResponseModel>();
                        salesData = salesData.Distinct().ToList();
                        salesData.ForEach(t => tankInventoryDetails.Add(t.ToSalesViewModel()));

                        // get job details from exchange
                        var jobIds = salesData.Select(t => t.TfxJobId).ToList();
                        var jobDetails = await Context.DataContext.Jobs.Where(t => jobIds.Contains(t.Id))
                                                                       .Select(t => new
                                                                       {
                                                                           t.Id,
                                                                           t.UoM,
                                                                           t.CountryId
                                                                       }).ToListAsync();
                        foreach (var item in tankInventoryDetails)
                        {
                            var uoM = jobDetails.Where(t => t.Id == item.JobId).Select(t => t.CountryId == (int)Country.CAN ? UoM.Litres : UoM.Gallons).FirstOrDefault();
                            item.UoM = uoM.GetDisplayName();
                        }

                        response.ResponseData = tankInventoryDetails;
                        response.StatusCode = Status.Success;
                        response.StatusMessage = Status.Success.ToString();
                    }
                    else
                    {
                        response.ResponseData = new List<TankInventoryDataCaptureResponseModel>();
                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.lblNoDataAvailable;
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
                LogManager.Logger.WriteException("AssetDomain", "GetTankInventoryDetails", ex.Message, ex);
                response.StatusCode = Status.Failed;
                response.StatusMessage = Status.Failed.ToString();
            }

            return response;
        }
        public async Task<StatusViewModel> UploadAssetFileToBlob(UserContext userContext, Stream fileStream, string fileName, CompanyType companyType)
        {
            using (var tracer = new Tracer("AssetDomain", "UploadAssetFileToBlob"))
            {
                var response = new StatusViewModel(Status.Failed);
                try
                {
                    var azureBlob = new AzureBlobStorage();

                    var filePath = await azureBlob.SaveBlobAsync(fileStream, GenerateFileName(userContext.Id), BlobContainerType.JobAssetbulkupload.ToString().ToLower());
                    if (!string.IsNullOrWhiteSpace(filePath))
                    {
                        var queueDomain = new QueueMessageDomain();
                        var queueRequest = GetEnqueueMessageRequestViewModel(userContext, filePath, companyType);
                        var queueId = queueDomain.EnqeueMessage(queueRequest);

                        response.StatusCode = Status.Success;
                        response.StatusMessage = string.Format(Resource.successMessageOrderBulkWithRequestNo, string.Concat(Constants.AssetBulk, queueId.ToString("000")));
                    }
                    else
                        response.StatusMessage = Resource.errMessageErrorInAzureServer;
                }
                catch (Exception ex)
                {
                    response.StatusMessage = Resource.errMessageErrorInAzureServer;
                    LogManager.Logger.WriteException("AssetDomain", "UploadAssetFileToBlob", ex.Message, ex);
                }
                return response;
            }
        }
        private string GenerateFileName(int userId)
        {
            return string.Concat(values: Constants.AssetBulk + Resource.lblSingleHyphen + userId + Resource.lblSingleHyphen + DateTime.Now.Ticks + ".csv");
        }
        private QueueMessageViewModel GetEnqueueMessageRequestViewModel(UserContext userContext, string blobStoragePath, CompanyType companyType)
        {
            var jsonViewModel = new AssetBulkUploadProcessorReqViewModel();
            jsonViewModel.FileLineNumberToStart = 0;
            jsonViewModel.UserId = userContext.Id;
            jsonViewModel.CompanyId = userContext.CompanyId;
            jsonViewModel.FileUploadPath = blobStoragePath;
            jsonViewModel.CompanyType = companyType;
            string json = JsonConvert.SerializeObject(jsonViewModel);

            return new QueueMessageViewModel()
            {
                CreatedBy = userContext.Id,
                QueueProcessType = QueueProcessType.AssetBulkUpload,
                JsonMessage = json
            };
        }

        public string ProcessAssetBulkUploadJsonMessage(AssetBulkUploadProcessorReqViewModel bulkRequestViewModel, List<string> errorInfo)
        {
            using (var tracer = new Tracer("AssetBulkUploadDomain", "ProcessAssetBulkUploadJsonMessage"))
            {
                StringBuilder processMessage = new StringBuilder();

                try
                {
                    if (!string.IsNullOrWhiteSpace(bulkRequestViewModel.FileUploadPath))
                    {
                        //processing actual bulk file
                        var azureBlob = new AzureBlobStorage();
                        var fileStream = azureBlob.DownloadBlob(bulkRequestViewModel.FileUploadPath, BlobContainerType.JobAssetbulkupload.ToString().ToLower());
                        if (fileStream != null)
                        {
                            Task.Run(() => ProcessTpoAssetBulkFile(bulkRequestViewModel, errorInfo, processMessage, fileStream)).Wait();
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (!(ex is QueueMessageFatalException))
                        LogManager.Logger.WriteException("AssetDomain", "ProcessAssetBulkUploadJsonMessage", ex.Message, ex);
                    if (processMessage.Length == 0)
                    {
                        processMessage.Append(Constants.ErrorWhileProcessingBulkOrder);
                        errorInfo.Add(processMessage.ToString());
                    }
                    throw new QueueMessageFatalException(errorInfo[0], errorInfo);
                }
                return processMessage.ToString();
            }
        }
        private async Task ProcessTpoAssetBulkFile(AssetBulkUploadProcessorReqViewModel bulkRequestViewModel, List<string> errorInfo, StringBuilder processMessage, Stream fileStream)
        {
            using (var tracer = new Tracer("AssetDomain", "ProcessTpoAssetBulkFile"))
            {
                var csvList = ReadCSVFile<AssetTPOBulkRecordViewModel>(fileStream, true);
                if (csvList != null)
                {
                    AuthenticationDomain authenticationDomain = new AuthenticationDomain(this);
                    var lineNumber = 0;
                    foreach (var item in csvList)
                    {
                        lineNumber++;
                        if (IsRequiredFieldMissing(item))
                        {
                            errorInfo.Add(SetMissingFieldProcessMessage(string.Format(Resource.errMsgRequiredFieldAreMissing, lineNumber)));
                        }
                    }

                    var companyAssets = csvList.Where(t => !IsRequiredFieldMissing(t)).GroupBy(t => new { t.Customer, t.SiteName })
                                                                .Select(t => new { t.Key.Customer, t.Key.SiteName, Assets = t.ToList() }).ToList();
                    var productTypesList = new MasterDomain(this).GetProductTypes();
                    foreach (var item in companyAssets)
                    {
                        try
                        {
                            processMessage.Clear();
                            var company = Context.DataContext.Companies.Where(t => t.Name.ToLower() == item.Customer.ToLower())
                                                .Select(t => new {
                                                    t.Id,
                                                    Jobs = t.Jobs.Where(t1 => t1.Name.ToLower() == item.SiteName.ToLower()
                                                                               && t1.IsActive).Select(t2 => new { t2.Id, t2.CreatedBy }).FirstOrDefault()
                                                })
                                                .FirstOrDefault();
                            if (company == null || company.Id == 0)
                            {
                                SetFailedProcessMessage(processMessage, item.SiteName, ($"{item.Customer} not exists. Failed to process the records for {item.Customer} customer.")) ;
                                errorInfo.Add(processMessage.ToString());
                                continue;
                            }
                            else if (company.Jobs == null || company.Jobs.Id == 0)
                            {
                                SetFailedProcessMessage(processMessage, item.SiteName, ($"{item.SiteName} not exists. Failed to process the records for {item.SiteName} job."));
                                errorInfo.Add(processMessage.ToString());
                                continue;
                            }
                            List<Asset> assets = new List<Asset>();
                            foreach (var asset in item.Assets)
                            {
                                var productType = productTypesList.FirstOrDefault(t => t.Name.ToLower() == asset.FuelType.ToLower());
                                assets.Add(GetAssetObject(asset, company.Jobs.CreatedBy, company.Id, productType));
                            }
                            var result = await SaveAssetList(company.Jobs.CreatedBy, company.Jobs.Id, assets);
                            if(result.StatusCode == (int)Status.Success)
                            {
                                var assetNames = assets.Select(t => t.Name).ToList();
                                errorInfo.Add(SetSuccessProcessMessage(string.Join(", ", assetNames)));
                            }
                            else
                            {
                                SetFailedProcessMessage(processMessage, item.SiteName, result.StatusMessage);
                                errorInfo.Add(processMessage.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            SetFailedProcessMessage(processMessage, item.SiteName, Constants.ErrorWhileProcessingBulkOrder);
                            errorInfo.Add(processMessage.ToString());
                        }
                    }
                }
                else
                {
                    processMessage.Append(Resource.errMessageFailedToReadFileContent);
                }
            }
        }

        private bool IsRequiredFieldMissing(AssetTPOBulkRecordViewModel record)
        {
            return string.IsNullOrWhiteSpace(record.SiteName) || string.IsNullOrWhiteSpace(record.Customer) || string.IsNullOrWhiteSpace(record.Name);
        }
        private static string SetMissingFieldProcessMessage(string msg)
        {
            StringBuilder dispMessage = new StringBuilder();
            dispMessage.Append("<p class='color-maroon'>").Append("<b>Info: </b>")
                        .Append($"{msg}</p><br>");
            return dispMessage.ToString();
        }

        private static void SetFailedProcessMessage(StringBuilder processMessage, string siteName, string message)
        {
            processMessage.Append("<p class='color-maroon'>").Append("<b>Info: </b>")
                        .Append($"Assets of Site Name: {siteName} <br><b>Processing failed Reason:</b> {message}</p><br>");
        }

        private static string SetSuccessProcessMessage(string assets)
        {
            StringBuilder processMessage = new StringBuilder();
            processMessage.Append("<p class='color-green'>").Append("<b>Info: </b>")
                        .Append($"Assets: {assets} <br><b>processed successfully</b></p><br>");
            return processMessage.ToString();
        }

        #region Unauthenticated Requests
        public async Task<UnAthorizedInventoryData> GetInventoryDetailsForUnauthorizedUser(string token, int supplierCompanyId)
        {
            var response = new UnAthorizedInventoryData();
            response.SupplierList.Add(new DropdownDisplayItem() { Id = 0, Name = "Select Supplier" });
            response.CompanyToken = token;
            try
            {
                bool isFromExchangeApiForDataExpose = true;
                int decodedCompanyId = token.ToIntNumber();

                if (decodedCompanyId > 0)
                {
                    var salesData = new List<SalesDataModel>();
                    var authDomain = new AuthenticationDomain(this);
                    var salesDomain = new SalesDomain(authDomain);
                    var companyInfo = Context.DataContext.Companies.Where(t => t.Id == decodedCompanyId && t.IsActive && !t.IsDeleted)
                                            .Select(t => new { t.Id, t.CompanyTypeId }).SingleOrDefault();
                    if (companyInfo != null)
                    {
                        //if (companyInfo.CompanyTypeId == (int)CompanyType.Buyer || companyInfo.CompanyTypeId == (int)CompanyType.BuyerAndSupplier || companyInfo.CompanyTypeId == (int)CompanyType.BuyerSupplierAndCarrier)
                        //{
                        //    if (companyInfo.CompanyTypeId == (int)CompanyType.Buyer)
                        //    {
                        //        salesData = await salesDomain.GetBuyerSalesDataAsync(companyInfo.Id, "", 0, 1, 0, false, "", "", true, "", isFromExchangeApiForDataExpose);
                        //    }
                        //    else
                        //    {
                        //        salesData = await salesDomain.GetBuyerSalesDataAsync(companyInfo.Id, "", 0, 1, 0, false, "", "", true, "", isFromExchangeApiForDataExpose);
                        //        var salesDataForSupplier = await salesDomain.GetSalesDataAsync(companyInfo.Id, "", "", "", 0, 1, false, "", true, "", isFromExchangeApiForDataExpose);
                        //        if (salesDataForSupplier.Any())
                        //            salesData.AddRange(salesDataForSupplier);
                        //    }
                        //}
                        //else if (companyInfo.CompanyTypeId == (int)CompanyType.Carrier)
                        //{
                        //    string customerId = supplierCompanyId > 0 ? supplierCompanyId.ToString() : "";
                        //    salesData = await salesDomain.GetSalesDataAsync(companyInfo.Id, "",customerId, "", 0, 1, true, "", true, "", isFromExchangeApiForDataExpose);
                        //}
                        //else
                        //{
                        //    salesData = await salesDomain.GetSalesDataAsync(companyInfo.Id, "", "", "", 0, 1, false, "", true, "", isFromExchangeApiForDataExpose);
                        //}

                        if (companyInfo.CompanyTypeId == (int)CompanyType.Carrier || companyInfo.CompanyTypeId == (int)CompanyType.SupplierAndCarrier || companyInfo.CompanyTypeId == (int)CompanyType.BuyerSupplierAndCarrier)
                        {
                            string customerId = supplierCompanyId > 0 ? supplierCompanyId.ToString() : "";
                            salesData = await salesDomain.GetSalesDataForExternalResourceAsync(companyInfo.Id, "", customerId, "", 0, 1, "", true, "", 
                                                    isFromExchangeApiForDataExpose, response.SupplierList);
                        }

                        if (salesData != null && salesData.Any())
                        {
                            var tankInventoryDetails = new List<TankInventoryDataCaptureResponseModel>();
                            salesData = salesData.Distinct().ToList();
                            salesData.ForEach(t => tankInventoryDetails.Add(t.ToSalesViewModel()));

                            // get job details from exchange
                            var jobIds = salesData.Select(t => t.TfxJobId).ToList();
                            var jobDetails = await Context.DataContext.Jobs.Where(t => jobIds.Contains(t.Id))
                                                                           .Select(t => new
                                                                           {
                                                                               t.Id,
                                                                               t.UoM,
                                                                               t.CountryId
                                                                           }).ToListAsync();
                            foreach (var item in tankInventoryDetails)
                            {
                                var uoM = jobDetails.Where(t => t.Id == item.JobId).Select(t => t.CountryId == (int)Country.CAN ? UoM.Litres : UoM.Gallons).FirstOrDefault();
                                item.UoM = uoM.GetDisplayName();
                            }

                            response.Data = tankInventoryDetails;
                            response.StatusCode = Status.Success;
                            response.StatusMessage = Status.Success.ToString();
                        }
                        else
                        {
                            response.Data = new List<TankInventoryDataCaptureResponseModel>();
                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.lblNoDataAvailable;
                        }
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMsgInvalidToken;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AssetDomain", "GetInventoryDetailsForUnauthorizedUser", ex.Message, ex);
                response.StatusCode = Status.Failed;
                response.StatusMessage = Status.Failed.ToString();
            }

            return response;
        }

        #endregion
    }
}
