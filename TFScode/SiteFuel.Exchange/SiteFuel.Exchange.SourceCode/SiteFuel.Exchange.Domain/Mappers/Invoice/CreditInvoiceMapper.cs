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
    public static class CreditInvoiceMapper
    {
        public static Invoice ToCreditInvoice(this CreditInvoiceInputViewModel viewModel, int userId)
        {
            Invoice creditInvoice = new Invoice();
            var currentDate = DateTimeOffset.Now.ToTargetDateTimeOffset(viewModel.Job.TimeZoneName);
            var entity = viewModel.Invoice;
            creditInvoice.OrderId = entity.OrderId;
            creditInvoice.Version = 1;
            creditInvoice.InvoiceVersionStatusId = (int)InvoiceVersionStatus.Active;
            creditInvoice.InvoiceTypeId = (int)InvoiceType.CreditInvoice;
            creditInvoice.DroppedGallons = -1 * entity.DroppedGallons;
            creditInvoice.DropStartDate = entity.DropStartDate;
            creditInvoice.DropEndDate = entity.DropEndDate;
            creditInvoice.PoNumber = entity.PoNumber;
            creditInvoice.StateTax = -1 * entity.StateTax;
            creditInvoice.FedTax = -1 * entity.FedTax;
            creditInvoice.SalesTax = -1 * entity.SalesTax;
            creditInvoice.BasicAmount = -1 * entity.BasicAmount;
            creditInvoice.IsWetHosingDelivery = entity.IsWetHosingDelivery;
            creditInvoice.IsOverWaterDelivery = entity.IsOverWaterDelivery;
            creditInvoice.PaymentTermId = entity.PaymentTermId;
            creditInvoice.NetDays = entity.NetDays;
            creditInvoice.PaymentDueDate = entity.PaymentDueDate;
            creditInvoice.PaymentDate = entity.PaymentDate;
            creditInvoice.CreatedBy = userId;
            creditInvoice.UpdatedBy = userId;
            creditInvoice.CreatedDate = currentDate;
            creditInvoice.IsActive = true;
            creditInvoice.UpdatedDate = currentDate;
            creditInvoice.TotalTaxAmount = -1 * entity.TotalTaxAmount;
            creditInvoice.TransactionId = entity.TransactionId;
            creditInvoice.DriverId = entity.DriverId;
            creditInvoice.TraceId = entity.TraceId;
            creditInvoice.SignatureId = entity.SignatureId;
            creditInvoice.FilePath = entity.FilePath;
            creditInvoice.WaitingFor = entity.WaitingFor;
            creditInvoice.SupplierPreferredInvoiceTypeId = entity.SupplierPreferredInvoiceTypeId;
            creditInvoice.IsBuyPriceInvoice = entity.IsBuyPriceInvoice;
            creditInvoice.TotalFeeAmount = -1 * entity.TotalFeeAmount;
            creditInvoice.BrokeredChainId = entity.BrokeredChainId;
            creditInvoice.BaseDroppedQuntity = -1 * entity.BaseDroppedQuntity;
            creditInvoice.BasePrice = -1 * entity.BasePrice;
            creditInvoice.BaseStateTax = -1 * entity.BaseStateTax;
            creditInvoice.BaseFedTax = -1 * entity.BaseFedTax;
            creditInvoice.BaseSalesTax = -1 * entity.BaseSalesTax;
            creditInvoice.BaseBasicAmount = -1 * entity.BaseBasicAmount;
            creditInvoice.BaseTotalTaxAmount = -1 * entity.BaseTotalTaxAmount;
            creditInvoice.BaseRackPrice = entity.BaseRackPrice;
            creditInvoice.BaseTotalFeeAmount = -1 * entity.BaseTotalFeeAmount;
            creditInvoice.Currency = entity.Currency;
            creditInvoice.ExchangeRate = entity.ExchangeRate;
            creditInvoice.UoM = entity.UoM;
            creditInvoice.TotalDiscountAmount = -1 * entity.TotalDiscountAmount;
            creditInvoice.PDIInvoiceNumber = entity.PDIInvoiceNumber;
            creditInvoice.GrossGallons = -1 * entity.GrossGallons;
            creditInvoice.NetGallons = -1 * entity.NetGallons;
            creditInvoice.QuantityIndicatorTypeId = entity.QuantityIndicatorTypeId;
            creditInvoice.AssetDrops = viewModel.AssetDrops.ToAssetDrops(userId);
            creditInvoice.InvoiceXAdditionalDetail = viewModel.InvoiceXAdditionalDetail.ToInvoiceXAdditionalDetail();
            creditInvoice.InvoiceXAdditionalDetail.OriginalInvoiceId = viewModel.Invoice.Id;
            creditInvoice.InvoiceXAdditionalDetail.CreationMethod = CreationMethod.SFX;
            viewModel.SpecialInstructions.ToList().ForEach(t => creditInvoice.InvoiceXSpecialInstructions.Add(new InvoiceXSpecialInstruction() { SpecialInstructionId = t.SpecialInstructionId, IsInstructionFollowed = t.IsInstructionFollowed }));
            creditInvoice.FuelRequestFees = viewModel.Fees.ToFuelRequestFees();
            creditInvoice.InvoiceDispatchLocation = viewModel.InvoiceDispatchLocation.ToDispatchLocation(currentDate);
            creditInvoice.PaymentDiscounts = viewModel.PaymentDiscounts.ToPaymentDiscounts();
            creditInvoice.TaxDetails = viewModel.TaxDetails.ToTaxDetails();
            creditInvoice.Discounts = viewModel.Discounts.ToDiscounts();
            creditInvoice.ConvertedQuantity = (entity.ConvertedQuantity != null && entity.ConvertedQuantity > 0) ? (-1 * entity.ConvertedQuantity) : entity.ConvertedQuantity;
            creditInvoice.ConvertedPricing = (entity.ConvertedPricing != null && entity.ConvertedPricing > 0) ? (-1 * entity.ConvertedPricing) : entity.ConvertedPricing;
            InvoiceXInvoiceStatusDetail statusDetail = new InvoiceXInvoiceStatusDetail();
            statusDetail.IsActive = true;
            statusDetail.StatusId = (int)InvoiceStatus.Received;
            statusDetail.UpdatedBy = userId;
            statusDetail.UpdatedDate = currentDate;
            creditInvoice.InvoiceXInvoiceStatusDetails.Add(statusDetail);
            
            if (viewModel.IsMarineLocation && viewModel.BDRDetails !=null)
            {
                creditInvoice.BDRDetail = viewModel.BDRDetails;
            }
            

            return creditInvoice;
        }

        public static Invoice ToPartialCreditEntity(this InvoiceModel viewModel, DateTimeOffset currentDate, Invoice entity = null)
        {
            if (entity == null)
            {
                entity = new Invoice();
            }
            entity.Version = 1;
            entity.OrderId = viewModel.OrderId;
            entity.InvoiceVersionStatusId = viewModel.InvoiceVersionStatusId;
            entity.Version = viewModel.Version;
            entity.InvoiceTypeId = viewModel.InvoiceTypeId;
            entity.DroppedGallons = -1 * viewModel.DroppedGallons;
            entity.DropStartDate = viewModel.DropStartDate;
            entity.DropEndDate = viewModel.DropEndDate;
            entity.PoNumber = viewModel.PoNumber;
            entity.StateTax = -1 * viewModel.StateTax;
            entity.FedTax = -1 * viewModel.FedTax;
            entity.SalesTax = -1 * viewModel.SalesTax;
            entity.BasicAmount = -1 * viewModel.BasicAmount;
            entity.IsOverWaterDelivery = viewModel.IsOverWaterDelivery;
            entity.IsWetHosingDelivery = viewModel.IsWetHosingDelivery;
            entity.PaymentTermId = viewModel.PaymentTermId;
            entity.NetDays = viewModel.NetDays;
            entity.PaymentDueDate = viewModel.PaymentDueDate;
            entity.PaymentDate = viewModel.PaymentDate;
            entity.ParentId = viewModel.ParentId;
            entity.CreatedBy = viewModel.CreatedBy;
            entity.CreatedDate = currentDate;
            entity.IsActive = viewModel.IsActive;
            entity.UpdatedBy = viewModel.UpdatedBy;
            entity.UpdatedDate = currentDate;
            entity.TotalTaxAmount = viewModel.TotalTaxAmount;
            entity.TransactionId = viewModel.TransactionId;
            entity.TraceId = viewModel.TraceId;
            entity.FilePath = viewModel.FilePath;
            entity.WaitingFor = (int)viewModel.WaitingFor;
            entity.SupplierPreferredInvoiceTypeId = viewModel.SupplierPreferredInvoiceTypeId;
            entity.IsBuyPriceInvoice = viewModel.IsBuyPriceInvoice;
            entity.TotalFeeAmount = viewModel.TotalFeeAmount;
            entity.BrokeredChainId = viewModel.BrokeredChainId;
            entity.QuantityIndicatorTypeId = viewModel.QuantityIndicatorTypeId;
            entity.BaseDroppedQuntity = -1 * viewModel.BaseDroppedQuntity;
            entity.BasePrice = -1 * viewModel.BasePrice;
            entity.BaseStateTax = -1 * viewModel.BaseStateTax;
            entity.BaseFedTax = -1 * viewModel.BaseFedTax;
            entity.BaseStateTax = -1 * viewModel.BaseSalesTax;
            entity.BaseBasicAmount = -1 * viewModel.BaseBasicAmount;
            entity.BaseTotalTaxAmount = -1 * viewModel.BaseTotalTaxAmount;
            entity.BaseRackPrice = viewModel.BaseRackPrice;
            entity.BaseTotalFeeAmount = -1 * viewModel.BaseTotalFeeAmount;
            entity.Currency = viewModel.Currency;
            entity.ExchangeRate = viewModel.ExchangeRate;
            entity.UoM = viewModel.UoM;
            entity.TotalDiscountAmount = -1 * viewModel.TotalDiscountAmount;
            entity.ConvertedQuantity = (viewModel.ConvertedQuantity != null && viewModel.ConvertedQuantity > 0) ? (- 1 * viewModel.ConvertedQuantity) : viewModel.ConvertedQuantity;
            entity.ConvertedPricing = (viewModel.ConvertedPricing != null && viewModel.ConvertedPricing > 0)  ? (-1 * viewModel.ConvertedPricing) : viewModel.ConvertedPricing;

            entity.InvoiceXInvoiceStatusDetails.ToList().ForEach(t => t.IsActive = false);
            InvoiceXInvoiceStatusDetail invoiceStatus = new InvoiceXInvoiceStatusDetail
            {
                StatusId = (int)InvoiceStatus.Received,
                UpdatedBy = viewModel.UpdatedBy,
                UpdatedDate = viewModel.UpdatedDate,
                IsActive = true
            };
            entity.InvoiceXInvoiceStatusDetails.Add(invoiceStatus);
            entity.InvoiceXAdditionalDetail = viewModel.AdditionalDetail.ToEntity(entity.InvoiceXAdditionalDetail);
            entity.InvoiceXAdditionalDetail.CreationMethod = viewModel.CreationMethod;
            entity.FuelRequestFees = viewModel.FuelFees.Select(t => t.ToEntity()).ToList();
            if (viewModel.TaxDetails != null && viewModel.TaxDetails.AvaTaxDetails.Any())
            {
                entity.TaxDetails = viewModel.TaxDetails.ToEntity();
            }

            return entity;
        }

        public static InvoiceHeaderDetail ToPartialCreditInvoiceHeaderEntity(this InvoiceModel invoice, InvoiceHeaderDetail entity = null)
        {
            if (entity == null)
            {
                entity = new InvoiceHeaderDetail();
            }
            entity.InvoiceNumberId = invoice.InvoiceNumberId;
            entity.TotalDroppedGallons = -1 * invoice.DroppedGallons;
            entity.TotalBasicAmount = -1 * invoice.BasicAmount;
            entity.TotalFeeAmount = invoice.TotalFeeAmount ?? 0;
            entity.TotalTaxAmount = invoice.TotalTaxAmount;
            entity.TotalDiscountAmount = -1 * invoice.TotalDiscountAmount;
            entity.Version = 1;
            entity.CreatedBy = entity.UpdatedBy = invoice.CreatedBy;
            entity.CreatedDate = entity.UpdatedDate = invoice.CreatedDate;
            entity.IsActive = true;            
            return entity;            
        }

        public static ICollection<AssetDrop> ToAssetDrops(this ICollection<AssetDrop> entity, int userId)
        {
            List<AssetDrop> assetDrops = new List<AssetDrop>();
            foreach (var assetdrop in entity.Where(t => t.IsActive))
            {
                AssetDrop drop = new AssetDrop();
                drop.JobXAssetId = assetdrop.JobXAssetId;
                drop.OrderId = assetdrop.OrderId;
                drop.MeterStartReading = assetdrop.MeterStartReading;
                drop.MeterEndReading = assetdrop.MeterEndReading;
                drop.DroppedGallons = -1 * assetdrop.DroppedGallons;
                drop.DropStartDate = assetdrop.DropStartDate;
                drop.DropEndDate = assetdrop.DropEndDate;
                drop.DroppedBy = assetdrop.DroppedBy;
                drop.IsActive = true;
                drop.UpdatedDate = DateTimeOffset.Now;
                drop.UpdatedBy = userId;
                drop.ImageId = assetdrop.ImageId;
                drop.SubcontractorName = assetdrop.SubcontractorName;
                drop.ContractNumber = assetdrop.ContractNumber;
                drop.DropStatus = assetdrop.DropStatus;
                assetDrops.Add(drop);
            }

            return assetDrops;
        }

        public static BDRDetail ToBDREntity(this BDRDetail entity, InvoiceNumber invoiceNumber, string firstOrderId)
        {
            BDRDetail BdrDetails = new BDRDetail();
            BdrDetails.BDRNumber = firstOrderId;/*ApplicationConstants.TFBD + invoiceNumber.Id;*/
            BdrDetails.CloseMeterReading = entity.CloseMeterReading;
            BdrDetails.DensityInVaccum = entity.DensityInVaccum;
            BdrDetails.FlashPoint = entity.FlashPoint;
            BdrDetails.IsEngineerInvitedToWitnessSample = entity.IsEngineerInvitedToWitnessSample;
            BdrDetails.IsNoticeToProtestIssued = entity.IsNoticeToProtestIssued;
            BdrDetails.MarpolSampleNumbers = entity.MarpolSampleNumbers;
            BdrDetails.MVMarpolSampleNumbers = entity.MVMarpolSampleNumbers;
            BdrDetails.MeasuredVolume = entity.MeasuredVolume;
            BdrDetails.ObservedTemperature = entity.ObservedTemperature;
            BdrDetails.OpenMeterReading = entity.OpenMeterReading;
            BdrDetails.PumpingStartTime = entity.PumpingStartTime;
            BdrDetails.PumpingStopTime = entity.PumpingStopTime;
            BdrDetails.StandardVolume = string.IsNullOrWhiteSpace(entity.StandardVolume) ? entity.MeasuredVolume : entity.StandardVolume;
            BdrDetails.SulphurContent = entity.SulphurContent;
            BdrDetails.Viscosity = entity.Viscosity;
            BdrDetails.IsActive = true;
            return BdrDetails;
        }

        public static InvoiceFtlDetail ToCreditPartialEntity(this BolDetailViewModel viewModel, decimal droppedGallons)
        {
            var entity = new InvoiceFtlDetail
            {
                CreatedBy = viewModel.CreatedBy,
                CreatedDate = viewModel.CreatedDate,
                GrossQuantity = droppedGallons,
                NetQuantity = droppedGallons,
                BolNumber = viewModel.BolNumber,
                Carrier = viewModel.Carrier,
                PricePerGallon = viewModel.PricePerGallon,
                RackPrice = viewModel.RackPrice,
                TerminalId = viewModel.TerminalId,
                CityGroupTerminalId = viewModel.CityGroupTerminalId
            };
            return entity;
        }

        public static InvoiceFtlDetail ToBolDetails(this InvoiceFtlDetail entity, DateTimeOffset currentDate)
        {
            if (entity == null)
                return null;
            InvoiceFtlDetail invoiceFtlDetail = new InvoiceFtlDetail();
            invoiceFtlDetail.BolNumber = entity.BolNumber;
            invoiceFtlDetail.Carrier = entity.Carrier;
            invoiceFtlDetail.CreatedBy = entity.CreatedBy;
            invoiceFtlDetail.CreatedDate = currentDate;
            invoiceFtlDetail.GrossQuantity = entity.GrossQuantity;
            invoiceFtlDetail.ImageId = entity.ImageId;
            invoiceFtlDetail.NetQuantity = entity.NetQuantity;
            return invoiceFtlDetail;
        }

        public static InvoiceXAdditionalDetail ToInvoiceXAdditionalDetail(this InvoiceXAdditionalDetail entity)
        {

            InvoiceXAdditionalDetail additionalDetail = new InvoiceXAdditionalDetail();
            additionalDetail.Latitude = entity.Latitude;
            additionalDetail.Longitude = entity.Longitude;
            additionalDetail.AssignedBy = entity.AssignedBy;
            additionalDetail.AssignedDate = entity.AssignedDate;
            additionalDetail.DriverComment = entity.DriverComment;
            additionalDetail.AssetFilled = -1 * entity.AssetFilled;
            additionalDetail.PoContactName = entity.PoContactName;
            additionalDetail.PoContactEmail = entity.PoContactEmail;
            additionalDetail.PoContactPhoneNumber = entity.PoContactPhoneNumber;
            additionalDetail.PoContactId = entity.PoContactId;
            additionalDetail.JobId = entity.JobId;
            additionalDetail.DisplayJobID = entity.DisplayJobID;
            additionalDetail.CustomAttribute = entity.CustomAttribute;
            additionalDetail.JobName = entity.JobName;
            additionalDetail.JobAddress = entity.JobAddress;
            additionalDetail.JobCity = entity.JobCity;
            additionalDetail.JobStateCode = entity.JobStateCode;
            additionalDetail.JobStateName = entity.JobStateName;
            additionalDetail.JobCountryCode = entity.JobCountryCode;
            additionalDetail.JobCountryName = entity.JobCountryName;
            additionalDetail.JobZipCode = entity.JobZipCode;
            additionalDetail.BillingAddessId = entity.BillingAddessId;
            additionalDetail.BillingAddress = entity.BillingAddress;
            additionalDetail.BillingCity = entity.BillingCity;
            additionalDetail.BillingStateCode = entity.BillingStateCode;
            additionalDetail.BillingStateName = entity.BillingStateName;
            additionalDetail.BillingCountryCode = entity.BillingCountryCode;
            additionalDetail.BillingCountryName = entity.BillingCountryName;
            additionalDetail.BillingZipCode = entity.BillingZipCode;
            additionalDetail.CxmlCheckOutDate = entity.CxmlCheckOutDate;
            additionalDetail.Notes = entity.Notes;
            additionalDetail.TankFrequencyId = entity.TankFrequencyId;
            additionalDetail.IsSurchargeApplicable = entity.IsSurchargeApplicable;
            additionalDetail.SurchargePricingType = entity.SurchargePricingType;
            additionalDetail.SurchargePercentage = entity.SurchargePercentage;
            additionalDetail.SurchargeEIAPrice = entity.SurchargeEIAPrice;
            additionalDetail.SurchargeTableRangeStart = entity.SurchargeTableRangeStart;
            additionalDetail.SurchargeTableRangeEnd = entity.SurchargeTableRangeEnd;
            additionalDetail.Distance = entity.Distance;
            additionalDetail.SupplierAllowance = entity.SupplierAllowance;
            additionalDetail.TotalAllowance = -1 * entity.TotalAllowance;


            return additionalDetail;
        }

        public static ICollection<FuelFee> ToFuelRequestFees(this ICollection<FuelFee> entity)
        {
            List<FuelFee> fuelFees = new List<FuelFee>();
            foreach (var fee in entity)
            {
                FuelFee fuelFee = new FuelFee();
                fuelFee.FeeTypeId = fee.FeeTypeId;
                fuelFee.FeeSubTypeId = fee.FeeSubTypeId;
                fuelFee.MinimumGallons = fee.MinimumGallons;
                fuelFee.Fee = fee.Fee;
                fuelFee.FeeDetails = fee.FeeDetails;
                fuelFee.MarginTypeId = fee.MarginTypeId;
                fuelFee.Margin = fee.Margin;
                fuelFee.IncludeInPPG = fee.IncludeInPPG;
                fuelFee.InvoiceId = fee.InvoiceId;
                fuelFee.FeeSubQuantity = -1 * fee.FeeSubQuantity;
                fuelFee.TotalFee = -1 * fee.TotalFee;
                fuelFee.OtherFeeTypeId = fee.OtherFeeTypeId;
                fuelFee.FeeConstraintTypeId = fee.FeeConstraintTypeId;
                fuelFee.TaxDetailId = fee.TaxDetailId;
                fuelFee.SpecialDate = fee.SpecialDate;
                fuelFee.Currency = fee.Currency;
                fuelFee.UoM = fee.UoM;
                fuelFee.OfferPricingId = fee.OfferPricingId;
                fuelFee.DiscountLineItemId = fee.DiscountLineItemId;
                fuelFee.WaiveOffTime = fee.WaiveOffTime;
                fuelFee.StartTime = fee.StartTime;
                fuelFee.EndTime = fee.EndTime;
                foreach (var byQuantity in fee.FeeByQuantities)
                {
                    var feeByQty = new FeeByQuantity();
                    feeByQty.Currency = byQuantity.Currency;
                    feeByQty.Fee = byQuantity.Fee;
                    feeByQty.FeeSubTypeId = byQuantity.FeeSubTypeId;
                    feeByQty.FeeTypeId = byQuantity.FeeTypeId;
                    feeByQty.MaxQuantity = byQuantity.MaxQuantity;
                    feeByQty.MinQuantity = byQuantity.MinQuantity;
                    feeByQty.UoM = byQuantity.UoM;
                    fuelFee.FeeByQuantities.Add(feeByQty);
                }
                fuelFees.Add(fuelFee);
            }

            return fuelFees;
        }

        public static ICollection<InvoiceDispatchLocation> ToDispatchLocation(this ICollection<InvoiceDispatchLocation> entity, DateTimeOffset currentDate)
        {
            List<InvoiceDispatchLocation> locations = new List<InvoiceDispatchLocation>();
            foreach (var location in entity)
            {
                InvoiceDispatchLocation dispatchLocation = new InvoiceDispatchLocation();
                dispatchLocation.LocationType = location.LocationType;
                dispatchLocation.OrderId = location.OrderId;
                dispatchLocation.Address = location.Address;
                dispatchLocation.City = location.City;
                dispatchLocation.StateCode = location.StateCode;
                dispatchLocation.StateId = location.StateId;
                dispatchLocation.ZipCode = location.ZipCode;
                dispatchLocation.CountryCode = location.CountryCode;
                dispatchLocation.Latitude = location.Latitude;
                dispatchLocation.Longitude = location.Longitude;
                dispatchLocation.CreatedDate = currentDate;
                dispatchLocation.CreatedBy = location.CreatedBy;
                dispatchLocation.CountyName = location.CountyName;
                dispatchLocation.SiteName = location.SiteName;
                locations.Add(dispatchLocation);
            }

            return locations;
        }

        public static ICollection<PaymentDiscount> ToPaymentDiscounts(this ICollection<PaymentDiscount> entity)
        {
            List<PaymentDiscount> paymentDiscounts = new List<PaymentDiscount>();
            foreach (var discount in entity)
            {
                PaymentDiscount paymentDiscount = new PaymentDiscount();
                paymentDiscount.DiscountPercentage = discount.DiscountPercentage;
                paymentDiscount.FuelRequestId = discount.FuelRequestId;
                paymentDiscount.WithInDays = discount.WithInDays;

                paymentDiscounts.Add(paymentDiscount);
            }

            return paymentDiscounts;
        }

        public static ICollection<TaxDetail> ToTaxDetails(this ICollection<TaxDetail> entity)
        {
            List<TaxDetail> taxes = new List<TaxDetail>();
            foreach (var tax in entity)
            {
                TaxDetail taxDetail = new TaxDetail();
                taxDetail.ProductCategory = tax.ProductCategory;
                taxDetail.TaxingLevel = tax.TaxingLevel;
                taxDetail.TaxType = tax.TaxType;
                taxDetail.RateType = tax.RateType;
                taxDetail.RateSubType = tax.RateSubType;
                taxDetail.CalculationTypeInd = tax.CalculationTypeInd;
                taxDetail.TaxRate = tax.TaxRate;
                taxDetail.TaxAmount = tax.TaxAmount;
                taxDetail.IsModified = tax.IsModified;
                taxDetail.LicenseId = tax.LicenseId;
                taxDetail.LicenseNumber = tax.LicenseNumber;
                taxDetail.UnitOfMeasure = tax.UnitOfMeasure;
                taxDetail.Currency = tax.Currency;
                taxDetail.RateDescription = tax.RateDescription;
                taxDetail.RelatedLineItem = tax.RelatedLineItem;
                taxDetail.SalesTaxBaseAmount = tax.SalesTaxBaseAmount;
                taxDetail.TaxExemptionInd = tax.TaxExemptionInd;
                taxDetail.TaxPricingTypeId = tax.TaxPricingTypeId;
                taxDetail.TradingTaxAmount = -1 * tax.TradingTaxAmount;
                taxDetail.TradingCurrency = tax.TradingCurrency;
                taxDetail.ExchangeRate = tax.ExchangeRate;
                taxes.Add(taxDetail);
            }

            return taxes;
        }

        public static ICollection<Discount> ToDiscounts(this ICollection<Discount> entity)
        {
            List<Discount> discounts = new List<Discount>();
            foreach (var discount in entity)
            {
                Discount discnt = new Discount();
                discnt.DealName = discount.DealName;
                discnt.DealStatus = discount.DealStatus;
                discnt.CreatedBy = discount.CreatedBy;
                discnt.CreatedDate = DateTimeOffset.Now;
                discnt.CreatedCompanyId = discount.CreatedCompanyId;
                discnt.StatusChangedBy = discount.StatusChangedBy;
                discnt.StatusChangedDate = discount.StatusChangedDate;
                discnt.StatusChangedCompanyId = discount.StatusChangedCompanyId;
                discnt.OrderId = discount.OrderId;
                discnt.Notes = discount.Notes;
                foreach (var lineitem in discount.DiscountLineItems)
                {
                    DiscountLineItem item = new DiscountLineItem();
                    item.Amount = -1 * lineitem.Amount;
                    item.FeeDetails = lineitem.FeeDetails;
                    item.FeeSubTypeId = lineitem.FeeSubTypeId;
                    item.FeeTypeId = lineitem.FeeTypeId;
                    discnt.DiscountLineItems.Add(item);
                }
                discounts.Add(discnt);
            }

            return discounts;
        }
    }
}
