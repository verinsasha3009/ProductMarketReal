using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductMarket.Presentation;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using ProductMarket.Domain.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using ProductMarket.Domain.Result;
using ProductMarket.Domain.Dto.Product;
using ProductMarket.Domain.Entity;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductMarket.Domain.Interfaces.Services;
using ProductMarket.Presentation.Controllers;
namespace ProductMarket.Test
{
    public class ProductControllerTest
    {
        Random random = new Random();
        [Fact]
        public async Task GetTestProduct()
        {
            int Id = 2;
            HttpClient httpClient = new HttpClient();
            var result = await httpClient.GetAsync($"https://localhost:7147/api/v1/Product/{Id}");
            var data = result.Content.ReadFromJsonAsync<BaseResult<ProductDto>>();

            Assert.Equal((int?)HttpStatusCode.OK, (int?)result.StatusCode);
            Assert.NotNull(data);
            Assert.IsType<BaseResult<ProductDto>>(data?.Result);
        }
        [Fact]
        public async Task CreateTestProduct()
        {
            HttpClient httpClient = new HttpClient();
            var prod = new CreateProductDto($"#NameProduct{random.Next(121211)}Test3", "#DescriptionProductTest");
            var result = await httpClient.PostAsJsonAsync("https://localhost:7147/api/v1/Product/Create", prod);
            var data = result.Content.ReadFromJsonAsync<BaseResult<ProductDto>>();

            Assert.Equal((int?)HttpStatusCode.OK, (int?)result.StatusCode);
            Assert.NotNull(data);
            Assert.IsType<BaseResult<ProductDto>>(data?.Result);

            await httpClient.DeleteAsync($"https://localhost:7147/api/v1/Product/Delete/{data.Result.Data.Id}");
        }
        [Fact]
        public async Task UpdateTestProduct()
        {
            HttpClient httpClient = new HttpClient();
            var prod2 = new CreateProductDto($"#NameProduct{random.Next(121211)}Test3", "#DescriptionProductTest");
            var result2 = await httpClient.PostAsJsonAsync("https://localhost:7147/api/v1/Product/Create", prod2);
            var data2 = result2.Content.ReadFromJsonAsync<BaseResult<ProductDto>>();

            Assert.Equal((int?)HttpStatusCode.OK, (int?)result2.StatusCode);
            Assert.NotNull(data2);

            var prodUpdate = new UpdateProductDto(data2.Result.Data.Id, $"NewProduct{random.Next(12129)}Test", ";;;;;l;;;;;;;;");
            var result = await httpClient.PutAsJsonAsync($"https://localhost:7147/api/v1/Product", prodUpdate);
            var data = result.Content.ReadFromJsonAsync<BaseResult<ProductDto>>();

            Assert.Equal((int?)HttpStatusCode.OK, (int?)result.StatusCode);
            Assert.NotNull(data);
            Assert.IsType<BaseResult<ProductDto>>(data?.Result);

            await httpClient.DeleteAsync($"https://localhost:7147/api/v1/Product/Delete/{data.Result.Data.Id}");
        }
        [Fact]
        public async Task GetAllProductTest()
        {
            var httpClient = new HttpClient();
            var result = await httpClient.GetAsync($"https://localhost:7147/api/v1/Product/GetAll/2");
            var data = result.Content.ReadFromJsonAsync<CollectResult<Product>>();

            Assert.Equal((int?)HttpStatusCode.OK, (int?)result.StatusCode);
            Assert.NotNull(data);
            Assert.IsType<CollectResult<Product>>(data?.Result);
        }
        [Fact]
        public async Task DeleteTestProduct()
        {
            var httpClient = new HttpClient();
            var prod2 = new CreateProductDto($"#NameProduct{random.Next(121211)}Test3", "#DescriptionProductTest");
            var result2 = await httpClient.PostAsJsonAsync("https://localhost:7147/api/v1/Product/Create", prod2);
            var data2 = result2.Content.ReadFromJsonAsync<BaseResult<ProductDto>>();

            Assert.Equal((int?)HttpStatusCode.OK, (int?)result2.StatusCode);
            Assert.NotNull(data2);

            var result = await httpClient.DeleteAsync($"https://localhost:7147/api/v1/Product/Delete/{data2.Result.Data.Id}");
            var data = result.Content.ReadFromJsonAsync<BaseResult<ProductDto>>();

            Assert.Equal((int?)HttpStatusCode.OK, (int?)result.StatusCode);
            Assert.NotNull(data);
            Assert.IsType<BaseResult<ProductDto>>(data?.Result);
        }
        [Fact]
        public async Task TaskGetAsync()
        {
            var todoService = new Mock<IProductService>();
            todoService.Setup(_ => _.GetProductAsync(1)).ReturnsAsync(ToDoMockData.GetTodos());
            var sut = new ProductController(todoService.Object);

            /// Act
            var result = await sut.GetProduct(1);

            /// Assert
            Assert.Null(result.Value);
            var e = result.Value.ErrorCode;
            Assert.Equal((int)HttpStatusCode.OK,result.Value.ErrorCode );
            
        }
        [Fact]
        public async Task TaskDeleteAsync()
        {
            var todoService = new Mock<IProductService>();

            var prod = new CreateProductDto("Test", "DescriptionTest");
            todoService.Setup(_ => _.CreateProductAsync(prod)).ReturnsAsync(ToDoMockData.GetTodos());
            
            todoService.Setup(_ => _.DeleteProductAsync(1)).ReturnsAsync(ToDoMockData.GetTodos());

            var sut = new ProductController(todoService.Object);

            /// Act
            await sut.CreateProduct(prod);
            var result = await sut.DeleteProduct(1);

            // /// Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Null(result?.Value?.ErrorCode);
            Assert.True(result?.Value.IsSucces);
            //Assert.Equal((int)HttpStatusCode.OK,result.Value.ErrorCode);
            var p = result.Value.Data;
            Assert.IsType<ProductDto>(result.Value.Data);
        }
    }
}
