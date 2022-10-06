using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers.TankRental;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System.Linq;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class TankRentalInvoiceMapper
    {
        public static Invoice ToTankRentalInvoiceEntity(this InvoiceModel viewModel, Invoice entity = null)
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
            entity.TransactionId = "TankRental";
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
            entity.QbInvoiceNumber = viewModel.QbInvoiceNumber;
            entity.TotalDiscountAmount = viewModel.TotalDiscountAmount;
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
                CheckForProcessingFeeOnTotalAmount(viewModel);
                entity.TotalFeeAmount = viewModel.TotalFeeAmount;
                entity.FuelRequestFees = viewModel.FuelFees.Select(t => t.ToEntity()).ToList();
            }
            else
            {
                entity.FuelRequestFees = viewModel.FuelFees.Where(t => t.FeeTypeId != (int)FeeType.ProcessingFee).Select(t => t.ToEntity()).ToList();
            }

            if (viewModel.TankFrequency != null && viewModel.TankFrequency.Tanks.Any(t => t.IsToBeIncludedInInvoice))
            {
                foreach (var item in viewModel.TankFrequency.Tanks.Where(t => t.IsToBeIncludedInInvoice))
                {
                    entity.BasicAmount += item.RentalFee;
                    viewModel.BasicAmount = entity.BasicAmount;
                    var fuelFee = item.ToEntity();
                    entity.FuelRequestFees.Add(fuelFee);

                    if (fuelFee.TaxDetails != null)
                    {
                        entity.TaxDetails.Add(fuelFee.TaxDetails);
                        entity.TotalTaxAmount += fuelFee.TaxDetails.TradingTaxAmount;
                        viewModel.TotalTaxAmount = entity.TotalTaxAmount;
                    }
                }
                entity.InvoiceXAdditionalDetail.TankFrequencyId = viewModel.AdditionalDetail.TankFrequencyId;
            }

            return entity;
        }

        private static void CheckForProcessingFeeOnTotalAmount(InvoiceModel invoiceModel)
        {
            var percentProcessingFee = invoiceModel.FuelFees.FirstOrDefault(t => t.FeeTypeId == (int)FeeType.ProcessingFee && t.FeeSubTypeId == (int)FeeSubType.Percent);
            if (percentProcessingFee != null)
            {
                var totalAmount = (invoiceModel.BasicAmount + (invoiceModel.TotalFeeAmount ?? 0) + invoiceModel.BaseTotalTaxAmount - invoiceModel.TotalDiscountAmount);
                percentProcessingFee.TotalFee = totalAmount * percentProcessingFee.Fee / 100;
                invoiceModel.TotalFeeAmount += percentProcessingFee.TotalFee;
            }
        }

        public static Invoice ToAutoTankRentalInvoiceEntity(this InvoiceModel viewModel, Invoice entity = null)
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
            entity.TransactionId = "TankRental";
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
            entity.QbInvoiceNumber = viewModel.QbInvoiceNumber;
            entity.TotalDiscountAmount = viewModel.TotalDiscountAmount;
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
                CheckForProcessingFeeOnTotalAmount(viewModel);
                entity.TotalFeeAmount = viewModel.TotalFeeAmount;
                entity.FuelRequestFees = viewModel.FuelFees.Select(t => t.ToEntity()).ToList();
            }
            else
            {
                entity.FuelRequestFees = viewModel.FuelFees.Where(t => t.FeeTypeId != (int)FeeType.ProcessingFee).Select(t => t.ToEntity()).ToList();
            }

            if (viewModel.TankFrequency != null && viewModel.TankFrequency.Tanks.Any(t => t.IsToBeIncludedInInvoice))
            {
                foreach (var item in viewModel.TankFrequency.Tanks.Where(t => t.IsToBeIncludedInInvoice))
                {
                    entity.BasicAmount = viewModel.BasicAmount;
                    var fuelFee = item.ToEntity();
                    entity.FuelRequestFees.Add(fuelFee);

                    if (fuelFee.TaxDetails != null)
                    {
                        entity.TaxDetails.Add(fuelFee.TaxDetails);
                        entity.TotalTaxAmount = viewModel.TotalTaxAmount;
                    }
                }
            }

            return entity;
        }
    }
}
