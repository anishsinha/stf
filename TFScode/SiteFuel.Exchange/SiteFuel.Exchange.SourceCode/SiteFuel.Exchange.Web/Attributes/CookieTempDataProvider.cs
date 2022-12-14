using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace SiteFuel.Exchange.Web.Attributes
{
    public class CookieTempDataProvider : ITempDataProvider
    {
        const string CookieName = "SFX_TempData";

        public void SaveTempData(ControllerContext controllerContext, IDictionary<string, object> values)
        {
            // convert the temp data dictionary into json
            string value = Serialize(values);
            //// compress the json (it really helps)
            // issue the cookie
            IssueCookie(controllerContext, value);
        }

        public IDictionary<string, object> LoadTempData(ControllerContext controllerContext)
        {
            // get the cookie
            var value = GetCookieValue(controllerContext);
            //// verify and decrypt the value via the asp.net machine key
            // convert the json back to a dictionary
            return Deserialize(value);
        }
        string GetCookieValue(ControllerContext controllerContext)
        {
            HttpCookie c = controllerContext.HttpContext.Request.Cookies[CookieName];
            if (c != null)
            {
                var value = c.Value;
                if (value != null)
                    value = value.Replace("%3B", ";");
                return value;
            }
            return null;
        }

        void IssueCookie(ControllerContext controllerContext, string value)
        {
            if (value != null)
                value = value.Replace(";", "%3B");
            HttpCookie c = new HttpCookie(CookieName, value)
            {
                // don't allow javascript access to the cookie
                HttpOnly = true,
                // set the path so other apps on the same server don't see the cookie
                Path = controllerContext.HttpContext.Request.ApplicationPath,
                // ideally we're always going over SSL, but be flexible for non-SSL apps
                Secure = controllerContext.HttpContext.Request.IsSecureConnection
            };

            if (value == null)
            {
                // if we have no data then issue an expired cookie to clear the cookie
                c.Expires = DateTime.Now.AddMonths(-1);
            }

            if (value != null || controllerContext.HttpContext.Request.Cookies[CookieName] != null)
            {
                // if we have data, then issue the cookie
                // also, if the request has a cookie then we need to issue the cookie
                // which might act as a means to clear the cookie 
                controllerContext.HttpContext.Response.Cookies.Add(c);
            }
        }

        byte[] Compress(string value)
        {
            if (value == null) return null;

            var data = Encoding.UTF8.GetBytes(value);
            using (var input = new MemoryStream(data))
            {
                using (var output = new MemoryStream())
                {
                    using (Stream cs = new DeflateStream(output, CompressionMode.Compress))
                    {
                        input.CopyTo(cs);
                    }

                    return output.ToArray();
                }
            }
        }

        string Decompress(byte[] data)
        {
            if (data == null || data.Length == 0) return null;

            using (var input = new MemoryStream(data))
            {
                using (var output = new MemoryStream())
                {
                    using (Stream cs = new DeflateStream(input, CompressionMode.Decompress))
                    {
                        cs.CopyTo(output);
                    }

                    var result = output.ToArray();
                    return Encoding.UTF8.GetString(result);
                }
            }
        }

        string Serialize(IDictionary<string, object> data)
        {
            if (data == null || data.Keys.Count == 0) return null;

            JavaScriptSerializer ser = new JavaScriptSerializer();
            return ser.Serialize(data);
        }

        IDictionary<string, object> Deserialize(string data)
        {
            if (String.IsNullOrWhiteSpace(data)) return null;

            JavaScriptSerializer ser = new JavaScriptSerializer();
            return ser.Deserialize<IDictionary<string, object>>(data);
        }
    }
}