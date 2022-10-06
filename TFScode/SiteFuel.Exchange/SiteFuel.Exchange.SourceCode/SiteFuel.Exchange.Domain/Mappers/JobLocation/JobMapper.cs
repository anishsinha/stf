using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.MobileAPI.TPD;
using SiteFuel.Exchange.ViewModels.Offer;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class JobMapper
    {
        public static JobViewModel ToViewModel(this Job entity, JobViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new JobViewModel(Status.Success);

            viewModel.Id = entity.Id;
            viewModel.Name = entity.Name;
            viewModel.JobID = entity.DisplayJobID;
            viewModel.Address = entity.Address;
            viewModel.City = entity.City;
            viewModel.State = entity.MstState.ToViewModel();
            viewModel.Country = entity.MstCountry.ToViewModel();
            viewModel.Country.Currency = entity.Currency;
            viewModel.Country.UoM = entity.UoM;
            if (entity.IsMarine)
                viewModel.MarineUom = entity.UoM;
            viewModel.ZipCode = entity.ZipCode;
            viewModel.CountyName = entity.CountyName;
            viewModel.Latitude = entity.Latitude;
            viewModel.Longitude = entity.Longitude;
            viewModel.TimeZoneName = entity.TimeZoneName;
            viewModel.StatusId = entity.JobXStatuses.FirstOrDefault(t => t.IsActive).StatusId;
            viewModel.IsBackdatedJob = entity.IsBackdatedJob;
            viewModel.IsGeocodeUsed = entity.IsGeocodeUsed;
            viewModel.CreatedBy = entity.CreatedBy;
            viewModel.CreatedDate = entity.CreatedDate;
            viewModel.StartDate = entity.StartDate;
            viewModel.EndDate = entity.EndDate;
            viewModel.IsReopened = entity.IsReopened;
            viewModel.ReopenDate = entity.ReopenDate;
            viewModel.IsApprovalWorkFlowEnabled = entity.IsApprovalWorkflowEnabled;
            viewModel.SignatureEnabled = entity.SignatureEnabled;
            viewModel.IsResaleEnabled = entity.IsResaleEnabled;
            viewModel.IsProFormaPoEnabled = entity.IsProFormaPoEnabled;
            viewModel.IsRetailJob = entity.IsRetailJob;
            viewModel.SignatureEnabled = entity.SignatureEnabled;
            viewModel.IsMarine = entity.IsMarine;
            if (viewModel.IsResaleEnabled)
            {
                viewModel.ContractNumber = entity.ContractNumber;
                viewModel.CustomerEmail = entity.ResaleCustomers.Select(t => t.Email).FirstOrDefault();
                viewModel.CustomerName = entity.ResaleCustomers.Select(t => t.Name).FirstOrDefault();
            }
            viewModel.IsOpenFuelRequestsExist = entity.FuelRequests.Any(t => t.FuelRequestXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)FuelRequestStatus.Open);
            viewModel.PoContactId = entity.PoContactId;
            viewModel.AssignedTo = entity.Users.Select(t => t.Id).ToList();
            viewModel.JobLicenses = entity.TaxExemptLicenses.Select(t => t.Id).ToList();
            viewModel.OnsiteContacts = entity.Users1.Select(t => t.Id).ToList();
            viewModel.OnsiteContactPersons = entity.Users1.Select(t => new ContactPersonViewModel()
            {
                Id = t.Id,
                Name = $"{t.FirstName} {t.LastName}",
                Email = t.Email,
                PhoneNumber = t.PhoneNumber
            }).ToList();
            if (entity.JobXApprovalUsers.Any(t => t.IsActive))
                viewModel.ApprovalUser = entity.JobXApprovalUsers.First(t => t.IsActive).UserId;
            viewModel.LocationType = entity.LocationType;
            viewModel.LocationManagedType = entity.LocationManagedType;

            if (entity.LocationType == JobLocationTypes.Various)
            {
                viewModel.IsVarious = true;
            }

            viewModel.IsActive = entity.IsActive;
            viewModel.UpdatedBy = entity.UpdatedBy;
            viewModel.UpdatedDate = entity.UpdatedDate;

            if (entity.EndDate == null || entity.EndDate.Value.Date >= DateTimeOffset.Now.ToTargetDateTimeOffset(entity.TimeZoneName).Date)
                viewModel.AllowBuyFuel = true;

            var freightServiceDomain = new FreightServiceDomain();
            var jobAdditionalDetails = Task.Run(() => freightServiceDomain.GetAdditionalJobDetails(entity.Id)).Result;
            if (jobAdditionalDetails != null)
            {
                jobAdditionalDetails.SetJobAdditionalDetails(viewModel);
            }

            if (entity.IsBillToEnabled && entity.BillingAddressId.HasValue)
            {
                viewModel.BillToInfo.Id = entity.BillingAddress.Id;
                viewModel.BillToInfo.Address = entity.BillingAddress.Address;
                viewModel.BillToInfo.AddressLine2 = entity.BillingAddress.AddressLine2;
                viewModel.BillToInfo.AddressLine3 = entity.BillingAddress.AddressLine3;
                viewModel.BillToInfo.City = entity.BillingAddress.City;
                viewModel.BillToInfo.County = entity.BillingAddress.County;
                viewModel.BillToInfo.Country.Id = entity.BillingAddress.CountryId ?? 0;
                viewModel.BillToInfo.Country.Name = entity.BillingAddress.CountryName;
                viewModel.BillToInfo.Name = entity.BillingAddress.Name;
                viewModel.BillToInfo.State.Id = entity.BillingAddress.StateId ?? 0;
                viewModel.BillToInfo.State.Name = entity.BillingAddress.StateName;
                viewModel.BillToInfo.ZipCode = entity.BillingAddress.ZipCode;
                viewModel.IsJobSpecificBillToEnabled = entity.IsBillToEnabled;
            }

            viewModel.SiteInstructions = entity.SiteInstructions;
            if (entity.LocationInventoryManagedBy != null)
            {
                viewModel.LocationInventoryManagedBy = new System.Collections.Generic.List<LocationInventoryManagedBy>();
                viewModel.LocationInventoryManagedBy.Add((LocationInventoryManagedBy)entity.LocationInventoryManagedBy);
            }
            viewModel.InventoryDataCaptureType = entity.InventoryDataCaptureType;
            return viewModel;
        }
        public static JobViewModel SetJobAdditionalDetails(this JobAdditionalDetailsViewModel entity, JobViewModel viewModel)
        {
            if (viewModel == null)
                viewModel = new JobViewModel();

            viewModel.IsAutoCreateDREnable = entity.IsAutoCreateDREnable;
            if (entity.DeliveryDaysList != null && entity.DeliveryDaysList.Count > 0)
            {
                foreach (var delivery in entity.DeliveryDaysList)
                {
                    DeliveryDaysViewModel Deliverydays = new DeliveryDaysViewModel();
                    Deliverydays.DeliveryDays = delivery.DeliveryDays;
                    Deliverydays.FromDeliveryTime = Convert.ToDateTime(delivery.FromDeliveryTime.ToString()).ToShortTimeString();
                    Deliverydays.ToDeliveryTime = Convert.ToDateTime(delivery.ToDeliveryTime.ToString()).ToShortTimeString();
                    Deliverydays.IsAcceptNightDeliveries = delivery.IsAcceptNightDeliveries;
                    viewModel.DeliveryDaysList.Add(Deliverydays);
                }
            }
            if (!string.IsNullOrEmpty(entity.SiteImageFilePath))
            {
                viewModel.SiteImage = new ImageViewModel()
                {
                    FilePath = entity.SiteImageFilePath
                };
            }
            if (!string.IsNullOrEmpty(entity.AdditionalImageFilePath))
            {
                viewModel.AdditionalImage = new AdditionalSiteImage()
                {
                    SiteImage = new ImageViewModel()
                    {
                        FilePath = entity.AdditionalImageFilePath,
                    },
                    Description = entity.AdditionalImageDescription
                };
            }
            viewModel.TrailerType = entity.TrailerType;
            return viewModel;
        }
        public static JobSelectionViewModel ToJobViewModel(this Job entity, JobSelectionViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new JobSelectionViewModel(Status.Success);

            viewModel.JobId = entity.Id;
            viewModel.Name = entity.Name;
            viewModel.Address = entity.Address;
            viewModel.City = entity.City;
            viewModel.State = entity.MstState.ToViewModel();
            viewModel.Country = entity.MstCountry.ToViewModel();
            viewModel.ZipCode = entity.ZipCode;
            viewModel.Country.Name = entity.CountyName;
            viewModel.TimeZoneName = entity.TimeZoneName;
            viewModel.StatusId = entity.JobXStatuses.FirstOrDefault(t => t.IsActive).StatusId;
            viewModel.JobStartDate = Convert.ToString(entity.StartDate);
            viewModel.JobEndDate = Convert.ToString(entity.EndDate);
            viewModel.Country.Currency = entity.Currency;
            viewModel.Country.UoM = entity.UoM;

            return viewModel;
        }

        public static JobGridViewModel ToGridViewModel(this Job entity, JobGridViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new JobGridViewModel(Status.Success);

            viewModel.Id = entity.Id;
            viewModel.Name = entity.Name;
            viewModel.JobID = entity.DisplayJobID ?? Resource.lblHyphen;
            viewModel.Address = entity.LocationType != JobLocationTypes.Various ? entity.Address + ", " + entity.City + ", " + entity.MstState.Code + " " + entity.ZipCode : entity.MstState.Code;
            viewModel.AssetAssigned = entity.JobXAssets.Count(t => t.Asset.IsActive && t.RemovedBy == null && t.RemovedDate == null);
            viewModel.Budget = entity.JobBudget == null ? 0 : entity.JobBudget.Budget.GetPreciseValue(6);
            viewModel.TotalSpend = (entity.HedgeDroppedAmount + entity.SpotDroppedAmount).GetPreciseValue(6);
            viewModel.Status = entity.JobXStatuses.FirstOrDefault(t => t.IsActive).MstJobStatus.Name;
            viewModel.ContactPerson = Resource.lblHyphen;
            var onsiteContacts = entity.Users1.Where(t => t.IsActive).ToList();
            if (onsiteContacts != null && onsiteContacts.Any())
            {
                foreach (var contact in onsiteContacts)
                {
                    viewModel.ContactPerson += $"{contact.FirstName} {contact.LastName}" + " - " + contact.PhoneNumber + " " + contact.Email + ";";
                }
            }

            viewModel.LastUpdated = entity.UpdatedDate.ToString(Resource.constFormatDate);
            viewModel.StartDate = entity.StartDate.ToString(Resource.constFormatDate);
            viewModel.EndDate = entity.EndDate.HasValue ? entity.EndDate.Value.ToString(Resource.constFormatDate) : Resource.lblHyphen;

            viewModel.IsActive = entity.IsActive;
            viewModel.UpdatedBy = entity.UpdatedBy;
            viewModel.UpdatedDate = entity.UpdatedDate;
            viewModel.IsRetailJob = entity.IsRetailJob;
            if(entity.LocationInventoryManagedBy != null)
            {
                viewModel.LocationInventoryManagedByNames = string.Join(",", entity.LocationInventoryManagedBy);
            }
            else
            {
                viewModel.LocationInventoryManagedByNames = "-";
            }
            viewModel.IsMarine = entity.IsMarine;
            return viewModel;
        }

        public static MapViewModel ToMapViewModel(this Job entity, MapViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new MapViewModel(Status.Success);

            viewModel.JobId = entity.Id;
            viewModel.Name = entity.Name;
            viewModel.Address = entity.Address;
            viewModel.City = entity.City;
            viewModel.State = entity.MstState.Code;
            viewModel.Country = entity.MstCountry.Code;
            viewModel.ZipCode = entity.ZipCode;
            viewModel.ContactPersons = entity.Users1.Select(t => new ContactPersonViewModel()
            {
                Id = t.Id,
                Name = $"{t.FirstName} {t.LastName}",
                Email = t.Email,
                PhoneNumber = t.PhoneNumber
            }).ToList();
            viewModel.Latitude = entity.Latitude;
            viewModel.Longitude = entity.Longitude;
            return viewModel;
        }

        public static Job ToEntity(this JobViewModel viewModel, Job entity = null)
        {
            if (entity == null)
                entity = new Job();

            entity.Id = viewModel.Id;
            entity.Name = viewModel.Name;
            if(String.IsNullOrWhiteSpace(viewModel.JobID))
            {
                viewModel.JobID = viewModel.Name;
            }
            entity.DisplayJobID = viewModel.JobID;
            entity.Address = viewModel.Address;
            entity.City = viewModel.City;
            entity.StateId = viewModel.State.Id;
            entity.CountryId = viewModel.Country.Id;
            entity.ZipCode = viewModel.ZipCode;
            entity.CountyName = viewModel.CountyName;
            entity.IsGeocodeUsed = viewModel.IsGeocodeUsed;
            entity.Latitude = viewModel.Latitude;
            entity.Longitude = viewModel.Longitude;
            entity.TimeZoneName = viewModel.TimeZoneName;
            entity.Currency = viewModel.Country.Currency;
            entity.UoM = viewModel.Country.UoM;
            entity.IsProFormaPoEnabled = viewModel.IsProFormaPoEnabled;
            entity.IsRetailJob = viewModel.IsRetailJob;
            entity.LocationType = viewModel.LocationType;
            if (entity.JobXStatuses.Count == 0 || (entity.JobXStatuses.Count > 0 && entity.JobXStatuses.FirstOrDefault(t => t.IsActive).StatusId == (int)JobStatus.Draft))
            {
                entity.IsBackdatedJob = viewModel.StartDate.Date < DateTime.Now.Date;
            }
            if (entity.JobXStatuses.Count == 0 || (entity.JobXStatuses.Count > 0 && entity.JobXStatuses.FirstOrDefault(t => t.IsActive).StatusId != viewModel.StatusId))
            {
                if (entity.JobXStatuses.Count > 0)
                    entity.JobXStatuses.FirstOrDefault(t => t.IsActive).IsActive = false;

                JobXStatus jobStatus = new JobXStatus();
                jobStatus.StatusId = viewModel.StatusId;
                jobStatus.IsActive = true;
                jobStatus.UpdatedBy = viewModel.UpdatedBy;
                jobStatus.UpdatedDate = DateTimeOffset.Now;
                entity.JobXStatuses.Add(jobStatus);
            }
            entity.CreatedBy = viewModel.CreatedBy;
            entity.CreatedDate = viewModel.CreatedDate;
            entity.StartDate = viewModel.StartDate;
            entity.EndDate = viewModel.EndDate;
            entity.IsReopened = viewModel.IsReopened;
            entity.ReopenDate = viewModel.ReopenDate;
            entity.PoContactId = viewModel.PoContactId;
            entity.IsApprovalWorkflowEnabled = viewModel.IsApprovalWorkFlowEnabled;
            entity.IsActive = viewModel.IsActive;
            entity.UpdatedBy = viewModel.UpdatedBy;
            entity.UpdatedDate = viewModel.UpdatedDate;
            entity.IsResaleEnabled = viewModel.IsResaleEnabled;
            entity.ContractNumber = entity.IsResaleEnabled ? viewModel.ContractNumber : string.Empty;

            if (viewModel.BillToInfo.BillingAddressId.HasValue)
            {
                entity.BillingAddressId = viewModel.BillToInfo.BillingAddressId.Value;
                entity.IsBillToEnabled = true;
            }

            entity.SiteInstructions = viewModel.SiteInstructions;
            if (viewModel.LocationInventoryManagedBy != null && viewModel.LocationInventoryManagedBy.Any())
                entity.LocationInventoryManagedBy = viewModel.LocationInventoryManagedBy[0];
            else
                entity.LocationInventoryManagedBy = null;
            entity.IsMarine = viewModel.IsMarine;
            entity.InventoryDataCaptureType = viewModel.InventoryDataCaptureType;
            if (!string.IsNullOrWhiteSpace(viewModel.ExternalRefId))
                entity.ExternalRefID = viewModel.ExternalRefId;
            return entity;
        }

        public static ApiJobDetailViewModel ToApiJobViewModel(this JobAdditionalDetailsViewModel viewModel)
        {
            var apiJobViewModel = new ApiJobDetailViewModel();
            apiJobViewModel.SiteImageFilePath = viewModel.SiteImageFilePath;
            apiJobViewModel.AdditionalImageFilePath = viewModel.AdditionalImageFilePath;
            apiJobViewModel.JobId = viewModel.JobId;
            apiJobViewModel.DisplayJobId = viewModel.SiteId;
            apiJobViewModel.AdditionalImageDescription = viewModel.AdditionalImageDescription;
            return apiJobViewModel;
        }

        public static Job ToEntityForSuperAdmin(this JobViewModelForSuperAdmin viewModel, Job entity = null)
        {
            if (entity == null)
                entity = new Job();

            entity.Id = viewModel.Id;
            entity.Name = viewModel.Name;
            entity.DisplayJobID = string.IsNullOrEmpty(viewModel.JobID) ? viewModel.Name : viewModel.JobID;
            entity.StateId = viewModel.State.Id;
            entity.CountryId = viewModel.Country.Id;
            entity.IsGeocodeUsed = viewModel.IsGeocodeUsed;
            entity.Latitude = viewModel.Latitude;
            entity.Longitude = viewModel.Longitude;
            entity.TimeZoneName = viewModel.TimeZoneName;
            entity.Currency = viewModel.Country.Currency;
            entity.UoM = viewModel.Country.UoM;
            entity.IsProFormaPoEnabled = viewModel.IsProFormaPoEnabled;
            entity.LocationType = viewModel.LocationType;
            entity.InventoryDataCaptureType = viewModel.InventoryDataCaptureType;
            if (viewModel.LocationType == JobLocationTypes.Various)
            {
                entity.Address = Resource.lblVarious;
                entity.City = Resource.lblVarious;
                entity.ZipCode = Resource.lblVarious;
                entity.CountyName = Resource.lblVarious;
            }
            else
            {
                entity.Address = viewModel.Address;
                entity.City = viewModel.City;
                entity.ZipCode = viewModel.ZipCode;
                entity.CountyName = viewModel.CountyName;
            }
            if (entity.JobXStatuses.Count == 0 || (entity.JobXStatuses.Count > 0 && entity.JobXStatuses.FirstOrDefault(t => t.IsActive).StatusId == (int)JobStatus.Draft))
            {
                entity.IsBackdatedJob = viewModel.StartDate.Date < DateTime.Now.Date;
            }
            if (entity.JobXStatuses.Count == 0 || (entity.JobXStatuses.Count > 0 && entity.JobXStatuses.FirstOrDefault(t => t.IsActive).StatusId != viewModel.StatusId))
            {
                if (entity.JobXStatuses.Count > 0)
                    entity.JobXStatuses.FirstOrDefault(t => t.IsActive).IsActive = false;

                JobXStatus jobStatus = new JobXStatus();
                jobStatus.StatusId = viewModel.StatusId;
                jobStatus.IsActive = true;
                jobStatus.UpdatedBy = viewModel.UpdatedBy;
                jobStatus.UpdatedDate = DateTimeOffset.Now;
                entity.JobXStatuses.Add(jobStatus);
            }
            entity.CreatedBy = viewModel.CreatedBy;
            entity.CreatedDate = viewModel.CreatedDate;
            entity.StartDate = viewModel.StartDate;
            entity.EndDate = viewModel.EndDate;
            entity.IsReopened = viewModel.IsReopened;
            entity.ReopenDate = viewModel.ReopenDate;
            entity.IsActive = viewModel.IsActive;
            entity.UpdatedBy = viewModel.UpdatedBy;
            entity.UpdatedDate = viewModel.UpdatedDate;
            entity.ContractNumber = entity.IsResaleEnabled ? viewModel.ContractNumber : string.Empty;
            entity.SiteInstructions = viewModel.SiteInstructions;
            if (viewModel.LocationInventoryManagedBy != null && viewModel.LocationInventoryManagedBy.Any())
            {
                entity.LocationInventoryManagedBy = viewModel.LocationInventoryManagedBy[0];
            }
            else
            {
                entity.LocationInventoryManagedBy = null;
            }
            return entity;
        }

        public static JobViewModelForSuperAdmin ToViewModelForSuperAdmin(this Job entity, int supplierCompanyId = 0, JobViewModelForSuperAdmin viewModel = null)
        {
            if (viewModel == null)
                viewModel = new JobViewModelForSuperAdmin(Status.Success);

            viewModel.Id = entity.Id;
            viewModel.Name = entity.Name;
            viewModel.JobID = entity.DisplayJobID;
            viewModel.Address = entity.Address;
            viewModel.City = entity.City;
            viewModel.State = entity.MstState.ToViewModel();
            viewModel.Country = entity.MstCountry.ToViewModel();
            viewModel.Country.Currency = entity.Currency;
            viewModel.Country.UoM = entity.UoM;
            viewModel.ZipCode = entity.ZipCode;
            viewModel.CountyName = entity.CountyName;
            viewModel.Latitude = entity.Latitude;
            viewModel.Longitude = entity.Longitude;
            viewModel.TimeZoneName = entity.TimeZoneName;
            viewModel.StatusId = entity.JobXStatuses.FirstOrDefault(t => t.IsActive).StatusId;
            viewModel.IsBackdatedJob = entity.IsBackdatedJob;
            viewModel.IsGeocodeUsed = entity.IsGeocodeUsed;
            viewModel.CreatedBy = entity.CreatedBy;
            viewModel.CreatedDate = entity.CreatedDate;
            viewModel.StartDate = entity.StartDate;
            viewModel.EndDate = entity.EndDate;
            viewModel.IsReopened = entity.IsReopened;
            viewModel.ReopenDate = entity.ReopenDate;
            viewModel.SignatureEnabled = entity.SignatureEnabled;
            viewModel.IsProFormaPoEnabled = entity.IsProFormaPoEnabled;
            viewModel.SignatureEnabled = entity.SignatureEnabled;
            viewModel.IsOpenFuelRequestsExist = entity.FuelRequests.Any(t => t.FuelRequestXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)FuelRequestStatus.Open);
            viewModel.LocationType = entity.LocationType;
            viewModel.IsActive = entity.IsActive;
            viewModel.UpdatedBy = entity.UpdatedBy;
            viewModel.UpdatedDate = entity.UpdatedDate;
            viewModel.SiteInstructions = entity.SiteInstructions;
            viewModel.IsRetailJob = entity.IsRetailJob;
            viewModel.InventoryDataCaptureType = entity.InventoryDataCaptureType;
            var freightServiceDomain = new FreightServiceDomain();
            var jobAdditionalDetails = Task.Run(() => freightServiceDomain.GetAdditionalJobDetails(entity.Id, supplierCompanyId)).Result;
            if (supplierCompanyId > 0)
            {
                viewModel.RegionId = jobAdditionalDetails.RegionId;
            }
            if (jobAdditionalDetails != null)
            {
                viewModel.IsAutoCreateDREnable = jobAdditionalDetails.IsAutoCreateDREnable;
                if (jobAdditionalDetails.DeliveryDaysList != null && jobAdditionalDetails.DeliveryDaysList.Count > 0)
                {
                    foreach (var delivery in jobAdditionalDetails.DeliveryDaysList)
                    {
                        DeliveryDaysViewModel Deliverydays = new DeliveryDaysViewModel();
                        Deliverydays.DeliveryDays = delivery.DeliveryDays;
                        Deliverydays.FromDeliveryTime = Convert.ToDateTime(delivery.FromDeliveryTime.ToString()).ToShortTimeString();
                        Deliverydays.ToDeliveryTime = Convert.ToDateTime(delivery.ToDeliveryTime.ToString()).ToShortTimeString();
                        Deliverydays.IsAcceptNightDeliveries = delivery.IsAcceptNightDeliveries;
                        viewModel.DeliveryDaysList.Add(Deliverydays);
                    }
                }

                // set the site image details to view model.
                viewModel.ImageDetails.SiteImage = new ImageViewModel()
                {
                    FilePath = jobAdditionalDetails.SiteImageFilePath
                };

                viewModel.ImageDetails.AdditionalImage = new AdditionalSiteImage()
                {
                    SiteImage = new ImageViewModel()
                    {
                        FilePath = jobAdditionalDetails.AdditionalImageFilePath,
                    },
                    Description = jobAdditionalDetails.AdditionalImageDescription
                };
            }
            if (jobAdditionalDetails != null)
            {
                viewModel.TrailerType = jobAdditionalDetails.TrailerType;
            }
            if (entity.LocationInventoryManagedBy != null)
            {
                viewModel.LocationInventoryManagedBy = new System.Collections.Generic.List<LocationInventoryManagedBy>();
                viewModel.LocationInventoryManagedBy.Add((LocationInventoryManagedBy)entity.LocationInventoryManagedBy);
            }
            return viewModel;
        }

        public static Job ToEntityFromTPO(this ThirdPartyOrderViewModel viewModel, Job entity = null)
        {
            if (entity == null)
                entity = new Job();

            entity.Id = 0;
            entity.Name = viewModel.AddressDetails.JobName;
            entity.Address = viewModel.AddressDetails.Address;
            entity.City = viewModel.AddressDetails.City;
            entity.StateId = viewModel.AddressDetails.State.Id;
            entity.CountryId = viewModel.AddressDetails.Country.Id;
            entity.ZipCode = viewModel.AddressDetails.ZipCode;
            entity.CountyName = viewModel.AddressDetails.CountyName;
            entity.IsGeocodeUsed = false;
            entity.Latitude = viewModel.AddressDetails.Latitude;
            entity.Longitude = viewModel.AddressDetails.Longitude;
            entity.TimeZoneName = viewModel.AddressDetails.TimeZoneName.ParseTimeZone();
            entity.IsBackdatedJob = true;
            entity.Currency = viewModel.AddressDetails.Country.Currency;
            entity.UoM = viewModel.AddressDetails.IsMarineLocation ? viewModel.AddressDetails.MarineUoM : viewModel.AddressDetails.Country.UoM;
            entity.LocationType = viewModel.FuelDetails.IsMarineLocation ? JobLocationTypes.Port : viewModel.AddressDetails.JobLocationType;
            if (string.IsNullOrEmpty(viewModel.AddressDetails.DisplayJobID))
            {
                viewModel.AddressDetails.DisplayJobID = viewModel.AddressDetails.JobName;
            }
            entity.DisplayJobID = viewModel.AddressDetails.DisplayJobID;
            entity.IsProFormaPoEnabled = viewModel.AddressDetails.IsProFormaPoEnabled;
            entity.IsRetailJob = viewModel.AddressDetails.IsRetailJob;
            entity.SignatureEnabled = viewModel.AddressDetails.SignatureEnabled;
            entity.InventoryDataCaptureType = viewModel.AddressDetails.InventoryDataCaptureType;
            JobXStatus jobStatus = new JobXStatus();
            jobStatus.StatusId = (int)JobStatus.Open;
            jobStatus.IsActive = true;
            jobStatus.UpdatedBy = viewModel.UpdatedBy;
            jobStatus.UpdatedDate = DateTimeOffset.Now;
            entity.JobXStatuses.Add(jobStatus);

            entity.CreatedBy = (int)viewModel.CustomerDetails.UserId;
            entity.CreatedDate = DateTimeOffset.Now;
            entity.StartDate = viewModel.FuelDeliveryDetails.StartDate;
            entity.IsReopened = false;
            entity.ReopenDate = entity.CreatedDate;
            entity.IsApprovalWorkflowEnabled = false;
            entity.IsActive = true;
            entity.UpdatedBy = viewModel.UpdatedBy;
            entity.UpdatedDate = viewModel.UpdatedDate;
            entity.IsResaleEnabled = false;
            entity.LocationManagedType = viewModel.AddressDetails.LocationManagedType;
            entity.IsMarine = viewModel.AddressDetails.IsMarineLocation;

            if (viewModel.BillToInfo.BillingAddressId.HasValue) //For New Billing Address
            {
                entity.IsBillToEnabled = true;
                entity.BillingAddressId = viewModel.BillToInfo.BillingAddressId;
            }
            if (viewModel.LocationInventoryManagedBy != null && viewModel.LocationInventoryManagedBy.Any())
            {
                entity.LocationInventoryManagedBy = viewModel.LocationInventoryManagedBy[0];
            }           
            else
            {
                entity.LocationInventoryManagedBy = null;
            }              
            return entity;
        }

        public static Job ToEntityFromOffer(this OfferOrderViewModel viewModel, Job entity = null)
        {
            if (entity == null)
                entity = new Job();

            entity.Id = 0;
            entity.Name = viewModel.AddressDetails.JobName;
            entity.Address = viewModel.AddressDetails.Address;
            entity.City = viewModel.AddressDetails.City;
            entity.StateId = viewModel.AddressDetails.State.Id;
            entity.CountryId = viewModel.AddressDetails.Country.Id;
            entity.ZipCode = viewModel.AddressDetails.ZipCode;
            entity.CountyName = viewModel.AddressDetails.CountyName;
            entity.IsGeocodeUsed = viewModel.AddressDetails.IsGeocodeUsed;
            entity.Latitude = viewModel.AddressDetails.Latitude;
            entity.Longitude = viewModel.AddressDetails.Longitude;
            entity.TimeZoneName = viewModel.AddressDetails.TimeZoneName;
            entity.IsBackdatedJob = true;
            entity.Currency = entity.CountryId == (int)Country.USA ? Currency.USD : Currency.CAD;
            entity.UoM = entity.CountryId == (int)Country.USA ? UoM.Gallons : UoM.Litres;
            entity.DisplayJobID = viewModel.AddressDetails.DisplayJobID;
            entity.IsProFormaPoEnabled = viewModel.AddressDetails.IsProFormaPoEnabled;
            entity.SignatureEnabled = viewModel.AddressDetails.SignatureEnabled;
            JobXStatus jobStatus = new JobXStatus();
            jobStatus.StatusId = (int)JobStatus.Open;
            jobStatus.IsActive = true;
            jobStatus.UpdatedBy = viewModel.UpdatedBy;
            jobStatus.UpdatedDate = DateTimeOffset.Now;
            entity.JobXStatuses.Add(jobStatus);
            entity.LocationType = JobLocationTypes.Location;
            entity.CreatedBy = viewModel.UpdatedBy;
            entity.CreatedDate = DateTimeOffset.Now;
            entity.StartDate = DateTimeOffset.Now;
            entity.IsReopened = false;
            entity.ReopenDate = entity.CreatedDate;
            entity.IsApprovalWorkflowEnabled = false;
            entity.IsActive = true;
            entity.UpdatedBy = viewModel.UpdatedBy;
            entity.UpdatedDate = DateTimeOffset.Now;
            entity.IsResaleEnabled = false;
            return entity;
        }

        public static AddressViewModel ToAddressViewModel(this Job entity, AddressViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new AddressViewModel(Status.Success);

            viewModel.Address = entity.Address;
            viewModel.City = entity.City;
            viewModel.StateCode = entity.MstState.Code;
            viewModel.CountryCode = entity.MstCountry.Code;
            viewModel.ZipCode = entity.ZipCode;
            viewModel.CountyName = entity.CountyName;
            return viewModel;
        }

        public static DispatchLocationViewModel ToDispatchViewModel(this Job entity)
        {
            if (entity != null)
            {
                var location = new DispatchLocationViewModel
                {
                    Address = entity.Address,
                    AddressLine2 = entity.AddressLine2,
                    AddressLine3 = entity.AddressLine3,
                    City = entity.City,
                    CountryCode = entity.MstCountry.Code,
                    CountyName = entity.CountyName,
                    Latitude = entity.Latitude,
                    LocationType = (int)LocationType.Drop,
                    Longitude = entity.Longitude,
                    SiteName = entity.Name,
                    StateCode = entity.MstState.Code,
                    StateId = entity.StateId,
                    ZipCode = entity.ZipCode,
                };
                return location;
            }
            return null;
        }

        public static JobGridViewModel ToGridViewModel(this UspJobViewModel entity, JobGridViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new JobGridViewModel(Status.Success);

            viewModel.Id = entity.Id;
            viewModel.Name = entity.Name;
            viewModel.JobID = entity.DisplayJobId ?? Resource.lblHyphen;
            viewModel.Address = entity.Address;
            viewModel.AssetAssigned = entity.AssetAssigned;
            viewModel.Budget = entity.Budget.GetPreciseValue(6);
            viewModel.TotalSpend = entity.TotalSpend.GetPreciseValue(6);
            viewModel.Status = entity.Status;
            viewModel.ContactPerson = entity.ContactPerson ?? Resource.lblHyphen;

            viewModel.LastUpdated = entity.UpdatedDate.ToString(Resource.constFormatDate);
            viewModel.StartDate = entity.StartDate.ToString(Resource.constFormatDate);
            viewModel.EndDate = entity.EndDate.HasValue ? entity.EndDate.Value.ToString(Resource.constFormatDate) : Resource.lblHyphen;

            viewModel.IsActive = entity.IsActive;
            viewModel.UpdatedBy = entity.UpdatedBy;
            viewModel.UpdatedDate = entity.UpdatedDate;

            viewModel.IsCompanyOnboardingComplete = entity.IsOnboardingComplete;
            viewModel.UserOnboardedTypeId = entity.OnboardedTypeId;

            viewModel.IsRetailJob = entity.IsRetailJob;
            viewModel.AccountingCompanyId = entity.AccountingCompanyId;
            viewModel.BuyerCompanyId = entity.BuyerCompanyId;
            return viewModel;
        }
        public static JobGridSupplierViewModel ToJobGridSupplierViewModel(this UspJobViewModel entity, JobGridSupplierViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new JobGridSupplierViewModel(Status.Success);
            viewModel.CustomerId = entity.BuyerCompanyId;
            viewModel.CustomerName = entity.BuyerCompanyName;
            viewModel.JobID = entity.DisplayJobId ?? Resource.lblHyphen;
            viewModel.Id = entity.Id;
            viewModel.JobName = entity.Name;
            viewModel.Address = entity.Address;
            viewModel.ContactPerson = entity.ContactPerson ?? Resource.lblHyphen;
            viewModel.LastUpdated = entity.UpdatedDate.ToString(Resource.constFormatDate);
            viewModel.StartDate = entity.StartDate.ToString(Resource.constFormatDate);
            viewModel.EndDate = entity.EndDate.HasValue ? entity.EndDate.Value.ToString(Resource.constFormatDate) : Resource.lblHyphen;
            viewModel.UpdatedBy = entity.UpdatedBy;
            viewModel.UpdatedDate = entity.UpdatedDate;
            viewModel.UserOnboardedTypeId = entity.OnboardedTypeId;
            viewModel.CreatedBy = entity.CreatedBy;
            viewModel.IsRetailJob = entity.IsRetailJob;
            viewModel.AssetAssigned = entity.AssetAssigned;
            viewModel.AccountingCompanyId = entity.AccountingCompanyId;
            viewModel.IsBadgeMandatory = entity.IsBadgeMandatory;
             if(entity.LocationInventoryManagedBy != null)
            {
                viewModel.LocationInventoryManagedByNames = string.Join(",", entity.LocationInventoryManagedBy);
            }
            else
            {
                viewModel.LocationInventoryManagedByNames = "-";
            }
            viewModel.CompanyOwnedLocation = entity.CompanyOwnedLocation;
            viewModel.InventoryDataCaptureType = entity.InventoryDataCaptureType;
            return viewModel;
        }

        public static JobStepsViewModel ToGetJobStepViewModel(this Job port, JobStepsViewModel response=null)
        {
            if (response == null)
                response = new JobStepsViewModel();
                    response.Job.Name = port.Name;
                    //response.Job.Id = port.Id;
                    response.Job.StartDate = DateTimeOffset.Now;
                    response.Job.EndDate = null;
                    response.Job.Address = port.Address;
                    response.Job.ZipCode = port.ZipCode;
                    response.Job.City = port.City;

                    response.Job.State.Id = port.StateId;
                    response.Job.State.Name = port.MstState.Name;
                    response.Job.Country.Id = port.CountryId;
                    response.Job.Country.Name = port.MstCountry.Name;
                    response.Job.CountyName = port.CountyName;
                    // set default currency  as USD and uom as Gallons for CAR country 
                    if ((response.Job.Country.Id == (int)Country.USA) || (response.Job.Country.Id == (int)Country.CAR))
                    {
                        response.Job.Country.Currency = (Currency)Enum.Parse(typeof(Currency), "USD");
                        response.Job.Country.UoM = (UoM)Enum.Parse(typeof(UoM), "Gallons");
                    }
                    else
                    {
                        response.Job.Country.Currency = (Currency)Enum.Parse(typeof(Currency), "CAD");
                        response.Job.Country.UoM = (UoM)Enum.Parse(typeof(UoM), "Litres");
                    }

                    response.Job.IsGeocodeUsed = port.IsGeocodeUsed;
                    response.Job.Latitude = port.Latitude ;
                    response.Job.Longitude = port.Longitude ;



                    response.Job.InventoryDataCaptureType = (InventoryDataCaptureType)Enum.Parse(typeof(InventoryDataCaptureType), "NotSpecified");
                    response.Job.IsProFormaPoEnabled = false;
                    response.Job.IsRetailJob = false;
                    response.Job.IsAutoCreateDREnable = false;
                    response.JobBudget.IsTaxExempted = false;
                    response.Job.IsAssetTracked = false;
                    response.Job.StatusId = (int)JobStatus.Open;
                    response.Job.CreatedDate = DateTimeOffset.Now;
                    response.Job.ReopenDate = DateTimeOffset.Now;
                    response.Job.IsBackdatedJob = true;
                    response.Job.IsVarious = false;
                    response.Job.LocationType = JobLocationTypes.Port;
                    response.Job.IsMarine = true;
                    response.Job.LocationManagedType = LocationManagedType.NotSpecified;
                    response.UserId = port.Id;
                    response.CompanyId = port.CreatedByCompanyId; // for the time being
                    response.Job.TrailerType = Enum.GetValues(typeof(TrailerTypeStatus)).Cast<TrailerTypeStatus>().ToList();
            return response;
        }

        public static JobStepsViewModel ToJobStepViewModel(this TPDLocationViewModel job,int createdByCompanyId)
        {
            if (job != null)
            {
                HelperDomain helperDomain = new HelperDomain();
                var viewModel = new JobStepsViewModel();
                viewModel.Job.Id = job.JobId;
                viewModel.Job.CreatedBy = job.BuyerCompanyUserId;
                viewModel.Job.StatusId = (int)JobStatus.Open;
                viewModel.UserId = job.BuyerCompanyUserId;
                viewModel.Job.Name = job.LocationName;
                viewModel.Job.JobID = job.ThirdPartyLocationID;
                viewModel.Job.Address = job.LocationAddressLine1;
                viewModel.Job.City = job.LocationAddressCity;
                viewModel.Job.State.Id = job.StateId;
                viewModel.Job.ZipCode = job.LocationAddressZip;
                viewModel.Job.Latitude = job.LocationAddressLat;
                viewModel.Job.Longitude = job.LocationAddressLong;
                viewModel.Job.StartDate = job.StartDate;
                viewModel.Job.EndDate = job.EndDate;
                var countryId = helperDomain.GetCountryFromState(job.StateId);
                viewModel.Job.Country.Id = countryId;
                viewModel.Job.CountyName = job.LocationAddressCounty;
                viewModel.Job.TimeZoneName = job.TimeZoneName;
                if (countryId == (int)Country.CAN)
                {
                    viewModel.Job.Country.UoM = UoM.Litres;
                    viewModel.Job.Country.Currency = Currency.CAD;
                }
                else
                {
                    viewModel.Job.Country.UoM = UoM.Gallons;
                    viewModel.Job.Country.Currency = Currency.USD;
                }
                viewModel.Job.LocationType = job.LocationAddressLat == 0 && job.LocationAddressLong == 0 ? JobLocationTypes.Location : JobLocationTypes.GeoLocation;

                viewModel.Job.SiteInstructions = job.SiteInstruction;
                viewModel.Job.ExternalRefId = job.LocationXRefID;

                viewModel.IsJobCreationFromAPI = true;
                viewModel.SupplierCompanyId = createdByCompanyId;
                viewModel.Job.TrailerType = Enum.GetValues(typeof(TrailerTypeStatus)).Cast<TrailerTypeStatus>().ToList();

                //jobs specific billing address
                if (!string.IsNullOrWhiteSpace(job.LocationBillToAddressLine1))
                {
                    viewModel.Job.IsJobSpecificBillToEnabled = true;
                    viewModel.Job.BillToInfo.Name = job.LocationBillToAddressName;
                    viewModel.Job.BillToInfo.Address = job.LocationBillToAddressLine1;
                    viewModel.Job.BillToInfo.City = job.LocationBillToAddressCity;
                    viewModel.Job.BillToInfo.State.Name = job.LocationBillToAddressState;
                    viewModel.Job.BillToInfo.County = job.LocationBillToAddressCounty;
                    viewModel.Job.BillToInfo.Country.Name = job.LocationBillToAddressCountry;
                    viewModel.Job.BillToInfo.ZipCode = job.LocationBillToAddressZip;
                    viewModel.Job.BillToInfo.AddressLine2 = job.LocationBillToAddressLine2;
                    viewModel.Job.BillToInfo.AddressLine3 = job.LocationBillToAddressLine3;
                    viewModel.Job.BillToInfo.Id = job.BillingAddressId;
                    if (job.BillingAddressId > 0)
                        viewModel.Job.BillToInfo.IsExistingBillAddress = true;
                }

                return viewModel;
            }
            return null;
        }
    }
}
