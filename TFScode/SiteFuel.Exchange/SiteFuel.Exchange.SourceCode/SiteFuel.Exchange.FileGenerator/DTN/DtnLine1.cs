using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.FileGenerator.DTN
{
    public class DtnLine1
    {
        public string REFID { get; set; } = "REFID";
        public string PASSWORD { get; set; } = "PASSWORD";
        public override string ToString()
        {
            return REFID + "," + PASSWORD;
        }
    }
}
