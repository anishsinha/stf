using SiteFuel.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Models.CustomerException
{
    public class CustomerExceptionModel
    {
        public int ExceptionTypeId { get; set; }
        public string ExceptionTypeName { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsActive { get; set; }
        public List<ListItem> Resolutions { get; set; } = new List<ListItem>();
    }
}
