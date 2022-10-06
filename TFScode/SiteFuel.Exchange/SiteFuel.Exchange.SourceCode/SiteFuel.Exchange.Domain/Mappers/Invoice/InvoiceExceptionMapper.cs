using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.InvoiceExceptions;
using SiteFuel.Exchange.ViewModels.MobileAPI.TPD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class InvoiceExceptionMapper
    {
        public static InvoiceExceptionRequestModel ToExceptionRequestModel(this InvoiceCreateViewModel viewModel)
        {
            var model = new InvoiceExceptionRequestModel();
            model.BuyerCompanyId = viewModel.BuyerCompanyId;
            model.BuyerCompanyName = viewModel.BuyerCompanyName;
            model.SupplierCompanyId = viewModel.AcceptedCompanyId;
            model.SupplierCompanyName = viewModel.SupplierCompanyName;
            model.PoNumber = viewModel.PoNumber;
            model.DropDate = viewModel.DropEndDate;
            model.JobName = viewModel.JobName;
            model.OrderId = viewModel.OrderId;
            model.OrderedQuantity = viewModel.MaxQuantity;
            if (viewModel.BolDetails != null)
            {
                //if (viewModel.BolDetails.IsBolAvailable())
                    model.BolQuantity = viewModel.QuantityIndicatorTypeId == (int)QuantityIndicatorTypes.Net ? viewModel.BolDetails.NetQuantity ?? 0 : viewModel.BolDetails.GrossQuantity ?? 0;
                //else
                    //model.BolQuantity = viewModel.BolDetails.LiftQuantity ?? 0;
            }
            model.DroppedQuantity = viewModel.ActualDropQuantity ?? 0;

            return model;
        }

        public static InvoiceExceptionRequestModel ToExceptionRequestModel(this DropAdditionalDetailsModel viewModel, InvoiceModel invoice)
        {
            var model = new InvoiceExceptionRequestModel();

            model.BuyerCompanyId = viewModel.BuyerCompanyId;
            model.BuyerCompanyName = viewModel.BuyerCompanyName;
            model.SupplierCompanyId = viewModel.SupplierCompanyId;
            model.SupplierCompanyName = viewModel.SupplierCompanyName;
            model.CarrierName = string.Empty;
            model.OrderId = viewModel.OrderId;
            model.PoNumber = invoice.PoNumber;
            model.DropDate = invoice.DropEndDate;
            model.JobName = viewModel.JobName;
            model.OrderedQuantity = viewModel.MaxQuantity;
            model.BolQuantity = invoice.QuantityIndicatorTypeId == (int)QuantityIndicatorTypes.Net ? invoice.BolDetails.Sum(t => t.NetQuantity ?? 0) : invoice.BolDetails.Sum(t => t.GrossQuantity ?? 0);
            //model.BolQuantity += invoice.BolDetails.Sum(t => t.LiftQuantity ?? 0);
            model.DroppedQuantity = invoice.DroppedGallons;
            model.UserId = invoice.CreatedBy;
            model.DriverId = invoice.DriverId;
            model.UOM = invoice.UoM.ToString();
            return model;
        }

        public static InvoiceExceptionRequestModel ToExceptionRequestModel(this SplitLoadDraftDdtDetails viewModel)
        {
            var model = new InvoiceExceptionRequestModel();
            model.BuyerCompanyId = viewModel.Order.BuyerCompanyId;
            model.BuyerCompanyName = viewModel.BuyerCompanyName;
            model.SupplierCompanyId = viewModel.Order.AcceptedCompanyId;
            model.SupplierCompanyName = viewModel.SupplierCompanyName;
            model.PoNumber = viewModel.DraftDdt.PoNumber;
            model.InvoiceNumber = viewModel.DisplayInvoiceNumber;
            model.DropDate = viewModel.DraftDdt.DropEndDate;
            model.JobName = viewModel.Job.Name;
            model.OrderedQuantity = viewModel.FuelRequest.MaxQuantity;
            model.BolQuantity = viewModel.InvoiceCreateViewModel.QuantityIndicatorTypeId == (int)QuantityIndicatorTypes.Net ? viewModel.BolDetails.NetQuantity ?? 0 : viewModel.BolDetails.GrossQuantity ?? 0;
            model.DroppedQuantity = viewModel.InvoiceCreateViewModel.ActualDropQuantity ?? 0;

            return model;
        }

        public static InvoiceExceptionViewModel ToInvoiceExceptionModel(this ExceptionRaised exception)
        {
            var model = new InvoiceExceptionViewModel();
            model.InvoiceId = exception.InvoiceId ?? 0;
            model.GeneratedExceptionId = exception.ExceptionId;
            model.ExceptionTypeId = exception.ExceptionTypeId;
            model.RaisedOn = exception.RaisedOn;
            model.ApproverCompanyId = exception.ApproverCompanyId;
            model.StatusId = exception.StatusId;
            if (model.StatusId == (int)ExceptionStatus.Raised)
                model.IsActive = true;
            return model;
        }

        public static InvoiceException ToEntity(this InvoiceExceptionViewModel model)
        {
            var entity = new InvoiceException();
            entity.ExceptionTypeId = model.ExceptionTypeId;
            entity.GeneratedExceptionId = model.GeneratedExceptionId;
            entity.RaisedOn = model.RaisedOn;
            entity.StatusId = model.StatusId;
            entity.IsActive = model.IsActive;
            entity.CreatedDate = DateTimeOffset.Now;
            return entity;
        }

        public static InvoiceExceptionViewModel ToViewModel(this InvoiceException entity)
        {
            var model = new InvoiceExceptionViewModel();
            model.ExceptionTypeId = entity.ExceptionTypeId;
            model.GeneratedExceptionId = entity.GeneratedExceptionId;
            model.RaisedOn = entity.RaisedOn;
            model.StatusId = entity.StatusId;
            model.CreatedDate = entity.CreatedDate;
            model.IsActive = entity.IsActive;
            return model;
        }

        public static InvoiceExceptionRequestModel ToExceptionModel(this InvoiceCreateViewModel viewModel)
        {
            var model = new InvoiceExceptionRequestModel();

            model.BuyerCompanyId = viewModel.BuyerCompanyId;
            model.BuyerCompanyName = viewModel.BuyerCompanyName;
            model.SupplierCompanyId = viewModel.AcceptedCompanyId;
            model.SupplierCompanyName = viewModel.SupplierCompanyName;
            model.JobName = viewModel.JobName;
            model.OrderId = viewModel.OrderId;
            model.PricePerGallon = viewModel.PricePerGallon;
            model.DropDate = viewModel.DropEndDate;
            model.PoNumber = viewModel.PoNumber;

            return model;
        }

        public static InvoiceExceptionRequestModel ToExceptionModel(this DropAdditionalDetailsModel viewModel, InvoiceModel invoice)
        {
            var model = new InvoiceExceptionRequestModel();

            model.BuyerCompanyId = viewModel.BuyerCompanyId;
            model.BuyerCompanyName = viewModel.BuyerCompanyName;
            model.SupplierCompanyId = viewModel.SupplierCompanyId;
            model.SupplierCompanyName = viewModel.SupplierCompanyName;
            model.JobName = viewModel.JobName;
            model.OrderId = invoice.OrderId.Value;
            model.PricePerGallon = invoice.BolDetails.Select(t => t.PricePerGallon).FirstOrDefault();
            model.DropDate = invoice.DropEndDate;
            model.PoNumber = invoice.PoNumber;

            return model;
        }

        public static DuplicateInvoiceViewModel ToViewModel(this InvoiceExceptionRequestModel viewModel)
        {
            var model = new DuplicateInvoiceViewModel();

            model.OrderId = viewModel.OrderId;
            model.PricePerGallon = viewModel.PricePerGallon;
            model.DropDate = viewModel.DropDate;
            model.DropQuantity = viewModel.DroppedQuantity;
            model.UserId = viewModel.UserId;
            return model;
        }

        public static InvoiceExceptionModel ToViewModel(this UspDuplicateInvoice viewModel)
        {
            var model = new InvoiceExceptionModel();

            model.OrderId = viewModel.OrderId;
            model.InvoiceId = viewModel.InvoiceId;
            model.PoNumber = viewModel.PoNumber;
            model.InvoiceNumber = viewModel.DisplayInvoiceNumber;
            model.PricePerGallon = viewModel.PricePerGallon;
            model.DropDate = viewModel.DropDate;
            model.DroppedQuantity = viewModel.DroppedGallons;
            model.UserId = viewModel.CreatedBy;
            model.UserName = viewModel.UserName;
            return model;
        }

        public static InvoiceExceptionRequestModel ToExceptionModel(this InvoiceModel viewModel, InvoiceDropViewModel drop, TPDInvoiceViewModel apiViewModel)
        {
            var model = new InvoiceExceptionRequestModel();

            model.InvoiceNumberId = viewModel.InvoiceNumberId;
            model.DroppedQuantity = viewModel.DroppedGallons;
            model.DropDate = viewModel.DropEndDate;
            model.PoNumber = drop.PoNumber;
            model.OrderId = drop.OrderId;
            model.DriverName = $"{apiViewModel.DriverFirstName} {apiViewModel.DriverLastName}";
            model.DriverId = viewModel.DriverId;
            model.InvoiceNumber = viewModel.DisplayInvoiceNumber;
            model.OrderedQuantity = drop.OrderQuantity;
            model.ExternalRefID = apiViewModel.ExternalRefID;
            return model;
        }

        public static DeliveryMismatchExceptionModel ToExceptionRequestModel(this ExceptionApprovalResponseModel viewModel)
        {
            var model = new DeliveryMismatchExceptionModel();

            model.Id = viewModel.Id;
            model.CustomerId = viewModel.CustomerId;
            model.Customer = viewModel.Customer;
            model.Vendor = viewModel.Vendor;
            model.PoNumber = viewModel.PoNumber;
            model.InvoiceNumber = viewModel.InvoiceNumber;
            model.JobName = viewModel.JobName;
            model.DropDate = viewModel.DropDate;
            model.DropTime = viewModel.DropTime;
            model.OrderedQuantity = viewModel.OrderedQuantity;
            model.BolQuantity = viewModel.BolQuantity;
            model.DeliveredQuantity = viewModel.DeliveredQuantity;
            model.PricePerGallon = viewModel.PricePerGallon;
            model.Tolerance = viewModel.Tolerance;
            model.Varience = viewModel.Varience;
            model.StatusId = viewModel.StatusId;
            model.StatusName = viewModel.StatusName;
            model.AutoApprove = viewModel.AutoApprove;
            model.DriverName = viewModel.DriverName;
            model.ApprovedDate = viewModel.ApprovedDate;
            model.ResolvedOn = viewModel.ResolvedOn;
            model.UOM = viewModel.UOM;
            model.CarrierName = viewModel.CarrierName;
            model.ExceptionAdditionalDetail = viewModel.ExceptionAdditionalDetail;
            return model;
        }
    }
}
