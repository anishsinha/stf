using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Logger;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using TrueFill.SCIM2;
using TrueFill.SCIM2.Model;
using TrueFill.SCIM2.Model.Authentication;

namespace TrueFill.SCIM2Service
{
    public partial class Scim2Controller : Page
    {
        private readonly IScimDomain _scimDomain;

        public Scim2Controller()
        {
            ContextFactory.Register(new ApplicationContext());
            _scimDomain = new ScimDomain();

        }

        public Scim2Controller(IScimDomain scimDomain, IContext appContext)
        {
            ContextFactory.Register(appContext);
            _scimDomain = scimDomain;

        }

        //private SCIMUser currentDataContext = null;

        //public SCIMUser DataContext { get {return currentDataContext;} }

        public void ProcessScimRequest(HttpContext currentContext)
        {
            //HttpContext currentContext = HttpContext.Current;
            //LogException(string.Format("<p>ProcessScimRequest:</p><hr /> <p> Requested Httmp Method: {0} </p><hr /> <p>Requested raw URL: {1}</p><hr />", currentContext.Request.HttpMethod, currentContext.Request.RawUrl));

            try
            {
                BasicAuth authentication = _scimDomain.GetBasicAuth();
                TrueFill.SCIM2.SCIM2 scim2 = new TrueFill.SCIM2.SCIM2(currentContext.Request, currentContext.Response);
                scim2.Authenticate(authentication);
                scim2.NewUser += NewUser;
                scim2.GetUsers += GetUsers;
                scim2.GetUsersFilter += GetUsersFilter;
                scim2.GetGroups += Scim2_GetGroups;
                scim2.Process();
            }
            catch (Exception ex)
            {
                //Ok, admit this is the worst logger ever. Don't use for production!!
                //LogException(string.Format("ProcessScimRequest Exception: <p>{0}</p><hr />", ex.Message));
                LogManager.Logger.WriteException("Scim2Controller", "ProcessScimRequest", ex.Message, ex);
            }

        }

        private Result Scim2_GetGroups(int startIndex, int count)
        {
            throw new NotImplementedException();
        }

        public void NewUser(JsonDocument jsonData)
        {
            // var username = jsonData.RootElement.GetProperty("userName").ToString();
            //LogException(String.Format("<p>NewUser Start: {0}</p><hr /><p> for User Name: {1}</p><hr />", jsonData, username));
            try
            {
                _scimDomain.AddNewUser(jsonData);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("Scim2Controller", "NewUser", ex.Message, ex);
            }
            //LogException(string.Format("<p>AddUser: {0}</p><hr />", username));
        }

        public Result GetUsers(int startIndex, int count)
        {
            //SaveDebugInfo(String.Format("<p>GetUsers Start Index: {0}</p><hr /><p> Count: {1}</p><hr />", startIndex, count));
            Result result = new Result(SchemasHelper.Schema.ListResponse);
            try
            {   
                List<SCIMUser> users = _scimDomain.GetUsers(companyId: 1293, 1);
                result.Resources = users.ToArray();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("Scim2Controller", "GetUsers", ex.Message, ex);
            }
            return result;

        }

        public Result GetUsersFilter(Filter filter, int startIndex, int count)
        {
            //LogException(String.Format("<p>GetUsers Start Index: {0}</p><hr/><p> Count: {1}</p><hr/> <p> Filter: {2}</p><hr/>", startIndex, count,
            //    JsonSerializer.Serialize<Filter>(filter)));


            Result result = new Result(SchemasHelper.Schema.ListResponse);
            try
            {   
                if (filter.Operator == "eq")
                {
                    List<SCIMUser> users = GetFilteredUsers(filter);
                    result.Resources = users.ToArray();
                }
              
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("Scim2Controller", "GetUsersFilter", ex.Message, ex);
            }
            return result;

        }

        //private void LogException(string message)
        //{
        //    //Logger.LogException(message);
        //    LogManager.Logger.WriteException("Scim2Controller", "LogException", message,new Exception());
        //}

        #region Private CRUD operations for Users

        #region Temp test DB
       // public void Init_SourceDB()
      //  {
            //if (currentDataContext == null)
            //{
            //    DataTable db = new DataTable("worstdbever");
            //    db.Columns.Add("Id", typeof(int));
            //    db.Columns.Add("Username");
            //    db.Columns.Add("Smurf");

            //    db.Rows.Add(1, "rajesh@truefill.com", "red");
            //    db.Rows.Add(2, "anish@truefill.com", "blue");

            //    currentDataContext = db;

            //}
       // }

        #endregion

        //private void AddNewUser(JsonDocument jsonData)
        //{
        //    //DataTable db = currentDataContext;
        //    //DataRow dr = db.NewRow();
        //    //dr["id"] = db.Rows.Count + 1;
        //    //dr["userName"] = username;
        //    //dr["smurf"] = "";
        //    //db.Rows.Add(dr);
        //    //currentDataContext = db;

        //    _scimDomain.AddNewUser(jsonData);



        //}

        private List<SCIMUser> GetFilteredUsers(Filter filter)
        {
            //DataRow[] drs = currentDataContext.Select(string.Format("{0} = '{1}'", filter.Value1, filter.Value2));
            //List<SCIMUser> users = new List<SCIMUser>();
            //foreach (DataRow r in drs) users.Add(new SCIMUser(r));
            //return users;
            List<SCIMUser> users = new List<SCIMUser>();
            try
            {
               
                var user = _scimDomain.GetUserByEmail(filter.Value2);
                users.Add(new SCIMUser(user)); //filter.value1 , filter.Value2 is userName, email id respectively
                
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("Scim2Controller", "GetFilteredUsers", ex.Message, ex);
            }
            return users;

        }

        #endregion

    }
}