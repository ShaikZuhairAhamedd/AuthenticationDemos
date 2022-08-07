using Microsoft.AspNetCore.Authorization;
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
    public class HomeController : Controller
    {
        public HomeController() { 
        }
        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public IActionResult Secret() {
            return View();
        }
        
        public IActionResult Authenticate() {
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
                  notBefore:DateTime.Now,
                  expires:DateTime.Now.AddHours(1),
                  signingCredentials
                );

            var jwtSecurityHandler = new JwtSecurityTokenHandler();
            var tokenString = jwtSecurityHandler.WriteToken(token);

            return Ok(new { access_token= tokenString });

        }

        public IActionResult Decode(string part) {
            // JWT Token is in the String format of the original Data into the base64 

            var bytes = Convert.FromBase64String(part);
            return Ok(Encoding.UTF8.GetString(bytes));
        }
    }
}
