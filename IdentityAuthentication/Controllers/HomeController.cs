using IdentityAuthentication.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public HomeController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [Route("Login",Name ="Login")]
        [HttpPost]
        public async Task<IActionResult> Login(string Username,string password) {

            var user = await userManager.FindByNameAsync(Username);
            if (user != null) {
                //  signin
                var signInResult= await signInManager.PasswordSignInAsync(Username,password,false,false);
                if (signInResult.Succeeded) { 
                 //
                }
            }

            return null;
        }

        [Route("Register", Name = "Register")]
        [HttpPost]
        public async Task<IActionResult> Register(string Username,string Password) {

            var User = new IdentityUser
            {
                UserName=Username,
                Email=""

            };
            var result= await  userManager.CreateAsync(User);
            //here register is successfulll;
            //so now we are signing

            return Ok( new
            {
                User
            });
        }
    }
}
