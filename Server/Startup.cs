using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("OAuth")
                    .AddJwtBearer("OAuth",(config)=> {

                        //This Event is to receive the token from The Query String
                        // and assign i.e token to the required place

                        // by default this token value is to be extracted from  the Authentication Middleware throug
                        // jwt Bearer Authorization Handler from the Authorization Header

                        config.Events = new JwtBearerEvents()
                        {
                            OnMessageReceived = (context) => {
                                if (context.Request.Query.ContainsKey("access_token")) {

                                    context.Token = context.Request.Query["access_token"];
                                }
                                
                                return Task.CompletedTask;
                            }
                        };

                        config.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters() {
                                                       IssuerSigningKey = new SymmetricSecurityKey(
                                                                                        Encoding.UTF8.GetBytes(Constant.Secret)),
                                                       ValidIssuer = Constant.Issuer,
                                                      ValidAudience=Constant.Audiance,
                                                };




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
