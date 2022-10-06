using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class Usp_ExceptionLogsViewModel
    {
        public int ID { get; set; }
        public string MachineName { get; set; }
        public DateTime LogDateTime { get; set; }
        public string Level { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public int TotalCount { get; set; }
    }
}
