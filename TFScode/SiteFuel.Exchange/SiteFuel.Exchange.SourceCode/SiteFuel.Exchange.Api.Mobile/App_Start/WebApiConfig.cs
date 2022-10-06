using Newtonsoft.Json.Serialization;
using System.Web.Http;
using Swashbuckle.Application;
using System;
using System.Collections.Generic;
using System.Linq;


namespace SiteFuel.Exchange.Api.Mobile
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();
          
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // This is a quick fix for using camelcase to match with old architecture - Shrinivas
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.XmlFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("multipart/form-data"));

            
            //enable swagger
            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    c.ApiKey("Token")
        .Description("Filling bearer token here")
        .Name("Authorization")
        .In("header");
                    c.DocumentFilter<SwaggerFilterOutControllers>();
                    c.SingleApiVersion("v1", "SiteFuel.Exchange.Api.Mobile");
                })
                .EnableSwaggerUi(c =>
                {
                    c.EnableApiKeySupport("token", "header");
                });
        }
    }
    }

