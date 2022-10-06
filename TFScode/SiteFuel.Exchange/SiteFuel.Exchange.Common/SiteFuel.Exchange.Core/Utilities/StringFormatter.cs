using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Core.Utilities
{
    public static class StringFormatter
    {
        public static string Format(string prefix, int numberId)
        {
            return prefix + numberId.ToString().PadLeft(7, '0');
        }
    }
}
