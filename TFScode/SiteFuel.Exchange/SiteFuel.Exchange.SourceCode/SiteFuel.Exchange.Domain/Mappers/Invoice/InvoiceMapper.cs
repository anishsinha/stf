using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Invoice.Pdf;
using SiteFuel.Exchange.ViewModels.MobileAPI.TPD;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class InvoiceMapper
    {
        public static InvoiceViewModel ToViewModel(this Invoice entity, InvoiceViewModel viewModel = null)
        {
            if (viewModel == null)
            {
                viewModel = new InvoiceViewModel(Status.Success);
            }

            viewModel.Id = entity.Id;
            viewModel.Version = entity.Version;
            viewModel.OrderId = entity.OrderId;
            viewModel.InvoiceNumber.Id = entity.InvoiceHeader.InvoiceNumberId;
            viewModel.InvoiceNumber.Number = entity.DisplayInvoiceNumber;
            viewModel.InvoiceHeaderId = entity.InvoiceHeaderId;
            viewModel.InvoiceTypeId = entity.InvoiceTypeId;
            viewModel.DroppedGallons = entity.DroppedGallons;
            viewModel.DropStartDate = entity.DropStartDate;
            viewModel.DropEndDate = entity.DropEndDate;
            viewModel.StateTax = entity.StateTax;
            viewModel.FederalTax = entity.FedTax;
            viewModel.SalesTax = entity.SalesTax;
            viewModel.BasicAmount = entity.BasicAmount;
            viewModel.PaymentDueDate = entity.PaymentDueDate;
            viewModel.PaymentDate = entity.PaymentDate;
            viewModel.IsOverWaterDelivery = entity.IsOverWaterDelivery;
            viewModel.IsWetHosingDelivery = entity.IsWetHosingDelivery;
            viewModel.InvoiceVersionStatusId = entity.InvoiceVersionStatusId;
            viewModel.DisplayInvoiceNumber = entity.DisplayInvoiceNumber;
            viewModel.ReferenceId = entity.ReferenceId;
            viewModel.QbInvoiceNumber = entity.QbInvoiceNumber;
            var status = entity.InvoiceXInvoiceStatusDetails.FirstOrDefault(t => t.IsActive);
            viewModel.StatusId = status.StatusId;
            if (status.MstInvoiceStatus != null)
            {
                viewModel.StatusName = status.MstInvoiceStatus.Name;
            }
            viewModel.CreatedBy = entity.CreatedBy;
            viewModel.CreatedDate = entity.CreatedDate;
            viewModel.SpecialInstructions = entity.InvoiceXSpecialInstructions.Select(t => t.ToViewModel()).ToList();

            if (entity.InvoiceXAdditionalDetail != null)
            {
                viewModel.AdditionalDetail = entity.InvoiceXAdditionalDetail.ToViewModel();
            }

            if (entity.TaxDetails != null && entity.TaxDetails.Count > 0)
            {
                viewModel.TaxDetails = entity.TaxDetails.Where(t => t.TaxExemptionInd != ApplicationConstants.AvaTaxExemptedInd).ToList().ToViewModel();
            }

            viewModel.TotalTaxAmount = entity.TotalTaxAmount;
            viewModel.TotalDiscountAmount = entity.TotalDiscountAmount;
            viewModel.TransactionId = entity.TransactionId;
            viewModel.TraceId = entity.TraceId;

            if (entity.ImageId.HasValue)
            {
                viewModel.Image = new ImageViewModel()
                {
                    Id = entity.ImageId.Value
                };
            }

            viewModel.CsvFilePath = entity.FilePath;
            viewModel.WaitingForAction = entity.WaitingFor;
            viewModel.IsBuyPriceInvoice = entity.IsBuyPriceInvoice;
            viewModel.IsActive = entity.IsActive;
            viewModel.UpdatedBy = entity.UpdatedBy;
            viewModel.UpdatedDate = entity.UpdatedDate;
            var bolDetail = entity.InvoiceXBolDetails.Select(t2 => t2.InvoiceFtlDetail).FirstOrDefault();
            if (bolDetail != null)
            {
                viewModel.PricePerGallon = bolDetail.PricePerGallon;
                viewModel.TerminalId = bolDetail.TerminalId;
                viewModel.CityGroupTerminalId = bolDetail.CityGroupTerminalId;
                if (bolDetail.CityGroupTerminalId.HasValue && bolDetail.CityGroupTerminalId.Value > 0)
                {
                    var terminal = ContextFactory.Current.GetDomain<HelperDomain>().GetExternalTerminal(bolDetail.CityGroupTerminalId.Value);
                    if (terminal != null)
                    {
                        viewModel.CityGroupTerminalName = $"{terminal.Name}, {terminal.StateCode}";
                    }
                }
            }
            viewModel.Currency = entity.Currency;
            viewModel.UoM = entity.UoM;
            viewModel.DDTConversionReason = entity.DDTConversionReason;
            viewModel.IsBOLImageReq = entity.IsBolImageReq;
            viewModel.IsDropImageReq = entity.IsDropImageReq;
            viewModel.IsSignatureReq = entity.IsSignatureReq;
            return viewModel;
        }

        public static InvoiceViewModel ToViewModel(this UspGetSupplierInvoiceDetails entity)
        {
            var viewModel = new InvoiceViewModel(Status.Success);

            viewModel.Id = entity.Id;
            viewModel.OrderId = entity.OrderId;
            viewModel.InvoiceNumber.Number = entity.DisplayInvoiceNumber;
            viewModel.InvoiceTypeId = entity.InvoiceTypeId;
            viewModel.DroppedGallons = entity.DroppedGallons;
            viewModel.PricePerGallon = entity.PricePerGallon;
            viewModel.DropStartDate = entity.DropStartDate;
            viewModel.DropEndDate = entity.DropEndDate;
            viewModel.StateTax = entity.StateTax;
            viewModel.FederalTax = entity.FedTax;
            viewModel.SalesTax = entity.SalesTax;
            viewModel.BasicAmount = entity.BasicAmount;
            viewModel.PaymentDueDate = entity.PaymentDueDate;
            viewModel.PaymentDate = entity.PaymentDate;
            viewModel.InvoiceVersionStatusId = entity.InvoiceVersionStatusId;
            viewModel.DisplayInvoiceNumber = entity.DisplayInvoiceNumber;
            viewModel.ReferenceId = entity.ReferenceId;
            viewModel.StatusId = entity.StatusId;
            viewModel.StatusName = entity.StatusName;
            viewModel.CityGroupTerminalId = entity.CityGroupTerminalId;
            viewModel.CreatedDate = entity.CreatedDate;
            viewModel.TotalTaxAmount = entity.TotalTaxAmount;
            viewModel.TotalDiscountAmount = entity.TotalDiscountAmount;
            viewModel.TotalFees = entity.TotalFeeAmount;
            viewModel.IsApprovalWorkflowEnabledForJob = entity.IsApprovalWorkflowEnabled ?? false;
            viewModel.InvoiceHeaderId = entity.InvoiceHeaderId;
            viewModel.BrokeredChainId = entity.BrokeredChainId;
            //if (entity.ImageId.HasValue)
            //{
            //    viewModel.Image = new ImageViewModel()
            //    {
            //        Id = entity.ImageId.Value,

            //    };
            //}

            viewModel.CsvFilePath = entity.FilePath;
            viewModel.WaitingForAction = entity.WaitingFor;
            viewModel.IsBuyPriceInvoice = entity.IsBuyPriceInvoice;
            if (entity.CityGroupTerminalId.HasValue && entity.CityGroupTerminalId.Value > 0)
            {
                viewModel.CityGroupTerminalName = entity.CityGroupTerminalName;
            }
            viewModel.Currency = entity.Currency;
            viewModel.UoM = entity.UoM;
            viewModel.IsPendingInvoiceAdjustment = entity.IsPendingInvoiceAdjustment;
            viewModel.IsSupplierPreferenceDDT = entity.SupplierPreferredInvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual || entity.SupplierPreferredInvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp;
            viewModel.TimeZoneName = entity.OrderId == null ? string.Empty : entity.TimeZoneName;
            if (!string.IsNullOrWhiteSpace(entity.Notes))
            {
                viewModel.AdditionalDetail = new InvoiceXAdditionalDetailViewModel() { Notes = entity.Notes };
            }

            if (viewModel.AdditionalDetail == null)
                viewModel.AdditionalDetail = new InvoiceXAdditionalDetailViewModel();
            viewModel.AdditionalDetail.CreationMethod = entity.CreationMethod;
            viewModel.AdditionalDetail.OriginalInvoiceId = entity.OriginalInvoiceId;
            viewModel.AdditionalDetail.CarrierOrderId = entity.CarrierOrderId;
            viewModel.DDTConversionReason = entity.DDTConversionReason;
            viewModel.IsExceptionDdt = entity.IsExceptionDdt;
            viewModel.IsBDNConfirmationRequired = entity.IsBDNConfirmationRequired;
            viewModel.IsInvoiceConfirmationRequired = entity.IsInvoiceConfirmationRequired;
            if (entity.UoM == UoM.Barrels || entity.UoM == UoM.MetricTons)
            {
                viewModel.ConvertedQuantity = entity.ConvertedQuantity;
            }
            viewModel.LiftStartTime = entity.LiftStartTime;
            viewModel.LiftEndTime = entity.LiftEndTime;

            return viewModel;
        }

        public static InvoiceViewModel ToInvoiceViewModel(this UspInvoicePdfDetail entity, InvoiceViewModel viewModel = null)
        {
            if (viewModel == null)
            {
                viewModel = new InvoiceViewModel(Status.Success);
            }
            viewModel.Id = entity.Id;
            viewModel.Version = entity.Version;
            viewModel.OrderId = entity.OrderId;
            viewModel.PoNumber = entity.PoNumber;
            viewModel.InvoiceNumber.Id = entity.InvoiceNumberId;
            viewModel.InvoiceNumber.Number = entity.DisplayInvoiceNumber;
            viewModel.InvoiceTypeId = entity.InvoiceTypeId;
            viewModel.DroppedGallons = entity.DroppedGallons;
            viewModel.PricePerGallon = entity.PricePerGallon;
            viewModel.DropStartDate = entity.DropStartDate;
            viewModel.DropEndDate = entity.DropEndDate;
            viewModel.StateTax = entity.StateTax;
            viewModel.FederalTax = entity.FedTax;
            viewModel.SalesTax = entity.SalesTax;
            viewModel.BasicAmount = entity.BasicAmount;
            viewModel.TotalFees = entity.TotalFeeAmount;
            viewModel.TotalTaxAmount = entity.TotalTaxAmount;
            viewModel.TotalDiscountAmount = entity.TotalDiscountAmount;
            viewModel.PaymentDueDate = entity.PaymentDueDate;
            viewModel.PaymentDate = entity.PaymentDate;
            viewModel.IsOverWaterDelivery = entity.IsOverWaterDelivery;
            viewModel.IsWetHosingDelivery = entity.IsWetHosingDelivery;
            viewModel.InvoiceVersionStatusId = entity.InvoiceVersionStatusId;
            viewModel.DisplayInvoiceNumber = entity.DisplayInvoiceNumber;
            viewModel.QbInvoiceNumber = entity.QbInvoiceNumber;
            viewModel.CreatedDate = entity.CreatedDate;
            viewModel.DisplayJobID = entity.DisplayJobID;
            viewModel.CreationMethod = (CreationMethod)entity.CreationMethod;

            viewModel.AdditionalDetail = new InvoiceXAdditionalDetailViewModel()
            {
                BillingAddress = entity.BillingAddress,
                BillingCity = entity.BillingCity,
                BillingStateCode = entity.BillingStateCode,
                BillingZipCode = entity.BillingZipCode,
                JobName = entity.JobName,
                DisplayJobID = entity.DisplayJobID,
                JobAddress = entity.JobAddress,
                JobAddressLine2 = entity.JobAddressLine2,
                JobAddressLine3 = entity.JobAddressLine3,
                JobCity = entity.JobCity,
                JobStateCode = entity.JobStateCode,
                JobZipCode = entity.JobZipCode,
                Notes = entity.Notes,
                DropTicketNumber = entity.DropTicketNumber,
                TotalAllowance = entity.TotalAllowance ?? 0,
                IsJobSpecificBillToEnabled = entity.IsBillToEnabled,
                BillToAddress = entity.BillToAddress,
                BillToCity = entity.BillToCity,
                BillToCounty = entity.BillToCounty,
                BillToZipCode = entity.BillToZipCode,
                BillToStateCode = entity.BillToStateCode,
                BillToStateName = entity.BillToStateName,
                BillToCountryCode = entity.BillToCountryCode,
                BillToCountryName = entity.BillToCountryName,
                BillToName = entity.BillToName,
                CarrierOrderId = entity.CarrierOrderId
            };

            if (entity.ImageId.HasValue)
            {
                viewModel.Image = new ImageViewModel()
                {
                    Id = entity.ImageId.Value
                };
            }
            viewModel.WaitingForAction = entity.WaitingFor;
            viewModel.IsBuyPriceInvoice = entity.IsBuyPriceInvoice;
            viewModel.UpdatedDate = entity.UpdatedDate;
            viewModel.Currency = (Currency)entity.Currency;
            viewModel.UoM = (UoM)entity.UoM;

            viewModel.BolDetails = new BolDetailViewModel()
            {
                BolNumber = entity.BolNumber,
                Carrier = entity.Carrier,
                NetQuantity = entity.NetQuantity,
                GrossQuantity = entity.GrossQuantity,
                LiftTicketNumber = entity.LiftTicketNumber,
                LiftQuantity = entity.LiftQuantity
            };
            viewModel.IsVariousFobOrigin = entity.IsVariousFobOrigin;
            viewModel.IsDropLocationAvailable = entity.IsDropLocationAvailable;

            viewModel.WBSNumber = entity.WBSNumber;
            viewModel.IsTrueFillTax = entity.IsTrueFillTax;
            viewModel.IsApprovalWorkflowEnabledForJob = entity.IsApprovalWorkflowEnabled;
            viewModel.CxmlCheckOutDate = entity.CxmlCheckOutDate;
            viewModel.StatementNumber = entity.StatementNumber;
            viewModel.StatementId = entity.StatementId;
            viewModel.IsExceptionDdt = entity.IsExceptionDdt;


            return viewModel;
        }

        public static ConsolidatedInvoiceViewModel ToInvoiceViewModel(this UspPdfDetail entity, CompanyType companyType, ConsolidatedInvoiceViewModel viewModel = null)
        {
            if (viewModel == null)
            {
                viewModel = new ConsolidatedInvoiceViewModel(Status.Success);
            }
            viewModel.Id = entity.Id;
            viewModel.Version = entity.Version;
            viewModel.OrderId = entity.OrderId;
            viewModel.PoNumber = entity.PoNumber;
            viewModel.InvoiceNumber.Id = entity.InvoiceNumberId;
            viewModel.InvoiceNumber.Number = entity.DisplayInvoiceNumber;
            viewModel.InvoiceTypeId = entity.InvoiceTypeId;
            viewModel.DroppedGallons = entity.DroppedGallons;
            viewModel.PricePerGallon = entity.PricePerGallon ?? 0;
            viewModel.PricePerGallonDisplay = entity.FuelRequestPPG;
            viewModel.DropStartDate = entity.DropStartDate;
            viewModel.DropEndDate = entity.DropEndDate;
            viewModel.StateTax = entity.StateTax;
            viewModel.FederalTax = entity.FedTax;
            viewModel.SalesTax = entity.SalesTax;
            viewModel.BasicAmount = entity.BasicAmount;
            viewModel.TotalFees = entity.TotalFeeAmount;
            viewModel.TotalTaxAmount = entity.TotalTaxAmount;
            viewModel.TotalDiscountAmount = entity.TotalDiscountAmount;
            viewModel.PaymentDueDate = entity.PaymentDueDate;
            viewModel.PaymentDate = entity.PaymentDate;
            viewModel.IsOverWaterDelivery = entity.IsOverWaterDelivery;
            viewModel.IsWetHosingDelivery = entity.IsWetHosingDelivery;
            viewModel.InvoiceVersionStatusId = entity.InvoiceVersionStatusId;
            viewModel.StatusId = entity.StatusId;
            viewModel.DisplayInvoiceNumber = entity.DisplayInvoiceNumber;
            viewModel.ReferenceId = entity.ReferenceId;
            viewModel.QbInvoiceNumber = entity.QbInvoiceNumber;
            viewModel.CreatedDate = entity.CreatedDate;
            viewModel.OriginalInvoiceNumberId = entity.OriginalInvoiceNumberId;
            viewModel.OriginalInvoiceNumber = entity.OriginalInvoiceNumber;
            viewModel.OriginalInvoiceQbNumber = entity.OriginalInvoiceQbNumber;
            viewModel.CreditInvoiceDisplayNumber = entity.CreditInvoiceDisplayNumber;
            viewModel.OriginalInvoiceType = entity.OriginalInvoiceTypeId;
            viewModel.QuantityIndicatorTypeId = entity.QuantityIndicatorTypeId;
            viewModel.NetQuantity = entity.NetQuantity;
            viewModel.GrossQuantity = entity.GrossQuantity;
            viewModel.LiftQuantity = entity.LiftQuantity;
            viewModel.CreationMethod = (CreationMethod)entity.CreationMethod;
            viewModel.IsMarineLocation = entity.IsMarineLocation;
            viewModel.ConvertedQuantity = entity.ConvertedQuantity;
            viewModel.FRUoM = entity.FRUoM;
            viewModel.ConvertedPricing = entity.ConvertedPricing;
            viewModel.Gravity = entity.Gravity;
            viewModel.GallonsPerMetricTon = entity.GallonsPerMetricTon;
            viewModel.SuperAdminProductDescription = entity.SuperAdminProductDescription;
            viewModel.AdditionalDetail = new InvoiceXAdditionalDetailViewModel()
            {
                IsSurchargeApplicable = entity.IsSurchargeApplicable,
                IsFreightCostApplicable = entity.IsFreightCostApplicable,
                SurchargePricingType = entity.SurchargePricingType,
                SurchargePercentage = entity.SurchargePercentage,
                SurchargeEIAPrice = entity.SurchargeEIAPrice,
                Notes = entity.Notes,
                DropTicketNumber = entity.DropTicketNumber,
                SupplierAllowance = entity.SupplierAllowance,
                TotalAllowance = entity.TotalAllowance ?? 0,
                IsShowProductDescriptionOnInvoice = entity.IsShowProductDescriptionOnInvoice
            };

            if (entity.ImageId.HasValue)
            {
                viewModel.Image = new ImageViewModel()
                {
                    Id = entity.ImageId.Value
                };
            }

            viewModel.JobName = entity.JobName;
            viewModel.FuelTypeId = entity.FuelTypeId;
            viewModel.ProductTypeId = entity.ProductTypeId;
            viewModel.PricingTypeId = entity.PricingTypeId;
            viewModel.FuelDisplayGroupId = entity.ProductDisplayGroupId;
            viewModel.FuelType = entity.ProductName;
            if (entity.ProductDisplayGroupId == (int)ProductDisplayGroups.OtherFuelType)
            {
                viewModel.FuelType = entity.ProductName;
                viewModel.NonStandardFuelName = entity.ProductName;
                viewModel.NonStandardFuelDescription = entity.ProductDescription;
            }

            viewModel.TerminalName = entity.TerminalName;
            viewModel.PickupTerminal = entity.PickupTerminal;
            viewModel.PaymentTermId = entity.PaymentTermId;
            viewModel.PaymentTermName = entity.PaymentTermName;
            viewModel.NetDays = entity.NetDays;
            viewModel.IsFTL = entity.IsFTL;
            viewModel.IsRebillInvoice = viewModel.OriginalInvoiceNumberId.HasValue && entity.InvoiceTypeId != (int)InvoiceType.CreditInvoice && entity.InvoiceTypeId != (int)InvoiceType.PartialCredit;

            viewModel.IsBuyAndSellOrder = entity.IsBuyAndSellOrder;
            if (viewModel.IsBuyAndSellOrder)
            {
                viewModel.BuyAndSellPricingDetail = new BuyAndSellPricingDetailViewModel()
                {
                    BasePrice = entity.BasePrice,
                    BrokerMarkUp = entity.BrokerMarkUp,
                    SupplierMarkUp = entity.SupplierMarkUp,
                    BuyPrice = entity.BasePrice + entity.BrokerMarkUp,
                    SellPrice = entity.BasePrice + entity.BrokerMarkUp + entity.SupplierMarkUp,
                    IsBuyPriceInvoice = entity.IsBuyPriceInvoice
                };
                viewModel.BuyAndSellPricingDetail.BuyPriceDetail = $"{Resource.lblBasePrice} + {Resource.constSymbolCurrency}{viewModel.BuyAndSellPricingDetail.BrokerMarkUp.ToString(ApplicationConstants.DecimalFormat2)}";
                viewModel.BuyAndSellPricingDetail.SellPriceDetail = $"{Resource.lblBuyPrice} + {Resource.constSymbolCurrency}{viewModel.BuyAndSellPricingDetail.SupplierMarkUp.ToString(ApplicationConstants.DecimalFormat2)}";
            }

            viewModel.WaitingForAction = entity.WaitingFor;
            viewModel.IsBuyPriceInvoice = entity.IsBuyPriceInvoice;
            viewModel.UpdatedDate = entity.UpdatedDate;
            viewModel.Currency = (Currency)entity.Currency;
            viewModel.UoM = (UoM)entity.UoM;

            viewModel.IsVariousFobOrigin = entity.IsVariousFobOrigin;
            viewModel.WBSNumber = entity.WBSNumber;
            viewModel.IsTrueFillTax = entity.IsTrueFillTax;
            viewModel.IsApprovalWorkflowEnabledForJob = entity.IsApprovalWorkflowEnabled;
            viewModel.CxmlCheckOutDate = entity.CxmlCheckOutDate;
            viewModel.StatementNumber = entity.StatementNumber;
            viewModel.StatementId = entity.StatementId;
            viewModel.IsExceptionDdt = entity.IsExceptionDdt;
            if (entity.PDIDetailsDate.HasValue)
            {
                viewModel.PDIDetailsDate = entity.PDIDetailsDate.Value.Date.ToString("MM-dd-yyyy");
                viewModel.PDIDetailsTime = entity.PDIDetailsDate.Value.GetTimeInHhMmFormat();
            }
            viewModel.IsHidePricingEnabled = companyType == CompanyType.Buyer ? entity.IsHidePricingEnabledForBuyer : entity.IsHidePricingEnabledForSupplier;

            viewModel.IMONumber = entity.IMONumber;
            viewModel.Berth = entity.Berth;
            viewModel.Vessel = entity.Vessel;

            viewModel.OrderQuantity = entity.OrderQuantity;
            viewModel.DisplayDropEndDate = entity.DisplayDropEndDate;
            viewModel.DisplayDropStartDate = entity.DisplayDropStartDate;
            if (viewModel.IsMarineLocation && viewModel.UoM == UoM.MetricTons)
            {
                if (viewModel.PricingTypeId == (int)PricingType.PricePerGallon)
                {
                    var displayPrice = viewModel.PricePerGallonDisplay.Replace("$", "");
                    var decimalPrice = Convert.ToDecimal(displayPrice);
                    if (decimalPrice > 0)
                    {
                        viewModel.PricePerGallonDisplay = string.Concat("$", decimalPrice.GetCommaSeperatedValue());
                    }
                }
            }
            viewModel.DeliveryLevelPO = entity.DeliveryLevelPO;
            return viewModel;
        }

        public static PdfHeaderViewModel ToViewModel(this UspPdfHeaderViewModel entity, PdfHeaderViewModel viewModel = null)
        {
            if (viewModel == null)
            {
                viewModel = new PdfHeaderViewModel();
            }

            if (entity.CompanyLogoId > 0)
            {
                viewModel.CompanyLogo = new ImageViewModel()
                {
                    Id = entity.CompanyLogoId,
                    Data = entity.CompanyLogoData,
                    FilePath = entity.CompanyLogoPath
                };
            }

            viewModel.SupplierCompanyId = entity.SupplierCompanyId;
            viewModel.InvoiceFooterJson = entity.InvoiceFooterJson;
            viewModel.CustomerId = ApplicationConstants.CustomerNumberPrefix + entity.BuyerCompanyId.ToString(ApplicationConstants.SevenDigit);
            viewModel.Carrier = entity.Carrier;
            viewModel.InvoiceDate = entity.UpdatedDate;
            viewModel.PaymentTermId = entity.PaymentTermId;
            viewModel.PaymentTerm = entity.PaymentTermName;
            viewModel.NetDays = entity.NetDays;
            viewModel.PaymentDueDate = entity.PaymentDueDate;
            viewModel.SupplierCompanyName = entity.SupplierCompanyName;
            viewModel.SupplierPhoneNumber = entity.SupplierPhoneNumber;
            viewModel.DisplayInvoiceNumber = entity.DisplayInvoiceNumber;
            viewModel.JobName = entity.JobName;
            viewModel.UoM = entity.UoM;
            viewModel.Currency = entity.Currency;
            viewModel.AccountingCompanyId = entity.AccountingCompanyId;
            viewModel.DeliveryRequestId = entity.DeliveryRequestId;

            viewModel.CarrierOrderId = entity.CarrierOrderId;
            viewModel.JobCountryId = entity.JobCountryId;

            viewModel.SupplierLocation = new AddressViewModel()
            {
                Address = entity.SupplierAddress,
                City = entity.SupplierAddressCity,
                StateCode = entity.SupplierAddressStateCode,
                ZipCode = entity.SupplierAddressZipCode,
                CountryId = entity.SupplierAddressCountryId
            };

            viewModel.BuyerCompanyName = entity.BuyerCompanyName;
            viewModel.BuyerLocation = new AddressViewModel()
            {
                Address = entity.BillingAddress,
                AddressLine2 = entity.BillingAddressLine2,
                AddressLine3 = entity.BillingAddressLine3,
                City = entity.BillingCity,
                CountyName = entity.BillingCounty,
                StateCode = entity.BillingStateCode,
                ZipCode = entity.BillingZipCode,
                StateName = entity.BillingStateName,
                CountryName = entity.BillingCountryName
            };

            if (entity.IsBillToEnabled)
            {
                viewModel.JobSpecificBillTo = new JobSpecificBillingInfoViewModel()
                {
                    BillToAddress = entity.BillToAddress,
                    BillToAddressLine2 = entity.BillToAddressLine2,
                    BillToAddressLine3 = entity.BillToAddressLine3,
                    BillToCity = entity.BillToCity,
                    BillToZipCode = entity.BillToZipCode,
                    BillToStateCode = entity.BillToStateCode,
                    BillToStateName = entity.BillToStateName,
                    BillToCountryCode = entity.BillToCountryCode,
                    BillToCountryName = entity.BillToCountryName,
                    IsJobSpecificBillToEnabled = entity.IsBillToEnabled,
                    BillToName = entity.BillToName
                };
            }

            viewModel.PoContact = new ContactPersonViewModel()
            {
                Name = entity.PoContactName,
                Email = entity.PoContactEmail,
                PhoneNumber = entity.PoContactPhoneNumber
            };

            viewModel.ShippingLocation = new AddressViewModel()
            {
                Address = entity.JobAddress,
                AddressLine2 = entity.JobAddressLine2,
                AddressLine3 = entity.JobAddressLine3,
                City = entity.JobCity,
                StateCode = entity.JobStateCode,
                ZipCode = entity.JobZipCode
            };

            if (!string.IsNullOrWhiteSpace(entity.DropStateCode))
            {
                viewModel.ShippingLocation = new AddressViewModel()
                {
                    Address = entity.DropAddress,
                    City = entity.DropCity,
                    StateCode = entity.DropStateCode,
                    ZipCode = entity.DropZipCode
                };
            }
            viewModel.Vessel = entity.Vessel;
            viewModel.Berth = entity.Berth;
            viewModel.BDRNumber = entity.BDRNumber;
            return viewModel;
        }

        public static InvoicePdfViewModel ToInvoicePdfViewModel(this UspInvoicePdfDetail entity, InvoicePdfViewModel viewModel = null)
        {
            if (viewModel == null)
            {
                viewModel = new InvoicePdfViewModel(Status.Success);
            }
            viewModel.PoNumber = entity.PoNumber;
            if (entity.CompanyLogoId > 0)
            {
                viewModel.CompanyLogo = new ImageViewModel()
                {
                    Id = entity.CompanyLogoId,
                    Data = entity.CompanyLogoData
                };
            }
            viewModel.SupplierCompanyName = entity.SupplierCompanyName;
            viewModel.SupplierLocation = new AddressViewModel()
            {
                Address = entity.SupplierAddress,
                City = entity.SupplierAddressCity,
                StateCode = entity.SupplierAddressStateCode,
                ZipCode = entity.SupplierAddressZipCode
            };
            viewModel.PhoneNumber = entity.PhoneNumber;
            viewModel.OriginalInvoiceNumberId = entity.OriginalInvoiceNumberId;
            viewModel.OriginalInvoiceNumber = entity.OriginalInvoiceNumber;
            viewModel.OriginalInvoiceType = entity.OriginalInvoiceTypeId;
            viewModel.OriginalInvoiceQbNumber = entity.OriginalInvoiceQbNumber;
            viewModel.CreditInvoiceDisplayNumber = entity.CreditInvoiceDisplayNumber;
            viewModel.CustomerId = ApplicationConstants.CustomerNumberPrefix + entity.BuyerCompanyId.ToString(ApplicationConstants.SevenDigit);
            viewModel.BuyerCompanyName = entity.BuyerCompanyName;
            viewModel.BulkPlantName = entity.BulkPlantName;
            viewModel.BuyerLocation = new AddressViewModel()
            {
                Address = entity.BillingAddress,
                AddressLine2 = entity.BillingAddressLine2,
                AddressLine3 = entity.BillingAddressLine3,
                City = entity.BillingCity,
                StateCode = entity.BillingStateCode,
                ZipCode = entity.BillingZipCode
            };
            viewModel.ShippingLocation = new AddressViewModel()
            {
                Address = entity.JobAddress,
                AddressLine2 = entity.JobAddressLine2,
                AddressLine3 = entity.JobAddressLine3,
                City = entity.JobCity,
                StateCode = entity.JobStateCode,
                ZipCode = entity.JobZipCode
            };
            viewModel.PoContact = new ContactPersonViewModel()
            {
                Name = entity.PoContactName,
                Email = entity.PoContactEmail,
                PhoneNumber = entity.PoContactPhoneNumber
            };

            viewModel.FuelRequest.Job.Name = entity.JobName;
            viewModel.FuelRequest.FuelDetails.FuelTypeId = entity.FuelTypeId;
            viewModel.FuelRequest.FuelDetails.ProductTypeId = entity.ProductTypeId;
            viewModel.FuelRequest.FuelDetails.FuelPricing.PricingTypeId = entity.PricingTypeId;
            viewModel.FuelRequest.FuelDetails.FuelPricing.Currency = (Currency)entity.Currency;
            viewModel.FuelRequest.FuelDetails.FuelQuantity.UoM = (UoM)entity.UoM;
            viewModel.FuelRequest.FuelDetails.FuelDisplayGroupId = entity.ProductDisplayGroupId;
            viewModel.FuelRequest.FuelDetails.FuelType = entity.ProductName;
            if (entity.ProductDisplayGroupId == (int)ProductDisplayGroups.OtherFuelType)
            {
                viewModel.FuelRequest.FuelDetails.NonStandardFuelName = entity.ProductName;
                viewModel.FuelRequest.FuelDetails.NonStandardFuelDescription = entity.ProductDescription;
            }

            viewModel.TerminalName = entity.TerminalName;
            viewModel.PickupTerminal = entity.PickupTerminal;
            viewModel.PaymentTermId = entity.PaymentTermId;
            viewModel.PaymentTermName = entity.PaymentTermName;
            viewModel.NetDays = entity.NetDays;
            viewModel.IsFTL = entity.IsFTL;
            viewModel.IsRebillInvoice = viewModel.OriginalInvoiceNumberId.HasValue && entity.InvoiceTypeId != (int)InvoiceType.CreditInvoice && entity.InvoiceTypeId != (int)InvoiceType.PartialCredit;
            if (!string.IsNullOrWhiteSpace(entity.PickupAddress))
            {
                viewModel.PickUpLocation = new AddressViewModel()
                {
                    Address = entity.PickupAddress,
                    City = entity.PickupCity,
                    StateCode = entity.PickupStateCode,
                    ZipCode = entity.PickupZipCode,
                    LocationType = entity.PickupLocationType
                };
            }
            if (!string.IsNullOrWhiteSpace(entity.DropStateCode))
            {
                viewModel.ShippingLocation = new AddressViewModel()
                {
                    Address = entity.DropAddress,
                    City = entity.DropCity,
                    StateCode = entity.DropStateCode,
                    ZipCode = entity.DropZipCode
                };
            }
            viewModel.IsBuyAndSellOrder = entity.IsBuyAndSellOrder;
            if (viewModel.IsBuyAndSellOrder)
            {
                viewModel.BuyAndSellPricingDetail = new BuyAndSellPricingDetailViewModel()
                {
                    BasePrice = entity.BasePrice,
                    BrokerMarkUp = entity.BrokerMarkUp,
                    SupplierMarkUp = entity.SupplierMarkUp,
                    BuyPrice = entity.BasePrice + entity.BrokerMarkUp,
                    SellPrice = entity.BasePrice + entity.BrokerMarkUp + entity.SupplierMarkUp,
                    IsBuyPriceInvoice = entity.IsBuyPriceInvoice
                };
                viewModel.BuyAndSellPricingDetail.BuyPriceDetail = $"{Resource.lblBasePrice} + {Resource.constSymbolCurrency}{viewModel.BuyAndSellPricingDetail.BrokerMarkUp.ToString(ApplicationConstants.DecimalFormat2)}";
                viewModel.BuyAndSellPricingDetail.SellPriceDetail = $"{Resource.lblBuyPrice} + {Resource.constSymbolCurrency}{viewModel.BuyAndSellPricingDetail.SupplierMarkUp.ToString(ApplicationConstants.DecimalFormat2)}";
            }
            return viewModel;
        }

        public static ManualInvoiceViewModel GetOrderTaxesForNonStandardFuel(this ManualInvoiceViewModel manualInvoiceModel, Order order)
        {
            if (manualInvoiceModel.TypeofFuel == (int)ProductDisplayGroups.OtherFuelType)
            {
                manualInvoiceModel.Taxes = new List<TaxViewModel>();
                if (order.OrderTaxDetails != null && order.OrderTaxDetails.Any(t => t.IsActive))
                {
                    foreach (var tax in order.OrderTaxDetails.Where(t => t.IsActive).ToList())
                    {
                        manualInvoiceModel.Taxes.Add(new TaxViewModel
                        {
                            TaxAmount = tax.TaxRate,
                            TaxPricingTypeId = tax.TaxPricingTypeId,
                            TaxDescription = tax.TaxDescription
                        });
                    }
                }
            }
            return manualInvoiceModel;
        }

        public static ManualInvoiceViewModel SetFuelFeesForBrokerOrderToManualInvoiceViewModel(this ManualInvoiceViewModel manualInvoiceModel, Order order)
        {
            manualInvoiceModel.FuelRequestFee = order.FuelRequest.FuelRequestFees.ToViewModel();
            manualInvoiceModel.FuelRequestFee.AdditionalFee = order.FuelRequest.FuelRequestFees.ToAdditionalFeeViewModel().ToList();
            manualInvoiceModel.FuelDeliveryDetails.FuelFees.FuelRequestFees = order.FuelRequest.FuelRequestFees.ToFeesViewModel();

            return manualInvoiceModel;
        }

        public static Invoice ToEntity(this InvoiceViewModel viewModel, Invoice entity = null)
        {
            if (entity == null)
            {
                entity = new Invoice();
            }
            var offset = viewModel.DropEndDate.GetOffset(viewModel.TimeZoneName);
            entity.Id = viewModel.Id;
            entity.OrderId = viewModel.OrderId;
            entity.PoNumber = viewModel.PoNumber;
            entity.InvoiceTypeId = viewModel.InvoiceTypeId;
            entity.DroppedGallons = viewModel.DroppedGallons;
            entity.DropStartDate = viewModel.DropStartDate.AttachOffset(offset);
            entity.DropEndDate = viewModel.DropEndDate.AttachOffset(offset);
            entity.StateTax = viewModel.StateTax;
            entity.FedTax = viewModel.FederalTax;
            entity.SalesTax = viewModel.SalesTax;
            entity.BasicAmount = viewModel.BasicAmount;
            entity.PaymentDueDate = viewModel.PaymentDueDate;
            entity.PaymentDate = viewModel.PaymentDate;
            entity.IsOverWaterDelivery = viewModel.IsOverWaterDelivery;
            entity.IsWetHosingDelivery = viewModel.IsWetHosingDelivery;
            entity.CreatedBy = viewModel.CreatedBy;
            entity.CreatedDate = viewModel.CreatedDate;
            entity.InvoiceXSpecialInstructions = viewModel.SpecialInstructions.Select(t => t.ToEntity()).ToList();

            entity.TotalTaxAmount = viewModel.TotalTaxAmount;
            entity.TraceId = viewModel.TraceId;
            entity.BrokeredChainId = viewModel.BrokeredChainId;
            entity.DisplayInvoiceNumber = string.IsNullOrWhiteSpace(viewModel.DisplayInvoiceNumber) ?
                                            viewModel.InvoiceNumber.Number.FormattedInvoiceNumber(viewModel.InvoiceTypeId) :
                                            viewModel.DisplayInvoiceNumber.FormattedInvoiceNumber(viewModel.InvoiceTypeId);
            entity.ReferenceId = viewModel.ReferenceId;
            if (!string.IsNullOrEmpty(viewModel.TransactionId))
                entity.TransactionId = viewModel.TransactionId.Equals(entity.DisplayInvoiceNumber) ? Resource.lblNotAvailable : viewModel.TransactionId;
            if (viewModel.AdditionalDetail != null)
            {
                entity.InvoiceXAdditionalDetail = viewModel.AdditionalDetail.ToEntity(entity.InvoiceXAdditionalDetail);
                if (viewModel.FuelSurchargeFreightFee != null)
                {
                    entity.InvoiceXAdditionalDetail.IsSurchargeApplicable = viewModel.FuelSurchargeFreightFee.IsSurchargeApplicable;
                    if (viewModel.FuelSurchargeFreightFee.IsSurchargeApplicable)
                    {
                        //For Manual and Auto Freight Method both are applicable
                        entity.InvoiceXAdditionalDetail.SurchargeEIAPrice = viewModel.FuelSurchargeFreightFee.SurchargeEiaPrice;
                        entity.InvoiceXAdditionalDetail.SurchargePercentage = viewModel.FuelSurchargeFreightFee.SurchargePercentage;
                        entity.InvoiceXAdditionalDetail.SurchargePricingType = viewModel.FuelSurchargeFreightFee.SurchargePricingType;
                        entity.InvoiceXAdditionalDetail.SurchargeTableRangeEnd = viewModel.FuelSurchargeFreightFee.SurchargeTableRangeEnd;
                        entity.InvoiceXAdditionalDetail.SurchargeTableRangeStart = viewModel.FuelSurchargeFreightFee.SurchargeTableRangeStart;
                    }

                    if (viewModel.FuelSurchargeFreightFee.FreightPricingMethod == FreightPricingMethod.Auto)
                    {
                        ToEntityFuelSurchargeDetailsForAuto(viewModel, entity);
                    }
                }
            }

            if (viewModel.Image != null && viewModel.Image.Id > 0)
            {
                entity.ImageId = viewModel.Image.Id;
            }
            else
            {
                entity.Image = viewModel.Image == null || string.IsNullOrWhiteSpace(viewModel.Image?.FilePath) || viewModel.Image.IsRemoved ? null : viewModel.Image.ToEntity();
            }

            entity.InvoiceXInvoiceStatusDetails.ToList().ForEach(t => t.IsActive = false);
            InvoiceXInvoiceStatusDetail invoiceStatus = new InvoiceXInvoiceStatusDetail();
            invoiceStatus.StatusId = viewModel.StatusId;
            invoiceStatus.IsActive = true;
            invoiceStatus.UpdatedBy = viewModel.UserId;
            invoiceStatus.UpdatedDate = DateTimeOffset.Now.ToTargetDateTimeOffset(viewModel.TimeZoneName);
            entity.InvoiceXInvoiceStatusDetails.Add(invoiceStatus);

            if (viewModel.Signature != null)
                entity.Signaure = viewModel.Signature.ToEntity();

            entity.DriverId = viewModel.DriverId;
            entity.FilePath = viewModel.CsvFilePath;
            entity.WaitingFor = viewModel.WaitingForAction;
            if (entity.WaitingFor > 0)
            {
                entity.IsActive = false;
            }

            entity.IsActive = viewModel.IsActive;
            entity.UpdatedBy = viewModel.UpdatedBy;
            entity.UpdatedDate = viewModel.UpdatedDate;
            entity.IsBuyPriceInvoice = viewModel.IsBuyPriceInvoice;
            entity.BrokeredChainId = viewModel.BrokeredChainId;
            entity.QbInvoiceNumber = viewModel.QbInvoiceNumber;
            entity.Currency = viewModel.Currency;
            entity.UoM = viewModel.UoM;

            if (viewModel.AdditionalImage != null && viewModel.AdditionalImage.Id > 0)
            {
                if (entity.InvoiceXAdditionalDetail != null)
                    entity.InvoiceXAdditionalDetail.AdditionalImageId = viewModel.AdditionalImage.Id;
            }
            else
            {
                if (entity.InvoiceXAdditionalDetail.AdditionalImage == null)
                    entity.InvoiceXAdditionalDetail.AdditionalImage = viewModel.AdditionalImage == null || string.IsNullOrWhiteSpace(viewModel.AdditionalImage?.FilePath) || viewModel.AdditionalImage.IsRemoved ? null : viewModel.AdditionalImage.ToEntity();
            }

            if (viewModel.InspectionRequestVoucherImage != null && viewModel.InspectionRequestVoucherImage.Id > 0)
            {
                if (entity.InvoiceXAdditionalDetail != null)
                    entity.InvoiceXAdditionalDetail.InspectionRequestVoucherImageId = viewModel.InspectionRequestVoucherImage.Id;
            }
            else
            {
                if (entity.InvoiceXAdditionalDetail.InspectionRequestVoucherImage == null)
                    entity.InvoiceXAdditionalDetail.InspectionRequestVoucherImage = viewModel.InspectionRequestVoucherImage == null || string.IsNullOrWhiteSpace(viewModel.InspectionRequestVoucherImage?.FilePath) || viewModel.InspectionRequestVoucherImage.IsRemoved ? null : viewModel.InspectionRequestVoucherImage.ToEntity();
            }

            if (viewModel.FuelPickLocation != null)
            {
                entity.InvoiceDispatchLocation.Add(viewModel.FuelPickLocation.ToEntity());
            }

            if (viewModel.DropAddress != null && !string.IsNullOrWhiteSpace(viewModel.DropAddress.City))
            {
                var dropLocation = viewModel.ToDropLocation();
                entity.InvoiceDispatchLocation.Add(dropLocation.ToEntity());
            }
            entity.DDTConversionReason = viewModel.DDTConversionReason;
            entity.IsBolImageReq = viewModel.IsBOLImageReq;
            entity.IsSignatureReq = viewModel.IsSignatureReq;
            entity.IsDropImageReq = viewModel.IsDropImageReq;
            return entity;
        }

        private static void ToEntityFuelSurchargeDetailsForAuto(InvoiceViewModel viewModel, Invoice entity)
        {
            if (viewModel.FuelSurchargeFreightFee.FuelSurchargeTableId.HasValue && viewModel.FuelSurchargeFreightFee.FuelSurchargeTableId.Value > 0)
            {
                entity.InvoiceXAdditionalDetail.FuelSurchargeTableType = viewModel.FuelSurchargeFreightFee.FuelSurchargeTableType;
                entity.InvoiceXAdditionalDetail.FuelSurchargeTableId = viewModel.FuelSurchargeFreightFee.FuelSurchargeTableId;
            }

            if (viewModel.FuelSurchargeFreightFee.FreightRateRuleId.HasValue && viewModel.FuelSurchargeFreightFee.FreightRateRuleId.Value > 0)
            {
                entity.InvoiceXAdditionalDetail.IsFreightCostApplicable = viewModel.FuelSurchargeFreightFee.IsFreightCostApplicable;
                entity.InvoiceXAdditionalDetail.FreightRateRuleType = viewModel.FuelSurchargeFreightFee.FreightRateRuleType;
                entity.InvoiceXAdditionalDetail.FreightRateTableType = viewModel.FuelSurchargeFreightFee.FreightRateTableType;
                entity.InvoiceXAdditionalDetail.FreightRateRuleId = viewModel.FuelSurchargeFreightFee.FreightRateRuleId;
                entity.InvoiceXAdditionalDetail.Distance = viewModel.FuelSurchargeFreightFee.AutoFreightDistance;
            }
        }

        public static Invoice ToEntity(this InvoiceModel viewModel, Invoice entity = null)
        {
            if (entity == null)
            {
                entity = new Invoice();
            }
            entity.Id = viewModel.Id;
            entity.OrderId = viewModel.OrderId;
            entity.InvoiceVersionStatusId = viewModel.InvoiceVersionStatusId;
            entity.Version = viewModel.Version;
            entity.InvoiceTypeId = viewModel.InvoiceTypeId;
            entity.DroppedGallons = viewModel.DroppedGallons;
            entity.DropStartDate = viewModel.DropStartDate;
            entity.DropEndDate = viewModel.DropEndDate;
            entity.PoNumber = viewModel.PoNumber;
            entity.StateTax = viewModel.StateTax;
            entity.FedTax = viewModel.FedTax;
            entity.SalesTax = viewModel.SalesTax;
            entity.BasicAmount = viewModel.BasicAmount;
            entity.IsOverWaterDelivery = viewModel.IsOverWaterDelivery;
            entity.IsWetHosingDelivery = viewModel.IsWetHosingDelivery;
            entity.PaymentTermId = viewModel.PaymentTermId;
            entity.NetDays = viewModel.NetDays;
            entity.PaymentDueDate = viewModel.PaymentDueDate;
            entity.PaymentDate = viewModel.PaymentDate;
            entity.ParentId = viewModel.ParentId;
            entity.CreatedBy = viewModel.CreatedBy;
            entity.CreatedDate = viewModel.CreatedDate;
            entity.IsActive = viewModel.IsActive;
            entity.UpdatedBy = viewModel.UpdatedBy;
            entity.UpdatedDate = viewModel.UpdatedDate;
            entity.TotalTaxAmount = viewModel.TotalTaxAmount;
            //Set TrackableScheduleId
            if (viewModel.TrackableScheduleId.HasValue && viewModel.TrackableScheduleId.Value > 0)
            {
                entity.TrackableScheduleId = viewModel.TrackableScheduleId;
            }
            entity.DriverId = viewModel.DriverId;
            entity.TraceId = viewModel.TraceId;
            entity.SignatureId = viewModel.SignatureId;
            if (!viewModel.SignatureId.HasValue || viewModel.SignatureId.Value == 0)
            {
                if (viewModel.Signature != null && string.IsNullOrWhiteSpace(viewModel.Signature.Name))
                {
                    //viewModel.Signature.Name not coming in mobile drop.so set dash.
                    viewModel.Signature.Name = "-";
                }
                entity.Signaure = (viewModel.Signature != null && viewModel.Signature.Name != null) ? viewModel.Signature.ToEntity() : null;
            }
            else if (viewModel.Signature != null)
                entity.Signaure = viewModel.Signature.ToEntity();

            entity.FilePath = viewModel.FilePath;
            entity.WaitingFor = (int)viewModel.WaitingFor;
            entity.SupplierPreferredInvoiceTypeId = viewModel.SupplierPreferredInvoiceTypeId;
            entity.IsBuyPriceInvoice = viewModel.IsBuyPriceInvoice;
            entity.TotalFeeAmount = viewModel.TotalFeeAmount;
            entity.BrokeredChainId = viewModel.BrokeredChainId;
            entity.BaseDroppedQuntity = viewModel.BaseDroppedQuntity;
            entity.BasePrice = viewModel.BasePrice;
            entity.BaseStateTax = viewModel.BaseStateTax;
            entity.BaseFedTax = viewModel.BaseFedTax;
            entity.BaseStateTax = viewModel.BaseSalesTax;
            entity.BaseBasicAmount = viewModel.BaseBasicAmount;
            entity.BaseTotalTaxAmount = viewModel.BaseTotalTaxAmount;
            entity.BaseRackPrice = viewModel.BaseRackPrice;
            entity.BaseTotalFeeAmount = viewModel.BaseTotalFeeAmount;
            entity.Currency = viewModel.Currency;
            entity.ExchangeRate = viewModel.ExchangeRate;
            entity.UoM = viewModel.UoM;
            entity.DisplayInvoiceNumber = viewModel.DisplayInvoiceNumber;
            entity.ReferenceId = viewModel.ReferenceId;
            entity.QbInvoiceNumber = viewModel.QbInvoiceNumber;
            entity.TotalDiscountAmount = viewModel.TotalDiscountAmount;
            entity.DDTConversionReason = viewModel.DDTConversionReason;
            entity.IsBolImageReq = viewModel.IsBOLImageReq;
            entity.IsSignatureReq = viewModel.IsSignatureReq;
            entity.IsDropImageReq = viewModel.IsDropImageReq;
            entity.QuantityIndicatorTypeId = viewModel.QuantityIndicatorTypeId;
            entity.TransactionId = viewModel.TransactionId.Equals(entity.DisplayInvoiceNumber) ? Resource.lblNotAvailable : viewModel.TransactionId;
            entity.GroupParentDrId = viewModel.GroupParentDrId;
            entity.Gravity = viewModel.Gravity;
            entity.ConversionFactor = viewModel.ConversionFactor;
            entity.ConvertedQuantity = viewModel.ConvertedQuantity;
            entity.ConvertedPricing = viewModel.ConvertedPricing;
            entity.PDIInvoiceNumber = viewModel.PDIInvoiceNo;// will rename bol col to pdiinvoiceno [vipul]

            if (viewModel.Image != null && !viewModel.Image.IsRemoved && viewModel.Image.Id > 0)
            {
                entity.ImageId = viewModel.Image.Id;
            }
            else
            {
                entity.Image = viewModel.Image == null || string.IsNullOrWhiteSpace(viewModel.Image?.FilePath) || viewModel.Image.IsRemoved ? null : viewModel.Image.ToEntity();
            }

            entity.InvoiceXInvoiceStatusDetails.ToList().ForEach(t => t.IsActive = false);
            InvoiceXInvoiceStatusDetail invoiceStatus = new InvoiceXInvoiceStatusDetail
            {
                StatusId = viewModel.StatusId,
                UpdatedBy = viewModel.UpdatedBy,
                UpdatedDate = viewModel.UpdatedDate,
                IsActive = true
            };
            entity.InvoiceXInvoiceStatusDetails.Add(invoiceStatus);

            if (viewModel.AdditionalDetail != null)
            {
                entity.InvoiceXAdditionalDetail = viewModel.AdditionalDetail.ToEntity(entity.InvoiceXAdditionalDetail);
                SetFuelSurchargeDetails(viewModel, entity);
            }
            if (viewModel.AdditionalImage != null && !viewModel.AdditionalImage.IsRemoved && viewModel.AdditionalImage.Id > 0)
            {
                if (entity.InvoiceXAdditionalDetail != null)
                    entity.InvoiceXAdditionalDetail.AdditionalImageId = viewModel.AdditionalImage.Id;
            }
            else
            {
                if (entity.InvoiceXAdditionalDetail.AdditionalImage == null)
                    entity.InvoiceXAdditionalDetail.AdditionalImage = viewModel.AdditionalImage == null || string.IsNullOrWhiteSpace(viewModel.AdditionalImage?.FilePath) || viewModel.AdditionalImage.IsRemoved ? null : viewModel.AdditionalImage.ToEntity();
            }
            if (viewModel.TaxAffidavitImage != null && !viewModel.TaxAffidavitImage.IsRemoved && viewModel.TaxAffidavitImage.Id > 0 && String.IsNullOrEmpty(viewModel.TaxAffidavitImage.Name))
            {
                if (entity.InvoiceXAdditionalDetail != null)
                    entity.InvoiceXAdditionalDetail.TaxAffidavitImageId = viewModel.TaxAffidavitImage.Id;
            }
            else
            {
                if (entity.InvoiceXAdditionalDetail.TaxAffidavitImageId == null)
                    entity.InvoiceXAdditionalDetail.TaxAffidavitImage = viewModel.TaxAffidavitImage == null || string.IsNullOrWhiteSpace(viewModel.TaxAffidavitImage?.FilePath) || viewModel.TaxAffidavitImage.IsRemoved ? null : viewModel.TaxAffidavitImage.ToEntity();
            }

            if (viewModel.BDNImage != null && !viewModel.BDNImage.IsRemoved && viewModel.BDNImage.Id > 0 && String.IsNullOrEmpty(viewModel.BDNImage.Name))
            {
                if (entity.InvoiceXAdditionalDetail != null)
                    entity.InvoiceXAdditionalDetail.BDNImageId = viewModel.BDNImage.Id;
            }
            else
            {
                if (entity.InvoiceXAdditionalDetail.BDNImageId == null)
                    entity.InvoiceXAdditionalDetail.BDNImage = viewModel.BDNImage == null || string.IsNullOrWhiteSpace(viewModel.BDNImage?.FilePath) || viewModel.BDNImage.IsRemoved ? null : viewModel.BDNImage.ToEntity();
            }

            if (viewModel.CoastGuardInspectionImage != null && !viewModel.CoastGuardInspectionImage.IsRemoved && viewModel.CoastGuardInspectionImage.Id > 0 && String.IsNullOrEmpty(viewModel.CoastGuardInspectionImage.Name))
            {
                if (entity.InvoiceXAdditionalDetail != null)
                    entity.InvoiceXAdditionalDetail.CoastGuardInspectionImageId = viewModel.CoastGuardInspectionImage.Id;
            }
            else
            {
                if (entity.InvoiceXAdditionalDetail.CoastGuardInspectionImageId == null)
                    entity.InvoiceXAdditionalDetail.CoastGuardInspectionImage = viewModel.CoastGuardInspectionImage == null || string.IsNullOrWhiteSpace(viewModel.CoastGuardInspectionImage?.FilePath) || viewModel.CoastGuardInspectionImage.IsRemoved ? null : viewModel.CoastGuardInspectionImage.ToEntity();
            }
            if (viewModel.InspectionRequestVoucherImage != null && !viewModel.InspectionRequestVoucherImage.IsRemoved && viewModel.InspectionRequestVoucherImage.Id > 0 && String.IsNullOrEmpty(viewModel.InspectionRequestVoucherImage.Name))
            {
                if (entity.InvoiceXAdditionalDetail != null)
                    entity.InvoiceXAdditionalDetail.InspectionRequestVoucherImageId = viewModel.InspectionRequestVoucherImage.Id;
            }
            else
            {
                if (entity.InvoiceXAdditionalDetail.InspectionRequestVoucherImageId == null)
                    entity.InvoiceXAdditionalDetail.InspectionRequestVoucherImage = viewModel.InspectionRequestVoucherImage == null || string.IsNullOrWhiteSpace(viewModel.InspectionRequestVoucherImage?.FilePath) || viewModel.InspectionRequestVoucherImage.IsRemoved ? null : viewModel.InspectionRequestVoucherImage.ToEntity();
            }
            entity.InvoiceXSpecialInstructions = viewModel.SpecialInstructions.Select(t => t.ToEntity()).ToList();
            entity.AssetDrops = viewModel.AssetDrops.Select(t => t.ToEntity()).ToList();
            entity.FuelRequestFees = viewModel.FuelFees.Select(t => t.ToEntity()).ToList();
            entity.InvoiceXAccessorialFees = viewModel.AccessorialFeeDetails.Select(t => t.ToEntity()).ToList();
            entity.Discounts = viewModel.Discounts.Select(t => t.ToEntity()).ToList();
            if (viewModel.TaxDetails != null && viewModel.TaxDetails.AvaTaxDetails.Any())
            {
                entity.TaxDetails = viewModel.TaxDetails.ToEntity();
                var emptyCurrenyTaxes = entity.TaxDetails.Where(t => t.Currency == null || t.Currency == "").ToList();
                if (emptyCurrenyTaxes.Any())
                {
                    emptyCurrenyTaxes.ForEach(t => t.Currency = viewModel.Currency.ToString());
                }
            }

            if (viewModel.FuelDropLocation != null)
            {
                entity.InvoiceDispatchLocation.Add(viewModel.FuelDropLocation.ToEntity());
            }
            entity.InvoiceExceptions = viewModel.InvoiceExceptions.Select(t => t.ToEntity()).ToList();

            //BDR DETAILS
            if (viewModel.BDRDetails != null)
            {
                entity.BDRDetail = viewModel.ToBDREntity(entity.BDRDetail);
            }
            return entity;
        }

        public static BDRDetail ToBDREntity(this InvoiceModel invoiceModel, BDRDetail entity = null)
        {
            if (entity == null)
                entity = new BDRDetail();
            entity.BDRNumber = invoiceModel.OrderId.ToString(); /*string.IsNullOrWhiteSpace(invoiceModel.BDRDetails.BDRNumber) ? ApplicationConstants.TFBD + invoiceModel.InvoiceNumberId : invoiceModel.BDRDetails.BDRNumber;*/
            entity.CloseMeterReading = invoiceModel.BDRDetails.CloseMeterReading;
            entity.DensityInVaccum = invoiceModel.BDRDetails.DensityInVaccum;
            entity.FlashPoint = invoiceModel.BDRDetails.FlashPoint;
            entity.IsActive = true;
            entity.IsEngineerInvitedToWitnessSample = invoiceModel.BDRDetails.IsEngineerInvitedToWitnessSample;
            entity.IsNoticeToProtestIssued = invoiceModel.BDRDetails.IsNoticeToProtestIssued;
            entity.MarpolSampleNumbers = invoiceModel.BDRDetails.MarpolSampleNumbers;
            entity.MVMarpolSampleNumbers = invoiceModel.BDRDetails.MVMarpolSampleNumbers;
            entity.MeasuredVolume = invoiceModel.BDRDetails.MeasuredVolume;
            entity.ObservedTemperature = invoiceModel.BDRDetails.ObservedTemperature;
            entity.OpenMeterReading = invoiceModel.BDRDetails.OpenMeterReading;
            entity.PumpingStartTime = invoiceModel.BDRDetails.PumpingStartTime;
            entity.PumpingStopTime = invoiceModel.BDRDetails.PumpingStopTime;
            entity.StandardVolume = invoiceModel.BDRDetails.StandardVolume;
            entity.SulphurContent = invoiceModel.BDRDetails.SulphurContent;
            entity.Viscosity = invoiceModel.BDRDetails.Viscosity;

            return entity;
        }

        public static BDRDetailsModel ToViewModel(this BDRDetail entity, BDRDetailsModel viewModel = null)
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

            return viewModel;
        }
        public static InvoiceHeaderDetail ToInvoiceHeaderEntity(this InvoiceModel invoice, InvoiceHeaderDetail entity = null)
        {
            if (entity == null)
            {
                entity = new InvoiceHeaderDetail();
            }
            entity.InvoiceNumberId = invoice.InvoiceNumberId;
            entity.TotalDroppedGallons = invoice.DroppedGallons;
            entity.TotalBasicAmount = invoice.BasicAmount;
            entity.TotalFeeAmount = invoice.TotalFeeAmount ?? 0;
            entity.TotalTaxAmount = invoice.TotalTaxAmount;
            entity.TotalDiscountAmount = invoice.TotalDiscountAmount;
            entity.Version = 1;
            entity.CreatedBy = entity.UpdatedBy = invoice.CreatedBy;
            entity.CreatedDate = entity.UpdatedDate = invoice.CreatedDate;
            entity.IsActive = true;
            return entity;
        }

        public static InvoiceHeaderDetail ToConsolidatedInvoiceHeaderEntity(this List<InvoiceEditRequestViewModel> consolidateInvoice, InvoiceHeaderDetail entity = null)
        {
            if (entity == null)
            {
                entity = new InvoiceHeaderDetail();
            }

            var firstInvoice = consolidateInvoice.First();
            entity.InvoiceNumberId = firstInvoice.InvoiceModel.InvoiceNumberId;
            entity.Version = firstInvoice.InvoiceHeaderVersion + 1;
            entity.CreatedBy = entity.UpdatedBy = firstInvoice.InvoiceModel.CreatedBy;
            entity.CreatedDate = entity.UpdatedDate = firstInvoice.InvoiceModel.CreatedDate;
            entity.IsActive = true;
            foreach (var invoice in consolidateInvoice)
            {
                entity.TotalDroppedGallons = entity.TotalDroppedGallons + invoice.InvoiceModel.DroppedGallons;
                entity.TotalBasicAmount = entity.TotalBasicAmount + invoice.InvoiceModel.BasicAmount;
                entity.TotalFeeAmount = entity.TotalFeeAmount + invoice.InvoiceModel.TotalFeeAmount ?? 0;
                entity.TotalTaxAmount = entity.TotalTaxAmount + invoice.InvoiceModel.TotalTaxAmount;
                entity.TotalDiscountAmount = entity.TotalDiscountAmount + invoice.InvoiceModel.TotalDiscountAmount;
            }

            return entity;
        }

        public static InvoiceHeaderDetail ToInvoiceHeaderEntity(this InvoiceViewModel invoice, InvoiceHeaderDetail entity = null)
        {
            if (entity == null)
            {
                entity = new InvoiceHeaderDetail();
            }
            entity.InvoiceNumberId = invoice.InvoiceNumber.Id;
            entity.TotalDroppedGallons = invoice.DroppedGallons;
            entity.TotalBasicAmount = invoice.BasicAmount;
            entity.TotalFeeAmount = invoice.TotalFees;
            entity.TotalTaxAmount = invoice.TotalTaxAmount;
            entity.TotalDiscountAmount = invoice.TotalDiscountAmount;
            entity.Version = 1;
            entity.CreatedBy = entity.UpdatedBy = invoice.CreatedBy;
            entity.CreatedDate = entity.UpdatedDate = invoice.CreatedDate;
            entity.IsActive = true;
            return entity;
        }


        private static void SetFuelSurchargeDetails(InvoiceModel viewModel, Invoice entity)
        {
            if (viewModel.SurchargeFreightFeeViewModel != null)
            {
                entity.InvoiceXAdditionalDetail.IsSurchargeApplicable = viewModel.SurchargeFreightFeeViewModel.IsSurchargeApplicable;
                if (viewModel.SurchargeFreightFeeViewModel.IsSurchargeApplicable)
                {
                    entity.InvoiceXAdditionalDetail.SurchargeEIAPrice = viewModel.SurchargeFreightFeeViewModel.SurchargeEiaPrice;
                    entity.InvoiceXAdditionalDetail.SurchargePercentage = viewModel.SurchargeFreightFeeViewModel.SurchargePercentage;
                    entity.InvoiceXAdditionalDetail.SurchargePricingType = viewModel.SurchargeFreightFeeViewModel.SurchargePricingType;
                    entity.InvoiceXAdditionalDetail.SurchargeTableRangeEnd = viewModel.SurchargeFreightFeeViewModel.SurchargeTableRangeEnd;
                    entity.InvoiceXAdditionalDetail.SurchargeTableRangeStart = viewModel.SurchargeFreightFeeViewModel.SurchargeTableRangeStart;
                }

                if (viewModel.SurchargeFreightFeeViewModel.FreightPricingMethod == FreightPricingMethod.Auto)
                {
                    SetFuelSurchargeDetailsForAuto(viewModel, entity);
                }
            }
        }

        private static void SetFuelSurchargeDetailsForAuto(InvoiceModel viewModel, Invoice entity)
        {
            if (viewModel.SurchargeFreightFeeViewModel.IsSurchargeApplicable && viewModel.SurchargeFreightFeeViewModel.FuelSurchargeTableId.HasValue && viewModel.SurchargeFreightFeeViewModel.FuelSurchargeTableId.Value > 0)
            {
                entity.InvoiceXAdditionalDetail.FuelSurchargeTableId = viewModel.SurchargeFreightFeeViewModel.FuelSurchargeTableId;
                entity.InvoiceXAdditionalDetail.FuelSurchargeTableType = viewModel.SurchargeFreightFeeViewModel.FuelSurchargeTableType;
            }

            entity.InvoiceXAdditionalDetail.IsFreightCostApplicable = viewModel.SurchargeFreightFeeViewModel.IsFreightCostApplicable;
            if (viewModel.SurchargeFreightFeeViewModel.IsFreightCostApplicable && viewModel.SurchargeFreightFeeViewModel.FreightRateRuleId.HasValue && viewModel.SurchargeFreightFeeViewModel.FreightRateRuleId.Value > 0)
            {
                entity.InvoiceXAdditionalDetail.FreightRateRuleId = viewModel.SurchargeFreightFeeViewModel.FreightRateRuleId;
                entity.InvoiceXAdditionalDetail.FreightRateRuleType = viewModel.SurchargeFreightFeeViewModel.FreightRateRuleType;
                entity.InvoiceXAdditionalDetail.FreightRateTableType = viewModel.SurchargeFreightFeeViewModel.FreightRateTableType;
                entity.InvoiceXAdditionalDetail.Distance = viewModel.SurchargeFreightFeeViewModel.AutoFreightDistance;
            }
        }

        public static AvalaraTaxInputViewModel ToAvaTaxViewModel(this InvoiceViewModel entity, Order order, Job job, AvalaraTaxInputViewModel viewModel = null)
        {
            if (viewModel == null)
            {
                viewModel = new AvalaraTaxInputViewModel(Status.Success);
            }

            if (job.Company.TaxExemptLicenses.Any(t => t.IsActive))
            {
                viewModel.BuyerCustomId = job.Company.TaxExemptLicenses.FirstOrDefault(t => t.IsActive).EntityCustomId;
            }
            if (order.Company.TaxExemptLicenses.Any(t => t.IsActive) && order.IsEndSupplier)
            {
                viewModel.SellerCustomId = order.Company.TaxExemptLicenses.FirstOrDefault(t => t.IsActive).EntityCustomId;
            }
            viewModel.DestinationCity = job.City;
            viewModel.DestinationCountryCode = job.MstCountry.Code;
            viewModel.DestinationJurisdiction = job.MstState.Code;
            viewModel.DestinationPostalCode = job.ZipCode;
            viewModel.DestinationCounty = job.CountyName;
            viewModel.DestinationAddress = job.Address;
            viewModel.EffectiveDate = entity.DropEndDate.Date;
            viewModel.InvoiceDate = entity.CreatedDate.Date;
            viewModel.InvoiceNumber = entity.InvoiceNumber.Number;

            viewModel.JobId = job.Id;
            viewModel.BuyerCompanyId = order.BuyerCompany.Id;
            viewModel.SupplierCompanyId = order.Company.Id;

            if (entity.IsTerminalPickup)
            {
                if (entity.BolDetails != null && entity.BolDetails.NetQuantity != null && entity.BolDetails.NetQuantity > 0)
                    viewModel.NetUnitsDropped = entity.BolDetails.NetQuantity.Value;
                else
                    viewModel.NetUnitsDropped = entity.DroppedGallons;
                if (entity.BolDetails != null && entity.BolDetails.GrossQuantity != null && entity.BolDetails.GrossQuantity > 0)
                    viewModel.GrossUnitsDropped = entity.BolDetails.GrossQuantity.Value;
                else
                    viewModel.GrossUnitsDropped = entity.DroppedGallons;

                viewModel.BilledUnitsDropped = entity.DroppedGallons;
            }
            else
            {
                viewModel.NetUnitsDropped = entity.DroppedGallons;
                viewModel.GrossUnitsDropped = entity.DroppedGallons;
                viewModel.BilledUnitsDropped = entity.DroppedGallons;
            }

            if (entity.IsFTL && entity.IsVariousFobOrigin && entity.DropAddress != null)
            {
                viewModel.DestinationCity = entity.DropAddress.City;
                viewModel.DestinationJurisdiction = entity.DropAddress.State.Code;
                viewModel.DestinationPostalCode = entity.DropAddress.ZipCode;
                viewModel.DestinationCounty = entity.DropAddress.CountyName;
                viewModel.DestinationAddress = entity.DropAddress.Address;
            }

            if (!entity.IsTerminalPickup && entity.FuelPickLocation != null)
            {
                viewModel.OriginCity = entity.FuelPickLocation.City;
                viewModel.OriginCountryCode = entity.FuelPickLocation.CountryCode;
                viewModel.OriginJurisdiction = entity.FuelPickLocation.StateCode;
                viewModel.OriginPostalCode = entity.FuelPickLocation.ZipCode;
                viewModel.OriginCounty = entity.FuelPickLocation.CountyName;
                viewModel.OriginAddress = entity.FuelPickLocation.Address;

            }
            else
            {
                var terminalDetails = ContextFactory.Current.GetDomain<HelperDomain>().GetExternalTerminal(entity.TerminalId ?? order.TerminalId ?? 0);
                if (terminalDetails != null)
                {
                    viewModel.OriginCity = terminalDetails.City;
                    viewModel.OriginCountryCode = terminalDetails.CountryCode;
                    viewModel.OriginJurisdiction = terminalDetails.StateCode;
                    viewModel.OriginPostalCode = terminalDetails.ZipCode;
                    viewModel.OriginCounty = terminalDetails.CountyName;
                    viewModel.OriginAddress = terminalDetails.Address;
                }
            }
            viewModel.ProductCode = order.FuelRequest.MstProduct.ProductCode;
            viewModel.UnitPrice = entity.PricePerGallon;
            viewModel.Currency = order.FuelRequest.Currency;

            var uom = order.FuelRequest.UoM;
            if (uom == UoM.MetricTons || uom == UoM.Barrels)
            {
                if (viewModel.DestinationCountryCode == "CAN")
                    uom = UoM.Litres;
                else
                    uom = UoM.Gallons;
            }
            viewModel.UoM = uom;

            return viewModel;
        }

        public static ExternalDropDetail ToExternalDropDetailEntity(this ExternalDropDetailViewModel viewModel, ExternalDropDetail entity = null)
        {
            if (entity == null)
            {
                entity = new ExternalDropDetail();
            }

            entity.Id = viewModel.Id;
            entity.OrderId = viewModel.OrderId;
            entity.TrackableScheduleId = viewModel.TrackableScheduleId;
            entity.DropStartLatitude = viewModel.DropStartLatitude;
            entity.DropStartLongitude = viewModel.DropStartLongitude;
            entity.DropEndLatitude = viewModel.DropEndLatitude;
            entity.DropEndLongitude = viewModel.DropEndLongitude;
            entity.DropStartDate = DateTimeOffset.FromUnixTimeMilliseconds(viewModel.DropStartDate);
            entity.DropEndDate = DateTimeOffset.FromUnixTimeMilliseconds(viewModel.DropEndDate);
            entity.UserId = viewModel.UserId;
            entity.DropDate = entity.DropEndDate;
            entity.IsActive = true;
            return entity;
        }

        public static ApiInvoiceViewModel ToApiInvoiceViewModel(this Invoice entity, ApiInvoiceViewModel viewModel = null)
        {
            if (viewModel == null)
            {
                viewModel = new ApiInvoiceViewModel();
            }

            viewModel.InvoiceNumber.Id = entity.InvoiceHeader.InvoiceNumberId;
            viewModel.InvoiceNumber.Number = entity.DisplayInvoiceNumber;
            viewModel.InvoiceTypeId = entity.InvoiceTypeId;
            viewModel.DroppedGallons = entity.DroppedGallons;
            viewModel.PricePerGallon = entity.InvoiceXBolDetails.Select(t => t.InvoiceFtlDetail.PricePerGallon).FirstOrDefault();
            viewModel.DropStartDate = entity.DropStartDate;
            viewModel.DropEndDate = entity.DropEndDate;
            viewModel.BasicAmount = entity.BasicAmount;
            viewModel.RackPrice = entity.InvoiceXBolDetails.Select(t => t.InvoiceFtlDetail.RackPrice).FirstOrDefault();
            var status = entity.InvoiceXInvoiceStatusDetails.FirstOrDefault(t => t.IsActive);
            viewModel.StatusId = status.StatusId;
            viewModel.TotalTaxAmount = entity.TotalTaxAmount;
            viewModel.UnitOfMeasurement = (int)entity.UoM;
            viewModel.Currency = (int)entity.Currency;
            viewModel.InvoiceHeaderId = entity.InvoiceHeaderId;
            return viewModel;
        }

        public static AvalaraTaxInputViewModel ToAvalaraTaxViewModel(this AvalaraServiceViewModel entity, InvoiceCreateViewModel invoiceCreateViewModel, BolDetailViewModel bolDetails, List<FuelFeeViewModel> fuelFees)
        {

            var viewModel = new AvalaraTaxInputViewModel(Status.Success);
            viewModel.BuyerCustomId = entity.BuyerCustomId;
            viewModel.SellerCustomId = entity.SellerCustomId;
            viewModel.DestinationCity = entity.DestinationJobAddress.City;
            viewModel.DestinationCountryCode = entity.DestinationJobAddress?.CountryCode;
            viewModel.DestinationJurisdiction = entity.DestinationJobAddress?.StateCode;
            viewModel.DestinationPostalCode = entity.DestinationJobAddress?.ZipCode;
            viewModel.DestinationCounty = entity.DestinationJobAddress?.CountyName;
            viewModel.DestinationAddress = entity.DestinationJobAddress?.Address;
            viewModel.EffectiveDate = entity.DropEndDate.Date;
            viewModel.InvoiceDate = entity.InvoiceDate.Date;
            viewModel.InvoiceNumber = entity.InvoiceNumber;
            if (invoiceCreateViewModel.IsTerminalPickup)
            {
                viewModel.NetUnitsDropped = bolDetails.NetQuantity.HasValue && bolDetails.NetQuantity > 0 ? bolDetails.NetQuantity.Value : entity.DroppedGallons;
                viewModel.GrossUnitsDropped = bolDetails.GrossQuantity.HasValue && bolDetails.GrossQuantity > 0 ? bolDetails.GrossQuantity.Value : entity.DroppedGallons;
                viewModel.BilledUnitsDropped = entity.DroppedGallons;
            }
            else
            {
                viewModel.NetUnitsDropped = entity.DroppedGallons;
                viewModel.GrossUnitsDropped = entity.DroppedGallons;
                viewModel.BilledUnitsDropped = entity.DroppedGallons;
            }
            viewModel.OriginCity = entity.SourceTerminalAddress?.City;
            viewModel.OriginCountryCode = entity.SourceTerminalAddress?.CountryCode;
            viewModel.OriginJurisdiction = entity.SourceTerminalAddress?.StateCode;
            viewModel.OriginPostalCode = entity.SourceTerminalAddress?.ZipCode;
            viewModel.OriginCounty = entity.SourceTerminalAddress?.CountyName;
            viewModel.OriginAddress = entity.SourceTerminalAddress?.Address;
            viewModel.ProductCode = entity.FuelProductCode;
            viewModel.UnitPrice = entity.PricePerGallon;
            viewModel.Currency = entity.JobCurrency;
            var uom = entity.JobUoM;
            if (uom == UoM.MetricTons || uom == UoM.Barrels)
            {
                if (viewModel.DestinationCountryCode == "CAN")
                    uom = UoM.Litres;
                else
                    uom = UoM.Gallons;
            }
            viewModel.UoM = uom;
            viewModel.Exclusions = entity.Exclusions;
            viewModel.IsDirectTaxCompany = entity.IsDirectTaxCompany;
            viewModel.SupplierAllowance = invoiceCreateViewModel.SupplierAllowance;
            viewModel.BuyerCompanyId = entity.BuyerCompanyId;
            viewModel.SupplierCompanyId = entity.SupplierCompanyId;
            viewModel.JobId = entity.JobId;
            fuelFees.ToAvaTransactionLineItem(viewModel);

            return viewModel;
        }

        public static AvalaraTaxInputViewModel ToFtlAvalaraTaxViewModel(this AvalaraServiceViewModel entity, InvoiceCreateViewModel invoiceCreateViewModel, BolDetailViewModel bolDetails, List<FuelFeeViewModel> fuelFees)
        {
            var viewModel = new AvalaraTaxInputViewModel(Status.Success);
            viewModel.BuyerCustomId = entity.BuyerCustomId;
            viewModel.SellerCustomId = entity.SellerCustomId;
            viewModel.DestinationCity = entity.DestinationJobAddress.City;
            viewModel.DestinationCountryCode = entity.DestinationJobAddress.CountryCode;
            viewModel.DestinationJurisdiction = entity.DestinationJobAddress.StateCode;
            viewModel.DestinationPostalCode = entity.DestinationJobAddress.ZipCode;
            viewModel.DestinationCounty = entity.DestinationJobAddress.CountyName;
            viewModel.DestinationAddress = entity.DestinationJobAddress.Address;
            viewModel.EffectiveDate = entity.DropEndDate.Date;
            viewModel.InvoiceDate = entity.InvoiceDate.Date;
            viewModel.InvoiceNumber = entity.InvoiceNumber;
            //set state net/gross
            if (invoiceCreateViewModel.IsTerminalPickup)
            {
                viewModel.NetUnitsDropped = bolDetails.NetQuantity ?? entity.DroppedGallons;
                viewModel.GrossUnitsDropped = bolDetails.GrossQuantity ?? entity.DroppedGallons;
                viewModel.BilledUnitsDropped = entity.DroppedGallons;
            }
            else
            {
                viewModel.NetUnitsDropped = entity.DroppedGallons;
                viewModel.GrossUnitsDropped = entity.DroppedGallons;
                viewModel.BilledUnitsDropped = entity.DroppedGallons;
            }
            viewModel.OriginCity = entity.SourceTerminalAddress.City;
            viewModel.OriginCountryCode = entity.SourceTerminalAddress.CountryCode;
            viewModel.OriginJurisdiction = entity.SourceTerminalAddress.StateCode;
            viewModel.OriginPostalCode = entity.SourceTerminalAddress.ZipCode;
            viewModel.OriginCounty = entity.SourceTerminalAddress.CountyName;
            viewModel.OriginAddress = entity.SourceTerminalAddress.Address;
            viewModel.ProductCode = entity.FuelProductCode;
            viewModel.UnitPrice = entity.PricePerGallon;
            viewModel.Currency = entity.JobCurrency;

            var uom = entity.JobUoM;
            if (uom == UoM.MetricTons || uom == UoM.Barrels)
            {
                if (viewModel.DestinationCountryCode == "CAN")
                    uom = UoM.Litres;
                else
                    uom = UoM.Gallons;
            }
            viewModel.UoM = uom;

            viewModel.SupplierAllowance = invoiceCreateViewModel.SupplierAllowance;
            viewModel.IsDirectTaxCompany = invoiceCreateViewModel.IsDirectTaxCompany;
            viewModel.IsFobOrigin = invoiceCreateViewModel.IsVariousFobOrigin;
            viewModel.Exclusions = entity.Exclusions;
            viewModel.JobId = entity.JobId;
            viewModel.BuyerCompanyId = entity.BuyerCompanyId;
            viewModel.SupplierCompanyId = entity.SupplierCompanyId;
            fuelFees.ToAvaTransactionLineItem(viewModel);

            return viewModel;
        }
        public static AvalaraTaxMultipleInputViewModel ToAvalaraTaxMultipleInputViewModel(this List<InvoiceModel> invoiceModels, AvalaraServiceViewModel avaInput)
        {

            var viewModel = new AvalaraTaxMultipleInputViewModel(Status.Success);
            var inv = invoiceModels.FirstOrDefault();
            viewModel.BuyerCustomId = avaInput.BuyerCustomId;
            viewModel.SellerCustomId = avaInput.SellerCustomId;

            viewModel.Currency = inv.Currency;
            viewModel.EffectiveDate = inv.DropEndDate.Date;
            viewModel.InvoiceDate = inv.CreatedDate.Date;
            viewModel.InvoiceNumber = inv.DisplayInvoiceNumber;
            viewModel.Exclusions = avaInput.Exclusions;
            viewModel.JobId = inv.AdditionalDetail.JobId ?? 0;
            viewModel.IsDirectTaxCompany = avaInput.IsDirectTaxCompany;
            viewModel.IsFobOrigin = inv.IsVariousFobOrigin;
            viewModel.InputTransactionLines = new List<AvalaraInputTransactionsViewModel>();
            // for loop for transactions
            foreach (var item in invoiceModels)
            {
                var transactionLine = new AvalaraInputTransactionsViewModel();
                transactionLine.DestinationCity = item.FuelDropLocation.City;
                transactionLine.DestinationCountryCode = item.FuelDropLocation.CountryCode;
                transactionLine.DestinationJurisdiction = item.FuelDropLocation.StateCode;
                transactionLine.DestinationPostalCode = item.FuelDropLocation.ZipCode;
                transactionLine.DestinationCounty = item.FuelDropLocation.CountyName;
                transactionLine.DestinationAddress = item.FuelDropLocation.Address;
                transactionLine.ProductCode = item.FuelProductCode;
                transactionLine.SupplierAllowance = item.AdditionalDetail?.SupplierAllowance ?? 0;
                var uom = item.UoM;
                if (uom == UoM.MetricTons || uom == UoM.Barrels)
                {
                    if (item.FuelDropLocation.CountryCode == "CAN")
                        uom = UoM.Litres;
                    else
                        uom = UoM.Gallons;
                }
                transactionLine.UoM = uom;

                var bol = item.BolDetails.FirstOrDefault();
                transactionLine.OriginCity = bol.City;
                transactionLine.OriginCountryCode = bol.CountryCode;
                transactionLine.OriginJurisdiction = bol.StateCode;
                transactionLine.OriginPostalCode = bol.ZipCode;
                transactionLine.OriginCounty = bol.CountyName;
                transactionLine.OriginAddress = bol.Address;
                transactionLine.UnitPrice = item.PricePerGallon != 0 ? item.PricePerGallon : bol.PricePerGallon;

                transactionLine.BilledUnitsDropped = item.DroppedGallons;
                transactionLine.NetUnitsDropped = item.DroppedGallons;
                transactionLine.GrossUnitsDropped = item.DroppedGallons;
                if (item.BolDetails.Any())
                {
                    if (!bol.TierPricingForBol.Any())
                    {
                        transactionLine.NetUnitsDropped = 0;
                        transactionLine.GrossUnitsDropped = 0;
                        foreach (var bolValue in item.BolDetails)
                        {
                            transactionLine.NetUnitsDropped += getDroppedQuantity(bolValue.NetQuantity, bolValue.LiftQuantity, item.DroppedGallons);
                            transactionLine.GrossUnitsDropped += getDroppedQuantity(bolValue.GrossQuantity, bolValue.LiftQuantity, item.DroppedGallons);
                        }
                        //if (item.TaxQuantityIndicatorTypeId == (int)QuantityIndicatorTypes.Gross)
                        //    transactionLine.BilledUnitsDropped = transactionLine.GrossUnitsDropped;
                        //else
                        //    transactionLine.BilledUnitsDropped = transactionLine.NetUnitsDropped;
                    }
                }
                transactionLine.BuyerCompanyId = avaInput.BuyerCompanyId;
                transactionLine.SupplierCompanyId = avaInput.SupplierCompanyId;
                viewModel.InputTransactionLines.Add(transactionLine);
            }

            AddFeeLineItems(invoiceModels, avaInput, viewModel);

            return viewModel;
        }

        public static void AddFeeLineItems(List<InvoiceModel> invoiceModels, AvalaraServiceViewModel avaInput, AvalaraTaxMultipleInputViewModel viewModel)
        {
            var isFeeTaxEnable = ApplicationDomain.ApplicationSettings.GetApplicationSettingValue<int>(ApplicationConstants.AvaFeeTax);
            if (isFeeTaxEnable != 1)
                return;

            foreach (var item in invoiceModels)
            {
                decimal droppedGallons = 0;
                if (item.BolDetails.Count > 0)
                {
                    foreach (var bolValue in item.BolDetails)
                    {
                        droppedGallons += getDroppedQuantity(bolValue.NetQuantity, bolValue.LiftQuantity, item.DroppedGallons);
                    }
                }
                else
                {
                    droppedGallons = item.DroppedGallons;
                }
                var applicableFees = item.FuelFees.Where(t => !t.IncludeInPPG && !(t.FeeTypeId == (int)FeeType.ProcessingFee || t.FeeTypeId == (int)FeeType.DryRunFee)).ToList();
                foreach (var fee in applicableFees)
                {
                    var transactionLine = new AvalaraInputTransactionsViewModel();
                    transactionLine.DestinationCity = item.FuelDropLocation.City;
                    transactionLine.DestinationCountryCode = item.FuelDropLocation.CountryCode;
                    transactionLine.DestinationJurisdiction = item.FuelDropLocation.StateCode;
                    transactionLine.DestinationPostalCode = item.FuelDropLocation.ZipCode;
                    transactionLine.DestinationCounty = item.FuelDropLocation.CountyName;
                    transactionLine.DestinationAddress = item.FuelDropLocation.Address;
                    //Below condition is temporary added , need to remove when FreightCost Fee is mapped in Avalara.
                    transactionLine.ProductCode = fee.FeeTypeId == (int)FeeType.FreightCost ? "Fee_" + (int)FeeType.FreightFee : "Fee_" + fee.FeeTypeId;
                    transactionLine.UoM = UoM.Gallons;

                    var bol = item.BolDetails.FirstOrDefault();
                    transactionLine.OriginCity = bol.City;
                    transactionLine.OriginCountryCode = bol.CountryCode;
                    transactionLine.OriginJurisdiction = bol.StateCode;
                    transactionLine.OriginPostalCode = bol.ZipCode;
                    transactionLine.OriginCounty = bol.CountyName;
                    transactionLine.OriginAddress = bol.Address;

                    if (fee.FeeSubTypeId == (int)FeeSubType.ByQuantity)
                    {
                        droppedGallons = item.DroppedGallons;
                    }

                    if (GetAdditionalDetailPerFeetype(fee, transactionLine, droppedGallons))
                    {
                        transactionLine.BillOfLaddingNumber = ((FeeType)fee.FeeTypeId).GetDisplayName();
                        transactionLine.BillOfLaddingDate = DateTimeOffset.Now.DateTime;
                    }
                    else
                    {
                        continue;
                    }

                    transactionLine.BuyerCompanyId = avaInput.BuyerCompanyId;
                    transactionLine.SupplierCompanyId = avaInput.SupplierCompanyId;
                    viewModel.InputTransactionLines.Add(transactionLine);
                }
            }
        }

        public static void ToAvaTransactionLineItem(this List<FuelFeeViewModel> fuelFees, AvalaraTaxInputViewModel taxInputViewModel)
        {
            var isFeeTaxEnable = ApplicationDomain.ApplicationSettings.GetApplicationSettingValue<int>(ApplicationConstants.AvaFeeTax);
            if (isFeeTaxEnable != 1)
                return;

            if (fuelFees != null && fuelFees.Any() && taxInputViewModel.FeeTransactionLines == null)
                taxInputViewModel.FeeTransactionLines = new List<AvalaraInputTransactionsViewModel>();

            var applicableFees = fuelFees.Where(t => !t.IncludeInPPG && !(t.FeeTypeId == (int)FeeType.ProcessingFee || t.FeeTypeId == (int)FeeType.DryRunFee)).ToList();
            foreach (var fee in applicableFees)
            {
                var transactionLine = new AvalaraInputTransactionsViewModel();
                transactionLine.DestinationCity = taxInputViewModel.DestinationCity;
                transactionLine.DestinationCountryCode = taxInputViewModel.DestinationCountryCode;
                transactionLine.DestinationJurisdiction = taxInputViewModel.DestinationJurisdiction;
                transactionLine.DestinationPostalCode = taxInputViewModel.DestinationPostalCode;
                transactionLine.DestinationCounty = taxInputViewModel.DestinationCounty;
                transactionLine.DestinationAddress = taxInputViewModel.DestinationAddress;
                transactionLine.ProductCode = "Fee_" + fee.FeeTypeId;
                transactionLine.UoM = UoM.Gallons;

                transactionLine.OriginCity = taxInputViewModel.OriginCity;
                transactionLine.OriginCountryCode = taxInputViewModel.OriginCountryCode;
                transactionLine.OriginJurisdiction = taxInputViewModel.OriginJurisdiction;
                transactionLine.OriginPostalCode = taxInputViewModel.OriginPostalCode;
                transactionLine.OriginCounty = taxInputViewModel.OriginCounty;
                transactionLine.OriginAddress = taxInputViewModel.OriginAddress;

                if (GetAdditionalDetailPerFeetype(fee, transactionLine, taxInputViewModel.BilledUnitsDropped))
                {
                    transactionLine.BillOfLaddingNumber = ((FeeType)fee.FeeTypeId).GetDisplayName();
                    transactionLine.BillOfLaddingDate = DateTimeOffset.Now.DateTime;
                }
                else
                {
                    continue;
                }
                transactionLine.BuyerCompanyId = taxInputViewModel.BuyerCompanyId;
                transactionLine.SupplierCompanyId = taxInputViewModel.SupplierCompanyId;

                taxInputViewModel.FeeTransactionLines.Add(transactionLine);
            }
        }

        private static bool GetAdditionalDetailPerFeetype(FuelFeeViewModel fee, AvalaraInputTransactionsViewModel transactionLine, decimal droppedGallons)
        {
            var response = true;
            if (fee.FeeTypeId == (int)FeeType.UnderGallonFee)
            {
                if (fee.MinimumGallons <= droppedGallons)
                    return false;
            }

            switch (fee.FeeSubTypeId)
            {
                case (int)FeeSubType.FlatFee:
                    transactionLine.UnitPrice = fee.Fee;
                    transactionLine.BilledUnitsDropped = 1;
                    transactionLine.NetUnitsDropped = 1;
                    transactionLine.GrossUnitsDropped = 1;
                    break;

                case (int)FeeSubType.HourlyRate:
                    transactionLine.UnitPrice = fee.Fee;
                    var TotalHours = (decimal)(Convert.ToDouble(fee.FeeSubQuantity) / 3600);
                    if (TotalHours == 0)
                        return false;
                    transactionLine.BilledUnitsDropped = TotalHours;
                    transactionLine.NetUnitsDropped = TotalHours;
                    transactionLine.GrossUnitsDropped = TotalHours;
                    break;

                case (int)FeeSubType.PerGallon:
                    transactionLine.UnitPrice = fee.Fee;
                    transactionLine.BilledUnitsDropped = droppedGallons;
                    transactionLine.NetUnitsDropped = droppedGallons;
                    transactionLine.GrossUnitsDropped = droppedGallons;
                    break;

                case (int)FeeSubType.ByAssetCount:
                    if (!fee.FeeSubQuantity.HasValue || fee.FeeSubQuantity.Value == 0)
                        return false;
                    transactionLine.UnitPrice = fee.Fee;
                    transactionLine.BilledUnitsDropped = (fee.FeeSubQuantity ?? 0);
                    transactionLine.NetUnitsDropped = (fee.FeeSubQuantity ?? 0);
                    transactionLine.GrossUnitsDropped = (fee.FeeSubQuantity ?? 0);
                    break;

                case (int)FeeSubType.ByQuantity:
                    FuelFeesDomain.CalculateAndSetByQuantityFee(droppedGallons, fee);
                    if (fee.TotalFee.HasValue && fee.TotalFee.Value > 0)
                    {
                        transactionLine.UnitPrice = fee.TotalFee.Value;
                        transactionLine.BilledUnitsDropped = 1;
                        transactionLine.NetUnitsDropped = 1;
                        transactionLine.GrossUnitsDropped = 1;
                    }
                    else
                    {
                        response = false;
                    }
                    break;

                case (int)FeeSubType.Percent:
                    transactionLine.UnitPrice = fee.Fee / 100;
                    transactionLine.BilledUnitsDropped = droppedGallons;
                    transactionLine.NetUnitsDropped = droppedGallons;
                    transactionLine.GrossUnitsDropped = droppedGallons;
                    break;

                default: //perRoute,Flatfee
                    transactionLine.UnitPrice = fee.Fee;
                    transactionLine.BilledUnitsDropped = 1;
                    transactionLine.NetUnitsDropped = 1;
                    transactionLine.GrossUnitsDropped = 1;
                    break;
            }
            return response;
        }

        private static decimal getDroppedQuantity(decimal? quantity, decimal? liftQuantity, decimal billedQuantity)
        {
            if (quantity.HasValue && quantity.Value > 0)
                return quantity.Value;
            if (liftQuantity.HasValue && liftQuantity.Value > 0)
                return liftQuantity.Value;
            return billedQuantity;
        }
        public static ManualInvoiceCreateRequestViewModel ToManualInvoiceViewModel(this ManualInvoiceViewModel entity)
        {
            var viewModel = new ManualInvoiceCreateRequestViewModel(Status.Success);
            viewModel.DropStartDate = new DateTimeOffset(entity.DeliveryDate.Add(Convert.ToDateTime(entity.StartTime).TimeOfDay));
            viewModel.DropEndDate = new DateTimeOffset(entity.DeliveryDate.Add(Convert.ToDateTime(entity.EndTime).TimeOfDay));
            viewModel.FuelDropped = entity.FuelDropped ?? 0.0M;

            viewModel.PaymentTermId = entity.PaymentTermId;
            viewModel.NetDays = entity.NetDays;
            viewModel.IsWetHosingDelivery = entity.FuelDeliveryDetails.FuelFees.FuelRequestFees.Any(t => t.FeeTypeId == ((int)FeeType.WetHoseFee).ToString() && t.DiscountLineItemId == null);
            viewModel.IsOverWaterDelivery = entity.FuelDeliveryDetails.FuelFees.FuelRequestFees.Any(t => t.FeeTypeId == ((int)FeeType.OverWaterFee).ToString() && t.DiscountLineItemId == null);
            if (entity.IsThirdPartyHardwareUsed)
            {
                viewModel.ExternalBrokeredOrder.BrokeredOrderFee = entity.ExternalBrokeredOrder.BrokeredOrderFee;
            }
            else
            {
                viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees = entity.FuelDeliveryDetails.FuelFees.FuelRequestFees;
                if (entity.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.IsSurchargeApplicable)
                {
                    viewModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee = entity.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee;
                    viewModel.FuelDeliveryDetails.FuelFees.FuelSurchargeFreightFee.GallonsDelivered = viewModel.FuelDropped.Value;
                }
            }

            viewModel.InvoiceTypeId = entity.InvoiceTypeId;
            viewModel.UserId = entity.userId;
            viewModel.OrderId = entity.OrderId;
            viewModel.InvoiceImage = entity.InvoiceImage;
            if (entity.SignatureImage != null && !string.IsNullOrWhiteSpace(entity.SignatureImage?.FilePath))
                viewModel.Signature = entity.SignatureImage?.ToCustomerSignature();
            viewModel.BolImage = entity.BolImage;
            viewModel.AdditionalImage = entity.AdditionalImage;
            viewModel.TaxAffidavitImage = entity.TaxAffidavitImage;
            viewModel.BDNImage = entity.BDNImage;
            viewModel.CoastGuardInspectionImage = entity.CoastGuardInspectionImage;
            viewModel.InspectionRequestVoucherImage = entity.InspectionRequestVoucherImage;
            viewModel.DriverId = entity.DriverId;
            viewModel.TrackableScheduleId = entity.TrackableScheduleId;
            viewModel.InvoiceStatusId = entity.StatusId;
            viewModel.CsvFilePath = entity.CsvFilePath;
            viewModel.AssetDrops = entity.Assets;
            viewModel.BolDetails = entity.BolDetails;
            viewModel.IsVariousFobOrigin = entity.IsVariousFobOrigin;
            viewModel.DropLocation = entity.ToDropLocation();
            if (entity.PickUpAddress != null && entity.PickUpAddress.IsAddressAvailable)
            {
                viewModel.PickupLocation = entity.ToPickUpLocation();
            }
            viewModel.InvoiceStatusId = entity.StatusId;
            if (entity.SplitLoadSequence.HasValue)
            {
                viewModel.AdditionalDetail = new InvoiceXAdditionalDetailViewModel()
                {
                    SplitLoadSequence = entity.SplitLoadSequence,
                    SplitLoadChainId = entity.SplitLoadChainId
                };
            }
            if (viewModel.AdditionalDetail == null)
            {
                viewModel.AdditionalDetail = new InvoiceXAdditionalDetailViewModel();
                viewModel.AdditionalDetail.ActualDropQuantity = entity.FuelDropped ?? 0;
            }
            if (!string.IsNullOrWhiteSpace(entity.Notes))
            {
                viewModel.AdditionalDetail.Notes = entity.Notes;
            }
            viewModel.AdditionalDetail.PaymentMethod = entity.PaymentMethod;
            if (entity.TankFrequency != null && entity.TankFrequency.Tanks.Any())
                viewModel.AdditionalDetail.TankFrequencyId = entity.TankFrequency.Tanks.Select(t => t.BillingFrequencyId).FirstOrDefault();
            viewModel.AdditionalDetail.TruckNumber = entity.TruckNumber;
            viewModel.AdditionalDetail.DropTicketNumber = entity.DropTicketNumber;
            viewModel.TankFrequency = entity.TankFrequency;
            viewModel.CreationMethod = entity.CreationMethod;
            viewModel.IsTerminalPickup = entity.PickUpAddress == null || !entity.PickUpAddress.IsAddressAvailable || string.IsNullOrEmpty(entity.PickUpAddress.Address);
            if ((viewModel.CreationMethod == CreationMethod.BulkUploaded || viewModel.CreationMethod == CreationMethod.APIUpload) && entity.FuelCost.HasValue)
            {
                viewModel.CurrentCost = entity.FuelCost.Value;
                viewModel.SupplierCost = entity.FuelCost.Value;
            }
            return viewModel;
        }

        public static List<InvoiceFtlDetail> ToBolDetailsEntity(this InvoiceViewModel invoiceViewModel)
        {
            List<InvoiceFtlDetail> invoiceFtls = null;
            if (invoiceViewModel.BolDetails != null)
            {
                var viewModel = invoiceViewModel.BolDetails;
                invoiceFtls = new List<InvoiceFtlDetail>();

                var entity = new InvoiceFtlDetail
                {
                    Id = viewModel.Id,
                    GrossQuantity = viewModel.GrossQuantity,
                    NetQuantity = viewModel.NetQuantity,
                    BolNumber = viewModel.BolNumber,
                    Carrier = viewModel.Carrier,
                    CreatedBy = viewModel.CreatedBy,
                    CreatedDate = viewModel.CreatedDate,
                    LiftDate = viewModel.LiftDate,
                    BolCreationTime = viewModel.BolCreationTime,
                    LiftTicketNumber = viewModel.LiftTicketNumber,
                    LiftArrivalTime = viewModel.LiftArrivalTime,
                    LiftStartTime = Convert.ToDateTime(viewModel.StartTime).TimeOfDay,
                    LiftEndTime = Convert.ToDateTime(viewModel.EndTime).TimeOfDay,
                    LiftQuantity = viewModel.LiftQuantity
                };

                if (invoiceViewModel.FuelPickLocation != null && invoiceViewModel.FuelPickLocation.ZipCode != null)
                    SetPickupAddressToBolDetails(entity, invoiceViewModel.FuelPickLocation);

                if (viewModel.ImageId > 0)
                    entity.ImageId = viewModel.ImageId;

                if (invoiceViewModel.BolImage != null && !string.IsNullOrWhiteSpace(invoiceViewModel.BolImage?.FilePath))
                    entity.Image = invoiceViewModel.BolImage.ToEntity();

                invoiceFtls.Add(entity);
            }

            return invoiceFtls;
        }

        public static void SetPickupAddressToBolDetails(InvoiceFtlDetail entity, DispatchLocationViewModel pickUplocation)
        {
            entity.Address = pickUplocation.Address;
            entity.City = pickUplocation.City;
            entity.StateCode = pickUplocation.StateCode;
            entity.StateId = pickUplocation.StateId;
            entity.ZipCode = pickUplocation.ZipCode;
            entity.Latitude = pickUplocation.Latitude;
            entity.Longitude = pickUplocation.Longitude;
            entity.CountryCode = pickUplocation.CountryCode;
            entity.CountyName = pickUplocation.CountyName;
            entity.SiteName = pickUplocation.SiteName;
            entity.PickupLocation = pickUplocation.PickupLocationType;
        }

        public static List<InvoiceFtlDetail> ToBolDetailsEntity(this InvoiceModel invoiceModel)
        {
            List<InvoiceFtlDetail> invoiceFtls = null;
            if (invoiceModel.BolDetails != null)
            {
                var viewModel = invoiceModel.BolDetails.First();
                invoiceFtls = new List<InvoiceFtlDetail>();

                var entity = new InvoiceFtlDetail
                {
                    Id = viewModel.Id,
                    GrossQuantity = viewModel.GrossQuantity,
                    NetQuantity = viewModel.NetQuantity,
                    BolNumber = viewModel.BolNumber,
                    Carrier = viewModel.Carrier,
                    CreatedBy = viewModel.CreatedBy,
                    CreatedDate = viewModel.CreatedDate,
                    LiftDate = viewModel.LiftDate,
                    BolCreationTime = viewModel.BolCreationTime,
                    LiftTicketNumber = viewModel.LiftTicketNumber,
                    LiftArrivalTime = viewModel.LiftArrivalTime,
                    LiftStartTime = Convert.ToDateTime(viewModel.StartTime).TimeOfDay,
                    LiftEndTime = Convert.ToDateTime(viewModel.EndTime).TimeOfDay,
                    LiftQuantity = viewModel.LiftQuantity
                };

                if (invoiceModel.FuelPickLocation != null && invoiceModel.FuelPickLocation.ZipCode != null)
                    SetPickupAddressToBolDetails(entity, invoiceModel.FuelPickLocation);

                if (invoiceModel.BolImage != null && invoiceModel.BolImage.Id > 0)
                    entity.ImageId = invoiceModel.BolImage.Id;

                if (invoiceModel.BolImage != null && !string.IsNullOrWhiteSpace(invoiceModel.BolImage?.FilePath))
                    entity.Image = invoiceModel.BolImage.ToEntity();

                invoiceFtls.Add(entity);
            }

            return invoiceFtls;
        }

        public static ConversionNewsfeedViewModel ToNewsfeedViewModel(this InvoiceModel invoiceModel)
        {
            ConversionNewsfeedViewModel viewModel = null;
            if (invoiceModel != null)
            {
                viewModel = new ConversionNewsfeedViewModel();
                viewModel.CreatedDate = invoiceModel.CreatedDate;
                viewModel.InvoiceId = invoiceModel.Id;
                viewModel.InvoiceHeaderId = invoiceModel.InvoiceHeaderId;
                viewModel.InvoiceNumber = invoiceModel.DisplayInvoiceNumber;
                viewModel.InvoiceNumberId = invoiceModel.InvoiceNumberId;
                viewModel.OrderId = invoiceModel.OrderId.Value;
                viewModel.SupplierCompanyId = invoiceModel.UpdatedByCompanyId;
                viewModel.UserId = invoiceModel.CreatedBy;
            }
            return viewModel;
        }

        public static InvoiceModel ToApiInvoiceDropViewModel(this InvoiceDropViewModel drop, TPDInvoiceViewModel apiRequestModel, InvoiceModel invoiceModel = null)
        {
            if (invoiceModel == null)
            {
                invoiceModel = new InvoiceModel();
            }

            invoiceModel.DroppedGallons = drop.ActualDropQuantity;
            invoiceModel.DropStartDate = new DateTimeOffset(drop.DropDate.Date.Add(Convert.ToDateTime(drop.StartTime).TimeOfDay)); //CHECK ENDDATE AND SET HERE
            if (drop.DropEndDate != null && drop.DropEndDate.HasValue)
                invoiceModel.DropEndDate = drop.DropEndDate.Value.Add(Convert.ToDateTime(drop.EndTime).TimeOfDay);
            else
                invoiceModel.DropEndDate = new DateTimeOffset(drop.DropDate.Date.Add(Convert.ToDateTime(drop.EndTime).TimeOfDay));

            invoiceModel.DriverId = apiRequestModel.DriverId;
            invoiceModel.CurrentCost = drop.SupplierFuelCost;
            invoiceModel.DisplayInvoiceNumber = apiRequestModel.SupplierInvoiceNumber;
            invoiceModel.TransactionId = apiRequestModel.SupplierInvoiceNumber;
            invoiceModel.PoNumber = drop.PoNumber;

            if (invoiceModel.AdditionalDetail == null)
                invoiceModel.AdditionalDetail = new InvoiceXAdditionalDetailViewModel();

            invoiceModel.AdditionalDetail.DropTicketNumber = drop.DropTicketNumber;
            invoiceModel.AdditionalDetail.SupplierAllowance = drop.Allowance;
            if (invoiceModel.AdditionalDetail.SupplierAllowance.HasValue)
            {
                invoiceModel.AdditionalDetail.SupplierAllowance = Math.Round(drop.Allowance.Value, ApplicationConstants.InvoiceSuppplierAllowanceUnitPriceDecimalDisplay);
            }
            invoiceModel.AdditionalDetail.TotalAllowance = Math.Round(drop.ActualDropQuantity * invoiceModel.AdditionalDetail.SupplierAllowance ?? 0, ApplicationConstants.InvoiceSuppplierAllowanceTotalDecimalDisplay);
            invoiceModel.AdditionalDetail.PaymentMethod = PaymentMethods.None;
            invoiceModel.AdditionalDetail.CarrierOrderId = drop.CarrierOrderId;
            invoiceModel.AdditionalDetail.CarrierOrder = drop.CarrierOrder;
            invoiceModel.AdditionalDetail.OrderDate = drop.OrderDate;
            invoiceModel.AdditionalDetail.OrderQuantity = drop.OrderQuantity;
            invoiceModel.AdditionalDetail.LoadingBadge = drop.LoadingBadge;
            invoiceModel.AdditionalDetail.Tracktor = drop.Tractor;
            invoiceModel.AdditionalDetail.TruckNumber = drop.Truck;
            invoiceModel.AdditionalDetail.ExternalRefId = apiRequestModel.ExternalRefID;
            invoiceModel.AdditionalDetail.Notes = apiRequestModel.Notes;
            invoiceModel.AdditionalDetail.CreationMethod = CreationMethod.APIUpload;

            return invoiceModel;
        }
        public static PDITaxDetailsViewModel ToPDITaxDetailsViewModel(this PDITaxFTPViewModel item)
        {
            var model = new PDITaxDetailsViewModel();
            model.PDIOrderNumber = item.OrderNumber;
            model.CustomerDescription = item.CustomerDescription;
            Decimal.TryParse(item.TaxAmount, out decimal taxAmt);
            model.TaxAmount = taxAmt;
            Decimal.TryParse(item.TaxBasis, out decimal taxBasis);
            model.TaxBasis = taxBasis;
            model.TaxCertificateNumber = item.TaxCertificateNo;
            model.TaxDescription = item.TaxDescription;
            model.TaxExemptionDescription = item.TaxExceptionDescription;
            model.TaxMethod = item.TaxMethod;
            model.TaxType = item.TaxMethod;
            Decimal.TryParse(item.TaxRate, out decimal taxRate);
            model.TaxRate = taxRate;
            //!String.IsNullOrEmpty(item.TaxExceptionOverride) ? Convert.ToInt32(item.TaxExceptionOverride) : 0;
            Int32.TryParse(item.TaxExceptionOverride, out Int32 taxExceptionOverride);
            model.TaxExemptionOverride = taxExceptionOverride;
            model.BasedOnUnits = item.BasedOnUnits;
            model.PDIInvoiceNo = item.PDIInvoiceNo;
            if (model.TaxMethod == "Per Unit")
                model.TaxPricingTypeId = (int)OtherProductTaxPricingType.DollarPerGallon;
            else if (model.TaxMethod == "Percent")
                model.TaxPricingTypeId = (int)OtherProductTaxPricingType.PercentageOnTotalAmount;
            else
                model.TaxPricingTypeId = 0;

            return model;
        }

        public static CompanySpecificDeliveryDetailsCsvViewModel ToCsvViewModel(this Usp_CompanySpecificDeliveryDetails viewModel)
        {
            var csvViewModel = new CompanySpecificDeliveryDetailsCsvViewModel();
            csvViewModel.API = viewModel.API;
            csvViewModel.Badge = viewModel.Badge;
            csvViewModel.Berth = viewModel.Berth;
            csvViewModel.BolNumber = viewModel.BolNumber;
            csvViewModel.BulkplantName = viewModel.BulkplantName;
            csvViewModel.Carrier = viewModel.Carrier;
            csvViewModel.Customer = viewModel.Customer;
            csvViewModel.CustomerLocation = viewModel.CustomerLocation;
            csvViewModel.DDTInvUpdatedDate = viewModel.DDTInvUpdatedDate;
            csvViewModel.DDTModifiedby = viewModel.DDTModifiedby;
            csvViewModel.DriverName = viewModel.DriverName;
            csvViewModel.DropDate = viewModel.DropDate;
            csvViewModel.DropTicketDate = viewModel.DropTicketDate;
            csvViewModel.DropTime = viewModel.DropTime;
            csvViewModel.FuelType = viewModel.FuelType;
            csvViewModel.GallonsDelivered = viewModel.GallonsDelivered;
            csvViewModel.GrossQuantity = viewModel.GrossQuantity;
            csvViewModel.InvoiceAmount = viewModel.InvoiceAmount.ToString();
            csvViewModel.InvoiceNumber = viewModel.InvoiceNumber;
            csvViewModel.LiftDate = viewModel.LiftDate;
            csvViewModel.LiftTicketNumber = viewModel.LiftTicketNumber;
            csvViewModel.LocationAddress = viewModel.LocationAddress;
            csvViewModel.LocationCity = viewModel.LocationCity;
            csvViewModel.LocationState = viewModel.LocationState;
            csvViewModel.MarineFlag = viewModel.MarineFlag;
            csvViewModel.MTQty = viewModel.MTQty;
            csvViewModel.NetQuantity = viewModel.NetQuantity;
            csvViewModel.OrderCurrentStatus = viewModel.OrderCurrentStatus;
            csvViewModel.OrderLastUpdatedBy = viewModel.OrderLastUpdatedBy;
            csvViewModel.OrderLastUpdatedDate = viewModel.OrderLastUpdatedDate;
            csvViewModel.OrderType = viewModel.OrderType;
            csvViewModel.ParentDropTicket = viewModel.ParentDropTicket;
            csvViewModel.PDIException = viewModel.PDIException;
            csvViewModel.PDIOrder = viewModel.PDIOrder;
            csvViewModel.PickupLocation = viewModel.PickupLocation;
            csvViewModel.PONumber = viewModel.PONumber;
            csvViewModel.Pricing = viewModel.Pricing.ToString();
            csvViewModel.SubDropTicket = viewModel.SubDropTicket;
            csvViewModel.Tank = viewModel.Tank;
            csvViewModel.Terminal = viewModel.Terminal;
            csvViewModel.Vessel = viewModel.Vessel;
            csvViewModel.WaitingForStatus = viewModel.WaitingForStatus;
            csvViewModel.Zip = viewModel.Zip;
            csvViewModel.SupplierCompanyName = viewModel.SupplierCompanyName;
            csvViewModel.DRID = viewModel.UniqueOrderNo;
            return csvViewModel;
        }
    }
}
