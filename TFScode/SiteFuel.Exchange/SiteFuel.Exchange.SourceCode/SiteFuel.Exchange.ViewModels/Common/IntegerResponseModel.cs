using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Common
{
    public class IntegerResponseModel : StatusViewModel
    {
        public IntegerResponseModel()
        {
        }

        public IntegerResponseModel(Status status) : base(status)
        {
        }

        public int Result { get; set; }
    }
}
