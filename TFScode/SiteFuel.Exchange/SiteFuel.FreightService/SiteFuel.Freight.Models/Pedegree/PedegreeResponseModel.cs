using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Web;

namespace SiteFuel.FreightModels
{

    public class PedegreeResponseModel
    {
        public string SensorName { get; set; }
        public string Time { get; set; }
        public string storageTime { get; set; }

        public VolumeReading ReadingList{get;set;}
        public string PedegreeId { get; set; }
    }

    public class VolumeReading
    {
        public List<NameValuePair> NameValuePairs { get; set; }
    }

    public class NameValuePair { 
    public string Name { get; set; }
        public string Value { get; set; }
        public string Time { get; set; }
        public string StorageTime { get; set; }

        public int Status { get; set; }
        public int UnitId { get; set; }
        public bool AlreadyProcessed { get; set; }


    }
}
