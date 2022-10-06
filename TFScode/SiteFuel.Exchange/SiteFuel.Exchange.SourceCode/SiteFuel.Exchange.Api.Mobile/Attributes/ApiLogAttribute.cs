using Newtonsoft.Json;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace SiteFuel.Exchange.Api.Mobile.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class ApiLogAttribute : ActionFilterAttribute
    {
        public bool Enabled { get; set; }
        public bool TPDLogEnabled { get; set; }
        public static List<string> imagePropertyName = new List<string> { "receipt", "SignatureData", "ImageList" };
        public ApiLogAttribute()
        {
            Enabled = true;
            TPDLogEnabled = false;
        }
        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            filterContext.Request.Properties["StartTime"] = DateTime.UtcNow;
            base.OnActionExecuting(filterContext);
        }
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            try
            {
                if (Enabled)
                {
                    double TotalMilliseconds = 0;
                    DateTime startTime = DateTime.UtcNow;
                    DateTime endTime = DateTime.UtcNow;
                    if (actionExecutedContext.Request.Properties["StartTime"] != null)
                    {
                        startTime = (DateTime)actionExecutedContext.Request.Properties["StartTime"];
                        TotalMilliseconds = (endTime - startTime).TotalMilliseconds;
                    }
                    Dictionary<string, object> changes = new Dictionary<string, object>();
                    Dictionary<string, object> actionArgDetails = actionExecutedContext.ActionContext.ActionArguments;
                    string username, deviceDetails;
                    //process the incoming parameter request details.
                    ProcessRequestParametersValues(actionExecutedContext, changes, out username, out deviceDetails, actionArgDetails);

                    //json serialize the request data.
                    var formData = actionArgDetails != null && actionArgDetails.Count > 0 ? JsonConvert.SerializeObject(actionArgDetails) : "";
                    var response = string.Empty;
                    //json serialize the response data.
                    if (actionExecutedContext.Response.Content != null)
                    {
                        response = actionExecutedContext.Response.Content.ReadAsStringAsync().Result;
                        response = Regex.Replace(response.Trim(), @"\r\n", string.Empty);
                    }
                    var jsonresponse = JsonConvert.SerializeObject(response);

                    string controllerName = actionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerName;
                    string actionName = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;

                    LogManager.Logger.WriteAPIInfo(username, controllerName, actionName, formData, jsonresponse, TotalMilliseconds, deviceDetails, startTime, endTime);
                    if (TPDLogEnabled && actionArgDetails != null && actionArgDetails.Count > 0)
                    {
                        // Log Tpd Log
                        var token = actionExecutedContext.Request.Headers.FirstOrDefault(t => t.Key.ToLower() == Utilities.ApplicationConstants.Token).Value.First();
                        string externalRefId = "";
                        if (actionArgDetails != null && actionArgDetails.Count > 0)
                        {
                            if (actionArgDetails.ContainsKey("viewModel") && actionArgDetails["viewModel"] != null)
                            {
                                dynamic req = actionArgDetails["viewModel"];
                                externalRefId = req.ExternalRefID;
                            }
                            else if (actionArgDetails.ContainsKey("request") && actionArgDetails["request"] != null)
                            {
                                dynamic req = actionArgDetails["request"];
                                externalRefId = req.ExternalRefID;
                            }
                            else if (actionArgDetails.ContainsKey("request") && actionArgDetails["request"] == null)
                            {
                                var requestJson = Convert.ToString(HttpContext.Current.Request.Form["request"]) ?? string.Empty;
                                var model = JsonConvert.DeserializeObject<ViewModels.MobileAPI.TPD.TPDImageFileUploadViewModel>(requestJson);
                                externalRefId = model != null ? model.ExternalRefID : "";
                            }
                            Domain.APILogger.AddAPILog(token, controllerName + "-" + actionName, formData, jsonresponse, externalRefId);
                        }
                        //else
                        //Domain.APILogger.AddAPILog(token, controllerName + "-" + actionName, formData, jsonresponse, null);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ApiLog", "OnActionExecuted", ex.Message, ex);
            }
            base.OnActionExecuted(actionExecutedContext);

        }
        /// <summary>
        /// process the incoming paramter request.
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        /// <param name="changes"></param>
        /// <param name="username"></param>
        /// <param name="deviceDetails"></param>
        private static void ProcessRequestParametersValues(HttpActionExecutedContext actionExecutedContext, Dictionary<string, object> changes, out string username, out string deviceDetails, Dictionary<string, object> actionArgDetails)
        {
            foreach (var arg in actionArgDetails.Where(a => a.Value != null))
            {
                var type = arg.Value.GetType();
                if (IsEnumerable(type))
                {
                    changes[arg.Key] = TrimEnumerable((IEnumerable)arg.Value);
                }
                else if (IsComplexObject(type))
                {
                    changes[arg.Key] = TrimObject(arg.Value);
                }
            }
            foreach (var change in changes)
            {
                actionArgDetails[change.Key] = change.Value;
            }
            IEnumerable<string> headerValues;
            bool userName_exists = actionExecutedContext.Request.Headers.TryGetValues("username", out headerValues);
            username = "NA";
            if (userName_exists)
            {
                username = headerValues.FirstOrDefault();
            }
            IEnumerable<string> deviceValues;
            bool device_exists = actionExecutedContext.Request.Headers.TryGetValues("device", out deviceValues);
            deviceDetails = "NA";
            if (device_exists)
            {
                deviceDetails = deviceValues.FirstOrDefault();
            }
        }

        private static IEnumerable TrimEnumerable(IEnumerable value)
        {
            var enumerable = value as object[] ?? value.Cast<object>().ToArray();
            return enumerable.OfType<string>().Any() ?
                        enumerable.Cast<string>().Select(s => s == null
                                ? null
                                : s.Trim())
                        : enumerable.Select(TrimObject);
        }
        /// <summary>
        /// determine the object is Enumerable type.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static bool IsEnumerable(Type t)
        {
            return t.IsAssignableFrom(typeof(IEnumerable));
        }
        /// <summary>
        /// determine the object is array or class.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool IsComplexObject(Type value)
        {
            return value.IsClass && !value.IsArray;
        }
        /// <summary>
        /// trim the object property and remove the space from the property value.
        /// </summary>
        /// <param name="argValue"></param>
        /// <returns></returns>
        private static object TrimObject(object argValue)
        {
            if (argValue == null) return null;
            var argType = argValue.GetType();
            if (IsEnumerable(argType))
            {
                TrimEnumerable((IEnumerable)argValue);
            }
            var s = argValue as string;
            if (s != null)
            {
                return s.Trim();
            }
            if (!IsComplexObject(argType))
            {
                return argValue;
            }
            var props = argType
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(prop => prop.PropertyType == typeof(string))
                    .Where(prop => prop.GetIndexParameters().Length == 0)
                    .Where(prop => prop.CanWrite && prop.CanRead);

            foreach (var prop in props)
            {
                if (imagePropertyName.Contains(prop.Name)) //trim the imageProperty that contain the base64 string.
                {
                    prop.SetValue(argValue, null, null);
                }
                else
                {
                    var value = (string)prop.GetValue(argValue, null);
                    if (value != null)
                    {
                        value = value.Trim();
                        prop.SetValue(argValue, value, null);
                    }
                }
            }
            return argValue;
        }
    }

}