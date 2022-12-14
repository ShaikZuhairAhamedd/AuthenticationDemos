using JwtWithRefreshTokenDemo.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtWithRefreshTokenDemo.Context
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions options):base(options)
        {
                
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserRefreshToken> userRefreshTokens { get; set; }
    }
}
