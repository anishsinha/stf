using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.FileGenerator.DTN
{
    public class C02CommandLine
    {
        /// <summary>
        /// Field 1 = C02
        /// </summary>
        public string Field1 { get; set; } = "C02";
        /// <summary>
        /// Field 2 = 4-digit command sequence number, this should increment for each C02 command.
        /// </summary>
        public string Field2 { get; set; } = "0001";
        /// <summary>
        /// Field 3 = / / (space space/space space/space space) only enter MM/DD/YY if file should be sent at a later date
        /// </summary>
        public string Field3 { get; set; } = "  /  /  ";
        /// <summary>
        /// Field 4 = 00:00 (24HH:MM) only enter a time if file should be sent at a later time
        /// </summary>
        public string Field4 { get; set; } = "00:00";
        /// <summary>
        /// Field 5 = 0000
        /// </summary>
        public string Field5 { get; set; } = "0000";
        /// <summary>
        /// Field 6 = 1
        /// </summary>
        public string Field6 { get; set; } = "1";
        /// <summary>
        /// Field 7 = 0001
        /// </summary>
        public string Field7 { get; set; } = "0001";
        /// <summary>
        /// Field 8 = S
        /// </summary>
        public string Field8 { get; set; } = "S";
        /// <summary>
        /// Field 9 = $BILLING CODE
        /// </summary>
        public string Field9 { get; set; } = "$";
        public override string ToString()
        {
            var values = new StringBuilder();
            values.Append(Field1 + "," + Field2 + "," + Field3 + "," + Field4 + ",");
            values.Append(Field5 + "," + Field6 + "," + Field7 + "," + Field8 + "," + Field9);
            return values.ToString();
        }
    }

    public class C02CommandLineGroup
    {
        /// <summary>
        /// C02 Line Field Descriptions:
        /// Field 1 = C02
        /// Field 2 = 4-digit command sequence number, this should increment for each C02 command.
        /// Field 3 = / / (space space/space space/space space) only enter MM/DD/YY if file should be sent at a later date
        /// Field 4 = 00:00 (24HH:MM) only enter a time if file should be sent at a later time
        /// Field 5 = 0000
        /// Field 6 = 1
        /// Field 7 = 0001
        /// Field 8 = S
        /// Field 9 = $BILLING CODE
        /// </summary>
        public C02CommandLine C02CommandLine { get; set; } = new C02CommandLine();
        /// <summary>
        /// The second record line is the DTN site number, which is unique for every customer.
        /// </summary>
        public string SiteNumber { get; set; } = "SITEID#";
        public override string ToString()
        {
            return C02CommandLine + DtnConstants.CRLF + SiteNumber;
        }
    }
}
