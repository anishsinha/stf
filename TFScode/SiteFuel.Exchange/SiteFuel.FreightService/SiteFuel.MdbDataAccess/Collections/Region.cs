using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.MdbDataAccess.Collections
{
    public class Region : CommonFields
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SlotPeriod { get; set; }
        public int TfxCompanyId { get; set; }
        public List<DropdownDisplayItem> TfxJobs { get; set; } = new List<DropdownDisplayItem>();
        public List<DropdownDisplayItem> TfxDrivers { get; set; } = new List<DropdownDisplayItem>();
        public List<DropdownDisplayItem> TfxDispatchers { get; set; } = new List<DropdownDisplayItem>();
        public List<DropdownDisplayItem> TfxTrailers { get; set; } = new List<DropdownDisplayItem>();
        public List<DropdownDisplayItem> TfxStates { get; set; } = new List<DropdownDisplayItem>();
        public List<TfxCarrierDropdownDisplayItem> TfxCarriers { get; set; } = new List<TfxCarrierDropdownDisplayItem>();
        public List<int> TfxProductTypeIds { get; set; } = new List<int>();
        public List<DropdownDisplayItem> TfxFuelTypeIds { get; set; } = new List<DropdownDisplayItem>();
        public RegionFavProductType? TfxFavProductTypeId { get; set; }
    }
}
