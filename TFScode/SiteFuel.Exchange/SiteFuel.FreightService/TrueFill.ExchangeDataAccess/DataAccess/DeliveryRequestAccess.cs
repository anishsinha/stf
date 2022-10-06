using SiteFuel.Exchange.Logger;
using SiteFuel.FreightModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace TrueFill.ExchangeDataAccess.DataAccess
{
    public class DeliveryRequestAccess
    {
        public async Task<DeliveryRequestViewModel> GetAdditionalDeliveryRequestDetails(int jobId, int supplierCompanyId, int productTypeId)
        {
            DeliveryRequestViewModel response = null;
            try
            {
                using (var connection = DatabaseConnection.GetSqlConnection())
                {
                    try
                    {

                        var queryString = $@"Select TOP 1 
                                                J.Name AS JobName,
                                                J.Address AS JobAddress,
                                                ACOM.Name AS SupplierCompany,
                                                ACOM.Id AS SupplierCompanyId,
                                                BCOM.Name AS BuyerCompany,
                                                BCOM.Id AS BuyerCompanyId,
                                                J.UoM,
                                                O.AcceptedBy AS SupplierUserId,
                                                J.City AS JobCity,
                                                J.TimeZoneName
                                        from JOBS J
                                        INNER JOIN FuelRequests FR ON FR.JobId = J.Id
                                        INNER JOIN Orders O ON O.FuelRequestId = FR.Id
                                        INNER JOIN Companies BCOM ON BCOM.Id = O.BuyerCompanyId
                                        INNER JOIN Companies ACOM ON ACOM.Id = O.AcceptedCompanyId
                                        where
                                        J.Id = {jobId}
                                        AND
                                        O.AcceptedCompanyId = {supplierCompanyId}";

                        var sqlCommand = connection.CreateCommand();
                        sqlCommand.CommandText = queryString;
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        connection.Open();
                        SqlDataReader reader = sqlCommand.ExecuteReader();
                        await reader.ReadAsync();
                        if (reader != null && reader.HasRows)
                        {
                            response = new DeliveryRequestViewModel()
                            {
                                JobName = reader[0].ToString(),
                                JobAddress = reader[1].ToString(),
                                JobCity = reader[8].ToString(),
                                SupplierCompanyId = Convert.ToInt32(reader[3]),
                                CustomerCompany = reader[4].ToString(),
                                CreatedByCompanyId = Convert.ToInt32(reader[3]),
                                UoM = Convert.ToInt32(reader[6]),
                                UpdatedBy = Convert.ToInt32(reader[7]),
                                CreatedBy = Convert.ToInt32(reader[7]),
                                JobTimeZoneOffset = GetOffsetForTimezones(Convert.ToString(reader[9]))
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        LogManager.Logger.WriteException("DeliveryRequestAccess", "GetAdditionalDeliveryRequestDetails", $"{ex.Message} jobId:{jobId}, SupplierCompanyId:{supplierCompanyId}, ProductTypeId:{productTypeId}", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestAccess", "GetAdditionalDeliveryRequestDetails", $"{ex.Message} jobId:{jobId}, SupplierCompanyId:{supplierCompanyId}, ProductTypeId:{productTypeId}", ex);
            }
            return response;
        }

        public TimeSpan GetOffsetForTimezones(string input)
        {
            try
            {
                DateTime dateTime = DateTime.Now;
                if (!string.IsNullOrWhiteSpace(input))
                {
                    TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(input);
                    return tzi.GetUtcOffset(dateTime);
                }
                else
                {
                    return TimeSpan.Zero;
                }
            }
            catch(Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestAccess", "GetOffsetForTimezones", $"{ex.Message}", ex);
            }
            return TimeSpan.Zero;
        }

        public bool AddNotificationEvent(int eventTypeId, int userId, string jsonMessage = "")
        {
            bool response = false;
            try
            {
                using (var connection = DatabaseConnection.GetSqlConnection())
                {
                    var queryString = $@"Insert Into Notifications(EventTypeId,EntityId,TriggeredBy,CreatedDate,JsonMessage) 
                        Values(" + eventTypeId + ",0," + userId + ",'" + DateTimeOffset.Now + "','" + jsonMessage + "')";
                    var sqlCmd = new SqlCommand(queryString, connection);
                    connection.Open();
                    sqlCmd.ExecuteNonQuery();
                    sqlCmd.Dispose();
                    response = true;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestAccess", "AddNotificationEvent", $"{ex.Message} EventTypeId:{eventTypeId}", ex);
            }
            return response;
        }
    }

    public class ExchangeAccess
    {
        public string GetAppSetting(string keyName)
        {
            string response = string.Empty;
            try
            {
                using (var connection = DatabaseConnection.GetSqlConnection())
                {
                    try
                    {
                        var queryString = $@"Select TOP 1 Value
                                        From MstAppSettings
                                        Where [Key] = '{keyName}'";

                        var sqlCommand = connection.CreateCommand();
                        sqlCommand.CommandText = queryString;
                        sqlCommand.CommandType = System.Data.CommandType.Text;
                        connection.Open();
                        var keyValue = sqlCommand.ExecuteScalar();
                        response = Convert.ToString(keyValue);
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        connection.Close();
                        LogManager.Logger.WriteException("DeliveryRequestAccess", "GetAppSetting", $"{ex.Message} key:{keyName}", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestAccess", "GetAppSetting", $"{ex.Message} key:{keyName}", ex);
            }
            return response;
        }

    }
}
