using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Core.Utilities
{
    public static class MediaType
    {
        public static string Pdf { get { return "application/pdf"; } }

        public static string Text { get { return "text/csv"; } }

        public static string Png { get { return "image/png"; } }

        public static string Jpg { get { return "image/jpeg"; } }

        public static string Jpeg { get { return "image/jpeg"; } }

        public static string Bmp { get { return "image/bmp"; } }

        public static string Doc { get { return "application/msword"; } }

        public static string Docx { get { return "application/vnd.openxmlformats-officedocument.wordprocessingml.document"; } }

        public static string Xls { get { return "application/vnd.ms-excel"; } }

        public static string Xlsx { get { return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; } }
    }
}
