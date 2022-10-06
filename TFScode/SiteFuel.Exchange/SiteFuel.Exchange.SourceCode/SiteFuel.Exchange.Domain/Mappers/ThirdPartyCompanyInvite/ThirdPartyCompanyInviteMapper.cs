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
    public static class ThirdPartyCompanyInviteMapper
    {
        public static ThirdPartyCompanyInvites ToEntity(this ThirdPartyCompanyInviteViewModel viewModel, ThirdPartyCompanyInvites entity = null)
        {
            if (entity == null)
            {
                entity = new ThirdPartyCompanyInvites();
            }
            entity.UserInfo = JsonConvert.SerializeObject(viewModel.UserInfo);
            entity.CompanyInfos = JsonConvert.SerializeObject(viewModel.CompanyInfo);
            entity.FleetInfo = JsonConvert.SerializeObject(viewModel.FleetInfo);
            entity.ServiceOffering = JsonConvert.SerializeObject(viewModel.ServiceOffering);
            entity.InvitedByCompanyId = viewModel.InvitedByCompanyId;
            entity.IsInvitedCompanyRegistered = viewModel.IsInvitedCompanyRegistered;
            entity.RegisteredCompanyId = viewModel.RegisteredCompanyId;
            entity.IsActive = viewModel.IsActive;
            entity.CreatedDate = DateTimeOffset.Now;
            return entity;
        }
        public static ThirdPartyCompanyInviteViewModel ToViewModel(this ThirdPartyCompanyInvites entity, ThirdPartyCompanyInviteViewModel viewModel = null)
        {
            if (viewModel == null)
            {
                viewModel = new ThirdPartyCompanyInviteViewModel();
            }
            viewModel.Id = entity.Id;
            viewModel.UserInfo = JsonConvert.DeserializeObject<UserInfo>(entity.UserInfo);
            viewModel.CompanyInfo = JsonConvert.DeserializeObject<CompanyInfo>(entity.CompanyInfos);
            viewModel.FleetInfo = JsonConvert.DeserializeObject<FleetInfo>(entity.FleetInfo);
            viewModel.ServiceOffering = JsonConvert.DeserializeObject<List<ServiceOffering>>(entity.ServiceOffering);
            viewModel.CreatedDate = Convert.ToDateTime(entity.CreatedDate.ToString()).ToShortDateString();
            viewModel.InvitedByCompanyId = entity.InvitedByCompanyId;
            viewModel.IsInvitedCompanyRegistered = entity.IsInvitedCompanyRegistered;
            viewModel.RegisteredCompanyId = entity.RegisteredCompanyId;
            viewModel.IsActive = entity.IsActive;
            viewModel.InvitedByCompanyId = entity.InvitedByCompanyId;
            return viewModel;
        }
        public static CarrierDetailsModel ToCarrierViewModel(this ThirdPartyCompanyInviteViewModel entity, CarrierDetailsModel viewModel = null)
        {
            if (viewModel == null)
            {
                viewModel = new CarrierDetailsModel();
            }
            viewModel.Id = entity.Id;
            viewModel.CompanyName = entity.CompanyInfo.CompanyName;
            viewModel.ContactInformation = $"{entity.UserInfo.FirstName} {entity.UserInfo.LastName}";
            viewModel.Email = entity.UserInfo.Email;
            viewModel.PhoneNumber = entity.CompanyInfo.PhoneNumber;
            viewModel.CompanyAddress = $"{entity.CompanyInfo.CompanyAddress}, {entity.CompanyInfo.City}";
            viewModel.ServiceOffered = string.Join(", ", entity.ServiceOffering.Where(t => t.IsEnable).Select(t => t.ServiceDeliveryType.GetDisplayName()).ToList());
            viewModel.FtlTrailers = entity.FleetInfo.FuelAssets.Where(t => t.FleetType != FleetType.DEF && t.FuelTrailerServiceTypeFTL == FuelTrailerAssetType.FTL).Count();
            viewModel.LtlTrailers = entity.FleetInfo.FuelAssets.Where(t => t.FleetType != FleetType.DEF && t.FuelTrailerServiceTypeFTL == FuelTrailerAssetType.LTL).Count();
            viewModel.DefTrailers = entity.FleetInfo.FuelAssets.Where(t => t.FleetType == FleetType.DEF).Count();
            return viewModel;
        }    
        public static InvitedUser ToExistingEntity(this ThirdPartyCompanyInviteViewModel viewModel, InvitedUser entity = null)
        {
            if (entity == null)
            {
                entity = new InvitedUser();
            }
            entity.Title = viewModel.UserInfo.Title;
            entity.FirstName = viewModel.UserInfo.FirstName;
            entity.LastName = viewModel.UserInfo.LastName;
            entity.Email = viewModel.UserInfo.Email;
            entity.IsActive = true;
            entity.CreatedDate = DateTimeOffset.Now;
            return entity;
        }
    }
}
