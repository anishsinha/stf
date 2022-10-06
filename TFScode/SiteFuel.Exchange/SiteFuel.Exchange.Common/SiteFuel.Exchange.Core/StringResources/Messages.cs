using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Core.StringResources
{
    public class ResourceMessages
    {
        public static string GetMessage(string message, object[] parameters = null)
        {
            if (string.IsNullOrEmpty(message))
            {
                message = "Empty message";
            }

            if (parameters != null && parameters.Length > 0)
            {
                message = string.Format(message, parameters);
            }

            return message;
        }
    }
}
