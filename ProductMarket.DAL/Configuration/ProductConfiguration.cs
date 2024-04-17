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
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Description).IsRequired().HasMaxLength(2000);
            builder.HasMany<Image>(p => p.Images).WithOne(p => p.Product).HasForeignKey(p => p.ProductId).HasPrincipalKey(p => p.Id);

            builder.HasMany(p => p.Carts).WithMany(p => p.Products).UsingEntity<CartProduct>(
                p => p.HasOne<Cart>().WithMany().HasForeignKey(p => p.CartId),
                p => p.HasOne<Product>().WithMany().HasForeignKey(p => p.ProductId));

            builder.HasData(new List<Product>()
            {
                new Product 
                { 
                    Id = 1,
                    Name = "Name",
                    Description = "Description",
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy =1,
                    Images = new List<Image>()
                    {
                        
                    }
                }
            });
        }
    }
}
