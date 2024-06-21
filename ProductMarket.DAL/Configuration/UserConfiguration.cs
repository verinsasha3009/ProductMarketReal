using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductMarket.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarket.DAL.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.Login).IsRequired();
            builder.Property(p=>p.Password).IsRequired();

            builder.HasMany(p => p.Roles).WithMany(p => p.Users).UsingEntity<UserRole>(
                p => p.HasOne<Role>().WithMany().HasForeignKey(p => p.RoleId),
                p => p.HasOne<User>().WithMany().HasForeignKey(p => p.UserId));

            //builder.Property(p => p.Cart).IsRequired();
            builder.HasData(new List<User>
            {
                new User { Id = 1,
                    Login ="aaaaa",
                    Password ="aaaaaaa",
                }
            });
        }
    }
}
