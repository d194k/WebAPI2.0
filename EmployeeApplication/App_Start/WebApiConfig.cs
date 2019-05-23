using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Web.Http.Cors;
using System.Web.Http.Dispatcher;
using EmployeeApplication.Custom;

namespace EmployeeApplication
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "ServiceVersion1",
                routeTemplate: "api/v1/employee/{id}",
                defaults: new { controller = "EmployeeV1", id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "ServiceVersion2",
                routeTemplate: "api/v2/employee/{id}",
                defaults: new { controller = "EmployeeV2", id = RouteParameter.Optional }
            );
            //Versioning through QueryString
            //config.Services.Remove(typeof(IHttpControllerSelector), new CustomControllerSelector(config));

            //Enable Cross Origin Resource Sharing
            EnableCorsAttribute cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);
        }
    }
}
