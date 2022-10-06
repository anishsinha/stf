using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class SourcingDashboardOrderMapper
    {
        public static SalesUserDashboardOrderViewModel ToOrdersModel(this UspGetSourcingOrder entity, SalesUserDashboardOrderViewModel viewModel = null)
        {
            var helperDomain = new HelperDomain();
            if(viewModel == null)
            {
                viewModel = new SalesUserDashboardOrderViewModel();
            }
            viewModel.Id = entity.Id;
            viewModel.PoNumber = entity.PoNumber;
            viewModel.JobName = entity.Customer;
            viewModel.Customer = entity.Customer;
            viewModel.FuelType = entity.FuelType;
            viewModel.Quantity = helperDomain.GetQuantityRequested(entity.Quantity);
            viewModel.StatusId = entity.StatusId;

            return viewModel;
        }
        public static InvoiceGridSalesUserDashboardModel ToInvoiceModel(this UspGetSourcingInvoice entity, InvoiceGridSalesUserDashboardModel viewModel = null)
        {
            
            if (viewModel == null)
            {
                viewModel = new InvoiceGridSalesUserDashboardModel();
            }
            viewModel.Id = entity.Id;
            viewModel.InvoiceNumber = entity.InvoiceNumber;
            viewModel.PoNumber = entity.PoNumber;
            viewModel.SourcingRequest = entity.SourcingRequest;
            viewModel.DropDate = entity.DropDate.Date.ToString("MM-dd-yyyy");
            viewModel.Status = entity.Status;

            return viewModel;
        }
    }
}
