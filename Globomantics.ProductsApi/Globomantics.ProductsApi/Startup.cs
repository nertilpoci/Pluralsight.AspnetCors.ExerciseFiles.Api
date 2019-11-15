using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

using ZNetCS.AspNetCore.Authentication.Basic;
using ZNetCS.AspNetCore.Authentication.Basic.Events;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication;

namespace Globomantics.ProductsApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // configure basic authentication 
            services
     .AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme)
     .AddBasicAuthentication(
       options =>
       {
           options.Realm = "My Application";
           options.Events = new BasicAuthenticationEvents
           {
               OnValidatePrincipal = context =>
               {
                   
                       var claims = new List<Claim>
               {
                new Claim(ClaimTypes.Name,
                          context.UserName,
                          context.Options.ClaimsIssuer)
               };
                       var principal = new ClaimsPrincipal(new ClaimsIdentity(
                          claims,
                          BasicAuthenticationDefaults.AuthenticationScheme));
                       var ticket = new AuthenticationTicket(principal,
                        new Microsoft.AspNetCore.Authentication.AuthenticationProperties(),
                        BasicAuthenticationDefaults.AuthenticationScheme);
                       context.Principal = principal;
                       return Task.FromResult(AuthenticateResult.Success(ticket));
                  

               }
           };
       });
           
            var allowedOrigins = Configuration.GetValue<string>("AllowedOrigins")?.Split(",") ?? new string[0];
            services.AddCors(options =>
            {
                options.AddPolicy("GlobomanticsInternal", builder => builder.AllowAnyOrigin().AllowCredentials());
                options.AddPolicy("PublicApi", builder => builder.AllowAnyOrigin().WithMethods("Get").WithHeaders("Content-Type"));
            });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
           
            app.UseRouting();
            app.UseCors("GlobomanticsInternal");
           
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
  }
