using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers.Forcasting;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Forcasting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class ForcastingServiceDomain : BaseDomain
    {
        public ForcastingServiceDomain()
            : base(ContextFactory.Current.ConnectionString)
        {
        }

        public ForcastingServiceDomain(BaseDomain domain)
            : base(domain)
        {
        }
        public async Task<StatusViewModel> SaveForeCastingPreferanceSetting(ForcastingPreferenceViewModel forcastingModel, UserContext userContext, int forcastingSettingLevel, int entityId, int prevaccountEntityId = 0, int tpoBuyerCompanyId = 0)
        {
            var statusModel = new StatusViewModel();
            statusModel.StatusCode = Status.Success;
            if (forcastingModel.IsEditable)
            {
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        IntializeforcastingModel(forcastingModel, userContext, forcastingSettingLevel, entityId, prevaccountEntityId, tpoBuyerCompanyId);
                        if (forcastingModel.ForcastingServiceSetting.IsEnabled)
                        {
                            switch (forcastingModel.ForcastingSettingLevel)
                            {
                                case (int)ForcastingSettingLevel.Account:
                                    await SaveAccountLevelForecastingSettings(forcastingModel, userContext, entityId);
                                    break;
                                case (int)ForcastingSettingLevel.Job:
                                    await SaveJobLevelForecastingSettings(forcastingModel, userContext);
                                    break;
                                default:
                                    await SaveTankLevelForecastingSettings(forcastingModel, userContext);
                                    break;
                            }
                        }
                        else
                        {
                            switch (forcastingModel.ForcastingSettingLevel)
                            {
                                case (int)ForcastingSettingLevel.Account:
                                    await InactiveAccountLevelForecastingSettings(forcastingModel, userContext, entityId);
                                    break;
                                case (int)ForcastingSettingLevel.Job:
                                    await InactiveJobLevelForecastingSettings(forcastingModel, userContext);
                                    break;
                                default:
                                    await InactiveTankLevelForecastingSettings(forcastingModel, userContext);
                                    break;
                            }
                        }
                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        statusModel.StatusCode = Status.Failed;
                        statusModel.StatusMessage = Resource.errorFailedSaveForecastingSetting;
                        LogManager.Logger.WriteException("ForcastingServiceDomain", "SaveForcastingPreferanceSetting", ex.Message, ex);
                    }
                }

            }
            return statusModel;
        }

        private async Task SaveTankLevelForecastingSettings(ForcastingPreferenceViewModel forcastingModel, UserContext userContext)
        {
            var tankforcastingDetails = await SaveForCastingDetails(forcastingModel, ForcastingSettingLevel.Tank, userContext);
            if (tankforcastingDetails != null)
            {
                //check if any data change.
                bool changeRecordStatus = VerifyDataChange(tankforcastingDetails, forcastingModel);
                if (changeRecordStatus)
                {
                    //update the records in below level.
                    //3) Tank Level
                    await InactiveCurrentForCastingSetting(tankforcastingDetails, forcastingModel.CreatedBy);
                    await CreateforcastingSettings(forcastingModel, (int)ForcastingSettingLevel.Tank, userContext);
                }
            }
        }
        private async Task InactiveTankLevelForecastingSettings(ForcastingPreferenceViewModel forcastingModel, UserContext userContext)
        {
            var tankforcastingDetails = await GetForeCastingDetails(forcastingModel, ForcastingSettingLevel.Tank, userContext);
            if (tankforcastingDetails != null)
            {
                await InactiveCurrentForCastingSetting(tankforcastingDetails, forcastingModel.CreatedBy);
            }
        }
        private async Task SaveJobLevelForecastingSettings(ForcastingPreferenceViewModel forcastingModel, UserContext userContext)
        {
            var jobforcastingDetails = await SaveForCastingDetails(forcastingModel, ForcastingSettingLevel.Job, userContext);
            if (jobforcastingDetails != null)
            {
                //check if any data change.
                bool changeRecordStatus = VerifyDataChange(jobforcastingDetails, forcastingModel);
                if (changeRecordStatus)
                {
                    //update the records in below level.
                    //2) Job Level
                    //3) Tank Level
                    await InactiveCurrentForCastingSetting(jobforcastingDetails, forcastingModel.CreatedBy);
                    await InactiveAndCreatejoblevelDetails(forcastingModel, userContext, 1);
                }
            }
        }
        private async Task InactiveJobLevelForecastingSettings(ForcastingPreferenceViewModel forcastingModel, UserContext userContext)
        {
            var jobforcastingDetails = await GetForeCastingDetails(forcastingModel, ForcastingSettingLevel.Job, userContext);
            if (jobforcastingDetails != null)
            {
                await InactiveCurrentForCastingSetting(jobforcastingDetails, forcastingModel.CreatedBy);
                await InactiveTankLevelForCastingSetting(new List<int>(), forcastingModel.CreatedBy, userContext, forcastingModel.EntityId);
            }
        }
        private async Task SaveAccountLevelForecastingSettings(ForcastingPreferenceViewModel forcastingModel, UserContext userContext, int entityId)
        {
            var accountforcastingDetails = await SaveForCastingDetails(forcastingModel, ForcastingSettingLevel.Account, userContext);
            if (accountforcastingDetails != null)
            {
                //check if any data change.
                bool changeRecordStatus = VerifyDataChange(accountforcastingDetails, forcastingModel);
                if (changeRecordStatus)
                {
                    //update the records in below level.
                    //1) Account Level
                    //2) Job Level
                    //3) Tank Level

                    await InactiveAndCreateAccountlevelSetting(forcastingModel, userContext, entityId, 1);
                }
                else
                {
                    //update the latest entity id for AccountPreferance.
                    await UpdateAccountLevelEntityId(entityId, accountforcastingDetails);
                }
            }
        }
        private async Task InactiveAccountLevelForecastingSettings(ForcastingPreferenceViewModel forcastingModel, UserContext userContext, int entityId)
        {
            var accountforcastingDetails = await GetForeCastingDetails(forcastingModel, ForcastingSettingLevel.Account, userContext);
            if (accountforcastingDetails != null)
            {
                await InactiveCurrentForCastingSetting(accountforcastingDetails, forcastingModel.CreatedBy);
                var companyJobDetails = await InactiveJobLevelForCastingSetting(forcastingModel.CreatedBy, userContext);
                await InactiveTankLevelForCastingSetting(companyJobDetails, forcastingModel.CreatedBy, userContext);
            }

        }
        private async Task UpdateAccountLevelEntityId(int entityId, ForcastingPreference accountforcastingDetails)
        {
            accountforcastingDetails.EntityId = entityId;
            Context.DataContext.Entry(accountforcastingDetails).State = EntityState.Modified;
            await Context.CommitAsync();
        }

        private async Task InactiveAndCreateAccountlevelSetting(ForcastingPreferenceViewModel forcastingModel, UserContext userContext, int entityId = 0, int needtoCreate = 0)
        {
            if (entityId > 0)
            {
                forcastingModel.EntityId = entityId;
            }
            if (needtoCreate == 1)
            {
                await InactiveAccountLevelSetting(forcastingModel, userContext);
                await CreateforcastingSettings(forcastingModel, (int)ForcastingSettingLevel.Account, userContext);
            }
        }

        private async Task InactiveAccountLevelSetting(ForcastingPreferenceViewModel forcastingModel, UserContext userContext)
        {
            var forcastingDetails = await Context.DataContext.ForcastingPreferences.Where(top => (top.ForcastingServicePreference == (int)ForcastingServicePreferance.Supplier) && top.SupplierCompanyId == userContext.CompanyId && top.IsActive && !top.IsDeleted).ToListAsync();
            InactiveForcastingRecords(forcastingModel.CreatedBy, forcastingDetails, (int)ForcastingSettingLevel.Account);
            await Context.CommitAsync();
        }

        private async Task InactiveAndCreatejoblevelDetails(ForcastingPreferenceViewModel forcastingModel, UserContext userContext, int needToCreate = 0)
        {
            var companyTankDetails = await InactiveTankLevelForCastingSetting(new List<int>(), forcastingModel.CreatedBy, userContext, forcastingModel.EntityId);
            //create forcasting setting for account , job and tank level.
            if (needToCreate == 1)
            {
                await CreateforcastingSettings(forcastingModel, (int)ForcastingSettingLevel.Job, userContext);
            }
        }

        private static void IntializeforcastingModel(ForcastingPreferenceViewModel forcastingModel, UserContext userContext, int forcastingSettingLevel, int entityId, int accountEntityId = 0, int tpoBuyerCompanyId = 0)
        {
            forcastingModel.ForcastingSettingLevel = forcastingSettingLevel;
            if (forcastingModel.ForcastingSettingLevel == (int)ForcastingSettingLevel.Account)
            {
                forcastingModel.prevEntityId = accountEntityId;
                forcastingModel.EntityId = entityId;
            }
            else
            {
                forcastingModel.EntityId = entityId;
            }
            forcastingModel.CreatedBy = userContext.Id;
            forcastingModel.ForcastingServiceSetting.CreatedBy = userContext.Id;
            if (userContext.IsBuyerCompany)
            {
                forcastingModel.BuyerCompanyId = userContext.CompanyId;
                forcastingModel.SupplierCompanyId = null;
                forcastingModel.ForcastingServicePreference = (int)ForcastingServicePreferance.Buyer;
            }
            else
            {
                forcastingModel.SupplierCompanyId = userContext.CompanyId;
                if (forcastingModel.ForcastingSettingLevel == (int)ForcastingSettingLevel.Job || forcastingModel.ForcastingSettingLevel == (int)ForcastingSettingLevel.Tank)
                {
                    if (tpoBuyerCompanyId > 0)
                        forcastingModel.BuyerCompanyId = tpoBuyerCompanyId;
                    else
                        forcastingModel.BuyerCompanyId = null;
                }
                else
                {
                    forcastingModel.BuyerCompanyId = null;
                }
                forcastingModel.ForcastingServicePreference = (int)ForcastingServicePreferance.Supplier;
            }
        }

        private async Task<ForcastingPreference> SaveForCastingDetails(ForcastingPreferenceViewModel forcastingModel, ForcastingSettingLevel forcastingSettingLevel, UserContext userContext)
        {
            ForcastingPreference forcastingDetails = null;
            if (userContext.IsBuyerCompany)
            {
                forcastingDetails = Context.DataContext.ForcastingPreferences
                                 .Where(top => top.ForcastingSettingLevel == (int)forcastingSettingLevel
                                 && top.IsActive && !top.IsDeleted && top.EntityId == forcastingModel.EntityId && top.BuyerCompanyId == userContext.CompanyId).FirstOrDefault();

            }
            else
            {
                if (forcastingModel.ForcastingSettingLevel == (int)ForcastingSettingLevel.Account)
                {
                    if (forcastingModel.prevEntityId > 0)
                    {
                        //setting from the setting .
                        forcastingDetails = Context.DataContext.ForcastingPreferences
                                        .Where(top => top.ForcastingSettingLevel == (int)forcastingSettingLevel
                                        && top.IsActive && !top.IsDeleted && top.EntityId == forcastingModel.prevEntityId && top.SupplierCompanyId == userContext.CompanyId).FirstOrDefault();
                    }
                    else
                    {
                        //onboarding preferance page.
                        forcastingDetails = Context.DataContext.ForcastingPreferences
                                        .Where(top => top.ForcastingSettingLevel == (int)forcastingSettingLevel
                                        && top.IsActive && !top.IsDeleted && top.EntityId == forcastingModel.EntityId && top.SupplierCompanyId == userContext.CompanyId).FirstOrDefault();
                    }
                }
                else
                {
                    forcastingDetails = Context.DataContext.ForcastingPreferences
                                    .Where(top => top.ForcastingSettingLevel == (int)forcastingSettingLevel
                                    && top.IsActive && !top.IsDeleted && top.EntityId == forcastingModel.EntityId).FirstOrDefault();
                }
            }
            if (forcastingDetails == null)
            {

                await CreateforcastingSettings(forcastingModel, (int)forcastingSettingLevel, userContext);
            }
            return forcastingDetails;
        }

        private static bool VerifyDataChange(ForcastingPreference forcastingPreference, ForcastingPreferenceViewModel forcastingModel)
        {
            bool changeRecord = false;
            var forcastingSetting = forcastingPreference.ForcastingServiceSetting;
            string startTiming = string.Empty;
            if (forcastingSetting.StartTiming != null)
            {
                DateTime time = DateTime.Today.Add(forcastingSetting.StartTiming);
                startTiming = time.ToString("hh:mm tt");
            }

            var forcastingSettingViewModel = forcastingModel.ForcastingServiceSetting;

            var isOttoAutoDRCreationAllCarrier = 0;
            if (forcastingSetting.IsAllCarrierEnabled)
                isOttoAutoDRCreationAllCarrier = 1;
            else
                isOttoAutoDRCreationAllCarrier = 2;

            if (forcastingPreference.ForcastingSettingLevel == (int)ForcastingSettingLevel.Account)
            {
                if (forcastingSetting.BandPeriod != forcastingSettingViewModel.BandPeriod.GetValueOrDefault()
                || startTiming.Trim() != ValidateStartTimining(forcastingSettingViewModel.StartTime).Trim()
                || forcastingSetting.MinimumLoadQty.ToString("N2") != forcastingSettingViewModel.MinimumLoad.GetValueOrDefault().ToString("N2")
                || forcastingSetting.AverageLoadQty.ToString("N2") != forcastingSettingViewModel.AverageLoad.GetValueOrDefault().ToString("N2")
                || forcastingSetting.InventoryPriorityType != forcastingSettingViewModel.ForcastingType.GetValueOrDefault()
                || forcastingSetting.InventoryUOM != forcastingSettingViewModel.InventoryUOM.GetValueOrDefault()
                || forcastingSetting.RetainCouldGo != forcastingSettingViewModel.Retain.GetValueOrDefault()
                || forcastingSetting.SafetyStockShouldGo != forcastingSettingViewModel.SafetyStock.GetValueOrDefault()
                || forcastingSetting.RunoutLevelMustGo != forcastingSettingViewModel.RunoutLevel.GetValueOrDefault()
                || forcastingSetting.IsAutoDRCreation != forcastingSettingViewModel.IsAutoDRCreation
                || forcastingSetting.StartBuffer != forcastingSettingViewModel.StartBuffer.GetValueOrDefault()
                || forcastingSetting.StartBufferUOM != forcastingSettingViewModel.StartBufferUOM.GetValueOrDefault()
                || forcastingSetting.EndBuffer != forcastingSettingViewModel.EndBuffer.GetValueOrDefault()
                || forcastingSetting.EndBufferUOM != forcastingSettingViewModel.EndBufferUOM.GetValueOrDefault()
                || forcastingSetting.RetainTimeBuffer != forcastingSettingViewModel.RetainTimeBuffer.GetValueOrDefault()
                || forcastingSetting.RetainTimeBufferUOM != forcastingSettingViewModel.RetainTimeBufferUOM.GetValueOrDefault()
                || forcastingSetting.LeadTime != forcastingSettingViewModel.LeadTime.GetValueOrDefault()
                || forcastingSetting.LeadTimeUOM != forcastingSettingViewModel.LeadTimeUOM.GetValueOrDefault()
                || forcastingSetting.SupplierLead != forcastingSettingViewModel.SupplierLead.GetValueOrDefault()
                || forcastingSetting.SupplierLeadUOM != forcastingSettingViewModel.SupplierLeadUOM.GetValueOrDefault()
                || forcastingSetting.IsOttoAutoDRCreation != forcastingSettingViewModel.IsOttoAutoDRCreation
                || forcastingSetting.IsOttoScheduleCreation != forcastingSettingViewModel.IsOttoScheduleCreation
                || isOttoAutoDRCreationAllCarrier != forcastingSettingViewModel.IsOttoAutoDRCreationAllCarrier
                )
                {
                    changeRecord = true;
                }

                if (!forcastingSetting.IsAllCarrierEnabled)
                {
                    var carrierIds = forcastingPreference.ForcastingServiceSetting.ForcastingServiceXCarriers.OrderBy(t => t.CarrierId).Select(t => t.CarrierId).ToList();
                    var selectedCarrierIds = forcastingModel.ForcastingServiceSetting.SelectedCarrierList.OrderBy(t => t).ToList();
                    bool isEqual = Enumerable.SequenceEqual(carrierIds, selectedCarrierIds);
                    if (!isEqual)
                        changeRecord = true;
                }
            }
            else
            {
                if (forcastingSetting.BandPeriod != forcastingSettingViewModel.BandPeriod.GetValueOrDefault()
               || startTiming.Trim() != ValidateStartTimining(forcastingSettingViewModel.StartTime).Trim()
               || forcastingSetting.MinimumLoadQty.ToString("N2") != forcastingSettingViewModel.MinimumLoad.GetValueOrDefault().ToString("N2")
               || forcastingSetting.AverageLoadQty.ToString("N2") != forcastingSettingViewModel.AverageLoad.GetValueOrDefault().ToString("N2")
               || forcastingSetting.InventoryPriorityType != forcastingSettingViewModel.ForcastingType.GetValueOrDefault()
               || forcastingSetting.InventoryUOM != forcastingSettingViewModel.InventoryUOM.GetValueOrDefault()
               || forcastingSetting.RetainCouldGo != forcastingSettingViewModel.Retain.GetValueOrDefault()
               || forcastingSetting.SafetyStockShouldGo != forcastingSettingViewModel.SafetyStock.GetValueOrDefault()
               || forcastingSetting.RunoutLevelMustGo != forcastingSettingViewModel.RunoutLevel.GetValueOrDefault()
               || forcastingSetting.IsAutoDRCreation != forcastingSettingViewModel.IsAutoDRCreation
               || forcastingSetting.StartBuffer != forcastingSettingViewModel.StartBuffer.GetValueOrDefault()
               || forcastingSetting.StartBufferUOM != forcastingSettingViewModel.StartBufferUOM.GetValueOrDefault()
               || forcastingSetting.EndBuffer != forcastingSettingViewModel.EndBuffer.GetValueOrDefault()
               || forcastingSetting.EndBufferUOM != forcastingSettingViewModel.EndBufferUOM.GetValueOrDefault()
               || forcastingSetting.RetainTimeBuffer != forcastingSettingViewModel.RetainTimeBuffer.GetValueOrDefault()
               || forcastingSetting.RetainTimeBufferUOM != forcastingSettingViewModel.RetainTimeBufferUOM.GetValueOrDefault()
               || forcastingSetting.LeadTime != forcastingSettingViewModel.LeadTime.GetValueOrDefault()
               || forcastingSetting.LeadTimeUOM != forcastingSettingViewModel.LeadTimeUOM.GetValueOrDefault()
               || forcastingSetting.SupplierLead != forcastingSettingViewModel.SupplierLead.GetValueOrDefault()
               || forcastingSetting.SupplierLeadUOM != forcastingSettingViewModel.SupplierLeadUOM.GetValueOrDefault()
               || forcastingSetting.IsOttoAutoDRCreation != forcastingSettingViewModel.IsOttoAutoDRCreation
               || forcastingSetting.IsOttoScheduleCreation != forcastingSettingViewModel.IsOttoScheduleCreation
               )
                {
                    changeRecord = true;
                }
            }
            return changeRecord;
        }

        private async Task CreateforcastingSettings(ForcastingPreferenceViewModel forcastingModel, int forcastingLevel, UserContext userContext)
        {
            forcastingModel.ForcastingSettingLevel = forcastingLevel;
            if (forcastingLevel == (int)ForcastingSettingLevel.Tank)
            {
                var forcastingPreference = forcastingModel.ToCloneEntity();
                var efModel = forcastingModel.ForcastingServiceSetting.ToCloneEntity();
                forcastingPreference.ForcastingServiceSetting = efModel;
                Context.DataContext.ForcastingPreferences.Add(forcastingPreference);
                await Context.CommitAsync();

                if (forcastingModel.ForcastingServiceSetting.IsOttoAutoDRCreationAllCarrier == 2)
                {
                    foreach (var carrier in forcastingModel.ForcastingServiceSetting.SelectedCarrierList)
                    {
                        ForcastingServiceXCarrier forcastingCarrier = new ForcastingServiceXCarrier();
                        forcastingCarrier.CarrierId = carrier;
                        forcastingCarrier.ForcastingServiceSettingId = forcastingPreference.ForcastingServiceSetting.Id;
                        Context.DataContext.ForcastingServiceXCarriers.Add(forcastingCarrier);
                    }
                    await Context.CommitAsync();
                }
                else if (forcastingModel.ForcastingServiceSetting.IsOttoAutoDRCreationAllCarrier == 1)
                {
                    var carrierList = await IntializeForcastingCarrierList(userContext.CompanyId);
                    foreach (var carrierInfo in carrierList)
                    {
                        ForcastingServiceXCarrier forcastingCarrier = new ForcastingServiceXCarrier();
                        forcastingCarrier.CarrierId = carrierInfo.Carrier.Id;
                        forcastingCarrier.ForcastingServiceSettingId = forcastingPreference.ForcastingServiceSetting.Id;
                        Context.DataContext.ForcastingServiceXCarriers.Add(forcastingCarrier);
                    }
                    await Context.CommitAsync();
                }
            }
            else if (forcastingLevel == (int)ForcastingSettingLevel.Job)
            {
                await InactiveTankLevelForCastingSetting(new List<int>(), forcastingModel.CreatedBy, userContext, forcastingModel.EntityId);
                await CreateJobLevelForcastingSetting(forcastingModel, userContext);
            }
            else
            {
                await InactiveAccountLevelSetting(forcastingModel, userContext);
                await CreateAccountLevelForcastingSetting(forcastingModel, userContext);
            }
        }

        private async Task CreateAccountLevelForcastingSetting(ForcastingPreferenceViewModel forcastingModel, UserContext userContext)
        {
            List<ForcastingPreference> forecastingSettingDetails = new List<ForcastingPreference>();
            List<int> jobsdetails = GetJoblevelForcastingSetting(forcastingModel, userContext, forecastingSettingDetails);
            GetAssetLevelForcastingSetting(forcastingModel, userContext, forecastingSettingDetails, jobsdetails);
            var forcastingAccountPreference = forcastingModel.ToCloneAccountEntity();
            var efModel = forcastingModel.ForcastingServiceSetting.ToCloneEntity();
            Context.DataContext.ForcastingServiceSettings.Add(efModel);
            await Context.CommitAsync();
            if (efModel.Id > 0)
            {
                forecastingSettingDetails.Add(forcastingAccountPreference);
                forecastingSettingDetails.ForEach(x => x.ForcastingSettingId = efModel.Id);
                Context.DataContext.ForcastingPreferences.AddRange(forecastingSettingDetails);
                await Context.CommitAsync();
                if (forcastingModel.ForcastingServiceSetting.IsOttoAutoDRCreationAllCarrier == 2)
                {
                    foreach (var carrier in forcastingModel.ForcastingServiceSetting.SelectedCarrierList)
                    {
                        ForcastingServiceXCarrier forcastingCarrier = new ForcastingServiceXCarrier();
                        forcastingCarrier.CarrierId = carrier;
                        forcastingCarrier.ForcastingServiceSettingId = efModel.Id;
                        Context.DataContext.ForcastingServiceXCarriers.Add(forcastingCarrier);
                    }
                    await Context.CommitAsync();
                }
                else if (forcastingModel.ForcastingServiceSetting.IsOttoAutoDRCreationAllCarrier == 1)
                {
                    var carrierList = await IntializeForcastingCarrierList(userContext.CompanyId);
                    foreach (var carrierInfo in carrierList)
                    {
                        ForcastingServiceXCarrier forcastingCarrier = new ForcastingServiceXCarrier();
                        forcastingCarrier.CarrierId = carrierInfo.Carrier.Id;
                        forcastingCarrier.ForcastingServiceSettingId = efModel.Id;
                        Context.DataContext.ForcastingServiceXCarriers.Add(forcastingCarrier);
                    }
                    await Context.CommitAsync();
                }
            }
        }
        private async Task CreateJobLevelForcastingSetting(ForcastingPreferenceViewModel forcastingModel, UserContext userContext)
        {
            List<ForcastingPreference> forecastingSettingDetails = new List<ForcastingPreference>();
            var jobsdetails = (from orders in Context.DataContext.Orders
                               join fuelRequest in Context.DataContext.FuelRequests
                               on orders.FuelRequestId equals fuelRequest.Id
                               join jobDetails in Context.DataContext.Jobs
                               on fuelRequest.JobId equals jobDetails.Id
                               where orders.AcceptedCompanyId == userContext.CompanyId && jobDetails.Id == forcastingModel.EntityId
                               select jobDetails.Id
                                      ).Distinct().ToList();
            GetAssetLevelForcastingSetting(forcastingModel, userContext, forecastingSettingDetails, jobsdetails);
            var forcastingJobPreference = forcastingModel.ToCloneAccountEntity();
            var efModel = forcastingModel.ForcastingServiceSetting.ToCloneEntity();
            Context.DataContext.ForcastingServiceSettings.Add(efModel);
            await Context.CommitAsync();
            if (efModel.Id > 0)
            {
                forecastingSettingDetails.Add(forcastingJobPreference);
                forecastingSettingDetails.ForEach(x => x.ForcastingSettingId = efModel.Id);
                Context.DataContext.ForcastingPreferences.AddRange(forecastingSettingDetails);
                await Context.CommitAsync();
            }
        }
        private void GetAssetLevelForcastingSetting(ForcastingPreferenceViewModel forcastingModel, UserContext userContext, List<ForcastingPreference> jobforcasting, List<int> jobsdetails)
        {
            var forcastingAssetDetails = (from orders in Context.DataContext.Orders
                                          join fuelRequest in Context.DataContext.FuelRequests
                                          on orders.FuelRequestId equals fuelRequest.Id
                                          join jobDetails in Context.DataContext.Jobs
                                          on fuelRequest.JobId equals jobDetails.Id
                                          join jxa in Context.DataContext.JobXAssets
                                          on jobDetails.Id equals jxa.JobId
                                          join assetDetails in Context.DataContext.Assets
                                          on jxa.AssetId equals assetDetails.Id
                                          where orders.AcceptedCompanyId == userContext.CompanyId && jobsdetails.Contains(jobDetails.Id) && jxa.RemovedBy == null && assetDetails.Type == 2
                                          select assetDetails.Id
                                            ).Distinct().ToList();
            if (forcastingAssetDetails.Count > 0)
            {
                foreach (var item in forcastingAssetDetails)
                {
                    ForcastingPreference entity = new ForcastingPreference();
                    entity.BuyerCompanyId = forcastingModel.BuyerCompanyId;
                    entity.SupplierCompanyId = forcastingModel.SupplierCompanyId;
                    entity.ForcastingServicePreference = forcastingModel.ForcastingServicePreference;
                    entity.ForcastingSettingLevel = (int)ForcastingSettingLevel.Tank;
                    entity.EntityId = item;
                    entity.IsActive = true;
                    entity.IsDeleted = false;
                    entity.CreatedDate = DateTime.Now;
                    entity.CreatedBy = forcastingModel.CreatedBy;
                    jobforcasting.Add(entity);
                }
            }
        }

        private List<int> GetJoblevelForcastingSetting(ForcastingPreferenceViewModel forcastingModel, UserContext userContext, List<ForcastingPreference> jobforcasting)
        {
            var jobsdetails = (from orders in Context.DataContext.Orders
                               join fuelRequest in Context.DataContext.FuelRequests
                               on orders.FuelRequestId equals fuelRequest.Id
                               join jobDetails in Context.DataContext.Jobs
                               on fuelRequest.JobId equals jobDetails.Id
                               where orders.AcceptedCompanyId == userContext.CompanyId
                               select jobDetails.Id
                                        ).Distinct().ToList();
            if (jobsdetails.Count > 0)
            {

                foreach (var item in jobsdetails)
                {
                    ForcastingPreference entity = new ForcastingPreference();
                    entity.BuyerCompanyId = forcastingModel.BuyerCompanyId;
                    entity.SupplierCompanyId = forcastingModel.SupplierCompanyId;
                    entity.ForcastingServicePreference = forcastingModel.ForcastingServicePreference;
                    entity.ForcastingSettingLevel = (int)ForcastingSettingLevel.Job;
                    entity.EntityId = item;
                    entity.IsActive = true;
                    entity.IsDeleted = false;
                    entity.CreatedDate = forcastingModel.CreatedDate;
                    entity.CreatedBy = forcastingModel.CreatedBy;
                    jobforcasting.Add(entity);
                }
            }

            return jobsdetails;
        }


        private async Task InactiveCurrentForCastingSetting(ForcastingPreference forcastingDetails, int UpdatedBy)
        {
            forcastingDetails.IsActive = false;
            forcastingDetails.IsDeleted = true;
            forcastingDetails.UpdatedBy = UpdatedBy;
            forcastingDetails.UpdatedDate = DateTime.Now;
            Context.DataContext.Entry(forcastingDetails).State = EntityState.Modified;
            await Context.CommitAsync();
        }
        private async Task<List<int>> InactiveJobLevelForCastingSetting(int UpdatedBy, UserContext userContext)
        {
            var forCastingJobDetails = new List<ForcastingPreference>();
            var jobdetails = (from orders in Context.DataContext.Orders
                              join fuelRequest in Context.DataContext.FuelRequests
                              on orders.FuelRequestId equals fuelRequest.Id
                              join jobDetails in Context.DataContext.Jobs
                              on fuelRequest.JobId equals jobDetails.Id
                              where orders.AcceptedCompanyId == userContext.CompanyId
                              select jobDetails.Id
                                      ).Distinct().ToList();
            forCastingJobDetails = await GetInactiveForcastingBuyerSupplierJobInfo(userContext, forCastingJobDetails, jobdetails);
            InactiveForcastingRecords(UpdatedBy, forCastingJobDetails);
            await Context.CommitAsync();
            return forCastingJobDetails.Select(top => top.EntityId).Distinct().ToList();
        }

        private static void InactiveForcastingRecords(int UpdatedBy, List<ForcastingPreference> forCastingJobDetails, int forcastingLevel = 0)
        {
            forCastingJobDetails.ForEach(x =>
            {
                x.IsActive = false;
                x.IsDeleted = true;
                x.UpdatedBy = UpdatedBy;
                x.UpdatedDate = DateTime.Now;
            });
        }

        private async Task<List<ForcastingPreference>> GetInactiveForcastingBuyerSupplierJobInfo(UserContext userContext, List<ForcastingPreference> forCastingJobDetails, List<int> jobDetails)
        {
            if (userContext.IsBuyerCompany)
            {
                forCastingJobDetails = await Context.DataContext.ForcastingPreferences.Where(top => top.ForcastingSettingLevel == (int)ForcastingSettingLevel.Job
                   && jobDetails.Contains(top.EntityId) && top.BuyerCompanyId == userContext.CompanyId && top.IsActive && !top.IsDeleted).ToListAsync();
            }
            else
            {
                forCastingJobDetails = await Context.DataContext.ForcastingPreferences.Where(top => top.ForcastingSettingLevel == (int)ForcastingSettingLevel.Job
                 && (top.ForcastingServicePreference == (int)ForcastingServicePreferance.Supplier) && jobDetails.Contains(top.EntityId) && top.SupplierCompanyId == userContext.CompanyId && top.IsActive && !top.IsDeleted).ToListAsync();
            }

            return forCastingJobDetails;
        }

        private async Task<List<int>> InactiveTankLevelForCastingSetting(List<int> jobIds, int UpdatedBy, UserContext userContext, int jobId = 0)
        {
            var forCastingTankDetails = new List<ForcastingPreference>();
            List<int> tankDetails = new List<int>();
            tankDetails = RetriveTankInformation(jobIds, userContext, jobId);
            forCastingTankDetails = await GetInactiveForcastingBuyerSupplierTankInfo(userContext, forCastingTankDetails, tankDetails);
            InactiveForcastingRecords(UpdatedBy, forCastingTankDetails);
            await Context.CommitAsync();
            return forCastingTankDetails.Select(top => top.EntityId).Distinct().ToList();
        }

        private List<int> RetriveTankInformation(List<int> jobIds, UserContext userContext, int jobId)
        {
            List<int> tankDetails;
            if (jobId > 0)
            {
                tankDetails = (from orders in Context.DataContext.Orders
                               join fuelRequest in Context.DataContext.FuelRequests
                               on orders.FuelRequestId equals fuelRequest.Id
                               join jobDetails in Context.DataContext.Jobs
                               on fuelRequest.JobId equals jobDetails.Id
                               join jxa in Context.DataContext.JobXAssets
                               on jobDetails.Id equals jxa.JobId
                               join assetDetails in Context.DataContext.Assets
                               on jxa.AssetId equals assetDetails.Id
                               where orders.AcceptedCompanyId == userContext.CompanyId && jobDetails.Id == jobId && jxa.RemovedBy == null && assetDetails.Type == (int)AssetType.Tank
                               select assetDetails.Id
                                          ).Distinct().ToList();
            }
            else
            {
                if (jobIds.Count > 0)
                {
                    tankDetails = (from orders in Context.DataContext.Orders
                                   join fuelRequest in Context.DataContext.FuelRequests
                                   on orders.FuelRequestId equals fuelRequest.Id
                                   join jobDetails in Context.DataContext.Jobs
                                   on fuelRequest.JobId equals jobDetails.Id
                                   join jxa in Context.DataContext.JobXAssets
                                   on jobDetails.Id equals jxa.JobId
                                   join assetDetails in Context.DataContext.Assets
                                   on jxa.AssetId equals assetDetails.Id
                                   where orders.AcceptedCompanyId == userContext.CompanyId && jobIds.Contains(jobDetails.Id) && jxa.RemovedBy == null && assetDetails.Type == (int)AssetType.Tank
                                   select assetDetails.Id
                                           ).Distinct().ToList();
                    tankDetails = Context.DataContext.JobXAssets.Where(top => jobIds.Contains(top.JobId) && top.Asset.IsActive && top.Asset.Type == (int)AssetType.Tank).Select(top => top.AssetId).Distinct().ToList();
                }
                else
                {
                    tankDetails = Context.DataContext.Assets.Where(top => top.CompanyId == userContext.CompanyId && top.IsActive && top.Type == (int)AssetType.Tank).Select(top => top.Id).Distinct().ToList();
                }
            }

            return tankDetails;
        }

        private async Task<List<ForcastingPreference>> GetInactiveForcastingBuyerSupplierTankInfo(UserContext userContext, List<ForcastingPreference> forCastingJobDetails, List<int> tankDetails)
        {
            if (userContext.IsBuyerCompany)
            {
                forCastingJobDetails = await Context.DataContext.ForcastingPreferences.Where(top => top.ForcastingSettingLevel == (int)ForcastingSettingLevel.Tank
                              && top.BuyerCompanyId == userContext.CompanyId && tankDetails.Contains(top.EntityId) && top.IsActive && !top.IsDeleted).ToListAsync();
            }
            else
            {
                forCastingJobDetails = await Context.DataContext.ForcastingPreferences.Where(top => top.ForcastingSettingLevel == (int)ForcastingSettingLevel.Tank
                           && (top.ForcastingServicePreference == (int)ForcastingServicePreferance.Supplier) && top.SupplierCompanyId == userContext.CompanyId && tankDetails.Contains(top.EntityId) && top.IsActive && !top.IsDeleted).ToListAsync();
            }

            return forCastingJobDetails;
        }

        private static string ValidateStartTimining(string inputTime)
        {
            string finalString = string.Empty;
            if (!string.IsNullOrEmpty(inputTime))
            {
                var splitRecord = inputTime.Split(':');
                if (splitRecord.Length > 0)
                {

                    if (splitRecord[0].Length == 1)
                    {
                        finalString = "0" + splitRecord[0].ToString();
                        finalString = finalString + ":" + splitRecord[1].ToString();
                    }
                    else
                    {
                        finalString = inputTime;
                    }
                }
            }

            return finalString;
        }
        public async Task<ForcastingPreferenceViewModel> GetForCastingPreferanceSetting(UserContext userContext, int level, int entityId)
        {
            var response = new ForcastingPreferenceViewModel();
            try
            {
                if (level == (int)ForcastingSettingLevel.Tank)
                    response = await GetForCastingPreferanceSettingForTank(userContext, entityId);
                else if (level == (int)ForcastingSettingLevel.Job)
                    response = await GetForCastingPreferanceSettingForJob(userContext, entityId);
                else
                    response = await GetForCastingPreferanceSettingForAccount(userContext, entityId);

                // IS EDITABLE
                if ((userContext.IsBuyerCompany) || (response.SupplierCompanyId == userContext.CompanyId) || (response.EntityId != entityId) || (response.Id == 0))
                {
                    response.IsEditable = true;
                    response.ForcastingServiceSetting.IsEditableTpo = true;
                }
                // CHECK BOX CHECKED - IF IMS SETTING EXIST
                if (response.Id > 0)
                {
                    if (response.EntityId == entityId)
                    {
                        response.ForcastingServiceSetting.IsEnabled = true;
                    }
                    else
                    {
                        response.ForcastingServiceSetting.IsEnabled = false;
                    }
                }
                //GET UOM DETAILS
                response.uomDetails = GetForCastingUOMDetails(userContext);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ForcastingServiceDomain", "GetForCastingPreferanceSetting", ex.Message, ex);
            }
            return response;
        }
        public async Task<ForcastingPreferenceViewModel> GetForCastingPreferanceSettingForTank(UserContext userContext, int entityId)
        {
            var response = new ForcastingPreferenceViewModel();
            try
            {
                response = await GetForCastingPreferanceSettingByQuery(ForcastingSettingLevel.Tank, ForcastingServicePreferance.Buyer, userContext.CompanyId, entityId);

                if (response.Id == 0)
                {
                    response = await GetForCastingPreferanceSettingByQuery(ForcastingSettingLevel.Tank, ForcastingServicePreferance.Supplier, userContext.CompanyId, entityId);
                }
                if (response.Id == 0)
                {
                    var jobId = ContextFactory.Current.GetDomain<JobDomain>().GetJobIdByAsset(entityId);
                    response = await GetForCastingPreferanceSettingForJob(userContext, jobId);
                }
                if (response.Id == 0 && userContext.IsSupplierCompany)
                {
                    var preferenceSeting = await ContextFactory.Current.GetDomain<CompanyDomain>().GetPreferencesSettingAsync(0, userContext);
                    response = await GetForCastingPreferanceSettingForAccount(userContext, preferenceSeting.Id);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ForcastingServiceDomain", "GetForCastingPreferanceSettingForTank", ex.Message, ex);
            }
            return response;
        }
        public async Task<ForcastingPreferenceViewModel> GetForCastingPreferanceSettingForJob(UserContext userContext, int entityId)
        {
            var response = new ForcastingPreferenceViewModel();
            try
            {
                if (userContext.IsBuyerAndSupplierCompany || userContext.IsBuyerSupplierAndCarrierCompany || userContext.IsSupplier || userContext.IsSupplierAdmin || userContext.IsSupplierAndCarrierCompany || userContext.IsSupplierCompany)
                {
                    var preferenceSeting = await ContextFactory.Current.GetDomain<CompanyDomain>().GetPreferencesSettingAsync(0, userContext);
                    response = await GetForCastingPreferanceSettingForAccount(userContext, preferenceSeting.Id);
                }
                response = await GetForCastingPreferanceSettingByQuery(ForcastingSettingLevel.Job, ForcastingServicePreferance.Buyer, userContext.CompanyId, entityId);
                if (response.Id == 0)
                {
                    response = await GetForCastingPreferanceSettingByQuery(ForcastingSettingLevel.Job, ForcastingServicePreferance.Supplier, userContext.CompanyId, entityId);
                }
                if (response.Id == 0 && (userContext.IsBuyerAndSupplierCompany || userContext.IsBuyerSupplierAndCarrierCompany || userContext.IsSupplier || userContext.IsSupplierAdmin || userContext.IsSupplierAndCarrierCompany || userContext.IsSupplierCompany))
                {
                    var preferenceSeting = await ContextFactory.Current.GetDomain<CompanyDomain>().GetPreferencesSettingAsync(0, userContext);
                    response = await GetForCastingPreferanceSettingForAccount(userContext, preferenceSeting.Id);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ForcastingServiceDomain", "GetForCastingPreferanceSettingForJob", ex.Message, ex);
            }
            return response;
        }
        public async Task<ForcastingPreferenceViewModel> GetForCastingPreferanceSettingForAccount(UserContext userContext, int entityId)
        {
            var response = new ForcastingPreferenceViewModel();
            try
            {
                response = await GetForCastingPreferanceSettingByQuery(ForcastingSettingLevel.Account, ForcastingServicePreferance.Supplier, userContext.CompanyId, entityId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ForcastingServiceDomain", "GetForCastingPreferanceSettingForAccount", ex.Message, ex);
            }
            return response;
        }
        public async Task<ForcastingPreferenceViewModel> GetForCastingPreferanceSettingByQuery(ForcastingSettingLevel level, ForcastingServicePreferance preferance, int companyId, int entityId)
        {
            var response = new ForcastingPreferenceViewModel();
            try
            {
                var entity = await Context.DataContext.ForcastingPreferences.Where(top => top.IsActive
                     && !top.IsDeleted
                     && top.ForcastingServicePreference == (int)preferance
                     && top.ForcastingSettingLevel == (int)level
                     && top.EntityId == entityId && top.IsActive && !top.IsDeleted
                     ).FirstOrDefaultAsync();

                if (entity != null)
                    response = entity.ToViewModel();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ForcastingServiceDomain", "GetForCastingPreferanceSettingByQuery", ex.Message, ex);
            }
            return response;
        }
        public string GetForCastingUOMDetails(UserContext userContext)
        {
            string umoDetails = UoM.Gallons.ToString();
            var companyAddress = Context.DataContext.CompanyAddresses.Where(top => top.CompanyId == userContext.CompanyId && top.IsDefault && top.IsActive).FirstOrDefault();
            if (companyAddress != null)
            {
                umoDetails = companyAddress.CountryId == (int)Country.USA ? UoM.Gallons.ToString() : UoM.Litres.ToString();
            }
            return umoDetails;
        }
        private async Task<ForcastingPreference> GetForeCastingDetails(ForcastingPreferenceViewModel forcastingModel, ForcastingSettingLevel forcastingSettingLevel, UserContext userContext)
        {
            ForcastingPreference forcastingDetails = null;
            if (userContext.IsBuyerCompany)
            {
                forcastingDetails = await Context.DataContext.ForcastingPreferences
                                 .Where(top => top.ForcastingSettingLevel == (int)forcastingSettingLevel
                                 && top.IsActive && !top.IsDeleted && top.EntityId == forcastingModel.EntityId && top.BuyerCompanyId == userContext.CompanyId).FirstOrDefaultAsync();

            }
            else
            {
                if (forcastingModel.ForcastingSettingLevel == (int)ForcastingSettingLevel.Account)
                {
                    if (forcastingModel.prevEntityId > 0)
                    {
                        //setting from the setting .
                        forcastingDetails = Context.DataContext.ForcastingPreferences
                                        .Where(top => top.ForcastingSettingLevel == (int)forcastingSettingLevel
                                        && top.IsActive && !top.IsDeleted && top.EntityId == forcastingModel.prevEntityId && top.SupplierCompanyId == userContext.CompanyId).FirstOrDefault();
                    }
                    else
                    {
                        //onboarding preferance page.
                        forcastingDetails = Context.DataContext.ForcastingPreferences
                                        .Where(top => top.ForcastingSettingLevel == (int)forcastingSettingLevel
                                        && top.IsActive && !top.IsDeleted && top.EntityId == forcastingModel.EntityId && top.SupplierCompanyId == userContext.CompanyId).FirstOrDefault();
                    }
                }
                else
                {
                    forcastingDetails = Context.DataContext.ForcastingPreferences
                                    .Where(top => top.ForcastingSettingLevel == (int)forcastingSettingLevel
                                    && top.IsActive && !top.IsDeleted && top.EntityId == forcastingModel.EntityId).FirstOrDefault();
                }
            }
            return forcastingDetails;
        }
        public StatusViewModel GetOttoSetting(UserContext userContext)
        {
            var response = new StatusViewModel();
            response.StatusCode = Status.Failed;
            try
            {
                var ottoSetting = (from item in Context.DataContext.ForcastingPreferences
                                   where item.IsActive && !item.IsDeleted && item.SupplierCompanyId == userContext.CompanyId
                                   && (item.ForcastingSettingLevel == (int)ForcastingSettingLevel.Account || item.ForcastingSettingLevel == (int)ForcastingSettingLevel.Job || item.ForcastingSettingLevel == (int)ForcastingSettingLevel.Tank)
                                   && item.ForcastingServiceSetting.IsOttoAutoDRCreation
                                   select new
                                   {
                                       item.ForcastingServiceSetting.Id,
                                   }).FirstOrDefault();
                if (ottoSetting != null)
                {
                    response.StatusCode = Status.Success;
                    response.StatusMessage = "IsAllCarrierEnabled";
                }
                else
                {
                    var carrierEnabled = (from item in Context.DataContext.ForcastingServiceXCarriers
                                          where item.CarrierId == userContext.CompanyId
                                          select item
                                          ).FirstOrDefault();
                    if (carrierEnabled != null)
                    {
                        response.StatusCode = Status.Success;
                        response.StatusMessage = "carrierEnabled";
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ForcastingServiceDomain", "GetOttoSetting", ex.Message, ex);
            }
            return response;
        }
        public async Task InactiveJOBPreferanceSetting(ForcastingPreferenceViewModel forcastingModel, UserContext userContext, int forcastingSettingLevel, int entityId, int prevaccountEntityId = 0, int tpoBuyerCompanyId = 0)
        {

            //setting from the setting .
            if (userContext.IsSupplierCompany)
            {
                var forcastingDetails = Context.DataContext.ForcastingPreferences
                                .Where(top => top.ForcastingSettingLevel == (int)forcastingSettingLevel
                                && top.IsActive && !top.IsDeleted && top.EntityId == entityId).FirstOrDefault();
                if (forcastingDetails != null && forcastingModel.IsEditable == false && forcastingModel.ForcastingServiceSetting.IsEnabled == false)
                {
                    forcastingDetails.IsActive = false;
                    forcastingDetails.IsDeleted = true;
                    forcastingDetails.UpdatedDate = DateTime.Now;
                    Context.DataContext.Entry(forcastingDetails).State = EntityState.Modified;
                    await Context.CommitAsync();
                }
            }
        }
        private async Task<List<SupplierCarrierViewModel>> IntializeForcastingCarrierList(int companyId)
        {
            var fsDomain = ContextFactory.Current.GetDomain<FreightServiceDomain>();
            var carrierresponse = await fsDomain.GetAssignedCarriersForSupplier(companyId, 0);
            carrierresponse = ContextFactory.Current.GetDomain<CarrierDomain>().GetCarrierUserEmails(carrierresponse);
            return carrierresponse;
        }
    }
}
