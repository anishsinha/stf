using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.CJob.DeliveryAlerts.Firebase
{
    public static class MobileInvoiceMapper
    {
        public static InvoiceViewModelNew ToInvoiceViewModelNew(this MobileInvoiceViewModel model)
        {
            var entity = new InvoiceViewModelNew();
            entity.Customer = model.Customer.ToCustomerViewModel();
            if(model.BolDetails != null || model.TicketDetails != null)
            {
                var prodQty = model.BolDetails.SelectMany(t => t.Products.Where(t1 => t1.NetQuantity.HasValue).Select(t1 => new  { t1.ProductId, t1.NetQuantity, t1.GrossQuantity })).ToList();
                prodQty.AddRange(model.TicketDetails.SelectMany(t => t.Products.Where(t1 => t1.NetQuantity.HasValue).Select(t1 => new { t1.ProductId, t1.NetQuantity, t1.GrossQuantity })).ToList());
                
                var productQuantity = prodQty.GroupBy(t => t.ProductId)
                                            .Select(t => new { ProductId = t.Key, NetQuantity = t.Sum(x => x.NetQuantity), GrossQuantity = t.Sum(x => x.GrossQuantity) })
                                            .Select(t => new ProductQuantityViewModel { 
                                                        ProductId = t.ProductId, 
                                                        DeliveredQuantity = model.Drops.Where(t1 => t1.FuelTypeId == t.ProductId).Sum(t1 => t1.ActualDropQuantity),
                                                        BillableType = (t.NetQuantity > t.GrossQuantity ? QuantityIndicatorTypes.Net : QuantityIndicatorTypes.Gross),
                                            }).ToList();
                
                if (model.BolDetails != null)
                {
                    entity.BolDetails = model.BolDetails.Select(t => t.ToBolDetail(productQuantity)).Distinct().ToList();
                    if (entity.BolDetails.Any(t => string.IsNullOrWhiteSpace( t.Images.FilePath)))
                    {
                        var bolWithImagePath = model.BolDetails.Where(t => !string.IsNullOrWhiteSpace(t.Images.FilePath))
                                                       .Select(t => new { t.BolNumber, t.Images, TerminalforBol = t.Products.Where(t1 => t1.TerminalId.HasValue).Select(x => x.TerminalId.Value).FirstOrDefault() })
                                                       .ToList();

                        foreach (var bolRecord in entity.BolDetails)
                        {
                            if(string.IsNullOrWhiteSpace(bolRecord.Images.FilePath))
                            {
                                var bolRecordsTerminals = bolRecord.Products.Where(t => t.TerminalId.HasValue).Select(t => t.TerminalId.Value).FirstOrDefault();
                                var imageObj = bolWithImagePath.Where(t => t.BolNumber == bolRecord.BolNumber && bolRecordsTerminals == t.TerminalforBol).Select(t => t.Images).FirstOrDefault();
                                if(imageObj != null)
                                {
                                    bolRecord.Images = imageObj.ToImageViewModel();
                                }
                            }
                        }
                    }
                }
                if (model.TicketDetails != null)
                {
                    entity.TicketDetails = model.TicketDetails.Select(t => t.ToLiftTicketViewModel(productQuantity)).Distinct().ToList();
                    if (entity.TicketDetails.Any(t => string.IsNullOrWhiteSpace(t.Images.FilePath)))
                    {
                        var bolWithImagePath = model.TicketDetails.Where(t => !string.IsNullOrWhiteSpace(t.Images.FilePath))
                                                       .Select(t => new { t.LiftTicketNumber, t.Images })
                                                       .ToList();

                        foreach (var ticketRecord in entity.TicketDetails)
                        {
                            if (string.IsNullOrWhiteSpace(ticketRecord.Images.FilePath))
                            {
                                var imageObj = bolWithImagePath.Where(t => t.LiftTicketNumber == ticketRecord.LiftTicketNumber).Select(t => t.Images).FirstOrDefault();
                                if (imageObj != null)
                                {
                                    ticketRecord.Images = imageObj.ToImageViewModel();
                                }
                            }
                        }
                    }
                }
            }

            entity.DivertedDrops = model.Drops.Where(d => d.DropStatus == MobileDropStatus.DropDiversion).Select(t => t.ToInvoiceDropViewModel()).ToList();
            entity.Driver = new Utilities.DropdownDisplayItem() { Id = model.DriverId };
            entity.Carrier = model.Carrier;

            if (model.Drops.All(d => d.DropStatus == MobileDropStatus.DropDiversion))
            {
                entity.DiversionType = DiversionType.Full;
                entity.Drops = model.Drops.Select(t => t.ToInvoiceDropViewModel()).ToList();
            }
            else if (model.Drops.Any(d => d.DropStatus == MobileDropStatus.DropDiversion))
            {
                entity.DiversionType = DiversionType.Partail;
                entity.Drops = model.Drops.Where(d => d.DropStatus != MobileDropStatus.DropDiversion).Select(t => t.ToInvoiceDropViewModel()).ToList();
            }
            else
            {
                entity.DiversionType = DiversionType.None;
                entity.Drops = model.Drops.Select(t => t.ToInvoiceDropViewModel()).ToList();
            }

            entity.InvoiceImage = model.InvoiceImage?.ToImageViewModel();
            entity.SignatureImage = model.SignatureImage?.ToImageViewModel();
            entity.AdditionalImage = model.AdditionalImage?.ToImageViewModel();
            entity.InspectionRequestVoucherImage = model.InspectionVoucherImage?.ToImageViewModel();
            entity.FuelDropLocation = model.FuelDropLocation.ToAddressViewModel();
            if (entity.Drops.Any(t => t.Assets.Count > 0))
            {
                entity.Drops.SelectMany(t => t.Assets).ToList().ForEach(t => t.DroppedBy = model.DriverId);
            }
            if (model.SpecialInstructions != null)
            {
                entity.SpecialInstructions = model.SpecialInstructions.ToInvoiceInstruction();
            }
            if (model.DemurrageDetails != null)
            {
                var demurrageFees = model.DemurrageDetails.Select(t => t.ToDemurrageDetailsViewModel()).ToList();
                entity.Fees = demurrageFees.ToDemurrageFees();
            }
            entity.CreationMethod = CreationMethod.Mobile;
            entity.DropStatus = model.DropStatus;
            entity.TraceId = model.TraceId;
            return entity;
        }

        public static CustomerViewModel ToCustomerViewModel(this MobileCustomerViewModel model)
        {
            var entity = new CustomerViewModel();
            entity.CompanyId = model.CompanyId;
            entity.CompanyName = model.CompanyName;
            entity.ContactEmail = model.ContactEmail;
            entity.ContactName = model.ContactName;
            entity.ContactPhone = model.ContactPhone;
            entity.Location = model.Location?.ToJobLocationViewModel();
            return entity;
        }

        public static JobLocationViewModel ToJobLocationViewModel(this MobileJobLocationViewModel model)
        {
            var entity = new JobLocationViewModel();
            entity.Address = model.Address;
            entity.City = model.City;
            entity.JobId = model.JobId;
            entity.SiteName = model.SiteName;
            entity.StateCode = model.StateCode;
            entity.ZipCode = model.ZipCode;
            return entity;
        }

        public static InvoiceBolViewModel ToBolDetail(this MobileInvoiceBolViewModel model, List<ProductQuantityViewModel> drops)
        {
            var entity = new InvoiceBolViewModel();
            entity.BolNumber = model.BolNumber;
            entity.Id = model.Id;
            entity.Images = model.Images?.ToImageViewModel();
            entity.LiftDate = DateTimeOffset.FromUnixTimeMilliseconds(model.LiftDate);
            if(model.LiftStartTime.HasValue && model.LiftStartTime.Value > 0)
                entity.LiftStartTime = DateTimeOffset.FromUnixTimeMilliseconds(model.LiftStartTime.Value).ToString();
            if (model.LiftEndTime.HasValue && model.LiftEndTime.Value > 0)
                entity.LiftEndTime = DateTimeOffset.FromUnixTimeMilliseconds(model.LiftEndTime.Value).ToString();

            entity.BadgeNumber = model.BadgeNumber;
            var allProds = model.Products.GroupBy(t => t.ProductId).ToList();
            if (allProds.AnyAndNotNull())
            {
                entity.Products = new List<BolProductViewModel>();
                var isDeliveredQtyPresent = model.Products.All(t => t.DeliveredQuantity.HasValue);
                if (isDeliveredQtyPresent)
                {
                    entity.Products = model.Products.Select(t => t.ToBolProductViewModel(t.DeliveredQuantity)).Distinct().ToList();
                }
                else
                {
                    foreach (var product in allProds)
                    {
                        var drop = drops.Where(t => t.ProductId == product.Key).FirstOrDefault();
                        double? dropQuantity = drop?.DeliveredQuantity;
                        foreach (var prod in product.ToList())
                        {
                            if (drop != null)
                            {
                                if (drop.BillableType == QuantityIndicatorTypes.Net)
                                {
                                    dropQuantity = prod.NetQuantity > drop?.DeliveredQuantity ? drop?.DeliveredQuantity : prod.NetQuantity;
                                }
                                else
                                {
                                    dropQuantity = prod.GrossQuantity > drop?.DeliveredQuantity ? drop?.DeliveredQuantity : prod.GrossQuantity;
                                }
                                drop.DeliveredQuantity = drop.DeliveredQuantity - dropQuantity;
                            }
                            var bolModel = prod.ToBolProductViewModel(dropQuantity);
                            entity.Products.Add(bolModel);
                        }
                    }
                }
            }
            return entity;
        }

        public static BolProductViewModel ToBolProductViewModel(this MobileBolProductViewModel model, double? deliveredQty)
        {
            var entity = new BolProductViewModel();
            entity.ProductId = model.ProductId;
            entity.ProductName = model.ProductName;
            if (model.NetQuantity.HasValue)
            {
                entity.NetQuantity = Convert.ToDecimal(model.NetQuantity.Value);
                if (model.DeliveredQuantity.HasValue)
                {
                    entity.DeliveredQuantity = Convert.ToDecimal(model.DeliveredQuantity);
                }
                else if(deliveredQty.HasValue)
                {
                    entity.DeliveredQuantity = Convert.ToDecimal(deliveredQty);
                }
            }
            if (model.GrossQuantity.HasValue)
            {
                entity.GrossQuantity = Convert.ToDecimal(model.GrossQuantity);
            }
            entity.TerminalId = model.TerminalId;
            entity.TerminalName = model.TerminalName;
            if (model.CompartmentInfo != null && model.CompartmentInfo.Count > 0)
            {
                entity.CompartmentInfo = model.CompartmentInfo.Select(t => t.ToCompartmentInfoViewModel()).ToList();
            }
            return entity;
        }

        public static CompartmentInfoViewModel ToCompartmentInfoViewModel(this MobileCompartmentInfoViewModel model)
        {
            var entity = new CompartmentInfoViewModel();
            entity.CompartmentId = model.CompartmentId;
            entity.TrailerId = model.TrailerId;
            entity.Quantity = Convert.ToDecimal(model.Quantity);
            entity.TrackableScheduleId = model.TrackableScheduleId;
            return entity;
        }

        public static InvoiceLiftTicketViewModel ToLiftTicketViewModel(this MobileInvoiceLiftTicketViewModel model, List<ProductQuantityViewModel> drops)
        {
            var entity = new InvoiceLiftTicketViewModel();
            entity.Id = model.Id;
            entity.LiftTicketNumber = model.LiftTicketNumber;
            entity.LiftDate = DateTimeOffset.FromUnixTimeMilliseconds(model.LiftDate);
            entity.BadgeNumber = model.BadgeNumber;
            entity.Images = model.Images?.ToImageViewModel();
            if(model.LiftStartTime.HasValue && model.LiftStartTime.Value > 0)
                entity.LiftStartTime = DateTimeOffset.FromUnixTimeMilliseconds(model.LiftStartTime.Value).ToString();
            if(model.LiftEndTime.HasValue && model.LiftEndTime.Value > 0)
                entity.LiftEndTime = DateTimeOffset.FromUnixTimeMilliseconds(model.LiftEndTime.Value).ToString();

            var allProds = model.Products.GroupBy(t => t.ProductId).ToList();
            if (allProds.AnyAndNotNull())
            {
                entity.Products = new List<LiftProductViewModel>();
                var isDeliveredQtyPresent = model.Products.All(t => t.DeliveredQuantity.HasValue);
                if (isDeliveredQtyPresent)
                {
                    entity.Products = model.Products.Select(t => t.ToLiftProductViewModel(t.DeliveredQuantity)).Distinct().ToList();
                }
                else
                {
                    foreach (var product in allProds)
                    {
                        var drop = drops.Where(t => t.ProductId == product.Key).FirstOrDefault();
                        double? dropQuantity = drop?.DeliveredQuantity;
                        foreach (var prod in product.ToList())
                        {
                            if (drop != null)
                            {
                                if (drop.BillableType == QuantityIndicatorTypes.Net)
                                {
                                    dropQuantity = prod.NetQuantity > drop?.DeliveredQuantity ? drop?.DeliveredQuantity : prod.NetQuantity;
                                }
                                else
                                {
                                    dropQuantity = prod.GrossQuantity > drop?.DeliveredQuantity ? drop?.DeliveredQuantity : prod.GrossQuantity;
                                }
                                drop.DeliveredQuantity = drop.DeliveredQuantity - dropQuantity;
                            }
                            var liftModel = prod.ToLiftProductViewModel(dropQuantity);
                            entity.Products.Add(liftModel);
                        }
                    }
                }
            }
            return entity;
        }

        public static LiftProductViewModel ToLiftProductViewModel(this MobileLiftProductViewModel model, double? deliveredQty)
        {
            var entity = new LiftProductViewModel();
            entity.ProductId = model.ProductId;
            entity.ProductName = model.ProductName;
            if (model.LiftQuantity.HasValue && model.LiftQuantity > 0)
            {
                entity.LiftQuantity = Convert.ToDecimal(model.LiftQuantity);
            }
            if (model.NetQuantity.HasValue)
            {
                entity.NetQuantity = Convert.ToDecimal(model.NetQuantity);
                if (model.DeliveredQuantity.HasValue)
                {
                    entity.DeliveredQuantity = Convert.ToDecimal(model.DeliveredQuantity);
                }
                else if (deliveredQty.HasValue)
                {
                    entity.DeliveredQuantity = Convert.ToDecimal(deliveredQty);
                }
            }
            if (model.GrossQuantity.HasValue)
            {
                entity.GrossQuantity = Convert.ToDecimal(model.GrossQuantity);
            }

            entity.BulkPlantId = model.BulkPlantId;
            entity.BulkPlantName = model.BulkPlantName;
            entity.Address = model.Address?.ToAddressViewModel();
            if (model.CompartmentInfo != null && model.CompartmentInfo.Count > 0)
            {
                entity.CompartmentInfo = model.CompartmentInfo.Select(t => t.ToCompartmentInfoViewModel()).ToList();
            }
            return entity;
        }

        public static DropAddressViewModel ToAddressViewModel(this MobileDropAddressViewModel model)
        {
            var entity = new DropAddressViewModel();
            entity.Address = model.Address;
            entity.City = model.City;
            if (model.StateId > 0)
            {
                entity.State = new StateViewModel() { Id = model.StateId };
            }
            if (model.CountryId > 0)
            {
                entity.Country = new CountryViewModel { Id = model.CountryId };
            }
            entity.ZipCode = model.ZipCode;
            entity.CountyName = model.CountyName;
            entity.Latitude = Convert.ToDecimal(model.Latitude);
            entity.Longitude = Convert.ToDecimal(model.Longitude);
            entity.SiteId = model.JobId;
            entity.SiteName = model.JobName;
            return entity;
        }

        public static InvoiceDropViewModel ToInvoiceDropViewModel(this MobileInvoiceDropViewModel model)
        {
            var entity = new InvoiceDropViewModel();
            entity.OrderId = model.OrderId;
            entity.TypeOfFuel = model.TypeOfFuel;
            entity.FuelTypeId = model.FuelTypeId;
            entity.ActualDropQuantity = Convert.ToDecimal(model.ActualDropQuantity);
            entity.DropDate = DateTimeOffset.FromUnixTimeMilliseconds(model.DropDate);
            entity.StartTime = model.StartTime;
            entity.EndTime = model.EndTime;
            entity.TrackableScheduleId = model.TrackableScheduleId;
            entity.DeliveryLevelPO = model.DeliveryLevelPO;
            entity.TerminalId = model.TerminalId;
            entity.PickupLocationType = model.PickupLocationType;
            entity.PickUpAddress = model.PickUpAddress?.ToAddressViewModel();
            entity.AssetCount = model.AssetCount;
            entity.IsAssetDropOffline = model.IsAssetDropOffline;
            entity.DropTicketNumber = model.DropTicketNumber;
            entity.IsFilldInvoke = model.IsFilldInvoke;
            entity.FilldStopId = model.FilldStopId;
            if (model.AssetDrops != null)
            {
                entity.Assets = model.AssetDrops.Select(t => t.ToAssetDropViewModel()).ToList();
            }
            if(model.BdrDetails != null)
            {
                entity.BdrDetails = model.BdrDetails.ToViewModel();
            }
            return entity;
        }

        public static AssetDropViewModel ToAssetDropViewModel(this MobileAssetDropViewModel model)
        {
            var entity = new AssetDropViewModel();
            entity.OrderId = model.OrderId;
            entity.AssetName = model.AssetName;
            entity.JobXAssetId = model.JobXAssetId;
            entity.DropStatusId = model.DropStatusId;
            if (model.DropGallons > 0)
            {
                entity.DropGallons = Convert.ToDecimal(model.DropGallons);
            }
            entity.DropDate = DateTimeOffset.FromUnixTimeMilliseconds(model.DropDate);
            entity.StartTime = model.StartTime;
            entity.EndTime = model.EndTime;
            entity.MeterStartReading = Convert.ToDecimal(model.MeterStartReading);
            entity.MeterEndReading = Convert.ToDecimal(model.MeterEndReading);
            entity.IsNewAsset = model.IsNewAsset;
            if (model.PreDip.HasValue)
                entity.PreDip = Convert.ToDecimal(model.PreDip);
            if (model.PostDip.HasValue)
                entity.PostDip = Convert.ToDecimal(model.PostDip);
            entity.IsOfflineMode = model.IsOfflineMode;
            entity.TankScaleMeasurement = (TankScaleMeasurement)(model.TankScaleMeasure ?? 0);
            return entity;
        }

        public static BDRDetailsModel ToViewModel(this MobileBDRDetailViewModel entity, BDRDetailsModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new BDRDetailsModel();

            viewModel.InvoiceId = entity.InvoiceId;
            viewModel.BDRNumber = entity.BDRNumber;
            viewModel.CloseMeterReading = entity.CloseMeterReading;
            viewModel.DensityInVaccum = entity.DensityInVaccum;
            viewModel.FlashPoint = entity.FlashPoint;
            viewModel.IsEngineerInvitedToWitnessSample = entity.IsEngineerInvitedToWitnessSample;
            viewModel.IsNoticeToProtestIssued = entity.IsNoticeToProtestIssued;
            viewModel.MarpolSampleNumbers = entity.MarpolSampleNumbers;
            viewModel.MVMarpolSampleNumbers = entity.MVMarpolSampleNumbers;
            viewModel.MeasuredVolume = entity.MeasuredVolume;
            viewModel.ObservedTemperature = entity.ObservedTemperature;
            viewModel.OpenMeterReading = entity.OpenMeterReading;
            viewModel.PumpingStartTime = entity.PumpingStartTime;
            viewModel.PumpingStopTime = entity.PumpingStopTime;
            viewModel.StandardVolume = entity.StandardVolume;
            viewModel.SulphurContent = entity.SulphurContent;
            viewModel.Viscosity = entity.Viscosity;
            viewModel.MVMarpolSampleNumbers = entity.MVMarpolSampleNumbers;

            return viewModel;
        }

        public static ImageViewModel ToImageViewModel(this MobileImageViewModel model)
        {
            var entity = new ImageViewModel();
            entity.Id = model.Id;
            entity.FilePath = model.FilePath;
            entity.IsPdf = model.IsPdf;
            entity.SignatureName = model.SignatureName;
            return entity;
        }

        public static List<InvoiceXSpecialInstructionViewModel> ToInvoiceInstruction(this Dictionary<string, bool> models)
        {
            var entities = new List<InvoiceXSpecialInstructionViewModel>();
            foreach (var model in models)
            {
                var entity = new InvoiceXSpecialInstructionViewModel();
                entity.Instruction = model.Key;
                entity.IsInstructionFollowed = model.Value;
                entities.Add(entity);
            }
            return entities;
        }

        public static DemurrageDetailsViewModel ToDemurrageDetailsViewModel(this DemurrageDetail entity, DemurrageDetailsViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new DemurrageDetailsViewModel();

            viewModel.StartTime = DateTimeOffset.FromUnixTimeMilliseconds(entity.StartTime);
            viewModel.StartTime = viewModel.StartTime.AttachOffset(new TimeSpan(0, entity.StartOffset, 0));

            viewModel.EndTime = DateTimeOffset.FromUnixTimeMilliseconds(entity.EndTime);
            viewModel.EndTime = viewModel.EndTime.AttachOffset(new TimeSpan(0, entity.EndOffset, 0));
            viewModel.FeeTypeId = entity.FeeTypeId;
            return viewModel;
        }

        public static List<FeesViewModel> ToDemurrageFees(this List<DemurrageDetailsViewModel> models)
        {
            var entities = new List<FeesViewModel>();
            foreach (var item in models)
            {
                var fee = new FeesViewModel();
                fee.StartTime = item.StartTime;
                fee.EndTime = item.EndTime;
                fee.FeeTypeId = item.FeeTypeId.ToString();
                entities.Add(fee);
            }
            return entities;
        }
    }
}
