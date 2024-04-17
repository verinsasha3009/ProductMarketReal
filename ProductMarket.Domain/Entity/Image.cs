using ProductMarket.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarket.Domain.Entity
{
    public class Image : IEntityId<long>
    {
        public long Id { get; set; }
        public string ImagePath { get; set; }

        public Product Product { get; set; }
        public long ProductId { get; set; }
    }
}
