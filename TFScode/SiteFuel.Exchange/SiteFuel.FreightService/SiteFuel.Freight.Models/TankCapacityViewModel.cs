using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels
{
	public class TankCapacityViewModel
	{
		public DeliveryReqPriority Priority { get; set; }
		public decimal MaxPercent { get; set; }
		public decimal MinPercent { get; set; }
	}
}
