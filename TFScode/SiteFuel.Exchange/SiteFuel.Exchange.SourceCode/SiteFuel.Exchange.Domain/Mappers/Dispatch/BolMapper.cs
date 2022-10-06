using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Invoice.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class BolMapper
    {
        public static BolDetailViewModel ToViewModel(this InvoiceFtlDetail entity)
        {
            if (entity != null)
            {
                BolDetailViewModel bolDetailViewModel = new BolDetailViewModel
                {
                    BolNumber = entity.BolNumber,
                    BadgeNumber = entity.BadgeNumber,
                    Carrier = entity.Carrier,
                    CreatedBy = entity.CreatedBy,
                    CreatedDate = entity.CreatedDate,
                    GrossQuantity = entity.GrossQuantity,
                    Id = entity.Id,
                    NetQuantity = entity.NetQuantity,
                    DeliveredQuantity = entity.DeliveredQuantity,
                    LiftDate = entity.LiftDate,
                    StartTime= Convert.ToString(entity.LiftStartTime), //edit invoice page
                    EndTime=Convert.ToString(entity.LiftEndTime),
                    LiftArrivalTime = entity.LiftArrivalTime,
                    LiftStartTime = entity.LiftStartTime,
                    LiftEndTime = entity.LiftEndTime,
                    LiftTicketNumber = entity.LiftTicketNumber,
                    BolCreationTime = entity.BolCreationTime,
                    LiftQuantity = entity.LiftQuantity,
                    TerminalId = entity.TerminalId, //WHY WE NEED TO ASSIGN THIS TO 0
                    PricePerGallon = entity.PricePerGallon,
                    CityGroupTerminalId = entity.CityGroupTerminalId,
                    Address = entity.Address,
                    AddressLine2 = entity.AddressLine2,
                    AddressLine3 = entity.AddressLine3,
                    City = entity.City,
                    CountryCode = entity.CountryCode,
                    CountryId = entity.StateId.HasValue ? entity.MstState.CountryId : 1,
                    CountyName = entity.CountyName,
                    FuelTypeId = entity.FuelTypeId,
                    Image = entity.Image != null ? entity.Image.ToViewModel() : null,
                    Latitude = entity.Latitude,
                    Longitude = entity.Longitude,
                    PickupLocationType = entity.PickupLocation,
                    RackPrice = entity.RackPrice,
                    SiteName = entity.SiteName,
                    StateCode = entity.StateCode,
                    StateId = entity.StateId ?? 0,
                    TerminalName = entity.TerminalName,
                    ZipCode = entity.ZipCode,
                    IsActive = entity.IsActive,
                    IsDeleted = entity.IsDeleted,
                    RecordHistory = entity.RecordHistory
                };

                if (entity.InvoiceTierPricingDetails != null && entity.InvoiceTierPricingDetails.Any())
                {
                    foreach (var tier in entity.InvoiceTierPricingDetails)
                    {
                        bolDetailViewModel.TierPricingForBol.Add(new TierPricingForBol() { PricePerGallon = tier.PricePerGallon, Quantity = tier.Quantity, TierMaxQuantity = tier.TierMaxQuantity, TierMinQuantity = tier.TierMinQuantity });
                    }
                }
                return bolDetailViewModel;
            }
            return null;
        }

        public static InvoiceBolViewModel ToBolViewModel(this List<InvoiceFtlDetail> entity)
        {
            if (entity.Any())
            {
                var bol = entity.FirstOrDefault();
                InvoiceBolViewModel bolDetailViewModel = new InvoiceBolViewModel()
                {
                    BolNumber = bol.BolNumber,
                    LiftDate = bol.LiftDate,
                    BadgeNumber = bol.BadgeNumber,                       
                    LiftStartTime = bol.LiftStartTime.ToShortTimeSafe(),
                    LiftEndTime= bol.LiftEndTime.ToShortTimeSafe()
                };
                bolDetailViewModel.Products = new List<BolProductViewModel>();
                foreach (var item in entity)
                {
                    if (!bolDetailViewModel.Products.Any(t => t.ProductId == item.FuelTypeId && t.NetQuantity == item.NetQuantity && t.GrossQuantity == item.GrossQuantity && t.DeliveredQuantity == item.DeliveredQuantity && t.TerminalName == item.TerminalName))
                    {
                        bolDetailViewModel.Products.Add(new BolProductViewModel()
                        {
                            ProductId = item.FuelTypeId,
                            ProductName = item.MstProduct.DisplayName ?? item.MstProduct.Name,
                            NetQuantity = item.NetQuantity,
                            DeliveredQuantity = item.DeliveredQuantity,
                            GrossQuantity = item.GrossQuantity,
                            TerminalId = item.TerminalId,
                            TerminalName = item.TerminalName
                        });
                    }
                }

                entity.Where(t => t.ImageId != null).ToList().ForEach(t => bolDetailViewModel.Images = t.Image.ToViewModel());
                if (bolDetailViewModel.Images != null)
                {
                    bolDetailViewModel.Images.BlobContainerType = BlobContainerType.InvoicePdfFiles;
                }

                return bolDetailViewModel;
            }
            return new InvoiceBolViewModel();
        }

        public static InvoiceLiftTicketViewModel ToLiftViewModel(this List<InvoiceFtlDetail> entity)
        {
            if (entity.Any())
            {
                var bol = entity.FirstOrDefault();
                InvoiceLiftTicketViewModel liftDetailViewModel = new InvoiceLiftTicketViewModel()
                {
                    LiftTicketNumber = bol.LiftTicketNumber,
                    BadgeNumber = bol.BadgeNumber,
                    LiftDate = bol.LiftDate,
                    LiftArrivalTime = bol.LiftArrivalTime,
                    BolCreationTime = bol.BolCreationTime,
                    LiftStartTime = bol.LiftStartTime.ToShortTimeSafe(),
                    LiftEndTime = bol.LiftEndTime.ToShortTimeSafe()
                };
                liftDetailViewModel.Products = new List<LiftProductViewModel>();
                entity.ForEach(t => liftDetailViewModel.Products.Add(new LiftProductViewModel()
                {
                    ProductId = t.FuelTypeId,
                    ProductName = t.MstProduct.DisplayName ?? t.MstProduct.Name,
                    LiftQuantity = t.LiftQuantity,
                    GrossQuantity = t.GrossQuantity,
                    NetQuantity = t.NetQuantity,
                    DeliveredQuantity = t.DeliveredQuantity,
                    BulkPlantName = t.SiteName,
                    LiftStartTime = t.LiftStartTime,
                    LiftEndTime = t.LiftEndTime,
                    Address = new DropAddressViewModel()
                    {
                        Address = t.Address,
                        AddressLine2 = t.AddressLine2,
                        AddressLine3 = t.AddressLine3,
                        City = t.City,
                        State = new StateViewModel() { Id = t.StateId.HasValue? t.StateId.Value:0, Code = t.StateCode },
                        Country = new CountryViewModel() { Code = t.CountryCode },
                        ZipCode = t.ZipCode,
                        CountyName = t.CountyName,
                        Latitude = t.Latitude,
                        Longitude = t.Longitude
                    }
                }));
                entity.Where(t => t.ImageId != null).ToList().ForEach(t => liftDetailViewModel.Images = t.Image.ToViewModel());
                if (liftDetailViewModel.Images != null)
                {
                    liftDetailViewModel.Images.BlobContainerType = BlobContainerType.InvoicePdfFiles;
                }
                return liftDetailViewModel;
            }
            return null;
        }

        public static InvoiceFtlDetail ToEntity(this BolDetailViewModel viewModel)
        {
            viewModel.LiftStartTime = string.IsNullOrEmpty(viewModel.StartTime) ? viewModel.LiftStartTime:Convert.ToDateTime(viewModel.StartTime).TimeOfDay;
            viewModel.LiftEndTime = string.IsNullOrEmpty(viewModel.EndTime) ? viewModel.LiftEndTime : Convert.ToDateTime(viewModel.EndTime).TimeOfDay;
            var entity = new InvoiceFtlDetail
            {
                Id = viewModel.Id,
                GrossQuantity = viewModel.GrossQuantity,
                NetQuantity = viewModel.NetQuantity,
                DeliveredQuantity = viewModel.DeliveredQuantity,
                BolNumber = viewModel.BolNumber,
                Carrier = viewModel.Carrier,
                CreatedBy = viewModel.CreatedBy,
                CreatedDate = viewModel.CreatedDate,
                LiftDate = viewModel.LiftDate,
                BolCreationTime = viewModel.BolCreationTime,
                LiftTicketNumber = viewModel.LiftTicketNumber,
                LiftArrivalTime = viewModel.LiftArrivalTime,
                LiftStartTime = viewModel.LiftStartTime,
                LiftEndTime = viewModel.LiftEndTime,
                LiftQuantity = viewModel.LiftQuantity,
                Address = viewModel.Address,
                AddressLine2 = viewModel.AddressLine2,
                AddressLine3 = viewModel.AddressLine3,
                City = viewModel.City,
                StateCode = viewModel.StateCode,
                StateId = viewModel.StateId.HasValue && viewModel.StateId.Value > 0 ? viewModel.StateId : null,
                ZipCode = viewModel.ZipCode,
                CountryCode = viewModel.CountryCode,
                Latitude = viewModel.Latitude,
                Longitude = viewModel.Longitude,
                CountyName = viewModel.CountyName,
                SiteName = viewModel.SiteName,
                PickupLocation = viewModel.PickupLocationType,
                TerminalId = viewModel.TerminalId.HasValue && viewModel.TerminalId > 0 ? viewModel.TerminalId : null,
                CityGroupTerminalId = viewModel.CityGroupTerminalId > 0 ? viewModel.CityGroupTerminalId : null,
                PricePerGallon = viewModel.PricePerGallon,
                FuelTypeId = viewModel.FuelTypeId,
                IsActive = viewModel.IsActive,
                IsDeleted = viewModel.IsDeleted,
                RackPrice = viewModel.RackPrice,
                TerminalName = viewModel.TerminalName,
                BadgeNumber = viewModel.BadgeNumber,
                EbolMatchStatus=viewModel.EbolMatchStatus,
                RecordHistory = viewModel.RecordHistory
            };

            if (viewModel.TierPricingForBol != null && viewModel.TierPricingForBol.Any())
            {
                foreach (var tierPricing in viewModel.TierPricingForBol)
                {
                    entity.InvoiceTierPricingDetails.Add(new InvoiceTierPricingDetail()
                    {
                        PricePerGallon = tierPricing.PricePerGallon,
                        Quantity = tierPricing.Quantity,
                        TierMinQuantity = tierPricing.TierMinQuantity,
                        TierMaxQuantity = tierPricing.TierMaxQuantity
                    });
                }
            }

            return entity;
        }

        public static InvoiceFtlDetail ToBolEntity(this InvoiceModel viewModel, InvoiceFtlDetail entity = null)
        {
            var bolDetails = viewModel.BolDetails.First();
            if ((entity != null) && (bolDetails.BolNumber != entity.BolNumber || bolDetails.Carrier != entity.Carrier || bolDetails.GrossQuantity != entity.GrossQuantity
                || bolDetails.NetQuantity != entity.NetQuantity || bolDetails.LiftDate != entity.LiftDate
                || IsImageChange(viewModel, entity)))
            {
                InvoiceFtlDetail bolEntity = new InvoiceFtlDetail();
                bolEntity.CreatedBy = entity.CreatedBy;
                bolEntity.CreatedDate = DateTimeOffset.Now;
                bolEntity.BolNumber = bolDetails.BolNumber;
                bolEntity.Carrier = bolDetails.Carrier;
                bolEntity.GrossQuantity = bolDetails.GrossQuantity;
                bolEntity.NetQuantity = bolDetails.NetQuantity;
                bolEntity.DeliveredQuantity = bolDetails.DeliveredQuantity;
                bolEntity.LiftDate = bolDetails.LiftDate;
                bolEntity.LiftStartTime = Convert.ToDateTime(bolDetails.StartTime).TimeOfDay;
                bolEntity.LiftEndTime = Convert.ToDateTime(bolDetails.EndTime).TimeOfDay;
                bolEntity.LiftQuantity = bolDetails.LiftQuantity;
                bolEntity.LiftArrivalTime = bolDetails.LiftArrivalTime;
                bolEntity.LiftStartTime = bolDetails.LiftStartTime;
                bolEntity.LiftEndTime = bolDetails.LiftEndTime;
                bolEntity.LiftTicketNumber = bolDetails.LiftTicketNumber;
                bolEntity.BolCreationTime = bolDetails.BolCreationTime;
                bolEntity.TerminalId = bolDetails.TerminalId;
                bolEntity.CityGroupTerminalId = bolDetails.CityGroupTerminalId;
                bolEntity.PricePerGallon = viewModel.PricePerGallon;
                bolEntity.RackPrice = viewModel.RackPrice;
                if (viewModel.FuelPickLocation != null && viewModel.FuelPickLocation.ZipCode != null)
                {
                    InvoiceMapper.SetPickupAddressToBolDetails(bolEntity, viewModel.FuelPickLocation);
                }

                if (viewModel.BolImage != null && viewModel.BolImage.Id > 0)
                {
                    bolEntity.ImageId = viewModel.BolImage.Id;
                }
                else if (viewModel.BolImage != null && !string.IsNullOrWhiteSpace(viewModel.BolImage?.FilePath))
                {
                    bolEntity.Image = viewModel.BolImage.ToEntity();
                }
                return bolEntity;
            }
            return entity;
        }

        public static InvoiceFtlDetail ToBolEntity(this InvoiceViewModel viewModel, InvoiceFtlDetail entity = null)
        {
            if ((entity != null) && (viewModel.BolDetails.BolNumber != entity.BolNumber || viewModel.BolDetails.Carrier != entity.Carrier || viewModel.BolDetails.GrossQuantity != entity.GrossQuantity
                || viewModel.BolDetails.NetQuantity != entity.NetQuantity || viewModel.BolDetails.LiftDate != entity.LiftDate
                || IsImageChange(viewModel, entity)))
            {
                InvoiceFtlDetail bolEntity = new InvoiceFtlDetail();
                bolEntity.CreatedBy = entity.CreatedBy;
                bolEntity.CreatedDate = DateTimeOffset.Now;
                bolEntity.BolNumber = viewModel.BolDetails.BolNumber;
                bolEntity.Carrier = viewModel.BolDetails.Carrier;
                bolEntity.GrossQuantity = viewModel.BolDetails.GrossQuantity.Value;
                bolEntity.NetQuantity = viewModel.BolDetails.NetQuantity.Value;
                bolEntity.DeliveredQuantity = viewModel.BolDetails.DeliveredQuantity.Value;
                bolEntity.LiftDate = viewModel.BolDetails.LiftDate;
                bolEntity.LiftStartTime = Convert.ToDateTime(viewModel.BolDetails.StartTime).TimeOfDay;
                bolEntity.LiftEndTime = Convert.ToDateTime(viewModel.BolDetails.EndTime).TimeOfDay;
                bolEntity.LiftArrivalTime = viewModel.BolDetails.LiftArrivalTime;
                bolEntity.LiftStartTime = viewModel.BolDetails.LiftStartTime;
                bolEntity.LiftEndTime = viewModel.BolDetails.LiftEndTime;
                bolEntity.LiftTicketNumber = viewModel.BolDetails.LiftTicketNumber;
                bolEntity.BolCreationTime = viewModel.BolDetails.BolCreationTime;
                bolEntity.LiftQuantity = viewModel.BolDetails.LiftQuantity;
                bolEntity.TerminalId = viewModel.TerminalId;
                bolEntity.CityGroupTerminalId = viewModel.CityGroupTerminalId;
                bolEntity.PricePerGallon = viewModel.PricePerGallon;


                if (viewModel.FuelPickLocation != null && viewModel.FuelPickLocation.ZipCode != null)
                {
                    InvoiceMapper.SetPickupAddressToBolDetails(bolEntity, viewModel.FuelPickLocation);
                }

                if (viewModel.BolImage != null && viewModel.BolImage.Id > 0)
                {
                    bolEntity.ImageId = viewModel.BolImage.Id;
                }
                else if (viewModel.BolImage != null && !string.IsNullOrWhiteSpace(viewModel.BolImage?.FilePath))
                {
                    bolEntity.Image = viewModel.BolImage.ToEntity();
                }
                return bolEntity;
            }
            return entity;
        }

        private static bool IsImageChange(dynamic viewModel, InvoiceFtlDetail entity)
        {
            return (viewModel.BolImage != null && ((viewModel.BolImage.Id == 0 && !string.IsNullOrWhiteSpace(viewModel.BolImage?.FilePath) && entity.Image == null) ||
                (viewModel.BolImage.Id == 0 && !string.IsNullOrWhiteSpace(viewModel.BolImage?.FilePath) && entity.Image != null) ||
                (!string.IsNullOrWhiteSpace(viewModel.BolImage?.FilePath) && entity.Image != null && viewModel.BolImage.FilePath != entity.Image.FilePath)) || viewModel.BolImage == null && entity.Image != null);
        }

        public static DispatchAddressViewModel ToDispatchAddress(this InvoiceDispatchLocation entity)
        {
            var dispatchAddress = new DispatchAddressViewModel();
            if (entity != null)
            {
                dispatchAddress.Address = entity.Address;
                dispatchAddress.AddressLine2 = entity.AddressLine2;
                dispatchAddress.AddressLine3 = entity.AddressLine3;
                dispatchAddress.City = entity.City;
                dispatchAddress.Country.Code = entity.CountryCode;
                dispatchAddress.CountyName = entity.CountyName;
                dispatchAddress.Latitude = entity.Latitude;
                dispatchAddress.Longitude = entity.Longitude;
                dispatchAddress.State.Id = entity.StateId ?? 0;
                dispatchAddress.ZipCode = entity.ZipCode;
            }
            return dispatchAddress;
        }

        public static DispatchLocationViewModel ToDropLocation(this ManualInvoiceViewModel entity)
        {
            var dispatchLocation = new DispatchLocationViewModel();
            if (entity.DropAddress != null)
            {
                dispatchLocation.Address = entity.DropAddress.Address;
                dispatchLocation.AddressLine2 = entity.DropAddress.AddressLine2;
                dispatchLocation.AddressLine3 = entity.DropAddress.AddressLine3;
                dispatchLocation.City = entity.DropAddress.City;
                dispatchLocation.ZipCode = entity.DropAddress.ZipCode;
                dispatchLocation.CountyName = entity.DropAddress.CountyName;
                dispatchLocation.Latitude = entity.DropAddress.Latitude;
                dispatchLocation.Longitude = entity.DropAddress.Longitude;
                dispatchLocation.IsAddressAvailable = entity.DropAddress.IsAddressAvailable;
                dispatchLocation.StateCode = entity.DropAddress.State.Code;
                dispatchLocation.StateId = entity.DropAddress.State.Id;
                dispatchLocation.CountryCode = entity.DropAddress.Country.Code;
                dispatchLocation.CountyName = entity.DropAddress.Country.Name;
                dispatchLocation.LocationType = (int)LocationType.Drop;
                dispatchLocation.OrderId = entity.OrderId;
                dispatchLocation.CreatedBy = entity.userId;
                dispatchLocation.SiteName = entity.DropAddress.SiteName;
                dispatchLocation.CreatedDate = entity.CreatedDate;
            }
            return dispatchLocation;
        }

        public static DispatchLocationViewModel ToDropLocation(this DropAddressViewModel entity)
        {
            var dispatchLocation = new DispatchLocationViewModel();
            dispatchLocation.Address = entity.Address;
            dispatchLocation.AddressLine2 = entity.AddressLine2;
            dispatchLocation.AddressLine3 = entity.AddressLine3;
            dispatchLocation.City = entity.City;
            dispatchLocation.ZipCode = entity.ZipCode;
            dispatchLocation.CountyName = entity.CountyName;
            dispatchLocation.Latitude = entity.Latitude;
            dispatchLocation.Longitude = entity.Longitude;
            dispatchLocation.IsAddressAvailable = entity.IsAddressAvailable;
            dispatchLocation.StateCode = entity.State.Code;
            dispatchLocation.StateId = entity.State.Id;
            dispatchLocation.CountryCode = entity.Country.Code;
            dispatchLocation.LocationType = (int)LocationType.Drop;
            dispatchLocation.SiteName = entity.SiteName;

            return dispatchLocation;
        }

        public static DispatchLocationViewModel ToDropLocation(this InvoiceViewModel entity)
        {
            var dispatchLocation = new DispatchLocationViewModel();
            if (entity.DropAddress != null)
            {
                dispatchLocation.Address = entity.DropAddress.Address;
                dispatchLocation.AddressLine2 = entity.DropAddress.AddressLine2;
                dispatchLocation.AddressLine3 = entity.DropAddress.AddressLine3;
                dispatchLocation.City = entity.DropAddress.City;
                dispatchLocation.CountryCode = entity.DropAddress.Country.Code;
                dispatchLocation.CountyName = entity.DropAddress.Country.Name;
                dispatchLocation.CountyName = entity.DropAddress.CountyName;
                dispatchLocation.Latitude = entity.DropAddress.Latitude;
                dispatchLocation.LocationType = (int)LocationType.Drop;
                dispatchLocation.Longitude = entity.DropAddress.Longitude;
                dispatchLocation.StateCode = entity.DropAddress.State.Code;
                dispatchLocation.StateId = entity.DropAddress.State.Id;
                dispatchLocation.ZipCode = entity.DropAddress.ZipCode;
                dispatchLocation.OrderId = entity.OrderId ?? 0;
                dispatchLocation.CreatedBy = entity.UserId;
                dispatchLocation.CreatedDate = entity.CreatedDate;
                dispatchLocation.SiteName = entity.DropAddress.SiteName;
            }
            return dispatchLocation;
        }

        public static DispatchLocationViewModel ToPickUpLocation(this ManualInvoiceViewModel entity)
        {
            if (entity.PickUpAddress != null)
            {
                var pickupLocation = new DispatchLocationViewModel();

                pickupLocation.Address = entity.PickUpAddress.Address;
                pickupLocation.AddressLine2 = entity.PickUpAddress.AddressLine2;
                pickupLocation.AddressLine3 = entity.PickUpAddress.AddressLine3;
                pickupLocation.City = entity.PickUpAddress.City;
                pickupLocation.CountryCode = entity.PickUpAddress.Country.Code;
                pickupLocation.CountyName = entity.PickUpAddress.Country.Name;
                pickupLocation.CountyName = entity.PickUpAddress.CountyName;
                pickupLocation.Latitude = entity.PickUpAddress.Latitude;
                pickupLocation.LocationType = (int)LocationType.PickUp;
                pickupLocation.Longitude = entity.PickUpAddress.Longitude;
                pickupLocation.StateCode = entity.PickUpAddress.State.Code;
                pickupLocation.StateId = entity.PickUpAddress.State.Id;
                pickupLocation.ZipCode = entity.PickUpAddress.ZipCode;
                pickupLocation.OrderId = entity.OrderId;
                pickupLocation.CreatedBy = entity.userId;
                pickupLocation.SiteName = entity.PickUpAddress.SiteName;
                pickupLocation.CreatedDate = entity.CreatedDate;

                return pickupLocation;
            }
            return null;
        }

        public static DropAddressViewModel ToPickUpLocation(this FuelDispatchLocation entity)
        {
            if (entity != null)
            {
                var pickupLocation = new DropAddressViewModel();

                pickupLocation.Address = entity.Address;
                pickupLocation.AddressLine2 = entity.AddressLine2;
                pickupLocation.AddressLine3 = entity.AddressLine3;
                pickupLocation.City = entity.City;
                pickupLocation.Country = new CountryViewModel()
                {
                    Code = entity.CountryCode,
                    Id = (entity.CountryCode.Contains("US") ? 1 : 2)
                };
                pickupLocation.CountyName = entity.CountyName;
                pickupLocation.Latitude = entity.Latitude;
                pickupLocation.Longitude = entity.Longitude;
                pickupLocation.State = new StateViewModel() { Code = entity.StateCode, Id = entity.StateId.Value };
                pickupLocation.ZipCode = entity.ZipCode;
                pickupLocation.IsAddressAvailable = true;
                pickupLocation.SiteName = entity.SiteName;
                pickupLocation.SiteId = entity.Id;
                pickupLocation.BulkPlantId = entity.BulkPlantId;
                return pickupLocation;
            }
            return null;
        }

        public static DispatchLocationViewModel ToPickUpLocation(this UspPickupAddressViewModel entity)
        {
            if (entity != null)
            {
                var pickupLocation = new DispatchLocationViewModel();

                pickupLocation.SiteName = entity.LocationName;
                pickupLocation.Address = entity.PickupAddress;
                pickupLocation.AddressLine2 = entity.PickupAddressLine2;
                pickupLocation.AddressLine3 = entity.PickupAddressLine3;
                pickupLocation.City = entity.PickupCity;
                pickupLocation.StateCode = entity.PickupStateCode;
                pickupLocation.CountryCode = entity.CountryCode;
                pickupLocation.ZipCode = entity.PickupZipCode;
                pickupLocation.PickupLocationType = entity.PickupLocationType;

                pickupLocation.FuelTypeId = entity.FuelTypeId;
                pickupLocation.TerminalName = entity.TerminalName;
                pickupLocation.FuelType = entity.FuelType;
                return pickupLocation;
            }
            return null;
        }
    }
}
