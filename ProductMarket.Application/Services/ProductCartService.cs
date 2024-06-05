using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductMarket.Domain.Enum.Errors;
using ProductMarket.Domain.Dto.Product;
using ProductMarket.Domain.Entity;
using ProductMarket.Domain.Interfaces.Repository;
using ProductMarket.Domain.Interfaces.Services;
using ProductMarket.Domain.Result;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductMarket.Domain.Dto.ProductCart;
using ProductMarket.Application.Resources.Errors;

namespace ProductMarket.Domain.Services
{
    public class ProductCartService : IProductCartService
    {
        private readonly IBaseRepository<CartProduct> _productCartRepository;
        private readonly IBaseRepository<Product> _productRepository;
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<Cart> _cartRepository;
        private readonly IMapper _mapper;
        public ProductCartService(IBaseRepository<Product> productRepository,IBaseRepository<User> userrepository,IMapper mapper
            ,IBaseRepository<Cart> cartRepository,IBaseRepository<CartProduct> ProductCartRepository) { 
            _productRepository= productRepository;
            _userRepository = userrepository;
            _mapper = mapper;
            _cartRepository = cartRepository;
            _productCartRepository = ProductCartRepository;
        }

        public async Task<BaseResult<ProductDto>> AddCartProductAsync(ProductCartDto dto)
        {
            var user = await _userRepository.GetAll().Include(p=>p.Cart).ThenInclude(p=>p.Products).FirstOrDefaultAsync(p=>p.Id == dto.userId);
            if (user == null)
            {
                return new BaseResult<ProductDto>()
                {
                    ErrorCode = (int)ErrorCode.UserNotFound,
                    ErrorMessage = ErrorMessage.UserNotFound
                };
            }
            var product = await _productRepository.GetAll().FirstOrDefaultAsync(p=>p.Id == dto.prodId);
            if(product == null)
            {
                return new BaseResult<ProductDto>()
                {
                    ErrorCode = (int)ErrorCode.ProductIsNotFound,
                    ErrorMessage = ErrorMessage.ProductIsNotFound
                };
            }
            var cart = await _cartRepository.GetAll().FirstOrDefaultAsync(p=>p.UserId == dto.userId);
            if (cart == null)
            {
                return new BaseResult<ProductDto>()
                {
                    ErrorMessage = ErrorMessage.СartNotFoundError,
                    ErrorCode = (int)ErrorCode.СartNotFoundError
                };
            }
            var productCart = await _productCartRepository.GetAll().Where(p=>p.CartId == cart.Id).FirstOrDefaultAsync(p=>p.ProductId == dto.prodId);
            if(productCart != null)
            {
                return new BaseResult<ProductDto>
                {
                    ErrorCode = (int)ErrorCode.ProductInCartAlreadyExists,
                    ErrorMessage = ErrorMessage.ProductInCartAlreadyExists
                };
            }
            try
            {
                productCart = new CartProduct()
                {
                    CartId = cart.Id,
                    ProductId = dto.prodId,
                };
                await _productCartRepository.CreateAsync(productCart);
                return new BaseResult<ProductDto>()
                {
                    Data = _mapper.Map<ProductDto>(product)
                };
            }
            catch (Exception ex)
            {
                return new BaseResult<ProductDto>()
                {
                    ErrorCode = (int)ErrorCode.UnexpectedCartError,
                    ErrorMessage = ErrorMessage.UnexpectedCartError
                };
            }
        }
        public async Task<BaseResult<ProductDto>> RemoveCartProductAsync(int userId, int productId)
        {
            var product = await _productRepository.GetAll().FirstOrDefaultAsync(p => p.Id == productId);
            if (product == null)
            {
                return new BaseResult<ProductDto>()
                {
                    ErrorCode = (int)ErrorCode.ProductIsNotFound,
                    ErrorMessage = ErrorMessage.ProductIsNotFound
                };
            }
            try
            {
                var cart = await _cartRepository.GetAll().Include(p=>p.Products).FirstOrDefaultAsync(p => p.UserId == userId);
                if (cart == null)
                {
                    return new BaseResult<ProductDto>()
                    {
                        ErrorMessage = ErrorMessage.СartNotFoundError,
                        ErrorCode = (int)ErrorCode.СartNotFoundError
                    };
                }
                var cartProduct = await _productCartRepository.GetAll().Where(p=>p.CartId == cart.Id).FirstOrDefaultAsync(p=>p.ProductId == productId);
                _productCartRepository.Remove(cartProduct);
                await _productCartRepository.SaveChangesAsync();
                return new BaseResult<ProductDto>()
                {
                    Data = _mapper.Map<ProductDto>(product)
                };
            }
            catch (Exception ex)
            {
                return new BaseResult<ProductDto>()
                {
                    ErrorCode = (int)ErrorCode.RemoveCartProductError,
                    ErrorMessage = ErrorMessage.RemoveCartProductError
                };
            }
        }
    }
}
