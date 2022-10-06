using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.Core;
using System.ComponentModel.DataAnnotations;
using SiteFuel.Exchange.Core.StringResources;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class InvoiceDataTableViewModel : DataTableAjaxPostModel
    {
        public InvoiceDataTableViewModel()
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            Filter = InvoiceFilterType.All;
        }

        public int JobId { get; set; }

        public InvoiceFilterType Filter { get; set; }

        public int AllowedInvoiceType { get; set; }

        public int OrderId { get; set; }

        [Display(Name = nameof(Resource.lblFrom), ResourceType = typeof(Resource))]
        public string StartDate { get; set; }

        [Display(Name = nameof(Resource.lblTo), ResourceType = typeof(Resource))]
        public string EndDate { get; set; }

        public Currency Currency { get; set; }

        public int CountryId { get; set; }

        public string GroupIds { get; set; }
        public int CarrierCompanyId { get; set; }
        public List<int> CustomerIds { get; set; } = new List<int>();
        public List<int> LocationIds { get; set; } = new List<int>();
        public List<int> VesselIds { get; set; } = new List<int>();
        public bool IsMarine { get; set; }
    }
}
