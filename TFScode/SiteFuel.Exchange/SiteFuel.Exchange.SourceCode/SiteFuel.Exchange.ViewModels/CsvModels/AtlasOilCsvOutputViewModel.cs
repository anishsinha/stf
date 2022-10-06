using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    [DelimitedRecord(",")]
    public class AtlasOilCsvOutputViewModel
    {
        public AtlasOilCsvOutputViewModel()
        {
        }

        [FieldOrder(1), FieldQuoted]
        public string OrderNumber { get; set; }

        [FieldOrder(2), FieldQuoted]
        public string LineItemNumber { get; set; }

        [FieldOrder(3), FieldQuoted]
        public string SequenceNumber { get; set; }

        [FieldOrder(4), FieldQuoted]
        public string OrderType { get; set; }

        [FieldOrder(5), FieldQuoted]
        public string Demurrage { get; set; }

        [FieldOrder(6), FieldQuoted]
        public string Source { get; set; }

        [FieldOrder(7), FieldQuoted]
        public string Vehicle { get; set; }

        [FieldOrder(8), FieldQuoted]
        public string Driver { get; set; }

        [FieldOrder(9), FieldQuoted]
        public string Account { get; set; }

        [FieldOrder(10), FieldQuoted]
        public string Product { get; set; }

        [FieldOrder(11)]
        public decimal GrossVolume { get; set; }

        [FieldOrder(12)]
        public decimal NetVolume { get; set; }

        [FieldOrder(13), FieldQuoted]
        public string Barcode { get; set; }

        [FieldOrder(14), FieldQuoted]
        public string UnitNumber { get; set; }

        [FieldOrder(15), FieldQuoted]
        public string DeliveryStart { get; set; }

        [FieldOrder(16), FieldQuoted]
        public string DeliveryEnd { get; set; }

        [FieldOrder(17)]
        public decimal StartingTotalizer { get; set; }

        [FieldOrder(18)]
        public decimal EndingTotalizer { get; set; }

        [FieldOrder(19), FieldQuoted]
        public string RegSalesNo { get; set; }

        [FieldOrder(20)]
        public decimal Lat { get; set; }

        [FieldOrder(21)]
        public decimal Lon { get; set; }

        [FieldOrder(22)]
        public decimal Price { get; set; }

        [FieldOrder(23), FieldQuoted]
        public string PONumber { get; set; }

        [FieldOrder(24), FieldQuoted]
        public string WarehouseID { get; set; }
    }
}
