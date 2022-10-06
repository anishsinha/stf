using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Utilities
{
    public static  class ListHelperMethods
    {
        public static bool AnyAndNotNull<TSource>(this List<TSource> list)
        {
            return (list?.Any() ?? false);
        }
    }
}
