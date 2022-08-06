using CompletWebApiCourser.IdentityServer.Auth;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CompletWebApiCourser.IdentityServer.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ICustomUserManager customUserManager;
        private readonly ICustomTokenManager customTokenManager;

        public AuthController(ICustomUserManager customUserManager,
            ICustomTokenManager customTokenManager)
        {
            this.customUserManager = customUserManager;
            this.customTokenManager = customTokenManager;
        }
        [HttpGet]
        [Route("Authenticate")]
        public Task<string> Authenticate(string username, string password) {
            return Task.FromResult(customUserManager.Authentiate(username, password));
        }
        [HttpGet]
        [Route("Verify")]
        public Task<bool> Verify(string token) {
            return Task.FromResult(customTokenManager.VerifyToken(token));
        }
        [HttpGet]
        [Route("GetUserInfoByToken")]
        public  Task<string> GetUserInfoByToken(string token) {
            return Task.FromResult(customTokenManager.GetUserInfoByToken(token));
        }

    }
}
