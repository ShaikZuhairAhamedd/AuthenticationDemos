using JwtWithRefreshTokenDemo.Context;
using JwtWithRefreshTokenDemo.Models;
using JwtWithRefreshTokenDemo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JwtWithRefreshTokenDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IJwtService jwtService;
        private readonly ApplicationDbContext applicationDbContext;

        public AccountController(IJwtService jwtService,ApplicationDbContext applicationDbContext)
        {
            this.jwtService = jwtService;
            this.applicationDbContext = applicationDbContext;
        }

        [HttpPost("[action]")]
        
        public async Task<IActionResult> AuthToken(AuthRequest authRequest) {

            if (!ModelState.IsValid) {
                return BadRequest(new AuthResponse { IsSuccess = false,Reason="UserName and Password Must Be Provided" }) ;
                
            }

            var authResponse = await this.jwtService.GetTokenAsync(authRequest,HttpContext.Connection.RemoteIpAddress.ToString());
            if (authResponse == null) {
                return Unauthorized();
            }
            return Ok(authResponse);
        }

        //Only Refresh Token will be created if accesstoken is valid
        [HttpPost("[action]")]
        public async Task<IActionResult> RefreshToken([FromBody]RefreshTokenRequest request) {
            if (!ModelState.IsValid) {
                return BadRequest(new AuthResponse { IsSuccess = false,Reason="Token Must be Provided" });
            }
            var token = GetJwtToken(request.ExpiresToken);
            var ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
            var userRefreshToken = 
                                 applicationDbContext
                                 .userRefreshTokens
                                 .FirstOrDefault(x => x.IsInvalidated == false &&
                                                  x.Token == request.ExpiresToken &&
                                                  x.RefreshToken == request.RefreshToken );

            AuthResponse response = validateDetails(token, userRefreshToken);
            if (!response.IsSuccess) { return BadRequest(response); }

            //InValidating the OldRefresh Token 
            userRefreshToken.IsInvalidated = true;
            applicationDbContext.userRefreshTokens.Update(userRefreshToken);
            await applicationDbContext.SaveChangesAsync();

            var userName = token.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            // Now here we are generating the Refresh token 
            AuthResponse newRefreshToken = await jwtService.GetRefreshTokenAsync(ipAddress,
                                                            userRefreshToken.user.UserId, userName);
            return Ok(newRefreshToken);
        }

        private AuthResponse validateDetails(JwtSecurityToken token, Entities.UserRefreshToken userRefreshToken)
        
        
        
        
        
        {
            /*
                 For Auth Respone IsSuccess=false is to be used to define i.e 
                 there is no need to create a refresh token for the given tokne
             */
            // 1) wheather the access Tokenis present or not in the database
            if (userRefreshToken == null) {
                return new AuthResponse { IsSuccess = false, Reason = "Invalidate Token Details" };
            }
              
            // 2) cheking the accessToken is expired or not
            if (token.ValidTo > DateTime.UtcNow) {
                return new AuthResponse { IsSuccess = false, Reason = "Token is not expired" };
            }
            // 3) checking the refresh token is expired or not
            if (!userRefreshToken.IsActive) {
                return new AuthResponse
                {
                    IsSuccess = false,
                    Reason = "Refresh Token Expired"
                };
            }

            // if refresh token is not expired and access token is expired then we can create a new access Token

            return new AuthResponse { IsSuccess = true };

        }

        private JwtSecurityToken GetJwtToken(string expiresToken)
        {
            var jwtSecurtiyTokenHandler = new JwtSecurityTokenHandler();
            //Converts a string into an instance of JwtSecurityToken.through the ReadJwtToken method

            return jwtSecurtiyTokenHandler.ReadJwtToken(expiresToken);
        }
    }
}
