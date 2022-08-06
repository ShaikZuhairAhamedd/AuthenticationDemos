using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompletWebApiCourser.IdentityServer.Auth
{
    public class CustomUserManager : ICustomUserManager
    {
        private readonly ICustomTokenManager customTokenManager;
        private Dictionary<string, string> credentials = new Dictionary<string, string> {
            {"shaik","shaik" },
            {"zuhair","zuhair" }
        };

        public CustomUserManager(ICustomTokenManager customTokenManager)
        {
            this.customTokenManager = customTokenManager;
        }


        public string Authentiate(string userName, string Password)
        {
            //Authenticate the Credentials
            if (credentials[userName] != Password) {
                return string.Empty;
            }
             
            //Generating the Token
            return customTokenManager.CreateToken(userName);

        }
  
    }
}
