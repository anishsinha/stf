using System;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using TrueFill.SCIM2.Model;
using TrueFill.SCIM2Service;

namespace TrueFill.Scim2Service
{
    public partial class Scim2Connector : Scim2Controller
    {
       
        protected void Page_Init(object sender, EventArgs e)
        { 
            //Init_SourceDB();           
        }

        protected void Page_Load(object sender, EventArgs e)
        {
           // Logger.LogException("Page_Load <pr/>");
           ProcessScimRequest(HttpContext.Current);
        }

        [WebMethod(MessageName = "Groups")]
        public void Groups()
        {
           // Logger.LogException("Get Groups Method<pr/>");
            ProcessScimRequest(HttpContext.Current);
        }

        [WebMethod(MessageName = "Users")]
        public void Users()
        {
           // Logger.LogException("Get Users Method<pr/>");
           ProcessScimRequest(HttpContext.Current);
        }

    }
}