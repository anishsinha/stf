using SiteFuel.Exchange.Core.StringResources;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class JobToCarrierAssignViewModel
    {
        [Display(Name = nameof(Resource.lblCarrierForLocation), ResourceType = typeof(Resource))]
        public int CarrierId { get; set; }
        public int JobId { get; set; }
        [Display(Name = nameof(Resource.lblLocation), ResourceType = typeof(Resource))]
        public string JobName { get; set; }
        public int UpdatedBy { get; set; }
        public int CompanyId { get; set; }
        public string CarrierName { get; set; }
        public int Id { get; set; }
        public int BuyerCompanyId { get; set; }
        public string BuyerCompanyName { get; set; }
         public bool IsCreateFreightOnlyOrder { get; set; }
    }
}
