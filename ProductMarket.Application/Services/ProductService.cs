using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using ProductMarket.Domain.Resources.Errors;
using ProductMarket.Domain.Dto.Product;
using ProductMarket.Domain.Entity;
using ProductMarket.Domain.Enum.Errors;
using ProductMarket.Domain.Interfaces.Repository;
using ProductMarket.Domain.Interfaces.Services;
using ProductMarket.Domain.Interfaces.Validation;
using ProductMarket.Domain.Result;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProductMarket.Domain.Services
{
    public class ProductService : IProductService
    {
        private readonly IBaseRepository<Product> _productRepository;
        private readonly IBaseRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly IProductValidation _productValidator;
        private readonly IRedisCacheService _CacheService;
        private readonly ILogger _logger;
        public ProductService(IBaseRepository<Product> prodrepository,IBaseRepository<User> userRepository,IMapper mapper, IProductValidation productvalidator,
            IRedisCacheService CacheService,ILogger logger) {
            _productRepository = prodrepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _productValidator = productvalidator;
            _CacheService=CacheService;
            _logger = logger;
        }
        public async Task<BaseResult<ProductDto>> CreateProductAsync(CreateProductDto dto)
        {
            var prod = await _productRepository.GetAll().FirstOrDefaultAsync(p=>p.Name == dto.Name);
            var result = _productValidator.EntityNullReference(prod);
            if(result.IsSucces)
            {
                return new BaseResult<ProductDto>()
                {
                    ErrorCode = (int)ErrorCode.ProductErrorCreate ,
                    ErrorMessage = ErrorMessage.ProductErrorCreate
                };
            }
            try
            {
                prod = new()
                {
                    Name = dto.Name ,
                    Description = dto.Description,
                };
                await _productRepository.CreateAsync(prod);
                _CacheService.Set<Product>(prod.Id.ToString(), prod);
                return new BaseResult<ProductDto>()
                {
                    Data = _mapper.Map<ProductDto>(prod)
                };
            }
            catch (Exception ex)
            {
                return new BaseResult<ProductDto>()
                {
                    ErrorCode = (int)ErrorCode.ProductErrorCreate,
                    ErrorMessage = ErrorMessage.ProductErrorCreate
                };
            }
        }

        public async Task<BaseResult<ProductDto>> DeleteProductAsync(int Id)
        {
            var prod = await _productRepository.GetAll().FirstOrDefaultAsync(p=>p.Id == Id);
            var result = _productValidator.EntityNullReference(prod);
            if (!result.IsSucces)
            {
                return new BaseResult<ProductDto>()
                {
                    ErrorCode = (int)ErrorCode.ProductDeleteError,
                    ErrorMessage = ErrorMessage.ProductDeleteError
                };
            }
            try
            {
                _productRepository.Remove(prod);
                await _productRepository.SaveChangesAsync();
                _CacheService.Delete<Product>(prod.Id.ToString());
                return new BaseResult<ProductDto>()
                {
                    Data= _mapper.Map<ProductDto>(prod)
                };
            }
            catch (Exception ex)
            {
                return new BaseResult<ProductDto>()
                {
                    ErrorCode = (int)ErrorCode.ProductDeleteError,
                    ErrorMessage = ErrorMessage.ProductDeleteError
                };
            }
        }

        public async Task<CollectResult<Product>> GetAllAsync(int userId)
        {
            _logger.Information($"Пользователь №{userId} извлек данные о всех продуктах из бд");
            try
            {
                var user = await _userRepository.GetAll().Include(p=>p.Cart).ThenInclude(p=>p.Products).FirstOrDefaultAsync(p => p.Id == userId);
                if(user == null)
                {
                    return new CollectResult<Product>()
                    {
                        ErrorCode = (int)ErrorCode.UserNotFound,
                        ErrorMessage = ErrorMessage.UserNotFound
                    };
                }
                if(user.Cart == null) {
                    user.Cart = new Cart();
                    user.Cart.Products = new List<Product>();
                }
                if(user.Cart.Products == null)
                {
                    user.Cart.Products = new List<Product>();
                }
                List<Product> ListProducts = user.Cart.Products;
                return new CollectResult<Product>()
                {
                    Result = ListProducts,
                    Count = user.Cart.Products.Count
                };
            }
            catch(Exception ex) 
            {
                return new CollectResult<Product>()
                {
                    ErrorMessage = ErrorMessage.ProductUnexpectedError,
                    ErrorCode = (int)ErrorCode.ProductUnexpectedError
                };
            }
        }

        public async Task<BaseResult<ProductDto>> GetProductAsync(int id)
        {
            try
            {
                var prod = _CacheService.Get<Product>(id.ToString());
                if (prod == null)
                {
                    prod = await _productRepository.GetAll().FirstOrDefaultAsync(p => p.Id == id);
                    var result = _productValidator.EntityNullReference(prod);
                    if (!result.IsSucces) 
                    {
                        return new BaseResult<ProductDto>()
                        {
                            ErrorCode = result.ErrorCode,
                            ErrorMessage = result.ErrorMessage
                        };
                    }
                    _CacheService.Set<Product>(id.ToString(), prod);
                }
                return new BaseResult<ProductDto>()
                {
                    Data = _mapper.Map<ProductDto>(prod)
                };
            }
            catch (Exception ex)
            {
                return new BaseResult<ProductDto>()
                {
                    ErrorCode = (int)ErrorCode.ProductNotDetected,
                    ErrorMessage = ErrorMessage.ProductNotDetected
                };
            }
        }
        public async Task<BaseResult<ProductDto>> UpdateProductAsync(UpdateProductDto dto)
        {
            var prod = await _productRepository.GetAll().FirstOrDefaultAsync(p => p.Id == dto.Id);
            var result =_productValidator.EntityNullReference(prod);
            if (!result.IsSucces)
            {
                return new BaseResult<ProductDto>()
                {
                    ErrorCode = result.ErrorCode,
                    ErrorMessage = result.ErrorMessage
                };
            }
            try
            {
                prod.Name = dto.Name;
                prod.Description = dto.Description;
                var prodCache = _CacheService.Get<Product>(prod.Id.ToString());

                result = _productValidator.EntityNullReference(prodCache);
                if (result.IsSucces)
                {
                    _CacheService.Delete<Product>(prodCache.Id.ToString());
                }
                _productRepository.Update(prod);
                await _productRepository.SaveChangesAsync();
                _CacheService.Set(dto.Id.ToString(), prod);
                return new BaseResult<ProductDto>()
                {
                    Data = _mapper.Map<ProductDto>(prod)
                };
            }
            catch (Exception ex)
            {
                return new BaseResult<ProductDto>()
                {
                    ErrorCode = (int)ErrorCode.ProductUpdateError,
                    ErrorMessage =  ErrorMessage.ProductUpdateError
                };
            }
        }
    }
}
