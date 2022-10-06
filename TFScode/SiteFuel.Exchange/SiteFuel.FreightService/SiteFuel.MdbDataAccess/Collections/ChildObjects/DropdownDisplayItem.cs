using MongoDB.Bson;

namespace SiteFuel.MdbDataAccess.Collections
{
    public class DropdownDisplayItem
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal FuelCapacity { get; set; }
    }
    public class TfxCarrierDropdownDisplayItem
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int SequenceNo { get; set; } = 0;
        public string RegionId { get; set; } =string.Empty;
    }
}