using JwtWithRefreshTokenDemo.Context;
using JwtWithRefreshTokenDemo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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
        public async Task<string> GetTokenAsync(AuthRequest authRequest)
        {
            var user =  await applicationDbContext.Users.FirstOrDefaultAsync(x=>x.UserName==authRequest.UserName && x.Password==authRequest.Password);
            if (user == null) {
                return null;
            }

            var jwtKey = configuration.GetValue<string>("JwtSettings:Key");
            var keyBytes = Encoding.ASCII.GetBytes(jwtKey);
            var tokenHandler = new JwtSecurityTokenHandler();
            var descripter = new SecurityTokenDescriptor()
            {
                Subject=new System.Security.Claims.ClaimsIdentity(new Claim[] { 
                  new Claim(ClaimTypes.NameIdentifier,user.UserName)
                  
                }),
                Expires=DateTime.Now.AddSeconds(60),
                SigningCredentials=new SigningCredentials(new SymmetricSecurityKey(keyBytes),SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(descripter);
            return await Task.FromResult(tokenHandler.WriteToken(token));
        }
    }
}
