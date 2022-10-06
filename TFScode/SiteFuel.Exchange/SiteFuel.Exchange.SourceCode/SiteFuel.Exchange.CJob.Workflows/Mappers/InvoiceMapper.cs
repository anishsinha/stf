using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Quickbooks.Models;
using SiteFuel.Exchange.Quickbooks.Workflows.Extensions;
using SiteFuel.Exchange.Quickbooks.Workflows.Models;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.CJob.Workflows.Mappers
{
    public static class InvoiceMapper
    {
        public static QbInvoiceViewModel ToInvoiceViewModel(this ConsolidatedInvoicePdfViewModel viewModel, List<FeesViewModel> prevOrderFees, List<DiscountLineItemViewModel> prevDiscountItems)
        {
            var buyerCompanyName = viewModel.InvoicePdfHeaderDetail.BuyerCompanyName.SubstringBeforeLast(" ", QbConstants.MaxStringLength);
            var firstInvoice = viewModel.Invoices.FirstOrDefault();
            var invoice = new QbInvoiceViewModel();
            invoice.InvoiceId = firstInvoice.Id;
            invoice.InvoiceHeaderId = viewModel.InvoicePdfHeaderDetail.InvoiceHeaderId;
            invoice.InvoiceNumberId = firstInvoice.InvoiceNumber.Id;
            invoice.InvoiceNumber = firstInvoice.InvoiceNumber.Number;
            invoice.OrderId = firstInvoice.OrderId.Value;
            invoice.CustomerId = Convert.ToInt32(viewModel.InvoicePdfHeaderDetail.CustomerId.Replace(QbConstants.CustomerNumberPrefix, string.Empty));
            invoice.CustomerName = buyerCompanyName;
            invoice.CustomerCompanyName = buyerCompanyName;
            invoice.TxnDate = firstInvoice.DropEndDate;
            invoice.ShipDate = firstInvoice.DropEndDate;
            invoice.DueDate = firstInvoice.PaymentDueDate;
            invoice.BillAddress = viewModel.InvoicePdfHeaderDetail.BuyerLocation.ToQbAddressViewModel(viewModel.InvoicePdfHeaderDetail.BuyerCompanyName);
            invoice.ShipAddress = viewModel.InvoicePdfHeaderDetail.ShippingLocation.ToQbAddressViewModel(viewModel.InvoicePdfHeaderDetail.JobName);
            invoice.PONumber = firstInvoice.PoNumber;
            invoice.Memo = firstInvoice.PricePerGallonDisplay.Contains("+") || firstInvoice.PricePerGallonDisplay.Contains("-") ? firstInvoice.PricePerGallonDisplay : null;
            invoice.Items = GetOrderItems(viewModel, prevOrderFees, invoice.Memo);
            invoice.DiscountItems = GetDiscountItems(viewModel, prevDiscountItems);
            invoice.PaymentTermName = GetPaymentTermName(firstInvoice);
            invoice.PaymentTermDays = firstInvoice.NetDays;
            //invoice.PaymentTermDiscountDays = firstInvoice.dis.PaymentDiscount.WithinDays;
            //invoice.PaymentTermDiscountPct = firstInvoice.PaymentDiscount.DiscountPercent;
            invoice.OriginalInvoiceNumberId = firstInvoice.OriginalInvoiceNumberId;
            invoice.IsRebillInvoice = firstInvoice.IsRebillInvoice;
            return invoice;
        }

        private static string GetPaymentTermName(ConsolidatedInvoiceViewModel viewModel)
        {
            string termName = Resource.lblDueOnReceipt;
            if (viewModel.PaymentTermId == (int)PaymentTerms.NetDays)
            {
                termName = $"{Resource.lblNet} {viewModel.NetDays}";
            }
            else if (viewModel.PaymentTermId == (int)PaymentTerms.PrePaidInFull)
            {
                termName = Resource.lblPrePaidInFull;
            }
            return termName;
        }

        public static PurchaseOrderViewModel ToInvoicePoViewModel(this ConsolidatedInvoicePdfViewModel viewModel, List<FeesViewModel> prevOrderFees, List<DiscountLineItemViewModel> prevDiscountItems)
        {
            var supplierCompanyName = viewModel.InvoicePdfHeaderDetail.SupplierCompanyName.SubstringBeforeLast(" ", QbConstants.MaxCompanyLength);
            var purchaseOrder = new PurchaseOrderViewModel();
            var firstInvoice = viewModel.Invoices.FirstOrDefault();
            purchaseOrder.OrderId = firstInvoice.OrderId.Value;
            purchaseOrder.VendorId = Convert.ToInt32(viewModel.InvoicePdfHeaderDetail.CustomerId.Replace(QbConstants.CustomerNumberPrefix, string.Empty));
            purchaseOrder.VendorName = $"{supplierCompanyName}{QbConstants.Vendor}";
            purchaseOrder.VendorCompanyName = $"{supplierCompanyName}{QbConstants.Vendor}";
            purchaseOrder.CustomerId = viewModel.InvoicePdfHeaderDetail.PoContact?.CompanyId ?? 0;
            purchaseOrder.CustomerCompanyName = viewModel.InvoicePdfHeaderDetail.PoContact?.CompanyName;
            purchaseOrder.TxnDate = firstInvoice.DropEndDate;
            purchaseOrder.ShipDate = firstInvoice.DropEndDate;
            purchaseOrder.DueDate = firstInvoice.PaymentDueDate;
            purchaseOrder.InvoiceNumberId = firstInvoice.InvoiceNumber.Id;
            purchaseOrder.OriginalInvoiceNumberId = firstInvoice.OriginalInvoiceNumberId;
            purchaseOrder.IsRebillInvoice = purchaseOrder.OriginalInvoiceNumberId.HasValue && firstInvoice.InvoiceTypeId != (int)InvoiceType.CreditInvoice && firstInvoice.InvoiceTypeId != (int)InvoiceType.PartialCredit;
            purchaseOrder.VendorAddress = viewModel.InvoicePdfHeaderDetail.SupplierLocation.ToQbAddressViewModel(viewModel.InvoicePdfHeaderDetail.SupplierCompanyName);
            purchaseOrder.ShipAddress = viewModel.InvoicePdfHeaderDetail.ShippingLocation.ToQbAddressViewModel(viewModel.InvoicePdfHeaderDetail.JobName);
            purchaseOrder.PONumber = firstInvoice.PoNumber;
            purchaseOrder.Memo = firstInvoice.PricePerGallonDisplay.Contains("+") || firstInvoice.PricePerGallonDisplay.Contains("-") ? firstInvoice.PricePerGallonDisplay : null;
            purchaseOrder.Items = GetOrderItems(viewModel, prevOrderFees, purchaseOrder.Memo);
            purchaseOrder.DiscountItems = GetDiscountItems(viewModel, prevDiscountItems);
            return purchaseOrder;
        }

        public static QbBillViewModel ToBillViewModel(this ConsolidatedInvoicePdfViewModel viewModel, List<FeesViewModel> prevOrderFees = null, List<DiscountLineItemViewModel> prevDiscountItems = null)
        {
            if (prevOrderFees == null)
            {
                prevOrderFees = new List<FeesViewModel>();
            }
            if (prevDiscountItems == null)
            {
                prevDiscountItems = new List<DiscountLineItemViewModel>();
            }
            var supplierCompanyName = viewModel.InvoicePdfHeaderDetail.SupplierCompanyName.SubstringBeforeLast(" ", QbConstants.MaxCompanyLength);
            var bill = new QbBillViewModel();
            var firstBill = viewModel.Invoices.FirstOrDefault();
            bill.OrderId = firstBill.OrderId.Value;
            bill.IsRebillInvoice = firstBill.IsRebillInvoice;
            bill.OriginalInvoiceNumberId = firstBill.OriginalInvoiceNumberId;
            bill.OriginalInvoiceNumber = firstBill.OriginalInvoiceNumber;
            bill.VendorId = Convert.ToInt32(viewModel.InvoicePdfHeaderDetail.CustomerId.Replace(QbConstants.CustomerNumberPrefix, string.Empty));
            bill.VendorName = $"{supplierCompanyName}{QbConstants.Vendor}";
            bill.VendorCompanyName = $"{supplierCompanyName}{QbConstants.Vendor}";
            bill.TxnDate = firstBill.DropEndDate;
            bill.DueDate = firstBill.PaymentDueDate;
            bill.InvoiceNumberId = firstBill.InvoiceNumber.Id;
            bill.VendorAddress = viewModel.InvoicePdfHeaderDetail.SupplierLocation.ToQbAddressViewModel(viewModel.InvoicePdfHeaderDetail.SupplierCompanyName);
            bill.PoNumber = firstBill.PoNumber;
            bill.PaymentTermName = GetPaymentTermName(firstBill);
            bill.PaymentTermDays = firstBill.NetDays;
            bill.ReferenceNum = firstBill.InvoiceNumber.Number.Replace(ApplicationConstants.SFIN, string.Empty).Replace(ApplicationConstants.SFRB, string.Empty);
            //bill.PaymentTermDiscountDays = viewModel.PaymentDiscount.WithinDays;
            //bill.PaymentTermDiscountPct = viewModel.PaymentDiscount.DiscountPercent;
            bill.Memo = !string.IsNullOrEmpty(viewModel.InvoicePdfHeaderDetail.BrokerInvoiceNumber) ? viewModel.InvoicePdfHeaderDetail.BrokerInvoiceNumber + "/" + viewModel.InvoicePdfHeaderDetail.BrokerCompany : null;
            var memo = firstBill.PricePerGallonDisplay.Contains("+") || firstBill.PricePerGallonDisplay.Contains("-") ? firstBill.PricePerGallonDisplay : null;
            bill.Items = GetOrderItems(viewModel, prevOrderFees, memo);
            bill.DiscountItems = GetDiscountItems(viewModel, prevDiscountItems);
            return bill;
        }

        private static List<OrderItemViewModel> GetOrderItems(ConsolidatedInvoicePdfViewModel viewModel, List<FeesViewModel> orderFees, string memo = null)
        {
            var orderItems = new List<OrderItemViewModel>();
            foreach (var item in viewModel.Invoices)
            {
                if (item.InvoiceTypeId == (int)InvoiceType.DryRun)
                {
                    orderItems.Add(new OrderItemViewModel
                    {
                        Name = QbConstants.DryRunFee,
                        Desc = QbConstants.DryRunFee,
                        Quantity = 1,
                        Rate = item.BasicAmount.GetPreciseValue(QbConstants.MaxDecimal)
                    });
                }
                else
                {
                    if (item.InvoiceTypeId != (int)InvoiceType.Balance && item.InvoiceTypeId != (int)InvoiceType.TankRental)
                    {
                        orderItems.Add(new OrderItemViewModel
                        {
                            Name = item.FuelType.ToQbItemName(item.FuelTypeId ?? 0),
                            Desc = item.FuelType + (string.IsNullOrWhiteSpace(memo) ? string.Empty : $"({memo})"),
                            Quantity = Math.Abs(item.DroppedGallons).GetPreciseValue(QbConstants.MaxDecimal),
                            Rate = item.PricePerGallon.GetPreciseValue(QbConstants.MaxDecimal)
                        });
                    }
                }
            }

            AddTaxItem(viewModel, orderItems);
            var fuelfees = viewModel.FuelFees.FuelRequestFees
                            .Where(t => t.FeeTypeId != ((int)FeeType.DryRunFee).ToString()
                            && t.FeeSubTypeId != (int)FeeSubType.NoFee && !t.IncludeInPPG);

            foreach (var item in fuelfees)
            {
                orderItems.Add(item.ToQbInvoiceItem(orderFees));
            }

            return orderItems;
        }

        private static void AddTaxItem(ConsolidatedInvoicePdfViewModel viewModel, List<OrderItemViewModel> orderItems)
        {
            decimal totalTaxAmount = 0;
            if (viewModel.TaxDetail != null && viewModel.TaxDetail.TotalTaxAmount != 0)
            {
                totalTaxAmount = viewModel.TaxDetail.TotalTaxAmount;
            }
            orderItems.Add(GetTaxItem(totalTaxAmount));
        }

        private static OrderItemViewModel GetTaxItem(decimal totalTaxAmount)
        {
            return new OrderItemViewModel
            {
                Name = QbConstants.FuelSurcharges,
                Desc = QbConstants.FuelSurcharges,
                Quantity = 1,
                Rate = Math.Abs(totalTaxAmount).GetPreciseValue(QbConstants.MaxDecimal),
                IsNewlyAdded = true
            };
        }
        private static List<OrderItemViewModel> GetDiscountItems(ConsolidatedInvoicePdfViewModel viewModel, List<DiscountLineItemViewModel> DiscountItems)
        {
            var orderItems = new List<OrderItemViewModel>();
            foreach (var item in viewModel.Invoices)
            {
                if (item.AdditionalDetail.TotalAllowance.HasValue && item.AdditionalDetail.TotalAllowance != 0)
                {
                    var supplierAllowanceItem = GetSupplierAllowanceItem(item.AdditionalDetail.TotalAllowance.Value,
                                                        QbConstants.SupplierAllowance, QbConstants.SupplierAllowance);
                    orderItems.Add(supplierAllowanceItem);
                }
            }
            
            if (viewModel.Invoices.All(t => t.InvoiceTypeId != (int)InvoiceType.DryRun))
            {
                foreach (var item in viewModel.FuelFees.DiscountLineItems)
                {
                    orderItems.Add(item.ToQbInvoiceItem(DiscountItems));
                }
            }

            return orderItems;
        }
        private static Quickbooks.Workflows.Models.AddressViewModel ToQbAddressViewModel(this ViewModels.AddressViewModel address, string customerName)
        {
            return new Quickbooks.Workflows.Models.AddressViewModel()
            {
                CustomerName = customerName,
                Address = address.Address,
                City = address.City.CovertUnicodeToNormal(),
                Country = address.CountryCode,
                PostalCode = address.ZipCode,
                State = address.StateCode
            };
        }

        private static OrderItemViewModel GetSupplierAllowanceItem(decimal rate, string name, string desc)
        {
            var orderItem = new OrderItemViewModel();
            orderItem.Name = name;
            orderItem.Desc = desc;
            orderItem.Rate = rate.GetPreciseValue(QbConstants.MaxDecimal);
            orderItem.IsNewlyAdded = true;
            return orderItem;
        }
    }
}
