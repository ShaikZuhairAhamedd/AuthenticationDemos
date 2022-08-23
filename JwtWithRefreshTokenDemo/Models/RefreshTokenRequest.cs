using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JwtWithRefreshTokenDemo.Models
{
    public class RefreshTokenRequest
    {
        [Required]
        public string ExpiresToken { get; set; }
        [Required]
        public string RefreshToken { get; set; } 
    }
}
