using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class MappedSupplierProductViewModel
    {
        public int Id { get; set; }
        public string AssignedName { get; set; }
        public string TerminalName { get; set; }
        public string BackOfficeProductCode { get; set; }
        public string FuelTypes { get; set; }
        public string SeaBoardProductCode { get; set; }
    }
}
