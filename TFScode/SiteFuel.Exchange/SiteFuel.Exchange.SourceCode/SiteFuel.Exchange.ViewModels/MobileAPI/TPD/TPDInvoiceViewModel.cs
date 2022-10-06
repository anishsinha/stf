using Newtonsoft.Json;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SiteFuel.Exchange.ViewModels.MobileAPI.TPD
{
    public class TPDInvoiceViewModel
    {
        public string CustomerID { get; set; }
        public string CarrierOrderID { get; set; }
        public string LocationId { get; set; }
        public string DriverFirstName { get; set; }
        public string DriverLastName { get; set; }
        public string DropAddress1 { get; set; }
        public string DropAddress2 { get; set; }
        public string DropCity { get; set; }
        public string DropStateCode { get; set; }
        public string DropZip { get; set; }
        public string DropLatitude { get; set; }
        public string DropLongitude { get; set; }
        public string Notes { get; set; }
        public string SiteOutOfFuel { get; set; }
        public string OutOfFuelProduct { get; set; }
        public string SupplierInvoiceNumber { get; set; }
        public string DropDemurrageTime { get; set; }
        public string DropDemurrageFees { get; set; }
        public string DropWethoseFees { get; set; }
        public string DropFreightFees { get; set; }
        public string DropSurchargeFees { get; set; }
        public string DropDryRunCount { get; set; }
        public string DropDryRunFees { get; set; }
        public string DropLoadFees { get; set; }
        public string DropEnvironmentalFees { get; set; }
        public string DropServiceFees { get; set; }
        public string DropOverWaterFees { get; set; }
        public string DropOtherFees { get; set; }
        public string ExternalRefID { get; set; }
        public int? DriverId { get; set; }
        public List<TPDDropDetails> DropDetails { get; set; } = new List<TPDDropDetails>(); 
    }
    public class TPDDropDetails
    {
        public int? OrderId { get; set; }
        public string Product { get; set; }
        public string PONumber { get; set; }
        public string FuelCost { get; set; }
        public string DropArrivalDate { get; set; }
        public string DropCompleteDate { get; set; }
        public string DropArrivalTime { get; set; }
        public string DropCompleteTime { get; set; }
        public string CarrierOrder { get; set; }
        public string CarrierOrderID { get; set; }
        public string OrderDate { get; set; }
        public decimal OrderQuantity { get; set; }
        public string Tractor { get; set; }
        public string Truck { get; set; }
        public string LoadingBadge { get; set; }
        public string DropTicketNumber { get; set; }
        public decimal TotalDropQuantity { get; set; }
        public int? FuelTypeId { get; set; }
        public decimal? ApiGravity { get; set; }
        public List<TPDBols> BolDetails { get; set; } = new List<TPDBols>();
        public List<TPDTanks> Tanks { get; set; } = new List<TPDTanks>();
        public List<TPDLifts> LiftDetails { get; set; } = new List<TPDLifts>();
        public string FreightRateRuleType { get; set; }
        public string FreightRateTableName { get; set; }
        public string FreightRateTableType { get; set; }
        public decimal Distance { get; set; }
        public decimal FreightCost { get; set; }
        public string FuelSurchargeTableType { get; set; }
        public string FuelSurchargeTableName { get; set; }
        public string AccessorialFeesTableType { get; set; }
        public string AccessorialFeesTableName { get; set; }
    }
    public class TPDTanks
    {
        public string TankId { get; set; }
        public decimal DropQuantity { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public decimal PreDip { get; set; }
        public decimal PostDip { get; set; }
    }
    public class TPDBols
    {
        public string BolNumber { get; set; }
        public string BolCreationTime { get; set; }
        public decimal BolNet { get; set; }

        public string LiftStartTime { get; set; }

        public string LiftEndTime { get; set; }
        public decimal BolGross { get; set; }
        public decimal BolDelivered { get; set; }
        public string BolCarrier { get; set; }
        public string TerminalControl { get; set; }
    }
    public class TPDLifts
    {
        public string LiftTicketNumber { get; set; }
        public decimal LiftQuantity { get; set; }
        public string LiftDate { get; set; }
        public string LiftArrivalTime { get; set; }
        public string LiftStartTime { get; set; }
        public string LiftEndTime { get; set; }
        public string LiftTicketCreationTime { get; set; }
        public string BulkPlantName { get; set; }
        public string LiftAddressStreet1 { get; set; }
        public string LiftAddressStreet2 { get; set; }
        public string LiftAddressCity { get; set; }
        public string LiftAddressCounty { get; set; }
        public string LiftAddressState { get; set; }
        public string LiftAddressZip { get; set; }
        public string LiftAddressLat { get; set; }
        public string LiftAddressLong { get; set; }
        public decimal LiftNet { get; set; }
        public decimal LiftGross { get; set; }
        public decimal LiftDelivered { get; set; }
        public string LiftCarrier { get; set; }
    }

    public class TPDImageFileUploadViewModel
    {
        public string EntityId { get; set; }
        public string CustomerId { get; set; }
        public string CarrierOrderID { get; set; }
        public string ExternalRefID { get; set; }
        public HttpPostedFile DropFile { get; set; }
        public HttpPostedFile BolFile { get; set; }
        public HttpPostedFile SignatureFile { get; set; }
        public HttpPostedFile AdditionalFile { get; set; }
        public HttpPostedFile InspectionVoucherFile { get; set; }
    }

    public class TPDImagesToUpdate
    {
        public ImageViewModel DropImage { get; set; }
        public ImageViewModel BolImage { get; set; }
        public ImageViewModel AdditionalImage { get; set; }
        public ImageViewModel InspectionVoucherImage { get; set; }
        public ImageViewModel SignatureImage { get; set; }
        public string BrokerChainId { get; set; }
        public bool CanConverToInvoice { get; set; }
        public string ExternalRefID { get; set; }
    }

    public class BrokerImageUpdateProcessorViewModel
    {
        public TPDImagesToUpdate ImagesToUpdate { get; set; } = new TPDImagesToUpdate();
        public int UpdateFromDDTId { get; set; }
    }

    #region DS ViewModels
    public class TPDCreateScheduleViewModel
    {
        public string DriverFirstName { get; set; }
        public string DriverLastName { get; set; }
        public long DriverContactNumber { get; set; }
        public string DriverEmail { get; set; }

        public string ExternalRefID { get; set; }
        public string ScheduleDate { get; set; }
        public string ScheduleStartTime { get; set; }
        public string ScheduleEndTime { get; set; }
        //public bool IsCommonPickUp { get; set; }
        //public TPDBulkPlantAddressModel PickUpAddress { get; set; }
        public List<TpdDeliverySchedule> ScheduleDetails { get; set; }
    }

    public class TpdDeliverySchedule
    {
        public string PoNumber { get; set; }
        public string LocationID { get; set; }
        public string ProductID { get; set; }
        public string CustomerID { get; set; }
        public decimal ScheduleQuantity { get; set; }
        public string TankID { get; set; }
        public string StorageID { get; set; }
        public string BadgeNumber { get; set; }
        public string TerminalControlNumber { get; set; }
        public string DispatcherNote { get; set; }
        public string CarrierOrderID { get; set; }
        public TPDBulkPlantAddressModel BulkPlantDetails { get; set; }
    }

    public class ProcessDSBCreation
    {
        public int CarrierCompanyId { get; set; }
        public string CarrierCompanyName { get; set; }
        public string CarrierRegionId { get; set; }
        public string SupplierRegionId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string ExternalRefID { get; set; }
        public List<DropAddressViewModel> Terminals { get; set; } = new List<DropAddressViewModel>();
        public DropdownDisplayExtendedItem Drivers { get; set; } = new DropdownDisplayExtendedItem();
        public List<ScheduleDetails> ScheduleDetails { get; set; } = new List<ScheduleDetails>();
        public List<ScheduleApiResponse> ApiResponseModel { get; set; } = new List<ScheduleApiResponse>();
    }

    public class ScheduleDetails
    {
        public int JobId { get; set; }
        public string JobName { get; set; }
        public string JobAddress { get; set; }
        public string JobCity { get; set; }
        public int OrderId { get; set; }
        public string PoNumber { get; set; }
        public string CustomerCompanyName { get; set; }
        public int SupplierCompanyId { get; set; }
        public string SupplierCompanyName { get; set; }
        public string TCNFromAPI { get; set; } //used only to remove multiple calls to MstExternalTerminalTable
        public DropdownDisplayItem Terminal { get; set; }
        public PickupLocationType PickupLocationType { get; set; }
        public DropAddressViewModel BulkPlant { get; set; }
        public string TankId { get; set; }
        public string StorageId { get; set; }
        public string BadgeNumber { get; set; }
        public decimal RequiredQuantity { get; set; }
        public string DispatcherNote { get; set; }
        public int FuelTypeId { get; set; }
        public string SiteId { get; set; }
        public string CarrierOrderID { get; set; }
        public string TimeZoneName { get; set; }
        public Currency Currency { get; set; }
        public int ProductTypeId { get; set; }
        public string ProductTypeName { get; set; }
        public UoM UoM { get; set; }
    }

    public class TPDBulkPlantAddressModel
    {
        public string SiteName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string CountyName { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }

    public class TPDTerminalDetails
    {
        public string APITcn { get; set; }
        public DropAddressViewModel Terminal { get; set; } = new DropAddressViewModel();
    }
    #endregion

    public class TPDOrderDetails
    {
        public string PONumber { get; set; }
        public string Customer { get; set; }
        public string Location { get; set; }
        public string Product { get; set; }
    }


    public class TPDScheduleStatusViewModel
    {
        public string CarrierOrderID { get; set; }
        public int TFXScheduleID { get; set; }
        public string DriversLatestLat { get; set; }
        public string DriversLatestLong { get; set; }
        public int DeliveryScheduleStatus { get; set; }
        public string ExternalRefID { get; set; }
    }

    public class TPDCustomerViewModel
    {
        public string TFXCompanyId { get; set; }

        [Required]
        [DisplayName("Customer Company Name")]
        public string CustomerCompanyName { get; set; }

        [Required]
        [DisplayName("Company Address Line1")]
        public string CompanyAddressLine1 { get; set; }

        [Required]
        [DisplayName("Company Address City")]
        public string CompanyAddressCity { get; set; }

        [Required]
        [DisplayName("Company Address County")]
        public string CompanyAddressCounty { get; set; }

        [Required]
        [DisplayName("Company Address State")]
        public string CompanyAddressState { get; set; }

        [Required]
        [DisplayName("Company Address Zip")]
        public string CompanyAddressZip { get; set; }

        [Required]
        [DisplayName("Company Address Country")]
        public string CompanyAddressCountry { get; set; }

        [DisplayName("Company Address Office Number")]
        public string CompanyAddressOfficeNumber { get; set; }

        [DisplayName("Company Address Mobile Number")]
        public string CompanyAddressMobileNumber { get; set; }

        public List<TPDCompanyBillingAddressModel> CompanyBillingAddresses = new List<TPDCompanyBillingAddressModel>();

        [Required]
        [DisplayName("User(s)")]
        public List<TPDUserViewModel> Users { get; set; }
        //[Required]
        [DisplayName("ExternalRefID")]
        public string ExternalRefID { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool SendInvitationLinkToUser { get; set; }

    }

    public class TPDCompanyBillingAddressModel
    {
        public string CompanyBillingAddressName { get; set; }

        [DisplayName("Company Billing AddressLine1")]
        public string CompanyBillingAddressLine1 { get; set; }
        public string CompanyBillingAddressCity { get; set; }
        public string CompanyBillingAddressCounty { get; set; }
        public string CompanyBillingAddressState { get; set; }
        public string CompanyBillingAddressZip { get; set; }
        public string CompanyBillingAddressCountry { get; set; }
        public string CompanyBillingAddressOfficeNumer { get; set; }
        public string CompanyBillingAddressMobileNumber { get; set; }
    }

    public class TPDUserViewModel
    {
        [Required]
        [DisplayName("User FirstName")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("User LastName")]
        public string LastName { get; set; }

        [Required]
        [DisplayName("User Email")]
        public string Email { get; set; }

        [Required]
        [DisplayName("User PhoneNumber")]
        public string PhoneNumber { get; set; }

        [Required]
        [DisplayName("User Role")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int Role { get; set; }
    }

    public class TPDLocationViewModel
    {
        public string TfxLocationID { get; set; }
        public string LocationName { get; set; }
        public string ThirdPartyLocationID { get; set; }
        public string LocationStartDate { get; set; }

        public string LocationEndDate { get; set; }
        public string LocationAddressLine1 { get; set; }

        public string LocationAddressLine2 { get; set; }
        public string LocationAddressLine3 { get; set; }
        public string LocationAddressCity { get; set; }
        public string LocationAddressCounty { get; set; }
        public string LocationAddressState { get; set; }
        public string LocationAddressZip { get; set; }
        public string LocationAddressCountry { get; set; }
        public decimal LocationAddressLat { get; set; }
        public decimal LocationAddressLong { get; set; }
        public string LocationBillToAddressName { get; set; }
        public string LocationBillToAddressLine1 { get; set; }
        public string LocationBillToAddressLine2 { get; set; }
        public string LocationBillToAddressLine3 { get; set; }
        public string LocationBillToAddressCity { get; set; }
        public string LocationBillToAddressCounty { get; set; }
        public string LocationBillToAddressState { get; set; }
        public string LocationBillToAddressZip { get; set; }
        public string LocationBillToAddressCountry { get; set; }
        public string SiteInstruction { get; set; }
        public bool SetDeliveryAcknowledgement { get; set; }
        public string LocationXRefID { get; set; }

        //used only for internal purpose
        [JsonIgnore]
        public int JobId { get; set; }
        [JsonIgnore]
        public int StateId { get; set; }

        [JsonIgnore]
        public int CountryId { get; set; }
        [JsonIgnore]
        public DateTimeOffset StartDate { get; set; }
        [JsonIgnore]
        public DateTimeOffset? EndDate { get; set; }
        [JsonIgnore]
        public int BuyerCompanyUserId { get; set; }

        [JsonIgnore]
        public string TimeZoneName { get; set; }
        [JsonIgnore]
        public int BillingAddressId { get; set; }
    }

    public class TPDLocationCreateModel
    {
        public string TFXCompanyId { get; set; }
        public string ExternalRefID { get; set; }
        public List<TPDLocationViewModel> Locations { get; set; } = new List<TPDLocationViewModel>();
    }

    public class TPDDeliveryDetailsViewModel
    {
        public string TFXOrderNo { get; set; }
        public string LiftTicketNo { get; set; }
        public string TerminalControl { get; set; }
        public string CustomerID { get; set; }
        public string LocationID { get; set; }
        public string LiftDate { get; set; }
        public string LiftStartTime { get; set; }
        public string LiftEndTime { get; set; }
        public string SAP_Doc_No { get; set; }
        public string DropTicketNo { get; set; }
        public string TruckID { get; set; }
        public TPDProductDetailsViewModel Product { get; set; }

    }
    public class TPDProductDetailsViewModel
    {
        public string ProductID { get; set; }
        public decimal TotalDropQuantity { get; set; }
        public UoM UoM { get; set; }
        public decimal Price { get; set; }
    }
}
