using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Microsoft.Web.Http.Versioning;
using Microsoft.Web.Http;
using Microsoft.Web.Http.Routing;
using System.Net.Http.Headers;
using Microsoft.Owin.Cors;
using System.Web.Cors;
using Newtonsoft.Json.Serialization;

[assembly: OwinStartup(typeof(GlobomnaticsShop.Api.Startup))]

namespace GlobomnaticsShop.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
           
            //web api config
            var config = new HttpConfiguration();
            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
              name: "Api",
              routeTemplate: "api/{controller}/{id}",
               defaults: new { id = RouteParameter.Optional });

            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));


            var policy = new CorsPolicy()
            {
                AllowAnyHeader = true,
                AllowAnyMethod = true,
                AllowAnyOrigin = true,
            };



            app.UseCors(new CorsOptions
            {
                PolicyProvider = new CorsPolicyProvider
                {
                    PolicyResolver = context => Task.FromResult(policy)
                }
            });
            app.UseWebApi(config);
            System.Web.Http.GlobalConfiguration.Configuration.EnsureInitialized();
        }



    }
}