using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.PricingSources.DataAccess
{
    public class SqlParam
    {
        public static SqlParameter GetParameter(string paramName, object value)
        {
            var sqlParameter = new SqlParameter(paramName, value);
            if (value == null)
            {
                sqlParameter = new SqlParameter(paramName, DBNull.Value);
            }
            return sqlParameter;
        }
    }
}
