using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarket.Domain.Dto.Product
{
    public record ProductDto(long Id,string Name, string Description,string CreateAt);
}
