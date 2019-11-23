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
            config.EnableCors();
            var policy = new CorsPolicy()
            {
                AllowAnyOrigin=true,
                SupportsCredentials=false

            };
            policy.Origins.Add("http://localhost:8080");


            app.UseCors(new CorsOptions
            {
                PolicyProvider = new CorsPolicyProvider
                {
                    PolicyResolver = context => Task.FromResult(policy)
                }
            });

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