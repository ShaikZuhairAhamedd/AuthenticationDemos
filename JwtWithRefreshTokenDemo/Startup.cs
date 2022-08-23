using JwtWithRefreshTokenDemo.Context;
using JwtWithRefreshTokenDemo.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtWithRefreshTokenDemo
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration config)
        {
            Configuration = config;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<ApplicationDbContext>(options=> {
                options.UseSqlServer(Configuration.GetConnectionString("AppDbContext"));
            });
            services.AddTransient<IJwtService,JwtService>();

           

            services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwtOptions =>
            {
                var jwtKey = Configuration.GetValue<string>("JwtSettings:Key");
                var keyBytes = Encoding.UTF8.GetBytes(jwtKey);
                var tokenValidaton = new TokenValidationParameters()
                {
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                   
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuerSigningKey=true

                };

                // assiging this  TokenValidationProvider

                jwtOptions.TokenValidationParameters = tokenValidaton;
                // after validating the Token through based on the details specified in the tokenValidation Parameter
                jwtOptions.Events = new JwtBearerEvents() {
                    OnTokenValidated = async (context) => {
                        // this method is used for custom Validation--?--?--?
                        //  other than the validatin like keyValidation,TimeValidation etc...

                        var ipAddress = context.Request.HttpContext.Connection.RemoteIpAddress.ToString();
                        var jwtService = context.Request.HttpContext.RequestServices.GetService<IJwtService>();
                        var jwtToken = context.SecurityToken as JwtSecurityToken;
                        if (!await jwtService.IsTokenValid(jwtToken.RawData, ipAddress)) {
                            context.Fail("Invalid Token Details");
                        }
                         
                     }
                };



            });
            services.AddAuthorization();
                    
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
