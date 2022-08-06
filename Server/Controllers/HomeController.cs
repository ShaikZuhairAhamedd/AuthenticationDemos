using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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

            var token = new JwtSecurityToken();



            return RedirectToAction("Index");
        }
    }
}
