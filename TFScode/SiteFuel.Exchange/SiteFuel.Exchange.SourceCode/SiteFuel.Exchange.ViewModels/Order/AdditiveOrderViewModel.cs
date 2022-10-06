using System.IO;

namespace SiteFuel.Exchange.ViewModels
{
    public class AdditiveOrderViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BuyerCompanyId { get; set; }
        public int JobId { get; set; }
        public string UoM { get; set; }
    }
}
