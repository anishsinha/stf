using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class ThirdPartyCompanyInviteViewModel : BaseViewModel
    {
        public ThirdPartyCompanyInviteViewModel()
        {
            InstanceInitialize();
        }

        public ThirdPartyCompanyInviteViewModel(Status status) : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            UserInfo = new UserInfo();
            CompanyInfo = new CompanyInfo();
            FleetInfo = new FleetInfo();
            ServiceOffering = new List<ServiceOffering>();
            IsActive = true;
        }
        public int Id { get; set; }
        public UserInfo UserInfo { get; set; }
        public CompanyInfo CompanyInfo { get; set; }
        public FleetInfo FleetInfo { get; set; }
        public List<ServiceOffering> ServiceOffering { get; set; }
        public string CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public int InvitedBy { get; set; }
        public int InvitedByCompanyId { get; set; }
        public bool IsInvitedCompanyRegistered { get; set; }
        public int RegisteredCompanyId { get; set; }
        public string Token { get; set; }
        
    }
    public class UserInfo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }

    public class CompanyInfo
    {
        public bool IsNewCompany { get; set; } = true;
        public string CompanyName { get; set; }
        public int CompanyTypeId { get; set; }
        public string CompanyAddress { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string Zip { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneType { get; set; }
    }
    public class FleetInfo
    {
        public List<FleetTrailers> FuelAssets { get; set; } = new List<FleetTrailers>();
        public List<FleetTrailers> DefAssets { get; set; } = new List<FleetTrailers>();
    }
    public class FleetTrailers
    {
        public FleetType FleetType { get; set; }
        public DefTrailerAssetType DEFTrailerServiceType { get; set; }
        public FuelTrailerAssetType FuelTrailerServiceTypeFTL { get; set; }
        public int Capacity { get; set; }
        public bool TrailerHasPump { get; set; }
        public bool IsTrailerMetered { get; set; }
        public int Count { get; set; }
        public bool PackagedGoods { get; set; }
        public bool IsFuelAssets { get; set; }
    }
    public class ServiceOffering
    {
        public bool IsEnable { get; set; }
        public ServiceOfferingType ServiceDeliveryType { get; set; }
        public List<ServiceArea> ServiceAreas { get; set; }
    }
    public class ServiceArea
    {
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int? CityId { get; set; }
        public string CityName { get; set; }
        public string ZipCode { get; set; }
    }
    public class CompanyServiceAreaModel
    {
        public bool IsEnable { get; set; }
        public ServiceOfferingType ServiceDeliveryType { get; set; }
        public int CountryId { get; set; }
        public ServiceAreaType AreaWide { get; set; } = ServiceAreaType.StateWide;
        public List<int> StateIds { get; set; } = new List<int>();
        public List<int?> CityIds { get; set; } = new List<int?>();
        public List<string> ZipCodes { get; set; } = new List<string>();
    }
    public class CarrierDetailsModel
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ContactInformation { get; set; }
        public string CompanyAddress { get; set; }
        public string ServiceOffered { get; set; }
        public int FtlTrailers { get; set; }
        public int LtlTrailers { get; set; }
        public int DefTrailers { get; set; }
    }

    public class ThirdPartyCompanyFilter
    {
        public int CountryId { get; set; }
        public string States { get; set; }
        public string ZipCodes { get; set; }
        public string ServicesOffered { get; set; }
        public bool IsPump { get; set; }
        public bool IsMetered { get; set; }
        public bool IsPackagedGoods { get; set; }
    }
}
