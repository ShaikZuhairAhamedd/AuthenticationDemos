using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CompletWebApiCourser.WebApi.Filters
{
    public class CustomTokenAuthFilterAttribute : Attribute, IAuthorizationFilter
    {
        const string TokenHeader = "TokenHeader";
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(TokenHeader, out var token)) {
                context.Result = new UnauthorizedResult();
                return;
            }
            //calling to the Authorization Server for verifying wheather the current token is valid or not


            var url = $"https://localhost:44312/Auth/Verify?token={token}";
            var httpClient = new HttpClient();
            var response= httpClient.GetAsync(url).Result;
            var result = response.Content.ReadAsStringAsync().Result;
            if (result != "true") {
                context.Result = new UnauthorizedResult();
                return;
            }
        }
    }
}
