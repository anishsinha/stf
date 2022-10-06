using MongoDB.Bson.Serialization.Attributes;

namespace SiteFuel.MdbDataAccess.Collections
{
    [BsonIgnoreExtraElements]
    public class BrokeredDRJob
    {
        public int TfxAssignToCompanyId { get; set; }
        public string TfxCustomerCompany { get; set; }
        public int? TfxSupplierCompanyId { get; set; }
        public int TfxJobId { get; set; }
        public int? TfxOrderId { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
    }
}
