using JwtAuthenticationDemo.BusinessDomain;
using JwtAuthenticationDemo.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace JwtAuthenticationDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // ....DataBase User.....
        static User user=new User();

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<User>> Register(UserDTO userDto) {
            
            CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
            
            //...IN Database we are storing a) password Hash and Password Salt for the particular user....

            user.Username = userDto.UserName;
            user.PasswordHash = passwordHash;
            user.PasswordHash = passwordSalt;


            // saving this user in the database .. temporarily in the database

            return Ok(user);
        }
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<string>> Login(UserDTO request) {
            if (user.Username != request.UserName) {
                return BadRequest("User Not Found");
            }
            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt)) {
                return BadRequest("Incorrect Password");
            }

            // after getting the user Data From the Database Now Creating the Token for this User

            string token = CreateToken(user);
            return Ok(token);
        }

        private string CreateToken(User user)
        {

            var secretKey = "kkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk";
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
             claims: new List<Claim> {
               new Claim(ClaimTypes.Name,user.Username)
             },
             expires: DateTime.Now.AddDays(1),
             signingCredentials: signingCredentials
            );
            var tkhandler = new JwtSecurityTokenHandler();
            var jwt= tkhandler.WriteToken(token);
            return jwt;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt) {
            using (var hmac = new HMACSHA512()) {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt) {
            using (var hmac = new HMACSHA512(user.PasswordSalt)) {

                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return passwordHash == computeHash;
            }
        }
    }
}
