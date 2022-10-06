using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Models
{
    public class SyncPricingResponseModel : BaseResponseModel
    {
        public SyncPricingResponseModel()
        {
        }
        public SyncPricingResponseModel(Status status) : base(status)
        {
            Status = status;
        }

        public List<SyncPricingResponse> PricingResponse { get; set; }
    }

    public class SyncPricingResponse
    {
        public int RecordInserted { get; set; }
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public int? SourceId { get; set; }
        public string ProductCode { get; set; }
        public int? TfxProductId { get; set; }
        public int? MstProductId { get; set; }
    }
}
