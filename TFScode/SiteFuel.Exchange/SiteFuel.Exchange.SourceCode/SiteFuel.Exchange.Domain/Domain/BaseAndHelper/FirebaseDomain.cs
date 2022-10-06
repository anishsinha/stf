using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class FirebaseDomain : BaseDomain
    {
        public FirebaseDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public FirebaseDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task<FirebaseConfiguration> GetFirebaseConfigurationAsync()
        {
            var firebaseConfig = new FirebaseConfiguration();
            try
            {
                var keys = new List<string>()
                {
                    ApplicationConstants.KeyAppSettingGoogleFirebaseProjectId,
                    ApplicationConstants.KeyAppSettingGoogleServiceAccountJson,
                    ApplicationConstants.KeyAppSettingGoogleFirebaseCollectionName,
                    ApplicationConstants.KeyAppSettingGoogleFirebasePreLoadBolCollectionName,
                    ApplicationConstants.KeyAppSettingGoogleFirebaseUploadDateTime,
                    ApplicationConstants.KeyAppSettingGoogleFirebasePreLoadBolUploadDateTime,
                    //edit bol details
                    ApplicationConstants.KeyAppSettingGoogleFirebaseEditedPreLoadBolCollectionName,
                    ApplicationConstants.KeyAppSettingGoogleFirebaseEditedPreLoadBolSyncDateTime,
                    //delete bol records
                    ApplicationConstants.KeyAppSettingGoogleFirebaseDeletedPreLoadBolCollectionName,
                    ApplicationConstants.KeyAppSettingGoogleFirebaseDeletedPreLoadBolSyncDateTime,

                    ApplicationConstants.KeyAppSettingGoogleFirebaseFuelRetainCollectionName,
                    ApplicationConstants.KeyAppSettingGoogleFirebaseFuelRetainSyncDateTime,

                    ApplicationConstants.KeyAppSettingGoogleFirebasePickupBOLRetainCollectionName,
                    ApplicationConstants.KeyAppSettingGoogleFirebasePickupBOLRetainSyncDateTime,

                    ApplicationConstants.KeyAppSettingGoogleFirebaseCanceledScheduleCollectionName,
                    ApplicationConstants.KeyAppSettingGoogleFirebaseCanceledScheduleSyncDateTime
                };
                var configs = await Context.DataContext.MstAppSettings.Where(t => keys.Contains(t.Key))
                                    .Select(t => new { t.Key, t.Value }).ToListAsync();
                foreach (var item in configs)
                {
                    switch (item.Key)
                    {
                        case ApplicationConstants.KeyAppSettingGoogleFirebaseProjectId:
                            firebaseConfig.ProjectId = item.Value;
                            break;
                        case ApplicationConstants.KeyAppSettingGoogleServiceAccountJson:
                            firebaseConfig.ServiceAccountJson = item.Value;
                            break;
                        case ApplicationConstants.KeyAppSettingGoogleFirebaseCollectionName:
                            firebaseConfig.CollectionName = item.Value;
                            break;
                        case ApplicationConstants.KeyAppSettingGoogleFirebasePreLoadBolCollectionName:
                            firebaseConfig.PreLoadBolCollectionName = item.Value;
                            break;
                        case ApplicationConstants.KeyAppSettingGoogleFirebaseUploadDateTime:
                            firebaseConfig.LastUpdatedDateTime = DateTimeOffset.Parse(item.Value);
                            break;
                        case ApplicationConstants.KeyAppSettingGoogleFirebasePreLoadBolUploadDateTime:
                            firebaseConfig.PreLoadBolLastUpdatedDateTime = DateTimeOffset.Parse(item.Value);
                            break;
                        //edit bol details
                        case ApplicationConstants.KeyAppSettingGoogleFirebaseEditedPreLoadBolCollectionName:
                            firebaseConfig.EditedPreLoadBolCollectionName = item.Value;
                            break;
                        case ApplicationConstants.KeyAppSettingGoogleFirebaseEditedPreLoadBolSyncDateTime:
                            firebaseConfig.EditedPreLoadBolLastUpdatedDateTime = DateTimeOffset.Parse(item.Value);
                            break;
                        //delete bol records
                        case ApplicationConstants.KeyAppSettingGoogleFirebaseDeletedPreLoadBolCollectionName:
                            firebaseConfig.DeletedPreLoadBolCollectionName = item.Value;
                            break;
                        case ApplicationConstants.KeyAppSettingGoogleFirebaseDeletedPreLoadBolSyncDateTime:
                            firebaseConfig.DeletedPreLoadBolLastUpdatedDateTime = DateTimeOffset.Parse(item.Value);
                            break;
                        //fuel retain
                        case ApplicationConstants.KeyAppSettingGoogleFirebaseFuelRetainCollectionName:
                            firebaseConfig.FuelRetainCollectionName = item.Value;
                            break;
                        case ApplicationConstants.KeyAppSettingGoogleFirebaseFuelRetainSyncDateTime:
                            firebaseConfig.FuelRetainLastUpdatedDateTime = DateTimeOffset.Parse(item.Value);
                            break;

                        //bol pickup retain
                        case ApplicationConstants.KeyAppSettingGoogleFirebasePickupBOLRetainCollectionName:
                            firebaseConfig.PickupBOLRetainCollectionName = item.Value;
                            break;
                        case ApplicationConstants.KeyAppSettingGoogleFirebasePickupBOLRetainSyncDateTime:
                            firebaseConfig.PickupBOLRetainLastUpdatedDateTime = DateTimeOffset.Parse(item.Value);
                            break;

                        case ApplicationConstants.KeyAppSettingGoogleFirebaseCanceledScheduleCollectionName:
                            firebaseConfig.CancelledScheduleCollectionName = item.Value;
                            break;
                        case ApplicationConstants.KeyAppSettingGoogleFirebaseCanceledScheduleSyncDateTime:
                            firebaseConfig.CancelledScheduleLastUpdatedDateTime = DateTimeOffset.Parse(item.Value);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FirebaseDomain", "GetFirebaseConfigurationAsync", ex.Message, ex);
            }
            return firebaseConfig;
        }

        public async Task SetInvoiceDropLastUpdatedDateTimeAsync(string updatedDateTime)
        {
            var sqlCommand = string.Format("UPDATE MstAppSettings SET [Value] = '{0}' WHERE [Key] = '{1}'",
                updatedDateTime, ApplicationConstants.KeyAppSettingGoogleFirebaseUploadDateTime);

            await Context.DataContext.Database.ExecuteSqlCommandAsync(sqlCommand);
        }

        public async Task SetPreLoadBolLastUpdatedDateTimeAsync(string updatedDateTime)
        {
            var sqlCommand = string.Format("UPDATE MstAppSettings SET [Value] = '{0}' WHERE [Key] = '{1}'",
                updatedDateTime, ApplicationConstants.KeyAppSettingGoogleFirebasePreLoadBolUploadDateTime);

            await Context.DataContext.Database.ExecuteSqlCommandAsync(sqlCommand);
        }
        public async Task SetPickupRetainLastUpdatedDateTimeAsync(string updatedDateTime)
        {
            var sqlCommand = string.Format("UPDATE MstAppSettings SET [Value] = '{0}' WHERE [Key] = '{1}'",
                updatedDateTime, ApplicationConstants.KeyAppSettingGoogleFirebasePickupBOLRetainSyncDateTime);

            await Context.DataContext.Database.ExecuteSqlCommandAsync(sqlCommand);
        }

        public async Task<StatusViewModel> AddDocumentToQueueMessage(InvoiceViewModelNew invoiceViewModel, int invoiceStatusId)
        {
            var consolidatedDomain = new ConsolidatedInvoiceDomain(this);
            invoiceViewModel = await consolidatedDomain.SetMobileDropInformation(invoiceViewModel, invoiceStatusId);
            var userContext = new UserContext() { Id = invoiceViewModel.Driver.Id, CompanyId = invoiceViewModel.SupplierCompanyId };
            var invoiceCommonDomain = new InvoiceCommonDomain(consolidatedDomain);
            var response = await invoiceCommonDomain.AddCreateInvioceToQueue(userContext, invoiceViewModel);
            return response;
        }

        public async Task<StatusViewModel> SavePreLoadBolDetails(List<PreLoadBolViewModel> preLoadBolViewModel)
        {
            var consolidatedDomain = new ConsolidatedInvoiceDomain(this);
            var userContext = new UserContext() { Id = preLoadBolViewModel.First().Driver.Id, CompanyId = preLoadBolViewModel.First().SupplierCompanyId };
            var response = await consolidatedDomain.SavePreLoadBolDetails(userContext, preLoadBolViewModel);
            return response;
        }

        public async Task<StatusViewModel> SaveFuelRetainDetails(FuelRetainViewModel fuelRetainViewModel)
        {
            var consolidatedDomain = new ConsolidatedInvoiceDomain(this);
            var response = await consolidatedDomain.SaveFuelRetainDetails(fuelRetainViewModel);
            return response;
        }

        public async Task<StatusViewModel> SaveCancelledScheduleDetails(CancelScheduleModel model)
        {
            var scheduleBuilderDomain = new ScheduleBuilderDomain(this);
            var response = await scheduleBuilderDomain.SaveCanceledScheduleDetails(model);
            return response;
        }

        public async Task<StatusViewModel> SavePickupBolRetainDetails(List<PreLoadBolViewModel> preLoadBolViewModel)
        {
            var consolidatedDomain = new ConsolidatedInvoiceDomain(this);
            var userContext = new UserContext() { Id = preLoadBolViewModel.First().Driver.Id, CompanyId = preLoadBolViewModel.First().SupplierCompanyId };
            var response = await consolidatedDomain.SavePickupBolRetainDetails(userContext, preLoadBolViewModel);
            return response;
        }

        #region Edit/delete pre load bol details

        public async Task<StatusViewModel> SaveEditedPreLoadBolDetails(EditPreLoadBolViewModel editPreLoadBolViewModel)
        {
            var consolidatedDomain = new OrderDomain(this);
            var userContext = new UserContext() { Id = editPreLoadBolViewModel.UserId, CompanyId = editPreLoadBolViewModel.CompanyId };
            var response = await consolidatedDomain.UpdatePreLoadBolDetails(userContext, editPreLoadBolViewModel);
            return response;
        }



        public async Task<StatusViewModel> UpdateDeletedPreLoadBolDetails(EditPreLoadBolViewModel deletePreLoadBolViewModel)
        {
            var consolidatedDomain = new OrderDomain(this);
            var userContext = new UserContext() { Id = deletePreLoadBolViewModel.UserId, CompanyId = deletePreLoadBolViewModel.CompanyId };
            var response = await consolidatedDomain.DeletePreLoadBolDetails(userContext, deletePreLoadBolViewModel);
            return response;
        }



        #endregion Edit/delete pre load bol details
    }
}
