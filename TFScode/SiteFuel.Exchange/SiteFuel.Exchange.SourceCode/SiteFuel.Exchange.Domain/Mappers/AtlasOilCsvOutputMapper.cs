using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class AtlasOilCsvOutputMapper
    {
        public static AtlasOilCsvOutputViewModel ToCsvViewModel(this UspAtlasOilReportViewModel viewModel)
        {
            var csvViewModel = new AtlasOilCsvOutputViewModel();
            csvViewModel.OrderNumber = viewModel.InvoiceNumber;
            csvViewModel.Driver = viewModel.Driver;
            csvViewModel.Account = viewModel.Account.RemoveWalmartWord();
            csvViewModel.Product = viewModel.Product;
            csvViewModel.GrossVolume = viewModel.GrossVolume.GetPreciseValue(5);
            csvViewModel.NetVolume = viewModel.NetVolume.GetPreciseValue(5);
            csvViewModel.UnitNumber = viewModel.UnitNumber;
            csvViewModel.DeliveryStart = viewModel.DeliveryStart;
            csvViewModel.DeliveryEnd = viewModel.DeliveryEnd;
            csvViewModel.StartingTotalizer = viewModel.StartingTotalizer.GetPreciseValue(5);
            csvViewModel.EndingTotalizer = viewModel.EndingTotalizer.GetPreciseValue(5);
            csvViewModel.Lat = viewModel.Lat.GetPreciseValue();
            csvViewModel.Lon = viewModel.Lon.GetPreciseValue();
            csvViewModel.Price = viewModel.Price.GetPreciseValue(5);
            csvViewModel.PONumber = viewModel.PONumber;
            csvViewModel.Source = viewModel.Source;
            csvViewModel.WarehouseID = viewModel.WarehouseID;
            return csvViewModel;
        }

        public static AtlasOilCarrierCsvOutputViewModel ToCsvViewModel(this UspAtlasOilCarrierReportViewModel viewModel)
        {
            var csvViewModel = new AtlasOilCarrierCsvOutputViewModel();
            csvViewModel.InvoiceNumber = viewModel.InvoiceNumber;
            csvViewModel.Product = viewModel.Product;
            csvViewModel.GrossVolume = viewModel.GrossVolume.GetPreciseValue(5);
            csvViewModel.NetVolume = viewModel.NetVolume.GetPreciseValue(5);
            csvViewModel.LoadingStartDate = viewModel.LoadingStartDate;
            csvViewModel.LoadingStartTime = viewModel.LoadingStartTime;
            csvViewModel.LoadingEndDate = viewModel.LoadingEndDate;
            csvViewModel.LoadingEndTime = viewModel.LoadingEndTime;
            csvViewModel.Source = viewModel.Source;
            csvViewModel.CarrierName = viewModel.CarrierName;
            csvViewModel.SupplierName = viewModel.CarrierName;
            csvViewModel.Supplier_StateCode = viewModel.CarrierName + " " + viewModel.StateCode;
            return csvViewModel;
        }

        private static string RemoveWalmartWord(this string input)
        {
            if (!string.IsNullOrWhiteSpace(input))
            {
                input = input.Replace("Walmart #", "");
                input = input.Replace("walmart #", "");
                input = input.Replace("Walmart#", "");
                input = input.Replace("walmart#", "");
            }
            return input;
        }
    }
}
