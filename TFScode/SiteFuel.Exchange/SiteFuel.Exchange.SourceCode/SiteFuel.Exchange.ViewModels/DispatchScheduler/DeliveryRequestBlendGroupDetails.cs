using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class DeliveryRequestBlendGroupDetails
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public string DeliveryLevelPO { get; set; }
    }
}
