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
    public static class SalesOrderMapper
    {
        public static SalesOrderViewModel ToSalesOrderViewModel(this OrderPoViewModel viewModel)
        {
            var buyerCompanyName = viewModel.BuyerCompanyName.SubstringBeforeLast(" ", QbConstants.MaxStringLength);
            var salesOrder = new SalesOrderViewModel();
            salesOrder.OrderId = viewModel.OrderId;
            salesOrder.CustomerId = Convert.ToInt32(viewModel.CustomerId.Replace(QbConstants.CustomerNumberPrefix, string.Empty));
            salesOrder.CustomerName = buyerCompanyName;
            salesOrder.CustomerCompanyName = buyerCompanyName;
            salesOrder.TxnDate = viewModel.QbTxnDate;
            salesOrder.ShipDate = viewModel.DeliveryDetails.StartDate;
            salesOrder.BillAddress = new Quickbooks.Workflows.Models.AddressViewModel
            {
                CustomerName = viewModel.BuyerCompanyName,
                Address = viewModel.BuyerLocation.Address,
                City = viewModel.BuyerLocation.City.CovertUnicodeToNormal(),
                State = viewModel.BuyerLocation.StateCode,
                Country = viewModel.BuyerLocation.CountryCode,
                PostalCode = viewModel.BuyerLocation.ZipCode
            };
            salesOrder.ShipAddress = new Quickbooks.Workflows.Models.AddressViewModel
            {
                CustomerName = viewModel.JobName,
                Address = viewModel.ShippingLocation.Address,
                City = viewModel.ShippingLocation.City.CovertUnicodeToNormal(),
                State = viewModel.ShippingLocation.StateCode,
                Country = viewModel.ShippingLocation.CountryCode,
                PostalCode = viewModel.ShippingLocation.ZipCode
            };
            salesOrder.PONumber = viewModel.PoNumber;
            salesOrder.Memo = viewModel.PricePerGallon.Contains("+") || viewModel.PricePerGallon.Contains("-") ? viewModel.PricePerGallon : null;
            salesOrder.Items.Add(new OrderItemViewModel
            {
                Name = viewModel.FuelName.ToQbItemName(viewModel.TypeOfFuel),
                Desc = viewModel.FuelName + (string.IsNullOrWhiteSpace(salesOrder.Memo) ? string.Empty : $"({salesOrder.Memo})"),
                Quantity = viewModel.QuantityTypeId == (int)QuantityType.NotSpecified ? 1 : viewModel.GallonsOrdered.GetPreciseValue(QbConstants.MaxDecimal),
                Rate = viewModel.QbRate.GetPreciseValue(QbConstants.MaxDecimal)
            });

            foreach (var item in viewModel.FuelFees.FuelRequestFees)
            {
                var orderItem = item.ToQbOrderItem(viewModel.GallonsOrdered);
                salesOrder.Items.Add(orderItem);
            }

            return salesOrder;
        }
    }
}
