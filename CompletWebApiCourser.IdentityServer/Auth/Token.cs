using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompletWebApiCourser.IdentityServer.Auth
{
    public class Token
    {

        public Token(string UserName) {
            this.UserName = UserName;
            this.TokenString = Guid.NewGuid().ToString();
            this.ExpiryDate = DateTime.Now.AddMinutes(1);
        }
        public string TokenString { get; set; }
        public string UserName { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
