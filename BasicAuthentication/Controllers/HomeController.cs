using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BasicAuthentication.Controllers
{
    public class HomeController:Controller
    {
        public IActionResult Index() {
            return View();       
        }
        [Authorize]
        public IActionResult Secret() {
            return View();
        }

        public IActionResult Authenticate() {

            var grandMaClients = new List<Claim>()
            {
                new  Claim(ClaimTypes.Name,"Zuhair"),
                new Claim(ClaimTypes.Email,"Zuhair@gmail.com"),
                new Claim("GranmaSays","Very Nice Boi")


            };

            var licenceClaims = new List<Claim>
            {
                new Claim(ClaimTypes.GivenName,"ZUHAIR"),
                new Claim("Licence No","0123456")
            };
            var grandManIdentity = new ClaimsIdentity(grandMaClients,"GrandMa Identity");
            var licenceIdentity = new ClaimsIdentity(licenceClaims, "Government");
            var userPrinciple = new ClaimsPrincipal(new[] { grandManIdentity });
            // this method help to generate the session Cookie
            HttpContext.SignInAsync(userPrinciple);

            return RedirectToAction("Index");
        }
    }
}
