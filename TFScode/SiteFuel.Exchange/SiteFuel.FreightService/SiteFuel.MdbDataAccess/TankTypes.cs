using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SiteFuel.MdbDataAccess.Interfaces;
using System;
using System.Collections.Generic;

namespace SiteFuel.MdbDataAccess
{
    public class TankModalType : ITankModalType
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string Modal { get; set; }
        public int ScaleMeasurement { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public int BuyerCompanyId { get; set; }
        public int SupplierCompanyId { get; set; }
        public int CreatedByCompanyId { get; set; }
        public bool IsActive { get; set; }
        public string PdfFilePath { get; set; }
        public List<DipChartDetails> DipChartDetails { get; set; }
    }
    public class DipChartDetails : IDipChartDetails
    {
        public decimal Dip { get; set; }
        public decimal Volume { get; set; }
        public decimal Ullage { get; set; }

    }
}
