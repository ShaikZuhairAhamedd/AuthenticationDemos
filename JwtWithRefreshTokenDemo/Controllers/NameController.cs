using JwtWithRefreshTokenDemo.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtWithRefreshTokenDemo.Controllers
{
    [Route("api/[controller]")]
  
    [ApiController]
    public class NameController : ControllerBase
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IConfiguration configuration;

        public NameController(ApplicationDbContext applicationDbContext, IConfiguration configuration)
        {
            this.applicationDbContext = applicationDbContext;
            this.configuration = configuration;
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetNames() {
            var data = await Task.FromResult(new List<string>() { "hello" });
            /*
             applicationDbContext.userRefreshTokens.Select(x=> { 
                RowNumber=EntryPointNotFoundException.
             });
            */
            return Ok(data);
        }
            
            
    }
}
