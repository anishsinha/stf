using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SiteFuel.Exchange.FileGenerator.DTN
{
    public class DTN
    {

        /// <summary>
        /// The first line in a file is reserved for the Log-on string (Ref ID) and Password.
        /// </summary>
        public DtnLine1 DtnLine1 { get; } = new DtnLine1();
        /// <summary>
        /// The second line in file contains the message type(INV in this case).
        /// </summary>
        public string MessageType { get { return "INV"; } }
        /// <summary>
        /// A C02 command line group is REQUIRED for each document. The command line group is
        /// made up of two record lines.The first record line is comma delimited and explained below.The
        /// second record line is the DTN site number, which is unique for every customer.
        /// </summary>
        public C02CommandLineGroup C02CommandLineGroup { get; set; } = new C02CommandLineGroup();
        public BinaryData BinaryData { get; set; } = new BinaryData();
        public string GetCsvText()
        {
            var csvText = new StringBuilder();
            try
            {
                csvText.Append(DtnLine1.ToString() + DtnConstants.CRLF);
                csvText.Append(MessageType + DtnConstants.CRLF);
                csvText.Append(C02CommandLineGroup.ToString() + DtnConstants.CRLF);
                csvText.Append(BinaryData.ToString() + DtnConstants.CRLF);
                csvText.Append(DtnConstants.EndOfFile);
            }
            catch (Exception)
            {
                throw;
            }
            return csvText.ToString();
        }
    }
}
