using JwtWithRefreshTokenDemo.Context;
using JwtWithRefreshTokenDemo.Entities;
using JwtWithRefreshTokenDemo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JwtWithRefreshTokenDemo.Services
{
    public class JwtService : IJwtService
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IConfiguration configuration;

        public JwtService(ApplicationDbContext applicationDbContext,IConfiguration configuration)
        {
            this.applicationDbContext = applicationDbContext;
            this.configuration = configuration;
        }

        public async Task<AuthResponse> GetRefreshTokenAsync(string ipAddress, int userId, string userName)
        {
            var refreshtToken =  GenerateRefreshToken();
            var accessToken = await GenerateToken(userName);
            var user = await applicationDbContext.Users.FirstOrDefaultAsync(x=>x.UserId==userId);
            return await saveTokenDetails(user,accessToken,refreshtToken);

        }

        public async Task<AuthResponse> GetTokenAsync(AuthRequest authRequest,string ipAddress)
        {
            var user = await applicationDbContext.Users.FirstOrDefaultAsync(x => x.UserName == authRequest.UserName && x.Password == authRequest.Password);
            if (user == null)
            {
                return null;
            }

            string tokenString = await GenerateToken(user.UserName);
            string refresToken = GenerateRefreshToken();
            return await saveTokenDetails
                (user, tokenString, refresToken);
          
        }

        private async Task<AuthResponse> saveTokenDetails(User user, string tokenString, string refresToken)
        {
            var userRefreshToken = new UserRefreshToken()
            {
                CreateDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.AddMinutes(2),
                IpAddress = "",
                IsInvalidated = false,
                RefreshToken = refresToken,
                Token = tokenString,
                user = user


            };
            await applicationDbContext.userRefreshTokens.AddAsync(userRefreshToken);
            await applicationDbContext.SaveChangesAsync();

            return new AuthResponse
            {
                Token = tokenString,
                RefreshToken = refresToken,
                IsSuccess=true
            };
        }

        private string GenerateRefreshToken() {
            var bytesArray = new byte[64];
            using (var crtyptoProvider=new RNGCryptoServiceProvider()) {
                crtyptoProvider.GetBytes(bytesArray);
                return Convert.ToBase64String(bytesArray);
            }
        }

        private async Task<string> GenerateToken(string UserName)
        {
            var jwtKey = configuration.GetValue<string>("JwtSettings:Key");
            var keyBytes = Encoding.ASCII.GetBytes(jwtKey);
            var tokenHandler = new JwtSecurityTokenHandler();
            var descripter = new SecurityTokenDescriptor()
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[] {
                  new Claim(ClaimTypes.NameIdentifier,UserName)

                }),
                Expires = DateTime.Now.AddSeconds(60),
                Issuer= "https://localhost:44395/Account",
                Audience= "https://localhost:44395/Account",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(descripter);
            var tokenString = await Task.FromResult(tokenHandler.WriteToken(token));
            return tokenString;
        }

        public async Task<bool> IsTokenValid(string accessToken, string ipAddress)
        {
            var isValid = await applicationDbContext.userRefreshTokens.FirstOrDefaultAsync(x => x.Token == accessToken && x.IpAddress == ipAddress);
            return await Task.FromResult(isValid != null);
        }
    }
}
