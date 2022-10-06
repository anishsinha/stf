using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Script.Serialization;

namespace SiteFuel.Exchange.Web.Helpers
{
    public static class HtmlHelperMethods
    {
        public static string ImageSrc(this HtmlHelper html, string path)
        {
            var img = string.Empty;
            if (path != null)
            {
                byte[] imageArray = File.ReadAllBytes(path);
                img = Convert.ToBase64String(imageArray);
            }
            return img;
        }

        public static string ImageSrc(this HtmlHelper html, byte[] image)
        {
            var img = string.Empty;
            if (image != null)
            {
                img = String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(image));
            }
            return img;
        }

        public static MvcHtmlString Image(this HtmlHelper html, byte[] image)
        {
            var img = string.Empty;
            if (image != null)
            {
                img = String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(image));
            }
            return new MvcHtmlString("<img src='" + img + "' />");
        }

        public static MvcHtmlString ActionLinkIcon(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes = null, String iconName = "")
        {
            var linkMarkup = htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes).ToHtmlString();
            var iconMarkup = String.Format("<i class=\"{0}\" aria-hidden=\"true\"></i>", iconName);
            return new MvcHtmlString(linkMarkup.Insert(linkMarkup.IndexOf(@"</a>"), iconMarkup));
        }

        public static HtmlString LabelForEx<TModel, TResult>(this HtmlHelper<TModel> html, Expression<Func<TModel, TResult>> expression, string labelText)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string id = html.IdFor(expression).ToString();
            string resolvedLabelText = labelText ?? metadata.DisplayName ?? metadata.PropertyName;
            string renderedHtml = string.Format("<label for=\"{0}\">{1}</label>", id, resolvedLabelText);
            return new HtmlString(renderedHtml);
        }

        private static HtmlString LabelForEx<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);

            string id = html.IdFor(expression).ToString();
            string resolvedLabelText = metadata.DisplayName ?? metadata.PropertyName;

            if (metadata.IsRequired)
            {
                resolvedLabelText = $"<label for=\"{id}\" >{resolvedLabelText}</label><span class=\"required\">*</span>";
            }
            return new HtmlString(resolvedLabelText);
        }

        public static HtmlString MirrorFor<TModel, TProperty>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProperty>> expression)
        {
            string id = html.IdFor(expression).ToString();
            var value = html.ValueFor(expression);
            return new HtmlString($"<span id=Mirror_{id}>{value.ToString()}</span>");
        }

        public static HtmlString GetEnums<T>(this HtmlHelper helper) where T : struct
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("<script type=\"text/javascript\">");
            sb.AppendLine("if(!window.Enum) Enum = {};");
            var type = typeof(T);
            var values = Enum.GetValues(type).Cast<T>();
            var dict = values.ToDictionary(e => e.ToString(), e => Convert.ToInt32(e));
            var json = new JavaScriptSerializer().Serialize(dict);
            var script = string.Format("{0}={1};", type.Name, json);
            sb.AppendLine("Enum." + script);
            sb.AppendLine("</script>");
            return new HtmlString(sb.ToString());
        }

        public static HtmlString GetFormattedResource(this HtmlHelper helper, string resourceKey, params object[] parameters)
        {
            var resourceStr = ResourceMessages.GetMessage(resourceKey, parameters);
            return new HtmlString(resourceStr);
        }

        public static string GetAppSetting(this HtmlHelper html, string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                return ContextFactory
                        .Current
                        .GetDomain<ApplicationDomain>()
                        .GetApplicationSettingValue<string>(key);
            }
            return string.Empty;
        }

        public static string GetStripePublicKey(this HtmlHelper html)
        {
            var environment = (ApplicationEnvironment)ConfigHelperMethods.GetConfigSetting(ApplicationConstants.Environment).GetValue<int>();
            if (environment == ApplicationEnvironment.Prod)
            {
                return GetAppSetting(html, ApplicationConstants.KeyAppSettingLiveStripePublicKey);
            }
            else
            {
                return GetAppSetting(html, ApplicationConstants.KeyAppSettingTestStripePublicKey);
            }
        }

        public static MvcHtmlString PartialFor<TModel, TProperty>(this HtmlHelper<TModel> html,
            string partialName,
            Expression<Func<TModel, TProperty>> expression)
        {
            string name = ExpressionHelper.GetExpressionText(expression);
            string modelName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            ModelMetadata metaData = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            object model = metaData.Model;

            if (partialName == null)
            {
                partialName = metaData.TemplateHint ?? typeof(TProperty).Name;
            }

            // Use a ViewData copy with a new TemplateInfo with the prefix set
            ViewDataDictionary viewData = new ViewDataDictionary(html.ViewData)
            {
                TemplateInfo = new TemplateInfo { HtmlFieldPrefix = modelName }
            };

            // Call standard MVC Partial
            return html.Partial(partialName, model, viewData);
        }

        public static MvcHtmlString PartialFor<TModel, TProperty>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProperty>> expression)
        {
            return html.PartialFor(null, expression);
        }

        //For including JS/CSS from partial views into layout at the end
        // => Start
        private class ScriptBlock : IDisposable
        {
            private const string scriptsKey = "scripts";
            public static List<string> PageScripts
            {
                get
                {
                    if (HttpContext.Current.Items[scriptsKey] == null)
                        HttpContext.Current.Items[scriptsKey] = new List<string>();
                    return (List<string>)HttpContext.Current.Items[scriptsKey];
                }
            }

            WebViewPage webPageBase;

            public ScriptBlock(WebViewPage webPageBase)
            {
                this.webPageBase = webPageBase;
                this.webPageBase.OutputStack.Push(new StringWriter());
            }

            public void Dispose()
            {
                PageScripts.Add(((StringWriter)this.webPageBase.OutputStack.Pop()).ToString());
            }
        }

        public static IDisposable BeginScripts(this HtmlHelper html)
        {
            return new ScriptBlock((WebViewPage)html.ViewDataContainer);
        }

        public static MvcHtmlString PageScripts(this HtmlHelper html)
        {
            return MvcHtmlString.Create(string.Join(Environment.NewLine, ScriptBlock.PageScripts.Select(s => s.ToString())));
        }
        // => End

        public static MvcHtmlString IsReadonly(this MvcHtmlString htmlString, bool readOnly)
        {
            string rawstring = htmlString.ToString();
            if (readOnly)
            {
                rawstring = rawstring.Insert(rawstring.Length - 2, "readonly=\"readonly\"");
            }
            return new MvcHtmlString(rawstring);
        }

        public static MvcHtmlString Disabled(this MvcHtmlString htmlString, bool disable)
        {
            string rawstring = htmlString.ToString();
            if (disable)
            {
                var elementEnd = rawstring.IndexOf("/>") > 0 ? "/>" : ">";
                rawstring = rawstring.Insert(rawstring.IndexOf(elementEnd), "disabled=\"disabled\"");
            }
            return new MvcHtmlString(rawstring);
        }
    }
}