using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Core.Utilities;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.cXML;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class PunchoutOrderMessageDomain : BaseDomain
    {
        public PunchoutOrderMessageDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public PunchoutOrderMessageDomain(BaseDomain domain) : base(domain)
        {
        }

        private static readonly string _manufacturerCompanyName = "TrueFill";
        private static readonly string _diesel1 = "-1";
        private static readonly string _diesel2 = "-2";

        public async Task UpdateCxmlCheckoutFlag(int headerId)
        {
            try
            {
                var invoiceIdList = await Context.DataContext.Invoices.Where(t => t.InvoiceHeaderId == headerId).Select(t => t.Id).ToListAsync();
                if (invoiceIdList != null && invoiceIdList.Any())
                {
                    var list = string.Join(",", invoiceIdList.Select(t => t.ToString()));
                    if (!string.IsNullOrWhiteSpace(list))
                    {
                        var cmdText = string.Format("UPDATE InvoiceXAdditionalDetails SET CxmlCheckOutDate='" + DateTimeOffset.Now + "' WHERE InvoiceId in ({0})", list);
                        Context.DataContext.Database.ExecuteSqlCommand(cmdText);
                        await Context.CommitAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PunchoutOrderMessageDomain", "UpdateCxmlCheckoutFlag", ex.Message, ex);
            }
        }

        public async Task<PunchoutOrderMessageResponseViewModel> GetPunchoutOrderMessageXmlString(UserContext userContext, int invoiceHeaderId, int supplierCompanyId)
        {
            var response = new PunchoutOrderMessageResponseViewModel();
            try
            {
                
                //var invoicePdf = await invoiceDomain.GetInvoicePdfNewAsync(invoiceId, userContext.CompanyTypeId);
                var invoicePdf = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetConsolidatedInvoicePdfAsync(invoiceHeaderId, userContext.CompanyTypeId);
                if (invoicePdf != null && invoicePdf.Invoices.Any())
                {
                    var UNSPSC = await GetUNSPSCMapping();
                    response.PunchoutCxml = GetPunchoutOrderMessageXml(userContext, invoicePdf, UNSPSC);
                    response.CxmlCheckOutDate = invoicePdf.Invoices.FirstOrDefault().CxmlCheckOutDate;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PunchoutOrderMessageDomain", "GetPunchoutOrderMessageXmlString", ex.Message, ex);
            }
            return response;
        }

        private string GetPunchoutOrderMessageXml(UserContext userContext, ConsolidatedInvoicePdfViewModel invoicePdf, Dictionary<string, string> UNSPSC)
        {
            string xmlString;
            OrderMessage orderMessage = new OrderMessage();
            orderMessage.Header.From.Credential.Identity = userContext.Email;
            orderMessage.Header.To.Credential.Identity = $"{userContext.CompanyName}-SFX";
            orderMessage.Header.Sender.Credential.Identity = userContext.Email;
            orderMessage.Header.Sender.UserAgent = "Buyerquest Punchout WebClient";

            var punchOutOrderMessage = GetPunchoutOrderMessage(invoicePdf, userContext.CxmlBuyerCookie, UNSPSC);
            orderMessage.Message.PunchOutOrderMessage = punchOutOrderMessage;
            xmlString = XmlSerialization.Serialize(orderMessage);
            LogManager.Logger.WriteInfo("PunchoutOrderMessageDomain", "GetPunchoutOrderMessageXmlString", xmlString);

            var bytes = Encoding.UTF8.GetBytes(xmlString);
            xmlString = Convert.ToBase64String(bytes);
            return xmlString;
        }

        private async Task<Dictionary<string, string>> GetUNSPSCMapping()
        {
            var response = new Dictionary<string, string>();
            try
            {
                var UNSPSCMapping = await Context.DataContext.MstAppSettings.Where(t => t.Key == ApplicationConstants.KeyAppSettingUnspscMapping)
                                            .Select(t => t.Value).FirstOrDefaultAsync();
                if (!string.IsNullOrWhiteSpace(UNSPSCMapping))
                {
                    var mappingList = UNSPSCMapping.Split(';').ToList();
                    foreach (var item in mappingList.Where(t => !string.IsNullOrWhiteSpace(t)))
                    {
                        var keyValue = item.Split('-');
                        response.Add(keyValue[0], keyValue[1]);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PunchoutOrderMessageDomain", "GetUNSPSCMapping", ex.Message, ex);
            }
            return response;
        }

        private PunchOutOrderMessage GetPunchoutOrderMessage(ConsolidatedInvoicePdfViewModel pdfViewModel, string buyerCookie, Dictionary<string, string> UNSPSC)
        {
            PunchOutOrderMessage punchOutOrderMessage = new PunchOutOrderMessage();
            try
            {
                SetPunchOutOrderMessageHeader(punchOutOrderMessage, pdfViewModel, buyerCookie);

                //assuming that dry run invoice will be only one even for consolidated case
                if (pdfViewModel.Invoices.Any(t => t.InvoiceTypeId == (int)InvoiceType.DryRun))
                {
                    var fuelUnspsc = GetProductCode(UNSPSC, ApplicationConstants.UnspscDryRunFee);
                    var fueltype = GetDryRunAsItemIn(pdfViewModel);
                    fueltype.ItemDetail.Classification = GetUnspscNode(fuelUnspsc);
                    punchOutOrderMessage.ItemIn.Add(fueltype);
                }
                else
                {
                    foreach (var invoiceItem in pdfViewModel.Invoices)
                    {
                        var fuelUnspsc = GetProductCode(UNSPSC, invoiceItem.ProductTypeId.ToString());
                        var fueltype = GetFuelTypeAsItemIn(invoiceItem, fuelUnspsc);
                        punchOutOrderMessage.ItemIn.Add(fueltype);

                        var fees = pdfViewModel.FuelFees.FuelRequestFees.Where(t => t.InvoiceId == invoiceItem.Id).ToList();
                        SetFeeItemsToPunchOutOrderMessage(punchOutOrderMessage, invoiceItem, fees, UNSPSC);
                        SetDiscountItemsToPunchOutOrderMessage(punchOutOrderMessage, invoiceItem, pdfViewModel.FuelFees.DiscountLineItems, UNSPSC);
                        SetSupplierAllowanceItemsToPunchOutOrderMessage(punchOutOrderMessage, invoiceItem, UNSPSC);
                    }
                    var firstInvoice = pdfViewModel.Invoices.First();
                    SetTaxItemsToPunchOutOrderMessage(punchOutOrderMessage, firstInvoice, pdfViewModel.TaxDetail, UNSPSC);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PunchoutOrderMessageDomain", "GetPunchoutOrderMessage", ex.Message, ex);
            }
            return punchOutOrderMessage;
        }

        private static void SetPunchOutOrderMessageHeader(PunchOutOrderMessage punchOutOrderMessage, ConsolidatedInvoicePdfViewModel pdfViewModel, string buyerCookie)
        {
            var currency = pdfViewModel.Invoices.FirstOrDefault().Currency.ToString();
            punchOutOrderMessage.BuyerCookie = buyerCookie;
            punchOutOrderMessage.PunchOutOrderMessageHeader.OperationAllowed = "edit";
            punchOutOrderMessage.PunchOutOrderMessageHeader.Total.Money.Currency = currency;
            if (pdfViewModel.Invoices.Any(t => t.InvoiceTypeId == (int)InvoiceType.DryRun))
            {
                //punchOutOrderMessage.PunchOutOrderMessageHeader.Total.Money.Text = pdfViewModel.Invoice.BasicAmount.GetPreciseValue(5).ToString();
                punchOutOrderMessage.PunchOutOrderMessageHeader.Total.Money.Text = pdfViewModel.Invoices.Sum(t => t.BasicAmount).GetPreciseValue(5).ToString();
            }
            else
            {
                //var totalAmount = pdfViewModel.Invoice.BasicAmount + pdfViewModel.Invoice.TotalFees + pdfViewModel.Invoice.TotalTaxAmount - pdfViewModel.Invoice.TotalDiscountAmount;
                var totalAmount = pdfViewModel.Invoices.Sum(t => t.BasicAmount + t.TotalFees + t.TotalTaxAmount - t.TotalDiscountAmount);
                punchOutOrderMessage.PunchOutOrderMessageHeader.Total.Money.Text = totalAmount.GetPreciseValue(5).ToString();
            }
        }

        private static void SetFeeItemsToPunchOutOrderMessage(PunchOutOrderMessage punchOutOrderMessage, ConsolidatedInvoiceViewModel pdfViewModel, List<FeesViewModel> fees, Dictionary<string, string> UNSPSC)
        {
            var currency = pdfViewModel.Currency.ToString();
            var droppedQuantity = pdfViewModel.DroppedGallons;

            var filteredfees = fees.Where(t => t.FeeTypeId != ((int)FeeType.DryRunFee).ToString()
                                                                   && t.FeeSubTypeId != (int)FeeSubType.NoFee);
            foreach (var item in filteredfees)
            {
                var keyUnspsc = $"{ApplicationConstants.UnspscFee}{item.FeeTypeId}";
                var unspscCode = GetProductCode(UNSPSC, keyUnspsc);
                var feeItem = GetFeeAsItemIn(item, pdfViewModel.InvoiceTypeId, droppedQuantity, currency, "EA");
                feeItem.ItemDetail.Classification = GetUnspscNode(unspscCode);
                punchOutOrderMessage.ItemIn.Add(feeItem);
            }
        }

        private static void SetDiscountItemsToPunchOutOrderMessage(PunchOutOrderMessage punchOutOrderMessage, ConsolidatedInvoiceViewModel pdfViewModel, List<DiscountLineItemViewModel> discountLineItems, Dictionary<string, string> UNSPSC)
        {
            var currency = pdfViewModel.Currency.ToString();

            foreach (var item in discountLineItems)
            {
                var keyUnspsc = $"{ApplicationConstants.UnspscFee}{item.FeeTypeId}";
                var unspscCode = GetProductCode(UNSPSC, keyUnspsc);
                var feeItem = GetDiscountAsItemIn(item, pdfViewModel.InvoiceTypeId, currency, "EA");
                feeItem.ItemDetail.Classification = GetUnspscNode(unspscCode);
                punchOutOrderMessage.ItemIn.Add(feeItem);
            }
        }

        private static void SetSupplierAllowanceItemsToPunchOutOrderMessage(PunchOutOrderMessage punchOutOrderMessage, ConsolidatedInvoiceViewModel pdfViewModel, Dictionary<string, string> UNSPSC)
        {
            if (pdfViewModel.AdditionalDetail.TotalAllowance.HasValue && pdfViewModel.AdditionalDetail.TotalAllowance != 0)
            {
                var fuelUnspsc = GetProductCode(UNSPSC, ApplicationConstants.UnspscOtherFee);
                var supplierAllowance = GetSupplierAllowanceAsItemIn(pdfViewModel);
                supplierAllowance.ItemDetail.Classification = GetUnspscNode(fuelUnspsc);
                punchOutOrderMessage.ItemIn.Add(supplierAllowance);
            }
        }

        private static ItemIn GetSupplierAllowanceAsItemIn(ConsolidatedInvoiceViewModel pdfViewModel)
        {
            var itemIn = new ItemIn();

            var itemId = pdfViewModel.Id;
            itemIn.Quantity = (-1 * pdfViewModel.DroppedGallons).GetPreciseValue(2).ToString();
            itemIn.ItemID.SupplierPartID = itemId.ToString();
            itemIn.ItemID.SupplierPartAuxiliaryID = itemId.ToString();

            itemIn.ItemDetail.UnitPrice.Money.Currency = pdfViewModel.Currency.ToString();
            var supplierAllowance = pdfViewModel.AdditionalDetail.SupplierAllowance.Value;
            itemIn.ItemDetail.UnitPrice.Money.Text = supplierAllowance.GetPreciseValue(5).ToString();
            itemIn.ItemDetail.Description.Text = Resource.headingSupplierAllowance;
            itemIn.ItemDetail.UnitOfMeasure = "EA";

            return itemIn;
        }

        private static void SetTaxItemsToPunchOutOrderMessage(PunchOutOrderMessage punchOutOrderMessage, ConsolidatedInvoiceViewModel pdfViewModel, InvoiceTaxDetailsViewModel taxDetailsViewModel, Dictionary<string, string> UNSPSC)
        {
            if (taxDetailsViewModel != null)
            {
                var currency = pdfViewModel.Currency.ToString();
                var taxUnspscCode = GetProductCode(UNSPSC, ApplicationConstants.UnspscTax);
                foreach (var item in taxDetailsViewModel.AvaTaxDetails)
                {
                    var taxItem = GetTaxAsItemIn(item, currency, "EA", pdfViewModel.InvoiceTypeId);
                    taxItem.ItemDetail.Classification = GetUnspscNode(taxUnspscCode);
                    punchOutOrderMessage.ItemIn.Add(taxItem);
                }
            }
        }

        private static ItemIn GetFuelTypeAsItemIn(ConsolidatedInvoiceViewModel pdfViewModel, string fuelUnspsc)
        {
            var itemIn = new ItemIn();

            var itemId = pdfViewModel.FuelTypeId ?? 0;
            itemIn.Quantity = pdfViewModel.DroppedGallons.GetPreciseValue(2).ToString();
            itemIn.ItemID.SupplierPartID = itemId.ToString();
            itemIn.ItemID.SupplierPartAuxiliaryID = itemId.ToString();

            itemIn.ItemDetail.UnitPrice.Money.Currency = pdfViewModel.Currency.ToString();
            itemIn.ItemDetail.UnitPrice.Money.Text = pdfViewModel.PricePerGallon.GetPreciseValue(5).ToString();
            itemIn.ItemDetail.Description.Text = pdfViewModel.FuelType;
            itemIn.ItemDetail.UnitOfMeasure = pdfViewModel.UoM.ToString().Substring(0, 3).ToUpper();
            if (string.Equals(itemIn.ItemDetail.UnitOfMeasure, "LIT", StringComparison.OrdinalIgnoreCase))
            {
                itemIn.ItemDetail.UnitOfMeasure = "L";
            }
            itemIn.ItemDetail.Classification = GetUnspscNode(fuelUnspsc);
            itemIn.ItemDetail.ManufacturerName = _manufacturerCompanyName;

            if (pdfViewModel.ProductTypeId == (int)ProductTypes.ClearDiesel || pdfViewModel.ProductTypeId == (int)ProductTypes.RedDyeDiesel)
            {
                itemIn.ItemDetail.ManufacturerPartID = string.Concat(fuelUnspsc, _diesel1);
            }
            else if (pdfViewModel.ProductTypeId == (int)ProductTypes.ClearDiesel2 || pdfViewModel.ProductTypeId == (int)ProductTypes.RedDyeDiesel2)
            {
                itemIn.ItemDetail.ManufacturerPartID = string.Concat(fuelUnspsc, _diesel2);
            }
            else
            {
                itemIn.ItemDetail.ManufacturerPartID = fuelUnspsc;
            }

            return itemIn;
        }

        private static ItemIn GetDryRunAsItemIn(ConsolidatedInvoicePdfViewModel pdfViewModel)
        {
            var itemIn = new ItemIn();

            var itemId = pdfViewModel.Invoices.FirstOrDefault().FuelTypeId ?? 0;
            itemIn.Quantity = pdfViewModel.Invoices.Any(t => t.InvoiceTypeId == (int)InvoiceType.CreditInvoice) ? "-1" : "1";
            itemIn.ItemID.SupplierPartID = itemId.ToString();
            itemIn.ItemID.SupplierPartAuxiliaryID = itemId.ToString();

            itemIn.ItemDetail.UnitPrice.Money.Currency = pdfViewModel.Invoices.FirstOrDefault().Currency.ToString();
            itemIn.ItemDetail.UnitPrice.Money.Text = pdfViewModel.Invoices.FirstOrDefault().BasicAmount.GetPreciseValue(5).ToString();
            itemIn.ItemDetail.Description.Text = Resource.lblDryRunFee;
            itemIn.ItemDetail.UnitOfMeasure = pdfViewModel.Invoices.FirstOrDefault().UoM.ToString().Substring(0, 3).ToUpper();
            if (string.Equals(itemIn.ItemDetail.UnitOfMeasure, "LIT", StringComparison.OrdinalIgnoreCase))
            {
                itemIn.ItemDetail.UnitOfMeasure = "L";
            }
            return itemIn;
        }

        private static ItemIn GetFeeAsItemIn(FeesViewModel fee, int invoiceTypeId, decimal gallonsDropped, string currency, string uom)
        {
            var itemIn = new ItemIn();
            var isCommonFee = int.TryParse(fee.FeeTypeId, out int feeTypeId);

            itemIn.ItemID.SupplierPartID = fee.FeeTypeId;
            itemIn.ItemID.SupplierPartAuxiliaryID = fee.FeeTypeId;

            itemIn.Quantity = invoiceTypeId == (int)InvoiceType.CreditInvoice ? "-1" : "1";
            itemIn.ItemDetail.UnitOfMeasure = uom;
            itemIn.ItemDetail.UnitPrice.Money.Currency = currency;
            itemIn.ItemDetail.UnitPrice.Money.Text = (fee.Fee ?? 0).GetPreciseValue(2).ToString();
            itemIn.ItemDetail.Description.Text = $"{fee.FeeTypeName} - {fee.FeeSubTypeName}";
            if (!isCommonFee || feeTypeId == (int)FeeType.OtherFee)
            {
                itemIn.ItemID.SupplierPartAuxiliaryID = (fee.OtherFeeTypeId ?? feeTypeId).ToString();
                itemIn.ItemDetail.Description.Text = $"{fee.OtherFeeDescription} - {fee.FeeSubTypeName}";
            }

            if (fee.FeeSubTypeId == (int)FeeSubType.PerGallon)
            {
                itemIn.Quantity = gallonsDropped.GetPreciseValue(2).ToString();
            }
            else if (fee.FeeSubTypeId == (int)FeeSubType.ByQuantity)
            {
                var feeByQuantity = fee.DeliveryFeeByQuantity.FirstOrDefault(t => gallonsDropped >= t.MinQuantity && gallonsDropped <= (t.MaxQuantity ?? gallonsDropped));
                if (feeByQuantity != null)
                {
                    itemIn.ItemDetail.UnitPrice.Money.Text = feeByQuantity.Fee.GetPreciseValue(5).ToString();
                    itemIn.ItemDetail.Description.Text += $" From {feeByQuantity.MinQuantity} gallons to {feeByQuantity.MaxQuantity} gallons";
                }
            }
            else if (fee.FeeSubTypeId == (int)FeeSubType.ByAssetCount)
            {
                itemIn.Quantity = (fee.FeeSubQuantity ?? 0).GetPreciseValue(2).ToString();
            }
            else if (fee.FeeSubTypeId == (int)FeeSubType.HourlyRate)
            {
                itemIn.ItemDetail.UnitPrice.Money.Text = fee.TotalFee.GetPreciseValue(2).ToString();
                itemIn.ItemDetail.Description.Text += $" ({fee.TotalHours})";
            }
            else if (fee.FeeSubTypeId == (int)FeeSubType.Percent)
            {
                itemIn.ItemDetail.UnitPrice.Money.Text = fee.TotalFee.GetPreciseValue(2).ToString();
            }

            return itemIn;
        }

        private static ItemIn GetDiscountAsItemIn(DiscountLineItemViewModel fee, int invoiceTypeId, string currency, string uom)
        {
            var itemIn = new ItemIn();

            itemIn.ItemID.SupplierPartID = fee.FeeTypeId.ToString();
            itemIn.ItemID.SupplierPartAuxiliaryID = fee.FeeTypeId.ToString();

            itemIn.Quantity = invoiceTypeId == (int)InvoiceType.CreditInvoice ? "-1" : "1";
            itemIn.ItemDetail.UnitOfMeasure = uom;
            itemIn.ItemDetail.UnitPrice.Money.Currency = currency;
            itemIn.ItemDetail.UnitPrice.Money.Text = Math.Abs(fee.TotalFee).GetPreciseValue(2).ToString();
            itemIn.ItemDetail.Description.Text = $"{@Resource.lblDiscountOn} {fee.FeeTypeName} - {fee.FeeSubTypeName}";

            if (fee.FeeSubTypeId == (int)FeeSubType.Percent)
            {
                itemIn.ItemDetail.Description.Text = $"{@Resource.lblDiscountOn} {fee.FeeTypeName} - {fee.Amount}{"%"}";
            }

            if (fee.FeeTypeId == (int)FeeType.OtherFee)
            {
                itemIn.ItemID.SupplierPartAuxiliaryID = (fee.OtherFeeTypeId ?? fee.FeeTypeId).ToString();
                itemIn.ItemDetail.Description.Text = $"{@Resource.lblDiscountOn} {fee.FeeSubTypeName}";

                if (fee.FeeSubTypeId == (int)FeeSubType.Percent)
                {
                    itemIn.ItemDetail.Description.Text = $"{@Resource.lblDiscountOn} {fee.FeeSubTypeName} - {fee.Amount}{"%"}";
                }
            }

            decimal quantity = invoiceTypeId == (int)InvoiceType.CreditInvoice ? -1 : 1;
            itemIn.Quantity = (-1 * quantity).GetPreciseValue(2).ToString();
            return itemIn;
        }

        private static ItemIn GetTaxAsItemIn(TaxDetailsViewModel viewModel, string currency, string uom, int invoiceTypeId)
        {
            var itemIn = new ItemIn();

            itemIn.Quantity = invoiceTypeId == (int)InvoiceType.CreditInvoice ? "-1" : "1";
            itemIn.ItemID.SupplierPartID = viewModel.Id.ToString();
            itemIn.ItemID.SupplierPartAuxiliaryID = viewModel.Id.ToString();

            itemIn.ItemDetail.UnitPrice.Money.Currency = currency;
            itemIn.ItemDetail.UnitPrice.Money.Text = Math.Abs(viewModel.TradingTaxAmount).GetPreciseValue(5).ToString();
            itemIn.ItemDetail.Description.Text = viewModel.RateDescription;
            itemIn.ItemDetail.UnitOfMeasure = uom;

            return itemIn;
        }

        private static Classification GetUnspscNode(string fuelUnspsc)
        {
            Classification classification = null;
            if (!string.IsNullOrWhiteSpace(fuelUnspsc))
            {
                classification = new Classification();
                classification.Domain = ApplicationConstants.UNSPSC;
                classification.Text = fuelUnspsc;
            }
            return classification;
        }

        private static string GetProductCode(Dictionary<string, string> UNSPSC, string productTypeId)
        {
            var response = string.Empty;
            try
            {
                response = UNSPSC[productTypeId];
            }
            catch
            {
                if (productTypeId.StartsWith("FEE"))
                {
                    response = UNSPSC[ApplicationConstants.UnspscOtherFee];
                }
            }
            return response;
        }
    }
}
