using ProductMarket.Domain.Dto;
using ProductMarket.Domain.Dto.User;
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
    public class TokenControllerTest
    {
        [Fact]
        public async Task RefrechTokenTest()
        {

            var httpClient = new HttpClient();
            var userDto = new UserDto("NewTest106406User2", "11111111");
            var result = await httpClient.PostAsJsonAsync("https://localhost:7147/api/v1/User/login", userDto);
            var data = result.Content.ReadFromJsonAsync<BaseResult<TokenDto>>();

            Assert.Equal((int?)HttpStatusCode.OK, (int?)result.StatusCode);
            Assert.NotNull(data);
            Assert.IsType<BaseResult<TokenDto>>(data?.Result);

            var token = new TokenDto()
            {
                RefreshToken = data.Result.Data.RefreshToken,
                AccessToken = data.Result.Data.AccessToken,
            };
            var resultRefresh = await httpClient.PostAsJsonAsync("https://localhost:7147/api/v1/Token/Refresh", token);
            var dataRefresh = resultRefresh.Content.ReadFromJsonAsync<BaseResult<TokenDto>>();

            Assert.Equal((int?)HttpStatusCode.OK, (int?)resultRefresh.StatusCode);
            Assert.NotNull(dataRefresh);
            Assert.IsType<BaseResult<TokenDto>>(dataRefresh?.Result);
        }
    }
}
