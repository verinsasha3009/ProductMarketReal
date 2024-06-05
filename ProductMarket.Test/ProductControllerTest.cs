using System.Net;
using System.Net.Http.Json;
using ProductMarket.Domain.Result;
using ProductMarket.Domain.Dto.Product;
using ProductMarket.Domain.Entity;
using ProductMarket.Domain.Interfaces.Repository;
using ProductMarket.Presentation.Controllers;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using ProductMarket.DAL.Repository;
using ProductMarket.DAL;
using ProductMarket.Domain.Interfaces.Services;
using ProductMarket.Domain.Services;
using ProductMarket.Domain.Validation;
using Microsoft.Extensions.Caching.Distributed;
using Serilog;
using Microsoft.AspNetCore.Mvc;
using ProductMarket.Domain.Dto.ProductCart;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ProductMarket.Application.Mapping;
using ProductMarket.Domain.Mapping;
namespace ProductMarket.Test
{
    public class ProductControllerTest
    {
        private readonly ProductController _controller;
        public ProductControllerTest()
        {
            
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql("Server=localhost;Port=5432;Database=ProductMarket.Tests;User Id=postgres;Password=qwerpoiu")
                //.UseSnakeCaseNamingConvention()
                .Options;
            var DbContext = new ApplicationDbContext(options);
            var repositoryMockProduct = new BaseRepository<Product>(DbContext);
            var repositoryMockUser = new BaseRepository<User>(DbContext);
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ProductMapping());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            var validation = new ProductValidation();
            var opts = Options.Create<MemoryDistributedCacheOptions>(new MemoryDistributedCacheOptions());
            IDistributedCache distrCache = new MemoryDistributedCache(opts);
            var cache = new RedisCacheService(distrCache);
            var logger = new Mock<ILogger>();
            IProductService prodService = new ProductService(repositoryMockProduct
                , repositoryMockUser, mapper,validation,cache,logger.Object);
            _controller = new(prodService);
        }
        Random random = new Random();
        [Fact]
        public async Task GetTestProduct()
        {
            _controller.ModelState.AddModelError("FirstName", "Required");
            var result = await _controller.GetProduct(2);
            var actionResult = Assert
            .IsType<ActionResult<BaseResult<ProductDto>>>(result);
            var badRequestResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.IsType<BaseResult<ProductDto>>(badRequestResult.Value);
        }
        [Fact]
        public async Task CreateTestProduct()
        {
            var dto = new CreateProductDto($"Test_Product_{random.Next(1233427)}","Test_Description");
            _controller.ModelState.AddModelError("FirstName", "Required");
            var result = await _controller.CreateProduct(dto);
            var actionResult = Assert
            .IsType<ActionResult<BaseResult<ProductDto>>>(result);
            var badRequestResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var Data = Assert.IsType<BaseResult<ProductDto>>(badRequestResult.Value);

            Assert.NotNull(Data);

            int ptodId = (int)Data.Data.Id;
            _controller.ModelState.AddModelError("FirstName", "Required");
            var resultDelete = await _controller.DeleteProduct(ptodId);
            var actionResultDelete = Assert
            .IsType<ActionResult<BaseResult<ProductDto>>>(resultDelete);
            var badRequestResultDelete = Assert.IsType<OkObjectResult>(actionResultDelete.Result);
            Assert.IsType<BaseResult<ProductDto>>(badRequestResultDelete.Value);
        }
        [Fact]
        public async Task UpdateTestProduct()
        {
            var dto = new UpdateProductDto(2,$"Test_Product_{random.Next(123342)}", "Test_Description");
            _controller.ModelState.AddModelError("FirstName", "Required");
            var result = await _controller.UpdateProduct(dto);
            var actionResult = Assert
            .IsType<ActionResult<BaseResult<ProductDto>>>(result);
            var badRequestResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.IsType<BaseResult<ProductDto>>(badRequestResult.Value);
        }
        [Fact]
        public async Task GetAllProductTest()
        {
            _controller.ModelState.AddModelError("FirstName", "Required");
            var result = await _controller.GetAll(1);
            var actionResult = Assert
            .IsType<ActionResult<CollectResult<Product>>>(result);
            var badRequestResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var Data = Assert.IsType<CollectResult<Product>>(badRequestResult.Value);
            
        }
        
    }
}
