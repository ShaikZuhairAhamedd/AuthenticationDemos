using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompletWebApiCourser.WebApi.Filters
{
    public class APIKeyAuthFilterSingleCLientAttribute : Attribute, IAuthorizationFilter
    {
        private const string ApiKeyHeader="ApiKey";
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeader, out var ClientApiKey)) {

                context.Result = new UnauthorizedResult();
                return;
            }

            // we are getting the Iconfiguarion Service so i.e to extract the configuration value
            var config=context.HttpContext.RequestServices.GetService(typeof(IConfiguration)) as IConfiguration;
            var apiKey = config.GetValue<string>("ApiKey");

            if (apiKey != ClientApiKey) {
                context.Result = new UnauthorizedResult();

            }
        }
    }
}
