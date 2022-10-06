using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiteFuel.Exchange.Utilities;
namespace SiteFuel.Exchange.ViewModels
{
    public class AssignAssetsViewModel
    {
        public AssignAssetsViewModel()
        {
            Assets = new List<int>();
        }
        public int BuyerId { get; set; }

        public int JobId { get; set; }

        public List<int> Assets { get; set; }

    }
}
