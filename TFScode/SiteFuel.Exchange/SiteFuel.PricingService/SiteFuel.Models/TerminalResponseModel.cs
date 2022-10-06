using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Models
{
    public class TerminalResponseModel : BaseResponseModel
    {
        public TerminalResponseModel()
        {
        }
        public TerminalResponseModel(Status status) : base(status)
        {
            Status = status;
        }
        public List<TerminalDetails> Terminals { get; set; }
    }

    public class TerminalDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Distance { get; set; }
        public string AvgPrice { get; set; }
    }
}
