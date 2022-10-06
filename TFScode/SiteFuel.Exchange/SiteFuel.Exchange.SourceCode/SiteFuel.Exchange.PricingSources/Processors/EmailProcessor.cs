using SiteFuel.Exchange.EmailManager;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.PricingSources.DataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.PricingSources.Processors
{
    public class EmailProcessor
    {
        public static void ProcessEmail(string message)
        {
            var recepients = GetRecepientsIfEnable();
            if (recepients != null)
            {
                SendFailedEmail(recepients, message);
            }
        }

        private static void SendFailedEmail(string recepients, string message)
        {
            try
            {
                var from = ConfigurationManager.AppSettings.Get("EmailFromAddress");
                var subject = ConfigurationManager.AppSettings.Get("EmailSubject");
                var toEmails = recepients.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                if (toEmails.Any())
                {
                    Email email = new Email();
                    email.Send(from, toEmails, subject, message);
                }
                LogManager.Logger.WriteDebug("PricingSources", "SendFailedEmail", "Success");
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingSources", "SendFailedEmail", "Failed :" + ex.Message, ex);
                LogManager.Logger.WriteDebug("PricingSources", "SendFailedEmail", ex.Message);
            }
        }

        private static string GetRecepientsIfEnable()
        {
            var response = string.Empty;
            try
            {
                using (var connection = DatabaseConnection.GetSqlConnection())
                {
                    var queryString = "Select IsEnable,Recepient From Monitors where Name = 'SFX-Pricing'";
                    var sqlCommand = connection.CreateCommand();
                    sqlCommand.CommandText = queryString;
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    connection.Open();
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    if (reader.Read())
                    {
                        if (reader.GetBoolean(0)) // if isEnable = true
                        {
                            response = reader.GetString(1);
                        }
                    }
                    reader.Close();
                }
                LogManager.Logger.WriteDebug("PricingSources", "GetRecepientsIfEnable", "Succcess");
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingSources", "GetRecepientsIfEnable", "Failed :" + ex.Message, ex);
                LogManager.Logger.WriteDebug("PricingSources", "GetRecepientsIfEnable", ex.Message);
            }
            return response;
        }
    }
}
