﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JwtWithRefreshTokenDemo.Entities
{
    [Table("UserRefreshToken")]
    public class UserRefreshToken
    {
        [Key]
        public int UserRefreshTokenId { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        [NotMapped]
        public bool IsActive { get {

                return ExpirationDate > DateTime.Now;
            } }

        public string IpAddress { get; set; }
        public bool IsInvalidated { get; set; }
        [ForeignKey("UserId")]
        public virtual User user { get; set; }
    }
}
