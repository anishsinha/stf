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
    public static class ReportOutputMapper
    {
        public static InvoiceReportCsvViewModel ToCsvViewModel(this InvoiceReportViewModel viewModel)
        {
            var csvViewModel = new InvoiceReportCsvViewModel();
            csvViewModel.InvoiceNumber = viewModel.InvoiceNumber;
            csvViewModel.InvoiceAmount = viewModel.InvoiceAmount.ToString();
            csvViewModel.InvoiceDate = viewModel.InvoiceDate;
            csvViewModel.DeliveryAmount = viewModel.DeliveryAmount.ToString();
            csvViewModel.FuelAmount = viewModel.FuelAmount.ToString();
            csvViewModel.StateSalesTax = viewModel.StateSalesTax.ToString();
            csvViewModel.StateTax = viewModel.StateTax.ToString();
            csvViewModel.FederalTax = viewModel.FederalTax.ToString();
            csvViewModel.Description = viewModel.FuelType;
            csvViewModel.JobName = viewModel.JobName;
            return csvViewModel;
        }
    }
}
