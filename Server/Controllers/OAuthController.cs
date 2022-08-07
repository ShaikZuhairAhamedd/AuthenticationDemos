using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public IActionResult Token(string grant_type,string code,string redirect_uri,string client_id)
        {
            //video:33.31
            //here redirection will be happening
            return View();
        }
        
    }
}
