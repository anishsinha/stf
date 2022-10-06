using System.Web.Http;
using WebActivatorEx;
using SiteFuel.Exchange.Api.Mobile;
using Swashbuckle.Application;

using System;
using System.Web.Http.Description;
using Swashbuckle.Swagger;
[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace SiteFuel.Exchange.Api.Mobile
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            //    var thisAssembly = typeof(SwaggerConfig).Assembly;

            //    GlobalConfiguration.Configuration
            //        .EnableSwagger(c =>
            //            {
            //                c.ApiKey("Token")
            //    .Description("Filling bearer token here")
            //    .Name("Authorization")
            //    .In("header");
            //                c.DocumentFilter<SwaggerFilterOutControllers>();
            //                c.SingleApiVersion("v1", "SiteFuel.Exchange.Api.Mobile");
            //            })
            //        .EnableSwaggerUi(c =>
            //            {
            //                c.EnableApiKeySupport("token", "header");
            //            });
            //}
        }
    }
    public class SwaggerFilterOutControllers : IDocumentFilter
    {
        void IDocumentFilter.Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {
            foreach (ApiDescription apiDescription in apiExplorer.ApiDescriptions)
            {
                Console.WriteLine(apiDescription.Route.RouteTemplate);

                if ((apiDescription.RelativePathSansQueryString().StartsWith("api/LiftFile/"))
                    || (apiDescription.RelativePath.StartsWith("api/Master/"))
                    || (apiDescription.Route.RouteTemplate.StartsWith("api/Notification/"))
                    )
                {
                    swaggerDoc.paths.Remove("/" + apiDescription.RelativePath.TrimEnd('/'));
                }
            }
        }
    }

}




