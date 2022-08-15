using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ResourceApi.JwtRequirment
{
    public class JwtRequirementHandler : AuthorizationHandler<JwtRequirement>
    {
        HttpClient _client;
        HttpContext _httpContext;
        public JwtRequirementHandler(IHttpClientFactory httpClientFactory,IHttpContextAccessor httpContextAccessor) {
             _client = httpClientFactory.CreateClient();
            _httpContext = httpContextAccessor.HttpContext;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, JwtRequirement requirement)
        {
            if (_httpContext.Request.Headers.TryGetValue("Authorization",out var authHeader)) {
                var accessToken = authHeader.ToString().Split(' ')[1];
                var serverResponse = await _client.GetAsync($"https://localhost:44393/OAuth/Validate?access_token={accessToken}");

                if (serverResponse.StatusCode == System.Net.HttpStatusCode.OK) {
                    context.Succeed(requirement);
                }


            }

        }
    }
}
