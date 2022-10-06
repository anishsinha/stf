using Newtonsoft.Json;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class NewsfeedParameters
    {
        public NewsfeedParameters()
        {
            MessageParameters = new Dictionary<string, string>();
        }

        public int EntityId { get; set; }

        public EntityType EntityType { get; set; }

        public NewsfeedEvent EventTypeId { get; set; }

        public int CreatedByUserId { get; set; }

        public int BuyerCompanyId { get; set; }

        public int SupplierCompanyId { get; set; }

        public int TargetEntityId { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public IDictionary<string, string> MessageParameters { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    public class ConversionNewsfeedViewModel
    {
        public int OrderId { get; set; }
        public int JobId { get; set; }
        public int InvoiceId { get; set; }
        public int InvoiceHeaderId { get; set; }
        public int InvoiceNumberId { get; set; }
        public string InvoiceNumber { get; set; }
        public int BuyerCompanyId { get; set; }
        public int SupplierCompanyId { get; set; }
        public int UserId { get; set; }
        public string BuyerCompanyName { get; set; }
        public string SupplierCompanyName { get; set; }
        public DateTimeOffset CreatedDate { get; set; }

    }
}
