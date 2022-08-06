using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompletWebApiCourser.IdentityServer.Auth
{
    public class CustomTokenManager : ICustomTokenManager
    {
        List<Token> tokens = new List<Token>();

        public string CreateToken(string UserName)
        {
            var tk = new Token(UserName);
            tokens.Add(tk);
            return tk.TokenString;
        }

        public string GetUserInfoByToken(string token)
        {
            var tk = tokens.FirstOrDefault(x => x.TokenString == token);
            if (tk != null) return tk.UserName;
            return string.Empty;

        }

        public bool VerifyToken(string token)
        {
            if (token == null) return false;
            return tokens.Any(y => y.TokenString == token && y.ExpiryDate>DateTime.Now);
        }
    }
}
