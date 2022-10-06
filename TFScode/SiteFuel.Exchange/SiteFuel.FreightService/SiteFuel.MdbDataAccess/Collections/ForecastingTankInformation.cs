using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace SiteFuel.MdbDataAccess.Collections
{
   public class ForecastingTankInformation
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string SiteId { get; set; }
        public string TankId { get; set; }
        public string StorageId { get; set; }
        public float DaysLeft { get; set; }
        public decimal EstimatedCurrentInventory { get; set; }
        public TankInformation TankInformation { get; set; } = new TankInformation();
    }
    public class TankInformation
    {
        public string Date { get; set; }
        public int BandNumber { get; set; }
        public int SaleTankId { get; set; }
        public decimal TotalSale { get; set; }
        public decimal AverageSale { get; set; }
    }
}
