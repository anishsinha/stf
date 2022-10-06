using System;
using TrueFill.SCIM2.Model;

namespace TrueFill.Scim2Service
{
    public partial class LogView : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Logger.LogException("Page_Load<hr/>");
            llog.Text = Logger.GetLog();
        }

        protected void BClear_Click(object sender, EventArgs e)
        {
            Logger.ClearLog();
            llog.Text = "";
        }
    }
}