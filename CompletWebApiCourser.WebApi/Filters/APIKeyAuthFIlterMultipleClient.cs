using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompletWebApiCourser.WebApi.Filters
{
    public class APIKeyAuthFIlterMultipleClient : Attribute, IAuthorizationFilter
    {
        const string ClientIdHeader = "ClientId";
        const string ApiKeyHeader = "ApiKey";
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(ClientIdHeader, out var clientId)) {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeader, out var clientApiKey)) {
                context.Result = new UnauthorizedResult();
                return;
            }

            var config = context.HttpContext.RequestServices.GetService(typeof(IConfiguration)) as IConfiguration;
            var apiKey = config.GetValue<string>($"ApiMultipleClientKey:{clientId}");
            if (apiKey != clientApiKey) {
                context.Result = new UnauthorizedResult();
                return;

            }

        }
    }
}
