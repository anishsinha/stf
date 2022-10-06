using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace SiteFuel.Exchange.Domain
{
    public class StoredProcedureLogDomain
    {
        public StoredProcedureLogDomain()
        {
        }

        #region Exception Logs
        // Exception Logs Methods
        public List<Usp_ExceptionLogsViewModel> GetExceptionLogs(string FromDateTime, string ToDateTime, DataTableSearchModel dataTableSearchValues, int timeout = 30)
        {
            var response = new List<Usp_ExceptionLogsViewModel>();
            try
            {
                DataTable objDt = new DataTable();
                string StartDate = DateTime.Now.AddDays(ApplicationConstants.DateFilterDefaultDays).ToString();
                string EndDate = DateTime.Now.ToString();

                string connectionString = ConfigurationManager.ConnectionStrings["LogDatabaseConnection"] != null ? ConfigurationManager.ConnectionStrings["LogDatabaseConnection"].ConnectionString : string.Empty;
                if (!string.IsNullOrEmpty(FromDateTime))
                {
                    StartDate = FromDateTime;
                }
                if (!string.IsNullOrEmpty(ToDateTime))
                {
                    EndDate = ToDateTime;
                }
                if (!string.IsNullOrEmpty(connectionString))
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        try
                        {
                            using (SqlCommand cmd = new SqlCommand("usp_GetExceptionLogs"))
                            {
                                con.Open();
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Connection = con;
                                cmd.Parameters.AddWithValue("@StartDate", StartDate);
                                cmd.Parameters.AddWithValue("@EndDate", EndDate);
                                cmd.Parameters.AddWithValue("@GlobalSearchText", dataTableSearchValues.GlobalSearchText);
                                cmd.Parameters.AddWithValue("@SortId", dataTableSearchValues.SortId);
                                cmd.Parameters.AddWithValue("@SortDirection", dataTableSearchValues.SortDirection);
                                cmd.Parameters.AddWithValue("@PageSize", dataTableSearchValues.PageSize);
                                cmd.Parameters.AddWithValue("@PageNumber", dataTableSearchValues.PageNumber);
                                CreateTableTypeParameter(cmd, dataTableSearchValues.DataTableSearchValues);
                                SqlDataAdapter objDa = new SqlDataAdapter();
                                objDa.SelectCommand = cmd;
                                DataSet objDs = new DataSet();
                                objDa.Fill(objDs);
                                if (objDs.Tables.Count > 0)
                                {
                                    objDt = objDs.Tables[0];
                                }
                                con.Close();
                            }
                        }
                        catch (Exception ex)
                        {
                            con.Close();
                            LogManager.Logger.WriteException("StoredProcedureLogDomain", "GetExceptionLogs", ex.Message, ex);
                        }

                    }
                    response = ConvertDataTableToList<Usp_ExceptionLogsViewModel>(objDt);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureLogDomain", "GetExceptionLogs", ex.Message, ex);
            }
            return response;
        }
        public String GetParticularException(int id, int timeout = 30)
        {
            var response = "";
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["LogDatabaseConnection"] != null ? ConfigurationManager.ConnectionStrings["LogDatabaseConnection"].ConnectionString : string.Empty;
                if (!string.IsNullOrEmpty(connectionString))
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        try
                        {
                            using (SqlCommand cmd = new SqlCommand("usp_GetParticularException"))
                            {
                                con.Open();
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Connection = con;
                                cmd.Parameters.AddWithValue("@Id", id);
                                SqlDataReader reader = cmd.ExecuteReader();
                                if (reader.Read())
                                {
                                    response = reader.GetString(0);
                                }
                            }
                            con.Close();

                        }
                        catch (Exception ex)
                        {
                            con.Close();
                            LogManager.Logger.WriteException("StoredProcedureLogDomain", "GetParticularException", ex.Message, ex);
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureDomain", "GetParticularException", ex.Message, ex);
            }
            return response;
        }
        private static void CreateTableTypeParameter(SqlCommand command, List<DataTableSearchValues> dataTableSearchValues)
        {

            foreach (var itemSearchValues in dataTableSearchValues)
            {
                DataTable table = new DataTable();
                table.Columns.Add($"{itemSearchValues.Name}SearchTypes", typeof(string));
                foreach (var item in itemSearchValues.SearchVals)
                {
                    table.Rows.Add(item);
                }
                command.Parameters.AddWithValue($"{itemSearchValues.Name}SearchTypes", table);
            }

        }
        private static List<T> ConvertDataTableToList<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }
        #endregion

    }

}
