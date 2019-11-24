using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Newtonsoft.Json.Serialization;
using Owin;

[assembly: OwinStartup(typeof(GlobomanticsShop.Startup))]

namespace GlobomanticsShop
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            //web api config
            var config = new HttpConfiguration();


            //cors
            var globalPolicy = new CorsPolicy();
            globalPolicy.AllowAnyHeader = true;
            globalPolicy.AllowAnyMethod = true;
            globalPolicy.Origins.Add("http://globomanticsshop.com");

            app.UseCors(new Microsoft.Owin.Cors.CorsOptions
            {
                PolicyProvider = new CorsPolicyProvider
                {
                    PolicyResolver = context => Task.FromResult(globalPolicy)
                }
            }); ;
            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
              name: "Api",
              routeTemplate: "api/{controller}/{id}",
               defaults: new { id = RouteParameter.Optional });

            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            //app.UseCors(CorsOptions.AllowAll);


            app.UseWebApi(config);
            System.Web.Http.GlobalConfiguration.Configuration.EnsureInitialized();
        }



    }
}
