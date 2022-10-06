using SiteFuel.Exchange.Core.StringResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteFuel.Exchange.Web.Helpers
{
    public static class DataTypeHelperMethods
    {
        public static string ToYesNo(this bool value)
        {
            return value == true ? Resource.lblYes : Resource.lblNo;
        }

        public static string ToEnabledDisabled(this bool value)
        {
            return value == true ? Resource.lblEnabled : Resource.lblDisabled;
        }
    }
}