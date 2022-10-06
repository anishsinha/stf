using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using TrueFill.SCIM2.Model;
using TrueFill.SCIM2.Model.Authentication;
using static TrueFill.SCIM2.Authentication;

namespace TrueFill.SCIM2
{
    public class SCIM2
    {
        //https://tools.ietf.org/html/rfc7644
        //https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to
        public enum HttpMethod { GET, POST, DELETE, PATCH, PUT }

        private readonly HttpRequest _httpRequest;
        private readonly HttpResponse _httpResponse;
        private Authentication authenticate;
        private bool Authenticated;
        public Authentication CurrentAuthentication { get { return authenticate; } }

        #region Delegates
        public delegate Result getUser(object id);
        public delegate Result getUsers(int startIndex, int count);
        public delegate Result getUsersFilter(Filter filter, int startIndex, int count);
        public delegate void newUser(JsonDocument json);

        public delegate Result getGroup(object id);
        public delegate Result getGroups(int startIndex, int count);

        public event getUser GetUser;
        public event getUsers GetUsers;
        public event getUsersFilter GetUsersFilter;
        public event newUser NewUser;

        public event getGroup GetGroup;
        public event getGroups GetGroups;
        #endregion

        #region Constructor
        public SCIM2(HttpRequest httpRequest, HttpResponse httpResponse)
        {
            //LogException(string.Format("Constructor (Request,response): {0}{1}<hr/>", httpRequest, httpResponse));
            _httpRequest = httpRequest;
            _httpResponse = httpResponse;
            Authenticated = false;
        }
        #endregion

        #region Public Method
        public void Authenticate(IAuthenticationModes authenticationMode)
        {
            //LogException(string.Format("Authenitcate: {0}<hr/>",authenticationMode));

            authenticate = new Authentication(_httpRequest, authenticationMode);
            Authenticated = true;
        }

        public void Process()
        {
            if (!Authenticated)
            {
                //LogException(string.Format("Process - Authenticated: {0}<hr/>", Authenticated));
                throw new HttpException(401, "not authenticated");
            }
            if (!Enum.TryParse(_httpRequest.HttpMethod, out HttpMethod httpMethod))
            {
                //LogException("Process - PartseHttmpMethod: Unknown Parse Method<hr/>");

                throw new Exception("unknown HTTP Method");
            }

            string body = string.Empty;
            JsonDocument json = null;
            if (httpMethod == HttpMethod.POST)
            {
                //LogException("Process - Post Http Mehtod request<hr/>");

                _httpRequest.InputStream.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(_httpRequest.InputStream);
                body = reader.ReadToEnd();
                json = JsonDocument.Parse(body);
                //LogException(string.Format("Process - Input stream: {0} <hr/>", json));

            }
            string endPoint = _httpRequest.PathInfo;
            //LogException(string.Format("Process - endPoint: {0} <hr/>", endPoint));

            Result result = null;

            switch (endPoint.ToUpper())
            {
                case "/USERS":
                    switch (httpMethod)
                    {
                        case HttpMethod.GET:
                            //LogException("Process - endPoint: Users - Get Http method <hr/>");

                            int startIndex = IQString(_httpRequest, "startIndex");
                            int count = IQString(_httpRequest, "count");
                            string sfilter = QString(_httpRequest, "filter");

                            //LogException(string.Format("Process - <p>StartIndex: {0} </p><hr/><p>Count: {1} </p><hr/><p>Filter: {2} </p><hr/>", startIndex, count, sfilter));


                            if (sfilter == string.Empty)
                            {
                                if (GetUsers != null) result = GetUsers(startIndex, count);
                            }
                            else
                            {
                                Filter filter = new Filter(sfilter);
                                if (GetUsersFilter != null) result = GetUsersFilter(filter, startIndex, count);

                            }
                            break;
                        case HttpMethod.POST:
                            //LogException(string.Format("Process-Isers-PostHttpMethod - <p>NewUser: {0} </p><hr/>", json));
                            NewUser?.Invoke(json);
                            break;
                    }
                    break;
                case "/GROUPS":
                    switch (httpMethod)
                    {
                        case HttpMethod.GET:
                            //LogException("Process - endPoint: Groups - Get Http method <hr/>");

                            int startIndex = IQString(_httpRequest, "startIndex");
                            int count = IQString(_httpRequest, "count");

                            //LogException(string.Format("Process-Groups-GetHttpMethod - <p>StartIndex: {0} </p><hr/><p>Count: {1} </p><hr/>", startIndex, count));

                            if (GetUsers != null) result = GetGroups(startIndex, count);
                            break;
                    }
                    break;

            }

            if (result != null)
            {
                string resultjson = JsonSerializer.Serialize<Result>(result);
                //LogException(string.Format("Process - Response - return Result - <p>{0} </p><hr/>", resultjson));
                _httpResponse.Clear();
                _httpResponse.ContentType = "application/json";
                _httpResponse.Write(resultjson);
                _httpResponse.End();
            }else
            {
                ThrowExceptionAsRequestNotProcessed(body);
            }

        }

        public Result ProcessForController()
        {
            if (!Authenticated) throw new HttpException(401, "not authenticated");
            if (!Enum.TryParse(_httpRequest.HttpMethod, out HttpMethod httpMethod)) throw new Exception("unknown HTTP Method");
            string body = string.Empty;
            JsonDocument json = null;
            if (httpMethod == HttpMethod.POST)
            {
                _httpRequest.InputStream.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(_httpRequest.InputStream);
                body = reader.ReadToEnd();
                json = JsonDocument.Parse(body);
            }
            string endPoint = _httpRequest.PathInfo;

            Result result = null;

            switch (endPoint.ToUpper())
            {
                case "/USERS":
                    switch (httpMethod)
                    {
                        case HttpMethod.GET:
                            int startIndex = IQString(_httpRequest, "startIndex");
                            int count = IQString(_httpRequest, "count");
                            string sfilter = QString(_httpRequest, "filter");
                            if (sfilter == string.Empty)
                            {
                                if (GetUsers != null) result = GetUsers(startIndex, count);
                            }
                            else
                            {
                                Filter filter = new Filter(sfilter);
                                if (GetUsersFilter != null) result = GetUsersFilter(filter, startIndex, count);

                            }
                            break;
                        case HttpMethod.POST:
                            NewUser?.Invoke(json);
                            break;
                    }
                    break;
                case "/GROUPS":
                    switch (httpMethod)
                    {
                        case HttpMethod.GET:
                            int startIndex = IQString(_httpRequest, "startIndex");
                            int count = IQString(_httpRequest, "count");
                            if (GetUsers != null) result = GetGroups(startIndex, count);
                            break;
                    }
                    break;
            }
            return result;
        }

        #endregion

        #region Private Method
        private void ThrowExceptionAsRequestNotProcessed(string body)
        {
            String errorMsg = string.Format("{0} {1} {2}", _httpRequest.RawUrl, authenticate.GetAuthenticationMode, body);
            //LogException(string.Format("Error Being Report as Process Response:{0}", errorMsg));
            throw new Exception(errorMsg);
        }

     

        private int IQString(HttpRequest httpRequest, string key, int defaultValue = 1)
        {
            int retval = defaultValue;
            if (httpRequest.QueryString[key] != null)
            {
                string qString = httpRequest.QueryString[key];
                if (int.TryParse(qString, out int ivalue))
                {
                    retval = ivalue;
                }                
               // if (!int.TryParse(httpRequest.QueryString[key], out int ivalue)) retval = ivalue;
            }
            return retval;
        }
        private string QString(HttpRequest httpRequest, string key, string defaultValue = "")
        {
            return (httpRequest.QueryString[key] ?? defaultValue);
        }

        private static void LogException(string message)
        {
            Logger.LogException(message);
        }
        #endregion


    }
}
