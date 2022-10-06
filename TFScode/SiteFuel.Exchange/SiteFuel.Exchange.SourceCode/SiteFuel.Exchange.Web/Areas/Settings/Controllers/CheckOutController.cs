using SiteFuel.Exchange.Core.Utilities;
using SiteFuel.Exchange.ViewModels.cXML;
using System;
using System.Text;
using System.Web.Mvc;
using System.Xml;

namespace SiteFuel.Exchange.Web.Areas.Settings.Controllers
{
    /// <summary>
    /// Create the Checkout Controller for displaying the xml data to form.
    /// </summary>
    [AllowAnonymous]
    public class CheckOutController : Controller
    {
        // POST: Settings/CheckOut - validate the xml data. display xml data into index view.
        [HttpPost]
        public ActionResult ValidateCxml()
        {
            OrderMessage orderMessage = new OrderMessage();

            if (Request.Params != null && Request.Params.Count > 0 && Request.Params["cxml-base64"] != null)
            {
                if (!string.IsNullOrEmpty(Request.Params["cxml-base64"].ToString()))
                {
                    string cxml_base64 = Request.Params["cxml-base64"].ToString();
                    string cxmlbase64 = Encoding.UTF8.GetString(Convert.FromBase64String(cxml_base64));
                    DeserializeMessage(cxmlbase64, ref orderMessage);
                }
            }
            return View("Index", orderMessage);
        }
        /// <summary>
        /// deserialize the xml message and return the ordermessage model.
        /// </summary>
        /// <param name="xmlData"></param>
        /// <param name="orderMessage"></param>
        /// <returns></returns>
        internal OrderMessage DeserializeMessage(string xmlData, ref OrderMessage orderMessage)
        {

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlData.Substring(xmlData.IndexOf(Environment.NewLine)));
            Logger.LogManager.Logger.WriteInfo("DeSerializemessage", "CheckOutController", "Valid_XML=>" + xmlData);
            orderMessage = XmlSerialization.Deserialize<OrderMessage>(xmlDocument.InnerXml.ToString());
            return orderMessage;
        }

    }





}