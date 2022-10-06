﻿using System.Configuration;
using System.Data.SqlClient;

namespace SiteFuel.Exchange.PricingSources.DataAccess
{
    public static class DatabaseConnection
    {
        public static SqlConnection GetSqlConnection()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString;
            var _connection = new SqlConnection(connectionString);
            return _connection;
        }
    }
}
