using Newtonsoft.Json;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class CompanyServiceLocationsMapper
    {
        public static List<ThirdPartyCompanyInviteViewModel> ToModel(this List<ThirdPartyCompanyInvites> entity, List<ThirdPartyCompanyInviteViewModel> viewModel = null)
        {
            if (viewModel == null)
            {
                viewModel = new List<ThirdPartyCompanyInviteViewModel>();
            }
            foreach (var companyDetails in entity)
            {
                var response = new ThirdPartyCompanyInviteViewModel();
                response.Id = companyDetails.Id;
                //response.UserInfo = JsonConvert.DeserializeObject<UserInfo>(companyDetails.UserInfo);
                //response.CompanyInfo = JsonConvert.DeserializeObject<ContactInfo>(companyDetails.CompanyInfo);
                response.FleetInfo = JsonConvert.DeserializeObject<FleetInfo>(companyDetails.FleetInfo);
                response.ServiceOffering = JsonConvert.DeserializeObject<List<ServiceOffering>>(companyDetails.ServiceOffering);
                response.CreatedDate = Convert.ToDateTime(companyDetails.CreatedDate.ToString()).ToShortDateString();
                response.IsActive = companyDetails.IsActive;
                response.InvitedByCompanyId = companyDetails.InvitedByCompanyId;
                viewModel.Add(response);
            }
            return viewModel;
        }

        public static List<CompanyXServingLocation> ToEntity(this List<ServiceOffering> viewModel, List<CompanyXServingLocation> entity = null)
        {
            if (entity == null)
            {
                entity = new List<CompanyXServingLocation>();
            }
            foreach (var companyServing in viewModel)
            {
                var serviceCities = companyServing.ServiceAreas.GroupBy(t => new { t.StateId, t.CityId }).Select(t => new { t.Key.StateId, t.Key.CityId, zips = t.ToList().Select(t1 => t1.ZipCode).ToList() });
                foreach (var serviceArea in serviceCities)
                {
                    var response = new CompanyXServingLocation { ServiceOfferingType = companyServing.ServiceDeliveryType };
                    response.StateId = serviceArea.StateId;
                    response.CityId = serviceArea.CityId;
                    response.ZipCode = string.Join(",", serviceArea.zips);
                    entity.Add(response);
                }
            }
            return entity;
        }

        public static ThirdPartyCompanyInviteViewModel ToOnboardViewModel(this ThirdPartyCompanyInvites entity, ThirdPartyCompanyInviteViewModel response = null)
        {
            if (response == null)
            {
                response = new ThirdPartyCompanyInviteViewModel();
            }

            response.Id = entity.Id;
            response.UserInfo = JsonConvert.DeserializeObject<UserInfo>(entity.UserInfo);
            response.CompanyInfo = JsonConvert.DeserializeObject<CompanyInfo>(entity.CompanyInfos);
            response.FleetInfo = JsonConvert.DeserializeObject<FleetInfo>(entity.FleetInfo);
            response.ServiceOffering = JsonConvert.DeserializeObject<List<ServiceOffering>>(entity.ServiceOffering);
            response.IsActive = entity.IsActive;

            return response;

        }

        public static CompanyAddress ToEntity(this CompanyInfo viewModel, CompanyAddress response = null)
        {
            if (response == null)
            {
                response = new CompanyAddress();
            }

            response.Address = viewModel.CompanyAddress;
            response.CountryId = viewModel.CountryId;
            response.StateId = viewModel.StateId;
            response.City = viewModel.City;
            response.ZipCode = viewModel.Zip;
            response.IsActive = true;
            response.IsDefault = true;
            response.PhoneNumber = viewModel.PhoneNumber;
            response.PhoneTypeId = int.TryParse(viewModel.PhoneType, out int phoneTypeId) ? phoneTypeId : 1;
            return response;
        }
        public static List<CompanyServiceAreaModel> ToViewModel(this List<CompanyXServingLocation> entity, List<CompanyServiceAreaModel> viewModel = null)
        {
            if (viewModel == null)
            {
                viewModel = new List<CompanyServiceAreaModel>();
            }
            var serviceLocations = entity.GroupBy(t => t.ServiceOfferingType).Select(t => new { t.Key, CountryId = t.Select(t1 => t1.MstState.CountryId).FirstOrDefault() ,Areas = t.ToList() }).ToList();

            foreach (var item in Enum.GetValues(typeof(ServiceOfferingType)))
            {
                var serviceOffer = new CompanyServiceAreaModel
                {
                    ServiceDeliveryType = (ServiceOfferingType)item
                };
                var servingCompany = serviceLocations.Where(t => t.Key == (ServiceOfferingType)item).FirstOrDefault();
                if(servingCompany != null)
                {
                    serviceOffer.IsEnable = servingCompany.Areas.Any();
                    serviceOffer.CountryId = servingCompany.CountryId;
                    serviceOffer.StateIds = servingCompany.Areas.Select(t => t.StateId).ToList();
                    serviceOffer.CityIds = servingCompany.Areas.Where(t => t.CityId.HasValue).Select(t => t.CityId).ToList();
                    serviceOffer.AreaWide = servingCompany.Areas.Where(t => t.CityId.HasValue).Any() ? ServiceAreaType.ZipWide : ServiceAreaType.StateWide;
                    servingCompany.Areas.Where(t => t.CityId.HasValue && t.ZipCode != null).Select(t => t.ZipCode).ToList().ForEach(t => {
                        serviceOffer.ZipCodes.AddRange(t.Split(',').ToList());
                    });
                }
                viewModel.Add(serviceOffer);
            }
            //if (entity.Any())
            //{
            //    //var serviceLocations = entity.GroupBy(t => t.ServiceOfferingType).Select(t => new { t.Key, Areas = t.ToList() }).ToList();
            //    foreach (var servingCompany in serviceLocations)
            //    {
            //        var serviceOffer = new CompanyServiceAreaModel { 
            //            ServiceDeliveryType = servingCompany.Key, 
            //            IsEnable = servingCompany.Areas.Any(),
            //            StateIds = servingCompany.Areas.Select(t => t.StateId).ToList(),
            //            CityIds = servingCompany.Areas.Select(t => t.CityId).ToList()
                        
            //        };
            //        servingCompany.Areas.Select(t => t.ZipCode).ToList().ForEach(t => {
            //            serviceOffer.ZipCodes.AddRange(t.Split(',').ToList());
            //        });
            //        viewModel.Add(serviceOffer);
            //    }
            //}
            return viewModel;
        }

        public static List<ServiceOffering> ToServiceViewModel(this List<CompanyXServingLocation> entity, List<ServiceOffering> viewModel = null)
        {
            if (viewModel == null)
            {
                viewModel = new List<ServiceOffering>();
            }
            var serviceLocations = entity.GroupBy(t => t.ServiceOfferingType).Select(t => new { t.Key, Areas = t.ToList() }).ToList();

            foreach (var item in Enum.GetValues(typeof(ServiceOfferingType)))
            {
                var serviceOffer = new ServiceOffering
                {
                    ServiceDeliveryType = (ServiceOfferingType)item
                };
                var servingCompany = serviceLocations.Where(t => t.Key == (ServiceOfferingType)item).FirstOrDefault();
                if (servingCompany != null)
                {
                    serviceOffer.IsEnable = servingCompany.Areas.Any();
                    serviceOffer.ServiceAreas = new List<ServiceArea>();
                    foreach (var area in servingCompany.Areas)
                    {
                        var serArea = new ServiceArea { StateId = area.StateId, CityId = area.CityId, ZipCode = area.ZipCode };
                        if(area.StateId > 0 && area.MstState != null)
                        {
                            serArea.CountryId = area.MstState.CountryId;
                            serArea.CityName = area.MstState.MstCities.Where(t1 => t1.Id == area.CityId).Select(t1 => t1.Name).FirstOrDefault();
                        }
                        serviceOffer.ServiceAreas.Add(serArea);
                    }
                }
                viewModel.Add(serviceOffer);
            }
            return viewModel;
        }
    }
}
