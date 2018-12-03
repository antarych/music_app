using System.Configuration;
using System.Web.Http;
using System.Web.Http.Cors;
using MusicManagement.Application;

namespace Frontend
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            ConfigureCrossDomainRequestsSupport(config);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var authorizer = config.DependencyResolver.GetService(typeof(IAuthorizer)) as IAuthorizer;
        }

        private static void ConfigureCrossDomainRequestsSupport(HttpConfiguration config)
        {
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);
        }
    }
}
