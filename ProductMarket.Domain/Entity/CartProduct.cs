using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarket.Domain.Entity
{
    public class CartProduct
    {
        public long ProductId { get; set; }
        public long CartId { get; set; }
    }
}
