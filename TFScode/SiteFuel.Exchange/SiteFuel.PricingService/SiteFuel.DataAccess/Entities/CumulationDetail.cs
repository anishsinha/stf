using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.DataAccess.Entities
{
    public class CumulationDetail
    {
        public int Id { get; set; }
        public int RequestPriceDetailId { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public decimal CumulatedQuantity { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("RequestPriceDetailId")]
        public virtual RequestPriceDetail RequestPriceDetails { get; set; }
    }
}
