using MongoDB.Bson;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels.Driver;
using SiteFuel.MdbDataAccess.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightRepository.Mappers
{
    public static class DriverMapper
    {
        public static Driver ToEntity(this DriverObjectModel model)
        {
            var entity = new Driver();
            if (!string.IsNullOrWhiteSpace(model.Id))
            {
                entity.Id = ObjectId.Parse(model.Id);
            }
            entity.DriverId = model.DriverId;
            entity.DriverName = model.DriverName;
            entity.CompanyName = model.CompanyName;
            entity.ExpiryDate = model.ExpiryDate;
            entity.LicenseTypeId = model.LicenseTypeId;
            entity.CompanyId = model.CompanyId;
            entity.ProfilePhoto = model.ProfilePhoto;
            entity.LicenseNumber = model.LicenseNumber;
            entity.ShiftId = model.ShiftId.ToObjectList();
            entity.TrailerType = model.TrailerType == null ? new List<TrailerTypeStatus>() : model.TrailerType;
            entity.CardNumbers = model.CardNumbers.Select(t => t.ToEntity()).ToList();
            entity.Regions = model.Regions.ToObjectList();
            entity.IsActive = true;
            entity.IsDeleted = model.IsDeleted;
            entity.CreatedBy = model.CreatedBy;
            entity.CreatedOn = model.CreatedOn;
            entity.IsFilldAuthorized = model.IsFilldAuthorized;
            return entity;
        }

        public static TerminalCardNumber ToEntity(this TerminalCardNumberModel model)
        {
            var entity = new TerminalCardNumber();
            entity.TerminalId = model.TerminalId;
            entity.TerminalName = model.TerminalName;
            entity.CardNumber = model.CardNumber;
            return entity;
        }

        public static DriverObjectModel ToModel(this Driver entity)
        {
            var model = new DriverObjectModel();
            model.Id = entity.Id.ToString();
            model.DriverId = entity.DriverId;
            model.DriverName = entity.DriverName;
            model.CompanyId = entity.CompanyId;
            model.ProfilePhoto = entity.ProfilePhoto;
            model.LicenseNumber = entity.LicenseNumber;
            model.ShiftId = entity.ShiftId.ToStringList();
            model.TrailerType = entity.TrailerType == null ? new List<TrailerTypeStatus>() : entity.TrailerType.ToList();
            model.CardNumbers = entity.CardNumbers.Select(t => t.ToModel()).ToList();
            model.IsActive = entity.IsActive;
            model.IsDeleted = entity.IsDeleted;
            model.CreatedBy = entity.CreatedBy;
            model.CreatedOn = entity.CreatedOn;
            model.CompanyName = entity.CompanyName;
            model.ExpiryDate = entity.ExpiryDate;
            model.LicenseTypeId = entity.LicenseTypeId;
            model.Regions = entity.Regions.ToStringList();
            model.IsFilldAuthorized = entity.IsFilldAuthorized;

            return model;
        }

        private static List<string> ToStringList(this List<ObjectId> entity)
        {
            List<string> response = new List<string>();
            if (entity != null && entity.Any())
            {
                entity.ForEach(t => response.Add(t.ToString()));
            }
            return response;
        }

        private static List<ObjectId> ToObjectList(this List<string> entity)
        {
            List<ObjectId> response = new List<ObjectId>();
            if (entity != null && entity.Any())
            {
                entity.ForEach(t => response.Add(ObjectId.Parse(t)));
            }
            return response;
        }

        public static TerminalCardNumberModel ToModel(this TerminalCardNumber entity)
        {
            var model = new TerminalCardNumberModel();
            model.TerminalId = entity.TerminalId;
            model.TerminalName = entity.TerminalName;
            model.CardNumber = entity.CardNumber;
            return model;
        }
    }
}
