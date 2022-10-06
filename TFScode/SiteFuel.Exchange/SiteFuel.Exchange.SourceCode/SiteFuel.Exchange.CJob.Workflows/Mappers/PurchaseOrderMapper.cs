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
    public static class PurchaseOrderMapper
    {
        public static PurchaseOrderViewModel ToPurchaseOrderViewModel(this OrderPoViewModel viewModel)
        {
            var supplierCompanyName = viewModel.SupplierCompanyName.SubstringBeforeLast(" ", QbConstants.MaxCompanyLength);
            var purchaseOrder = new PurchaseOrderViewModel();
            purchaseOrder.OrderId = viewModel.OrderId;
            purchaseOrder.VendorId = Convert.ToInt32(viewModel.CustomerId.Replace(QbConstants.CustomerNumberPrefix, string.Empty));
            purchaseOrder.VendorName = $"{supplierCompanyName}{QbConstants.Vendor}";
            purchaseOrder.VendorCompanyName = $"{supplierCompanyName}{QbConstants.Vendor}";
            purchaseOrder.CustomerId = viewModel.PoContact?.CompanyId ?? 0;
            purchaseOrder.CustomerCompanyName = viewModel.PoContact?.CompanyName;
            purchaseOrder.TxnDate = viewModel.QbTxnDate;
            purchaseOrder.ShipDate = viewModel.DeliveryDetails.StartDate;
            purchaseOrder.VendorAddress = new Quickbooks.Workflows.Models.AddressViewModel
            {
                CustomerName = viewModel.SupplierCompanyName,
                Address = viewModel.SupplierLocation.Address,
                City = viewModel.SupplierLocation.City.CovertUnicodeToNormal(),
                State = viewModel.SupplierLocation.StateCode,
                Country = viewModel.SupplierLocation.CountryCode,
                PostalCode = viewModel.SupplierLocation.ZipCode
            };
            purchaseOrder.ShipAddress = new Quickbooks.Workflows.Models.AddressViewModel
            {
                CustomerName = viewModel.JobName,
                Address = viewModel.ShippingLocation.Address,
                City = viewModel.ShippingLocation.City.CovertUnicodeToNormal(),
                State = viewModel.ShippingLocation.StateCode,
                Country = viewModel.ShippingLocation.CountryCode,
                PostalCode = viewModel.ShippingLocation.ZipCode
            };
            purchaseOrder.PONumber = viewModel.PoNumber;
            purchaseOrder.Memo = viewModel.PricePerGallon.Contains("+") || viewModel.PricePerGallon.Contains("-") ? viewModel.PricePerGallon : null;
            purchaseOrder.Items.Add(new OrderItemViewModel
            {
                Name = viewModel.FuelName.ToQbItemName(viewModel.TypeOfFuel),
                Desc = viewModel.FuelName + (string.IsNullOrWhiteSpace(purchaseOrder.Memo) ? string.Empty : $"({purchaseOrder.Memo})"),
                Quantity = viewModel.QuantityTypeId == (int)QuantityType.NotSpecified ? 1 : viewModel.GallonsOrdered.GetPreciseValue(QbConstants.MaxDecimal),
                Rate = viewModel.QbRate.GetPreciseValue(QbConstants.MaxDecimal)
            });

            foreach (var item in viewModel.FuelFees.FuelRequestFees)
            {
                var orderItem = item.ToQbOrderItem(viewModel.GallonsOrdered);
                purchaseOrder.Items.Add(orderItem);
            }

            return purchaseOrder;
        }
    }
}
