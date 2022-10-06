using Newtonsoft.Json;
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
    public class DeliveryReqJobInfoDomain : FreightServiceApiDomain
    {
        public DeliveryReqJobInfoDomain()
            : base(ContextFactory.Current.ConnectionString)
        {
        }

        public DeliveryReqJobInfoDomain(string connectionString) : base(connectionString)
        {
        }

        public DeliveryReqJobInfoDomain(BaseDomain domain)
            : base(domain)
        {
        }

        protected async Task SetCustomerBrandId(int companyId, List<DeliveryRequestViewModel> response)
        {
            try
            {
                var jobIds = response.Select(t => t.JobId).Distinct().ToList();
                var buyerCompanyIdsOfJob = await Context.DataContext.Jobs.Where(t => jobIds.Contains(t.Id)).Select(t => new
                {
                    BuyerCompanyId = t.CompanyId,
                    JobId = t.Id
                }).ToListAsync();
                var buyerCompanyIds = buyerCompanyIdsOfJob.Select(t => t.BuyerCompanyId).Distinct().ToList();
                var customerIds = await Context.DataContext.SupplierXBuyerSettings.
                                           Where(t => t.SupplierCompanyId == companyId && buyerCompanyIds.Contains(t.BuyerCompanyId)).
                                           Select(t => new { t.SupplierCompanyId, t.BuyerCompanyId, t.CustomerId }).ToListAsync();
                foreach (var dr in response)
                {
                    var BuyerCompId = buyerCompanyIdsOfJob.Where(t => t.JobId == dr.JobId).Select(t => t.BuyerCompanyId).FirstOrDefault();
                    var customerBrandId = customerIds.Where(t => t.BuyerCompanyId == BuyerCompId && t.SupplierCompanyId == companyId).Select(t => t.CustomerId).FirstOrDefault();
                    dr.CustomerBrandId = customerBrandId;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryReqJobInfoDomain", "SetCustomerBrandId", ex.Message, ex);
            }
        }


        protected void SetLoadAndDRQueueAttributes(List<DeliveryRequestViewModel> drs, int companyId)
        {
            try
            {
                if (drs != null && drs.Any())
                {
                    bool isDRQueueSettingExists = false;
                    bool isLoadQueueSetttingExists = false;
                    var attributes = Context.DataContext.OnboardingPreferences.Where(t => t.CompanyId == companyId && t.IsActive)
                                                        .Select(t => new
                                                        {
                                                            LoadQueueAttribute = t.LoadQueueAttributes,
                                                            DRQueueAttributes = t.DRQueueAttributes
                                                        }).FirstOrDefault();
                    var drQueueAttributes = new DRQueueAttributesViewModel();
                    var loadQueueAttributes = new LoadQueueAttributesViewModel();
                    if (attributes != null)
                    {
                        if (!string.IsNullOrWhiteSpace(attributes.LoadQueueAttribute))
                        {
                            var loadQueuesetting = JsonConvert.DeserializeObject<LoadQueueAttributesViewModel>(attributes.LoadQueueAttribute);
                            loadQueueAttributes.CustomerName = loadQueuesetting.CustomerName;
                            loadQueueAttributes.Driver = loadQueuesetting.Driver;
                            loadQueueAttributes.LocationName = loadQueuesetting.LocationName;
                            loadQueueAttributes.TrailerName = loadQueuesetting.TrailerName;
                            isLoadQueueSetttingExists = true;
                        }
                        if (!string.IsNullOrWhiteSpace(attributes.DRQueueAttributes))
                        {
                            var drQueuesetting = JsonConvert.DeserializeObject<DRQueueAttributesViewModel>(attributes.DRQueueAttributes);
                            drQueueAttributes.CustomerName = drQueuesetting.CustomerName;
                            drQueueAttributes.DeliveryShift = drQueuesetting.DeliveryShift;
                            drQueueAttributes.HoursToCoverDistance = drQueuesetting.HoursToCoverDistance;
                            drQueueAttributes.TrailerCompatibility = drQueuesetting.TrailerCompatibility;
                            isDRQueueSettingExists = true;
                        }
                    }
                    if (!isDRQueueSettingExists)
                    {
                        drQueueAttributes.CustomerName = true;
                        drQueueAttributes.DeliveryShift = true;
                        drQueueAttributes.HoursToCoverDistance = true;
                        drQueueAttributes.TrailerCompatibility = true;
                    }
                    if (!isLoadQueueSetttingExists)
                    {
                        loadQueueAttributes.CustomerName = true;
                        loadQueueAttributes.Driver = true;
                        loadQueueAttributes.LocationName = true;
                        loadQueueAttributes.TrailerName = true;
                    }
                    foreach (DeliveryRequestViewModel item in drs)
                    {
                        item.LoadQueueAttributes = loadQueueAttributes;
                        item.DRQueueAttributes = drQueueAttributes;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryReqJobInfoDomain", "SetLoadAndDRQueueAttributes", ex.Message, ex);
            }
        }
    }
}
