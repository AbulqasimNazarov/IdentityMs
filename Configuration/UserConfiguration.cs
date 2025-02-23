using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityMs.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityMs.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(u => u.Email).IsUnique();
            builder.Property(e => e.Password).IsRequired();
        }
    }
}