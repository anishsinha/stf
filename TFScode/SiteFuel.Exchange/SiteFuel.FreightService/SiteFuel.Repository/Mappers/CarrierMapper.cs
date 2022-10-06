using MongoDB.Bson;
using SiteFuel.FreightModels;
using SiteFuel.MdbDataAccess.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightRepository.Mappers
{
    public static class CarrierMapper
    {
        public static Carrier ToEntity(this CarrierViewModel carrier)
        {
            var entity = new Carrier();
            entity.TfxSupplierCompanyId = carrier.SupplierCompanyId;
            entity.TfxSupplierCompanyName = carrier.SupplierCompanyName;
            entity.TfxCarrierCompanyId = carrier.Carrier.Id;
            entity.TfxCarrierCompanyName = carrier.Carrier.Name;
            entity.CreatedBy = carrier.CreatedBy;
            entity.CreatedOn = carrier.CreatedOn;
            entity.IsActive = carrier.IsActive;
            entity.IsDeleted = carrier.IsDeleted;
            return entity;
        }
    }
}
