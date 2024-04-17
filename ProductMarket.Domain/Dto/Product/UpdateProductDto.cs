using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarket.Domain.Dto.Product
{
    public record UpdateProductDto(long Id,string Name,string Description);
}
