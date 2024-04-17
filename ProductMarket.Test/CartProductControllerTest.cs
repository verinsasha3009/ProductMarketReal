using ProductMarket.Domain.Dto.Product;
using ProductMarket.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ProductMarket.Test
{
    public class CartProductControllerTest
    {
        [Fact]
        public async Task AddProductInCart()
        {
            var httpClient = new HttpClient();
            var result = await httpClient.GetAsync("https://localhost:7147/api/v1/Cart/2/2");
            var data = result.Content.ReadFromJsonAsync<BaseResult<ProductDto>>();

            Assert.Equal((int?)HttpStatusCode.OK, (int?)result.StatusCode);
            Assert.NotNull(data);
            Assert.IsType<BaseResult<ProductDto>>(data?.Result);
        }
        [Fact]
        public async Task DeleteProductInCart()
        {
            var httpClient = new HttpClient();
            var result = await httpClient.DeleteAsync("https://localhost:7147/api/v1/Cart/2/2");
            var data = result.Content.ReadFromJsonAsync<BaseResult<ProductDto>>();

            Assert.Equal((int?)HttpStatusCode.OK, (int?)result.StatusCode);
            Assert.NotNull(data);
            Assert.IsType<BaseResult<ProductDto>>(data?.Result);
        }
    }
}
