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
            services.AddAuthentication(config => {
                //we check thecookie to confirm i.e we are authenticated
                config.DefaultAuthenticateScheme = "ClientCookie";
                //when we sign in we will deal out a cookie
                config.DefaultSignInScheme = "ClientCookie";
                //use this to check if we are allowed to do someThing
                config.DefaultChallengeScheme = "OurServer";
            })
                    .AddCookie("ClientCookie")
                    .AddOAuth("OurServer", config => {
                        config.ClientId = "client_id";
                        config.ClientSecret = "client_server";
                        config.CallbackPath = "/oauth/Callback";
                        config.AuthorizationEndpoint = "https://localhost:44393/oauth/Authorize";
                        config.TokenEndpoint = "https://localhost:44393/oauth/token";
                        config.SaveTokens = true;
                        config.Events = new Microsoft.AspNetCore.Authentication.OAuth.OAuthEvents()
                        {

                            OnCreatingTicket = (context) =>
                            {
                                
                                /*By Default this whole Part is impletend what we are doing
                                 * i.e is from accessToken creating of the cookie and i.e
                                 
                                var accessToken = context.AccessToken;
                                var base64Payload= accessToken.Split('.')[1];
                                var bytes = Convert.FromBase64String(base64Payload);
                                var jsonPayload = System.Text.Encoding.UTF8.GetString(bytes);
                                var claims = System.Text.Json.JsonSerializer.Deserialize <Dictionary<string, object >>(jsonPayload);
                                /* previously during Dictionary<string,string> we are getting error bz
                                   this deserializer cannnot converting the number into string type....

                                 
                                foreach (var claim in claims) {
                                    context.Identity.AddClaim(new System.Security.Claims.Claim(claim.Key, Convert.ToString(claim.Value)));
                                }
                                */

                                return Task.CompletedTask;
                            }
                        };


                    });
            services.AddHttpClient();
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
