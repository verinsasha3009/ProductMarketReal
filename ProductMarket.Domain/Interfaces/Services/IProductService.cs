using ProductMarket.Domain.Dto.Product;
using ProductMarket.Domain.Entity;
using ProductMarket.Domain.Interfaces.Repository;
using ProductMarket.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarket.Domain.Interfaces.Services
{
    public interface IProductService
    {
        /// <summary>
        /// Создание Продукта для списка всех продуктов
        /// </summary>
        /// <returns></returns>
        Task<BaseResult<ProductDto>> CreateProductAsync(CreateProductDto dto);
        Task<BaseResult<ProductDto>> UpdateProductAsync(UpdateProductDto dto);
        Task<BaseResult<ProductDto>> DeleteProductAsync(int Id);
        Task<BaseResult<ProductDto>> GetProductAsync(int id);
        Task<CollectResult<Product>> GetAllAsync(int userId);
    }
}
