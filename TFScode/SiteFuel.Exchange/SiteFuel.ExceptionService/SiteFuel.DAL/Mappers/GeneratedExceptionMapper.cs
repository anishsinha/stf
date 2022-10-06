using Newtonsoft.Json;
using SiteFuel.DataAccess.Entities;
using SiteFuel.Models.InvoiceException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.DAL.Mappers
{
    public static class GeneratedExceptionMapper
    {
        public static GeneratedException ToEntity(this GeneratedExceptionModel model, GeneratedException entity = null)
        {
            if (entity == null)
                entity = new GeneratedException();

            entity.InvoiceId = model.InvoiceId;
            entity.ExceptionTypeId = model.ExceptionTypeId;
            entity.OwnerCompanyId = model.OwnerCompanyId;
            entity.ApproverCompanyId = model.ApproverCompanyId;
            entity.BuyerCompanyId = model.BuyerCompanyId;
            entity.SupplierCompanyId = model.SupplierCompanyId;
            entity.StatusId = (int)model.Status;
            entity.ResolutionTypeId = model.ResolutionTypeId;
            entity.GeneratedOn = model.GeneratedOn;
            entity.ResolvedOn = model.ResolvedOn;
            entity.AutoApprovedOn = model.AutoApprovedOn;
            entity.GeneratedExceptionDetail = model.GeneratedExceptionDetail.ToEntity();

            return entity;
        }

        public static GeneratedExceptionDetail ToEntity(this GeneratedExceptionDetailModel model, GeneratedExceptionDetail entity = null)
        {
            if (entity == null)
                entity = new GeneratedExceptionDetail();

            var invoiceNumber = model.InvoiceNumber?.Replace("SFIN", "SFEDD");
            invoiceNumber = invoiceNumber?.Replace("SFDD", "SFEDDT");

            entity.DropDate = model.DropDate;
            entity.BuyerCompanyName = model.BuyerCompanyName;
            entity.SupplierCompanyName = model.SupplierCompanyName;
            entity.PoNumber = model.PoNumber;
            entity.InvoiceNumber = invoiceNumber;
            entity.JobName = model.JobName;
            entity.OrderedQuantity = model.OrderedQuantity;
            entity.BolQuantity = model.BolQuantity;
            entity.DeliveredQuantity = model.DeliveredQuantity;
            entity.Tolerance = model.Tolerance;
            entity.Varience = model.Varience;
            entity.ScheduledLocation = model.ScheduledLocation;
            entity.DroppedLocation = model.DroppedLocation;
            entity.PricePerGallon = model.PricePerGallon;
            entity.UserName = model.UserName;
            entity.ParameterJson = model.OrigionalInvoice != null ? JsonConvert.SerializeObject(model.OrigionalInvoice) : model.ParameterJson;
            entity.DriverId = model.DriverId;
            entity.DriverName = model.DriverName;
            entity.CarrierName = model.CarrierName;
            entity.UOM = model.UOM;
            return entity;
        }
    }
}
