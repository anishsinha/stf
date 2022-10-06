using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public class CompanyXServingLocation
    {
        [Key]
        public int Id { get; set; }
        public int AddressId { get; set; }
        public ServiceOfferingType ServiceOfferingType { get; set; }
        public int StateId { get; set; }
        public int? CityId { get; set; }
        public string ZipCode { get; set; }
        public virtual MstState MstState { get; set; }
        [ForeignKey("AddressId")]
        public virtual CompanyAddress CompanyAddress { get; set; }

    }
}
