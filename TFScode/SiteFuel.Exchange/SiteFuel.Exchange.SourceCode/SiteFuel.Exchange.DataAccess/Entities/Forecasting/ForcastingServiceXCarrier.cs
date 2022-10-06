using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public class ForcastingServiceXCarrier
    {
        [Key, Column(Order = 0)]
        public int ForcastingServiceSettingId { get; set; }

        [Key, Column(Order = 1)]
        public int CarrierId { get; set; }

        [ForeignKey("CarrierId")]
        public virtual Company Company { get; set; }

        [ForeignKey("ForcastingServiceSettingId")]
        public virtual ForcastingServiceSetting ForcastingServiceSetting { get; set; }
    }
}
