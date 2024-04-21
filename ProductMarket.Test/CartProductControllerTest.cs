using ProductMarket.Domain.Dto.Product;
using ProductMarket.Domain.Dto.ProductCart;
using ProductMarket.Domain.Result;
using System.Net;
using System.Net.Http.Json;

namespace ProductMarket.Test
{
    public class CartProductControllerTest
    {
        [Fact]
        public async Task AddProductInCart()
        {
            var httpClient = new HttpClient();
            var cartProd =new ProductCartDto(2,2);
            var result = await httpClient.PostAsJsonAsync("https://localhost:7147/api/v1/Cart",cartProd);
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
