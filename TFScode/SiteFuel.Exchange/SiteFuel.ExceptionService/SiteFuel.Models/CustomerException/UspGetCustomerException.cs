using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Models.CustomerException
{
    public class UspGetCustomerException
    {
        public int ExceptionTypeId { get; set; }
        public string ExceptionTypeName { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsActive { get; set; }
        public int ResolutionId { get; set; }
        public string ResolutionName { get; set; }
    }
}
