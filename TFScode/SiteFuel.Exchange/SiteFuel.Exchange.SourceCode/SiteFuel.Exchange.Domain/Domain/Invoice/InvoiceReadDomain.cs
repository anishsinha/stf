using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.FileGenerator.DTN;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiteFuel.Exchange.ViewModels.Queue;

namespace SiteFuel.Exchange.Domain
{
    public class InvoiceReadDomain : BaseDomain
    {
        public InvoiceReadDomain()
            : base(ContextFactory.Current.ConnectionString)
        {
        }

        public InvoiceReadDomain(BaseDomain domain)
            : base(domain)
        {
        }

        public async Task<string> GenerateDtnFileAsync(int invoiceHeaderId, string refId, string password, string siteNumber)
        {
            string response = string.Empty;
            try
            {
                var invoices = await GetInvoiceDataForDtnFile(invoiceHeaderId);

                DTN dtn = new DTN();
                dtn.C02CommandLineGroup.SiteNumber = siteNumber;
                dtn.DtnLine1.REFID = refId;
                dtn.DtnLine1.PASSWORD = password;
                GetInvoiceHeader(dtn, invoices);
                dtn.BinaryData.InvoiceHeader.Identifier = refId;
                UpdateLineItemType(invoices);
                GetItemDetails(dtn, invoices);
                GetTaxItemDetails(dtn, invoices);
                if (invoices.All(t => t.InvoiceTypeId != (int)InvoiceType.DryRun))
                {
                    GetFeeDetails(invoices, dtn);
                    GetFreightFeeDetails(invoices, dtn);
                    GetSupplierAllowance(invoices, dtn);
                }
                response = dtn.GetCsvText();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceCommonDomain", "GenerateDtnFileAsync", ex.Message, ex);
            }
            return response;
        }

        private async Task<List<DtnFileViewModel>> GetInvoiceDataForDtnFile(int invoiceHeaderId)
        {
            var invoices = await Context.DataContext.InvoiceHeaderDetails.Where(t => t.Id == invoiceHeaderId).SelectMany(t => t.Invoices).Select(t => new DtnFileViewModel()
            {
                InvoiceNumberId = t.InvoiceHeader.InvoiceNumberId,
                SupplierCompanyName = t.Order.Company.Name,
                BuyerCompanyName = t.Order.BuyerCompany.Name,
                TaxDetails = t.TaxDetails.Where(t1 => t1.TaxExemptionInd != ApplicationConstants.AvaTaxExemptedInd).Select(t1 => new TaxDetailsViewModel()
                {
                    TradingTaxAmount = t1.TradingTaxAmount,
                    RateDescription = string.IsNullOrEmpty(t1.RelatedLineItem) ? t1.RateDescription : (t1.RateDescription + " - " + t1.RelatedLineItem),
                    TaxRate = t1.TaxRate,
                    TaxPricingTypeId = t1.TaxPricingTypeId,
                    CalculationTypeInd = t1.CalculationTypeInd
                }).ToList(),
                FuelRequestFees = t.FuelRequestFees.Where(t1 => t1.FeeTypeId != (int)FeeType.ResaleFee && t1.DiscountLineItemId == null && t1.FeeSubTypeId != (int)FeeSubType.NoFee && t1.FeeTypeId != (int)FeeType.DryRunFee).OrderBy(t1 => t1.FeeTypeId).Select(t1 => new FeesViewModel()
                {
                    FeeTypeId = t1.FeeTypeId.ToString(),
                    FeeSubTypeId = t1.FeeSubTypeId,
                    IncludeInPPG = t1.IncludeInPPG,
                    TotalFee = t1.TotalFee ?? 0,
                    FeeTypeName = t1.Currency == Currency.CAD ? t1.MstFeeType.Name.Replace(Constants.Gallon, Constants.Litre) : t1.MstFeeType.Name,
                    FeeSubQuantity = t1.FeeSubQuantity ?? 0,
                    OtherFeeTypeId = t1.OtherFeeTypeId,
                    OtherFeeDescription = (t1.FeeTypeId == (int)FeeType.OtherFee && t1.OtherFeeTypeId.HasValue && t1.FeeDetails != null && t1.FeeDetails != "") ? t1.MstOtherFeeType.Name : t1.FeeDetails,
                    Fee = t1.Fee
                }).ToList(),
                DueDate = t.PaymentDueDate,
                PoNumber = t.PoNumber,
                InvoiceNumber = t.DisplayInvoiceNumber,
                NetDays = t.Order.FuelRequest.NetDays,
                JobName = t.InvoiceXAdditionalDetail.JobName,
                JobAddress = t.InvoiceXAdditionalDetail.JobAddress,
                JobCity = t.InvoiceXAdditionalDetail.JobCity,
                JobStateCode = t.InvoiceXAdditionalDetail.JobStateCode,
                JobZipCode = t.InvoiceXAdditionalDetail.JobZipCode,
                PaymentTermId = t.Order.FuelRequest.PaymentTermId,
                ControlNumber = t.Order.MstExternalTerminal.ControlNumber, // user terminal from InvoiceFtlDetail
                InvoiceTypeId = t.InvoiceTypeId,
                BasicAmount = t.BasicAmount,
                FuelType = t.Order.FuelRequest.MstProduct.TfxProductId.HasValue ? t.Order.FuelRequest.MstProduct.MstTFXProduct.Name : t.Order.FuelRequest.MstProduct.Name,
                DroppedGallons = t.DroppedGallons,
                PricePerGallon = t.InvoiceXBolDetails.Select(t1 => t1.InvoiceFtlDetail.PricePerGallon).FirstOrDefault(),
                UpdatedDate = t.UpdatedDate,
                TotalTaxAmount = t.TotalTaxAmount,
                TotalFeeAmount = t.TotalFeeAmount,
                TotalDiscountAmount = t.TotalDiscountAmount,
                BolNumber = t.InvoiceXBolDetails.Select(t1 => t1.InvoiceFtlDetail.BolNumber).FirstOrDefault() ?? "",
                LiftTicketNumber = t.InvoiceXBolDetails.Select(t1 => t1.InvoiceFtlDetail.LiftTicketNumber).FirstOrDefault() ?? "",
                PickupLocationType = t.InvoiceXBolDetails.Select(t1 => t1.InvoiceFtlDetail.PickupLocation).FirstOrDefault(),
                GrossQuantity = t.InvoiceXBolDetails.Sum(t1 => t1.InvoiceFtlDetail.GrossQuantity) ?? t.DroppedGallons,
                NetQuantity = t.InvoiceXBolDetails.Sum(t1 => t1.InvoiceFtlDetail.NetQuantity) ?? t.DroppedGallons,
                PricingQuantityIndicatorTypeId = t.Order.FuelRequest.FuelRequestDetail.PricingQuantityIndicatorTypeId,
                PaymentTerm = t.MstPaymentTerm.Name,
                DropStartDate = t.DropStartDate,
                Carrier = t.InvoiceXBolDetails.Select(t1 => t1.InvoiceFtlDetail.Carrier).FirstOrDefault() ?? "",
                Version = t.Version,
                TotalAllowance = t.InvoiceXAdditionalDetail.TotalAllowance ?? 0,
                SupplierAllowance = t.InvoiceXAdditionalDetail.SupplierAllowance ?? 0,
                TerminalName = t.InvoiceXBolDetails.Select(t1 => t1.InvoiceFtlDetail.TerminalName).FirstOrDefault() ?? t.InvoiceXBolDetails.Select(t1 => t1.InvoiceFtlDetail.SiteName).FirstOrDefault() ?? "",
                SurchargePercentage = t.InvoiceXAdditionalDetail.SurchargePercentage,
                OriginalInvoiceNumber = t.InvoiceXAdditionalDetail.OriginalInvoiceId != null ? t.InvoiceXAdditionalDetail.OriginalInvoice.DisplayInvoiceNumber : string.Empty,
                IsFtl = t.Order.IsFTL,
                OrderId = t.OrderId ?? 0
            }).ToListAsync();

            foreach (var invoice in invoices)
            {
                if (invoice.PickupLocationType == PickupLocationType.BulkPlant)
                {
                    invoice.BolNumber = invoice.LiftTicketNumber;
                }

                //other way is to add Foreign key of TerminalId in InvoiceFtlDetail table
                if (!string.IsNullOrWhiteSpace(invoice.TerminalName))
                {
                    var controlNumberFromInvoice = Context.DataContext.MstExternalTerminals
                                                    .Where(t => t.Name.ToLower() == invoice.TerminalName.ToLower())
                                                    .Select(t => t.ControlNumber).FirstOrDefault();

                    if (!string.IsNullOrWhiteSpace(controlNumberFromInvoice))
                        invoice.ControlNumber = controlNumberFromInvoice;
                }
            }
            return invoices;
        }

        private void GetInvoiceHeader(DTN dtn, List<DtnFileViewModel> invoices)
        {
            var invoice = invoices.FirstOrDefault();
            dtn.BinaryData.InvoiceHeader = new InvoiceHeader()
            {
                InvoiceNumber = invoice.InvoiceNumber.CropToLastChars(22),
                InvoiceDate = invoice.UpdatedDate.Date,
                DocumentType = invoice.IsCreditInvoice() ? "CR" : "PR",
                TermsDescription = invoice.PaymentTermId == (int)PaymentTerms.NetDays ? invoice.PaymentTerm + " " + invoice.NetDays : invoice.PaymentTerm,
                DocumentGrandTotal = invoices.Sum(t => t.BasicAmount) + invoices.Sum(t => t.TotalTaxAmount) + (invoices.Sum(t => t.TotalFeeAmount ?? 0)),
                InvoiceDueDate = invoice.DueDate.Date,
                TotalInvoiceAmount = invoices.Sum(t => t.BasicAmount) + invoices.Sum(t => t.TotalTaxAmount) + (invoices.Sum(t => t.TotalFeeAmount ?? 0)) - invoices.Sum(t => t.TotalDiscountAmount),
                //Discount = invoice.TotalDiscountAmount,
                DiscountedAmountDue = invoices.Sum(t => t.BasicAmount) + invoices.Sum(t => t.TotalTaxAmount) + (invoices.Sum(t => t.TotalFeeAmount ?? 0)) + invoices.Sum(t => t.TotalDiscountAmount),
                SellerName = invoice.SupplierCompanyName.CropToLastChars(60),
                SoldToName = invoice.BuyerCompanyName.CropToLastChars(60),
                PurchaseOrderNumber = invoice.PoNumber.CropToLastChars(22),
                ShipToName = invoice.JobName.CropToLastChars(60),
                ShipToAddress = invoice.JobAddress == Resource.lblVarious ? string.Empty : invoice.JobAddress.CropToLastChars(55),
                ShipToCity = invoice.JobCity == Resource.lblVarious ? string.Empty : invoice.JobCity.CropToLastChars(30),
                ShipToState = invoice.JobStateCode.CropToLastChars(2),
                ShipToZip = invoice.JobZipCode == Resource.lblVarious ? string.Empty : invoice.JobZipCode.CropToLastChars(15),
                ShipFromName = invoice.TerminalName.CropToLastChars(60),
                TCN_SPLC = invoice.ControlNumber.CropToLastChars(30)
            };
            if (invoice.IsCreditInvoice())
            {
                dtn.BinaryData.InvoiceHeader.OriginalInvoiceNumber = invoice.OriginalInvoiceNumber;
            }
            if (invoice.Version > 1)
            {
                var originalInvoiceNumber = Context.DataContext.Invoices.Where(t => t.InvoiceHeader.InvoiceNumberId == invoice.InvoiceNumberId
                                                                                                                && t.Version < invoice.Version)
                                                                                                  .OrderByDescending(t => t.Version).Select(t => t.DisplayInvoiceNumber).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(originalInvoiceNumber))
                {
                    dtn.BinaryData.InvoiceHeader.OriginalInvoiceNumber = originalInvoiceNumber.CropToLastChars(22);
                }
            }
        }

        private void GetItemDetails(DTN dtn, List<DtnFileViewModel> invoices)
        {
            List<DetailItem> detailsItems = new List<DetailItem>();
            foreach (var invoice in invoices)
            {
                if (invoice.InvoiceTypeId == (int)InvoiceType.DryRun)
                {
                    detailsItems.Add(GetDetailItem(invoice, invoice.BasicAmount, Resource.lblDryRunFee, 1, invoice.BasicAmount));
                }
                else if (invoice.DroppedGallons != 0)
                {
                    detailsItems.Add(GetDetailItem(invoice, invoice.BasicAmount + invoice.TotalAllowance, invoice.FuelType, Convert.ToInt32(invoice.DroppedGallons), invoice.PricePerGallon));
                    dtn.BinaryData.DetailItems = detailsItems;
                }
            }
        }

        private void GetTaxItemDetails(DTN dtn, List<DtnFileViewModel> invoices)
        {
            List<TaxDetailItem> taxDetailItems = new List<TaxDetailItem>();
            var appDomain = new ApplicationDomain(this);
            var dtnTaxRateDecimalFormat = appDomain.GetKeySettingValue(Constants.DtnTXDLRateDecimalFormat, DtnConstants.NumberFormat4);
            foreach (var invoice in invoices)
            {
                if (invoice.InvoiceTypeId != (int)InvoiceType.DryRun)
                {
                    foreach (var tax in invoice.TaxDetails)
                    {
                        taxDetailItems.Add(new TaxDetailItem(dtnTaxRateDecimalFormat)
                        {
                            Rate = GetTaxRate(tax),
                            LineTotal = tax.TradingTaxAmount,
                            Description = tax.RateDescription.CropToLastChars(80),
                            QuantityBilled = GetTaxQuantity(tax, invoice),
                            BilledQuantityIndicator = invoice.IsFtl ? (invoice.PricingQuantityIndicatorTypeId.Value == (int)QuantityIndicatorTypes.Net ? "N" : "G") : "N"
                        });
                    }
                    dtn.BinaryData.TaxDetailItems = taxDetailItems;
                }
            }
        }

        private decimal GetTaxRate(TaxDetailsViewModel taxItem)
        {
            decimal taxRate = 0;
            switch (taxItem.CalculationTypeInd)
            {
                case "C":
                case "P":
                    taxRate = taxItem.TaxRate;
                    break;
                case "MANUAL":
                    switch (taxItem.TaxPricingTypeId)
                    {
                        case (int)OtherProductTaxPricingType.DollarOnTotalAmount:
                        case (int)OtherProductTaxPricingType.DollarPerGallon:
                            taxRate = taxItem.TaxRate;
                            break;
                        case (int)OtherProductTaxPricingType.PercentagePerGallon:
                        case (int)OtherProductTaxPricingType.PercentageOnTotalAmount:
                            taxRate = taxItem.TaxRate / 100;
                            break;
                        default:
                            taxRate = taxItem.TradingTaxAmount;
                            break;
                    }
                    break;
                case "F":
                case "SFX":
                    taxRate = taxItem.TradingTaxAmount;
                    break;
            }
            return Math.Abs(taxRate);
        }

        private decimal GetTaxQuantity(TaxDetailsViewModel taxItem, DtnFileViewModel invoice)
        {
            decimal taxQuantity = 0;
            switch (taxItem.CalculationTypeInd)
            {
                case "C":
                    taxQuantity = invoice.DroppedGallons;
                    break;
                case "P":
                    taxQuantity = taxItem.TaxRate == 0 ? 0 : taxItem.TradingTaxAmount / taxItem.TaxRate;
                    break;
                case "MANUAL":
                    switch (taxItem.TaxPricingTypeId)
                    {
                        case (int)OtherProductTaxPricingType.DollarOnTotalAmount:
                            taxQuantity = invoice.IsCreditInvoice() ? -1 : 1;
                            break;
                        case (int)OtherProductTaxPricingType.DollarPerGallon:
                        case (int)OtherProductTaxPricingType.PercentagePerGallon:
                            taxQuantity = invoice.DroppedGallons;
                            break;
                        case (int)OtherProductTaxPricingType.PercentageOnTotalAmount:
                            taxQuantity = invoice.BasicAmount;
                            break;
                        default:
                            taxQuantity = invoice.IsCreditInvoice() ? -1 : 1;
                            break;
                    }
                    break;
                case "F":
                case "SFX":
                    taxQuantity = invoice.IsCreditInvoice() ? -1 : 1;
                    break;
            }
            return taxQuantity;
        }

        private void UpdateLineItemType(List<DtnFileViewModel> invoices)
        {
            var dtnLineItemType = Context.DataContext.MstAppSettings.Where(t => t.Key == ApplicationConstants.KeyAppSettingDtnLineItemTypeSettings).Select(t => t.Value).FirstOrDefault();
            if (!string.IsNullOrEmpty(dtnLineItemType))
            {
                var dtnLineItemSettings = JsonConvert.DeserializeObject<dynamic>(dtnLineItemType);
                foreach (var invoice in invoices)
                {
                    invoice.AllFeeAsTxdl = Convert.ToBoolean(dtnLineItemSettings.AllFeeAsTxdl);
                    invoice.AllowanceAsTxdl = Convert.ToBoolean(dtnLineItemSettings.AllowanceAsTxdl);
                    invoice.FreightFeeType = dtnLineItemSettings.FrieghtFeeType;
                }
            }
        }

        private void GetFeeDetails(List<DtnFileViewModel> invoices, DTN dtn)
        {
            foreach (var invoice in invoices)
            {
                var invoiceFees = invoice.FuelRequestFees.Where(t => t.FeeTypeId != ((int)FeeType.DryRunFee).ToString() && t.FeeTypeId != ((int)FeeType.FreightFee).ToString()
                                                                           && t.FeeTypeId != ((int)FeeType.FreightCost).ToString()
                                                                           && t.FeeSubTypeId != (int)FeeSubType.NoFee && !t.IncludeInPPG && t.TotalFee != 0);
                if (!invoice.AllFeeAsTxdl)
                {
                    string description;
                    decimal quantityBilled, rate;
                    foreach (var item in invoiceFees)
                    {
                        description = (Convert.ToInt32(item.FeeTypeId) == (int)FeeType.OtherFee && !string.IsNullOrWhiteSpace(item.OtherFeeDescription)) ? (item.FeeTypeName + " - " + item.OtherFeeDescription) : item.FeeTypeName;
                        quantityBilled = Convert.ToInt32(item.FeeSubQuantity ?? 0);
                        if (item.FeeSubTypeId == (int)FeeSubType.HourlyRate)
                        {
                            quantityBilled = invoice.IsCreditInvoice() ? -1 : 1;
                        }
                        rate = item.FeeSubTypeId == (int)FeeSubType.HourlyRate || item.FeeSubTypeId == (int)FeeSubType.Percent ? Math.Abs(item.TotalFee) : item.Fee.Value;
                        dtn.BinaryData.DetailItems.Add(GetDetailItem(invoice, item.TotalFee, description, quantityBilled, rate));
                    }
                }
                else
                {
                    var appDomain = new ApplicationDomain(this);
                    var dtnTaxRateDecimalFormat = appDomain.GetKeySettingValue(Constants.DtnTXDLRateDecimalFormat, DtnConstants.NumberFormat4);

                    foreach (var item in invoiceFees)
                    {
                        TaxDetailItem taxDetail = new TaxDetailItem(dtnTaxRateDecimalFormat);
                        var feeTypeId = Convert.ToInt32(item.FeeTypeId);
                        if (feeTypeId == (int)FeeType.SurchargeFreightFee)
                        {
                            taxDetail = new TaxDetailItem();
                        }
                        taxDetail.LineTotal = item.TotalFee;
                        taxDetail.Description = item.FeeTypeName.CropToLastChars(80);
                        if (feeTypeId == (int)FeeType.OtherFee && !string.IsNullOrWhiteSpace(item.OtherFeeDescription))
                        {
                            taxDetail.Description = (item.FeeTypeName + " - " + item.OtherFeeDescription).CropToLastChars(80);
                        }
                        taxDetail.QuantityBilled = Convert.ToInt32(item.FeeSubQuantity ?? 0);
                        if (item.FeeSubTypeId == (int)FeeSubType.HourlyRate)
                        {
                            taxDetail.QuantityBilled = invoice.IsCreditInvoice() ? -1 : 1;
                        }
                        taxDetail.Rate = item.FeeSubTypeId == (int)FeeSubType.HourlyRate || item.FeeSubTypeId == (int)FeeSubType.Percent ? Math.Abs(item.TotalFee) : item.Fee.Value;
                        taxDetail.BilledQuantityIndicator = invoice.IsFtl ? (invoice.PricingQuantityIndicatorTypeId.Value == (int)QuantityIndicatorTypes.Net ? "N" : "G") : "N";
                        if (feeTypeId == (int)FeeType.SurchargeFreightFee)
                        {
                            taxDetail.Description = Resource.lblFuelSurcharge;
                            taxDetail.QuantityBilled = invoice.DroppedGallons;
                            taxDetail.Rate = item.Fee.Value * invoice.SurchargePercentage.Value / 100;
                        }
                        dtn.BinaryData.TaxDetailItems.Add(taxDetail);
                    }
                }
            }
        }

        private static DetailItem GetDetailItem(DtnFileViewModel invoice, decimal lineTotal, string description, decimal quantityBilled, decimal rate)
        {
            DetailItem detailItem = new DetailItem();

            if (invoice.IsFtl)
            {
                detailItem.BolNumber = invoice.BolNumber.CropToLastChars(30);
                detailItem.NetQuantity = invoice.IsCreditInvoice() ? -1 * Convert.ToInt32(invoice.NetQuantity) : Convert.ToInt32(invoice.NetQuantity);
                detailItem.GrossQuantity = invoice.IsCreditInvoice() ? -1 * Convert.ToInt32(invoice.GrossQuantity) : Convert.ToInt32(invoice.GrossQuantity);
                detailItem.QuantityBilled = quantityBilled;
                detailItem.BilledQuantityIndicator = invoice.PricingQuantityIndicatorTypeId.Value == (int)QuantityIndicatorTypes.Net ? "N" : "G";
            }
            else
            {
                detailItem.BolNumber = $"{invoice.InvoiceNumber}-{invoice.OrderId}".CropToLastChars(30);
                detailItem.NetQuantity = quantityBilled;
                detailItem.GrossQuantity = quantityBilled;
                detailItem.QuantityBilled = quantityBilled;
                detailItem.BilledQuantityIndicator = "N";
            }

            detailItem.LineTotal = lineTotal;
            detailItem.Description = description.CropToLastChars(80);
            detailItem.Rate = rate;
            detailItem.ShipDateTime = invoice.DropStartDate.DateTime;
            detailItem.CarrierDescription = invoice.Carrier.CropToLastChars(35);
            return detailItem;
        }

        private void GetSupplierAllowance(List<DtnFileViewModel> invoices, DTN dtn)
        {
            foreach (var invoice in invoices)
            {
                if (!invoice.AllowanceAsTxdl && invoice.TotalAllowance > 0)
                {
                    dtn.BinaryData.DetailItems.Add(GetDetailItem(invoice, -1 * invoice.TotalAllowance, Resource.headingSupplierAllowance, Convert.ToInt32(invoice.DroppedGallons), -1 * invoice.SupplierAllowance));
                }
                if (invoice.AllowanceAsTxdl && invoice.TotalAllowance != 0)
                {
                    var appDomain = new ApplicationDomain(this);
                    var dtnTaxRateDecimalFormat = appDomain.GetKeySettingValue(Constants.DtnTXDLRateDecimalFormat, DtnConstants.NumberFormat4);

                    dtn.BinaryData.TaxDetailItems.Add(new TaxDetailItem(dtnTaxRateDecimalFormat)
                    {
                        LineTotal = -1 * invoice.TotalAllowance,
                        Description = Resource.headingSupplierAllowance.CropToLastChars(80),
                        QuantityBilled = Convert.ToInt32(invoice.DroppedGallons),
                        Rate = -1 * invoice.SupplierAllowance,
                        BilledQuantityIndicator = invoice.IsFtl ? (invoice.PricingQuantityIndicatorTypeId.Value == (int)QuantityIndicatorTypes.Net ? "N" : "G") : "N"
                    });
                }
            }
        }

        private void GetFreightFeeDetails(List<DtnFileViewModel> invoices, DTN dtn)
        {
            foreach (var invoice in invoices)
            {
                var invoiceFees = invoice.FuelRequestFees.Where(t => t.FeeTypeId == ((int)FeeType.FreightFee).ToString()
                                                                           && t.FeeSubTypeId != (int)FeeSubType.NoFee && !t.IncludeInPPG && t.TotalFee != 0);
                if (invoice.FreightFeeType == (int)FreightFeeItemTypeInDTNFile.ITMD)
                {
                    string description;
                    decimal quantityBilled, rate;
                    foreach (var item in invoiceFees)
                    {
                        description = (Convert.ToInt32(item.FeeTypeId) == (int)FeeType.OtherFee && !string.IsNullOrWhiteSpace(item.OtherFeeDescription)) ? (item.FeeTypeName + " - " + item.OtherFeeDescription) : item.FeeTypeName;
                        quantityBilled = item.FeeSubTypeId == (int)FeeSubType.HourlyRate ? (invoice.IsCreditInvoice() ? -1 : 1) : Convert.ToInt32(item.FeeSubQuantity ?? 0);
                        rate = item.FeeSubTypeId == (int)FeeSubType.HourlyRate ? Math.Abs(item.TotalFee) : item.Fee.Value;
                        dtn.BinaryData.DetailItems.Add(GetDetailItem(invoice, item.TotalFee, description, quantityBilled, rate));
                    }
                }
                else if (invoice.FreightFeeType == (int)FreightFeeItemTypeInDTNFile.TXDL)
                {
                    var appDomain = new ApplicationDomain(this);
                    var dtnTaxRateDecimalFormat = appDomain.GetKeySettingValue(Constants.DtnTXDLRateDecimalFormat, DtnConstants.NumberFormat4);

                    foreach (var item in invoiceFees)
                    {
                        dtn.BinaryData.TaxDetailItems.Add(new TaxDetailItem(dtnTaxRateDecimalFormat)
                        {
                            LineTotal = item.TotalFee,
                            Description = (Convert.ToInt32(item.FeeTypeId) == (int)FeeType.OtherFee && !string.IsNullOrWhiteSpace(item.OtherFeeDescription)) ? (item.FeeTypeName + " - " + item.OtherFeeDescription) : item.FeeTypeName.CropToLastChars(80),
                            QuantityBilled = item.FeeSubTypeId == (int)FeeSubType.HourlyRate ? (invoice.IsCreditInvoice() ? -1 : 1) : Convert.ToInt32(item.FeeSubQuantity ?? 0),
                            Rate = item.FeeSubTypeId == (int)FeeSubType.HourlyRate ? Math.Abs(item.TotalFee) : item.Fee.Value,
                            BilledQuantityIndicator = invoice.IsFtl ? (invoice.PricingQuantityIndicatorTypeId.Value == (int)QuantityIndicatorTypes.Net ? "N" : "G") : "N"
                        });
                    }
                }
                else
                {
                    foreach (var item in invoiceFees)
                    {
                        dtn.BinaryData.FreightDetailItems.Add(new FreightDetailItem
                        {
                            LineTotal = item.TotalFee,
                            Description = (Convert.ToInt32(item.FeeTypeId) == (int)FeeType.OtherFee && !string.IsNullOrWhiteSpace(item.OtherFeeDescription)) ? (item.FeeTypeName + " - " + item.OtherFeeDescription) : item.FeeTypeName.CropToLastChars(80),
                            QuantityBilled = item.FeeSubTypeId == (int)FeeSubType.HourlyRate ? (invoice.IsCreditInvoice() ? -1 : 1) : Convert.ToInt32(item.FeeSubQuantity ?? 0),
                            Rate = item.FeeSubTypeId == (int)FeeSubType.HourlyRate ? Math.Abs(item.TotalFee) : item.Fee.Value
                        });
                    }
                }
            }
        }

        public async Task<StatusViewModel> UploadDtnFileToFtp(int invoiceId, string invoiceNumber, int supplierCompanyId, int buyerCompanyId,int userId = 0)
        {
            StatusViewModel response = new StatusViewModel(Status.Failed);
            try
            {
                string ftlSupplierDetails = Context.DataContext.MstAppSettings.Where(t => t.Key == ApplicationConstants.KeyAppSettingFTLSupplierDtnDetails).Select(t => t.Value).FirstOrDefault();
                if (!string.IsNullOrEmpty(ftlSupplierDetails))
                {
                    DtnSupplierDetails ftlSuppliers = JsonConvert.DeserializeObject<DtnSupplierDetails>(ftlSupplierDetails);
                    DtnSuppliers ftlSupplier = ftlSuppliers.DtnSuppliers.FirstOrDefault(t => t.CompanyId == supplierCompanyId);
                    if (ftlSupplier != null)
                    {
                        string buyerSiteNumber = ftlSuppliers.SiteNumbers.Where(t => t.BuyerCompanyId == buyerCompanyId && t.SupplierCompanyId == supplierCompanyId).Select(t => t.BuyerSiteNumber).FirstOrDefault();
                        if (string.IsNullOrWhiteSpace(buyerSiteNumber))
                        {
                            buyerSiteNumber = ftlSuppliers.SiteNumbers.Where(t => t.BuyerCompanyId == buyerCompanyId && t.SupplierCompanyId == null).Select(t => t.BuyerSiteNumber).FirstOrDefault();
                        }
                        if(!string.IsNullOrEmpty(buyerSiteNumber) && IsCallTelaFuelService(supplierCompanyId,buyerCompanyId, invoiceId))
                        {
                            var invoiceHeaderDetails = Context.DataContext.InvoiceHeaderDetails.Where(t => t.Id == invoiceId).FirstOrDefault();
                            if (invoiceHeaderDetails != null)
                            {
                                var invoice = invoiceHeaderDetails.Invoices.FirstOrDefault();
                                var invoiceBaseDomain = new InvoiceBaseDomain(this);
                                invoiceBaseDomain.CreateTelaFuelServiceWorkflow(invoice, invoice.Order,userId);                              
                            }                                
                        }
                       else if (!string.IsNullOrEmpty(buyerSiteNumber))
                        {
                            var jsonViewModel = new DtnFileProcessingRequestViewModel();
                            jsonViewModel.InvoiceId = invoiceId;
                            jsonViewModel.InvoiceNumber = invoiceNumber;
                            jsonViewModel.RefId = ftlSupplier.RefId;
                            jsonViewModel.Password = ftlSupplier.Password;
                            jsonViewModel.SiteNumber = buyerSiteNumber;
                            jsonViewModel.FtpUrl = ftlSuppliers.FtpUrl;
                            jsonViewModel.FtpUserName = ftlSupplier.FtpUserName;
                            jsonViewModel.FtpPassword = ftlSupplier.FtpPassword;
                            jsonViewModel.PathToUpload = ftlSupplier.FtpPathToUpload;
                            jsonViewModel.ReceiversEmail = ftlSuppliers.NotifiedUsers;
                            ThirdPartyOrderDomain thirdPartyOrderDomain = new ThirdPartyOrderDomain(this);
                            await thirdPartyOrderDomain.UploadDtnFileToFtpLocation(jsonViewModel);
                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.successMsgDtnUploaded;
                        }
                        else
                        {
                            response.StatusMessage = Resource.warningForDtnCannotbeUploaded;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.StatusMessage = Resource.errMessageForDtnUploadFailed;
                LogManager.Logger.WriteException("InvoiceReadDomain", "UploadDtnFileToFtp", ex.Message, ex);
            }
            return response;
        }
        private bool IsCallTelaFuelService(int supplierCompanyId, int buyerCompanyId, int invoiceHeaderId)
        {
            bool isConfigured = false;
            bool isCallService = false;
            try
            {
                string telaFuelServiceSupplierDetails = Context.DataContext.MstAppSettings.Where(t => t.Key == ApplicationConstants.KeyAppSettingTelaOrderAddApiSettings).Select(t => t.Value).FirstOrDefault();
                TelaFuelServiceSupplierDetails telaSupplierDetails = new TelaFuelServiceSupplierDetails();
                if (!string.IsNullOrEmpty(telaFuelServiceSupplierDetails))
                {
                    telaSupplierDetails = JsonConvert.DeserializeObject<TelaFuelServiceSupplierDetails>(telaFuelServiceSupplierDetails);
                }
                isConfigured = telaSupplierDetails.TelaSuppliers != null
                                && telaSupplierDetails.TelaSuppliers.Any(t => t.CompanyId == supplierCompanyId && t.BuyerCompanyIds.Contains(buyerCompanyId));
                if (isConfigured)
                {
                    isCallService = !Context.DataContext.QueueMessages.Any(t => t.ProcessTypeId == (int)QueueProcessType.TelaFuelServiceOrderAdd && t.JsonMessage.Contains("\"InvoiceId\":"+invoiceHeaderId));
                }            
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceReadDomain", "IsCallTelaFuelService", ex.Message, ex);

            }
            return isCallService;
        }      
    }
}
