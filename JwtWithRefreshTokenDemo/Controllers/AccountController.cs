using JwtWithRefreshTokenDemo.Models;
using JwtWithRefreshTokenDemo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtWithRefreshTokenDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IJwtService jwtService;

        public AccountController(IJwtService jwtService)
        {
            this.jwtService = jwtService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AuthToken(AuthRequest authRequest) {

            var token = await this.jwtService.GetTokenAsync(authRequest);
            if (token == null) {
                return Unauthorized();
            }
            return Ok(new AuthResponse { Token = token });
        }
    }
}
