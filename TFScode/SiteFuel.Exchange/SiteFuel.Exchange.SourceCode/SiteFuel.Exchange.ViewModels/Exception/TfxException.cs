using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class TfxException:Exception
    {
        public TfxException(string message) : base(message)
        {

        }
    }
}
