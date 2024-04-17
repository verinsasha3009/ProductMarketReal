using ProductMarket.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProductMarket.Domain.Entity
{
    public class Cart : IEntityId<long>
    {
        public long Id { get; set; }
        public List<Product> Products { get; set; }

        public User User { get; set; }
        [JsonIgnore]
        public long UserId { get; set; }
    }
}
