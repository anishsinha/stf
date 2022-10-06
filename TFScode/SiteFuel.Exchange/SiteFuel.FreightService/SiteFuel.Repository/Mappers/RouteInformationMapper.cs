using MongoDB.Bson;
using SiteFuel.FreightModels;
using SiteFuel.MdbDataAccess.Collections;
using System.Linq;

namespace SiteFuel.FreightRepository.Mappers
{
    public static class RouteInformationMapper
    {
        public static RouteInformations ToEntity(this RouteInformationModel model)
        {
            var entity = new RouteInformations();
            entity.Id = string.IsNullOrEmpty(model.Id) ? ObjectId.Empty : ObjectId.Parse(model.Id);
            entity.Name = model.Name;
            entity.RegionId = ObjectId.Parse(model.RegionId);
            if (model.TfxJobs != null)
                entity.TfxJobs = model.TfxJobs.Select(t => new TfxJobsDetails { Id = t.Id, Code = t.Code, Name = t.Name,SequenceNo=t.SequenceNo }).ToList();
            entity.TfxCompanyId = model.TfxCompanyId;
            entity.IsActive = true;
            entity.IsDeleted = false;
            entity.CreatedBy = model.CreatedBy;
            return entity;
        }
        public static ShiftInfo ToEntity(this ShiftInfoViewModel model)
        {
            var entity = new ShiftInfo();
            entity.Id = string.IsNullOrEmpty(model.Id) ? ObjectId.Empty : ObjectId.Parse(model.Id);
            entity.DriverColIndex = model.DriverColIndex;
            entity.DriverRowIndex = model.DriverRowIndex;
            entity.TripId = string.IsNullOrEmpty(model.TripId) ? ObjectId.Empty : ObjectId.Parse(model.TripId);
            entity.ShiftIndex = model.ShiftIndex;
            return entity;
        }
        public static ShiftInfoViewModel ToEntity(this ShiftInfo model)
        {
            var entity = new ShiftInfoViewModel();
            entity.Id = model.Id.ToString();
            entity.DriverColIndex = model.DriverColIndex;
            entity.DriverRowIndex = model.DriverRowIndex;
            entity.TripId = model.TripId.ToString();
            entity.ShiftIndex = model.ShiftIndex;
            return entity;
        }
    }
}
