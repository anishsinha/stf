using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class QuickUpdateHistoryViewModel
    {
        public int Id { get; set; }
        public string Nm { get; set; }
        public string OTyp { get; set; }
        public string Trs { get; set; }
        public string Cstmrs { get; set; }
        public string FTyp { get; set; }
        public string Sts { get; set; }
        public string Cts { get; set; }
        public string ZpCds { get; set; }
        public string UTyp { get; set; }
        public string UNm { get; set; }
        public string Oprn { get; set; }
        public string UpdtdBy { get; set; }
        public string UDt { get; set; }
        public string UndoBy { get; set; }
        public string UndoDt { get; set; }
        public bool? IsVld { get; set; }
    }
}
