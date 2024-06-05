using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using ProductMarket.DAL;
using ProductMarket.DAL.Repository;
using ProductMarket.Domain.Dto.Product;
using ProductMarket.Domain.Dto.ProductCart;
using ProductMarket.Domain.Entity;
using ProductMarket.Domain.Interfaces.Repository;
using ProductMarket.Domain.Interfaces.Services;
using ProductMarket.Domain.Result;
using ProductMarket.Domain.Services;
using ProductMarket.Presentation.Controllers;
using System.Net;
using System.Net.Http.Json;

namespace ProductMarket.Test
{
    public class CartProductControllerTest
    {
        private CartController CartController;
        public CartProductControllerTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql("Server=localhost;Port=5432;Database=ProductMarket.Tests;User Id=postgres;Password=qwerpoiu")
                //.UseSnakeCaseNamingConvention()
                .Options;
            var DbContext = new ApplicationDbContext(options);
            var repositoryMockProduct = new BaseRepository<Product>(DbContext);
            var repositoryMockUser = new BaseRepository<User>(DbContext);
            var repositoryMockCart = new BaseRepository<Cart>(DbContext);
            var repositoryMockProductCart= new BaseRepository<CartProduct>(DbContext);
            var mapper = new Mock<IMapper>();
            IProductCartService cartService = new ProductCartService(repositoryMockProduct
                ,repositoryMockUser,mapper.Object
                ,repositoryMockCart,repositoryMockProductCart);
            CartController = new(cartService);
        }
        [Fact]
        public async Task AddProductInCart()
        {
            var dto = new ProductCartDto(1, 1);
            CartController.ModelState.AddModelError("FirstName", "Required");
            var result = await CartController.AddProductInCart(dto);
            var actionResult = Assert
            .IsType<ActionResult<BaseResult<ProductDto>>>(result);
            var badRequestResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.IsType<BaseResult<ProductDto>>(badRequestResult.Value);
        }
        [Fact]
        public async Task DeleteProductInCart()
        {
            CartController.ModelState.AddModelError("FirstName", "Required");
            var result = await CartController.RemoveProductInCart(1, 1);
            var actionResult = Assert
            .IsType<ActionResult<BaseResult<ProductDto>>>(result);
            var badRequestResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.IsType<BaseResult<ProductDto>>(badRequestResult.Value);
        }
    }
}
