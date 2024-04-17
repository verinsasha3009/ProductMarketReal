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
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.ImagePath).IsRequired().HasMaxLength(200);
            builder.HasData(new List<Image>()
            {
                new Image()
                {
                    Id = 1,
                    ImagePath="ProductMarket.Application/Resourses/Image/2024-02-27_21-40-04.png",
                    ProductId=1
                }
            });
        }
    }
}
