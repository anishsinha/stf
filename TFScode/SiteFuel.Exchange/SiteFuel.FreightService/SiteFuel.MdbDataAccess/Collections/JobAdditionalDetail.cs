using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using System;
using System.Collections.Generic;


namespace SiteFuel.MdbDataAccess.Collections
{
    public class JobAdditionalDetail 
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string SiteImageFilePath { get; set; }
        public string AdditionalImageFilePath { get; set; }
        public string AdditionalImageDescription { get; set; }
        public List<int> DeliveryDays { get; set; }
        public List<DeliveryDaysViewModel> DeliveryDaysList { get; set; } = new List<DeliveryDaysViewModel>();
        public DateTimeOffset FromDeliveryTime { get; set; }
        public DateTimeOffset ToDeliveryTime { get; set; }
        public int TfxJobId { get; set; }
        public string TfxDisplayJobId { get; set; }
        public string TfxJobName { get; set; }
        public bool IsActive { get; set; }
        public bool IsAutoCreateDREnable { get; set; }
        public List<TankDetail> Tanks { get; set; } = new List<TankDetail>();
        public List<TrailerTypeStatus> TrailerType { get; set; }
        public string DistanceCovered { get; set; }

    }
}
