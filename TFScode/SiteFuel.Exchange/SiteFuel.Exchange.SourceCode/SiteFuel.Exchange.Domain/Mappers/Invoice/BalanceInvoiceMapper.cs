using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System.Linq;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class BalanceInvoiceMapper
    {
        public static Invoice ToBalanceInvoiceEntity(this InvoiceModel viewModel, Invoice entity = null)
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
            entity.IsOverWaterDelivery = false;
            entity.IsWetHosingDelivery = false;
            entity.PaymentTermId = viewModel.PaymentTermId;
            entity.NetDays = viewModel.NetDays;
            entity.PaymentDueDate = viewModel.PaymentDueDate;
            entity.PaymentDate = viewModel.PaymentDate;
            entity.CreatedBy = viewModel.CreatedBy;
            entity.CreatedDate = viewModel.CreatedDate;
            entity.IsActive = viewModel.IsActive;
            entity.UpdatedBy = viewModel.UpdatedBy;
            entity.UpdatedDate = viewModel.UpdatedDate;
            entity.TotalTaxAmount = viewModel.TotalTaxAmount;
            entity.TransactionId = "Balance";
            entity.TraceId = viewModel.TraceId;
            entity.WaitingFor = (int)WaitingAction.Nothing;
            entity.SupplierPreferredInvoiceTypeId = (int)InvoiceType.Manual;
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
            entity.QuantityIndicatorTypeId = viewModel.QuantityIndicatorTypeId;
            entity.InvoiceXInvoiceStatusDetails.ToList().ForEach(t => t.IsActive = false);
            InvoiceXInvoiceStatusDetail invoiceStatus = new InvoiceXInvoiceStatusDetail
            {
                StatusId = (int)InvoiceStatus.Received,
                UpdatedBy = viewModel.UpdatedBy,
                UpdatedDate = viewModel.UpdatedDate,
                IsActive = true
            };
            entity.InvoiceXInvoiceStatusDetails.Add(invoiceStatus);

            if (viewModel.AdditionalDetail != null)
            {
                entity.InvoiceXAdditionalDetail = viewModel.AdditionalDetail.ToEntity(entity.InvoiceXAdditionalDetail);
            }
            if (viewModel.FuelFees != null)
            {
                viewModel.FuelFees.ForEach(t => t.TotalFee = t.Fee);
            }
            if (viewModel.AdditionalDetail.PaymentMethod == PaymentMethods.CreditCard)
            {
                entity.FuelRequestFees = viewModel.FuelFees.Select(t => t.ToEntity()).ToList();
            }
            else
            {
                entity.FuelRequestFees = viewModel.FuelFees.Where(t => t.FeeTypeId != (int)FeeType.ProcessingFee).Select(t => t.ToEntity()).ToList();
            }

            if (entity.FuelRequestFees != null)
            {
                foreach (var item in entity.FuelRequestFees.Where(t => t.TaxDetails != null))
                {
                    entity.TaxDetails.Add(item.TaxDetails);
                    entity.TotalTaxAmount += item.TaxDetails.TradingTaxAmount;
                    viewModel.TotalTaxAmount = entity.TotalTaxAmount;
                }
            }
            //update processingfeeValue
            CheckForProcessingFeeOnTotalAmount(entity, viewModel);
            return entity;
        }

        private static void CheckForProcessingFeeOnTotalAmount(Invoice invoice, InvoiceModel invoiceModel)
        {
            var percentProcessingFee = invoice.FuelRequestFees.FirstOrDefault(t => t.FeeTypeId == (int)FeeType.ProcessingFee && t.FeeSubTypeId == (int)FeeSubType.Percent);
            if (percentProcessingFee != null)
            {
                var totalAmount = (invoice.BaseBasicAmount + (invoice.TotalFeeAmount ?? 0) + invoice.TotalTaxAmount - invoice.TotalDiscountAmount);
                percentProcessingFee.TotalFee = totalAmount * percentProcessingFee.Fee / 100;
                invoice.TotalFeeAmount += percentProcessingFee.TotalFee;
                invoiceModel.TotalFeeAmount += percentProcessingFee.TotalFee;
            }
        }
    }
}
