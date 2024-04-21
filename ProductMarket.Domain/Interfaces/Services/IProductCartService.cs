using ProductMarket.Domain.Dto.Product;
using ProductMarket.Domain.Dto.ProductCart;
using ProductMarket.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarket.Domain.Interfaces.Services
{
    public interface IProductCartService
    {
        public Task<BaseResult<ProductDto>> AddCartProductAsync(ProductCartDto dto);
        public Task<BaseResult<ProductDto>> RemoveCartProductAsync(int userId, int productId);
    }
}
