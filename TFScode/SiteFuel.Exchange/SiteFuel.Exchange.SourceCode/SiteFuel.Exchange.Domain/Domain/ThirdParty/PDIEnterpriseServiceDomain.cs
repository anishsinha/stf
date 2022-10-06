using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Domain.PDIEnterprise;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SiteFuel.Exchange.Domain
{
    public class PDIEnterpriseServiceDomain : BaseDomain
    {
        PDIEnterpriseWebSoapClient api;
        public PDIEnterpriseServiceDomain() : base(ContextFactory.Current.ConnectionString)
        {
            api = new PDIEnterpriseWebSoapClient();
            string endpointAddress = string.Format(ConfigurationManager.AppSettings[ApplicationConstants.KeyAppSettingPDIEnterpriseServiceAddress]);
            api.Endpoint.Address = new EndpointAddress(new Uri(endpointAddress));
        }

        public PDIEnterpriseServiceDomain(BaseDomain domain) : base(domain)
        {
        }


        public PDIAddFuelOrderResponse AddFuelOrder(PDIAPIKeyDetails pdiAPIKeyDetails, XmlNode orderAddRequestModel)
        {
            string response = "";
            PDIAddFuelOrderResponse pdiAddFuelOrderResponse = new PDIAddFuelOrderResponse();
            DateTime startTime = new DateTime();
            DateTime endTime = new DateTime();
            try
            {
                UserCredentials userCredentials = new UserCredentials();
                userCredentials.PartnerID = pdiAPIKeyDetails.Partner;
                userCredentials.Password = pdiAPIKeyDetails.Password;
                startTime = DateTime.UtcNow;
                response = api.AddFuelOrder(userCredentials, (int)PDIOrderStatus.ReleasedForDispatch, orderAddRequestModel);
                pdiAddFuelOrderResponse = GetPDIAddFuelResponse(response);
            }
            catch (Exception)
            {

                throw;
            }

            finally
            {
                string requestJson = orderAddRequestModel.InnerXml;
                string responseJson = response;
                endTime = DateTime.UtcNow;
                PDIServiceLogs(requestJson, responseJson, "AddFuelOrder", startTime, endTime);
            }
            return pdiAddFuelOrderResponse;
        }



        public PDIUpdateFuelOrderResponse UpdateFuelOrder(PDIAPIKeyDetails pDIAPIKeyDetails, XmlNode orderAddRequestModel)
        {
            string response = "";
            PDIUpdateFuelOrderResponse pdiUpdateFuelOrderResponse = new PDIUpdateFuelOrderResponse();
            DateTime startTime = new DateTime();
            DateTime endTime = new DateTime();
            try
            {
                UserCredentials userCredentials = new UserCredentials();
                userCredentials.PartnerID = pDIAPIKeyDetails.Partner;
                userCredentials.Password = pDIAPIKeyDetails.Password;
                startTime = DateTime.UtcNow;
                response = api.UpdateFuelOrder(userCredentials, (int)PDIOrderStatus.ReleasedForDispatch, orderAddRequestModel);
                pdiUpdateFuelOrderResponse = GetPDIUpdateFuelResponse(response);
            }
            catch (Exception)
            {

                throw;
            }

            finally
            {
                string requestJson = orderAddRequestModel.InnerXml;
                string responseJson = response;
                endTime = DateTime.UtcNow;
                PDIServiceLogs(requestJson, responseJson, "UpdateFuelOrder", startTime, endTime);
            }
            return pdiUpdateFuelOrderResponse;
        }

        public PDIAddFuelOrderResponse GetPDIAddFuelResponse(string response)
        {

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(PDIAddFuelOrderResponse));
            MemoryStream xmlStream = new MemoryStream(Encoding.UTF8.GetBytes(response));
            xmlStream.Seek(0, SeekOrigin.Begin);
            var pdiAddFuelOrderResponse = (PDIAddFuelOrderResponse)xmlSerializer.Deserialize(xmlStream);
            if(pdiAddFuelOrderResponse != null && pdiAddFuelOrderResponse.Order != null 
                && pdiAddFuelOrderResponse.Order.Result == Convert.ToString((int)PDIApiResponse.RejectedWithErrors))
            {
                pdiAddFuelOrderResponse.Order.PDIExceptionMessage = new List<PDIExceptionMessage>();
                pdiAddFuelOrderResponse.Order.PDIExceptionMessage = GetPDIExceptionMessages(response);             
            }         
            return pdiAddFuelOrderResponse;
        }
        public PDIUpdateFuelOrderResponse GetPDIUpdateFuelResponse(string response)
        {

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(PDIUpdateFuelOrderResponse));
            MemoryStream xmlStream = new MemoryStream(Encoding.UTF8.GetBytes(response));
            xmlStream.Seek(0, SeekOrigin.Begin);
            var pdiUpdateFuelOrderResponse = (PDIUpdateFuelOrderResponse)xmlSerializer.Deserialize(xmlStream);
            pdiUpdateFuelOrderResponse.Order.PDIExceptionMessage = new List<PDIExceptionMessage>();
            pdiUpdateFuelOrderResponse.Order.PDIExceptionMessage = GetPDIExceptionMessages(response);        
            return pdiUpdateFuelOrderResponse;
        }

        public List<PDIExceptionMessage> GetPDIExceptionMessages(string response)
        {
            List<PDIExceptionMessage> exceptionMessageresponse = new List<PDIExceptionMessage>();
            XmlReader rdr = XmlReader.Create(new System.IO.StringReader(response));
            while (rdr.Read())
            {
                    if (rdr.Name == "PDIExceptionMessage" && rdr.IsStartElement("PDIExceptionMessage"))
                    {
                        PDIExceptionMessage pDIExceptionMessage = new PDIExceptionMessage();
                        XmlSerializer xmlException = new XmlSerializer(typeof(PDIExceptionMessage));
                        MemoryStream streamException = new MemoryStream(Encoding.UTF8.GetBytes(rdr.ReadOuterXml()));
                        streamException.Seek(0, SeekOrigin.Begin);
                        pDIExceptionMessage = (PDIExceptionMessage)xmlException.Deserialize(streamException);
                        if (pDIExceptionMessage != null && !string.IsNullOrEmpty(pDIExceptionMessage.Description))
                        {
                            exceptionMessageresponse.Add(pDIExceptionMessage);
                        }                      

                    }
                
            }
            return exceptionMessageresponse;
        }

        public void PDIServiceLogs(string requestJson, string responseJson, string actionName, DateTime startTime, DateTime endTime)
        {
            try
            {
                double TotalMilliseconds = (endTime - startTime).TotalMilliseconds;
                LogManager.Logger.WriteAPIInfo("", "PDIEnterpriseServiceDomain", actionName, requestJson, responseJson, TotalMilliseconds, "", startTime, endTime);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PDIEnterpriseServiceDomain", "PDIServiceLogs", ex.Message, ex);
            }

        }
    }

}
