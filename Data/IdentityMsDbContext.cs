using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityMs.Configuration;
using IdentityMs.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityMs.Data
{
    public class IdentityMsDbContext : IdentityDbContext<User>
    {
        public IdentityMsDbContext(DbContextOptions<IdentityMsDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new UserConfiguration());
        }
    }
}