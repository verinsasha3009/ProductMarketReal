using ProductMarket.Domain.Dto;
using ProductMarket.Domain.Dto.Product;
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
    public class UserControllerTest
    {
        int rand = new Random().Next(165442);
        [Fact]
        public async Task RegistrationTest()
        {
            var httpClient = new HttpClient();
            var userDto = new RegistrUserDto($"NewTest{rand}User2", "11111111", "11111111");
            var result = await httpClient.PostAsJsonAsync("https://localhost:7147/api/v1/User/registration", userDto);
            var data = result.Content.ReadFromJsonAsync<BaseResult<UserDto>>();

            Assert.Equal((int?)HttpStatusCode.OK, (int?)result.StatusCode);
            Assert.NotNull(data);
            Assert.IsType<BaseResult<UserDto>>(data?.Result);
        }
        [Fact]
        public async Task LoginTest()
        {
            var httpClient = new HttpClient();
            var userDto = new UserDto($"NewTest106406User2", "11111111");
            var result = await httpClient.PostAsJsonAsync("https://localhost:7147/api/v1/User/login", userDto);
            var data = result.Content.ReadFromJsonAsync<BaseResult<TokenDto>>();

            Assert.Equal((int?)HttpStatusCode.OK, (int?)result.StatusCode);
            Assert.NotNull(data);
            Assert.IsType<BaseResult<TokenDto>>(data?.Result);
        }
    }
}
