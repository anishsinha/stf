using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SiteFuel.Exchange.Domain
{
    public class PDIEnterpriseDomain : BaseDomain
    {
        public PDIEnterpriseDomain(BaseDomain domain)
           : base(domain)
        {
        }
        public PDIEnterpriseDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }
        public async Task<List<string>> ProcessDeliveryDetailsToPDI(PDIAPIRequestViewModel viewModel)
        {
            var errorInfo = new List<string>();
            using (var tracer = new Tracer("PDIEnterpriseDomain", "ProcessDeliveryDetailsToPDI"))
            {
                StringBuilder processMessage = new StringBuilder();
                try
                {
                    string pdiAPIKeysDetails = Context.DataContext.MstAppSettings.Where(t => t.Key == ApplicationConstants.KeyAppSettingPDIEnterpriseApiSettings).Select(t => t.Value).FirstOrDefault();
                    if (!string.IsNullOrEmpty(pdiAPIKeysDetails))
                    {
                        PDIAPIKeyDetails pdiKeys = JsonConvert.DeserializeObject<PDIAPIKeyDetails>(pdiAPIKeysDetails);
                        var invoiceDomain = new InvoiceBaseDomain(this);
                        PDIFuelOrder orderAddRequestModel = await invoiceDomain.GetFuelOrderAddInput(viewModel.InvoiceHeaderId);
                        if (orderAddRequestModel != null)
                        {
                            var invoiceHeader = Context.DataContext.Invoices.Where(t => t.InvoiceHeaderId == viewModel.InvoiceHeaderId && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active).Select(t => new { t.Version, t.InvoiceHeader.InvoiceNumberId }).FirstOrDefault();

                            if (invoiceHeader.Version > 1)
                            {
                                var minVersionId = Context.DataContext.InvoiceHeaderDetails.Where(t => t.InvoiceNumberId == invoiceHeader.InvoiceNumberId).Min(t => t.Version);
                                orderAddRequestModel.OrderNo = GetPDIOrderNoForUpdate(minVersionId, invoiceHeader.Version, invoiceHeader.InvoiceNumberId);
                            }
                            if (orderAddRequestModel != null)
                            {
                                XmlDocument xmlDoc;
                                GetFuelOrderXml(orderAddRequestModel, out xmlDoc);

                                XmlNode PDIFuelOrderXml = xmlDoc;
                                if (orderAddRequestModel.FuelDetails != null && orderAddRequestModel.FuelDetails.Any())
                                {
                                    PDIEnterpriseServiceDomain pDIEnterpriseServiceDomain = new PDIEnterpriseServiceDomain();
                                    FormatFuelDetailsNode(PDIFuelOrderXml);

                                    var exception = string.Empty;
                                    if (!string.IsNullOrEmpty(orderAddRequestModel.OrderNo))
                                    {
                                        PDIUpdateFuelOrderResponse pdiUpdateFuelOrderResponse = new PDIUpdateFuelOrderResponse();
                                        pdiUpdateFuelOrderResponse = pDIEnterpriseServiceDomain.UpdateFuelOrder(pdiKeys, PDIFuelOrderXml);

                                        exception = GetPDIException(pdiUpdateFuelOrderResponse.Order.PDIExceptionMessage);
                                        UpdatePDIDate(viewModel.InvoiceHeaderId, pdiUpdateFuelOrderResponse.Order.Result, pdiUpdateFuelOrderResponse.Order.OrderNo, exception);

                                    }
                                    else
                                    {
                                        PDIAddFuelOrderResponse pdiAddFuelOrderResponse = new PDIAddFuelOrderResponse();
                                        pdiAddFuelOrderResponse = pDIEnterpriseServiceDomain.AddFuelOrder(pdiKeys, PDIFuelOrderXml);

                                        exception = GetPDIException(pdiAddFuelOrderResponse.Order.PDIExceptionMessage);
                                        UpdatePDIDate(viewModel.InvoiceHeaderId, pdiAddFuelOrderResponse.Order.Result, pdiAddFuelOrderResponse.Order.OrderNo, exception);

                                    }
                                    if (!string.IsNullOrEmpty(exception))
                                    {
                                        throw new Exception(exception);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (!(ex is QueueMessageFatalException))
                        LogManager.Logger.WriteException("PDIEnterpriseDomain", "ProcessDeliveryDetailsToPDI", ex.Message, ex);
                    if (processMessage.Length == 0)
                    {
                        processMessage.Append(Constants.RequestError);
                        errorInfo.Add(processMessage.ToString());
                    }
                    throw new QueueMessageFatalException(errorInfo[0], errorInfo);
                }
                return errorInfo;
            }
        }

        private static void GetFuelOrderXml(PDIFuelOrder orderAddRequestModel, out XmlDocument xmlDoc)
        {
            PDIFuelOrders pDIFuelOrders = new PDIFuelOrders();
            pDIFuelOrders.PDIFuelOrder = orderAddRequestModel;
            xmlDoc = new XmlDocument();
            XmlSerializer xmlSerializer = new XmlSerializer(pDIFuelOrders.GetType());
            using (MemoryStream xmlStream = new MemoryStream())
            {
                xmlSerializer.Serialize(xmlStream, pDIFuelOrders);
                xmlStream.Position = 0;
                xmlDoc.Load(xmlStream);
            }
        }

        private void FormatFuelDetailsNode(XmlNode PDIFuelOrdersXml)
        {
            XmlNode fuelDetails = PDIFuelOrdersXml.SelectSingleNode("/PDIFuelOrders/PDIFuelOrder/FuelDetails");
            if (fuelDetails != null && fuelDetails.HasChildNodes)
            {
                var fuelDetailChilds = fuelDetails.ChildNodes;
                if (fuelDetailChilds != null && fuelDetailChilds.Count > 0)
                {
                    List<XmlNode> nodes = new List<XmlNode>();
                    foreach (XmlNode fuelDetail in fuelDetailChilds)
                    {
                        nodes.Add(fuelDetail);
                    }

                    nodes.ForEach(t => fuelDetails.ParentNode.AppendChild(t));
                }


                fuelDetails.RemoveAll();
                fuelDetails.ParentNode.RemoveChild(fuelDetails);
            }
        }

        private static void FormatLoadDetailsNode(XmlNode fuelDetail)
        {
            XmlNode loadDetails = fuelDetail.SelectSingleNode("LoadDetails");
            if (loadDetails != null && loadDetails.HasChildNodes)
            {
                var loadChildCount = loadDetails.ChildNodes.Count;
                for (int j = 0; j < loadChildCount; j++)
                {
                    XmlNode loadDetail = loadDetails.ChildNodes[j];
                    loadDetails.ParentNode.AppendChild(loadDetail);
                }
                loadDetails.RemoveAll();
                loadDetails.ParentNode.RemoveChild(loadDetails);
            }
            else if (loadDetails != null)
            {
                loadDetails.ParentNode.RemoveChild(loadDetails);
            }
        }

        /// <summary>
        /// Below commented code is for multiple load detail nodes
        /// </summary>
        /// <param name="minVersionId"></param>
        /// <param name="currentVersionId"></param>
        /// <param name="invoiceNumberId"></param>
        /// <returns></returns>
        /// 
        //private void FormatFuelDetailsNode(XmlNode PDIFuelOrdersXml)
        //{
        //    XmlNode fuelDetails = PDIFuelOrdersXml.SelectSingleNode("/PDIFuelOrders/PDIFuelOrder/FuelDetails");
        //    if (fuelDetails != null && fuelDetails.HasChildNodes)
        //    {
        //        var fuelDetailChilds = fuelDetails.ChildNodes;
        //        if (fuelDetailChilds != null && fuelDetailChilds.Count > 0)
        //        {
        //            List<XmlNode> fuelDetailNodes = new List<XmlNode>();

        //            foreach (XmlNode fuelDetail in fuelDetailChilds)
        //            {
        //                FormatLoadDetailsNode(fuelDetail);
        //                fuelDetailNodes.Add(fuelDetail);
        //            }

        //            fuelDetailNodes.ForEach(t => fuelDetails.ParentNode.AppendChild(t));
        //        }

        //        fuelDetails.RemoveAll();
        //        fuelDetails.ParentNode.RemoveChild(fuelDetails);
        //    }
        //}

        //private static void FormatLoadDetailsNode(XmlNode fuelDetail)
        //{
        //    List<XmlNode> loadDetailNodes = new List<XmlNode>();

        //    XmlNode loadDetails = fuelDetail.SelectSingleNode("LoadDetails");
        //    if (loadDetails != null && loadDetails.HasChildNodes)
        //    {
        //        var loadDetailChilds = loadDetails.ChildNodes;
        //        if (loadDetailChilds != null && loadDetailChilds.Count > 0)
        //        {
        //            foreach (XmlNode load in loadDetails)
        //            {
        //                loadDetailNodes.Add(load);
        //            }
        //            loadDetailNodes.ForEach(t => fuelDetail.AppendChild(t));
        //        }
        //    }
        //    loadDetails.RemoveAll();
        //    loadDetails.ParentNode.RemoveChild(loadDetails);
        //}        

        public string GetPDIOrderNoForUpdate(int minVersionId, int currentVersionId, int invoiceNumberId)
        {
            string pdiOrderNo = null;
            var originalInvoiceDetails = Context.DataContext.Invoices.FirstOrDefault(t => t.InvoiceHeader.InvoiceNumberId == invoiceNumberId && t.Version == (currentVersionId - 1));

            if (originalInvoiceDetails != null)
            {
                if (originalInvoiceDetails.Id > 0 && string.IsNullOrEmpty(originalInvoiceDetails.InvoiceXAdditionalDetail?.PDIDeliveryOrderNo) && originalInvoiceDetails.Version > minVersionId)
                {
                    pdiOrderNo = GetPDIOrderNoForUpdate(minVersionId, originalInvoiceDetails.Version, originalInvoiceDetails.InvoiceHeader.InvoiceNumberId);
                }
                else
                {
                    pdiOrderNo = originalInvoiceDetails.InvoiceXAdditionalDetail.PDIDeliveryOrderNo;
                }
            }

            return pdiOrderNo;
        }
        public StatusViewModel UpdatePDIDate(int InvoiceHeaderId, string result, string OrderNo , string pDIExceptionMessage)
        {
            var response = new StatusViewModel();
            try
            {
                var jobTimeZoneName = Context.DataContext.Invoices.Where(t => t.InvoiceHeaderId == InvoiceHeaderId).Select(t => t.Order.FuelRequest.Job.TimeZoneName).FirstOrDefault();
                var jobLocationTime = DateTimeOffset.Now.ToTargetDateTimeOffset(jobTimeZoneName);
                Context.DataContext.Invoices.Where(t => t.InvoiceHeaderId == InvoiceHeaderId).ToList().ForEach(t =>
                {
                    LogManager.Logger.WriteDebug("PDIEnterpriseDomain", "UpdateInvoicePONumber", string.Format("Delivery Details sent for ddt number {0} on {1} ", t.DisplayInvoiceNumber, DateTime.UtcNow));
                    if (!string.IsNullOrWhiteSpace(OrderNo))
                    {
                        t.InvoiceXAdditionalDetail.DeliverySentToPDIOn = jobLocationTime;
                        t.InvoiceXAdditionalDetail.PDIDeliveryOrderNo = OrderNo;
                    }

                    t.InvoiceXAdditionalDetail.PDIDeliveryDetailsStatus = result;
                    
                    if (string.IsNullOrWhiteSpace(OrderNo) && !string.IsNullOrWhiteSpace(pDIExceptionMessage))
                        t.InvoiceXAdditionalDetail.ExceptionMessage = pDIExceptionMessage;
                    else
                        t.InvoiceXAdditionalDetail.ExceptionMessage = null;
                });
                Context.Commit();
                response.StatusCode = Status.Success;
            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                LogManager.Logger.WriteException("PDIEnterpriseDomain", "UpdatePDIDate", ex.Message, ex);
            }
            return response;
        }

        private static string GetPDIException(List<PDIExceptionMessage> pDIExceptionMessage)
        {
            string exception = "";
            if (pDIExceptionMessage != null && pDIExceptionMessage.Any())
            {
                pDIExceptionMessage.ForEach(t => exception += (string.IsNullOrEmpty(exception) ? t.Description : ", " + t.Description));
            }
            return exception;
        }
    }
}
