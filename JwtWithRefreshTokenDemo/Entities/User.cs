using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JwtWithRefreshTokenDemo.Entities
{
    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        [Key]
        public int UserId { get; set; }

        public List<UserRefreshToken> userRefreshTokens { get; set; }

    }
}
