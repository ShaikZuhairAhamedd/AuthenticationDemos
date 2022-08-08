using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(config=> {
                //we check thecookie to confirm i.e we are authenticated
                config.DefaultAuthenticateScheme = "ClientCookie";
                //when we sign in we will deal out a cookie
                config.DefaultSignInScheme = "ClientCookie";
                //use this to check if we are allowed to do someThing
                config.DefaultChallengeScheme = "OurServer";
                   })
                    .AddCookie("ClientCookie")
                    .AddOAuth("OurServer", config=> {
                        config.ClientId = "client_id";
                        config.ClientSecret = "client_server";
                        config.CallbackPath = "/oauth/Callback";
                        config.AuthorizationEndpoint = "https://localhost:44393/oauth/Authorize";
                        config.TokenEndpoint = "https://localhost:44393/oauth/token";


                    });
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}