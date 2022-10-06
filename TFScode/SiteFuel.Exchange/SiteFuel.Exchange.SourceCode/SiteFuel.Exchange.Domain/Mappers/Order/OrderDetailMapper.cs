using Newtonsoft.Json;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class OrderDetailMapper
    {
        public static void ToOrderAdditionalDetailViewModel(this UspGetSupplierOrderDetail spResponse, OrderDetailsViewModel response)
        {
            response.OrderAdditionalDetails.Allowance = spResponse.Allowance;
            response.OrderAdditionalDetails.BOLInvoicePreferenceTypes = spResponse.BolInvoicePreferenceId.HasValue ? (InvoiceNotificationPreferenceTypes)spResponse.BolInvoicePreferenceId : InvoiceNotificationPreferenceTypes.SendInvoiceDDTWithoutBillingFile;
            response.OrderAdditionalDetails.IsDriverToUpdateBOL = spResponse.IsDriverToUpdateBol.Value;
            response.OrderAdditionalDetails.IsFuelSurcharge = spResponse.IsFuelSurcharge.Value;
            response.OrderAdditionalDetails.IsFreightCost = spResponse.IsFreightCost.Value;
            response.OrderAdditionalDetails.IsFuelSurchargeAuto = spResponse.IsFuelSurcharge.Value;
            response.OrderAdditionalDetails.FuelSurchagePricingType = spResponse.FuelSurchagePricingType;
            response.OrderAdditionalDetails.SupplierAssignedProductName = spResponse.MyProductId;
            response.OrderAdditionalDetails.DriverProductId = spResponse.DriverProductId;

            if (spResponse.CarrierId.HasValue)
            {
                response.OrderAdditionalDetails.Carrier = new CarrierViewModel() { Id = spResponse.CarrierId.Value, Name = spResponse.CarrierName };
            }
            else
            {
                response.OrderAdditionalDetails.Carrier = new CarrierViewModel();
            }
            response.OrderAdditionalDetails.LoadCode = spResponse.LoadCode;
            response.OrderAdditionalDetails.Notes = spResponse.Notes;
            response.OrderAdditionalDetails.DRNotes = spResponse.Notes;
            response.OrderAdditionalDetails.SupplierSource = new SupplierSourceViewModel { ContractNumber = spResponse.SupplierContract };
            if (spResponse.SupplierSourceId.HasValue)
            {
                response.OrderAdditionalDetails.SupplierSource.Id = spResponse.SupplierSourceId;
                response.OrderAdditionalDetails.SupplierSource.Name = spResponse.SupplierSourceName;
            }
            else
            {
                response.OrderAdditionalDetails.SupplierSource = new SupplierSourceViewModel();
            }
            response.OrderAdditionalDetails.IsIncludePricing = spResponse.IsIncludePricing;
            response.OrderAdditionalDetails.IsPdiTaxRequired = spResponse.IsPdiTaxRequired;
            response.OrderAdditionalDetails.IsManualBDNConfirmationRequired = spResponse.IsBDNConfirmationRequired;
            response.OrderAdditionalDetails.IsManualInvoiceConfirmationRequired = spResponse.IsInvoiceConfirmationRequired;
            response.OrderAdditionalDetails.FreightPricingMethod = spResponse.FreightPricingMethod;
            if (spResponse.FreightRateRuleId.HasValue && spResponse.FreightRateRuleId.Value > 0)
            {
                response.OrderAdditionalDetails.FreightRateRuleType = spResponse.FreightRateRuleType;
                response.OrderAdditionalDetails.FreightRateTableType = spResponse.FreightRateTableType;
                response.OrderAdditionalDetails.FreightRateRuleId = spResponse.FreightRateRuleId;
            }
            if (spResponse.FuelSurchargeTableId.HasValue && spResponse.FuelSurchargeTableId.Value > 0)
            {
                response.OrderAdditionalDetails.FuelSurchargeTableType = spResponse.FuelSurchargeTableType;
                response.OrderAdditionalDetails.FuelSurchargeTableId = spResponse.FuelSurchargeTableId;
            }
            if (spResponse.AccessorialFeeId.HasValue && spResponse.AccessorialFeeId.Value > 0)
            {
                response.OrderAdditionalDetails.AccessorialFeeTableType = spResponse.AccessorialFeeTableType;
                response.OrderAdditionalDetails.AccessorialFeeId = spResponse.AccessorialFeeId;
            }
        }

        public static FuelDetailsViewModel ToFuelDetailViewModel(this UspGetSupplierOrderDetail spResponse, OrderDetailsViewModel orderDetails = null)
        {
            FuelDetailsViewModel viewModel = new FuelDetailsViewModel();
            viewModel.IsMarineLocation = spResponse.IsMarineLocation;
            viewModel.Berth = spResponse.Berth;
            viewModel.IMONumber = spResponse.IMONumber;
            viewModel.Flag = spResponse.Flag;
            viewModel.VessleId = spResponse.VessleId;
            viewModel.JobXAssetId = spResponse.JobXAssetId;
            viewModel.FuelQuantity.QuantityTypeId = spResponse.QuantityTypeId;
            viewModel.FuelQuantity.UoM = spResponse.UoM;
            if (spResponse.QuantityTypeId == (int)QuantityType.SpecificAmount)
            {
                viewModel.FuelQuantity.Quantity = spResponse.MaxQuantity.GetPreciseValue(6);
            }
            else if (spResponse.QuantityTypeId == (int)QuantityType.Range)
            {
                viewModel.FuelQuantity.MinimumQuantity = spResponse.MinQuantity.GetPreciseValue(6);
                viewModel.FuelQuantity.MaximumQuantity = spResponse.MaxQuantity.GetPreciseValue(6);
            }
            if (spResponse.FrPricingDetailId.HasValue)
            {
                viewModel.FuelPricing.FuelPricingDetails.PricingQuantityIndicatorTypeId = spResponse.PricingQuantityIndicatorTypeId;
                viewModel.FuelPricing.FuelPricingDetails.FeedTypeId = spResponse.FeedTypeId;
                viewModel.FuelPricing.FuelPricingDetails.PricingSourceId = spResponse.PricingSourceId ?? (int)PricingSource.Axxis;
                viewModel.FuelPricing.FuelPricingDetails.PricingSourceQuantityIndicatorTypeId = spResponse.PricingSourceQuantityIndicatorTypeId;
                var weekendPricingDropDay = spResponse.WeekendDropPricingDay ?? (int)WeekendDropPricingDay.Friday;
                viewModel.FuelPricing.FuelPricingDetails.WeekendDropPricingDay = (WeekendDropPricingDay)weekendPricingDropDay;
                viewModel.FuelPricing.FuelPricingDetails.FuelClassTypeId = spResponse.FuelClassTypeId;
                viewModel.FuelQuantity.QuantityIndicatorTypes = (QuantityIndicatorTypes)spResponse.PricingQuantityIndicatorTypeId;
            }
            viewModel.IsFTLEnabled = spResponse.IsFTL;
            viewModel.FuelTypeId = spResponse.FuelTypeId;
            viewModel.FuelType = spResponse.FuelType;
            viewModel.FreightOnBoard = spResponse.FreightOnboardTypeId;
            viewModel.FuelPricing.Currency = spResponse.Currency;
            viewModel.FuelPricing.CityGroupTerminalName = spResponse.CityGroupTerminalName;
            if (!string.IsNullOrEmpty(spResponse.PickupAddress))
            {
                viewModel.PickUpLocation = new PickUpAddressViewModel();
                viewModel.PickUpLocation.Address = new DispatchAddressViewModel { SiteName = spResponse.SiteName, Address = spResponse.PickupAddress, City = spResponse.PickupCity,
                                                        State = new StateViewModel { Code = spResponse.PickupStateCode },
                                                    ZipCode = spResponse.PickupZipCode };
            }
            if (orderDetails != null)
            {
                if(orderDetails.FuelDetails.IsTierPricing
                  && orderDetails.FuelDetails.FuelPricing.TierPricing.Pricings != null
                  && orderDetails.FuelDetails.FuelPricing.TierPricing.Pricings.Any())
                {
                    viewModel.FuelPricing.TierPricing.Pricings = orderDetails.FuelDetails.FuelPricing.TierPricing.Pricings;

                    if (!string.IsNullOrWhiteSpace(orderDetails.FuelDetails.FuelPricing.TierPricing.DisplayCumulationFrequency))
                    {
                        viewModel.FuelPricing.TierPricing.DisplayCumulationFrequency = orderDetails.FuelDetails.FuelPricing.TierPricing.DisplayCumulationFrequency;
                    }

                    viewModel.IsTierPricing = true;
                }
                //viewModel.FuelPricing.TierPricing.Pricings = tierPricingViewModel.Pricings;

            }

            return viewModel;
        }

        public static JobSpecificBillToViewModel BillToInfoViewModel(this UspGetSupplierOrderDetail orderDetail, JobSpecificBillToViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new JobSpecificBillToViewModel();

            if(orderDetail.BillingAddressId.HasValue/*orderDetail.IsBillToEnabled*/)
            {
                viewModel.BillingAddressId = orderDetail.BillingAddressId;
                viewModel.Name = orderDetail.BillToName;
                viewModel.Address = orderDetail.BillToAddress;
                viewModel.AddressLine2 = orderDetail.BillToAddressLine2;
                viewModel.AddressLine3 = orderDetail.BillToAddressLine3;
                viewModel.City = orderDetail.BillToCity;
                viewModel.County = orderDetail.BillToCounty;
                viewModel.ZipCode = orderDetail.BillToZipCode;
                viewModel.State.Code = orderDetail.BillToStateCode;
                viewModel.State.Id = orderDetail.BillToStateId ?? 0;
                viewModel.State.Name = orderDetail.BillToStateName;
                viewModel.Country.Code = orderDetail.BillToCountryCode;
                viewModel.Country.Id = orderDetail.BillToCountryId ?? 0;
                viewModel.Country.Name = orderDetail.BillToCountryName;
            }

            return viewModel;
        }

        public static Order ToNewOrder(this Order existingOrder, FuelRequest fuelRequest, UserContext carrierUserContext)
        {
            var newOrder = new Order();
            newOrder.AcceptedBy = carrierUserContext.Id;
            newOrder.AcceptedCompanyId = carrierUserContext.CompanyId;
            newOrder.AcceptedDate = DateTimeOffset.Now;
            newOrder.BrokeredMaxQuantity = existingOrder.BrokeredMaxQuantity;
            newOrder.BuyerCompany = existingOrder.BuyerCompany; //do we need to show anytime, carrier info to Buyer company?
            newOrder.BuyerCompanyId = existingOrder.BuyerCompanyId;
            newOrder.CityGroupTerminalId = existingOrder.CityGroupTerminalId;
            newOrder.DefaultInvoiceType = existingOrder.DefaultInvoiceType;

            newOrder.ExternalBrokerId = existingOrder.ExternalBrokerId;
            newOrder.ExternalMeterServiceId = existingOrder.ExternalMeterServiceId;
            newOrder.IsActive = existingOrder.IsActive;
            newOrder.IsEndSupplier = existingOrder.IsEndSupplier;
            newOrder.IsFTL = existingOrder.IsFTL;
            newOrder.IsProFormaPo = existingOrder.IsProFormaPo;

            OrderXStatus orderStatus = new OrderXStatus();
            orderStatus.StatusId = (int)OrderStatus.Open;
            orderStatus.IsActive = true;
            orderStatus.UpdatedBy = carrierUserContext.Id;
            orderStatus.UpdatedDate = DateTimeOffset.Now;
            newOrder.OrderXStatuses.Add(orderStatus);

            //order.PoNumber = helperDomain.GetPoNumber(fuelRequest, tpoViewModel.AddressDetails.IsProFormaPoEnabled, order.Id);

            newOrder.SignatureEnabled = existingOrder.SignatureEnabled;
            newOrder.TerminalId = existingOrder.TerminalId;
            newOrder.UpdatedBy = carrierUserContext.Id;
            newOrder.UpdatedDate = DateTimeOffset.Now;

            return newOrder;
        }

        public static void SetNewOrderAdditionalDetails(this Order newOrder, Order existingOrder, FuelRequest fuelRequest, UserContext carrierUserContext)
        {
            var orderDetailVersion = new HelperDomain().GetOrderDetailVersion(existingOrder, fuelRequest, carrierUserContext.Id);
            newOrder.OrderDetailVersions.Add(orderDetailVersion);

            //newOrder.DeliveryScheduleXTrackableSchedules
            var schedules = existingOrder.DeliveryScheduleXTrackableSchedules.Where(t => t.Date.Date >= DateTimeOffset.Now.Date && t.IsActive).ToList();
            foreach (var item in schedules)
            {
                newOrder.DeliveryScheduleXTrackableSchedules.Add(new DeliveryScheduleXTrackableSchedule()
                {
                    Date = item.Date,
                    ShiftStartDate = item.Date,
                    DeliveryGroupId = item.DeliveryGroupId,
                    DeliveryScheduleId = item.DeliveryScheduleId,
                    DeliveryScheduleStatusId = item.DeliveryScheduleStatusId,
                    DeliveryStatus = item.DeliveryStatus,
                    DeliveryStatusUpdatedDate = item.DeliveryStatusUpdatedDate,
                    DriverId = item.DriverId,
                    EndTime = item.EndTime,
                    FrDeliveryRequestId = item.FrDeliveryRequestId,
                    IsActive = true,
                    LoadCode = item.LoadCode,
                    OrderId = newOrder.Id,
                    Quantity = item.Quantity,
                    QuantityTypeId = item.QuantityTypeId,
                    StartTime = item.StartTime,
                    SupplierContract = item.SupplierContract,
                    SupplierSourceId = item.SupplierSourceId,
                    UoM = item.UoM
                });
            }

            //newOrder.OrderAdditionalDetail
            var additionalDetails = existingOrder.OrderAdditionalDetail;
            if (additionalDetails != null)
            {
                newOrder.OrderAdditionalDetail = new OrderAdditionalDetail()
                {
                    Allowance = additionalDetails.Allowance,
                    BOLInvoicePreferenceId = additionalDetails.BOLInvoicePreferenceId,
                    CustomAttribute = additionalDetails.CustomAttribute,
                    FileDetails = additionalDetails.FileDetails,
                    FuelSurchagePricingType = additionalDetails.FuelSurchagePricingType,
                    IsDriverToUpdateBOL = additionalDetails.IsDriverToUpdateBOL,
                    IsFuelSurcharge = additionalDetails.IsFuelSurcharge,
                    LoadCode = additionalDetails.LoadCode,
                    OrderId = newOrder.Id,
                    Notes = additionalDetails.Notes,
                    PreferencesSettingId = additionalDetails.PreferencesSettingId,
                    SupplierContract = additionalDetails.SupplierContract,
                    SupplierSourceId = additionalDetails.SupplierSourceId
                };
            }

            //newOrder.OrderDeliverySchedules
            var deliverySchedules = existingOrder.OrderDeliverySchedules.Where(t => t.IsActive && t.DeliverySchedule.Date >= DateTimeOffset.Now.Date).ToList();
            foreach (var item in deliverySchedules)
            {
                newOrder.OrderDeliverySchedules.Add(new OrderVersionXDeliverySchedule()
                {
                    AdditionalNotes = item.AdditionalNotes,
                    CreatedBy = item.CreatedBy,
                    CreatedDate = item.CreatedDate,
                    DeliveryRequestId = item.DeliveryRequestId,
                    IsActive = item.IsActive,
                    Version = orderDetailVersion.Id,
                });
            }


            //newOrder.OrderXDrivers
            var drivers = existingOrder.OrderXDrivers.Where(t => t.IsActive).ToList();
            foreach (var item in drivers)
            {
                newOrder.OrderXDrivers.Add(new OrderXDriver()
                {
                    AssignedBy = item.AssignedBy,
                    AssignedDate = item.AssignedDate,
                    DriverId = item.DriverId,
                    IsActive = item.IsActive,
                    OrderId = newOrder.Id,
                });
            }

            //order.PoNumber = helperDomain.GetPoNumber(fuelRequest, tpoViewModel.AddressDetails.IsProFormaPoEnabled, order.Id);
        }

        public static SourceRegionsViewModel ToSourceRegionsViewModel(this OrderDetailsViewModel response)
        {
            var viewmodel = new SourceRegionsViewModel();
            if (response != null)
            {
                viewmodel.FuelTypeId = response.TfxFuelTypeId;
                viewmodel.JobId = response.JobId;
                if (response.JobLocation !=null)
                {
                    viewmodel.Latitude = response.JobLocation.Latitude;
                    viewmodel.Longitude = response.JobLocation.Longitude;
                }

                viewmodel.PricingCodeId = response.PricingCodeId;
                if (response.FuelDetails != null)
                {
                    if (response.FuelDetails.FuelPricing !=null)
                    {
                        if (response.FuelDetails.FuelPricing.FuelPricingDetails !=null)
                        {
                            viewmodel.PricingSourceId = response.FuelDetails.FuelPricing.FuelPricingDetails.PricingSourceId;
                        }
                    }
                }
                if (response.Country !=null)
                {
                    viewmodel.CountryId = response.Country.Id;
                }
                if (!string.IsNullOrWhiteSpace(response.SourceRegionJsonParameters))
                {
                    SourceRegionJSONParameter sourceRegionsParameters = JsonConvert.DeserializeObject<SourceRegionJSONParameter>(response.SourceRegionJsonParameters);
                    if (sourceRegionsParameters != null)
                    {
                        if (!string.IsNullOrWhiteSpace(sourceRegionsParameters.SourceRegion))
                        {
                            List<int> sourceRegionIds = sourceRegionsParameters.SourceRegion.Split(',').Select(int.Parse).ToList();
                            viewmodel.SelectedSourceRegions = sourceRegionIds;
                        }
                        if (!string.IsNullOrWhiteSpace(sourceRegionsParameters.SelectedTerminals))
                        {
                            List<int> terminalIds = sourceRegionsParameters.SelectedTerminals.Split(',').Select(int.Parse).ToList();
                            viewmodel.SelectedTerminals = terminalIds;
                        }
                        if (!string.IsNullOrWhiteSpace(sourceRegionsParameters.SelectedBulkPlants))
                        {
                            List<int> bulkplantIds = sourceRegionsParameters.SelectedBulkPlants.Split(',').Select(int.Parse).ToList();
                            viewmodel.SelectedBulkPlants = bulkplantIds;
                        }
                    }
                }
                viewmodel.ApprovedTerminalId = response.TerminalId;
                viewmodel.ApprovedTerminal = response.TerminalName;
                viewmodel.ApprovedBulkPlantId = response.BulkPlantId;
                viewmodel.PricingTypeId = response.PricingTypeId;
                viewmodel.OrderId = response.Id;
                viewmodel.FreightPricingMethod = response.OrderAdditionalDetails.FreightPricingMethod;
            }
            return viewmodel;
        }
    }
}
