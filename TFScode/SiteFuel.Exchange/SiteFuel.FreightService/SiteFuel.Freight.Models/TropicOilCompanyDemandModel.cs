using FileHelpers;
using Newtonsoft.Json;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels
{
    [DelimitedRecord(",")]
    [IgnoreEmptyLines]
    public class TropicOilCompanyDemandModel
    {
        [FieldQuoted]
        public string LocationID { get; set; }
        [FieldQuoted]
        public string TankID { get; set; }
        [FieldQuoted]
        public string Customer { get; set; }
        [FieldQuoted]
        public string Location { get; set; }
        [FieldQuoted]
        public string Address { get; set; }
        [FieldQuoted]
        public string City { get; set; }
        [FieldQuoted]
        public string State { get; set; }
        [FieldQuoted]
        public string ZipCode { get; set; }
        [FieldQuoted]
        public string Tank { get; set; }
        [FieldQuoted]
        public string Product { get; set; }
        [FieldQuoted]
        public string InventoryTimeUTC { get; set; }
        [FieldQuoted]
        public string Inventory { get; set; }
        [FieldQuoted]
        public string Level { get; set; }
        [FieldQuoted]
        public string TankCapacity { get; set; }
        [FieldQuoted]
        public string TankHeight { get; set; }
        [FieldQuoted]
        public string MIN { get; set; }
        [FieldQuoted]
        public string RTUID { get; set; }
        [FieldQuoted]
        public string Temp_F { get; set; }
        [FieldQuoted]
        public string Region { get; set; }
        [FieldQuoted]
        public string CriticalHighAlarm { get; set; }
        [FieldQuoted]
        public string HighAlarm { get; set; }
        [FieldQuoted]
        public string LowAlarm { get; set; }
        [FieldQuoted]
        public string CriticalLowAlarm { get; set; }
        [FieldQuoted]
        public string UserLocationId { get; set; }
        [FieldQuoted]
        public string UserProductId { get; set; }
        [FieldQuoted]
        public string BatteryLevel { get; set; }
        [FieldQuoted]
        public string RSSI { get; set; }
        [FieldQuoted]
        public string DoNotFill { get; set; }
        [FieldQuoted]
        public string UserTankId { get; set; }
        [FieldQuoted]
        public string VolumeUOM { get; set; }
        [FieldQuoted]
        public string LevelUOM { get; set; }
        [FieldQuoted]
        public string SpecificGravity { get; set; }
        [FieldQuoted]
        public string Latitude { get; set; }
        [FieldQuoted]
        public string Longitude { get; set; }
        [FieldQuoted]
        public string Type { get; set; }
    }

    public class FTPConfig
    {
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RemoteDirectory { get; set; }
        public string ConsumerId { get; set; }
        public string Authentication { get; set; }
        public string Url { get; set; }
        public string SoapAction { get; set; }

    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Inventory
    {
        public string sUserOrganizationID { get; set; }
        public object sUserLocationID { get; set; }
        public string sUserTankID { get; set; }
        public string iTankID { get; set; }
        public string iInventoryID { get; set; }
        public string sUTCInventoryTime { get; set; }
        public string dtLocalInventoryTime { get; set; }
        public string sProductName { get; set; }
        public string sUserProductID { get; set; }
        public string dLevel { get; set; }
        public string dGrossVolume { get; set; }
        public string dNetVolume { get; set; }
        public string dTemperature { get; set; }
        public string iBatteryLevel { get; set; }
        public string iRSSILevel { get; set; }
        public string iSensorData { get; set; }
        public string iSensorMax { get; set; }
        public string iTemperatureData { get; set; }
        public string dSensorRange { get; set; }
        public string iSensorOffset { get; set; }
        public string iLowSetPoint { get; set; }
        public string iHighSetPoint { get; set; }
        public string sSerialNumber { get; set; }
    }

    public class GetInventoryResult
    {
        public List<Inventory> Inventory { get; set; }
    }

    public class GetInventoryResponse
    {
        [JsonProperty("@xmlns")]
        public string Xmlns { get; set; }
        public GetInventoryResult GetInventoryResult { get; set; }
        public string iErrorCode { get; set; }
        public object sErrorMsg { get; set; }
        public string iTransactionId { get; set; }
    }

    public class SoapBody
    {
        public GetInventoryResponse GetInventoryResponse { get; set; }
    }

    public class SoapEnvelope
    {
        [JsonProperty("@xmlns:soap")]
        public string XmlnsSoap { get; set; }

        [JsonProperty("@xmlns:xsi")]
        public string XmlnsXsi { get; set; }

        [JsonProperty("@xmlns:xsd")]
        public string XmlnsXsd { get; set; }

        [JsonProperty("soap:Body")]
        public SoapBody SoapBody { get; set; }
    }

    public class Root
    {
        [JsonProperty("soap:Envelope")]
        public SoapEnvelope SoapEnvelope { get; set; }
    }
}
