using DiligenteSCIM2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Web;

namespace TrueFill.IdentityProviderService
{
    /// <summary>
    /// Summary description for OktaScim
    /// </summary>
    public class OktaScim : IHttpHandler
    {
        HttpContext _context = null;
        public void ProcessRequest(HttpContext context)
        {
            //context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            _context = context;
            //// Authentication.HTTPHeader httpHeader = new Authentication.HTTPHeader() { Token="secrettoken"};
            Authentication.BasicAuth authentication = new Authentication.BasicAuth() { Username = "username", Password = "password" };
            SCIM2 scim2 = new SCIM2(context.Request, context.Response);
            scim2.Authenticate(authentication);
            scim2.NewUser += Scim2_NewUser;
            scim2.GetUser += Scim2_GetUser;
            scim2.GetUsers += Scim2_GetUsers;
            scim2.GetUserKey += Scim2_GetUserKey;
            scim2.DelUser += Scim2_DelUser;
            scim2.UpdateUser += Scim2_UpdateUser;
            
            scim2.Log += Scim2_Log;

            scim2.Process();
        }

        private void Scim2_Log(string log)
        {
            _context.Response.Write("Scim2_Log " + log);
        }

        private string Scim2_UpdateUser(object id, JsonDocument json)
        {
            _context.Response.Write("Scim2_UpdateUser ");
            return "";
        }

        private void Scim2_DelUser(object id)
        {
            int Id;
            int.TryParse(id.ToString(), out Id);
            _context.Response.Write("Scim2_DelUser " + Id);
        }

        private object Scim2_GetUserKey(string key)
        {
            _context.Response.Write("Scim2_GetUserKey " + key);
            return null;
        }

        private Results Scim2_GetUsers(int startIndex, int count)
        {
            _context.Response.Write("Scim2_GetUsers " + startIndex + " : " + startIndex);
            return null;
        }

        private object Scim2_GetUser(object id)
        {
            int Id;
            int.TryParse(id.ToString(), out Id);
            _context.Response.Write("Scim2_GetUser " + Id);
            return null;
        }

        private string Scim2_NewUser(JsonDocument json)
        {
            _context.Response.Write("Scim2_NewUser " + json.ToString());
            return "";
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}