using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _client;
        public HomeController(IHttpClientFactory httpClientFactor) {
            _client= httpClientFactor.CreateClient();
        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public async Task<IActionResult> Secret() {
            var token = await HttpContext.GetTokenAsync("access_token");
            _client.DefaultRequestHeaders.Add("Authorization",$"Bearer {token}");
              //   var serverRespone = await _client.GetAsync("https://localhost:44393/Secret/index");

            //10 Min Referesh Token Is Completed...?
            var apiResponse=await _client.GetAsync("https://localhost:44338/Secret/index");
            
            return View();
        }

        /*
         With current Access Token we are requesting to the secure Api server it is returning with 401. bz access token is expired
         */

    }
}
