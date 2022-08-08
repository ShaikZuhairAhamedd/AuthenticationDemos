using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Server.Controllers
{
    public class OAuthController : Controller
    {
        [HttpGet]
        public IActionResult Authorize(string response_type,
                                       string client_id,
                                       string redirect_uri,
                                       string scope,
                                       string state
                                      )        
        
        {
            var query = new QueryBuilder();
            query.Add("redirect_uri", redirect_uri);
            query.Add("state",state);
            return View(model:query.ToString());
        }
        [HttpPost]
        public IActionResult Authorize(string UserName, 
                                       
                                       string redirect_uri,
                                       string scope,
                                       string state)
        {
            const string code = "testing";
            var query = new QueryBuilder();
            query.Add("code",code);
            query.Add("state",state);
            return Redirect($"{redirect_uri}{query.ToString()}");
        }
        [HttpPost]
        public async Task<IActionResult> Token(string grant_type,string code,string redirect_uri,string client_id)
        {
            
                //Please see the documentation what they are returning...?

            var claims = new[] {
                 new Claim(JwtRegisteredClaimNames.Sub,"some_id"),
                 new Claim("Grammy","Cookie")
             };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Constant.Secret));

            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                  Constant.Issuer,
                  Constant.Audiance,
                  claims,
                  notBefore: DateTime.Now,
                  expires: DateTime.Now.AddHours(1),
                  signingCredentials
                );

            var jwtSecurityHandler = new JwtSecurityTokenHandler();
            var tokenString = jwtSecurityHandler.WriteToken(token);

            var responseObject = new responseType
            {
                access_token = tokenString,
                token_type = "Bearer",
                raw_claim = "OAuthTutorials"
            };

            var responseJSON = System.Text.Json.JsonSerializer.Serialize(responseObject,typeof(responseType));
            var responseBytes = Encoding.UTF8.GetBytes(responseJSON);
            await  Response.Body.WriteAsync(responseBytes,0,responseBytes.Length);

            return Redirect(redirect_uri);
        }
        
    }
}
