using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class BadgeApiResponseViewModel
    {
        //"Carrier_ID": "2643           ",
        //"Badge_Number": "1                                       ",
        //"Customer_Number": 0.0,
        //"Carrier_Name": "WIEBE TRANSPORT                         ",
        //"Status": " "

        public string Carrier_ID { get; set; }
        public string Badge_Number { get; set; }
        public decimal Customer_Number { get; set; }
        public string Carrier_Name { get; set; }
        public string Status { get; set; }

    }
}
