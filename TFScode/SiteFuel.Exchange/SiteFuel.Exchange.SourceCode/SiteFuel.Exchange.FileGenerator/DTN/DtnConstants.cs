using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.FileGenerator.DTN
{
    public static class DtnConstants
    {
        public static readonly string CRLF = "\r\n";
        public static readonly string BeginBinaryData = "BEGIN-BINARY-DATA";
        public static readonly string EndBinaryData = "END-BINARY-DATA";
        public static readonly string EndOfFile = "<!--END OF FILE-->";
        public static readonly string DateFormat = "MM/dd/yyyy";
        public static readonly string TimeFormat = "HH:mm";
        public static readonly string NumberFormat0 = "0";
        public static readonly string NumberFormat2 = "0.00";
        public static readonly string NumberFormat4 = "0.0000";
    }
}
