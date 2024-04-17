using ProductMarket.Domain.Interfaces.Services;
using Xunit;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ProductMarket.Application.Services;
using AutoMapper;
using ProductMarket.Domain.Entity;
using ProductMarket.DAL.Repository;
using ProductMarket.Application.Validation;
using Xunit.Abstractions;
using ProductMarket.Domain.Dto.Product;
using ProductMarket.DAL;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductMarket.Presentation.Controllers;
using ProductMarket.Domain.Result;

namespace ProductMarket.Tests
{

    public class ProductServiceTest 
    {

        private readonly ITestOutputHelper output;
        private readonly ProductMarketTests app;
        public ProductServiceTest(ITestOutputHelper output)
        {
            this.output = output;
            app= new ProductMarketTests();
        }

        [Fact]
        public async Task Can_Create_Product()
        {

            Mock<IProductService> todoService = new Mock<IProductService>();
            var createdto = new CreateProductDto("CreateProductDto", "ProductDto");
            todoService.Setup(_ => _.GetAllAsync(1).ReturnsAsync(ProductTestData.GetTols()));
            var sut = new ProductController(todoService.Object);

            /// Act
            var result = (OkObjectResult)await sut.GetAllAsync(1);


            // /// Assert
            result.StatusCode.Should().Be(200);
        }
    }
    internal class ProductMarketTests : WebApplicationFactory<Program>
    {
        public BaseRepository<Product> productRepository { get; set; }
        public BaseRepository<User> userRepository { get; set; }
        public Mapper mapper { get; set; }
        public ProductValidation validation { get; set; }
        public RedisCacheService CacheService { get; set; }
        
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices(s => {
                s.AddScoped<IProductService, ProductService>(
                    _ => new ProductService(productRepository, userRepository, mapper, validation, CacheService)
                    { }
               );
            });

            return base.CreateHost(builder);
        }
    }
    public static class ProductTestData
    {
        public static CollectResult<List<Product>> GetTols()
        {
            return new CollectResult<List<Product>>()
            {
                Data = new List<Product>()
                {
                   //new (1, "Prod1", "ProdDescription", DateTime.UtcNow.ToLongDateString())
                }
            };
        }
    }
}
