using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace CompletWebApiCourser.IdentityServer.Auth
{
    public class JwtTokenManager : ICustomTokenManager
    {
        private JwtSecurityTokenHandler tokenHandler;
        private readonly IConfiguration configration;
        private byte[] secretKey;
        public JwtTokenManager(IConfiguration configration) {
            this.configration = configration;
            this.tokenHandler = new JwtSecurityTokenHandler();
            var key= configration.GetValue<String>("JwtSecretKey");
            this.secretKey = Encoding.ASCII.GetBytes(key);
        }
        public string CreateToken(string UserName)
        {
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new List<Claim>{
                   new Claim(ClaimTypes.Name,UserName)
                  }),
                Expires=DateTime.Now.AddMinutes(30),
                SigningCredentials=new SigningCredentials(new SymmetricSecurityKey(secretKey),SecurityAlgorithms.HmacSha256)
              

            };

            var securityToken=tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);
        }

        public string GetUserInfoByToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return null;
            var jwtToken = tokenHandler.ReadToken(token.Replace("\"", string.Empty)) as JwtSecurityToken;
            var claim = jwtToken.Claims.FirstOrDefault(x => x.Type == "unique_name");
             if (claim != null) 
             return claim.Type;

            return null;
        }

        public bool VerifyToken(string token)
        {
            if (string.IsNullOrEmpty(token)) return false;
            // tlhis added for dummy puposetesting
            var myIssuer = "https://localhost:44312/";
            var myAudience = "https://localhost:44341";
            SecurityToken securityToken;

            try
            {

              

                var tokenValidatorParameter = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = false,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ValidIssuer = myIssuer,
                    ValidAudience = myAudience,
                    ValidateAudience = false,
                    ValidateIssuer=false,
                 };

                tokenHandler.ValidateToken(token.Replace("\"", string.Empty), tokenValidatorParameter, out securityToken);
            }
            catch (SecurityTokenException ex) {
                return false;
            }
            catch (Exception ex)
            {
                throw;
            }

            return securityToken != null;


        }
    }
}
