using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.Tank;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SiteFuel.Exchange.ViewModels
{
    public class JobLocationReponse
    {
        public List<JobLocationDetailsViewModel> jobLocationDetails { get; set; } = new List<JobLocationDetailsViewModel>();
        public List<JobCities> citiesDetails { get; set; } = new List<JobCities>();
        public List<JobStates> stateDetails { get; set; } = new List<JobStates>();
        public List<JobCustomers> customerDetails { get; set; } = new List<JobCustomers>();
        public List<string> fuelTypeDetails { get; set; } = new List<string>();
    }
    public class JobLocationDetailsViewModel
    {
        public string CustomerName { get; set; }
        public int CustomerId { get; set; }
        public int CountryId { get; set; }
        public string CountryCode { get; set; }
        public InventoryDataCaptureType InventoryDataCaptureType { get; set; }
        public int JobID { get; set; }
        public string JobName { get; set; }
        public int JobLocationType { get; set; }
        public string JobLocationTypeName
        {
            get
            {
                if (this.JobLocationType > 0)
                {
                    return Enum.GetName(typeof(JobLocationTypes), this.JobLocationType);
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int? CityId { get; set; }

        public string State { get; set; }
        public int? StateID { get; set; }
        public string ZipCode { get; set; }
        public string ContractNumber { get; set; }
        public int IsContactDetailsAvailable { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPhoneNumber { get; set; }
        public int IsPhoneNumberConfirmed { get; set; }
        public string SiteImageFilePath { get; set; }
        public string SiteAvailability { get; set; }
        public List<string> SiteAvailabilityArray
        {
            get
            {
                if (!string.IsNullOrEmpty(this.SiteAvailability))
                {
                    return this.SiteAvailability.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                }
                return new List<string>();
            }
        }
        public int SiteAvailabilityTotalDays { get; set; }
        public string SiteAvailabilityTiming { get; set; }
        public string SiteInstructions { get; set; }
        public int TankCount { get; set; }
        public int AssetCount { get; set; }
        public int TotalCount { get { return TankCount + AssetCount; } }
        public List<JobDeliveryRequestsViewModel> jobDeliveryRequests { get; set; } = new List<JobDeliveryRequestsViewModel>();
        public List<JobAssetDetailsViewModel> jobAssetDetails { get; set; } = new List<JobAssetDetailsViewModel>();
        public string FuleTypeID { get; set; }
        public string FuelTypeName { get; set; }
        public List<string> FuelTypeNameList
        {
            get
            {
                return string.IsNullOrEmpty(this.FuelTypeName) ? new List<string>() : this.FuelTypeName.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
        }
        public string ScheduleStatus { get; set; }
        public List<JobSupplierForBuyer> supplierDetails { get; set; } = new List<JobSupplierForBuyer>();//for buyer wally board supplier filter DDL
        public List<JobCarrierForBuyer> carrierDetails { get; set; } = new List<JobCarrierForBuyer>();
        public string RegionId { get; set; }
    }
    public class JobDeliveryRequestsViewModel
    {
        public string Id { get; set; }
        public string TfxProductType { get; set; }
        public int TfxUoM { get; set; }
        public decimal RequiredQuantity { get; set; }
        public DeliveryReqPriority Priority { get; set; }

        public string DeliveryReqPriority
        {
            get
            {
                if (this.Priority > 0)
                {
                    return Enum.GetName(typeof(DeliveryReqPriority), this.Priority);
                }
                else
                {
                    return string.Empty;
                }
            }

        }
        public int Status { get; set; }
        public string DeliveryReqStatus
        {
            get
            {
                if (this.Status > 0)
                {
                    return Enum.GetName(typeof(DeliveryReqStatus), this.Status);
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string StorageTypeId { get; set; }
        public string StorageId { get; set; }
        public string CreatedRegionId { get; set; }
        public int TfxJobId { get; set; }
    }
    public class JobAssetTankDetailsViewModel
    {
        public int TfxAssetId { get; set; }
        public string TankId { get; set; }
        public string TankName { get; set; }
        public string TankNumber { get; set; }
        public string StorageId { get; set; }
        public decimal? ThresholdDeliveryRequest { get; set; }
        public FillType? FillType { get; set; }
        public string FillTypeStatus
        {
            get
            {
                if (this.FillType > 0)
                {
                    return Enum.GetName(typeof(FillType), this.FillType);
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public DipTestMethod? DipTestMethod { get; set; }
        public string DipTestMethodName
        {
            get
            {
                if (this.DipTestMethod > 0)
                {
                    return Enum.GetName(typeof(DipTestMethod), this.DipTestMethod);
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public ManiFolded? ManiFolded { get; set; }
        public string ManiFoldedName
        {
            get
            {
                if (this.ManiFolded > 0)
                {
                    return Enum.GetName(typeof(ManiFolded), this.ManiFolded);
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string TfxProductTypeName { get; set; }
        public string SiteId { get; set; }
        public float LastReading { get; set; }
        public DateTime CaptureTime { get; set; }
        public List<JobDipChartDetails> dipChartDetails { get; set; } = new List<JobDipChartDetails>();
        public decimal? MinFill { get; set; }
        public decimal? MinFillPercent { get; set; }
        public decimal? MaxFill { get; set; }
        public decimal? MaxFillPercent { get; set; }
        public decimal? FuelCapacity { get; set; }
        public string TankChartPath { get; set; }
    }

    public class JobAssetDetailsViewModel
    {
        public int AssetId { get; set; }
        public int JobId { get; set; }
        public string AssetName { get; set; }
        public int AssetType { get; set; }
        public string AssetTypeName
        {
            get
            {
                if (this.AssetType > 0)
                {
                    return Enum.GetName(typeof(AssetType), this.AssetType);
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string ProductType { get; set; }
        public decimal FuelCapacity { get; set; }
        public int TankType { get; set; }
        public string TankTypeName
        {
            get
            {
                if (this.TankType > 0)
                {
                    var type = typeof(TankType);
                    TankType tankType = (TankType)this.TankType;
                    var memberInfo = type.GetMember(tankType.ToString());
                    if (memberInfo.Count() > 0)
                    {
                        DisplayAttribute displayName = (DisplayAttribute)memberInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();
                        if (displayName != null)
                        {
                            return displayName.Name;
                        }
                    }
                    return string.Empty;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public decimal MinFill { get; set; }
        public decimal MaxFill { get; set; }
        public decimal ThresholdDeliveryRequest { get; set; }
        public int DipTestMethod { get; set; }
        public string DipTestMethodName
        {
            get
            {
                if (this.DipTestMethod > 0)
                {
                    return Enum.GetName(typeof(TankType), this.DipTestMethod);
                }
                else
                {
                    //DEFAULT value here. 
                    return string.Empty;
                }
            }
        }
        public List<JobAssetTankDetailsViewModel> jobTankAdditionalDetails { get; set; } = new List<JobAssetTankDetailsViewModel>();

    }

    public class JobCities
    {
        public string Name { get; set; }
        public int? Id { get; set; }
    }
    public class JobStates
    {
        public string Name { get; set; }
        public int? Id { get; set; }
    }
    public class JobCustomers
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }

    public class JobSupplierForBuyer
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }

    public class JobCarrierForBuyer
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }

    public class SupplierCarrierInfoDDL
    {
       public List<JobSupplierForBuyer> supplierDetails = new List<JobSupplierForBuyer>();
       public List<JobCarrierForBuyer> carrierDetails = new List<JobCarrierForBuyer>();
    }

}
