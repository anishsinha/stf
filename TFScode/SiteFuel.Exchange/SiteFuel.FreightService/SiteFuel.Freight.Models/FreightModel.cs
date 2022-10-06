using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels
{
    public class FreightModel
    {
        public string Id { get; set; }
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public decimal FuelCapacity { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string ContractNumber { get; set; }
        public string Description { get; set; }
        public FreightTypeModel FreightType { get; set; }
        public List<CompartmentModel> Compartments { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
