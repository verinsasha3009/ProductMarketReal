using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductMarket.Domain.Resources.Errors;
using ProductMarket.Domain.Dto.Product;
using ProductMarket.Domain.Entity;
using ProductMarket.Domain.Enum.Errors;
using ProductMarket.Domain.Interfaces.Repository;
using ProductMarket.Domain.Interfaces.Services;
using ProductMarket.Domain.Result;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarket.Domain.Services
{
    public class ProductCartService : IProductCartService
    {
        private readonly IBaseRepository<Product> _productRepository;
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<Cart> _cartRepository;
        private readonly IMapper _mapper;
        public ProductCartService(IBaseRepository<Product> productRepository,IBaseRepository<User> userrepository,IMapper mapper
            ,IBaseRepository<Cart> cartRepository) { 
            _productRepository= productRepository;
            _userRepository = userrepository;
            _mapper = mapper;
            _cartRepository = cartRepository;
        }

        public async Task<BaseResult<ProductDto>> AddCartProductAsync(int userId, int productId)
        {
            var user = await _userRepository.GetAll().Include(p=>p.Cart).ThenInclude(p=>p.Products).FirstOrDefaultAsync(p=>p.Id == userId);
            if (user == null)
            {
                return new BaseResult<ProductDto>()
                {
                    ErrorCode = (int)ErrorCode.UserNotFound,
                    ErrorMessage = ErrorMessage.UserNotFound
                };
            }
            var product = await _productRepository.GetAll().FirstOrDefaultAsync(p=>p.Id == productId);
            if(product == null)
            {
                return new BaseResult<ProductDto>()
                {
                    ErrorCode = (int)ErrorCode.ProductIsNotFound,
                    ErrorMessage = ErrorMessage.ProductIsNotFound
                };
            }
            try
            {
                ProductIdentityCart(user);
                var cart = await _cartRepository.GetAll().FirstOrDefaultAsync(p=>p.Id == user.Cart.Id);
                
                if (cart== null)
                {
                    return new BaseResult<ProductDto>()
                    {
                        ErrorMessage = ErrorMessage.СartNotFoundError,
                        ErrorCode = (int)ErrorCode.СartNotFoundError
                    };
                }
                cart.Products.Add(product);
                user.Cart = cart;
                _userRepository.Update(user);
                await _userRepository.SaveChangesAsync();

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
            var user = await _userRepository.GetAll().Include(p=>p.Cart).FirstOrDefaultAsync(p => p.Id == userId);
            if (user == null)
            {
                return new BaseResult<ProductDto>()
                {
                    ErrorCode = (int)ErrorCode.UserNotFound,
                    ErrorMessage = ErrorMessage.UserNotFound
                };
            }
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

                ProductIdentityCart(user);
                var cart = await _cartRepository.GetAll().Include(p=>p.Products).FirstOrDefaultAsync(p => p.Id == user.Cart.Id);

                if (cart == null)
                {
                    return new BaseResult<ProductDto>()
                    {
                        ErrorMessage = ErrorMessage.СartNotFoundError,
                        ErrorCode = (int)ErrorCode.СartNotFoundError
                    };
                }
                var productDelete = cart.Products.FirstOrDefault(p=>p.Id ==productId);
                if (productDelete == null)
                {
                    return new BaseResult<ProductDto>()
                    {
                        ErrorCode = (int)ErrorCode.ProductDeleteError,
                        ErrorMessage = ErrorMessage.ProductDeleteError
                    };
                }
                cart.Products.Remove(product);
                user.Cart = cart;
                _userRepository.Update(user);
                await _userRepository.SaveChangesAsync();
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
        public void ProductIdentityCart(User user)
        {
            if (user.Cart == null)
            {
                user.Cart = new Cart();
            }
            if (user.Cart.Products == null)
            {
                user.Cart.Products = new List<Product>();
            }
        }
    }
}
