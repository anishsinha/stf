using SiteFuel.BAL;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Logger;
using SiteFuel.FreightModels.ForcastingHelpers;
using SiteFuel.FreightRepository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TrueFill.HourlyWebJob
{
    public class ForecastingService
    {
        public async Task<bool> ProcessDailySalesCalculation()
        {
            var response = false;
            try
            {
                using (var tracer = new Tracer("ForecastingService", "ProcessDailySalesCalculation"))
                {
                    CultureInfo myCI = new CultureInfo("en-US", false);
                    myCI.DateTimeFormat.LongDatePattern = Resource.constFormatDate;
                    myCI.DateTimeFormat.LongTimePattern = Resource.constFormat24HourTime;

                    var startTime = DateTime.Now.Date.AddDays(-29);
                    var endDate = DateTime.Now.Date.AddDays(-1);
                    var drDomain = new ForecastingDomain(new ForecastingRepository());
                    await drDomain.ProcessMonthlySalesCalculation(startTime, endDate);
                    response = true;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ForecastingService", "ProcessDailySalesCalculation", ex.Message, ex);
            }
            return response;
        }

        public async Task<bool> ProcessForecastingTankCaculation()
        {
            var response = false;
            try
            {
                using (var tracer = new Tracer("ForecastingService", "ProcessForecastingTankCaculation"))
                {
                    var forecastignTankInformation = GetForecastingTankInformation();
                    if (forecastignTankInformation.Any())
                    {
                        var drDomain = new ForecastingDomain(new ForecastingRepository());
                        await drDomain.ProcessforcastingTankCaculation(forecastignTankInformation);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ForecastingService", "ProcessForecastingTankCaculation", ex.Message, ex);
            }
            return response;
        }
        private List<UspForecastingTankInfomation> GetForecastingTankInformation()
        {
            var response = new List<UspForecastingTankInfomation>();
            try
            {
                DataTable objDt = new DataTable();

                string connectionString = ConfigurationManager.ConnectionStrings["DatabaseConnection"] != null ? ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString : string.Empty;
                if (!string.IsNullOrEmpty(connectionString))
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        try
                        {
                            using (SqlCommand cmd = new SqlCommand("usp_GetTankForecastingSettingDetails"))
                            {
                                con.Open();
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Connection = con;
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
                            LogManager.Logger.WriteException("StoredProcedureLogDomain", "usp_GetTankForecastingSettingDetails", ex.Message, ex);
                        }

                    }
                    response = ConvertDataTableToList<UspForecastingTankInfomation>(objDt);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("StoredProcedureLogDomain", "GetExceptionLogs", ex.Message, ex);
            }
            return response;
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
                    {
                        if (dr[column.ColumnName] == DBNull.Value)
                        {
                            dr[column.ColumnName] = string.Empty;
                        }
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    }
                    else
                        continue;
                }
            }
            return obj;
        }

    }
}
